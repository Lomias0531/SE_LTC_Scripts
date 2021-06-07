using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct UniqueListReader<T> : IEnumerable<T>, IEnumerable
	{
		public static UniqueListReader<T> Empty = new UniqueListReader<T>(new MyUniqueList<T>());

		private readonly MyUniqueList<T> m_list;

		public int Count => m_list.Count;

		public UniqueListReader(MyUniqueList<T> list)
		{
			m_list = list;
		}

		public static implicit operator ListReader<T>(UniqueListReader<T> list)
		{
			return list.m_list.ItemList;
		}

		public static implicit operator UniqueListReader<T>(MyUniqueList<T> list)
		{
			return new UniqueListReader<T>(list);
		}

		public T ItemAt(int index)
		{
			return m_list[index];
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
