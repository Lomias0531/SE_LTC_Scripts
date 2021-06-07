using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Refinery))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyRefinery),
		typeof(Sandbox.ModAPI.Ingame.IMyRefinery)
	})]
	public class MyRefinery : MyProductionBlock, Sandbox.ModAPI.IMyRefinery, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyRefinery
	{
		private class Sandbox_Game_Entities_Cube_MyRefinery_003C_003EActor : IActivator, IActivator<MyRefinery>
		{
			private sealed override object CreateInstance()
			{
				return new MyRefinery();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRefinery CreateInstance()
			{
				return new MyRefinery();
			}

			MyRefinery IActivator<MyRefinery>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyEntity m_currentUser;

		private MyRefineryDefinition m_refineryDef;

		private bool m_queueNeedsRebuild;

		private bool m_processingLock;

		private readonly List<KeyValuePair<int, MyBlueprintDefinitionBase>> m_tmpSortedBlueprints = new List<KeyValuePair<int, MyBlueprintDefinitionBase>>();

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.UpgradeValues.Add("Productivity", 0f);
			base.UpgradeValues.Add("Effectiveness", 1f);
			base.UpgradeValues.Add("PowerEfficiency", 1f);
			base.Init(objectBuilder, cubeGrid);
			m_refineryDef = (base.BlockDefinition as MyRefineryDefinition);
			if (base.InventoryAggregate.InventoryCount > 2)
			{
				FixInputOutputInventories(m_refineryDef.InputInventoryConstraint, m_refineryDef.OutputInventoryConstraint);
			}
			base.InputInventory.Constraint = m_refineryDef.InputInventoryConstraint;
			base.InputInventory.FilterItemsUsingConstraint();
			base.OutputInventory.Constraint = m_refineryDef.OutputInventoryConstraint;
			base.OutputInventory.FilterItemsUsingConstraint();
			m_queueNeedsRebuild = true;
			m_baseIdleSound = base.BlockDefinition.PrimarySound;
			m_processSound = base.BlockDefinition.ActionSound;
			base.ResourceSink.RequiredInputChanged += PowerReceiver_RequiredInputChanged;
			base.OnUpgradeValuesChanged += delegate
			{
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
			};
			SetDetailedInfoDirty();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		protected override void OnBeforeInventoryRemovedFromAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			if (inventory == base.InputInventory)
			{
				base.InputInventory.ContentsChanged += inventory_OnContentsChanged;
			}
			else if (inventory == base.OutputInventory)
			{
				base.OutputInventory.ContentsChanged += inventory_OnContentsChanged;
			}
			base.OnBeforeInventoryRemovedFromAggregate(aggregate, inventory);
		}

		protected override void OnInventoryAddedToAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			base.OnInventoryAddedToAggregate(aggregate, inventory);
			if (inventory == base.InputInventory)
			{
				base.InputInventory.ContentsChanged += inventory_OnContentsChanged;
			}
			else if (inventory == base.OutputInventory)
			{
				base.OutputInventory.ContentsChanged += inventory_OnContentsChanged;
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && base.IsWorking && (bool)m_useConveyorSystem)
			{
				if (base.InputInventory.VolumeFillFactor < 0.6f && MyGridConveyorSystem.PullAllRequest(this, base.InputInventory, base.OwnerId, base.InputInventory.Constraint, m_refineryDef.OreAmountPerPullRequest))
				{
					m_queueNeedsRebuild = true;
				}
				if (base.OutputInventory.VolumeFillFactor > 0.75f)
				{
					MyGridConveyorSystem.PushAnyRequest(this, base.OutputInventory, base.OwnerId);
				}
			}
		}

		private void PowerReceiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
		}

		private void inventory_OnContentsChanged(MyInventoryBase inv)
		{
			if (!m_processingLock && Sync.IsServer)
			{
				m_queueNeedsRebuild = true;
			}
		}

		private void RebuildQueue()
		{
			m_queueNeedsRebuild = false;
			ClearQueue(sendEvent: false);
			m_tmpSortedBlueprints.Clear();
			MyPhysicalInventoryItem[] array = base.InputInventory.GetItems().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < m_refineryDef.BlueprintClasses.Count; j++)
				{
					foreach (MyBlueprintDefinitionBase item2 in m_refineryDef.BlueprintClasses[j])
					{
						bool flag = false;
						MyDefinitionId other = new MyDefinitionId(array[i].Content.TypeId, array[i].Content.SubtypeId);
						for (int k = 0; k < item2.Prerequisites.Length; k++)
						{
							if (item2.Prerequisites[k].Id.Equals(other))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							m_tmpSortedBlueprints.Add(new KeyValuePair<int, MyBlueprintDefinitionBase>(i, item2));
							break;
						}
					}
				}
			}
			for (int l = 0; l < m_tmpSortedBlueprints.Count; l++)
			{
				MyBlueprintDefinitionBase value = m_tmpSortedBlueprints[l].Value;
				MyFixedPoint myFixedPoint = MyFixedPoint.MaxValue;
				MyBlueprintDefinitionBase.Item[] prerequisites = value.Prerequisites;
				for (int m = 0; m < prerequisites.Length; m++)
				{
					MyBlueprintDefinitionBase.Item item = prerequisites[m];
					MyFixedPoint amount = array[l].Amount;
					if (amount == 0)
					{
						myFixedPoint = 0;
						break;
					}
					myFixedPoint = MyFixedPoint.Min(amount * (1f / (float)item.Amount), myFixedPoint);
				}
				if (value.Atomic)
				{
					myFixedPoint = MyFixedPoint.Floor(myFixedPoint);
				}
				if (myFixedPoint > 0 && myFixedPoint != MyFixedPoint.MaxValue)
				{
					InsertQueueItemRequest(-1, value, myFixedPoint);
				}
			}
			m_tmpSortedBlueprints.Clear();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(GetOperationalPowerConsumption(), detailedInfo);
			detailedInfo.AppendFormat("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.AppendFormat("\n\n");
			detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertiesText_Productivity));
			detailedInfo.Append(((base.UpgradeValues["Productivity"] + 1f) * 100f).ToString("F0"));
			detailedInfo.Append("%\n");
			detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertiesText_Effectiveness));
			detailedInfo.Append((base.UpgradeValues["Effectiveness"] * 100f).ToString("F0"));
			detailedInfo.Append("%\n");
			detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.BlockPropertiesText_Efficiency));
			detailedInfo.Append((base.UpgradeValues["PowerEfficiency"] * 100f).ToString("F0"));
			detailedInfo.Append("%\n\n");
			PrintUpgradeModuleInfo(detailedInfo);
		}

		protected override void UpdateProduction(int timeDelta)
		{
			if (m_queueNeedsRebuild && Sync.IsServer)
			{
				RebuildQueue();
			}
			base.IsProducing = (base.IsWorking && !base.IsQueueEmpty && !base.OutputInventory.IsFull && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId));
			if (base.IsProducing)
			{
				ProcessQueueItems(timeDelta);
			}
		}

		private void ProcessQueueItems(int timeDelta)
		{
			m_processingLock = true;
			if (Sync.IsServer)
			{
				while (!base.IsQueueEmpty && timeDelta > 0)
				{
					QueueItem value = TryGetFirstQueueItem().Value;
					MyFixedPoint myFixedPoint = (MyFixedPoint)((float)timeDelta * (m_refineryDef.RefineSpeed + base.UpgradeValues["Productivity"]) * MySession.Static.RefinerySpeedMultiplier / (value.Blueprint.BaseProductionTimeInSeconds * 1000f));
					MyBlueprintDefinitionBase.Item[] prerequisites = value.Blueprint.Prerequisites;
					for (int i = 0; i < prerequisites.Length; i++)
					{
						MyBlueprintDefinitionBase.Item item = prerequisites[i];
						MyFixedPoint itemAmount = base.InputInventory.GetItemAmount(item.Id);
						MyFixedPoint b = myFixedPoint * item.Amount;
						if (itemAmount < b)
						{
							myFixedPoint = itemAmount * (1f / (float)item.Amount);
						}
					}
					if (myFixedPoint == 0)
					{
						m_queueNeedsRebuild = true;
						break;
					}
					timeDelta -= Math.Max(1, (int)((float)myFixedPoint * value.Blueprint.BaseProductionTimeInSeconds / m_refineryDef.RefineSpeed * 1000f));
					if (timeDelta < 0)
					{
						timeDelta = 0;
					}
					ChangeRequirementsToResults(value.Blueprint, myFixedPoint);
				}
			}
			base.IsProducing = !base.IsQueueEmpty;
			m_processingLock = false;
		}

		private void ChangeRequirementsToResults(MyBlueprintDefinitionBase queueItem, MyFixedPoint blueprintAmount)
		{
			if (m_refineryDef == null)
			{
				MyLog.Default.WriteLine("m_refineryDef shouldn't be null!!!" + this);
			}
			else
			{
				if (!Sync.IsServer || MySession.Static == null || queueItem == null || queueItem.Prerequisites == null || base.OutputInventory == null || base.InputInventory == null || queueItem.Results == null || m_refineryDef == null)
				{
					return;
				}
				if (!MySession.Static.CreativeMode)
				{
					blueprintAmount = MyFixedPoint.Min(base.OutputInventory.ComputeAmountThatFits(queueItem), blueprintAmount);
				}
				if (blueprintAmount == 0)
				{
					return;
				}
				MyBlueprintDefinitionBase.Item[] prerequisites = queueItem.Prerequisites;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					MyBlueprintDefinitionBase.Item item = prerequisites[i];
					MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject = MyObjectBuilderSerializer.CreateNewObject(item.Id) as MyObjectBuilder_PhysicalObject;
					if (myObjectBuilder_PhysicalObject == null)
					{
						MyLog.Default.WriteLine("obPrerequisite shouldn't be null!!! " + this);
						continue;
					}
					float d = (float)blueprintAmount * (float)item.Amount;
					base.InputInventory.RemoveItemsOfType((MyFixedPoint)d, myObjectBuilder_PhysicalObject, spawn: false, onlyWhole: false);
					MyFixedPoint itemAmount = base.InputInventory.GetItemAmount(item.Id);
					if (itemAmount < (MyFixedPoint)0.01f)
					{
						base.InputInventory.RemoveItemsOfType(itemAmount, item.Id);
					}
				}
				prerequisites = queueItem.Results;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					MyBlueprintDefinitionBase.Item item2 = prerequisites[i];
					MyObjectBuilder_PhysicalObject myObjectBuilder_PhysicalObject2 = MyObjectBuilderSerializer.CreateNewObject(item2.Id) as MyObjectBuilder_PhysicalObject;
					if (myObjectBuilder_PhysicalObject2 == null)
					{
						MyLog.Default.WriteLine("obResult shouldn't be null!!! " + this);
						continue;
					}
					float num = (float)item2.Amount * m_refineryDef.MaterialEfficiency * base.UpgradeValues["Effectiveness"];
					MyFixedPoint amount = (MyFixedPoint)((float)blueprintAmount * num);
					base.OutputInventory.AddItems(amount, myObjectBuilder_PhysicalObject2);
				}
				RemoveFirstQueueItemAnnounce(blueprintAmount);
			}
		}

		protected override float GetOperationalPowerConsumption()
		{
			return base.GetOperationalPowerConsumption() * (1f + base.UpgradeValues["Productivity"]) * (1f / base.UpgradeValues["PowerEfficiency"]);
		}
	}
}
