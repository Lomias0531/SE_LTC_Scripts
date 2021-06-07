#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenMainMenuBase : MyGuiScreenBase
	{
		protected const float TEXT_LINE_HEIGHT = 0.014f;

		protected const int INITIAL_TRANSITION_TIME = 1500;

		protected bool m_pauseGame;

		protected bool m_musicPlayed;

		private static bool m_firstLoadup = true;

		private List<MyStringId> m_warningNotifications = new List<MyStringId>();

		private static readonly StringBuilder BUILD_DATE = new StringBuilder("Build: " + MySandboxGame.BuildDateTime.ToString("yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));

		private static readonly StringBuilder APP_VERSION = MyFinalBuildConstants.APP_VERSION_STRING;

		private static readonly StringBuilder STEAM_INACTIVE = new StringBuilder("STEAM NOT AVAILABLE");

		private static readonly StringBuilder NOT_OBFUSCATED = new StringBuilder("NOT OBFUSCATED");

		private static readonly StringBuilder NON_OFFICIAL = new StringBuilder(" NON-OFFICIAL");

		private static readonly StringBuilder PROFILING = new StringBuilder(" PROFILING");

		private static readonly StringBuilder PLATFORM = new StringBuilder(Environment.Is64BitProcess ? " 64-bit" : " 32-bit");

		private static StringBuilder BranchName = new StringBuilder(50);

		public bool DrawBuildInformation
		{
			get;
			set;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenMainMenu";
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public MyGuiScreenMainMenuBase(bool pauseGame = false)
			: base(Vector2.Zero)
		{
			if (MyScreenManager.IsScreenOfTypeOpen(typeof(MyGuiScreenGamePlay)))
			{
				m_pauseGame = pauseGame;
				if (m_pauseGame && !Sync.MultiplayerActive)
				{
					MySandboxGame.PausePush();
				}
			}
			else
			{
				m_closeOnEsc = false;
			}
			m_drawEvenWithoutFocus = false;
			DrawBuildInformation = true;
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (!m_musicPlayed)
			{
				if (MyGuiScreenGamePlay.Static == null)
				{
					MyAudio.Static.PlayMusic(MyPerGameSettings.MainMenuTrack);
				}
				m_musicPlayed = true;
			}
			if (MyReloadTestComponent.Enabled && base.State == MyGuiScreenState.OPENED)
			{
				MyReloadTestComponent.DoReload();
			}
			return true;
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			if (MySandboxGame.Config.EnablePerformanceWarnings && MySandboxGame.Config.ExperimentalMode && !m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode))
			{
				m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode);
			}
			MyGuiSandbox.DrawGameLogoHandler(m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
			DrawPerformanceWarning();
			if (DrawBuildInformation)
			{
				DrawObfuscationStatus();
				DrawSteamStatus();
				DrawAppVersion();
			}
			return true;
		}

		public override bool CloseScreen()
		{
			if (m_pauseGame && !Sync.MultiplayerActive)
			{
				MySandboxGame.PausePop();
			}
			bool result = base.CloseScreen();
			m_firstLoadup = false;
			m_musicPlayed = false;
			MyAnalyticsHelper.ReportActivityEnd(null, "show_main_menu");
			return result;
		}

		public override void CloseScreenNow()
		{
			m_firstLoadup = false;
			base.CloseScreenNow();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELP_SCREEN))
			{
				if (MyInput.Static.IsAnyShiftKeyPressed() && MyPerGameSettings.GUI.PerformanceWarningScreen != null)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
				}
				else
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
				}
			}
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply) && MyInput.Static.IsAnyShiftKeyPressed())
				{
					GC.Collect();
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.M))
				{
					RecreateControls(constructor: false);
				}
			}
		}

		public override void LoadContent()
		{
			base.LoadContent();
			RecreateControls(constructor: true);
		}

		public override bool HideScreen()
		{
			m_firstLoadup = false;
			return base.HideScreen();
		}

		public override int GetTransitionOpeningTime()
		{
			if (m_firstLoadup)
			{
				return 1500;
			}
			return base.GetTransitionOpeningTime();
		}

		private void DrawPerformanceWarning()
		{
			if (m_warningNotifications.Count != 0)
			{
				Vector2 vector = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 4, 42);
				vector -= new Vector2(MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE.SizeGui.X / 1.5f, 0f);
				MyGuiPaddedTexture tEXTURE_HUD_BG_PERFORMANCE = MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE;
				MyGuiManager.DrawSpriteBatch(tEXTURE_HUD_BG_PERFORMANCE.Texture, vector, tEXTURE_HUD_BG_PERFORMANCE.SizeGui / 1.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				MyGuiManager.DrawString("White", new StringBuilder(MyTexts.GetString(m_warningNotifications[0])), vector + new Vector2(0.09f, -0.011f), 0.7f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				StringBuilder stringBuilder = new StringBuilder();
				MyGuiManager.DrawString("White", stringBuilder.AppendFormat(MyCommonTexts.PerformanceWarningCombination, MyGuiSandbox.GetKeyName(MyControlsSpace.HELP_SCREEN)), vector + new Vector2(0.09f, 0.011f), 0.7f, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				stringBuilder.Clear();
				MyGuiManager.DrawString("White", stringBuilder.AppendFormat("({0})", m_warningNotifications.Count), vector + new Vector2(0.177f, -0.023f), 0.55f, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				m_warningNotifications.RemoveAt(0);
			}
		}

		private void DrawBuildDate()
		{
			Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
			normalizedCoord.Y -= 0f;
			MyGuiManager.DrawString("BuildInfo", BUILD_DATE, normalizedCoord, 0.6f, new Color(MyGuiConstants.LABEL_TEXT_COLOR * m_transitionAlpha, 0.6f), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
		}

		private void DrawAppVersion()
		{
			Vector2 zero = Vector2.Zero;
			Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, 8, 8);
			if (!string.IsNullOrEmpty(MyGameService.BranchName))
			{
				BranchName.Clear();
				BranchName.Append(" ");
				BranchName.Append(MyGameService.BranchName);
				zero = MyGuiManager.MeasureString("BuildInfoHighlight", BranchName, 0.6f);
				MyGuiManager.DrawString("BuildInfoHighlight", BranchName, normalizedCoord, 0.6f, new Color(MyGuiConstants.LABEL_TEXT_COLOR * m_transitionAlpha, 0.6f), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				normalizedCoord.X -= zero.X;
			}
			MyGuiManager.DrawString("BuildInfo", MyFinalBuildConstants.APP_VERSION_STRING_DOTS, normalizedCoord, 0.6f, new Color(MyGuiConstants.LABEL_TEXT_COLOR * m_transitionAlpha, 0.6f), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
		}

		private void DrawSteamStatus()
		{
			if (!MyGameService.IsActive)
			{
				Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				normalizedCoord.Y -= 0.028f;
				MyGuiManager.DrawString("BuildInfo", STEAM_INACTIVE, normalizedCoord, 0.6f, new Color(MyGuiConstants.LABEL_TEXT_COLOR * m_transitionAlpha, 0.6f), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
			}
		}

		private void DrawObfuscationStatus()
		{
			if (MyPerGameSettings.ShowObfuscationStatus && !MyObfuscation.Enabled)
			{
				Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				normalizedCoord.Y -= 0.0420000032f;
				MyGuiManager.DrawString("BuildInfoHighlight", NOT_OBFUSCATED, normalizedCoord, 0.6f, new Color(MyGuiConstants.LABEL_TEXT_COLOR * m_transitionAlpha, 0.6f), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
			}
		}

		protected MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick, MyStringId? tooltip = null)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.StripeLeft, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(text), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(MyTexts.GetString(tooltip.Value));
			}
			myGuiControlButton.BorderEnabled = false;
			myGuiControlButton.BorderSize = 0;
			myGuiControlButton.BorderHighlightEnabled = false;
			myGuiControlButton.BorderColor = Vector4.Zero;
			return myGuiControlButton;
		}

		protected void CheckLowMemSwitchToLow()
		{
			if (MySandboxGame.Config.LowMemSwitchToLow == MyConfig.LowMemSwitch.TRIGGERED)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MySpaceTexts.LowMemSwitchToLowQuestion), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.ARMED;
						MySandboxGame.Config.SetToLowQuality();
						MySandboxGame.Config.Save();
						if (MySpaceAnalytics.Instance != null)
						{
							MySpaceAnalytics.Instance.ReportGameQuit("Exit to Windows");
						}
						MyScreenManager.CloseAllScreensNowExcept(null);
						MySandboxGame.ExitThreadSafe();
					}
					else
					{
						MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.USER_SAID_NO;
						MySandboxGame.Config.Save();
					}
				}));
			}
		}
	}
}
