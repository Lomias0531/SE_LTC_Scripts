using Sandbox.Game.AI.Navigation;
using VRageMath;

namespace Sandbox.Game.AI.Logic
{
	public class MyAnimalBotLogic : MyAgentLogic
	{
		private MyCharacterAvoidance m_characterAvoidance;

		public MyAnimalBot AnimalBot => m_bot as MyAnimalBot;

		public override BotType BotType => BotType.ANIMAL;

		public MyAnimalBotLogic(MyAnimalBot bot)
			: base(bot)
		{
			MyBotNavigation navigation = AnimalBot.Navigation;
			navigation.AddSteering(new MyTreeAvoidance(navigation, 0.1f));
			m_characterAvoidance = new MyCharacterAvoidance(navigation, 1f);
			navigation.AddSteering(m_characterAvoidance);
			navigation.MaximumRotationAngle = MathHelper.ToRadians(23f);
		}

		public void EnableCharacterAvoidance(bool isTrue)
		{
			MyBotNavigation navigation = AnimalBot.Navigation;
			bool flag = navigation.HasSteeringOfType(m_characterAvoidance.GetType());
			if (isTrue && !flag)
			{
				navigation.AddSteering(m_characterAvoidance);
			}
			else if (!isTrue && flag)
			{
				navigation.RemoveSteering(m_characterAvoidance);
			}
		}
	}
}
