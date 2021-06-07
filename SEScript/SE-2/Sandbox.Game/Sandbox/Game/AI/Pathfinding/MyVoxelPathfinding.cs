using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyVoxelPathfinding
	{
		public struct CellId : IEquatable<CellId>
		{
			public MyVoxelBase VoxelMap;

			public Vector3I Pos;

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() != typeof(CellId))
				{
					return false;
				}
				return Equals((CellId)obj);
			}

			public override int GetHashCode()
			{
				return VoxelMap.GetHashCode() * 1610612741 + Pos.GetHashCode();
			}

			public bool Equals(CellId other)
			{
				if (VoxelMap == other.VoxelMap)
				{
					return Pos == other.Pos;
				}
				return false;
			}
		}

		private int m_updateCtr;

		private const int UPDATE_PERIOD = 5;

		private Dictionary<MyVoxelBase, MyVoxelNavigationMesh> m_navigationMeshes;

		private List<Vector3D> m_tmpUpdatePositions;

		private List<MyVoxelBase> m_tmpVoxelMaps;

		private List<MyVoxelNavigationMesh> m_tmpNavmeshes;

		private MyNavmeshCoordinator m_coordinator;

		public MyVoxelPathfindingLog DebugLog;

		private static float MESH_DIST = 40f;

		public MyVoxelPathfinding(MyNavmeshCoordinator coordinator)
		{
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
			m_navigationMeshes = new Dictionary<MyVoxelBase, MyVoxelNavigationMesh>();
			m_tmpUpdatePositions = new List<Vector3D>(8);
			m_tmpVoxelMaps = new List<MyVoxelBase>();
			m_tmpNavmeshes = new List<MyVoxelNavigationMesh>();
			m_coordinator = coordinator;
			coordinator.SetVoxelPathfinding(this);
			if (MyFakes.REPLAY_NAVMESH_GENERATION || MyFakes.LOG_NAVMESH_GENERATION)
			{
				DebugLog = new MyVoxelPathfindingLog("PathfindingLog.log");
			}
		}

		private void MyEntities_OnEntityAdd(MyEntity entity)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && (MyPerGameSettings.Game != GameEnum.SE_GAME || myVoxelBase is MyPlanet))
			{
				m_navigationMeshes.Add(myVoxelBase, new MyVoxelNavigationMesh(myVoxelBase, m_coordinator, MyCestmirPathfindingShorts.Pathfinding.NextTimestampFunction));
				RegisterVoxelMapEvents(myVoxelBase);
			}
		}

		private void RegisterVoxelMapEvents(MyVoxelBase voxelMap)
		{
			voxelMap.OnClose += voxelMap_OnClose;
		}

		private void voxelMap_OnClose(MyEntity entity)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null && (MyPerGameSettings.Game != GameEnum.SE_GAME || myVoxelBase is MyPlanet))
			{
				m_navigationMeshes.Remove(myVoxelBase);
			}
		}

		public void UnloadData()
		{
			if (DebugLog != null)
			{
				DebugLog.Close();
				DebugLog = null;
			}
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
		}

		public void Update()
		{
			m_updateCtr++;
			int num = m_updateCtr % 6;
			if ((num == 0 || num == 2 || num == 4) && MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING)
			{
				if (!MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP)
				{
					return;
				}
				MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP = false;
			}
			if (MyFakes.REPLAY_NAVMESH_GENERATION)
			{
				DebugLog.PerformOneOperation(MyFakes.REPLAY_NAVMESH_GENERATION_TRIGGER);
				MyFakes.REPLAY_NAVMESH_GENERATION_TRIGGER = false;
				return;
			}
			switch (num)
			{
			case 1:
			case 3:
				break;
			case 0:
				GetUpdatePositions();
				PerformCellMarking(m_tmpUpdatePositions);
				PerformCellUpdates();
				m_tmpUpdatePositions.Clear();
				break;
			case 2:
				GetUpdatePositions();
				PerformCellMarking(m_tmpUpdatePositions);
				PerformCellAdditions(m_tmpUpdatePositions);
				m_tmpUpdatePositions.Clear();
				break;
			case 4:
				GetUpdatePositions();
				PerformCellRemovals(m_tmpUpdatePositions);
				RemoveFarHighLevelGroups(m_tmpUpdatePositions);
				m_tmpUpdatePositions.Clear();
				break;
			}
		}

		private void GetUpdatePositions()
		{
			m_tmpUpdatePositions.Clear();
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
				if (controlledEntity != null)
				{
					m_tmpUpdatePositions.Add(controlledEntity.Entity.PositionComp.GetPosition());
				}
			}
		}

		private void PerformCellRemovals(List<Vector3D> updatePositions)
		{
			ShuffleMeshes();
			using (List<MyVoxelNavigationMesh>.Enumerator enumerator = m_tmpNavmeshes.GetEnumerator())
			{
				while (enumerator.MoveNext() && !enumerator.Current.RemoveOneUnusedCell(updatePositions))
				{
				}
			}
			m_tmpNavmeshes.Clear();
		}

		private void RemoveFarHighLevelGroups(List<Vector3D> updatePositions)
		{
			foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				navigationMesh.Value.RemoveFarHighLevelGroups(updatePositions);
			}
		}

		private void PerformCellAdditions(List<Vector3D> updatePositions)
		{
			MarkCellsOnPaths();
			ShuffleMeshes();
			using (List<MyVoxelNavigationMesh>.Enumerator enumerator = m_tmpNavmeshes.GetEnumerator())
			{
				while (enumerator.MoveNext() && !enumerator.Current.AddOneMarkedCell(updatePositions))
				{
				}
			}
			m_tmpNavmeshes.Clear();
		}

		private void PerformCellUpdates()
		{
			ShuffleMeshes();
			using (List<MyVoxelNavigationMesh>.Enumerator enumerator = m_tmpNavmeshes.GetEnumerator())
			{
				while (enumerator.MoveNext() && !enumerator.Current.RefreshOneChangedCell())
				{
				}
			}
			m_tmpNavmeshes.Clear();
		}

		private void ShuffleMeshes()
		{
			m_tmpNavmeshes.Clear();
			foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				m_tmpNavmeshes.Add(navigationMesh.Value);
			}
			m_tmpNavmeshes.ShuffleList();
		}

		private void PerformCellMarking(List<Vector3D> updatePositions)
		{
			Vector3D value = new Vector3D(1.0);
			foreach (Vector3D updatePosition in updatePositions)
			{
				BoundingBoxD box = new BoundingBoxD(updatePosition - value, updatePosition + value);
				m_tmpVoxelMaps.Clear();
				MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpVoxelMaps);
				foreach (MyVoxelBase tmpVoxelMap in m_tmpVoxelMaps)
				{
					MyVoxelNavigationMesh value2 = null;
					m_navigationMeshes.TryGetValue(tmpVoxelMap, out value2);
					value2?.MarkBoxForAddition(box);
				}
			}
			m_tmpVoxelMaps.Clear();
		}

		private void MarkCellsOnPaths()
		{
			foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				navigationMesh.Value.MarkCellsOnPaths();
			}
		}

		public void InvalidateBox(ref BoundingBoxD bbox)
		{
			foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				if (navigationMesh.Key.GetContainedVoxelCoords(ref bbox, out Vector3I min, out Vector3I max))
				{
					navigationMesh.Value.InvalidateRange(min, max);
				}
			}
		}

		public MyVoxelNavigationMesh GetVoxelMapNavmesh(MyVoxelBase map)
		{
			MyVoxelNavigationMesh value = null;
			m_navigationMeshes.TryGetValue(map, out value);
			return value;
		}

		public MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq, MyVoxelBase voxelMap = null)
		{
			MyNavigationPrimitive result = null;
			if (voxelMap != null)
			{
				MyVoxelNavigationMesh value = null;
				if (m_navigationMeshes.TryGetValue(voxelMap, out value))
				{
					result = value.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				}
				return result;
			}
			foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
			{
				MyNavigationPrimitive myNavigationPrimitive = navigationMesh.Value.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
				if (myNavigationPrimitive != null)
				{
					result = myNavigationPrimitive;
				}
			}
			return result;
		}

		[Conditional("DEBUG")]
		public void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				if (DebugLog != null)
				{
					DebugLog.DebugDraw();
				}
				foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in m_navigationMeshes)
				{
					_ = (Matrix)navigationMesh.Key.WorldMatrix;
				}
			}
		}

		[Conditional("DEBUG")]
		public void RemoveTriangle(int index)
		{
			if (m_navigationMeshes.Count != 0)
			{
				foreach (MyVoxelNavigationMesh value in m_navigationMeshes.Values)
				{
					value.RemoveTriangle(index);
				}
			}
		}
	}
}
