using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDialogRemoveTriangle : MyGuiScreenBase
	{
		private MyGuiControlTextbox m_textbox;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		public MyGuiScreenDialogRemoveTriangle()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogRemoveTriangle";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, "Enter the number of a navmesh triangle to remove", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_textbox = new MyGuiControlTextbox(new Vector2(0.2f, 0f), null, 512, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			Controls.Add(m_textbox);
			Controls.Add(m_confirmButton);
			Controls.Add(m_cancelButton);
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			Convert.ToInt32(m_textbox.Text);
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
