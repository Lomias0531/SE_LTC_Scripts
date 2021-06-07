using System.Diagnostics;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public static class MyDebugWorkTrackerExtensions
	{
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Hit(this MyDebugWorkTracker<int> self)
		{
			self.Current++;
		}

		public static int Min(this MyDebugWorkTracker<int> self)
		{
			int num = int.MaxValue;
			int count = self.History.Count;
			for (int i = 0; i < count; i++)
			{
				int num2 = self.History[i];
				if (num > num2)
				{
					num = num2;
				}
			}
			return num;
		}

		public static int Max(this MyDebugWorkTracker<int> self)
		{
			int num = int.MinValue;
			int count = self.History.Count;
			for (int i = 0; i < count; i++)
			{
				int num2 = self.History[i];
				if (num < num2)
				{
					num = num2;
				}
			}
			return num;
		}

		public static int Average(this MyDebugWorkTracker<int> self)
		{
			long num = 0L;
			int count = self.History.Count;
			for (int i = 0; i < count; i++)
			{
				num += self.History[i];
			}
			return (int)(num / count);
		}

		public static Vector4I Stats(this MyDebugWorkTracker<int> self)
		{
			if (self.History.Count == 0)
			{
				return new Vector4I(0, 0, 0, 0);
			}
			long num = 0L;
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			int count = self.History.Count;
			for (int i = 0; i < count; i++)
			{
				int num4 = self.History[i];
				if (num3 < num4)
				{
					num3 = num4;
				}
				if (num2 > num4)
				{
					num2 = num4;
				}
				num += num4;
			}
			Vector4I result = default(Vector4I);
			result.X = self.History[count - 1];
			result.Y = num2;
			result.Z = (int)(num / count);
			result.W = num3;
			return result;
		}
	}
}
