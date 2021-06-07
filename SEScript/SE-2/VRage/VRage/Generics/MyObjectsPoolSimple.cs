using System;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Generics
{
	public class MyObjectsPoolSimple<T> where T : class, new()
	{
		private T[] m_items;

		private int m_nextAllocateIndex;

		public MyObjectsPoolSimple(int capacity)
		{
			m_items = new T[capacity];
		}

		public T Allocate()
		{
			int num = Interlocked.Increment(ref m_nextAllocateIndex) - 1;
			if (num >= m_items.Length)
			{
				return null;
			}
			T val = m_items[num];
			if (val == null)
			{
				val = (m_items[num] = new T());
			}
			return val;
		}

		public int GetAllocatedCount()
		{
			return Math.Min(m_nextAllocateIndex, m_items.Length);
		}

		public int GetCapacity()
		{
			return m_items.Length;
		}

		public void ClearAllAllocated()
		{
			if (m_nextAllocateIndex > m_items.Length)
			{
				Array.Resize(ref m_items, Math.Max(m_nextAllocateIndex, m_items.Length * 2));
			}
			m_nextAllocateIndex = 0;
		}

		public T GetAllocatedItem(int index)
		{
			return m_items[index];
		}

		public void Sort(IComparer<T> comparer)
		{
			if (m_nextAllocateIndex > 1)
			{
				Array.Sort(m_items, 0, GetAllocatedCount(), comparer);
			}
		}
	}
}
