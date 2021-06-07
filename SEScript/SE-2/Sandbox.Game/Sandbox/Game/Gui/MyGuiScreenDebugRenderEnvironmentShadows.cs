using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Environment Shadows")]
	internal class MyGuiScreenDebugRenderEnvironmentShadows : MyGuiScreenDebugBase
	{
		private static float timeOfDay;

		private static TimeSpan? OriginalTime;

		public MyGuiScreenDebugRenderEnvironmentShadows()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Environment Shadows", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			_ = MySector.SunProperties;
			AddLabel("Sun", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Time of day", 0f, MySession.Static.Settings.SunRotationIntervalMinutes, () => MyTimeOfDayHelper.TimeOfDay, MyTimeOfDayHelper.UpdateTimeOfDay);
			m_currentPosition.Y += 0.01f;
			AddSlider("Shadow fadeout", MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier = x.Value;
			});
			AddSlider("Env Shadow fadeout", MySector.SunProperties.EnvironmentLight.EnvShadowFadeoutMultiplier, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.SunProperties.EnvironmentLight.EnvShadowFadeoutMultiplier = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Ambient Occlusion", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("IndirectLight", MySector.SunProperties.EnvironmentLight.AOIndirectLight, 0f, 2f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AOIndirectLight = v.Value;
			});
			AddSlider("DirLight", MySector.SunProperties.EnvironmentLight.AODirLight, 0f, 2f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AODirLight = v.Value;
			});
			AddSlider("AOPointLight", MySector.SunProperties.EnvironmentLight.AOPointLight, 0f, 2f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AOPointLight = v.Value;
			});
			AddSlider("AOSpotLight", MySector.SunProperties.EnvironmentLight.AOSpotLight, 0f, 2f, delegate(MyGuiControlSlider v)
			{
				MySector.SunProperties.EnvironmentLight.AOSpotLight = v.Value;
			});
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderEnvironmentShadows";
		}
	}
}
