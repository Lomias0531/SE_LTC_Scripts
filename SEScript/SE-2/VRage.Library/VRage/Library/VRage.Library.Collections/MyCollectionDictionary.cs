using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Base class for dictionaries that store multiple elements under a key using some collection.
	/// </summary>
	public abstract class MyCollectionDictionary<TKey, TCollection, TValue> : IEnumerable<KeyValuePair<TKey, TCollection>>, IEnumerable where TCollection : ICollection<TValue>, new()
	{
		private readonly Stack<TCollection> m_collectionCache = new Stack<TCollection>();

		private readonly Dictionary<TKey, TCollection> m_dictionary;

		public TCollection this[TKey key] => m_dictionary[key];

		public Dictionary<TKey, TCollection>.ValueCollection Values => m_dictionary.Values;

		public Dictionary<TKey, TCollection>.KeyCollection Keys => m_dictionary.Keys;

		public int KeyCount => m_dictionary.Count;

		private TCollection Get()
		{
			if (m_collectionCache.Count > 0)
			{
				return m_collectionCache.Pop();
			}
			return CreateCollection();
		}

		protected virtual TCollection CreateCollection()
		{
			return new TCollection();
		}

		private void Return(TCollection list)
		{
			list.Clear();
			m_collectionCache.Push(list);
		}

		public MyCollectionDictionary()
			: this((IEqualityComparer<TKey>)null)
		{
		}

		public MyCollectionDictionary(IEqualityComparer<TKey> keyComparer = null)
		{
			m_dictionary = new Dictionary<TKey, TCollection>(keyComparer);
		}

		/// <summary>
		/// Try get a collection for the provided key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public bool TryGet(TKey key, out TCollection list)
		{
			return m_dictionary.TryGetValue(key, out list);
		}

		public TCollection GetOrDefault(TKey key)
		{
			return m_dictionary.GetValueOrDefault(key);
		}

		public TCollection GetOrAdd(TKey key)
		{
			if (!m_dictionary.TryGetValue(key, out TCollection value))
			{
				value = Get();
				m_dictionary.Add(key, value);
			}
			return value;
		}

		public void Add(TKey key, TValue value)
		{
			GetOrAdd(key).Add(value);
		}

		public void Add(TKey key, IEnumerable<TValue> values)
		{
			TCollection orAdd = GetOrAdd(key);
			foreach (TValue value in values)
			{
				orAdd.Add(value);
			}
		}

		public void Add(TKey key, TValue first, TValue second)
		{
			TCollection orAdd = GetOrAdd(key);
			orAdd.Add(first);
			orAdd.Add(second);
		}

		public void Add(TKey key, TValue first, TValue second, TValue third)
		{
			TCollection orAdd = GetOrAdd(key);
			orAdd.Add(first);
			orAdd.Add(second);
			orAdd.Add(third);
		}

		public void Add(TKey key, params TValue[] values)
		{
			TCollection orAdd = GetOrAdd(key);
			foreach (TValue item in values)
			{
				orAdd.Add(item);
			}
		}

		public bool Remove(TKey key)
		{
			if (m_dictionary.TryGetValue(key, out TCollection value))
			{
				m_dictionary.Remove(key);
				Return(value);
				return true;
			}
			return false;
		}

		public bool Remove(TKey key, TValue value)
		{
			if (m_dictionary.TryGetValue(key, out TCollection value2))
			{
				return value2.Remove(value);
			}
			return false;
		}

		public void Clear()
		{
			foreach (KeyValuePair<TKey, TCollection> item in m_dictionary)
			{
				Return(item.Value);
			}
			m_dictionary.Clear();
		}

		public IEnumerator<KeyValuePair<TKey, TCollection>> GetEnumerator()
		{
			return m_dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
