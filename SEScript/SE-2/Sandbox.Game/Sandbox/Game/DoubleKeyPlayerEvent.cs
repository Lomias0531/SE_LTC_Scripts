using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		false
	})]
	public delegate void DoubleKeyPlayerEvent(string entityName, long playerId, string gridName);
}
