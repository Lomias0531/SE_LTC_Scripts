using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;

namespace VRageMath.Spatial
{
	public class MyVector3Grid<T>
	{
		private struct Entry
		{
			public Vector3 Point;

			public T Data;

			public int NextEntry;

			public override string ToString()
			{
				return Point.ToString() + ", -> " + NextEntry + ", Data: " + Data.ToString();
			}
		}

		public struct SphereQuery
		{
			private MyVector3Grid<T> m_parent;

			private Vector3 m_point;

			private float m_distSq;

			private int m_previousIndex;

			private int m_storageIndex;

			private Vector3I_RangeIterator m_rangeIterator;

			public T Current => m_parent.m_storage[m_storageIndex].Data;

			public Vector3I CurrentBin => m_rangeIterator.Current;

			public int PreviousIndex => m_previousIndex;

			public int StorageIndex => m_storageIndex;

			public SphereQuery(MyVector3Grid<T> parent, ref Vector3 point, float dist)
			{
				m_parent = parent;
				m_point = point;
				m_distSq = dist * dist;
				Vector3 value = new Vector3(dist);
				Vector3I start = m_parent.GetBinIndex(point - value);
				Vector3I end = m_parent.GetBinIndex(point + value);
				m_rangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				m_previousIndex = -1;
				m_storageIndex = -1;
			}

			/// <summary>
			/// Removes the current entry and returns true whether there is another entry.
			/// May invalidate indices and queries in the same bin.
			/// To remove values from more queries while ensuring their validity use MyVector3Grid.RemoveTwo(...).
			/// </summary>
			public bool RemoveCurrent()
			{
				m_storageIndex = m_parent.RemoveEntry(m_storageIndex);
				if (m_previousIndex == -1)
				{
					if (m_storageIndex == -1)
					{
						m_parent.m_bins.Remove(m_rangeIterator.Current);
					}
					else
					{
						m_parent.m_bins[m_rangeIterator.Current] = m_storageIndex;
					}
				}
				else
				{
					Entry value = m_parent.m_storage[m_previousIndex];
					value.NextEntry = m_storageIndex;
					m_parent.m_storage[m_previousIndex] = value;
				}
				return FindFirstAcceptableEntry();
			}

			public bool MoveNext()
			{
				if (m_storageIndex == -1)
				{
					if (!FindNextNonemptyBin())
					{
						return false;
					}
				}
				else
				{
					m_previousIndex = m_storageIndex;
					m_storageIndex = m_parent.m_storage[m_storageIndex].NextEntry;
				}
				return FindFirstAcceptableEntry();
			}

			private bool FindFirstAcceptableEntry()
			{
				while (true)
				{
					if (m_storageIndex != -1)
					{
						Entry entry = m_parent.m_storage[m_storageIndex];
						if ((entry.Point - m_point).LengthSquared() < m_distSq)
						{
							return true;
						}
						m_previousIndex = m_storageIndex;
						m_storageIndex = entry.NextEntry;
					}
					else
					{
						m_rangeIterator.MoveNext();
						if (!FindNextNonemptyBin())
						{
							break;
						}
					}
				}
				return false;
			}

			private bool FindNextNonemptyBin()
			{
				m_previousIndex = -1;
				if (!m_rangeIterator.IsValid())
				{
					return false;
				}
				Vector3I next = m_rangeIterator.Current;
				while (!m_parent.m_bins.TryGetValue(next, out m_storageIndex))
				{
					m_rangeIterator.GetNext(out next);
					if (!m_rangeIterator.IsValid())
					{
						return false;
					}
				}
				return true;
			}
		}

		private float m_cellSize;

		private float m_divisor;

		private int m_nextFreeEntry;

		private int m_count;

		private MyList<Entry> m_storage;

		private Dictionary<Vector3I, int> m_bins;

		private IEqualityComparer<T> m_equalityComparer;

		public int Count => m_count;

		public int InvalidIndex => -1;

		public MyVector3Grid(float cellSize)
			: this(cellSize, (IEqualityComparer<T>)EqualityComparer<T>.Default)
		{
		}

