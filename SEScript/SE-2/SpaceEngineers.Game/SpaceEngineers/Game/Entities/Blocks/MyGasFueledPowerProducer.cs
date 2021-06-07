using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	public abstract class MyGasFueledPowerProducer : MyFueledPowerProducer, IMyGasTank
	{
		private MyResourceSinkComponent m_sinkComponent;

		private bool m_needsUpdate;

		public new MyGasFueledPowerProducerDefinition BlockDefinition => (MyGasFueledPowerProducerDefinition)base.BlockDefinition;

		public MyResourceSinkComponent SinkComp
		{
			get
			{
				return m_sinkComponent;
			}
			set
			{
				if (m_sinkComponent != null)
				{
					m_sinkComponent.CurrentInputChanged -= OnFuelInputChanged;
				}
				MyEntityComponentContainer components = base.Components;
				m_sinkComponent = value;
				components.Remove<MyResourceSinkComponent>();
				components.Add(value);
				if (m_sinkComponent != null)
				{
					m_sinkComponent.CurrentInputChanged += OnFuelInputChanged;
				}
			}
		}

		public double FilledRatio => base.Capacity / BlockDefinition.FuelCapacity;

		public Action FilledRatioChanged
		{
			get;
			set;
		}

		public float GasCapacity => BlockDefinition.FuelCapacity;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyGasFueledPowerProducerDefinition.FuelInfo fuel = BlockDefinition.Fuel;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo
			{
				ResourceTypeId = fuel.FuelId,
				MaxRequiredInput = fuel.Ratio * BlockDefinition.FuelProductionToCapacityMultiplier * 2f
			});
			SinkComp = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			base.SourceComp.OutputChanged += OnElectricityOutputChanged;
			MarkForUpdate();
		}

		protected override void OnCapacityChanged(SyncBase obj)
		{
			base.OnCapacityChanged(obj);
			FilledRatioChanged.InvokeIfNotNull();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			MarkForUpdate();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			if (base.Enabled)
			{
				MarkForUpdate();
			}
			CheckEmissiveState();
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if (!base.Enabled || !base.IsFunctional)
			{
				foreach (MyDefinitionId acceptedResource in SinkComp.AcceptedResources)
				{
					SinkComp.SetRequiredInputByType(acceptedResource, 0f);
				}
			}
			DisableUpdate();
			CheckEmissiveState();
		}

		private void OnFuelInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			MarkForUpdate();
		}

		private void OnElectricityOutputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSourceComponent source)
		{
			MarkForUpdate();
		}

		private void MarkForUpdate()
		{
			if (!m_needsUpdate)
			{
				m_needsUpdate = true;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void DisableUpdate()
		{
			if (m_needsUpdate)
			{
				m_needsUpdate = false;
				if (!base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!m_needsUpdate)
			{
				return;
			}
			bool isWorking = base.IsWorking;
			UpdateCapacity();
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

		private void UpdateCapacity()
		{
			MyGasFueledPowerProducerDefinition blockDefinition = BlockDefinition;
			MyDefinitionId fuelId = blockDefinition.Fuel.FuelId;
			float num = base.SourceComp.CurrentOutput / blockDefinition.FuelProductionToCapacityMultiplier / 60f;
			float num2 = SinkComp.CurrentInputByType(fuelId) / 60f / MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER;
			if (num2 == 0f && MySession.Static.CreativeMode)
			{
				num2 = num + GetFillingOffset();
			}
			bool flag = num2 == 0f && SinkComp.RequiredInputByType(fuelId) > 0f;
			float num3 = num2 - num;
			bool num4 = num3 != 0f;
			if (num4)
			{
				if (Sync.IsServer)
				{
					base.Capacity += num3;
				}
				UpdateDisplay();
			}
			float fillingOffset = GetFillingOffset();
			if (!num4 && (flag || fillingOffset == 0f))
			{
				DisableUpdate();
			}
			float num5 = num + fillingOffset * MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER;
			SinkComp.SetRequiredInputByType(fuelId, num5 * 60f);
			CheckEmissiveState();
		}

		public override bool SetEmissiveStateWorking()
		{
			MyStringHash state = base.IsSupplied ? MyCubeBlock.m_emissiveNames.Working : MyCubeBlock.m_emissiveNames.Warning;
			return SetEmissiveState(state, base.Render.RenderObjectIDs[0]);
		}

		public override bool SetEmissiveStateDisabled()
		{
			if (base.Enabled)
			{
				MyStringHash state = base.IsSupplied ? MyCubeBlock.m_emissiveNames.Disabled : MyCubeBlock.m_emissiveNames.Warning;
				return SetEmissiveState(state, base.Render.RenderObjectIDs[0]);
			}
			return SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
		}

		private float GetFillingOffset()
		{
			if (base.Enabled && base.IsFunctional)
			{
				float capacity = base.Capacity;
				float fuelCapacity = BlockDefinition.FuelCapacity;
				return MathHelper.Clamp(fuelCapacity - capacity, 0f, fuelCapacity / 20f);
			}
			return 0f;
		}

		protected override void UpdateDetailedInfo(StringBuilder sb)
		{
			base.UpdateDetailedInfo(sb);
			float fuelCapacity = BlockDefinition.FuelCapacity;
			float num = Math.Min(base.Capacity, fuelCapacity);
			sb.Append(string.Format(arg0: (num / fuelCapacity * 100f).ToString("F1"), format: MyTexts.GetString(MySpaceTexts.Oxygen_Filled), arg1: num.ToString("0"), arg2: fuelCapacity.ToString("0")));
		}

		bool IMyGasTank.IsResourceStorage(MyDefinitionId resourceDefinition)
		{
			foreach (MyDefinitionId acceptedResource in SinkComp.AcceptedResources)
			{
				if (acceptedResource == resourceDefinition)
				{
					return true;
				}
			}
			return false;
		}
	}
}
