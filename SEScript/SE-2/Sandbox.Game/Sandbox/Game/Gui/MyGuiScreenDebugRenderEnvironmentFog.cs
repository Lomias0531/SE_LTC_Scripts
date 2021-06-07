using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Environment Fog")]
	internal class MyGuiScreenDebugRenderEnvironmentFog : MyGuiScreenDebugBase
	{
		private static float timeOfDay;

		private static TimeSpan? OriginalTime;

		public MyGuiScreenDebugRenderEnvironmentFog()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Environment Fog", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddSlider("Fog multiplier", MySector.FogProperties.FogMultiplier, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.FogProperties.FogMultiplier = x.Value;
			});
			AddSlider("Fog density", MySector.FogProperties.FogDensity, 0f, 0.01f, delegate(MyGuiControlSlider x)
			{
				MySector.FogProperties.FogDensity = x.Value;
			});
			AddColor("Fog color", MySector.FogProperties.FogColor, delegate(MyGuiControlColor x)
			{
				MySector.FogProperties.FogColor = x.Color;
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderEnvironmentFog";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderFogSettings myRenderFogSettings = default(MyRenderFogSettings);
			myRenderFogSettings.FogMultiplier = MySector.FogProperties.FogMultiplier;
			myRenderFogSettings.FogColor = MySector.FogProperties.FogColor;
			myRenderFogSettings.FogDensity = MySector.FogProperties.FogDensity;
			MyRenderFogSettings settings = myRenderFogSettings;
			MyRenderProxy.UpdateFogSettings(ref settings);
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
