using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ChatCommandAttribute : Attribute
	{
		public string CommandText;

		public string HelpText;

		public string HelpSimpleText;

		public MyPromoteLevel VisibleTo;

		internal bool DebugCommand;

		public ChatCommandAttribute(string commandText, string helpText, string helpSimpleText, MyPromoteLevel visibleTo = MyPromoteLevel.None)
		{
			CommandText = commandText;
			HelpText = helpText;
			HelpSimpleText = helpSimpleText;
			VisibleTo = visibleTo;
		}

		internal ChatCommandAttribute(string commandText, string helpText, string helpSimpleText, bool debugCommand, MyPromoteLevel visibleTo = MyPromoteLevel.None)
		{
			CommandText = commandText;
			HelpText = helpText;
			HelpSimpleText = helpSimpleText;
			DebugCommand = debugCommand;
			VisibleTo = visibleTo;
		}
	}
}
