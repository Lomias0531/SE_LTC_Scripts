using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key)
		{
			dictionary.TryGetValue(key, out V value);
			return value;
		}

		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
		{
			if (!dictionary.TryGetValue(key, out V value))
			{
				return defaultValue;
			}
			return value;
		}

		public static KeyValuePair<K, V> FirstPair<K, V>(this Dictionary<K, V> dictionary)
		{
			Dictionary<K, V>.Enumerator enumerator = dictionary.GetEnumerator();
			enumerator.MoveNext();
			return enumerator.Current;
		}

		public static V GetValueOrDefault<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V defaultValue)
		{
			if (!dictionary.TryGetValue(key, out V value))
			{
				return defaultValue;
			}
			return value;
		}

		public static void Remove<K, V>(this ConcurrentDictionary<K, V> dictionary, K key)
		{
			dictionary.TryRemove(key, out V _);
		}

		public static TValue GetOrAdd<TKey, TValue, TContext>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TContext context, Func<TContext, TKey, TValue> activator)
		{
			if (!dictionary.TryGetValue(key, out TValue value))
			{
				return dictionary.GetOrAdd(key, activator(context, key));
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[DebuggerStepThrough]
		public static void AssertEmpty<K, V>(this Dictionary<K, V> collection)
		{
			if (collection.Count != 0)
			{
				collection.Clear();
			}
		}
	}
}
