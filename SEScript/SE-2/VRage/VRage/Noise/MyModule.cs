using VRageMath;

namespace VRage.Noise
{
	/// <summary>
	/// High quality noise module that combines properties of Value noise and gradient noise.
	/// Value noise is used as input for gradient function. This leads to no artifacts or zero values at integer points.
	/// It's so called Value-Gradient noise.
	/// </summary>
	public abstract class MyModule : IMyModule
	{
		private double GradNoise(double fx, int ix, long seed)
		{
			long num = (1619 * ix + 1013 * seed) & uint.MaxValue;
			num = (((num >> 8) ^ num) & 0xFF);
			return MyNoiseDefaults.RandomVectors[num] * (fx - (double)ix);
		}

		private double GradNoise(double fx, double fy, int ix, int iy, long seed)
		{
			long num = (1619 * ix + 31337 * iy + 1013 * seed) & uint.MaxValue;
			num = (((num >> 8) ^ num) & 0xFF) << 1;
			double num2 = MyNoiseDefaults.RandomVectors[num];
			double num3 = MyNoiseDefaults.RandomVectors[num + 1];
			double num4 = fx - (double)ix;
			double num5 = fy - (double)iy;
			return num2 * num4 + num3 * num5;
		}

		private double GradNoise(double fx, double fy, double fz, int ix, int iy, int iz, long seed)
		{
			long num = (1619 * ix + 31337 * iy + 6971 * iz + 1013 * seed) & int.MaxValue;
			num = (((num >> 8) ^ num) & 0xFF) * 3;
			double num2 = MyNoiseDefaults.RandomVectors[num];
			double num3 = MyNoiseDefaults.RandomVectors[num + 1];
			double num4 = MyNoiseDefaults.RandomVectors[num + 2];
			double num5 = fx - (double)ix;
			double num6 = fy - (double)iy;
			double num7 = fz - (double)iz;
			return num2 * num5 + num3 * num6 + num4 * num7;
		}

		protected double GradCoherentNoise(double x, int seed, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			double amount = 0.0;
			switch (quality)
			{
			case MyNoiseQuality.Low:
				amount = x - (double)num;
				break;
			case MyNoiseQuality.Standard:
				amount = MathHelper.SCurve3(x - (double)num);
				break;
			case MyNoiseQuality.High:
				amount = MathHelper.SCurve5(x - (double)num);
				break;
			}
			return MathHelper.Lerp(GradNoise(x, num, seed), GradNoise(x, num + 1, seed), amount);
		}

		protected double GradCoherentNoise(double x, double y, int seed, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			int num2 = MathHelper.Floor(y);
			int ix = num + 1;
			int iy = num2 + 1;
			double amount = 0.0;
			double amount2 = 0.0;
			switch (quality)
			{
			case MyNoiseQuality.Low:
				amount = x - (double)num;
				amount2 = y - (double)num2;
				break;
			case MyNoiseQuality.Standard:
				amount = MathHelper.SCurve3(x - (double)num);
				amount2 = MathHelper.SCurve3(y - (double)num2);
				break;
			case MyNoiseQuality.High:
				amount = MathHelper.SCurve5(x - (double)num);
				amount2 = MathHelper.SCurve5(y - (double)num2);
				break;
			}
			return MathHelper.Lerp(MathHelper.Lerp(GradNoise(x, y, num, num2, seed), GradNoise(x, y, ix, num2, seed), amount), MathHelper.Lerp(GradNoise(x, y, num, iy, seed), GradNoise(x, y, ix, iy, seed), amount), amount2);
		}

		protected double GradCoherentNoise(double x, double y, double z, int seed, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			int num2 = MathHelper.Floor(y);
			int num3 = MathHelper.Floor(z);
			int ix = num + 1;
			int iy = num2 + 1;
			int iz = num3 + 1;
			double amount = 0.0;
			double amount2 = 0.0;
			double amount3 = 0.0;
			switch (quality)
			{
			case MyNoiseQuality.Low:
				amount = x - (double)num;
				amount2 = y - (double)num2;
				amount3 = z - (double)num3;
				break;
			case MyNoiseQuality.Standard:
				amount = MathHelper.SCurve3(x - (double)num);
				amount2 = MathHelper.SCurve3(y - (double)num2);
				amount3 = MathHelper.SCurve3(z - (double)num3);
				break;
			case MyNoiseQuality.High:
				amount = MathHelper.SCurve5(x - (double)num);
				amount2 = MathHelper.SCurve5(y - (double)num2);
				amount3 = MathHelper.SCurve5(z - (double)num3);
				break;
			}
			return MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp(GradNoise(x, y, z, num, num2, num3, seed), GradNoise(x, y, z, ix, num2, num3, seed), amount), MathHelper.Lerp(GradNoise(x, y, z, num, iy, num3, seed), GradNoise(x, y, z, ix, iy, num3, seed), amount), amount2), MathHelper.Lerp(MathHelper.Lerp(GradNoise(x, y, z, num, num2, iz, seed), GradNoise(x, y, z, ix, num2, iz, seed), amount), MathHelper.Lerp(GradNoise(x, y, z, num, iy, iz, seed), GradNoise(x, y, z, ix, iy, iz, seed), amount), amount2), amount3);
		}

		public abstract double GetValue(double x);

		public abstract double GetValue(double x, double y);

		public abstract double GetValue(double x, double y, double z);
	}
}
