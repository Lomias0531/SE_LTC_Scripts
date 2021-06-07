using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	public static class MyClientStateExtensions
	{
		public static MyNetworkClient GetClient(this MyClientStateBase state)
		{
			if (state == null)
			{
				return null;
			}
			Sync.Clients.TryGetClient(state.EndpointId.Id.Value, out MyNetworkClient client);
			return client;
		}

		public static MyPlayer GetPlayer(this MyClientStateBase state)
		{
			return state.GetClient()?.FirstPlayer;
		}
	}
}
