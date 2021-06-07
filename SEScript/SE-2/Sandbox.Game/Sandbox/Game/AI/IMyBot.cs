using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using VRage.Game;

namespace Sandbox.Game.AI
{
	public interface IMyBot
	{
		bool IsValidForUpdate
		{
			get;
		}

		bool CreatedByPlayer
		{
			get;
		}

		string BehaviorSubtypeName
		{
			get;
		}

		ActionCollection ActionCollection
		{
			get;
		}

		MyBotMemory BotMemory
		{
			get;
		}

		MyBotMemory LastBotMemory
		{
			get;
			set;
		}

		MyBotDefinition BotDefinition
		{
			get;
		}

		MyBotActionsBase BotActions
		{
			get;
			set;
		}

		MyBotLogic BotLogic
		{
			get;
		}

		void Init(MyObjectBuilder_Bot botBuilder);

		void InitActions(ActionCollection actionCollection);

		void InitLogic(MyBotLogic logic);

		void Cleanup();

		void Update();

		void DebugDraw();

		void Reset();

		MyObjectBuilder_Bot GetObjectBuilder();

		void ReturnToLastMemory();
	}
}
