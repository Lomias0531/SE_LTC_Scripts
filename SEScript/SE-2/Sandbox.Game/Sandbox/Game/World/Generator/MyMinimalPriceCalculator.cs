using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
	public sealed class MyMinimalPriceCalculator
	{
		private struct MyPrefabInfo
		{
			public int MinimalPrice;

			public int MinimalRepairPrice;

			public int TotalPcu;

			public MyEnvironmentTypes EnvironmentType;
		}

		private struct MyBlockInfo
		{
			public int MinimalPrice;

			public int Pcu;
		}

		public const float BLOCK_INTEGRITY_THRESHOLD = 0.5f;

		private Dictionary<MyDefinitionId, int> m_minimalPrices = new Dictionary<MyDefinitionId, int>();

		private Dictionary<MyDefinitionId, MyBlockInfo> m_blocksInfo = new Dictionary<MyDefinitionId, MyBlockInfo>();

		private Dictionary<string, MyPrefabInfo> m_prefabsInfo = new Dictionary<string, MyPrefabInfo>();

		public bool TryGetItemMinimalPrice(MyDefinitionId itemId, out int minimalPrice)
		{
			return m_minimalPrices.TryGetValue(itemId, out minimalPrice);
		}

		public bool TryGetPrefabMinimalPrice(string prefabName, out int minimalPrice)
		{
			MyPrefabInfo value;
			bool result = m_prefabsInfo.TryGetValue(prefabName, out value);
			minimalPrice = value.MinimalPrice;
			return result;
		}

		public bool TryGetPrefabMinimalRepairPrice(string prefabName, out int minimalRepairPrice)
		{
			MyPrefabInfo value;
			bool result = m_prefabsInfo.TryGetValue(prefabName, out value);
			minimalRepairPrice = value.MinimalRepairPrice;
			return result;
		}

		public bool TryGetPrefabTotalPcu(string prefabName, out int totalPcu)
		{
			MyPrefabInfo value;
			bool result = m_prefabsInfo.TryGetValue(prefabName, out value);
			totalPcu = value.TotalPcu;
			return result;
		}

		public bool TryGetPrefabEnvironmentType(string prefabName, out MyEnvironmentTypes environmentType)
		{
			MyPrefabInfo value;
			bool result = m_prefabsInfo.TryGetValue(prefabName, out value);
			environmentType = value.EnvironmentType;
			return result;
		}

		public void CalculatePrefabInformation(string[] prefabNames, float baseCostProductionSpeedMultiplier = 1f)
		{
			foreach (string text in prefabNames)
			{
				int num = 0;
				int num2 = 0;
				MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(text);
				if (prefabDefinition != null && prefabDefinition.CubeGrids != null && prefabDefinition.CubeGrids.Length != 0 && !string.IsNullOrEmpty(prefabDefinition.CubeGrids[0].DisplayName))
				{
					int num3 = 0;
					MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
					for (int j = 0; j < cubeGrids.Length; j++)
					{
						foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrids[j].CubeBlocks)
						{
							MyDefinitionId myDefinitionId = new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeName);
							if (!m_blocksInfo.TryGetValue(myDefinitionId, out MyBlockInfo value))
							{
								int minimalPrice = 0;
								int pcu = 0;
								CalculateBlockMinimalPriceAndPcu(myDefinitionId, baseCostProductionSpeedMultiplier, ref minimalPrice, ref pcu);
								MyBlockInfo myBlockInfo = default(MyBlockInfo);
								myBlockInfo.MinimalPrice = minimalPrice;
								myBlockInfo.Pcu = pcu;
								value = myBlockInfo;
							}
							num3 += value.Pcu;
							num += value.MinimalPrice;
							if (cubeBlock.IntegrityPercent <= 0.5f)
							{
								num2 += value.MinimalPrice;
							}
						}
					}
					m_prefabsInfo[text] = new MyPrefabInfo
					{
						MinimalPrice = num,
						MinimalRepairPrice = num2,
						TotalPcu = num3,
						EnvironmentType = prefabDefinition.EnvironmentType
					};
				}
			}
		}

		public void CalculateMinimalPrices(SerializableDefinitionId[] itemsList, float baseCostProductionSpeedMultiplier = 1f)
		{
			if (itemsList == null)
			{
				return;
			}
			foreach (SerializableDefinitionId v in itemsList)
			{
				if (!m_minimalPrices.ContainsKey(v))
				{
					int minimalPrice = 0;
					CalculateItemMinimalPrice(v, baseCostProductionSpeedMultiplier, ref minimalPrice);
					m_minimalPrices[v] = minimalPrice;
				}
			}
		}

		private void CalculateItemMinimalPrice(MyDefinitionId itemId, float baseCostProductionSpeedMultiplier, ref int minimalPrice)
		{
			MyPhysicalItemDefinition definition = null;
			if (MyDefinitionManager.Static.TryGetDefinition(itemId, out definition) && definition.MinimalPricePerUnit != -1)
			{
				minimalPrice += definition.MinimalPricePerUnit;
				return;
			}
			MyBlueprintDefinitionBase definition2 = null;
			if (!MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(itemId, out definition2))
			{
				return;
			}
			float num = definition.IsIngot ? 1f : MySession.Static.AssemblerEfficiencyMultiplier;
			int num2 = 0;
			MyBlueprintDefinitionBase.Item[] prerequisites = definition2.Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = prerequisites[i];
				int minimalPrice2 = 0;
				CalculateItemMinimalPrice(item.Id, baseCostProductionSpeedMultiplier, ref minimalPrice2);
				float num3 = (float)item.Amount / num;
				num2 += (int)((float)minimalPrice2 * num3);
			}
			float num4 = definition.IsIngot ? MySession.Static.RefinerySpeedMultiplier : MySession.Static.AssemblerSpeedMultiplier;
			int num5 = 0;
			float num6;
			while (true)
			{
				if (num5 >= definition2.Results.Length)
				{
					return;
				}
				MyBlueprintDefinitionBase.Item item2 = definition2.Results[num5];
				if (item2.Id == itemId)
				{
					num6 = (float)item2.Amount;
					if (num6 != 0f)
					{
						break;
					}
					MyLog.Default.WriteToLogAndAssert("Amount is 0 for - " + item2.Id);
				}
				num5++;
			}
			float num7 = 1f + (float)Math.Log(definition2.BaseProductionTimeInSeconds + 1f) * baseCostProductionSpeedMultiplier / num4;
			minimalPrice += (int)((float)num2 * (1f / num6) * num7);
		}

		private void CalculateBlockMinimalPriceAndPcu(MyDefinitionId blockId, float baseCostProductionSpeedMultiplier, ref int minimalPrice, ref int pcu)
		{
			minimalPrice = 0;
			pcu = 0;
			if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockId, out MyCubeBlockDefinition blockDefinition))
			{
				return;
			}
			pcu += blockDefinition.PCU;
			MyCubeBlockDefinition.Component[] components = blockDefinition.Components;
			foreach (MyCubeBlockDefinition.Component component in components)
			{
				int value = 0;
				if (!m_minimalPrices.TryGetValue(component.Definition.Id, out value))
				{
					CalculateItemMinimalPrice(component.Definition.Id, baseCostProductionSpeedMultiplier, ref value);
					m_minimalPrices[component.Definition.Id] = value;
				}
				minimalPrice += value * component.Count;
			}
		}
	}
}
