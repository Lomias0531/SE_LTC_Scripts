using System.Collections.Generic;

namespace System.Linq
{
	public static class LinqExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
			{
				action(item);
			}
		}

		public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K k, out V v)
		{
			k = pair.Key;
			v = pair.Value;
		}
	}
}
