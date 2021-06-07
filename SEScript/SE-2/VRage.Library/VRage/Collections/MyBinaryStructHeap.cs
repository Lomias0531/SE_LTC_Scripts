using System;
using System.Collections.Generic;

namespace VRage.Collections
{
	public class MyBinaryStructHeap<TKey, TValue> where TValue : struct
	{
		public struct HeapItem
		{
			public TKey Key
			{
				get;
				internal set;
			}

			public TValue Value
			{
				get;
				internal set;
			}

			public override string ToString()
			{
				return Key.ToString() + ": " + Value;
			}
		}

		private HeapItem[] m_array;

		private int m_count;

		private int m_capacity;

		private IComparer<TKey> m_comparer;

		public int Count => m_count;

		public bool Full => m_count == m_capacity;

		public MyBinaryStructHeap(int initialCapacity = 128, IComparer<TKey> comparer = null)
		{
			m_array = new HeapItem[initialCapacity];
			m_count = 0;
			m_capacity = initialCapacity;
			m_comparer = (comparer ?? Comparer<TKey>.Default);
		}

		public void Insert(TValue value, TKey key)
		{
			if (m_count == m_capacity)
			{
				Reallocate();
			}
			HeapItem heapItem = default(HeapItem);
			heapItem.Key = key;
			heapItem.Value = value;
			HeapItem heapItem2 = heapItem;
			m_array[m_count] = heapItem2;
			Up(m_count);
			m_count++;
		}

		public TValue Min()
		{
			return m_array[0].Value;
		}

		public TKey MinKey()
		{
			return m_array[0].Key;
		}

		public TValue RemoveMin()
		{
			TValue value = m_array[0].Value;
			if (m_count != 1)
			{
				MoveItem(m_count - 1, 0);
				m_array[m_count - 1].Key = default(TKey);
				m_array[m_count - 1].Value = default(TValue);
				m_count--;
				Down(0);
			}
			else
			{
				m_count--;
				m_array[0].Key = default(TKey);
				m_array[0].Value = default(TValue);
			}
			return value;
		}

		public TValue RemoveMax()
		{
			int num = 0;
			for (int i = 1; i < m_count; i++)
			{
				if (m_comparer.Compare(m_array[num].Key, m_array[i].Key) < 0)
				{
					num = i;
				}
			}
			TValue value = m_array[num].Value;
			if (num != m_count)
			{
				MoveItem(m_count - 1, num);
				Up(num);
			}
			m_count--;
			return value;
		}

		public TValue Remove(TValue value, IEqualityComparer<TValue> comparer = null)
		{
			if (m_count == 0)
			{
				return default(TValue);
			}
			if (comparer == null)
			{
				comparer = EqualityComparer<TValue>.Default;
			}
			int num = 0;
			for (int i = 0; i < m_count; i++)
			{
				if (comparer.Equals(value, m_array[i].Value))
				{
					num = i;
				}
			}
			TValue result;
			if (num != m_count)
			{
				result = m_array[num].Value;
				MoveItem(m_count - 1, num);
				Up(num);
				Down(num);
				m_count--;
			}
			else
			{
				result = default(TValue);
			}
			return result;
		}

		public TValue Remove(TKey key)
		{
			int num = 0;
			for (int i = 1; i < m_count; i++)
			{
				if (m_comparer.Compare(key, m_array[i].Key) == 0)
				{
					num = i;
				}
			}
			TValue result;
			if (num != m_count)
			{
				result = m_array[num].Value;
				MoveItem(m_count - 1, num);
				Up(num);
				Down(num);
			}
			else
			{
				result = default(TValue);
			}
			m_count--;
			return result;
		}

		public void Clear()
		{
			for (int i = 0; i < m_count; i++)
			{
				m_array[i].Key = default(TKey);
				m_array[i].Value = default(TValue);
			}
			m_count = 0;
		}

		private void Up(int index)
		{
			if (index == 0)
			{
				return;
			}
			int num = (index - 1) / 2;
			if (m_comparer.Compare(m_array[num].Key, m_array[index].Key) <= 0)
			{
				return;
			}
			HeapItem fromItem = m_array[index];
			do
			{
				MoveItem(num, index);
				index = num;
				if (index == 0)
				{
					break;
				}
				num = (index - 1) / 2;
			}
			while (m_comparer.Compare(m_array[num].Key, fromItem.Key) > 0);
			MoveItem(ref fromItem, index);
		}

		private void Down(int index)
		{
			if (m_count == index + 1)
			{
				return;
			}
			int num = index * 2 + 1;
			int num2 = num + 1;
			HeapItem fromItem = m_array[index];
			while (num2 <= m_count)
			{
				if (num2 == m_count || m_comparer.Compare(m_array[num].Key, m_array[num2].Key) < 0)
				{
					if (m_comparer.Compare(fromItem.Key, m_array[num].Key) <= 0)
					{
						break;
					}
					MoveItem(num, index);
					index = num;
					num = index * 2 + 1;
					num2 = num + 1;
				}
				else
				{
					if (m_comparer.Compare(fromItem.Key, m_array[num2].Key) <= 0)
					{
						break;
					}
					MoveItem(num2, index);
					index = num2;
					num = index * 2 + 1;
					num2 = num + 1;
				}
			}
			MoveItem(ref fromItem, index);
		}

		private void MoveItem(int fromIndex, int toIndex)
		{
			m_array[toIndex] = m_array[fromIndex];
		}

		private void MoveItem(ref HeapItem fromItem, int toIndex)
		{
			m_array[toIndex] = fromItem;
		}

		private void Reallocate()
		{
			HeapItem[] array = new HeapItem[m_capacity * 2];
			Array.Copy(m_array, array, m_capacity);
			m_array = array;
			m_capacity *= 2;
		}
	}
}
