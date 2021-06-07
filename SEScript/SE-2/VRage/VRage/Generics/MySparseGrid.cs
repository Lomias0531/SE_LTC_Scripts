using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Collections;
using VRageMath;

namespace VRage.Generics
{
	public class MySparseGrid<TItemData, TCellData> : IDictionary<Vector3I, TItemData>, ICollection<KeyValuePair<Vector3I, TItemData>>, IEnumerable<KeyValuePair<Vector3I, TItemData>>, IEnumerable
	{
		public class Cell
		{
			internal Dictionary<Vector3I, TItemData> m_items = new Dictionary<Vector3I, TItemData>();

			public TCellData CellData;

			public DictionaryReader<Vector3I, TItemData> Items => new DictionaryReader<Vector3I, TItemData>(m_items);
		}

		private int m_itemCount;

		private Dictionary<Vector3I, Cell> m_cells = new Dictionary<Vector3I, Cell>();

		private HashSet<Vector3I> m_dirtyCells = new HashSet<Vector3I>();

		public readonly int CellSize;

		public DictionaryReader<Vector3I, Cell> Cells => new DictionaryReader<Vector3I, Cell>(m_cells);

		public HashSetReader<Vector3I> DirtyCells => m_dirtyCells;

		public int ItemCount => m_itemCount;

		public int CellCount => m_cells.Count;

		ICollection<Vector3I> IDictionary<Vector3I, TItemData>.Keys
		{
			get
			{
				throw new InvalidOperationException("Operation not supported");
			}
		}

		ICollection<TItemData> IDictionary<Vector3I, TItemData>.Values
		{
			get
			{
				throw new InvalidOperationException("Operation not supported");
			}
		}

		TItemData IDictionary<Vector3I, TItemData>.this[Vector3I key]
		{
			get
			{
				return Get(key);
			}
			set
			{
				Remove(key);
				Add(key, value);
			}
		}

		int ICollection<KeyValuePair<Vector3I, TItemData>>.Count => m_itemCount;

		bool ICollection<KeyValuePair<Vector3I, TItemData>>.IsReadOnly => false;

		public MySparseGrid(int cellSize)
		{
			CellSize = cellSize;
		}

		public Vector3I Add(Vector3I pos, TItemData data)
		{
			Vector3I vector3I = pos / CellSize;
			GetCell(vector3I, createIfNotExists: true).m_items.Add(pos, data);
			MarkDirty(vector3I);
			m_itemCount++;
			return vector3I;
		}

		public bool Contains(Vector3I pos)
		{
			return GetCell(pos / CellSize, createIfNotExists: false)?.m_items.ContainsKey(pos) ?? false;
		}

		public bool Remove(Vector3I pos, bool removeEmptyCell = true)
		{
			Vector3I vector3I = pos / CellSize;
			Cell cell = GetCell(vector3I, createIfNotExists: false);
			if (cell != null && cell.m_items.Remove(pos))
			{
				MarkDirty(vector3I);
				m_itemCount--;
				if (removeEmptyCell && cell.m_items.Count == 0)
				{
					m_cells.Remove(vector3I);
				}
				return true;
			}
			return false;
		}

		public void Clear()
		{
			m_cells.Clear();
			m_itemCount = 0;
		}

		/// <summary>
		/// Clears cells, but keep them preallocated
		/// </summary>
		public void ClearCells()
		{
			foreach (KeyValuePair<Vector3I, Cell> cell in m_cells)
			{
				cell.Value.m_items.Clear();
			}
			m_itemCount = 0;
		}

		public TItemData Get(Vector3I pos)
		{
			return GetCell(pos / CellSize, createIfNotExists: false).m_items[pos];
		}

		public bool TryGet(Vector3I pos, out TItemData data)
		{
			Cell cell = GetCell(pos / CellSize, createIfNotExists: false);
			if (cell != null)
			{
				return cell.m_items.TryGetValue(pos, out data);
			}
			data = default(TItemData);
			return false;
		}

		public Cell GetCell(Vector3I cell)
		{
			return m_cells[cell];
		}

		public bool TryGetCell(Vector3I cell, out Cell result)
		{
			return m_cells.TryGetValue(cell, out result);
		}

		private Cell GetCell(Vector3I cell, bool createIfNotExists)
		{
			if (!m_cells.TryGetValue(cell, out Cell value) && createIfNotExists)
			{
				value = new Cell();
				m_cells[cell] = value;
			}
			return value;
		}

		public bool IsDirty(Vector3I cell)
		{
			return m_dirtyCells.Contains(cell);
		}

		public void MarkDirty(Vector3I cell)
		{
			m_dirtyCells.Add(cell);
		}

		public void UnmarkDirty(Vector3I cell)
		{
			m_dirtyCells.Remove(cell);
		}

		public void UnmarkDirtyAll()
		{
			m_dirtyCells.Clear();
		}

		public Dictionary<Vector3I, Cell>.Enumerator GetEnumerator()
		{
			return m_cells.GetEnumerator();
		}

		void IDictionary<Vector3I, TItemData>.Add(Vector3I key, TItemData value)
		{
			Add(key, value);
		}

		bool IDictionary<Vector3I, TItemData>.ContainsKey(Vector3I key)
		{
			return Contains(key);
		}

		bool IDictionary<Vector3I, TItemData>.Remove(Vector3I key)
		{
			return Remove(key);
		}

		bool IDictionary<Vector3I, TItemData>.TryGetValue(Vector3I key, out TItemData value)
		{
			return TryGet(key, out value);
		}

		void ICollection<KeyValuePair<Vector3I, TItemData>>.Add(KeyValuePair<Vector3I, TItemData> item)
		{
			Add(item.Key, item.Value);
		}

		void ICollection<KeyValuePair<Vector3I, TItemData>>.Clear()
		{
			Clear();
		}

		bool ICollection<KeyValuePair<Vector3I, TItemData>>.Contains(KeyValuePair<Vector3I, TItemData> item)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		void ICollection<KeyValuePair<Vector3I, TItemData>>.CopyTo(KeyValuePair<Vector3I, TItemData>[] array, int arrayIndex)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		bool ICollection<KeyValuePair<Vector3I, TItemData>>.Remove(KeyValuePair<Vector3I, TItemData> item)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		IEnumerator<KeyValuePair<Vector3I, TItemData>> IEnumerable<KeyValuePair<Vector3I, TItemData>>.GetEnumerator()
		{
			throw new InvalidOperationException("Operation not supported");
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new InvalidOperationException("Operation not supported");
		}
	}
}
