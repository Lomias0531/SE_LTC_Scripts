using System;
using System.Collections.Generic;

namespace VRage.Collections
{
	public class MyBinaryHeap<K, V> where V : HeapItem<K>
	{
		private HeapItem<K>[] m_array;

		private int m_count;

		private int m_capacity;

		private IComparer<K> m_comparer;

		public int Count => m_count;

		public bool Full => m_count == m_capacity;

		public MyBinaryHeap()
		{
			m_array = new HeapItem<K>[128];
			m_count = 0;
			m_capacity = 128;
			m_comparer = Comparer<K>.Default;
		}

		public MyBinaryHeap(int initialCapacity)
		{
			m_array = new HeapItem<K>[initialCapacity];
			m_count = 0;
			m_capacity = initialCapacity;
			m_comparer = Comparer<K>.Default;
		}

		public MyBinaryHeap(int initialCapacity, IComparer<K> comparer)
		{
			m_array = new HeapItem<K>[initialCapacity];
			m_count = 0;
			m_capacity = initialCapacity;
			m_comparer = comparer;
		}

		public void Insert(V value, K key)
		{
			if (m_count == m_capacity)
			{
				Reallocate();
			}
			value.HeapKey = key;
			MoveItem(value, m_count);
			Up(m_count);
			m_count++;
		}

		public V GetItem(int index)
		{
			return m_array[index] as V;
		}

		public V Min()
		{
			return (V)m_array[0];
		}

		public V RemoveMin()
		{
			V result = (V)m_array[0];
			if (m_count != 1)
			{
				MoveItem(m_count - 1, 0);
				m_array[m_count - 1] = null;
				m_count--;
				Down(0);
			}
			else
			{
				m_count--;
				m_array[0] = null;
			}
			return result;
		}

		public V RemoveMax()
		{
			int num = 0;
			for (int i = 1; i < m_count; i++)
			{
				if (m_comparer.Compare(m_array[num].HeapKey, m_array[i].HeapKey) < 0)
				{
					num = i;
				}
			}
			V result = m_array[num] as V;
			if (num != m_count)
			{
				MoveItem(m_count - 1, num);
				Up(num);
			}
			m_count--;
			return result;
		}

		public void Remove(V item)
		{
			if (m_count != 1)
			{
				if (m_count - 1 == item.HeapIndex)
				{
					m_array[m_count - 1] = null;
					m_count--;
					return;
				}
				MoveItem(m_count - 1, item.HeapIndex);
				m_array[m_count - 1] = null;
				m_count--;
				if (m_comparer.Compare(item.HeapKey, m_array[item.HeapIndex].HeapKey) < 0)
				{
					Down(item.HeapIndex);
				}
				else
				{
					Up(item.HeapIndex);
				}
			}
			else
			{
				m_count--;
				m_array[0] = null;
			}
		}

		public void Modify(V item, K newKey)
		{
			K heapKey = item.HeapKey;
			item.HeapKey = newKey;
			if (m_comparer.Compare(heapKey, newKey) <= 0)
			{
				Down(item.HeapIndex);
			}
			else
			{
				Up(item.HeapIndex);
			}
		}

		public void ModifyUp(V item, K newKey)
		{
			item.HeapKey = newKey;
			Up(item.HeapIndex);
		}

		public void ModifyDown(V item, K newKey)
		{
			item.HeapKey = newKey;
			Down(item.HeapIndex);
		}

		public void Clear()
		{
			for (int i = 0; i < m_count; i++)
			{
				m_array[i] = null;
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
			if (m_comparer.Compare(m_array[num].HeapKey, m_array[index].HeapKey) <= 0)
			{
				return;
			}
			HeapItem<K> heapItem = m_array[index];
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
			while (m_comparer.Compare(m_array[num].HeapKey, heapItem.HeapKey) > 0);
			MoveItem(heapItem, index);
		}

		private void Down(int index)
		{
			if (m_count == index + 1)
			{
				return;
			}
			int num = index * 2 + 1;
			int num2 = num + 1;
			HeapItem<K> heapItem = m_array[index];
			while (num2 <= m_count)
			{
				if (num2 == m_count || m_comparer.Compare(m_array[num].HeapKey, m_array[num2].HeapKey) < 0)
				{
					if (m_comparer.Compare(heapItem.HeapKey, m_array[num].HeapKey) <= 0)
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
					if (m_comparer.Compare(heapItem.HeapKey, m_array[num2].HeapKey) <= 0)
					{
						break;
					}
					MoveItem(num2, index);
					index = num2;
					num = index * 2 + 1;
					num2 = num + 1;
				}
			}
			MoveItem(heapItem, index);
		}

		private void MoveItem(int fromIndex, int toIndex)
		{
			m_array[toIndex] = m_array[fromIndex];
			m_array[toIndex].HeapIndex = toIndex;
		}

		private void MoveItem(HeapItem<K> fromItem, int toIndex)
		{
			m_array[toIndex] = fromItem;
			m_array[toIndex].HeapIndex = toIndex;
		}

		private void Reallocate()
		{
			HeapItem<K>[] array = new HeapItem<K>[m_capacity * 2];
			Array.Copy(m_array, array, m_capacity);
			m_array = array;
			m_capacity *= 2;
		}

		public void QueryAll(List<V> list)
		{
			HeapItem<K>[] array = m_array;
			foreach (HeapItem<K> heapItem in array)
			{
				if (heapItem != null)
				{
					list.Add((V)heapItem);
				}
			}
		}
	}
}
