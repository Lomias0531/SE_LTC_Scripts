using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting.Campaign;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof(MyObjectBuilder_CampaignSessionComponent), null)]
	public class MyCampaignSessionComponent : MySessionComponentBase
	{
		protected sealed class Reconnect_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Reconnect();
			}
		}

		protected sealed class CloseGame_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CloseGame();
			}
		}

		private MyCampaignStateMachine m_runningCampaignSM;

		private readonly Dictionary<ulong, MyObjectBuilder_Inventory> m_savedCharacterInventoriesPlayerIds = new Dictionary<ulong, MyObjectBuilder_Inventory>();

		private static ulong m_ownerId;

		private static ulong m_oldLobbyId;

		private static ulong m_elapsedMs;

		public string CampaignLevelOutcome
		{
			get;
			set;
		}

		public bool Running
		{
			get
			{
				if (m_runningCampaignSM != null)
				{
					return m_runningCampaignSM.CurrentNode != null;
				}
				return false;
			}
		}

		private void LoadCampaignStateMachine(string activeState = null)
		{
			m_runningCampaignSM = new MyCampaignStateMachine();
			m_runningCampaignSM.Deserialize(MyCampaignManager.Static.ActiveCampaign.StateMachine);
			if (activeState != null)
			{
				m_runningCampaignSM.SetState(activeState);
			}
			else
			{
				m_runningCampaignSM.ResetToStart();
			}
			m_runningCampaignSM.CurrentNode.OnUpdate(m_runningCampaignSM);
		}

		private void UpdateStateMachine()
		{
			string name = m_runningCampaignSM.CurrentNode.Name;
			m_runningCampaignSM.TriggerAction(MyStringId.GetOrCompute(CampaignLevelOutcome));
			m_runningCampaignSM.Update();
			MyCampaignStateMachineNode myCampaignStateMachineNode = m_runningCampaignSM.CurrentNode as MyCampaignStateMachineNode;
			if (name == myCampaignStateMachineNode.Name)
			{
				MySandboxGame.Log.WriteLine("ERROR: Campaign is stuck in one state! Check the campaign file.");
			}
			CampaignLevelOutcome = null;
		}

		private void LoadPlayersInventories()
		{
			if (m_savedCharacterInventoriesPlayerIds.TryGetValue(MySession.Static.LocalHumanPlayer.Id.SteamId, out MyObjectBuilder_Inventory value) && MySession.Static.LocalCharacter != null)
			{
				MyInventory inventory = MySession.Static.LocalCharacter.GetInventory();
				foreach (MyObjectBuilder_InventoryItem item in value.Items)
				{
					inventory.AddItems(item.Amount, item.PhysicalContent);
				}
			}
			if (MyMultiplayer.Static != null && MyMultiplayer.Static.IsServer)
			{
				MySession.Static.Players.PlayersChanged += delegate(bool added, MyPlayer.PlayerId id)
				{
					MyPlayer playerById = MySession.Static.Players.GetPlayerById(id);
					if (playerById.Character != null && m_savedCharacterInventoriesPlayerIds.TryGetValue(playerById.Id.SteamId, out MyObjectBuilder_Inventory value2))
					{
						MyInventory inventory2 = MySession.Static.LocalCharacter.GetInventory();
						foreach (MyObjectBuilder_InventoryItem item2 in value2.Items)
						{
							inventory2.AddItems(item2.Amount, item2.PhysicalContent);
						}
					}
				};
			}
		}

		private void SavePlayersInventories()
		{
			m_savedCharacterInventoriesPlayerIds.Clear();
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Character != null)
				{
					MyInventory inventory = onlinePlayer.Character.GetInventory();
					if (inventory != null)
					{
						MyObjectBuilder_Inventory objectBuilder = inventory.GetObjectBuilder();
						m_savedCharacterInventoriesPlayerIds[onlinePlayer.Id.SteamId] = objectBuilder;
					}
				}
			}
		}

		public void LoadNextCampaignMission()
		{
			if (MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer)
			{
				return;
			}
			SavePlayersInventories();
			string directoryName = Path.GetDirectoryName(MySession.Static.CurrentPath.Replace(MyFileSystem.SavesPath + "\\", ""));
			if (m_runningCampaignSM.Finished)
			{
				CallCloseOnClients();
				MySessionLoader.UnloadAndExitToMenu();
				MyCampaignManager.Static.NotifyCampaignFinished();
				if (MyCampaignManager.Static.ActiveCampaign.IsVanilla)
				{
					MyScreenManager.AddScreen(new MyGuiScreenGameCredits());
				}
			}
			else
			{
				UpdateStateMachine();
				string savePath = (m_runningCampaignSM.CurrentNode as MyCampaignStateMachineNode).SavePath;
				CallReconnectOnClients();
				MyCampaignManager.Static.LoadSessionFromActiveCampaign(savePath, delegate
				{
					MySession.Static.RegisterComponent(this, MyUpdateOrder.NoUpdate, 555);
					LoadPlayersInventories();
				}, directoryName, MyCampaignManager.Static.ActiveCampaignName);
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyVisualScriptManagerSessionComponent component = ((MySession)Session).GetComponent<MyVisualScriptManagerSessionComponent>();
			component.CampaignModPath = null;
			if (MyMultiplayer.Static != null && !MyMultiplayer.Static.IsServer)
			{
				return;
			}
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = sessionComponent as MyObjectBuilder_CampaignSessionComponent;
			if (myObjectBuilder_CampaignSessionComponent == null || string.IsNullOrEmpty(myObjectBuilder_CampaignSessionComponent.CampaignName))
			{
				if (MyCampaignManager.Static.IsNewCampaignLevelLoading)
				{
					component.CampaignModPath = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
				}
				return;
			}
			CampaignLevelOutcome = myObjectBuilder_CampaignSessionComponent.CurrentOutcome;
			if (!MyCampaignManager.Static.SwitchCampaign(myObjectBuilder_CampaignSessionComponent.CampaignName, myObjectBuilder_CampaignSessionComponent.IsVanilla, myObjectBuilder_CampaignSessionComponent.Mod.PublishedFileId, myObjectBuilder_CampaignSessionComponent.LocalModFolder))
			{
				MyLog.Default.WriteLine("MyCampaignManager - Unable to download or switch to campaign: " + myObjectBuilder_CampaignSessionComponent.CampaignName);
				throw new Exception("MyCampaignManager - Unable to download or switch to campaign: " + myObjectBuilder_CampaignSessionComponent.CampaignName);
			}
			LoadCampaignStateMachine(myObjectBuilder_CampaignSessionComponent.ActiveState);
			component.CampaignModPath = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = base.GetObjectBuilder() as MyObjectBuilder_CampaignSessionComponent;
			if (myObjectBuilder_CampaignSessionComponent != null && Running)
			{
				myObjectBuilder_CampaignSessionComponent.ActiveState = m_runningCampaignSM.CurrentNode.Name;
				myObjectBuilder_CampaignSessionComponent.CampaignName = MyCampaignManager.Static.ActiveCampaign.Name;
				myObjectBuilder_CampaignSessionComponent.CurrentOutcome = CampaignLevelOutcome;
				myObjectBuilder_CampaignSessionComponent.IsVanilla = MyCampaignManager.Static.ActiveCampaign.IsVanilla;
				myObjectBuilder_CampaignSessionComponent.LocalModFolder = MyCampaignManager.Static.ActiveCampaign.ModFolderPath;
			}
			return myObjectBuilder_CampaignSessionComponent;
		}

		private void CallReconnectOnClients()
		{
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Identity.IdentityId != MySession.Static.LocalPlayerId)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => Reconnect, new EndpointId(onlinePlayer.Id.SteamId));
				}
			}
		}

		private void CallCloseOnClients()
		{
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Identity.IdentityId != MySession.Static.LocalPlayerId)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CloseGame, new EndpointId(onlinePlayer.Id.SteamId));
				}
			}
		}

		[Event(null, 287)]
		[Reliable]
		[Client]
		private static void Reconnect()
		{
			m_ownerId = MyMultiplayer.Static.ServerId;
			m_elapsedMs = 0uL;
			m_oldLobbyId = (MyMultiplayer.Static as MyMultiplayerLobbyClient).LobbyId;
			MySessionLoader.UnloadAndExitToMenu();
			MyGuiSandbox.AddScreen(new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.LoadingDialogServerIsLoadingWorld), MyCommonTexts.Cancel));
			Parallel.Start(FindLobby);
		}

		[Event(null, 305)]
		[Reliable]
		[Client]
		private static void CloseGame()
		{
			MySessionLoader.UnloadAndExitToMenu();
		}

		private static void FindLobby()
		{
			Thread.Sleep(5000);
			MyGameService.RequestLobbyList(LobbiesRequestCompleted);
		}

		private static void LobbiesRequestCompleted(bool success)
		{
			if (success)
			{
				List<IMyLobby> list = new List<IMyLobby>();
				MyGameService.AddPublicLobbies(list);
				MyGameService.AddFriendLobbies(list);
				string b = MyFinalBuildConstants.APP_VERSION.FormattedText.ToString();
				foreach (IMyLobby item in list)
				{
					if (!(item.GetData("appVersion") != b) && MyMultiplayerLobby.GetLobbyHostSteamId(item) == m_ownerId && item.LobbyId != m_oldLobbyId)
					{
						MyScreenManager.RemoveScreenByType(typeof(MyGuiScreenProgress));
						MyJoinGameHelper.JoinGame(item);
						return;
					}
				}
				m_elapsedMs += 5000uL;
				if (m_elapsedMs > 120000)
				{
					MyScreenManager.RemoveScreenByType(typeof(MyGuiScreenProgress));
				}
				else
				{
					FindLobby();
				}
			}
		}
	}
}
