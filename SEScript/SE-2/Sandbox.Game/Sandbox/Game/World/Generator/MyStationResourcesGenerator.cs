using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
	internal class MyStationResourcesGenerator
	{
		private MyObjectBuilder_PhysicalObject m_iceObjectBuilder;

		private List<MyCubeBlock> m_blocksCache;

		private MyDefinitionId? m_generatedItemsContainerTypeId;

		internal MyStationResourcesGenerator(MyDefinitionId? generatedItemsContainerTypeId)
		{
			m_iceObjectBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Ice");
			m_generatedItemsContainerTypeId = generatedItemsContainerTypeId;
		}

		internal void UpdateStation(long stationEntityId)
		{
			if (MyEntities.TryGetEntityById(stationEntityId, out MyCubeGrid entity))
			{
				UpdateStation(entity);
			}
		}

		internal void UpdateStation(MyCubeGrid grid)
		{
			if (grid.MarkedForClose || grid.Closed)
			{
				ClearBlocksCache();
				return;
			}
			if (m_blocksCache == null)
			{
				InitBlocksCache(grid);
			}
			foreach (MyCubeBlock item in m_blocksCache)
			{
				if (!item.MarkedForClose)
				{
					MyGasGenerator thisEntity;
					MyReactor myReactor;
					MyBatteryBlock myBatteryBlock;
					MyLargeTurretBase myLargeTurretBase;
					MyGasTank myGasTank;
					if ((thisEntity = (item as MyGasGenerator)) != null)
					{
						thisEntity.GetInventory().AddItems(10000, m_iceObjectBuilder);
					}
					else if ((myReactor = (item as MyReactor)) != null)
					{
						MyInventory inventory = myReactor.GetInventory();
						MyFixedPoint fp = inventory.MaxVolume - inventory.CurrentVolume;
						MyReactorDefinition.FuelInfo[] fuelInfos = myReactor.BlockDefinition.FuelInfos;
						for (int i = 0; i < fuelInfos.Length; i++)
						{
							MyReactorDefinition.FuelInfo fuelInfo = fuelInfos[i];
							int i2 = (int)((float)fp / fuelInfo.FuelDefinition.Volume);
							inventory.AddItems(i2, fuelInfo.FuelItem);
						}
					}
					else if ((myBatteryBlock = (item as MyBatteryBlock)) != null)
					{
						myBatteryBlock.CurrentStoredPower = myBatteryBlock.MaxStoredPower;
					}
					else if ((myLargeTurretBase = (item as MyLargeTurretBase)) != null)
					{
						MyInventory inventory2 = myLargeTurretBase.GetInventory();
						int i3 = (int)((float)(inventory2.MaxVolume - inventory2.CurrentVolume) / myLargeTurretBase.GunBase.WeaponProperties.AmmoMagazineDefinition.Volume);
						MyObjectBuilder_Base objectBuilder = MyObjectBuilderSerializer.CreateNewObject(myLargeTurretBase.GunBase.CurrentAmmoMagazineId);
						inventory2.AddItems(i3, objectBuilder);
					}
					else if ((myGasTank = (item as MyGasTank)) != null)
					{
						double transferAmount = (1.0 - myGasTank.FilledRatio) * (double)myGasTank.Capacity;
						myGasTank.Transfer(transferAmount);
					}
					else if ((item is MyCargoContainer || item is MyCryoChamber) && m_generatedItemsContainerTypeId.HasValue && item.InventoryCount != 0)
					{
						MyInventory inventory3 = item.GetInventory();
						if (inventory3.ItemCount == 0)
						{
							MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(m_generatedItemsContainerTypeId.Value);
							if (containerTypeDefinition != null && containerTypeDefinition.Items.Length != 0)
							{
								inventory3.GenerateContent(containerTypeDefinition);
							}
						}
					}
				}
			}
		}

		private void InitBlocksCache(MyCubeGrid grid)
		{
			m_blocksCache = new List<MyCubeBlock>();
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				MyGasGenerator item;
				MyReactor item2;
				MyBatteryBlock item3;
				MyLargeTurretBase item4;
				MyGasTank item5;
				MyCryoChamber myCryoChamber;
				MyCargoContainer myCargoContainer;
				if ((item = (block.FatBlock as MyGasGenerator)) != null)
				{
					m_blocksCache.Add(item);
				}
				else if ((item2 = (block.FatBlock as MyReactor)) != null)
				{
					m_blocksCache.Add(item2);
				}
				else if ((item3 = (block.FatBlock as MyBatteryBlock)) != null)
				{
					m_blocksCache.Add(item3);
				}
				else if ((item4 = (block.FatBlock as MyLargeTurretBase)) != null)
				{
					m_blocksCache.Add(item4);
				}
				else if ((item5 = (block.FatBlock as MyGasTank)) != null)
				{
					m_blocksCache.Add(item5);
				}
				else if ((myCryoChamber = (block.FatBlock as MyCryoChamber)) != null)
				{
					m_blocksCache.Add(myCryoChamber);
					myCryoChamber.ShowInInventory = false;
				}
				else if ((myCargoContainer = (block.FatBlock as MyCargoContainer)) != null)
				{
					m_blocksCache.Add(myCargoContainer);
					myCargoContainer.ShowInInventory = false;
				}
			}
		}

		internal void ClearBlocksCache()
		{
			if (m_blocksCache != null)
			{
				m_blocksCache.Clear();
				m_blocksCache = null;
			}
		}
	}
}
