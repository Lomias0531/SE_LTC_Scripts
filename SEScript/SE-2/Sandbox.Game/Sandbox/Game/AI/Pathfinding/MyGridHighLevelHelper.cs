using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyGridHighLevelHelper
	{
		private MyGridNavigationMesh m_mesh;

		private Vector3I m_cellSize;

		private ulong m_packedCoord;

		private int m_currentComponentRel;

		private List<List<int>> m_currentCellConnections;

		private MyVector3ISet m_changedCells;

		private MyVector3ISet m_changedCubes;

		private Dictionary<Vector3I, List<int>> m_triangleRegistry;

		private MyNavmeshComponents m_components;

		private List<MyNavigationTriangle> m_tmpComponentTriangles = new List<MyNavigationTriangle>();

		private List<int> m_tmpNeighbors = new List<int>();

		private static HashSet<int> m_tmpCellTriangles = new HashSet<int>();

		private static MyGridHighLevelHelper m_currentHelper = null;

		private static readonly Vector3I CELL_COORD_SHIFT = new Vector3I(524288);

		private Predicate<MyNavigationPrimitive> m_processTrianglePredicate = ProcessTriangleForHierarchyStatic;

		public bool IsDirty => !m_changedCells.Empty;

		public MyGridHighLevelHelper(MyGridNavigationMesh mesh, Dictionary<Vector3I, List<int>> triangleRegistry, Vector3I cellSize)
		{
			m_mesh = mesh;
			m_cellSize = cellSize;
			m_packedCoord = 0uL;
			m_currentCellConnections = new List<List<int>>();
			m_changedCells = new MyVector3ISet();
			m_changedCubes = new MyVector3ISet();
			m_triangleRegistry = triangleRegistry;
			m_components = new MyNavmeshComponents();
		}

		public void MarkBlockChanged(MySlimBlock block)
		{
			Vector3I cube = block.Min - Vector3I.One;
			Vector3I cube2 = block.Max + Vector3I.One;
			Vector3I next = cube;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref block.Min, ref block.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				m_changedCubes.Add(next);
				vector3I_RangeIterator.GetNext(out next);
			}
			Vector3I start = CubeToCell(ref cube);
			Vector3I end = CubeToCell(ref cube2);
			next = start;
			Vector3I_RangeIterator vector3I_RangeIterator2 = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator2.IsValid())
			{
				m_changedCells.Add(next);
				vector3I_RangeIterator2.GetNext(out next);
			}
		}

		public void ProcessChangedCellComponents()
		{
			m_currentHelper = this;
			List<int> value = null;
			foreach (Vector3I changedCell in m_changedCells)
			{
				Vector3I start = CellToLowestCube(changedCell);
				Vector3I end = start + m_cellSize - Vector3I.One;
				Vector3I next = start;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					if (m_triangleRegistry.TryGetValue(next, out value))
					{
						foreach (int item in value)
						{
							m_tmpCellTriangles.Add(item);
						}
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				if (m_tmpCellTriangles.Count != 0)
				{
					ulong cellCoord = new MyCellCoord(0, changedCell).PackId64();
					m_components.OpenCell(cellCoord);
					long num = m_mesh.GetCurrentTimestamp() + 1;
					long num2 = num;
					m_currentComponentRel = 0;
					m_tmpComponentTriangles.Clear();
					foreach (int tmpCellTriangle in m_tmpCellTriangles)
					{
						MyNavigationTriangle triangle = m_mesh.GetTriangle(tmpCellTriangle);
						if (m_currentComponentRel == 0 || !m_mesh.VisitedBetween(triangle, num, num2))
						{
							m_components.OpenComponent();
							if (m_currentComponentRel >= m_currentCellConnections.Count)
							{
								m_currentCellConnections.Add(new List<int>());
							}
							m_components.AddComponentTriangle(triangle, triangle.Center);
							triangle.ComponentIndex = m_currentComponentRel;
							m_tmpComponentTriangles.Add(triangle);
							m_mesh.PrepareTraversal(triangle, null, m_processTrianglePredicate);
							m_mesh.PerformTraversal();
							m_tmpComponentTriangles.Add(null);
							m_components.CloseComponent();
							num2 = m_mesh.GetCurrentTimestamp();
							if (m_currentComponentRel == 0)
							{
								num = num2;
							}
							m_currentComponentRel++;
						}
					}
					m_tmpCellTriangles.Clear();
					MyNavmeshComponents.ClosedCellInfo output = default(MyNavmeshComponents.ClosedCellInfo);
					m_components.CloseAndCacheCell(ref output);
					int num3 = output.StartingIndex;
					foreach (MyNavigationTriangle tmpComponentTriangle in m_tmpComponentTriangles)
					{
						if (tmpComponentTriangle == null)
						{
							num3++;
						}
						else
						{
							tmpComponentTriangle.ComponentIndex = num3;
						}
					}
					m_tmpComponentTriangles.Clear();
					if (!output.NewCell && output.ComponentNum != output.OldComponentNum)
					{
						for (int i = 0; i < output.OldComponentNum; i++)
						{
							m_mesh.HighLevelGroup.RemovePrimitive(output.OldStartingIndex + i);
						}
					}
					if (output.NewCell || output.ComponentNum != output.OldComponentNum)
					{
						for (int j = 0; j < output.ComponentNum; j++)
						{
							m_mesh.HighLevelGroup.AddPrimitive(output.StartingIndex + j, m_components.GetComponentCenter(j));
						}
					}
					if (!output.NewCell && output.ComponentNum == output.OldComponentNum)
					{
						for (int k = 0; k < output.ComponentNum; k++)
						{
							m_mesh.HighLevelGroup.GetPrimitive(output.StartingIndex + k).UpdatePosition(m_components.GetComponentCenter(k));
						}
					}
					for (int l = 0; l < output.ComponentNum; l++)
					{
						int num4 = output.StartingIndex + l;
						m_mesh.HighLevelGroup.GetPrimitive(num4).GetNeighbours(m_tmpNeighbors);
						foreach (int item2 in m_currentCellConnections[l])
						{
							if (!m_tmpNeighbors.Remove(item2))
							{
								m_mesh.HighLevelGroup.ConnectPrimitives(num4, item2);
							}
						}
						foreach (int tmpNeighbor in m_tmpNeighbors)
						{
							MyHighLevelPrimitive myHighLevelPrimitive = m_mesh.HighLevelGroup.TryGetPrimitive(tmpNeighbor);
							if (myHighLevelPrimitive != null && myHighLevelPrimitive.IsExpanded)
							{
								m_mesh.HighLevelGroup.DisconnectPrimitives(num4, tmpNeighbor);
							}
						}
						m_tmpNeighbors.Clear();
						m_currentCellConnections[l].Clear();
					}
					for (int m = 0; m < output.ComponentNum; m++)
					{
						num3 = output.StartingIndex + m;
						MyHighLevelPrimitive primitive = m_mesh.HighLevelGroup.GetPrimitive(num3);
						if (primitive != null)
						{
							primitive.IsExpanded = true;
						}
					}
				}
			}
			m_changedCells.Clear();
			m_currentHelper = null;
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
			if (m_tmpCellTriangles.Contains(triangle.Index))
			{
				m_components.AddComponentTriangle(triangle, triangle.Center);
				m_tmpComponentTriangles.Add(triangle);
				return true;
			}
			if (m_components.TryGetComponentCell(triangle.ComponentIndex, out ulong _) && !m_currentCellConnections[m_currentComponentRel].Contains(triangle.ComponentIndex))
			{
				m_currentCellConnections[m_currentComponentRel].Add(triangle.ComponentIndex);
			}
			return false;
		}

		public MyHighLevelPrimitive GetHighLevelNavigationPrimitive(MyNavigationTriangle triangle)
		{
			if (triangle == null)
			{
				return null;
			}
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

		private void TryClearCell(ulong packedCoord)
		{
			if (m_components.TryGetCell(packedCoord, out MyNavmeshComponents.CellInfo cellInfo))
			{
				m_components.ClearCell(packedCoord, ref cellInfo);
			}
		}

		private Vector3I CubeToCell(ref Vector3I cube)
		{
			Vector3D v = cube;
			v /= (Vector3D)m_cellSize;
			Vector3I.Floor(ref v, out Vector3I r);
			return r + CELL_COORD_SHIFT;
		}

		private Vector3I CellToLowestCube(Vector3I cell)
		{
			return (cell - CELL_COORD_SHIFT) * m_cellSize;
		}
	}
}
