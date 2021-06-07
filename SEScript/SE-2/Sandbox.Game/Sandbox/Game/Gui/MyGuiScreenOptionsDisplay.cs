using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsDisplay : MyGuiScreenBase
	{
		private MyGuiControlLabel m_labelRecommendAspectRatio;

		private MyGuiControlLabel m_labelUnsupportedAspectRatio;

		private MyGuiControlCombobox m_comboVideoAdapter;

		private MyGuiControlCombobox m_comboResolution;

		private MyGuiControlCombobox m_comboWindowMode;

		private MyGuiControlCheckbox m_checkboxVSync;

		private MyGuiControlCheckbox m_checkboxCaptureMouse;

		private MyGuiControlCombobox m_comboScreenshotMultiplier;

		private MyRenderDeviceSettings m_settingsOld;

		private MyRenderDeviceSettings m_settingsNew;

		private bool m_waitingForConfirmation;

		private bool m_doRevert;

		private MyGuiControlElementGroup m_elementGroup;

		public MyGuiScreenOptionsDisplay()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.5696565f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsVideo";
		}

		public override void RecreateControls(bool constructor)
		{
			if (constructor)
			{
				base.RecreateControls(constructor);
				m_elementGroup = new MyGuiControlElementGroup();
				m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
				AddCaption(MyCommonTexts.ScreenCaptionDisplay, null, new Vector2(0f, 0.003f));
				MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
				Controls.Add(myGuiControlSeparatorList);
				MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
				myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
				Controls.Add(myGuiControlSeparatorList2);
				MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				Vector2 value = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				Vector2 value2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
				float num2 = 25f;
				float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
				float num3 = 0.0015f;
				Vector2 value3 = new Vector2(0f, 0.045f);
				float num4 = 0f;
				Vector2 value4 = new Vector2(0f, 0.008f);
				Vector2 value5 = (m_size.Value / 2f - value) * new Vector2(-1f, -1f) + new Vector2(0f, y);
				Vector2 value6 = (m_size.Value / 2f - value) * new Vector2(1f, -1f) + new Vector2(0f, y);
				Vector2 value7 = (m_size.Value / 2f - value2) * new Vector2(0f, 1f);
				Vector2 value8 = new Vector2(value6.X - (num + num3), value6.Y);
				num4 -= 0.045f;
				MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.VideoAdapter))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_comboVideoAdapter = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVideoAdapter))
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2
				};
				int num5 = 0;
				MyAdapterInfo[] adapters = MyVideoSettingsManager.Adapters;
				for (int i = 0; i < adapters.Length; i++)
				{
					MyAdapterInfo myAdapterInfo = adapters[i];
					m_comboVideoAdapter.AddItem(num5++, new StringBuilder(myAdapterInfo.Name));
				}
				num4 += 1f;
				MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsVideo_WindowMode))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_comboWindowMode = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_WindowMode))
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2
				};
				num4 += 1f;
				MyGuiControlLabel control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.VideoMode))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_comboResolution = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVideoMode))
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2
				};
				num4 += 1f;
				m_labelUnsupportedAspectRatio = new MyGuiControlLabel(null, null, null, MyGuiConstants.LABEL_TEXT_COLOR * 0.9f, 0.578f)
				{
					Position = new Vector2(value6.X - (num - num3), value6.Y) + num4 * value3,
					OriginAlign = originAlign
				};
				m_labelUnsupportedAspectRatio.Text = $"* {MyTexts.Get(MyCommonTexts.UnsupportedAspectRatio)}";
				num4 += 0.45f;
				m_labelRecommendAspectRatio = new MyGuiControlLabel(null, null, null, MyGuiConstants.LABEL_TEXT_COLOR * 0.9f, 0.578f)
				{
					Position = new Vector2(value6.X - (num - num3), value6.Y) + num4 * value3,
					OriginAlign = originAlign
				};
				num4 += 0.66f;
				MyGuiControlLabel control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenshotMultiplier))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_comboScreenshotMultiplier = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_ScreenshotMultiplier))
				{
					Position = value6 + num4 * value3,
					OriginAlign = originAlign2
				};
				num4 += 1.26f;
				MyGuiControlLabel control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.VerticalSync))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_checkboxVSync = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsVerticalSync))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				num4 += 1f;
				MyGuiControlLabel control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.CaptureMouse))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_checkboxCaptureMouse = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsCaptureMouse))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				num4 += 1f;
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick);
				myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
				MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
				myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
				myGuiControlButton.Position = value7 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
				myGuiControlButton2.Position = value7 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
				Controls.Add(control);
				Controls.Add(m_comboVideoAdapter);
				Controls.Add(control3);
				Controls.Add(m_comboResolution);
				Controls.Add(m_labelUnsupportedAspectRatio);
				Controls.Add(m_labelRecommendAspectRatio);
				Controls.Add(control2);
				Controls.Add(m_comboWindowMode);
				Controls.Add(control4);
				Controls.Add(m_comboScreenshotMultiplier);
				Controls.Add(control6);
				Controls.Add(m_checkboxCaptureMouse);
				Controls.Add(control5);
				Controls.Add(m_checkboxVSync);
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				Controls.Add(myGuiControlButton2);
				m_elementGroup.Add(myGuiControlButton2);
				m_comboVideoAdapter.ItemSelected += UpdateWindowModeComboBox;
				m_comboVideoAdapter.ItemSelected += UpdateResolutionComboBox;
				m_comboWindowMode.ItemSelected += UpdateRecommendecAspectRatioLabel;
				m_comboWindowMode.ItemSelected += UpdateResolutionComboBox;
				m_settingsOld = MyVideoSettingsManager.CurrentDeviceSettings;
				m_settingsNew = m_settingsOld;
				WriteSettingsToControls(m_settingsOld);
				ReadSettingsFromControls(ref m_settingsOld);
				ReadSettingsFromControls(ref m_settingsNew);
				base.FocusedControl = myGuiControlButton;
				base.CloseButtonEnabled = true;
			}
		}

		private void m_elementGroup_HighlightChanged(MyGuiControlElementGroup obj)
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

		private MyAdapterInfo GetSelectedAdapter()
		{
			return MyVideoSettingsManager.Adapters[(int)m_comboVideoAdapter.GetSelectedKey()];
		}

		private MyWindowModeEnum GetSelectedWindowMode()
		{
			return (MyWindowModeEnum)m_comboWindowMode.GetSelectedKey();
		}

		private void SelectWindowMode(MyWindowModeEnum mode)
		{
			m_comboWindowMode.SelectItemByKey((int)mode);
		}

		private long GetResolutionKey(Vector2I resolution)
		{
			return ((long)resolution.X << 32) | resolution.Y;
		}

		private Vector2I GetResolutionFromKey(long key)
		{
			return new Vector2I((int)(key >> 32), (int)(key & uint.MaxValue));
		}

		private Vector2I GetSelectedResolution()
		{
			long selectedKey = m_comboResolution.GetSelectedKey();
			return GetResolutionFromKey(selectedKey);
		}

		private void SelectResolution(Vector2I resolution)
		{
			int num = int.MaxValue;
			Vector2I resolution2 = resolution;
			for (int i = 0; i < m_comboResolution.GetItemsCount(); i++)
			{
				Vector2I resolutionFromKey = GetResolutionFromKey(m_comboResolution.GetItemByIndex(i).Key);
				if (resolutionFromKey == resolution)
				{
					resolution2 = resolution;
					break;
				}
				int num2 = Math.Abs(resolutionFromKey.X * resolutionFromKey.Y - resolution.X * resolution.Y);
				if (num2 < num)
				{
					num = num2;
					resolution2 = resolutionFromKey;
				}
			}
			m_comboResolution.SelectItemByKey(GetResolutionKey(resolution2));
		}

		private void UpdateAdapterComboBox()
		{
			long selectedKey = m_comboVideoAdapter.GetSelectedKey();
			m_comboVideoAdapter.ClearItems();
			int num = 0;
			MyAdapterInfo[] adapters = MyVideoSettingsManager.Adapters;
			for (int i = 0; i < adapters.Length; i++)
			{
				MyAdapterInfo myAdapterInfo = adapters[i];
				m_comboVideoAdapter.AddItem(num++, new StringBuilder(myAdapterInfo.Name));
			}
			m_comboVideoAdapter.SelectItemByKey(selectedKey);
		}

		private void UpdateWindowModeComboBox()
		{
			MyWindowModeEnum myWindowModeEnum = (MyWindowModeEnum)m_comboWindowMode.GetSelectedKey();
			m_comboWindowMode.ClearItems();
			bool isOutputAttached = GetSelectedAdapter().IsOutputAttached;
			m_comboWindowMode.AddItem(0L, MyCommonTexts.ScreenOptionsVideo_WindowMode_Window);
			m_comboWindowMode.AddItem(1L, MyCommonTexts.ScreenOptionsVideo_WindowMode_FullscreenWindow);
			if (isOutputAttached)
			{
				m_comboWindowMode.AddItem(2L, MyCommonTexts.ScreenOptionsVideo_WindowMode_Fullscreen);
			}
			if (myWindowModeEnum == MyWindowModeEnum.Fullscreen && !isOutputAttached)
			{
				myWindowModeEnum = MyWindowModeEnum.FullscreenWindow;
			}
			m_comboWindowMode.SelectItemByKey((int)myWindowModeEnum);
		}

		private void UpdateResolutionComboBox()
		{
			Vector2I selectedResolution = GetSelectedResolution();
			MyWindowModeEnum selectedWindowMode = GetSelectedWindowMode();
			m_comboResolution.ClearItems();
			MyDisplayMode[] supportedDisplayModes = GetSelectedAdapter().SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode myDisplayMode = supportedDisplayModes[i];
				Vector2I vector2I = new Vector2I(myDisplayMode.Width, myDisplayMode.Height);
				bool flag = true;
				if (selectedWindowMode == MyWindowModeEnum.Window)
				{
					Vector2I fixedWindowResolution = MyRenderProxyUtils.GetFixedWindowResolution(vector2I, GetSelectedAdapter());
					if (vector2I != fixedWindowResolution)
					{
						flag = false;
					}
				}
				if (m_comboResolution.TryGetItemByKey(GetResolutionKey(vector2I)) != null)
				{
					flag = false;
				}
				if (flag)
				{
					MyAspectRatio recommendedAspectRatio = MyVideoSettingsManager.GetRecommendedAspectRatio((int)m_comboVideoAdapter.GetSelectedKey());
					MyAspectRatioEnum closestAspectRatio = MyVideoSettingsManager.GetClosestAspectRatio((float)vector2I.X / (float)vector2I.Y);
					MyAspectRatio aspectRatio = MyVideoSettingsManager.GetAspectRatio(closestAspectRatio);
					string textShort = aspectRatio.TextShort;
					string text = (!aspectRatio.IsSupported) ? " *" : ((closestAspectRatio == recommendedAspectRatio.AspectRatioEnum) ? " ***" : "");
					m_comboResolution.AddItem(GetResolutionKey(vector2I), new StringBuilder($"{vector2I.X} x {vector2I.Y} - {textShort}{text}"));
				}
			}
			SelectResolution(selectedResolution);
		}

		private void UpdateScreenshotMultiplierComboBox()
		{
			int num = (int)m_comboScreenshotMultiplier.GetSelectedKey();
			m_comboScreenshotMultiplier.ClearItems();
			m_comboScreenshotMultiplier.AddItem(1L, "1x");
			m_comboScreenshotMultiplier.AddItem(2L, "2x");
			m_comboScreenshotMultiplier.AddItem(4L, "4x");
			m_comboScreenshotMultiplier.AddItem(8L, "8x");
			m_comboScreenshotMultiplier.SelectItemByKey(num);
		}

		private void UpdateRecommendecAspectRatioLabel()
		{
			MyAspectRatio recommendedAspectRatio = MyVideoSettingsManager.GetRecommendedAspectRatio((int)m_comboVideoAdapter.GetSelectedKey());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.RecommendedAspectRatio), recommendedAspectRatio.TextShort);
			m_labelRecommendAspectRatio.Text = $"*** {stringBuilder}";
		}

		private bool ReadSettingsFromControls(ref MyRenderDeviceSettings deviceSettings)
		{
			bool flag = false;
			MyRenderDeviceSettings myRenderDeviceSettings = default(MyRenderDeviceSettings);
			myRenderDeviceSettings.AdapterOrdinal = deviceSettings.AdapterOrdinal;
			MyRenderDeviceSettings myRenderDeviceSettings2 = myRenderDeviceSettings;
			Vector2I selectedResolution = GetSelectedResolution();
			myRenderDeviceSettings2.BackBufferWidth = selectedResolution.X;
			myRenderDeviceSettings2.BackBufferHeight = selectedResolution.Y;
			myRenderDeviceSettings2.WindowMode = GetSelectedWindowMode();
			myRenderDeviceSettings2.NewAdapterOrdinal = (int)m_comboVideoAdapter.GetSelectedKey();
			flag |= (myRenderDeviceSettings2.NewAdapterOrdinal != myRenderDeviceSettings2.AdapterOrdinal);
			myRenderDeviceSettings2.VSync = (m_checkboxVSync.IsChecked ? 1 : 0);
			myRenderDeviceSettings2.RefreshRate = 0;
			if (m_checkboxCaptureMouse.IsChecked != MySandboxGame.Config.CaptureMouse)
			{
				MySandboxGame.Config.CaptureMouse = m_checkboxCaptureMouse.IsChecked;
				MySandboxGame.Static.UpdateMouseCapture();
			}
			MyDisplayMode[] supportedDisplayModes = MyVideoSettingsManager.Adapters[deviceSettings.AdapterOrdinal].SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode myDisplayMode = supportedDisplayModes[i];
				if (myDisplayMode.Width == myRenderDeviceSettings2.BackBufferWidth && myDisplayMode.Height == myRenderDeviceSettings2.BackBufferHeight && myRenderDeviceSettings2.RefreshRate < myDisplayMode.RefreshRate)
				{
					myRenderDeviceSettings2.RefreshRate = myDisplayMode.RefreshRate;
				}
			}
			flag = (flag || !myRenderDeviceSettings2.Equals(ref deviceSettings));
			deviceSettings = myRenderDeviceSettings2;
			return flag;
		}

		private void WriteSettingsToControls(MyRenderDeviceSettings deviceSettings)
		{
			UpdateAdapterComboBox();
			m_comboVideoAdapter.SelectItemByKey(deviceSettings.NewAdapterOrdinal, sendEvent: false);
			UpdateWindowModeComboBox();
			UpdateResolutionComboBox();
			UpdateScreenshotMultiplierComboBox();
			m_comboScreenshotMultiplier.SelectItemByKey((int)MySandboxGame.Config.ScreenshotSizeMultiplier);
			SelectResolution(new Vector2I(deviceSettings.BackBufferWidth, deviceSettings.BackBufferHeight));
			SelectWindowMode(deviceSettings.WindowMode);
			m_checkboxVSync.IsChecked = (deviceSettings.VSync > 0);
			m_checkboxCaptureMouse.IsChecked = MySandboxGame.Config.CaptureMouse;
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			bool num = ReadSettingsFromControls(ref m_settingsNew);
			MySandboxGame.Config.ScreenshotSizeMultiplier = m_comboScreenshotMultiplier.GetSelectedKey();
			if (num)
			{
				OnVideoModeChangedAndConfirm(MyVideoSettingsManager.Apply(m_settingsNew));
			}
			else
			{
				CloseScreen();
			}
		}

		private void OnVideoModeChangedAndConfirm(MyVideoSettingsManager.ChangeResult result)
		{
			switch (result)
			{
			case MyVideoSettingsManager.ChangeResult.NothingChanged:
				break;
			case MyVideoSettingsManager.ChangeResult.Success:
				m_waitingForConfirmation = true;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_TIMEOUT, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: MyTexts.Get(MyCommonTexts.DoYouWantToKeepTheseSettingsXSecondsRemaining), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnMessageBoxCallback, timeoutInMiliseconds: 60000));
				break;
			case MyVideoSettingsManager.ChangeResult.Failed:
				m_doRevert = true;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SorryButSelectedSettingsAreNotSupportedByYourHardware), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				break;
			}
		}

		private void OnVideoModeChanged(MyVideoSettingsManager.ChangeResult result)
		{
			WriteSettingsToControls(m_settingsOld);
			ReadSettingsFromControls(ref m_settingsNew);
		}

		public void OnMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				if (m_settingsNew.NewAdapterOrdinal != m_settingsNew.AdapterOrdinal)
				{
					MySandboxGame.Config.DisableUpdateDriverNotification = false;
				}
				MyVideoSettingsManager.SaveCurrentSettings();
				ReadSettingsFromControls(ref m_settingsOld);
				CloseScreenNow();
				if (m_settingsNew.NewAdapterOrdinal != m_settingsNew.AdapterOrdinal)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextRestartNeededAfterAdapterSwitch), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnMessageBoxAdapterChangeCallback));
				}
			}
			else
			{
				m_doRevert = true;
			}
			m_waitingForConfirmation = false;
		}

		public void OnMessageBoxAdapterChangeCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MySessionLoader.ExitGame();
			}
		}

		public override bool CloseScreen()
		{
			bool num = base.CloseScreen();
			if (num)
			{
				_ = m_waitingForConfirmation;
			}
			return num;
		}

		public override bool Draw()
		{
			if (!base.Draw())
			{
				return false;
			}
			if (m_doRevert)
			{
				OnVideoModeChanged(MyVideoSettingsManager.Apply(m_settingsOld));
				m_doRevert = false;
			}
			return true;
		}
	}
}
