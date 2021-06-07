using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		false
	})]
	public delegate void SingleKeyPlayerEvent(long playerId);
}
