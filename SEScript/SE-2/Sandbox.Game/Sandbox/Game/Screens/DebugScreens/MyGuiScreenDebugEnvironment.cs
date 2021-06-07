using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Environment")]
	public class MyGuiScreenDebugEnvironment : MyGuiScreenDebugBase
	{
		public static Action DeleteEnvironmentItems;

		public MyGuiScreenDebugEnvironment()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderEnvironment";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddShareFocusHint();
			Spacing = 0.01f;
			AddCaption("World Environment", Color.Yellow.ToVector4());
			AddCaption("Debug Tools:", Color.Yellow.ToVector4());
			AddCheckBox("Update Environment Sectors", () => MyPlanetEnvironmentSessionComponent.EnableUpdate, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.EnableUpdate = x;
			});
			AddButton("Refresh Sectors", delegate
			{
				RefreshSectors();
			});
			AddLabel("Debug Draw Options:", Color.White, 1f);
			AddCheckBox("Debug Draw Sectors", () => MyPlanetEnvironmentSessionComponent.DebugDrawSectors, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawSectors = x;
			});
			AddCheckBox("Debug Draw Clipmap Proxies", () => MyPlanetEnvironmentSessionComponent.DebugDrawProxies, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawProxies = x;
			});
			AddCheckBox("Debug Draw Dynamic Clusters", () => MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters = x;
			});
			AddCheckBox("Debug Draw Collision Boxes", () => MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers = x;
			});
			AddCheckBox("Debug Draw Providers", () => MyPlanetEnvironmentSessionComponent.DebugDrawEnvironmentProviders, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawEnvironmentProviders = x;
			});
			AddCheckBox("Debug Draw Active Sector Items", () => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems = x;
			});
			AddCheckBox("Debug Draw Active Sector Provider", () => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider, delegate(bool x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider = x;
			});
			AddSlider("Sector Name Draw Distance:", new MyGuiSliderPropertiesExponential(1f, 1000f), () => MyPlanetEnvironmentSessionComponent.DebugDrawDistance, delegate(float x)
			{
				MyPlanetEnvironmentSessionComponent.DebugDrawDistance = x;
			});
		}

		private void RefreshSectors()
		{
			foreach (MyPlanet planet in MyPlanets.GetPlanets())
			{
				planet.Components.Get<MyPlanetEnvironmentComponent>().CloseAll();
			}
		}
	}
}
