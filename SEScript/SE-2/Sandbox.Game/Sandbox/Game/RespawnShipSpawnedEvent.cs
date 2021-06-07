using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		false
	})]
	public delegate void RespawnShipSpawnedEvent(long shipEntityId, long playerId, string respawnShipPrefabName);
}
