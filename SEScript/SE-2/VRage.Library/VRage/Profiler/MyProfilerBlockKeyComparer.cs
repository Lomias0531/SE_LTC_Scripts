using System.Collections.Generic;

namespace VRage.Profiler
{
	public class MyProfilerBlockKeyComparer : IEqualityComparer<MyProfilerBlockKey>
	{
		public bool Equals(MyProfilerBlockKey x, MyProfilerBlockKey y)
		{
			if (x.ParentId == y.ParentId && x.Name == y.Name && x.Member == y.Member && x.File == y.File)
			{
				return x.Line == y.Line;
			}
			return false;
		}

		public int GetHashCode(MyProfilerBlockKey obj)
		{
			return obj.HashCode;
		}
	}
}
