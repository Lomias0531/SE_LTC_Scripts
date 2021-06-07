using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlScreenSwitchPanel : MyGuiControlParent
	{
		private List<Action> m_screenSwithingActions = new List<Action>();

		private List<bool> m_screenEnabled = new List<bool>();

		private int m_activeScreenIndex = -1;

		public MyGuiControlScreenSwitchPanel(MyGuiScreenBase owner, StringBuilder ownerDescription, bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
		{
			m_screenSwithingActions.Add(OpenCampaignScreen);
			m_screenSwithingActions.Add(OpenWorkshopScreen);
			m_screenSwithingActions.Add(OpenCustomWorldScreen);
			m_screenEnabled.Add(displayTabScenario);
			m_screenEnabled.Add(displayTabWorkshop);
			m_screenEnabled.Add(displayTabCustom);
			Vector2 value = new Vector2(0.002f, 0.05f);
			Vector2 vector = new Vector2(50f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlMultilineText control = new MyGuiControlMultilineText
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(0.002f, 0.13f),
				Size = new Vector2(owner.Size.Value.X - 0.1f, 0.05f),
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Text = ownerDescription,
				Font = "Blue"
			};
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionNewGame), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Campaign), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnCampaignButtonClick)
			{
				CanHaveFocus = false
			};
			myGuiControlButton.Enabled = displayTabScenario;
			value.X += myGuiControlButton.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_WorkshopContent), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnWorkshopButtonClick)
			{
				CanHaveFocus = false
			};
			myGuiControlButton2.Enabled = displayTabWorkshop;
			value.X += myGuiControlButton2.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
			MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionCustomWorld), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_CustomGame), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnCustomWorldButtonClick)
			{
				CanHaveFocus = false
			};
			myGuiControlButton3.Enabled = displayTabCustom;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.0305f), owner.Size.Value.X - 2f * vector2.X);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.098f), owner.Size.Value.X - 2f * vector2.X);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.166f), owner.Size.Value.X - 2f * vector2.X);
			if (owner is MyGuiScreenNewGame)
			{
				owner.FocusedControl = myGuiControlButton;
				myGuiControlButton.HighlightType = MyGuiControlHighlightType.FORCED;
				myGuiControlButton.HasHighlight = true;
				myGuiControlButton.Selected = true;
				myGuiControlButton.Name = "CampaignButton";
				m_activeScreenIndex = 0;
			}
			else if (owner is MyGuiScreenWorldSettings)
			{
				owner.FocusedControl = myGuiControlButton3;
				myGuiControlButton3.HighlightType = MyGuiControlHighlightType.FORCED;
				myGuiControlButton3.HasHighlight = true;
				myGuiControlButton3.Selected = true;
				m_activeScreenIndex = 2;
			}
			else if ((owner is MyGuiScreenLoadSubscribedWorld || owner is MyGuiScreenNewWorkshopGame) && myGuiControlButton2 != null)
			{
				owner.FocusedControl = myGuiControlButton2;
				myGuiControlButton2.HighlightType = MyGuiControlHighlightType.FORCED;
				myGuiControlButton2.HasHighlight = true;
				myGuiControlButton2.Selected = true;
				m_activeScreenIndex = 1;
			}
			base.Controls.Add(control);
			base.Controls.Add(myGuiControlSeparatorList);
			base.Controls.Add(myGuiControlButton);
			base.Controls.Add(myGuiControlButton3);
			if (myGuiControlButton2 != null)
			{
				base.Controls.Add(myGuiControlButton2);
			}
			base.Position = -owner.Size.Value / 2f + new Vector2(vector2.X, vector.Y);
			base.Size = new Vector2(1f, 0.2f);
			owner.Controls.Add(this);
		}

		private void OnCampaignButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenCampaignScreen();
		}

		private void OpenCampaignScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenNewGame))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenNewGame());
			}
		}

		private void OnCustomWorldButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenCustomWorldScreen();
		}

		private void OpenCustomWorldScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenWorldSettings))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenWorldSettings());
			}
		}

		private void OnWorkshopButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenWorkshopScreen();
		}

		private void OpenWorkshopScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenNewWorkshopGame))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenNewWorkshopGame());
			}
		}

		private static void SeamlesslyChangeScreen(MyGuiScreenBase focusedScreen, MyGuiScreenBase exchangedFor)
		{
			focusedScreen.SkipTransition = true;
			focusedScreen.CloseScreen();
			exchangedFor.SkipTransition = true;
			MyScreenManager.AddScreenNow(exchangedFor);
		}

		private void SwitchToNextScreen(bool positiveDirection = true)
		{
			if (m_activeScreenIndex < 0 || m_activeScreenIndex >= m_screenSwithingActions.Count)
			{
				return;
			}
			if (positiveDirection)
			{
				int num = m_activeScreenIndex;
				bool flag = false;
				do
				{
					num = (num + 1) % m_screenSwithingActions.Count;
					if (m_screenEnabled[num])
					{
						flag = true;
						break;
					}
				}
				while (num != m_activeScreenIndex);
				if (flag)
				{
					m_screenSwithingActions[num]();
				}
				return;
			}
			int num2 = m_activeScreenIndex;
			bool flag2 = false;
			do
			{
				num2 = (num2 + m_screenSwithingActions.Count - 1) % m_screenSwithingActions.Count;
				if (m_screenEnabled[num2])
				{
					flag2 = true;
					break;
				}
			}
			while (num2 != m_activeScreenIndex);
			if (flag2)
			{
				m_screenSwithingActions[num2]();
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
			{
				SwitchToNextScreen(positiveDirection: false);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				SwitchToNextScreen();
			}
			return base.HandleInput();
		}
	}
}
