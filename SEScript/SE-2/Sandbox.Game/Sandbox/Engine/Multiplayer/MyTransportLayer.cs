using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;

namespace Sandbox.Engine.Multiplayer
{
	internal class MyTransportLayer
	{
		private struct HandlerId
		{
			public MyMessageId messageId;

			public byte receiverIndex;
		}

		private static readonly int m_messageTypeCount;

		private readonly Queue<int>[] m_slidingWindows = (from s in Enumerable.Range(0, m_messageTypeCount)
			select new Queue<int>(120)).ToArray();

		private readonly int[] m_thisFrameTraffic = new int[m_messageTypeCount];

		private bool m_isBuffering;

		private readonly int m_channel;

		private List<MyPacket> m_buffer;

		private byte m_largeMessageParts;

		private int m_largeMessageSize;

		private readonly Dictionary<HandlerId, Action<MyPacket>> m_handlers = new Dictionary<HandlerId, Action<MyPacket>>();

		public bool IsProcessingBuffer
		{
			get;
			private set;
		}

		public bool IsBuffering
		{
			get
			{
				return m_isBuffering;
			}
			set
			{
				m_isBuffering = value;
				if (m_isBuffering && m_buffer == null)
				{
					m_buffer = new List<MyPacket>();
				}
				else if (!m_isBuffering && m_buffer != null)
				{
					ProcessBuffer();
					m_buffer = null;
				}
			}
		}

		public Action<ulong> DisconnectPeerOnError
		{
			get;
			set;
		}

		static MyTransportLayer()
		{
			m_messageTypeCount = (int)(MyEnum<MyMessageId>.Range.Max + 1);
		}

		public MyTransportLayer(int channel)
		{
			m_channel = channel;
			DisconnectPeerOnError = null;
			MyNetworkReader.SetHandler(channel, HandleMessage, delegate(ulong x)
			{
				DisconnectPeerOnError(x);
			});
		}

		public void SendFlush(ulong sendTo)
		{
			MyNetworkWriter.SendPacket(InitSendStream(new EndpointId(sendTo), MyP2PMessageEnum.ReliableWithBuffering, MyMessageId.FLUSH, 0));
		}

		private MyNetworkWriter.MyPacketDescriptor InitSendStream(EndpointId endpoint, MyP2PMessageEnum msgType, MyMessageId msgId, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.GetPacketDescriptor(endpoint, msgType, m_channel);
			packetDescriptor.Header.WriteByte((byte)msgId);
			packetDescriptor.Header.WriteByte(index);
			return packetDescriptor;
		}

