using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class Vector3GetScreenWithCaption : MyGuiScreenBase
	{
		public delegate bool Vector3GetScreenAction(string value1, string value2, string value3);

		private MyGuiControlTextbox m_nameTextbox1;

		private MyGuiControlTextbox m_nameTextbox2;

		private MyGuiControlTextbox m_nameTextbox3;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private string m_title;

		private string m_caption1;

		private string m_caption2;

		private string m_caption3;

		private Vector3GetScreenAction m_acceptCallback;

		public Vector3GetScreenWithCaption(string title, string caption1, string caption2, string caption3, Vector3GetScreenAction ValueAcceptedCallback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_acceptCallback = ValueAcceptedCallback;
			m_title = title;
			m_caption1 = caption1;
			m_caption2 = caption2;
			m_caption3 = caption3;
			m_canShareInput = false;
			m_isTopMostScreen = true;
			m_isTopScreen = true;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "Vector3GetScreenWithCaption";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, m_title, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			float num = 0f;
			float num2 = 0.04f;
			m_nameTextbox1 = new MyGuiControlTextbox(new Vector2(0f - num, 0f - num2), m_caption1);
			m_nameTextbox2 = new MyGuiControlTextbox(new Vector2(0f, 0f), m_caption2);
			m_nameTextbox3 = new MyGuiControlTextbox(new Vector2(0f + num, 0f + num2), m_caption3);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Confirm));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel));
			Controls.Add(m_nameTextbox1);
			Controls.Add(m_nameTextbox2);
			Controls.Add(m_nameTextbox3);
			Controls.Add(m_confirmButton);
			Controls.Add(m_cancelButton);
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsKeyPress(MyKeys.Enter))
			{
				confirmButton_OnButtonClick(m_confirmButton);
			}
			if (MyInput.Static.IsKeyPress(MyKeys.Escape))
			{
				cancelButton_OnButtonClick(m_cancelButton);
			}
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (m_acceptCallback(m_nameTextbox1.Text, m_nameTextbox2.Text, m_nameTextbox3.Text))
			{
				CloseScreen();
			}
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
