using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true
	})]
	public delegate void CutsceneEvent(string cutsceneName);
}
