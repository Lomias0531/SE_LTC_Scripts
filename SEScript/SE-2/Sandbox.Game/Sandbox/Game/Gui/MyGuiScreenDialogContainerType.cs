using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenDialogContainerType : MyGuiScreenBase
	{
		private MyGuiControlTextbox m_containerTypeTextbox;

		private MyGuiControlButton m_confirmButton;

		private MyGuiControlButton m_cancelButton;

		private MyCargoContainer m_container;

		public MyGuiScreenDialogContainerType(MyCargoContainer container)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			m_container = container;
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogContainerType";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, -0.1f), null, "Type the container type name for this cargo container:", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_containerTypeTextbox = new MyGuiControlTextbox(new Vector2(0f, 0f), m_container.ContainerType, 100);
			m_confirmButton = new MyGuiControlButton(new Vector2(0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Confirm"));
			m_cancelButton = new MyGuiControlButton(new Vector2(-0.21f, 0.1f), MyGuiControlButtonStyleEnum.Default, new Vector2(0.2f, 0.05f), null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder("Cancel"));
			Controls.Add(m_containerTypeTextbox);
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
			if (m_containerTypeTextbox.Text != null && m_containerTypeTextbox.Text != "")
			{
				m_container.ContainerType = m_containerTypeTextbox.Text;
			}
			else
			{
				m_container.ContainerType = null;
			}
			CloseScreen();
		}

		private void cancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
