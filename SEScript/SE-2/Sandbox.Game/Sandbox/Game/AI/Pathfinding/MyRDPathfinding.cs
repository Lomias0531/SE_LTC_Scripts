using RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyRDPathfinding : IMyPathfinding
	{
		private class RequestedPath
		{
			public List<Vector3D> Path;

			public int LocalTicks;
		}

		private const int DEBUG_PATH_MAX_TICKS = 150;

		private const int TILE_SIZE = 16;

		private const int TILE_HEIGHT = 70;

		private const int TILE_LINE_COUNT = 25;

		private readonly double MIN_NAVMESH_MANAGER_SQUARED_DISTANCE = Math.Pow(160.0, 2.0);

		private Dictionary<MyPlanet, List<MyNavmeshManager>> m_planetManagers = new Dictionary<MyPlanet, List<MyNavmeshManager>>();

		private HashSet<MyCubeGrid> m_grids = new HashSet<MyCubeGrid>();

		private bool m_drawNavmesh;

		private BoundingBoxD? m_debugInvalidateTileAABB;

		private List<RequestedPath> m_debugDrawPaths = new List<RequestedPath>();

		public MyRDPathfinding()
		{
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
			MyEntities.OnEntityRemove += MyEntities_OnEntityRemove;
		}

		public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity relativeEntity)
		{
			MyRDPath myRDPath = new MyRDPath(this, begin, end);
			if (!myRDPath.GetNextTarget(begin, out Vector3D _, out float _, out IMyEntity _))
			{
				myRDPath = null;
			}
			return myRDPath;
		}

		public bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance)
		{
			return true;
		}

		public IMyPathfindingLog GetPathfindingLog()
		{
			return null;
		}

		public void Update()
		{
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				for (int i = 0; i < planetManager.Value.Count; i++)
				{
					if (!planetManager.Value[i].Update())
					{
						planetManager.Value.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public void UnloadData()
		{
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			foreach (MyCubeGrid grid in m_grids)
			{
				grid.OnBlockAdded -= Grid_OnBlockAdded;
				grid.OnBlockRemoved -= Grid_OnBlockRemoved;
			}
			m_grids.Clear();
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.UnloadData();
				}
			}
		}

		public void DebugDraw()
		{
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.DebugDraw();
				}
			}
			if (m_debugInvalidateTileAABB.HasValue)
			{
				MyRenderProxy.DebugDrawAABB(m_debugInvalidateTileAABB.Value, Color.Yellow, 0f);
			}
			DebugDrawPaths();
		}

		public static BoundingBoxD GetVoxelAreaAABB(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
		{
			MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref minVoxelChanged, out Vector3D worldPosition);
			MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref maxVoxelChanged, out Vector3D worldPosition2);
			return new BoundingBoxD(worldPosition, worldPosition2);
		}

		public List<Vector3D> GetPath(MyPlanet planet, Vector3D initialPosition, Vector3D targetPosition)
		{
			if (!m_planetManagers.ContainsKey(planet))
			{
				m_planetManagers[planet] = new List<MyNavmeshManager>();
				planet.RangeChanged += VoxelChanged;
			}
			List<Vector3D> bestPathFromManagers = GetBestPathFromManagers(planet, initialPosition, targetPosition);
			if (bestPathFromManagers.Count > 0)
			{
				m_debugDrawPaths.Add(new RequestedPath
				{
					Path = bestPathFromManagers,
					LocalTicks = 0
				});
			}
			return bestPathFromManagers;
		}

		public bool AddToTrackedGrids(MyCubeGrid cubeGrid)
		{
			if (m_grids.Add(cubeGrid))
			{
				cubeGrid.OnBlockAdded += Grid_OnBlockAdded;
				cubeGrid.OnBlockRemoved += Grid_OnBlockRemoved;
				return true;
			}
			return false;
		}

		public void InvalidateArea(BoundingBoxD areaBox)
		{
			MyPlanet planet = GetPlanet(areaBox.Center);
			AreaChanged(planet, areaBox);
		}

		public void SetDrawNavmesh(bool drawNavmesh)
		{
			m_drawNavmesh = drawNavmesh;
			foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in m_planetManagers)
			{
				foreach (MyNavmeshManager item in planetManager.Value)
				{
					item.DrawNavmesh = m_drawNavmesh;
				}
			}
		}

		private MyPlanet GetPlanet(Vector3D position)
		{
			int num = 500;
			BoundingBoxD box = new BoundingBoxD(position - (float)num * 0.5f, position + (float)num * 0.5f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		private void MyEntities_OnEntityAdd(MyEntity obj)
		{
			MyCubeGrid myCubeGrid = obj as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return;
			}
			MyPlanet planet = GetPlanet(myCubeGrid.PositionComp.WorldAABB.Center);
			if (planet != null && m_planetManagers.TryGetValue(planet, out List<MyNavmeshManager> value))
			{
				bool flag = false;
				foreach (MyNavmeshManager item in value)
				{
					flag |= item.InvalidateArea(myCubeGrid.PositionComp.WorldAABB);
				}
				if (flag)
				{
					AddToTrackedGrids(myCubeGrid);
				}
			}
		}

		private void MyEntities_OnEntityRemove(MyEntity obj)
		{
			MyCubeGrid myCubeGrid = obj as MyCubeGrid;
			if (myCubeGrid != null && m_grids.Remove(myCubeGrid))
			{
				myCubeGrid.OnBlockAdded -= Grid_OnBlockAdded;
				myCubeGrid.OnBlockRemoved -= Grid_OnBlockRemoved;
				MyPlanet planet = GetPlanet(myCubeGrid.PositionComp.WorldAABB.Center);
				if (planet != null && m_planetManagers.TryGetValue(planet, out List<MyNavmeshManager> value))
				{
					foreach (MyNavmeshManager item in value)
					{
						item.InvalidateArea(myCubeGrid.PositionComp.WorldAABB);
					}
				}
			}
		}

		private void Grid_OnBlockAdded(MySlimBlock slimBlock)
		{
			MyPlanet planet = GetPlanet(slimBlock.WorldPosition);
			if (planet != null && m_planetManagers.TryGetValue(planet, out List<MyNavmeshManager> value))
			{
				BoundingBoxD worldAABB = slimBlock.WorldAABB;
				foreach (MyNavmeshManager item in value)
				{
					item.InvalidateArea(worldAABB);
				}
			}
		}

		private void Grid_OnBlockRemoved(MySlimBlock slimBlock)
		{
			MyPlanet planet = GetPlanet(slimBlock.WorldPosition);
			if (planet != null && m_planetManagers.TryGetValue(planet, out List<MyNavmeshManager> value))
			{
				BoundingBoxD worldAABB = slimBlock.WorldAABB;
				foreach (MyNavmeshManager item in value)
				{
					item.InvalidateArea(worldAABB);
				}
			}
		}

		private void VoxelChanged(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			MyPlanet myPlanet = storage as MyPlanet;
			if (myPlanet != null)
			{
				BoundingBoxD voxelAreaAABB = GetVoxelAreaAABB(myPlanet, minVoxelChanged, maxVoxelChanged);
				AreaChanged(myPlanet, voxelAreaAABB);
				m_debugInvalidateTileAABB = voxelAreaAABB;
			}
		}

		private void AreaChanged(MyPlanet planet, BoundingBoxD areaBox)
		{
			if (m_planetManagers.TryGetValue(planet, out List<MyNavmeshManager> value))
			{
				foreach (MyNavmeshManager item in value)
				{
					item.InvalidateArea(areaBox);
				}
			}
		}

		private List<Vector3D> GetBestPathFromManagers(MyPlanet planet, Vector3D initialPosition, Vector3D targetPosition)
		{
			List<MyNavmeshManager> list = m_planetManagers[planet].Where((MyNavmeshManager m) => m.ContainsPosition(initialPosition)).ToList();
			if (list.Count > 0)
			{
				List<Vector3D> path;
				bool noTilesToGenerate;
				foreach (MyNavmeshManager item in list)
				{
					if (item.ContainsPosition(targetPosition) && (item.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate) || !noTilesToGenerate))
					{
						return path;
					}
				}
				MyNavmeshManager myNavmeshManager = null;
				double num = double.MaxValue;
				foreach (MyNavmeshManager item2 in list)
				{
					double num2 = (item2.Center - initialPosition).LengthSquared();
					if (num > num2)
					{
						num = num2;
						myNavmeshManager = item2;
					}
				}
				if (!myNavmeshManager.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate) && noTilesToGenerate && path.Count <= 2 && num > MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
				{
					double num3 = (initialPosition - targetPosition).LengthSquared();
					if ((myNavmeshManager.Center - targetPosition).LengthSquared() - num3 > MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
					{
						CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
					}
				}
				return path;
			}
			CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
			return new List<Vector3D>();
		}

		private MyNavmeshManager CreateManager(Vector3D center, Vector3D? forwardDirection = null)
		{
			if (!forwardDirection.HasValue)
			{
				Vector3D v = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
				forwardDirection = Vector3D.CalculatePerpendicularVector(v);
			}
			int tileSize = 16;
			int tileHeight = 70;
			int tileLineCount = 25;
			MyRecastOptions recastOptions = GetRecastOptions(null);
			MyNavmeshManager myNavmeshManager = new MyNavmeshManager(this, center, forwardDirection.Value, tileSize, tileHeight, tileLineCount, recastOptions);
			myNavmeshManager.DrawNavmesh = m_drawNavmesh;
			m_planetManagers[myNavmeshManager.Planet].Add(myNavmeshManager);
			return myNavmeshManager;
		}

		private MyRecastOptions GetRecastOptions(MyCharacter character)
		{
			return new MyRecastOptions
			{
				cellHeight = 0.2f,
				agentHeight = 1.5f,
				agentRadius = 0.5f,
				agentMaxClimb = 0.6f,
				agentMaxSlope = 60f,
				regionMinSize = 1f,
				regionMergeSize = 10f,
				edgeMaxLen = 50f,
				edgeMaxError = 3f,
				vertsPerPoly = 6f,
				detailSampleDist = 6f,
				detailSampleMaxError = 1f,
				partitionType = 1
			};
		}

		private void DebugDrawSinglePath(List<Vector3D> path)
		{
			for (int i = 1; i < path.Count; i++)
			{
				MyRenderProxy.DebugDrawSphere(path[i], 0.5f, Color.Yellow, 0f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(path[i - 1], path[i], Color.Yellow, Color.Yellow, depthRead: false);
			}
		}

		private void DebugDrawPaths()
		{
			_ = DateTime.Now;
			for (int i = 0; i < m_debugDrawPaths.Count; i++)
			{
				RequestedPath requestedPath = m_debugDrawPaths[i];
				requestedPath.LocalTicks++;
				if (requestedPath.LocalTicks > 150)
				{
					m_debugDrawPaths.RemoveAt(i);
					i--;
				}
				else
				{
					DebugDrawSinglePath(requestedPath.Path);
				}
			}
		}
	}
}
