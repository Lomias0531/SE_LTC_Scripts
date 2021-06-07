using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.Collections;
using VRage.Library;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRageMath;

namespace VRage.Network
{
	internal class MyClient
	{
		public class UpdateLayer
		{
			public MyLayers.UpdateLayerDesc Descriptor;

			public HashSet<IMyReplicable> Replicables;

			public MyDistributedUpdater<List<IMyReplicable>, IMyReplicable> Sender;

			public int UpdateTimer;
		}

		private struct IslandData
		{
			public HashSet<IMyReplicable> Replicables;

			public byte Index;
		}

		private struct MyOrderedPacket
		{
			public byte Id;

			public MyPacket Packet;

			public override string ToString()
			{
				return Id.ToString();
			}
		}

		public readonly MyClientStateBase State;

		private readonly IReplicationServerCallback m_callback;

		public float PriorityMultiplier = 1f;

		public bool IsReady;

		private MyTimeSpan m_lastClientRealtime;

		private MyTimeSpan m_lastClientTimestamp;

		private MyTimeSpan m_lastStateSyncTimeStamp;

		private byte m_stateSyncPacketId;

		public readonly Dictionary<IMyReplicable, byte> PermanentReplicables = new Dictionary<IMyReplicable, byte>();

		public readonly HashSet<IMyReplicable> CrucialReplicables = new HashSet<IMyReplicable>();

		public readonly MyConcurrentDictionary<IMyReplicable, MyReplicableClientData> Replicables = new MyConcurrentDictionary<IMyReplicable, MyReplicableClientData>(InstanceComparer<IMyReplicable>.Default);

		public int PendingReplicables;

		public bool WantsBatchCompleteConfirmation = true;

		public readonly MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker> BlockedReplicables = new MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker>();

		public readonly Dictionary<IMyStateGroup, MyStateDataEntry> StateGroups = new Dictionary<IMyStateGroup, MyStateDataEntry>(InstanceComparer<IMyStateGroup>.Default);

		public readonly FastPriorityQueue<MyStateDataEntry> DirtyQueue = new FastPriorityQueue<MyStateDataEntry>(1024);

		private readonly HashSet<string> m_clientCachedData = new HashSet<string>();

		public MyPacketStatistics Statistics;

		public UpdateLayer[] UpdateLayers;

		public readonly Dictionary<IMyReplicable, UpdateLayer> ReplicableToLayer = new Dictionary<IMyReplicable, UpdateLayer>();

		private readonly CachingHashSet<IslandData> m_islands = new CachingHashSet<IslandData>();

		private readonly Dictionary<IMyReplicable, IslandData> m_replicableToIsland = new Dictionary<IMyReplicable, IslandData>();

		private byte m_nextIslandIndex;

		private readonly List<MyOrderedPacket> m_incomingBuffer = new List<MyOrderedPacket>();

		private bool m_incomingBuffering = true;

		private byte m_lastProcessedClientPacketId = byte.MaxValue;

		private readonly MyPacketTracker m_clientTracker = new MyPacketTracker();

		private bool m_processedPacket;

		private MyTimeSpan m_lastReceivedTimeStamp = MyTimeSpan.Zero;

		private const int MINIMUM_INCOMING_BUFFER = 4;

		private const byte OUT_OF_ORDER_RESET_PROTECTION = 64;

		private const byte OUT_OF_ORDER_ACCEPT_THRESHOLD = 6;

		private byte m_lastReceivedAckId;

		private bool m_waitingForReset;

		private readonly List<IMyStateGroup>[] m_pendingStateSyncAcks = (from s in Enumerable.Range(0, 256)
			select new List<IMyStateGroup>(8)).ToArray();

		private static readonly MyTimeSpan MAXIMUM_PACKET_GAP = MyTimeSpan.FromSeconds(0.40000000596046448);

		public bool UsePlayoutDelayBufferForCharacter
		{
			get;
			set;
		}

		public bool UsePlayoutDelayBufferForJetpack
		{
			get;
			set;
		}

		public bool UsePlayoutDelayBufferForGrids
		{
			get;
			set;
		}

		private bool UsePlayoutDelayBuffer
		{
			get
			{
				if ((!State.IsControllingCharacter || !UsePlayoutDelayBufferForCharacter) && (!State.IsControllingJetpack || !UsePlayoutDelayBufferForJetpack))
				{
					if (State.IsControllingGrid)
					{
						return UsePlayoutDelayBufferForGrids;
					}
					return false;
				}
				return true;
			}
		}

