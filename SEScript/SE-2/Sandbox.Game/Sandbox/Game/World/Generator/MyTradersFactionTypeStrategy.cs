using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.World.Generator
{
	internal class MyTradersFactionTypeStrategy : MyFactionTypeBaseStrategy
	{
		private static MyDefinitionId TRADER_ID = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Trader");

		public MyTradersFactionTypeStrategy()
			: base(TRADER_ID)
		{
		}

		public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration)
		{
			base.UpdateStationsStoreItems(faction, firstGeneration);
		}
	}
}
