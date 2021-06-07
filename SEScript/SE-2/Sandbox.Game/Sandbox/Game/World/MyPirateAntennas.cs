using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 1000, typeof(MyObjectBuilder_PirateAntennas), null)]
	public class MyPirateAntennas : MySessionComponentBase
	{
		private class PirateAntennaInfo
		{
			public MyPirateAntennaDefinition AntennaDefinition;

			public int LastGenerationGameTime;

			public int SpawnedDrones;

			public bool IsActive;

			public List<int> SpawnPositionsIndexes;

			public int CurrentSpawnPositionsIndex = -1;

			public static List<PirateAntennaInfo> m_pool = new List<PirateAntennaInfo>();

			public static PirateAntennaInfo Allocate(MyPirateAntennaDefinition antennaDef)
			{
				PirateAntennaInfo pirateAntennaInfo = null;
				if (m_pool.Count == 0)
				{
					pirateAntennaInfo = new PirateAntennaInfo();
				}
				else
				{
					pirateAntennaInfo = m_pool[m_pool.Count - 1];
					m_pool.RemoveAt(m_pool.Count - 1);
				}
				pirateAntennaInfo.Reset(antennaDef);
				return pirateAntennaInfo;
			}

			public static void Deallocate(PirateAntennaInfo toDeallocate)
			{
				toDeallocate.AntennaDefinition = null;
				toDeallocate.SpawnPositionsIndexes = null;
				m_pool.Add(toDeallocate);
			}

			public void Reset(MyPirateAntennaDefinition antennaDef)
			{
				AntennaDefinition = antennaDef;
				LastGenerationGameTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + antennaDef.FirstSpawnTimeMs - antennaDef.SpawnTimeMs;
				SpawnedDrones = 0;
				IsActive = false;
				SpawnPositionsIndexes = null;
				CurrentSpawnPositionsIndex = -1;
			}
		}

		private class DroneInfo
		{
			public long AntennaEntityId;

			public int DespawnTime;

			public static List<DroneInfo> m_pool = new List<DroneInfo>();

			public static DroneInfo Allocate(long antennaEntityId, int despawnTime)
			{
				DroneInfo droneInfo = null;
				if (m_pool.Count == 0)
				{
					droneInfo = new DroneInfo();
				}
				else
				{
					droneInfo = m_pool[m_pool.Count - 1];
					m_pool.RemoveAt(m_pool.Count - 1);
				}
				droneInfo.AntennaEntityId = antennaEntityId;
				droneInfo.DespawnTime = despawnTime;
				return droneInfo;
			}

			public static void Deallocate(DroneInfo toDeallocate)
			{
				toDeallocate.AntennaEntityId = 0L;
				toDeallocate.DespawnTime = 0;
				m_pool.Add(toDeallocate);
			}
		}

		private static readonly string IDENTITY_NAME = "Pirate";

		private static readonly string PIRATE_FACTION_TAG = "SPRT";

		private static readonly int DRONE_DESPAWN_TIMER = 600000;

		private static readonly int DRONE_DESPAWN_RETRY = 5000;

		private static CachingDictionary<long, PirateAntennaInfo> m_pirateAntennas;

		private static bool m_iteratingAntennas;

		private static Dictionary<string, MyPirateAntennaDefinition> m_definitionsByAntennaName;

		private static int m_ctr = 0;

		private static int m_ctr2 = 0;

		private static CachingDictionary<long, DroneInfo> m_droneInfos;

		private static long m_piratesIdentityId = 0L;

		public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

		public override void LoadData()
		{
			base.LoadData();
			m_piratesIdentityId = 0L;
			m_pirateAntennas = new CachingDictionary<long, PirateAntennaInfo>();
			m_definitionsByAntennaName = new Dictionary<string, MyPirateAntennaDefinition>();
			m_droneInfos = new CachingDictionary<long, DroneInfo>();
			foreach (MyPirateAntennaDefinition pirateAntennaDefinition in MyDefinitionManager.Static.GetPirateAntennaDefinitions())
			{
				m_definitionsByAntennaName[pirateAntennaDefinition.Name] = pirateAntennaDefinition;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_PirateAntennas myObjectBuilder_PirateAntennas = sessionComponent as MyObjectBuilder_PirateAntennas;
			m_piratesIdentityId = myObjectBuilder_PirateAntennas.PiratesIdentity;
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (myObjectBuilder_PirateAntennas.Drones != null)
			{
				MyObjectBuilder_PirateAntennas.MyPirateDrone[] drones = myObjectBuilder_PirateAntennas.Drones;
				foreach (MyObjectBuilder_PirateAntennas.MyPirateDrone myPirateDrone in drones)
				{
					m_droneInfos.Add(myPirateDrone.EntityId, DroneInfo.Allocate(myPirateDrone.AntennaEntityId, totalGamePlayTimeInMilliseconds + myPirateDrone.DespawnTimer), immediate: true);
				}
			}
			m_iteratingAntennas = false;
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionByTag(PIRATE_FACTION_TAG);
			if (myFaction != null)
			{
				if (m_piratesIdentityId != 0L)
				{
					if (Sync.IsServer)
					{
						MyIdentity myIdentity = Sync.Players.TryGetIdentity(m_piratesIdentityId);
						if (myIdentity == null)
						{
							Sync.Players.CreateNewIdentity(IDENTITY_NAME, m_piratesIdentityId, null, null);
						}
						myIdentity.LastLoginTime = DateTime.Now;
						if (MySession.Static.Factions.GetPlayerFaction(m_piratesIdentityId) == null)
						{
							MyFactionCollection.SendJoinRequest(myFaction.FactionId, m_piratesIdentityId);
						}
					}
				}
				else
				{
					m_piratesIdentityId = myFaction.FounderId;
				}
				if (!Sync.Players.IdentityIsNpc(m_piratesIdentityId))
				{
					Sync.Players.MarkIdentityAsNPC(m_piratesIdentityId);
				}
			}
			foreach (KeyValuePair<long, DroneInfo> droneInfo in m_droneInfos)
			{
				MyEntities.TryGetEntityById(droneInfo.Key, out MyEntity entity);
				if (entity == null)
				{
					DroneInfo.Deallocate(droneInfo.Value);
					m_droneInfos.Remove(droneInfo.Key);
				}
				else if (!MySession.Static.Settings.EnableDrones)
				{
					MyCubeGrid myCubeGrid = entity as MyCubeGrid;
					MyRemoteControl myRemoteControl = entity as MyRemoteControl;
					if (myCubeGrid == null)
					{
						myCubeGrid = myRemoteControl.CubeGrid;
					}
					UnregisterDrone(entity, immediate: false);
					myCubeGrid.Close();
				}
				else
				{
					RegisterDrone(droneInfo.Value.AntennaEntityId, entity, immediate: false);
				}
			}
			m_droneInfos.ApplyRemovals();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_definitionsByAntennaName = null;
			foreach (KeyValuePair<long, DroneInfo> droneInfo in m_droneInfos)
			{
				MyEntities.TryGetEntityById(droneInfo.Key, out MyEntity entity);
				if (entity != null)
				{
					UnregisterDrone(entity, immediate: false);
				}
			}
			m_droneInfos.Clear();
			m_droneInfos = null;
			m_pirateAntennas = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_PirateAntennas myObjectBuilder_PirateAntennas = base.GetObjectBuilder() as MyObjectBuilder_PirateAntennas;
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			myObjectBuilder_PirateAntennas.PiratesIdentity = m_piratesIdentityId;
			DictionaryReader<long, DroneInfo> reader = m_droneInfos.Reader;
			myObjectBuilder_PirateAntennas.Drones = new MyObjectBuilder_PirateAntennas.MyPirateDrone[reader.Count];
			int num = 0;
			foreach (KeyValuePair<long, DroneInfo> item in reader)
			{
				myObjectBuilder_PirateAntennas.Drones[num] = new MyObjectBuilder_PirateAntennas.MyPirateDrone();
				myObjectBuilder_PirateAntennas.Drones[num].EntityId = item.Key;
				myObjectBuilder_PirateAntennas.Drones[num].AntennaEntityId = item.Value.AntennaEntityId;
				myObjectBuilder_PirateAntennas.Drones[num].DespawnTimer = Math.Max(0, item.Value.DespawnTime - totalGamePlayTimeInMilliseconds);
				num++;
			}
			return myObjectBuilder_PirateAntennas;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			_ = MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
			if (Sync.IsServer)
			{
				if (++m_ctr > 30)
				{
					m_ctr = 0;
					UpdateDroneSpawning();
				}
				if (++m_ctr2 > 100)
				{
					m_ctr2 = 0;
					UpdateDroneDespawning();
				}
			}
		}

		private void UpdateDroneSpawning()
		{
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_iteratingAntennas = true;
			foreach (KeyValuePair<long, PirateAntennaInfo> pirateAntenna in m_pirateAntennas)
			{
				PirateAntennaInfo value = pirateAntenna.Value;
				if (value.IsActive && value.AntennaDefinition != null && totalGamePlayTimeInMilliseconds - value.LastGenerationGameTime > value.AntennaDefinition.SpawnTimeMs)
				{
					MyRadioAntenna entity = null;
					MyEntities.TryGetEntityById(pirateAntenna.Key, out entity);
					if (value.AntennaDefinition.SpawnGroupSampler == null)
					{
						return;
					}
					MySpawnGroupDefinition mySpawnGroupDefinition = value.AntennaDefinition.SpawnGroupSampler.Sample();
					if (entity == null || mySpawnGroupDefinition == null)
					{
						value.LastGenerationGameTime = totalGamePlayTimeInMilliseconds;
					}
					else
					{
						bool flag = true;
						if (entity.OwnerId != 0L)
						{
							flag = (MySession.Static.Players.TryGetIdentity(entity.OwnerId)?.BlockLimits.HasRemainingPCU ?? false);
						}
						if (!MySession.Static.Settings.EnableDrones || value.SpawnedDrones >= value.AntennaDefinition.MaxDrones || !flag || m_droneInfos.Reader.Count >= MySession.Static.Settings.MaxDrones)
						{
							value.LastGenerationGameTime = totalGamePlayTimeInMilliseconds;
						}
						else
						{
							mySpawnGroupDefinition.ReloadPrefabs();
							BoundingSphereD boundingSphereD = new BoundingSphereD(entity.WorldMatrix.Translation, entity.GetRadius());
							ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
							bool flag2 = false;
							foreach (MyPlayer item in onlinePlayers)
							{
								if (boundingSphereD.Contains(item.GetPosition()) == ContainmentType.Contains && MyIDModule.GetRelationPlayerPlayer(entity.OwnerId, item.Identity.IdentityId) == MyRelationsBetweenPlayers.Enemies)
								{
									Vector3D? vector3D = null;
									for (int i = 0; i < 10; i++)
									{
										vector3D = MyEntities.FindFreePlace(entity.WorldMatrix.Translation + MyUtils.GetRandomVector3Normalized() * value.AntennaDefinition.SpawnDistance, mySpawnGroupDefinition.SpawnRadius);
										if (vector3D.HasValue)
										{
											break;
										}
									}
									flag2 = SpawnDrone(entity, entity.OwnerId, vector3D.Value, mySpawnGroupDefinition);
									break;
								}
							}
							if (flag2)
							{
								value.LastGenerationGameTime = totalGamePlayTimeInMilliseconds;
							}
						}
					}
				}
			}
			m_pirateAntennas.ApplyChanges();
			m_iteratingAntennas = false;
		}

		private void UpdateDroneDespawning()
		{
			foreach (KeyValuePair<long, DroneInfo> droneInfo in m_droneInfos)
			{
				if (droneInfo.Value.DespawnTime < MySandboxGame.TotalGamePlayTimeInMilliseconds)
				{
					MyEntity entity = null;
					MyEntities.TryGetEntityById(droneInfo.Key, out entity);
					if (entity != null)
					{
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						MyRemoteControl myRemoteControl = entity as MyRemoteControl;
						if (myCubeGrid == null)
						{
							myCubeGrid = myRemoteControl.CubeGrid;
						}
						if (CanDespawn(myCubeGrid, myRemoteControl))
						{
							UnregisterDrone(entity, immediate: false);
							MyEntities.SendCloseRequest(myCubeGrid);
						}
						else
						{
							droneInfo.Value.DespawnTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + DRONE_DESPAWN_RETRY;
						}
					}
					else
					{
						DroneInfo.Deallocate(droneInfo.Value);
						m_droneInfos.Remove(droneInfo.Key);
					}
				}
			}
			m_droneInfos.ApplyChanges();
		}

		public bool CanDespawn(MyCubeGrid grid, MyRemoteControl remote)
		{
			if (remote != null && !remote.IsFunctional)
			{
				return false;
			}
			BoundingSphereD worldVolume = grid.PositionComp.WorldVolume;
			worldVolume.Radius += 4000.0;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (worldVolume.Contains(onlinePlayer.GetPosition()) == ContainmentType.Contains)
				{
					return false;
				}
			}
			foreach (HashSet<IMyGunObject<MyDeviceBase>> value in grid.GridSystems.WeaponSystem.GetGunSets().Values)
			{
				foreach (IMyGunObject<MyDeviceBase> item in value)
				{
					if (item.IsShooting)
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool SpawnDrone(MyRadioAntenna antenna, long ownerId, Vector3D position, MySpawnGroupDefinition spawnGroup, Vector3? spawnUp = null, Vector3? spawnForward = null)
		{
			long antennaEntityId = antenna.EntityId;
			Vector3D position2 = antenna.PositionComp.GetPosition();
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			Vector3D axis;
			if (closestPlanet != null)
			{
				if (!MyGravityProviderSystem.IsPositionInNaturalGravity(position2) && MyGravityProviderSystem.IsPositionInNaturalGravity(position))
				{
					MySandboxGame.Log.WriteLine("Couldn't spawn drone; antenna is not in natural gravity but spawn location is.");
					return false;
				}
				closestPlanet.CorrectSpawnLocation(ref position, (double)spawnGroup.SpawnRadius * 2.0);
				axis = position - closestPlanet.PositionComp.GetPosition();
				axis.Normalize();
			}
			else
			{
				Vector3 vector = MyGravityProviderSystem.CalculateTotalGravityInPoint(position);
				if (!(vector != Vector3.Zero))
				{
					axis = ((!spawnUp.HasValue) ? ((Vector3D)MyUtils.GetRandomVector3Normalized()) : ((Vector3D)spawnUp.Value));
				}
				else
				{
					axis = -vector;
					axis.Normalize();
				}
			}
			Vector3D vector3D = MyUtils.GetRandomPerpendicularVector(ref axis);
			if (spawnForward.HasValue)
			{
				Vector3 value = spawnForward.Value;
				if (Math.Abs(Vector3.Dot(value, axis)) >= 0.98f)
				{
					value = Vector3.CalculatePerpendicularVector(axis);
				}
				else
				{
					Vector3 vector2 = Vector3.Cross(value, axis);
					vector2.Normalize();
					value = Vector3.Cross(axis, vector2);
					value.Normalize();
				}
				vector3D = value;
			}
			MatrixD matrix = MatrixD.CreateWorld(position, vector3D, axis);
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab in spawnGroup.Prefabs)
			{
				Vector3D position3 = Vector3D.Transform((Vector3D)shipPrefab.Position, matrix);
				Stack<Action> stack = new Stack<Action>();
				List<MyCubeGrid> createdGrids = new List<MyCubeGrid>();
				if (!string.IsNullOrEmpty(shipPrefab.Behaviour))
				{
					stack.Push(delegate
					{
						foreach (MyCubeGrid item in createdGrids)
						{
							if (!MyDroneAI.SetAIToGrid(item, shipPrefab.Behaviour, shipPrefab.BehaviourActivationDistance))
							{
								MyLog.Default.Error("Could not inject AI to encounter {0}. No remote control.", item.DisplayName);
							}
						}
					});
				}
				stack.Push(delegate
				{
					ChangeDroneOwnership(createdGrids, ownerId, antennaEntityId);
				});
				MyPrefabManager.Static.SpawnPrefab(createdGrids, shipPrefab.SubtypeId, position3, vector3D, axis, default(Vector3), default(Vector3), null, null, SpawningOptions.SpawnRandomCargo | SpawningOptions.SetAuthorship, ownerId, updateSync: true, stack);
			}
			return true;
		}

		private void ChangeDroneOwnership(List<MyCubeGrid> gridList, long ownerId, long antennaEntityId)
		{
			foreach (MyCubeGrid grid in gridList)
			{
				grid.ChangeGridOwnership(ownerId, MyOwnershipShareModeEnum.None);
				MyRemoteControl myRemoteControl = null;
				foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
				{
					if (cubeBlock.FatBlock != null)
					{
						(cubeBlock.FatBlock as MyProgrammableBlock)?.SendRecompile();
						MyRemoteControl myRemoteControl2 = cubeBlock.FatBlock as MyRemoteControl;
						if (myRemoteControl == null)
						{
							myRemoteControl = myRemoteControl2;
						}
					}
				}
				RegisterDrone(antennaEntityId, (MyEntity)(((object)myRemoteControl) ?? ((object)grid)));
			}
		}

		private void RegisterDrone(long antennaEntityId, MyEntity droneMainEntity, bool immediate = true)
		{
			DroneInfo value = DroneInfo.Allocate(antennaEntityId, MySandboxGame.TotalGamePlayTimeInMilliseconds + DRONE_DESPAWN_TIMER);
			m_droneInfos.Add(droneMainEntity.EntityId, value, immediate);
			droneMainEntity.OnClosing += DroneMainEntityOnClosing;
			PirateAntennaInfo value2 = null;
			if (!m_pirateAntennas.TryGetValue(antennaEntityId, out value2) && MyEntities.TryGetEntityById(antennaEntityId, out MyEntity entity))
			{
				MyRadioAntenna myRadioAntenna = entity as MyRadioAntenna;
				if (myRadioAntenna != null)
				{
					myRadioAntenna.UpdatePirateAntenna();
					m_pirateAntennas.TryGetValue(antennaEntityId, out value2);
				}
			}
			if (value2 != null)
			{
				value2.SpawnedDrones++;
			}
			MyRemoteControl myRemoteControl = droneMainEntity as MyRemoteControl;
			if (myRemoteControl != null)
			{
				myRemoteControl.OwnershipChanged += DroneRemoteOwnershipChanged;
			}
		}

		private void UnregisterDrone(MyEntity entity, bool immediate = true)
		{
			long key = 0L;
			DroneInfo value = null;
			m_droneInfos.TryGetValue(entity.EntityId, out value);
			if (value != null)
			{
				key = value.AntennaEntityId;
				DroneInfo.Deallocate(value);
			}
			m_droneInfos.Remove(entity.EntityId, immediate);
			PirateAntennaInfo value2 = null;
			m_pirateAntennas.TryGetValue(key, out value2);
			if (value2 != null)
			{
				value2.SpawnedDrones--;
			}
			entity.OnClosing -= DroneMainEntityOnClosing;
			MyRemoteControl myRemoteControl = entity as MyRemoteControl;
			if (myRemoteControl != null)
			{
				myRemoteControl.OwnershipChanged -= DroneRemoteOwnershipChanged;
			}
		}

		private void DroneMainEntityOnClosing(MyEntity entity)
		{
			UnregisterDrone(entity);
		}

		private void DroneRemoteOwnershipChanged(MyTerminalBlock remote)
		{
			long ownerId = remote.OwnerId;
			if (!Sync.Players.IdentityIsNpc(ownerId))
			{
				UnregisterDrone(remote);
			}
		}

		public static void UpdatePirateAntenna(long antennaEntityId, bool remove, bool activeState, StringBuilder antennaName)
		{
			if (m_pirateAntennas == null)
			{
				return;
			}
			if (remove)
			{
				m_pirateAntennas.Remove(antennaEntityId, !m_iteratingAntennas);
				return;
			}
			string text = antennaName.ToString();
			PirateAntennaInfo value = null;
			if (!m_pirateAntennas.TryGetValue(antennaEntityId, out value))
			{
				MyPirateAntennaDefinition value2 = null;
				if (m_definitionsByAntennaName.TryGetValue(text, out value2))
				{
					value = PirateAntennaInfo.Allocate(value2);
					value.IsActive = activeState;
					m_pirateAntennas.Add(antennaEntityId, value, !m_iteratingAntennas);
				}
			}
			else if (value.AntennaDefinition.Name != text)
			{
				MyPirateAntennaDefinition value3 = null;
				if (!m_definitionsByAntennaName.TryGetValue(text, out value3))
				{
					PirateAntennaInfo.Deallocate(value);
					m_pirateAntennas.Remove(antennaEntityId, !m_iteratingAntennas);
				}
				else
				{
					value.Reset(value3);
					value.IsActive = activeState;
				}
			}
			else
			{
				value.IsActive = activeState;
			}
		}

		public static long GetPiratesId()
		{
			return m_piratesIdentityId;
		}

		private static void DebugDraw()
		{
			foreach (KeyValuePair<long, PirateAntennaInfo> pirateAntenna in m_pirateAntennas)
			{
				MyRadioAntenna entity = null;
				MyEntities.TryGetEntityById(pirateAntenna.Key, out entity);
				if (entity != null)
				{
					MyRenderProxy.DebugDrawText3D(text: "Time remaining: " + Math.Max(0, pirateAntenna.Value.AntennaDefinition.SpawnTimeMs - MySandboxGame.TotalGamePlayTimeInMilliseconds + pirateAntenna.Value.LastGenerationGameTime), worldCoord: entity.WorldMatrix.Translation, color: Color.Red, scale: 1f, depthRead: false);
				}
			}
			foreach (KeyValuePair<long, PirateAntennaInfo> pirateAntenna2 in m_pirateAntennas)
			{
				MyEntities.TryGetEntityById(pirateAntenna2.Key, out MyEntity entity2);
				if (entity2 != null)
				{
					MyRenderProxy.DebugDrawSphere(entity2.WorldMatrix.Translation, (float)entity2.PositionComp.WorldVolume.Radius, Color.BlueViolet, 1f, depthRead: false);
				}
			}
			foreach (KeyValuePair<long, DroneInfo> droneInfo in m_droneInfos)
			{
				MyEntities.TryGetEntityById(droneInfo.Key, out MyEntity entity3);
				if (entity3 != null)
				{
					MyCubeGrid myCubeGrid = entity3 as MyCubeGrid;
					if (myCubeGrid == null)
					{
						myCubeGrid = (entity3 as MyRemoteControl).CubeGrid;
					}
					MyRenderProxy.DebugDrawSphere(myCubeGrid.PositionComp.WorldVolume.Center, (float)myCubeGrid.PositionComp.WorldVolume.Radius, Color.Cyan, 1f, depthRead: false);
					MyRenderProxy.DebugDrawText3D(myCubeGrid.PositionComp.WorldVolume.Center, ((droneInfo.Value.DespawnTime - MySandboxGame.TotalGamePlayTimeInMilliseconds) / 1000).ToString(), Color.Cyan, 0.7f, depthRead: false);
				}
			}
		}

		private static void RandomShuffle<T>(List<T> input)
		{
			for (int num = input.Count - 1; num > 1; num--)
			{
				int randomInt = MyUtils.GetRandomInt(0, num);
				T value = input[num];
				input[num] = input[randomInt];
				input[randomInt] = value;
			}
		}
	}
}
