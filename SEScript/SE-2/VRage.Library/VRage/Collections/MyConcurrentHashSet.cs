using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe queue.
	/// Uses spin-lock
	/// </summary>
	public class MyConcurrentHashSet<T> : IEnumerable<T>, IEnumerable
	{
		private HashSet<T> m_set;

		private SpinLockRef m_lock = new SpinLockRef();

		public int Count
		{
			get
			{
				using (m_lock.Acquire())
				{
					return m_set.Count;
				}
			}
		}

		public MyConcurrentHashSet()
		{
			m_set = new HashSet<T>();
		}

		public MyConcurrentHashSet(IEqualityComparer<T> comparer)
		{
			m_set = new HashSet<T>(comparer);
		}

		public void Clear()
		{
			using (m_lock.Acquire())
			{
				m_set.Clear();
			}
		}

		public bool Add(T instance)
		{
			using (m_lock.Acquire())
			{
				return m_set.Add(instance);
			}
		}

		public bool Remove(T value)
		{
			using (m_lock.Acquire())
			{
				return m_set.Remove(value);
			}
		}

		public bool Contains(T value)
		{
			bool flag = false;
			using (m_lock.Acquire())
			{
				return m_set.Contains(value);
			}
		}

		public ConcurrentEnumerator<SpinLockRef.Token, T, HashSet<T>.Enumerator> GetEnumerator()
		{
			SpinLockRef.Token @lock = m_lock.Acquire();
			return ConcurrentEnumerator.Create<SpinLockRef.Token, T, HashSet<T>.Enumerator>(@lock, m_set.GetEnumerator());
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
