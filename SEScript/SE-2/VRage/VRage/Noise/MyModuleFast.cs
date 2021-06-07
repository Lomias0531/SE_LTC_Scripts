using System;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Noise
{
	/// <summary>
	/// Faster version of MyModule.
	/// This time we do not compute the gradient position directly but we're using gradient table lookup via permutation table.
	/// This leads to more 'grid' result as the local min and max (like in Value noise) are always appearing at integer points.
	/// </summary>
	public abstract class MyModuleFast : IMyModule
	{
		private int m_seed;

		private byte[] m_perm = new byte[512];

		private float[] m_grad = new float[512];

		public virtual int Seed
		{
			get
			{
				return m_seed;
			}
			set
			{
				m_seed = value;
				Random random = new Random(MyRandom.EnableDeterminism ? 1 : m_seed);
				for (int i = 0; i < 256; i++)
				{
					byte b = (byte)random.Next(255);
					m_perm[i] = b;
					m_perm[256 + i] = b;
					m_grad[i] = -1f + 2f * ((float)(int)m_perm[i] / 255f);
					m_grad[256 + i] = m_grad[i];
				}
			}
		}

		protected double GradCoherentNoise(double x, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			int num2 = num & 0xFF;
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
			return MathHelper.Lerp(m_grad[m_perm[num2]], m_grad[m_perm[num2 + 1]], amount);
		}

		protected double GradCoherentNoise(double x, double y, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			int num2 = MathHelper.Floor(y);
			int num3 = num & 0xFF;
			int num4 = num2 & 0xFF;
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
			int num5 = m_perm[num3] + num4;
			int num6 = m_perm[num3 + 1] + num4;
			int num7 = m_perm[num5];
			int num8 = m_perm[num5 + 1];
			int num9 = m_perm[num6];
			int num10 = m_perm[num6 + 1];
			return MathHelper.Lerp(MathHelper.Lerp(m_grad[num7], m_grad[num9], amount), MathHelper.Lerp(m_grad[num8], m_grad[num10], amount), amount2);
		}

		protected double GradCoherentNoise(double x, double y, double z, MyNoiseQuality quality)
		{
			int num = MathHelper.Floor(x);
			int num2 = MathHelper.Floor(y);
			int num3 = MathHelper.Floor(z);
			int num4 = num & 0xFF;
			int num5 = num2 & 0xFF;
			int num6 = num3 & 0xFF;
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
			int num7 = m_perm[num4] + num5;
			int num8 = m_perm[num4 + 1] + num5;
			int num9 = m_perm[num7] + num6;
			int num10 = m_perm[num7 + 1] + num6;
			int num11 = m_perm[num8] + num6;
			int num12 = m_perm[num8 + 1] + num6;
			return MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp(m_grad[num9], m_grad[num11], amount), MathHelper.Lerp(m_grad[num10], m_grad[num12], amount), amount2), MathHelper.Lerp(MathHelper.Lerp(m_grad[num9 + 1], m_grad[num11 + 1], amount), MathHelper.Lerp(m_grad[num10 + 1], m_grad[num12 + 1], amount), amount2), amount3);
		}

		public abstract double GetValue(double x);

		public abstract double GetValue(double x, double y);

		public abstract double GetValue(double x, double y, double z);
	}
}
