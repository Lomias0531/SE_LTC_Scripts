using Sandbox.Engine.Networking;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Multiplayer
{
	public abstract class MyDedicatedServerBase : MyMultiplayerServerBase
	{
		protected string m_worldName;

		protected MyGameModeEnum m_gameMode;

		protected string m_hostName;

		protected ulong m_worldSize;

		protected int m_appVersion = MyFinalBuildConstants.APP_VERSION;

		protected int m_membersLimit;

		protected string m_dataHash;

		protected ulong m_groupId;

		private readonly List<ulong> m_members = new List<ulong>();

		private readonly Dictionary<ulong, MyConnectedClientData> m_memberData = new Dictionary<ulong, MyConnectedClientData>();

		private bool m_gameServerDataDirty;

		private readonly Dictionary<ulong, MyConnectedClientData> m_pendingMembers = new Dictionary<ulong, MyConnectedClientData>();

		private readonly HashSet<ulong> m_waitingForGroup = new HashSet<ulong>();

		private int m_modCount;

		private List<MyObjectBuilder_Checkpoint.ModItem> m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();

		private const string STEAM_ID_PREFIX = "STEAM_";

		private const ulong STEAM_ID_MAGIC_CONSTANT = 76561197960265728uL;

		protected override bool IsServerInternal => true;

		public bool ServerStarted
		{
			get;
			private set;
		}

		public bool HasSteamServerResponsed
		{
			get;
			private set;
		}

		public string ServerInitError
		{
			get;
			private set;
		}

		public override string WorldName
		{
			get
			{
				return m_worldName;
			}
			set
			{
				m_worldName = (string.IsNullOrEmpty(value) ? "noname" : value);
				m_gameServerDataDirty = true;
			}
		}

		public override MyGameModeEnum GameMode
		{
			get
			{
				return m_gameMode;
			}
			set
			{
				m_gameMode = value;
			}
		}

		public override string HostName
		{
			get
			{
				return m_hostName;
			}
			set
			{
				m_hostName = value;
			}
		}

		public override ulong WorldSize
		{
			get
			{
				return m_worldSize;
			}
			set
			{
				m_worldSize = value;
			}
		}

		public override int AppVersion
		{
			get
			{
				return m_appVersion;
			}
			set
			{
				m_appVersion = value;
			}
		}

		public override string DataHash
		{
			get
			{
				return m_dataHash;
			}
			set
			{
				m_dataHash = value;
			}
		}

		public override int MaxPlayers => 1024;

		public override int ModCount
		{
			get
			{
				return m_modCount;
			}
			protected set
			{
				m_modCount = value;
				MyGameService.GameServer.SetKeyValue("mods", m_modCount.ToString());
			}
		}

		public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get
			{
				return m_mods;
			}
			set
			{
				m_mods = value;
				ModCount = m_mods.Count;
			}
		}

		public override int ViewDistance
		{
			get;
			set;
		}

		public override int SyncDistance
		{
			get
			{
				return MyLayers.GetSyncDistance();
			}
			set
			{
				MyLayers.SetSyncDistance(value);
			}
		}

		public bool IsPasswordProtected
		{
			get;
			set;
		}

		public override IEnumerable<ulong> Members => m_members;

		public override int MemberCount => m_members.Count;

		public override ulong LobbyId => 0uL;

		public override int MemberLimit
		{
			get
			{
				return m_membersLimit;
			}
			set
			{
				SetMemberLimit(value);
			}
		}

		private static string ConvertSteamIDFrom64(ulong from)
		{
			from -= 76561197960265728L;
			return new StringBuilder("STEAM_").Append("0:").Append(from % 2uL).Append(':')
				.Append(from / 2uL)
				.ToString();
		}

		private static ulong ConvertSteamIDTo64(string from)
		{
			string[] array = from.Replace("STEAM_", "").Split(new char[1]
			{
				':'
			});
			if (array.Length != 3)
			{
				return 0uL;
			}
			return 76561197960265728L + Convert.ToUInt64(array[1]) + Convert.ToUInt64(array[2]) * 2;
		}

		protected MyDedicatedServerBase(MySyncLayer syncLayer)
			: base(syncLayer, new EndpointId(Sync.MyId))
		{
			syncLayer.TransportLayer.Register(MyMessageId.CLIENT_CONNNECTED, byte.MaxValue, ClientConnected);
		}

		protected void Initialize(IPEndPoint serverEndpoint)
		{
			m_groupId = MySandboxGame.ConfigDedicated.GroupID;
			ServerStarted = false;
			HostName = "Dedicated server";
			SetMemberLimit(MaxPlayers);
			MyGameService.Peer2Peer.SessionRequest += Peer2Peer_SessionRequest;
			MyGameService.Peer2Peer.ConnectionFailed += Peer2Peer_ConnectionFailed;
			base.ClientLeft += MyDedicatedServer_ClientLeft;
			MyGameService.GameServer.PlatformConnected += GameServer_ServersConnected;
			MyGameService.GameServer.PlatformConnectionFailed += GameServer_ServersConnectFailure;
			MyGameService.GameServer.PlatformDisconnected += GameServer_ServersDisconnected;
			MyGameService.GameServer.PolicyResponse += GameServer_PolicyResponse;
			MyGameService.GameServer.ValidateAuthTicketResponse += GameServer_ValidateAuthTicketResponse;
			MyGameService.GameServer.UserGroupStatusResponse += GameServer_UserGroupStatus;
			string text = MySandboxGame.ConfigDedicated.ServerName;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "Unnamed server";
			}
			MyGameService.GameServer.SetServerName(text);
			bool num = MyGameService.GameServer.Start(serverEndpoint, (ushort)MySandboxGame.ConfigDedicated.SteamPort, MyFinalBuildConstants.APP_VERSION.ToString());
			MyGameService.Peer2Peer.SetServer(server: true);
			if (!num)
			{
				return;
			}
			MyGameService.GameServer.SetModDir(MyPerGameSettings.SteamGameServerGameDir);
			MyGameService.GameServer.ProductName = MyPerGameSettings.SteamGameServerProductName;
			MyGameService.GameServer.GameDescription = MyPerGameSettings.SteamGameServerDescription;
			MyGameService.GameServer.SetDedicated(isDedicated: true);
			if (!string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordHash) && !string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordSalt))
			{
				MyGameService.GameServer.SetPasswordProtected(passwdProtected: true);
				IsPasswordProtected = true;
			}
			MyGameService.GameServer.LogOnAnonymous();
			MyGameService.GameServer.EnableHeartbeats(enable: true);
			if (m_groupId != 0L && MyGameService.GetServerAccountType(m_groupId) != MyGameServiceAccountType.Clan)
			{
				MyLog.Default.WriteLineAndConsole("Specified group ID is invalid: " + m_groupId);
			}
			uint num2 = 0u;
			ulong userId = 0uL;
			int num3 = 100;
			while (num2 == 0 && num3 > 0)
			{
				MyGameService.GameServer.Update();
				Thread.Sleep(100);
				num3--;
				num2 = MyGameService.GameServer.GetPublicIP();
				userId = MyGameService.GameServer.ServerId;
			}
			MyGameService.UserId = userId;
			if (num2 == 0)
			{
				MyLog.Default.WriteLineAndConsole("Error: No IP assigned.");
				return;
			}
			IPAddress iPAddress = IPAddressExtensions.FromIPv4NetworkOrder(num2);
			base.ServerId = MyGameService.GameServer.ServerId;
			base.ReplicationLayer.SetLocalEndpoint(new EndpointId(MyGameService.GameServer.ServerId));
			m_members.Add(base.ServerId);
			MemberDataAdd(base.ServerId, new MyConnectedClientData
			{
				Name = MyTexts.GetString(MySpaceTexts.ChatBotName),
				IsAdmin = true
			});
			SyncLayer.RegisterClientEvents(this);
			MyLog.Default.WriteLineAndConsole("Server successfully started");
			MyLog.Default.WriteLineAndConsole("Product name: " + MyGameService.GameServer.ProductName);
			MyLog.Default.WriteLineAndConsole("Desc: " + MyGameService.GameServer.GameDescription);
			MyLog.Default.WriteLineAndConsole("Public IP: " + iPAddress.ToString());
			MyLog.Default.WriteLineAndConsole("Steam ID: " + userId);
			num3 = 1000;
			while (!HasSteamServerResponsed && num3 > 0)
			{
				MyGameService.Update();
				MyGameService.ServerUpdate();
				Thread.Sleep(100);
				num3--;
			}
			ServerStarted = true;
		}

		private void MemberDataAdd(ulong steamId, MyConnectedClientData data)
		{
			m_memberData.Add(steamId, data);
			m_gameServerDataDirty = true;
		}

		private void MemberDataRemove(ulong steamId)
		{
			m_memberData.Remove(steamId);
			m_gameServerDataDirty = true;
		}

		protected bool MemberDataGet(ulong steamId, out MyConnectedClientData data)
		{
			return m_memberData.TryGetValue(steamId, out data);
		}

		protected void MemberDataSet(ulong steamId, MyConnectedClientData data)
		{
			m_memberData[steamId] = data;
			m_gameServerDataDirty = true;
		}

		internal abstract void SendGameTagsToSteam();

		protected abstract void SendServerData();

		private void Peer2Peer_SessionRequest(ulong remoteUserId)
		{
			MyLog.Default.WriteLineAndConsole("Peer2Peer_SessionRequest " + remoteUserId);
			MyGameService.Peer2Peer.AcceptSession(remoteUserId);
		}

		private void Peer2Peer_ConnectionFailed(ulong remoteUserId, string error)
		{
			MyLog.Default.WriteLineAndConsole("Peer2Peer_ConnectionFailed " + remoteUserId + ", " + error);
			MySandboxGame.Static.Invoke(delegate
			{
				RaiseClientLeft(remoteUserId, MyChatMemberStateChangeEnum.Disconnected);
			}, "RaiseClientLeft");
		}

		private void MyDedicatedServer_ClientLeft(ulong user, MyChatMemberStateChangeEnum arg2)
		{
			MyGameService.Peer2Peer.CloseSession(user);
			MyLog.Default.WriteLineAndConsole("User left " + GetMemberName(user));
			if (m_members.Contains(user))
			{
				m_members.Remove(user);
			}
			if (m_pendingMembers.ContainsKey(user))
			{
				m_pendingMembers.Remove(user);
			}
			if (m_waitingForGroup.Contains(user))
			{
				m_waitingForGroup.Remove(user);
			}
			if (arg2 != MyChatMemberStateChangeEnum.Kicked && arg2 != MyChatMemberStateChangeEnum.Banned)
			{
				foreach (ulong member in m_members)
				{
					if (member != base.ServerId)
					{
						MyControlDisconnectedMsg myControlDisconnectedMsg = default(MyControlDisconnectedMsg);
						myControlDisconnectedMsg.Client = user;
						MyControlDisconnectedMsg message = myControlDisconnectedMsg;
						SendControlMessage(member, ref message);
					}
				}
			}
			MyGameService.GameServer.SendUserDisconnect(user);
			MemberDataRemove(user);
		}

		private void GameServer_ValidateAuthTicketResponse(ulong steamID, JoinResult response, ulong steamOwner)
		{
			MyLog.Default.WriteLineAndConsole(string.Concat("Server ValidateAuthTicketResponse (", response, "), owner: ", steamOwner));
			if (IsClientBanned(steamOwner) || MySandboxGame.ConfigDedicated.Banned.Contains(steamOwner))
			{
				UserRejected(steamID, JoinResult.BannedByAdmins);
				RaiseClientKicked(steamID);
			}
			else if (IsClientKicked(steamOwner))
			{
				UserRejected(steamID, JoinResult.KickedRecently);
				RaiseClientKicked(steamID);
			}
			else if (response == JoinResult.OK)
			{
				if (MySandboxGame.ConfigDedicated.Administrators.Contains(steamID.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(ConvertSteamIDFrom64(steamID)) || MySandboxGame.ConfigDedicated.Reserved.Contains(steamID))
				{
					UserAccepted(steamID);
				}
				else if (MemberLimit > 0 && m_members.Count - 1 >= MemberLimit)
				{
					UserRejected(steamID, JoinResult.ServerFull);
				}
				else if (ClientIsProfiling(steamID))
				{
					UserRejected(steamID, JoinResult.ProfilingNotAllowed);
				}
				else if (m_groupId == 0L)
				{
					UserAccepted(steamID);
				}
				else if (MyGameService.GetServerAccountType(m_groupId) != MyGameServiceAccountType.Clan)
				{
					UserRejected(steamID, JoinResult.GroupIdInvalid);
				}
				else if (MyGameService.GameServer.RequestGroupStatus(steamID, m_groupId))
				{
					m_waitingForGroup.Add(steamID);
				}
				else
				{
					UserRejected(steamID, JoinResult.SteamServersOffline);
				}
			}
			else
			{
				UserRejected(steamID, response);
			}
		}

		private void GameServer_UserGroupStatus(ulong userId, ulong groupId, bool member, bool officier)
		{
			if (groupId == m_groupId && m_waitingForGroup.Remove(userId))
			{
				if (member || officier)
				{
					UserAccepted(userId);
				}
				else
				{
					UserRejected(userId, JoinResult.NotInGroup);
				}
			}
		}

		private void GameServer_PolicyResponse(sbyte result)
		{
			MyLog.Default.WriteLineAndConsole("Server PolicyResponse (" + result + ")");
		}

		private void GameServer_ServersDisconnected(string result)
		{
			MyLog.Default.WriteLineAndConsole("Server disconnected (" + result + ")");
		}

		private void GameServer_ServersConnectFailure(string result)
		{
			MyLog.Default.WriteLineAndConsole("Server connect failure (" + result + ")");
			HasSteamServerResponsed = true;
		}

		private void GameServer_ServersConnected()
		{
			MyLog.Default.WriteLineAndConsole("Server connected to Steam");
			HasSteamServerResponsed = true;
		}

		private void UserRejected(ulong steamID, JoinResult reason)
		{
			m_pendingMembers.Remove(steamID);
			m_waitingForGroup.Remove(steamID);
			if (m_members.Contains(steamID))
			{
				RaiseClientLeft(steamID, MyChatMemberStateChangeEnum.Disconnected);
			}
			else
			{
				SendJoinResult(steamID, reason, 0uL);
			}
		}

		private void UserAccepted(ulong steamID)
		{
			m_members.Add(steamID);
			if (m_pendingMembers.TryGetValue(steamID, out MyConnectedClientData value))
			{
				m_pendingMembers.Remove(steamID);
				MemberDataSet(steamID, value);
				foreach (ulong member in m_members)
				{
					if (member != base.ServerId)
					{
						SendClientData(member, steamID, value.Name, join: true);
					}
				}
			}
			SendServerData();
			if (IsPasswordProtected)
			{
				SendJoinResult(steamID, JoinResult.PasswordRequired, 0uL);
			}
			else
			{
				SendJoinResult(steamID, JoinResult.OK, 0uL);
			}
		}

		private bool ClientIsProfiling(ulong steamID)
		{
			if (m_pendingMembers.TryGetValue(steamID, out MyConnectedClientData value))
			{
				return value.IsProfiling;
			}
			return false;
		}

		protected override void OnPasswordHash(ref MyControlSendPasswordHashMsg message, ulong sender)
		{
			base.OnPasswordHash(ref message, sender);
			if (!IsPasswordProtected || string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordHash))
			{
				SendJoinResult(sender, JoinResult.OK, 0uL);
				return;
			}
			byte[] passwordHash = message.PasswordHash;
			byte[] array = Convert.FromBase64String(MySandboxGame.ConfigDedicated.ServerPasswordHash);
			if (passwordHash == null || passwordHash.Length != array.Length)
			{
				RejectUserWithWrongPassword(sender);
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != passwordHash[i])
				{
					RejectUserWithWrongPassword(sender);
					return;
				}
			}
			ResetWrongPasswordCounter(sender);
			SendJoinResult(sender, JoinResult.OK, 0uL);
		}

		private void RejectUserWithWrongPassword(ulong sender)
		{
			AddWrongPasswordClient(sender);
			if (IsOutOfWrongPasswordTries(sender))
			{
				KickClient(sender);
			}
			else
			{
				SendJoinResult(sender, JoinResult.WrongPassword, 0uL);
			}
		}

		public virtual bool IsCorrectVersion()
		{
			return m_appVersion == (int)MyFinalBuildConstants.APP_VERSION;
		}

		public override void DownloadWorld()
		{
		}

		public override void DisconnectClient(ulong userId)
		{
			MyControlDisconnectedMsg myControlDisconnectedMsg = default(MyControlDisconnectedMsg);
			myControlDisconnectedMsg.Client = base.ServerId;
			MyControlDisconnectedMsg message = myControlDisconnectedMsg;
			SendControlMessage(userId, ref message);
			RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Disconnected);
		}

		public override void BanClient(ulong userId, bool banned)
		{
			MyControlBanClientMsg myControlBanClientMsg;
			if (banned)
			{
				MyLog.Default.WriteLineAndConsole("Player " + GetMemberName(userId) + " banned");
				myControlBanClientMsg = default(MyControlBanClientMsg);
				myControlBanClientMsg.BannedClient = userId;
				myControlBanClientMsg.Banned = true;
				MyControlBanClientMsg message = myControlBanClientMsg;
				SendControlMessageToAll(ref message, 0uL);
				AddBannedClient(userId);
				if (m_members.Contains(userId))
				{
					RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Banned);
				}
				MySandboxGame.ConfigDedicated.Banned.Add(userId);
			}
			else
			{
				MyLog.Default.WriteLineAndConsole("Player " + userId + " unbanned");
				myControlBanClientMsg = default(MyControlBanClientMsg);
				myControlBanClientMsg.BannedClient = userId;
				myControlBanClientMsg.Banned = false;
				MyControlBanClientMsg message2 = myControlBanClientMsg;
				SendControlMessageToAll(ref message2, 0uL);
				RemoveBannedClient(userId);
				MySandboxGame.ConfigDedicated.Banned.Remove(userId);
			}
			MySandboxGame.ConfigDedicated.Save();
		}

		public override void Tick()
		{
			base.Tick();
			UpdateSteamServerData();
		}

		private void UpdateSteamServerData()
		{
			if (m_gameServerDataDirty)
			{
				MyGameService.GameServer.SetMapName(m_worldName);
				MyGameService.GameServer.SetMaxPlayerCount(m_membersLimit);
				foreach (KeyValuePair<ulong, MyConnectedClientData> memberDatum in m_memberData)
				{
					MyGameService.GameServer.BrowserUpdateUserData(memberDatum.Key, memberDatum.Value.Name, 0);
				}
				m_gameServerDataDirty = false;
			}
		}

		public override void SendChatMessage(string text, ChatChannel channel, long targetId, string customAuthor)
		{
			ChatMsg chatMsg = default(ChatMsg);
			chatMsg.Text = text;
			chatMsg.Author = Sync.MyId;
			chatMsg.Channel = (byte)channel;
			chatMsg.TargetId = targetId;
			chatMsg.CustomAuthorName = (customAuthor ?? string.Empty);
			ChatMsg msg = chatMsg;
			MyMultiplayerBase.SendChatMessage(ref msg);
		}

		public void SendChatMessageToPlayer(string text, ulong steamId)
		{
			if (MyMultiplayer.Static.IsServer)
			{
				ChatMsg chatMsg = default(ChatMsg);
				chatMsg.Text = text;
				chatMsg.Author = Sync.MyId;
				chatMsg.Channel = 3;
				chatMsg.CustomAuthorName = string.Empty;
				ChatMsg arg = chatMsg;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerBase.OnChatMessageReceived_ToPlayer, arg, new EndpointId(steamId));
			}
		}

		private void SendJoinResult(ulong sendTo, JoinResult joinResult, ulong adminID = 0uL)
		{
			JoinResultMsg joinResultMsg = default(JoinResultMsg);
			joinResultMsg.JoinResult = joinResult;
			joinResultMsg.ServerExperimental = MySession.Static.IsSettingsExperimental();
			joinResultMsg.Admin = adminID;
			JoinResultMsg msg = joinResultMsg;
			base.ReplicationLayer.SendJoinResult(ref msg, sendTo);
		}

		public override void Dispose()
		{
			foreach (ulong member in m_members)
			{
				MyControlDisconnectedMsg myControlDisconnectedMsg = default(MyControlDisconnectedMsg);
				myControlDisconnectedMsg.Client = base.ServerId;
				MyControlDisconnectedMsg message = myControlDisconnectedMsg;
				if (member != base.ServerId)
				{
					SendControlMessage(member, ref message);
				}
			}
			Thread.Sleep(200);
			try
			{
				CloseMemberSessions();
				MyGameService.GameServer.EnableHeartbeats(enable: false);
				base.Dispose();
				MyLog.Default.WriteLineAndConsole("Logging off Steam...");
				MyGameService.GameServer.LogOff();
				MyLog.Default.WriteLineAndConsole("Shutting down server...");
				MyGameService.GameServer.Shutdown();
				MyLog.Default.WriteLineAndConsole("Done");
				MyGameService.Peer2Peer.SessionRequest -= Peer2Peer_SessionRequest;
				MyGameService.Peer2Peer.ConnectionFailed -= Peer2Peer_ConnectionFailed;
				base.ClientLeft -= MyDedicatedServer_ClientLeft;
			}
			catch (Exception arg)
			{
				MyLog.Default.WriteLineAndConsole("catch exception : " + arg);
			}
		}

		public override ulong GetOwner()
		{
			return base.ServerId;
		}

		public override MyLobbyType GetLobbyType()
		{
			return MyLobbyType.Public;
		}

		public override void SetLobbyType(MyLobbyType myLobbyType)
		{
		}

		public override void SetMemberLimit(int limit)
		{
			int membersLimit = m_membersLimit;
			m_membersLimit = (MyDedicatedServerOverrides.MaxPlayers.HasValue ? MyDedicatedServerOverrides.MaxPlayers.Value : Math.Max(limit, 2));
			m_gameServerDataDirty |= (membersLimit != m_membersLimit);
		}

		private void OnConnectedClient(ref ConnectedClientDataMsg msg, ulong steamId)
		{
			if (!MyGameService.GameServer.BeginAuthSession(steamId, msg.Token))
			{
				MyLog.Default.WriteLineAndConsole("Authentication failed.");
				return;
			}
			if (!msg.ExperimentalMode && ExperimentalMode)
			{
				MyLog.Default.WriteLineAndConsole("Server and client Experimental Mode does not match.");
				SendJoinResult(steamId, JoinResult.ExperimentalMode, 0uL);
				return;
			}
			RaiseClientJoined(steamId);
			MyLog.Default.WriteLineAndConsole("OnConnectedClient " + msg.Name + " attempt");
			if (m_members.Contains(steamId))
			{
				MyLog.Default.WriteLineAndConsole("Already joined");
				SendJoinResult(steamId, JoinResult.AlreadyJoined, 0uL);
			}
			else if (MySandboxGame.ConfigDedicated.Banned.Contains(steamId))
			{
				MyLog.Default.WriteLineAndConsole("User is banned by admins");
				ulong result = 0uL;
				foreach (KeyValuePair<ulong, MyConnectedClientData> memberDatum in m_memberData)
				{
					if (memberDatum.Value.IsAdmin && memberDatum.Key != base.ServerId)
					{
						result = memberDatum.Key;
						break;
					}
				}
				if (result == 0L && MySandboxGame.ConfigDedicated.Administrators.Count > 0)
				{
					ulong.TryParse(MySandboxGame.ConfigDedicated.Administrators[0], out result);
				}
				SendJoinResult(steamId, JoinResult.BannedByAdmins, result);
			}
			else
			{
				m_pendingMembers.Add(steamId, new MyConnectedClientData
				{
					Name = msg.Name,
					IsAdmin = (MySandboxGame.ConfigDedicated.Administrators.Contains(steamId.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(ConvertSteamIDFrom64(steamId))),
					IsProfiling = msg.IsProfiling
				});
			}
		}

		public override string GetMemberName(ulong steamUserID)
		{
			MemberDataGet(steamUserID, out MyConnectedClientData data);
			if (data.Name != null)
			{
				return data.Name;
			}
			return "ID:" + steamUserID;
		}

		private void SendClientData(ulong steamTo, ulong connectedSteamID, string connectedClientName, bool join)
		{
			ConnectedClientDataMsg connectedClientDataMsg = default(ConnectedClientDataMsg);
			connectedClientDataMsg.SteamID = connectedSteamID;
			connectedClientDataMsg.Name = connectedClientName;
			connectedClientDataMsg.IsAdmin = (MySandboxGame.ConfigDedicated.Administrators.Contains(connectedSteamID.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(ConvertSteamIDFrom64(connectedSteamID)));
			connectedClientDataMsg.Join = join;
			ConnectedClientDataMsg msg = connectedClientDataMsg;
			base.ReplicationLayer.SendClientConnected(ref msg, steamTo);
		}

		protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
		{
			if (MySession.Static.IsUserAdmin(sender))
			{
				BanClient(data.BannedClient, data.Banned);
			}
		}

		private void ClientConnected(MyPacket packet)
		{
			Sync.ClientConnected(packet.Sender.Id.Value);
			ConnectedClientDataMsg msg = base.ReplicationLayer.OnClientConnected(packet);
			OnConnectedClient(ref msg, msg.SteamID);
			packet.Return();
		}
	}
}
