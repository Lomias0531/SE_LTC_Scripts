using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SolarPanel))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMySolarPanel),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMySolarPanel)
	})]
	public class MySolarPanel : MyEnvironmentalPowerProducer, SpaceEngineers.Game.ModAPI.IMySolarPanel, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMySolarPanel
	{
		private static readonly string[] m_emissiveTextureNames = new string[4]
		{
			"Emissive0",
			"Emissive1",
			"Emissive2",
			"Emissive3"
		};

		public MySolarPanelDefinition SolarPanelDefinition
		{
			get;
			private set;
		}

		public MySolarGameLogicComponent SolarComponent
		{
			get;
			private set;
		}

		protected override float CurrentProductionRatio => SolarComponent.MaxOutput;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			SolarPanelDefinition = (MySolarPanelDefinition)base.BlockDefinition;
			MySolarGameLogicComponent mySolarGameLogicComponent2 = (MySolarGameLogicComponent)(base.GameLogic = (SolarComponent = new MySolarGameLogicComponent()));
			SolarComponent.OnProductionChanged += OnProductionChanged;
			SolarComponent.Initialize(SolarPanelDefinition.PanelOrientation, SolarPanelDefinition.IsTwoSided, SolarPanelDefinition.PanelOffset, this);
			base.Init(objectBuilder, cubeGrid);
			AddDebugRenderComponent(new MyDebugRenderComponentSolarPanel(this));
		}

		protected override void OnProductionChanged()
		{
			base.OnProductionChanged();
			UpdateEmissivity();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateEmissivity();
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		public override void SetDamageEffect(bool show)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				base.SetDamageEffect(show);
				if (m_soundEmitter != null && base.BlockDefinition.DamagedSound != null && !show && (m_soundEmitter.SoundId == base.BlockDefinition.DamagedSound.Arcade || m_soundEmitter.SoundId != base.BlockDefinition.DamagedSound.Realistic))
				{
					m_soundEmitter.StopSound(forced: false);
				}
			}
		}

		protected void UpdateEmissivity()
		{
			if (!base.InScene)
			{
				return;
			}
			Color emissivePartColor = Color.Red;
			MyEmissiveColorStateResult result;
			if (!base.IsFunctional)
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				for (int i = 0; i < 4; i++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], emissivePartColor, 0f);
				}
			}
			else if (!base.IsWorking)
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				for (int j = 0; j < 4; j++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[j], emissivePartColor, 1f);
				}
			}
			else if (base.SourceComp.MaxOutput > 0f)
			{
				for (int k = 0; k < 4; k++)
				{
					if ((float)k < base.SourceComp.MaxOutput / base.BlockDefinition.MaxPowerOutput * 4f)
					{
						emissivePartColor = Color.Green;
						if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
						MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[k], emissivePartColor, 1f);
					}
					else
					{
						emissivePartColor = Color.Black;
						if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
						{
							emissivePartColor = result.EmissiveColor;
						}
						MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[k], emissivePartColor, 1f);
					}
				}
			}
			else
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[0], emissivePartColor, 1f);
				for (int l = 1; l < 4; l++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[l], emissivePartColor, 1f);
				}
			}
		}
	}
}
