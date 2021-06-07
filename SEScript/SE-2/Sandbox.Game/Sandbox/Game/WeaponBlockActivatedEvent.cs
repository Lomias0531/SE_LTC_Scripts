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
		false
	})]
	public delegate void WeaponBlockActivatedEvent(long entityId, long gridId, string entityName, string gridName, string blockType, string blockSubtype);
}
