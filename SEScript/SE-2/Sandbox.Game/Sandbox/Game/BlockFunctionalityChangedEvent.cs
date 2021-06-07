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
	public delegate void BlockFunctionalityChangedEvent(long entityId, long gridId, string enitytName, string gridName, string typeId, string subtypeId, bool BecameFunctional);
}
