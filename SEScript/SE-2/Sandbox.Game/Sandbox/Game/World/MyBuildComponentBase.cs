using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRageMath;

namespace Sandbox.Game.World
{
	public abstract class MyBuildComponentBase : MySessionComponentBase
	{
		protected MyComponentList m_materialList = new MyComponentList();

		protected MyComponentCombiner m_componentCombiner = new MyComponentCombiner();

		public DictionaryReader<MyDefinitionId, int> TotalMaterials => m_materialList.TotalMaterials;

		public abstract MyInventoryBase GetBuilderInventory(long entityId);

		public abstract MyInventoryBase GetBuilderInventory(MyEntity builder);

		public abstract bool HasBuildingMaterials(MyEntity builder, bool testTotal = false);

		public virtual void AfterCharacterCreate(MyCharacter character)
		{
			if (MyFakes.ENABLE_MEDIEVAL_INVENTORY)
			{
				character.InventoryAggregate = new MyInventoryAggregate("CharacterInventories");
				character.InventoryAggregate.AddComponent(new MyInventoryAggregate("Internal"));
			}
		}

		public abstract void GetGridSpawnMaterials(MyCubeBlockDefinition definition, MatrixD worldMatrix, bool isStatic);

		public abstract void GetGridSpawnMaterials(MyObjectBuilder_CubeGrid grid);

		public abstract void GetBlockPlacementMaterials(MyCubeBlockDefinition definition, Vector3I position, MyBlockOrientation orientation, MyCubeGrid grid);

		public abstract void GetBlocksPlacementMaterials(HashSet<MyCubeGrid.MyBlockLocation> hashSet, MyCubeGrid grid);

		public abstract void GetBlockAmountPlacementMaterials(MyCubeBlockDefinition definition, int amount);

		public abstract void GetMultiBlockPlacementMaterials(MyMultiBlockDefinition multiBlockDefinition);

		public virtual void BeforeCreateBlock(MyCubeBlockDefinition definition, MyEntity builder, MyObjectBuilder_CubeBlock ob, bool buildAsAdmin)
		{
			if (definition.EntityComponents != null)
			{
				if (ob.ComponentContainer == null)
				{
					ob.ComponentContainer = new MyObjectBuilder_ComponentContainer();
				}
				foreach (KeyValuePair<string, MyObjectBuilder_ComponentBase> entityComponent in definition.EntityComponents)
				{
					MyObjectBuilder_ComponentContainer.ComponentData componentData = new MyObjectBuilder_ComponentContainer.ComponentData();
					componentData.TypeId = entityComponent.Key.ToString();
					componentData.Component = entityComponent.Value;
					ob.ComponentContainer.Components.Add(componentData);
				}
			}
		}

		public abstract void AfterSuccessfulBuild(MyEntity builder, bool instantBuild);

		protected internal MyFixedPoint GetItemAmountCombined(MyInventoryBase availableInventory, MyDefinitionId myDefinitionId)
		{
			return m_componentCombiner.GetItemAmountCombined(availableInventory, myDefinitionId);
		}

		protected internal void RemoveItemsCombined(MyInventoryBase inventory, int itemAmount, MyDefinitionId itemDefinitionId)
		{
			m_materialList.Clear();
			m_materialList.AddMaterial(itemDefinitionId, itemAmount);
			m_componentCombiner.RemoveItemsCombined(inventory, m_materialList.TotalMaterials);
			m_materialList.Clear();
		}
	}
}
