using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public static class HudBlockInfoExtensions
	{
		public static void LoadDefinition(this MyHudBlockInfo blockInfo, MyCubeBlockDefinition definition, bool merge = true)
		{
			blockInfo.InitBlockInfo(definition);
			if (definition.MultiBlock != null)
			{
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_MultiBlockDefinition), definition.MultiBlock);
				MyMultiBlockDefinition myMultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(id);
				if (myMultiBlockDefinition != null)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition[] blockDefinitions = myMultiBlockDefinition.BlockDefinitions;
					foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition in blockDefinitions)
					{
						MyCubeBlockDefinition definition2 = null;
						MyDefinitionManager.Static.TryGetDefinition(myMultiBlockPartDefinition.Id, out definition2);
						if (definition2 != null)
						{
							blockInfo.AddComponentsForBlock(definition2);
						}
					}
				}
			}
			else
			{
				blockInfo.AddComponentsForBlock(definition);
			}
			if (merge)
			{
				blockInfo.MergeSameComponents();
			}
		}

		public static void LoadDefinition(this MyHudBlockInfo blockInfo, MyCubeBlockDefinition definition, DictionaryReader<MyDefinitionId, int> materials, bool merge = true)
		{
			blockInfo.InitBlockInfo(definition);
			foreach (KeyValuePair<MyDefinitionId, int> item2 in materials)
			{
				MyDefinitionBase definition2 = MyDefinitionManager.Static.GetDefinition(item2.Key);
				MyHudBlockInfo.ComponentInfo item = default(MyHudBlockInfo.ComponentInfo);
				if (definition2 == null)
				{
					MyPhysicalItemDefinition definition3 = null;
					if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(item2.Key, out definition3))
					{
						continue;
					}
					item.ComponentName = definition3.DisplayNameText;
					item.Icons = definition3.Icons;
					item.DefinitionId = definition3.Id;
					item.TotalCount = 1;
				}
				else
				{
					item.DefinitionId = definition2.Id;
					item.ComponentName = definition2.DisplayNameText;
					item.Icons = definition2.Icons;
					item.TotalCount = item2.Value;
				}
				blockInfo.Components.Add(item);
			}
			if (merge)
			{
				blockInfo.MergeSameComponents();
			}
		}

		public static void AddComponentsForBlock(this MyHudBlockInfo blockInfo, MyCubeBlockDefinition definition)
		{
			for (int i = 0; i < definition.Components.Length; i++)
			{
				MyCubeBlockDefinition.Component component = definition.Components[i];
				MyHudBlockInfo.ComponentInfo item = default(MyHudBlockInfo.ComponentInfo);
				item.DefinitionId = component.Definition.Id;
				item.ComponentName = component.Definition.DisplayNameText;
				item.Icons = component.Definition.Icons;
				item.TotalCount = component.Count;
				blockInfo.Components.Add(item);
			}
		}

		public static void InitBlockInfo(this MyHudBlockInfo blockInfo, MyCubeBlockDefinition definition)
		{
			blockInfo.BlockName = definition.DisplayNameText;
			blockInfo.SetContextHelp(definition);
			blockInfo.PCUCost = definition.PCU;
			blockInfo.BlockIcons = definition.Icons;
			blockInfo.BlockIntegrity = 0f;
			blockInfo.CriticalComponentIndex = definition.CriticalGroup;
			blockInfo.CriticalIntegrity = definition.CriticalIntegrityRatio;
			blockInfo.OwnershipIntegrity = definition.OwnershipIntegrityRatio;
			blockInfo.MissingComponentIndex = -1;
			blockInfo.GridSize = definition.CubeSize;
			blockInfo.Components.Clear();
			blockInfo.BlockBuiltBy = 0L;
		}

		public static void MergeSameComponents(this MyHudBlockInfo blockInfo)
		{
			for (int num = blockInfo.Components.Count - 1; num >= 0; num--)
			{
				for (int num2 = num - 1; num2 >= 0; num2--)
				{
					if (blockInfo.Components[num].DefinitionId == blockInfo.Components[num2].DefinitionId)
					{
						MyHudBlockInfo.ComponentInfo value = blockInfo.Components[num2];
						value.TotalCount += blockInfo.Components[num].TotalCount;
						value.MountedCount += blockInfo.Components[num].MountedCount;
						value.StockpileCount += blockInfo.Components[num].StockpileCount;
						blockInfo.Components[num2] = value;
						blockInfo.Components.RemoveAt(num);
						break;
					}
				}
			}
		}
	}
}
