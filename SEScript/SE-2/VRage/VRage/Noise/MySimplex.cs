using VRageMath;

namespace VRage.Noise
{
	public class MySimplex : IMyModule
	{
		private int m_seed;

		private byte[] m_perm = new byte[512];

		public int Seed
		{
			get
			{
				return m_seed;
			}
			set
			{
				m_seed = value;
				MyRNG myRNG = new MyRNG(m_seed);
				for (int i = 0; i < 256; i++)
				{
					m_perm[i] = (byte)myRNG.NextIntRange(0f, 255f);
					m_perm[256 + i] = m_perm[i];
				}
			}
		}

		public double Frequency
		{
			get;
			set;
		}

		private static double Grad(int hash, double x)
		{
			int num = hash & 0xF;
			double num2 = 1.0 + (double)(num & 7);
			if ((num & 8) != 0)
			{
				num2 = 0.0 - num2;
			}
			return num2 * x;
		}

		private static double Grad(int hash, double x, double y)
		{
			int num = hash & 7;
			double num2 = (num < 4) ? x : y;
			double num3 = (num < 4) ? y : x;
			return (((num & 1) != 0) ? (0.0 - num2) : num2) + (((num & 2) != 0) ? (-2.0 * num3) : (2.0 * num3));
		}

		private static double Grad(int hash, double x, double y, double z)
		{
			int num = hash & 0xF;
			double num2 = (num < 8) ? x : y;
			double num3 = (num < 4) ? y : ((num == 12 || num == 14) ? x : z);
			return (((num & 1) != 0) ? num2 : (0.0 - num2)) + (((num & 2) != 0) ? num3 : (0.0 - num3));
		}

		public MySimplex(int seed = 1, double frequency = 1.0)
		{
			Seed = seed;
			Frequency = frequency;
		}

		public double GetValue(double x)
		{
			x *= Frequency;
			int num = MathHelper.Floor(x);
			double num2 = x - (double)num;
			double num3 = num2 - 1.0;
			double num4 = 1.0 - num2 * num2;
			double num5 = 1.0 - num3 * num3;
			num4 *= num4;
			double num6 = num5 * num5;
			double num7 = num4 * num4 * Grad(m_perm[num & 0xFF], num2);
			double num8 = num6 * num6 * Grad(m_perm[(num + 1) & 0xFF], num3);
			return 0.395 * (num7 + num8);
		}

		public double GetValue(double x, double y)
		{
			x *= Frequency;
			y *= Frequency;
			double num = (x + y) * 0.3660254037844386;
			int num2 = MathHelper.Floor(x + num);
			int num3 = MathHelper.Floor(y + num);
			double num4 = (double)(num2 + num3) * 0.21132486540518711;
			double num5 = x - (double)num2 + num4;
			double num6 = y - (double)num3 + num4;
			int num7;
			int num8;
			if (num5 > num6)
			{
				num7 = 1;
				num8 = 0;
			}
			else
			{
				num7 = 0;
				num8 = 1;
			}
			double num9 = num5 - (double)num7 + 0.21132486540518711;
			double num10 = num6 - (double)num8 + 0.21132486540518711;
			double num11 = num5 - 1.0 + 0.21132486540518711 + 0.21132486540518711;
			double num12 = num6 - 1.0 + 0.21132486540518711 + 0.21132486540518711;
			int num13 = num2 & 0xFF;
			int num14 = num3 & 0xFF;
			double num15 = 0.5 - num5 * num5 - num6 * num6;
			double num16 = 0.5 - num9 * num9 - num10 * num10;
			double num17 = 0.5 - num11 * num11 - num12 * num12;
			double num18;
			if (num15 < 0.0)
			{
				num18 = 0.0;
			}
			else
			{
				num15 *= num15;
				num18 = num15 * num15 * Grad(m_perm[num13 + m_perm[num14]], num5, num6);
			}
			double num19;
			if (num16 < 0.0)
			{
				num19 = 0.0;
			}
			else
			{
				num16 *= num16;
				num19 = num16 * num16 * Grad(m_perm[num13 + num7 + m_perm[num14 + num8]], num9, num10);
			}
			double num20;
			if (num17 < 0.0)
			{
				num20 = 0.0;
			}
			else
			{
				num17 *= num17;
				num20 = num17 * num17 * Grad(m_perm[num13 + 1 + m_perm[num14 + 1]], num11, num12);
			}
			return 40.0 * (num18 + num19 + num20);
		}

