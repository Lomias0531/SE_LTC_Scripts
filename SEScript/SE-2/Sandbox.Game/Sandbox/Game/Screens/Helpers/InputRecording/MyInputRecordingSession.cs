using System.Reflection;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public enum MyInputRecordingSession
	{
		Specific,
		NewGame,
		MainMenu
	}
}
