using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct SortedDictionaryReader<K, V> : IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		private readonly SortedDictionary<K, V> m_collection;

		public static readonly SortedDictionaryReader<K, V> Empty;

		public bool IsValid => m_collection != null;

		public int Count => m_collection.Count;

		public V this[K key] => m_collection[key];

		public IEnumerable<K> Keys => m_collection.Keys;

		public IEnumerable<V> Values => m_collection.Values;

		public SortedDictionaryReader(SortedDictionary<K, V> collection)
		{
			m_collection = collection;
		}

		public bool ContainsKey(K key)
		{
			return m_collection.ContainsKey(key);
		}

		public bool TryGetValue(K key, out V value)
		{
			return m_collection.TryGetValue(key, out value);
		}

		public SortedDictionary<K, V>.Enumerator GetEnumerator()
		{
			return m_collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static implicit operator SortedDictionaryReader<K, V>(SortedDictionary<K, V> v)
		{
			return new SortedDictionaryReader<K, V>(v);
		}
	}
}
