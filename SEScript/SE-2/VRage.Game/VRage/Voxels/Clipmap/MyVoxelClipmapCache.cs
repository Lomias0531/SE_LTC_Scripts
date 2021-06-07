using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage.Library.Collections;
using VRage.Utils;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Clipmap
{
	public class MyVoxelClipmapCache
	{
		private struct CellKey
		{
			private sealed class ClipmapIdCoordEqualityComparer : IEqualityComparer<CellKey>
			{
				public bool Equals(CellKey x, CellKey y)
				{
					if (x.ClipmapId == y.ClipmapId)
					{
						return x.Coord.Equals(y.Coord);
					}
					return false;
				}

				public int GetHashCode(CellKey obj)
				{
					uint num = obj.ClipmapId * 397;
					MyCellCoord coord = obj.Coord;
					return (int)num ^ coord.GetHashCode();
				}
			}

			public readonly uint ClipmapId;

			public readonly MyCellCoord Coord;

			private static readonly IEqualityComparer<CellKey> ComparerInstance = new ClipmapIdCoordEqualityComparer();

			public static IEqualityComparer<CellKey> Comparer => ComparerInstance;

			public CellKey(uint clipmapId, MyCellCoord cell)
			{
				ClipmapId = clipmapId;
				Coord = cell;
			}
		}

		private struct CellData
		{
			public VrSewGuide Guide;

			public CellData(VrSewGuide guide)
			{
				Guide = guide;
			}
		}

		/// <summary>
		/// Struct used to store the range data when invalidating cells by iterating the cache.
		/// </summary>
		private struct EvictionRanges
		{
			private const int StructSize = 24;

			private unsafe fixed byte m_data[384];

			public unsafe BoundingBoxI* Ranges
			{
				get
				{
					fixed (byte* result = m_data)
					{
						return (BoundingBoxI*)result;
					}
				}
			}
		}

		/// <summary>
		/// Size of the cell cache.
		/// </summary>
		public static int DefaultCacheSize = 1024;

		private static MyVoxelClipmapCache m_instance;

		/// <summary>
		/// Actual cache.
		/// </summary>
		private readonly LRUCache<CellKey, CellData> m_cells;

		/// <summary>
		/// Info about each clipmap in the cache.
		/// </summary>
		private readonly ConcurrentDictionary<uint, MyVoxelClipmap> m_evictionHandlers = new ConcurrentDictionary<uint, MyVoxelClipmap>();

		/// <summary>
		/// Maximum lod for which cells are cached.
		/// </summary>
		private int m_lodThreshold;

		private MyDebugHitCounter m_hitCounter = new MyDebugHitCounter();

		private long m_hits;

		private long m_tries;

		/// <summary>
		/// Default cache instance used when none is provided to the clipmap.
		/// </summary>
		public static MyVoxelClipmapCache Instance => m_instance ?? (m_instance = new MyVoxelClipmapCache(DefaultCacheSize));

		/// <summary>
		/// Maximum lod for which cells are cached.
		/// </summary>
		public int LodThreshold
		{
			get
			{
				return m_lodThreshold;
			}
			set
			{
				m_lodThreshold = MathHelper.Clamp(value, 0, 15);
			}
		}

		/// <summary>
		/// Utilization ratio of the cell cache.
		/// </summary>
		public float CacheUtilization => m_cells.Usage;

		/// <summary>
		/// Rate of cache hits.
		/// </summary>
		public float HitRate
		{
			get
			{
				float[] array = (from x in m_hitCounter
					select x.Value into x
					where !float.IsNaN(x)
					select x).ToArray();
				if (array.Length == 0)
				{
					return 0f;
				}
				return array.Average();
			}
		}

		/// <summary>
		/// Internal cache hit counter.
		///
		/// This is only for debug purposes and may not be accurate if multiple clipmaps share this cache.
		/// </summary>
		public MyDebugHitCounter DebugHitCounter => m_hitCounter;

		public MyVoxelClipmapCache(int maxCachedCells, int lodThreshold = 6)
		{
			m_lodThreshold = lodThreshold;
			m_cells = new LRUCache<CellKey, CellData>(maxCachedCells, CellKey.Comparer);
			LRUCache<CellKey, CellData> cells = m_cells;
			cells.OnItemDiscarded = (Action<CellKey, CellData>)Delegate.Combine(cells.OnItemDiscarded, (Action<CellKey, CellData>)delegate(CellKey key, CellData cell)
			{
				if (cell.Guide != null)
				{
					cell.Guide.RemoveReference();
					m_evictionHandlers[key.ClipmapId].HandleCacheEviction(key.Coord, cell.Guide);
				}
			});
		}

		/// <summary>
		/// Register a clipmap with this cache.
		/// </summary>
		/// <param name="clipmapId">The id of the clipmap.</param>
		/// <param name="clipmap">The clipmap object.</param>
		public void Register(uint clipmapId, MyVoxelClipmap clipmap)
		{
			m_evictionHandlers.TryAdd(clipmapId, clipmap);
		}

		/// <summary>
		/// Unregister a clipmap with this cache.
		///
		/// This automatically evicts all cells cached for that clipmap.
		/// </summary>
		/// <param name="clipmapId"></param>
		public void Unregister(uint clipmapId)
		{
			EvictAll(clipmapId);
			m_evictionHandlers.Remove(clipmapId);
		}

		/// <summary>
		/// Evict all cached cells belonging to the provided clipmap id.
		/// </summary>
		/// <param name="clipmapId"></param>
		public void EvictAll(uint clipmapId)
		{
			if (!m_evictionHandlers.ContainsKey(clipmapId))
			{
				throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
			}
			m_cells.RemoveWhere((CellKey k, CellData v) => k.ClipmapId == clipmapId);
		}

		/// <summary>
		/// Evict all cells from the provided clipmap in the given cell range.
		/// </summary>
		/// <param name="clipmapId">The clipmap owning the evicted cells.</param>
		/// <param name="range">The range of cells to be evicted.</param>
		public unsafe void EvictAll(uint clipmapId, BoundingBoxI range)
		{
			if (!m_evictionHandlers.ContainsKey(clipmapId))
			{
				throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
			}
			if (range.Size.Size > m_cells.Count)
			{
				EvictionRanges evictionRanges = default(EvictionRanges);
				BoundingBoxI* ranges = evictionRanges.Ranges;
				for (int i = 0; i <= LodThreshold; i++)
				{
					ranges[i] = new BoundingBoxI(range.Min >> i, range.Max + ((1 << i) - 1) >> i);
				}
				m_cells.RemoveWhere((CellKey k, CellData v) => k.ClipmapId == clipmapId && ranges[k.Coord.Lod].Contains(k.Coord.CoordInLod) == ContainmentType.Contains);
			}
			else
			{
				for (int j = 0; j <= m_lodThreshold; j++)
				{
					foreach (Vector3I item in BoundingBoxI.EnumeratePoints(new BoundingBoxI(range.Min >> j, range.Max + ((1 << j) - 1) >> j)))
					{
						CellKey key = new CellKey(clipmapId, new MyCellCoord(j, item));
						m_cells.Remove(key);
					}
				}
			}
		}

		/// <summary>
		/// Try to retrieve cached cell data given it's id.
		/// </summary>
		/// <param name="clipmapId">Clipmap owning the cell.</param>
		/// <param name="cell">The cell coordinate.</param>
		/// <param name="data">The cached cell data if any.</param>
		/// <returns>Whether the cell was found in the cache or not.</returns>
		public bool TryRead(uint clipmapId, MyCellCoord cell, out VrSewGuide data)
		{
			if (!m_evictionHandlers.ContainsKey(clipmapId))
			{
				throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
			}
			if (cell.Lod > m_lodThreshold)
			{
				data = null;
				return false;
			}
			CellKey key = new CellKey(clipmapId, cell);
			if (m_cells.TryRead(key, out CellData value))
			{
				data = value.Guide;
				return true;
			}
			data = null;
			return false;
		}

		/// <summary>
		/// Whether a cell is cached.
		/// </summary>
		/// <param name="clipmapId"></param>
		/// <param name="cell"></param>
		/// <param name="dataGuide"></param>
		/// <returns></returns>
		public bool IsCached(uint clipmapId, MyCellCoord cell, VrSewGuide dataGuide)
		{
			if (!m_evictionHandlers.ContainsKey(clipmapId))
			{
				throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
			}
			if (cell.Lod > m_lodThreshold)
			{
				return false;
			}
			CellKey key = new CellKey(clipmapId, cell);
			if (m_cells.TryPeek(key, out CellData value))
			{
				return value.Guide == dataGuide;
			}
			return false;
		}

		/// <summary>
		/// Record mesh data for a cell into the cache, evicting the oldest entry if necessary.
		/// </summary>
		/// <param name="clipmapId"></param>
		/// <param name="cell"></param>
		/// <param name="guide"></param>
		public void Write(uint clipmapId, MyCellCoord cell, VrSewGuide guide)
		{
			if (!m_evictionHandlers.ContainsKey(clipmapId))
			{
				throw new ArgumentException("The provided clipmap id does not correspond to any registered handler.");
			}
			if (cell.Lod <= m_lodThreshold)
			{
				guide.AddReference();
				CellKey key = new CellKey(clipmapId, cell);
				CellData value = new CellData(guide);
				m_cells.Write(key, value);
			}
		}

		/// <summary>
		/// Cycle the internal hit counter.
		///
		/// This is only for debug purposes and may not be accurate if multiple clipmaps share this cache.
		/// </summary>
		[Conditional("DEBUG")]
		internal void CycleDebugCounters()
		{
			m_hitCounter.Cycle();
		}

		[Conditional("DEBUG")]
		private void Hit()
		{
			Interlocked.Increment(ref m_hits);
			Interlocked.Increment(ref m_tries);
		}

		[Conditional("DEBUG")]
		private void Miss()
		{
			Interlocked.Increment(ref m_tries);
		}
	}
}
