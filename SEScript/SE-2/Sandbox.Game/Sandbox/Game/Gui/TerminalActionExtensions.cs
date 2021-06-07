using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Collections;

namespace Sandbox.Game.Gui
{
	public static class TerminalActionExtensions
	{
		public static Sandbox.ModAPI.Interfaces.ITerminalAction GetAction(this IMyTerminalBlock block, string name)
		{
			return block.GetActionWithName(name);
		}

		public static void ApplyAction(this IMyTerminalBlock block, string name)
		{
			block.GetAction(name).Apply(block);
		}

		public static void ApplyAction(this IMyTerminalBlock block, string name, ListReader<TerminalActionParameter> parameters)
		{
			block.GetAction(name).Apply(block, parameters);
		}
	}
}
