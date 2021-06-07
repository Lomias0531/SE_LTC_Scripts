using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Stores items in list with fixed index (no reordering).
	/// Null is used as special value and cannot be added into list.
	/// </summary>
	public class MyIndexList<T> : IEnumerable<T>, IEnumerable where T : class
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private MyIndexList<T> m_list;

			private int m_index;

			private int m_version;

			public T Current
			{
				get
				{
					if (m_version != m_list.m_version)
					{
						throw new InvalidOperationException("Collection was modified after enumerator was created");
					}
					return m_list[m_index];
				}
			}

			object IEnumerator.Current => Current;

			public Enumerator(MyIndexList<T> list)
			{
				m_list = list;
				m_index = -1;
				m_version = list.m_version;
			}

			public bool MoveNext()
			{
				do
				{
					m_index++;
					if (m_index >= m_list.Count)
					{
						return false;
					}
				}
				while (m_list[m_index] == null);
				return true;
			}

			void IDisposable.Dispose()
			{
			}

			void IEnumerator.Reset()
			{
				m_index = -1;
				m_version = m_list.m_version;
			}
		}

		private List<T> m_list;

		private Queue<int> m_freeList;

		private int m_version;

		public int Count => m_list.Count;

		public T this[int index] => m_list[index];

		/// <summary>
		/// Returns what will be next index returned by Add operation.
		/// </summary>
		public int NextIndex
		{
			get
			{
				if (m_freeList.Count <= 0)
				{
					return m_list.Count;
				}
				return m_freeList.Peek();
			}
		}

		public MyIndexList(int capacity = 0)
		{
			m_list = new List<T>(capacity);
			m_freeList = new Queue<int>(capacity);
		}

		public int Add(T item)
		{
			if (item == null)
			{
				throw new ArgumentException("Null cannot be stored in IndexList, it's used as 'empty' indicator");
			}
			if (QueueExtensions.TryDequeue(m_freeList, out int result))
			{
				m_list[result] = item;
				m_version++;
				return result;
			}
			m_list.Add(item);
			m_version++;
			return m_list.Count - 1;
		}

		public void Remove(int index)
		{
			if (!TryRemove(index))
			{
				throw new InvalidOperationException($"Item at index {index} is already empty");
			}
		}

		public void Remove(int index, out T removedItem)
		{
			if (!TryRemove(index, out removedItem))
			{
				throw new InvalidOperationException($"Item at index {index} is already empty");
			}
		}

		public bool TryRemove(int index)
		{
			T removedItem;
			return TryRemove(index, out removedItem);
		}

		public bool TryRemove(int index, out T removedItem)
		{
			removedItem = m_list[index];
			if (removedItem == null)
			{
				return false;
			}
			m_version++;
			m_list[index] = null;
			m_freeList.Enqueue(index);
			return true;
		}

		private Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
