using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[StaticEventOwner]
	public static class MyEntities
	{
		public class InitEntityData : WorkData
		{
			private readonly MyObjectBuilder_EntityBase m_objectBuilder;

			private readonly bool m_addToScene;

			private readonly Action<MyEntity> m_completionCallback;

			private MyEntity m_entity;

			private List<IMyEntity> m_resultIDs;

			private readonly MyEntity m_relativeSpawner;

			private Vector3D? m_relativeOffset;

			private readonly bool m_checkPosition;

			private readonly bool m_fadeIn;

			public InitEntityData(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> completionCallback, MyEntity entity, bool fadeIn, MyEntity relativeSpawner = null, Vector3D? relativeOffset = null, bool checkPosition = false)
			{
				m_objectBuilder = objectBuilder;
				m_addToScene = addToScene;
				m_completionCallback = completionCallback;
				m_entity = entity;
				m_fadeIn = fadeIn;
				m_relativeSpawner = relativeSpawner;
				m_relativeOffset = relativeOffset;
				m_checkPosition = checkPosition;
			}

			public bool CallInitEntity()
			{
				try
				{
					MyEntityIdentifier.InEntityCreationBlock = true;
					MyEntityIdentifier.LazyInitPerThreadStorage(2048);
					m_entity.Render.FadeIn = m_fadeIn;
					InitEntity(m_objectBuilder, ref m_entity);
					return m_entity != null;
				}
				finally
				{
					m_resultIDs = new List<IMyEntity>();
					MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
					MyEntityIdentifier.ClearPerThreadEntities();
					MyEntityIdentifier.InEntityCreationBlock = false;
				}
			}

			public void OnEntityInitialized()
			{
				if (m_relativeSpawner != null && m_relativeOffset.HasValue)
				{
					MatrixD worldMatrix = m_entity.WorldMatrix;
					worldMatrix.Translation = m_relativeSpawner.WorldMatrix.Translation + m_relativeOffset.Value;
					m_entity.WorldMatrix = worldMatrix;
				}
				MyCubeGrid myCubeGrid = m_entity as MyCubeGrid;
				if (MyFakes.ENABLE_GRID_PLACEMENT_TEST && m_checkPosition && myCubeGrid != null && myCubeGrid.CubeBlocks.Count == 1)
				{
					MyGridPlacementSettings settings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic);
					if (!MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false))
					{
						MyLog.Default.Info($"OnEntityInitialized removed entity '{myCubeGrid.Name}:{myCubeGrid.DisplayName}' with entity id '{myCubeGrid.EntityId}'");
						m_entity.Close();
						return;
					}
				}
				foreach (IMyEntity resultID in m_resultIDs)
				{
					MyEntityIdentifier.TryGetEntity(resultID.EntityId, out IMyEntity entity);
					if (entity != null)
					{
						MyLog.Default.WriteLineAndConsole("Dropping entity with duplicated id: " + resultID.EntityId);
						resultID.Close();
					}
					else
					{
						MyEntityIdentifier.AddEntityWithId(resultID);
					}
				}
				if (m_entity != null && m_entity.EntityId != 0L)
				{
					if (m_addToScene)
					{
						bool insertIntoScene = (m_objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
						Add(m_entity, insertIntoScene);
					}
					if (m_completionCallback != null)
					{
						m_completionCallback(m_entity);
					}
				}
			}
		}

		private struct BoundingBoxDrawArgs
		{
			public Color Color;

			public float LineWidth;

			public Vector3 InflateAmount;

			public MyStringId lineMaterial;
		}

		protected sealed class OnEntityCloseRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnEntityCloseRequest(entityId);
			}
		}

		protected sealed class ForceCloseEntityOnClients_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ForceCloseEntityOnClients(entityId);
			}
		}

		private static readonly long EntityNativeMemoryLimit;

		private static readonly long EntityManagedMemoryLimit;

		private static MyConcurrentHashSet<MyEntity> m_entities;

		private static CachingList<MyEntity> m_entitiesForUpdateOnce;

		private static MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity> m_entitiesForUpdate;

		private static MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate10;

		private static MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate100;

		private static MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForSimulate;

		private static CachingList<IMyEntity> m_entitiesForDraw;

		private static readonly List<IMySceneComponent> m_sceneComponents;

		[ThreadStatic]
		private static MyEntityIdRemapHelper m_remapHelper;

		private static readonly int MAX_ENTITIES_CLOSE_PER_UPDATE;

		public static bool IsClosingAll;

		public static bool IgnoreMemoryLimits;

		private static MyEntityCreationThread m_creationThread;

		private static Dictionary<uint, IMyEntity> m_renderObjectToEntityMap;

		private static readonly FastResourceLock m_renderObjectToEntityMapLock;

		[ThreadStatic]
		private static List<MyEntity> m_overlapRBElementList;

		private static readonly List<List<MyEntity>> m_overlapRBElementListCollection;

		private static List<HkBodyCollision> m_rigidBodyList;

		private static readonly List<MyLineSegmentOverlapResult<MyEntity>> LineOverlapEntityList;

		private static readonly List<MyPhysics.HitInfo> m_hits;

		[ThreadStatic]
		private static HashSet<IMyEntity> m_entityResultSet;

		private static readonly List<HashSet<IMyEntity>> m_entityResultSetCollection;

		[ThreadStatic]
		private static List<MyEntity> m_entityInputList;

		private static readonly List<List<MyEntity>> m_entityInputListCollection;

		private static HashSet<MyEntity> m_entitiesToDelete;

		private static HashSet<MyEntity> m_entitiesToDeleteNextFrame;

		public static ConcurrentDictionary<string, MyEntity> m_entityNameDictionary;

		private static bool m_isLoaded;

		private static HkShape m_cameraSphere;

		public static FastResourceLock EntityCloseLock;

		public static FastResourceLock EntityMarkForCloseLock;

		public static FastResourceLock UnloadDataLock;

		public static bool UpdateInProgress;

		public static bool CloseAllowed;

		private static int m_update10Index;

		private static int m_update100Index;

		private static float m_update10Count;

		private static float m_update100Count;

		[ThreadStatic]
		private static List<MyEntity> m_allIgnoredEntities;

		private static readonly List<List<MyEntity>> m_allIgnoredEntitiesCollection;

		private static readonly HashSet<Type> m_hiddenTypes;

		public static bool SafeAreasHidden;

		public static bool SafeAreasSelectable;

		public static bool DetectorsHidden;

		public static bool DetectorsSelectable;

		public static bool ParticleEffectsHidden;

		public static bool ParticleEffectsSelectable;

		private static readonly Dictionary<string, int> m_typesStats;

		private static List<MyCubeGrid> m_cubeGridList;

		private static readonly HashSet<MyCubeGrid> m_cubeGridHash;

		private static readonly HashSet<IMyEntity> m_entitiesForDebugDraw;

		private static readonly HashSet<object> m_groupDebugHelper;

		private static readonly MyStringId GIZMO_LINE_MATERIAL;

		private static readonly CachingDictionary<MyEntity, BoundingBoxDrawArgs> m_entitiesForBBoxDraw;

		private static List<MyEntity> OverlapRBElementList
		{
			get
			{
				if (m_overlapRBElementList == null)
				{
					m_overlapRBElementList = new List<MyEntity>(256);
					lock (m_overlapRBElementListCollection)
					{
						m_overlapRBElementListCollection.Add(m_overlapRBElementList);
					}
				}
				return m_overlapRBElementList;
			}
		}

		private static HashSet<IMyEntity> EntityResultSet
		{
			get
			{
				if (m_entityResultSet == null)
				{
					m_entityResultSet = new HashSet<IMyEntity>();
					lock (m_entityResultSetCollection)
					{
						m_entityResultSetCollection.Add(m_entityResultSet);
					}
				}
				return m_entityResultSet;
			}
		}

		private static List<MyEntity> EntityInputList
		{
			get
			{
				if (m_entityInputList == null)
				{
					m_entityInputList = new List<MyEntity>(32);
					lock (m_entityInputListCollection)
					{
						m_entityInputListCollection.Add(m_entityInputList);
					}
				}
				return m_entityInputList;
			}
		}

		public static bool IsLoaded => m_isLoaded;

		private static List<MyEntity> AllIgnoredEntities
		{
			get
			{
				if (m_allIgnoredEntities == null)
				{
					m_allIgnoredEntities = new List<MyEntity>();
					m_allIgnoredEntitiesCollection.Add(m_allIgnoredEntities);
				}
				return m_allIgnoredEntities;
			}
		}

		public static bool MemoryLimitAddFailure
		{
			get;
			private set;
		}

		public static event Action<MyEntity> OnEntityRemove;

		public static event Action<MyEntity> OnEntityAdd;

		public static event Action<MyEntity> OnEntityCreate;

		public static event Action<MyEntity> OnEntityDelete;

		public static event Action OnCloseAll;

		public static event Action<MyEntity, string, string> OnEntityNameSet;

		static MyEntities()
		{
			EntityNativeMemoryLimit = 1363148800L;
			EntityManagedMemoryLimit = 681574400L;
			m_entities = new MyConcurrentHashSet<MyEntity>();
			m_entitiesForUpdateOnce = new CachingList<MyEntity>();
			m_entitiesForUpdate = new MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity>(1);
			m_entitiesForUpdate10 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(10);
			m_entitiesForUpdate100 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);
			m_entitiesForSimulate = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(1);
			m_entitiesForDraw = new CachingList<IMyEntity>();
			m_sceneComponents = new List<IMySceneComponent>();
			MAX_ENTITIES_CLOSE_PER_UPDATE = 10;
			IsClosingAll = false;
			IgnoreMemoryLimits = false;
			m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
			m_renderObjectToEntityMapLock = new FastResourceLock();
			m_overlapRBElementListCollection = new List<List<MyEntity>>();
			m_rigidBodyList = new List<HkBodyCollision>();
			LineOverlapEntityList = new List<MyLineSegmentOverlapResult<MyEntity>>();
			m_hits = new List<MyPhysics.HitInfo>();
			m_entityResultSetCollection = new List<HashSet<IMyEntity>>();
			m_entityInputListCollection = new List<List<MyEntity>>();
			m_entitiesToDelete = new HashSet<MyEntity>();
			m_entitiesToDeleteNextFrame = new HashSet<MyEntity>();
			m_entityNameDictionary = new ConcurrentDictionary<string, MyEntity>();
			m_isLoaded = false;
			EntityCloseLock = new FastResourceLock();
			EntityMarkForCloseLock = new FastResourceLock();
			UnloadDataLock = new FastResourceLock();
			UpdateInProgress = false;
			CloseAllowed = false;
			m_update10Index = 0;
			m_update100Index = 0;
			m_update10Count = 0f;
			m_update100Count = 0f;
			m_allIgnoredEntitiesCollection = new List<List<MyEntity>>();
			m_hiddenTypes = new HashSet<Type>();
			m_typesStats = new Dictionary<string, int>();
			m_cubeGridList = new List<MyCubeGrid>();
			m_cubeGridHash = new HashSet<MyCubeGrid>();
			m_entitiesForDebugDraw = new HashSet<IMyEntity>();
			m_groupDebugHelper = new HashSet<object>();
			GIZMO_LINE_MATERIAL = MyStringId.GetOrCompute("GizmoDrawLine");
			m_entitiesForBBoxDraw = new CachingDictionary<MyEntity, BoundingBoxDrawArgs>();
			Type typeFromHandle = typeof(MyEntity);
			MyEntityFactory.RegisterDescriptor(typeFromHandle.GetCustomAttribute<MyEntityTypeAttribute>(inherit: false), typeFromHandle);
			MyEntityFactory.RegisterDescriptorsFromAssembly(typeof(MyEntities).Assembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.GameAssembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.SandboxAssembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.UserAssemblies);
			MyEntityExtensions.SetCallbacks();
			MyEntitiesInterface.RegisterUpdate = RegisterForUpdate;
			MyEntitiesInterface.UnregisterUpdate = UnregisterForUpdate;
			MyEntitiesInterface.RegisterDraw = RegisterForDraw;
			MyEntitiesInterface.UnregisterDraw = UnregisterForDraw;
			MyEntitiesInterface.SetEntityName = SetEntityName;
			MyEntitiesInterface.IsUpdateInProgress = IsUpdateInProgress;
			MyEntitiesInterface.IsCloseAllowed = IsCloseAllowed;
			MyEntitiesInterface.RemoveName = RemoveName;
			MyEntitiesInterface.RemoveFromClosedEntities = RemoveFromClosedEntities;
			MyEntitiesInterface.Remove = Remove;
			MyEntitiesInterface.RaiseEntityRemove = RaiseEntityRemove;
			MyEntitiesInterface.Close = Close;
		}

		public static void AddRenderObjectToMap(uint id, IMyEntity entity)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				using (m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
				{
					m_renderObjectToEntityMap.Add(id, entity);
				}
			}
		}

		public static void RemoveRenderObjectFromMap(uint id)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				using (m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
				{
					m_renderObjectToEntityMap.Remove(id);
				}
			}
		}

		public static bool IsShapePenetrating(HkShape shape, ref Vector3D position, ref Quaternion rotation, int filter = 15, MyEntity ignoreEnt = null)
		{
			using (MyUtils.ReuseCollection(ref m_rigidBodyList))
			{
				MyPhysics.GetPenetrationsShape(shape, ref position, ref rotation, m_rigidBodyList, filter);
				if (ignoreEnt != null)
				{
					for (int num = m_rigidBodyList.Count - 1; num >= 0; num--)
					{
						if (m_rigidBodyList[num].GetCollisionEntity() == ignoreEnt)
						{
							m_rigidBodyList.RemoveAtFast(num);
							break;
						}
					}
				}
				return m_rigidBodyList.Count > 0;
			}
		}

		public static bool IsSpherePenetrating(ref BoundingSphereD bs)
		{
			return IsShapePenetrating(m_cameraSphere, ref bs.Center, ref Quaternion.Identity);
		}

		public static Vector3D? FindFreePlace(ref MatrixD matrix, Vector3 axis, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f)
		{
			Vector3 v = matrix.Forward;
			v.Normalize();
			Vector3D position = matrix.Translation;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation) && FindFreePlaceVoxelMap(position, radius, ref shape, ref position))
				{
					return position;
				}
				int num = (int)Math.Ceiling((float)maxTestCount / (float)testsPerDistance);
				float num2 = MathF.PI * 2f / (float)testsPerDistance;
				float num3 = 0f;
				for (int i = 0; i < num; i++)
				{
					num3 += radius * stepSize;
					Vector3D value = v;
					float num4 = 0f;
					for (int j = 0; j < testsPerDistance; j++)
					{
						if (j != 0)
						{
							num4 += num2;
							Quaternion rotation2 = Quaternion.CreateFromAxisAngle(axis, num4);
							value = Vector3D.Transform(v, rotation2);
						}
						position = matrix.Translation + value * num3;
						if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation) && FindFreePlaceVoxelMap(position, radius, ref shape, ref position))
						{
							return position;
						}
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		public static Vector3D? FindFreePlace(Vector3D basePos, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f, MyEntity ignoreEnt = null)
		{
			return FindFreePlaceCustom(basePos, radius, maxTestCount, testsPerDistance, stepSize, 0f, ignoreEnt);
		}

		public static Vector3D? FindFreePlaceCustom(Vector3D basePos, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f, float radiusIncrement = 0f, MyEntity ignoreEnt = null)
		{
			Vector3D position = basePos;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation, 15, ignoreEnt))
				{
					BoundingSphereD sphere = new BoundingSphereD(position, radius);
					MyVoxelBase overlappingWithSphere = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere);
					if (overlappingWithSphere == null)
					{
						return position;
					}
					if (overlappingWithSphere is MyPlanet)
					{
						(overlappingWithSphere as MyPlanet).CorrectSpawnLocation(ref basePos, radius);
					}
					return basePos;
				}
				int num = (int)Math.Ceiling((float)maxTestCount / (float)testsPerDistance);
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					num2 += radius * stepSize + radiusIncrement;
					for (int j = 0; j < testsPerDistance; j++)
					{
						position = basePos + MyUtils.GetRandomVector3Normalized() * num2;
						if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation, 15, ignoreEnt))
						{
							BoundingSphereD sphere2 = new BoundingSphereD(position, radius);
							MyVoxelBase overlappingWithSphere2 = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere2);
							if (overlappingWithSphere2 == null)
							{
								return position;
							}
							if (overlappingWithSphere2 is MyPlanet)
							{
								(overlappingWithSphere2 as MyPlanet).CorrectSpawnLocation(ref basePos, radius);
							}
						}
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		public static Vector3D? TestPlaceInSpace(Vector3D basePos, float radius)
		{
			List<MyVoxelBase> list = new List<MyVoxelBase>();
			Vector3D position = basePos;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation))
				{
					BoundingSphereD sphere = new BoundingSphereD(position, radius);
					MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere, list);
					if (list.Count == 0)
					{
						return position;
					}
					bool flag = true;
					foreach (MyVoxelBase item in list)
					{
						MyPlanet myPlanet = item as MyPlanet;
						if (myPlanet == null)
						{
							flag = false;
							break;
						}
						if ((position - myPlanet.PositionComp.GetPosition()).Length() < (double)myPlanet.MaximumRadius)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return position;
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		private static bool FindFreePlaceVoxelMap(Vector3D currentPos, float radius, ref HkShape shape, ref Vector3D ret)
		{
			BoundingSphereD sphere = new BoundingSphereD(currentPos, radius);
			MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere)?.RootVoxel;
			if (myVoxelBase == null)
			{
				ret = currentPos;
				return true;
			}
			MyPlanet myPlanet = myVoxelBase as MyPlanet;
			if (myPlanet != null)
			{
				bool num = myPlanet.CorrectSpawnLocation2(ref currentPos, radius);
				Quaternion rotation = Quaternion.Identity;
				if (num)
				{
					if (!IsShapePenetrating(shape, ref currentPos, ref rotation))
					{
						ret = currentPos;
						return true;
					}
					if (myPlanet.CorrectSpawnLocation2(ref currentPos, radius, resumeSearch: true) && !IsShapePenetrating(shape, ref currentPos, ref rotation))
					{
						ret = currentPos;
						return true;
					}
				}
			}
			return false;
		}

		public static void GetInflatedPlayerBoundingBox(ref BoundingBoxD playerBox, float inflation)
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				playerBox.Include(onlinePlayer.GetPosition());
			}
			playerBox.Inflate(inflation);
		}

		public static bool IsInsideVoxel(Vector3D pos, Vector3D hintPosition, out Vector3D lastOutsidePos)
		{
			m_hits.Clear();
			lastOutsidePos = pos;
			MyPhysics.CastRay(hintPosition, pos, m_hits, 15);
			int num = 0;
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				if (hit.HkHitInfo.GetHitEntity() is MyVoxelMap)
				{
					num++;
					lastOutsidePos = hit.Position;
				}
			}
			m_hits.Clear();
			return num % 2 != 0;
		}

		public static bool IsWorldLimited()
		{
			if (MySession.Static != null)
			{
				return MySession.Static.Settings.WorldSizeKm != 0;
			}
			return false;
		}

		public static float WorldHalfExtent()
		{
			return (MySession.Static != null) ? (MySession.Static.Settings.WorldSizeKm * 500) : 0;
		}

		public static float WorldSafeHalfExtent()
		{
			float num = WorldHalfExtent();
			if (num != 0f)
			{
				return num - 600f;
			}
			return 0f;
		}

		public static bool IsInsideWorld(Vector3D pos)
		{
			float num = WorldHalfExtent();
			if (num == 0f)
			{
				return true;
			}
			return pos.AbsMax() <= (double)num;
		}

		public static bool IsRaycastBlocked(Vector3D pos, Vector3D target)
		{
			m_hits.Clear();
			MyPhysics.CastRay(pos, target, m_hits);
			return m_hits.Count > 0;
		}

		public static List<MyEntity> GetEntitiesInAABB(ref BoundingBox boundingBox)
		{
			BoundingBoxD box = boundingBox;
			MyGamePruningStructure.GetAllEntitiesInBox(ref box, OverlapRBElementList);
			return OverlapRBElementList;
		}

		public static List<MyEntity> GetEntitiesInAABB(ref BoundingBoxD boundingBox, bool exact = false)
		{
			MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, OverlapRBElementList);
			if (exact)
			{
				int num = 0;
				while (num < OverlapRBElementList.Count)
				{
					MyEntity myEntity = OverlapRBElementList[num];
					if (!boundingBox.Intersects(myEntity.PositionComp.WorldAABB))
					{
						OverlapRBElementList.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
			return OverlapRBElementList;
		}

		public static List<MyEntity> GetEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			MyGamePruningStructure.GetAllEntitiesInSphere(ref boundingSphere, OverlapRBElementList);
			return OverlapRBElementList;
		}

		public static List<MyEntity> GetEntitiesInOBB(ref MyOrientedBoundingBoxD obb)
		{
			MyGamePruningStructure.GetAllEntitiesInOBB(ref obb, OverlapRBElementList);
			return OverlapRBElementList;
		}

		public static List<MyEntity> GetTopMostEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref boundingSphere, OverlapRBElementList);
			return OverlapRBElementList;
		}

		public static void GetElementsInBox(ref BoundingBoxD boundingBox, List<MyEntity> foundElements)
		{
			MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, foundElements);
		}

		public static void GetTopMostEntitiesInBox(ref BoundingBoxD boundingBox, List<MyEntity> foundElements, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			MyGamePruningStructure.GetAllTopMostStaticEntitiesInBox(ref boundingBox, foundElements, qtype);
		}

		private static void AddComponents()
		{
			m_sceneComponents.Add(new MyCubeGridGroups());
			m_sceneComponents.Add(new MyWeldingGroups());
			m_sceneComponents.Add(new MyGridPhysicalHierarchy());
			m_sceneComponents.Add(new MySharedTensorsGroups());
			m_sceneComponents.Add(new MyFixedGrids());
		}

		public static void LoadData()
		{
			m_entities.Clear();
			m_entitiesToDelete.Clear();
			m_entitiesToDeleteNextFrame.Clear();
			m_cameraSphere = new HkSphereShape(0.125f);
			AddComponents();
			foreach (IMySceneComponent sceneComponent in m_sceneComponents)
			{
				sceneComponent.Load();
			}
			m_creationThread = new MyEntityCreationThread();
			m_isLoaded = true;
		}

		public static void UnloadData()
		{
			if (m_isLoaded)
			{
				m_cameraSphere.RemoveReference();
			}
			using (UnloadDataLock.AcquireExclusiveUsing())
			{
				m_creationThread.Dispose();
				m_creationThread = null;
				CloseAll();
				m_overlapRBElementList = null;
				m_entityResultSet = null;
				m_isLoaded = false;
				lock (m_entityInputListCollection)
				{
					foreach (List<MyEntity> item in m_entityInputListCollection)
					{
						item.Clear();
					}
				}
				lock (m_overlapRBElementListCollection)
				{
					foreach (List<MyEntity> item2 in m_overlapRBElementListCollection)
					{
						item2.Clear();
					}
				}
				lock (m_entityResultSetCollection)
				{
					foreach (HashSet<IMyEntity> item3 in m_entityResultSetCollection)
					{
						item3.Clear();
					}
				}
				lock (m_allIgnoredEntitiesCollection)
				{
					foreach (List<MyEntity> item4 in m_allIgnoredEntitiesCollection)
					{
						item4.Clear();
					}
				}
			}
			for (int num = m_sceneComponents.Count - 1; num >= 0; num--)
			{
				m_sceneComponents[num].Unload();
			}
			m_sceneComponents.Clear();
			MyEntities.OnEntityRemove = null;
			MyEntities.OnEntityAdd = null;
			MyEntities.OnEntityCreate = null;
			MyEntities.OnEntityDelete = null;
			m_entities = new MyConcurrentHashSet<MyEntity>();
			m_entitiesForUpdateOnce = new CachingList<MyEntity>();
			m_entitiesForUpdate = new MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity>(1);
			m_entitiesForUpdate10 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(10);
			m_entitiesForUpdate100 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);
			m_entitiesForDraw = new CachingList<IMyEntity>();
			m_remapHelper = new MyEntityIdRemapHelper();
			m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
			m_entityNameDictionary.Clear();
			m_entitiesForBBoxDraw.Clear();
		}

		public static void Add(MyEntity entity, bool insertIntoScene = true)
		{
			if (insertIntoScene)
			{
				entity.OnAddedToScene(entity);
			}
			if (!Exist(entity))
			{
				if (entity is MyVoxelBase)
				{
					MySession.Static.VoxelMaps.Add((MyVoxelBase)entity);
				}
				m_entities.Add(entity);
				if (GetEntityById(entity.EntityId) == null)
				{
					MyEntityIdentifier.AddEntityWithId(entity);
				}
				RaiseEntityAdd(entity);
			}
		}

		public static void SetEntityName(MyEntity myEntity, bool possibleRename = true)
		{
			string arg = null;
			string name = myEntity.Name;
			if (possibleRename)
			{
				foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
				{
					if (item.Value == myEntity)
					{
						m_entityNameDictionary.Remove(item.Key);
						arg = item.Key;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(myEntity.Name) && !m_entityNameDictionary.ContainsKey(myEntity.Name))
			{
				m_entityNameDictionary.TryAdd(myEntity.Name, myEntity);
			}
			if (MyEntities.OnEntityNameSet != null)
			{
				MyEntities.OnEntityNameSet(myEntity, arg, name);
			}
		}

		public static bool IsNameExists(MyEntity entity, string name)
		{
			foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
			{
				if (item.Key == name && item.Value != entity)
				{
					return true;
				}
			}
			return false;
		}

		public static bool Remove(MyEntity entity)
		{
			if (entity is MyVoxelBase)
			{
				MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase)entity);
			}
			if (m_entities.Remove(entity))
			{
				entity.OnRemovedFromScene(entity);
				RaiseEntityRemove(entity);
				return true;
			}
			return false;
		}

		public static void DeleteRememberedEntities()
		{
			CloseAllowed = true;
			while (m_entitiesToDelete.Count > 0)
			{
				using (EntityCloseLock.AcquireExclusiveUsing())
				{
					MyEntity myEntity = m_entitiesToDelete.FirstElement();
					if (!myEntity.Pinned)
					{
						MyEntities.OnEntityDelete?.Invoke(myEntity);
						myEntity.Delete();
					}
					else
					{
						Remove(myEntity);
						m_entitiesToDelete.Remove(myEntity);
						m_entitiesToDeleteNextFrame.Add(myEntity);
					}
				}
			}
			CloseAllowed = false;
			HashSet<MyEntity> entitiesToDelete = m_entitiesToDelete;
			m_entitiesToDelete = m_entitiesToDeleteNextFrame;
			m_entitiesToDeleteNextFrame = entitiesToDelete;
		}

		public static bool HasEntitiesToDelete()
		{
			return m_entitiesToDelete.Count > 0;
		}

		public static void RemoveFromClosedEntities(MyEntity entity)
		{
			if (m_entitiesToDelete.Count > 0)
			{
				m_entitiesToDelete.Remove(entity);
			}
			if (m_entitiesToDeleteNextFrame.Count > 0)
			{
				m_entitiesToDeleteNextFrame.Remove(entity);
			}
		}

		public static void RemoveName(MyEntity entity)
		{
			if (!string.IsNullOrEmpty(entity.Name))
			{
				m_entityNameDictionary.Remove(entity.Name);
			}
		}

		public static bool Exist(MyEntity entity)
		{
			if (m_entities == null)
			{
				return false;
			}
			return m_entities.Contains(entity);
		}

		public static void Close(MyEntity entity)
		{
			if (CloseAllowed)
			{
				m_entitiesToDeleteNextFrame.Add(entity);
			}
			else if (!m_entitiesToDelete.Contains(entity))
			{
				using (EntityMarkForCloseLock.AcquireExclusiveUsing())
				{
					m_entitiesToDelete.Add(entity);
				}
			}
		}

		public static void CloseAll()
		{
			IsClosingAll = true;
			if (MyEntities.OnCloseAll != null)
			{
				MyEntities.OnCloseAll();
			}
			CloseAllowed = true;
			List<MyEntity> list = new List<MyEntity>();
			foreach (MyEntity entity in m_entities)
			{
				entity.Close();
				m_entitiesToDelete.Add(entity);
			}
			MyEntity[] array = m_entitiesToDelete.ToArray();
			foreach (MyEntity myEntity in array)
			{
				if (!myEntity.Pinned)
				{
					myEntity.Render.FadeOut = false;
					myEntity.Delete();
				}
				else
				{
					list.Add(myEntity);
				}
			}
			while (list.Count > 0)
			{
				MyEntity myEntity2 = list.First();
				if (!myEntity2.Pinned)
				{
					myEntity2.Render.FadeOut = false;
					myEntity2.Delete();
					list.Remove(myEntity2);
				}
				else
				{
					Thread.Sleep(10);
				}
			}
			m_entitiesForUpdateOnce.ApplyRemovals();
			m_entitiesForUpdate.List.ApplyRemovals();
			m_entitiesForUpdate10.List.ApplyRemovals();
			m_entitiesForUpdate100.List.ApplyRemovals();
			CloseAllowed = false;
			m_entitiesToDelete.Clear();
			MyEntityIdentifier.Clear();
			MyGamePruningStructure.Clear();
			MyRadioBroadcasters.Clear();
			m_entitiesForDraw.ApplyChanges();
			IsClosingAll = false;
		}

		public static void RegisterForUpdate(MyEntity entity)
		{
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdateOnce.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate10.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate100.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.SIMULATE) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForSimulate.List.Add(entity);
			}
		}

		public static void RegisterForDraw(IMyEntity entity)
		{
			if (entity.Render.NeedsDraw)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_entitiesForDraw.Add(entity);
				}
				entity.Render.SetVisibilityUpdates(state: true);
			}
		}

		public static void UnregisterForUpdate(MyEntity entity, bool immediate = false)
		{
			if ((entity.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != 0)
			{
				m_entitiesForUpdateOnce.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate) != 0)
			{
				m_entitiesForUpdate.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate10) != 0)
			{
				m_entitiesForUpdate10.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate100) != 0)
			{
				m_entitiesForUpdate100.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsSimulate) != 0)
			{
				m_entitiesForSimulate.List.Remove(entity, immediate);
			}
		}

		public static void UnregisterForDraw(IMyEntity entity)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_entitiesForDraw.Remove(entity);
			}
			entity.Render.SetVisibilityUpdates(state: false);
		}

		public static bool IsUpdateInProgress()
		{
			return UpdateInProgress;
		}

		public static bool IsCloseAllowed()
		{
			return CloseAllowed;
		}

		public static void UpdateBeforeSimulation()
		{
			if (MySandboxGame.IsGameReady)
			{
				UpdateInProgress = true;
				UpdateOnceBeforeFrame();
				m_entitiesForUpdate.List.ApplyChanges();
				m_entitiesForUpdate.Update();
				MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
				m_entitiesForUpdate.Iterate(delegate(MyEntity x)
				{
					if (!x.MarkedForClose)
					{
						x.UpdateBeforeSimulation();
					}
				});
				m_entitiesForUpdate10.List.ApplyChanges();
				m_entitiesForUpdate10.Update();
				m_entitiesForUpdate10.Iterate(delegate(MyEntity x)
				{
					if (!x.MarkedForClose)
					{
						x.UpdateBeforeSimulation10();
					}
				});
				m_entitiesForUpdate100.List.ApplyChanges();
				m_entitiesForUpdate100.Update();
				m_entitiesForUpdate100.Iterate(delegate(MyEntity x)
				{
					if (!x.MarkedForClose)
					{
						x.UpdateBeforeSimulation100();
					}
				});
				MySimpleProfiler.End("UpdateBeforeSimulation");
				UpdateInProgress = false;
			}
		}

		public static void UpdateOnceBeforeFrame()
		{
			m_entitiesForUpdateOnce.ApplyChanges();
			foreach (MyEntity item in m_entitiesForUpdateOnce)
			{
				item.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				if (!item.MarkedForClose)
				{
					item.UpdateOnceBeforeFrame();
				}
			}
		}

		public static void Simulate()
		{
			if (MySandboxGame.IsGameReady)
			{
				UpdateInProgress = true;
				m_entitiesForSimulate.List.ApplyChanges();
				m_entitiesForSimulate.Iterate(delegate(MyEntity x)
				{
					if (!x.MarkedForClose)
					{
						x.Simulate();
					}
				});
				UpdateInProgress = false;
			}
		}

		public static void UpdateAfterSimulation()
		{
			if (!MySandboxGame.IsGameReady)
			{
				return;
			}
			UpdateInProgress = true;
			m_entitiesForUpdate.List.ApplyChanges();
			MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation");
			m_entitiesForUpdate.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation();
				}
			});
			m_entitiesForUpdate10.List.ApplyChanges();
			m_entitiesForUpdate10.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation10();
				}
			});
			m_entitiesForUpdate100.List.ApplyChanges();
			m_entitiesForUpdate100.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation100();
				}
			});
			MySimpleProfiler.End("UpdateAfterSimulation");
			UpdateInProgress = false;
			DeleteRememberedEntities();
			if (MyMultiplayer.Static != null && m_creationThread.AnyResult)
			{
				while (m_creationThread.ConsumeResult(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime()))
				{
				}
			}
		}

		public static void UpdatingStopped()
		{
			for (int i = 0; i < m_entitiesForUpdate.List.Count; i++)
			{
				m_entitiesForUpdate.List[i].UpdatingStopped();
			}
		}

		private static bool IsAnyRenderObjectVisible(MyEntity entity)
		{
			uint[] renderObjectIDs = entity.Render.RenderObjectIDs;
			foreach (uint item in renderObjectIDs)
			{
				if (MyRenderProxy.VisibleObjectsRead.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public static void Draw()
		{
			m_entitiesForDraw.ApplyChanges();
			foreach (MyEntity item in m_entitiesForDraw)
			{
				if (IsAnyRenderObjectVisible(item))
				{
					item.PrepareForDraw();
					_ = item.GetType().Name;
					item.Render.Draw();
				}
			}
			m_entitiesForBBoxDraw.ApplyChanges();
			foreach (KeyValuePair<MyEntity, BoundingBoxDrawArgs> item2 in m_entitiesForBBoxDraw)
			{
				MatrixD worldMatrix = item2.Key.WorldMatrix;
				BoundingBoxD localbox = item2.Key.PositionComp.LocalAABB;
				BoundingBoxDrawArgs value = item2.Value;
				localbox.Min -= value.InflateAmount;
				localbox.Max += value.InflateAmount;
				MatrixD worldToLocal = MatrixD.Invert(worldMatrix);
				MySimpleObjectDraw.DrawAttachedTransparentBox(ref worldMatrix, ref localbox, ref value.Color, item2.Key.Render.GetRenderObjectID(), ref worldToLocal, MySimpleObjectRasterizer.Wireframe, Vector3I.One, value.LineWidth, null, value.lineMaterial, onlyFrontFaces: false, MyBillboard.BlendTypeEnum.LDR);
			}
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return GetIntersectionWithSphere(ref sphere, null, null, ignoreVoxelMaps: false, volumetricTest: false);
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1)
		{
			return GetIntersectionWithSphere(ref sphere, ignoreEntity0, ignoreEntity1, ignoreVoxelMaps: false, volumetricTest: true);
		}

		public static void GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, ref List<MyEntity> result)
		{
			BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid().Include(sphere);
			List<MyEntity> entitiesInAABB = GetEntitiesInAABB(ref boundingBox);
			foreach (MyEntity item in entitiesInAABB)
			{
				if ((!ignoreVoxelMaps || !(item is MyVoxelMap)) && item != ignoreEntity0 && item != ignoreEntity1)
				{
					if (item.GetIntersectionWithSphere(ref sphere))
					{
						result.Add(item);
					}
					if (volumetricTest && item is MyVoxelMap && (item as MyVoxelMap).DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
					{
						result.Add(item);
					}
				}
			}
			entitiesInAABB.Clear();
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, bool excludeEntitiesWithDisabledPhysics = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true)
		{
			BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid().Include(sphere);
			MyEntity result = null;
			List<MyEntity> entitiesInAABB = GetEntitiesInAABB(ref boundingBox);
			foreach (MyEntity item in entitiesInAABB)
			{
				if ((!ignoreVoxelMaps || !(item is MyVoxelMap)) && item != ignoreEntity0 && item != ignoreEntity1 && (!excludeEntitiesWithDisabledPhysics || item.Physics == null || item.Physics.Enabled) && (!ignoreFloatingObjects || (!(item is MyFloatingObject) && !(item is MyDebrisBase))) && (!ignoreHandWeapons || (!(item is IMyHandheldGunObject<MyDeviceBase>) && !(item.Parent is IMyHandheldGunObject<MyDeviceBase>))))
				{
					if (volumetricTest && item.IsVolumetric && item.DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
					{
						result = item;
						break;
					}
					if (item.GetIntersectionWithSphere(ref sphere))
					{
						result = item;
						break;
					}
				}
			}
			entitiesInAABB.Clear();
			return result;
		}

		public static void OverlapAllLineSegment(ref LineD line, List<MyLineSegmentOverlapResult<MyEntity>> resultList)
		{
			MyGamePruningStructure.GetAllEntitiesInRay(ref line, resultList);
		}

		public static MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(ref LineD line, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreChildren = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES, float timeFrame = 0f, bool ignoreObjectsWithoutPhysics = true)
		{
			EntityResultSet.Clear();
			if (ignoreChildren)
			{
				if (ignoreEntity0 != null)
				{
					ignoreEntity0 = ignoreEntity0.GetBaseEntity();
					ignoreEntity0.Hierarchy.GetChildrenRecursive(EntityResultSet);
				}
				if (ignoreEntity1 != null)
				{
					ignoreEntity1 = ignoreEntity1.GetBaseEntity();
					ignoreEntity1.Hierarchy.GetChildrenRecursive(EntityResultSet);
				}
			}
			LineOverlapEntityList.Clear();
			MyGamePruningStructure.GetAllEntitiesInRay(ref line, LineOverlapEntityList);
			LineOverlapEntityList.Sort(MyLineSegmentOverlapResult<MyEntity>.DistanceComparer);
			MyIntersectionResultLineTriangleEx? a = null;
			RayD ray = new RayD(line.From, line.Direction);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlapEntity in LineOverlapEntityList)
			{
				if (a.HasValue)
				{
					double? num = lineOverlapEntity.Element.PositionComp.WorldAABB.Intersects(ray);
					if (num.HasValue)
					{
						double num2 = Vector3D.DistanceSquared(line.From, a.Value.IntersectionPointInWorldSpace);
						double num3 = num.Value * num.Value;
						if (num2 < num3)
						{
							break;
						}
					}
				}
				MyEntity element = lineOverlapEntity.Element;
				if (element != ignoreEntity0 && element != ignoreEntity1 && (!ignoreChildren || !EntityResultSet.Contains(element)) && (!ignoreObjectsWithoutPhysics || (element.Physics != null && element.Physics.Enabled)) && !element.MarkedForClose && (!ignoreFloatingObjects || (!(element is MyFloatingObject) && !(element is MyDebrisBase))) && (!ignoreHandWeapons || (!(element is IMyHandheldGunObject<MyDeviceBase>) && !(element.Parent is IMyHandheldGunObject<MyDeviceBase>))))
				{
					MyIntersectionResultLineTriangleEx? t = null;
					if (timeFrame == 0f || element.Physics == null || element.Physics.LinearVelocity.LengthSquared() < 0.1f || !element.IsCCDForProjectiles)
					{
						element.GetIntersectionWithLine(ref line, out t, flags);
					}
					else
					{
						float num4 = element.Physics.LinearVelocity.Length() * timeFrame;
						float radius = element.PositionComp.LocalVolume.Radius;
						float num5 = 0f;
						Vector3D position = element.PositionComp.GetPosition();
						Vector3 value = Vector3.Normalize(element.Physics.LinearVelocity);
						while (!t.HasValue && num5 < num4)
						{
							element.PositionComp.SetPosition(position + (Vector3D)(num5 * value));
							element.GetIntersectionWithLine(ref line, out t, flags);
							num5 += radius;
						}
						element.PositionComp.SetPosition(position);
					}
					if (t.HasValue && t.Value.Entity != ignoreEntity0 && t.Value.Entity != ignoreEntity1 && (!ignoreChildren || !EntityResultSet.Contains(t.Value.Entity)))
					{
						a = MyIntersectionResultLineTriangleEx.GetCloserIntersection(ref a, ref t);
					}
				}
			}
			LineOverlapEntityList.Clear();
			return a;
		}

		public static MyConcurrentHashSet<MyEntity> GetEntities()
		{
			return m_entities;
		}

		public static MyEntity GetEntityById(long entityId, bool allowClosed = false)
		{
			return MyEntityIdentifier.GetEntityById(entityId, allowClosed) as MyEntity;
		}

		public static bool IsEntityIdValid(long entityId)
		{
			MyEntity myEntity = MyEntityIdentifier.GetEntityById(entityId, allowClosed: true) as MyEntity;
			if (myEntity != null)
			{
				return !myEntity.GetTopMostParent().MarkedForClose;
			}
			return false;
		}

		public static MyEntity GetEntityByIdOrDefault(long entityId, MyEntity defaultValue = null, bool allowClosed = false)
		{
			MyEntityIdentifier.TryGetEntity(entityId, out IMyEntity entity, allowClosed);
			return (entity as MyEntity) ?? defaultValue;
		}

		public static T GetEntityByIdOrDefault<T>(long entityId, T defaultValue = null, bool allowClosed = false) where T : MyEntity
		{
			MyEntityIdentifier.TryGetEntity(entityId, out IMyEntity entity, allowClosed);
			return (entity as T) ?? defaultValue;
		}

		public static bool EntityExists(long entityId)
		{
			return MyEntityIdentifier.ExistsById(entityId);
		}

		public static bool TryGetEntityById(long entityId, out MyEntity entity, bool allowClosed = false)
		{
			return MyEntityIdentifier.TryGetEntity(entityId, out entity, allowClosed);
		}

		public static bool TryGetEntityById<T>(long entityId, out T entity, bool allowClosed = false) where T : MyEntity
		{
			MyEntity entity2;
			bool result = MyEntityIdentifier.TryGetEntity(entityId, out entity2, allowClosed) && entity2 is T;
			entity = (entity2 as T);
			return result;
		}

		public static MyEntity GetEntityByName(string name)
		{
			return m_entityNameDictionary[name];
		}

		public static bool TryGetEntityByName(string name, out MyEntity entity)
		{
			return m_entityNameDictionary.TryGetValue(name, out entity);
		}

		public static bool EntityExists(string name)
		{
			return m_entityNameDictionary.ContainsKey(name);
		}

		public static void RaiseEntityRemove(MyEntity entity)
		{
			if (MyEntities.OnEntityRemove != null)
			{
				MyEntities.OnEntityRemove(entity);
			}
		}

		public static void RaiseEntityAdd(MyEntity entity)
		{
			if (MyEntities.OnEntityAdd != null)
			{
				MyEntities.OnEntityAdd(entity);
			}
		}

		public static void SetTypeHidden(Type type, bool hidden)
		{
			if (hidden != m_hiddenTypes.Contains(type))
			{
				if (hidden)
				{
					m_hiddenTypes.Add(type);
				}
				else
				{
					m_hiddenTypes.Remove(type);
				}
			}
		}

		public static bool IsTypeHidden(Type type)
		{
			foreach (Type hiddenType in m_hiddenTypes)
			{
				if (hiddenType.IsAssignableFrom(type))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsVisible(IMyEntity entity)
		{
			return !IsTypeHidden(entity.GetType());
		}

		public static void UnhideAllTypes()
		{
			foreach (Type item in m_hiddenTypes.ToList())
			{
				SetTypeHidden(item, hidden: false);
			}
		}

		public static void DebugDrawGridStatistics()
		{
			m_cubeGridList.Clear();
			m_cubeGridHash.Clear();
			int num = 0;
			int num2 = 0;
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed grid statistics", Color.Yellow, 1f);
			foreach (MyEntity entity in GetEntities())
			{
				if (entity is MyCubeGrid)
				{
					m_cubeGridList.Add(entity as MyCubeGrid);
					m_cubeGridHash.Add(MyGridPhysicalHierarchy.Static.GetRoot(entity as MyCubeGrid));
					if ((entity as MyCubeGrid).NeedsPerFrameUpdate)
					{
						num++;
					}
					if ((entity as MyCubeGrid).NeedsPerFrameDraw)
					{
						num2++;
					}
				}
			}
			m_cubeGridList = m_cubeGridList.OrderByDescending((MyCubeGrid x) => x.BlocksCount).ToList();
			float scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids by blocks (" + m_cubeGridList.Count + "):", Color.Yellow, scale);
			screenCoord.Y += 30f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing update: " + num, Color.Yellow, scale);
			screenCoord.Y += 30f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing draw: " + num2, Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (MyCubeGrid cubeGrid in m_cubeGridList)
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid.DisplayName + ": " + cubeGrid.BlocksCount + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			screenCoord.Y = 0f;
			screenCoord.X += 800f;
			screenCoord.Y += 50f;
			m_cubeGridList = m_cubeGridHash.OrderByDescending((MyCubeGrid x) => (MyGridPhysicalHierarchy.Static.GetNode(x) != null) ? MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count : 0).ToList();
			m_cubeGridList.RemoveAll((MyCubeGrid x) => MyGridPhysicalHierarchy.Static.GetNode(x) == null || MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count == 0);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Root grids (" + m_cubeGridList.Count + "):", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (MyCubeGrid cubeGrid2 in m_cubeGridList)
			{
				int num3 = (MyGridPhysicalHierarchy.Static.GetNode(cubeGrid2) != null) ? MyGridPhysicalHierarchy.Static.GetNode(cubeGrid2).Children.Count : 0;
				MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid2.DisplayName + ": " + num3 + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
		}

		public static void DebugDrawStatistics()
		{
			m_typesStats.Clear();
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
			foreach (MyEntity item in m_entitiesForUpdate.List)
			{
				string key = item.GetType().Name.ToString();
				if (!m_typesStats.ContainsKey(key))
				{
					m_typesStats.Add(key, 0);
				}
				m_typesStats[key]++;
			}
			float scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update:", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<string, int> item2 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item2.Key + ": " + item2.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
			foreach (MyEntity item3 in m_entitiesForUpdate10.List)
			{
				string key2 = item3.GetType().Name.ToString();
				if (!m_typesStats.ContainsKey(key2))
				{
					m_typesStats.Add(key2, 0);
				}
				m_typesStats[key2]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update10:", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<string, int> item4 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item4.Key + ": " + item4.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
			foreach (MyEntity item5 in m_entitiesForUpdate100.List)
			{
				string key3 = item5.GetType().Name.ToString();
				if (!m_typesStats.ContainsKey(key3))
				{
					m_typesStats.Add(key3, 0);
				}
				m_typesStats[key3]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update100:", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<string, int> item6 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item6.Key + ": " + item6.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
			foreach (MyEntity entity in m_entities)
			{
				string key4 = entity.GetType().Name.ToString();
				if (!m_typesStats.ContainsKey(key4))
				{
					m_typesStats.Add(key4, 0);
				}
				m_typesStats[key4]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "All entities:", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<string, int> item7 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item7.Key + ": " + item7.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
		}

		public static IMyEntity GetEntityFromRenderObjectID(uint renderObjectID)
		{
			using (m_renderObjectToEntityMapLock.AcquireSharedUsing())
			{
				IMyEntity value = null;
				m_renderObjectToEntityMap.TryGetValue(renderObjectID, out value);
				return value;
			}
		}

		private static void DebugDrawGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups) where TNode : MyCubeGrid where TGroupData : IGroupData<TNode>, new()
		{
			int num = 0;
			foreach (MyGroups<TNode, TGroupData>.Group group in groups.Groups)
			{
				Color color = new Vector3((float)(num++ % 15) / 15f, 1f, 1f).HSVtoColor();
				foreach (MyGroups<TNode, TGroupData>.Node node2 in group.Nodes)
				{
					try
					{
						foreach (MyGroups<TNode, TGroupData>.Node child in node2.Children)
						{
							m_groupDebugHelper.Add(child);
						}
						foreach (object item in m_groupDebugHelper)
						{
							MyGroups<TNode, TGroupData>.Node node = null;
							int num2 = 0;
							foreach (MyGroups<TNode, TGroupData>.Node child2 in node2.Children)
							{
								if (item == child2)
								{
									node = child2;
									num2++;
								}
							}
							MyRenderProxy.DebugDrawLine3D(node2.NodeData.PositionComp.WorldAABB.Center, node.NodeData.PositionComp.WorldAABB.Center, color, color, depthRead: false);
							MyRenderProxy.DebugDrawText3D((node2.NodeData.PositionComp.WorldAABB.Center + node.NodeData.PositionComp.WorldAABB.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
						}
						Color color2 = new Color(color.ToVector3() + 0.25f);
						MyRenderProxy.DebugDrawSphere(node2.NodeData.PositionComp.WorldAABB.Center, 0.2f, color2.ToVector3(), 0.5f, depthRead: false, smooth: true);
						MyRenderProxy.DebugDrawText3D(node2.NodeData.PositionComp.WorldAABB.Center, node2.LinkCount.ToString(), color2, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					finally
					{
						m_groupDebugHelper.Clear();
					}
				}
			}
		}

		public static void DebugDraw()
		{
			MyEntityComponentsDebugDraw.DebugDraw();
			if (MyCubeGridGroups.Static != null)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_PHYSICAL)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.Physical);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_LOGICAL)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.Logical);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_SMALL_TO_LARGE_BLOCK_GROUPS)
				{
					MyCubeGridGroups.DebugDrawBlockGroups(MyCubeGridGroups.Static.SmallToLargeBlockConnections);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_DYNAMIC_PHYSICAL_GROUPS)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.PhysicalDynamic);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS || MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyFakes.SHOW_INVALID_TRIANGLES)
			{
				using (m_renderObjectToEntityMapLock.AcquireSharedUsing())
				{
					m_entitiesForDebugDraw.Clear();
					foreach (uint item in MyRenderProxy.VisibleObjectsRead)
					{
						m_renderObjectToEntityMap.TryGetValue(item, out IMyEntity value);
						if (value != null)
						{
							IMyEntity topMostParent = value.GetTopMostParent();
							if (!m_entitiesForDebugDraw.Contains(topMostParent))
							{
								m_entitiesForDebugDraw.Add(topMostParent);
							}
						}
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_GRID_COUNTER)
					{
						MyRenderProxy.DebugDrawText2D(new Vector2(700f, 0f), "Grid number: " + MyCubeGrid.GridCounter, Color.Red, 1f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
					}
					foreach (MyEntity entity in m_entities)
					{
						m_entitiesForDebugDraw.Add(entity);
					}
					foreach (IMyEntity item2 in m_entitiesForDebugDraw)
					{
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
						{
							item2.DebugDraw();
						}
						if (MyFakes.SHOW_INVALID_TRIANGLES)
						{
							item2.DebugDrawInvalidTriangles();
						}
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_RIGID_BODY_ACTIONS)
					{
						foreach (IMyEntity item3 in m_entitiesForDebugDraw)
						{
							if (item3.Physics != null && Vector3D.Distance(MySector.MainCamera.Position, item3.WorldAABB.Center) < 500.0)
							{
								MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(item3.LocalAABB, item3.WorldMatrix);
								if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES)
								{
									Color color = Color.Yellow;
									if (item3.Physics.IsStatic)
									{
										color = Color.RoyalBlue;
									}
									else if (!item3.Physics.IsActive)
									{
										color = Color.Red;
									}
									MyRenderProxy.DebugDrawOBB(obb, color, 1f, depthRead: false, smooth: false);
									MyRenderProxy.DebugDrawLine3D(item3.WorldAABB.Center, item3.WorldAABB.Center + item3.Physics.LinearVelocity * 100f, Color.Green, Color.White, depthRead: false);
								}
								if (MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES)
								{
									HkRigidBody rigidBody = item3.Physics.RigidBody;
									if (rigidBody != null && rigidBody.GetCustomVelocity(out Vector3 velocity))
									{
										MyRenderProxy.DebugDrawOBB(obb, Color.RoyalBlue, 1f, depthRead: false, smooth: false);
										MyRenderProxy.DebugDrawLine3D(item3.WorldAABB.Center, item3.WorldAABB.Center + velocity * 100f, Color.Green, Color.White, depthRead: false);
									}
								}
							}
						}
					}
					m_entitiesForDebugDraw.Clear();
					if (MyDebugDrawSettings.DEBUG_DRAW_GAME_PRUNNING)
					{
						MyGamePruningStructure.DebugDraw();
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS)
					{
						MyRadioBroadcasters.DebugDraw();
					}
				}
				m_entitiesForDebugDraw.Clear();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS != MyPhysics.DebugDrawClustersEnable && MySector.MainCamera != null)
				{
					MyPhysics.DebugDrawClustersMatrix = MySector.MainCamera.WorldMatrix;
				}
				MyPhysics.DebugDrawClusters();
			}
			MyPhysics.DebugDrawClustersEnable = MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
			if (MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS)
			{
				DebugDrawStatistics();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS)
			{
				DebugDrawGridStatistics();
			}
		}

		public static MyEntity CreateFromObjectBuilderAndAdd(MyObjectBuilder_EntityBase objectBuilder, bool fadeIn)
		{
			bool insertIntoScene = (objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
			if (MyFakes.ENABLE_LARGE_OFFSET && objectBuilder.PositionAndOrientation.Value.Position.X < 10000.0)
			{
				objectBuilder.PositionAndOrientation = new MyPositionAndOrientation
				{
					Forward = objectBuilder.PositionAndOrientation.Value.Forward,
					Up = objectBuilder.PositionAndOrientation.Value.Up,
					Position = new SerializableVector3D(objectBuilder.PositionAndOrientation.Value.Position + new Vector3D(1000000000.0))
				};
			}
			MyEntity myEntity = CreateFromObjectBuilder(objectBuilder, fadeIn);
			if (myEntity != null)
			{
				if (myEntity.EntityId == 0L)
				{
					myEntity = null;
				}
				else
				{
					Add(myEntity, insertIntoScene);
				}
			}
			return myEntity;
		}

		public static void CreateAsync(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> doneHandler = null)
		{
			if (m_creationThread != null)
			{
				m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler, null, 0);
			}
		}

		public static void InitAsync(MyEntity entity, MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> doneHandler = null, byte islandIndex = 0, double serializationTimestamp = 0.0, bool fadeIn = false)
		{
			if (m_creationThread != null)
			{
				m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler, entity, islandIndex, serializationTimestamp, fadeIn);
			}
		}

		public static void ReleaseWaitingAsync(byte index, Dictionary<long, MatrixD> matrices)
		{
			m_creationThread.ReleaseWaiting(index, matrices);
		}

		public static void CallAsync(MyEntity entity, Action<MyEntity> doneHandler)
		{
			InitAsync(entity, null, addToScene: false, doneHandler, 0);
		}

		public static void CallAsync(Action doneHandler)
		{
			InitAsync(null, null, addToScene: false, delegate
			{
				doneHandler();
			}, 0);
		}

		public static void MemoryLimitAddFailureReset()
		{
			MemoryLimitAddFailure = false;
		}

		public static void RemapObjectBuilderCollection(IEnumerable<MyObjectBuilder_EntityBase> objectBuilders)
		{
			if (m_remapHelper == null)
			{
				m_remapHelper = new MyEntityIdRemapHelper();
			}
			foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
			{
				objectBuilder.Remap(m_remapHelper);
			}
			m_remapHelper.Clear();
		}

		public static void RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder)
		{
			if (m_remapHelper == null)
			{
				m_remapHelper = new MyEntityIdRemapHelper();
			}
			objectBuilder.Remap(m_remapHelper);
			m_remapHelper.Clear();
		}

		public static MyEntity CreateFromObjectBuilderNoinit(MyObjectBuilder_EntityBase objectBuilder)
		{
			return MyEntityFactory.CreateEntity(objectBuilder);
		}

		public static MyEntity CreateFromObjectBuilderParallel(MyObjectBuilder_EntityBase objectBuilder, bool addToScene = false, Action<MyEntity> completionCallback = null, MyEntity entity = null, MyEntity relativeSpawner = null, Vector3D? relativeOffset = null, bool checkPosition = false, bool fadeIn = false)
		{
			if (entity == null)
			{
				entity = CreateFromObjectBuilderNoinit(objectBuilder);
				if (entity == null)
				{
					return null;
				}
			}
			InitEntityData initData = new InitEntityData(objectBuilder, addToScene, completionCallback, entity, fadeIn, relativeSpawner, relativeOffset, checkPosition);
			Parallel.Start(delegate
			{
				if (CallInitEntity(initData))
				{
					MySandboxGame.Static.Invoke(delegate
					{
						OnEntityInitialized(initData);
					}, "CreateFromObjectBuilderParallel(alreadyParallel: true)");
				}
			});
			return entity;
		}

		private static bool CallInitEntity(WorkData workData)
		{
			InitEntityData initEntityData = workData as InitEntityData;
			if (initEntityData == null)
			{
				workData.FlagAsFailed();
				return false;
			}
			return initEntityData.CallInitEntity();
		}

		private static void OnEntityInitialized(WorkData workData)
		{
			InitEntityData initEntityData = workData as InitEntityData;
			if (initEntityData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				initEntityData.OnEntityInitialized();
			}
		}

		public static MyEntity CreateFromObjectBuilder(MyObjectBuilder_EntityBase objectBuilder, bool fadeIn)
		{
			MyEntity entity = CreateFromObjectBuilderNoinit(objectBuilder);
			entity.Render.FadeIn = fadeIn;
			InitEntity(objectBuilder, ref entity);
			return entity;
		}

		public static void InitEntity(MyObjectBuilder_EntityBase objectBuilder, ref MyEntity entity)
		{
			if (entity != null)
			{
				try
				{
					entity.Init(objectBuilder);
				}
				catch (Exception arg)
				{
					MySandboxGame.Log.WriteLine("ERROR Entity init!: " + arg);
					entity.EntityId = 0L;
					entity = null;
				}
			}
		}

		public static bool Load(List<MyObjectBuilder_EntityBase> objectBuilders)
		{
			MyEntityIdentifier.AllocationSuspended = true;
			bool allEntitiesAdded = true;
			InitEntityData[] results = null;
			try
			{
				if (objectBuilders != null)
				{
					results = new InitEntityData[objectBuilders.Count];
					if (MySandboxGame.Config.SyncRendering)
					{
						MyEntityIdentifier.PrepareSwapData();
						MyEntityIdentifier.SwapPerThreadData();
					}
					Parallel.For(0, objectBuilders.Count, delegate(int i)
					{
						allEntitiesAdded &= LoadEntity(i, results, objectBuilders);
					});
					if (MySandboxGame.Config.SyncRendering)
					{
						MyEntityIdentifier.ClearSwapDataAndRestore();
					}
				}
			}
			finally
			{
				MyEntityIdentifier.AllocationSuspended = false;
			}
			if (results != null)
			{
				MyEntityIdentifier.InEntityCreationBlock = true;
				InitEntityData[] array = results;
				for (int j = 0; j < array.Length; j++)
				{
					array[j]?.OnEntityInitialized();
				}
				MyEntityIdentifier.InEntityCreationBlock = false;
			}
			return allEntitiesAdded;
		}

		private static bool LoadEntity(int i, InitEntityData[] results, List<MyObjectBuilder_EntityBase> objectBuilders)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = objectBuilders[i];
			MyObjectBuilder_Character myObjectBuilder_Character = myObjectBuilder_EntityBase as MyObjectBuilder_Character;
			if (myObjectBuilder_Character != null && MyMultiplayer.Static != null && Sync.IsServer && !myObjectBuilder_Character.IsPersistenceCharacter)
			{
				return true;
			}
			if (MyFakes.SKIP_VOXELS_DURING_LOAD && myObjectBuilder_EntityBase.TypeId == typeof(MyObjectBuilder_VoxelMap) && (myObjectBuilder_EntityBase as MyObjectBuilder_VoxelMap).StorageName != "BaseAsteroid")
			{
				return true;
			}
			bool result = true;
			MyEntity myEntity = CreateFromObjectBuilderNoinit(myObjectBuilder_EntityBase);
			if (myEntity != null)
			{
				InitEntityData initEntityData = new InitEntityData(myObjectBuilder_EntityBase, addToScene: true, null, myEntity, fadeIn: false);
				if (initEntityData.CallInitEntity())
				{
					results[i] = initEntityData;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal static List<MyObjectBuilder_EntityBase> Save()
		{
			List<MyObjectBuilder_EntityBase> list = new List<MyObjectBuilder_EntityBase>();
			foreach (MyEntity entity in m_entities)
			{
				if (entity.Save && !m_entitiesToDelete.Contains(entity) && !entity.MarkedForClose)
				{
					entity.BeforeSave();
					MyObjectBuilder_EntityBase objectBuilder = entity.GetObjectBuilder();
					list.Add(objectBuilder);
				}
			}
			return list;
		}

		public static void EnableEntityBoundingBoxDraw(MyEntity entity, bool enable, Vector4? color = null, float lineWidth = 0.01f, Vector3? inflateAmount = null, MyStringId? lineMaterial = null)
		{
			if (enable)
			{
				if (!m_entitiesForBBoxDraw.ContainsKey(entity))
				{
					entity.OnClose += entityForBBoxDraw_OnClose;
				}
				m_entitiesForBBoxDraw[entity] = new BoundingBoxDrawArgs
				{
					Color = (color ?? Vector4.One),
					LineWidth = lineWidth,
					InflateAmount = (inflateAmount ?? Vector3.Zero),
					lineMaterial = (lineMaterial ?? GIZMO_LINE_MATERIAL)
				};
			}
			else
			{
				m_entitiesForBBoxDraw.Remove(entity);
				entity.OnClose -= entityForBBoxDraw_OnClose;
			}
		}

		private static void entityForBBoxDraw_OnClose(MyEntity entity)
		{
			m_entitiesForBBoxDraw.Remove(entity);
		}

		public static MyEntity CreateFromComponentContainerDefinitionAndAdd(MyDefinitionId entityContainerDefinitionId, bool fadeIn, bool insertIntoScene = true)
		{
			if (!typeof(MyObjectBuilder_EntityBase).IsAssignableFrom(entityContainerDefinitionId.TypeId))
			{
				return null;
			}
			if (!MyComponentContainerExtension.TryGetContainerDefinition(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeId, out MyContainerDefinition _))
			{
				MySandboxGame.Log.WriteLine("Entity container definition not found: " + entityContainerDefinitionId);
				return null;
			}
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeName) as MyObjectBuilder_EntityBase;
			if (myObjectBuilder_EntityBase == null)
			{
				MySandboxGame.Log.WriteLine("Entity builder was not created: " + entityContainerDefinitionId);
				return null;
			}
			if (insertIntoScene)
			{
				myObjectBuilder_EntityBase.PersistentFlags |= MyPersistentEntityFlags2.InScene;
			}
			return CreateFromObjectBuilderAndAdd(myObjectBuilder_EntityBase, fadeIn);
		}

		public static void RaiseEntityCreated(MyEntity entity)
		{
			MyEntities.OnEntityCreate?.Invoke(entity);
		}

		public static MyEntity CreateEntityAndAdd(MyDefinitionId entityContainerId, bool fadeIn, bool setPosAndRot = false, Vector3? position = null, Vector3? up = null, Vector3? forward = null)
		{
			if (MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out MyContainerDefinition _))
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerId) as MyObjectBuilder_EntityBase;
				if (myObjectBuilder_EntityBase != null)
				{
					if (setPosAndRot)
					{
						myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation(position.HasValue ? position.Value : Vector3.Zero, forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up);
					}
					return CreateFromObjectBuilderAndAdd(myObjectBuilder_EntityBase, fadeIn);
				}
				return null;
			}
			return null;
		}

		public static MyEntity CreateEntity(MyDefinitionId entityContainerId, bool fadeIn, bool setPosAndRot = false, Vector3? position = null, Vector3? up = null, Vector3? forward = null)
		{
			if (MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out MyContainerDefinition _))
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerId) as MyObjectBuilder_EntityBase;
				if (myObjectBuilder_EntityBase != null)
				{
					if (setPosAndRot)
					{
						myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation(position.HasValue ? position.Value : Vector3.Zero, forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up);
					}
					return CreateFromObjectBuilder(myObjectBuilder_EntityBase, fadeIn);
				}
				return null;
			}
			return null;
		}

		public static void SendCloseRequest(IMyEntity entity)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnEntityCloseRequest, entity.EntityId);
		}

		[Event(null, 2779)]
		[Reliable]
		[Server]
		private static void OnEntityCloseRequest(long entityId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value) && !MySession.Static.CreativeMode && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			TryGetEntityById(entityId, out MyEntity entity);
			if (entity != null)
			{
				MyLog.Default.Info($"OnEntityCloseRequest removed entity '{entity.Name}:{entity.DisplayName}' with entity id '{entity.EntityId}'");
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (MyMultiplayer.Static != null && myVoxelBase != null && !myVoxelBase.Save && !myVoxelBase.ContentChanged && !myVoxelBase.BeforeContentChanged)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ForceCloseEntityOnClients, entityId);
				}
				if (!entity.MarkedForClose)
				{
					entity.Close();
				}
			}
		}

		[Event(null, 2804)]
		[Reliable]
		[Broadcast]
		public static void ForceCloseEntityOnClients(long entityId)
		{
			TryGetEntityById(entityId, out MyEntity entity);
			if (entity != null && !entity.MarkedForClose)
			{
				entity.Close();
			}
		}
	}
}
