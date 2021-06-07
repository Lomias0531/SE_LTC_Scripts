using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
	/// <summary>
	/// List wrapper that allows for addition and removal even during enumeration.
	/// Done by caching changes and allowing explicit application using Apply* methods.
	///
	/// This version has individual locks for cached and non-cached versions, allowing
	/// each to be managed efficiently even across multiple threads
	/// </summary>
	public class ConcurrentCachingList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		private readonly List<T> m_list = new List<T>();

		private readonly List<T> m_toAdd = new List<T>();

		private readonly List<T> m_toRemove = new List<T>();

		private FastResourceLock m_listLock = new FastResourceLock();

		private SpinLockRef m_cacheLock = new SpinLockRef();

		private bool m_dirty;

		public int Count
		{
			get
			{
				using (m_listLock.AcquireSharedUsing())
				{
					return m_list.Count;
				}
			}
		}

		public T this[int index]
		{
			get
			{
				using (m_listLock.AcquireSharedUsing())
				{
					return m_list[index];
				}
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (m_list.Count == 0 && m_toAdd.Count == 0)
				{
					return m_toRemove.Count == 0;
				}
				return false;
			}
		}

		public ConcurrentCachingList()
		{
		}

		public ConcurrentCachingList(int capacity)
		{
			m_list = new List<T>(capacity);
		}

		public void Add(T entity)
		{
			using (m_cacheLock.Acquire())
			{
				if (m_toRemove.Contains(entity))
				{
					m_toRemove.Remove(entity);
				}
				else
				{
					m_toAdd.Add(entity);
					m_dirty = true;
				}
			}
		}

		public void Remove(T entity, bool immediate = false)
		{
			using (m_cacheLock.Acquire())
			{
				if (!m_toAdd.Remove(entity))
				{
					m_toRemove.Add(entity);
				}
			}
			if (immediate)
			{
				using (m_listLock.AcquireExclusiveUsing())
				{
					using (m_cacheLock.Acquire())
					{
						m_list.Remove(entity);
						m_toRemove.Remove(entity);
					}
				}
			}
			else
			{
				m_dirty = true;
			}
		}

		/// <summary>
		/// Immediately removes an element at the specified index.
		/// </summary>
		/// <param name="index">Index of the element to remove immediately.</param>
		public void RemoveAtImmediately(int index)
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				if (index >= 0 && index < m_list.Count)
				{
					m_list.RemoveAt(index);
				}
			}
		}

		public void ClearList()
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				m_list.Clear();
			}
		}

		public void ClearImmediate()
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				using (m_cacheLock.Acquire())
				{
					m_toAdd.Clear();
					m_toRemove.Clear();
					m_list.Clear();
					m_dirty = false;
				}
			}
		}

		public void ApplyChanges()
		{
			if (m_dirty)
			{
				m_dirty = false;
				ApplyAdditions();
				ApplyRemovals();
			}
		}

		public void ApplyAdditions()
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				using (m_cacheLock.Acquire())
				{
					m_list.AddRange(m_toAdd);
					m_toAdd.Clear();
				}
			}
		}

		public void ApplyRemovals()
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				using (m_cacheLock.Acquire())
				{
					foreach (T item in m_toRemove)
					{
						m_list.Remove(item);
					}
					m_toRemove.Clear();
				}
			}
		}

		public void Sort(IComparer<T> comparer)
		{
			using (m_listLock.AcquireExclusiveUsing())
			{
				m_list.Sort(comparer);
			}
		}

		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, T, List<T>.Enumerator> GetEnumerator()
		{
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, T, List<T>.Enumerator>(m_listLock.AcquireSharedUsing(), m_list.GetEnumerator());
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
