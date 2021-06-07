using System;
using VRageMath;

namespace VRage.Noise.Patterns
{
	/// <summary>
	/// Noise that outputs concentric spheres.
	/// </summary>
	internal class MySpheres : IMyModule
	{
		public double Frequency
		{
			get;
			set;
		}

		public MySpheres(double frequnecy = 1.0)
		{
			Frequency = frequnecy;
		}

		public double GetValue(double x)
		{
			x *= Frequency;
			int num = MathHelper.Floor(x);
			double num2 = Math.Sqrt(x * x + x * x) - (double)num;
			double val = 1.0 - num2;
			double num3 = Math.Min(num2, val);
			return 1.0 - num3 * 4.0;
		}

		public double GetValue(double x, double y)
		{
			x *= Frequency;
			y *= Frequency;
			int num = MathHelper.Floor(x);
			double num2 = Math.Sqrt(x * x + y * y) - (double)num;
			double val = 1.0 - num2;
			double num3 = Math.Min(num2, val);
			return 1.0 - num3 * 4.0;
		}

		public double GetValue(double x, double y, double z)
		{
			throw new NotImplementedException();
		}
	}
}
