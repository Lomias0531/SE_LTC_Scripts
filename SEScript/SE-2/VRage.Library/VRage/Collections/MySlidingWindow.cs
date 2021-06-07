using System;

namespace VRage.Collections
{
	public class MySlidingWindow<T>
	{
		private MyQueue<T> m_items;

		public int Size;

		public T DefaultValue;

		public Func<MyQueue<T>, T> AverageFunc;

		public T Average
		{
			get
			{
				if (m_items.Count == 0)
				{
					return DefaultValue;
				}
				return AverageFunc(m_items);
			}
		}

		public T Last
		{
			get
			{
				if (m_items.Count <= 0)
				{
					return DefaultValue;
				}
				return m_items[m_items.Count - 1];
			}
		}

		public MySlidingWindow(int size, Func<MyQueue<T>, T> avg, T defaultValue = default(T))
		{
			AverageFunc = avg;
			Size = size;
			DefaultValue = defaultValue;
			m_items = new MyQueue<T>(size + 1);
		}

		public void Add(T item)
		{
			m_items.Enqueue(item);
			RemoveExcess();
		}

		public void Clear()
		{
			m_items.Clear();
		}

		private void RemoveExcess()
		{
			while (m_items.Count > Size)
			{
				m_items.Dequeue();
			}
		}
	}
}
