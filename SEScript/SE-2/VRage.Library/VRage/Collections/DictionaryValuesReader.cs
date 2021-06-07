using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct DictionaryValuesReader<K, V> : IEnumerable<V>, IEnumerable
	{
		private readonly Dictionary<K, V> m_collection;

		public int Count => m_collection.Count;

		public V this[K key] => m_collection[key];

		public DictionaryValuesReader(Dictionary<K, V> collection)
		{
			m_collection = collection;
		}

		public bool TryGetValue(K key, out V result)
		{
			return m_collection.TryGetValue(key, out result);
		}

		public Dictionary<K, V>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_collection.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<V> IEnumerable<V>.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static implicit operator DictionaryValuesReader<K, V>(Dictionary<K, V> v)
		{
			return new DictionaryValuesReader<K, V>(v);
		}
	}
}
