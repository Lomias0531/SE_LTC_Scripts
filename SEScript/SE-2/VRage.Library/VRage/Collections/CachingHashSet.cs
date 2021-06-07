using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public class CachingHashSet<T> : IEnumerable<T>, IEnumerable
	{
		private HashSet<T> m_hashSet = new HashSet<T>();

		private HashSet<T> m_toAdd = new HashSet<T>();

		private HashSet<T> m_toRemove = new HashSet<T>();

		public int Count => m_hashSet.Count;

		public void Clear()
		{
			m_hashSet.Clear();
			m_toAdd.Clear();
			m_toRemove.Clear();
		}

		public bool Contains(T item)
		{
			return m_hashSet.Contains(item);
		}

		public void Add(T item)
		{
			if (!m_toRemove.Remove(item) && !m_hashSet.Contains(item))
			{
				m_toAdd.Add(item);
			}
		}

		public void Remove(T item, bool immediate = false)
		{
			if (immediate)
			{
				m_toAdd.Remove(item);
				m_hashSet.Remove(item);
				m_toRemove.Remove(item);
			}
			else if (!m_toAdd.Remove(item) && m_hashSet.Contains(item))
			{
				m_toRemove.Add(item);
			}
		}

		public void ApplyChanges()
		{
			ApplyAdditions();
			ApplyRemovals();
		}

		public void ApplyAdditions()
		{
			foreach (T item in m_toAdd)
			{
				m_hashSet.Add(item);
			}
			m_toAdd.Clear();
		}

		public void ApplyRemovals()
		{
			foreach (T item in m_toRemove)
			{
				m_hashSet.Remove(item);
			}
			m_toRemove.Clear();
		}

		public HashSet<T>.Enumerator GetEnumerator()
		{
			return m_hashSet.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			return $"Count = {m_hashSet.Count}; ToAdd = {m_toAdd.Count}; ToRemove = {m_toRemove.Count}";
		}
	}
}