		public void SendMessage(MyMessageId id, IPacketData data, bool reliable, EndpointId endpoint, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor myPacketDescriptor = InitSendStream(endpoint, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
			myPacketDescriptor.Data = data;
			MyNetworkWriter.SendPacket(myPacketDescriptor);
		}

		public void SendMessage(MyMessageId id, IPacketData data, bool reliable, List<EndpointId> endpoints, byte index = 0)
		{
			MyNetworkWriter.MyPacketDescriptor myPacketDescriptor = InitSendStream(EndpointId.Null, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
			myPacketDescriptor.Recipients.AddRange(endpoints);
			myPacketDescriptor.Data = data;
			MyNetworkWriter.SendPacket(myPacketDescriptor);
		}

		private void ProfilePacketStatistics(bool begin)
		{
			if (begin)
			{
				MyStatsGraph.ProfileAdvanced(begin: true);
				MyStatsGraph.Begin("Packet statistics", 0, "ProfilePacketStatistics", 117, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			}
			else
			{
				MyStatsGraph.End(null, 0f, "", "{0} B", null, "ProfilePacketStatistics", 121, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				MyStatsGraph.ProfileAdvanced(begin: false);
			}
		}

		public void Tick()
		{
			int num = 0;
			ProfilePacketStatistics(begin: true);
			MyStatsGraph.Begin("Average data", int.MaxValue, "Tick", 130, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			for (int i = 0; i < m_messageTypeCount; i++)
			{
				Queue<int> queue = m_slidingWindows[i];
				queue.Enqueue(m_thisFrameTraffic[i]);
				m_thisFrameTraffic[i] = 0;
				while (queue.Count > 60)
				{
					queue.Dequeue();
				}
				int num2 = 0;
				foreach (int item in queue)
				{
					num2 += item;
				}
				if (num2 > 0)
				{
					MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName((MyMessageId)i), int.MaxValue, "Tick", 147, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
					MyStatsGraph.End((float)num2 / 60f, (float)num2 / 1024f, "{0} KB/s", "{0} B", null, "Tick", 148, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				}
				num += num2;
			}
			MyStatsGraph.End((float)num / 60f, (float)num / 1024f, "{0} KB/s", "{0} B", null, "Tick", 152, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			ProfilePacketStatistics(begin: false);
		}

		private void ProcessBuffer()
		{
			try
			{
				IsProcessingBuffer = true;
				ProfilePacketStatistics(begin: true);
				MyStatsGraph.Begin("Live data", 0, "ProcessBuffer", 162, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				foreach (MyPacket item in m_buffer)
				{
					ProcessMessage(item);
				}
				MyStatsGraph.End(null, 0f, "", "{0} B", null, "ProcessBuffer", 167, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				ProfilePacketStatistics(begin: false);
			}
			finally
			{
				IsProcessingBuffer = false;
			}
		}

		private void HandleMessage(MyPacket p)
		{
			int bitPosition = p.BitStream.BitPosition;
			MyMessageId myMessageId = (MyMessageId)p.BitStream.ReadByte();
			if (myMessageId == MyMessageId.FLUSH)
			{
				ClearBuffer();
				p.Return();
				return;
			}
			p.BitStream.SetBitPositionRead(bitPosition);
			if (IsBuffering && myMessageId != MyMessageId.JOIN_RESULT && myMessageId != MyMessageId.WORLD_DATA && myMessageId != MyMessageId.WORLD && myMessageId != MyMessageId.PLAYER_DATA)
			{
				m_buffer.Add(p);
				return;
			}
			ProfilePacketStatistics(begin: true);
			MyStatsGraph.Begin("Live data", 0, "HandleMessage", 197, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			ProcessMessage(p);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, "HandleMessage", 199, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			ProfilePacketStatistics(begin: false);
		}

		private void ProcessMessage(MyPacket p)
		{
			HandlerId key = default(HandlerId);
			key.messageId = (MyMessageId)p.BitStream.ReadByte();
			key.receiverIndex = p.BitStream.ReadByte();
			if ((long)key.messageId < (long)m_thisFrameTraffic.Length)
			{
				m_thisFrameTraffic[(uint)key.messageId] += p.BitStream.ByteLength;
			}
			p.Sender = new Endpoint(p.Sender.Id, key.receiverIndex);
			if (!m_handlers.TryGetValue(key, out Action<MyPacket> value))
			{
				m_handlers.TryGetValue(new HandlerId
				{
					messageId = key.messageId,
					receiverIndex = byte.MaxValue
				}, out value);
			}
			if (value != null)
			{
				int byteLength = p.BitStream.ByteLength;
				MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName(key.messageId), int.MaxValue, "ProcessMessage", 226, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
				value(p);
				MyStatsGraph.End(byteLength, 0f, "", "{0} B", null, "ProcessMessage", 228, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
			}
			else
			{
				p.Return();
			}
		}

		public void AddMessageToBuffer(MyPacket packet)
		{
			m_buffer.Add(packet);
		}

		[Conditional("DEBUG")]
		private void TraceMessage(string text, string messageText, ulong userId, long messageSize, MyP2PMessageEnum sendType)
		{
			if (MyMultiplayer.Static != null && MyMultiplayer.Static.SyncLayer.Clients.TryGetClient(userId, out MyNetworkClient client))
			{
				_ = client.DisplayName;
			}
			else
			{
				userId.ToString();
			}
			if (sendType != MyP2PMessageEnum.Reliable)
			{
				_ = 3;
			}
		}

		public void Register(MyMessageId messageId, byte receiverIndex, Action<MyPacket> handler)
		{
			HandlerId handlerId = default(HandlerId);
			handlerId.messageId = messageId;
			handlerId.receiverIndex = receiverIndex;
			HandlerId key = handlerId;
			m_handlers.Add(key, handler);
		}

		public void Unregister(MyMessageId messageId, byte receiverIndex)
		{
			HandlerId handlerId = default(HandlerId);
			handlerId.messageId = messageId;
			handlerId.receiverIndex = receiverIndex;
			HandlerId key = handlerId;
			m_handlers.Remove(key);
		}

		private void ClearBuffer()
		{
			if (m_buffer != null)
			{
				foreach (MyPacket item in m_buffer)
				{
					item.Return();
				}
				m_buffer.Clear();
			}
		}

		public void Clear()
		{
			MyNetworkReader.ClearHandler(2);
			ClearBuffer();
		}
	}
}
