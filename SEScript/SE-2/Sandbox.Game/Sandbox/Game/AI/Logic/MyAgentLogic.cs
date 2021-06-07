using Sandbox.Game.Entities.Character;

namespace Sandbox.Game.AI.Logic
{
	public abstract class MyAgentLogic : MyBotLogic
	{
		protected IMyEntityBot m_entityBot;

		public MyAgentBot AgentBot => m_bot as MyAgentBot;

		public MyAiTargetBase AiTarget
		{
			get;
			private set;
		}

		public override BotType BotType => BotType.UNKNOWN;

		public MyAgentLogic(IMyBot bot)
			: base(bot)
		{
			m_entityBot = (m_bot as IMyEntityBot);
			AiTarget = MyAIComponent.BotFactory.CreateTargetForBot(AgentBot);
		}

		public override void Init()
		{
			base.Init();
			AiTarget = AgentBot.AgentActions.AiTargetBase;
		}

		public override void Cleanup()
		{
			base.Cleanup();
			AiTarget.Cleanup();
		}

		public override void Update()
		{
			base.Update();
			AiTarget.Update();
		}

		public virtual void OnCharacterControlAcquired(MyCharacter character)
		{
		}
	}
}