		public MyClient(MyClientStateBase emptyState, IReplicationServerCallback callback)
		{
			m_callback = callback;
			State = emptyState;
			InitLayers();
		}

		private void InitLayers()
		{
			UpdateLayers = new UpdateLayer[MyLayers.UpdateLayerDescriptors.Count];
			for (int i = 0; i < MyLayers.UpdateLayerDescriptors.Count; i++)
			{
				MyLayers.UpdateLayerDesc descriptor = MyLayers.UpdateLayerDescriptors[i];
				UpdateLayers[i] = new UpdateLayer
				{
					Descriptor = descriptor,
					Replicables = new HashSet<IMyReplicable>(),
					Sender = new MyDistributedUpdater<List<IMyReplicable>, IMyReplicable>(descriptor.SendInterval),
					UpdateTimer = i + 1
				};
			}
		}

		public UpdateLayer GetLayerOfReplicable(IMyReplicable rep)
		{
			BoundingBoxD aABB = rep.GetAABB();
			if (!State.Position.HasValue)
			{
				return null;
			}
			UpdateLayer[] updateLayers = UpdateLayers;
			foreach (UpdateLayer updateLayer in updateLayers)
			{
				BoundingBoxD boundingBoxD = new BoundingBoxD(State.Position.Value - new Vector3D(updateLayer.Descriptor.Radius), State.Position.Value + new Vector3D(updateLayer.Descriptor.Radius));
				if (boundingBoxD.Intersects(aABB))
				{
					return updateLayer;
				}
			}
			return null;
		}

		public void TryCreateNewCachedIsland(IMyReplicable replicable, HashSet<IMyReplicable> dependencies)
		{
			if (!m_replicableToIsland.ContainsKey(replicable))
			{
				if (m_nextIslandIndex == 0)
				{
					m_nextIslandIndex++;
				}
				IslandData islandData = default(IslandData);
				islandData.Replicables = new HashSet<IMyReplicable>();
				islandData.Index = m_nextIslandIndex;
				IslandData islandData2 = islandData;
				islandData2.Replicables.Add(replicable);
				m_replicableToIsland[replicable] = islandData2;
				if (dependencies != null)
				{
					foreach (IMyReplicable dependency in dependencies)
					{
						islandData2.Replicables.Add(dependency);
						m_replicableToIsland[dependency] = islandData2;
					}
				}
				m_islands.Add(islandData2);
				m_islands.ApplyAdditions();
				m_nextIslandIndex++;
			}
		}

		private void SendReplicationIslandDone(IslandData island, Endpoint clientEndpoint)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteByte(island.Index);
			bitStreamPacketData.Stream.WriteInt32(island.Replicables.Count);
			foreach (IMyReplicable replicable in island.Replicables)
			{
				IMyEntityReplicable myEntityReplicable = replicable as IMyEntityReplicable;
				if (myEntityReplicable != null)
				{
					bitStreamPacketData.Stream.WriteInt64(myEntityReplicable.EntityId);
					bitStreamPacketData.Stream.Write(myEntityReplicable.WorldMatrix.Translation);
					bitStreamPacketData.Stream.WriteQuaternion(Quaternion.CreateFromRotationMatrix(myEntityReplicable.WorldMatrix));
				}
			}
			m_callback.SendReplicationIslandDone(bitStreamPacketData, clientEndpoint);
		}

		private void RemoveCachedIsland(IslandData island)
		{
			m_islands.Remove(island);
			foreach (IMyReplicable replicable in island.Replicables)
			{
				m_replicableToIsland.Remove(replicable);
			}
		}

		public void UpdateIslands()
		{
			foreach (IslandData island in m_islands)
			{
				bool flag = true;
				foreach (IMyReplicable replicable in island.Replicables)
				{
					IMyStreamableReplicable myStreamableReplicable = replicable as IMyStreamableReplicable;
					if (myStreamableReplicable != null && StateGroups.TryGetValue(myStreamableReplicable.GetStreamingStateGroup(), out MyStateDataEntry value) && DirtyQueue.Contains(value))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					SendReplicationIslandDone(island, State.EndpointId);
					RemoveCachedIsland(island);
				}
			}
			m_islands.ApplyRemovals();
		}

