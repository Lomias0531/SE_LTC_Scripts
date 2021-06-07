using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.ObjectBuilders.AI.Bot;

namespace Sandbox.Game.AI
{
	[MyBotType(typeof(MyObjectBuilder_AnimalBot))]
	public class MyAnimalBot : MyAgentBot
	{
		public MyCharacter AnimalEntity => base.AgentEntity;

		public MyAnimalBotDefinition AnimalDefinition => m_botDefinition as MyAnimalBotDefinition;

		public MyAnimalBot(MyPlayer player, MyBotDefinition botDefinition)
			: base(player, botDefinition)
		{
		}
	}
}
