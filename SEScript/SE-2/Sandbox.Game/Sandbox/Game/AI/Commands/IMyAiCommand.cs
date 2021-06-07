using Sandbox.Definitions;

namespace Sandbox.Game.AI.Commands
{
	public interface IMyAiCommand
	{
		void InitCommand(MyAiCommandDefinition definition);

		void ActivateCommand();
	}
}
