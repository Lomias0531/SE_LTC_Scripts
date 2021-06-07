using System.Collections.Generic;
using System.Linq;

namespace VRage.Game
{
	internal static class CollectionDictExtensions
	{
		public static IEnumerable<TVal> GetOrEmpty<TKey, TValCol, TVal>(this Dictionary<TKey, TValCol> self, TKey key) where TValCol : IEnumerable<TVal>
		{
			if (!self.TryGetValue(key, out TValCol value))
			{
				return Enumerable.Empty<TVal>();
			}
			return value;
		}

		public static IEnumerable<TVal> GetOrEmpty<TKey, TKey2, TVal>(this Dictionary<TKey, Dictionary<TKey2, TVal>> self, TKey key)
		{
			if (!self.TryGetValue(key, out Dictionary<TKey2, TVal> value))
			{
				return Enumerable.Empty<TVal>();
			}
			return value.Values;
		}
	}
}
