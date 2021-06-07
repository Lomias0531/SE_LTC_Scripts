using Sandbox.Engine.Utils;
using Sandbox.Game.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyVoxelGeometry
	{
		public class CellData
		{
			public int VoxelTrianglesCount;

			public int VoxelVerticesCount;

			public MyVoxelTriangle[] VoxelTriangles;

			private Vector3 m_positionOffset;

			private Vector3 m_positionScale;

			private Vector3[] m_positions;

			private MyOctree m_octree;

			internal MyOctree Octree
			{
				get
				{
					if (m_octree == null && VoxelTrianglesCount > 0)
					{
						m_octree = new MyOctree();
						m_octree.Init(m_positions, VoxelVerticesCount, VoxelTriangles, VoxelTrianglesCount, out VoxelTriangles);
					}
					return m_octree;
				}
			}

			public void Init(Vector3 positionOffset, Vector3 positionScale, Vector3[] positions, int vertexCount, MyVoxelTriangle[] triangles, int triangleCount)
			{
				if (vertexCount == 0)
				{
					VoxelVerticesCount = 0;
					VoxelTrianglesCount = 0;
					m_octree = null;
					m_positions = null;
					return;
				}
				m_positionOffset = positionOffset;
				m_positionScale = positionScale;
				m_positions = new Vector3[vertexCount];
				Array.Copy(positions, m_positions, vertexCount);
				if (m_octree == null)
				{
					m_octree = new MyOctree();
				}
				m_octree.Init(m_positions, vertexCount, triangles, triangleCount, out VoxelTriangles);
				VoxelVerticesCount = vertexCount;
				VoxelTrianglesCount = triangleCount;
			}

			public void GetUnpackedPosition(int index, out Vector3 unpacked)
			{
				unpacked = m_positions[index] * m_positionScale + m_positionOffset;
			}

			public void GetPackedPosition(ref Vector3 unpacked, out Vector3 packed)
			{
				packed = (unpacked - m_positionOffset) / m_positionScale;
			}
		}

		private static List<Vector3I> m_sweepResultCache = new List<Vector3I>();

		private static List<int> m_overlapElementCache = new List<int>();

		private MyStorageBase m_storage;

		private Vector3I m_cellsCount;

		private readonly Dictionary<ulong, CellData> m_cellsByCoordinate = new Dictionary<ulong, CellData>();

		private readonly Dictionary<ulong, MyIsoMesh> m_coordinateToMesh = new Dictionary<ulong, MyIsoMesh>();

		private readonly FastResourceLock m_lock = new FastResourceLock();

		private readonly LRUCache<ulong, ulong> m_isEmptyCache = new LRUCache<ulong, ulong>(128);

		public Vector3I CellsCount => m_cellsCount;

		public void Init(MyStorageBase storage)
		{
			m_storage = storage;
			m_storage.RangeChanged += storage_RangeChanged;
			Vector3I size = m_storage.Size;
			m_cellsCount.X = size.X >> 3;
			m_cellsCount.Y = size.Y >> 3;
			m_cellsCount.Z = size.Z >> 3;
		}

		public bool Intersects(ref BoundingSphere localSphere)
		{
			BoundingBox boundingBox = BoundingBox.CreateInvalid();
			boundingBox.Include(ref localSphere);
			Vector3 localPosition = boundingBox.Min;
			Vector3 localPosition2 = boundingBox.Max;
			MyVoxelCoordSystems.LocalPositionToGeometryCellCoord(ref localPosition, out Vector3I geometryCellCoord);
			MyVoxelCoordSystems.LocalPositionToGeometryCellCoord(ref localPosition2, out Vector3I geometryCellCoord2);
			ClampCellCoord(ref geometryCellCoord);
			ClampCellCoord(ref geometryCellCoord2);
			MyCellCoord cell = default(MyCellCoord);
			cell.CoordInLod.X = geometryCellCoord.X;
			MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
			while (cell.CoordInLod.X <= geometryCellCoord2.X)
			{
				cell.CoordInLod.Y = geometryCellCoord.Y;
				while (cell.CoordInLod.Y <= geometryCellCoord2.Y)
				{
					cell.CoordInLod.Z = geometryCellCoord.Z;
					while (cell.CoordInLod.Z <= geometryCellCoord2.Z)
					{
						MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref cell.CoordInLod, out BoundingBox localAABB);
						if (localAABB.Intersects(ref localSphere))
						{
							CellData cell2 = GetCell(ref cell);
							if (cell2 != null)
							{
								for (int i = 0; i < cell2.VoxelTrianglesCount; i++)
								{
									MyVoxelTriangle myVoxelTriangle = cell2.VoxelTriangles[i];
									cell2.GetUnpackedPosition(myVoxelTriangle.V0, out triangle.Vertex0);
									cell2.GetUnpackedPosition(myVoxelTriangle.V1, out triangle.Vertex1);
									cell2.GetUnpackedPosition(myVoxelTriangle.V2, out triangle.Vertex2);
									BoundingBox boundingBox2 = BoundingBox.CreateInvalid();
									boundingBox2.Include(ref triangle.Vertex0);
									boundingBox2.Include(ref triangle.Vertex1);
									boundingBox2.Include(ref triangle.Vertex2);
									if (boundingBox2.Intersects(ref localSphere))
									{
										Plane trianglePlane = new Plane(triangle.Vertex0, triangle.Vertex1, triangle.Vertex2);
										if (MyUtils.GetSphereTriangleIntersection(ref localSphere, ref trianglePlane, ref triangle).HasValue)
										{
											return true;
										}
									}
								}
							}
						}
						cell.CoordInLod.Z++;
					}
					cell.CoordInLod.Y++;
				}
				cell.CoordInLod.X++;
			}
			return false;
		}

		public bool Intersect(ref Line localLine, out MyIntersectionResultLineTriangle result, IntersectionFlags flags)
		{
			m_sweepResultCache.Clear();
			MyGridIntersection.Calculate(m_sweepResultCache, 8f, localLine.From, localLine.To, new Vector3I(0, 0, 0), m_cellsCount - 1);
			float? minDistanceUntilNow = null;
			MyCellCoord cell = default(MyCellCoord);
			MyIntersectionResultLineTriangle? result2 = null;
			for (int i = 0; i < m_sweepResultCache.Count; i++)
			{
				cell.CoordInLod = m_sweepResultCache[i];
				MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref cell.CoordInLod, out BoundingBox localAABB);
				float? lineBoundingBoxIntersection = MyUtils.GetLineBoundingBoxIntersection(ref localLine, ref localAABB);
				if (minDistanceUntilNow.HasValue && lineBoundingBoxIntersection.HasValue && minDistanceUntilNow + 15.5884562f < lineBoundingBoxIntersection.Value)
				{
					break;
				}
				CellData cell2 = GetCell(ref cell);
				if (cell2 != null && cell2.VoxelTrianglesCount != 0)
				{
					GetCellLineIntersectionOctree(ref result2, ref localLine, ref minDistanceUntilNow, cell2, flags);
				}
			}
			result = (result2 ?? default(MyIntersectionResultLineTriangle));
			return result2.HasValue;
		}

		private bool TryGetCell(MyCellCoord cell, out bool isEmpty, out CellData nonEmptyCell)
		{
			using (m_lock.AcquireSharedUsing())
			{
				if (IsEmpty(ref cell))
				{
					isEmpty = true;
					nonEmptyCell = null;
					return true;
				}
				ulong key = cell.PackId64();
				if (m_cellsByCoordinate.TryGetValue(key, out nonEmptyCell))
				{
					isEmpty = false;
					return true;
				}
				isEmpty = false;
				nonEmptyCell = null;
				return false;
			}
		}

		public bool TryGetMesh(MyCellCoord cell, out bool isEmpty, out MyIsoMesh nonEmptyMesh)
		{
			using (m_lock.AcquireSharedUsing())
			{
				if (IsEmpty(ref cell))
				{
					isEmpty = true;
					nonEmptyMesh = null;
					return true;
				}
				ulong key = cell.PackId64();
				if (m_coordinateToMesh.TryGetValue(key, out nonEmptyMesh))
				{
					isEmpty = false;
					return true;
				}
				isEmpty = false;
				nonEmptyMesh = null;
				return false;
			}
		}

		public void SetMesh(MyCellCoord cell, MyIsoMesh mesh)
		{
			if (cell.Lod == 0)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (mesh != null)
					{
						ulong key = cell.PackId64();
						m_coordinateToMesh[key] = mesh;
					}
					else
					{
						SetEmpty(ref cell, value: true);
					}
				}
			}
		}

		private void storage_RangeChanged(Vector3I minChanged, Vector3I maxChanged, MyStorageDataTypeFlags changedData)
		{
			Vector3I voxelCoord = minChanged - MyPrecalcComponent.InvalidatedRangeInflate;
			Vector3I voxelCoord2 = maxChanged + MyPrecalcComponent.InvalidatedRangeInflate;
			m_storage.ClampVoxelCoord(ref voxelCoord);
			m_storage.ClampVoxelCoord(ref voxelCoord2);
			Vector3I start = voxelCoord >> 3;
			Vector3I end = voxelCoord2 >> 3;
			using (m_lock.AcquireExclusiveUsing())
			{
				if (start == Vector3I.Zero && end == m_cellsCount - 1)
				{
					m_cellsByCoordinate.Clear();
					m_coordinateToMesh.Clear();
					m_isEmptyCache.Reset();
				}
				else
				{
					MyCellCoord cell = default(MyCellCoord);
					if (m_cellsByCoordinate.Count > 0 || m_coordinateToMesh.Count > 0)
					{
						cell.CoordInLod = start;
						Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
						while (vector3I_RangeIterator.IsValid())
						{
							ulong key = cell.PackId64();
							m_cellsByCoordinate.Remove(key);
							m_coordinateToMesh.Remove(key);
							vector3I_RangeIterator.GetNext(out cell.CoordInLod);
						}
					}
					if ((end - start).Volume() > 100000)
					{
						Vector3I start2 = start >> 2;
						Vector3I end2 = end >> 2;
						end2 += 1;
						cell.CoordInLod = start2;
						Vector3I_RangeIterator vector3I_RangeIterator2 = new Vector3I_RangeIterator(ref start2, ref end2);
						while (vector3I_RangeIterator2.IsValid())
						{
							cell.CoordInLod <<= 2;
							RemoveEmpty(ref cell);
							vector3I_RangeIterator2.GetNext(out cell.CoordInLod);
						}
					}
					else
					{
						Vector3I_RangeIterator vector3I_RangeIterator3 = new Vector3I_RangeIterator(ref start, ref end);
						while (vector3I_RangeIterator3.IsValid())
						{
							SetEmpty(ref cell, value: false);
							vector3I_RangeIterator3.GetNext(out cell.CoordInLod);
						}
					}
				}
			}
		}

		private void GetCellLineIntersectionOctree(ref MyIntersectionResultLineTriangle? result, ref Line modelSpaceLine, ref float? minDistanceUntilNow, CellData cachedDataCell, IntersectionFlags flags)
		{
			m_overlapElementCache.Clear();
			if (cachedDataCell.Octree != null)
			{
				cachedDataCell.GetPackedPosition(ref modelSpaceLine.From, out Vector3 packed);
				cachedDataCell.GetPackedPosition(ref modelSpaceLine.To, out Vector3 packed2);
				Ray ray = new Ray(packed, packed2 - packed);
				cachedDataCell.Octree.GetIntersectionWithLine(ref ray, m_overlapElementCache);
			}
			MyTriangle_Vertices inputTriangle = default(MyTriangle_Vertices);
			for (int i = 0; i < m_overlapElementCache.Count; i++)
			{
				int num = m_overlapElementCache[i];
				if (cachedDataCell.VoxelTriangles == null || num >= cachedDataCell.VoxelTriangles.Length)
				{
					continue;
				}
				MyVoxelTriangle myVoxelTriangle = cachedDataCell.VoxelTriangles[num];
				cachedDataCell.GetUnpackedPosition(myVoxelTriangle.V0, out inputTriangle.Vertex0);
				cachedDataCell.GetUnpackedPosition(myVoxelTriangle.V1, out inputTriangle.Vertex1);
				cachedDataCell.GetUnpackedPosition(myVoxelTriangle.V2, out inputTriangle.Vertex2);
				Vector3 triangleNormal = MyUtils.GetNormalVectorFromTriangle(ref inputTriangle);
				if (triangleNormal.IsValid() && ((flags & IntersectionFlags.FLIPPED_TRIANGLES) != 0 || !(Vector3.Dot(modelSpaceLine.Direction, triangleNormal) > 0f)))
				{
					float? lineTriangleIntersection = MyUtils.GetLineTriangleIntersection(ref modelSpaceLine, ref inputTriangle);
					if (lineTriangleIntersection.HasValue && (!result.HasValue || lineTriangleIntersection.Value < result.Value.Distance))
					{
						minDistanceUntilNow = lineTriangleIntersection.Value;
						result = new MyIntersectionResultLineTriangle(0, ref inputTriangle, ref triangleNormal, lineTriangleIntersection.Value);
					}
				}
			}
		}

		private void ClampCellCoord(ref Vector3I cellCoord)
		{
			Vector3I max = m_cellsCount - 1;
			Vector3I.Clamp(ref cellCoord, ref Vector3I.Zero, ref max, out cellCoord);
		}

		internal CellData GetCell(ref MyCellCoord cell)
		{
			if (TryGetCell(cell, out bool isEmpty, out CellData nonEmptyCell))
			{
				return nonEmptyCell;
			}
			if (!TryGetMesh(cell, out isEmpty, out MyIsoMesh nonEmptyMesh))
			{
				Vector3I vector3I = cell.CoordInLod << 3;
				Vector3I lodVoxelMax = vector3I + 8;
				vector3I -= 1;
				lodVoxelMax += 2;
				nonEmptyMesh = MyPrecalcComponent.IsoMesher.Precalc(m_storage, 0, vector3I, lodVoxelMax, MyStorageDataTypeFlags.Content);
			}
			if (nonEmptyMesh != null)
			{
				nonEmptyCell = new CellData();
				nonEmptyCell.Init(nonEmptyMesh.PositionOffset, nonEmptyMesh.PositionScale, nonEmptyMesh.Positions.GetInternalArray(), nonEmptyMesh.VerticesCount, nonEmptyMesh.Triangles.GetInternalArray(), nonEmptyMesh.TrianglesCount);
			}
			if (cell.Lod == 0)
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					if (nonEmptyCell == null)
					{
						SetEmpty(ref cell, value: true);
						return nonEmptyCell;
					}
					ulong key = cell.PackId64();
					m_cellsByCoordinate[key] = nonEmptyCell;
					return nonEmptyCell;
				}
			}
			return nonEmptyCell;
		}

		private void ComputeIsEmptyLookup(MyCellCoord cell, out ulong outCacheKey, out int outBit)
		{
			Vector3I vector3I = cell.CoordInLod % 4;
			cell.CoordInLod >>= 2;
			outCacheKey = cell.PackId64();
			outBit = vector3I.X + 4 * (vector3I.Y + 4 * vector3I.Z);
		}

		private bool IsEmpty(ref MyCellCoord cell)
		{
			ComputeIsEmptyLookup(cell, out ulong outCacheKey, out int outBit);
			return ((long)m_isEmptyCache.Read(outCacheKey) & (1L << outBit)) != 0;
		}

		private void RemoveEmpty(ref MyCellCoord cell)
		{
			ComputeIsEmptyLookup(cell, out ulong outCacheKey, out int _);
			m_isEmptyCache.Remove(outCacheKey);
		}

		private void SetEmpty(ref MyCellCoord cell, bool value)
		{
			ComputeIsEmptyLookup(cell, out ulong outCacheKey, out int outBit);
			ulong num = m_isEmptyCache.Read(outCacheKey);
			num = (ulong)((!value) ? ((long)num & ~(1L << outBit)) : ((long)num | (1L << outBit)));
			m_isEmptyCache.Write(outCacheKey, num);
		}
	}
}
