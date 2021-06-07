using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.World.Generator
{
	public class MyStoreItemsGenerator
	{
		private Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy> m_generatorStrategies = new Dictionary<MyFactionTypes, IMyStoreItemsGeneratorFactionTypeStrategy>();

		public MyStoreItemsGenerator()
		{
			m_generatorStrategies.Add(MyFactionTypes.Miner, new MyMinersFactionTypeStrategy());
			m_generatorStrategies.Add(MyFactionTypes.Trader, new MyTradersFactionTypeStrategy());
			m_generatorStrategies.Add(MyFactionTypes.Builder, new MyBuildersFactionTypeStrategy());
		}

		public void Update(MyFaction faction, bool firstGeneration)
		{
			if (m_generatorStrategies.TryGetValue(faction.FactionType, out IMyStoreItemsGeneratorFactionTypeStrategy value))
			{
				value.UpdateStationsStoreItems(faction, firstGeneration);
			}
			else if (faction.FactionType != 0)
			{
				_ = faction.FactionType;
				_ = 1;
			}
		}
	}
}
