using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
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
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_OxygenGenerator))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyGasGenerator),
		typeof(Sandbox.ModAPI.Ingame.IMyGasGenerator),
		typeof(Sandbox.ModAPI.IMyOxygenGenerator),
		typeof(Sandbox.ModAPI.Ingame.IMyOxygenGenerator)
	})]
	public class MyGasGenerator : MyFunctionalBlock, IMyGasBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyOxygenGenerator, Sandbox.ModAPI.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyOxygenGenerator, IMyInventoryOwner, IMyEventProxy, IMyEventOwner
	{
		protected sealed class OnRefillCallback_003C_003E : ICallSite<MyGasGenerator, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyGasGenerator @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRefillCallback();
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyGasGenerator)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected class m_autoRefill_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoRefill;
				ISyncType result = autoRefill = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyGasGenerator)P_0).m_autoRefill = (Sync<bool, SyncDirection.BothWays>)autoRefill;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyGasGenerator_003C_003EActor : IActivator, IActivator<MyGasGenerator>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasGenerator();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasGenerator CreateInstance()
			{
				return new MyGasGenerator();
			}

			MyGasGenerator IActivator<MyGasGenerator>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private readonly Sync<bool, SyncDirection.BothWays> m_autoRefill;

		private bool m_isProducing;

		private MyInventoryConstraint m_oreConstraint;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		private MyResourceSourceComponent m_sourceComp;

		private float m_productionCapacityMultiplier = 1f;

		private float m_powerConsumptionMultiplier = 1f;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool CanPressurizeRoom => false;

		public bool CanProduce
		{
			get
			{
				if (((MySession.Static != null && MySession.Static.Settings.EnableOxygen) || !BlockDefinition.ProducedGases.TrueForAll((MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo info) => info.Id == m_oxygenGasId)) && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.IsWorking && base.Enabled)
				{
					return base.IsFunctional;
				}
				return false;
			}
		}

		public bool AutoRefill
		{
			get
			{
				return m_autoRefill;
			}
			set
			{
				m_autoRefill.Value = value;
			}
		}

		public MyResourceSourceComponent SourceComp
		{
			get
			{
				return m_sourceComp;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceSourceComponent)))
				{
					base.Components.Remove<MyResourceSourceComponent>();
				}
				base.Components.Add(value);
				m_sourceComp = value;
			}
		}

		private new MyOxygenGeneratorDefinition BlockDefinition => (MyOxygenGeneratorDefinition)base.BlockDefinition;

		public bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		float Sandbox.ModAPI.IMyGasGenerator.ProductionCapacityMultiplier
		{
			get
			{
				return m_productionCapacityMultiplier;
			}
			set
			{
				m_productionCapacityMultiplier = value;
				if (m_productionCapacityMultiplier < 0.01f)
				{
					m_productionCapacityMultiplier = 0.01f;
				}
			}
		}

		float Sandbox.ModAPI.IMyGasGenerator.PowerConsumptionMultiplier
		{
			get
			{
				return m_powerConsumptionMultiplier;
			}
			set
			{
				m_powerConsumptionMultiplier = value;
				if (m_powerConsumptionMultiplier < 0.01f)
				{
					m_powerConsumptionMultiplier = 0.01f;
				}
				if (base.ResourceSink != null)
				{
					base.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, BlockDefinition.OperationalPowerConsumption * m_powerConsumptionMultiplier);
					base.ResourceSink.Update();
				}
			}
		}

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		public MyGasGenerator()
		{
			CreateTerminalControls();
			SourceComp = new MyResourceSourceComponent(2);
			base.ResourceSink = new MyResourceSinkComponent();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGasGenerator>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyGasGenerator> obj = new MyTerminalControlOnOffSwitch<MyGasGenerator>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyGasGenerator x) => x.UseConveyorSystem),
					Setter = delegate(MyGasGenerator x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyGasGenerator> obj2 = new MyTerminalControlButton<MyGasGenerator>("Refill", MySpaceTexts.BlockPropertyTitle_Refill, MySpaceTexts.BlockPropertyTitle_Refill, OnRefillButtonPressed)
				{
					Enabled = ((MyGasGenerator x) => x.CanRefill())
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlCheckbox<MyGasGenerator> obj3 = new MyTerminalControlCheckbox<MyGasGenerator>("Auto-Refill", MySpaceTexts.BlockPropertyTitle_AutoRefill, MySpaceTexts.BlockPropertyTitle_AutoRefill)
				{
					Getter = ((MyGasGenerator x) => x.AutoRefill),
					Setter = delegate(MyGasGenerator x, bool v)
					{
						x.AutoRefill = v;
					}
				};
				obj3.EnableAction();
				MyTerminalControlFactory.AddControl(obj3);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			List<MyResourceSourceInfo> list = new List<MyResourceSourceInfo>();
			foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in BlockDefinition.ProducedGases)
			{
				list.Add(new MyResourceSourceInfo
				{
					ResourceTypeId = producedGase.Id,
					DefinedOutput = BlockDefinition.IceConsumptionPerSecond * producedGase.IceToGasRatio * (MySession.Static.CreativeMode ? 10f : 1f),
					ProductionToCapacityMultiplier = 1f
				});
			}
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, list);
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_OxygenGenerator myObjectBuilder_OxygenGenerator = objectBuilder as MyObjectBuilder_OxygenGenerator;
			InitializeConveyorEndpoint();
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_OxygenGenerator.UseConveyorSystem);
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			MyInventory myInventory = this.GetInventory();
			if (myInventory == null)
			{
				myInventory = new MyInventory(BlockDefinition.InventoryMaxVolume, BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
				myInventory.Constraint = BlockDefinition.InputInventoryConstraint;
				base.Components.Add((MyInventoryBase)myInventory);
			}
			else
			{
				myInventory.Constraint = BlockDefinition.InputInventoryConstraint;
			}
			m_oreConstraint = new MyInventoryConstraint(myInventory.Constraint.Description, myInventory.Constraint.Icon, myInventory.Constraint.IsWhitelist);
			foreach (MyDefinitionId constrainedId in myInventory.Constraint.ConstrainedIds)
			{
				if (constrainedId.TypeId != typeof(MyObjectBuilder_GasContainerObject))
				{
					m_oreConstraint.Add(constrainedId);
				}
			}
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			AutoRefill = myObjectBuilder_OxygenGenerator.AutoRefill;
			SourceComp.Enabled = base.Enabled;
			if (Sync.IsServer)
			{
				SourceComp.OutputChanged += Source_OutputChanged;
			}
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = BlockDefinition.OperationalPowerConsumption,
				RequiredInputFunc = ComputeRequiredPower
			});
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			myInventory?.Init(myObjectBuilder_OxygenGenerator.Inventory);
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyGasGenerator_IsWorkingChanged;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_OxygenGenerator obj = (MyObjectBuilder_OxygenGenerator)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			obj.AutoRefill = AutoRefill;
			return obj;
		}

		public void RefillBottles()
		{
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				double num = 0.0;
				if (MySession.Static.CreativeMode)
				{
					num = 3.4028234663852886E+38;
				}
				else
				{
					foreach (MyPhysicalInventoryItem item in items)
					{
						if (!(item.Content is MyObjectBuilder_GasContainerObject))
						{
							num += IceToGas(ref gasId, (float)item.Amount) * (double)((Sandbox.ModAPI.IMyGasGenerator)this).ProductionCapacityMultiplier;
						}
					}
				}
				double num2 = 0.0;
				foreach (MyPhysicalInventoryItem item2 in items)
				{
					if (num <= 0.0)
					{
						return;
					}
					MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item2.Content as MyObjectBuilder_GasContainerObject;
					if (myObjectBuilder_GasContainerObject != null && !(myObjectBuilder_GasContainerObject.GasLevel >= 1f))
					{
						MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
						if (!(myOxygenContainerDefinition.StoredGasId != resourceType))
						{
							float num3 = myObjectBuilder_GasContainerObject.GasLevel * myOxygenContainerDefinition.Capacity;
							double num4 = Math.Min(myOxygenContainerDefinition.Capacity - num3, num);
							myObjectBuilder_GasContainerObject.GasLevel = (float)Math.Min(((double)num3 + num4) / (double)myOxygenContainerDefinition.Capacity, 1.0);
							num2 += num4;
							num -= num4;
						}
					}
				}
				if (num2 > 0.0)
				{
					ProduceGas(ref gasId, num2);
					this.GetInventory().UpdateGasAmount();
				}
			}
		}

		private static void OnRefillButtonPressed(MyGasGenerator generator)
		{
			if (generator.IsWorking)
			{
				generator.SendRefillRequest();
			}
		}

		private bool CanRefill()
		{
			if (!CanProduce || !HasIce())
			{
				return false;
			}
			foreach (MyPhysicalInventoryItem item in this.GetInventory().GetItems())
			{
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null && myObjectBuilder_GasContainerObject.GasLevel < 1f)
				{
					return true;
				}
			}
			return false;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			SetRemainingCapacities();
			base.ResourceSink.Update();
			if (MySession.Static != null && !MySession.Static.Settings.EnableOxygen)
			{
				m_sourceComp.SetMaxOutputByType(m_oxygenGasId, 0f);
			}
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				double gasAmount = GasOutputPerUpdate(ref gasId);
				ProduceGas(ref gasId, gasAmount);
			}
			SetEmissiveStateWorking();
			if (MyFakes.ENABLE_OXYGEN_SOUNDS)
			{
				UpdateSounds();
			}
			m_isProducing = false;
			foreach (MyDefinitionId resourceType2 in SourceComp.ResourceTypes)
			{
				m_isProducing |= (SourceComp.CurrentOutputByType(resourceType2) > 0f);
			}
			if (!m_isProducing && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateAfterSimulation100()
		{
			if (Sync.IsServer && base.IsWorking)
			{
				if ((bool)m_useConveyorSystem && this.GetInventory().VolumeFillFactor < 0.6f)
				{
					MyGridConveyorSystem.PullAllRequest(this, this.GetInventory(), base.OwnerId, HasIce() ? this.GetInventory().Constraint : m_oreConstraint, null, pullFullBottles: false);
				}
				if (AutoRefill && CanRefill())
				{
					RefillBottles();
				}
			}
			m_isProducing = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void UpdateSounds()
		{
			if (m_soundEmitter == null)
			{
				return;
			}
			if (base.IsWorking)
			{
				if (m_isProducing)
				{
					if (m_soundEmitter.SoundId != BlockDefinition.GenerateSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.GenerateSound.Realistic)
					{
						m_soundEmitter.PlaySound(BlockDefinition.GenerateSound, stopPrevious: true);
					}
				}
				else if (m_soundEmitter.SoundId != BlockDefinition.IdleSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.IdleSound.Realistic && (m_soundEmitter.SoundId == BlockDefinition.GenerateSound.Arcade || m_soundEmitter.SoundId == BlockDefinition.GenerateSound.Realistic) && m_soundEmitter.Loop)
				{
					m_soundEmitter.StopSound(forced: false);
				}
				if (!m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.PlaySound(BlockDefinition.IdleSound, stopPrevious: true);
				}
			}
			else if (m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private float ComputeRequiredPower()
		{
			if ((!MySession.Static.Settings.EnableOxygen && BlockDefinition.ProducedGases.TrueForAll((MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo info) => info.Id == m_oxygenGasId)) || !base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			bool flag = false;
			foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in BlockDefinition.ProducedGases)
			{
				flag = (flag || (SourceComp.CurrentOutputByType(producedGase.Id) > 0f && (MySession.Static.Settings.EnableOxygen || producedGase.Id != m_oxygenGasId)));
			}
			return (flag ? BlockDefinition.OperationalPowerConsumption : BlockDefinition.StandbyPowerConsumption) * m_powerConsumptionMultiplier;
		}

		private void SetRemainingCapacities()
		{
			float num = IceAmount();
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				m_sourceComp.SetRemainingCapacityByType(resourceType, (float)IceToGas(ref gasId, num));
			}
		}

		private void Inventory_ContentsChanged(MyInventoryBase obj)
		{
			SetRemainingCapacities();
			RaisePropertiesChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void MyGasGenerator_IsWorkingChanged(MyCubeBlock obj)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					SourceComp.Enabled = CanProduce;
					SetEmissiveStateWorking();
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}, "MyGasGenerator_IsWorkingChanged");
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			SourceComp.Enabled = CanProduce;
			base.ResourceSink.Update();
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				base.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = CanProduce;
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void Source_OutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!BlockDefinition.ProducedGases.TrueForAll((MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo info) => info.Id != changedResourceId))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			SetEmissiveStateWorking();
		}

		public override bool SetEmissiveStateWorking()
		{
			if (CanProduce)
			{
				if (this.GetInventory() == null)
				{
					return false;
				}
				if (!this.GetInventory().FindItem((MyPhysicalInventoryItem item) => !(item.Content is MyObjectBuilder_GasContainerObject)).HasValue)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
				}
				if (m_isProducing)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			if (!MySession.Static.Settings.EnableOxygen)
			{
				detailedInfo.Append("\n");
				detailedInfo.Append("Oxygen disabled in world settings!");
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			CheckEmissiveState(force: true);
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		public void SetInventory(MyInventory inventory, int index)
		{
			throw new NotImplementedException("TODO Dusan inventory sync");
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null && MyPerGameSettings.InventoryMass)
			{
				this.GetInventory().ContentsChanged += Inventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null && MyPerGameSettings.InventoryMass)
			{
				myInventory.ContentsChanged -= Inventory_ContentsChanged;
			}
		}

		bool IMyGasBlock.IsWorking()
		{
			return CanProduce;
		}

		private float IceAmount()
		{
			if (MySession.Static.CreativeMode)
			{
				return 10000f;
			}
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			MyFixedPoint fp = 0;
			foreach (MyPhysicalInventoryItem item in items)
			{
				if (!(item.Content is MyObjectBuilder_GasContainerObject))
				{
					fp += item.Amount;
				}
			}
			return (float)fp;
		}

		private bool HasIce()
		{
			if (MySession.Static.CreativeMode)
			{
				return true;
			}
			foreach (MyPhysicalInventoryItem item in this.GetInventory().GetItems())
			{
				if (!(item.Content is MyObjectBuilder_GasContainerObject))
				{
					return true;
				}
			}
			return false;
		}

		private void ProduceGas(ref MyDefinitionId gasId, double gasAmount)
		{
			if (!(gasAmount <= 0.0))
			{
				double iceAmount = GasToIce(ref gasId, gasAmount);
				ConsumeFuel(ref gasId, iceAmount);
			}
		}

		private void ConsumeFuel(ref MyDefinitionId gasTypeId, double iceAmount)
		{
			if (!Sync.IsServer || base.CubeGrid.GridSystems.ControlSystem == null || iceAmount <= 0.0 || MySession.Static.CreativeMode)
			{
				return;
			}
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			if (items.Count <= 0 || !(iceAmount > 0.0))
			{
				return;
			}
			int num = 0;
			MyPhysicalInventoryItem myPhysicalInventoryItem;
			while (true)
			{
				if (num >= items.Count)
				{
					return;
				}
				myPhysicalInventoryItem = items[num];
				if (myPhysicalInventoryItem.Content is MyObjectBuilder_GasContainerObject)
				{
					num++;
					continue;
				}
				if (iceAmount < (double)(float)myPhysicalInventoryItem.Amount)
				{
					break;
				}
				iceAmount -= (double)(float)myPhysicalInventoryItem.Amount;
				this.GetInventory().RemoveItems(myPhysicalInventoryItem.ItemId);
			}
			MyFixedPoint value = MyFixedPoint.Max((MyFixedPoint)iceAmount, MyFixedPoint.SmallestPossibleValue);
			this.GetInventory().RemoveItems(myPhysicalInventoryItem.ItemId, value);
		}

		private double GasOutputPerSecond(ref MyDefinitionId gasId)
		{
			return SourceComp.CurrentOutputByType(gasId) * ((Sandbox.ModAPI.IMyGasGenerator)this).ProductionCapacityMultiplier;
		}

		private double GasOutputPerUpdate(ref MyDefinitionId gasId)
		{
			return GasOutputPerSecond(ref gasId) * 0.01666666753590107;
		}

		private double IceToGas(ref MyDefinitionId gasId, double iceAmount)
		{
			return iceAmount * IceToGasRatio(ref gasId);
		}

		private double GasToIce(ref MyDefinitionId gasId, double gasAmount)
		{
			return gasAmount / IceToGasRatio(ref gasId);
		}

		private double IceToGasRatio(ref MyDefinitionId gasId)
		{
			return SourceComp.DefinedOutputByType(gasId) / BlockDefinition.IceConsumptionPerSecond;
		}

		public void SendRefillRequest()
		{
			MyMultiplayer.RaiseEvent(this, (MyGasGenerator x) => x.OnRefillCallback);
		}

		[Event(null, 749)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnRefillCallback()
		{
			RefillBottles();
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public PullInformation GetPullInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
