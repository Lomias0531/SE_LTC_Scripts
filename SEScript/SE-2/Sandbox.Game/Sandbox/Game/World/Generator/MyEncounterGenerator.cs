using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof(MyObjectBuilder_Encounters), null)]
	internal class MyEncounterGenerator : MySessionComponentBase
	{
		protected sealed class PersistEncounterClient_003C_003EVRage_Game_MyEncounterId : ICallSite<IMyEventOwner, MyEncounterId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyEncounterId encounterId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PersistEncounterClient(encounterId);
			}
		}

		private const double MIN_DISTANCE_TO_RECOGNIZE_MOVEMENT = 1000.0;

		private HashSet<MyEncounterId> m_persistentEncounters = new HashSet<MyEncounterId>();

		private HashSet<MyEncounterId> m_encounterSpawnInProgress = new HashSet<MyEncounterId>();

		private HashSet<MyEncounterId> m_encounterRemoveRequested = new HashSet<MyEncounterId>();

		private Dictionary<MyEntity, MyEncounterId> m_entityToEncounterId = new Dictionary<MyEntity, MyEncounterId>();

		private Dictionary<MyEncounterId, List<MyEntity>> m_encounterEntities = new Dictionary<MyEncounterId, List<MyEntity>>();

		private MyRandom m_random = new MyRandom();

		private MySpawnGroupDefinition[] m_encounterSpawnGroups;

		private MyConcurrentPool<List<MyEntity>> m_entityListsPool = new MyConcurrentPool<List<MyEntity>>(10, delegate(List<MyEntity> x)
		{
			x.Clear();
		}, 1000, () => new List<MyEntity>(10));

		private static List<float> m_spawnGroupCumulativeFrequencies;

		private readonly MyVoxelBase.StorageChanged m_OnVoxelChanged;

		private readonly Action<MySlimBlock> m_OnBlockChanged;

		private readonly Action<MyCubeGrid> m_OnGridChanged;

		private readonly Action<MyEntity> m_OnEntityClosed;

		private readonly Action<MyPositionComponentBase> m_OnEntityPositionChanged;

		public static MyEncounterGenerator Static
		{
			get;
			private set;
		}

		public override Type[] Dependencies => new Type[2]
		{
			typeof(MySector),
			typeof(MyPirateAntennas)
		};

		public void RemoveEncounter(BoundingBoxD boundingVolume, int seed)
		{
			if (MySession.Static.Settings.EnableEncounters && IsAcceptableEncounter(boundingVolume))
			{
				MyEncounterId myEncounterId = new MyEncounterId(boundingVolume, seed, 0);
				if (m_encounterSpawnInProgress.Contains(myEncounterId))
				{
					m_encounterRemoveRequested.Add(myEncounterId);
				}
				else if (!m_encounterEntities.ContainsKey(myEncounterId))
				{
					m_persistentEncounters.Contains(myEncounterId);
				}
				else
				{
					RemoveEncounter(myEncounterId, markPersistent: false, close: true);
				}
			}
		}

		private void PersistEncounter(MyEntity encounterEntity)
		{
			if (m_entityToEncounterId.TryGetValue(encounterEntity, out MyEncounterId value))
			{
				PersistEncounter(value);
			}
		}

		private void PersistEncounter(MyEncounterId id)
		{
			if (MySession.Static.Ready)
			{
				RemoveEncounter(id, markPersistent: true, close: false);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => PersistEncounterClient, id);
			}
		}

		[Event(null, 112)]
		[Reliable]
		[Broadcast]
		private static void PersistEncounterClient(MyEncounterId encounterId)
		{
			if (Static.m_encounterEntities.ContainsKey(encounterId))
			{
				Static.RemoveEncounter(encounterId, markPersistent: true, close: false);
			}
			else
			{
				Static.m_persistentEncounters.Add(encounterId);
			}
		}

		private void RemoveEncounter(MyEncounterId encounter, bool markPersistent, bool close)
		{
			if (!m_persistentEncounters.Contains(encounter))
			{
				if (markPersistent)
				{
					m_persistentEncounters.Add(encounter);
				}
				if (m_encounterEntities.TryGetValue(encounter, out List<MyEntity> value))
				{
					foreach (MyEntity item in value)
					{
						MyCubeGrid myCubeGrid = item as MyCubeGrid;
						MyVoxelBase myVoxelBase = item as MyVoxelBase;
						item.OnMarkForClose -= m_OnEntityClosed;
						item.PositionComp.OnPositionChanged -= m_OnEntityPositionChanged;
						if (myCubeGrid != null)
						{
							myCubeGrid.OnBlockAdded -= m_OnBlockChanged;
							myCubeGrid.OnGridChanged -= m_OnGridChanged;
							myCubeGrid.OnBlockRemoved -= m_OnBlockChanged;
							myCubeGrid.OnBlockIntegrityChanged -= m_OnBlockChanged;
						}
						if (myVoxelBase != null)
						{
							myVoxelBase.RangeChanged -= m_OnVoxelChanged;
						}
						if (close)
						{
							if (Sync.IsServer || item is MyVoxelBase)
							{
								item.Close();
							}
						}
						else if (markPersistent && !item.MarkedForClose)
						{
							item.Save = true;
						}
						m_entityToEncounterId.Remove(item);
					}
					m_entityListsPool.Return(value);
					m_encounterEntities.Remove(encounter);
					m_encounterRemoveRequested.Remove(encounter);
				}
			}
		}

		public void PlaceEncounterToWorld(BoundingBoxD boundingVolume, int seed)
		{
			if (!MySession.Static.Settings.EnableEncounters || m_encounterSpawnGroups.Length == 0 || !IsAcceptableEncounter(boundingVolume))
			{
				return;
			}
			MyEncounterId myEncounterId = new MyEncounterId(boundingVolume, seed, 0);
			if (m_encounterEntities.ContainsKey(myEncounterId))
			{
				m_encounterRemoveRequested.Remove(myEncounterId);
			}
			else
			{
				if (m_persistentEncounters.Contains(myEncounterId))
				{
					return;
				}
				OpenEncounter(myEncounterId);
				m_random.SetSeed(seed);
				MySpawnGroupDefinition mySpawnGroupDefinition = PickRandomEncounter(m_random, m_encounterSpawnGroups);
				Vector3D center = boundingVolume.Center;
				for (int i = 0; i < mySpawnGroupDefinition.Voxels.Count; i++)
				{
					MySpawnGroupDefinition.SpawnGroupVoxel spawnGroupVoxel = mySpawnGroupDefinition.Voxels[i];
					MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(spawnGroupVoxel.StorageName));
					if (myStorageBase != null)
					{
						Vector3D vector3D = center + spawnGroupVoxel.Offset;
						string storageName = $"Asteroid_{myEncounterId.GetHashCode()}_{boundingVolume.Round().GetHashCode()}_{i}";
						long asteroidEntityId = MyProceduralAsteroidCellGenerator.GetAsteroidEntityId(storageName);
						RegisterEntityToEncounter(entity: (!spawnGroupVoxel.CenterOffset) ? MyWorldGenerator.AddVoxelMap(storageName, myStorageBase, vector3D, asteroidEntityId) : MyWorldGenerator.AddVoxelMap(storageName, myStorageBase, MatrixD.CreateWorld(vector3D), asteroidEntityId, lazyPhysics: false, useVoxelOffset: false), id: myEncounterId);
					}
				}
				if (!Sync.IsServer)
				{
					return;
				}
				bool flag = true;
				if (mySpawnGroupDefinition.IsPirate)
				{
					MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(MyPirateAntennas.GetPiratesId());
					if (myIdentity == null)
					{
						MyLog.Default.Error("Missing pirate identity. Encounter will not spawn.");
						flag = false;
					}
					else if (!myIdentity.BlockLimits.HasRemainingPCU)
					{
						MyLog.Default.Log(MyLogSeverity.Info, "Exhausted pirate PCUs. Encounter will not spawn.");
						flag = false;
					}
				}
				if (flag)
				{
					SpawnEncounterGrids(myEncounterId, center, mySpawnGroupDefinition);
				}
				else if (mySpawnGroupDefinition.Prefabs.Count > 0)
				{
					PersistEncounter(myEncounterId);
				}
			}
		}

		private void SpawnEncounterGrids(MyEncounterId encounterId, Vector3D placePosition, MySpawnGroupDefinition spawnGroup)
		{
			m_encounterSpawnInProgress.Add(encounterId);
			long ownerId = 0L;
			if (spawnGroup.IsPirate)
			{
				ownerId = MyPirateAntennas.GetPiratesId();
			}
			int remainingPrefabsToSpawn = spawnGroup.Prefabs.Count + 1;
			Action action = delegate
			{
				remainingPrefabsToSpawn--;
				if (remainingPrefabsToSpawn == 0)
				{
					m_encounterSpawnInProgress.Remove(encounterId);
					if (m_encounterRemoveRequested.Contains(encounterId))
					{
						RemoveEncounter(encounterId, markPersistent: false, close: true);
					}
				}
			};
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab selectedPrefab in spawnGroup.Prefabs)
			{
				List<MyCubeGrid> createdGrids = new List<MyCubeGrid>();
				Vector3D vector3D = Vector3D.Forward;
				Vector3D v = Vector3D.Up;
				SpawningOptions spawningOptions = SpawningOptions.DisableSave | SpawningOptions.UseGridOrigin | SpawningOptions.SetAuthorship;
				if (selectedPrefab.Speed > 0f)
				{
					spawningOptions |= (SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SpawnRandomCargo | SpawningOptions.DisableDampeners);
					float num = (float)Math.Atan(2000.0 / placePosition.Length());
					vector3D = -Vector3D.Normalize(placePosition);
					float num2 = m_random.NextFloat(num, num + 0.5f);
					float num3 = m_random.NextFloat(0f, 6.283186f);
					Vector3D vector3D2 = Vector3D.CalculatePerpendicularVector(vector3D);
					Vector3D value = Vector3D.Cross(vector3D, vector3D2);
					vector3D2 *= Math.Sin(num2) * Math.Cos(num3);
					value *= Math.Sin(num2) * Math.Sin(num3);
					vector3D = vector3D * Math.Cos(num2) + vector3D2 + value;
					v = Vector3D.CalculatePerpendicularVector(vector3D);
				}
				if (selectedPrefab.PlaceToGridOrigin)
				{
					spawningOptions |= SpawningOptions.UseGridOrigin;
				}
				if (selectedPrefab.ResetOwnership && !spawnGroup.IsPirate)
				{
					spawningOptions |= SpawningOptions.SetNeutralOwner;
				}
				if (!spawnGroup.ReactorsOn)
				{
					spawningOptions |= SpawningOptions.TurnOffReactors;
				}
				Stack<Action> stack = new Stack<Action>();
				stack.Push(action);
				stack.Push(delegate
				{
					foreach (MyCubeGrid item in createdGrids)
					{
						RegisterEntityToEncounter(encounterId, item);
					}
					string behaviour = selectedPrefab.Behaviour;
					if (!string.IsNullOrWhiteSpace(behaviour))
					{
						foreach (MyCubeGrid item2 in createdGrids)
						{
							if (!MyDroneAI.SetAIToGrid(item2, behaviour, selectedPrefab.BehaviourActivationDistance))
							{
								MyLog.Default.Error("Could not inject AI to encounter {0}. No remote control.", item2.DisplayName);
							}
						}
					}
				});
				MyPrefabManager.Static.SpawnPrefab(createdGrids, selectedPrefab.SubtypeId, placePosition + selectedPrefab.Position, vector3D, v, beaconName: selectedPrefab.BeaconText, initialLinearVelocity: vector3D * selectedPrefab.Speed, initialAngularVelocity: default(Vector3), entityName: null, spawningOptions: spawningOptions, ownerId: ownerId, updateSync: true, callbacks: stack);
			}
			action();
		}

		private static MySpawnGroupDefinition PickRandomEncounter(MyRandom random, MySpawnGroupDefinition[] candidates)
		{
			using (MyUtils.ReuseCollection(ref m_spawnGroupCumulativeFrequencies))
			{
				float num = 0f;
				foreach (MySpawnGroupDefinition mySpawnGroupDefinition in candidates)
				{
					num += mySpawnGroupDefinition.Frequency;
					m_spawnGroupCumulativeFrequencies.Add(num);
				}
				int j = 0;
				for (float num2 = random.NextFloat(0f, num); j < m_spawnGroupCumulativeFrequencies.Count && !(num2 <= m_spawnGroupCumulativeFrequencies[j]); j++)
				{
				}
				if (j >= m_spawnGroupCumulativeFrequencies.Count)
				{
					j = m_spawnGroupCumulativeFrequencies.Count - 1;
				}
				return candidates[j];
			}
		}

		private void OpenEncounter(MyEncounterId id)
		{
			if (!m_encounterEntities.TryGetValue(id, out List<MyEntity> value))
			{
				value = m_entityListsPool.Get();
				m_encounterEntities.Add(id, value);
			}
		}

		private void RegisterEntityToEncounter(MyEncounterId id, MyEntity entity)
		{
			entity.Save = false;
			if (!m_encounterEntities.TryGetValue(id, out List<MyEntity> value))
			{
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (Sync.IsServer)
			{
				entity.OnMarkForClose += m_OnEntityClosed;
				entity.PositionComp.OnPositionChanged += m_OnEntityPositionChanged;
				if (myCubeGrid != null)
				{
					myCubeGrid.OnGridChanged += m_OnGridChanged;
					myCubeGrid.OnBlockAdded += m_OnBlockChanged;
					myCubeGrid.OnBlockRemoved += m_OnBlockChanged;
					myCubeGrid.OnBlockIntegrityChanged += m_OnBlockChanged;
				}
				if (myVoxelBase != null)
				{
					myVoxelBase.RangeChanged += m_OnVoxelChanged;
				}
			}
			value.Add(entity);
			m_entityToEncounterId.Add(entity, id);
		}

		private void OnVoxelChanged(MyVoxelBase voxel, Vector3I minvoxelchanged, Vector3I maxvoxelchanged, MyStorageDataTypeFlags changeddata)
		{
			PersistEncounter(voxel);
		}

		private void OnBlockChanged(MySlimBlock block)
		{
			AssertNotClosed(block.CubeGrid);
			PersistEncounter(block.CubeGrid);
		}

		private void OnGridChanged(MyCubeGrid grid)
		{
			AssertNotClosed(grid);
			Static.PersistEncounter(grid);
		}

		private void OnEntityClosed(MyEntity entity)
		{
			PersistEncounter(entity);
		}

		private void OnEntityPositionChanged(MyPositionComponentBase obj)
		{
			MyEntity myEntity = (MyEntity)obj.Container.Entity;
			AssertNotClosed(myEntity);
			if (m_entityToEncounterId.TryGetValue(myEntity, out MyEncounterId value))
			{
				Vector3D position = obj.GetPosition();
				if (Vector3D.Distance(value.BoundingBox.Center, position) > 1000.0)
				{
					PersistEncounter(myEntity);
				}
			}
		}

		private static void AssertNotClosed(MyEntity entity)
		{
			_ = entity.MarkedForClose;
		}

		private static bool IsAcceptableEncounter(BoundingBoxD boundingBox)
		{
			Vector3D center = boundingBox.Center;
			MyWorldGeneratorStartingStateBase[] possiblePlayerStarts = MySession.Static.Scenario.PossiblePlayerStarts;
			for (int i = 0; i < possiblePlayerStarts.Length; i++)
			{
				Vector3D value = possiblePlayerStarts[i].GetStartingLocation() ?? Vector3D.Zero;
				if (Vector3D.DistanceSquared(center, value) < 225000000.0)
				{
					return false;
				}
			}
			return true;
		}

		public MyEncounterGenerator()
		{
			m_OnGridChanged = OnGridChanged;
			m_OnBlockChanged = OnBlockChanged;
			m_OnEntityClosed = OnEntityClosed;
			m_OnVoxelChanged = OnVoxelChanged;
			m_OnEntityPositionChanged = OnEntityPositionChanged;
		}

		public override void LoadData()
		{
			Static = this;
			m_encounterSpawnGroups = (from x in MyDefinitionManager.Static.GetSpawnGroupDefinitions()
				where x.IsEncounter
				select x).ToArray();
		}

		protected override void UnloadData()
		{
			while (m_encounterEntities.Count > 0)
			{
				RemoveEncounter(m_encounterEntities.FirstPair().Key, markPersistent: false, close: true);
			}
			base.UnloadData();
			Static = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_Encounters obj = (MyObjectBuilder_Encounters)base.GetObjectBuilder();
			obj.SavedEncounters = new HashSet<MyEncounterId>(m_persistentEncounters);
			return obj;
		}

		public override void Init(MyObjectBuilder_SessionComponent objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_Encounters myObjectBuilder_Encounters = (MyObjectBuilder_Encounters)objectBuilder;
			if (myObjectBuilder_Encounters.SavedEncounters != null)
			{
				m_persistentEncounters = myObjectBuilder_Encounters.SavedEncounters;
			}
		}

		public void DebugDraw()
		{
			Vector3D position = MySector.MainCamera.Position;
			foreach (MyEncounterId key in m_encounterEntities.Keys)
			{
				MyRenderProxy.DebugDrawAABB(key.BoundingBox, Color.Blue);
				BoundingBoxD boundingBox = key.BoundingBox;
				Vector3D center = boundingBox.Center;
				if (Vector3D.Distance(position, center) < 500.0)
				{
					MyRenderProxy.DebugDrawText3D(center, key.ToString(), Color.Blue, 0.7f, depthRead: false);
				}
			}
			foreach (MyEncounterId persistentEncounter in m_persistentEncounters)
			{
				MyRenderProxy.DebugDrawAABB(persistentEncounter.BoundingBox, Color.Red);
				BoundingBoxD boundingBox = persistentEncounter.BoundingBox;
				Vector3D center2 = boundingBox.Center;
				if (Vector3D.Distance(position, center2) < 500.0)
				{
					MyRenderProxy.DebugDrawText3D(center2, persistentEncounter.ToString(), Color.Red, 0.7f, depthRead: false);
				}
			}
		}

		public bool IsEncounter(MyEntity entity)
		{
			return m_entityToEncounterId.ContainsKey(entity);
		}
	}
}
