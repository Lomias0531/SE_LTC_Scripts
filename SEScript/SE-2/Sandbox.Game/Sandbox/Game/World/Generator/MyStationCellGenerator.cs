using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyStationCellGenerator : MyProceduralWorldModule
	{
		private HashSet<MyStation> m_spawnInProgress = new HashSet<MyStation>();

		private HashSet<MyStation> m_removeRequested = new HashSet<MyStation>();

		public MyStationCellGenerator(double cellSize, int radiusMultiplier, int seed, double density, MyProceduralWorldModule parent = null)
			: base(cellSize, radiusMultiplier, seed, density, parent)
		{
		}

		protected override MyProceduralCell GenerateProceduralCell(ref Vector3I cellId)
		{
			MyProceduralCell myProceduralCell = new MyProceduralCell(cellId, CELL_SIZE);
			bool flag = false;
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					if (myProceduralCell.BoundingVolume.Contains(station.Position) == ContainmentType.Contains)
					{
						double stationSpawnDistance = GetStationSpawnDistance(station.Type);
						MyObjectSeed myObjectSeed = new MyObjectSeed(myProceduralCell, station.Position, stationSpawnDistance);
						myObjectSeed.UserData = station;
						myObjectSeed.Params.Type = MyObjectSeedType.Station;
						myObjectSeed.Params.Generated = (station.StationEntityId != 0);
						myProceduralCell.AddObject(myObjectSeed);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			return myProceduralCell;
		}

		private double GetStationSpawnDistance(MyStationTypeEnum stationType)
		{
			MyDefinitionId subtypeId = default(MyDefinitionId);
			switch (stationType)
			{
			case MyStationTypeEnum.MiningStation:
				subtypeId = MyStationGenerator.MINING_STATIONS_ID;
				break;
			case MyStationTypeEnum.OrbitalStation:
				subtypeId = MyStationGenerator.ORBITAL_STATIONS_ID;
				break;
			case MyStationTypeEnum.Outpost:
				subtypeId = MyStationGenerator.OUTPOST_STATIONS_ID;
				break;
			case MyStationTypeEnum.SpaceStation:
				subtypeId = MyStationGenerator.SPACE_STATIONS_ID;
				break;
			default:
				MyLog.Default.Error($"Stations list for type {stationType} not defined. Go to Economy_Stations.sbc to add definition.");
				break;
			}
			MyStationsListDefinition definition = MyDefinitionManager.Static.GetDefinition<MyStationsListDefinition>(subtypeId);
			if (definition == null)
			{
				return CELL_SIZE;
			}
			return definition.SpawnDistance;
		}

		public override void GenerateObjects(List<MyObjectSeed> list, HashSet<MyObjectSeedParams> existingObjectsSeeds)
		{
			foreach (MyObjectSeed seed in list)
			{
				MyStation station = seed.UserData as MyStation;
				if (station.StationEntityId == 0L)
				{
					IMyFaction faction = MySession.Static.Factions.TryGetFactionById(station.FactionId);
					if (faction != null && !m_spawnInProgress.Contains(station))
					{
						MySafeZone safeZone = station.CreateSafeZone(faction);
						safeZone.AccessTypeGrids = MySafeZoneAccess.Blacklist;
						safeZone.AccessTypeFloatingObjects = MySafeZoneAccess.Blacklist;
						safeZone.AccessTypePlayers = MySafeZoneAccess.Blacklist;
						safeZone.AccessTypeFactions = MySafeZoneAccess.Blacklist;
						safeZone.DisplayName = (safeZone.Name = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), faction.Tag, station.Id));
						MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
						{
							Position = station.Position,
							Forward = station.Forward,
							Up = station.Up,
							PrefabName = station.PrefabName,
							OwnerId = faction.FounderId,
							Color = faction.CustomColor,
							SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
							UpdateSync = true
						};
						m_spawnInProgress.Add(station);
						seed.Params.Generated = true;
						MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
						{
							m_spawnInProgress.Remove(station);
							if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
							{
								MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
								if (m_removeRequested.Contains(station))
								{
									RemoveStationGrid(station, myCubeGrid);
									m_removeRequested.Remove(station);
								}
								else
								{
									station.StationEntityId = myCubeGrid.EntityId;
									myCubeGrid.DisplayName = (myCubeGrid.Name = string.Format(MyTexts.GetString(MySpaceTexts.Grid_Name_Station), faction.Tag, station.Type.ToString(), station.Id));
									station.ResourcesGenerator.UpdateStation(myCubeGrid);
									station.StationGridSpawned();
									if (Sync.IsServer)
									{
										MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddStationGrid(myCubeGrid.EntityId);
										MyPlanetEnvironmentSessionComponent component = MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>();
										if (component != null)
										{
											component.ClearEnvironmentItems(worldBBox: new BoundingBoxD(station.Position - safeZone.Radius, station.Position + safeZone.Radius), entity: safeZone);
										}
									}
								}
							}
						}, delegate
						{
							m_spawnInProgress.Remove(station);
							seed.Params.Generated = false;
							station.StationEntityId = 0L;
						});
					}
				}
			}
		}

		protected override void CloseObjectSeed(MyObjectSeed objectSeed)
		{
			MyStation myStation = objectSeed.UserData as MyStation;
			MySafeZone entity;
			if (m_spawnInProgress.Contains(myStation))
			{
				m_removeRequested.Add(myStation);
			}
			else if (MyEntities.TryGetEntityById(myStation.SafeZoneEntityId, out entity) && (entity.IsEmpty() || entity.IsEntityInsideAlone(myStation.StationEntityId)))
			{
				entity.Close();
				myStation.SafeZoneEntityId = 0L;
				objectSeed.Params.Generated = false;
				if (!MyEntities.TryGetEntityById(myStation.StationEntityId, out MyCubeGrid entity2))
				{
					MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(myStation.StationEntityId);
					myStation.StationEntityId = 0L;
				}
				else
				{
					RemoveStationGrid(myStation, entity2);
				}
			}
		}

		private void RemoveStationGrid(MyStation station, MyCubeGrid stationGrid)
		{
			stationGrid.Close();
			MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(station.StationEntityId);
			station.StationEntityId = 0L;
			station.ResourcesGenerator.ClearBlocksCache();
		}
	}
}
