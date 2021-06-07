#define VRAGE
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenScenario : MyGuiScreenScenarioBase
	{
		private class LoadWorkshopResult : IMyAsyncResult
		{
			public List<MyWorkshopItem> SubscribedScenarios;

			public bool IsCompleted => Task.IsComplete;

			public Task Task
			{
				get;
				private set;
			}

			public LoadWorkshopResult()
			{
				Task = Parallel.Start(delegate
				{
					SubscribedScenarios = new List<MyWorkshopItem>();
					if (MyGameService.IsOnline)
					{
						MyWorkshop.GetSubscribedScenariosBlocking(SubscribedScenarios);
					}
				});
			}
		}

		private int m_listLoadedParts;

		private List<Tuple<string, MyWorldInfo>> m_availableSavesLocal = new List<Tuple<string, MyWorldInfo>>();

		private List<Tuple<string, MyWorldInfo>> m_availableSavesKeens = new List<Tuple<string, MyWorldInfo>>();

		private List<Tuple<string, MyWorldInfo>> m_availableSavesWorkshop = new List<Tuple<string, MyWorldInfo>>();

		private string m_sessionPath;

		private List<MyWorkshopItem> m_subscribedScenarios;

		protected MyObjectBuilder_SessionSettings m_settings;

		private MyObjectBuilder_Checkpoint m_checkpoint;

		private MyGuiControlLabel m_difficultyLabel;

		private MyGuiControlCombobox m_difficultyCombo;

		private MyGuiControlLabel m_onlineModeLabel;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlButton m_removeButton;

		private MyGuiControlButton m_publishButton;

		private MyGuiControlButton m_editButton;

		private MyGuiControlButton m_browseWorkshopButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_openInWorkshopButton;

		private MyGuiControlList m_scenarioTypesList;

		private MyGuiControlRadioButtonGroup m_scenarioTypesGroup;

		public MyObjectBuilder_SessionSettings Settings => m_settings;

		public MyObjectBuilder_Checkpoint Checkpoint => m_checkpoint;

		protected override MyStringId ScreenCaption => MySpaceTexts.ScreenCaptionScenario;

		protected override bool IsOnlineMode => m_onlineMode.GetSelectedKey() != 0;

		public MyGuiScreenScenario()
		{
			RecreateControls(constructor: true);
		}

		public override bool CloseScreen()
		{
			return base.CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenScenario";
		}

		protected override void BuildControls()
		{
			base.BuildControls();
			AddCaption(ScreenCaption);
			_ = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = m_size.Value / 2f - new Vector2(0.65f, 0.1f);
			MyGuiControlLabel control = MakeLabel(MySpaceTexts.Difficulty);
			MyGuiControlLabel control2 = MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
			m_maxPlayersLabel = MakeLabel(MyCommonTexts.MaxPlayers);
			float x = 0.309375018f;
			m_difficultyCombo = new MyGuiControlCombobox(null, new Vector2(x, 0.04f));
			m_difficultyCombo.Enabled = false;
			m_difficultyCombo.AddItem(0L, MySpaceTexts.DifficultyEasy);
			m_difficultyCombo.AddItem(1L, MySpaceTexts.DifficultyNormal);
			m_difficultyCombo.AddItem(2L, MySpaceTexts.DifficultyHard);
			m_onlineMode = new MyGuiControlCombobox(null, new Vector2(x, 0.04f));
			m_onlineMode.Enabled = false;
			m_onlineMode.ItemSelected += OnOnlineModeSelect;
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X, maxValue: MyMultiplayerLobby.MAX_PLAYERS, defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.05f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true);
			m_scenarioTypesList = new MyGuiControlList();
			m_removeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonRemove), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, base.OnOkButtonClick);
			m_publishButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonPublish), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnPublishButtonClick);
			m_editButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonEdit), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnEditButtonClick);
			m_browseWorkshopButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonBrowseWorkshop), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnBrowseWorkshopClick);
			m_refreshButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonRefresh), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnRefreshButtonClick);
			m_openInWorkshopButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.buttonOpenInWorkshop), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, base.OnOkButtonClick);
			m_removeButton.Enabled = false;
			m_publishButton.Enabled = false;
			m_editButton.Enabled = false;
			m_openInWorkshopButton.Enabled = false;
			base.CloseButtonEnabled = true;
			m_sideMenuLayout.Add(control, MyAlignH.Left, MyAlignV.Top, 2, 0);
			m_sideMenuLayout.Add(m_difficultyCombo, MyAlignH.Left, MyAlignV.Top, 2, 1);
			m_sideMenuLayout.Add(control2, MyAlignH.Left, MyAlignV.Top, 3, 0);
			m_sideMenuLayout.Add(m_onlineMode, MyAlignH.Left, MyAlignV.Top, 3, 1);
			m_sideMenuLayout.Add(m_maxPlayersLabel, MyAlignH.Left, MyAlignV.Top, 4, 0);
			m_sideMenuLayout.Add(m_maxPlayersSlider, MyAlignH.Left, MyAlignV.Top, 4, 1);
			m_buttonsLayout.Add(m_removeButton, MyAlignH.Left, MyAlignV.Top, 0, 0);
			m_buttonsLayout.Add(m_publishButton, MyAlignH.Left, MyAlignV.Top, 0, 1);
			m_buttonsLayout.Add(m_editButton, MyAlignH.Left, MyAlignV.Top, 0, 2);
			m_buttonsLayout.Add(m_browseWorkshopButton, MyAlignH.Left, MyAlignV.Top, 0, 3);
			m_buttonsLayout.Add(m_refreshButton, MyAlignH.Left, MyAlignV.Top, 1, 0);
			m_buttonsLayout.Add(m_openInWorkshopButton, MyAlignH.Left, MyAlignV.Top, 1, 1);
		}

		private void OnOnlineModeSelect()
		{
			m_maxPlayersSlider.Enabled = (m_onlineMode.GetSelectedKey() != 0);
			m_maxPlayersLabel.Enabled = (m_onlineMode.GetSelectedKey() != 0);
		}

		private void OnEditButtonClick(object sender)
		{
			MyGuiControlTable.Row selectedRow = m_scenarioTable.SelectedRow;
			if (selectedRow != null)
			{
				Tuple<string, MyWorldInfo> tuple = FindSave(selectedRow);
				if (tuple != null)
				{
					CloseScreen();
					MySessionLoader.LoadSingleplayerSession(tuple.Item1);
				}
			}
		}

		protected override void LoadSandboxInternal(Tuple<string, MyWorldInfo> save, bool MP)
		{
			base.LoadSandboxInternal(save, MP);
			if (save.Item1 == "workshop")
			{
				MyWorkshop.CreateWorldInstanceAsync(FindWorkshopScenario(save.Item2.WorkshopId.Value), MyWorkshop.MyWorkshopPathInfo.CreateScenarioInfo(), overwrite: true, delegate(bool success, string sessionPath)
				{
					if (success)
					{
						ulong sizeInBytes;
						MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
						myObjectBuilder_Checkpoint.Briefing = save.Item2.Briefing;
						MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, sessionPath);
						MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Scenario);
						MyScenarioSystem.LoadMission(sessionPath, MP, (MyOnlineModeEnum)m_onlineMode.GetSelectedKey(), (short)m_maxPlayersSlider.Value);
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextWorkshopDownloadFailed), MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop)));
					}
				});
				return;
			}
			MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Scenario);
			MyScenarioSystem.LoadMission(save.Item1, MP, (MyOnlineModeEnum)m_onlineMode.GetSelectedKey(), (short)m_maxPlayersSlider.Value);
		}

		private MyWorkshopItem FindWorkshopScenario(ulong workshopId)
		{
			foreach (MyWorkshopItem subscribedScenario in m_subscribedScenarios)
			{
				if (subscribedScenario.Id == workshopId)
				{
					return subscribedScenario;
				}
			}
			return null;
		}

		protected override MyGuiHighlightTexture GetIcon(Tuple<string, MyWorldInfo> save)
		{
			if (save.Item1 == "workshop")
			{
				return MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP;
			}
			if (save.Item2.ScenarioEditMode)
			{
				return MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
			}
			return MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL;
		}

		private void OnRefreshButtonClick(object sender)
		{
			m_state = StateEnum.ListNeedsReload;
		}

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			m_difficultyCombo.SelectItemByIndex(1);
			m_onlineMode.SelectItemByIndex(0);
		}

		protected override void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			base.OnTableItemSelected(sender, eventArgs);
			if (eventArgs.RowIndex < 2)
			{
				m_publishButton.Enabled = false;
				m_onlineMode.Enabled = false;
				m_onlineMode.SelectItemByIndex(0);
				m_editButton.Enabled = false;
				return;
			}
			m_publishButton.Enabled = false;
			m_onlineMode.Enabled = true;
			m_editButton.Enabled = false;
			if (m_scenarioTable.SelectedRow != null)
			{
				m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
				if (FindSave(m_scenarioTable.SelectedRow).Item1 != "workshop")
				{
					m_editButton.Enabled = true;
				}
			}
		}

		protected override void FillList()
		{
			base.FillList();
			m_listLoadedParts = 0;
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginKeens, endKeens));
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginWorkshop, endWorkshop));
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginLocal, endLocal));
		}

		private void AfterPartLoaded()
		{
			if (++m_listLoadedParts == 3)
			{
				ClearSaves();
				m_state = StateEnum.ListLoaded;
				AddSaves(m_availableSavesKeens);
				m_availableSavesKeens = null;
				AddSaves(m_availableSavesWorkshop);
				m_availableSavesWorkshop.Clear();
				foreach (Tuple<string, MyWorldInfo> item in m_availableSavesLocal)
				{
					if (item.Item2.ScenarioEditMode)
					{
						AddSave(item);
					}
				}
				m_availableSavesLocal.Clear();
				RefreshGameList();
			}
		}

		private IMyAsyncResult beginKeens()
		{
			return new MyLoadMissionListResult();
		}

		private void endKeens(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			m_availableSavesKeens = myLoadListResult.AvailableSaves;
			m_availableSavesKeens.Sort((Tuple<string, MyWorldInfo> x, Tuple<string, MyWorldInfo> y) => x.Item2.SessionName.CompareTo(y.Item2.SessionName));
			if (myLoadListResult.ContainsCorruptedWorlds)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SomeWorldFilesCouldNotBeLoaded), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			AfterPartLoaded();
			screen.CloseScreen();
		}

		private IMyAsyncResult beginLocal()
		{
			return new MyLoadWorldInfoListResult();
		}

		private void endLocal(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			myLoadListResult.AvailableSaves.Sort((Tuple<string, MyWorldInfo> x, Tuple<string, MyWorldInfo> y) => x.Item2.SessionName.CompareTo(y.Item2.SessionName));
			m_availableSavesLocal = myLoadListResult.AvailableSaves;
			AfterPartLoaded();
			screen.CloseScreen();
		}

		private IMyAsyncResult beginWorkshop()
		{
			return new LoadWorkshopResult();
		}

		private void endWorkshop(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadWorkshopResult loadWorkshopResult = (LoadWorkshopResult)result;
			m_subscribedScenarios = loadWorkshopResult.SubscribedScenarios;
			foreach (MyWorkshopItem subscribedScenario in loadWorkshopResult.SubscribedScenarios)
			{
				MyWorldInfo myWorldInfo = new MyWorldInfo();
				myWorldInfo.SessionName = subscribedScenario.Title;
				myWorldInfo.Briefing = subscribedScenario.Description;
				myWorldInfo.WorkshopId = subscribedScenario.Id;
				m_availableSavesWorkshop.Add(new Tuple<string, MyWorldInfo>("workshop", myWorldInfo));
			}
			m_availableSavesWorkshop.Sort((Tuple<string, MyWorldInfo> x, Tuple<string, MyWorldInfo> y) => x.Item2.SessionName.CompareTo(y.Item2.SessionName));
			AfterPartLoaded();
			screen.CloseScreen();
		}

		private MyWorkshopItem GetSubscribedItem(ulong? publishedFileId)
		{
			foreach (MyWorkshopItem subscribedScenario in m_subscribedScenarios)
			{
				if (subscribedScenario.Id == publishedFileId)
				{
					return subscribedScenario;
				}
			}
			return null;
		}

		private void OnPublishButtonClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_scenarioTable.SelectedRow;
			if (selectedRow != null && selectedRow.UserData != null)
			{
				string fullPath = ((Tuple<string, MyWorldInfo>)selectedRow.UserData).Item1;
				MyWorldInfo worldInfo = FindSave(m_scenarioTable.SelectedRow).Item2;
				StringBuilder messageText;
				MyStringId id;
				if (worldInfo.WorkshopId.HasValue)
				{
					messageText = new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextDoYouWishToUpdateScenario), MySession.GameServiceName);
					id = MySpaceTexts.MessageBoxCaptionDoYouWishToUpdateScenario;
				}
				else
				{
					messageText = new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextDoYouWishToPublishScenario), MySession.GameServiceName, MySession.PlatformLinkAgreement);
					id = MySpaceTexts.MessageBoxCaptionDoYouWishToPublishScenario;
				}
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageText, MyTexts.Get(id), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum val)
				{
					if (val == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyWorkshopItem subscribedItem = GetSubscribedItem(worldInfo.WorkshopId);
						if (subscribedItem != null)
						{
							subscribedItem.Tags.ToArray();
							if (subscribedItem.OwnerId != Sync.MyId)
							{
								MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_OwnerMismatchMod), MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublishFailed)));
								return;
							}
						}
						MyWorkshop.PublishScenarioAsync(fullPath, worldInfo.SessionName, worldInfo.Description, worldInfo.WorkshopId, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, MyWorkshopItemPublisher publishedFile)
						{
							if (success)
							{
								ulong sizeInBytes;
								MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(fullPath, out sizeInBytes);
								worldInfo.WorkshopId = publishedFile.Id;
								myObjectBuilder_Checkpoint.WorkshopId = publishedFile.Id;
								MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, fullPath);
								MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextScenarioPublished), MySession.GameServiceName), MyTexts.Get(MySpaceTexts.MessageBoxCaptionScenarioPublished), null, null, null, null, delegate
								{
									MyGameService.OpenOverlayUrl(MyGameService.WorkshopService.GetItemUrl(publishedFile.Id));
									FillList();
								}));
							}
							else
							{
								StringBuilder messageText2 = (result != MyGameServiceCallResult.AccessDenied) ? new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextScenarioPublishFailed), MySession.GameServiceName) : MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied);
								MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText2, MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublishFailed)));
							}
						});
					}
				}));
			}
		}

		private void OnBrowseWorkshopClick(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(MyGameService.WorkshopService.GetItemListUrl(MySteamConstants.TAG_SCENARIOS), MyGameService.WorkshopService.ServiceName + " Workshop");
		}
	}
}
