using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct DictionaryReader<K, V> : IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		private readonly Dictionary<K, V> m_collection;

		public static readonly DictionaryReader<K, V> Empty;

		public bool IsValid => m_collection != null;

		public int Count => m_collection.Count;

		public V this[K key] => m_collection[key];

		public IEnumerable<K> Keys => m_collection.Keys;

		public IEnumerable<V> Values => m_collection.Values;

		public DictionaryReader(Dictionary<K, V> collection)
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

		public Dictionary<K, V>.Enumerator GetEnumerator()
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

		public static implicit operator DictionaryReader<K, V>(Dictionary<K, V> v)
		{
			return new DictionaryReader<K, V>(v);
		}
	}
}
