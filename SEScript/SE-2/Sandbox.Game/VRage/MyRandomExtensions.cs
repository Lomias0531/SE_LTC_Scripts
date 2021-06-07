using System;
using VRage.Library.Utils;
using VRageMath;

namespace VRage
{
	public static class MyRandomExtensions
	{
		public static float FloatNormal(this MyRandom rnd)
		{
			double d = rnd.NextDouble();
			double num = rnd.NextDouble();
			double num2 = Math.Sqrt(-2.0 * Math.Log(d));
			double a = Math.PI * 2.0 * num;
			return (float)(num2 * Math.Sin(a));
		}

		public static float FloatNormal(this MyRandom rnd, float mean, float standardDeviation)
		{
			if ((double)standardDeviation <= 0.0)
			{
				throw new ArgumentOutOfRangeException($"Shape must be positive. Received {standardDeviation}.");
			}
			return mean + standardDeviation * rnd.FloatNormal();
		}

		public static float FloatExponential(this MyRandom rnd, float mean)
		{
			if ((double)mean <= 0.0)
			{
				throw new ArgumentOutOfRangeException($"Mean of exponential distribution must be positive. Received {mean}.");
			}
			return (float)((0.0 - Math.Log(rnd.NextDouble())) * (double)mean);
		}

		public static float phi(float x)
		{
			float num = 1f;
			if (x < 0f)
			{
				num = -1f;
			}
			x = (float)((double)Math.Abs(x) / Math.Sqrt(2.0));
			float num2 = 1f / (1f + 0.3275911f * x);
			float num3 = 1f - ((((1.06140542f * num2 + -1.45315206f) * num2 + 1.42141378f) * num2 + -0.284496725f) * num2 + 0.2548296f) * num2 * (float)Math.Exp((0f - x) * x);
			return 0.5f * (1f + num * num3);
		}

		public static float NextFloat(this MyRandom random, float minValue, float maxValue)
		{
			return (float)random.NextDouble() * (maxValue - minValue) + minValue;
		}

		public static Vector3 NextDeviatingVector(this MyRandom random, ref Matrix matrix, float maxAngle)
		{
			float angle = random.NextFloat(0f - maxAngle, maxAngle);
			float angle2 = random.NextFloat(0f, MathF.PI * 2f);
			return Vector3.TransformNormal(-new Vector3(MyMath.FastSin(angle) * MyMath.FastCos(angle2), MyMath.FastSin(angle) * MyMath.FastSin(angle2), MyMath.FastCos(angle)), matrix);
		}
	}
}
