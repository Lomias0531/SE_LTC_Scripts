using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// MultiKeyIndex supports value lookups using multiple different keys.
	/// The keys must derivable from value, if it's not the case use MultiKeyDictionary.
	/// </summary>
	public class MyMultiKeyIndex<TKey1, TKey2, TValue> : IEnumerable<TValue>, IEnumerable
	{
		private Dictionary<TKey1, TValue> m_index1 = new Dictionary<TKey1, TValue>();

		private Dictionary<TKey2, TValue> m_index2 = new Dictionary<TKey2, TValue>();

		public readonly Func<TValue, TKey1> KeySelector1;

		public readonly Func<TValue, TKey2> KeySelector2;

		public TValue this[TKey1 key] => m_index1[key];

		public TValue this[TKey2 key] => m_index2[key];

		public int Count => m_index1.Count;

		public MyMultiKeyIndex(Func<TValue, TKey1> keySelector1, Func<TValue, TKey2> keySelector2, int capacity = 0, EqualityComparer<TKey1> keyComparer1 = null, EqualityComparer<TKey2> keyComparer2 = null)
		{
			m_index1 = new Dictionary<TKey1, TValue>(capacity, keyComparer1);
			m_index2 = new Dictionary<TKey2, TValue>(capacity, keyComparer2);
			KeySelector1 = keySelector1;
			KeySelector2 = keySelector2;
		}

		public void Add(TValue value)
		{
			TKey1 key = KeySelector1(value);
			m_index1.Add(key, value);
			try
			{
				m_index2.Add(KeySelector2(value), value);
			}
			catch
			{
				m_index1.Remove(key);
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
			if (m_index1.TryGetValue(key1, out TValue value) && m_index2.Remove(KeySelector2(value)))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool Remove(TKey2 key2)
		{
			if (m_index2.TryGetValue(key2, out TValue value) && m_index1.Remove(KeySelector1(value)))
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

		public bool TryRemove(TKey1 key1, out TValue removedValue)
		{
			if (m_index1.TryGetValue(key1, out removedValue) && m_index2.Remove(KeySelector2(removedValue)))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool TryRemove(TKey2 key2, out TValue removedValue)
		{
			if (m_index2.TryGetValue(key2, out removedValue) && m_index1.Remove(KeySelector1(removedValue)))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool TryGetValue(TKey1 key1, out TValue result)
		{
			return m_index1.TryGetValue(key1, out result);
		}

		public bool TryGetValue(TKey2 key2, out TValue result)
		{
			return m_index2.TryGetValue(key2, out result);
		}

		public Dictionary<TKey1, TValue>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_index1.Values.GetEnumerator();
		}

		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	/// <summary>
	/// MultiKeyIndex supports value lookups using multiple different keys.
	/// The keys must derivable from value, if it's not the case use MultiKeyDictionary.
	/// </summary>
	public class MyMultiKeyIndex<TKey1, TKey2, TKey3, TValue> : IEnumerable<TValue>, IEnumerable
	{
		private Dictionary<TKey1, TValue> m_index1 = new Dictionary<TKey1, TValue>();

		private Dictionary<TKey2, TValue> m_index2 = new Dictionary<TKey2, TValue>();

		private Dictionary<TKey3, TValue> m_index3 = new Dictionary<TKey3, TValue>();

		public readonly Func<TValue, TKey1> KeySelector1;

		public readonly Func<TValue, TKey2> KeySelector2;

		public readonly Func<TValue, TKey3> KeySelector3;

		public TValue this[TKey1 key] => m_index1[key];

		public TValue this[TKey2 key] => m_index2[key];

		public TValue this[TKey3 key] => m_index3[key];

		public int Count => m_index1.Count;

		public MyMultiKeyIndex(Func<TValue, TKey1> keySelector1, Func<TValue, TKey2> keySelector2, Func<TValue, TKey3> keySelector3, int capacity = 0, EqualityComparer<TKey1> keyComparer1 = null, EqualityComparer<TKey2> keyComparer2 = null, EqualityComparer<TKey3> keyComparer3 = null)
		{
			m_index1 = new Dictionary<TKey1, TValue>(capacity, keyComparer1);
			m_index2 = new Dictionary<TKey2, TValue>(capacity, keyComparer2);
			m_index3 = new Dictionary<TKey3, TValue>(capacity, keyComparer3);
			KeySelector1 = keySelector1;
			KeySelector2 = keySelector2;
			KeySelector3 = keySelector3;
		}

		public void Add(TValue value)
		{
			TKey1 key = KeySelector1(value);
			m_index1.Add(key, value);
			try
			{
				TKey2 key2 = KeySelector2(value);
				m_index2.Add(key2, value);
				try
				{
					m_index3.Add(KeySelector3(value), value);
				}
				catch
				{
					m_index2.Remove(key2);
					throw;
				}
			}
			catch
			{
				m_index1.Remove(key);
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
			if (m_index1.TryGetValue(key1, out TValue value) && m_index3.Remove(KeySelector3(value)) && m_index2.Remove(KeySelector2(value)))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool Remove(TKey2 key2)
		{
			if (m_index2.TryGetValue(key2, out TValue value) && m_index3.Remove(KeySelector3(value)) && m_index1.Remove(KeySelector1(value)))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool Remove(TKey3 key3)
		{
			if (m_index3.TryGetValue(key3, out TValue value) && m_index1.Remove(KeySelector1(value)) && m_index2.Remove(KeySelector2(value)))
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
			if (m_index1.TryGetValue(key1, out removedValue) && m_index3.Remove(KeySelector3(removedValue)) && m_index2.Remove(KeySelector2(removedValue)))
			{
				return m_index1.Remove(key1);
			}
			return false;
		}

		public bool TryRemove(TKey2 key2, out TValue removedValue)
		{
			if (m_index2.TryGetValue(key2, out removedValue) && m_index3.Remove(KeySelector3(removedValue)) && m_index1.Remove(KeySelector1(removedValue)))
			{
				return m_index2.Remove(key2);
			}
			return false;
		}

		public bool TryRemove(TKey3 key3, out TValue removedValue)
		{
			if (m_index3.TryGetValue(key3, out removedValue) && m_index1.Remove(KeySelector1(removedValue)) && m_index2.Remove(KeySelector2(removedValue)))
			{
				return m_index3.Remove(key3);
			}
			return false;
		}

		public bool TryGetValue(TKey1 key1, out TValue result)
		{
			return m_index1.TryGetValue(key1, out result);
		}

		public bool TryGetValue(TKey2 key2, out TValue result)
		{
			return m_index2.TryGetValue(key2, out result);
		}

		public bool TryGetValue(TKey3 key3, out TValue result)
		{
			return m_index3.TryGetValue(key3, out result);
		}

		public Dictionary<TKey1, TValue>.ValueCollection.Enumerator GetEnumerator()
		{
			return m_index1.Values.GetEnumerator();
		}

		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
