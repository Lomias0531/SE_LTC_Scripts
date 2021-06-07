using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		false,
		false
	})]
	public delegate void ButtonPanelEvent(string name, int button, long playerId, long blockId);
}
