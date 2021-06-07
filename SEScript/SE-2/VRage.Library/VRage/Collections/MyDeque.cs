using System;
using System.Diagnostics;

namespace VRage.Collections
{
	[DebuggerDisplay("Count = {Count}")]
	public class MyDeque<T>
	{
		private T[] m_buffer;

		private int m_front;

		private int m_back;

		public bool Empty => m_front == m_back;

		private bool Full => (m_back + 1) % m_buffer.Length == m_front;

		public int Count => m_back - m_front + ((m_back < m_front) ? m_buffer.Length : 0);

		public MyDeque(int baseCapacity = 8)
		{
			m_buffer = new T[baseCapacity + 1];
		}

		public void Clear()
		{
			Array.Clear(m_buffer, 0, m_buffer.Length);
			m_front = 0;
			m_back = 0;
		}

		public void EnqueueFront(T value)
		{
			EnsureCapacityForOne();
			Decrement(ref m_front);
			m_buffer[m_front] = value;
		}

		public void EnqueueBack(T value)
		{
			EnsureCapacityForOne();
			m_buffer[m_back] = value;
			Increment(ref m_back);
		}

		public T DequeueFront()
		{
			T result = m_buffer[m_front];
			m_buffer[m_front] = default(T);
			Increment(ref m_front);
			return result;
		}

		public T DequeueBack()
		{
			Decrement(ref m_back);
			T result = m_buffer[m_back];
			m_buffer[m_back] = default(T);
			return result;
		}

		private void Increment(ref int index)
		{
			index = (index + 1) % m_buffer.Length;
		}

		private void Decrement(ref int index)
		{
			index--;
			if (index < 0)
			{
				index += m_buffer.Length;
			}
		}

		private void EnsureCapacityForOne()
		{
			if (Full)
			{
				T[] array = new T[(m_buffer.Length - 1) * 2 + 1];
				int back = 0;
				int index = m_front;
				while (index != m_back)
				{
					array[back++] = m_buffer[index];
					Increment(ref index);
				}
				m_buffer = array;
				m_front = 0;
				m_back = back;
			}
		}
	}
}
