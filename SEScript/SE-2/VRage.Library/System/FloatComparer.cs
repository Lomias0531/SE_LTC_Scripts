using System.Collections.Generic;

namespace System
{
	public class FloatComparer : IComparer<float>
	{
		public static FloatComparer Instance = new FloatComparer();

		public int Compare(float x, float y)
		{
			return x.CompareTo(y);
		}
	}
}
