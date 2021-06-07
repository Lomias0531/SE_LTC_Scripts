using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Utils;

namespace SpaceEngineers.Game.Entities.Blocks
{
	public abstract class MyEnvironmentalPowerProducer : MyFunctionalBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		protected MySoundPair m_processSound = new MySoundPair();

		private MyResourceSourceComponent m_sourceComponent;

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
					m_sourceComponent.OutputChanged -= OnCurrentOutputChanged;
				}
				if (ContainsDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerSource)))
				{
					RemoveDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerSource));
				}
				if (base.Components.Contains(typeof(MyResourceSourceComponent)))
				{
					base.Components.Remove<MyResourceSourceComponent>();
				}
				base.Components.Add(value);
				m_sourceComponent = value;
				if (m_sourceComponent != null)
				{
					AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerSource(m_sourceComponent, this));
					m_sourceComponent.OutputChanged += OnCurrentOutputChanged;
				}
			}
		}

		public new MyPowerProducerDefinition BlockDefinition => (MyPowerProducerDefinition)base.BlockDefinition;

		public float CurrentOutput => SourceComp.CurrentOutput;

		public float MaxOutput => SourceComp.MaxOutput;

		protected abstract float CurrentProductionRatio
		{
			get;
		}

		protected MyEnvironmentalPowerProducer()
		{
			SourceComp = new MyResourceSourceComponent();
			base.IsWorkingChanged += OnIsWorkingChanged;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				DefinedOutput = BlockDefinition.MaxPowerOutput,
				IsInfiniteCapacity = true,
				ProductionToCapacityMultiplier = 3600f
			});
			m_processSound = BlockDefinition.ActionSound;
			SourceComp.SetMaxOutput(0f);
			base.Init(objectBuilder, cubeGrid);
		}

		protected void UpdateDisplay()
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		protected override void UpdateDetailedInfo(StringBuilder sb)
		{
			base.UpdateDetailedInfo(sb);
			float maxOutput = SourceComp.MaxOutput;
			float workInMegaWatts = Math.Min(maxOutput, SourceComp.CurrentOutput);
			sb.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			sb.Append(BlockDefinition.DisplayNameText);
			sb.Append('\n');
			sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
			MyValueFormatter.AppendWorkInBestUnit(maxOutput, sb);
			sb.Append('\n');
			sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
			MyValueFormatter.AppendWorkInBestUnit(workInMegaWatts, sb);
			sb.Append('\n');
		}

		private void OnIsWorkingChanged(MyCubeBlock obj)
		{
			OnProductionChanged();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = base.Enabled;
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
			if (!base.InScene || base.CubeGrid.IsPreview)
			{
				return;
			}
			float num = CurrentProductionRatio * BlockDefinition.MaxPowerOutput;
			SourceComp.SetMaxOutput(num);
			SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, num > 0f);
			UpdateDisplay();
			RaisePropertiesChanged();
			if (m_soundEmitter != null && m_processSound != null)
			{
				if (num > 0f)
				{
					m_soundEmitter.PlaySound(m_processSound, stopPrevious: true);
				}
				else
				{
					m_soundEmitter.StopSound(forced: true);
				}
			}
		}

		protected void OnCurrentOutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!source.CurrentOutputByType(changedResourceId).IsEqual(oldOutput, oldOutput * 0.1f))
			{
				UpdateDisplay();
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
				m_soundEmitter = null;
			}
		}
	}
}
