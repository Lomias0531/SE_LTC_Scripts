using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// MyMultiKeyDictionary supports value lookups using multiple different keys.
	/// When keys can be derived from value, use MultiKeyIndex.
	/// </summary>
	public class MyMultiKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>, IEnumerable
	{
		public struct Triple
		{
			public TKey1 Key1;

			public TKey2 Key2;

			public TValue Value;

			public Triple(TKey1 key1, TKey2 key2, TValue value)
			{
				Key1 = key1;
				Key2 = key2;
				Value = value;
			}
		}

		private Dictionary<TKey1, Triple> m_index1 = new Dictionary<TKey1, Triple>();

		private Dictionary<TKey2, Triple> m_index2 = new Dictionary<TKey2, Triple>();

		public TValue this[TKey1 key] => m_index1[key].Value;

		public TValue this[TKey2 key] => m_index2[key].Value;

		public int Count => m_index1.Count;

		public MyMultiKeyDictionary(int capacity = 0, EqualityComparer<TKey1> keyComparer1 = null, EqualityComparer<TKey2> keyComparer2 = null)
		{
			m_index1 = new Dictionary<TKey1, Triple>(capacity, keyComparer1);
			m_index2 = new Dictionary<TKey2, Triple>(capacity, keyComparer2);
		}

		public void Add(TKey1 key1, TKey2 key2, TValue value)
		{
			Triple value2 = new Triple(key1, key2, value);
			m_index1.Add(key1, value2);
			try
			{
				m_index2.Add(key2, value2);
			}
			catch
			{
				m_index1.Remove(key1);
				throw;
			}
		}

		public bool ContainsKey(TKey1 key1)
		{
			return m_index1.ContainsKey(key1);
		}

		public bool ContainsKey(TKey2 key2)
		{
			return m_index2.ContainsKey(key2);
		}

		public bool Remove(TKey1 key1)
		{
			if (m_index1.TryGetValue(key1, out Triple value) && m_index2.Remove(value.Key2))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool Remove(TKey2 key2)
		{
			if (m_index2.TryGetValue(key2, out Triple value) && m_index1.Remove(value.Key1))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool Remove(TKey1 key1, TKey2 key2)
		{
			if (m_index1.Remove(key1))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool TryRemove(TKey1 key1, out Triple removedValue)
		{
			if (m_index1.TryGetValue(key1, out removedValue) && m_index2.Remove(removedValue.Key2))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool TryRemove(TKey2 key2, out Triple removedValue)
		{
			if (m_index2.TryGetValue(key2, out removedValue) && m_index1.Remove(removedValue.Key1))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool TryRemove(TKey1 key1, out TValue removedValue)
		{
			Triple value;
			bool result = m_index1.TryGetValue(key1, out value) && m_index2.Remove(value.Key2) && m_index1.Remove(key1);
			removedValue = value.Value;
			return result;
		}

		public bool TryRemove(TKey2 key2, out TValue removedValue)
		{
			Triple value;
			bool result = m_index2.TryGetValue(key2, out value) && m_index1.Remove(value.Key1) && m_index2.Remove(key2);
			removedValue = value.Value;
			return result;
		}

		public bool TryGetValue(TKey1 key1, out Triple result)
		{
			return m_index1.TryGetValue(key1, out result);
		}

		public bool TryGetValue(TKey2 key2, out Triple result)
		{
			return m_index2.TryGetValue(key2, out result);
		}

		public bool TryGetValue(TKey1 key1, out TValue result)
		{
			Triple value;
			bool result2 = m_index1.TryGetValue(key1, out value);
			result = value.Value;
			return result2;
		}

		public bool TryGetValue(TKey2 key2, out TValue result)
		{
			Triple value;
			bool result2 = m_index2.TryGetValue(key2, out value);
			result = value.Value;
			return result2;
		}

		private Dictionary<TKey1, Triple>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_index1.Values.GetEnumerator();
		}

		IEnumerator<Triple> IEnumerable<Triple>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	/// <summary>
	/// MyMultiKeyDictionary supports value lookups using multiple different keys.
	/// When keys can be derived from value, use MultiKeyIndex.
	/// </summary>
	public class MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue> : IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>, IEnumerable
	{
		public struct Quadruple
		{
			public TKey1 Key1;

			public TKey2 Key2;

			public TKey3 Key3;

			public TValue Value;

			public Quadruple(TKey1 key1, TKey2 key2, TKey3 key3, TValue value)
			{
				Key1 = key1;
				Key2 = key2;
				Key3 = key3;
				Value = value;
			}
		}

		private Dictionary<TKey1, Quadruple> m_index1 = new Dictionary<TKey1, Quadruple>();

		private Dictionary<TKey2, Quadruple> m_index2 = new Dictionary<TKey2, Quadruple>();

		private Dictionary<TKey3, Quadruple> m_index3 = new Dictionary<TKey3, Quadruple>();

		public TValue this[TKey1 key] => m_index1[key].Value;

		public TValue this[TKey2 key] => m_index2[key].Value;

		public TValue this[TKey3 key] => m_index3[key].Value;

		public int Count => m_index1.Count;

		public MyMultiKeyDictionary(int capacity = 0, EqualityComparer<TKey1> keyComparer1 = null, EqualityComparer<TKey2> keyComparer2 = null, EqualityComparer<TKey3> keyComparer3 = null)
		{
			m_index1 = new Dictionary<TKey1, Quadruple>(capacity, keyComparer1);
			m_index2 = new Dictionary<TKey2, Quadruple>(capacity, keyComparer2);
			m_index3 = new Dictionary<TKey3, Quadruple>(capacity, keyComparer3);
		}

		public void Add(TKey1 key1, TKey2 key2, TKey3 key3, TValue value)
		{
			Quadruple value2 = new Quadruple(key1, key2, key3, value);
			m_index1.Add(key1, value2);
			try
			{
				m_index2.Add(key2, value2);
				try
				{
					m_index3.Add(key3, value2);
				}
				catch
				{
					m_index2.Remove(key2);
					throw;
				}
			}
			catch
			{
				m_index1.Remove(key1);
				throw;
			}
		}

		public bool ContainsKey(TKey1 key1)
		{
			return m_index1.ContainsKey(key1);
		}

		public bool ContainsKey(TKey2 key2)
		{
			return m_index2.ContainsKey(key2);
		}

		public bool ContainsKey(TKey3 key3)
		{
			return m_index3.ContainsKey(key3);
		}

		public bool Remove(TKey1 key1)
		{
			if (m_index1.TryGetValue(key1, out Quadruple value) && m_index3.Remove(value.Key3) && m_index2.Remove(value.Key2))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool Remove(TKey2 key2)
		{
			if (m_index2.TryGetValue(key2, out Quadruple value) && m_index3.Remove(value.Key3) && m_index1.Remove(value.Key1))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool Remove(TKey3 key3)
		{
			if (m_index3.TryGetValue(key3, out Quadruple value) && m_index1.Remove(value.Key1) && m_index2.Remove(value.Key2))
			{
				return m_index3.Remove(key3);
			}
			return false;
		}

		public bool Remove(TKey1 key1, TKey2 key2, TKey3 key3)
		{
			if (m_index1.Remove(key1) && m_index2.Remove(key2))
			{
				return m_index3.Remove(key3);
			}
			return false;
		}

		public bool TryRemove(TKey1 key1, out TValue removedValue)
		{
			Quadruple value;
			bool result = m_index1.TryGetValue(key1, out value) && m_index3.Remove(value.Key3) && m_index2.Remove(value.Key2) && m_index1.Remove(key1);
			removedValue = value.Value;
			return result;
		}

		public bool TryRemove(TKey2 key2, out TValue removedValue)
		{
			Quadruple value;
			bool result = m_index2.TryGetValue(key2, out value) && m_index3.Remove(value.Key3) && m_index1.Remove(value.Key1) && m_index2.Remove(key2);
			removedValue = value.Value;
			return result;
		}

		public bool TryRemove(TKey3 key3, out TValue removedValue)
		{
			Quadruple value;
			bool result = m_index3.TryGetValue(key3, out value) && m_index1.Remove(value.Key1) && m_index2.Remove(value.Key2) && m_index3.Remove(key3);
			removedValue = value.Value;
			return result;
		}

		public bool TryRemove(TKey1 key1, out Quadruple removedValue)
		{
			if (m_index1.TryGetValue(key1, out removedValue) && m_index3.Remove(removedValue.Key3) && m_index2.Remove(removedValue.Key2))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool TryRemove(TKey2 key2, out Quadruple removedValue)
		{
			if (m_index2.TryGetValue(key2, out removedValue) && m_index3.Remove(removedValue.Key3) && m_index1.Remove(removedValue.Key1))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool TryRemove(TKey3 key3, out Quadruple removedValue)
		{
			if (m_index3.TryGetValue(key3, out removedValue) && m_index1.Remove(removedValue.Key1) && m_index2.Remove(removedValue.Key2))
			{
				return m_index3.Remove(key3);
			}
			return false;
		}

		public bool TryGetValue(TKey1 key1, out Quadruple result)
		{
			return m_index1.TryGetValue(key1, out result);
		}

		public bool TryGetValue(TKey2 key2, out Quadruple result)
		{
			return m_index2.TryGetValue(key2, out result);
		}

		public bool TryGetValue(TKey3 key3, out Quadruple result)
		{
			return m_index3.TryGetValue(key3, out result);
		}

		public bool TryGetValue(TKey1 key1, out TValue result)
		{
			Quadruple value;
			bool result2 = m_index1.TryGetValue(key1, out value);
			result = value.Value;
			return result2;
		}

		public bool TryGetValue(TKey2 key2, out TValue result)
		{
			Quadruple value;
			bool result2 = m_index2.TryGetValue(key2, out value);
			result = value.Value;
			return result2;
		}

		public bool TryGetValue(TKey3 key3, out TValue result)
		{
			Quadruple value;
			bool result2 = m_index3.TryGetValue(key3, out value);
			result = value.Value;
			return result2;
		}

		private Dictionary<TKey1, Quadruple>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_index1.Values.GetEnumerator();
		}

		IEnumerator<Quadruple> IEnumerable<Quadruple>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
