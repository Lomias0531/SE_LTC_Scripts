using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class ValueGetScreenWithCaption : MyGuiScreenBase
	{
		public delegate bool ValueGetScreenAction(string valueText);

		private MyGuiControlTextbox m_nameTextbox;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private string m_title;

		private string m_caption;

		private ValueGetScreenAction m_acceptCallback;

		public ValueGetScreenWithCaption(string title, string caption, ValueGetScreenAction ValueAcceptedCallback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			m_acceptCallback = ValueAcceptedCallback;
			m_title = title;
			m_caption = caption;
			m_canShareInput = false;
			m_isTopMostScreen = true;
			m_isTopScreen = true;
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "ValueGetScreenWithCaption";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, m_title, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_nameTextbox = new MyGuiControlTextbox(new Vector2(0f, 0f), m_caption);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Confirm));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel));
			Controls.Add(m_nameTextbox);
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
			if (m_acceptCallback(m_nameTextbox.Text))
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
