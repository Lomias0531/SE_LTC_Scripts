using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ReflectorLight))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyReflectorLight),
		typeof(Sandbox.ModAPI.Ingame.IMyReflectorLight)
	})]
	public class MyReflectorLight : MyLightingBlock, Sandbox.ModAPI.IMyReflectorLight, Sandbox.ModAPI.IMyLightingBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLightingBlock, Sandbox.ModAPI.Ingame.IMyReflectorLight
	{
		protected class m_rotationSpeedForSubparts_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType rotationSpeedForSubparts;
				ISyncType result = rotationSpeedForSubparts = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyReflectorLight)P_0).m_rotationSpeedForSubparts = (Sync<float, SyncDirection.BothWays>)rotationSpeedForSubparts;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyReflectorLight_003C_003EActor : IActivator, IActivator<MyReflectorLight>
		{
			private sealed override object CreateInstance()
			{
				return new MyReflectorLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyReflectorLight CreateInstance()
			{
				return new MyReflectorLight();
			}

			MyReflectorLight IActivator<MyReflectorLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyFlareDefinition m_flare;

		private readonly Sync<float, SyncDirection.BothWays> m_rotationSpeedForSubparts;

		private Matrix m_rotationMatrix = Matrix.Identity;

		private static readonly Color COLOR_OFF = new Color(30, 30, 30);

		private bool m_wasWorking = true;

		private float GlareQuerySizeDef => base.CubeGrid.GridScale * (base.IsLargeLight ? 0.5f : 0.1f);

		public override bool IsReflector => true;

		public bool IsReflectorEnabled
		{
			get
			{
				if (m_lights.Count <= 0)
				{
					return false;
				}
				return m_lights[0].ReflectorOn;
			}
		}

		protected override bool SupportsFalloff => false;

		public string ReflectorConeMaterial => BlockDefinition.ReflectorConeMaterial;

		protected override bool NeedPerFrameUpdate => base.NeedPerFrameUpdate | ((float)m_rotationSpeedForSubparts > 0f && base.CurrentLightPower > 0f);

		public new MyReflectorBlockDefinition BlockDefinition
		{
			get
			{
				if (base.BlockDefinition is MyReflectorBlockDefinition)
				{
					return (MyReflectorBlockDefinition)base.BlockDefinition;
				}
				SlimBlock.BlockDefinition = new MyReflectorBlockDefinition();
				return (MyReflectorBlockDefinition)base.BlockDefinition;
			}
		}

		public MyReflectorLight()
		{
			base.Render = new MyRenderComponentReflectorLight(m_lights);
			m_rotationSpeedForSubparts.ValueChanged += delegate
			{
				RotationSpeedChanged();
			};
		}

		private void RotationSpeedChanged()
		{
			m_rotationMatrix = Matrix.CreateRotationZ(m_rotationSpeedForSubparts);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_ReflectorLight myObjectBuilder_ReflectorLight = (MyObjectBuilder_ReflectorLight)objectBuilder;
			m_rotationSpeedForSubparts.SetLocalValue(BlockDefinition.RotationSpeedBounds.Clamp((myObjectBuilder_ReflectorLight.RotationSpeed == -1f) ? BlockDefinition.RotationSpeedBounds.Default : myObjectBuilder_ReflectorLight.RotationSpeed));
		}

		protected override void InitLight(MyLight light, Vector4 color, float radius, float falloff)
		{
			light.Start(color, base.CubeGrid.GridScale * radius, DisplayNameText);
			light.ReflectorOn = true;
			light.LightType = MyLightType.SPOTLIGHT;
			light.ReflectorTexture = BlockDefinition.ReflectorTexture;
			light.Falloff = 0.3f;
			light.GlossFactor = 0f;
			light.ReflectorGlossFactor = 1f;
			light.ReflectorFalloff = 0.5f;
			light.GlareOn = light.LightOn;
			light.GlareQuerySize = GlareQuerySizeDef;
			light.GlareType = MyGlareTypeEnum.Directional;
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), BlockDefinition.Flare);
			m_flare = ((MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition) ?? new MyFlareDefinition());
			light.GlareSize = m_flare.Size;
			light.SubGlares = m_flare.SubGlares;
			UpdateIntensity();
			base.Render.NeedsDrawFromParent = true;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyReflectorLight>())
			{
				base.CreateTerminalControls();
				MyTerminalControlSlider<MyReflectorLight> myTerminalControlSlider = new MyTerminalControlSlider<MyReflectorLight>("RotationSpeed", MySpaceTexts.BlockPropertyTitle_LightReflectorRotationSpeed, MySpaceTexts.BlockPropertyTitle_LightReflectorRotationSpeed);
				myTerminalControlSlider.SetLimits((MyReflectorLight x) => x.BlockDefinition.RotationSpeedBounds.Min, (MyReflectorLight x) => x.BlockDefinition.RotationSpeedBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = ((MyReflectorLight x) => x.BlockDefinition.RotationSpeedBounds.Default);
				myTerminalControlSlider.Getter = ((MyReflectorLight x) => x.m_rotationSpeedForSubparts);
				myTerminalControlSlider.Setter = delegate(MyReflectorLight x, float v)
				{
					x.m_rotationSpeedForSubparts.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyReflectorLight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_rotationSpeedForSubparts, 2));
				};
				myTerminalControlSlider.Visible = ((MyReflectorLight x) => x.Subparts.Count > 0);
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ReflectorLight obj = (MyObjectBuilder_ReflectorLight)base.GetObjectBuilderCubeBlock(copy);
			obj.RotationSpeed = m_rotationSpeedForSubparts;
			return obj;
		}

		protected override void UpdateEnabled(bool state)
		{
			if (m_lights != null)
			{
				bool flag = state && base.CubeGrid.Projector == null;
				foreach (MyLight light in m_lights)
				{
					light.ReflectorOn = flag;
					light.LightOn = flag;
					light.GlareOn = flag;
				}
			}
		}

		protected override void UpdateIntensity()
		{
			float num = base.CurrentLightPower * base.Intensity;
			foreach (MyLight light in m_lights)
			{
				light.ReflectorIntensity = num * 8f;
				light.Intensity = num * 0.3f;
				float num2 = num / base.IntensityBounds.Max;
				float num3 = m_flare.Intensity * num;
				if (num3 < m_flare.Intensity)
				{
					num3 = m_flare.Intensity;
				}
				light.GlareIntensity = num3;
				float scaleFactor = num2 / 2f + 0.5f;
				light.GlareSize = m_flare.Size * scaleFactor;
				base.BulbColor = ComputeBulbColor();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!((float)m_rotationSpeedForSubparts > 0f) || !(base.CurrentLightPower > 0f))
			{
				return;
			}
			m_positionDirty = true;
			for (int i = 0; i < m_lightLocalData.Count; i++)
			{
				if (m_lightLocalData[i].Subpart != null)
				{
					m_lightLocalData[i].Subpart.PositionComp.LocalMatrix = m_rotationMatrix * m_lightLocalData[i].Subpart.PositionComp.LocalMatrix;
				}
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity(force: true);
		}

		protected override void UpdateRadius(float value)
		{
			base.UpdateRadius(value);
			base.Radius = 10f * (base.ReflectorRadius / base.ReflectorRadiusBounds.Max);
		}

		protected override void UpdateEmissivity(bool force = false)
		{
			bool flag = m_lights.Count > 0 && m_lights[0].ReflectorOn;
			if (m_lights != null && (m_wasWorking != (base.IsWorking && flag) || force))
			{
				m_wasWorking = (base.IsWorking && flag);
				if (m_wasWorking)
				{
					MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 1f, base.Color, Color.White);
				}
				else
				{
					MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 0f, COLOR_OFF, Color.White);
				}
			}
		}
	}
}
