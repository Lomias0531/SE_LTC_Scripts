using System;
using VRageMath;

namespace VRage.Noise
{
	public class MyRidgedMultifractal : MyModule
	{
		private const int MAX_OCTAVES = 20;

		private double m_lacunarity;

		private double[] m_spectralWeights = new double[20];

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

		public double Gain
		{
			get;
			set;
		}

		public double Lacunarity
		{
			get
			{
				return m_lacunarity;
			}
			set
			{
				m_lacunarity = value;
				CalculateSpectralWeights();
			}
		}

		public double Offset
		{
			get;
			set;
		}

		private void CalculateSpectralWeights()
		{
			double num = 1.0;
			double num2 = 1.0;
			for (int i = 0; i < 20; i++)
			{
				m_spectralWeights[i] = Math.Pow(num2, 0.0 - num);
				num2 *= Lacunarity;
			}
		}

		public MyRidgedMultifractal(MyNoiseQuality quality = MyNoiseQuality.Standard, int layerCount = 6, int seed = 0, double frequency = 1.0, double gain = 2.0, double lacunarity = 2.0, double offset = 1.0)
		{
			Quality = quality;
			LayerCount = layerCount;
			Seed = seed;
			Frequency = frequency;
			Gain = gain;
			Lacunarity = lacunarity;
			Offset = offset;
		}

		public override double GetValue(double x)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 1.0;
			x *= Frequency;
			for (int i = 0; i < LayerCount; i++)
			{
				long num4 = (Seed + i) & int.MaxValue;
				num2 = GradCoherentNoise(x, (int)num4, Quality);
				num2 = Math.Abs(num2);
				num2 = Offset - num2;
				num2 *= num2;
				num2 *= num3;
				num3 = MathHelper.Saturate(num2 * Gain);
				num += num2 * m_spectralWeights[i];
				x *= Lacunarity;
			}
			return num - 1.0;
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
				long num4 = (Seed + i) & int.MaxValue;
				num2 = GradCoherentNoise(x, y, (int)num4, Quality);
				num2 = Math.Abs(num2);
				num2 = Offset - num2;
				num2 *= num2;
				num2 *= num3;
				num3 = MathHelper.Saturate(num2 * Gain);
				num += num2 * m_spectralWeights[i];
				x *= Lacunarity;
				y *= Lacunarity;
			}
			return num - 1.0;
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
				long num4 = (Seed + i) & int.MaxValue;
				num2 = GradCoherentNoise(x, y, z, (int)num4, Quality);
				num2 = Math.Abs(num2);
				num2 = Offset - num2;
				num2 *= num2;
				num2 *= num3;
				num3 = MathHelper.Saturate(num2 * Gain);
				num += num2 * m_spectralWeights[i];
				x *= Lacunarity;
				y *= Lacunarity;
				z *= Lacunarity;
			}
			return num * 1.25 - 1.0;
		}
	}
}
