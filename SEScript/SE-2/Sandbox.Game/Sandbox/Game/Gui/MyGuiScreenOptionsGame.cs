using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsGame : MyGuiScreenBase
	{
		private struct OptionsGameSettings
		{
			public MyLanguagesEnum Language;

			public MyCubeBuilder.BuildingModeEnum BuildingMode;

			public MyStringId SkinId;

			public bool ExperimentalMode;

			public bool ControlHints;

			public bool GoodBotHints;

			public bool EnableNewNewGameScreen;

			public bool RotationHints;

			public bool ShowCrosshair;

			public bool EnableTrading;

			public bool EnableSteamCloud;

			public bool EnablePrediction;

			public bool ShowPlayerNamesOnHud;

			public bool EnablePerformanceWarnings;

			public float UIOpacity;

			public float UIBkOpacity;

			public float HUDBkOpacity;
		}

		private MyGuiControlCombobox m_languageCombobox;

		private MyGuiControlCombobox m_buildingModeCombobox;

		private MyGuiControlCheckbox m_experimentalCheckbox;

		private MyGuiControlCheckbox m_controlHintsCheckbox;

		private MyGuiControlCheckbox m_goodbotHintsCheckbox;

		private MyGuiControlCheckbox m_enableNewNewGameScreen;

		private MyGuiControlButton m_goodBotResetButton;

		private MyGuiControlCheckbox m_rotationHintsCheckbox;

		private MyGuiControlCheckbox m_crosshairCheckbox;

		private MyGuiControlCheckbox m_cloudCheckbox;

		private MyGuiControlCheckbox m_GDPRConsentCheckbox;

		private MyGuiControlCheckbox m_enableTradingCheckbox;

		private MyGuiControlSlider m_UIOpacitySlider;

		private MyGuiControlSlider m_UIBkOpacitySlider;

		private MyGuiControlSlider m_HUDBkOpacitySlider;

		private MyGuiControlButton m_localizationWebButton;

		private MyGuiControlLabel m_localizationWarningLabel;

		private OptionsGameSettings m_settings = new OptionsGameSettings
		{
			UIOpacity = 1f,
			UIBkOpacity = 1f,
			HUDBkOpacity = 0.6f
		};

		private bool m_languangeChanged;

		private MyGuiControlElementGroup m_elementGroup;

		private MyLanguagesEnum m_loadedLanguage;

		public MyGuiScreenOptionsGame()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 237f / 262f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
			AddCaption(MyCommonTexts.ScreenCaptionGameOptions, null, new Vector2(0f, 0.003f));
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
			Vector2 value3 = new Vector2(0f, 0.042f);
			float num4 = 0f;
			Vector2 value4 = new Vector2(0f, 0.008f);
			Vector2 value5 = (m_size.Value / 2f - value) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 value6 = (m_size.Value / 2f - value) * new Vector2(1f, -1f) + new Vector2(0f, y);
			Vector2 value7 = (m_size.Value / 2f - value2) * new Vector2(0f, 1f);
			Vector2 value8 = new Vector2(value6.X - (num + num3), value6.Y);
			num4 -= 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.Language))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_languageCombobox = new MyGuiControlCombobox
			{
				Position = value6 + num4 * value3,
				OriginAlign = originAlign2
			};
			m_languageCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_Language));
			foreach (MyLanguagesEnum supportedLanguage in MyLanguage.SupportedLanguages)
			{
				MyTexts.MyLanguageDescription myLanguageDescription = MyTexts.Languages[supportedLanguage];
				string text = myLanguageDescription.Name;
				if (myLanguageDescription.IsCommunityLocalized)
				{
					text += " *";
				}
				m_languageCombobox.AddItem((int)supportedLanguage, text);
			}
			m_languageCombobox.CustomSortItems((MyGuiControlCombobox.Item a, MyGuiControlCombobox.Item b) => a.Key.CompareTo(b.Key));
			m_languageCombobox.ItemSelected += m_languageCombobox_ItemSelected;
			num4 += 1f;
			m_localizationWarningLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_LocalizationWarning), null, 0.578f)
			{
				Position = value6 + num4 * value3 - new Vector2(num - 0.005f, 0f),
				OriginAlign = originAlign
			};
			Vector2? position = value6 + num4 * value3 - new Vector2(num - 0.008f - m_localizationWarningLabel.Size.X, 0f);
			StringBuilder text2 = MyTexts.Get(MyCommonTexts.ScreenOptionsGame_MoreInfo);
			Action<MyGuiControlButton> onButtonClick = LocalizationWebButtonClicked;
			m_localizationWebButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, null, text2, 0.6f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick);
			m_localizationWebButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			num4 += 0.83f;
			MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_BuildingMode))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_buildingModeCombobox = new MyGuiControlCombobox
			{
				Position = value6 + num4 * value3,
				OriginAlign = originAlign2
			};
			m_buildingModeCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_BuildingMode));
			m_buildingModeCombobox.AddItem(0L, MyCommonTexts.ScreenOptionsGame_SingleBlock);
			m_buildingModeCombobox.AddItem(1L, MyCommonTexts.ScreenOptionsGame_Line);
			m_buildingModeCombobox.AddItem(2L, MyCommonTexts.ScreenOptionsGame_Plane);
			m_buildingModeCombobox.ItemSelected += m_buildingModeCombobox_ItemSelected;
			num4 += 1.26f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ExperimentalMode))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			myGuiControlLabel.Enabled = (MyGuiScreenGamePlay.Static == null);
			m_experimentalCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsExperimentalMode))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			m_experimentalCheckbox.Enabled = (MyGuiScreenGamePlay.Static == null);
			num4 += 1f;
			MyGuiControlLabel control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowControlsHints))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_controlHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowControlsHints))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox controlHintsCheckbox = m_controlHintsCheckbox;
			controlHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(controlHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num4 += 1f;
			MyGuiControlLabel control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowGoodBotHints))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_goodbotHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowGoodBotHints))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox goodbotHintsCheckbox = m_goodbotHintsCheckbox;
			goodbotHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(goodbotHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			m_goodBotResetButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, onButtonClick: OnResetGoodbotPressed, toolTip: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints_TTIP))
			{
				Position = new Vector2(base.Size.Value.X * 0.5f - value.X, m_goodbotHintsCheckbox.Position.Y),
				OriginAlign = originAlign2,
				Text = MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints)
			};
			MyGuiControlLabel control5 = null;
			if (MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				num4 += 1f;
				control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.EnableNewNewGameScreen))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_enableNewNewGameScreen = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableNewNewGameScreen))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				MyGuiControlCheckbox enableNewNewGameScreen = m_enableNewNewGameScreen;
				enableNewNewGameScreen.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(enableNewNewGameScreen.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			}
			MyGuiControlLabel control6 = null;
			if (MyFakes.ENABLE_ROTATION_HINTS)
			{
				num4 += 1f;
				control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowRotationHints))
				{
					Position = value5 + num4 * value3 + value4,
					OriginAlign = originAlign
				};
				m_rotationHintsCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowRotationHints))
				{
					Position = value8 + num4 * value3,
					OriginAlign = originAlign
				};
				MyGuiControlCheckbox rotationHintsCheckbox = m_rotationHintsCheckbox;
				rotationHintsCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(rotationHintsCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			}
			num4 += 1f;
			MyGuiControlLabel control7 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ShowCrosshair))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_crosshairCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowCrosshair))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox crosshairCheckbox = m_crosshairCheckbox;
			crosshairCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(crosshairCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num4 += 1f;
			MyGuiControlLabel control8 = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MyCommonTexts.EnableSteamCloud), MyGameService.Service.ServiceName))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_cloudCheckbox = new MyGuiControlCheckbox(null, null, string.Format(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableSteamCloud), MyGameService.Service.ServiceName))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox cloudCheckbox = m_cloudCheckbox;
			cloudCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(cloudCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num4 += 1f;
			MyGuiControlLabel control9 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_enableTradingCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading_TTIP))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			MyGuiControlCheckbox enableTradingCheckbox = m_enableTradingCheckbox;
			enableTradingCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(enableTradingCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			num4 += 1f;
			MyGuiControlLabel control10 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.GDPR_Caption))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_GDPRConsentCheckbox = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_GDPRConsent))
			{
				Position = value8 + num4 * value3,
				OriginAlign = originAlign
			};
			m_GDPRConsentCheckbox.IsChecked = (MySandboxGame.Config.GDPRConsent == true || !MySandboxGame.Config.GDPRConsent.HasValue);
			MyGuiControlCheckbox gDPRConsentCheckbox = m_GDPRConsentCheckbox;
			gDPRConsentCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(gDPRConsentCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ReleasingAltResetsCamera))
			{
				Position = value5 + num4 * value3,
				OriginAlign = originAlign
			};
			num4 += 1.35f;
			MyGuiControlLabel control11 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIOpacity))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_UIOpacitySlider = new MyGuiControlSlider(null, 0.1f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIOpacity), defaultValue: 1f)
			{
				Position = value6 + num4 * value3,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider uIOpacitySlider = m_UIOpacitySlider;
			uIOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(uIOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			num4 += 1.08f;
			MyGuiControlLabel control12 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIBkOpacity))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_UIBkOpacitySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIBkOpacity), defaultValue: 1f)
			{
				Position = value6 + num4 * value3,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider uIBkOpacitySlider = m_UIBkOpacitySlider;
			uIBkOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(uIBkOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			num4 += 1.08f;
			MyGuiControlLabel control13 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_HUDBkOpacity))
			{
				Position = value5 + num4 * value3 + value4,
				OriginAlign = originAlign
			};
			m_HUDBkOpacitySlider = new MyGuiControlSlider(null, 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsHUDBkOpacity), defaultValue: 1f)
			{
				Position = value6 + num4 * value3,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			MyGuiControlSlider hUDBkOpacitySlider = m_HUDBkOpacitySlider;
			hUDBkOpacitySlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(hUDBkOpacitySlider.ValueChanged, new Action<MyGuiControlSlider>(sliderChanged));
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick);
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
			myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			myGuiControlButton.Position = value7 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton2.Position = value7 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			Controls.Add(control);
			Controls.Add(m_languageCombobox);
			Controls.Add(m_localizationWebButton);
			Controls.Add(m_localizationWarningLabel);
			Controls.Add(control2);
			Controls.Add(m_buildingModeCombobox);
			Controls.Add(myGuiControlLabel);
			Controls.Add(control3);
			Controls.Add(control4);
			Controls.Add(m_experimentalCheckbox);
			Controls.Add(m_controlHintsCheckbox);
			Controls.Add(m_goodbotHintsCheckbox);
			if (MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				Controls.Add(control5);
				Controls.Add(m_enableNewNewGameScreen);
			}
			Controls.Add(m_goodBotResetButton);
			if (MyFakes.ENABLE_ROTATION_HINTS)
			{
				Controls.Add(control6);
				Controls.Add(m_rotationHintsCheckbox);
			}
			Controls.Add(control7);
			Controls.Add(m_crosshairCheckbox);
			Controls.Add(control8);
			Controls.Add(m_cloudCheckbox);
			Controls.Add(control9);
			Controls.Add(m_enableTradingCheckbox);
			Controls.Add(control10);
			Controls.Add(m_GDPRConsentCheckbox);
			Controls.Add(control11);
			Controls.Add(m_UIOpacitySlider);
			Controls.Add(control12);
			Controls.Add(m_UIBkOpacitySlider);
			Controls.Add(control13);
			Controls.Add(m_HUDBkOpacitySlider);
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			Controls.Add(myGuiControlButton2);
			m_elementGroup.Add(myGuiControlButton2);
			UpdateControls(constructor);
			MyGuiControlCheckbox experimentalCheckbox = m_experimentalCheckbox;
			experimentalCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(experimentalCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(checkboxChanged));
			base.FocusedControl = myGuiControlButton;
			base.CloseButtonEnabled = true;
		}

		private void OnResetGoodbotPressed(MyGuiControlButton obj)
		{
			if (MySession.Static != null)
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
			}
			else
			{
				MySandboxGame.Config.TutorialsFinished.Clear();
				MySandboxGame.Config.Save();
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaption_GoodBotHintsReset), messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxText_GoodBotHintReset))));
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

		private void checkboxChanged(MyGuiControlCheckbox obj)
		{
			if (obj == m_experimentalCheckbox)
			{
				if (obj.IsChecked)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmExperimental)), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							m_settings.ExperimentalMode = obj.IsChecked;
						}
						else
						{
							m_settings.ExperimentalMode = false;
							obj.IsChecked = false;
						}
					}));
				}
			}
			else if (obj == m_controlHintsCheckbox)
			{
				m_settings.ControlHints = obj.IsChecked;
			}
			if (m_rotationHintsCheckbox != null && obj == m_rotationHintsCheckbox)
			{
				m_settings.RotationHints = obj.IsChecked;
			}
			else if (obj == m_crosshairCheckbox)
			{
				m_settings.ShowCrosshair = obj.IsChecked;
			}
			else if (obj == m_enableTradingCheckbox)
			{
				m_settings.EnableTrading = obj.IsChecked;
			}
			else if (obj == m_cloudCheckbox)
			{
				m_settings.EnableSteamCloud = obj.IsChecked;
			}
			else if (obj == m_goodbotHintsCheckbox)
			{
				ref OptionsGameSettings settings = ref m_settings;
				bool goodBotHints = m_goodBotResetButton.Enabled = obj.IsChecked;
				settings.GoodBotHints = goodBotHints;
			}
			else if (obj == m_enableNewNewGameScreen)
			{
				m_settings.EnableNewNewGameScreen = m_enableNewNewGameScreen.Enabled;
			}
			else if (obj == m_GDPRConsentCheckbox)
			{
				MySandboxGame.Config.GDPRConsent = obj.IsChecked;
				MySandboxGame.Config.Save();
				ConsentSenderGDPR.TrySendConsent();
			}
		}

		private void sliderChanged(MyGuiControlSlider obj)
		{
			if (obj == m_UIOpacitySlider)
			{
				m_settings.UIOpacity = obj.Value;
				m_guiTransition = obj.Value;
			}
			else if (obj == m_UIBkOpacitySlider)
			{
				m_settings.UIBkOpacity = obj.Value;
				m_backgroundTransition = obj.Value;
			}
			else if (obj == m_HUDBkOpacitySlider)
			{
				m_settings.HUDBkOpacity = obj.Value;
			}
		}

		private void m_buildingModeCombobox_ItemSelected()
		{
			m_settings.BuildingMode = (MyCubeBuilder.BuildingModeEnum)m_buildingModeCombobox.GetSelectedKey();
		}

		private void LocalizationWebButtonClicked(MyGuiControlButton obj)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextOpenBrowser), MyPerGameSettings.GameWebUrl), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
			{
				if (retval == MyGuiScreenMessageBox.ResultEnum.YES && !MyVRage.Platform.OpenUrl(MyPerGameSettings.LocalizationWebUrl))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.TitleFailedToStartInternetBrowser), MyPerGameSettings.LocalizationWebUrl);
					StringBuilder messageCaption2 = MyTexts.Get(MyCommonTexts.TitleFailedToStartInternetBrowser);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, messageCaption2));
				}
			}));
		}

		private void m_languageCombobox_ItemSelected()
		{
			m_settings.Language = (MyLanguagesEnum)m_languageCombobox.GetSelectedKey();
			if (MyTexts.Languages[m_settings.Language].IsCommunityLocalized)
			{
				m_localizationWarningLabel.ColorMask = Color.Red.ToVector4();
			}
			else
			{
				m_localizationWarningLabel.ColorMask = Color.White.ToVector4();
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsGame";
		}

		private void UpdateControls(bool constructor)
		{
			if (constructor)
			{
				m_languageCombobox.SelectItemByKey((int)MySandboxGame.Config.Language);
				m_loadedLanguage = (MyLanguagesEnum)m_languageCombobox.GetSelectedKey();
				m_buildingModeCombobox.SelectItemByKey((long)MyCubeBuilder.BuildingMode);
				m_controlHintsCheckbox.IsChecked = MySandboxGame.Config.ControlsHints;
				m_goodbotHintsCheckbox.IsChecked = MySandboxGame.Config.GoodBotHints;
				if (MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN)
				{
					m_enableNewNewGameScreen.IsChecked = MySandboxGame.Config.EnableNewNewGameScreen;
				}
				m_goodBotResetButton.Enabled = m_goodbotHintsCheckbox.IsChecked;
				m_experimentalCheckbox.IsChecked = MySandboxGame.Config.ExperimentalMode;
				if (m_rotationHintsCheckbox != null)
				{
					m_rotationHintsCheckbox.IsChecked = MySandboxGame.Config.RotationHints;
				}
				m_crosshairCheckbox.IsChecked = MySandboxGame.Config.ShowCrosshair;
				m_enableTradingCheckbox.IsChecked = MySandboxGame.Config.EnableTrading;
				m_cloudCheckbox.IsChecked = MySandboxGame.Config.EnableSteamCloud;
				m_UIOpacitySlider.Value = MySandboxGame.Config.UIOpacity;
				m_UIBkOpacitySlider.Value = MySandboxGame.Config.UIBkOpacity;
				m_HUDBkOpacitySlider.Value = MySandboxGame.Config.HUDBkOpacity;
			}
			else
			{
				m_languageCombobox.SelectItemByKey((int)m_settings.Language);
				m_buildingModeCombobox.SelectItemByKey((long)m_settings.BuildingMode);
				m_controlHintsCheckbox.IsChecked = m_settings.ControlHints;
				m_goodbotHintsCheckbox.IsChecked = m_settings.GoodBotHints;
				if (MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN)
				{
					m_enableNewNewGameScreen.IsChecked = m_settings.EnableNewNewGameScreen;
				}
				m_goodBotResetButton.Enabled = m_goodbotHintsCheckbox.IsChecked;
				m_experimentalCheckbox.IsChecked = m_settings.ExperimentalMode;
				if (m_rotationHintsCheckbox != null)
				{
					m_rotationHintsCheckbox.IsChecked = m_settings.RotationHints;
				}
				m_crosshairCheckbox.IsChecked = m_settings.ShowCrosshair;
				m_enableTradingCheckbox.IsChecked = m_settings.EnableTrading;
				m_cloudCheckbox.IsChecked = m_settings.EnableSteamCloud;
				m_UIOpacitySlider.Value = m_settings.UIOpacity;
				m_UIBkOpacitySlider.Value = m_settings.UIBkOpacity;
				m_HUDBkOpacitySlider.Value = m_settings.HUDBkOpacity;
			}
		}

		private void DoChanges()
		{
			MySandboxGame.Config.ExperimentalMode = m_experimentalCheckbox.IsChecked;
			MySandboxGame.Config.ShowCrosshair = m_crosshairCheckbox.IsChecked;
			MySandboxGame.Config.EnableTrading = m_enableTradingCheckbox.IsChecked;
			MySandboxGame.Config.EnableSteamCloud = m_cloudCheckbox.IsChecked;
			MySandboxGame.Config.UIOpacity = m_UIOpacitySlider.Value;
			MySandboxGame.Config.UIBkOpacity = m_UIBkOpacitySlider.Value;
			MySandboxGame.Config.HUDBkOpacity = m_HUDBkOpacitySlider.Value;
			MyLanguage.CurrentLanguage = (MyLanguagesEnum)m_languageCombobox.GetSelectedKey();
			if (m_loadedLanguage != MyLanguage.CurrentLanguage)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextRestartNeededAfterLanguageSwitch), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
				MyScreenManager.RecreateControls();
			}
			MyCubeBuilder.BuildingMode = (MyCubeBuilder.BuildingModeEnum)m_buildingModeCombobox.GetSelectedKey();
			MySandboxGame.Config.ControlsHints = m_controlHintsCheckbox.IsChecked;
			if (MyFakes.ENABLE_SIMPLE_NEWGAME_SCREEN)
			{
				MySandboxGame.Config.EnableNewNewGameScreen = m_enableNewNewGameScreen.IsChecked;
			}
			MySandboxGame.Config.GoodBotHints = m_goodbotHintsCheckbox.IsChecked;
			if (m_rotationHintsCheckbox != null)
			{
				MySandboxGame.Config.RotationHints = m_rotationHintsCheckbox.IsChecked;
			}
			if (MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.RegisterAlphaMultiplier(VisualStyleCategory.Background, m_HUDBkOpacitySlider.Value);
			}
			MySandboxGame.Config.Save();
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			DoChanges();
			CloseScreen();
		}
	}
}
