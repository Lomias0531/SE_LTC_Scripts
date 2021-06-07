using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 500)]
	public class MyPlanets : MySessionComponentBase
	{
		private readonly List<MyPlanet> m_planets = new List<MyPlanet>();

		public readonly List<BoundingBoxD> m_planetAABBsCache = new List<BoundingBoxD>();

		private MyPlanetWhitelistDefinition m_planetWhitelist;

		public static MyPlanets Static
		{
			get;
			private set;
		}

		public event Action<MyPlanet> OnPlanetAdded;

		public event Action<MyPlanet> OnPlanetRemoved;

		public override void LoadData()
		{
			Static = this;
			base.LoadData();
			MyDefinitionManager.Static.GetPlanetWhitelistDefinitions().TryGetValue("BaseGame", out MyPlanetWhitelistDefinition value);
			m_planetWhitelist = value;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public static void Register(MyPlanet myPlanet)
		{
			Static.m_planets.Add(myPlanet);
			Static.m_planetAABBsCache.Clear();
			Static.OnPlanetAdded.InvokeIfNotNull(myPlanet);
		}

		public static void UnRegister(MyPlanet myPlanet)
		{
			Static.m_planets.Remove(myPlanet);
			Static.m_planetAABBsCache.Clear();
			Static.OnPlanetRemoved.InvokeIfNotNull(myPlanet);
		}

		public static List<MyPlanet> GetPlanets()
		{
			return Static?.m_planets;
		}

		public MyPlanet GetClosestPlanet(Vector3D position)
		{
			List<MyPlanet> planets = m_planets;
			if (planets.Count == 0)
			{
				return null;
			}
			return planets.MinBy((MyPlanet x) => (float)((Vector3D.DistanceSquared(x.PositionComp.GetPosition(), position) - (double)(x.AtmosphereRadius * x.AtmosphereRadius)) / 1000.0));
		}

		public ListReader<BoundingBoxD> GetPlanetAABBs()
		{
			if (m_planetAABBsCache.Count == 0)
			{
				foreach (MyPlanet planet in m_planets)
				{
					m_planetAABBsCache.Add(planet.PositionComp.WorldAABB);
				}
			}
			return m_planetAABBsCache;
		}

		public bool CanSpawnPlanet(MyPlanetGeneratorDefinition planetType, out string errorMessage)
		{
			if (!MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				if (!MyFakes.ENABLE_PLANETS)
				{
					errorMessage = MyTexts.GetString(MySpaceTexts.Notification_PlanetsNotSupported);
					return false;
				}
				MyPlanetWhitelistDefinition planetWhitelist = m_planetWhitelist;
				if (planetWhitelist != null && !planetWhitelist.WhitelistedPlanets.Contains(planetType))
				{
					errorMessage = string.Format(MyTexts.GetString(MySpaceTexts.Notification_PlanetNotWhitelisted), planetType.Id.SubtypeId.String);
					return false;
				}
			}
			errorMessage = null;
			return true;
		}
	}
}
