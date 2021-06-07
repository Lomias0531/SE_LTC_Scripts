using System;
using System.Collections.Generic;
using System.Text;
using VRage;

namespace Sandbox.Game.GUI
{
	[PreloadRequired]
	public class MyCommandCharacter : MyCommand
	{
		private class MyCommandArgsValuesList : MyCommandArgs
		{
			public List<int> values;
		}

		public override string Prefix()
		{
			return "Character";
		}

		static MyCommandCharacter()
		{
			MyConsole.AddCommand(new MyCommandCharacter());
		}

		private MyCommandCharacter()
		{
			m_methods.Add("AddSomeValues", new MyCommandAction
			{
				AutocompleteHint = new StringBuilder("int_val1 int_val2 ..."),
				Parser = ((List<string> x) => ParseValues(x)),
				CallAction = ((MyCommandArgs x) => PassValuesToCharacter(x))
			});
		}

		private MyCommandArgs ParseValues(List<string> args)
		{
			MyCommandArgsValuesList myCommandArgsValuesList = new MyCommandArgsValuesList();
			myCommandArgsValuesList.values = new List<int>();
			foreach (string arg in args)
			{
				myCommandArgsValuesList.values.Add(int.Parse(arg));
			}
			return myCommandArgsValuesList;
		}

		private StringBuilder PassValuesToCharacter(MyCommandArgs args)
		{
			MyCommandArgsValuesList myCommandArgsValuesList = args as MyCommandArgsValuesList;
			if (myCommandArgsValuesList.values.Count == 0)
			{
				return new StringBuilder("No values passed onto character");
			}
			foreach (int value in myCommandArgsValuesList.values)
			{
				_ = value;
			}
			StringBuilder stringBuilder = new StringBuilder().Append("Added values ");
			foreach (int value2 in myCommandArgsValuesList.values)
			{
				stringBuilder.Append(value2).Append(" ");
			}
			stringBuilder.Append("to character");
			return stringBuilder;
		}
	}
}
