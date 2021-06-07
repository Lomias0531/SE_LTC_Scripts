using System.Collections.Generic;

namespace VRage.Stats
{
	internal class MyPriorityComparer : Comparer<KeyValuePair<string, MyStat>>
	{
		public override int Compare(KeyValuePair<string, MyStat> x, KeyValuePair<string, MyStat> y)
		{
			return -x.Value.Priority.CompareTo(y.Value.Priority);
		}
	}
}
