using System;
using VRageMath;

namespace VRage.Noise
{
	public class MyBillow : MyModule
	{
		public MyNoiseQuality Quality
		{
			get;
			set;
		}

		public int LayerCount
		{
			get;
			set;
		}

		public int Seed
		{
			get;
			set;
		}

		public double Frequency
		{
			get;
			set;
		}

		public double Lacunarity
		{
			get;
			set;
		}

		public double Persistence
		{
			get;
			set;
		}

		public MyBillow(MyNoiseQuality quality = MyNoiseQuality.Standard, int layerCount = 6, int seed = 0, double frequency = 1.0, double lacunarity = 2.0, double persistence = 0.5)
		{
			Quality = quality;
			LayerCount = layerCount;
			Seed = seed;
			Frequency = frequency;
			Lacunarity = lacunarity;
			Persistence = persistence;
		}

		public override double GetValue(double x)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 1.0;
			x *= Frequency;
			for (int i = 0; i < LayerCount; i++)
			{
				long num4 = (Seed + i) & uint.MaxValue;
				num2 = GradCoherentNoise(x, (int)num4, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				num3 *= Persistence;
			}
			return MathHelper.Clamp(num + 0.5, -1.0, 1.0);
		}

		public override double GetValue(double x, double y)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 1.0;
			x *= Frequency;
			y *= Frequency;
			for (int i = 0; i < LayerCount; i++)
			{
				long num4 = (Seed + i) & uint.MaxValue;
				num2 = GradCoherentNoise(x, y, (int)num4, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				y *= Lacunarity;
				num3 *= Persistence;
			}
			return MathHelper.Clamp(num + 0.5, -1.0, 1.0);
		}

		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 1.0;
			x *= Frequency;
			y *= Frequency;
			z *= Frequency;
			for (int i = 0; i < LayerCount; i++)
			{
				long num4 = (Seed + i) & uint.MaxValue;
				num2 = GradCoherentNoise(x, y, z, (int)num4, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				y *= Lacunarity;
				z *= Lacunarity;
				num3 *= Persistence;
			}
			return MathHelper.Clamp(num + 0.5, -1.0, 1.0);
		}
	}
}
