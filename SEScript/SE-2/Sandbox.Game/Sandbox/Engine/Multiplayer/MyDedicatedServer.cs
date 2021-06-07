using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	public class MyDedicatedServer : MyDedicatedServerBase
	{
		private float m_inventoryMultiplier;

		private float m_blocksInventoryMultiplier;

		private float m_assemblerMultiplier;

		private float m_refineryMultiplier;

		private float m_welderMultiplier;

		private float m_grinderMultiplier;

		private List<MyChatMessage> m_globalChatHistory = new List<MyChatMessage>();

		public List<MyChatMessage> GlobalChatHistory => m_globalChatHistory;

		public override float InventoryMultiplier
		{
			get
			{
				return m_inventoryMultiplier;
			}
			set
			{
				m_inventoryMultiplier = value;
			}
		}

		public override float BlocksInventoryMultiplier
		{
			get
			{
				return m_blocksInventoryMultiplier;
			}
			set
			{
				m_blocksInventoryMultiplier = value;
			}
		}

		public override float AssemblerMultiplier
		{
			get
			{
				return m_assemblerMultiplier;
			}
			set
			{
				m_assemblerMultiplier = value;
			}
		}

		public override float RefineryMultiplier
		{
			get
			{
				return m_refineryMultiplier;
			}
			set
			{
				m_refineryMultiplier = value;
			}
		}

		public override float WelderMultiplier
		{
			get
			{
				return m_welderMultiplier;
			}
			set
			{
				m_welderMultiplier = value;
			}
		}

		public override float GrinderMultiplier
		{
			get
			{
				return m_grinderMultiplier;
			}
			set
			{
				m_grinderMultiplier = value;
			}
		}

		public override bool Scenario
		{
			get;
			set;
		}

		public override string ScenarioBriefing
		{
			get;
			set;
		}

		public override DateTime ScenarioStartTime
		{
			get;
			set;
		}

		public override bool ExperimentalMode
		{
			get;
			set;
		}

		internal MyDedicatedServer(IPEndPoint serverEndpoint)
			: base(new MySyncLayer(new MyTransportLayer(2)))
		{
			Initialize(serverEndpoint);
		}

		internal override void SendGameTagsToSteam()
		{
			if (MyGameService.GameServer != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				switch (GameMode)
				{
				case MyGameModeEnum.Survival:
					stringBuilder.Append($"S{(int)InventoryMultiplier}-{(int)BlocksInventoryMultiplier}-{(int)AssemblerMultiplier}-{(int)RefineryMultiplier}");
					break;
				case MyGameModeEnum.Creative:
					stringBuilder.Append("C");
					break;
				}
				string gameTags = string.Concat("groupId", m_groupId, " version", MyFinalBuildConstants.APP_VERSION, " datahash", MyDataIntegrityChecker.GetHashBase64(), " mods", ModCount, " gamemode", stringBuilder, " view", SyncDistance);
				MyGameService.GameServer.SetGameTags(gameTags);
				MyGameService.GameServer.SetGameData(MyFinalBuildConstants.APP_VERSION.ToString());
			}
		}

		protected override void SendServerData()
		{
			ServerDataMsg serverDataMsg = default(ServerDataMsg);
			serverDataMsg.WorldName = m_worldName;
			serverDataMsg.GameMode = m_gameMode;
			serverDataMsg.InventoryMultiplier = m_inventoryMultiplier;
			serverDataMsg.BlocksInventoryMultiplier = m_blocksInventoryMultiplier;
			serverDataMsg.AssemblerMultiplier = m_assemblerMultiplier;
			serverDataMsg.RefineryMultiplier = m_refineryMultiplier;
			serverDataMsg.WelderMultiplier = m_welderMultiplier;
			serverDataMsg.GrinderMultiplier = m_grinderMultiplier;
			serverDataMsg.HostName = m_hostName;
			serverDataMsg.WorldSize = m_worldSize;
			serverDataMsg.AppVersion = m_appVersion;
			serverDataMsg.MembersLimit = m_membersLimit;
			serverDataMsg.DataHash = m_dataHash;
			serverDataMsg.ServerPasswordSalt = MySandboxGame.ConfigDedicated.ServerPasswordSalt;
			ServerDataMsg msg = serverDataMsg;
			base.ReplicationLayer.SendWorldData(ref msg);
		}

		public override void OnChatMessage(ref ChatMsg msg)
		{
			bool flag = false;
			if (MemberDataGet(msg.Author, out MyConnectedClientData data) && (data.IsAdmin || flag))
			{
				MyServerDebugCommands.Process(msg.Text, msg.Author);
			}
			string text = Sync.Players.TryGetIdentityNameFromSteamId(msg.Author);
			if (string.IsNullOrEmpty(text) && msg.Author == Sync.MyId)
			{
				text = MyTexts.GetString(MySpaceTexts.ChatBotName);
			}
			if (!string.IsNullOrEmpty(text))
			{
				m_globalChatHistory.Add(new MyChatMessage
				{
					SteamId = msg.Author,
					AuthorName = (string.IsNullOrEmpty(msg.CustomAuthorName) ? text : msg.CustomAuthorName),
					Text = msg.Text,
					Timestamp = DateTime.UtcNow
				});
			}
			RaiseChatMessageReceived(msg.Author, msg.Text, (ChatChannel)msg.Channel, msg.TargetId, string.IsNullOrEmpty(msg.CustomAuthorName) ? null : msg.CustomAuthorName);
		}
	}
}
