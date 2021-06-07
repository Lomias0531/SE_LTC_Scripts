using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
	public class MyReplicationServer : MyReplicationLayer
	{
		internal class MyDestroyBlocker
		{
			public bool Remove;

			public bool IsProcessing;

			public readonly List<IMyReplicable> Blockers = new List<IMyReplicable>();
		}

		private const int MAX_NUM_STATE_SYNC_PACKETS_PER_CLIENT = 7;

		private readonly IReplicationServerCallback m_callback;

		private readonly CacheList<IMyStateGroup> m_tmpGroups = new CacheList<IMyStateGroup>(4);

		private HashSet<IMyReplicable> m_toRecalculateHash = new HashSet<IMyReplicable>();

		private readonly List<IMyReplicable> m_tmpReplicableList = new List<IMyReplicable>();

		private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();

		private readonly HashSet<IMyReplicable> m_lastLayerAdditions = new HashSet<IMyReplicable>();

		private readonly CachingHashSet<IMyReplicable> m_postponedDestructionReplicables = new CachingHashSet<IMyReplicable>();

		private readonly ConcurrentCachingHashSet<IMyReplicable> m_priorityUpdates = new ConcurrentCachingHashSet<IMyReplicable>();

		private MyTimeSpan m_serverTimeStamp = MyTimeSpan.Zero;

		private long m_serverFrame;

		private readonly ConcurrentQueue<IMyStateGroup> m_dirtyGroups = new ConcurrentQueue<IMyStateGroup>();

		public static SerializableVector3I StressSleep = new SerializableVector3I(0, 0, 0);

		/// <summary>
		/// All replicables on server.
		/// </summary>
		/// <summary>
		/// All replicable state groups.
		/// </summary>
		private readonly Dictionary<IMyReplicable, List<IMyStateGroup>> m_replicableGroups = new Dictionary<IMyReplicable, List<IMyStateGroup>>();

		/// <summary>
		/// Network objects and states which are actively replicating to clients.
		/// </summary>
		private readonly ConcurrentDictionary<Endpoint, MyClient> m_clientStates = new ConcurrentDictionary<Endpoint, MyClient>();

		/// <summary>
		/// Clients that recently disconnected are saved here for some time so that the server doesn't freak out in case some calls are still pending for them
		/// </summary>
		private readonly ConcurrentDictionary<Endpoint, MyTimeSpan> m_recentClientsStates = new ConcurrentDictionary<Endpoint, MyTimeSpan>();

		private readonly HashSet<Endpoint> m_recentClientStatesToRemove = new HashSet<Endpoint>();

		private readonly MyTimeSpan SAVED_CLIENT_DURATION = MyTimeSpan.FromSeconds(60.0);

		[ThreadStatic]
		private static List<EndpointId> m_recipients;

		private readonly List<EndpointId> m_endpoints = new List<EndpointId>();

		public MyReplicationServer(IReplicationServerCallback callback, EndpointId localEndpoint, Thread mainThread)
			: base(isNetworkAuthority: true, localEndpoint, mainThread)
		{
			m_callback = callback;
			m_replicables = new MyReplicablesAABB(mainThread);
		}

		protected override MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			return m_callback.GetBitStreamPacketData();
		}

		public void Replicate(IMyReplicable obj)
		{
			if (!IsTypeReplicated(obj.GetType()))
			{
				return;
			}
			if (!obj.IsReadyForReplication)
			{
				obj.ReadyForReplicationAction.Add(obj, delegate
				{
					Replicate(obj);
				});
				return;
			}
			AddNetworkObjectServer(obj);
			m_replicables.Add(obj, out IMyReplicable _);
			AddStateGroups(obj);
			if (obj.PriorityUpdate)
			{
				m_priorityUpdates.Add(obj);
			}
		}

		/// <summary>
		/// Hack to allow thing like: CreateCharacter, Respawn sent from server
		/// </summary>
		public void ForceReplicable(IMyReplicable obj, IMyReplicable parent = null)
		{
			if (obj != null)
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if ((parent == null || clientState.Value.Replicables.ContainsKey(parent)) && !clientState.Value.Replicables.ContainsKey(obj))
					{
						RefreshReplicable(obj, clientState.Key, clientState.Value, force: true);
					}
				}
			}
		}

		/// <summary>
		/// Hack to allow thing like: CreateCharacter, Respawn sent from server
		/// </summary>
		private void ForceReplicable(IMyReplicable obj, Endpoint clientEndpoint)
		{
			if (!(m_localEndpoint == clientEndpoint.Id) && !clientEndpoint.Id.IsNull && obj != null && m_clientStates.ContainsKey(clientEndpoint))
			{
				MyClient myClient = m_clientStates[clientEndpoint];
				if (!myClient.Replicables.ContainsKey(obj))
				{
					AddForClient(obj, clientEndpoint, myClient, force: true);
				}
			}
		}

		public void RemoveForClientIfIncomplete(IMyEventProxy objA)
		{
			if (objA != null)
			{
				IMyReplicable replicableA = GetProxyTarget(objA) as IMyReplicable;
				RemoveForClients(replicableA, (MyClient x) => x.IsReplicablePending(replicableA), sendDestroyToClient: true);
			}
		}

		/// <summary>
		/// Sends everything in the world to client. Use with extreme caution!
		/// </summary>
		public void ForceEverything(Endpoint clientEndpoint)
		{
			m_replicables.IterateRoots(delegate(IMyReplicable replicable)
			{
				ForceReplicable(replicable, clientEndpoint);
			});
		}

		public void Destroy(IMyReplicable obj)
		{
			if (IsTypeReplicated(obj.GetType()) && obj.IsReadyForReplication && (obj.ReadyForReplicationAction == null || obj.ReadyForReplicationAction.Count <= 0) && !GetNetworkIdByObject(obj).IsInvalid)
			{
				m_priorityUpdates.Remove(obj);
				m_priorityUpdates.ApplyChanges();
				bool isAnyClientPending = false;
				bool flag = obj.GetParent() != null && !obj.GetParent().IsValid;
				RemoveForClients(obj, delegate(MyClient client)
				{
					if (client.BlockedReplicables.ContainsKey(obj))
					{
						client.BlockedReplicables[obj].Remove = true;
						if (!obj.HasToBeChild && !m_priorityUpdates.Contains(obj))
						{
							m_priorityUpdates.Add(obj);
						}
						isAnyClientPending = true;
						return false;
					}
					client.PermanentReplicables.Remove(obj);
					client.CrucialReplicables.Remove(obj);
					client.RemoveReplicableFromIslands(obj);
					return true;
				}, !flag);
				m_replicables.RemoveHierarchy(obj);
				if (!isAnyClientPending)
				{
					RemoveStateGroups(obj);
					RemoveNetworkedObject(obj);
					m_postponedDestructionReplicables.Remove(obj);
					obj.OnRemovedFromReplication();
				}
				else
				{
					m_postponedDestructionReplicables.Add(obj);
				}
			}
		}

		/// <summary>
		/// Destroys replicable for all clients (used for testing and debugging).
		/// </summary>
		public void ResetForClients(IMyReplicable obj)
		{
			RemoveForClients(obj, (MyClient client) => client.Replicables.ContainsKey(obj), sendDestroyToClient: true);
		}

		public void AddClient(Endpoint endpoint, MyClientStateBase clientState)
		{
			if (!m_clientStates.ContainsKey(endpoint))
			{
				clientState.EndpointId = endpoint;
				m_clientStates.TryAdd(endpoint, new MyClient(clientState, m_callback));
			}
		}

		private void OnClientConnected(EndpointId endpointId, MyClientStateBase clientState)
		{
			AddClient(new Endpoint(endpointId, 0), clientState);
		}

		public void OnClientReady(Endpoint endpointId, ref ClientReadyDataMsg msg)
		{
			if (m_clientStates.TryGetValue(endpointId, out MyClient value))
			{
				value.IsReady = true;
				value.UsePlayoutDelayBufferForCharacter = msg.UsePlayoutDelayBufferForCharacter;
				value.UsePlayoutDelayBufferForJetpack = msg.UsePlayoutDelayBufferForJetpack;
				value.UsePlayoutDelayBufferForGrids = msg.UsePlayoutDelayBufferForGrids;
			}
		}

		public void OnClientReady(Endpoint endpointId, MyPacket packet)
		{
			ClientReadyDataMsg msg = MySerializer.CreateAndRead<ClientReadyDataMsg>(packet.BitStream);
			OnClientReady(endpointId, ref msg);
			SendServerData(endpointId);
		}

		private void SendServerData(Endpoint endpointId)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			SerializeTypeTable(bitStreamPacketData.Stream);
			m_callback.SendServerData(bitStreamPacketData, endpointId);
		}

		public void OnClientLeft(EndpointId endpointId)
		{
			bool flag;
			do
			{
				flag = false;
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if (clientState.Key.Id == endpointId)
					{
						flag = true;
						RemoveClient(clientState.Key);
						break;
					}
				}
			}
			while (flag);
		}

		private void RemoveClient(Endpoint endpoint)
		{
			if (m_clientStates.TryGetValue(endpoint, out MyClient value))
			{
				while (value.Replicables.Count > 0)
				{
					RemoveForClient(value.Replicables.FirstPair().Key, value, sendDestroyToClient: false);
				}
				m_clientStates.Remove(endpoint);
				m_recentClientsStates[endpoint] = m_callback.GetUpdateTime() + SAVED_CLIENT_DURATION;
			}
		}

		public override void Disconnect()
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				RemoveClient(clientState.Key);
			}
		}

		public override void SetPriorityMultiplier(EndpointId id, float priority)
		{
			if (m_clientStates.TryGetValue(new Endpoint(id, 0), out MyClient value))
			{
				value.PriorityMultiplier = priority;
			}
		}

		public void OnClientJoined(EndpointId endpointId, MyClientStateBase clientState)
		{
			OnClientConnected(endpointId, clientState);
		}

		public void OnClientAcks(MyPacket packet)
		{
			if (m_clientStates.TryGetValue(packet.Sender, out MyClient value))
			{
				value.OnClientAcks(packet);
				packet.Return();
			}
		}

		public void OnClientUpdate(MyPacket packet)
		{
			if (m_clientStates.TryGetValue(packet.Sender, out MyClient value))
			{
				value.OnClientUpdate(packet, m_serverTimeStamp);
			}
		}

		public override MyTimeSpan GetSimulationUpdateTime()
		{
			return m_serverTimeStamp;
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public override void UpdateBefore()
		{
			Endpoint endpoint = default(Endpoint);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				try
				{
					clientState.Value.Update(m_serverTimeStamp);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
					endpoint = clientState.Key;
				}
			}
			if (endpoint.Id.IsValid)
			{
				m_callback.DisconnectClient(endpoint.Id.Value);
			}
			MyTimeSpan updateTime = m_callback.GetUpdateTime();
			foreach (KeyValuePair<Endpoint, MyTimeSpan> recentClientsState in m_recentClientsStates)
			{
				if (recentClientsState.Value < updateTime)
				{
					m_recentClientStatesToRemove.Add(recentClientsState.Key);
				}
			}
			foreach (Endpoint item in m_recentClientStatesToRemove)
			{
				m_recentClientsStates.Remove(item);
			}
			m_recentClientStatesToRemove.Clear();
			m_postponedDestructionReplicables.ApplyAdditions();
			foreach (IMyReplicable postponedDestructionReplicable in m_postponedDestructionReplicables)
			{
				Destroy(postponedDestructionReplicable);
			}
			m_postponedDestructionReplicables.ApplyRemovals();
		}

		public override void UpdateAfter()
		{
		}

		public override void UpdateClientStateGroups()
		{
		}

		public override void Simulate()
		{
		}

		public override void SendUpdate()
		{
			m_serverTimeStamp = m_callback.GetUpdateTime();
			m_serverFrame++;
			ApplyDirtyGroups();
			if (m_clientStates.Count != 0)
			{
				m_priorityUpdates.ApplyChanges();
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if (clientState.Value.IsReady)
					{
						IMyReplicable controlledReplicable = clientState.Value.State.ControlledReplicable;
						IMyReplicable characterReplicable = clientState.Value.State.CharacterReplicable;
						int num = clientState.Value.UpdateLayers.Length;
						_ = clientState.Value.UpdateLayers[clientState.Value.UpdateLayers.Length - 1];
						MyClient.UpdateLayer[] updateLayers = clientState.Value.UpdateLayers;
						foreach (MyClient.UpdateLayer updateLayer in updateLayers)
						{
							num--;
							updateLayer.UpdateTimer--;
							if (updateLayer.UpdateTimer <= 0)
							{
								updateLayer.UpdateTimer = updateLayer.Descriptor.UpdateInterval;
								if (clientState.Value.State.Position.HasValue)
								{
									BoundingBoxD aabb = new BoundingBoxD(clientState.Value.State.Position.Value - new Vector3D(updateLayer.Descriptor.Radius), clientState.Value.State.Position.Value + new Vector3D(updateLayer.Descriptor.Radius));
									m_replicables.GetReplicablesInBox(aabb, m_tmpReplicableList);
								}
								HashSet<IMyReplicable> replicables = updateLayer.Replicables;
								updateLayer.Replicables = m_toRecalculateHash;
								m_toRecalculateHash = replicables;
								updateLayer.Sender.List.Clear();
								foreach (IMyReplicable tmpReplicable in m_tmpReplicableList)
								{
									AddReplicableToLayer(tmpReplicable, updateLayer, clientState.Value, num == 0);
								}
								m_tmpReplicableList.Clear();
								foreach (KeyValuePair<IMyReplicable, byte> permanentReplicable in clientState.Value.PermanentReplicables)
								{
									if (clientState.Value.UpdateLayers.Length - permanentReplicable.Value - 1 == num)
									{
										AddReplicableToLayer(permanentReplicable.Key, updateLayer, clientState.Value, addDependencies: true);
									}
								}
								if (num == 0)
								{
									clientState.Value.CrucialReplicables.Clear();
									if (controlledReplicable != null)
									{
										AddReplicableToLayer(controlledReplicable, updateLayer, clientState.Value, addDependencies: true);
										AddReplicableToLayer(characterReplicable, updateLayer, clientState.Value, addDependencies: true);
										AddCrucialReplicable(clientState.Value, controlledReplicable);
										AddCrucialReplicable(clientState.Value, characterReplicable);
										HashSet<IMyReplicable> dependencies = characterReplicable.GetDependencies(forPlayer: true);
										if (dependencies != null)
										{
											foreach (IMyReplicable item in dependencies)
											{
												AddReplicableToLayer(item, updateLayer, clientState.Value, addDependencies: false);
											}
										}
									}
									foreach (IMyReplicable lastLayerAddition in m_lastLayerAdditions)
									{
										AddReplicableToLayer(lastLayerAddition, updateLayer, clientState.Value, addDependencies: false);
									}
									m_lastLayerAdditions.Clear();
								}
								foreach (IMyReplicable replicable in updateLayer.Replicables)
								{
									IMyReplicable parent = replicable.GetParent();
									if (!clientState.Value.HasReplicable(replicable) && (parent == null || clientState.Value.HasReplicable(parent)))
									{
										AddForClient(replicable, clientState.Key, clientState.Value, force: false, addDependencies: true);
									}
								}
								foreach (IMyReplicable item2 in m_toRecalculateHash)
								{
									RefreshReplicable(item2, clientState.Key, clientState.Value);
								}
								m_toRecalculateHash.Clear();
								if (clientState.Value.WantsBatchCompleteConfirmation && num == 0 && clientState.Value.PendingReplicables == 0)
								{
									m_callback.SendPendingReplicablesDone(clientState.Key);
									clientState.Value.WantsBatchCompleteConfirmation = false;
								}
							}
						}
						foreach (IMyReplicable priorityUpdate in m_priorityUpdates)
						{
							RefreshReplicable(priorityUpdate, clientState.Key, clientState.Value);
						}
					}
				}
				foreach (IMyReplicable priorityUpdate2 in m_priorityUpdates)
				{
					m_priorityUpdates.Remove(priorityUpdate2);
				}
				m_priorityUpdates.ApplyRemovals();
				foreach (KeyValuePair<Endpoint, MyClient> clientState2 in m_clientStates)
				{
					FilterStateSync(clientState2.Value);
				}
				foreach (KeyValuePair<Endpoint, MyClient> clientState3 in m_clientStates)
				{
					clientState3.Value.SendUpdate(m_serverTimeStamp);
				}
				if (StressSleep.X > 0)
				{
					int millisecondsTimeout = (StressSleep.Z != 0) ? ((int)(Math.Sin(m_serverTimeStamp.Milliseconds * Math.PI / (double)StressSleep.Z) * (double)StressSleep.Y + (double)StressSleep.X)) : MyRandom.Instance.Next(StressSleep.X, StressSleep.Y);
					Thread.Sleep(millisecondsTimeout);
				}
			}
		}

		/// <summary>
		/// Adds replicable to a replication layer if it's not already in a smaller one.
		/// If it's the last layer, it also adds any physically connected replicables.
		/// </summary>
		private void AddReplicableToLayer(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client, bool addDependencies)
		{
			if (IsReplicableInPreviousLayer(rep, layer, client))
			{
				return;
			}
			AddReplicableToLayerSingle(rep, layer, client);
			if (addDependencies)
			{
				HashSet<IMyReplicable> physicalDependencies = rep.GetPhysicalDependencies(m_serverTimeStamp, m_replicables);
				if (physicalDependencies != null)
				{
					foreach (IMyReplicable item in physicalDependencies)
					{
						if (!IsReplicableInPreviousLayer(item, layer, client))
						{
							AddReplicableToLayerSingle(item, layer, client);
						}
					}
				}
			}
		}

		private void AddReplicableToLayerSingle(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client, bool removeFromDelete = true)
		{
			layer.Replicables.Add(rep);
			layer.Sender.List.Add(rep);
			client.ReplicableToLayer[rep] = layer;
			if (removeFromDelete)
			{
				m_toRecalculateHash.Remove(rep);
			}
			HashSet<IMyReplicable> dependencies = rep.GetDependencies(forPlayer: false);
			if (dependencies != null)
			{
				foreach (IMyReplicable item in dependencies)
				{
					m_lastLayerAdditions.Add(item);
				}
			}
		}

		private bool IsReplicableInPreviousLayer(IMyReplicable rep, MyClient.UpdateLayer layer, MyClient client)
		{
			int num = 0;
			MyClient.UpdateLayer updateLayer;
			do
			{
				updateLayer = client.UpdateLayers[num];
				num++;
				if (updateLayer.Replicables.Contains(rep))
				{
					m_toRecalculateHash.Remove(rep);
					return true;
				}
			}
			while (updateLayer != layer);
			return false;
		}

		private void RefreshReplicable(IMyReplicable replicable, Endpoint endPoint, MyClient client, bool force = false)
		{
			if (!replicable.IsSpatial)
			{
				IMyReplicable parent = replicable.GetParent();
				if (parent == null)
				{
					if (client.Replicables.ContainsKey(replicable) && !client.BlockedReplicables.ContainsKey(replicable))
					{
						RemoveForClient(replicable, client, sendDestroyToClient: true);
					}
				}
				else if (client.HasReplicable(parent))
				{
					AddForClient(replicable, endPoint, client, force);
				}
				return;
			}
			bool flag = true;
			if (replicable.HasToBeChild)
			{
				IMyReplicable parent2 = replicable.GetParent();
				if (parent2 != null)
				{
					flag = client.HasReplicable(parent2);
				}
			}
			MyClient.UpdateLayer layerOfReplicable = client.GetLayerOfReplicable(replicable);
			if (layerOfReplicable != null && flag)
			{
				AddForClient(replicable, endPoint, client, force);
				AddReplicableToLayerSingle(replicable, layerOfReplicable, client, removeFromDelete: false);
			}
			else if ((replicable == client.State.ControlledReplicable || replicable == client.State.CharacterReplicable || client.CrucialReplicables.Contains(replicable)) && flag)
			{
				AddReplicableToLayerSingle(replicable, client.UpdateLayers[0], client, removeFromDelete: false);
				AddForClient(replicable, endPoint, client, force);
			}
			else if (client.Replicables.ContainsKey(replicable) && !client.BlockedReplicables.ContainsKey(replicable))
			{
				RemoveForClient(replicable, client, sendDestroyToClient: true);
			}
		}

		private void AddCrucialReplicable(MyClient client, IMyReplicable replicable)
		{
			client.CrucialReplicables.Add(replicable);
			HashSet<IMyReplicable> physicalDependencies = replicable.GetPhysicalDependencies(m_serverTimeStamp, m_replicables);
			if (physicalDependencies != null)
			{
				foreach (IMyReplicable item in physicalDependencies)
				{
					client.CrucialReplicables.Add(item);
				}
			}
		}

		public void AddToDirtyGroups(IMyStateGroup group)
		{
			if (group.Owner.IsReadyForReplication)
			{
				m_dirtyGroups.Enqueue(group);
			}
		}

		private void ApplyDirtyGroups()
		{
			IMyStateGroup result;
			while (m_dirtyGroups.TryDequeue(out result))
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if (clientState.Value.StateGroups.TryGetValue(result, out MyStateDataEntry value))
					{
						ScheduleStateGroupSync(clientState.Value, value, SyncFrameCounter);
					}
				}
			}
		}

		private void SendStreamingEntry(MyClient client, MyStateDataEntry entry)
		{
			Endpoint endpointId = client.State.EndpointId;
			if (entry.Group.IsProcessingForClient(endpointId) == MyStreamProcessingState.Finished)
			{
				MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
				BitStream stream = bitStreamPacketData.Stream;
				client.WritePacketHeader(stream, streaming: true, m_serverTimeStamp, out MyTimeSpan clientTimestamp);
				stream.Terminate();
				stream.WriteNetworkId(entry.GroupId);
				int bitPosition = stream.BitPosition;
				stream.WriteInt32(0);
				int bitPosition2 = stream.BitPosition;
				client.Serialize(entry.Group, stream, clientTimestamp);
				int bitPosition3 = stream.BitPosition;
				stream.SetBitPositionWrite(bitPosition);
				stream.WriteInt32(bitPosition3 - bitPosition2);
				stream.SetBitPositionWrite(bitPosition3);
				stream.Terminate();
				m_callback.SendStateSync(bitStreamPacketData, endpointId, reliable: true);
			}
			else
			{
				client.Serialize(entry.Group, null, MyTimeSpan.Zero);
				ScheduleStateGroupSync(client, entry, SyncFrameCounter);
			}
			IMyReplicable owner = entry.Group.Owner;
			if (owner != null)
			{
				using (m_tmp)
				{
					m_replicables.GetAllChildren(owner, m_tmp);
					foreach (IMyReplicable item in m_tmp)
					{
						if (!client.HasReplicable(item))
						{
							AddForClient(item, endpointId, client, force: false);
						}
					}
				}
			}
		}

		private void FilterStateSync(MyClient client)
		{
			if (!client.IsAckAvailable())
			{
				return;
			}
			ApplyDirtyGroups();
			int num = 0;
			MyPacketDataBitStreamBase data = null;
			List<MyStateDataEntry> list = PoolManager.Get<List<MyStateDataEntry>>();
			int mTUSize = m_callback.GetMTUSize(client.State.EndpointId);
			int count = client.DirtyQueue.Count;
			int num2 = 7;
			MyStateDataEntry myStateDataEntry = null;
			while (count-- > 0 && num2 > 0 && client.DirtyQueue.First.Priority < SyncFrameCounter)
			{
				MyStateDataEntry myStateDataEntry2 = client.DirtyQueue.Dequeue();
				list.Add(myStateDataEntry2);
				if (myStateDataEntry2.Owner != null && !myStateDataEntry2.Group.IsStreaming && (!client.Replicables.TryGetValue(myStateDataEntry2.Owner, out MyReplicableClientData value) || !value.HasActiveStateSync))
				{
					continue;
				}
				if (myStateDataEntry2.Group.IsStreaming)
				{
					if (myStateDataEntry == null && myStateDataEntry2.Group.IsProcessingForClient(client.State.EndpointId) != MyStreamProcessingState.Processing)
					{
						myStateDataEntry = myStateDataEntry2;
					}
					else
					{
						ScheduleStateGroupSync(client, myStateDataEntry2, SyncFrameCounter);
					}
					continue;
				}
				if (!client.SendStateSync(myStateDataEntry2, mTUSize, ref data, m_serverTimeStamp))
				{
					break;
				}
				num++;
				if (data == null)
				{
					num2--;
				}
			}
			if (data != null)
			{
				data.Stream.Terminate();
				m_callback.SendStateSync(data, client.State.EndpointId, reliable: false);
			}
			if (myStateDataEntry != null)
			{
				SendStreamingEntry(client, myStateDataEntry);
			}
			client.UpdateIslands();
			long syncFrameCounter = SyncFrameCounter;
			foreach (MyStateDataEntry item in list)
			{
				if (item.Group.IsStillDirty(client.State.EndpointId))
				{
					ScheduleStateGroupSync(client, item, syncFrameCounter);
				}
			}
		}

		private void ScheduleStateGroupSync(MyClient client, MyStateDataEntry groupEntry, long currentTime)
		{
			IMyReplicable parent = groupEntry.Owner.GetParent();
			IMyReplicable myReplicable = parent ?? groupEntry.Owner;
			MyClient.UpdateLayer value = null;
			if (!client.ReplicableToLayer.TryGetValue(myReplicable, out value))
			{
				value = client.GetLayerOfReplicable(myReplicable);
				if (value == null)
				{
					return;
				}
			}
			int num = (myReplicable == client.State.ControlledReplicable) ? 1 : (groupEntry.Group.IsHighPriority ? Math.Max(value.Descriptor.SendInterval >> 4, 1) : value.Descriptor.SendInterval);
			num = MyRandom.Instance.Next(1, num * 2);
			long num2 = num + currentTime;
			if (!client.DirtyQueue.Contains(groupEntry))
			{
				if (groupEntry.Owner.IsValid || m_postponedDestructionReplicables.Contains(groupEntry.Owner))
				{
					client.DirtyQueue.Enqueue(groupEntry, num2);
				}
			}
			else if (groupEntry.Owner.IsValid || m_postponedDestructionReplicables.Contains(groupEntry.Owner))
			{
				long priority = groupEntry.Priority;
				num2 = Math.Min(priority, num2);
				if (num2 != priority)
				{
					client.DirtyQueue.UpdatePriority(groupEntry, num2);
				}
			}
			else
			{
				client.DirtyQueue.Remove(groupEntry);
			}
		}

		public override void UpdateStatisticsData(int outgoing, int incoming, int tamperred, float gcMemory, float processMemory)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				clientState.Value.Statistics.UpdateData(outgoing, incoming, tamperred, gcMemory, processMemory);
			}
		}

		public override MyPacketStatistics ClearServerStatistics()
		{
			return default(MyPacketStatistics);
		}

		public override MyPacketStatistics ClearClientStatistics()
		{
			MyPacketStatistics result = default(MyPacketStatistics);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				result.Add(clientState.Value.Statistics);
			}
			return result;
		}

		private void AddForClient(IMyReplicable replicable, Endpoint clientEndpoint, MyClient client, bool force, bool addDependencies = false)
		{
			if (!replicable.IsReadyForReplication || client.HasReplicable(replicable) || !replicable.ShouldReplicate(new MyClientInfo(client)) || !replicable.IsValid)
			{
				return;
			}
			AddClientReplicable(replicable, client, force);
			if (GetClientReplicableIslandIndex(replicable, clientEndpoint) == 0)
			{
				HashSet<IMyReplicable> physicalDependencies = replicable.GetPhysicalDependencies(m_serverTimeStamp, m_replicables);
				if (physicalDependencies != null && physicalDependencies.Count > 0)
				{
					client.TryCreateNewCachedIsland(replicable, physicalDependencies);
					if (addDependencies)
					{
						new MyClientInfo(client);
						foreach (IMyReplicable item in physicalDependencies)
						{
							AddForClient(item, clientEndpoint, client, force);
						}
					}
				}
			}
			SendReplicationCreate(replicable, client, clientEndpoint);
			if (!(replicable is IMyStreamableReplicable))
			{
				foreach (IMyReplicable child in m_replicables.GetChildren(replicable))
				{
					AddForClient(child, clientEndpoint, client, force);
				}
			}
		}

		private void RemoveForClients(IMyReplicable replicable, Func<MyClient, bool> validate, bool sendDestroyToClient)
		{
			using (m_tmp)
			{
				if (m_recipients == null)
				{
					m_recipients = new List<EndpointId>();
				}
				bool flag = true;
				foreach (MyClient value in m_clientStates.Values)
				{
					if (validate(value))
					{
						if (flag)
						{
							m_replicables.GetAllChildren(replicable, m_tmp);
							m_tmp.Add(replicable);
							flag = false;
						}
						RemoveForClientInternal(replicable, value);
						if (sendDestroyToClient)
						{
							m_recipients.Add(value.State.EndpointId.Id);
						}
					}
				}
				if (m_recipients.Count > 0)
				{
					SendReplicationDestroy(replicable, m_recipients);
					m_recipients.Clear();
				}
			}
		}

		private void RemoveForClient(IMyReplicable replicable, MyClient client, bool sendDestroyToClient)
		{
			if (m_recipients == null)
			{
				m_recipients = new List<EndpointId>();
			}
			using (m_tmp)
			{
				m_replicables.GetAllChildren(replicable, m_tmp);
				m_tmp.Add(replicable);
				RemoveForClientInternal(replicable, client);
				if (sendDestroyToClient)
				{
					m_recipients.Add(client.State.EndpointId.Id);
					SendReplicationDestroy(replicable, m_recipients);
					m_recipients.Clear();
				}
			}
		}

		private void RemoveForClientInternal(IMyReplicable replicable, MyClient client)
		{
			foreach (IMyReplicable item in m_tmp)
			{
				client.BlockedReplicables.Remove(item);
				RemoveClientReplicable(item, client);
			}
			MyClient.UpdateLayer[] updateLayers = client.UpdateLayers;
			for (int i = 0; i < updateLayers.Length; i++)
			{
				updateLayers[i].Replicables.Remove(replicable);
			}
		}

		private void SendReplicationCreate(IMyReplicable obj, MyClient client, Endpoint clientEndpoint)
		{
			TypeId typeIdByType = GetTypeIdByType(obj.GetType());
			NetworkId networkIdByObject = GetNetworkIdByObject(obj);
			NetworkId networkId = NetworkId.Invalid;
			IMyReplicable parent = obj.GetParent();
			if (parent != null)
			{
				networkId = GetNetworkIdByObject(parent);
			}
			List<IMyStateGroup> list = m_replicableGroups[obj];
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			BitStream stream = bitStreamPacketData.Stream;
			stream.WriteTypeId(typeIdByType);
			stream.WriteNetworkId(networkIdByObject);
			stream.WriteNetworkId(networkId);
			IMyStreamableReplicable myStreamableReplicable = obj as IMyStreamableReplicable;
			bool flag = myStreamableReplicable?.NeedsToBeStreamed ?? false;
			if (myStreamableReplicable != null && !myStreamableReplicable.NeedsToBeStreamed)
			{
				stream.WriteByte((byte)(list.Count - 1));
			}
			else
			{
				stream.WriteByte((byte)list.Count);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (flag || !list[i].IsStreaming)
				{
					stream.WriteNetworkId(GetNetworkIdByObject(list[i]));
				}
			}
			if (flag)
			{
				client.Replicables[obj].IsStreaming = true;
				m_callback.SendReplicationCreateStreamed(bitStreamPacketData, clientEndpoint);
			}
			else
			{
				obj.OnSave(stream, clientEndpoint);
				m_callback.SendReplicationCreate(bitStreamPacketData, clientEndpoint);
			}
		}

		private void SendReplicationDestroy(IMyReplicable obj, List<EndpointId> recipients)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteNetworkId(GetNetworkIdByObject(obj));
			m_callback.SendReplicationDestroy(bitStreamPacketData, recipients);
		}

		public void ReplicableReady(MyPacket packet)
		{
			NetworkId id = packet.BitStream.ReadNetworkId();
			bool flag = packet.BitStream.ReadBool();
			if (!packet.BitStream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			if (m_clientStates.TryGetValue(packet.Sender, out MyClient value))
			{
				IMyReplicable myReplicable = GetObjectByNetworkId(id) as IMyReplicable;
				if (myReplicable != null)
				{
					if (value.Replicables.TryGetValue(myReplicable, out MyReplicableClientData value2))
					{
						if (flag)
						{
							value2.IsPending = false;
							value2.IsStreaming = false;
							value.PendingReplicables--;
							if (value.WantsBatchCompleteConfirmation && value.PendingReplicables == 0)
							{
								m_callback.SendPendingReplicablesDone(packet.Sender);
								value.WantsBatchCompleteConfirmation = false;
							}
						}
					}
					else if (!flag)
					{
						RemoveForClient(myReplicable, value, sendDestroyToClient: false);
					}
				}
				if (myReplicable != null)
				{
					ProcessBlocker(myReplicable, packet.Sender, value, null);
				}
			}
			packet.Return();
		}

		public void ReplicableRequest(MyPacket packet)
		{
			long entityId = packet.BitStream.ReadInt64();
			bool flag = packet.BitStream.ReadBool();
			byte value = 0;
			if (flag)
			{
				value = packet.BitStream.ReadByte();
			}
			IMyReplicable replicableByEntityId = m_callback.GetReplicableByEntityId(entityId);
			if (m_clientStates.TryGetValue(packet.Sender, out MyClient value2))
			{
				if (flag)
				{
					if (replicableByEntityId != null)
					{
						value2.PermanentReplicables[replicableByEntityId] = value;
					}
				}
				else if (replicableByEntityId != null)
				{
					value2.PermanentReplicables.Remove(replicableByEntityId);
				}
			}
			packet.Return();
		}

		private bool ProcessBlocker(IMyReplicable replicable, Endpoint endpoint, MyClient client, IMyReplicable parent)
		{
			if (client.BlockedReplicables.ContainsKey(replicable))
			{
				MyDestroyBlocker myDestroyBlocker = client.BlockedReplicables[replicable];
				if (myDestroyBlocker.IsProcessing)
				{
					return true;
				}
				myDestroyBlocker.IsProcessing = true;
				foreach (IMyReplicable blocker in myDestroyBlocker.Blockers)
				{
					if (!client.IsReplicableReady(replicable) || !client.IsReplicableReady(blocker))
					{
						myDestroyBlocker.IsProcessing = false;
						return false;
					}
					bool flag = true;
					if (blocker != parent)
					{
						flag = ProcessBlocker(blocker, endpoint, client, replicable);
					}
					if (!flag)
					{
						myDestroyBlocker.IsProcessing = false;
						return false;
					}
				}
				client.BlockedReplicables.Remove(replicable);
				if (myDestroyBlocker.Remove)
				{
					RemoveForClient(replicable, client, sendDestroyToClient: true);
				}
				myDestroyBlocker.IsProcessing = false;
			}
			return true;
		}

		private void AddStateGroups(IMyReplicable replicable)
		{
			using (m_tmpGroups)
			{
				(replicable as IMyStreamableReplicable)?.CreateStreamingStateGroup();
				replicable.GetStateGroups(m_tmpGroups);
				foreach (IMyStateGroup tmpGroup in m_tmpGroups)
				{
					AddNetworkObjectServer(tmpGroup);
				}
				m_replicableGroups.Add(replicable, new List<IMyStateGroup>(m_tmpGroups));
			}
		}

		private void RemoveStateGroups(IMyReplicable replicable)
		{
			foreach (MyClient value in m_clientStates.Values)
			{
				RemoveClientReplicable(replicable, value);
			}
			foreach (IMyStateGroup item in m_replicableGroups[replicable])
			{
				RemoveNetworkedObject(item);
				item.Destroy();
			}
			m_replicableGroups.Remove(replicable);
		}

		private void AddClientReplicable(IMyReplicable replicable, MyClient client, bool force)
		{
			client.Replicables.Add(replicable, new MyReplicableClientData());
			client.PendingReplicables++;
			if (m_replicableGroups.ContainsKey(replicable))
			{
				foreach (IMyStateGroup item in m_replicableGroups[replicable])
				{
					NetworkId networkIdByObject = GetNetworkIdByObject(item);
					if (!item.IsStreaming || (replicable as IMyStreamableReplicable).NeedsToBeStreamed)
					{
						client.StateGroups.Add(item, new MyStateDataEntry(replicable, networkIdByObject, item));
						ScheduleStateGroupSync(client, client.StateGroups[item], SyncFrameCounter);
						item.CreateClientData(client.State);
						if (force)
						{
							item.ForceSend(client.State);
						}
					}
				}
			}
		}

		private void RemoveClientReplicable(IMyReplicable replicable, MyClient client)
		{
			if (m_replicableGroups.ContainsKey(replicable))
			{
				using (m_tmpGroups)
				{
					replicable.GetStateGroups(m_tmpGroups);
					foreach (IMyStateGroup item in m_replicableGroups[replicable])
					{
						item.DestroyClientData(client.State);
						if (client.StateGroups.ContainsKey(item))
						{
							if (client.DirtyQueue.Contains(client.StateGroups[item]))
							{
								client.DirtyQueue.Remove(client.StateGroups[item]);
							}
							client.StateGroups.Remove(item);
						}
					}
					if (client.Replicables.TryGetValue(replicable, out MyReplicableClientData value) && value.IsPending)
					{
						client.PendingReplicables--;
					}
					client.Replicables.Remove(replicable);
					m_tmpGroups.Clear();
				}
			}
		}

		private bool ShouldSendEvent(IMyNetObject eventInstance, Vector3D? position, MyClient client)
		{
			if (position.HasValue)
			{
				if (!client.State.Position.HasValue)
				{
					return false;
				}
				int syncDistance = MyLayers.GetSyncDistance();
				if (Vector3D.DistanceSquared(position.Value, client.State.Position.Value) > (double)(syncDistance * syncDistance))
				{
					return false;
				}
			}
			if (eventInstance == null)
			{
				return true;
			}
			if (eventInstance is IMyReplicable)
			{
				return client.Replicables.ContainsKey((IMyReplicable)eventInstance);
			}
			return false;
		}

		public override MyClientStateBase GetClientData(Endpoint endpointId)
		{
			if (!m_clientStates.TryGetValue(endpointId, out MyClient value))
			{
				return null;
			}
			return value.State;
		}

		protected override bool DispatchBlockingEvent(IPacketData data, CallSite site, EndpointId target, IMyNetObject targetReplicable, Vector3D? position, IMyNetObject blockingReplicable)
		{
			Endpoint key = new Endpoint(target, 0);
			IMyReplicable blockingReplicable2 = blockingReplicable as IMyReplicable;
			IMyReplicable targetReplicable2 = targetReplicable as IMyReplicable;
			MyClient value;
			if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && clientState.Key.Index == 0 && ShouldSendEvent(targetReplicable, position, clientState.Value))
					{
						TryAddBlockerForClient(clientState.Value, targetReplicable2, blockingReplicable2);
					}
				}
			}
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(key, out value))
			{
				TryAddBlockerForClient(value, targetReplicable2, blockingReplicable2);
			}
			return DispatchEvent(data, site, target, targetReplicable, position);
		}

		private static void TryAddBlockerForClient(MyClient client, IMyReplicable targetReplicable, IMyReplicable blockingReplicable)
		{
			if (!client.IsReplicableReady(targetReplicable) || !client.IsReplicableReady(blockingReplicable) || client.BlockedReplicables.ContainsKey(targetReplicable) || client.BlockedReplicables.ContainsKey(blockingReplicable))
			{
				if (!client.BlockedReplicables.TryGetValue(targetReplicable, out MyDestroyBlocker value))
				{
					value = new MyDestroyBlocker();
					client.BlockedReplicables.Add(targetReplicable, value);
				}
				value.Blockers.Add(blockingReplicable);
				if (!client.BlockedReplicables.TryGetValue(blockingReplicable, out MyDestroyBlocker value2))
				{
					value2 = new MyDestroyBlocker();
					client.BlockedReplicables.Add(blockingReplicable, value2);
				}
				value2.Blockers.Add(targetReplicable);
			}
		}

		protected override bool DispatchEvent(IPacketData data, CallSite site, EndpointId target, IMyNetObject eventInstance, Vector3D? position)
		{
			if (m_recipients == null)
			{
				m_recipients = new List<EndpointId>();
			}
			Endpoint key = new Endpoint(target, 0);
			bool flag = false;
			MyClient value;
			if (site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
				{
					if ((!site.HasBroadcastExceptFlag || !(clientState.Key.Id == target)) && clientState.Key.Index == 0 && ShouldSendEvent(eventInstance, position, clientState.Value))
					{
						m_recipients.Add(clientState.Key.Id);
					}
				}
				if (m_recipients.Count > 0)
				{
					DispatchEvent(data, m_recipients, site.IsReliable);
					flag = true;
					m_recipients.Clear();
				}
			}
			else if (site.HasClientFlag && m_localEndpoint != target && m_clientStates.TryGetValue(key, out value) && ShouldSendEvent(eventInstance, position, value))
			{
				DispatchEvent(data, value, site.IsReliable);
				flag = true;
			}
			if (!flag)
			{
				data.Return();
			}
			return MyReplicationLayerBase.ShouldServerInvokeLocally(site, m_localEndpoint, target);
		}

		private void DispatchEvent(IPacketData data, MyClient client, bool reliable)
		{
			m_recipients.Add(client.State.EndpointId.Id);
			DispatchEvent(data, m_recipients, reliable);
			m_recipients.Clear();
		}

		private void DispatchEvent(IPacketData data, List<EndpointId> recipients, bool reliable)
		{
			m_callback.SendEvent(data, reliable, recipients);
		}

		protected override void OnEvent(MyPacketDataBitStreamBase data, CallSite site, object obj, IMyNetObject sendAs, Vector3D? position, EndpointId source)
		{
			MyClientStateBase clientData = GetClientData(new Endpoint(source, 0));
			if (clientData == null)
			{
				data.Return();
				return;
			}
			if (site.HasServerInvokedFlag)
			{
				data.Return();
				m_callback.ValidationFailed(source.Value, kick: true, "ServerInvoked " + site.ToString(), stackTrace: false);
				return;
			}
			IMyReplicable myReplicable = sendAs as IMyReplicable;
			if (myReplicable != null)
			{
				ValidationResult validationResult = myReplicable.HasRights(source, site.ValidationFlags);
				if (validationResult != 0)
				{
					data.Return();
					m_callback.ValidationFailed(source.Value, validationResult.HasFlag(ValidationResult.Kick), validationResult.ToString() + " " + site.ToString(), stackTrace: false);
					return;
				}
			}
			if (!Invoke(site, data.Stream, obj, source, clientData, validate: true))
			{
				data.Return();
				return;
			}
			if (!data.Stream.CheckTerminator())
			{
				throw new EndOfStreamException("Invalid BitStream terminator");
			}
			if (site.HasClientFlag || site.HasBroadcastFlag || site.HasBroadcastExceptFlag)
			{
				DispatchEvent(data, site, source, sendAs, position);
			}
			else
			{
				data.Return();
			}
		}

		public void SendJoinResult(ref JoinResultMsg msg, ulong sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SendJoinResult(bitStreamPacketData, new EndpointId(sendTo));
		}

		public void SendWorldData(ref ServerDataMsg msg)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_endpoints.Clear();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				m_endpoints.Add(clientState.Key.Id);
			}
			m_callback.SendWorldData(bitStreamPacketData, m_endpoints);
		}

		public void SendWorld(byte[] worldData, EndpointId sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref worldData);
			m_callback.SendWorld(bitStreamPacketData, sendTo);
		}

		public void SendPlayerData(ref PlayerDataMsg msg)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_endpoints.Clear();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				m_endpoints.Add(clientState.Key.Id);
			}
			m_callback.SendPlayerData(bitStreamPacketData, m_endpoints);
		}

		public ConnectedClientDataMsg OnClientConnected(MyPacket packet)
		{
			return MySerializer.CreateAndRead<ConnectedClientDataMsg>(packet.BitStream);
		}

		public void SendClientConnected(ref ConnectedClientDataMsg msg, ulong sendTo)
		{
			MyPacketDataBitStreamBase bitStreamPacketData = m_callback.GetBitStreamPacketData();
			MySerializer.Write(bitStreamPacketData.Stream, ref msg);
			m_callback.SentClientJoined(bitStreamPacketData, new EndpointId(sendTo));
		}

		public void InvalidateClientCache(IMyReplicable replicable, string storageName)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (clientState.Value.RemoveCache(replicable, storageName))
				{
					m_callback.SendVoxelCacheInvalidated(storageName, clientState.Key.Id);
				}
			}
		}

		public void InvalidateSingleClientCache(string storageName, EndpointId clientId)
		{
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (clientState.Key.Id == clientId)
				{
					clientState.Value.RemoveCache(null, storageName);
				}
			}
		}

		public byte GetClientReplicableIslandIndex(IMyReplicable replicable, Endpoint clientEndpoint)
		{
			return m_clientStates[clientEndpoint].GetReplicableIslandIndex(replicable);
		}

		public MyTimeSpan GetClientRelevantServerTimestamp(Endpoint clientEndpoint)
		{
			return m_serverTimeStamp;
		}

		public void GetClientPings(out SerializableDictionary<ulong, short> pings)
		{
			pings = new SerializableDictionary<ulong, short>();
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				pings[clientState.Key.Id.Value] = clientState.Value.State.Ping;
			}
		}

		public void ResendMissingReplicableChildren(IMyEventProxy target)
		{
			ResendMissingReplicableChildren(GetProxyTarget(target) as IMyReplicable);
		}

		private void ResendMissingReplicableChildren(IMyReplicable replicable)
		{
			m_replicables.GetAllChildren(replicable, m_tmpReplicableList);
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				if (clientState.Value.HasReplicable(replicable))
				{
					foreach (IMyReplicable tmpReplicable in m_tmpReplicableList)
					{
						if (!clientState.Value.HasReplicable(tmpReplicable))
						{
							AddForClient(tmpReplicable, clientState.Key, clientState.Value, force: false);
						}
					}
				}
			}
			m_tmpReplicableList.Clear();
		}

		public void SetClientBatchConfrmation(Endpoint clientEndpoint, bool value)
		{
			if (m_clientStates.TryGetValue(clientEndpoint, out MyClient value2))
			{
				value2.WantsBatchCompleteConfirmation = value;
				if (value)
				{
					value2.ResetLayerTimers();
				}
			}
		}

		/// <summary>
		/// Indicates if a replicable is replicated to at least one client.
		/// </summary>
		/// <param name="replicable">Replicable to check.</param>
		/// <returns>True if replicated at least to one client.</returns>
		public bool IsReplicated(IMyReplicable replicable)
		{
			foreach (MyClient value in m_clientStates.Values)
			{
				if (value.HasReplicable(replicable) && value.IsReplicableReady(replicable))
				{
					return true;
				}
			}
			return false;
		}

		public override string GetMultiplayerStat()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string multiplayerStat = base.GetMultiplayerStat();
			stringBuilder.Append(multiplayerStat);
			stringBuilder.AppendLine("Client state info:");
			foreach (KeyValuePair<Endpoint, MyClient> clientState in m_clientStates)
			{
				string value = string.Concat("    Endpoint: ", clientState.Key, ", Blocked Close Msgs Count: ", clientState.Value.BlockedReplicables.Count);
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString();
		}
	}
}
