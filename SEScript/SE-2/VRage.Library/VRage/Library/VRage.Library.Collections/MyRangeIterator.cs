using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public struct MyRangeIterator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		public struct Enumerable : IEnumerable<T>, IEnumerable
		{
			private MyRangeIterator<T> m_enumerator;

			public Enumerable(MyRangeIterator<T> enume)
			{
				m_enumerator = enume;
			}

			public IEnumerator<T> GetEnumerator()
			{
				return m_enumerator;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private IList<T> m_list;

		private int m_start;

		private int m_current;

		private int m_end;

		public T Current => m_list[m_current];

		object IEnumerator.Current => Current;

		public static Enumerable ForRange(T[] array, int start, int end)
		{
			return new Enumerable(new MyRangeIterator<T>(array, start, end));
		}

		public static Enumerable ForRange(List<T> list, int start, int end)
		{
			return new Enumerable(new MyRangeIterator<T>(list, start, end));
		}

		public MyRangeIterator(T[] list, int start, int end)
		{
			m_list = list;
			m_start = start;
			m_current = start - 1;
			m_end = end - 1;
		}

		public MyRangeIterator(IList<T> list, int start, int end)
		{
			m_list = list;
			m_start = start;
			m_current = start - 1;
			m_end = end - 1;
		}

		public void Dispose()
		{
			m_list = null;
		}

		public bool MoveNext()
		{
			if (m_current != m_end)
			{
				m_current++;
				return true;
			}
			return false;
		}

		public void Reset()
		{
			m_current = m_start - 1;
		}
	}
}
