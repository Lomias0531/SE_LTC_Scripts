using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct ListReader<T> : IEnumerable<T>, IEnumerable
	{
		public static ListReader<T> Empty = new ListReader<T>(new List<T>(0));

		private readonly List<T> m_list;

		public int Count => m_list.Count;

		public T this[int index] => m_list[index];

		public ListReader(List<T> list)
		{
			m_list = (list ?? Empty.m_list);
		}

		public static implicit operator ListReader<T>(List<T> list)
		{
			return new ListReader<T>(list);
		}

		public T ItemAt(int index)
		{
			return m_list[index];
		}

		public int IndexOf(T item)
		{
			return m_list.IndexOf(item);
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return m_list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
