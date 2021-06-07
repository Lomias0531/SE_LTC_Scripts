using System.Collections.Generic;

namespace VRage.Stats
{
	internal class MyNameComparer : Comparer<KeyValuePair<string, MyStat>>
	{
		public override int Compare(KeyValuePair<string, MyStat> x, KeyValuePair<string, MyStat> y)
		{
			return x.Key.CompareTo(y.Key);
		}
	}
}
