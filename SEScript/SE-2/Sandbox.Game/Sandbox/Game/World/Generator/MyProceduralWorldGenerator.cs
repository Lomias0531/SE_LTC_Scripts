using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof(MyObjectBuilder_WorldGenerator), null)]
	[StaticEventOwner]
	public class MyProceduralWorldGenerator : MySessionComponentBase
	{
		protected sealed class AddExistingObjectsSeed_003C_003EVRage_Game_MyObjectSeedParams : ICallSite<IMyEventOwner, MyObjectSeedParams, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectSeedParams seed, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddExistingObjectsSeed(seed);
			}
		}

		public static MyProceduralWorldGenerator Static;

		private int m_seed;

		private double m_objectDensity = -1.0;

		private List<MyProceduralWorldModule> m_modules = new List<MyProceduralWorldModule>();

		private Dictionary<MyEntity, MyEntityTracker> m_trackedEntities = new Dictionary<MyEntity, MyEntityTracker>();

		private Dictionary<MyEntity, MyEntityTracker> m_toAddTrackedEntities = new Dictionary<MyEntity, MyEntityTracker>();

		private HashSet<EmptyArea> m_markedAreas = new HashSet<EmptyArea>();

		private HashSet<EmptyArea> m_deletedAreas = new HashSet<EmptyArea>();

		private HashSet<MyObjectSeedParams> m_existingObjectsSeeds = new HashSet<MyObjectSeedParams>();

		private List<MyProceduralCell> m_tempProceduralCellsList = new List<MyProceduralCell>();

		private List<MyObjectSeed> m_tempObjectSeedList = new List<MyObjectSeed>();

		private MyProceduralPlanetCellGenerator m_planetsModule;

		private MyProceduralAsteroidCellGenerator m_asteroidsModule;

		public bool Enabled
		{
			get;
			private set;
		}

		public override Type[] Dependencies => new Type[2]
		{
			typeof(MySector),
			typeof(MyEncounterGenerator)
		};

		public DictionaryReader<MyEntity, MyEntityTracker> GetTrackedEntities()
		{
			return new DictionaryReader<MyEntity, MyEntityTracker>(m_trackedEntities);
		}

		public void GetAllExistingCells(List<MyProceduralCell> list)
		{
			list.Clear();
			foreach (MyProceduralWorldModule module in m_modules)
			{
				module.GetAllCells(list);
			}
		}

		public void GetAllExisting(List<MyObjectSeed> list)
		{
			list.Clear();
			GetAllExistingCells(m_tempProceduralCellsList);
			foreach (MyProceduralCell tempProceduralCells in m_tempProceduralCellsList)
			{
				tempProceduralCells.GetAll(list, clear: false);
			}
			m_tempProceduralCellsList.Clear();
		}

		public void GetAllInSphere(BoundingSphereD area, List<MyObjectSeed> list)
		{
			foreach (MyProceduralWorldModule module in m_modules)
			{
				module.GetObjectSeeds(area, list, scale: false);
				module.MarkCellsDirty(area, null, scale: false);
			}
		}

		public void GetAllInSphere<T>(BoundingSphereD area, List<MyObjectSeed> list) where T : MyProceduralWorldModule
		{
			foreach (MyProceduralWorldModule module in m_modules)
			{
				if (module is T)
				{
					module.GetObjectSeeds(area, list, scale: false);
					break;
				}
			}
		}

		public void GetOverlapAllBoundingBox<T>(BoundingBoxD area, List<MyObjectSeed> list) where T : MyProceduralWorldModule
		{
			foreach (MyProceduralWorldModule module in m_modules)
			{
				if (module is T)
				{
					module.OverlapAllBoundingBox(ref area, list);
					break;
				}
			}
		}

		public void OverlapAllPlanetSeedsInSphere(BoundingSphereD area, List<MyObjectSeed> list)
		{
			if (m_planetsModule != null)
			{
				m_planetsModule.GetObjectSeeds(area, list, scale: false);
				m_planetsModule.MarkCellsDirty(area, null, scale: false);
			}
		}

		public void OverlapAllAsteroidSeedsInSphere(BoundingSphereD area, List<MyObjectSeed> list)
		{
			if (m_asteroidsModule != null)
			{
				m_asteroidsModule.GetObjectSeeds(area, list, scale: false);
				m_asteroidsModule.MarkCellsDirty(area, null, scale: false);
			}
		}

		public override void LoadData()
		{
			Static = this;
			Enabled = true;
			MySandboxGame.Log.WriteLine("Loading Procedural World Generator");
			if (Sync.IsServer)
			{
				MyStationCellGenerator item = new MyStationCellGenerator(25000.0, 1, 0, 1.0);
				m_modules.Add(item);
			}
			if (MyFakes.ENABLE_ASTEROID_FIELDS)
			{
				MyObjectBuilder_SessionSettings settings = MySession.Static.Settings;
				if (settings.ProceduralDensity == 0f)
				{
					MySandboxGame.Log.WriteLine("Procedural Density is 0. Skipping Procedural World Generator for asteroids and encounters.");
					return;
				}
				m_seed = settings.ProceduralSeed;
				m_objectDensity = MathHelper.Clamp(settings.ProceduralDensity * 2f - 1f, -1f, 1f);
				MySandboxGame.Log.WriteLine($"Loading Procedural World Generator: Seed = '{settings.ProceduralSeed}' = {m_seed}, Density = {settings.ProceduralDensity}");
				using (MyRandom.Instance.PushSeed(m_seed))
				{
					m_asteroidsModule = new MyProceduralAsteroidCellGenerator(m_seed, m_objectDensity);
					m_modules.Add(m_asteroidsModule);
				}
			}
		}

		protected override void UnloadData()
		{
			Enabled = false;
			MySandboxGame.Log.WriteLine("Unloading Procedural World Generator");
			m_modules.Clear();
			m_trackedEntities.Clear();
			m_tempObjectSeedList.Clear();
			Static = null;
		}

		public override void UpdateBeforeSimulation()
		{
			if (Enabled)
			{
				if (m_toAddTrackedEntities.Count != 0)
				{
					foreach (KeyValuePair<MyEntity, MyEntityTracker> toAddTrackedEntity in m_toAddTrackedEntities)
					{
						m_trackedEntities.Add(toAddTrackedEntity.Key, toAddTrackedEntity.Value);
					}
					m_toAddTrackedEntities.Clear();
				}
				foreach (MyEntityTracker value in m_trackedEntities.Values)
				{
					if (value.ShouldGenerate())
					{
						BoundingSphereD boundingVolume = value.BoundingVolume;
						value.UpdateLastPosition();
						foreach (MyProceduralWorldModule module in m_modules)
						{
							module.GetObjectSeeds(value.BoundingVolume, m_tempObjectSeedList);
							module.GenerateObjects(m_tempObjectSeedList, m_existingObjectsSeeds);
							m_tempObjectSeedList.Clear();
							module.MarkCellsDirty(boundingVolume, value.BoundingVolume);
						}
					}
				}
				foreach (MyProceduralWorldModule module2 in m_modules)
				{
					module2.ProcessDirtyCells(m_trackedEntities);
				}
			}
			if (!MySandboxGame.AreClipmapsReady && MySession.Static.VoxelMaps.Instances.Count == 0 && Sync.IsServer)
			{
				MySandboxGame.AreClipmapsReady = true;
			}
		}

		public void TrackEntity(MyEntity entity)
		{
			if (!Enabled)
			{
				return;
			}
			if (entity is MyCharacter)
			{
				int num = MySession.Static.Settings.ViewDistance;
				if (MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
				{
					num = 50000;
				}
				TrackEntity(entity, num);
			}
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = (entity as MyCubeGrid)) != null && !myCubeGrid.IsStatic)
			{
				TrackEntity(entity, entity.PositionComp.WorldAABB.HalfExtents.Length());
				myCubeGrid.OnStaticChanged += OnGridStaticChanged;
			}
		}

		private void OnGridStaticChanged(MyCubeGrid grid, bool newIsStatic)
		{
			if (!newIsStatic)
			{
				Static.TrackEntity(grid, grid.PositionComp.WorldAABB.HalfExtents.Length());
			}
			else
			{
				Static.RemoveTrackedEntity(grid);
			}
		}

		private void TrackEntity(MyEntity entity, double range)
		{
			if (m_trackedEntities.TryGetValue(entity, out MyEntityTracker value) || m_toAddTrackedEntities.TryGetValue(entity, out value))
			{
				value.Radius = range;
				return;
			}
			value = new MyEntityTracker(entity, range);
			m_toAddTrackedEntities.Add(entity, value);
			entity.OnClose += delegate(MyEntity e)
			{
				RemoveTrackedEntity(e);
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = (entity as MyCubeGrid)) != null)
				{
					myCubeGrid.OnStaticChanged -= OnGridStaticChanged;
				}
			};
		}

		public void RemoveTrackedEntity(MyEntity entity)
		{
			if (m_trackedEntities.TryGetValue(entity, out MyEntityTracker value))
			{
				m_trackedEntities.Remove(entity);
				m_toAddTrackedEntities.Remove(entity);
				foreach (MyProceduralWorldModule module in m_modules)
				{
					module.MarkCellsDirty(value.BoundingVolume);
				}
			}
		}

		public override bool UpdatedBeforeInit()
		{
			return true;
		}

		public static Vector3D GetRandomDirection(MyRandom random)
		{
			double num = random.NextDouble() * 2.0 * Math.PI;
			double num2 = random.NextDouble() * 2.0 - 1.0;
			double num3 = Math.Sqrt(1.0 - num2 * num2);
			return new Vector3D(num3 * Math.Cos(num), num3 * Math.Sin(num), num2);
		}

		public void MarkDeletedArea(Vector3D pos, float radius)
		{
			MarkModules(pos, radius, planet: false);
			lock (m_deletedAreas)
			{
				m_deletedAreas.Add(new EmptyArea
				{
					Position = pos,
					Radius = radius
				});
			}
		}

		public void MarkEmptyArea(Vector3D pos, float radius)
		{
			MarkModules(pos, radius, planet: true);
			lock (m_deletedAreas)
			{
				m_markedAreas.Add(new EmptyArea
				{
					Position = pos,
					Radius = radius
				});
			}
		}

		private void MarkModules(Vector3D pos, float radius, bool planet)
		{
			MySphereDensityFunction func = (!planet) ? new MySphereDensityFunction(pos, radius, 0.0) : new MySphereDensityFunction(pos, (double)radius * 1.1 + 16000.0, 16000.0);
			foreach (MyProceduralWorldModule module in m_modules)
			{
				module.AddDensityFunctionRemoved(func);
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_WorldGenerator myObjectBuilder_WorldGenerator = (MyObjectBuilder_WorldGenerator)sessionComponent;
			if (!Sync.IsServer)
			{
				m_markedAreas = myObjectBuilder_WorldGenerator.MarkedAreas;
			}
			m_deletedAreas = myObjectBuilder_WorldGenerator.DeletedAreas;
			m_existingObjectsSeeds = myObjectBuilder_WorldGenerator.ExistingObjectsSeeds;
			if (m_markedAreas == null)
			{
				m_markedAreas = new HashSet<EmptyArea>();
			}
			foreach (EmptyArea markedArea in m_markedAreas)
			{
				MarkModules(markedArea.Position, markedArea.Radius, planet: true);
			}
			foreach (EmptyArea deletedArea in m_deletedAreas)
			{
				MarkModules(deletedArea.Position, deletedArea.Radius, planet: false);
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_WorldGenerator obj = (MyObjectBuilder_WorldGenerator)base.GetObjectBuilder();
			obj.MarkedAreas = m_markedAreas;
			obj.DeletedAreas = m_deletedAreas;
			obj.ExistingObjectsSeeds = m_existingObjectsSeeds;
			return obj;
		}

		[Event(null, 551)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public static void AddExistingObjectsSeed(MyObjectSeedParams seed)
		{
			Static.m_existingObjectsSeeds.Add(seed);
		}
	}
}
