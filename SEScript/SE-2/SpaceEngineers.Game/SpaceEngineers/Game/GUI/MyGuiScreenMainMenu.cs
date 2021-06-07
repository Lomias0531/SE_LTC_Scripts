#define VRAGE
using ParallelTasks;
using Sandbox;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenMainMenu : MyGuiScreenMainMenuBase
	{
		private static MyStringHash SkinSale = MyStringHash.GetOrCompute("SkinSale");

		private static MyStringHash GhostSkin = MyStringHash.GetOrCompute("GhostSkin");

		private readonly int DLC_UPDATE_INTERVAL = 5000;

		private MyGuiControlNews m_newsControl;

		private MyGuiControlDLCBanners m_dlcBannersControl;

		private MyGuiControlBase m_continueTooltipcontrol;

		private MyGuiControlElementGroup m_elementGroup;

		private int m_currentDLCcounter;

		private bool isStartMenu = true;

		private MyBadgeHelper m_myBadgeHelper;

		private const int CONTROLS_PER_BANNER = 3;

		public MyGuiControlButton m_exitGameButton;

		public MyGuiControlImageButton m_lastClickedBanner;

		private bool m_canChangeBannerHighlight = true;

		public MyGuiScreenMainMenu()
			: this(pauseGame: false)
		{
		}

		public MyGuiScreenMainMenu(bool pauseGame)
			: base(pauseGame)
		{
			m_myBadgeHelper = new MyBadgeHelper();
			if (!pauseGame && MyGuiScreenGamePlay.Static == null)
			{
				AddIntroScreen();
			}
			MyGuiSandbox.DrawGameLogoHandler = m_myBadgeHelper.DrawGameLogo;
		}

		private void AddIntroScreen()
		{
			if (MyFakes.ENABLE_MENU_VIDEO_BACKGROUND)
			{
				MyGuiSandbox.AddScreen(MyGuiScreenIntroVideo.CreateBackgroundScreen());
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			m_elementGroup.HighlightChanged += m_elementGroup_SelectedChanged;
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			Vector2 value = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM) + new Vector2(minSizeGui.X / 2f, 0f) + new Vector2(15f, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			value.Y += 0.043f;
			_ = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) + new Vector2((0f - minSizeGui.X) / 2f, 0f);
			if (MyGuiScreenGamePlay.Static == null)
			{
				base.EnabledBackgroundFade = false;
				MyGuiControlButton myGuiControlButton = null;
				int num = MyPerGameSettings.MultiplayerEnabled ? 7 : 6;
				MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
				if (lastSession != null && (!lastSession.IsLobby || MyGameService.Service.ContinueToLobbySupported))
				{
					myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA - MyGuiConstants.MENU_BUTTONS_POSITION_DELTA / 2f, MyCommonTexts.ScreenMenuButtonContinueGame, OnContinueGameClicked);
					Controls.Add(myGuiControlButton);
					m_elementGroup.Add(myGuiControlButton);
					GenerateContinueTooltip(lastSession, myGuiControlButton, new Vector2(0.003f, -0.0025f));
					myGuiControlButton.FocusChanged += FocusChangedContinue;
				}
				else
				{
					num--;
				}
				myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonCampaign, OnClickNewGame);
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonLoadGame, OnClickLoad);
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				if (MyPerGameSettings.MultiplayerEnabled)
				{
					myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonJoinGame, OnJoinWorld);
					Controls.Add(myGuiControlButton);
					m_elementGroup.Add(myGuiControlButton);
				}
				myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonOptions, OnClickOptions);
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				if (!MyFakes.LIMITED_MAIN_MENU || MyInput.Static.ENABLE_DEVELOPER_KEYS)
				{
					if (MyFakes.ENABLE_MAIN_MENU_INVENTORY_SCENE)
					{
						myGuiControlButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonInventory, OnClickInventory);
						Controls.Add(myGuiControlButton);
						m_elementGroup.Add(myGuiControlButton);
					}
					m_exitGameButton = MakeButton(value - num-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonExitToWindows, OnClickExitToWindows);
					Controls.Add(m_exitGameButton);
					m_elementGroup.Add(m_exitGameButton);
				}
			}
			else
			{
				MyAnalyticsHelper.ReportActivityStart(null, "show_main_menu", string.Empty, "gui", string.Empty);
				base.EnabledBackgroundFade = true;
				int num2 = Sync.MultiplayerActive ? 6 : 5;
				MyGuiControlButton myGuiControlButton2 = MakeButton(value - --num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonSave, OnClickSaveWorld);
				MyGuiControlButton myGuiControlButton3 = MakeButton(value - --num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.LoadScreenButtonSaveAs, OnClickSaveAs);
				if (!Sync.IsServer || !MySession.Static.Settings.EnableSaving)
				{
					MyStringId toolTip = (!Sync.IsServer) ? MyCommonTexts.NotificationClientCannotSave : MyCommonTexts.NotificationSavingDisabled;
					myGuiControlButton2.Enabled = false;
					myGuiControlButton2.ShowTooltipWhenDisabled = true;
					myGuiControlButton2.SetToolTip(toolTip);
					myGuiControlButton3.Enabled = false;
					myGuiControlButton3.ShowTooltipWhenDisabled = true;
					myGuiControlButton3.SetToolTip(toolTip);
				}
				Controls.Add(myGuiControlButton2);
				m_elementGroup.Add(myGuiControlButton2);
				Controls.Add(myGuiControlButton3);
				m_elementGroup.Add(myGuiControlButton3);
				MyGuiControlButton myGuiControlButton4;
				if (Sync.MultiplayerActive)
				{
					myGuiControlButton4 = MakeButton(value - --num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonPlayers, OnClickPlayers);
					Controls.Add(myGuiControlButton4);
					m_elementGroup.Add(myGuiControlButton4);
				}
				myGuiControlButton4 = MakeButton(value - --num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonOptions, OnClickOptions);
				Controls.Add(myGuiControlButton4);
				m_elementGroup.Add(myGuiControlButton4);
				m_exitGameButton = MakeButton(value - --num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonExitToMainMenu, OnExitToMainMenuClick);
				Controls.Add(m_exitGameButton);
				m_elementGroup.Add(m_exitGameButton);
			}
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 49, 82), MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
			Controls.Add(myGuiControlPanel);
			if (!MyFakes.SHOW_BANNERS)
			{
				m_myBadgeHelper.RefreshGameLogo();
			}
			m_newsControl = new MyGuiControlNews
			{
				Position = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) - 5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA,
				Size = new Vector2(0.4f, 0.28f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			Controls.Add(m_newsControl);
			float num3 = m_newsControl.Size.X - 0.004f;
			float num4 = 0.407226563f;
			float num5 = num3 * num4 * 1.33333337f;
			Vector2 size = new Vector2(m_newsControl.Size.X, num5 + 0.052f);
			m_dlcBannersControl = new MyGuiControlDLCBanners
			{
				Position = new Vector2(m_newsControl.Position.X, 0.26f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			m_dlcBannersControl.Size = size;
			m_dlcBannersControl.Visible = false;
			Controls.Add(m_dlcBannersControl);
			CheckLowMemSwitchToLow();
		}

		private void GenerateContinueTooltip(MyObjectBuilder_LastSession lastSession, MyGuiControlButton button, Vector2 correction)
		{
			string thumbnail = GetThumbnail(lastSession);
			string text = (!lastSession.IsOnline) ? $"{MyTexts.GetString(MyCommonTexts.ToolTipContinueGame)}{Environment.NewLine}{lastSession.GameName}" : ((!lastSession.IsLobby) ? $"{MyTexts.GetString(MyCommonTexts.ToolTipContinueGame)}{Environment.NewLine}{lastSession.GameName} - {lastSession.ServerIP}:{lastSession.ServerPort}" : $"{MyTexts.GetString(MyCommonTexts.ToolTipContinueGame)}{Environment.NewLine}{lastSession.GameName} - {lastSession.ServerIP}");
			MyGuiControlBase myGuiControlBase = null;
			if (thumbnail != null)
			{
				MyRenderProxy.PreloadTextures(new List<string>
				{
					thumbnail
				}, TextureType.GUIWithoutPremultiplyAlpha);
			}
			myGuiControlBase = CreateImageTooltip(thumbnail, text);
			myGuiControlBase.Visible = false;
			myGuiControlBase.Position = button.Position + new Vector2(0.5f * button.Size.X, -1f * button.Size.Y) + correction;
			m_continueTooltipcontrol = myGuiControlBase;
			Controls.Add(m_continueTooltipcontrol);
		}

		private void FocusChangedContinue(MyGuiControlBase controls, bool focused)
		{
			m_continueTooltipcontrol.Visible = focused;
		}

		private string GetThumbnail(MyObjectBuilder_LastSession session)
		{
			if (session == null)
			{
				return null;
			}
			string path = session.Path;
			if (Directory.Exists(path + MyGuiScreenLoadSandbox.CONST_BACKUP))
			{
				string[] directories = Directory.GetDirectories(path + MyGuiScreenLoadSandbox.CONST_BACKUP);
				if (directories.Any())
				{
					string text = directories.Last() + MyGuiScreenLoadSandbox.CONST_THUMB;
					if (File.Exists(text) && new FileInfo(text).Length > 0)
					{
						return Directory.GetDirectories(path + MyGuiScreenLoadSandbox.CONST_BACKUP).Last() + MyGuiScreenLoadSandbox.CONST_THUMB;
					}
				}
			}
			string text2 = path + MyGuiScreenLoadSandbox.CONST_THUMB;
			if (File.Exists(text2) && new FileInfo(text2).Length > 0)
			{
				return path + MyGuiScreenLoadSandbox.CONST_THUMB;
			}
			return null;
		}

		private MyGuiControlBase CreateImageTooltip(string path, string text)
		{
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds"),
				ColorMask = MyGuiConstants.THEMED_GUI_BACKGROUND_COLOR
			};
			myGuiControlParent.CanHaveFocus = false;
			myGuiControlParent.HighlightType = MyGuiControlHighlightType.NEVER;
			myGuiControlParent.BorderEnabled = true;
			Vector2 value = new Vector2(0.005f, 0.002f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(Vector2.Zero, null, text)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlLabel.CanHaveFocus = false;
			myGuiControlLabel.HighlightType = MyGuiControlHighlightType.NEVER;
			MyGuiControlImage myGuiControlImage = null;
			if (!string.IsNullOrEmpty(path))
			{
				myGuiControlImage = new MyGuiControlImage(Vector2.Zero, new Vector2(0.175625f, 0.131718755f))
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM
				};
				myGuiControlImage.SetTexture(path);
				myGuiControlImage.CanHaveFocus = false;
				myGuiControlImage.HighlightType = MyGuiControlHighlightType.NEVER;
			}
			else
			{
				myGuiControlImage = new MyGuiControlImage(Vector2.Zero, Vector2.Zero)
				{
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM
				};
			}
			myGuiControlParent.Size = new Vector2(Math.Max(myGuiControlLabel.Size.X, myGuiControlImage.Size.X) + value.X * 2f, myGuiControlLabel.Size.Y + myGuiControlImage.Size.Y + value.Y * 4f);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			myGuiControlParent.Controls.Add(myGuiControlLabel);
			myGuiControlLabel.Position = -myGuiControlParent.Size / 2f + value;
			myGuiControlImage.Position = new Vector2(0f, myGuiControlParent.Size.Y / 2f - value.Y);
			return myGuiControlParent;
		}

		private void MenuRefocusImageButton(MyGuiControlImageButton sender)
		{
			m_lastClickedBanner = sender;
		}

		private void OnClickBack(MyGuiControlButton obj)
		{
			isStartMenu = true;
			RecreateControls(constructor: false);
		}

		private void OnPlayClicked(MyGuiControlButton obj)
		{
			isStartMenu = false;
			RecreateControls(constructor: false);
		}

		private void OnClickInventory(MyGuiControlButton obj)
		{
			if (MyGameService.IsActive)
			{
				if (MySession.Static == null)
				{
					MyGuiScreenLoadInventory inventory = MyGuiSandbox.CreateScreen<MyGuiScreenLoadInventory>(Array.Empty<object>());
					MyGuiScreenLoading screen = new MyGuiScreenLoading(inventory, null);
					MyGuiScreenLoadInventory myGuiScreenLoadInventory = inventory;
					myGuiScreenLoadInventory.OnLoadingAction = (Action)Delegate.Combine(myGuiScreenLoadInventory.OnLoadingAction, (Action)delegate
					{
						MySessionLoader.LoadInventoryScene();
						MySandboxGame.IsUpdateReady = true;
						inventory.Initialize(inGame: false, null);
					});
					MyGuiSandbox.AddScreen(screen);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenLoadInventory>(new object[2]
					{
						false,
						null
					}));
				}
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.SteamIsOfflinePleaseRestart)));
			}
		}

		private void m_elementGroup_SelectedChanged(MyGuiControlElementGroup obj)
		{
			foreach (MyGuiControlBase item in m_elementGroup)
			{
				if (item.HasFocus && obj.SelectedElement != item)
				{
					base.FocusedControl = obj.SelectedElement;
					break;
				}
			}
		}

		private void OnContinueGameClicked(MyGuiControlButton myGuiControlButton)
		{
			RunWithTutorialCheck(delegate
			{
				MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
				if (lastSession != null)
				{
					if (lastSession.IsOnline)
					{
						if (lastSession.IsLobby)
						{
							MyJoinGameHelper.JoinGame(ulong.Parse(lastSession.ServerIP));
						}
						else
						{
							try
							{
								string serverIP = lastSession.ServerIP;
								ushort port = (ushort)lastSession.ServerPort;
								IPAddress[] hostAddresses = Dns.GetHostAddresses(serverIP);
								MyGameService.OnPingServerResponded += MySandboxGame.Static.ServerResponded;
								MyGameService.OnPingServerFailedToRespond += MySandboxGame.Static.ServerFailedToRespond;
								MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), port);
							}
							catch (Exception ex)
							{
								MyLog.Default.WriteLine(ex);
								MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
							}
						}
					}
					else
					{
						MySessionLoader.LoadLastSession();
					}
				}
			});
		}

		private void OnCustomGameClicked(MyGuiControlButton myGuiControlButton)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenWorldSettings>(Array.Empty<object>()));
		}

		private void OnClickReportBug(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrl(MyPerGameSettings.BugReportUrl, UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextOpenBrowser, "forums.keenswh.com"));
		}

		private void OnJoinWorld(MyGuiControlButton sender)
		{
			RunWithTutorialCheck(delegate
			{
				if (MyGameService.IsOnline)
				{
					MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: true, delegate(bool granted)
					{
						if (granted)
						{
							MyGuiScreenJoinGame myGuiScreenJoinGame = new MyGuiScreenJoinGame();
							myGuiScreenJoinGame.Closed += joinGameScreen_Closed;
							MyGuiSandbox.AddScreen(myGuiScreenJoinGame);
						}
					});
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyGameService.IsActive ? MyCommonTexts.SteamIsOfflinePleaseRestart : MyCommonTexts.ErrorJoinSessionNoUser), MySession.GameServiceName)));
				}
			});
		}

		private void joinGameScreen_Closed(MyGuiScreenBase source)
		{
			if (source.Cancelled)
			{
				base.State = MyGuiScreenState.OPENING;
				source.Closed -= joinGameScreen_Closed;
			}
		}

		private void OnClickRecommend(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.MessageBoxCaptionRecommend), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.MessageBoxTextRecommend), MySession.GameServiceName), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnClickRecommendOK));
		}

		private void OnClickRecommendOK(MyGuiScreenMessageBox.ResultEnum result)
		{
			MyGuiSandbox.OpenUrl(MySteamConstants.URL_RECOMMEND_GAME, UrlOpenMode.SteamOrExternal);
		}

		private void RunWithTutorialCheck(Action afterTutorial)
		{
			if (MySandboxGame.Config.FirstTimeTutorials)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenTutorialsScreen(afterTutorial));
			}
			else
			{
				afterTutorial();
			}
		}

		private void OnClickNewGame(MyGuiControlButton sender)
		{
			if (MySandboxGame.Config.EnableNewNewGameScreen)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenSimpleNewGame>(Array.Empty<object>()));
			}
			else
			{
				RunWithTutorialCheck(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenNewGame>(new object[3]
					{
						true,
						true,
						true
					}));
				});
			}
		}

		private void OnClickLoad(MyGuiControlBase sender)
		{
			RunWithTutorialCheck(delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenLoadSandbox());
			});
		}

		private void OnClickPlayers(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenPlayers>(Array.Empty<object>()));
		}

		private void OnExitToMainMenuClick(MyGuiControlButton sender)
		{
			base.CanBeHidden = false;
			MyGuiScreenMessageBox myGuiScreenMessageBox = (!Sync.IsServer) ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAnyWorldBeforeExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), null, null, null, null, OnExitToMainMenuFromCampaignMessageBoxCallback) : ((MySession.Static.Settings.EnableSaving && Sync.IsServer) ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, MyTexts.Get(MyCommonTexts.MessageBoxTextSaveChangesBeforeExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), null, null, null, null, OnExitToMainMenuMessageBoxCallback) : MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextCampaignBeforeExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), null, null, null, null, OnExitToMainMenuFromCampaignMessageBoxCallback));
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.InstantClose = false;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		private void OnExitToMainMenuMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			switch (callbackReturn)
			{
			case MyGuiScreenMessageBox.ResultEnum.YES:
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MyAsyncSaving.Start(delegate
				{
					MySandboxGame.Static.OnScreenshotTaken += UnloadAndExitAfterScreeshotWasTaken;
				}, null, wait: true);
				break;
			case MyGuiScreenMessageBox.ResultEnum.NO:
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MySessionLoader.UnloadAndExitToMenu();
				break;
			case MyGuiScreenMessageBox.ResultEnum.CANCEL:
				base.CanBeHidden = true;
				break;
			}
		}

		private void OnExitToMainMenuFromCampaignMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyAudio.Static.Mute = true;
				MyAudio.Static.StopMusic();
				MySessionLoader.UnloadAndExitToMenu();
			}
			else
			{
				base.CanBeHidden = true;
			}
		}

		private void UnloadAndExitAfterScreeshotWasTaken(object sender, EventArgs e)
		{
			MySandboxGame.Static.OnScreenshotTaken -= UnloadAndExitAfterScreeshotWasTaken;
			MySessionLoader.UnloadAndExitToMenu();
		}

		private void OnClickOptions(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen<MyGuiScreenOptionsSpace>(Array.Empty<object>()));
		}

		private void OnClickExitToWindows(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), null, null, null, null, OnExitToWindowsMessageBoxCallback));
		}

		private void OnExitToWindowsMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				OnLogoutProgressClosed();
			}
			else if (m_exitGameButton != null && m_exitGameButton.Visible)
			{
				base.FocusedControl = m_exitGameButton;
				m_exitGameButton.Selected = true;
			}
		}

		private void OnLogoutProgressClosed()
		{
			MySandboxGame.Log.WriteLine("Application closed by user");
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.ReportGameQuit("Exit to Windows");
			}
			MyScreenManager.CloseAllScreensNowExcept(null);
			MySandboxGame.ExitThreadSafe();
		}

		private void OnClickSaveWorld(MyGuiControlButton sender)
		{
			base.CanBeHidden = false;
			MyGuiScreenMessageBox myGuiScreenMessageBox = (!MyAsyncSaving.InProgress) ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToSaveYourProgress), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnSaveWorldMessageBoxCallback) : MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.InstantClose = false;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		private void OnSaveWorldMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyAsyncSaving.Start();
			}
			else
			{
				base.CanBeHidden = true;
			}
		}

		private void OnClickSaveAs(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenSaveAs(MySession.Static.Name));
		}

		public override bool Update(bool hasFocus)
		{
			base.Update(hasFocus);
			if (MySession.Static == null)
			{
				Parallel.RunCallbacks();
			}
			m_currentDLCcounter += 16;
			if (m_currentDLCcounter > DLC_UPDATE_INTERVAL)
			{
				m_currentDLCcounter = 0;
				m_myBadgeHelper.RefreshGameLogo();
			}
			if (MyGuiScreenGamePlay.Static == null && hasFocus && MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
			{
				if (isStartMenu)
				{
					OnClickExitToWindows(null);
				}
				else
				{
					OnClickBack(null);
				}
			}
			if (hasFocus && m_lastClickedBanner != null)
			{
				base.FocusedControl = null;
				m_lastClickedBanner = null;
			}
			return true;
		}

		protected override void OnShow()
		{
			base.OnShow();
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				RecreateControls(constructor: false);
			}
		}
	}
}
