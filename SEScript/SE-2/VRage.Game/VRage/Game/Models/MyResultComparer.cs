using System.Collections.Generic;

namespace VRage.Game.Models
{
	internal class MyResultComparer : IComparer<MyIntersectionResultLineTriangle>
	{
		public int Compare(MyIntersectionResultLineTriangle x, MyIntersectionResultLineTriangle y)
		{
			return x.Distance.CompareTo(y.Distance);
		}
	}
}
