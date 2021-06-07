using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Reactor))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyReactor),
		typeof(Sandbox.ModAPI.Ingame.IMyReactor)
	})]
	public class MyReactor : MyFueledPowerProducer, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyReactor, Sandbox.ModAPI.Ingame.IMyReactor, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyInventoryOwner
	{
		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyReactor)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyReactor_003C_003EActor : IActivator, IActivator<MyReactor>
		{
			private sealed override object CreateInstance()
			{
				return new MyReactor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyReactor CreateInstance()
			{
				return new MyReactor();
			}

			MyReactor IActivator<MyReactor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private float m_powerOutputMultiplier = 1f;

		public new MyReactorDefinition BlockDefinition => (MyReactorDefinition)base.BlockDefinition;

		public override float MaxOutput => base.MaxOutput * m_powerOutputMultiplier;

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

		bool Sandbox.ModAPI.Ingame.IMyReactor.UseConveyorSystem
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

		float Sandbox.ModAPI.IMyReactor.PowerOutputMultiplier
		{
			get
			{
				return m_powerOutputMultiplier;
			}
			set
			{
				m_powerOutputMultiplier = value;
				if (m_powerOutputMultiplier < 0.01f)
				{
					m_powerOutputMultiplier = 0.01f;
				}
				OnProductionChanged();
			}
		}

		public MyReactor()
		{
			CreateTerminalControls();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyReactor>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyReactor> obj = new MyTerminalControlOnOffSwitch<MyReactor>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyReactor x) => x.UseConveyorSystem),
					Setter = delegate(MyReactor x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_Reactor myObjectBuilder_Reactor = (MyObjectBuilder_Reactor)objectBuilder;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			MyInventory myInventory = this.GetInventory();
			if (myInventory == null)
			{
				myInventory = new MyInventory(BlockDefinition.InventoryMaxVolume, BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
				base.Components.Add((MyInventoryBase)myInventory);
				myInventory.Init(myObjectBuilder_Reactor.Inventory);
			}
			myInventory.Constraint = BlockDefinition.InventoryConstraint;
			if (Sync.IsServer)
			{
				RefreshRemainingCapacity();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_Reactor.UseConveyorSystem);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Reactor obj = (MyObjectBuilder_Reactor)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.Enabled && !MySession.Static.CreativeMode && Sync.IsServer && base.IsWorking)
			{
				int timeDeltaMilliseconds = 1600;
				ConsumeFuel(timeDeltaMilliseconds);
			}
			if (Sync.IsServer && base.IsFunctional && (bool)m_useConveyorSystem && this.GetInventory().VolumeFillFactor < 0.6f)
			{
				MyReactorDefinition.FuelInfo[] fuelInfos = BlockDefinition.FuelInfos;
				for (int i = 0; i < fuelInfos.Length; i++)
				{
					MyReactorDefinition.FuelInfo fuelInfo = fuelInfos[i];
					float d = fuelInfo.ConsumptionPerSecond_Items * 60f;
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(fuelInfo.FuelId, (MyFixedPoint)d, this, this.GetInventory(), remove: false, calcImmediately: false);
				}
			}
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		private void ConsumeFuel(int timeDeltaMilliseconds)
		{
			if (!base.SourceComp.HasCapacityRemaining)
			{
				return;
			}
			float currentOutput = base.SourceComp.CurrentOutput;
			if (currentOutput == 0f)
			{
				return;
			}
			MyInventory inventory = this.GetInventory();
			float num = currentOutput / BlockDefinition.MaxPowerOutput;
			MyReactorDefinition.FuelInfo[] fuelInfos = BlockDefinition.FuelInfos;
			for (int i = 0; i < fuelInfos.Length; i++)
			{
				MyReactorDefinition.FuelInfo fuelInfo = fuelInfos[i];
				float num2 = num * fuelInfo.ConsumptionPerSecond_Items / 1000f;
				MyFixedPoint myFixedPoint = (MyFixedPoint)((float)timeDeltaMilliseconds * num2);
				if (myFixedPoint == 0)
				{
					myFixedPoint = MyFixedPoint.SmallestPossibleValue;
				}
				MyFixedPoint amount = MyFixedPoint.Min(inventory.GetItemAmount(fuelInfo.FuelId), myFixedPoint);
				inventory.RemoveItemsOfType(amount, fuelInfo.FuelId);
				if (MyFakes.ENABLE_INFINITE_REACTOR_FUEL && !inventory.ContainItems(myFixedPoint, fuelInfo.FuelId))
				{
					inventory.AddItems(50 * myFixedPoint, fuelInfo.FuelItem);
				}
			}
		}

		private void RefreshRemainingCapacity()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory != null && Sync.IsServer)
			{
				float num = float.MaxValue;
				MyReactorDefinition.FuelInfo[] fuelInfos = BlockDefinition.FuelInfos;
				for (int i = 0; i < fuelInfos.Length; i++)
				{
					MyReactorDefinition.FuelInfo fuelInfo = fuelInfos[i];
					float val = (float)inventory.GetItemAmount(fuelInfo.FuelId) / fuelInfo.Ratio;
					num = Math.Min(num, val);
				}
				if (num == 0f && MySession.Static.CreativeMode)
				{
					MyReactorDefinition.FuelInfo fuelInfo2 = BlockDefinition.FuelInfos[0];
					num = fuelInfo2.FuelDefinition.Mass / fuelInfo2.Ratio;
				}
				base.Capacity = num;
			}
		}

		protected override void OnCurrentOrMaxOutputChanged(MyDefinitionId resourceTypeId, float oldOutput, MyResourceSourceComponent source)
		{
			base.OnCurrentOrMaxOutputChanged(resourceTypeId, oldOutput, source);
			if (base.SoundEmitter != null && base.SoundEmitter.Sound != null && base.SoundEmitter.Sound.IsPlaying)
			{
				if (base.SourceComp.MaxOutput != 0f)
				{
					float semitones = 4f * (base.SourceComp.CurrentOutput - 0.5f * base.SourceComp.MaxOutput) / base.SourceComp.MaxOutput;
					base.SoundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio(semitones);
				}
				else
				{
					base.SoundEmitter.Sound.FrequencyRatio = 1f;
				}
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			CheckEmissiveState(force: true);
		}

		public override void CheckEmissiveState(bool force = false)
		{
			if (base.IsWorking)
			{
				SetEmissiveStateWorking();
			}
			else if (base.IsFunctional)
			{
				if (base.Enabled)
				{
					SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0], "Emissive");
				}
				else
				{
					SetEmissiveStateDisabled();
				}
			}
			else
			{
				SetEmissiveStateDamaged();
			}
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (Sync.IsServer && this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += OnInventoryContentChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (Sync.IsServer && myInventory != null)
			{
				myInventory.ContentsChanged -= OnInventoryContentChanged;
			}
		}

		private void OnInventoryContentChanged(MyInventoryBase obj)
		{
			bool isWorking = base.IsWorking;
			RefreshRemainingCapacity();
			bool isWorking2 = base.IsWorking;
			if (isWorking != isWorking2)
			{
				if (isWorking2)
				{
					OnStartWorking();
				}
				else
				{
					OnStopWorking();
				}
			}
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public override PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				OwnerID = base.OwnerId,
				Inventory = this.GetInventory(),
				Constraint = BlockDefinition.InventoryConstraint
			};
		}

		int IMyInventoryOwner.get_InventoryCount()
		{
			return base.InventoryCount;
		}

		bool IMyInventoryOwner.get_HasInventory()
		{
			return base.HasInventory;
		}
	}
}
