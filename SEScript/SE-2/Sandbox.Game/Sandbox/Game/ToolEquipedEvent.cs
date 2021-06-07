using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true
	})]
	public delegate void ToolEquipedEvent(long playerId, string typeId, string subtypeId);
}
