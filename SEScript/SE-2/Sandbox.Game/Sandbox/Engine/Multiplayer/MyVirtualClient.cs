using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	internal class MyVirtualClient
	{
		private readonly MyClientStateBase m_clientState;

		private readonly List<byte> m_acks = new List<byte>();

		private byte m_lastStateSyncPacketId;

		private byte m_clientPacketId;

		public MyPlayer.PlayerId PlayerId
		{
			get;
			private set;
		}

		private static MyTransportLayer TransportLayer => MyMultiplayer.Static.SyncLayer.TransportLayer;

		public MyVirtualClient(Endpoint endPoint, MyClientStateBase clientState, MyPlayer.PlayerId playerId)
		{
			m_clientState = clientState;
			m_clientState.EndpointId = endPoint;
			m_clientState.PlayerSerialId = playerId.SerialId;
			PlayerId = playerId;
			TransportLayer.Register(MyMessageId.SERVER_DATA, endPoint.Index, OnServerData);
			TransportLayer.Register(MyMessageId.REPLICATION_CREATE, endPoint.Index, OnReplicationCreate);
			TransportLayer.Register(MyMessageId.REPLICATION_DESTROY, endPoint.Index, OnReplicationDestroy);
			TransportLayer.Register(MyMessageId.SERVER_STATE_SYNC, endPoint.Index, OnServerStateSync);
			TransportLayer.Register(MyMessageId.RPC, endPoint.Index, OnEvent);
			TransportLayer.Register(MyMessageId.REPLICATION_STREAM_BEGIN, endPoint.Index, OnReplicationStreamBegin);
			TransportLayer.Register(MyMessageId.JOIN_RESULT, endPoint.Index, OnJoinResult);
			TransportLayer.Register(MyMessageId.WORLD_DATA, endPoint.Index, OnWorldData);
			TransportLayer.Register(MyMessageId.CLIENT_CONNNECTED, endPoint.Index, OnClientConnected);
			TransportLayer.Register(MyMessageId.REPLICATION_ISLAND_DONE, endPoint.Index, OnReplicationIslandDone);
		}

		public void Tick()
		{
			SendUpdate();
		}

		private void SendUpdate()
		{
			MyPacketDataBitStreamBase bitStreamPacketData = MyNetworkWriter.GetBitStreamPacketData();
			BitStream stream = bitStreamPacketData.Stream;
			stream.WriteByte(m_lastStateSyncPacketId);
			byte value = (byte)m_acks.Count;
			stream.WriteByte(value);
			foreach (byte ack in m_acks)
			{
				stream.WriteByte(ack);
			}
			stream.Terminate();
			m_acks.Clear();
			SendClientAcks(bitStreamPacketData);
			bitStreamPacketData = MyNetworkWriter.GetBitStreamPacketData();
			stream = bitStreamPacketData.Stream;
			m_clientPacketId++;
			stream.WriteByte(m_clientPacketId);
			stream.WriteDouble(MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()).Milliseconds);
			stream.WriteDouble(0.0);
			m_clientState.Serialize(stream, outOfOrder: false);
			stream.Terminate();
			SendClientUpdate(bitStreamPacketData);
		}

		private void OnReplicationIslandDone(MyPacket packet)
		{
			packet.Return();
		}

		private void OnClientConnected(MyPacket packet)
		{
			throw new NotImplementedException();
		}

		private void OnWorldData(MyPacket packet)
		{
			throw new NotImplementedException();
		}

		private void OnJoinResult(MyPacket packet)
		{
			throw new NotImplementedException();
		}

		private void OnReplicationCreate(MyPacket packet)
		{
			packet.BitStream.ReadTypeId();
			NetworkId networkId = packet.BitStream.ReadNetworkId();
			MyPacketDataBitStreamBase bitStreamPacketData = MyNetworkWriter.GetBitStreamPacketData();
			bitStreamPacketData.Stream.WriteNetworkId(networkId);
			bitStreamPacketData.Stream.WriteBool(value: true);
			bitStreamPacketData.Stream.Terminate();
			SendReplicableReady(bitStreamPacketData);
			packet.Return();
		}

		private void OnReplicationStreamBegin(MyPacket packet)
		{
			OnReplicationCreate(packet);
			packet.Return();
		}

		private void OnEvent(MyPacket packet)
		{
			throw new NotImplementedException();
		}

		private void OnServerStateSync(MyPacket packet)
		{
			bool num = packet.BitStream.ReadBool();
			byte b = packet.BitStream.ReadByte();
			if (!num && !m_acks.Contains(b))
			{
				m_acks.Add(b);
			}
			m_lastStateSyncPacketId = b;
			packet.Return();
		}

		private void OnReplicationDestroy(MyPacket packet)
		{
			packet.Return();
		}

		private void OnServerData(MyPacket packet)
		{
			packet.Return();
		}

		private void SendClientUpdate(IPacketData data)
		{
			TransportLayer.SendMessage(MyMessageId.CLIENT_UPDATE, data, reliable: false, new EndpointId(Sync.ServerId), m_clientState.EndpointId.Index);
		}

		private void SendClientAcks(IPacketData data)
		{
			TransportLayer.SendMessage(MyMessageId.CLIENT_ACKS, data, reliable: true, new EndpointId(Sync.ServerId), m_clientState.EndpointId.Index);
		}

		private void SendEvent(IPacketData data, bool reliable)
		{
			TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, new EndpointId(Sync.ServerId), m_clientState.EndpointId.Index);
		}

		private void SendReplicableReady(IPacketData data)
		{
			TransportLayer.SendMessage(MyMessageId.REPLICATION_READY, data, reliable: true, new EndpointId(Sync.ServerId), m_clientState.EndpointId.Index);
		}

		private void SendConnectRequest(IPacketData data)
		{
			TransportLayer.SendMessage(MyMessageId.CLIENT_CONNNECTED, data, reliable: true, new EndpointId(Sync.ServerId), m_clientState.EndpointId.Index);
		}
	}
}
