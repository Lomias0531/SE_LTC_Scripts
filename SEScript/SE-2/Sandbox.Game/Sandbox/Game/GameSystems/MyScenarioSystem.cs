using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 1000)]
	public class MyScenarioSystem : MySessionComponentBase
	{
		private struct CheckpointData
		{
			public MyObjectBuilder_Checkpoint Checkpoint;

			public string SessionPath;

			public ulong CheckpointSize;

			public bool PersistentEditMode;
		}

		public enum MyState
		{
			Loaded,
			JoinScreen,
			WaitingForClients,
			Running,
			Ending
		}

		public static int LoadTimeout = 120;

		public static MyScenarioSystem Static;

		private readonly HashSet<ulong> m_playersReadyForBattle = new HashSet<ulong>();

		private TimeSpan m_startBattlePreparationOnClients = TimeSpan.FromSeconds(0.0);

		private static string m_newPath;

		private static MyWorkshopItem m_newWorkshopMap;

		private static CheckpointData? m_checkpointData;

		private MyState m_gameState;

		private TimeSpan m_stateChangePlayTime;

		private TimeSpan m_startBattleTime = TimeSpan.FromSeconds(0.0);

		private StringBuilder m_tmpStringBuilder = new StringBuilder();

		private MyGuiScreenScenarioWaitForPlayers m_waitingScreen;

		private TimeSpan? m_battleTimeLimit;

		private int m_bootUpCount;

		public MyState GameState
		{
			get
			{
				return m_gameState;
			}
			set
			{
				if (m_gameState != value)
				{
					m_gameState = value;
					m_stateChangePlayTime = MySession.Static.ElapsedPlayTime;
				}
			}
		}

		public DateTime ServerPreparationStartTime
		{
			get;
			private set;
		}

		public DateTime ServerStartGameTime
		{
			get;
			private set;
		}

		private bool OnlinePrivateMode => MySession.Static.OnlineMode == MyOnlineModeEnum.PRIVATE;

		private event Action EndAction;

		public MyScenarioSystem()
		{
			Static = this;
			ServerStartGameTime = DateTime.MaxValue;
		}

		private void MySyncScenario_ClientWorldLoaded()
		{
			MySyncScenario.ClientWorldLoaded -= MySyncScenario_ClientWorldLoaded;
			m_waitingScreen = new MyGuiScreenScenarioWaitForPlayers();
			MyGuiSandbox.AddScreen(m_waitingScreen);
		}

		private void MySyncScenario_StartScenario(long serverStartGameTime)
		{
			ServerStartGameTime = new DateTime(serverStartGameTime);
			StartScenario();
		}

		private void MySyncScenario_PlayerReadyToStart(ulong steamId)
		{
			if (GameState == MyState.WaitingForClients)
			{
				m_playersReadyForBattle.Add(steamId);
				if (AllPlayersReadyForBattle())
				{
					StartScenario();
					foreach (ulong item in m_playersReadyForBattle)
					{
						if (item != Sync.MyId)
						{
							MySyncScenario.StartScenarioRequest(item, ServerStartGameTime.Ticks);
						}
					}
				}
			}
			else if (GameState == MyState.Running)
			{
				MySyncScenario.StartScenarioRequest(steamId, ServerStartGameTime.Ticks);
			}
		}

		private bool AllPlayersReadyForBattle()
		{
			foreach (MyPlayer.PlayerId allPlayer in Sync.Players.GetAllPlayers())
			{
				if (!m_playersReadyForBattle.Contains(allPlayer.SteamId))
				{
					return false;
				}
			}
			return true;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if ((!MySession.Static.IsScenario && !MySession.Static.Settings.ScenarioEditMode) || !Sync.IsServer)
			{
				return;
			}
			if (MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE && GameState < MyState.Running)
			{
				if (GameState == MyState.Loaded)
				{
					GameState = MyState.Running;
					ServerStartGameTime = DateTime.UtcNow;
				}
				return;
			}
			switch (GameState)
			{
			case MyState.JoinScreen:
			case MyState.Running:
				break;
			case MyState.Loaded:
				if (MySession.Static.OnlineMode != 0 && MyMultiplayer.Static == null)
				{
					m_bootUpCount++;
					if (m_bootUpCount > 100)
					{
						MyPlayerCollection.RequestLocalRespawn();
						GameState = MyState.Running;
					}
				}
				else if (Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.Settings.ScenarioEditMode)
				{
					ServerPreparationStartTime = DateTime.UtcNow;
					MyMultiplayer.Static.ScenarioStartTime = ServerPreparationStartTime;
					GameState = MyState.Running;
					if (!Sandbox.Engine.Platform.Game.IsDedicated)
					{
						StartScenario();
					}
				}
				else if (MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE || MyMultiplayer.Static != null)
				{
					if (MyMultiplayer.Static != null)
					{
						MyMultiplayer.Static.Scenario = true;
						MyMultiplayer.Static.ScenarioBriefing = MySession.Static.GetWorld().Checkpoint.Briefing;
					}
					MyGuiSandbox.AddScreen(new MyGuiScreenScenarioMpServer
					{
						Briefing = MySession.Static.GetWorld().Checkpoint.Briefing
					});
					m_playersReadyForBattle.Add(Sync.MyId);
					GameState = MyState.JoinScreen;
				}
				break;
			case MyState.WaitingForClients:
			{
				TimeSpan elapsedPlayTime = MySession.Static.ElapsedPlayTime;
				if (AllPlayersReadyForBattle() || (LoadTimeout > 0 && elapsedPlayTime - m_startBattlePreparationOnClients > TimeSpan.FromSeconds(LoadTimeout)))
				{
					StartScenario();
					foreach (ulong item in m_playersReadyForBattle)
					{
						if (item != Sync.MyId)
						{
							MySyncScenario.StartScenarioRequest(item, ServerStartGameTime.Ticks);
						}
					}
				}
				break;
			}
			case MyState.Ending:
				if (this.EndAction != null && MySession.Static.ElapsedPlayTime - m_stateChangePlayTime > TimeSpan.FromSeconds(10.0))
				{
					this.EndAction();
				}
				break;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			MySyncScenario.PlayerReadyToStartScenario += MySyncScenario_PlayerReadyToStart;
			MySyncScenario.StartScenario += MySyncScenario_StartScenario;
			MySyncScenario.ClientWorldLoaded += MySyncScenario_ClientWorldLoaded;
			MySyncScenario.PrepareScenario += MySyncBattleGame_PrepareScenario;
		}

		private void MySyncBattleGame_PrepareScenario(long preparationStartTime)
		{
			ServerPreparationStartTime = new DateTime(preparationStartTime);
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MySyncScenario.PlayerReadyToStartScenario -= MySyncScenario_PlayerReadyToStart;
			MySyncScenario.StartScenario -= MySyncScenario_StartScenario;
			MySyncScenario.ClientWorldLoaded -= MySyncScenario_ClientWorldLoaded;
			MySyncScenario.PrepareScenario -= MySyncBattleGame_PrepareScenario;
		}

		internal void PrepareForStart()
		{
			GameState = MyState.WaitingForClients;
			m_startBattlePreparationOnClients = MySession.Static.ElapsedPlayTime;
			if (GetOnlineModeFromCurrentLobbyType() != 0)
			{
				m_waitingScreen = new MyGuiScreenScenarioWaitForPlayers();
				MyGuiSandbox.AddScreen(m_waitingScreen);
				ServerPreparationStartTime = DateTime.UtcNow;
				MyMultiplayer.Static.ScenarioStartTime = ServerPreparationStartTime;
				MySyncScenario.PrepareScenarioFromLobby(ServerPreparationStartTime.Ticks);
			}
			else
			{
				StartScenario();
			}
		}

		private void StartScenario()
		{
			if (Sync.IsServer)
			{
				ServerStartGameTime = DateTime.UtcNow;
			}
			if (m_waitingScreen != null)
			{
				MyGuiSandbox.RemoveScreen(m_waitingScreen);
				m_waitingScreen = null;
			}
			GameState = MyState.Running;
			m_startBattleTime = MySession.Static.ElapsedPlayTime;
			if (MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Character == null)
			{
				MyPlayerCollection.RequestLocalRespawn();
			}
		}

		internal static MyOnlineModeEnum GetOnlineModeFromCurrentLobbyType()
		{
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby == null)
			{
				return MyOnlineModeEnum.PRIVATE;
			}
			switch (myMultiplayerLobby.GetLobbyType())
			{
			case MyLobbyType.Private:
				return MyOnlineModeEnum.PRIVATE;
			case MyLobbyType.FriendsOnly:
				return MyOnlineModeEnum.FRIENDS;
			case MyLobbyType.Public:
				return MyOnlineModeEnum.PUBLIC;
			default:
				return MyOnlineModeEnum.PRIVATE;
			}
		}

		internal static void SetLobbyTypeFromOnlineMode(MyOnlineModeEnum onlineMode)
		{
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby != null)
			{
				MyLobbyType lobbyType = MyLobbyType.Private;
				switch (onlineMode)
				{
				case MyOnlineModeEnum.FRIENDS:
					lobbyType = MyLobbyType.FriendsOnly;
					break;
				case MyOnlineModeEnum.PUBLIC:
					lobbyType = MyLobbyType.Public;
					break;
				}
				myMultiplayerLobby.SetLobbyType(lobbyType);
			}
		}

		public static void LoadNextScenario(string id)
		{
			if (MySession.Static.OnlineMode != 0)
			{
				return;
			}
			MyAPIGateway.Utilities.ShowNotification(MyTexts.GetString(MySpaceTexts.NotificationNextScenarioWillLoad), 10000);
			if (ulong.TryParse(id, out ulong result))
			{
				if (!MyGameService.IsOnline)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextWorkshopDownloadFailed), MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop)));
					return;
				}
				MySandboxGame.Log.WriteLine(string.Format("Querying details of file " + result));
				List<MyWorkshopItem> modsInfo = MyWorkshop.GetModsInfo(new List<ulong>
				{
					result
				});
				if (modsInfo != null && modsInfo.Count > 0)
				{
					m_newWorkshopMap = modsInfo[0];
					Static.EndAction += EndActionLoadWorkshop;
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextWorkshopDownloadFailed), MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop)));
				}
				return;
			}
			string text = Path.Combine(MyFileSystem.ContentPath, "Missions", id);
			if (Directory.Exists(text))
			{
				m_newPath = text;
				Static.EndAction += EndActionLoadLocal;
				return;
			}
			string text2 = Path.Combine(MyFileSystem.SavesPath, id);
			if (Directory.Exists(text2))
			{
				m_newPath = text2;
				Static.EndAction += EndActionLoadLocal;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextScenarioNotFound), text, text2);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
		}

		private static void EndActionLoadLocal()
		{
			Static.EndAction -= EndActionLoadLocal;
			LoadMission(m_newPath, multiplayer: false, MyOnlineModeEnum.OFFLINE, 1);
		}

		private static void EndActionLoadWorkshop()
		{
			Static.EndAction -= EndActionLoadWorkshop;
			MyWorkshop.CreateWorldInstanceAsync(m_newWorkshopMap, MyWorkshop.MyWorkshopPathInfo.CreateScenarioInfo(), overwrite: true, delegate(bool success, string sessionPath)
			{
				if (success)
				{
					m_newPath = sessionPath;
					LoadMission(sessionPath, multiplayer: false, MyOnlineModeEnum.OFFLINE, 1);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextWorkshopDownloadFailed), MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop)));
				}
			});
		}

		private static void CheckDx11AndLoad(string sessionPath, bool multiplayer, MyOnlineModeEnum onlineMode, short maxPlayers, MyGameModeEnum gameMode, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes)
		{
			LoadMission(sessionPath, multiplayer, onlineMode, maxPlayers, gameMode, checkpoint, checkpointSizeInBytes);
		}

		public static void LoadMission(string sessionPath, bool multiplayer, MyOnlineModeEnum onlineMode, short maxPlayers, MyGameModeEnum gameMode = MyGameModeEnum.Survival)
		{
			MyLog.Default.WriteLine("LoadSession() - Start");
			MyLog.Default.WriteLine(sessionPath);
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			CheckDx11AndLoad(sessionPath, multiplayer, onlineMode, maxPlayers, gameMode, checkpoint, sizeInBytes);
		}

		public static void LoadMission(string sessionPath, bool multiplayer, MyOnlineModeEnum onlineMode, short maxPlayers, MyGameModeEnum gameMode, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes)
		{
			bool scenarioEditMode = checkpoint.Settings.ScenarioEditMode;
			checkpoint.Settings.OnlineMode = onlineMode;
			checkpoint.Settings.MaxPlayers = maxPlayers;
			checkpoint.Settings.Scenario = true;
			checkpoint.Settings.GameMode = gameMode;
			checkpoint.Settings.ScenarioEditMode = false;
			if (!MySession.IsCompatibleVersion(checkpoint))
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion)));
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			if (!MyWorkshop.CheckLocalModsAllowed(checkpoint.Mods, checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer).ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			CheckpointData value = default(CheckpointData);
			value.Checkpoint = checkpoint;
			value.CheckpointSize = checkpointSizeInBytes;
			value.PersistentEditMode = scenarioEditMode;
			value.SessionPath = sessionPath;
			m_checkpointData = value;
			if (checkpoint.BriefingVideo != null && checkpoint.BriefingVideo.Length > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.MessageBoxCaptionVideo), messageText: MyTexts.Get(MySpaceTexts.MessageBoxTextWatchVideo), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnVideoMessageBox));
				return;
			}
			CheckpointData value2 = m_checkpointData.Value;
			m_checkpointData = null;
			LoadMission(value2);
		}

		private static void OnVideoMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGuiSandbox.OpenUrlWithFallback(m_checkpointData.Value.Checkpoint.BriefingVideo, "Scenario briefing video", useWhitelist: true);
			}
			CheckpointData value = m_checkpointData.Value;
			m_checkpointData = null;
			LoadMission(value);
		}

		private static void LoadMission(CheckpointData data)
		{
			MyObjectBuilder_Checkpoint checkpoint = data.Checkpoint;
			MyWorkshop.DownloadModsAsync(checkpoint.Mods, delegate(bool success)
			{
				if (success || (checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(checkpoint.Mods)))
				{
					MyScreenManager.CloseAllScreensNowExcept(null);
					MyGuiSandbox.Update(16);
					if (MySession.Static != null)
					{
						MySession.Static.Unload();
						MySession.Static = null;
					}
					if (checkpoint.Settings.ProceduralSeed == 0)
					{
						checkpoint.Settings.ProceduralSeed = MyRandom.Instance.Next();
					}
					MySessionLoader.StartLoading(delegate
					{
						checkpoint.Settings.Scenario = true;
						MySession.LoadMission(data.SessionPath, checkpoint, data.CheckpointSize, data.PersistentEditMode);
					});
				}
				else
				{
					MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed).ToString());
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
					{
						if (MyFakes.QUICK_LAUNCH.HasValue)
						{
							MySessionLoader.UnloadAndExitToMenu();
						}
					}));
				}
				MyLog.Default.WriteLine("LoadSession() - End");
			});
		}
	}
}
