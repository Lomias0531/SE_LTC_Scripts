using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true
	})]
	public delegate void SingleKeyEntityNameGridNameEvent(string entityName, string gridName, string typeId, string subtypeId);
}
