using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		true
	})]
	public delegate void GridJumpedEvent(long playerId, string gridName, long gridId);
}