		public MyVector3Grid(float cellSize, IEqualityComparer<T> comparer)
		{
			m_cellSize = cellSize;
			m_divisor = 1f / m_cellSize;
			m_storage = new MyList<Entry>();
			m_bins = new Dictionary<Vector3I, int>();
			m_equalityComparer = comparer;
			Clear();
		}

		public void Clear()
		{
			m_nextFreeEntry = 0;
			m_count = 0;
			m_storage.Clear();
			m_bins.Clear();
		}

		/// <summary>
		/// Clears the storage faster than clear. Only use for value type T
		/// </summary>
		public void ClearFast()
		{
			m_nextFreeEntry = 0;
			m_count = 0;
			m_storage.ClearFast();
			m_bins.Clear();
		}

		public void AddPoint(ref Vector3 point, T data)
		{
			Vector3I binIndex = GetBinIndex(ref point);
			if (!m_bins.TryGetValue(binIndex, out int value))
			{
				int value2 = AddNewEntry(ref point, data);
				m_bins.Add(binIndex, value2);
				return;
			}
			Entry value3 = m_storage[value];
			for (int nextEntry = value3.NextEntry; nextEntry != InvalidIndex; nextEntry = value3.NextEntry)
			{
				value = nextEntry;
				value3 = m_storage[value];
			}
			int num = value3.NextEntry = AddNewEntry(ref point, data);
			m_storage[value] = value3;
		}

		public void RemovePoint(ref Vector3 point)
		{
			Vector3I binIndex = GetBinIndex(ref point);
			if (!m_bins.TryGetValue(binIndex, out int value))
			{
				return;
			}
			int index = InvalidIndex;
			int num = value;
			Entry value2 = default(Entry);
			while (value != InvalidIndex)
			{
				Entry entry = m_storage[value];
				if (entry.Point == point)
				{
					int num2 = RemoveEntry(value);
					if (num == value)
					{
						num = num2;
					}
					else
					{
						value2.NextEntry = num2;
						m_storage[index] = value2;
					}
					value = num2;
				}
				else
				{
					index = value;
					value2 = entry;
					value = entry.NextEntry;
				}
			}
			if (num == InvalidIndex)
			{
				m_bins.Remove(binIndex);
			}
			else
			{
				m_bins[binIndex] = num;
			}
		}

		public void MovePoint(int index, ref Vector3 newPosition)
		{
			Entry value = m_storage[index];
			Vector3I binIndex = GetBinIndex(m_storage[index].Point);
			Vector3I binIndex2 = GetBinIndex(ref newPosition);
			if (binIndex == binIndex2)
			{
				value.Point = newPosition;
				m_storage[index] = value;
				return;
			}
			int num = m_bins[binIndex];
			if (index == num)
			{
				int num2 = RemoveEntry(index);
				if (num2 == InvalidIndex)
				{
					m_bins.Remove(binIndex);
				}
				else
				{
					m_bins[binIndex] = num2;
				}
			}
			else
			{
				int num3 = num;
				while (num3 != InvalidIndex)
				{
					Entry value2 = m_storage[num3];
					int nextEntry = value2.NextEntry;
					if (nextEntry == index)
					{
						value2.NextEntry = RemoveEntry(index);
						m_storage[num3] = value2;
						break;
					}
					num3 = nextEntry;
				}
			}
			AddPoint(ref newPosition, value.Data);
		}

		/// <summary>
		/// Returns the index of the point containing the given data on the given position
		/// The index is only valid as long as the grid does not change!
		/// </summary>
		public int FindPointIndex(ref Vector3 point, T data)
		{
			Vector3I binIndex = GetBinIndex(ref point);
			int value = InvalidIndex;
			m_bins.TryGetValue(binIndex, out value);
			while (value != InvalidIndex)
			{
				Entry entry = m_storage[value];
				if (entry.Point == point && m_equalityComparer.Equals(entry.Data, data))
				{
					return value;
				}
				value = entry.NextEntry;
			}
			return value;
		}

		/// <summary>
		/// Returns the data stored at the given index
		/// </summary>
		public T GetData(int index)
		{
			return m_storage[index].Data;
		}

		/// <summary>
		/// Returns the point at which the data is stored at the given index
		/// </summary>
		public Vector3 GetPoint(int index)
		{
			return m_storage[index].Point;
		}

