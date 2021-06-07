using Sandbox.ModAPI.Ingame;
using System.Text;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Interfaces
{
	public interface ITerminalAction
	{
		string Id
		{
			get;
		}

		string Icon
		{
			get;
		}

		StringBuilder Name
		{
			get;
		}

		void Apply(IMyCubeBlock block);

		void Apply(IMyCubeBlock block, ListReader<TerminalActionParameter> terminalActionParameters);

		void WriteValue(IMyCubeBlock block, StringBuilder appendTo);

		bool IsEnabled(IMyCubeBlock block);
	}
}
