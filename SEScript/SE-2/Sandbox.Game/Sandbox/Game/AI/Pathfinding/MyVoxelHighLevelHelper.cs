using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyVoxelHighLevelHelper
	{
		public class Component : IMyHighLevelComponent
		{
			private int m_componentIndex;

			private Base6Directions.DirectionFlags m_exploredDirections;

			bool IMyHighLevelComponent.FullyExplored => m_exploredDirections == Base6Directions.DirectionFlags.All;

			public Component(int index, Base6Directions.DirectionFlags exploredDirections)
			{
				m_componentIndex = index;
				m_exploredDirections = exploredDirections;
			}

			bool IMyHighLevelComponent.Contains(MyNavigationPrimitive primitive)
			{
				MyNavigationTriangle myNavigationTriangle = primitive as MyNavigationTriangle;
				if (myNavigationTriangle == null)
				{
					return false;
				}
				return myNavigationTriangle.ComponentIndex == m_componentIndex;
			}
		}

		public struct ConnectionInfo
		{
			public int ComponentIndex;

			public Base6Directions.Direction Direction;
		}

		public static readonly bool DO_CONSISTENCY_CHECKS = true;

		private MyVoxelNavigationMesh m_mesh;

		private bool m_cellOpen;

		private MyIntervalList m_triangleList;

		private int m_currentComponentRel;

		private int m_currentComponentMarker;

		private Vector3I m_currentCell;

		private ulong m_packedCoord;

		private List<List<ConnectionInfo>> m_currentCellConnections;

		private static MyVoxelHighLevelHelper m_currentHelper;

		private Dictionary<ulong, MyIntervalList> m_triangleLists;

		private MyVector3ISet m_exploredCells;

		private MyNavmeshComponents m_navmeshComponents;

		private Predicate<MyNavigationPrimitive> m_processTrianglePredicate = ProcessTriangleForHierarchyStatic;

		private List<MyNavigationTriangle> m_tmpComponentTriangles = new List<MyNavigationTriangle>();

		private List<int> m_tmpNeighbors = new List<int>();

		private static List<ulong> m_removedHLpackedCoord = new List<ulong>();

		public MyVoxelHighLevelHelper(MyVoxelNavigationMesh mesh)
		{
			m_mesh = mesh;
			m_triangleList = new MyIntervalList();
			m_triangleLists = new Dictionary<ulong, MyIntervalList>();
			m_exploredCells = new MyVector3ISet();
			m_navmeshComponents = new MyNavmeshComponents();
			m_currentCellConnections = new List<List<ConnectionInfo>>();
			for (int i = 0; i < 8; i++)
			{
				m_currentCellConnections.Add(new List<ConnectionInfo>());
			}
		}

		public void OpenNewCell(MyCellCoord coord)
		{
			m_cellOpen = true;
			m_currentCell = coord.CoordInLod;
			m_packedCoord = coord.PackId64();
			m_triangleList.Clear();
		}

		public void AddTriangle(int triIndex)
		{
			m_triangleList.Add(triIndex);
		}

		public void CloseCell()
		{
			m_cellOpen = false;
			m_packedCoord = 0uL;
			m_triangleList.Clear();
		}

		public void ProcessCellComponents()
		{
			m_triangleLists.Add(m_packedCoord, m_triangleList.GetCopy());
			MyNavmeshComponents.ClosedCellInfo cellInfo = ConstructComponents();
			UpdateHighLevelPrimitives(ref cellInfo);
			MarkExploredDirections(ref cellInfo);
			for (int i = 0; i < cellInfo.ComponentNum; i++)
			{
				int index = cellInfo.StartingIndex + i;
				MyHighLevelPrimitive primitive = m_mesh.HighLevelGroup.GetPrimitive(index);
				if (primitive != null)
				{
					primitive.IsExpanded = true;
				}
			}
		}

		private void MarkExploredDirections(ref MyNavmeshComponents.ClosedCellInfo cellInfo)
		{
			Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
			foreach (Base6Directions.Direction direction in enumDirections)
			{
				Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(direction);
				if (cellInfo.ExploredDirections.HasFlag(directionFlag))
				{
					continue;
				}
				Vector3I intVector = Base6Directions.GetIntVector(direction);
				MyCellCoord myCellCoord = default(MyCellCoord);
				myCellCoord.Lod = 0;
				myCellCoord.CoordInLod = m_currentCell + intVector;
				if (myCellCoord.CoordInLod.X != -1 && myCellCoord.CoordInLod.Y != -1 && myCellCoord.CoordInLod.Z != -1)
				{
					ulong num = myCellCoord.PackId64();
					if (m_triangleLists.ContainsKey(num))
					{
						m_navmeshComponents.MarkExplored(num, Base6Directions.GetFlippedDirection(direction));
						cellInfo.ExploredDirections |= Base6Directions.GetDirectionFlag(direction);
					}
				}
			}
			m_navmeshComponents.SetExplored(m_packedCoord, cellInfo.ExploredDirections);
		}

		private void UpdateHighLevelPrimitives(ref MyNavmeshComponents.ClosedCellInfo cellInfo)
		{
			int num = cellInfo.StartingIndex;
			foreach (MyNavigationTriangle tmpComponentTriangle in m_tmpComponentTriangles)
			{
				if (tmpComponentTriangle == null)
				{
					num++;
				}
				else
				{
					tmpComponentTriangle.ComponentIndex = num;
				}
			}
			m_tmpComponentTriangles.Clear();
			if (!cellInfo.NewCell && cellInfo.ComponentNum != cellInfo.OldComponentNum)
			{
				for (int i = 0; i < cellInfo.OldComponentNum; i++)
				{
					m_mesh.HighLevelGroup.RemovePrimitive(cellInfo.OldStartingIndex + i);
				}
			}
			if (cellInfo.NewCell || cellInfo.ComponentNum != cellInfo.OldComponentNum)
			{
				for (int j = 0; j < cellInfo.ComponentNum; j++)
				{
					m_mesh.HighLevelGroup.AddPrimitive(cellInfo.StartingIndex + j, m_navmeshComponents.GetComponentCenter(j));
				}
			}
			if (!cellInfo.NewCell && cellInfo.ComponentNum == cellInfo.OldComponentNum)
			{
				for (int k = 0; k < cellInfo.ComponentNum; k++)
				{
					m_mesh.HighLevelGroup.GetPrimitive(cellInfo.StartingIndex + k).UpdatePosition(m_navmeshComponents.GetComponentCenter(k));
				}
			}
			for (int l = 0; l < cellInfo.ComponentNum; l++)
			{
				int num2 = cellInfo.StartingIndex + l;
				m_mesh.HighLevelGroup.GetPrimitive(num2).GetNeighbours(m_tmpNeighbors);
				foreach (ConnectionInfo item in m_currentCellConnections[l])
				{
					if (!m_tmpNeighbors.Remove(item.ComponentIndex))
					{
						m_mesh.HighLevelGroup.ConnectPrimitives(num2, item.ComponentIndex);
					}
				}
				foreach (int tmpNeighbor in m_tmpNeighbors)
				{
					MyHighLevelPrimitive myHighLevelPrimitive = m_mesh.HighLevelGroup.TryGetPrimitive(tmpNeighbor);
					if (myHighLevelPrimitive != null && myHighLevelPrimitive.IsExpanded)
					{
						m_mesh.HighLevelGroup.DisconnectPrimitives(num2, tmpNeighbor);
					}
				}
				m_tmpNeighbors.Clear();
				m_currentCellConnections[l].Clear();
			}
		}

		private MyNavmeshComponents.ClosedCellInfo ConstructComponents()
		{
			long num = m_mesh.GetCurrentTimestamp() + 1;
			long end = num;
			m_currentComponentRel = 0;
			m_navmeshComponents.OpenCell(m_packedCoord);
			m_tmpComponentTriangles.Clear();
			MyIntervalList.Enumerator enumerator = m_triangleList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				m_currentComponentMarker = -2 - m_currentComponentRel;
				MyNavigationTriangle triangle = m_mesh.GetTriangle(current);
				if (!m_mesh.VisitedBetween(triangle, num, end))
				{
					m_navmeshComponents.OpenComponent();
					if (m_currentComponentRel >= m_currentCellConnections.Count)
					{
						m_currentCellConnections.Add(new List<ConnectionInfo>());
					}
					m_currentHelper = this;
					m_navmeshComponents.AddComponentTriangle(triangle, triangle.Center);
					triangle.ComponentIndex = m_currentComponentMarker;
					m_tmpComponentTriangles.Add(triangle);
					m_mesh.PrepareTraversal(triangle, null, m_processTrianglePredicate);
					m_mesh.PerformTraversal();
					m_tmpComponentTriangles.Add(null);
					m_navmeshComponents.CloseComponent();
					end = m_mesh.GetCurrentTimestamp();
					m_currentComponentRel++;
				}
			}
			MyNavmeshComponents.ClosedCellInfo output = default(MyNavmeshComponents.ClosedCellInfo);
			m_navmeshComponents.CloseAndCacheCell(ref output);
			return output;
		}

		public MyIntervalList TryGetTriangleList(ulong packedCellCoord)
		{
			MyIntervalList value = null;
			m_triangleLists.TryGetValue(packedCellCoord, out value);
			return value;
		}

		public void CollectComponents(ulong packedCoord, List<int> output)
		{
			MyNavmeshComponents.CellInfo cellInfo = default(MyNavmeshComponents.CellInfo);
			if (m_navmeshComponents.TryGetCell(packedCoord, out cellInfo))
			{
				for (int i = 0; i < cellInfo.ComponentNum; i++)
				{
					output.Add(cellInfo.StartingIndex + i);
				}
			}
		}

		public IMyHighLevelComponent GetComponent(MyHighLevelPrimitive primitive)
		{
			if (m_navmeshComponents.GetComponentCell(primitive.Index, out ulong cellIndex))
			{
				if (m_navmeshComponents.GetComponentInfo(primitive.Index, cellIndex, out Base6Directions.DirectionFlags exploredDirections))
				{
					MyCellCoord myCellCoord = default(MyCellCoord);
					myCellCoord.SetUnpack(cellIndex);
					Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
					foreach (Base6Directions.Direction dir in enumDirections)
					{
						Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(dir);
						if (!exploredDirections.HasFlag(directionFlag))
						{
							Vector3I position = myCellCoord.CoordInLod + Base6Directions.GetIntVector(dir);
							if (m_exploredCells.Contains(ref position))
							{
								exploredDirections |= directionFlag;
							}
						}
					}
					return new Component(primitive.Index, exploredDirections);
				}
				return null;
			}
			return null;
		}

		public void ClearCachedCell(ulong packedCoord)
		{
			m_triangleLists.Remove(packedCoord);
			if (!m_navmeshComponents.TryGetCell(packedCoord, out MyNavmeshComponents.CellInfo cellInfo))
			{
				return;
			}
			for (int i = 0; i < cellInfo.ComponentNum; i++)
			{
				int index = cellInfo.StartingIndex + i;
				MyHighLevelPrimitive primitive = m_mesh.HighLevelGroup.GetPrimitive(index);
				if (primitive != null)
				{
					primitive.IsExpanded = false;
				}
			}
		}

		public void TryClearCell(ulong packedCoord)
		{
			if (m_triangleLists.ContainsKey(packedCoord))
			{
				ClearCachedCell(packedCoord);
			}
			RemoveExplored(packedCoord);
			if (!m_navmeshComponents.TryGetCell(packedCoord, out MyNavmeshComponents.CellInfo cellInfo))
			{
				return;
			}
			for (int i = 0; i < cellInfo.ComponentNum; i++)
			{
				int index = cellInfo.StartingIndex + i;
				m_mesh.HighLevelGroup.RemovePrimitive(index);
			}
			Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
			foreach (Base6Directions.Direction direction in enumDirections)
			{
				Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(direction);
				if (cellInfo.ExploredDirections.HasFlag(directionFlag))
				{
					Vector3I intVector = Base6Directions.GetIntVector(direction);
					MyCellCoord myCellCoord = default(MyCellCoord);
					myCellCoord.SetUnpack(packedCoord);
					myCellCoord.CoordInLod += intVector;
					if (m_navmeshComponents.TryGetCell(myCellCoord.PackId64(), out MyNavmeshComponents.CellInfo cellInfo2))
					{
						Base6Directions.DirectionFlags directionFlag2 = Base6Directions.GetDirectionFlag(Base6Directions.GetFlippedDirection(direction));
						m_navmeshComponents.SetExplored(myCellCoord.PackId64(), (Base6Directions.DirectionFlags)((int)cellInfo2.ExploredDirections & (int)(byte)(~(uint)directionFlag2)));
					}
				}
			}
			m_navmeshComponents.ClearCell(packedCoord, ref cellInfo);
		}

		public MyHighLevelPrimitive GetHighLevelNavigationPrimitive(MyNavigationTriangle triangle)
		{
			if (triangle.Parent != m_mesh)
			{
				return null;
			}
			if (triangle.ComponentIndex != -1)
			{
				return m_mesh.HighLevelGroup.GetPrimitive(triangle.ComponentIndex);
			}
			return null;
		}

		public void AddExplored(ref Vector3I cellPos)
		{
			m_exploredCells.Add(ref cellPos);
		}

		private void RemoveExplored(ulong packedCoord)
		{
			MyCellCoord myCellCoord = default(MyCellCoord);
			myCellCoord.SetUnpack(packedCoord);
			m_exploredCells.Remove(ref myCellCoord.CoordInLod);
		}

		private static bool ProcessTriangleForHierarchyStatic(MyNavigationPrimitive primitive)
		{
			MyNavigationTriangle triangle = primitive as MyNavigationTriangle;
			return m_currentHelper.ProcessTriangleForHierarchy(triangle);
		}

		private bool ProcessTriangleForHierarchy(MyNavigationTriangle triangle)
		{
			if (triangle.Parent != m_mesh)
			{
				return false;
			}
			if (triangle.ComponentIndex == -1)
			{
				m_navmeshComponents.AddComponentTriangle(triangle, triangle.Center);
				m_tmpComponentTriangles.Add(triangle);
				triangle.ComponentIndex = m_currentComponentMarker;
				return true;
			}
			if (triangle.ComponentIndex == m_currentComponentMarker)
			{
				return true;
			}
			if (m_navmeshComponents.GetComponentCell(triangle.ComponentIndex, out ulong cellIndex))
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				myCellCoord.SetUnpack(cellIndex);
				Vector3I vec = myCellCoord.CoordInLod - m_currentCell;
				if (vec.RectangularLength() != 1)
				{
					return false;
				}
				ConnectionInfo item = default(ConnectionInfo);
				item.Direction = Base6Directions.GetDirection(vec);
				item.ComponentIndex = triangle.ComponentIndex;
				if (!m_currentCellConnections[m_currentComponentRel].Contains(item))
				{
					m_currentCellConnections[m_currentComponentRel].Add(item);
				}
			}
			return false;
		}

		[Conditional("DEBUG")]
		public void CheckConsistency()
		{
			if (DO_CONSISTENCY_CHECKS)
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				foreach (KeyValuePair<ulong, MyIntervalList> triangleList in m_triangleLists)
				{
					myCellCoord.SetUnpack(triangleList.Key);
				}
			}
		}

		public void DebugDraw()
		{
			if (MyFakes.DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS)
			{
				foreach (Vector3I exploredCell in m_exploredCells)
				{
					Vector3I geometryCellCoord = exploredCell;
					MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(m_mesh.VoxelMapReferencePosition, ref geometryCellCoord, out BoundingBoxD worldAABB);
					MyRenderProxy.DebugDrawAABB(worldAABB, Color.Sienna, 1f, 1f, depthRead: false);
				}
			}
			if (MyFakes.DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS)
			{
				foreach (ulong presentCell in m_navmeshComponents.GetPresentCells())
				{
					MyCellCoord myCellCoord = default(MyCellCoord);
					myCellCoord.SetUnpack(presentCell);
					Vector3I position = myCellCoord.CoordInLod;
					if (m_exploredCells.Contains(ref position))
					{
						MyNavmeshComponents.CellInfo cellInfo = default(MyNavmeshComponents.CellInfo);
						if (m_navmeshComponents.TryGetCell(presentCell, out cellInfo))
						{
							for (int i = 0; i < cellInfo.ComponentNum; i++)
							{
								int index = cellInfo.StartingIndex + i;
								MyHighLevelPrimitive primitive = m_mesh.HighLevelGroup.GetPrimitive(index);
								Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
								foreach (Base6Directions.Direction dir in enumDirections)
								{
									Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(dir);
									if (!cellInfo.ExploredDirections.HasFlag(directionFlag) && !m_exploredCells.Contains(position + Base6Directions.GetIntVector(dir)))
									{
										Vector3 vector = Base6Directions.GetVector(dir);
										MyRenderProxy.DebugDrawLine3D(primitive.WorldPosition, primitive.WorldPosition + vector * 3f, Color.Red, Color.Red, depthRead: false);
									}
								}
							}
						}
					}
				}
			}
		}

		public void RemoveTooFarCells(List<Vector3D> importantPositions, float maxDistance, MyVector3ISet processedCells)
		{
			m_removedHLpackedCoord.Clear();
			foreach (Vector3I exploredCell in m_exploredCells)
			{
				Vector3I geometryCellCoord = exploredCell;
				MyVoxelCoordSystems.GeometryCellCenterCoordToWorldPos(m_mesh.VoxelMapReferencePosition, ref geometryCellCoord, out Vector3D worldPos);
				float num = float.PositiveInfinity;
				foreach (Vector3D importantPosition in importantPositions)
				{
					float num2 = Vector3.RectangularDistance(importantPosition, worldPos);
					if (num2 < num)
					{
						num = num2;
					}
				}
				if (num > maxDistance && !processedCells.Contains(geometryCellCoord))
				{
					MyCellCoord myCellCoord = new MyCellCoord(0, geometryCellCoord);
					m_removedHLpackedCoord.Add(myCellCoord.PackId64());
				}
			}
			foreach (ulong item in m_removedHLpackedCoord)
			{
				TryClearCell(item);
			}
		}

		public void GetCellsOfPrimitives(ref HashSet<ulong> cells, ref List<MyHighLevelPrimitive> primitives)
		{
			foreach (MyHighLevelPrimitive primitive in primitives)
			{
				if (m_navmeshComponents.GetComponentCell(primitive.Index, out ulong cellIndex))
				{
					cells.Add(cellIndex);
				}
			}
		}
	}
}
