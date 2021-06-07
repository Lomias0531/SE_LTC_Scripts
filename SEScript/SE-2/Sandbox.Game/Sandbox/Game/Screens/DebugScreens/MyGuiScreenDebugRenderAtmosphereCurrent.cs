using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Render", "Atmosphere Current")]
	public class MyGuiScreenDebugRenderAtmosphereCurrent : MyGuiScreenDebugBase
	{
		private static long m_selectedPlanetEntityID;

		private static MyAtmosphereSettings m_originalAtmosphereSettings;

		private static MyAtmosphereSettings m_atmosphereSettings;

		private static bool m_atmosphereEnabled = true;

		private static MyPlanet SelectedPlanet
		{
			get
			{
				if (MyEntities.TryGetEntityById(m_selectedPlanetEntityID, out MyEntity entity))
				{
					return entity as MyPlanet;
				}
				return null;
			}
		}

		public MyGuiScreenDebugRenderAtmosphereCurrent()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Atmosphere Current", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			PickPlanet();
			if (SelectedPlanet != null)
			{
				if (m_atmosphereSettings.MieColorScattering.X == 0f)
				{
					m_atmosphereSettings.MieColorScattering = new Vector3(m_atmosphereSettings.MieScattering);
				}
				if (m_atmosphereSettings.Intensity == 0f)
				{
					m_atmosphereSettings.Intensity = 1f;
				}
				AddLabel("Atmosphere Settings", Color.White, 1f);
				AddSlider("Rayleigh Scattering R", 1f, 100f, () => m_atmosphereSettings.RayleighScattering.X, delegate(float f)
				{
					m_atmosphereSettings.RayleighScattering.X = f;
					UpdateAtmosphere();
				});
				AddSlider("Rayleigh Scattering G", 1f, 100f, () => m_atmosphereSettings.RayleighScattering.Y, delegate(float f)
				{
					m_atmosphereSettings.RayleighScattering.Y = f;
					UpdateAtmosphere();
				});
				AddSlider("Rayleigh Scattering B", 1f, 100f, () => m_atmosphereSettings.RayleighScattering.Z, delegate(float f)
				{
					m_atmosphereSettings.RayleighScattering.Z = f;
					UpdateAtmosphere();
				});
				AddSlider("Mie Scattering R", 5f, 150f, () => m_atmosphereSettings.MieColorScattering.X, delegate(float f)
				{
					m_atmosphereSettings.MieColorScattering.X = f;
					UpdateAtmosphere();
				});
				AddSlider("Mie Scattering G", 5f, 150f, () => m_atmosphereSettings.MieColorScattering.Y, delegate(float f)
				{
					m_atmosphereSettings.MieColorScattering.Y = f;
					UpdateAtmosphere();
				});
				AddSlider("Mie Scattering B", 5f, 150f, () => m_atmosphereSettings.MieColorScattering.Z, delegate(float f)
				{
					m_atmosphereSettings.MieColorScattering.Z = f;
					UpdateAtmosphere();
				});
				AddSlider("Rayleigh Height Surfrace", 1f, 50f, () => m_atmosphereSettings.RayleighHeight, delegate(float f)
				{
					m_atmosphereSettings.RayleighHeight = f;
					UpdateAtmosphere();
				});
				AddSlider("Rayleigh Height Space", 1f, 25f, () => m_atmosphereSettings.RayleighHeightSpace, delegate(float f)
				{
					m_atmosphereSettings.RayleighHeightSpace = f;
					UpdateAtmosphere();
				});
				AddSlider("Rayleigh Transition", 0.1f, 1.5f, () => m_atmosphereSettings.RayleighTransitionModifier, delegate(float f)
				{
					m_atmosphereSettings.RayleighTransitionModifier = f;
					UpdateAtmosphere();
				});
				AddSlider("Mie Height", 5f, 200f, () => m_atmosphereSettings.MieHeight, delegate(float f)
				{
					m_atmosphereSettings.MieHeight = f;
					UpdateAtmosphere();
				});
				AddSlider("Sun size", 0.99f, 1f, () => m_atmosphereSettings.MieG, delegate(float f)
				{
					m_atmosphereSettings.MieG = f;
					UpdateAtmosphere();
				});
				AddSlider("Sea floor modifier", 0.9f, 1.1f, () => m_atmosphereSettings.SeaLevelModifier, delegate(float f)
				{
					m_atmosphereSettings.SeaLevelModifier = f;
					UpdateAtmosphere();
				});
				AddSlider("Atmosphere top modifier", 0.9f, 1.1f, () => m_atmosphereSettings.AtmosphereTopModifier, delegate(float f)
				{
					m_atmosphereSettings.AtmosphereTopModifier = f;
					UpdateAtmosphere();
				});
				AddSlider("Intensity", 0.1f, 200f, () => m_atmosphereSettings.Intensity, delegate(float f)
				{
					m_atmosphereSettings.Intensity = f;
					UpdateAtmosphere();
				});
				AddSlider("Fog Intensity", 0f, 1f, () => m_atmosphereSettings.FogIntensity, delegate(float f)
				{
					m_atmosphereSettings.FogIntensity = f;
					UpdateAtmosphere();
				});
				AddColor("Sun Light Color", m_atmosphereSettings.SunColor, delegate(MyGuiControlColor v)
				{
					m_atmosphereSettings.SunColor = v.Color;
					UpdateAtmosphere();
				});
				AddColor("Sun Light Specular Color", m_atmosphereSettings.SunSpecularColor, delegate(MyGuiControlColor v)
				{
					m_atmosphereSettings.SunSpecularColor = v.Color;
					UpdateAtmosphere();
				});
				AddButton(new StringBuilder("Restore"), OnRestoreButtonClicked);
				AddButton(new StringBuilder("Earth settings"), OnResetButtonClicked);
			}
		}

		private void OnRestoreButtonClicked(MyGuiControlButton button)
		{
			m_atmosphereSettings = m_originalAtmosphereSettings;
			RecreateControls(constructor: false);
			UpdateAtmosphere();
		}

		private void OnResetButtonClicked(MyGuiControlButton button)
		{
			m_atmosphereSettings = MyAtmosphereSettings.Defaults();
			RecreateControls(constructor: false);
			UpdateAtmosphere();
		}

		private void PickPlanet()
		{
			List<MyLineSegmentOverlapResult<MyEntity>> list = new List<MyLineSegmentOverlapResult<MyEntity>>();
			LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.ForwardVector);
			MyGamePruningStructure.GetAllEntitiesInRay(ref ray, list);
			float num = float.MaxValue;
			MyPlanet myPlanet = null;
			foreach (MyLineSegmentOverlapResult<MyEntity> item in list)
			{
				MyPlanet myPlanet2 = item.Element as MyPlanet;
				if (myPlanet2 != null && myPlanet2.EntityId != m_selectedPlanetEntityID && item.Distance < (double)num)
				{
					myPlanet = myPlanet2;
				}
			}
			if (myPlanet != null)
			{
				m_selectedPlanetEntityID = myPlanet.EntityId;
				m_atmosphereSettings = myPlanet.AtmosphereSettings;
				m_originalAtmosphereSettings = m_atmosphereSettings;
			}
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

		private void UpdateAtmosphere()
		{
			if (SelectedPlanet != null)
			{
				SelectedPlanet.AtmosphereSettings = m_atmosphereSettings;
			}
		}
	}
}
