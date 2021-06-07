using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.World.Generator
{
	internal class MyMinersFactionTypeStrategy : MyFactionTypeBaseStrategy
	{
		private static MyDefinitionId MINER_ID = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Miner");

		public MyMinersFactionTypeStrategy()
			: base(MINER_ID)
		{
		}

		public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration)
		{
			base.UpdateStationsStoreItems(faction, firstGeneration);
		}
	}
}
