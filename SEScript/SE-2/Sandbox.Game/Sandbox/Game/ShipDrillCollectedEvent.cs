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
		true,
		true
	})]
	public delegate void ShipDrillCollectedEvent(string entityName, long entityId, string gridName, long gridId, string typeId, string subtypeId, float amount);
}
