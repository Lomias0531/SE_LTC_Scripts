using System;
using System.Collections.Generic;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
	[PreloadRequired]
	public class MyCommandConsole : MyCommand
	{
		static MyCommandConsole()
		{
			MyConsole.AddCommand(new MyCommandConsole());
		}

		public override string Prefix()
		{
			return "Console";
		}

		private MyCommandConsole()
		{
			m_methods.Add("Clear", new MyCommandAction
			{
				Parser = ((List<string> x) => null),
				CallAction = ((MyCommandArgs x) => ClearConsole(x))
			});
			m_methods.Add("Script", new MyCommandAction
			{
				Parser = ((List<string> x) => null),
				CallAction = ScriptConsole
			});
		}

		private StringBuilder ScriptConsole(MyCommandArgs x)
		{
			return new StringBuilder("Scripting mode. Send blank line to compile and run.");
		}

		private StringBuilder ClearConsole(MyCommandArgs args)
		{
			MyConsole.Clear();
			return new StringBuilder("Console cleared...");
		}
	}
}
