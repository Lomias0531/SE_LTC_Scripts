#define VRAGE
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.World
{
	public static class MySessionLoader
	{
		private static readonly Random random = new Random();

		public static event Action BattleWorldLoaded;

		public static event Action ScenarioWorldLoaded;

		public static event Action<MyObjectBuilder_Checkpoint> CampaignWorldLoaded;

		public static void StartNewSession(string sessionName, MyObjectBuilder_SessionSettings settings, List<MyObjectBuilder_Checkpoint.ModItem> mods, MyScenarioDefinition scenarioDefinition = null, int asteroidAmount = 0, string description = "", string passwd = "")
		{
			MyLog.Default.WriteLine("StartNewSandbox - Start");
			if (!MyWorkshop.CheckLocalModsAllowed(mods, settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
			}
			else
			{
				MyWorkshop.DownloadModsAsync(mods, delegate(bool success)
				{
					if (success || (settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(mods)))
					{
						MyScreenManager.RemoveAllScreensExcept(null);
						if (asteroidAmount < 0)
						{
							MyWorldGenerator.SetProceduralSettings(asteroidAmount, settings);
							asteroidAmount = 0;
						}
						MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Custom);
						StartLoading(delegate
						{
							MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Custom);
							MySession.Start(sessionName, description, passwd, settings, mods, new MyWorldGenerator.Args
							{
								AsteroidAmount = asteroidAmount,
								Scenario = scenarioDefinition
							});
						});
					}
					else if (MyGameService.IsOnline)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed)));
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
					}
					MyLog.Default.WriteLine("StartNewSandbox - End");
				});
			}
		}

		public static void LoadLastSession()
		{
			string lastSessionPath = MyLocalCache.GetLastSessionPath();
			if (lastSessionPath == null || !MyFileSystem.DirectoryExists(lastSessionPath))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxLastSessionNotFound), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				LoadSingleplayerSession(lastSessionPath);
			}
		}

		public static void LoadMultiplayerSession(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			MyLog.Default.WriteLine("LoadSession() - Start");
			if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, allowLocalMods: false))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
			}
			else
			{
				MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, delegate(bool success)
				{
					if (success)
					{
						MyScreenManager.CloseAllScreensNowExcept(null);
						MyGuiSandbox.Update(16);
						if (MySession.Static != null)
						{
							MySession.Static.Unload();
							MySession.Static = null;
						}
						StartLoading(delegate
						{
							MySession.LoadMultiplayer(world, multiplayerSession);
						});
					}
					else
					{
						if (MyMultiplayer.Static != null)
						{
							MyMultiplayer.Static.Dispose();
						}
						if (MyGameService.IsOnline)
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed)));
						}
						else
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
						}
					}
					MyLog.Default.WriteLine("LoadSession() - End");
				}, delegate
				{
					multiplayerSession.Dispose();
					UnloadAndExitToMenu();
				});
			}
		}

		public static void LoadMultiplayerScenarioWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - Start");
			if (world.Checkpoint.BriefingVideo != null && world.Checkpoint.BriefingVideo.Length > 0)
			{
				MyGuiSandbox.OpenUrlWithFallback(world.Checkpoint.BriefingVideo, "Scenario briefing video", useWhitelist: true);
			}
			if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, allowLocalMods: false))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
				{
					UnloadAndExitToMenu();
				}));
				MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
			}
			else
			{
				MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, delegate(bool success)
				{
					if (success)
					{
						MyScreenManager.CloseAllScreensNowExcept(null);
						MyGuiSandbox.Update(16);
						StartLoading(delegate
						{
							MySession.Static.LoadMultiplayerWorld(world, multiplayerSession);
							if (MySessionLoader.ScenarioWorldLoaded != null)
							{
								MySessionLoader.ScenarioWorldLoaded();
							}
						});
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
						{
							MySandboxGame.Static.Invoke(UnloadAndExitToMenu, "UnloadAndExitToMenu");
						}));
					}
					MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
				}, delegate
				{
					UnloadAndExitToMenu();
				});
			}
		}

		private static void CheckDx11AndLoad(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad = null)
		{
			LoadSingleplayerSession(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
		}

		public static void LoadSingleplayerSession(string sessionPath, Action afterLoad = null, string contextName = null, MyOnlineModeEnum? onlineMode = null, int maxPlayers = 0)
		{
			MyLog.Default.WriteLine("LoadSession() - Start");
			MyLog.Default.WriteLine(sessionPath);
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			if (myObjectBuilder_Checkpoint == null)
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded).ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded)));
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			myObjectBuilder_Checkpoint.CustomLoadingScreenText = MyStatControlText.SubstituteTexts(myObjectBuilder_Checkpoint.CustomLoadingScreenText, contextName);
			if (onlineMode.HasValue)
			{
				myObjectBuilder_Checkpoint.OnlineMode = onlineMode.Value;
				myObjectBuilder_Checkpoint.MaxPlayers = (short)maxPlayers;
			}
			CheckDx11AndLoad(myObjectBuilder_Checkpoint, sessionPath, sizeInBytes, afterLoad);
		}

		private static string GetCustomLoadingScreenImagePath(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				return null;
			}
			string text = Path.Combine(MyFileSystem.SavesPath, relativePath);
			if (!MyFileSystem.FileExists(text))
			{
				text = Path.Combine(MyFileSystem.ContentPath, relativePath);
			}
			if (!MyFileSystem.FileExists(text))
			{
				text = Path.Combine(MyFileSystem.ModsPath, relativePath);
			}
			if (!MyFileSystem.FileExists(text))
			{
				text = null;
			}
			return text;
		}

		public static void LoadSingleplayerSession(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad = null)
		{
			if (!MySession.IsCompatibleVersion(checkpoint))
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion)));
				MyLog.Default.WriteLine("LoadSession() - End");
			}
			else if ((MyCampaignManager.Static == null || !MyCampaignManager.Static.IsNewCampaignLevelLoading || MyCampaignManager.Static.ActiveCampaign == null || !MyCampaignManager.Static.ActiveCampaign.IsVanilla || MyCampaignManager.Static.ActiveCampaign.PublishedFileId != 0L) && (string.IsNullOrEmpty(checkpoint.CustomLoadingScreenImage) || (checkpoint.Mods != null && checkpoint.Mods.Count != 0)) && !MySandboxGame.Config.ExperimentalMode && checkpoint.Settings.ExperimentalMode)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.SaveGameErrorExperimental)));
			}
			else if (!MyWorkshop.CheckLocalModsAllowed(checkpoint.Mods, checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer).ToString());
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
			}
			else
			{
				MyWorkshop.DownloadModsAsync(checkpoint.Mods, delegate(bool success)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						DownloadModsDone(success, checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
					}, "MySessionLoader::DownloadModsDone");
					MyLog.Default.WriteLine("LoadSession() - End");
				}, UnloadAndExitToMenu);
			}
		}

		private static void DownloadModsDone(bool success, MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad)
		{
			if (success || (checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(checkpoint.Mods)))
			{
				MyScreenManager.CloseAllScreensNowExcept(null);
				MyGuiSandbox.Update(16);
				string customLoadingScreenPath = GetCustomLoadingScreenImagePath(checkpoint.CustomLoadingScreenImage);
				StartLoading(delegate
				{
					if (MySession.Static != null)
					{
						MySession.Static.Unload();
						MySession.Static = null;
					}
					MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Load);
					MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes, saveLastStates: true, allowXml: false);
					if (afterLoad != null)
					{
						afterLoad();
					}
				}, delegate
				{
					StartLoading(delegate
					{
						if (MySession.Static != null)
						{
							MySession.Static.Unload();
							MySession.Static = null;
						}
						MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Load);
						MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes);
						if (afterLoad != null)
						{
							afterLoad();
						}
					}, null, customLoadingScreenPath, checkpoint.CustomLoadingScreenText);
				}, customLoadingScreenPath, checkpoint.CustomLoadingScreenText);
			}
			else
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed).ToString());
				if (MyGameService.IsOnline)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
					{
						if (MyFakes.QUICK_LAUNCH.HasValue)
						{
							UnloadAndExitToMenu();
						}
					}));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
				}
			}
		}

		public static void StartLoading(Action loadingAction, Action loadingActionXMLAllowed = null, string customLoadingBackground = null, string customLoadingtext = null)
		{
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.StoreLoadingStartTime();
			}
			MyGuiScreenGamePlay myGuiScreenGamePlay = new MyGuiScreenGamePlay();
			myGuiScreenGamePlay.OnLoadingAction = (Action)Delegate.Combine(myGuiScreenGamePlay.OnLoadingAction, loadingAction);
			MyGuiScreenLoading myGuiScreenLoading = new MyGuiScreenLoading(myGuiScreenGamePlay, MyGuiScreenGamePlay.Static, customLoadingBackground, customLoadingtext);
			myGuiScreenLoading.OnScreenLoadingFinished += delegate
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
			};
			myGuiScreenLoading.OnLoadingXMLAllowed = loadingActionXMLAllowed;
			MyGuiSandbox.AddScreen(myGuiScreenLoading);
		}

		public static void Unload()
		{
			MyScreenManager.CloseAllScreensNowExcept(null);
			MyGuiSandbox.Update(16);
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
				MySession.Static = null;
			}
			if (MyMusicController.Static != null)
			{
				MyMusicController.Static.Unload();
				MyMusicController.Static = null;
				MyAudio.Static.MusicAllowed = true;
				MyAudio.Static.Mute = false;
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.Dispose();
			}
		}

		public static void UnloadAndExitToMenu()
		{
			Unload();
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));
		}

		public static void LoadInventoryScene()
		{
			if (MyGameService.IsActive && MyFakes.ENABLE_MAIN_MENU_INVENTORY_SCENE)
			{
				string sessionPath = Path.Combine(MyFileSystem.ContentPath, "InventoryScenes\\Inventory-9");
				DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition> mainMenuInventoryScenes = MyDefinitionManager.Static.GetMainMenuInventoryScenes();
				if (mainMenuInventoryScenes.Count > 0)
				{
					List<MyMainMenuInventorySceneDefinition> list = mainMenuInventoryScenes.ToList();
					int index = random.Next(0, list.Count);
					sessionPath = Path.Combine(MyFileSystem.ContentPath, list[index].SceneDirectory);
				}
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
				MySession.Load(sessionPath, checkpoint, sizeInBytes, saveLastStates: false);
			}
		}

		public static void ExitGame()
		{
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.ReportGameQuit("Exit to Windows");
			}
			MyScreenManager.CloseAllScreensNowExcept(null);
			MySandboxGame.ExitThreadSafe();
		}
	}
}
