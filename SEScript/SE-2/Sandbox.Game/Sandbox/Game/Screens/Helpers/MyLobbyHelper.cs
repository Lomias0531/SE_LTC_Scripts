using System;
using System.Timers;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyLobbyHelper
	{
		private IMyLobby m_lobby;

		private MyLobbyDataUpdated m_dataUpdateHandler;

		public event Action<IMyLobby, bool> OnSuccess;

		public MyLobbyHelper(IMyLobby lobby)
		{
			m_lobby = lobby;
			m_dataUpdateHandler = JoinGame_LobbyUpdate;
		}

		private void t_Elapsed(object sender, ElapsedEventArgs e)
		{
			m_lobby.OnDataReceived -= m_dataUpdateHandler;
		}

		public bool RequestData()
		{
			m_lobby.OnDataReceived += m_dataUpdateHandler;
			if (!m_lobby.RequestData())
			{
				m_lobby.OnDataReceived -= m_dataUpdateHandler;
				return false;
			}
			return true;
		}

		public void Cancel()
		{
			m_lobby.OnDataReceived -= m_dataUpdateHandler;
		}

		private void JoinGame_LobbyUpdate(bool success, IMyLobby lobby, ulong memberOrLobby)
		{
			if (lobby.LobbyId == m_lobby.LobbyId)
			{
				m_lobby.OnDataReceived -= m_dataUpdateHandler;
				this.OnSuccess?.Invoke(lobby, success);
			}
		}
	}
}