		public double GetValue(double x, double y, double z)
		{
			x *= Frequency;
			y *= Frequency;
			z *= Frequency;
			double num = (x + y + z) * 0.33333333333333331;
			int num2 = MathHelper.Floor(x + num);
			int num3 = MathHelper.Floor(y + num);
			int num4 = MathHelper.Floor(z + num);
			double num5 = (double)(num2 + num3 + num4) * 0.16666666666666671;
			double num6 = x - (double)num2 + num5;
			double num7 = y - (double)num3 + num5;
			double num8 = z - (double)num4 + num5;
			int num9;
			int num10;
			int num11;
			int num12;
			int num13;
			int num14;
			if (num6 >= num7)
			{
				if (num7 >= num8)
				{
					num9 = 1;
					num10 = 0;
					num11 = 0;
					num12 = 1;
					num13 = 1;
					num14 = 0;
				}
				else if (num6 >= num8)
				{
					num9 = 1;
					num10 = 0;
					num11 = 0;
					num12 = 1;
					num13 = 0;
					num14 = 1;
				}
				else
				{
					num9 = 0;
					num10 = 0;
					num11 = 1;
					num12 = 1;
					num13 = 0;
					num14 = 1;
				}
			}
			else if (num7 < num8)
			{
				num9 = 0;
				num10 = 0;
				num11 = 1;
				num12 = 0;
				num13 = 1;
				num14 = 1;
			}
			else if (num6 < num8)
			{
				num9 = 0;
				num10 = 1;
				num11 = 0;
				num12 = 0;
				num13 = 1;
				num14 = 1;
			}
			else
			{
				num9 = 0;
				num10 = 1;
				num11 = 0;
				num12 = 1;
				num13 = 1;
				num14 = 0;
			}
			double num15 = num6 - (double)num9 + 0.16666666666666671;
			double num16 = num7 - (double)num10 + 0.16666666666666671;
			double num17 = num8 - (double)num11 + 0.16666666666666671;
			double num18 = num6 - (double)num12 + 0.16666666666666671 + 0.16666666666666671;
			double num19 = num7 - (double)num13 + 0.16666666666666671 + 0.16666666666666671;
			double num20 = num8 - (double)num14 + 0.16666666666666671 + 0.16666666666666671;
			double num21 = num6 - 1.0 + 0.16666666666666671 + 0.16666666666666671 + 0.16666666666666671;
			double num22 = num7 - 1.0 + 0.16666666666666671 + 0.16666666666666671 + 0.16666666666666671;
			double num23 = num8 - 1.0 + 0.16666666666666671 + 0.16666666666666671 + 0.16666666666666671;
			int num24 = num2 & 0xFF;
			int num25 = num3 & 0xFF;
			int num26 = num4 & 0xFF;
			double num27 = 0.6 - num6 * num6 - num7 * num7 - num8 * num8;
			double num28 = 0.6 - num15 * num15 - num16 * num16 - num17 * num17;
			double num29 = 0.6 - num18 * num18 - num19 * num19 - num20 * num20;
			double num30 = 0.6 - num21 * num21 - num22 * num22 - num23 * num23;
			double num31;
			if (num27 < 0.0)
			{
				num31 = 0.0;
			}
			else
			{
				num27 *= num27;
				num31 = num27 * num27 * Grad(m_perm[num24 + m_perm[num25 + m_perm[num26]]], num6, num7, num8);
			}
			double num32;
			if (num28 < 0.0)
			{
				num32 = 0.0;
			}
			else
			{
				num28 *= num28;
				num32 = num28 * num28 * Grad(m_perm[num24 + num9 + m_perm[num25 + num10 + m_perm[num26 + num11]]], num15, num16, num17);
			}
			double num33;
			if (num29 < 0.0)
			{
				num33 = 0.0;
			}
			else
			{
				num29 *= num29;
				num33 = num29 * num29 * Grad(m_perm[num24 + num12 + m_perm[num25 + num13 + m_perm[num26 + num14]]], num18, num19, num20);
			}
			double num34;
			if (num30 < 0.0)
			{
				num34 = 0.0;
			}
			else
			{
				num30 *= num30;
				num34 = num30 * num30 * Grad(m_perm[num24 + 1 + m_perm[num25 + 1 + m_perm[num26 + 1]]], num21, num22, num23);
			}
			return 32.0 * (num31 + num32 + num33 + num34);
		}
	}
}
