using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenServerPassword : MyGuiScreenBase
	{
		private readonly float _padding = 0.02f;

		private MyGuiControlTextbox m_passwordTextbox;

		private Action<string> m_connectAction;

		public MyGuiScreenServerPassword(Action<string> connectAction)
			: base(new Vector2(0.5f, 0.75f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 0.1908397f), isTopMostScreen: true)
		{
			m_connectAction = connectAction;
			CreateScreen();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = false;
			base.CanBeHidden = false;
			base.EnabledBackgroundFade = false;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			AddCaption(MyCommonTexts.MultiplayerEnterPassword, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.05f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			m_passwordTextbox = new MyGuiControlTextbox(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.105f), string.Empty);
			m_passwordTextbox.Size = new Vector2(m_passwordTextbox.Size.X / 1.33f, m_passwordTextbox.Size.Y);
			m_passwordTextbox.PositionX += m_passwordTextbox.Size.X / 2f;
			m_passwordTextbox.EnterPressed += AddressEnterPressed;
			m_passwordTextbox.FocusChanged += AddressFocusChanged;
			m_passwordTextbox.SetToolTip(MyTexts.GetString(MyCommonTexts.MultiplayerEnterPassword));
			m_passwordTextbox.Type = MyGuiControlTextboxType.Password;
			m_passwordTextbox.MoveCarriageToEnd();
			Controls.Add(m_passwordTextbox);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(m_passwordTextbox.PositionX + m_passwordTextbox.Size.X / 2f, m_passwordTextbox.PositionY + 0.007f), MyGuiControlButtonStyleEnum.ComboBoxButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.MultiplayerJoinConnect));
			myGuiControlButton.PositionX += myGuiControlButton.Size.X / 2f + _padding * 0.66f;
			myGuiControlButton.ButtonClicked += ConnectButtonClick;
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
			Controls.Add(myGuiControlButton);
		}

		private void AddressEnterPressed(MyGuiControlTextbox obj)
		{
			ConnectButtonClick(null);
		}

		private void AddressFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused)
			{
				m_passwordTextbox.SelectAll();
				m_passwordTextbox.MoveCarriageToEnd();
			}
		}

		private void ConnectButtonClick(MyGuiControlButton obj)
		{
			CloseScreen();
			if (m_connectAction != null)
			{
				m_connectAction(m_passwordTextbox.Text);
			}
		}

		public override string GetFriendlyName()
		{
			return "ServerPassword";
		}
	}
}
