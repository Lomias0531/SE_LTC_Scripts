using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct SortedDictionaryValuesReader<K, V> : IEnumerable<V>, IEnumerable
	{
		private readonly SortedDictionary<K, V> m_collection;

		public int Count => m_collection.Count;

		public V this[K key] => m_collection[key];

		public SortedDictionaryValuesReader(SortedDictionary<K, V> collection)
		{
			m_collection = collection;
		}

		public bool TryGetValue(K key, out V result)
		{
			return m_collection.TryGetValue(key, out result);
		}

		public SortedDictionary<K, V>.ValueCollection.Enumerator GetEnumerator()
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

		public static implicit operator SortedDictionaryValuesReader<K, V>(SortedDictionary<K, V> v)
		{
			return new SortedDictionaryValuesReader<K, V>(v);
		}
	}
}
