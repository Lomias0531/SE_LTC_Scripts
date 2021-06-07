using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;

namespace VRageMath.Spatial
{
	public class MyVector3DGrid<T>
	{
		private struct Entry
		{
			public Vector3D Point;

			public T Data;

			public int NextEntry;

			public override string ToString()
			{
				return Point.ToString() + ", -> " + NextEntry + ", Data: " + Data.ToString();
			}
		}

		public struct Enumerator
		{
			private MyVector3DGrid<T> m_parent;

			private Vector3D m_point;

			private double m_distSq;

			private int m_previousIndex;

			private int m_storageIndex;

			private Vector3I_RangeIterator m_rangeIterator;

			public T Current => m_parent.m_storage[m_storageIndex].Data;

			public Vector3I CurrentBin => m_rangeIterator.Current;

			public int PreviousIndex => m_previousIndex;

			public int StorageIndex => m_storageIndex;

			public Enumerator(MyVector3DGrid<T> parent, ref Vector3D point, double dist)
			{
				m_parent = parent;
				m_point = point;
				m_distSq = dist * dist;
				Vector3D value = new Vector3D(dist);
				Vector3I start = Vector3I.Floor((point - value) * parent.m_divisor);
				Vector3I end = Vector3I.Floor((point + value) * parent.m_divisor);
				m_rangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				m_previousIndex = -1;
				m_storageIndex = -1;
			}

			/// <summary>
			/// Removes the current entry and returns true whether there is another entry.
			/// May invalidate enumerators in the same bin.
			/// To remove values from more enumerators while ensuring their validity use MyVector3Grid.RemoveTwo(...).
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

		private double m_cellSize;

		private double m_divisor;

		private int m_nextFreeEntry;

		private int m_count;

		private MyList<Entry> m_storage;

		private Dictionary<Vector3I, int> m_bins;

		public int Count => m_count;

		public MyVector3DGrid(double cellSize)
		{
			m_cellSize = cellSize;
			m_divisor = 1.0 / m_cellSize;
			m_storage = new MyList<Entry>();
			m_bins = new Dictionary<Vector3I, int>();
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

		public void AddPoint(ref Vector3D point, T data)
		{
			Vector3I key = Vector3I.Floor(point * m_divisor);
			if (!m_bins.TryGetValue(key, out int value))
			{
				int value2 = AddNewEntry(ref point, data);
				m_bins.Add(key, value2);
				return;
			}
			Entry value3 = m_storage[value];
			for (int nextEntry = value3.NextEntry; nextEntry != -1; nextEntry = value3.NextEntry)
			{
				value = nextEntry;
				value3 = m_storage[value];
			}
			int num = value3.NextEntry = AddNewEntry(ref point, data);
			m_storage[value] = value3;
		}

		public void RemovePoint(ref Vector3D point)
		{
			Vector3I key = Vector3I.Floor(point * m_divisor);
			if (!m_bins.TryGetValue(key, out int value))
			{
				return;
			}
			int index = -1;
			int num = value;
			Entry value2 = default(Entry);
			while (value != -1)
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
			if (num == -1)
			{
				m_bins.Remove(key);
			}
			else
			{
				m_bins[key] = num;
			}
		}

		public Enumerator GetPointsCloserThan(ref Vector3D point, double dist)
		{
			return new Enumerator(this, ref point, dist);
		}

		/// <summary>
		/// Removes values pointed at by en0 and en1 and ensures that the enumerators both stay consistent
		/// </summary>
		public void RemoveTwo(ref Enumerator en0, ref Enumerator en1)
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

		public void GetLocalBinBB(ref Vector3I binPosition, out BoundingBoxD output)
		{
			output.Min = binPosition * m_cellSize;
			output.Max = output.Min + new Vector3D(m_cellSize);
		}

		public void CollectStorage(int startingIndex, ref List<T> output)
		{
			output.Clear();
			Entry entry = m_storage[startingIndex];
			output.Add(entry.Data);
			while (entry.NextEntry != -1)
			{
				entry = m_storage[entry.NextEntry];
				output.Add(entry.Data);
			}
		}

		private int AddNewEntry(ref Vector3D point, T data)
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
					NextEntry = -1
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
				NextEntry = -1
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
			while (num != -1 && num != m_storage.Count)
			{
				num = m_storage[num].NextEntry;
			}
		}
	}
}
