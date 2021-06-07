using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	public class StringSegmentIgnoreCaseComparer : IEqualityComparer<StringSegment>
	{
		public static readonly StringSegmentIgnoreCaseComparer DEFAULT = new StringSegmentIgnoreCaseComparer();

		public bool Equals(StringSegment x, StringSegment y)
		{
			if (x.Length != y.Length)
			{
				return false;
			}
			string text = x.Text;
			int num = x.Start;
			string text2 = y.Text;
			int num2 = y.Start;
			for (int i = 0; i < x.Length; i++)
			{
				if (char.ToUpperInvariant(text[num]) != char.ToUpperInvariant(text2[num2]))
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		public int GetHashCode(StringSegment obj)
		{
			return MyUtils.GetHashUpperCase(obj.Text, obj.Start, obj.Length);
		}
	}
}
