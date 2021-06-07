using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Replication;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Multiplayer
{
	public abstract class MyMultiplayerClientBase : MyMultiplayerBase, IReplicationClientCallback
	{
		protected sealed class ReceiveProfiler_003C_003ESystem_Byte_003C_0023_003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in byte[] profilerData, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveProfiler(profilerData);
			}
		}

		protected sealed class ReceivePendingReplicablesDone_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceivePendingReplicablesDone();
			}
		}

		protected sealed class InvalidateVoxelCacheClient_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string storageName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				InvalidateVoxelCacheClient(storageName);
			}
		}

		private const int CONNECTION_STATE_TICKS = 12;

		private int m_ticks;

		private bool m_removingVoxelCacheFromServer;

		protected new MyReplicationClient ReplicationLayer => (MyReplicationClient)base.ReplicationLayer;

		void IReplicationClientCallback.SendClientUpdate(IPacketData data)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_UPDATE, data, reliable: false, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendClientAcks(IPacketData data)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_ACKS, data, reliable: true, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendEvent(IPacketData data, bool reliable)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendReplicableReady(IPacketData data)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_READY, data, reliable: true, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendReplicableRequest(IPacketData data)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_REQUEST, data, reliable: true, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendConnectRequest(IPacketData data)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_CONNNECTED, data, reliable: true, new EndpointId(base.ServerId), 0);
		}

		void IReplicationClientCallback.SendClientReady(MyPacketDataBitStreamBase data)
		{
			Sync.Layer.TransportLayer.SendMessage(MyMessageId.CLIENT_READY, data, reliable: true, new EndpointId(Sync.ServerId), 0);
		}

		void IReplicationClientCallback.ReadCustomState(BitStream stream)
		{
			Sync.ServerSimulationRatio = stream.ReadFloat();
			Sync.ServerCPULoadSmooth = (Sync.ServerCPULoad = stream.ReadFloat());
			Sync.ServerThreadLoadSmooth = (Sync.ServerThreadLoad = stream.ReadFloat());
		}

		public MyTimeSpan GetUpdateTime()
		{
			return MySandboxGame.Static.SimulationTimeWithSpeed;
		}

		public void SetNextFrameDelayDelta(float delay)
		{
			MySandboxGame.Static.SetNextFrameDelayDelta(delay);
		}

		public void SetPing(long duration)
		{
			MyGeneralStats.Static.Ping = duration;
		}

		public void SetIslandDone(byte index, Dictionary<long, MatrixD> matrices)
		{
			MyEntities.ReleaseWaitingAsync(index, matrices);
		}

		public float GetServerSimulationRatio()
		{
			return Sync.ServerSimulationRatio;
		}

		public float GetClientSimulationRatio()
		{
			return Math.Min(100f / MySandboxGame.Static.CPULoadSmooth, 1f);
		}

		public void DisconnectFromHost()
		{
			DisconnectClient(0uL);
		}

		public void UpdateSnapshotCache()
		{
			MySnapshotCache.Apply();
		}

		public MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			return MyNetworkWriter.GetBitStreamPacketData();
		}

		public void PauseClient(bool pause)
		{
			if (pause)
			{
				MySandboxGame.PausePush();
				MyHud.Notifications.Add(MyNotificationSingletons.ConnectionProblem);
			}
			else
			{
				MySandboxGame.PausePop();
				MyHud.Notifications.Remove(MyNotificationSingletons.ConnectionProblem);
			}
		}

		protected MyMultiplayerClientBase(MySyncLayer syncLayer)
			: base(syncLayer)
		{
			SetReplicationLayer(new MyReplicationClient(new Endpoint(Sync.MyId, 0), this, CreateClientState(), 16.6666679f, JoinFailCallback, MyFakes.MULTIPLAYER_PREDICTION_RESET_CLIENT_FALLING_BEHIND, Thread.CurrentThread));
			ReplicationLayer.UseSmoothPing = MyFakes.MULTIPLAYER_SMOOTH_PING;
			MyReplicationClient.SynchronizationTimingType = MyReplicationClient.TimingType.LastServerTime;
			syncLayer.TransportLayer.Register(MyMessageId.SERVER_DATA, 0, ReplicationLayer.OnServerData);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_CREATE, 0, ReplicationLayer.ProcessReplicationCreate);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_DESTROY, 0, ReplicationLayer.ProcessReplicationDestroy);
			syncLayer.TransportLayer.Register(MyMessageId.SERVER_STATE_SYNC, 0, ReplicationLayer.OnServerStateSync);
			syncLayer.TransportLayer.Register(MyMessageId.RPC, 0, ReplicationLayer.OnEvent);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_STREAM_BEGIN, 0, ReplicationLayer.ProcessReplicationCreateBegin);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_ISLAND_DONE, 0, ReplicationLayer.ProcessReplicationIslandDone);
			syncLayer.TransportLayer.Register(MyMessageId.WORLD, 0, ReceiveWorld);
			syncLayer.TransportLayer.Register(MyMessageId.PLAYER_DATA, 0, ReceivePlayerData);
			MyNetworkMonitor.OnTick += OnTick;
			m_voxelMapData = new LRUCache<string, byte[]>(100);
			LRUCache<string, byte[]> voxelMapData = m_voxelMapData;
			voxelMapData.OnItemDiscarded = (Action<string, byte[]>)Delegate.Combine(voxelMapData.OnItemDiscarded, (Action<string, byte[]>)delegate(string name, byte[] _)
			{
				if (!m_removingVoxelCacheFromServer)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerBase.InvalidateVoxelCache, name);
				}
			});
		}

		public override void Dispose()
		{
			base.Dispose();
			MyNetworkMonitor.OnTick -= OnTick;
		}

		private void OnTick()
		{
			m_ticks++;
			if (m_ticks > 12)
			{
				MyP2PSessionState state = default(MyP2PSessionState);
				MyGameService.Peer2Peer.GetSessionState(base.ServerId, ref state);
				base.IsConnectionDirect = !state.UsingRelay;
				base.IsConnectionAlive = state.ConnectionActive;
				m_ticks = 0;
			}
		}

		private void JoinFailCallback(string message)
		{
			MyGuiSandbox.Show(new StringBuilder(message));
		}

		public override void Tick()
		{
			base.Tick();
			MySession.Static.VirtualClients.Tick();
		}

		public override void OnSessionReady()
		{
			ClientReadyDataMsg clientReadyDataMsg = default(ClientReadyDataMsg);
			clientReadyDataMsg.UsePlayoutDelayBufferForCharacter = true;
			clientReadyDataMsg.UsePlayoutDelayBufferForJetpack = true;
			clientReadyDataMsg.UsePlayoutDelayBufferForGrids = true;
			ClientReadyDataMsg msg = clientReadyDataMsg;
			ReplicationLayer.SendClientReady(ref msg);
		}

		public override void DownloadWorld()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerServerBase.WorldRequest);
		}

		public override void DownloadProfiler()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerServerBase.ProfilerRequest);
		}

		public void RequestBatchConfirmation()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerServerBase.RequestBatchConfirmation);
		}

		public void ReceiveWorld(MyPacket packet)
		{
			MyObjectBuilderSerializer.DeserializeGZippedXML(new MemoryStream(MySerializer.CreateAndRead<byte[]>(packet.BitStream)), out MyObjectBuilder_World objectBuilder);
			MyJoinGameHelper.WorldReceived(objectBuilder, MyMultiplayer.Static);
		}

		public void ReceivePlayerData(MyPacket packet)
		{
			PlayerDataMsg playerDataMsg = ReplicationLayer.OnPlayerData(packet);
			if (SyncLayer.TransportLayer.IsBuffering && playerDataMsg.ClientSteamId != Sync.MyId)
			{
				packet.BitStream.SetBitPositionRead(0);
				SyncLayer.TransportLayer.AddMessageToBuffer(packet);
			}
			else
			{
				MySession.Static.Players.OnInitialPlayerCreated(playerDataMsg.ClientSteamId, playerDataMsg.PlayerSerialId, playerDataMsg.IdentityId, playerDataMsg.DisplayName, playerDataMsg.BuildColors, playerDataMsg.RealPlayer, playerDataMsg.NewIdentity);
			}
		}

		[Event(null, 252)]
		[Reliable]
		[Client]
		public static void ReceiveProfiler(byte[] profilerData)
		{
			MemoryStream reader = new MemoryStream(profilerData);
			try
			{
				MyObjectBuilderSerializer.DeserializeGZippedXML(reader, out MyObjectBuilder_ProfilerSnapshot objectBuilder);
				objectBuilder.Init(MyRenderProxy.GetRenderProfiler(), SnapshotType.Server, subtract: false);
				MyMultiplayer.Static.ProfilerDone.InvokeIfNotNull("ProfilerDownload: Done.");
			}
			catch
			{
				MyMultiplayer.Static.ProfilerDone.InvokeIfNotNull("ProfilerDownload: Could not parse data.");
			}
		}

		[Event(null, 270)]
		[Reliable]
		[Client]
		public new static void ReceivePendingReplicablesDone()
		{
			MyMultiplayer.Static.ReceivePendingReplicablesDone();
		}

		public override void KickClient(ulong client, bool kicked = true, bool add = true)
		{
			MyControlKickClientMsg myControlKickClientMsg = default(MyControlKickClientMsg);
			myControlKickClientMsg.KickedClient = client;
			myControlKickClientMsg.Kicked = kicked;
			MyControlKickClientMsg message = myControlKickClientMsg;
			SendControlMessage(base.ServerId, ref message);
		}

		protected override void OnClientKick(ref MyControlKickClientMsg data, ulong sender)
		{
			if ((bool)data.Kicked)
			{
				if (data.KickedClient == Sync.MyId)
				{
					MySessionLoader.UnloadAndExitToMenu();
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionKicked), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextYouHaveBeenKicked)));
					return;
				}
				if ((bool)data.Add)
				{
					AddKickedClient(data.KickedClient);
				}
				RaiseClientLeft(data.KickedClient, MyChatMemberStateChangeEnum.Kicked);
			}
			else
			{
				RemoveKickedClient(data.KickedClient);
			}
		}

		[Event(null, 306)]
		[Reliable]
		[Client]
		public static void InvalidateVoxelCacheClient(string storageName)
		{
			MyMultiplayerClientBase obj = (MyMultiplayerClientBase)MyMultiplayer.Static;
			obj.m_removingVoxelCacheFromServer = true;
			obj.m_voxelMapData.Remove(storageName);
			obj.m_removingVoxelCacheFromServer = false;
		}
	}
}
