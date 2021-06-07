using Sandbox.Game.Entities.Cube;

namespace Sandbox.Game.Gui
{
	internal delegate TerminalControl FactoryDelegate<in T>(T property, MyTerminalBlock[] blocks);
}
