using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using System;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyPathfinding : MyPathFindingSystem<MyNavigationPrimitive>, IMyPathfinding
	{
		private MyVoxelPathfinding m_voxelPathfinding;

		private MyGridPathfinding m_gridPathfinding;

		private MyNavmeshCoordinator m_navmeshCoordinator;

		private MyDynamicObstacles m_obstacles;

		public readonly Func<long> NextTimestampFunction;

		private MyNavigationPrimitive m_reachEndPrimitive;

		private float m_reachPredicateDistance;

		public MyGridPathfinding GridPathfinding => m_gridPathfinding;

		public MyVoxelPathfinding VoxelPathfinding => m_voxelPathfinding;

		public MyNavmeshCoordinator Coordinator => m_navmeshCoordinator;

		public MyDynamicObstacles Obstacles => m_obstacles;

		public long LastHighLevelTimestamp
		{
			get;
			set;
		}

		private long GenerateNextTimestamp()
		{
			CalculateNextTimestamp();
			return GetCurrentTimestamp();
		}

		public MyPathfinding()
			: base(128, (Func<long>)null)
		{
			NextTimestampFunction = GenerateNextTimestamp;
			m_obstacles = new MyDynamicObstacles();
			m_navmeshCoordinator = new MyNavmeshCoordinator(m_obstacles);
			m_gridPathfinding = new MyGridPathfinding(m_navmeshCoordinator);
			m_voxelPathfinding = new MyVoxelPathfinding(m_navmeshCoordinator);
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
		}

		public void Update()
		{
			if (MyPerGameSettings.EnablePathfinding)
			{
				m_obstacles.Update();
				m_gridPathfinding.Update();
				m_voxelPathfinding.Update();
			}
		}

		public IMyPathfindingLog GetPathfindingLog()
		{
			return m_voxelPathfinding.DebugLog;
		}

		public void UnloadData()
		{
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			m_voxelPathfinding.UnloadData();
			m_gridPathfinding = null;
			m_voxelPathfinding = null;
			m_navmeshCoordinator = null;
			m_obstacles.Clear();
			m_obstacles = null;
		}

		private void MyEntities_OnEntityAdd(MyEntity newEntity)
		{
			m_obstacles.TryCreateObstacle(newEntity);
			MyCubeGrid myCubeGrid = newEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				m_gridPathfinding.GridAdded(myCubeGrid);
			}
		}

		public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity entity = null)
		{
			if (!MyPerGameSettings.EnablePathfinding)
			{
				return null;
			}
			MySmartPath mySmartPath = new MySmartPath(this);
			MySmartGoal goal = new MySmartGoal(end, entity);
			mySmartPath.Init(begin, goal);
			return mySmartPath;
		}

		private bool ReachablePredicate(MyNavigationPrimitive primitive)
		{
			return (m_reachEndPrimitive.WorldPosition - primitive.WorldPosition).LengthSquared() <= (double)(m_reachPredicateDistance * m_reachPredicateDistance);
		}

		public bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance)
		{
			m_reachPredicateDistance = thresholdDistance;
			MyNavigationPrimitive myNavigationPrimitive = FindClosestPrimitive(begin, highLevel: false);
			MyNavigationPrimitive myNavigationPrimitive2 = FindClosestPrimitive(end.GetDestination(), highLevel: false);
			if (myNavigationPrimitive == null || myNavigationPrimitive2 == null)
			{
				return false;
			}
			MyHighLevelPrimitive highLevelPrimitive = myNavigationPrimitive.GetHighLevelPrimitive();
			myNavigationPrimitive2.GetHighLevelPrimitive();
			if (new MySmartGoal(end).FindHighLevelPath(this, highLevelPrimitive) == null)
			{
				return false;
			}
			m_reachEndPrimitive = myNavigationPrimitive2;
			PrepareTraversal(myNavigationPrimitive, null, ReachablePredicate);
			try
			{
				using (Enumerator enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Equals(m_reachEndPrimitive))
						{
							return true;
						}
					}
				}
			}
			finally
			{
			}
			return false;
		}

		public MyPath<MyNavigationPrimitive> FindPathLowlevel(Vector3D begin, Vector3D end)
		{
			MyPath<MyNavigationPrimitive> result = null;
			if (!MyPerGameSettings.EnablePathfinding)
			{
				return result;
			}
			MyNavigationPrimitive myNavigationPrimitive = FindClosestPrimitive(begin, highLevel: false);
			MyNavigationPrimitive myNavigationPrimitive2 = FindClosestPrimitive(end, highLevel: false);
			if (myNavigationPrimitive != null && myNavigationPrimitive2 != null)
			{
				result = FindPath(myNavigationPrimitive, myNavigationPrimitive2);
			}
			return result;
		}

		public MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, MyEntity entity = null)
		{
			double closestDistanceSq = double.PositiveInfinity;
			MyNavigationPrimitive result = null;
			MyNavigationPrimitive myNavigationPrimitive = null;
			MyVoxelMap myVoxelMap = entity as MyVoxelMap;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myVoxelMap != null)
			{
				result = VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq, myVoxelMap);
			}
			else if (myCubeGrid != null)
			{
				result = GridPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq, myCubeGrid);
			}
			else
			{
				myNavigationPrimitive = VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
				myNavigationPrimitive = GridPathfinding.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
			}
			return result;
		}

		public void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != 0)
				{
					m_navmeshCoordinator.Links.DebugDraw(Color.Khaki);
				}
				if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
				{
					m_navmeshCoordinator.HighLevelLinks.DebugDraw(Color.LightGreen);
				}
				m_navmeshCoordinator.DebugDraw();
				m_obstacles.DebugDraw();
			}
		}
	}
}
