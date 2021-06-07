using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_OxygenFarm))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyOxygenFarm),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm)
	})]
	public class MyOxygenFarm : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyOxygenFarm, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm, IMyGasBlock, IMyConveyorEndpointBlock
	{
		private static readonly string[] m_emissiveTextureNames = new string[4]
		{
			"Emissive0",
			"Emissive1",
			"Emissive2",
			"Emissive3"
		};

		private float m_maxGasOutputFactor;

		private bool firstUpdate = true;

		private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		private MyResourceSourceComponent m_sourceComp;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		public new MyOxygenFarmDefinition BlockDefinition => base.BlockDefinition as MyOxygenFarmDefinition;

		public MySolarGameLogicComponent SolarComponent
		{
			get;
			private set;
		}

		public bool CanProduce
		{
			get
			{
				if ((MySession.Static.Settings.EnableOxygen || BlockDefinition.ProducedGas != m_oxygenGasId) && base.Enabled && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.IsWorking)
				{
					return base.IsFunctional;
				}
				return false;
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

		public bool CanPressurizeRoom => false;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm.CanProduce => CanProduce;

		public MyOxygenFarm()
		{
			base.ResourceSink = new MyResourceSinkComponent();
			SourceComp = new MyResourceSourceComponent();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			base.IsWorkingChanged += OnIsWorkingChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			InitializeConveyorEndpoint();
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo
			{
				ResourceTypeId = BlockDefinition.ProducedGas,
				DefinedOutput = BlockDefinition.MaxGasOutput,
				ProductionToCapacityMultiplier = 1f,
				IsInfiniteCapacity = true
			});
			SourceComp.Enabled = base.IsWorking;
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = BlockDefinition.OperationalPowerConsumption,
				RequiredInputFunc = ComputeRequiredPower
			});
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink.Update();
			base.GameLogic = new MySolarGameLogicComponent();
			SolarComponent = (base.GameLogic as MySolarGameLogicComponent);
			SolarComponent.Initialize(BlockDefinition.PanelOrientation, BlockDefinition.IsTwoSided, BlockDefinition.PanelOffset, this);
			AddDebugRenderComponent(new MyDebugRenderComponentSolarPanel(this));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_OnIsFunctionalChanged;
			OnModelChange();
			SetDetailedInfoDirty();
			UpdateEmissivity();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			return base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_OxygenFarm;
		}

		private float ComputeRequiredPower()
		{
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return BlockDefinition.OperationalPowerConsumption;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = base.IsWorking;
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void ComponentStack_OnIsFunctionalChanged()
		{
			SourceComp.Enabled = base.IsWorking;
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void OnIsWorkingChanged(MyCubeBlock obj)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				SourceComp.Enabled = base.IsWorking;
				UpdateEmissivity();
			}, "MyOxygenFarm::OnIsWorkingChanged");
		}

		protected override bool CheckIsWorking()
		{
			if ((MySession.Static.Settings.EnableOxygen || BlockDefinition.ProducedGas != m_oxygenGasId) && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
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
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_OxygenOutput));
			detailedInfo.Append((SourceComp.MaxOutputByType(BlockDefinition.ProducedGas) * 60f).ToString("F"));
			detailedInfo.Append(" L/min");
		}

		private void UpdateEmissivity()
		{
			if (!base.InScene)
			{
				return;
			}
			Color emissivePartColor = Color.Red;
			MyEmissiveColorStateResult result;
			if (!base.IsWorking)
			{
				if (base.IsFunctional)
				{
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
					{
						emissivePartColor = result.EmissiveColor;
					}
					for (int i = 0; i < 4; i++)
					{
						MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], emissivePartColor, 1f);
					}
					return;
				}
				emissivePartColor = Color.Black;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[0], emissivePartColor, 0f);
				for (int j = 1; j < 4; j++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[j], emissivePartColor, 0f);
				}
			}
			else if (m_maxGasOutputFactor > 0f)
			{
				for (int k = 0; k < 4; k++)
				{
					if ((float)k < m_maxGasOutputFactor * 4f)
					{
						emissivePartColor = Color.Green;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
						MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[k], emissivePartColor, 1f);
					}
					else
					{
						emissivePartColor = Color.Black;
						if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
						MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[k], emissivePartColor, 1f);
					}
				}
			}
			else
			{
				emissivePartColor = Color.Black;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[0], emissivePartColor, 0f);
				for (int l = 1; l < 4; l++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[l], emissivePartColor, 0f);
				}
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateIsWorking();
			UpdateEmissivity();
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (base.CubeGrid.Physics != null)
			{
				base.ResourceSink.Update();
				float num = (base.IsWorking && SourceComp.ProductionEnabledByType(BlockDefinition.ProducedGas)) ? SolarComponent.MaxOutput : 0f;
				if (num != m_maxGasOutputFactor || firstUpdate)
				{
					m_maxGasOutputFactor = num;
					SourceComp.SetMaxOutputByType(BlockDefinition.ProducedGas, SourceComp.DefinedOutputByType(BlockDefinition.ProducedGas) * m_maxGasOutputFactor);
					UpdateVisual();
					SetDetailedInfoDirty();
					RaisePropertiesChanged();
					UpdateEmissivity();
					firstUpdate = false;
				}
				base.ResourceSink.Update();
			}
		}

		bool IMyGasBlock.IsWorking()
		{
			if (MySession.Static.Settings.EnableOxygen && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.IsWorking)
			{
				return base.IsFunctional;
			}
			return false;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm.GetOutput()
		{
			if (base.IsWorking)
			{
				return SolarComponent.MaxOutput;
			}
			return 0f;
		}

		public PullInformation GetPullInformation()
		{
			return null;
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
