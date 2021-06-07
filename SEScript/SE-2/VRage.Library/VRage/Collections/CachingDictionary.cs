using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	/// <summary>
	/// Dictionary wrapper that allows for addition and removal even during enumeration.
	/// Done by caching changes and allowing explicit application using Apply* methods.
	/// </summary>
	public class CachingDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		private Dictionary<K, V> m_dictionary = new Dictionary<K, V>();

		private List<KeyValuePair<K, V>> m_additionsAndModifications = new List<KeyValuePair<K, V>>();

		private List<K> m_removals = new List<K>();

		private static K m_keyToCompare;

		private static Predicate<K> m_keyEquals = KeyEquals;

		private static Predicate<KeyValuePair<K, V>> m_keyValueEquals = KeyValueEquals;

		private static K KeyToCompare
		{
			set
			{
				m_keyToCompare = value;
			}
		}

		public DictionaryReader<K, V> Reader => m_dictionary;

		public V this[K key]
		{
			get
			{
				return m_dictionary[key];
			}
			set
			{
				Add(key, value);
			}
		}

		public Dictionary<K, V>.KeyCollection Keys => m_dictionary.Keys;

		public Dictionary<K, V>.ValueCollection Values => m_dictionary.Values;

		public void Add(K key, V value, bool immediate = false)
		{
			if (immediate)
			{
				m_dictionary[key] = value;
				return;
			}
			m_additionsAndModifications.Add(new KeyValuePair<K, V>(key, value));
			m_keyToCompare = key;
			m_removals.RemoveAll(m_keyEquals);
		}

		public void Remove(K key, bool immediate = false)
		{
			if (immediate)
			{
				m_dictionary.Remove(key);
				return;
			}
			m_removals.Add(key);
			m_keyToCompare = key;
			m_additionsAndModifications.RemoveAll(m_keyValueEquals);
		}

		public bool TryGetValue(K key, out V value)
		{
			return m_dictionary.TryGetValue(key, out value);
		}

		public bool ContainsKey(K key)
		{
			return m_dictionary.ContainsKey(key);
		}

		public void Clear()
		{
			m_dictionary.Clear();
			m_additionsAndModifications.Clear();
			m_removals.Clear();
		}

		public bool HasChanges()
		{
			if (m_additionsAndModifications.Count <= 0)
			{
				return m_removals.Count > 0;
			}
			return true;
		}

		public void ApplyChanges()
		{
			ApplyAdditionsAndModifications();
			ApplyRemovals();
		}

		public void ApplyAdditionsAndModifications()
		{
			foreach (KeyValuePair<K, V> additionsAndModification in m_additionsAndModifications)
			{
				m_dictionary[additionsAndModification.Key] = additionsAndModification.Value;
			}
			m_additionsAndModifications.Clear();
		}

		public void ApplyRemovals()
		{
			foreach (K removal in m_removals)
			{
				m_dictionary.Remove(removal);
			}
			m_removals.Clear();
		}

		private static bool KeyEquals(K key)
		{
			return EqualityComparer<K>.Default.Equals(key, m_keyToCompare);
		}

		private static bool KeyValueEquals(KeyValuePair<K, V> keyValue)
		{
			return EqualityComparer<K>.Default.Equals(keyValue.Key, m_keyToCompare);
		}

		public Dictionary<K, V>.Enumerator GetEnumerator()
		{
			return m_dictionary.GetEnumerator();
		}

		IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