		/// <summary>
		/// Returns a query for iterating over points inside a sphere of radius dist around the given point
		/// </summary>
		public SphereQuery QueryPointsSphere(ref Vector3 point, float dist)
		{
			return new SphereQuery(this, ref point, dist);
		}

		/// <summary>
		/// Removes values pointed at by en0 and en1 and ensures that the queries both stay consistent
		/// </summary>
		public void RemoveTwo(ref SphereQuery en0, ref SphereQuery en1)
		{
			if (en0.CurrentBin == en1.CurrentBin)
			{
				if (en0.StorageIndex == en1.PreviousIndex)
				{
					en1.RemoveCurrent();
					en0.RemoveCurrent();
					en1 = en0;
				}
				else if (en1.StorageIndex == en0.PreviousIndex)
				{
					en0.RemoveCurrent();
					en1.RemoveCurrent();
					en0 = en1;
				}
				else if (en0.StorageIndex == en1.StorageIndex)
				{
					en0.RemoveCurrent();
					en1 = en0;
				}
				else
				{
					en0.RemoveCurrent();
					en1.RemoveCurrent();
				}
			}
			else
			{
				en0.RemoveCurrent();
				en1.RemoveCurrent();
			}
		}

		public Dictionary<Vector3I, int>.Enumerator EnumerateBins()
		{
			return m_bins.GetEnumerator();
		}

		public int GetNextBinIndex(int currentIndex)
		{
			if (currentIndex == InvalidIndex)
			{
				return InvalidIndex;
			}
			return m_storage[currentIndex].NextEntry;
		}

		public void GetLocalBinBB(ref Vector3I binPosition, out BoundingBoxD output)
		{
			output.Min = binPosition * m_cellSize;
			output.Max = output.Min + new Vector3(m_cellSize);
		}

		public void CollectStorage(int startingIndex, ref List<T> output)
		{
			output.Clear();
			Entry entry = m_storage[startingIndex];
			output.Add(entry.Data);
			while (entry.NextEntry != InvalidIndex)
			{
				entry = m_storage[entry.NextEntry];
				output.Add(entry.Data);
			}
		}

		public void CollectEntireStorage(List<T> output)
		{
			output.Clear();
			foreach (KeyValuePair<Vector3I, int> bin in m_bins)
			{
				int num = bin.Value;
				do
				{
					Entry entry = m_storage[num];
					output.Add(entry.Data);
					num = entry.NextEntry;
				}
				while (num != InvalidIndex);
			}
		}

		private Vector3I GetBinIndex(ref Vector3 point)
		{
			return Vector3I.Floor(point * m_divisor);
		}

		private Vector3I GetBinIndex(Vector3 point)
		{
			return GetBinIndex(ref point);
		}

		private int AddNewEntry(ref Vector3 point, T data)
		{
			m_count++;
			Entry item;
			if (m_nextFreeEntry == m_storage.Count)
			{
				MyList<Entry> storage = m_storage;
				item = new Entry
				{
					Point = point,
					Data = data,
					NextEntry = InvalidIndex
				};
				storage.Add(item);
				return m_nextFreeEntry++;
			}
			if ((uint)m_nextFreeEntry > m_storage.Count)
			{
				return -1;
			}
			int nextFreeEntry = m_nextFreeEntry;
			m_nextFreeEntry = m_storage[m_nextFreeEntry].NextEntry;
			item = (m_storage[nextFreeEntry] = new Entry
			{
				Point = point,
				Data = data,
				NextEntry = InvalidIndex
			});
			return nextFreeEntry;
		}

		/// <summary>
		/// Removes entry with the given index and returns the index of the next entry (or -1 if the removed entry was the last one)
		/// </summary>
		private int RemoveEntry(int toRemove)
		{
			m_count--;
			Entry value = m_storage[toRemove];
			int nextEntry = value.NextEntry;
			value.NextEntry = m_nextFreeEntry;
			value.Data = default(T);
			m_nextFreeEntry = toRemove;
			m_storage[toRemove] = value;
			return nextEntry;
		}

		[Conditional("DEBUG")]
		private void CheckIndexIsValid(int index)
		{
			int num = m_nextFreeEntry;
			while (num != InvalidIndex && num != m_storage.Count)
			{
				num = m_storage[num].NextEntry;
			}
		}
	}
}
