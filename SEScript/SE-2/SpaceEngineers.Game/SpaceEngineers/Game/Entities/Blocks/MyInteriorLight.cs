using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Lights;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Lights;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_InteriorLight))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyInteriorLight),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyInteriorLight)
	})]
	public class MyInteriorLight : MyLightingBlock, SpaceEngineers.Game.ModAPI.IMyInteriorLight, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyInteriorLight
	{
		private MyFlareDefinition m_flare;

		public override bool IsReflector => false;

		protected override bool SupportsFalloff => true;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
		}

		protected override void InitLight(MyLight light, Vector4 color, float radius, float falloff)
		{
			light.Start(color, radius, DisplayNameText);
			light.Falloff = falloff;
			UpdateGlare(light);
		}

		private void UpdateGlare(MyLight light)
		{
			light.GlareOn = light.LightOn;
			light.GlareIntensity = 0.4f;
			light.GlareQuerySize = 0.2f;
			light.GlareType = MyGlareTypeEnum.Normal;
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), base.BlockDefinition.Flare);
			m_flare = ((MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition) ?? new MyFlareDefinition());
			light.GlareSize = m_flare.Size;
			light.SubGlares = m_flare.SubGlares;
			UpdateIntensity();
		}

		protected override void UpdateEnabled(bool state)
		{
			foreach (MyLight light in m_lights)
			{
				light.LightOn = state;
				light.GlareOn = state;
			}
		}

		protected override void UpdateIntensity()
		{
			float num = base.CurrentLightPower * base.Intensity;
			foreach (MyLight light in m_lights)
			{
				light.Intensity = num * 2f;
				float num2 = m_flare.Intensity * num;
				if (num2 < m_flare.Intensity)
				{
					num2 = m_flare.Intensity;
				}
				light.GlareIntensity = num2;
			}
			base.BulbColor = ComputeBulbColor();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			foreach (MyLight light in m_lights)
			{
				UpdateGlare(light);
				light.UpdateLight();
			}
			UpdateEmissivity(force: true);
		}

		protected override void UpdateEmissivity(bool force = false)
		{
			if (m_lights != null)
			{
				base.UpdateEmissivity(force);
				foreach (MyLight light in m_lights)
				{
					MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], light.LightOn ? light.Intensity : 0f, Color.Lerp(base.Color, base.Color.ToGray(), 0.5f), Color.Black);
				}
			}
		}
	}
}