		public byte GetReplicableIslandIndex(IMyReplicable replicable)
		{
			if (m_replicableToIsland.TryGetValue(replicable, out IslandData value))
			{
				return value.Index;
			}
			return 0;
		}

		public void RemoveReplicableFromIslands(IMyReplicable replicable)
		{
			if (m_replicableToIsland.TryGetValue(replicable, out IslandData value))
			{
				value.Replicables.Remove(replicable);
				m_replicableToIsland.Remove(replicable);
			}
		}

		private void AddIncomingPacketSorted(byte packetId, MyPacket packet)
		{
			MyOrderedPacket myOrderedPacket = default(MyOrderedPacket);
			myOrderedPacket.Id = packetId;
			myOrderedPacket.Packet = packet;
			MyOrderedPacket item = myOrderedPacket;
			int num = m_incomingBuffer.Count - 1;
			while (num >= 0 && packetId < m_incomingBuffer[num].Id && (packetId >= 64 || m_incomingBuffer[num].Id <= 192))
			{
				num--;
			}
			num++;
			m_incomingBuffer.Insert(num, item);
		}

		private bool ProcessIncomingPacket(MyPacket packet, bool skipControls, MyTimeSpan serverTimeStamp, bool skip = false)
		{
			byte b = packet.BitStream.ReadByte();
			m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
			m_lastClientRealtime = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
			m_lastReceivedTimeStamp = serverTimeStamp;
			Statistics.Update(m_clientTracker.Add(b));
			bool flag = b <= m_lastProcessedClientPacketId && (m_lastProcessedClientPacketId <= 192 || b >= 64);
			if (!flag)
			{
				m_lastProcessedClientPacketId = b;
			}
			skipControls = (skipControls || flag);
			State.Serialize(packet.BitStream, skipControls);
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			if (!skip)
			{
				m_processedPacket = true;
			}
			return skipControls;
		}

		private void UpdateIncoming(MyTimeSpan serverTimeStamp, bool skipAll = false)
		{
			if (m_incomingBuffer.Count == 0 || (m_incomingBuffering && m_incomingBuffer.Count < 4 && !skipAll))
			{
				if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
				{
					_ = m_incomingBuffer.Count;
				}
				m_incomingBuffering = true;
				m_lastProcessedClientPacketId = byte.MaxValue;
				State.Update();
				return;
			}
			if (m_incomingBuffering)
			{
				m_lastProcessedClientPacketId = (byte)(m_incomingBuffer[0].Id - 1);
			}
			m_incomingBuffering = false;
			string text = "";
			bool flag;
			do
			{
				bool skipControls = m_incomingBuffer.Count > 4 || skipAll;
				flag = ProcessIncomingPacket(m_incomingBuffer[0].Packet, skipControls, serverTimeStamp, skip: true);
				if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
				{
					text = m_incomingBuffer[0].Id + ", " + text;
					if (flag)
					{
						text = "-" + text;
					}
				}
				m_incomingBuffer[0].Packet.Return();
				m_incomingBuffer.RemoveAt(0);
			}
			while (m_incomingBuffer.Count > 4 || (flag && m_incomingBuffer.Count > 0));
			_ = MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking;
		}

		private void ClearBufferedIncomingPackets(MyTimeSpan serverTimeStamp)
		{
			if (m_incomingBuffer.Count > 0)
			{
				UpdateIncoming(serverTimeStamp, skipAll: true);
			}
		}

		public void OnClientUpdate(MyPacket packet, MyTimeSpan serverTimeStamp)
		{
			if (UsePlayoutDelayBuffer)
			{
				int bitPosition = packet.BitStream.BitPosition;
				byte packetId = packet.BitStream.ReadByte();
				packet.BitStream.SetBitPositionRead(bitPosition);
				AddIncomingPacketSorted(packetId, packet);
			}
			else
			{
				ClearBufferedIncomingPackets(serverTimeStamp);
				ProcessIncomingPacket(packet, skipControls: false, serverTimeStamp);
				packet.Return();
			}
		}

		public void Update(MyTimeSpan serverTimeStamp)
		{
			if (UsePlayoutDelayBuffer)
			{
				UpdateIncoming(serverTimeStamp);
			}
			else
			{
				if (!m_processedPacket)
				{
					State.Update();
				}
				m_processedPacket = false;
			}
			if (serverTimeStamp > m_lastReceivedTimeStamp + MAXIMUM_PACKET_GAP)
			{
				State.ResetControlledEntityControls();
			}
		}

