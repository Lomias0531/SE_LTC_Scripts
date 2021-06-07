using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		true,
		true,
		true,
		true
	})]
	public delegate void RemoteControlChangedEvent(bool GotControlled, long playerId, string entityName, long entityId, string gridName, long gridId);
}
