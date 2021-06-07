using System;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Collections
{
	public class ThreadSafeStore<TKey, TValue>
	{
		private readonly object m_lock = new object();

		private Dictionary<TKey, TValue> m_store;

		private readonly Func<TKey, TValue> m_creator;

		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			m_creator = creator;
			m_store = new Dictionary<TKey, TValue>();
		}

		public TValue Get(TKey key)
		{
			if (!m_store.TryGetValue(key, out TValue value))
			{
				return AddValue(key);
			}
			return value;
		}

		public TValue Get(TKey key, Func<TKey, TValue> creator)
		{
			if (!m_store.TryGetValue(key, out TValue value))
			{
				return AddValue(key, creator);
			}
			return value;
		}

		private TValue AddValue(TKey key, Func<TKey, TValue> creator = null)
		{
			Func<TKey, TValue> func = creator ?? m_creator;
			TValue val = func(key);
			lock (m_lock)
			{
				if (m_store == null)
				{
					m_store = new Dictionary<TKey, TValue>();
					m_store[key] = val;
				}
				else
				{
					if (m_store.TryGetValue(key, out TValue value))
					{
						return value;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(m_store);
					dictionary[key] = val;
					Thread.MemoryBarrier();
					m_store = dictionary;
				}
				return val;
			}
		}
	}
}
