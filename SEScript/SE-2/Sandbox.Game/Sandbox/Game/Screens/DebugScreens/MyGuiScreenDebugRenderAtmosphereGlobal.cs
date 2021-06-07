using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Render", "Atmosphere Global")]
	public class MyGuiScreenDebugRenderAtmosphereGlobal : MyGuiScreenDebugBase
	{
		private static bool m_atmosphereEnabled = true;

		public MyGuiScreenDebugRenderAtmosphereGlobal()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Atmosphere", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MySession.Static.GetComponent<MySectorWeatherComponent>() != null)
			{
				AddCheckBox("Enable Sun Rotation", () => MySession.Static.GetComponent<MySectorWeatherComponent>().Enabled, delegate(bool x)
				{
					MySession.Static.GetComponent<MySectorWeatherComponent>().Enabled = x;
				});
				AddSlider("Time of day", 0f, (MySession.Static == null) ? 1f : MySession.Static.Settings.SunRotationIntervalMinutes, () => MyTimeOfDayHelper.TimeOfDay, MyTimeOfDayHelper.UpdateTimeOfDay);
				AddSlider("Sun Speed", 0.5f, 60f, () => MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval, delegate(float f)
				{
					MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval = f;
				});
			}
			AddCheckBox("Enable atmosphere", () => m_atmosphereEnabled, delegate(bool b)
			{
				EnableAtmosphere(b);
			});
			AddSlider("Atmosphere Intensity", MySector.PlanetProperties.AtmosphereIntensityMultiplier, 0.1f, 150f, delegate(MyGuiControlSlider f)
			{
				MySector.PlanetProperties.AtmosphereIntensityMultiplier = f.Value;
				MyRenderProxy.SetSettingsDirty();
			});
			AddSlider("Atmosphere Intensity in Ambient", MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier, 0.1f, 150f, delegate(MyGuiControlSlider f)
			{
				MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier = f.Value;
				MyRenderProxy.SetSettingsDirty();
			});
			AddSlider("Atmosphere Desaturation in Ambient", MySector.PlanetProperties.AtmosphereDesaturationFactorForward, 0f, 1f, delegate(MyGuiControlSlider f)
			{
				MySector.PlanetProperties.AtmosphereDesaturationFactorForward = f.Value;
				MyRenderProxy.SetSettingsDirty();
			});
			AddSlider("Clouds Intensity", MySector.PlanetProperties.CloudsIntensityMultiplier, 0.5f, 150f, delegate(MyGuiControlSlider f)
			{
				MySector.PlanetProperties.CloudsIntensityMultiplier = f.Value;
				MyRenderProxy.SetSettingsDirty();
			});
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderPlanetSettings myRenderPlanetSettings = default(MyRenderPlanetSettings);
			myRenderPlanetSettings.AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier;
			myRenderPlanetSettings.AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier;
			myRenderPlanetSettings.AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward;
			myRenderPlanetSettings.CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier;
			MyRenderPlanetSettings settings = myRenderPlanetSettings;
			MyRenderProxy.UpdatePlanetSettings(ref settings);
		}

		private void EnableAtmosphere(bool enabled)
		{
			m_atmosphereEnabled = enabled;
			MyRenderProxy.EnableAtmosphere(m_atmosphereEnabled);
		}
	}
}
