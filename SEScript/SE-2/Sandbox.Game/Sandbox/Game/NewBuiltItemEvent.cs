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
		false
	})]
	public delegate void NewBuiltItemEvent(long entityId, long gridId, string entityName, string gridName, string ItemTypeName, string itemSubTypeName, int amount);
}
