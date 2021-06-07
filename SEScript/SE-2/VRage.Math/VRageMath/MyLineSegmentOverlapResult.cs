using System.Collections.Generic;

namespace VRageMath
{
	public struct MyLineSegmentOverlapResult<T>
	{
		public class MyLineSegmentOverlapResultComparer : IComparer<MyLineSegmentOverlapResult<T>>
		{
			public int Compare(MyLineSegmentOverlapResult<T> x, MyLineSegmentOverlapResult<T> y)
			{
				return x.Distance.CompareTo(y.Distance);
			}
		}

		public static MyLineSegmentOverlapResultComparer DistanceComparer = new MyLineSegmentOverlapResultComparer();

		public double Distance;

		public T Element;
	}
}
