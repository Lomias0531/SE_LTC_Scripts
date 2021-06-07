using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Lights;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation, 800)]
	public class MySector : MySessionComponentBase
	{
		public static Vector3 SunRotationAxis;

		public static MySunProperties SunProperties;

		public static MyFogProperties FogProperties;

		public static MyPlanetProperties PlanetProperties;

		public static MySSAOSettings SSAOSettings;

		public static MyHBAOData HBAOSettings;

		public static MyShadowsSettings ShadowSettings;

		public static MySectorLodding Lodding;

		internal static MyParticleDustProperties ParticleDustProperties;

		public static VRageRender.MyImpostorProperties[] ImpostorProperties;

		public static bool UseGenerator;

		public static List<int> PrimaryMaterials;

		public static List<int> SecondaryMaterials;

		public static MyEnvironmentDefinition EnvironmentDefinition;

		private static MyCamera m_camera;

		public static bool ResetEyeAdaptation;

		private static MyLight m_sunFlare;

		public static MyCamera MainCamera
		{
			get
			{
				return m_camera;
			}
			private set
			{
				m_camera = value;
				MyGuiManager.SetCamera(MainCamera);
				MyTransparentGeometry.SetCamera(MainCamera);
			}
		}

		public static Vector3 DirectionToSunNormalized => SunProperties.SunDirectionNormalized;

		public override Type[] Dependencies => new Type[9]
		{
			typeof(MyHud),
			typeof(MyPlanets),
			typeof(MyAntennaSystem),
			typeof(MyGravityProviderSystem),
			typeof(MyIGCSystemSessionComponent),
			typeof(MyUnsafeGridsSessionComponent),
			typeof(MyLights),
			typeof(MyThirdPersonSpectator),
			typeof(MyPhysics)
		};

		static MySector()
		{
			ShadowSettings = new MyShadowsSettings();
			Lodding = new MySectorLodding();
			UseGenerator = false;
			SetDefaults();
		}

		private static void SetDefaults()
		{
			SunProperties = MySunProperties.Default;
			FogProperties = MyFogProperties.Default;
			PlanetProperties = MyPlanetProperties.Default;
		}

		public static void InitEnvironmentSettings(MyObjectBuilder_EnvironmentSettings environmentBuilder = null)
		{
			if (environmentBuilder != null)
			{
				EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(environmentBuilder.EnvironmentDefinition);
			}
			else if (EnvironmentDefinition == null)
			{
				EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(MyStringHash.GetOrCompute("Default"));
			}
			MyEnvironmentDefinition environmentDefinition = EnvironmentDefinition;
			SunProperties = environmentDefinition.SunProperties;
			FogProperties = environmentDefinition.FogProperties;
			PlanetProperties = environmentDefinition.PlanetProperties;
			SSAOSettings = environmentDefinition.SSAOSettings;
			HBAOSettings = environmentDefinition.HBAOSettings;
			ShadowSettings.CopyFrom(environmentDefinition.ShadowSettings);
			SunRotationAxis = SunProperties.SunRotationAxis;
			MyRenderProxy.UpdateShadowsSettings(ShadowSettings);
			Lodding.UpdatePreset(environmentDefinition.LowLoddingSettings, environmentDefinition.MediumLoddingSettings, environmentDefinition.HighLoddingSettings, environmentDefinition.ExtremeLoddingSettings);
			MyPostprocessSettingsWrapper.Settings = environmentDefinition.PostProcessSettings;
			MyPostprocessSettingsWrapper.MarkDirty();
			if (environmentBuilder != null)
			{
				Vector3.CreateFromAzimuthAndElevation(environmentBuilder.SunAzimuth, environmentBuilder.SunElevation, out Vector3 direction);
				direction.Normalize();
				SunProperties.BaseSunDirectionNormalized = direction;
				SunProperties.SunDirectionNormalized = direction;
				FogProperties.FogMultiplier = environmentBuilder.FogMultiplier;
				FogProperties.FogDensity = environmentBuilder.FogDensity;
				FogProperties.FogColor = new Color(environmentBuilder.FogColor);
			}
		}

		public static void SaveEnvironmentDefinition()
		{
			EnvironmentDefinition.SunProperties = SunProperties;
			EnvironmentDefinition.FogProperties = FogProperties;
			EnvironmentDefinition.SSAOSettings = SSAOSettings;
			EnvironmentDefinition.HBAOSettings = HBAOSettings;
			EnvironmentDefinition.PostProcessSettings = MyPostprocessSettingsWrapper.Settings;
			EnvironmentDefinition.ShadowSettings.CopyFrom(ShadowSettings);
			EnvironmentDefinition.LowLoddingSettings.CopyFrom(Lodding.LowSettings);
			EnvironmentDefinition.MediumLoddingSettings.CopyFrom(Lodding.MediumSettings);
			EnvironmentDefinition.HighLoddingSettings.CopyFrom(Lodding.HighSettings);
			EnvironmentDefinition.ExtremeLoddingSettings.CopyFrom(Lodding.ExtremeSettings);
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = new MyObjectBuilder_Definitions();
			myObjectBuilder_Definitions.Environments = new MyObjectBuilder_EnvironmentDefinition[1]
			{
				(MyObjectBuilder_EnvironmentDefinition)EnvironmentDefinition.GetObjectBuilder()
			};
			myObjectBuilder_Definitions.Save(Path.Combine(MyFileSystem.ContentPath, "Data", "Environment.sbc"));
		}

		public static MyObjectBuilder_EnvironmentSettings GetEnvironmentSettings()
		{
			if (SunProperties.Equals(EnvironmentDefinition.SunProperties) && FogProperties.Equals(EnvironmentDefinition.FogProperties))
			{
				return null;
			}
			MyObjectBuilder_EnvironmentSettings myObjectBuilder_EnvironmentSettings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_EnvironmentSettings>();
			Vector3.GetAzimuthAndElevation(SunProperties.BaseSunDirectionNormalized, out float azimuth, out float elevation);
			myObjectBuilder_EnvironmentSettings.SunAzimuth = azimuth;
			myObjectBuilder_EnvironmentSettings.SunElevation = elevation;
			myObjectBuilder_EnvironmentSettings.FogMultiplier = FogProperties.FogMultiplier;
			myObjectBuilder_EnvironmentSettings.FogDensity = FogProperties.FogDensity;
			myObjectBuilder_EnvironmentSettings.FogColor = FogProperties.FogColor;
			myObjectBuilder_EnvironmentSettings.EnvironmentDefinition = EnvironmentDefinition.Id;
			return myObjectBuilder_EnvironmentSettings;
		}

		public override void LoadData()
		{
			MainCamera = new MyCamera(MySandboxGame.Config.FieldOfView, MySandboxGame.ScreenViewport)
			{
				FarPlaneDistance = ((MyMultiplayer.Static != null && Sync.IsServer) ? MySession.Static.Settings.SyncDistance : MySession.Static.Settings.ViewDistance)
			};
			MyEntities.LoadData();
			InitSunGlare();
			UpdateSunLight();
		}

		private void InitSunGlare()
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), "Sun");
			MyFlareDefinition myFlareDefinition = MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition;
			m_sunFlare = MyLights.AddLight();
			if (m_sunFlare != null)
			{
				m_sunFlare.Start("Sun");
				m_sunFlare.GlareOn = MyFakes.SUN_GLARE;
				m_sunFlare.GlareQuerySize = 100000f;
				m_sunFlare.GlareQueryFreqMinMs = 0f;
				m_sunFlare.GlareQueryFreqRndMs = 0f;
				m_sunFlare.GlareType = MyGlareTypeEnum.Distant;
				m_sunFlare.GlareMaxDistance = 2000000f;
				m_sunFlare.LightOn = false;
				if (myFlareDefinition != null && myFlareDefinition.SubGlares != null)
				{
					m_sunFlare.SubGlares = myFlareDefinition.SubGlares;
					m_sunFlare.GlareIntensity = myFlareDefinition.Intensity;
					m_sunFlare.GlareSize = myFlareDefinition.Size;
				}
			}
		}

		public static void UpdateSunLight()
		{
			if (m_sunFlare != null)
			{
				m_sunFlare.Position = MainCamera.Position + SunProperties.SunDirectionNormalized * 1000000f;
				m_sunFlare.UpdateLight();
			}
		}

		protected override void UnloadData()
		{
			if (m_sunFlare != null)
			{
				MyLights.RemoveLight(m_sunFlare);
			}
			m_sunFlare = null;
			MyEntities.UnloadData();
			MainCamera = null;
			base.UnloadData();
		}

		public override void UpdateBeforeSimulation()
		{
			MyEntities.UpdateBeforeSimulation();
			MyGameLogic.UpdateBeforeSimulation();
			base.UpdateBeforeSimulation();
		}

		public override void Simulate()
		{
			MyEntities.Simulate();
			base.Simulate();
		}

		public override void UpdateAfterSimulation()
		{
			MyEntities.UpdateAfterSimulation();
			MyGameLogic.UpdateAfterSimulation();
			base.UpdateAfterSimulation();
		}

		public override void UpdatingStopped()
		{
			MyEntities.UpdatingStopped();
			MyGameLogic.UpdatingStopped();
			base.UpdatingStopped();
		}

		public override void Draw()
		{
			base.Draw();
			MyEntities.Draw();
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
		}
	}
}
