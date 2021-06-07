using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe queue.
	/// Uses spin-lock
	/// </summary>
	public class MyConcurrentSortedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		private SortedDictionary<TKey, TValue> m_dictionary;

		private FastResourceLock m_lock = new FastResourceLock();

		public int Count
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_dictionary.Count;
				}
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_dictionary[key];
				}
			}
			set
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					m_dictionary[key] = value;
				}
			}
		}

		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TKey, SortedDictionary<TKey, TValue>.KeyCollection> Keys
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TKey, SortedDictionary<TKey, TValue>.KeyCollection>(@lock, m_dictionary.Keys);
			}
		}

		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TValue, SortedDictionary<TKey, TValue>.ValueCollection> Values
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TValue, SortedDictionary<TKey, TValue>.ValueCollection>(@lock, m_dictionary.Values);
			}
		}

		public MyConcurrentSortedDictionary(IComparer<TKey> comparer = null)
		{
			m_dictionary = new SortedDictionary<TKey, TValue>(comparer);
		}

		public TValue ChangeKey(TKey oldKey, TKey newKey)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				TValue val = m_dictionary[oldKey];
				m_dictionary.Remove(oldKey);
				m_dictionary[newKey] = val;
				return val;
			}
		}

		public void Clear()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_dictionary.Clear();
			}
		}

		public void Add(TKey key, TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_dictionary.Add(key, value);
			}
		}

		public bool TryAdd(TKey key, TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (!m_dictionary.ContainsKey(key))
				{
					m_dictionary.Add(key, value);
					return true;
				}
				return false;
			}
		}

		public bool ContainsKey(TKey key)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.ContainsKey(key);
			}
		}

		public bool ContainsValue(TValue value)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.ContainsValue(value);
			}
		}

		public bool Remove(TKey key)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				return m_dictionary.Remove(key);
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.TryGetValue(key, out value);
			}
		}

		public void GetValues(List<TValue> result)
		{
			using (m_lock.AcquireSharedUsing())
			{
				foreach (TValue value in m_dictionary.Values)
				{
					result.Add(value);
				}
			}
		}

		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> GetEnumerator()
		{
			FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator>(@lock, m_dictionary.GetEnumerator());
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
