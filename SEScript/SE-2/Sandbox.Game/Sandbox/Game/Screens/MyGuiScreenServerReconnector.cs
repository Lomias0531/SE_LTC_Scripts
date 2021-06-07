using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
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
	internal class MyGuiScreenServerReconnector : MyGuiScreenBase
	{
		private enum MyReconnectionState
		{
			IDLE,
			RETRY,
			WAITING,
			CONNECTING,
			RETRY_DELAY
		}

		private string m_address = string.Empty;

		private ushort m_port;

		private static int WAIT = 300;

		private static int WAIT_RETRY_DELAY = 75;

		private int m_counter = 600;

		private MyReconnectionState m_state;

		private MyReconnectionState m_stateLast;

		private int m_timeToReconnect;

		private int m_timeToReconnectLastFrame;

		private MyGuiControlMultilineText m_messageBoxText;

		private MyGuiControlLabel m_reconnectingCaption;

		public static MyGuiScreenServerReconnector ReconnectToLastSession()
		{
			MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
			if (lastSession == null)
			{
				return null;
			}
			if (lastSession.IsOnline)
			{
				if (lastSession.IsLobby)
				{
					MyJoinGameHelper.JoinGame(ulong.Parse(lastSession.ServerIP));
					return null;
				}
				string serverIP = lastSession.ServerIP;
				ushort port = (ushort)lastSession.ServerPort;
				MyGuiScreenServerReconnector myGuiScreenServerReconnector = new MyGuiScreenServerReconnector(serverIP, port);
				MyGuiSandbox.AddScreen(myGuiScreenServerReconnector);
				return myGuiScreenServerReconnector;
			}
			return null;
		}

		public MyGuiScreenServerReconnector(string address, ushort port)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR)
		{
			base.CanHideOthers = true;
			m_drawEvenWithoutFocus = true;
			m_address = address;
			m_port = port;
			m_state = MyReconnectionState.RETRY;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_size = new Vector2(0.35f, 0.3f);
			new Vector2(0.1f, 0.1f);
			new Vector2(0.05f, 0.05f);
			MyGuiControlLabel control = new MyGuiControlLabel(new Vector2(0f, -0.13f), null, MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Caption), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control);
			MyGuiControlLabel control2 = new MyGuiControlLabel(new Vector2(0f, -0.07f), null, MyTexts.GetString(MyCommonTexts.MultiplayerErrorServerHasLeft), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(control2);
			m_reconnectingCaption = new MyGuiControlLabel(new Vector2(0f, -0.01f), null, string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), 0), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			Controls.Add(m_reconnectingCaption);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.15f, -0.09f), 0.3f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlButton myGuiControlButton = null;
			Controls.Add(myGuiControlButton = MakeButton(new Vector2(0f, 0.12f), MyCommonTexts.Cancel, OnCancelClick, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM));
		}

		public override bool Update(bool hasFocus)
		{
			base.Update(hasFocus);
			switch (m_state)
			{
			case MyReconnectionState.RETRY:
				if (m_stateLast != m_state)
				{
					m_reconnectingCaption.Text = string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), m_timeToReconnect);
				}
				m_counter--;
				m_timeToReconnect = m_counter / 60;
				if (m_timeToReconnectLastFrame != m_timeToReconnect)
				{
					m_reconnectingCaption.Text = string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), m_timeToReconnect);
				}
				m_timeToReconnectLastFrame = m_timeToReconnect;
				if (m_counter < 0)
				{
					m_counter = WAIT;
					try
					{
						IPAddress[] hostAddresses = Dns.GetHostAddresses(m_address);
						MyGameService.OnPingServerResponded += ServerResponded_Ping;
						MyGameService.OnPingServerFailedToRespond += ServerFailedToRespond_Ping;
						MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), m_port);
						m_state = MyReconnectionState.WAITING;
					}
					catch (Exception ex)
					{
						MyLog.Default.WriteLine(ex);
						MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
					}
				}
				break;
			case MyReconnectionState.WAITING:
				m_reconnectingCaption.Text = MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_ReconnectionInProgress);
				break;
			case MyReconnectionState.RETRY_DELAY:
				if (m_counter > 0)
				{
					m_counter--;
					break;
				}
				m_state = MyReconnectionState.RETRY;
				m_counter = WAIT;
				break;
			}
			m_stateLast = m_state;
			return true;
		}

		public void ServerResponded_Ping(object sender, MyGameServerItem serverItem)
		{
			if (m_state == MyReconnectionState.WAITING)
			{
				m_state = MyReconnectionState.CONNECTING;
				MyLog.Default.WriteLineAndConsole("Server responded");
				CloseHandlers_Ping();
				MyJoinGameHelper.JoinGame(serverItem, enableGuiBackgroundFade: false);
			}
		}

		public void ServerFailedToRespond_Ping(object sender, object e)
		{
			MyLog.Default.WriteLineAndConsole("Server failed to respond");
			CloseHandlers_Ping();
			m_reconnectingCaption.Text = MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_ServerNoResponse);
			m_state = MyReconnectionState.RETRY_DELAY;
			m_counter = WAIT_RETRY_DELAY;
		}

		private void CloseHandlers_Ping()
		{
			MyGameService.OnPingServerResponded -= ServerResponded_Ping;
			MyGameService.OnPingServerFailedToRespond -= ServerFailedToRespond_Ping;
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum align)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, align, null, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			m_state = MyReconnectionState.IDLE;
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenServerReconnector";
		}
	}
}
