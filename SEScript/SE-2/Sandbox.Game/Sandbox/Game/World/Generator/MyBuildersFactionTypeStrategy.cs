using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
	internal class MyBuildersFactionTypeStrategy : MyFactionTypeBaseStrategy
	{
		private static MyDefinitionId BUILDER_ID = new MyDefinitionId(typeof(MyObjectBuilder_FactionTypeDefinition), "Builder");

		public MyBuildersFactionTypeStrategy()
			: base(BUILDER_ID)
		{
			MyFactionTypeDefinition definition = m_definition;
			if (definition != null && definition.GridsForSale?.Length > 0)
			{
				m_priceCalculator.CalculatePrefabInformation(m_definition.GridsForSale, m_definition.BaseCostProductionSpeedMultiplier);
			}
		}

		public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration)
		{
			base.UpdateStationsStoreItems(faction, firstGeneration);
		}

		protected override void UpdateStationsStoreItems(MyStation station, int existingOffers, int existingOrders)
		{
			base.UpdateStationsStoreItems(station, existingOffers, existingOrders);
			GenerateGridOffers(station, existingOffers);
		}

		private void GenerateGridOffers(MyStation station, int existingOffers)
		{
			int num = 0;
			foreach (MyStoreItem storeItem in station.StoreItems)
			{
				if (storeItem.StoreItemType == StoreItemTypes.Offer && storeItem.ItemType == ItemTypes.Grid)
				{
					num++;
				}
			}
			int num2 = (m_definition.GridsForSale != null) ? m_definition.GridsForSale.Length : 0;
			int num3 = num2 / 2;
			if (num2 == 0 || num3 <= num)
			{
				return;
			}
			num3 -= num;
			List<string> list = new List<string>(m_definition.GridsForSale);
			foreach (MyStoreItem storeItem2 in station.StoreItems)
			{
				if (!string.IsNullOrEmpty(storeItem2.PrefabName) && storeItem2.ItemType == ItemTypes.Grid)
				{
					list.Remove(storeItem2.PrefabName);
				}
			}
			for (int i = 0; i < num3 && list.Count != 0; i++)
			{
				int index = MyRandom.Instance.Next(0, list.Count);
				string text = list[index];
				int minimalPrice = 0;
				if (!m_priceCalculator.TryGetPrefabMinimalPrice(text, out minimalPrice))
				{
					continue;
				}
				if (minimalPrice <= 0)
				{
					MyLog.Default.WriteToLogAndAssert("Wrong price for the prefab - " + text);
					continue;
				}
				int totalPcu = 0;
				m_priceCalculator.TryGetPrefabTotalPcu(text, out totalPcu);
				m_priceCalculator.TryGetPrefabEnvironmentType(text, out MyEnvironmentTypes environmentType);
				if (environmentType != 0)
				{
					switch (station.Type)
					{
					case MyStationTypeEnum.MiningStation:
					case MyStationTypeEnum.OrbitalStation:
					case MyStationTypeEnum.SpaceStation:
						if ((environmentType & MyEnvironmentTypes.Space) != MyEnvironmentTypes.Space)
						{
							list.Remove(text);
							continue;
						}
						break;
					case MyStationTypeEnum.Outpost:
						if (station.IsOnPlanetWithAtmosphere)
						{
							if ((environmentType & MyEnvironmentTypes.PlanetWithAtmosphere) != MyEnvironmentTypes.PlanetWithAtmosphere)
							{
								list.Remove(text);
								continue;
							}
						}
						else if ((environmentType & MyEnvironmentTypes.PlanetWithoutAtmosphere) != MyEnvironmentTypes.PlanetWithoutAtmosphere)
						{
							list.Remove(text);
							continue;
						}
						break;
					}
				}
				MyStoreItem item = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), text, 1, minimalPrice, totalPcu, StoreItemTypes.Offer);
				station.StoreItems.Add(item);
				list.Remove(text);
			}
		}
	}
}
