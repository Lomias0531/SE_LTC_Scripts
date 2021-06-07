using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyVoxelNavigationMesh : MyNavigationMesh
	{
		private class CellToAddHeapItem : HeapItem<float>
		{
			public Vector3I Position;
		}

		private struct DebugDrawEdge
		{
			public Vector3 V1;

			public Vector3 V2;

			public DebugDrawEdge(Vector3 v1, Vector3 v2)
			{
				V1 = v1;
				V2 = v2;
			}
		}

		private static bool DO_CONSISTENCY_CHECKS = false;

		private MyVoxelBase m_voxelMap;

		private static MyVoxelBase m_staticVoxelMap;

		private Vector3 m_cellSize;

		private MyVector3ISet m_processedCells;

		private HashSet<ulong> m_cellsOnWayCoords;

		private List<Vector3I> m_cellsOnWay;

		private List<MyHighLevelPrimitive> m_primitivesOnPath;

		private MyBinaryHeap<float, CellToAddHeapItem> m_toAdd;

		private List<CellToAddHeapItem> m_heapItemList;

		private MyVector3ISet m_markedForAddition;

		private static MyDynamicObjectPool<CellToAddHeapItem> m_heapItemAllocator = new MyDynamicObjectPool<CellToAddHeapItem>(128);

		private static MyVector3ISet m_tmpCellSet = new MyVector3ISet();

		private static List<MyCubeGrid> m_tmpGridList = new List<MyCubeGrid>();

		private static List<MyGridPathfinding.CubeId> m_tmpLinkCandidates = new List<MyGridPathfinding.CubeId>();

		private static Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> m_tmpCubeLinkCandidates = new Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>>();

		private static MyDynamicObjectPool<List<MyNavigationPrimitive>> m_primitiveListPool = new MyDynamicObjectPool<List<MyNavigationPrimitive>>(8);

		private LinkedList<Vector3I> m_cellsToChange;

		private MyVector3ISet m_cellsToChangeSet;

		private static MyUnionFind m_vertexMapping = new MyUnionFind();

		private static List<int> m_tmpIntList = new List<int>();

		private MyVoxelConnectionHelper m_connectionHelper;

		private MyNavmeshCoordinator m_navmeshCoordinator;

		private MyHighLevelGroup m_higherLevel;

		private MyVoxelHighLevelHelper m_higherLevelHelper;

		private static HashSet<Vector3I> m_adjacentCells = new HashSet<Vector3I>();

		private static Dictionary<Vector3I, BoundingBoxD> m_adjacentBBoxes = new Dictionary<Vector3I, BoundingBoxD>();

		private static Vector3D m_halfMeterOffset = new Vector3D(0.5);

		private static BoundingBoxD m_cellBB = default(BoundingBoxD);

		private static Vector3D m_bbMinOffset = new Vector3D(-0.125);

		private Vector3I m_maxCellCoord;

		private const float ExploredRemovingDistance = 200f;

		private const float ProcessedRemovingDistance = 50f;

		private const float AddRemoveKoef = 0.5f;

		private const float MaxAddToProcessingDistance = 25f;

		private float LimitAddingWeight = GetWeight(25f);

		private const float CellsOnWayAdvance = 8f;

		public static float PresentEntityWeight = 100f;

		public static float RecountCellWeight = 10f;

		public static float JustAddedAdjacentCellWeight = 0.02f;

		public static float TooFarWeight = -100f;

		private Vector3 m_debugPos1;

		private Vector3 m_debugPos2;

		private Vector3 m_debugPos3;

		private Vector3 m_debugPos4;

		private Dictionary<ulong, List<DebugDrawEdge>> m_debugCellEdges;

		public const int NAVMESH_LOD = 0;

		private static readonly Vector3I[] m_cornerOffsets = new Vector3I[8]
		{
			new Vector3I(-1, -1, -1),
			new Vector3I(0, -1, -1),
			new Vector3I(-1, 0, -1),
			new Vector3I(0, 0, -1),
			new Vector3I(-1, -1, 0),
			new Vector3I(0, -1, 0),
			new Vector3I(-1, 0, 0),
			new Vector3I(0, 0, 0)
		};

		public static MyVoxelBase VoxelMap => m_staticVoxelMap;

		public Vector3D VoxelMapReferencePosition => m_voxelMap.PositionLeftBottomCorner;

		public Vector3D VoxelMapWorldPosition => m_voxelMap.PositionComp.GetPosition();

		public override MyHighLevelGroup HighLevelGroup => m_higherLevel;

		public MyVoxelNavigationMesh(MyVoxelBase voxelMap, MyNavmeshCoordinator coordinator, Func<long> timestampFunction)
			: base(coordinator.Links, 16, timestampFunction)
		{
			m_voxelMap = voxelMap;
			m_staticVoxelMap = m_voxelMap;
			m_processedCells = new MyVector3ISet();
			m_cellsOnWayCoords = new HashSet<ulong>();
			m_cellsOnWay = new List<Vector3I>();
			m_primitivesOnPath = new List<MyHighLevelPrimitive>(128);
			m_toAdd = new MyBinaryHeap<float, CellToAddHeapItem>(128);
			m_heapItemList = new List<CellToAddHeapItem>();
			m_markedForAddition = new MyVector3ISet();
			m_cellsToChange = new LinkedList<Vector3I>();
			m_cellsToChangeSet = new MyVector3ISet();
			m_connectionHelper = new MyVoxelConnectionHelper();
			m_navmeshCoordinator = coordinator;
			m_higherLevel = new MyHighLevelGroup(this, coordinator.HighLevelLinks, timestampFunction);
			m_higherLevelHelper = new MyVoxelHighLevelHelper(this);
			m_debugCellEdges = new Dictionary<ulong, List<DebugDrawEdge>>();
			voxelMap.Storage.RangeChanged += OnStorageChanged;
			m_maxCellCoord = m_voxelMap.Size / 8 - Vector3I.One;
		}

		public override string ToString()
		{
			return "Voxel NavMesh: " + m_voxelMap.StorageName;
		}

		private void OnStorageChanged(Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			if (changedData.HasFlag(MyStorageDataTypeFlags.Content))
			{
				InvalidateRange(minVoxelChanged, maxVoxelChanged);
			}
		}

		public void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
		{
			minVoxelChanged -= MyPrecalcComponent.InvalidatedRangeInflate;
			maxVoxelChanged += MyPrecalcComponent.InvalidatedRangeInflate;
			m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
			m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref minVoxelChanged, out Vector3I geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref maxVoxelChanged, out Vector3I geometryCellCoord2);
			Vector3I position = geometryCellCoord;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref geometryCellCoord, ref geometryCellCoord2);
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_processedCells.Contains(ref position))
				{
					if (!m_cellsToChangeSet.Contains(ref position))
					{
						m_cellsToChange.AddLast(position);
						m_cellsToChangeSet.Add(position);
					}
				}
				else
				{
					MyCellCoord myCellCoord = new MyCellCoord(0, position);
					m_higherLevelHelper.TryClearCell(myCellCoord.PackId64());
				}
				vector3I_RangeIterator.GetNext(out position);
			}
		}

		public void MarkBoxForAddition(BoundingBoxD box)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref box.Min, out Vector3I voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_voxelMap.PositionLeftBottomCorner, ref box.Max, out Vector3I voxelCoord2);
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord);
			m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out voxelCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out voxelCoord2);
			Vector3 value = voxelCoord + voxelCoord2;
			value *= 0.5f;
			voxelCoord /= 1;
			voxelCoord2 /= 1;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref voxelCoord, ref voxelCoord2);
			while (vector3I_RangeIterator.IsValid())
			{
				if (!(Vector3.RectangularDistance(voxelCoord, value) > 1f))
				{
					MarkCellForAddition(voxelCoord, PresentEntityWeight);
				}
				vector3I_RangeIterator.GetNext(out voxelCoord);
			}
		}

		private static float GetWeight(float rectDistance)
		{
			if (rectDistance < 0f)
			{
				return 1f;
			}
			return 1f / (1f + rectDistance);
		}

		private bool IsCellPosValid(ref Vector3I cellPos)
		{
			if (cellPos.X > m_maxCellCoord.X || cellPos.Y > m_maxCellCoord.Y || cellPos.Z > m_maxCellCoord.Z)
			{
				return false;
			}
			return new MyCellCoord(0, cellPos).IsCoord64Valid();
		}

		private void MarkCellForAddition(Vector3I cellPos, float weight)
		{
			if (m_processedCells.Contains(ref cellPos) || m_markedForAddition.Contains(ref cellPos) || !IsCellPosValid(ref cellPos))
			{
				return;
			}
			if (!m_toAdd.Full)
			{
				MarkCellForAdditionInternal(ref cellPos, weight);
				return;
			}
			float heapKey = m_toAdd.Min().HeapKey;
			if (weight > heapKey)
			{
				RemoveMinMarkedForAddition();
				MarkCellForAdditionInternal(ref cellPos, weight);
			}
		}

		private void MarkCellForAdditionInternal(ref Vector3I cellPos, float weight)
		{
			CellToAddHeapItem cellToAddHeapItem = m_heapItemAllocator.Allocate();
			cellToAddHeapItem.Position = cellPos;
			m_toAdd.Insert(cellToAddHeapItem, weight);
			m_markedForAddition.Add(cellPos);
		}

		private void RemoveMinMarkedForAddition()
		{
			CellToAddHeapItem cellToAddHeapItem = m_toAdd.RemoveMin();
			m_heapItemAllocator.Deallocate(cellToAddHeapItem);
			m_markedForAddition.Remove(cellToAddHeapItem.Position);
		}

		public bool RefreshOneChangedCell()
		{
			bool flag = false;
			while (!flag)
			{
				if (m_cellsToChange.Count == 0)
				{
					return flag;
				}
				Vector3I position = m_cellsToChange.First.Value;
				m_cellsToChange.RemoveFirst();
				m_cellsToChangeSet.Remove(ref position);
				if (m_processedCells.Contains(ref position))
				{
					RemoveCell(position);
					MarkCellForAddition(position, RecountCellWeight);
					flag = true;
				}
				else
				{
					MyCellCoord myCellCoord = new MyCellCoord(0, position);
					m_higherLevelHelper.TryClearCell(myCellCoord.PackId64());
				}
			}
			return flag;
		}

		public bool AddOneMarkedCell(List<Vector3D> importantPositions)
		{
			bool flag = false;
			foreach (Vector3I item in m_cellsOnWay)
			{
				Vector3I position = item;
				if (!m_processedCells.Contains(ref position) && !m_markedForAddition.Contains(ref position))
				{
					float weight = CalculateCellWeight(importantPositions, position);
					MarkCellForAddition(position, weight);
				}
			}
			while (!flag)
			{
				if (m_toAdd.Count == 0)
				{
					return flag;
				}
				m_toAdd.QueryAll(m_heapItemList);
				float num = float.NegativeInfinity;
				CellToAddHeapItem cellToAddHeapItem = null;
				foreach (CellToAddHeapItem heapItem in m_heapItemList)
				{
					float num2 = CalculateCellWeight(importantPositions, heapItem.Position);
					if (num2 > num)
					{
						num = num2;
						cellToAddHeapItem = heapItem;
					}
					m_toAdd.Modify(heapItem, num2);
				}
				m_heapItemList.Clear();
				if (cellToAddHeapItem == null || num < LimitAddingWeight)
				{
					return flag;
				}
				m_toAdd.Remove(cellToAddHeapItem);
				Vector3I position2 = cellToAddHeapItem.Position;
				m_heapItemAllocator.Deallocate(cellToAddHeapItem);
				m_markedForAddition.Remove(position2);
				m_adjacentCells.Clear();
				if (AddCell(position2, ref m_adjacentCells))
				{
					foreach (Vector3I adjacentCell in m_adjacentCells)
					{
						float weight2 = CalculateCellWeight(importantPositions, adjacentCell);
						MarkCellForAddition(adjacentCell, weight2);
					}
					flag = true;
					break;
				}
			}
			return flag;
		}

		private float CalculateCellWeight(List<Vector3D> importantPositions, Vector3I cellPos)
		{
			Vector3I geometryCellCoord = cellPos;
			MyVoxelCoordSystems.GeometryCellCenterCoordToWorldPos(m_voxelMap.PositionLeftBottomCorner, ref geometryCellCoord, out Vector3D worldPos);
			float num = float.PositiveInfinity;
			foreach (Vector3D importantPosition in importantPositions)
			{
				float num2 = Vector3.RectangularDistance(importantPosition, worldPos);
				if (num2 < num)
				{
					num = num2;
				}
			}
			if (m_cellsOnWayCoords.Contains(MyCellCoord.PackId64Static(0, cellPos)))
			{
				num -= 8f;
			}
			return GetWeight(num);
		}

		[Conditional("DEBUG")]
		private void AddDebugOuterEdge(ushort a, ushort b, List<DebugDrawEdge> debugEdgesList, Vector3D aTformed, Vector3D bTformed)
		{
			if (!m_connectionHelper.IsInnerEdge(a, b))
			{
				debugEdgesList.Add(new DebugDrawEdge(aTformed, bTformed));
			}
		}

		private bool AddCell(Vector3I cellPos, ref HashSet<Vector3I> adjacentCellPos)
		{
			if (MyFakes.LOG_NAVMESH_GENERATION)
			{
				MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.DebugLog.LogCellAddition(this, cellPos);
			}
			new MyCellCoord(0, cellPos);
			_ = cellPos * 8 + 8 + 1;
			return true;
		}

		private void PreprocessTriangles(MyIsoMesh generatedMesh, Vector3 centerDisplacement)
		{
			for (int i = 0; i < generatedMesh.TrianglesCount; i++)
			{
				ushort v = generatedMesh.Triangles[i].V0;
				ushort v2 = generatedMesh.Triangles[i].V1;
				ushort v3 = generatedMesh.Triangles[i].V2;
				generatedMesh.GetUnpackedPosition(v, out Vector3 position);
				Vector3 value = position - centerDisplacement;
				generatedMesh.GetUnpackedPosition(v2, out position);
				Vector3 vector = position - centerDisplacement;
				generatedMesh.GetUnpackedPosition(v3, out position);
				Vector3 value2 = position - centerDisplacement;
				bool flag = false;
				if ((vector - value).LengthSquared() <= MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
				{
					m_vertexMapping.Union(v, v2);
					flag = true;
				}
				if ((value2 - value).LengthSquared() <= MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
				{
					m_vertexMapping.Union(v, v3);
					flag = true;
				}
				if ((value2 - vector).LengthSquared() <= MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
				{
					m_vertexMapping.Union(v2, v3);
					flag = true;
				}
				if (!flag)
				{
					m_connectionHelper.PreprocessInnerEdge(v, v2);
					m_connectionHelper.PreprocessInnerEdge(v2, v3);
					m_connectionHelper.PreprocessInnerEdge(v3, v);
				}
			}
		}

		private bool RemoveCell(Vector3I cell)
		{
			if (!MyFakes.REMOVE_VOXEL_NAVMESH_CELLS)
			{
				return true;
			}
			if (!m_processedCells.Contains(cell))
			{
				return false;
			}
			if (MyFakes.LOG_NAVMESH_GENERATION)
			{
				MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.DebugLog.LogCellRemoval(this, cell);
			}
			MyVoxelPathfinding.CellId cellId = default(MyVoxelPathfinding.CellId);
			cellId.VoxelMap = m_voxelMap;
			cellId.Pos = cell;
			MyVoxelPathfinding.CellId cellId2 = cellId;
			m_navmeshCoordinator.RemoveVoxelNavmeshLinks(cellId2);
			ulong num = new MyCellCoord(0, cell).PackId64();
			MyIntervalList myIntervalList = m_higherLevelHelper.TryGetTriangleList(num);
			if (myIntervalList != null)
			{
				MyIntervalList.Enumerator enumerator = myIntervalList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					RemoveTerrainTriangle(GetTriangle(current));
				}
				m_higherLevelHelper.ClearCachedCell(num);
			}
			m_processedCells.Remove(ref cell);
			return myIntervalList != null;
		}

		private void RemoveTerrainTriangle(MyNavigationTriangle tri)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = tri.GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			Vector3 posv = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			Vector3 posv2 = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			Vector3 posv3 = vertexEnumerator.Current;
			int edgeIndex = tri.GetEdgeIndex(0);
			int edgeIndex2 = tri.GetEdgeIndex(1);
			int edgeIndex3 = tri.GetEdgeIndex(2);
			int edgeIndex4 = edgeIndex;
			if (!m_connectionHelper.TryRemoveOuterEdge(ref posv, ref posv2, ref edgeIndex4) && base.Mesh.GetEdge(edgeIndex).OtherFace(tri.Index) != -1)
			{
				m_connectionHelper.AddOuterEdgeIndex(ref posv2, ref posv, edgeIndex);
			}
			edgeIndex4 = edgeIndex2;
			if (!m_connectionHelper.TryRemoveOuterEdge(ref posv2, ref posv3, ref edgeIndex4) && base.Mesh.GetEdge(edgeIndex2).OtherFace(tri.Index) != -1)
			{
				m_connectionHelper.AddOuterEdgeIndex(ref posv3, ref posv2, edgeIndex2);
			}
			edgeIndex4 = edgeIndex3;
			if (!m_connectionHelper.TryRemoveOuterEdge(ref posv3, ref posv, ref edgeIndex4) && base.Mesh.GetEdge(edgeIndex3).OtherFace(tri.Index) != -1)
			{
				m_connectionHelper.AddOuterEdgeIndex(ref posv, ref posv3, edgeIndex3);
			}
			RemoveTriangle(tri);
		}

		public void RemoveTriangle(int index)
		{
			MyNavigationTriangle triangle = GetTriangle(index);
			RemoveTerrainTriangle(triangle);
		}

		public bool RemoveOneUnusedCell(List<Vector3D> importantPositions)
		{
			m_tmpCellSet.Clear();
			m_tmpCellSet.Union(m_processedCells);
			bool result = false;
			foreach (Vector3I item in m_tmpCellSet)
			{
				Vector3I geometryCellCoord = item * 1;
				MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out Vector3 localPosition);
				localPosition += new Vector3D(0.5);
				MyVoxelCoordSystems.LocalPositionToWorldPosition(m_voxelMap.PositionLeftBottomCorner, ref localPosition, out Vector3D worldPosition);
				bool flag = true;
				foreach (Vector3D importantPosition in importantPositions)
				{
					if (Vector3D.RectangularDistance(worldPosition, importantPosition) < 50.0)
					{
						flag = false;
						break;
					}
				}
				if (flag && !m_markedForAddition.Contains(item) && RemoveCell(item))
				{
					Vector3I cellPos = item;
					float weight = CalculateCellWeight(importantPositions, cellPos);
					MarkCellForAddition(cellPos, weight);
					result = true;
					break;
				}
			}
			m_tmpCellSet.Clear();
			return result;
		}

		public void RemoveFarHighLevelGroups(List<Vector3D> updatePositions)
		{
			m_higherLevelHelper.RemoveTooFarCells(updatePositions, 200f, m_processedCells);
		}

		public void MarkCellsOnPaths()
		{
			m_primitivesOnPath.Clear();
			m_higherLevel.GetPrimitivesOnPath(ref m_primitivesOnPath);
			m_cellsOnWayCoords.Clear();
			m_higherLevelHelper.GetCellsOfPrimitives(ref m_cellsOnWayCoords, ref m_primitivesOnPath);
			m_cellsOnWay.Clear();
			foreach (ulong cellsOnWayCoord in m_cellsOnWayCoords)
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				myCellCoord.SetUnpack(cellsOnWayCoord);
				Vector3I coordInLod = myCellCoord.CoordInLod;
				m_cellsOnWay.Add(coordInLod);
			}
		}

		[Conditional("DEBUG")]
		public void AddCellDebug(Vector3I cellPos)
		{
			HashSet<Vector3I> adjacentCellPos = new HashSet<Vector3I>();
			AddCell(cellPos, ref adjacentCellPos);
		}

		[Conditional("DEBUG")]
		public void RemoveCellDebug(Vector3I cellPos)
		{
			RemoveCell(cellPos);
		}

		public List<Vector4D> FindPathGlobal(Vector3D start, Vector3D end)
		{
			start = Vector3D.Transform(start, m_voxelMap.PositionComp.WorldMatrixNormalizedInv);
			end = Vector3D.Transform(end, m_voxelMap.PositionComp.WorldMatrixNormalizedInv);
			return FindPath(start, end);
		}

		public List<Vector4D> FindPath(Vector3 start, Vector3 end)
		{
			float closestDistanceSq = float.PositiveInfinity;
			MyNavigationTriangle closestNavigationTriangle = GetClosestNavigationTriangle(ref start, ref closestDistanceSq);
			if (closestNavigationTriangle == null)
			{
				return null;
			}
			closestDistanceSq = float.PositiveInfinity;
			MyNavigationTriangle closestNavigationTriangle2 = GetClosestNavigationTriangle(ref end, ref closestDistanceSq);
			if (closestNavigationTriangle2 == null)
			{
				return null;
			}
			m_debugPos1 = Vector3.Transform(closestNavigationTriangle.Position, m_voxelMap.PositionComp.WorldMatrix);
			m_debugPos2 = Vector3.Transform(closestNavigationTriangle2.Position, m_voxelMap.PositionComp.WorldMatrix);
			m_debugPos3 = Vector3.Transform(start, m_voxelMap.PositionComp.WorldMatrix);
			m_debugPos4 = Vector3.Transform(end, m_voxelMap.PositionComp.WorldMatrix);
			return FindRefinedPath(closestNavigationTriangle, closestNavigationTriangle2, ref start, ref end);
		}

		private MyNavigationTriangle GetClosestNavigationTriangle(ref Vector3 point, ref float closestDistanceSq)
		{
			MyNavigationTriangle result = null;
			Vector3I a = Vector3I.Round((Vector3)(point + (m_voxelMap.PositionComp.GetPosition() - m_voxelMap.PositionLeftBottomCorner)) / m_cellSize);
			for (int i = 0; i < 8; i++)
			{
				Vector3I vector3I = a + m_cornerOffsets[i];
				if (!m_processedCells.Contains(vector3I))
				{
					continue;
				}
				ulong packedCellCoord = new MyCellCoord(0, vector3I).PackId64();
				MyIntervalList myIntervalList = m_higherLevelHelper.TryGetTriangleList(packedCellCoord);
				if (myIntervalList == null)
				{
					continue;
				}
				MyIntervalList.Enumerator enumerator = myIntervalList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					MyNavigationTriangle triangle = GetTriangle(current);
					float num = Vector3.DistanceSquared(triangle.Center, point);
					if (num < closestDistanceSq)
					{
						closestDistanceSq = num;
						result = triangle;
					}
				}
			}
			return result;
		}

		private MyHighLevelPrimitive GetClosestHighLevelPrimitive(ref Vector3 point, ref float closestDistanceSq)
		{
			MyHighLevelPrimitive result = null;
			Vector3 value = point + (m_voxelMap.PositionComp.GetPosition() - m_voxelMap.PositionLeftBottomCorner);
			m_tmpIntList.Clear();
			Vector3I a = Vector3I.Round(value / m_cellSize);
			for (int i = 0; i < 8; i++)
			{
				Vector3I coordInLod = a + m_cornerOffsets[i];
				ulong packedCoord = new MyCellCoord(0, coordInLod).PackId64();
				m_higherLevelHelper.CollectComponents(packedCoord, m_tmpIntList);
			}
			foreach (int tmpInt in m_tmpIntList)
			{
				MyHighLevelPrimitive primitive = m_higherLevel.GetPrimitive(tmpInt);
				if (primitive != null)
				{
					float num = Vector3.DistanceSquared(primitive.Position, point);
					if (num < closestDistanceSq)
					{
						closestDistanceSq = num;
						result = primitive;
					}
				}
			}
			m_tmpIntList.Clear();
			return result;
		}

		public override MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq)
		{
			MatrixD worldMatrixNormalizedInv = m_voxelMap.PositionComp.WorldMatrixNormalizedInv;
			Vector3 point2 = Vector3D.Transform(point, worldMatrixNormalizedInv);
			float closestDistanceSq2 = (float)closestDistanceSq;
			MyNavigationPrimitive myNavigationPrimitive = null;
			myNavigationPrimitive = ((!highLevel) ? ((MyNavigationPrimitive)GetClosestNavigationTriangle(ref point2, ref closestDistanceSq2)) : ((MyNavigationPrimitive)GetClosestHighLevelPrimitive(ref point2, ref closestDistanceSq2)));
			if (myNavigationPrimitive != null)
			{
				closestDistanceSq = closestDistanceSq2;
			}
			return myNavigationPrimitive;
		}

		public override MatrixD GetWorldMatrix()
		{
			return m_voxelMap.WorldMatrix;
		}

		public override Vector3 GlobalToLocal(Vector3D globalPos)
		{
			return Vector3D.Transform(globalPos, m_voxelMap.PositionComp.WorldMatrixNormalizedInv);
		}

		public override Vector3D LocalToGlobal(Vector3 localPos)
		{
			return Vector3D.Transform(localPos, m_voxelMap.WorldMatrix);
		}

		public override MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle)
		{
			return m_higherLevelHelper.GetHighLevelNavigationPrimitive(myNavigationTriangle as MyNavigationTriangle);
		}

		public override IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive)
		{
			return m_higherLevelHelper.GetComponent(highLevelPrimitive);
		}

		[Conditional("DEBUG")]
		private void CheckOuterEdgeConsistency()
		{
			if (DO_CONSISTENCY_CHECKS)
			{
				foreach (MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3> item in new List<MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3>>())
				{
					int edgeIndex = item.Item1.EdgeIndex;
					MyWingedEdgeMesh.Edge edge = base.Mesh.GetEdge(edgeIndex);
					if (item.Item1.FirstPoint)
					{
						edge.GetFaceSuccVertex(-1);
					}
					else
					{
						edge.GetFacePredVertex(-1);
					}
				}
			}
		}

		public override void DebugDraw(ref Matrix drawMatrix)
		{
			if (MyFakes.DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS)
			{
				Vector3 vector = Vector3.TransformNormal(m_cellSize, drawMatrix);
				Vector3 value = Vector3.Transform(m_voxelMap.PositionLeftBottomCorner - m_voxelMap.PositionComp.GetPosition(), drawMatrix);
				foreach (Vector3I processedCell in m_processedCells)
				{
					BoundingBoxD aabb = default(BoundingBoxD);
					aabb.Min = value + vector * (new Vector3(0.0625f) + processedCell);
					aabb.Max = aabb.Min + vector;
					aabb.Inflate(-0.20000000298023224);
					MyRenderProxy.DebugDrawAABB(aabb, Color.Orange, 1f, 1f, depthRead: false);
					MyRenderProxy.DebugDrawText3D(aabb.Center, processedCell.ToString(), Color.Orange, 0.5f, depthRead: false);
				}
			}
			if (MyFakes.DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS)
			{
				Vector3 vector2 = Vector3.TransformNormal(m_cellSize, drawMatrix);
				Vector3 value2 = Vector3.Transform(m_voxelMap.PositionLeftBottomCorner - m_voxelMap.PositionComp.GetPosition(), drawMatrix);
				MyCellCoord myCellCoord = default(MyCellCoord);
				foreach (ulong cellsOnWayCoord in m_cellsOnWayCoords)
				{
					myCellCoord.SetUnpack(cellsOnWayCoord);
					Vector3I coordInLod = myCellCoord.CoordInLod;
					BoundingBoxD aabb2 = default(BoundingBoxD);
					aabb2.Min = value2 + vector2 * (new Vector3(0.0625f) + coordInLod);
					aabb2.Max = aabb2.Min + vector2;
					aabb2.Inflate(-0.30000001192092896);
					MyRenderProxy.DebugDrawAABB(aabb2, Color.Green, 1f, 1f, depthRead: false);
				}
			}
			if (MyFakes.DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS)
			{
				Vector3 vector3 = Vector3.TransformNormal(m_cellSize, drawMatrix);
				Vector3 value3 = Vector3.Transform(m_voxelMap.PositionLeftBottomCorner - m_voxelMap.PositionComp.GetPosition(), drawMatrix);
				float num = float.NegativeInfinity;
				Vector3I other = Vector3I.Zero;
				for (int i = 0; i < m_toAdd.Count; i++)
				{
					CellToAddHeapItem item = m_toAdd.GetItem(i);
					float heapKey = item.HeapKey;
					if (heapKey > num)
					{
						num = heapKey;
						other = item.Position;
					}
				}
				BoundingBoxD aabb3 = default(BoundingBoxD);
				for (int j = 0; j < m_toAdd.Count; j++)
				{
					CellToAddHeapItem item2 = m_toAdd.GetItem(j);
					float heapKey2 = item2.HeapKey;
					Vector3I position = item2.Position;
					aabb3.Min = value3 + vector3 * (new Vector3(0.0625f) + position);
					aabb3.Max = aabb3.Min + vector3;
					aabb3.Inflate(-0.10000000149011612);
					Color color = Color.Aqua;
					if (position.Equals(other))
					{
						color = Color.Red;
					}
					MyRenderProxy.DebugDrawAABB(aabb3, color, 1f, 1f, depthRead: false);
					string text = string.Format("{0}", heapKey2.ToString("n2"));
					MyRenderProxy.DebugDrawText3D(aabb3.Center, text, color, 0.7f, depthRead: false);
				}
			}
			MyRenderProxy.DebugDrawSphere(m_debugPos1, 0.2f, Color.Red, 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(m_debugPos2, 0.2f, Color.Green, 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(m_debugPos3, 0.1f, Color.Red, 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(m_debugPos4, 0.1f, Color.Green, 1f, depthRead: false);
			if (MyFakes.DEBUG_DRAW_VOXEL_CONNECTION_HELPER)
			{
				m_connectionHelper.DebugDraw(ref drawMatrix, base.Mesh);
			}
			if (MyFakes.DEBUG_DRAW_NAVMESH_CELL_BORDERS)
			{
				foreach (KeyValuePair<ulong, List<DebugDrawEdge>> debugCellEdge in m_debugCellEdges)
				{
					foreach (DebugDrawEdge item3 in debugCellEdge.Value)
					{
						MyRenderProxy.DebugDrawLine3D(item3.V1, item3.V2, Color.Orange, Color.Orange, depthRead: false);
					}
				}
			}
			else
			{
				m_debugCellEdges.Clear();
			}
			if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
			{
				if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE)
				{
					m_higherLevel.DebugDraw(lite: true);
				}
				else
				{
					m_higherLevel.DebugDraw(lite: false);
					m_higherLevelHelper.DebugDraw();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != MyWEMDebugDrawMode.LINES || m_voxelMap is MyVoxelPhysics)
			{
				return;
			}
			int num2 = 0;
			MyWingedEdgeMesh.EdgeEnumerator edges = base.Mesh.GetEdges();
			Vector3D position2 = m_voxelMap.PositionComp.GetPosition();
			while (edges.MoveNext())
			{
				Vector3D value4 = base.Mesh.GetVertexPosition(edges.Current.Vertex1) + position2;
				Vector3D value5 = base.Mesh.GetVertexPosition(edges.Current.Vertex2) + position2;
				Vector3D vector3D = (value4 + value5) * 0.5;
				if (MyCestmirPathfindingShorts.Pathfinding.Obstacles.IsInObstacle(vector3D))
				{
					MyRenderProxy.DebugDrawSphere(vector3D, 0.05f, Color.Red, 1f, depthRead: false);
				}
				num2++;
			}
		}
	}
}
