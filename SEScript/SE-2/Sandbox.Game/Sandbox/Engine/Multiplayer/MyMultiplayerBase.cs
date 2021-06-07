using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;
using VRage.Replication;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[StaticEventOwner]
	[PreloadRequired]
	public abstract class MyMultiplayerBase : MyMultiplayerMinimalBase, IDisposable
	{
		protected struct MyConnectedClientData
		{
			public string Name;

			public bool IsAdmin;

			public bool IsProfiling;
		}

		protected sealed class OnSetPriorityMultiplier_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float priority, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetPriorityMultiplier(priority);
			}
		}

		protected sealed class OnSetDebugEntity_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetDebugEntity(entityId);
			}
		}

		protected sealed class OnCharacterParentChangeTimeOut_003C_003ESystem_Double : ICallSite<IMyEventOwner, double, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in double delay, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterParentChangeTimeOut(delay);
			}
		}

		protected sealed class OnCharacterMaxJetpackGridDistance_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float distance, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMaxJetpackGridDistance(distance);
			}
		}

		protected sealed class OnCharacterMaxJetpackGridDisconnectDistance_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float distance, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMaxJetpackGridDisconnectDistance(distance);
			}
		}

		protected sealed class OnCharacterMinJetpackGridSpeed_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float speed, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMinJetpackGridSpeed(speed);
			}
		}

		protected sealed class OnCharacterMinJetpackDisconnectGridSpeed_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float speed, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMinJetpackDisconnectGridSpeed(speed);
			}
		}

		protected sealed class OnCharacterMaxJetpackGridAcceleration_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float accel, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMaxJetpackGridAcceleration(accel);
			}
		}

		protected sealed class OnCharacterMaxJetpackDisconnectGridAcceleration_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float accel, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMaxJetpackDisconnectGridAcceleration(accel);
			}
		}

		protected sealed class OnCharacterMinJetpackInsideGridSpeed_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float accel, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMinJetpackInsideGridSpeed(accel);
			}
		}

		protected sealed class OnCharacterMinJetpackDisconnectInsideGridSpeed_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float accel, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCharacterMinJetpackDisconnectInsideGridSpeed(accel);
			}
		}

		protected sealed class OnElapsedGameTime_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long elapsedGameTicks, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnElapsedGameTime(elapsedGameTicks);
			}
		}

		protected sealed class OnAllMembersReceived_003C_003ESandbox_Engine_Multiplayer_AllMembersDataMsg : ICallSite<IMyEventOwner, AllMembersDataMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AllMembersDataMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnAllMembersReceived(msg);
			}
		}

		protected sealed class OnChatMessageReceived_Server_003C_003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ChatMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChatMessageReceived_Server(msg);
			}
		}

		protected sealed class OnChatMessageReceived_SingleTarget_003C_003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ChatMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChatMessageReceived_SingleTarget(msg);
			}
		}

		protected sealed class OnChatMessageReceived_BroadcastExcept_003C_003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ChatMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChatMessageReceived_BroadcastExcept(msg);
			}
		}

		protected sealed class OnChatMessageReceived_ToPlayer_003C_003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ChatMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChatMessageReceived_ToPlayer(msg);
			}
		}

		protected sealed class OnScriptedChatMessageReceived_003C_003ESandbox_Engine_Multiplayer_ScriptedChatMsg : ICallSite<IMyEventOwner, ScriptedChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ScriptedChatMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnScriptedChatMessageReceived(msg);
			}
		}

		protected sealed class InvalidateVoxelCache_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string storageName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				InvalidateVoxelCache(storageName);
			}
		}

		private static readonly int NUMBER_OF_WRONG_PASSWORD_TRIES_BEFORE_KICK = 3;

		public readonly MySyncLayer SyncLayer;

		private IMyPeer2Peer m_networkService;

		private ulong m_serverId;

		protected LRUCache<string, byte[]> m_voxelMapData;

		private readonly ConcurrentDictionary<ulong, int> m_kickedClients;

		private readonly Dictionary<ulong, int> m_wrongPasswordClients;

		private readonly MyConcurrentHashSet<ulong> m_bannedClients;

		private static readonly List<ulong> m_tmpClientList = new List<ulong>();

		private int m_lastKickUpdate;

		private readonly Dictionary<int, ITransportCallback> m_controlMessageHandlers = new Dictionary<int, ITransportCallback>();

		private readonly Dictionary<Type, MyControlMessageEnum> m_controlMessageTypes = new Dictionary<Type, MyControlMessageEnum>();

		private TimeSpan m_lastSentTimeTimestamp;

		private readonly BitStream m_sendPhysicsStream = new BitStream();

		public const int KICK_TIMEOUT_MS = 300000;

		private float m_serverSimulationRatio = 1f;

		public Action<string> ProfilerDone;

		public bool IsServerExperimental
		{
			get;
			protected set;
		}

		public MyReplicationLayer ReplicationLayer
		{
			get;
			private set;
		}

		public ConcurrentDictionary<ulong, int> KickedClients => m_kickedClients;

		public MyConcurrentHashSet<ulong> BannedClients => m_bannedClients;

		public ulong ServerId
		{
			get;
			protected set;
		}

		public float ServerSimulationRatio
		{
			get
			{
				return (float)Math.Round(m_serverSimulationRatio, 2);
			}
			set
			{
				m_serverSimulationRatio = value;
			}
		}

		public LRUCache<string, byte[]> VoxelMapData => m_voxelMapData;

		public uint FrameCounter
		{
			get;
			private set;
		}

		public abstract string WorldName
		{
			get;
			set;
		}

		public abstract MyGameModeEnum GameMode
		{
			get;
			set;
		}

		public abstract float InventoryMultiplier
		{
			get;
			set;
		}

		public abstract float BlocksInventoryMultiplier
		{
			get;
			set;
		}

		public abstract float AssemblerMultiplier
		{
			get;
			set;
		}

		public abstract float RefineryMultiplier
		{
			get;
			set;
		}

		public abstract float WelderMultiplier
		{
			get;
			set;
		}

		public abstract float GrinderMultiplier
		{
			get;
			set;
		}

		public abstract string HostName
		{
			get;
			set;
		}

		public abstract ulong WorldSize
		{
			get;
			set;
		}

		public abstract int AppVersion
		{
			get;
			set;
		}

		public abstract string DataHash
		{
			get;
			set;
		}

		public abstract int MaxPlayers
		{
			get;
		}

		public abstract int ModCount
		{
			get;
			protected set;
		}

		public abstract List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get;
			set;
		}

		public abstract int ViewDistance
		{
			get;
			set;
		}

		public abstract bool Scenario
		{
			get;
			set;
		}

		public abstract string ScenarioBriefing
		{
			get;
			set;
		}

		public abstract DateTime ScenarioStartTime
		{
			get;
			set;
		}

		public virtual int SyncDistance
		{
			get;
			set;
		}

		public abstract bool ExperimentalMode
		{
			get;
			set;
		}

		public bool IsConnectionDirect
		{
			get;
			protected set;
		}

		public bool IsConnectionAlive
		{
			get;
			protected set;
		}

		public DateTime LastMessageReceived => MyMultiplayer.ReplicationLayer.LastMessageFromServer;

		public abstract IEnumerable<ulong> Members
		{
			get;
		}

		public abstract int MemberCount
		{
			get;
		}

		public abstract ulong LobbyId
		{
			get;
		}

		public abstract int MemberLimit
		{
			get;
			set;
		}

		public virtual bool IsLobby => false;

		public virtual bool IsChatAvailable => true;

		public event Action<ulong> ClientJoined;

		public event Action<ulong, MyChatMemberStateChangeEnum> ClientLeft;

		public event Action HostLeft;

		public event Action<ulong, string, ChatChannel, long, string> ChatMessageReceived;

		public event Action<string, string, string, Color> ScriptedChatMessageReceived;

		public event Action<ulong> ClientKicked;

		public event Action PendingReplicablesDone;

		public event Action LocalRespawnRequested;

		[Event(null, 212)]
		[Server]
		[Reliable]
		public static void OnSetPriorityMultiplier(float priority)
		{
			MyMultiplayer.Static.ReplicationLayer.SetPriorityMultiplier(MyEventContext.Current.Sender, priority);
		}

		[Event(null, 217)]
		[Server]
		[Reliable]
		public static void OnSetDebugEntity(long entityId)
		{
			MyFakes.VDB_ENTITY = MyEntities.GetEntityById(entityId);
		}

		[Event(null, 222)]
		[Server]
		[Reliable]
		public static void OnCharacterParentChangeTimeOut(double delay)
		{
			MyCharacterPhysicsStateGroup.ParentChangeTimeOut = MyTimeSpan.FromMilliseconds(delay);
		}

		[Event(null, 227)]
		[Server]
		[Reliable]
		public static void OnCharacterMaxJetpackGridDistance(float distance)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = distance;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, distance);
		}

		[Event(null, 233)]
		[Server]
		[Reliable]
		public static void OnCharacterMaxJetpackGridDisconnectDistance(float distance)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = distance;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, distance);
		}

		[Event(null, 239)]
		[Server]
		[Reliable]
		public static void OnCharacterMinJetpackGridSpeed(float speed)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = speed;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, speed);
		}

		[Event(null, 245)]
		[Server]
		[Reliable]
		public static void OnCharacterMinJetpackDisconnectGridSpeed(float speed)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = speed;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, speed);
		}

		[Event(null, 251)]
		[Server]
		[Reliable]
		public static void OnCharacterMaxJetpackGridAcceleration(float accel)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = accel;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, accel);
		}

		[Event(null, 257)]
		[Server]
		[Reliable]
		public static void OnCharacterMaxJetpackDisconnectGridAcceleration(float accel)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = accel;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, accel);
		}

		[Event(null, 263)]
		[Server]
		[Reliable]
		public static void OnCharacterMinJetpackInsideGridSpeed(float accel)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = accel;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, accel);
		}

		[Event(null, 269)]
		[Server]
		[Reliable]
		public static void OnCharacterMinJetpackDisconnectInsideGridSpeed(float accel)
		{
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = accel;
			MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, accel);
		}

		internal MyMultiplayerBase(MySyncLayer syncLayer)
		{
			SyncLayer = syncLayer;
			IsConnectionDirect = true;
			IsConnectionAlive = true;
			m_kickedClients = new ConcurrentDictionary<ulong, int>();
			m_bannedClients = new MyConcurrentHashSet<ulong>();
			m_wrongPasswordClients = new Dictionary<ulong, int>();
			m_lastKickUpdate = MySandboxGame.TotalTimeInMilliseconds;
			MyNetworkMonitor.StartSession();
			MyNetworkReader.SetHandler(0, ControlMessageReceived, DisconnectClient);
			RegisterControlMessage<MyControlKickClientMsg>(MyControlMessageEnum.Kick, OnClientKick, MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
			RegisterControlMessage<MyControlDisconnectedMsg>(MyControlMessageEnum.Disconnected, OnDisconnectedClient, MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
			RegisterControlMessage<MyControlBanClientMsg>(MyControlMessageEnum.Ban, OnClientBan, MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
			RegisterControlMessage<MyControlSendPasswordHashMsg>(MyControlMessageEnum.SendPasswordHash, OnPasswordHash, MyMessagePermissions.ToServer);
			syncLayer.TransportLayer.DisconnectPeerOnError = DisconnectClient;
			ClientKicked += KickClient;
		}

		private void KickClient(ulong client)
		{
			KickClient(client, kicked: true, add: false);
		}

		protected virtual void OnPasswordHash(ref MyControlSendPasswordHashMsg message, ulong sender)
		{
		}

		protected void SetReplicationLayer(MyReplicationLayer layer)
		{
			if (ReplicationLayer != null)
			{
				throw new InvalidOperationException("Replication layer already set");
			}
			ReplicationLayer = layer;
			ReplicationLayer.RegisterFromGameAssemblies();
		}

		internal void RegisterControlMessage<T>(MyControlMessageEnum msg, ControlMessageHandler<T> handler, MyMessagePermissions permission) where T : struct
		{
			MyControlMessageCallback<T> value = new MyControlMessageCallback<T>(handler, MySyncLayer.GetSerializer<T>(), permission);
			m_controlMessageHandlers.Add((int)msg, value);
			m_controlMessageTypes.Add(typeof(T), msg);
		}

		private void ControlMessageReceived(MyPacket p)
		{
			MyControlMessageEnum key = (MyControlMessageEnum)p.ByteStream.ReadUShort();
			if (m_controlMessageHandlers.TryGetValue((int)key, out ITransportCallback value))
			{
				value.Receive(p.ByteStream, p.Sender.Id.Value);
			}
			p.Return();
		}

		protected void SendControlMessage<T>(ulong user, ref T message, bool reliable = true) where T : struct
		{
			m_controlMessageTypes.TryGetValue(typeof(T), out MyControlMessageEnum value);
			m_controlMessageHandlers.TryGetValue((int)value, out ITransportCallback value2);
			MyControlMessageCallback<T> myControlMessageCallback = (MyControlMessageCallback<T>)value2;
			if (MySyncLayer.CheckSendPermissions(user, myControlMessageCallback.Permission))
			{
				MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.GetPacketDescriptor(new EndpointId(user), reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, 0);
				packetDescriptor.Header.WriteUShort((ushort)value);
				myControlMessageCallback.Write(packetDescriptor.Header, ref message);
				MyNetworkWriter.SendPacket(packetDescriptor);
			}
		}

		internal void SendControlMessageToAll<T>(ref T message, ulong exceptUserId = 0uL) where T : struct
		{
			foreach (ulong member in Members)
			{
				if (member != Sync.MyId && member != exceptUserId)
				{
					SendControlMessage(member, ref message);
				}
			}
		}

		protected abstract void OnClientKick(ref MyControlKickClientMsg data, ulong sender);

		protected abstract void OnClientBan(ref MyControlBanClientMsg data, ulong sender);

		public virtual void OnChatMessage(ref ChatMsg msg)
		{
		}

		protected void OnScriptedChatMessage(ref ScriptedChatMsg msg)
		{
			RaiseScriptedChatMessageReceived(msg.Author, msg.Text, msg.Font, msg.Color);
		}

		private void OnDisconnectedClient(ref MyControlDisconnectedMsg data, ulong sender)
		{
			RaiseClientLeft(data.Client, MyChatMemberStateChangeEnum.Disconnected);
			MyLog.Default.WriteLineAndConsole("Disconnected: " + sender);
		}

		public virtual void DownloadWorld()
		{
		}

		public virtual void DownloadProfiler()
		{
		}

		public abstract void DisconnectClient(ulong userId);

		public abstract void KickClient(ulong userId, bool kicked = true, bool add = true);

		public abstract void BanClient(ulong userId, bool banned);

		protected void AddWrongPasswordClient(ulong userId)
		{
			if (m_wrongPasswordClients.TryGetValue(userId, out int value))
			{
				value++;
				m_wrongPasswordClients[userId] = value;
			}
			else
			{
				m_wrongPasswordClients[userId] = 1;
			}
		}

		protected bool IsOutOfWrongPasswordTries(ulong userId)
		{
			if (m_wrongPasswordClients.TryGetValue(userId, out int value))
			{
				return value >= NUMBER_OF_WRONG_PASSWORD_TRIES_BEFORE_KICK;
			}
			return false;
		}

		protected void ResetWrongPasswordCounter(ulong userId)
		{
			if (m_wrongPasswordClients.ContainsKey(userId))
			{
				m_wrongPasswordClients.Remove(userId);
			}
		}

		protected void AddKickedClient(ulong userId)
		{
			if (!m_kickedClients.TryAdd(userId, MySandboxGame.TotalTimeInMilliseconds))
			{
				MySandboxGame.Log.WriteLine("Trying to kick player who was already kicked!");
			}
		}

		protected void RemoveKickedClient(ulong userId)
		{
			m_kickedClients.Remove(userId);
		}

		protected void AddBannedClient(ulong userId)
		{
			if (m_bannedClients.Contains(userId))
			{
				MySandboxGame.Log.WriteLine("Trying to ban player who was already banned!");
			}
			else
			{
				m_bannedClients.Add(userId);
			}
		}

		protected void RemoveBannedClient(ulong userId)
		{
			m_bannedClients.Remove(userId);
		}

		protected bool IsClientKickedOrBanned(ulong userId)
		{
			if (!m_kickedClients.ContainsKey(userId))
			{
				return m_bannedClients.Contains(userId);
			}
			return true;
		}

		protected bool IsClientKicked(ulong userId)
		{
			return m_kickedClients.ContainsKey(userId);
		}

		protected bool IsClientBanned(ulong userId)
		{
			return m_bannedClients.Contains(userId);
		}

		public void StartProcessingClientMessages()
		{
			SyncLayer.TransportLayer.IsBuffering = false;
		}

		public virtual void StartProcessingClientMessagesWithEmptyWorld()
		{
			StartProcessingClientMessages();
		}

		public void ReportReplicatedObjects()
		{
			if (VRage.Profiler.MyRenderProfiler.ProfilerVisible)
			{
				ReplicationLayer.ReportReplicatedObjects();
			}
		}

		public virtual void Tick()
		{
			FrameCounter++;
			if (IsServer && (MySession.Static.ElapsedGameTime - m_lastSentTimeTimestamp).Seconds > 30)
			{
				m_lastSentTimeTimestamp = MySession.Static.ElapsedGameTime;
				SendElapsedGameTime();
			}
			int totalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
			if (totalTimeInMilliseconds - m_lastKickUpdate > 20000)
			{
				m_tmpClientList.Clear();
				foreach (ulong key in m_kickedClients.Keys)
				{
					m_tmpClientList.Add(key);
				}
				foreach (ulong tmpClient in m_tmpClientList)
				{
					if (totalTimeInMilliseconds - m_kickedClients[tmpClient] > 300000)
					{
						m_kickedClients.Remove(tmpClient);
						if (m_wrongPasswordClients.ContainsKey(tmpClient))
						{
							m_wrongPasswordClients.Remove(tmpClient);
						}
					}
				}
				m_tmpClientList.Clear();
				m_lastKickUpdate = totalTimeInMilliseconds;
			}
			ReplicationLayer.SendUpdate();
		}

		public abstract void SendChatMessage(string text, ChatChannel channel, long targetId = 0L, string customAuthor = null);

		public virtual void Dispose()
		{
			MyNetworkMonitor.EndSession();
			m_voxelMapData = null;
			MyNetworkReader.ClearHandler(0);
			SyncLayer.TransportLayer.Clear();
			MyNetworkReader.Clear();
			m_sendPhysicsStream.Dispose();
			ReplicationLayer.Dispose();
			ClientKicked -= KickClient;
			MyMultiplayer.Static = null;
		}

		public abstract string GetMemberName(ulong steamUserID);

		protected void RaiseChatMessageReceived(ulong steamUserID, string messageText, ChatChannel channel, long targetId, string customAuthorName = null)
		{
			if (MyGameService.GetPlayerVoiceChatState(steamUserID) != MyPlayerVoiceChatState.Muted && (IsChatAvailable || channel == ChatChannel.GlobalScripted || channel == ChatChannel.ChatBot))
			{
				this.ChatMessageReceived.InvokeIfNotNull(steamUserID, messageText, channel, targetId, customAuthorName);
				MyAPIUtilities.Static.RecieveMessage(steamUserID, messageText);
			}
		}

		private void RaiseScriptedChatMessageReceived(string author, string messageText, string font, Color color)
		{
			this.ScriptedChatMessageReceived?.Invoke(messageText, author, font, color);
		}

		protected void RaiseHostLeft()
		{
			this.HostLeft?.Invoke();
		}

		protected void RaiseClientLeft(ulong changedUser, MyChatMemberStateChangeEnum stateChange)
		{
			this.ClientLeft?.Invoke(changedUser, stateChange);
		}

		protected void RaiseClientJoined(ulong changedUser)
		{
			this.ClientJoined?.Invoke(changedUser);
		}

		protected void RaiseClientKicked(ulong user)
		{
			this.ClientKicked?.Invoke(user);
		}

		public abstract ulong GetOwner();

		public abstract MyLobbyType GetLobbyType();

		public abstract void SetLobbyType(MyLobbyType type);

		public abstract void SetMemberLimit(int limit);

		protected void CloseMemberSessions()
		{
			foreach (ulong member in Members)
			{
				if (member != Sync.MyId && member == ServerId)
				{
					MyGameService.Peer2Peer.CloseSession(member);
				}
			}
		}

		protected void SendAllMembersDataToClient(ulong clientId)
		{
			AllMembersDataMsg arg = default(AllMembersDataMsg);
			if (Sync.Players != null)
			{
				arg.Identities = Sync.Players.SaveIdentities();
				arg.Players = Sync.Players.SavePlayers();
			}
			if (MySession.Static.Factions != null)
			{
				arg.Factions = MySession.Static.Factions.SaveFactions();
			}
			arg.Clients = MySession.Static.SaveMembers(forceSave: true);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAllMembersReceived, arg, new EndpointId(clientId));
		}

		protected virtual void OnAllMembersData(ref AllMembersDataMsg msg)
		{
		}

		protected void ProcessAllMembersData(ref AllMembersDataMsg msg)
		{
			Sync.Players.ClearIdentities();
			if (msg.Identities != null)
			{
				Sync.Players.LoadIdentities(msg.Identities);
			}
			Sync.Players.ClearPlayers();
			if (msg.Players != null)
			{
				Sync.Players.LoadPlayers(msg.Players);
			}
			MySession.Static.Factions.LoadFactions(msg.Factions);
		}

		protected MyClientState CreateClientState()
		{
			return Activator.CreateInstance(MyPerGameSettings.ClientStateType) as MyClientState;
		}

		private static void SendElapsedGameTime()
		{
			long ticks = MySession.Static.ElapsedGameTime.Ticks;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnElapsedGameTime, ticks);
		}

		[Event(null, 763)]
		[Broadcast]
		private static void OnElapsedGameTime(long elapsedGameTicks)
		{
			MySession.Static.ElapsedGameTime = new TimeSpan(elapsedGameTicks);
		}

		protected static void SendChatMessage(ref ChatMsg msg)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_Server, msg);
		}

		public static void SendScriptedChatMessage(ref ScriptedChatMsg msg)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnScriptedChatMessageReceived, msg);
		}

		[Event(null, 779)]
		[Reliable]
		[Client]
		private static void OnAllMembersReceived(AllMembersDataMsg msg)
		{
			MyMultiplayer.Static.OnAllMembersData(ref msg);
		}

		[Event(null, 785)]
		[Reliable]
		[Server]
		private static void OnChatMessageReceived_Server(ChatMsg msg)
		{
			EndpointId sender = MyEventContext.Current.Sender;
			string text = sender.ToString();
			string text2 = msg.TargetId.ToString();
			switch (msg.Channel)
			{
			case 0:
				text = GetPlayerName(MyEventContext.Current.Sender.Value);
				text2 = "everyone";
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_BroadcastExcept, msg, MyEventContext.Current.Sender);
				break;
			case 2:
			{
				IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(msg.TargetId);
				if (myFaction != null)
				{
					text = GetPlayerName(MyEventContext.Current.Sender.Value);
					text2 = myFaction.Tag;
					foreach (KeyValuePair<long, MyFactionMember> member in myFaction.Members)
					{
						if (MySession.Static.Players.IsPlayerOnline(member.Value.PlayerId))
						{
							ulong num3 = MySession.Static.Players.TryGetSteamId(member.Value.PlayerId);
							if (num3 != 0L && num3 != MyEventContext.Current.Sender.Value)
							{
								MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_SingleTarget, msg, new EndpointId(num3));
							}
						}
					}
				}
				break;
			}
			case 3:
			{
				ulong num2 = MySession.Static.Players.TryGetSteamId(msg.TargetId);
				if (num2 != 0L && num2 != MyEventContext.Current.Sender.Value)
				{
					text = GetPlayerName(MyEventContext.Current.Sender.Value);
					text2 = GetPlayerName(msg.TargetId);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_SingleTarget, msg, new EndpointId(num2));
				}
				break;
			}
			case 4:
				text = GetPlayerName(MyEventContext.Current.Sender.Value);
				text2 = GetPlayerName(msg.TargetId);
				break;
			case 1:
			{
				if (msg.TargetId <= 0)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_BroadcastExcept, msg);
					break;
				}
				ulong num = MySession.Static.Players.TryGetSteamId(msg.TargetId);
				if (num != 0L)
				{
					text = GetPlayerName(MyEventContext.Current.Sender.Value);
					text2 = GetPlayerName(msg.TargetId);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChatMessageReceived_SingleTarget, msg, new EndpointId(num));
				}
				break;
			}
			default:
				throw new InvalidBranchException("Unknown channel " + (ChatChannel)msg.Channel);
			}
			MyMultiplayer.Static.OnChatMessage(ref msg);
			if (Sandbox.Engine.Platform.Game.IsDedicated && MySandboxGame.ConfigDedicated.SaveChatToLog)
			{
				MyLog @default = MyLog.Default;
				string[] obj = new string[9]
				{
					"CHAT - channel: [",
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				ChatChannel channel = (ChatChannel)msg.Channel;
				obj[1] = channel.ToString();
				obj[2] = "], from [";
				obj[3] = text;
				obj[4] = "] to [";
				obj[5] = text2;
				obj[6] = "], message: '";
				obj[7] = msg.Text;
				obj[8] = "'";
				@default.WriteLine(string.Concat(obj));
			}
		}

		private static string GetPlayerName(ulong steamId)
		{
			long num = MySession.Static.Players.TryGetIdentityId(steamId);
			if (num == 0L)
			{
				return steamId.ToString();
			}
			return GetPlayerName(num);
		}

		private static string GetPlayerName(long identityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return identityId.ToString();
			}
			return myIdentity.DisplayName;
		}

		[Event(null, 892)]
		[Reliable]
		[Client]
		private static void OnChatMessageReceived_SingleTarget(ChatMsg msg)
		{
			MyMultiplayer.Static.OnChatMessage(ref msg);
		}

		[Event(null, 898)]
		[Reliable]
		[BroadcastExcept]
		private static void OnChatMessageReceived_BroadcastExcept(ChatMsg msg)
		{
			MyMultiplayer.Static.OnChatMessage(ref msg);
		}

		[Event(null, 904)]
		[Reliable]
		[Client]
		protected static void OnChatMessageReceived_ToPlayer(ChatMsg msg)
		{
			MyMultiplayer.Static.OnChatMessage(ref msg);
		}

		[Event(null, 910)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnScriptedChatMessageReceived(ScriptedChatMsg msg)
		{
			if (MySession.Static != null && (msg.Target == 0L || MySession.Static.LocalPlayerId == msg.Target))
			{
				MyMultiplayer.Static.OnScriptedChatMessage(ref msg);
			}
		}

		[Event(null, 922)]
		[Reliable]
		[Server]
		public static void InvalidateVoxelCache(string storageName)
		{
			MyMultiplayer.GetReplicationServer().InvalidateSingleClientCache(storageName, MyEventContext.Current.Sender);
		}

		public abstract void OnSessionReady();

		public void ReceivePendingReplicablesDone()
		{
			this.PendingReplicablesDone.InvokeIfNotNull();
		}

		public void InvokeLocalRespawnRequested()
		{
			this.LocalRespawnRequested.InvokeIfNotNull();
		}
	}
}