		/// <summary>
		/// Returns true when current packet is closely preceding last packet (is within threshold)
		/// </summary>
		private static bool IsPreceding(int currentPacketId, int lastPacketId, int threshold)
		{
			if (lastPacketId < currentPacketId)
			{
				lastPacketId += 256;
			}
			return lastPacketId - currentPacketId <= threshold;
		}

		public bool IsAckAvailable()
		{
			byte b = (byte)(m_lastReceivedAckId - 6);
			byte b2 = (byte)(m_stateSyncPacketId + 1);
			if (m_waitingForReset || b2 == b)
			{
				m_waitingForReset = true;
				return false;
			}
			return true;
		}

		public void OnClientAcks(MyPacket packet)
		{
			byte b = packet.BitStream.ReadByte();
			byte b2 = packet.BitStream.ReadByte();
			for (int i = 0; i < b2; i++)
			{
				OnAck(packet.BitStream.ReadByte());
			}
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			byte b3;
			byte b4;
			if (m_waitingForReset)
			{
				m_stateSyncPacketId = (byte)(b + 64);
				CheckStateSyncPacketId();
				b3 = (byte)(m_stateSyncPacketId + 1);
				b4 = (byte)(b3 - 64);
				m_waitingForReset = false;
			}
			else
			{
				b3 = (byte)(m_stateSyncPacketId + 1);
				b4 = (byte)(m_lastReceivedAckId - 6);
			}
			for (byte b5 = b3; b5 != b4; b5 = (byte)(b5 + 1))
			{
				RaiseAck(b5, delivered: false);
			}
		}

		private void OnAck(byte ackId)
		{
			if (IsPreceding(ackId, m_lastReceivedAckId, 6))
			{
				RaiseAck(ackId, delivered: true);
				return;
			}
			RaiseAck(ackId, delivered: true);
			m_lastReceivedAckId = ackId;
		}

		private void RaiseAck(byte ackId, bool delivered)
		{
			foreach (IMyStateGroup item in m_pendingStateSyncAcks[ackId])
			{
				if (StateGroups.ContainsKey(item))
				{
					item.OnAck(State, ackId, delivered);
				}
			}
			m_pendingStateSyncAcks[ackId].Clear();
		}

		private void AddPendingAck(byte stateSyncPacketId, IMyStateGroup group)
		{
			m_pendingStateSyncAcks[stateSyncPacketId].Add(group);
		}

		public bool IsReplicableReady(IMyReplicable replicable)
		{
			if (Replicables.TryGetValue(replicable, out MyReplicableClientData value) && !value.IsPending)
			{
				return !value.IsStreaming;
			}
			return false;
		}

		public bool IsReplicablePending(IMyReplicable replicable)
		{
			if (Replicables.TryGetValue(replicable, out MyReplicableClientData value))
			{
				if (!value.IsPending)
				{
					return value.IsStreaming;
				}
				return true;
			}
			return false;
		}

		public bool HasReplicable(IMyReplicable replicable)
		{
			return Replicables.ContainsKey(replicable);
		}

		public bool WritePacketHeader(BitStream sendStream, bool streaming, MyTimeSpan serverTimeStamp, out MyTimeSpan clientTimestamp)
		{
			m_lastStateSyncTimeStamp = serverTimeStamp;
			if (!streaming)
			{
				m_stateSyncPacketId++;
				if (!CheckStateSyncPacketId())
				{
					clientTimestamp = MyTimeSpan.Zero;
					return false;
				}
			}
			sendStream.WriteBool(streaming);
			sendStream.WriteByte((byte)((!streaming) ? m_stateSyncPacketId : 0));
			Statistics.Write(sendStream, m_callback.GetUpdateTime());
			sendStream.WriteDouble(serverTimeStamp.Milliseconds);
			sendStream.WriteDouble(m_lastClientTimestamp.Milliseconds);
			m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(-1.0);
			sendStream.WriteDouble(m_lastClientRealtime.Milliseconds);
			m_lastClientRealtime = MyTimeSpan.FromMilliseconds(-1.0);
			m_callback.WriteCustomState(sendStream);
			clientTimestamp = serverTimeStamp;
			return true;
		}

