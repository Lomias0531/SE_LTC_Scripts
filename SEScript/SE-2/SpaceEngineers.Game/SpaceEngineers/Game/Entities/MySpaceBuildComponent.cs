using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.Entities
{
	[PreloadRequired]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	internal class MySpaceBuildComponent : MyBuildComponentBase
	{
		public override void LoadData()
		{
			base.LoadData();
			MyCubeBuilder.BuildComponent = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyCubeBuilder.BuildComponent = null;
		}

		public override MyInventoryBase GetBuilderInventory(long entityId)
		{
			if (MySession.Static.CreativeMode)
			{
				return null;
			}
			MyEntities.TryGetEntityById(entityId, out MyEntity entity);
			if (entity == null)
			{
				return null;
			}
			return GetBuilderInventory(entity);
		}

		public override MyInventoryBase GetBuilderInventory(MyEntity entity)
		{
			if (MySession.Static.CreativeMode)
			{
				return null;
			}
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				return myCharacter.GetInventory();
			}
			return (entity as MyShipWelder)?.GetInventory();
		}

		public override bool HasBuildingMaterials(MyEntity builder, bool testTotal)
		{
			if (MySession.Static.CreativeMode || (MySession.Static.CreativeToolsEnabled(Sync.MyId) && builder == MySession.Static.LocalCharacter))
			{
				return true;
			}
			if (builder == null)
			{
				return false;
			}
			MyInventoryBase builderInventory = GetBuilderInventory(builder);
			if (builderInventory == null)
			{
				return false;
			}
			MyInventory myInventory = null;
			MyCockpit myCockpit = null;
			long playerId = MySession.Static.LocalPlayerId;
			if (builder is MyCharacter)
			{
				myCockpit = ((builder as MyCharacter).IsUsing as MyCockpit);
				if (myCockpit != null)
				{
					myInventory = myCockpit.GetInventory();
					playerId = myCockpit.ControllerInfo.ControllingIdentityId;
				}
				else if ((builder as MyCharacter).ControllerInfo != null)
				{
					playerId = (builder as MyCharacter).ControllerInfo.ControllingIdentityId;
				}
			}
			bool flag = true;
			if (!testTotal)
			{
				foreach (KeyValuePair<MyDefinitionId, int> requiredMaterial in m_materialList.RequiredMaterials)
				{
					flag &= (builderInventory.GetItemAmount(requiredMaterial.Key) >= requiredMaterial.Value);
					if (!flag && myInventory != null)
					{
						flag = (myInventory.GetItemAmount(requiredMaterial.Key) >= requiredMaterial.Value);
						if (!flag)
						{
							flag = (MyGridConveyorSystem.ConveyorSystemItemAmount(myCockpit, myInventory, playerId, requiredMaterial.Key) >= requiredMaterial.Value);
						}
					}
					if (!flag)
					{
						return flag;
					}
				}
				return flag;
			}
			foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in m_materialList.TotalMaterials)
			{
				flag &= (builderInventory.GetItemAmount(totalMaterial.Key) >= totalMaterial.Value);
				if (!flag && myInventory != null)
				{
					flag = (myInventory.GetItemAmount(totalMaterial.Key) >= totalMaterial.Value);
					if (!flag)
					{
						flag = (MyGridConveyorSystem.ConveyorSystemItemAmount(myCockpit, myInventory, playerId, totalMaterial.Key) >= totalMaterial.Value);
					}
				}
				if (!flag)
				{
					return flag;
				}
			}
			return flag;
		}

		public override void GetGridSpawnMaterials(MyCubeBlockDefinition definition, MatrixD worldMatrix, bool isStatic)
		{
			ClearRequiredMaterials();
			GetMaterialsSimple(definition, m_materialList);
		}

		public override void GetBlockPlacementMaterials(MyCubeBlockDefinition definition, Vector3I position, MyBlockOrientation orientation, MyCubeGrid grid)
		{
			ClearRequiredMaterials();
			GetMaterialsSimple(definition, m_materialList);
		}

		public override void GetBlocksPlacementMaterials(HashSet<MyCubeGrid.MyBlockLocation> hashSet, MyCubeGrid grid)
		{
			ClearRequiredMaterials();
			foreach (MyCubeGrid.MyBlockLocation item in hashSet)
			{
				MyCubeBlockDefinition blockDefinition = null;
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(item.BlockDefinition, out blockDefinition))
				{
					GetMaterialsSimple(blockDefinition, m_materialList);
				}
			}
		}

		public override void GetBlockAmountPlacementMaterials(MyCubeBlockDefinition definition, int amount)
		{
			ClearRequiredMaterials();
			GetMaterialsSimple(definition, m_materialList, amount);
		}

		public override void GetGridSpawnMaterials(MyObjectBuilder_CubeGrid grid)
		{
			ClearRequiredMaterials();
			foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
			{
				MyComponentStack.GetMountedComponents(m_materialList, cubeBlock);
				if (cubeBlock.ConstructionStockpile != null)
				{
					MyObjectBuilder_StockpileItem[] items = cubeBlock.ConstructionStockpile.Items;
					foreach (MyObjectBuilder_StockpileItem myObjectBuilder_StockpileItem in items)
					{
						if (myObjectBuilder_StockpileItem.PhysicalContent != null)
						{
							MyDefinitionId id = myObjectBuilder_StockpileItem.PhysicalContent.GetId();
							m_materialList.AddMaterial(id, myObjectBuilder_StockpileItem.Amount, myObjectBuilder_StockpileItem.Amount, addToDisplayList: false);
						}
					}
				}
			}
		}

		public override void GetMultiBlockPlacementMaterials(MyMultiBlockDefinition multiBlockDefinition)
		{
		}

		public override void BeforeCreateBlock(MyCubeBlockDefinition definition, MyEntity builder, MyObjectBuilder_CubeBlock ob, bool buildAsAdmin)
		{
			base.BeforeCreateBlock(definition, builder, ob, buildAsAdmin);
			if (builder != null && MySession.Static.SurvivalMode && !buildAsAdmin)
			{
				ob.IntegrityPercent = 1.52590219E-05f;
				ob.BuildPercent = 1.52590219E-05f;
			}
		}

		public override void AfterSuccessfulBuild(MyEntity builder, bool instantBuild)
		{
			if (!(builder == null || instantBuild) && MySession.Static.SurvivalMode)
			{
				TakeMaterialsFromBuilder(builder);
			}
		}

		private void ClearRequiredMaterials()
		{
			m_materialList.Clear();
		}

		private static void GetMaterialsSimple(MyCubeBlockDefinition definition, MyComponentList output, int amount = 1)
		{
			for (int i = 0; i < definition.Components.Length; i++)
			{
				MyCubeBlockDefinition.Component component = definition.Components[i];
				output.AddMaterial(component.Definition.Id, component.Count * amount, (i == 0) ? 1 : 0);
			}
		}

		private void TakeMaterialsFromBuilder(MyEntity builder)
		{
			if (builder == null)
			{
				return;
			}
			MyInventoryBase builderInventory = GetBuilderInventory(builder);
			if (builderInventory == null)
			{
				return;
			}
			MyInventory myInventory = null;
			MyCockpit myCockpit = null;
			if (builder is MyCharacter)
			{
				myCockpit = ((builder as MyCharacter).IsUsing as MyCockpit);
				if (myCockpit != null)
				{
					myInventory = myCockpit.GetInventory();
					_ = myCockpit.ControllerInfo.ControllingIdentityId;
				}
				else if ((builder as MyCharacter).ControllerInfo != null)
				{
					_ = (builder as MyCharacter).ControllerInfo.ControllingIdentityId;
				}
			}
			foreach (KeyValuePair<MyDefinitionId, int> requiredMaterial in m_materialList.RequiredMaterials)
			{
				MyFixedPoint myFixedPoint = requiredMaterial.Value;
				MyFixedPoint itemAmount = builderInventory.GetItemAmount(requiredMaterial.Key);
				if (itemAmount > requiredMaterial.Value)
				{
					builderInventory.RemoveItemsOfType(myFixedPoint, requiredMaterial.Key);
				}
				else
				{
					if (itemAmount > 0)
					{
						builderInventory.RemoveItemsOfType(itemAmount, requiredMaterial.Key);
						myFixedPoint -= itemAmount;
					}
					if (myInventory != null)
					{
						MyFixedPoint itemAmount2 = myInventory.GetItemAmount(requiredMaterial.Key);
						if (itemAmount2 >= myFixedPoint)
						{
							myInventory.RemoveItemsOfType(myFixedPoint, requiredMaterial.Key);
						}
						else
						{
							if (itemAmount2 > 0)
							{
								myInventory.RemoveItemsOfType(itemAmount2, requiredMaterial.Key);
								myFixedPoint -= itemAmount2;
							}
							myCockpit.CubeGrid.GridSystems.ConveyorSystem.PullItem(requiredMaterial.Key, myFixedPoint, myCockpit, myInventory, remove: true, calcImmediately: false);
						}
					}
				}
			}
		}
	}
}
