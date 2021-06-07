using System;
using VRageMath;

namespace VRage.Noise.Patterns
{
	/// <summary>
	/// Noise that outputs concentric cylinders.
	/// Each cylinder extends infinitely along the y axis.
	/// </summary>
	internal class MyCylinders : IMyModule
	{
		public double Frequency
		{
			get;
			set;
		}

		public MyCylinders(double frequnecy = 1.0)
		{
			Frequency = frequnecy;
		}

		public double GetValue(double x)
		{
			x *= Frequency;
			double num = Math.Sqrt(x * x + x * x);
			int num2 = MathHelper.Floor(num);
			double num3 = num - (double)num2;
			double val = 1.0 - num3;
			double num4 = Math.Min(num3, val);
			return 1.0 - num4 * 4.0;
		}

		public double GetValue(double x, double z)
		{
			x *= Frequency;
			z *= Frequency;
			double num = Math.Sqrt(x * x + z * z);
			int num2 = MathHelper.Floor(num);
			double num3 = num - (double)num2;
			double val = 1.0 - num3;
			double num4 = Math.Min(num3, val);
			return 1.0 - num4 * 4.0;
		}

		public double GetValue(double x, double y, double z)
		{
			throw new NotImplementedException();
		}
	}
}
