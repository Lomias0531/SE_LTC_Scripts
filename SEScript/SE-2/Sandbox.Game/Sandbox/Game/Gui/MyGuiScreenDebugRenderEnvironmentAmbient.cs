using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Environment Ambient")]
	internal class MyGuiScreenDebugRenderEnvironmentAmbient : MyGuiScreenDebugBase
	{
		private static float timeOfDay;

		private static TimeSpan? OriginalTime;

		private MyGuiControlSlider m_resolutionSlider;

		private MyGuiControlSlider m_resolutionFilteredSlider;

		public MyGuiScreenDebugRenderEnvironmentAmbient()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Environment Ambient", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Indirect light", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Diffuse factor", MySector.SunProperties.EnvironmentLight.AmbientDiffuseFactor, 0f, 5f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AmbientDiffuseFactor = v.Value;
			});
			AddCheckBox("Show Indirect Diffuse", MyRenderProxy.Settings.DisplayAmbientDiffuse, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAmbientDiffuse = x.IsChecked;
			});
			AddSlider("Specular factor", MySector.SunProperties.EnvironmentLight.AmbientSpecularFactor, 0f, 15f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AmbientSpecularFactor = v.Value;
			});
			AddCheckBox("Show Indirect Specular", MyRenderProxy.Settings.DisplayAmbientSpecular, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAmbientSpecular = x.IsChecked;
			});
			AddSlider("Glass Ambient", MySector.SunProperties.EnvironmentLight.GlassAmbient, 0f, 5f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.GlassAmbient = v.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Environment probe", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Distance", MySector.SunProperties.EnvironmentProbe.DrawDistance, 5f, 1000f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentProbe.DrawDistance = v.Value;
			});
			m_resolutionSlider = AddSlider("Resolution", MySector.SunProperties.EnvMapResolution, 32f, 4096f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvMapResolution = MathHelper.GetNearestBiggerPowerOfTwo((int)v.Value);
				if (MySector.SunProperties.EnvMapFilteredResolution > MySector.SunProperties.EnvMapResolution)
				{
					m_resolutionFilteredSlider.Value = MySector.SunProperties.EnvMapResolution;
				}
				if (v.Value != (float)MySector.SunProperties.EnvMapResolution)
				{
					v.Value = MySector.SunProperties.EnvMapResolution;
				}
			});
			m_resolutionFilteredSlider = AddSlider("Filtered Resolution", MySector.SunProperties.EnvMapFilteredResolution, 32f, 4096f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvMapFilteredResolution = MathHelper.GetNearestBiggerPowerOfTwo((int)v.Value);
				if (MySector.SunProperties.EnvMapFilteredResolution > MySector.SunProperties.EnvMapResolution)
				{
					m_resolutionSlider.Value = MySector.SunProperties.EnvMapFilteredResolution;
				}
				if (v.Value != (float)MySector.SunProperties.EnvMapFilteredResolution)
				{
					v.Value = MySector.SunProperties.EnvMapFilteredResolution;
				}
			});
			AddSlider("Dim Distance", MySector.SunProperties.EnvironmentLight.ForwardDimDistance, 0.1f, 10f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.ForwardDimDistance = v.Value;
			});
			AddSlider("Minimum Ambient", MySector.SunProperties.EnvironmentLight.AmbientForwardPass, 0f, 1f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AmbientForwardPass = v.Value;
			});
			AddSlider("Ambient radius", MySector.SunProperties.EnvironmentLight.AmbientRadius, 0f, 100f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AmbientRadius = v.Value;
			});
			AddSlider("Ambient Gather radius", MySector.SunProperties.EnvironmentLight.AmbientLightsGatherRadius, 0f, 100f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AmbientLightsGatherRadius = v.Value;
			});
			AddSlider("Ambient Gather scale", MySector.SunProperties.EnvironmentProbe.AmbientScale, 0f, 1f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentProbe.AmbientScale = v.Value;
			});
			AddSlider("Ambient Gather Min clamp", MySector.SunProperties.EnvironmentProbe.AmbientMinClamp, 0f, 0.1f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentProbe.AmbientMinClamp = v.Value;
			});
			AddSlider("Ambient Gather Max clamp", MySector.SunProperties.EnvironmentProbe.AmbientMaxClamp, 0f, 1f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentProbe.AmbientMaxClamp = v.Value;
			});
			AddSlider("Atmosphere Intensity", MySector.SunProperties.EnvironmentLight.EnvAtmosphereBrightness, 0f, 5f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.EnvAtmosphereBrightness = v.Value;
			});
			AddSlider("Timeout", MySector.SunProperties.EnvironmentProbe.TimeOut, 0f, 10f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentProbe.TimeOut = v.Value;
			});
			AddCheckBox("Render Blocks", MyRenderProxy.Settings.RenderBlocksToEnvProbe, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.RenderBlocksToEnvProbe = x.IsChecked;
			});
			AddSlider("Cubemap mipmap", MyRenderProxy.Settings.DisplayEnvProbeMipLevel, 0f, 30f, delegate(MyGuiControlSlider v)
			{
				MyRenderProxy.Settings.DisplayEnvProbeMipLevel = (int)v.Value;
			});
			AddCheckBox("DebugDisplay", MyRenderProxy.Settings.DisplayEnvProbe, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayEnvProbe = x.IsChecked;
			});
			AddCheckBox("DebugDisplayFar", MyRenderProxy.Settings.DisplayEnvProbeFar, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayEnvProbeFar = x.IsChecked;
			});
			AddCheckBox("Use Intensity display", MyRenderProxy.Settings.DisplayEnvProbeIntensities, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayEnvProbeIntensities = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Skybox", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Screen Intensity", MySector.SunProperties.EnvironmentLight.SkyboxBrightness, 0f, 5f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.SkyboxBrightness = v.Value;
			});
			AddSlider("Environment Intensity", MySector.SunProperties.EnvironmentLight.EnvSkyboxBrightness, 0f, 50f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.EnvSkyboxBrightness = v.Value;
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderEnvironmentAmbient";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
