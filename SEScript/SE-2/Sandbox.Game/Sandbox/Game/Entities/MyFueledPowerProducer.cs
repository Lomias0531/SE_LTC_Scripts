using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_FueledPowerProducer))]
	public abstract class MyFueledPowerProducer : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		protected class m_capacity_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType capacity;
				ISyncType result = capacity = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyFueledPowerProducer)P_0).m_capacity = (Sync<float, SyncDirection.FromServer>)capacity;
				return result;
			}
		}

		public static float FUEL_CONSUMPTION_MULTIPLIER = 1f;

		private MyResourceSourceComponent m_sourceComponent;

		private readonly Sync<float, SyncDirection.FromServer> m_capacity;

		public new MyFueledPowerProducerDefinition BlockDefinition => (MyFueledPowerProducerDefinition)base.BlockDefinition;

		public MyResourceSourceComponent SourceComp
		{
			get
			{
				return m_sourceComponent;
			}
			set
			{
				if (m_sourceComponent != null)
				{
					m_sourceComponent.OutputChanged -= OnCurrentOrMaxOutputChanged;
					m_sourceComponent.MaxOutputChanged -= OnCurrentOrMaxOutputChanged;
				}
				MyEntityComponentContainer components = base.Components;
				if (ContainsDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerSource)))
				{
					RemoveDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerSource));
				}
				m_sourceComponent = value;
				components.Remove<MyResourceSourceComponent>();
				components.Add(value);
				if (m_sourceComponent != null)
				{
					AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerSource(m_sourceComponent, this));
					m_sourceComponent.OutputChanged += OnCurrentOrMaxOutputChanged;
					m_sourceComponent.MaxOutputChanged += OnCurrentOrMaxOutputChanged;
				}
			}
		}

		public float Capacity
		{
			get
			{
				return m_capacity.Value;
			}
			set
			{
				m_capacity.Value = Math.Max(value, 0f);
			}
		}

		public bool IsSupplied => Capacity > 0f;

		public float CurrentOutput => SourceComp.CurrentOutput;

		public virtual float MaxOutput => BlockDefinition.MaxPowerOutput;

		public IMyConveyorEndpoint ConveyorEndpoint
		{
			get;
			private set;
		}

		protected MyFueledPowerProducer()
		{
			SourceComp = new MyResourceSourceComponent();
			m_capacity.ValueChanged += OnCapacityChanged;
			m_capacity.AlwaysReject();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo
			{
				DefinedOutput = BlockDefinition.MaxPowerOutput,
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				ProductionToCapacityMultiplier = BlockDefinition.FuelProductionToCapacityMultiplier
			});
			SourceComp.Enabled = base.Enabled;
			base.Init(objectBuilder, cubeGrid);
			SlimBlock.ComponentStack.IsFunctionalChanged += OnIsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			MyObjectBuilder_FueledPowerProducer myObjectBuilder_FueledPowerProducer = (MyObjectBuilder_FueledPowerProducer)objectBuilder;
			m_capacity.SetLocalValue(myObjectBuilder_FueledPowerProducer.Capacity);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_FueledPowerProducer obj = (MyObjectBuilder_FueledPowerProducer)base.GetObjectBuilderCubeBlock(copy);
			obj.Capacity = Capacity;
			return obj;
		}

		protected override bool CheckIsWorking()
		{
			MyResourceSourceComponent sourceComp = SourceComp;
			if (sourceComp.Enabled && IsSupplied && sourceComp.ProductionEnabled)
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.IsWorking)
			{
				OnStartWorking();
			}
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		private void OnIsFunctionalChanged()
		{
			OnProductionChanged();
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			else
			{
				OnStopWorking();
			}
		}

		protected virtual void OnCapacityChanged(SyncBase obj)
		{
			SourceComp.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, Capacity);
			UpdateIsWorking();
			OnProductionChanged();
		}

		protected override void OnEnabledChanged()
		{
			SourceComp.Enabled = base.Enabled;
			base.OnEnabledChanged();
			UpdateIsWorking();
			OnProductionChanged();
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			OnProductionChanged();
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			OnProductionChanged();
		}

		protected virtual void OnProductionChanged()
		{
			float maxOutput = 0f;
			if (base.Enabled && base.IsFunctional && IsSupplied)
			{
				maxOutput = ComputeMaxProduction();
			}
			SourceComp.SetMaxOutput(maxOutput);
		}

		protected virtual float ComputeMaxProduction()
		{
			if (CheckIsWorking() || (MySession.Static.CreativeMode && base.CheckIsWorking()))
			{
				return MaxOutput;
			}
			return 0f;
		}

		protected virtual void OnCurrentOrMaxOutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!source.CurrentOutputByType(changedResourceId).IsEqual(oldOutput, oldOutput * 0.1f))
			{
				UpdateDisplay();
			}
		}

		protected void UpdateDisplay()
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder sb)
		{
			base.UpdateDetailedInfo(sb);
			MyResourceSourceComponent sourceComp = SourceComp;
			sb.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			sb.Append(BlockDefinition.DisplayNameText);
			sb.Append('\n');
			sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
			MyValueFormatter.AppendWorkInBestUnit(MaxOutput, sb);
			sb.Append('\n');
			sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
			MyValueFormatter.AppendWorkInBestUnit(sourceComp.CurrentOutput, sb);
			sb.Append('\n');
		}

		public void InitializeConveyorEndpoint()
		{
			ConveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(ConveyorEndpoint));
		}

		public virtual bool AllowSelfPulling()
		{
			return false;
		}

		public virtual PullInformation GetPullInformation()
		{
			return null;
		}

		public virtual PullInformation GetPushInformation()
		{
			return null;
		}
	}
}
