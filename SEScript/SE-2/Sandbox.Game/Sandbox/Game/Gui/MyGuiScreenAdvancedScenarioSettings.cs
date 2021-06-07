using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenAdvancedScenarioSettings : MyGuiScreenBase
	{
		private MyGuiScreenMissionTriggers m_parent;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlCheckbox m_canJoinRunning;

		public event Action OnOkButtonClicked;

		public MyGuiScreenAdvancedScenarioSettings(MyGuiScreenMissionTriggers parent)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.9f, 0.9f))
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedScenarioSettings.ctor START");
			m_parent = parent;
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
			MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedScenarioSettings.ctor END");
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
		}

		public void BuildControls()
		{
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent(null, new Vector2(base.Size.Value.X - 0.05f, base.Size.Value.Y - 0.1f));
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent)
			{
				ScrollbarVEnabled = true,
				Size = new Vector2(base.Size.Value.X - 0.05f, 0.8f)
			};
			Controls.Add(myGuiControlScrollablePanel);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			Vector2 value = m_size.Value / 2f - new Vector2(0.23f, 0.03f);
			m_okButton = new MyGuiControlButton(value - new Vector2(0.01f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OkButtonClicked);
			m_cancelButton = new MyGuiControlButton(value + new Vector2(0.01f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, CancelButtonClicked);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			float num = 0.055f;
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MySpaceTexts.ScenarioSettings_CanJoinRunning);
			m_canJoinRunning = new MyGuiControlCheckbox();
			m_canJoinRunning.Position = new Vector2((0f - myGuiControlScrollablePanel.Size.X) / 2f + num, (0f - myGuiControlScrollablePanel.Size.Y) / 2f + num);
			myGuiControlLabel.Position = new Vector2(m_canJoinRunning.Position.X + num, m_canJoinRunning.Position.Y);
			m_canJoinRunning.IsChecked = MySession.Static.Settings.CanJoinRunning;
			myGuiControlParent.Controls.Add(m_canJoinRunning);
			myGuiControlParent.Controls.Add(myGuiControlLabel);
			base.CloseButtonEnabled = true;
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		private void CancelButtonClicked(object sender)
		{
			CloseScreen();
		}

		private void OkButtonClicked(object sender)
		{
			MySession.Static.Settings.CanJoinRunning = m_canJoinRunning.IsChecked;
			CloseScreen();
		}

		public void SetSettings(MyObjectBuilder_SessionSettings settings)
		{
		}

		public void GetSettings(MyObjectBuilder_SessionSettings settings)
		{
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAdvancedScenarioSettings";
		}
	}
}
