using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Library.Collections
{
	public class LRUCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		[DebuggerDisplay("Prev={Prev}, Next={Next}, Key={Key}, Data={Data}")]
		private struct CacheEntry
		{
			public int Prev;

			public int Next;

			public TValue Data;

			public TKey Key;
		}

		private static HashSet<int> m_debugEntrySet = new HashSet<int>();

		private int m_first;

		private int m_last;

		private readonly IEqualityComparer<TKey> m_comparer;

		private readonly Dictionary<TKey, int> m_index;

		private readonly CacheEntry[] m_entries;

		private readonly FastResourceLock m_lock = new FastResourceLock();

		public Action<TKey, TValue> OnItemDiscarded;

		private const int Null = -1;

		public float Usage => (float)m_index.Count / (float)m_entries.Length;

		public int Count => m_index.Count;

		public int Capacity => m_entries.Length;

		public LRUCache(int cacheSize, IEqualityComparer<TKey> comparer = null)
		{
			m_comparer = (comparer ?? EqualityComparer<TKey>.Default);
			m_entries = new CacheEntry[cacheSize];
			m_index = new Dictionary<TKey, int>(cacheSize, m_comparer);
			ResetInternal();
		}

		public void Reset()
		{
			if (m_index.Count <= 0)
			{
				return;
			}
			if (OnItemDiscarded != null)
			{
				for (int i = 0; i < m_entries.Length; i++)
				{
					if (m_entries[i].Data != null)
					{
						OnItemDiscarded(m_entries[i].Key, m_entries[i].Data);
					}
				}
			}
			ResetInternal();
		}

		private void ResetInternal()
		{
			CacheEntry cacheEntry = default(CacheEntry);
			cacheEntry.Data = default(TValue);
			cacheEntry.Key = default(TKey);
			for (int i = 0; i < m_entries.Length; i++)
			{
				cacheEntry.Prev = i - 1;
				cacheEntry.Next = i + 1;
				m_entries[i] = cacheEntry;
			}
			m_first = 0;
			m_last = m_entries.Length - 1;
			m_entries[m_last].Next = -1;
			m_index.Clear();
		}

		public TValue Read(TKey key)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				try
				{
					if (m_index.TryGetValue(key, out int value))
					{
						if (value != m_first)
						{
							Remove(value);
							AddFirst(value);
						}
						return m_entries[value].Data;
					}
					return default(TValue);
				}
				finally
				{
				}
			}
		}

		public bool TryRead(TKey key, out TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				try
				{
					if (m_index.TryGetValue(key, out int value2))
					{
						if (value2 != m_first)
						{
							Remove(value2);
							AddFirst(value2);
						}
						value = m_entries[value2].Data;
						return true;
					}
					value = default(TValue);
					return false;
				}
				finally
				{
				}
			}
		}

		/// <summary>
		/// Read an entry from the cache without modifying the cache order.
		/// </summary>
		/// <param name="key">The entry key.</param>
		/// <param name="value">The entry if found.</param>
		/// <returns>Whether the entry was found.</returns>
		public bool TryPeek(TKey key, out TValue value)
		{
			using (m_lock.AcquireSharedUsing())
			{
				if (m_index.TryGetValue(key, out int value2))
				{
					value = m_entries[value2].Data;
					return true;
				}
				value = default(TValue);
				return false;
			}
		}

		/// <summary>
		/// Write an entry into the cache.
		///
		/// If the cache is full the oldest record is evicted in order to make room for the new one.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Write(TKey key, TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_index.TryGetValue(key, out int value2))
				{
					m_entries[value2].Data = value;
				}
				else
				{
					int last = m_last;
					RemoveLast();
					if (m_entries[last].Key != null)
					{
						m_index.Remove(m_entries[last].Key);
					}
					m_entries[last].Key = key;
					m_entries[last].Data = value;
					AddFirst(last);
					m_index.Add(key, last);
				}
			}
		}

		/// <summary>
		/// Try to remove an entry from the cache.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(TKey key)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				try
				{
					if (m_index.TryGetValue(key, out int value))
					{
						Remove(value);
						CleanEntry(value);
						ReinsertLast(value);
					}
				}
				finally
				{
				}
			}
		}

		/// <summary>
		/// Remove every entry in the cache that matches the predicate.
		/// </summary>
		/// <param name="entryAction"></param>
		/// <returns>The number of entries removed.</returns>
		public int RemoveWhere(Func<TKey, TValue, bool> predicate)
		{
			int num = 0;
			using (m_lock.AcquireExclusiveUsing())
			{
				int num2 = m_first;
				while (num2 != -1)
				{
					int num3 = num2;
					num2 = m_entries[num3].Next;
					if (predicate(m_entries[num3].Key, m_entries[num3].Data))
					{
						Remove(num3);
						CleanEntry(num3);
						ReinsertLast(num3);
						num++;
					}
				}
				return num;
			}
		}

		private void RemoveLast()
		{
			int prev = m_entries[m_last].Prev;
			m_entries[prev].Next = -1;
			m_entries[m_last].Prev = -1;
			if (OnItemDiscarded != null && m_entries[m_last].Data != null)
			{
				OnItemDiscarded(m_entries[m_last].Key, m_entries[m_last].Data);
			}
			m_last = prev;
		}

		private void Remove(int entryIndex)
		{
			int prev = m_entries[entryIndex].Prev;
			int next = m_entries[entryIndex].Next;
			if (prev != -1)
			{
				m_entries[prev].Next = m_entries[entryIndex].Next;
			}
			else
			{
				m_first = m_entries[entryIndex].Next;
			}
			if (next != -1)
			{
				m_entries[next].Prev = m_entries[entryIndex].Prev;
			}
			else
			{
				m_last = m_entries[entryIndex].Prev;
			}
			m_entries[entryIndex].Prev = -1;
			m_entries[entryIndex].Next = -1;
		}

		private void ReinsertLast(int entryIndex)
		{
			m_entries[m_last].Next = entryIndex;
			m_entries[entryIndex].Prev = m_last;
			m_entries[entryIndex].Next = -1;
			m_last = entryIndex;
		}

		private void CleanEntry(int entryIndex)
		{
			if (OnItemDiscarded != null && m_entries[entryIndex].Data != null)
			{
				OnItemDiscarded(m_entries[entryIndex].Key, m_entries[entryIndex].Data);
			}
			m_index.Remove(m_entries[entryIndex].Key);
			m_entries[entryIndex].Key = default(TKey);
			m_entries[entryIndex].Data = default(TValue);
		}

		private void AddFirst(int entryIndex)
		{
			m_entries[m_first].Prev = entryIndex;
			m_entries[entryIndex].Next = m_first;
			m_first = entryIndex;
		}

		/// <summary>
		/// Verifies that all assumptions are met (linked list is connected and all lookups are correct).
		/// FULLDEBUG is only here to disable this. Enable by changing to DEBUG if you suspect problems.
		/// </summary>
		[Conditional("__UNUSED__")]
		private void AssertConsistent()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < m_entries.Length; j++)
				{
					m_debugEntrySet.Add(j);
				}
				switch (i)
				{
				case 0:
				{
					for (int num = m_first; num != -1; num = m_entries[num].Next)
					{
						bool flag2 = m_debugEntrySet.Remove(num);
					}
					break;
				}
				case 1:
				{
					for (int num2 = m_last; num2 != -1; num2 = m_entries[num2].Prev)
					{
						bool flag3 = m_debugEntrySet.Remove(num2);
					}
					break;
				}
				case 2:
					foreach (KeyValuePair<TKey, int> item in m_index)
					{
						bool flag = m_debugEntrySet.Remove(item.Value);
					}
					m_debugEntrySet.Clear();
					break;
				}
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			using (m_lock.AcquireSharedUsing())
			{
				foreach (int value in m_index.Values)
				{
					yield return new KeyValuePair<TKey, TValue>(m_entries[value].Key, m_entries[value].Data);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
