using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Environment Light")]
	internal class MyGuiScreenDebugRenderEnvironmentLight : MyGuiScreenDebugBase
	{
		private static float timeOfDay;

		private static TimeSpan? OriginalTime;

		public MyGuiScreenDebugRenderEnvironmentLight()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Environment Light", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			_ = MySector.SunProperties;
			AddLabel("Sun", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Time of day", 0f, MySession.Static.Settings.SunRotationIntervalMinutes, () => MyTimeOfDayHelper.TimeOfDay, MyTimeOfDayHelper.UpdateTimeOfDay);
			AddSlider("Intensity", MySector.SunProperties.SunIntensity, 0f, 1000f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.SunIntensity = v.Value;
			});
			AddColor("Color", MySector.SunProperties.EnvironmentLight.SunColor, delegate(MyGuiControlColor v)
			{
				MySector.SunProperties.EnvironmentLight.SunColor = v.Color;
			});
			AddColor("Specular Color", MySector.SunProperties.EnvironmentLight.SunSpecularColor, delegate(MyGuiControlColor v)
			{
				MySector.SunProperties.EnvironmentLight.SunSpecularColor = v.Color;
			});
			AddSlider("Specular factor", MySector.SunProperties.EnvironmentLight.SunSpecularFactor, 0f, 1f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.SunSpecularFactor = v.Value;
			});
			AddSlider("Gloss factor", MySector.SunProperties.EnvironmentLight.SunGlossFactor, 0f, 5f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.SunGlossFactor = v.Value;
			});
			AddSlider("Diffuse factor", MySector.SunProperties.EnvironmentLight.SunDiffuseFactor, 0f, 10f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.SunDiffuseFactor = v.Value;
			});
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderEnvironmentLight";
		}
	}
}
