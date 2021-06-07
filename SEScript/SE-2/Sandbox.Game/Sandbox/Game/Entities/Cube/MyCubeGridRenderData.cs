using Sandbox.Game.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Threading;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeGridRenderData
	{
		private struct MyDecalPartIdentity
		{
			public uint DecalId;

			public int CubePartIndex;
		}

		public const int SplitCellCubeCount = 30;

		private const int MAX_DECALS_PER_CUBE = 30;

		private ConcurrentDictionary<Vector3I, ConcurrentQueue<MyDecalPartIdentity>> m_cubeDecals = new ConcurrentDictionary<Vector3I, ConcurrentQueue<MyDecalPartIdentity>>();

		private Vector3 m_basePos;

		private readonly ConcurrentDictionary<Vector3I, MyCubeGridRenderCell> m_cells = new ConcurrentDictionary<Vector3I, MyCubeGridRenderCell>();

		private readonly object m_cellLock = new object();

		private readonly object m_cellsUpdateLock = new object();

		private MyConcurrentHashSet<MyCubeGridRenderCell> m_dirtyCells = new MyConcurrentHashSet<MyCubeGridRenderCell>();

		private MyRenderComponentCubeGrid m_gridRender;

		[ThreadStatic]
		private static List<MyCubeGridRenderCell> m_dirtyCellsBuffer;

		public bool HasDirtyCells => m_dirtyCells.Count > 0;

		public ConcurrentDictionary<Vector3I, MyCubeGridRenderCell> Cells => m_cells;

		public MyCubeGridRenderData(MyRenderComponentCubeGrid grid)
		{
			m_gridRender = grid;
		}

		public void AddCubePart(MyCubePart part)
		{
			Vector3 translation = part.InstanceData.Translation;
			MyCubeGridRenderCell orAddCell = GetOrAddCell(translation);
			orAddCell.AddCubePart(part);
			m_dirtyCells.Add(orAddCell);
		}

		public void RemoveCubePart(MyCubePart part)
		{
			Vector3 pos = part.InstanceData.Translation;
			MyCubeGridRenderCell orAddCell = GetOrAddCell(ref pos, create: false);
			if (orAddCell != null && orAddCell.RemoveCubePart(part))
			{
				m_dirtyCells.Add(orAddCell);
			}
		}

		private long CalculateEdgeHash(Vector3 point0, Vector3 point1)
		{
			long hash = point0.GetHash();
			long hash2 = point1.GetHash();
			return hash * hash2;
		}

		public void AddEdgeInfo(ref Vector3 point0, ref Vector3 point1, ref Vector3 normal0, ref Vector3 normal1, Color color, MySlimBlock owner)
		{
			long hash = CalculateEdgeHash(point0, point1);
			Vector3 pos = (point0 + point1) * 0.5f;
			Vector3I edgeDirection = Vector3I.Round((point0 - point1) / m_gridRender.GridSize);
			MyEdgeInfo info = new MyEdgeInfo(ref pos, ref edgeDirection, ref normal0, ref normal1, ref color, MyStringHash.GetOrCompute(owner.BlockDefinition.EdgeType));
			MyCubeGridRenderCell orAddCell = GetOrAddCell(pos);
			if (orAddCell.AddEdgeInfo(hash, info, owner))
			{
				m_dirtyCells.Add(orAddCell);
			}
		}

		public void RemoveEdgeInfo(Vector3 point0, Vector3 point1, MySlimBlock owner)
		{
			long hash = CalculateEdgeHash(point0, point1);
			Vector3 pos = (point0 + point1) * 0.5f;
			MyCubeGridRenderCell orAddCell = GetOrAddCell(pos);
			if (orAddCell.RemoveEdgeInfo(hash, owner))
			{
				m_dirtyCells.Add(orAddCell);
			}
		}

		public void RebuildDirtyCells(RenderFlags renderFlags)
		{
			using (MyUtils.ReuseCollection(ref m_dirtyCellsBuffer))
			{
				using (ConcurrentEnumerator<SpinLockRef.Token, MyCubeGridRenderCell, HashSet<MyCubeGridRenderCell>.Enumerator> concurrentEnumerator = m_dirtyCells.GetEnumerator())
				{
					while (concurrentEnumerator.MoveNext())
					{
						m_dirtyCellsBuffer.Add(concurrentEnumerator.Current);
					}
					m_dirtyCells.Clear();
				}
				lock (m_cellsUpdateLock)
				{
					foreach (MyCubeGridRenderCell item in m_dirtyCellsBuffer)
					{
						item.RebuildInstanceParts(renderFlags);
					}
				}
			}
		}

		public void OnRemovedFromRender()
		{
			IMyEntity entity = m_gridRender.Entity;
			bool flag = entity != null && !entity.MarkedForClose;
			lock (m_cellsUpdateLock)
			{
				foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in m_cells)
				{
					MyCubeGridRenderCell value = cell.Value;
					value.OnRemovedFromRender();
					if (flag)
					{
						m_dirtyCells.Add(value);
					}
				}
			}
		}

		public void SetBasePositionHint(Vector3 basePos)
		{
			if (m_cells.Count == 0)
			{
				m_basePos = basePos;
			}
		}

		internal MyCubeGridRenderCell GetOrAddCell(Vector3 pos, bool create = true)
		{
			return GetOrAddCell(ref pos, create);
		}

		internal MyCubeGridRenderCell GetOrAddCell(ref Vector3 pos, bool create = true)
		{
			Vector3 v = (pos - m_basePos) / (30f * m_gridRender.GridSize);
			Vector3I.Round(ref v, out Vector3I r);
			if (m_cells.TryGetValue(r, out MyCubeGridRenderCell value) || !create)
			{
				return value;
			}
			lock (m_cellLock)
			{
				value = new MyCubeGridRenderCell(m_gridRender)
				{
					DebugName = r.ToString()
				};
				m_cells.TryAdd(r, value);
				return value;
			}
		}

		public void AddDecal(Vector3I position, MyCubeGrid.MyCubeGridHitInfo gridHitInfo, uint decalId)
		{
			if (m_gridRender.CubeGrid.TryGetCube(position, out MyCube cube))
			{
				if (gridHitInfo.CubePartIndex != -1)
				{
					MyCubePart myCubePart = cube.Parts[gridHitInfo.CubePartIndex];
					GetOrAddCell(myCubePart.InstanceData.Translation).AddCubePartDecal(myCubePart, decalId);
				}
				ConcurrentQueue<MyDecalPartIdentity> orAdd = m_cubeDecals.GetOrAdd(position, (Vector3I x) => new ConcurrentQueue<MyDecalPartIdentity>());
				if (orAdd.Count > 30)
				{
					RemoveDecal(position, orAdd, cube);
				}
				orAdd.Enqueue(new MyDecalPartIdentity
				{
					DecalId = decalId,
					CubePartIndex = gridHitInfo.CubePartIndex
				});
			}
		}

		public void RemoveDecals(Vector3I position)
		{
			if (m_cubeDecals.TryGetValue(position, out ConcurrentQueue<MyDecalPartIdentity> value))
			{
				m_gridRender.CubeGrid.TryGetCube(position, out MyCube cube);
				while (!value.IsEmpty)
				{
					RemoveDecal(position, value, cube);
				}
			}
		}

		private void RemoveDecal(Vector3I position, ConcurrentQueue<MyDecalPartIdentity> decals, MyCube cube)
		{
			decals.TryDequeue(out MyDecalPartIdentity result);
			MyDecals.RemoveDecal(result.DecalId);
			if (result.CubePartIndex != -1)
			{
				MyCubePart part = cube.Parts[result.CubePartIndex];
				GetOrAddCell(position).RemoveCubePartDecal(part, result.DecalId);
			}
		}

		internal void DebugDraw()
		{
			foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in m_cells)
			{
				cell.Value.DebugDraw();
			}
		}
	}
}
