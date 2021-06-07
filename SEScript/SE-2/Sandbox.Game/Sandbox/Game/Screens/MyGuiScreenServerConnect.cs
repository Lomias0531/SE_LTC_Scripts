using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Net;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenServerConnect : MyGuiScreenBase
	{
		private readonly float _padding = 0.02f;

		private MyGuiControlTextbox m_addrTextbox;

		private MyGuiControlCheckbox m_favoriteCheckbox;

		private MyGuiScreenProgress m_progressScreen;

		public MyGuiScreenServerConnect()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 141f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			CreateScreen();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			AddCaption(MyCommonTexts.MultiplayerJoinDirectConnect, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.05f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(-new Vector2(m_size.Value.X * 0.385f, m_size.Value.Y / 2f - 0.116f), null, MyTexts.GetString(MyCommonTexts.JoinGame_Favorites_Add));
			Controls.Add(myGuiControlLabel);
			m_favoriteCheckbox = new MyGuiControlCheckbox(myGuiControlLabel.Position, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_favoriteCheckbox.PositionX += myGuiControlLabel.Size.X + 0.01f;
			m_favoriteCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameDirectConnect_Favorite));
			Controls.Add(m_favoriteCheckbox);
			m_addrTextbox = new MyGuiControlTextbox(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.17f), "0.0.0.0:27016");
			m_addrTextbox.Size = new Vector2(m_addrTextbox.Size.X / 1.33f, m_addrTextbox.Size.Y);
			m_addrTextbox.PositionX += m_addrTextbox.Size.X / 2f;
			m_addrTextbox.EnterPressed += AddressEnterPressed;
			m_addrTextbox.FocusChanged += AddressFocusChanged;
			m_addrTextbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameDirectConnect_IP));
			m_addrTextbox.MoveCarriageToEnd();
			Controls.Add(m_addrTextbox);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(m_addrTextbox.PositionX + m_addrTextbox.Size.X / 2f, m_addrTextbox.PositionY + 0.007f), MyGuiControlButtonStyleEnum.ComboBoxButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.MultiplayerJoinConnect));
			myGuiControlButton.PositionX += myGuiControlButton.Size.X / 2f + _padding * 0.66f;
			myGuiControlButton.ButtonClicked += ConnectButtonClick;
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
			Controls.Add(myGuiControlButton);
		}

		private void AddressEnterPressed(MyGuiControlTextbox obj)
		{
			ParseIPAndConnect();
		}

		private void AddressFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused)
			{
				m_addrTextbox.SelectAll();
				m_addrTextbox.MoveCarriageToEnd();
			}
		}

		private void ConnectButtonClick(MyGuiControlButton obj)
		{
			ParseIPAndConnect();
		}

		private void ParseIPAndConnect()
		{
			try
			{
				string[] array = m_addrTextbox.Text.Trim().Split(new char[1]
				{
					':'
				});
				ushort num = (ushort)((array.Length >= 2) ? ushort.Parse(array[1]) : 27016);
				IPAddress[] hostAddresses = Dns.GetHostAddresses(array[0]);
				if (m_favoriteCheckbox.IsChecked)
				{
					MyGameService.AddFavoriteGame(hostAddresses[0].ToIPv4NetworkOrder(), num, num);
				}
				StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
				m_progressScreen = new MyGuiScreenProgress(text, MyCommonTexts.Cancel, isTopMostScreen: false);
				MyGuiSandbox.AddScreen(m_progressScreen);
				m_progressScreen.ProgressCancelled += delegate
				{
					CloseHandlers();
					MySessionLoader.UnloadAndExitToMenu();
				};
				MyGameService.OnPingServerResponded += ServerResponded;
				MyGameService.OnPingServerFailedToRespond += ServerFailedToRespond;
				MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), num);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
			}
		}

		private void ServerResponded(object sender, MyGameServerItem serverItem)
		{
			CloseHandlers();
			m_progressScreen.CloseScreen();
			MyJoinGameHelper.JoinGame(serverItem);
		}

		private void ServerFailedToRespond(object sender, object e)
		{
			CloseHandlers();
			m_progressScreen.CloseScreen();
			MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding);
		}

		private void CloseHandlers()
		{
			MyGameService.OnPingServerResponded -= ServerResponded;
			MyGameService.OnPingServerFailedToRespond -= ServerFailedToRespond;
		}

		public override bool CloseScreen()
		{
			CloseHandlers();
			if (m_progressScreen != null)
			{
				m_progressScreen.CloseScreen();
			}
			return base.CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "ServerConnect";
		}
	}
}
