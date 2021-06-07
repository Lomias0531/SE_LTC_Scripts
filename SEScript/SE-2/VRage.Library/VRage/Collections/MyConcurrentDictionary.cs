using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe queue.
	/// Uses spin-lock
	/// </summary>
	public class MyConcurrentDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		private Dictionary<TKey, TValue> m_dictionary;

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

		[DebuggerHidden]
		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TKey, Dictionary<TKey, TValue>.KeyCollection> Keys
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TKey, Dictionary<TKey, TValue>.KeyCollection>(@lock, m_dictionary.Keys);
			}
		}

		[DebuggerHidden]
		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TValue, Dictionary<TKey, TValue>.ValueCollection> Values
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TValue, Dictionary<TKey, TValue>.ValueCollection>(@lock, m_dictionary.Values);
			}
		}

		public MyConcurrentDictionary(IEqualityComparer<TKey> comparer)
		{
			m_dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		public MyConcurrentDictionary(int capacity = 0, IEqualityComparer<TKey> comparer = null)
		{
			m_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
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

		public bool TryRemove(TKey key, out TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_dictionary.TryGetValue(key, out value))
				{
					m_dictionary.Remove(key);
					return true;
				}
			}
			return false;
		}

		public bool TryRemoveConditionally<TCondition>(TKey key, out TValue value, TCondition condition) where TCondition : IMyCondition<TValue>
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_dictionary.TryGetValue(key, out value) && condition.Evaluate(value))
				{
					m_dictionary.Remove(key);
					return true;
				}
			}
			return false;
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

		public TValue GetValueOrDefault(TKey key, TValue defaultValue)
		{
			if (!TryGetValue(key, out TValue value))
			{
				return defaultValue;
			}
			return value;
		}

		public KeyValuePair<TKey, TValue> FirstPair()
		{
			using (m_lock.AcquireSharedUsing())
			{
				Dictionary<TKey, TValue>.Enumerator enumerator = m_dictionary.GetEnumerator();
				enumerator.MoveNext();
				return enumerator.Current;
			}
		}

		[DebuggerHidden]
		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> GetEnumerator()
		{
			FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator>(@lock, m_dictionary.GetEnumerator());
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
