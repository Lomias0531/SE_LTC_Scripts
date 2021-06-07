using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Replication;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Multiplayer
{
	public abstract class MyMultiplayerServerBase : MyMultiplayerBase, IReplicationServerCallback
	{
		private class ProfilerData : WorkData
		{
			public EndpointId Sender;

			public byte[] Buffer;
		}

		protected sealed class WorldRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				WorldRequest();
			}
		}

		protected sealed class ProfilerRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProfilerRequest();
			}
		}

		protected sealed class RequestBatchConfirmation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestBatchConfirmation();
			}
		}

		private readonly MyReplicableFactory m_factory = new MyReplicableFactory();

		private MemoryStream m_worldSendStream;

		protected new MyReplicationServer ReplicationLayer => (MyReplicationServer)base.ReplicationLayer;

		protected MyMultiplayerServerBase(MySyncLayer syncLayer, EndpointId localClientEndpoint)
			: base(syncLayer)
		{
			MyReplicationServer replicationLayer = new MyReplicationServer(this, localClientEndpoint, Thread.CurrentThread);
			SetReplicationLayer(replicationLayer);
			base.ClientLeft += delegate(ulong steamId, MyChatMemberStateChangeEnum e)
			{
				MyMultiplayerServerBase myMultiplayerServerBase = this;
				MySandboxGame.Static.Invoke(delegate
				{
					myMultiplayerServerBase.ReplicationLayer.OnClientLeft(new EndpointId(steamId));
				}, "P2P Client left");
			};
			base.ClientJoined += delegate(ulong steamId)
			{
				ReplicationLayer.OnClientJoined(new EndpointId(steamId), CreateClientState());
			};
			MyEntities.OnEntityCreate += CreateReplicableForObject;
			MyEntityComponentBase.OnAfterAddedToContainer += CreateReplicableForObject;
			MyExternalReplicable.Destroyed += DestroyReplicable;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				CreateReplicableForObject(entity);
				MyEntityComponentContainer components = entity.Components;
				if (components != null)
				{
					foreach (MyComponentBase item in components)
					{
						CreateReplicableForObject(item);
					}
				}
			}
			syncLayer.TransportLayer.Register(MyMessageId.RPC, byte.MaxValue, ReplicationLayer.OnEvent);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_READY, byte.MaxValue, ReplicationLayer.ReplicableReady);
			syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_REQUEST, byte.MaxValue, ReplicationLayer.ReplicableRequest);
			syncLayer.TransportLayer.Register(MyMessageId.CLIENT_UPDATE, byte.MaxValue, ReplicationLayer.OnClientUpdate);
			syncLayer.TransportLayer.Register(MyMessageId.CLIENT_ACKS, byte.MaxValue, ReplicationLayer.OnClientAcks);
			syncLayer.TransportLayer.Register(MyMessageId.CLIENT_READY, byte.MaxValue, ClientReady);
		}

		public void RaiseReplicableCreated(object obj)
		{
			CreateReplicableForObject(obj);
		}

		private void CreateReplicableForObject(object obj)
		{
			if (obj == null || obj is MyInventoryAggregate)
			{
				return;
			}
			MyEntity myEntity = obj as MyEntity;
			if (myEntity == null || !myEntity.IsPreview)
			{
				Type type = m_factory.FindTypeFor(obj);
				if (type != null && ReplicationLayer.IsTypeReplicated(type))
				{
					MyExternalReplicable myExternalReplicable = (MyExternalReplicable)Activator.CreateInstance(type);
					myExternalReplicable.Hook(obj);
					ReplicationLayer.Replicate(myExternalReplicable);
					myExternalReplicable.OnServerReplicate();
				}
			}
		}

		private void DestroyReplicable(MyExternalReplicable obj)
		{
			ReplicationLayer.Destroy(obj);
		}

		public override void Dispose()
		{
			MyEntities.OnEntityCreate -= CreateReplicableForObject;
			MyEntityComponentBase.OnAfterAddedToContainer -= CreateReplicableForObject;
			MyExternalReplicable.Destroyed -= DestroyReplicable;
			base.Dispose();
		}

		private void ClientReady(MyPacket packet)
		{
			ReplicationLayer.OnClientReady(packet.Sender, packet);
			packet.Return();
		}

		[Event(null, 129)]
		[Reliable]
		[Server]
		public static void WorldRequest()
		{
			(MyMultiplayer.Static as MyMultiplayerServerBase).OnWorldRequest(MyEventContext.Current.Sender);
		}

		protected void OnWorldRequest(EndpointId sender)
		{
			MySandboxGame.Log.WriteLineAndConsole("World request received: " + GetMemberName(sender.Value));
			if (IsClientKickedOrBanned(sender.Value) || (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Banned.Contains(sender.Value)))
			{
				MySandboxGame.Log.WriteLineAndConsole("Sending no world, because client has been kicked or banned: " + GetMemberName(sender.Value) + " (Client is probably modified.)");
				RaiseClientLeft(sender.Value, MyChatMemberStateChangeEnum.Banned);
				return;
			}
			m_worldSendStream = new MemoryStream();
			if (IsServer && MySession.Static != null)
			{
				MySandboxGame.Log.WriteLine("...responding");
				long key = MySession.Static.Players.TryGetIdentityId(sender.Value);
				MyObjectBuilder_World world = MySession.Static.GetWorld(includeEntities: false);
				MyObjectBuilder_Checkpoint checkpoint = world.Checkpoint;
				checkpoint.WorkshopId = null;
				checkpoint.CharacterToolbar = null;
				checkpoint.Settings.ScenarioEditMode = (checkpoint.Settings.ScenarioEditMode && !MySession.Static.LoadedAsMission);
				checkpoint.Gps.Dictionary.TryGetValue(key, out MyObjectBuilder_Gps value);
				checkpoint.Gps.Dictionary.Clear();
				if (value != null)
				{
					checkpoint.Gps.Dictionary.Add(key, value);
				}
				world.Clusters = new List<BoundingBoxD>();
				MyPhysics.SerializeClusters(world.Clusters);
				world.Planets = MySession.Static.GetPlanetObjectBuilders();
				MyObjectBuilderSerializer.SerializeXML(m_worldSendStream, world, MyObjectBuilderSerializer.XmlCompression.Gzip);
				SyncLayer.TransportLayer.SendFlush(sender.Value);
				byte[] worldData = m_worldSendStream.ToArray();
				ReplicationLayer.SendWorld(worldData, MyEventContext.Current.Sender);
			}
		}

		private void ProfilerRequestAsync(WorkData data)
		{
			ProfilerData profilerData = data as ProfilerData;
			try
			{
				MyObjectBuilder_ProfilerSnapshot objectBuilder = MyObjectBuilder_ProfilerSnapshot.GetObjectBuilder(MyRenderProxy.GetRenderProfiler());
				VRage.Profiler.MyRenderProfiler.AddPause(pause: false);
				MemoryStream memoryStream = new MemoryStream();
				MyObjectBuilderSerializer.SerializeXML(memoryStream, objectBuilder, MyObjectBuilderSerializer.XmlCompression.Gzip);
				profilerData.Buffer = memoryStream.ToArray();
				MyLog.Default.WriteLine("Profiler for " + MySession.Static.Players.TryGetIdentityNameFromSteamId(profilerData.Sender.Value) + " serialized");
			}
			catch
			{
				MyLog.Default.WriteLine("Profiler serialization for " + MySession.Static.Players.TryGetIdentityNameFromSteamId(profilerData.Sender.Value) + " crashed");
			}
		}

		private void OnProfilerRequestComplete(WorkData data)
		{
			ProfilerData profilerData = data as ProfilerData;
			SyncLayer.TransportLayer.SendFlush(profilerData.Sender.Value);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerClientBase.ReceiveProfiler, profilerData.Buffer, new EndpointId(profilerData.Sender.Value));
		}

		[Event(null, 219)]
		[Reliable]
		[Server]
		public static void ProfilerRequest()
		{
			(MyMultiplayer.Static as MyMultiplayerServerBase).OnProfilerRequest(MyEventContext.Current.Sender);
		}

		private void OnProfilerRequest(EndpointId sender)
		{
			if (IsServer)
			{
				MyLog.Default.WriteLine("Profiler request received from " + MySession.Static.Players.TryGetIdentityNameFromSteamId(sender.Value));
				ProfilerData workData = new ProfilerData
				{
					Sender = sender,
					Priority = WorkPriority.Low
				};
				VRage.Profiler.MyRenderProfiler.AddPause(pause: true);
				Parallel.Start(ProfilerRequestAsync, OnProfilerRequestComplete, workData);
			}
			else
			{
				MyLog.Default.WriteLine("Profiler request received from " + MySession.Static.Players.TryGetIdentityNameFromSteamId(sender.Value) + ", but ignored");
			}
		}

		[Event(null, 244)]
		[Reliable]
		[Server]
		public static void RequestBatchConfirmation()
		{
			(MyMultiplayer.Static.ReplicationLayer as MyReplicationServer).SetClientBatchConfrmation(new Endpoint(MyEventContext.Current.Sender, 0), value: true);
		}

		public void SendPendingReplicablesDone(Endpoint endpoint)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerClientBase.ReceivePendingReplicablesDone, endpoint.Id);
		}

		void IReplicationServerCallback.SendServerData(IPacketData data, Endpoint endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.SERVER_DATA, data, reliable: true, endpoint.Id, endpoint.Index);
		}

		void IReplicationServerCallback.SendReplicationCreate(IPacketData data, Endpoint endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_CREATE, data, reliable: true, endpoint.Id, endpoint.Index);
		}

		void IReplicationServerCallback.SendReplicationCreateStreamed(IPacketData data, Endpoint endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_STREAM_BEGIN, data, reliable: true, endpoint.Id, endpoint.Index);
		}

		void IReplicationServerCallback.SendReplicationDestroy(IPacketData data, List<EndpointId> endpoints)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_DESTROY, data, reliable: true, endpoints, 0);
		}

		void IReplicationServerCallback.SendReplicationIslandDone(IPacketData data, Endpoint endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_ISLAND_DONE, data, reliable: true, endpoint.Id, endpoint.Index);
		}

		void IReplicationServerCallback.SendStateSync(IPacketData data, Endpoint endpoint, bool reliable)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.SERVER_STATE_SYNC, data, reliable, endpoint.Id, endpoint.Index);
		}

		void IReplicationServerCallback.SendWorldData(IPacketData data, List<EndpointId> endpoints)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.WORLD_DATA, data, reliable: true, endpoints, 0);
		}

		void IReplicationServerCallback.SendWorld(IPacketData data, EndpointId endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.WORLD, data, reliable: true, endpoint, 0);
		}

		void IReplicationServerCallback.SendJoinResult(IPacketData data, EndpointId endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.JOIN_RESULT, data, reliable: true, endpoint, 0);
		}

		void IReplicationServerCallback.SendEvent(IPacketData data, bool reliable, List<EndpointId> endpoints)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, endpoints, 0);
		}

		void IReplicationServerCallback.SentClientJoined(IPacketData data, EndpointId endpoint)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_CONNNECTED, data, reliable: true, endpoint, 0);
		}

		void IReplicationServerCallback.SendPlayerData(IPacketData data, List<EndpointId> endpoints)
		{
			SyncLayer.TransportLayer.SendMessage(MyMessageId.PLAYER_DATA, data, reliable: true, endpoints, 0);
		}

		void IReplicationServerCallback.WriteCustomState(BitStream stream)
		{
			stream.WriteFloat(MyPhysics.SimulationRatio);
			float cPULoad = MySandboxGame.Static.CPULoad;
			stream.WriteFloat(cPULoad);
			float threadLoad = MySandboxGame.Static.ThreadLoad;
			stream.WriteFloat(threadLoad);
		}

		int IReplicationServerCallback.GetMTUSize(Endpoint clientId)
		{
			return 1190;
		}

		int IReplicationServerCallback.GetMTRSize(Endpoint clientId)
		{
			return 1048575;
		}

		IMyReplicable IReplicationServerCallback.GetReplicableByEntityId(long entityId)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				return MyExternalReplicable.FindByObject(entity);
			}
			return null;
		}

		void IReplicationServerCallback.SendVoxelCacheInvalidated(string storageName, EndpointId endpoint)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerClientBase.InvalidateVoxelCacheClient, storageName, endpoint);
		}

		public MyTimeSpan GetUpdateTime()
		{
			return MySandboxGame.Static.SimulationTimeWithSpeed;
		}

		public MyPacketDataBitStreamBase GetBitStreamPacketData()
		{
			return MyNetworkWriter.GetBitStreamPacketData();
		}

		public override void KickClient(ulong userId, bool kicked = true, bool add = true)
		{
			MyControlKickClientMsg myControlKickClientMsg;
			if (kicked)
			{
				myControlKickClientMsg = default(MyControlKickClientMsg);
				myControlKickClientMsg.KickedClient = userId;
				myControlKickClientMsg.Kicked = kicked;
				myControlKickClientMsg.Add = add;
				MyControlKickClientMsg message = myControlKickClientMsg;
				MyLog.Default.WriteLineAndConsole("Player " + GetMemberName(userId) + " kicked");
				SendControlMessageToAll(ref message, 0uL);
				if (add)
				{
					AddKickedClient(userId);
				}
				RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Kicked);
			}
			else
			{
				myControlKickClientMsg = default(MyControlKickClientMsg);
				myControlKickClientMsg.KickedClient = userId;
				myControlKickClientMsg.Kicked = kicked;
				MyControlKickClientMsg message2 = myControlKickClientMsg;
				MyLog.Default.WriteLineAndConsole("Player " + userId + " unkicked");
				RemoveKickedClient(userId);
				SendControlMessageToAll(ref message2, 0uL);
			}
		}

		protected override void OnClientKick(ref MyControlKickClientMsg data, ulong sender)
		{
			if (MySession.Static.IsUserAdmin(sender))
			{
				KickClient(data.KickedClient, data.Kicked);
			}
		}

		public void ValidationFailed(ulong clientId, bool kick = true, string additionalInfo = null, bool stackTrace = true)
		{
			string msg = MySession.Static.Players.TryGetIdentityNameFromSteamId(clientId) + (kick ? " was trying to cheat!" : "'s action was blocked.");
			MyLog.Default.WriteLine(msg);
			if (additionalInfo != null)
			{
				MyLog.Default.WriteLine(additionalInfo);
			}
			if (stackTrace)
			{
				MyLog.Default.WriteLine(Environment.StackTrace);
			}
		}

		public override void OnSessionReady()
		{
		}
	}
}
