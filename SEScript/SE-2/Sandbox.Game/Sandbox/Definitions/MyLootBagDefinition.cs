using VRage.Game;

namespace Sandbox.Definitions
{
	public class MyLootBagDefinition
	{
		public MyDefinitionId ContainerDefinition;

		public float SearchRadius;

		public void Init(MyObjectBuilder_Configuration.LootBagDefinition objectBuilder)
		{
			ContainerDefinition = objectBuilder.ContainerDefinition;
			SearchRadius = objectBuilder.SearchRadius;
		}
	}
}
