using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Threading;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MySessionComponentDescriptor(MyUpdateOrder.Simulation)]
	public class MyGamePruningStructure : MySessionComponentBase
	{
		private static MyDynamicAABBTreeD m_dynamicObjectsTree;

		private static MyDynamicAABBTreeD m_staticObjectsTree;

		private static MyDynamicAABBTreeD m_voxelMapsTree;

		[ThreadStatic]
		private static List<MyVoxelBase> m_cachedVoxelList;

		private static readonly SpinLockRef m_movedLock;

		private static HashSet<MyEntity> m_moved;

		private static HashSet<MyEntity> m_movedUpdate;

		static MyGamePruningStructure()
		{
			m_movedLock = new SpinLockRef();
			m_moved = new HashSet<MyEntity>();
			m_movedUpdate = new HashSet<MyEntity>();
			Init();
		}

		private static void Init()
		{
			m_dynamicObjectsTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);
			m_voxelMapsTree = new MyDynamicAABBTreeD(MyConstants.GAME_PRUNING_STRUCTURE_AABB_EXTENSION);
			m_staticObjectsTree = new MyDynamicAABBTreeD(Vector3D.Zero);
		}

		private static BoundingBoxD GetEntityAABB(MyEntity entity)
		{
			BoundingBoxD result = entity.PositionComp.WorldAABB;
			if (entity.Physics != null)
			{
				result = result.Include(entity.WorldMatrix.Translation + entity.Physics.LinearVelocity * 0.0166666675f * 5f);
			}
			return result;
		}

		private static bool IsEntityStatic(MyEntity entity)
		{
			if (entity.Physics != null)
			{
				return entity.Physics.IsStatic;
			}
			return true;
		}

		public static void Add(MyEntity entity)
		{
			if ((entity.Parent != null && (entity.Flags & EntityFlags.IsGamePrunningStructureObject) == 0) || entity.TopMostPruningProxyId != -1)
			{
				return;
			}
			BoundingBoxD aabb = GetEntityAABB(entity);
			if (!(aabb.Size == Vector3D.Zero))
			{
				if (IsEntityStatic(entity))
				{
					entity.TopMostPruningProxyId = m_staticObjectsTree.AddProxy(ref aabb, entity, 0u);
					entity.StaticForPruningStructure = true;
				}
				else
				{
					entity.TopMostPruningProxyId = m_dynamicObjectsTree.AddProxy(ref aabb, entity, 0u);
					entity.StaticForPruningStructure = false;
				}
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (myVoxelBase != null)
				{
					myVoxelBase.VoxelMapPruningProxyId = m_voxelMapsTree.AddProxy(ref aabb, entity, 0u);
				}
			}
		}

		public static void Remove(MyEntity entity)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && myVoxelBase.VoxelMapPruningProxyId != -1)
			{
				m_voxelMapsTree.RemoveProxy(myVoxelBase.VoxelMapPruningProxyId);
				myVoxelBase.VoxelMapPruningProxyId = -1;
			}
			if (entity.TopMostPruningProxyId != -1)
			{
				if (entity.StaticForPruningStructure)
				{
					m_staticObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
				}
				else
				{
					m_dynamicObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
				}
				entity.TopMostPruningProxyId = -1;
			}
		}

		public static void Clear()
		{
			m_voxelMapsTree.Clear();
			m_dynamicObjectsTree.Clear();
			m_staticObjectsTree.Clear();
			using (m_movedLock.Acquire())
			{
				m_moved.Clear();
			}
		}

		public static void Move(MyEntity entity)
		{
			using (m_movedLock.Acquire())
			{
				m_moved.Add(entity);
			}
		}

		private static void MoveInternal(MyEntity entity)
		{
			if (entity.TopMostPruningProxyId == -1)
			{
				return;
			}
			BoundingBoxD aabb = GetEntityAABB(entity);
			if (aabb.Size == Vector3D.Zero)
			{
				Remove(entity);
				return;
			}
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null)
			{
				m_voxelMapsTree.MoveProxy(myVoxelBase.VoxelMapPruningProxyId, ref aabb, Vector3D.Zero);
			}
			if (entity.TopMostPruningProxyId == -1)
			{
				return;
			}
			bool flag = IsEntityStatic(entity);
			if (flag != entity.StaticForPruningStructure)
			{
				if (entity.StaticForPruningStructure)
				{
					m_staticObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
					entity.TopMostPruningProxyId = m_dynamicObjectsTree.AddProxy(ref aabb, entity, 0u);
				}
				else
				{
					m_dynamicObjectsTree.RemoveProxy(entity.TopMostPruningProxyId);
					entity.TopMostPruningProxyId = m_staticObjectsTree.AddProxy(ref aabb, entity, 0u);
				}
				entity.StaticForPruningStructure = flag;
			}
			else if (entity.StaticForPruningStructure)
			{
				m_staticObjectsTree.MoveProxy(entity.TopMostPruningProxyId, ref aabb, Vector3D.Zero);
			}
			else
			{
				m_dynamicObjectsTree.MoveProxy(entity.TopMostPruningProxyId, ref aabb, Vector3D.Zero);
			}
		}

		private static void Update()
		{
			using (m_movedLock.Acquire())
			{
				MyUtils.Swap(ref m_moved, ref m_movedUpdate);
			}
			foreach (MyEntity item in m_movedUpdate)
			{
				MoveInternal(item);
			}
			m_movedUpdate.Clear();
		}

		public static void GetAllEntitiesInBox(ref BoundingBoxD box, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
			int count = result.Count;
			for (int i = 0; i < count; i++)
			{
				if (result[i].Hierarchy != null)
				{
					result[i].Hierarchy.QueryAABB(ref box, result);
				}
			}
		}

		public static void GetAllEntitiesInOBB(ref MyOrientedBoundingBoxD obb, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingBox(ref obb, result, 0u, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingBox(ref obb, result, 0u, clear: false);
			}
		}

		public static void GetTopMostEntitiesInBox(ref BoundingBoxD box, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
		}

		public static void GetAllTopMostStaticEntitiesInBox(ref BoundingBoxD box, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
		}

		public static void GetAllEntitiesInSphere(ref BoundingSphereD sphere, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
			int count = result.Count;
			for (int i = 0; i < count; i++)
			{
				if (result[i].Hierarchy != null)
				{
					result[i].Hierarchy.QuerySphere(ref sphere, result);
				}
			}
		}

		public static void GetAllTopMostEntitiesInSphere(ref BoundingSphereD sphere, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
		}

		public static void GetAllTargetsInSphere(ref BoundingSphereD sphere, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
			}
			int count = result.Count;
			for (int i = 0; i < count; i++)
			{
				if (result[i].Hierarchy != null)
				{
					result[i].Hierarchy.QuerySphere(ref sphere, result);
				}
			}
		}

		public static void GetAllEntitiesInRay(ref LineD ray, List<MyLineSegmentOverlapResult<MyEntity>> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllLineSegment(ref ray, result);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllLineSegment(ref ray, result, clear: false);
			}
			int count = result.Count;
			for (int i = 0; i < count; i++)
			{
				if (result[i].Element.Hierarchy != null)
				{
					result[i].Element.Hierarchy.QueryLine(ref ray, result);
				}
			}
		}

		public static void GetTopmostEntitiesOverlappingRay(ref LineD ray, List<MyLineSegmentOverlapResult<MyEntity>> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllLineSegment(ref ray, result);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllLineSegment(ref ray, result, clear: false);
			}
		}

		public static void GetVoxelMapsOverlappingRay(ref LineD ray, List<MyLineSegmentOverlapResult<MyVoxelBase>> result)
		{
			m_voxelMapsTree.OverlapAllLineSegment(ref ray, result);
		}

		public static void GetAproximateDynamicClustersForSize(ref BoundingBoxD container, double clusterSize, List<BoundingBoxD> clusters)
		{
			m_dynamicObjectsTree.GetAproximateClustersForAabb(ref container, clusterSize, clusters);
		}

		public static void GetAllVoxelMapsInBox(ref BoundingBoxD box, List<MyVoxelBase> result)
		{
			m_voxelMapsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
		}

		public static bool AnyVoxelMapInBox(ref BoundingBoxD box)
		{
			return m_voxelMapsTree.OverlapsAnyLeafBoundingBox(ref box);
		}

		public static MyPlanet GetClosestPlanet(Vector3D position)
		{
			BoundingBoxD box = new BoundingBoxD(position, position);
			return GetClosestPlanet(ref box);
		}

		public static MyPlanet GetClosestPlanet(ref BoundingBoxD box)
		{
			using (MyUtils.ReuseCollection(ref m_cachedVoxelList))
			{
				m_voxelMapsTree.OverlapAllBoundingBox(ref box, m_cachedVoxelList, 0u, clear: false);
				MyPlanet result = null;
				Vector3D center = box.Center;
				double num = double.PositiveInfinity;
				foreach (MyVoxelBase cachedVoxel in m_cachedVoxelList)
				{
					if (cachedVoxel is MyPlanet)
					{
						double num2 = (center - cachedVoxel.WorldMatrix.Translation).LengthSquared();
						if (num2 < num)
						{
							num = num2;
							result = (MyPlanet)cachedVoxel;
						}
					}
				}
				return result;
			}
		}

		public static void GetAllVoxelMapsInSphere(ref BoundingSphereD sphere, List<MyVoxelBase> result)
		{
			m_voxelMapsTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
		}

		public static void DebugDraw()
		{
			List<BoundingBoxD> list = new List<BoundingBoxD>();
			m_dynamicObjectsTree.GetAllNodeBounds(list);
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.SkyBlue, 0.05f), depthRead: false, shaded: false))
			{
				foreach (BoundingBoxD item in list)
				{
					BoundingBoxD aabb = item;
					myDebugDrawBatchAabb.Add(ref aabb);
				}
			}
			list.Clear();
			m_staticObjectsTree.GetAllNodeBounds(list);
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb2 = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.Aquamarine, 0.05f), depthRead: false, shaded: false))
			{
				foreach (BoundingBoxD item2 in list)
				{
					BoundingBoxD aabb2 = item2;
					myDebugDrawBatchAabb2.Add(ref aabb2);
				}
			}
		}

		public override void Simulate()
		{
			base.Simulate();
			Update();
		}

		public static void GetTopmostEntitiesInBox(ref BoundingBoxD box, List<MyEntity> result, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			if (qtype.HasDynamic())
			{
				m_dynamicObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
			if (qtype.HasStatic())
			{
				m_staticObjectsTree.OverlapAllBoundingBox(ref box, result, 0u, clear: false);
			}
		}
	}
}
