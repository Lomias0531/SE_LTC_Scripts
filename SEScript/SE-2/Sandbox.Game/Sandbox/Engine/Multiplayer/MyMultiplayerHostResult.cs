using Sandbox.Engine.Networking;
using System;
using System.Threading;
using VRage.GameServices;

namespace Sandbox.Engine.Multiplayer
{
	public class MyMultiplayerHostResult
	{
		private bool m_done;

		public bool Cancelled
		{
			get;
			private set;
		}

		public bool Success
		{
			get;
			private set;
		}

		public MyLobbyStatusCode StatusCode
		{
			get;
			private set;
		}

		public event Action<bool, MyLobbyStatusCode, MyMultiplayerBase> Done;

		public void Cancel()
		{
			Cancelled = true;
		}

		public void RaiseDone(bool success, MyLobbyStatusCode reason, MyMultiplayerBase multiplayer)
		{
			Success = success;
			StatusCode = reason;
			this.Done?.Invoke(success, reason, multiplayer);
			m_done = true;
		}

		public void Wait(bool runCallbacks = true)
		{
			while (!Cancelled && !m_done)
			{
				if (runCallbacks)
				{
					MyGameService.Update();
				}
				Thread.Sleep(10);
			}
		}
	}
}
