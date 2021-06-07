using System;

namespace VRage.Noise
{
	public class MyBillowFast : MyModuleFast
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

		public MyBillowFast(MyNoiseQuality quality = MyNoiseQuality.Standard, int layerCount = 6, int seed = 0, double frequency = 1.0, double lacunarity = 2.0, double persistence = 0.5)
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
				_ = Seed;
				num2 = GradCoherentNoise(x, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				num3 *= Persistence;
			}
			return num + 0.5;
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
				_ = Seed;
				num2 = GradCoherentNoise(x, y, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				y *= Lacunarity;
				num3 *= Persistence;
			}
			return num + 0.5;
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
				_ = Seed;
				num2 = GradCoherentNoise(x, y, z, Quality);
				num2 = 2.0 * Math.Abs(num2) - 1.0;
				num += num2 * num3;
				x *= Lacunarity;
				y *= Lacunarity;
				z *= Lacunarity;
				num3 *= Persistence;
			}
			return num + 0.5;
		}
	}
}
