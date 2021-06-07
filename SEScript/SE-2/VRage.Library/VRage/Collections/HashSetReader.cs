using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct HashSetReader<T> : IEnumerable<T>, IEnumerable
	{
		private readonly HashSet<T> m_hashset;

		public bool IsValid => m_hashset != null;

		public int Count => m_hashset.Count;

		public HashSetReader(HashSet<T> set)
		{
			m_hashset = set;
		}

		public static implicit operator HashSetReader<T>(HashSet<T> v)
		{
			return new HashSetReader<T>(v);
		}

		public bool Contains(T item)
		{
			return m_hashset.Contains(item);
		}

		public T First()
		{
			using (HashSet<T>.Enumerator enumerator = GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("No elements in collection!");
				}
				return enumerator.Current;
			}
		}

		public T[] ToArray()
		{
			T[] array = new T[m_hashset.Count];
			m_hashset.CopyTo(array);
			return array;
		}

		public HashSet<T>.Enumerator GetEnumerator()
		{
			return m_hashset.GetEnumerator();
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
