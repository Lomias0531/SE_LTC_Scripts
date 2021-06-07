namespace Sandbox.Game.AI.Logic
{
	public abstract class MyHumanoidBotLogic : MyAgentLogic
	{
		public MyReservationStatus ReservationStatus;

		public MyAiTargetManager.ReservedEntityData ReservationEntityData;

		public MyAiTargetManager.ReservedAreaData ReservationAreaData;

		public MyHumanoidBot HumanoidBot => m_bot as MyHumanoidBot;

		public override BotType BotType => BotType.HUMANOID;

		public MyHumanoidBotLogic(IMyBot bot)
			: base(bot)
		{
		}
	}
}
