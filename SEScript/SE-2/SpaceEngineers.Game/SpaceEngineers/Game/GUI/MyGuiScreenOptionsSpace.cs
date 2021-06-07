using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	internal class MyGuiScreenOptionsSpace : MyGuiScreenBase
	{
		private MyGuiControlElementGroup m_elementGroup;

		public MyGuiScreenOptionsSpace()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.5200382f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
			AddCaption(MyCommonTexts.ScreenCaptionOptions, null, new Vector2(0f, 0.003f));
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.05f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			Vector2 value = new Vector2(0.001f, (0f - m_size.Value.Y) / 2f + 0.126f);
			int num = 0;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenOptionsButtonGame), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenOptionsGame());
			});
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			if (!MyFakes.LIMITED_MAIN_MENU || MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenOptionsButtonDisplay), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenOptionsDisplay());
				});
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
				myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenOptionsButtonGraphics), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenOptionsGraphics());
				});
				Controls.Add(myGuiControlButton);
				m_elementGroup.Add(myGuiControlButton);
			}
			myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenOptionsButtonAudio), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenOptionsAudio());
			});
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenOptionsButtonControls), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenOptionsControls());
			});
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			myGuiControlButton = new MyGuiControlButton(value + num++ * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenMenuButtonCredits), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, delegate(MyGuiControlButton sender)
			{
				OnClickCredits(sender);
			});
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			base.CloseButtonEnabled = true;
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

		private void OnClickCredits(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenGameCredits());
		}

		protected override void OnShow()
		{
			base.OnShow();
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptions";
		}

		public void OnBackClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
