using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
	internal class MyStationGenerator
	{
		private class MyStationCountsPerFaction
		{
			public MyFaction Faction;

			public int Outpost_req;

			public int Outpost_add;

			public int Orbit_req;

			public int Orbit_add;

			public int Mining_req;

			public int Mining_add;

			public int Station_req;

			public int Station_add;

			public int Deep_req;

			public int Deep_add;

			public bool PreferDeep;

			public int Outpost(bool req = false)
			{
				if (!req)
				{
					return Outpost_add;
				}
				return Outpost_req;
			}

			public int Orbit(bool req = false)
			{
				if (!req)
				{
					return Orbit_add;
				}
				return Orbit_req;
			}

			public int Mining(bool req = false)
			{
				if (!req)
				{
					return Mining_add;
				}
				return Mining_req;
			}

			public int Station(bool req = false)
			{
				if (!req)
				{
					return Station_add;
				}
				return Station_req;
			}

			public int Deep(bool req = false)
			{
				if (!req)
				{
					return Deep_add;
				}
				return Deep_req;
			}

			public void Outpost_Inc(bool req = false)
			{
				if (req)
				{
					Outpost_req++;
				}
				else
				{
					Outpost_add++;
				}
			}

			public void Orbit_Inc(bool req = false)
			{
				if (req)
				{
					Orbit_req++;
				}
				else
				{
					Orbit_add++;
				}
			}

			public void Mining_Inc(bool req = false)
			{
				if (req)
				{
					Mining_req++;
				}
				else
				{
					Mining_add++;
				}
			}

			public void Station_Inc(bool req = false)
			{
				if (req)
				{
					Station_req++;
				}
				else
				{
					Station_add++;
				}
			}

			public void Deep_Inc(bool req = false)
			{
				if (req)
				{
					Deep_req++;
				}
				else
				{
					Deep_add++;
				}
			}

			internal void PrintSum()
			{
				_ = 0 + (Outpost_req + Outpost_add) + (Orbit_req + Orbit_add) + (Mining_req + Mining_add) + (Station_req + Station_add);
				_ = Deep_req;
				_ = Deep_add;
			}
		}

		public static readonly double ASTEROID_CHECK_RADIUS = 30000.0;

		public static readonly double MIN_STATION_SPACING = 5000.0;

		public static readonly int NUMBER_OF_PLACEMENT_TRIES = 40;

		public static readonly int OUTPOST_NUMBER_OF_PLACEMENT_TRIES = 3;

		public static readonly int OUTPOST_NUMBER_OF_PLACEMENT_TRIES_PLANET_SPECIFIC = 20;

		public static readonly float MAX_STATION_RADIUS = 150f;

		internal static MyDefinitionId MINING_STATIONS_ID = new MyDefinitionId(typeof(MyObjectBuilder_StationsListDefinition), "MiningStations");

		internal static MyDefinitionId ORBITAL_STATIONS_ID = new MyDefinitionId(typeof(MyObjectBuilder_StationsListDefinition), "OrbitalStations");

		internal static MyDefinitionId OUTPOST_STATIONS_ID = new MyDefinitionId(typeof(MyObjectBuilder_StationsListDefinition), "Outposts");

		internal static MyDefinitionId SPACE_STATIONS_ID = new MyDefinitionId(typeof(MyObjectBuilder_StationsListDefinition), "SpaceStations");

		private MySessionComponentEconomyDefinition m_def;

		public MyStationGenerator(MySessionComponentEconomyDefinition def)
		{
			m_def = def;
		}

		public bool GenerateStations(MyFactionCollection factions)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			List<MyStationCountsPerFaction> list = new List<MyStationCountsPerFaction>();
			List<MyStationCountsPerFaction> list2 = new List<MyStationCountsPerFaction>();
			List<MyStationCountsPerFaction> list3 = new List<MyStationCountsPerFaction>();
			foreach (KeyValuePair<long, MyFaction> faction in factions)
			{
				switch (faction.Value.FactionType)
				{
				case MyFactionTypes.Miner:
					list.Add(new MyStationCountsPerFaction
					{
						Faction = faction.Value,
						Outpost_req = m_def.Station_Rule_Miner_Min_Outpost,
						Mining_req = m_def.Station_Rule_Miner_Min_StationM,
						Outpost_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Miner_Max_Outpost - m_def.Station_Rule_Miner_Min_Outpost + 1),
						Mining_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Miner_Max_StationM - m_def.Station_Rule_Miner_Min_StationM + 1),
						PreferDeep = false
					});
					break;
				case MyFactionTypes.Trader:
					list2.Add(new MyStationCountsPerFaction
					{
						Faction = faction.Value,
						Outpost_req = m_def.Station_Rule_Trader_Min_Outpost,
						Orbit_req = m_def.Station_Rule_Trader_Min_Orbit,
						Deep_req = m_def.Station_Rule_Trader_Min_Deep,
						Outpost_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Trader_Max_Outpost - m_def.Station_Rule_Trader_Min_Outpost + 1),
						Orbit_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Trader_Max_Orbit - m_def.Station_Rule_Trader_Min_Orbit + 1),
						Deep_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Trader_Max_Deep - m_def.Station_Rule_Trader_Min_Deep + 1),
						PreferDeep = true
					});
					break;
				case MyFactionTypes.Builder:
					list3.Add(new MyStationCountsPerFaction
					{
						Faction = faction.Value,
						Outpost_req = m_def.Station_Rule_Trader_Min_Outpost,
						Orbit_req = m_def.Station_Rule_Trader_Min_Orbit,
						Station_req = m_def.Station_Rule_Builder_Min_Station,
						Outpost_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Builder_Max_Outpost - m_def.Station_Rule_Builder_Min_Outpost + 1),
						Orbit_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Builder_Max_Orbit - m_def.Station_Rule_Builder_Min_Orbit + 1),
						Station_add = MyRandom.Instance.Next(0, m_def.Station_Rule_Builder_Max_Station - m_def.Station_Rule_Builder_Min_Station + 1),
						PreferDeep = false
					});
					break;
				}
			}
			HashSet<Vector3D> usedLocations = new HashSet<Vector3D>();
			GenerateSpecificStations(list, list2, list3, usedLocations, isRequired: true);
			GenerateSpecificStations(list, list2, list3, usedLocations, isRequired: false);
			return true;
		}

		private void GenerateSpecificStations(List<MyStationCountsPerFaction> minerCounts, List<MyStationCountsPerFaction> traderCounts, List<MyStationCountsPerFaction> builderCounts, HashSet<Vector3D> usedLocations, bool isRequired)
		{
			for (int i = 0; i < Math.Max(minerCounts.Count, Math.Max(traderCounts.Count, builderCounts.Count)); i++)
			{
				if (i < minerCounts.Count)
				{
					GenerateStationsForFaction(isRequired, minerCounts[i], usedLocations);
				}
				if (i < traderCounts.Count)
				{
					GenerateStationsForFaction(isRequired, traderCounts[i], usedLocations);
				}
				if (i < builderCounts.Count)
				{
					GenerateStationsForFaction(isRequired, builderCounts[i], usedLocations);
				}
			}
		}

		private bool GenerateStationsForFaction(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations)
		{
			MyStationCountsPerFaction myStationCountsPerFaction = new MyStationCountsPerFaction();
			List<MyPlanet> list = new List<MyPlanet>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyPlanet myPlanet = entity as MyPlanet;
				if (myPlanet != null)
				{
					list.Add(myPlanet);
				}
			}
			bool flag = MySession.Static.Settings.ProceduralDensity > 0f;
			bool flag2 = list.Count > 0;
			if (stationCounts.Outpost(required) > 0 && flag2)
			{
				GenerateOutposts(required, stationCounts, usedLocations, myStationCountsPerFaction, list);
			}
			if (stationCounts.Orbit(required) > 0 && flag2)
			{
				GenerateOrbitalStations(required, stationCounts, usedLocations, myStationCountsPerFaction, list);
			}
			if (stationCounts.Mining(required) > 0 && flag)
			{
				GenerateMiningStations(required, stationCounts, usedLocations, myStationCountsPerFaction);
			}
			if (stationCounts.Station(required) > 0 || ((!flag || !flag2) && !stationCounts.PreferDeep))
			{
				GenerateSpaceStations(required, stationCounts, usedLocations, myStationCountsPerFaction, flag, flag2);
			}
			if (stationCounts.Deep(required) > 0 || ((!flag || !flag2) && stationCounts.PreferDeep))
			{
				GenerateDeepSpaceStations(required, stationCounts, usedLocations, myStationCountsPerFaction, flag, flag2);
			}
			if (myStationCountsPerFaction.Outpost(required) + myStationCountsPerFaction.Orbit(required) + myStationCountsPerFaction.Mining(required) + myStationCountsPerFaction.Station(required) + myStationCountsPerFaction.Deep(required) != 0)
			{
				return false;
			}
			return true;
		}

		private void GenerateDeepSpaceStations(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations, MyStationCountsPerFaction missings, bool someAsteroids, bool somePlanets)
		{
			if (MySession.Static.Settings.StationsDistanceOuterRadiusEnd <= MySession.Static.Settings.StationsDistanceOuterRadiusStart)
			{
				MyLog.Default.WriteLine("Deep space stations were not spawned. 'Outer Radius End' must be higher than 'Outer Radius Start'.");
			}
			int num = stationCounts.Deep(required);
			if (stationCounts.PreferDeep)
			{
				if (somePlanets)
				{
					num += stationCounts.Outpost(required) + stationCounts.Orbit(required);
				}
				if (someAsteroids)
				{
					num += stationCounts.Mining(required);
				}
			}
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				bool flag = false;
				Vector3D position = Vector3D.Zero;
				for (int j = 0; j < NUMBER_OF_PLACEMENT_TRIES; j++)
				{
					if (PlaceRandomStation_Deep(out position) && IsStationFarFromOthers(position, usedLocations))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					missings.Deep_Inc(required);
					usedLocations.Add(position);
					continue;
				}
				MyStationsListDefinition stationTypeDefinition = GetStationTypeDefinition(MyStationTypeEnum.SpaceStation);
				if (stationTypeDefinition == null)
				{
					break;
				}
				MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.SpaceStation, stationCounts.Faction, GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType, null, null, isDeep: true);
				stationCounts.Faction.AddStation(station);
			}
		}

		private void GenerateSpaceStations(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations, MyStationCountsPerFaction missings, bool someAsteroids, bool somePlanets)
		{
			int num = stationCounts.Station(required);
			if (!stationCounts.PreferDeep)
			{
				if (!somePlanets)
				{
					num += stationCounts.Outpost(required) + stationCounts.Orbit(required);
				}
				if (!someAsteroids)
				{
					num += stationCounts.Mining(required);
				}
			}
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				bool flag = false;
				Vector3D position = Vector3D.Zero;
				for (int j = 0; j < NUMBER_OF_PLACEMENT_TRIES; j++)
				{
					if (PlaceRandomStation_Station(out position) && IsStationFarFromOthers(position, usedLocations))
					{
						flag = true;
						usedLocations.Add(position);
						break;
					}
				}
				if (!flag)
				{
					missings.Station_Inc(required);
					continue;
				}
				MyStationsListDefinition stationTypeDefinition = GetStationTypeDefinition(MyStationTypeEnum.SpaceStation);
				if (stationTypeDefinition == null)
				{
					break;
				}
				MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.SpaceStation, stationCounts.Faction, GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType);
				stationCounts.Faction.AddStation(station);
			}
		}

		private void GenerateMiningStations(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations, MyStationCountsPerFaction missings)
		{
			for (int i = 0; i < stationCounts.Mining(required); i++)
			{
				bool flag = false;
				Vector3D position = Vector3D.Zero;
				for (int j = 0; j < NUMBER_OF_PLACEMENT_TRIES; j++)
				{
					if (PlaceRandomStation_Mining(out position) && IsStationFarFromOthers(position, usedLocations))
					{
						flag = true;
						usedLocations.Add(position);
						break;
					}
				}
				if (!flag)
				{
					missings.Mining_Inc(required);
					continue;
				}
				MyStationsListDefinition stationTypeDefinition = GetStationTypeDefinition(MyStationTypeEnum.MiningStation);
				if (stationTypeDefinition == null)
				{
					break;
				}
				MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.MiningStation, stationCounts.Faction, GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType);
				stationCounts.Faction.AddStation(station);
			}
		}

		private void GenerateOrbitalStations(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations, MyStationCountsPerFaction missings, List<MyPlanet> planets)
		{
			for (int i = 0; i < stationCounts.Orbit(required); i++)
			{
				bool flag = false;
				Vector3D position = Vector3D.Zero;
				Vector3 up = Vector3.Zero;
				for (int j = 0; j < NUMBER_OF_PLACEMENT_TRIES; j++)
				{
					if (PlaceRandomStation_Orbital(planets[MyRandom.Instance.Next(0, planets.Count)], out position, out up) && IsStationFarFromOthers(position, usedLocations))
					{
						flag = true;
						usedLocations.Add(position);
						break;
					}
				}
				if (!flag)
				{
					missings.Orbit_Inc(required);
					continue;
				}
				MyStationsListDefinition stationTypeDefinition = GetStationTypeDefinition(MyStationTypeEnum.OrbitalStation);
				if (stationTypeDefinition == null)
				{
					break;
				}
				MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.OrbitalStation, stationCounts.Faction, GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType, up);
				stationCounts.Faction.AddStation(station);
			}
		}

		private void GenerateOutposts(bool required, MyStationCountsPerFaction stationCounts, HashSet<Vector3D> usedLocations, MyStationCountsPerFaction missings, List<MyPlanet> planets)
		{
			List<double> list = new List<double>();
			double num = 0.0;
			foreach (MyPlanet planet in planets)
			{
				num += (double)planet.AverageRadius;
				list.Add(num);
			}
			if (num == 0.0)
			{
				return;
			}
			double num2 = 1.0 / num;
			for (int i = 0; i < list.Count; i++)
			{
				list[i] *= num2;
			}
			for (int j = 0; j < stationCounts.Outpost(required); j++)
			{
				bool flag = false;
				Vector3D position = Vector3D.Zero;
				Vector3 up = Vector3.Zero;
				Vector3 forward = Vector3.Zero;
				MyStationsListDefinition stationTypeDefinition = GetStationTypeDefinition(MyStationTypeEnum.Outpost);
				if (stationTypeDefinition == null)
				{
					break;
				}
				string randomStationName = GetRandomStationName(stationTypeDefinition);
				MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(randomStationName);
				BoundingBox prefabLocalBBox = BoundingBox.CreateInvalid();
				MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
				for (int k = 0; k < cubeGrids.Length; k++)
				{
					BoundingBox box = cubeGrids[k].CalculateBoundingBox();
					prefabLocalBBox.Include(box);
				}
				MyPlanet myPlanet = null;
				for (int l = 0; l < OUTPOST_NUMBER_OF_PLACEMENT_TRIES; l++)
				{
					float num3 = MyRandom.Instance.NextFloat();
					for (int m = 0; m < planets.Count; m++)
					{
						if ((double)num3 < list[m])
						{
							myPlanet = planets[m];
							break;
						}
					}
					if (myPlanet == null)
					{
						return;
					}
					for (int n = 0; n < OUTPOST_NUMBER_OF_PLACEMENT_TRIES_PLANET_SPECIFIC; n++)
					{
						if (PlaceRandomStation_Outpost(myPlanet, prefabLocalBBox, out position, out up, out forward) && IsStationFarFromOthers(position, usedLocations))
						{
							flag = true;
							usedLocations.Add(position);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					missings.Outpost_Inc(required);
					continue;
				}
				MyStation myStation = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.Outpost, stationCounts.Faction, randomStationName, stationTypeDefinition.GeneratedItemsContainerType, up, forward);
				myStation.IsOnPlanetWithAtmosphere = myPlanet.HasAtmosphere;
				stationCounts.Faction.AddStation(myStation);
			}
		}

		private string GetRandomStationName(MyStationsListDefinition stationsListDef)
		{
			if (stationsListDef == null)
			{
				return "Economy_SpaceStation_1";
			}
			int index = MyRandom.Instance.Next(0, stationsListDef.StationNames.Count);
			return stationsListDef.StationNames[index].ToString();
		}

		internal static MyStationsListDefinition GetStationTypeDefinition(MyStationTypeEnum stationType)
		{
			MyDefinitionId subtypeId = default(MyDefinitionId);
			switch (stationType)
			{
			case MyStationTypeEnum.MiningStation:
				subtypeId = MINING_STATIONS_ID;
				break;
			case MyStationTypeEnum.OrbitalStation:
				subtypeId = ORBITAL_STATIONS_ID;
				break;
			case MyStationTypeEnum.Outpost:
				subtypeId = OUTPOST_STATIONS_ID;
				break;
			case MyStationTypeEnum.SpaceStation:
				subtypeId = SPACE_STATIONS_ID;
				break;
			default:
				MyLog.Default.Error($"Stations list for type {stationType} not defined. Go to Economy_Stations.sbc to add definition.");
				break;
			}
			return MyDefinitionManager.Static.GetDefinition<MyStationsListDefinition>(subtypeId);
		}

		private bool IsStationFarFromOthers(Vector3D position, HashSet<Vector3D> usedLocations)
		{
			double num = MIN_STATION_SPACING * MIN_STATION_SPACING;
			foreach (Vector3D usedLocation in usedLocations)
			{
				if ((usedLocation - position).LengthSquared() < num)
				{
					return false;
				}
			}
			return true;
		}

		private bool PlaceRandomStation_Outpost(MyPlanet planet, BoundingBox prefabLocalBBox, out Vector3D position, out Vector3 up, out Vector3 forward)
		{
			position = Vector3.Zero;
			forward = Vector3.Forward;
			up = Vector3.Up;
			MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = planet.Components.Get<MyGravityProviderComponent>() as MySphericalNaturalGravityComponent;
			if (mySphericalNaturalGravityComponent == null)
			{
				return false;
			}
			Vector3D position2 = planet.PositionComp.GetPosition();
			Vector3D vector3D = new BoundingSphereD(position2, mySphericalNaturalGravityComponent.GravityLimit).RandomToUniformPointOnSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
			up = Vector3.Normalize(vector3D - position2);
			position = planet.GetClosestSurfacePointGlobal(vector3D);
			Vector3 localPos = Vector3.Transform(position, planet.PositionComp.WorldMatrixInvScaled);
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out int face, out Vector2 texCoord);
			planet.Provider.Shape.GetValueForPositionCacheless(face, ref texCoord, out Vector3 localNormal);
			localNormal.Normalize();
			float radians = MyMath.AngleBetween(Vector3.UnitZ, localNormal);
			radians = MathHelper.ToDegrees(radians);
			if (radians > 16f)
			{
				return false;
			}
			Vector3 vector = Vector3.CalculatePerpendicularVector(up);
			vector.Normalize();
			forward = Vector3.Cross(up, vector);
			forward.Normalize();
			double num = (position - position2).Length();
			double num2 = 0.0;
			MatrixD matrix = MatrixD.CreateWorld(position, forward, up);
			Vector3D vector3D2 = Vector3D.Transform(prefabLocalBBox.Center, matrix);
			double num3 = (planet.GetClosestSurfacePointGlobal(vector3D2 + vector * prefabLocalBBox.HalfExtents.X) - position2).Length() - num;
			if (num3 < 0.0)
			{
				num2 = num3;
			}
			num3 = (planet.GetClosestSurfacePointGlobal(vector3D2 - vector * prefabLocalBBox.HalfExtents.X) - position2).Length() - num;
			if (num3 < num2)
			{
				num2 = num3;
			}
			num3 = (planet.GetClosestSurfacePointGlobal(vector3D2 + forward * prefabLocalBBox.HalfExtents.Z) - position2).Length() - num;
			if (num3 < num2)
			{
				num2 = num3;
			}
			num3 = (planet.GetClosestSurfacePointGlobal(vector3D2 - forward * prefabLocalBBox.HalfExtents.Z) - position2).Length() - num;
			if (num3 < num2)
			{
				num2 = num3;
			}
			Vector3D closestSurfacePointGlobal = planet.GetClosestSurfacePointGlobal(vector3D2);
			Vector3 vector2 = up * ((float)num2 - 0.25f);
			vector3D2 += vector2 - up * prefabLocalBBox.HalfExtents.Y * 0.5f;
			double num4 = (vector3D2 - position2).Length();
			double num5 = (closestSurfacePointGlobal - position2).Length();
			if (num4 - num5 < 0.0)
			{
				return false;
			}
			position += vector2;
			_ = position - up * 2f;
			if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawText3D(position, radians.ToString(), Color.Red, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, -1, persistent: true);
				MyRenderProxy.DebugDrawArrow3D(position, position + up * 10f, Color.Red, null, depthRead: false, 0.5, null, 0.5f, persistent: true);
				MyRenderProxy.DebugDrawSphere(position, MAX_STATION_RADIUS, Color.Yellow, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(position, planet.PositionComp.GetPosition(), Color.Yellow, Color.Yellow, depthRead: false, persistent: true);
			}
			return true;
		}

		private bool IsMaterialAtPositionBlacklistedForStations(Vector3D position, MyPlanet planet)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(planet.RootVoxel.PositionLeftBottomCorner, ref position, out Vector3I voxelCoord);
			voxelCoord += planet.RootVoxel.StorageMin;
			MyVoxelMaterialDefinition materialAt = planet.Storage.GetMaterialAt(ref voxelCoord);
			MyPlanetStorageProvider myPlanetStorageProvider = planet.RootVoxel.Storage.DataProvider as MyPlanetStorageProvider;
			if (myPlanetStorageProvider == null || materialAt == null)
			{
				return false;
			}
			return myPlanetStorageProvider.Material.IsMaterialBlacklistedForStation(materialAt.Id);
		}

		public bool PlaceRandomStation_Orbital(MyPlanet planet, out Vector3D position, out Vector3 up)
		{
			position = Vector3.Zero;
			up = Vector3.Zero;
			MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = planet.Components.Get<MyGravityProviderComponent>() as MySphericalNaturalGravityComponent;
			if (mySphericalNaturalGravityComponent == null)
			{
				return false;
			}
			up = Vector3.Normalize((position = new BoundingSphereD(planet.PositionComp.GetPosition(), mySphericalNaturalGravityComponent.GravityLimit).RandomToUniformPointOnSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble())) - planet.PositionComp.GetPosition());
			position += MyStation.SAFEZONE_SIZE * up;
			if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(position, MAX_STATION_RADIUS, Color.CornflowerBlue, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(position, planet.PositionComp.GetPosition(), Color.CornflowerBlue, Color.CornflowerBlue, depthRead: false, persistent: true);
			}
			return true;
		}

		public bool PlaceRandomStation_Mining(out Vector3D position)
		{
			position = Vector3D.Zero;
			Vector3D center = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceInnerRadius).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
			Vector3D vector3D = Vector3D.Zero;
			bool flag = false;
			Vector3 forward;
			Vector3 up;
			Vector3D? vector3D2 = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(new BoundingSphereD(center, ASTEROID_CHECK_RADIUS), null, takeOccupiedPositions: false, sortByDistance: true, MAX_STATION_RADIUS, 0f, out forward, out up);
			if (vector3D2.HasValue)
			{
				vector3D = vector3D2.Value;
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(vector3D, MAX_STATION_RADIUS, Color.Red, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(vector3D, Vector3D.Zero, Color.Red, Color.Red, depthRead: false, persistent: true);
			}
			position = vector3D;
			return true;
		}

		public bool PlaceRandomStation_Station(out Vector3D position)
		{
			position = Vector3D.Zero;
			Vector3D vector3D = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceInnerRadius).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
			List<MyObjectSeed> list = new List<MyObjectSeed>();
			MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D, MAX_STATION_RADIUS), list);
			if (list.Count > 0)
			{
				return false;
			}
			if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(vector3D, MAX_STATION_RADIUS, Color.Green, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(vector3D, Vector3D.Zero, Color.Green, Color.Green, depthRead: false, persistent: true);
			}
			position = vector3D;
			return true;
		}

		public bool PlaceRandomStation_Deep(out Vector3D position)
		{
			position = Vector3D.Zero;
			if (MySession.Static.Settings.StationsDistanceOuterRadiusEnd <= MySession.Static.Settings.StationsDistanceOuterRadiusStart)
			{
				return false;
			}
			Vector3D? vector3D = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceOuterRadiusEnd).RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MySession.Static.Settings.StationsDistanceOuterRadiusStart);
			if (!vector3D.HasValue)
			{
				return false;
			}
			List<MyObjectSeed> list = new List<MyObjectSeed>();
			MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D.Value, MAX_STATION_RADIUS), list);
			if (list.Count > 0)
			{
				return false;
			}
			if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawSphere(vector3D.Value, MAX_STATION_RADIUS, Color.Purple, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(vector3D.Value, Vector3D.Zero, Color.Purple, Color.Purple, depthRead: false, persistent: true);
			}
			position = vector3D.Value;
			return true;
		}

		private bool GetAsteroidsInSphere(BoundingSphereD sphere, List<MyObjectSeed> output)
		{
			MyProceduralWorldGenerator component = MySession.Static.GetComponent<MyProceduralWorldGenerator>();
			if (component == null)
			{
				return false;
			}
			output.Clear();
			component.GetAllInSphere(sphere, output);
			return true;
		}
	}
}
