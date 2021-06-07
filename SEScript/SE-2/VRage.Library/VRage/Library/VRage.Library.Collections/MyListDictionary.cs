using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Collection which stores multiple elements under same key by using list.
	/// Collection does not allow removing single value, only all items with same key.
	/// </summary>
	public class MyListDictionary<TKey, TValue> : MyCollectionDictionary<TKey, List<TValue>, TValue>
	{
		public MyListDictionary()
		{
		}

		public MyListDictionary(IEqualityComparer<TKey> keyComparer = null)
			: base(keyComparer)
		{
		}
	}
}
