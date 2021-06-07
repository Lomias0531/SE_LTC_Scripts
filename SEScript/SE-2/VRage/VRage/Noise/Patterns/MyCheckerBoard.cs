using VRageMath;

namespace VRage.Noise.Patterns
{
	public class MyCheckerBoard : IMyModule
	{
		public double GetValue(double x)
		{
			if ((MathHelper.Floor(x) & 1) != 1)
			{
				return 1.0;
			}
			return -1.0;
		}

		public double GetValue(double x, double y)
		{
			int num = MathHelper.Floor(x) & 1;
			int num2 = MathHelper.Floor(y) & 1;
			if ((num ^ num2) != 1)
			{
				return 1.0;
			}
			return -1.0;
		}

		public double GetValue(double x, double y, double z)
		{
			int num = MathHelper.Floor(x) & 1;
			int num2 = MathHelper.Floor(y) & 1;
			int num3 = MathHelper.Floor(z) & 1;
			if ((num ^ num2 ^ num3) != 1)
			{
				return 1.0;
			}
			return -1.0;
		}
	}
}