		private bool CheckStateSyncPacketId()
		{
			int stateSyncPacketId = m_stateSyncPacketId;
			byte b = 0;
			while (m_pendingStateSyncAcks[m_stateSyncPacketId].Count != 0)
			{
				m_stateSyncPacketId++;
				b = (byte)(b + 1);
				if (stateSyncPacketId == m_stateSyncPacketId)
				{
					Statistics.PendingPackets = b;
					return false;
				}
			}
			Statistics.PendingPackets = b;
			return true;
		}

		public void Serialize(IMyStateGroup group, BitStream sendStream, MyTimeSpan timeStamp, int messageBitSize = int.MaxValue)
		{
			if (timeStamp == MyTimeSpan.Zero)
			{
				timeStamp = State.ClientTimeStamp;
			}
			group.Serialize(sendStream, State.EndpointId, timeStamp, m_lastClientTimestamp, m_stateSyncPacketId, messageBitSize, m_clientCachedData);
		}

		public bool SendStateSync(MyStateDataEntry stateGroupEntry, int mtuBytes, ref MyPacketDataBitStreamBase data, MyTimeSpan serverTimeStamp)
		{
			if (data == null)
			{
				data = m_callback.GetBitStreamPacketData();
				if (!WritePacketHeader(data.Stream, streaming: false, serverTimeStamp, out MyTimeSpan clientTimestamp))
				{
					data.Return();
					data = null;
					return false;
				}
				State.ClientTimeStamp = clientTimestamp;
			}
			BitStream stream = data.Stream;
			int num = 8 * (mtuBytes - 2);
			int bitPosition = stream.BitPosition;
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			stream.Terminate();
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			stream.WriteNetworkId(stateGroupEntry.GroupId);
			if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
			{
				stateGroupEntry.Group.Owner.ToString();
				_ = stateGroupEntry.Group.GetType().FullName;
			}
			int bitPosition2 = stream.BitPosition;
			stream.WriteInt16(0);
			int bitPosition3 = stream.BitPosition;
			Serialize(stateGroupEntry.Group, stream, MyTimeSpan.Zero, num);
			int bitPosition4 = stream.BitPosition;
			stream.SetBitPositionWrite(bitPosition2);
			stream.WriteInt16((short)(bitPosition4 - bitPosition3));
			stream.SetBitPositionWrite(bitPosition4);
			int num2 = stream.BitPosition - bitPosition;
			_ = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking;
			if (num2 > 0 && stream.BitPosition <= num)
			{
				AddPendingAck(m_stateSyncPacketId, stateGroupEntry.Group);
			}
			else
			{
				if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
				{
					stateGroupEntry.Group.Owner.ToString();
					_ = stateGroupEntry.Group.GetType().FullName;
				}
				stateGroupEntry.Group.OnAck(State, m_stateSyncPacketId, delivered: false);
				stream.SetBitPositionWrite(bitPosition);
				data.Stream.Terminate();
				m_callback.SendStateSync(data, State.EndpointId, reliable: false);
				data = null;
			}
			return true;
		}

		private void SendEmptyStateSync(MyTimeSpan serverTimeStamp)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			if (!WritePacketHeader(bitStreamPacketData.Stream, streaming: false, serverTimeStamp, out MyTimeSpan _))
			{
				bitStreamPacketData.Return();
				return;
			}
			bitStreamPacketData.Stream.Terminate();
			m_callback.SendStateSync(bitStreamPacketData, State.EndpointId, reliable: false);
		}

		public void SendUpdate(MyTimeSpan serverTimeStamp)
		{
			if (serverTimeStamp > m_lastStateSyncTimeStamp + MAXIMUM_PACKET_GAP)
			{
				SendEmptyStateSync(serverTimeStamp);
			}
		}

		public bool RemoveCache(IMyReplicable replicable, string storageName)
		{
			if (replicable == null || !Replicables.ContainsKey(replicable))
			{
				return m_clientCachedData.Remove(storageName);
			}
			return false;
		}

		public void ResetLayerTimers()
		{
			int num = 0;
			UpdateLayer[] updateLayers = UpdateLayers;
			for (int i = 0; i < updateLayers.Length; i++)
			{
				updateLayers[i].UpdateTimer = num++;
			}
		}
	}
}
