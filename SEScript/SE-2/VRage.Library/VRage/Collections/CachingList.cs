using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Collections
{
	/// <summary>
	/// List wrapper that allows for addition and removal even during enumeration.
	/// Done by caching changes and allowing explicit application using Apply* methods.
	/// </summary>
	public class CachingList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		private List<T> m_list = new List<T>();

		private List<T> m_toAdd = new List<T>();

		private List<T> m_toRemove = new List<T>();

		public int Count => m_list.Count;

		public bool HasChanges
		{
			get
			{
				if (m_toAdd.Count <= 0)
				{
					return m_toRemove.Count > 0;
				}
				return true;
			}
		}

		public T this[int index] => m_list[index];

		public CachingList()
		{
		}

		public CachingList(int capacity)
		{
			m_list = new List<T>(capacity);
		}

		public void Add(T entity)
		{
			if (m_toRemove.Contains(entity))
			{
				m_toRemove.Remove(entity);
			}
			else
			{
				m_toAdd.Add(entity);
			}
		}

		public void Remove(T entity, bool immediate = false)
		{
			int num = m_toAdd.IndexOf(entity);
			if (num >= 0)
			{
				m_toAdd.RemoveAt(num);
			}
			else
			{
				m_toRemove.Add(entity);
			}
			if (immediate)
			{
				m_list.Remove(entity);
				m_toRemove.Remove(entity);
			}
		}

		/// <summary>
		/// Immediately removes an element at the specified index.
		/// </summary>
		/// <param name="index">Index of the element to remove immediately.</param>
		public void RemoveAtImmediately(int index)
		{
			if (index >= 0 && index < m_list.Count)
			{
				m_list.RemoveAt(index);
			}
		}

		public void Clear()
		{
			for (int i = 0; i < m_list.Count; i++)
			{
				Remove(m_list[i]);
			}
		}

		public void ClearImmediate()
		{
			m_toAdd.Clear();
			m_toRemove.Clear();
			m_list.Clear();
		}

		public void ApplyChanges()
		{
			ApplyAdditions();
			ApplyRemovals();
		}

		public void ApplyAdditions()
		{
			m_list.AddRange(m_toAdd);
			m_toAdd.Clear();
		}

		public void ApplyRemovals()
		{
			foreach (T item in m_toRemove)
			{
				m_list.Remove(item);
			}
			m_toRemove.Clear();
		}

		/// <summary>
		/// Create a copy of the internal list with changes applied.
		/// </summary>
		/// <remarks>This method does not modify the collection</remarks>
		/// <returns>A copy of the list with all changes applied.</returns>
		public List<T> CopyWithChanges()
		{
			List<T> list = new List<T>(m_list);
			list.AddRange(m_toAdd);
			foreach (T item in m_toRemove)
			{
				list.Remove(item);
			}
			return list;
		}

		public void Sort(IComparer<T> comparer)
		{
			m_list.Sort(comparer);
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return m_list.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[Conditional("DEBUG")]
		public void DebugCheckEmpty()
		{
		}

		public override string ToString()
		{
			return $"Count = {m_list.Count}; ToAdd = {m_toAdd.Count}; ToRemove = {m_toRemove.Count}";
		}
	}
}
