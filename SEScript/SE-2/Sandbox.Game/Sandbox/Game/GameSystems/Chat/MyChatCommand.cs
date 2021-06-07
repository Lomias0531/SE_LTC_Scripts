using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
	internal class MyChatCommand : IMyChatCommand
	{
		private readonly Action<string[]> m_action;

		public string CommandText
		{
			get;
			private set;
		}

		public string HelpText
		{
			get;
			private set;
		}

		public string HelpSimpleText
		{
			get;
			private set;
		}

		public MyPromoteLevel VisibleTo
		{
			get;
			private set;
		}

		public void Handle(string[] args)
		{
			m_action(args);
		}

		public MyChatCommand(string commandText, string helpText, string helpSimpleText, Action<string[]> action, MyPromoteLevel visibleTo = MyPromoteLevel.None)
		{
			CommandText = commandText;
			HelpText = helpText;
			HelpSimpleText = helpSimpleText;
			m_action = action;
			VisibleTo = visibleTo;
		}
	}
}
