using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems.Chat
{
	public interface IMyChatCommand
	{
		string CommandText
		{
			get;
		}

		string HelpText
		{
			get;
		}

		string HelpSimpleText
		{
			get;
		}

		MyPromoteLevel VisibleTo
		{
			get;
		}

		void Handle(string[] args);
	}
}
