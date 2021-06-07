using System;
using VRage.GameServices;

namespace Sandbox.Engine.Multiplayer
{
	public class MyMultiplayerJoinResult
	{
		public bool Cancelled
		{
			get;
			private set;
		}

		public event Action<bool, IMyLobby, MyLobbyStatusCode, MyMultiplayerBase> JoinDone;

		public void Cancel()
		{
			Cancelled = true;
		}

		public void RaiseJoined(bool success, IMyLobby lobby, MyLobbyStatusCode response, MyMultiplayerBase multiplayer)
		{
			this.JoinDone?.Invoke(success, lobby, response, multiplayer);
		}
	}
}
