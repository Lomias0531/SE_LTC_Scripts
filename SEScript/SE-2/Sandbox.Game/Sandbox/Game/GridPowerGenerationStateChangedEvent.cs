using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		false
	})]
	public delegate void GridPowerGenerationStateChangedEvent(long gridId, string gridName, bool IsPowered);
}
