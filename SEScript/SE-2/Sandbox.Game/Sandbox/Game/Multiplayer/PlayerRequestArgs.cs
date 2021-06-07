using Sandbox.Game.World;

namespace Sandbox.Game.Multiplayer
{
	public class PlayerRequestArgs
	{
		public MyPlayer.PlayerId PlayerId;

		public bool Cancel;

		public PlayerRequestArgs(MyPlayer.PlayerId playerId)
		{
			PlayerId = playerId;
			Cancel = false;
		}
	}
}
