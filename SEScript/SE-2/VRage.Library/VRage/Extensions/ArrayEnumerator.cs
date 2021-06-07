using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
	public struct ArrayEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		private T[] m_array;

		private int m_currentIndex;

		public T Current => m_array[m_currentIndex];

		object IEnumerator.Current => Current;

		public ArrayEnumerator(T[] array)
		{
			m_array = array;
			m_currentIndex = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			m_currentIndex++;
			return m_currentIndex < m_array.Length;
		}

		public void Reset()
		{
			m_currentIndex = -1;
		}
	}
}
