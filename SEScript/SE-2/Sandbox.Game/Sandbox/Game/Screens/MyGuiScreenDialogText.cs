using Sandbox.Graphics.GUI;
using System;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenDialogText : MyGuiScreenBase
	{
		private MyGuiControlLabel m_captionLabel;

		private MyGuiControlTextbox m_valueTextbox;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyStringId m_caption;

		private readonly string m_value;

		public event Action<string> OnConfirmed;

		public event Action<string> OnCancelled;

		public MyGuiScreenDialogText(string initialValue = null, MyStringId? caption = null, bool isTopMostScreen = false)
			: base(null, null, null, isTopMostScreen)
		{
			m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
			m_guiTransition = MySandboxGame.Config.UIOpacity;
			m_value = (initialValue ?? string.Empty);
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			m_caption = (caption ?? MyCommonTexts.DialogAmount_SetValueCaption);
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogText";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			string path = MyGuiScreenBase.MakeScreenFilepath("DialogText");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			m_valueTextbox = (MyGuiControlTextbox)Controls.GetControlByName("ValueTextbox");
			m_confirmButton = (MyGuiControlButton)Controls.GetControlByName("ConfirmButton");
			m_cancelButton = (MyGuiControlButton)Controls.GetControlByName("CancelButton");
			m_captionLabel = (MyGuiControlLabel)Controls.GetControlByName("CaptionLabel");
			m_captionLabel.Text = null;
			m_captionLabel.TextEnum = m_caption;
			m_confirmButton.ButtonClicked += confirmButton_OnButtonClick;
			m_cancelButton.ButtonClicked += cancelButton_OnButtonClick;
			m_valueTextbox.Text = m_value;
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
			{
				confirmButton_OnButtonClick(m_confirmButton);
			}
		}

		public override bool CloseScreen()
		{
			if (base.Cancelled)
			{
				this.OnCancelled.InvokeIfNotNull(m_valueTextbox.Text);
			}
			else
			{
				this.OnConfirmed.InvokeIfNotNull(m_valueTextbox.Text);
			}
			return base.CloseScreen();
		}

		private void confirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
