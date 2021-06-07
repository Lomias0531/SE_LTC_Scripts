using System;
using VRageMath;

namespace VRage.Noise
{
	public static class MyNoise2D
	{
		private static MyRNG m_rnd = default(MyRNG);

		private const int B = 256;

		private const int BM = 255;

		private static float[] rand = new float[256];

		private static int[] perm = new int[512];

		public static void Init(int seed)
		{
			m_rnd.Seed = (uint)seed;
			for (int i = 0; i < 256; i++)
			{
				rand[i] = m_rnd.NextFloat();
				perm[i] = i;
			}
			for (int j = 0; j < 256; j++)
			{
				int num = (int)(m_rnd.NextInt() & 0xFF);
				int num2 = perm[num];
				perm[num] = perm[j];
				perm[j] = num2;
				perm[j + 256] = perm[j];
			}
		}

		public static float Noise(float x, float y)
		{
			int num = (int)x;
			int num2 = (int)y;
			float amount = x - (float)num;
			float amount2 = y - (float)num2;
			int num3 = 0xFF & num;
			int num4 = 0xFF & (num + 1);
			int num5 = 0xFF & num2;
			int num6 = 0xFF & (num2 + 1);
			float value = rand[perm[perm[num3] + num5]];
			float value2 = rand[perm[perm[num4] + num5]];
			float value3 = rand[perm[perm[num3] + num6]];
			return MathHelper.SmoothStep(value2: MathHelper.SmoothStep(value3, rand[perm[perm[num4] + num6]], amount), value1: MathHelper.SmoothStep(value, value2, amount), amount: amount2);
		}

		public static float Rotation(float x, float y, int numLayers)
		{
			float[] array = new float[numLayers];
			float[] array2 = new float[numLayers];
			for (int i = 0; i < numLayers; i++)
			{
				array[i] = (float)Math.Sin(0.436332315f * (float)i);
				array2[i] = (float)Math.Cos(0.436332315f * (float)i);
			}
			float num = 0f;
			int num2 = 0;
			for (int j = 0; j < numLayers; j++)
			{
				num += Noise(x * array2[j] - y * array[j], x * array[j] + y * array2[j]);
				num2++;
			}
			return num / (float)num2;
		}

		public static float Fractal(float x, float y, int numOctaves)
		{
			int num = 1;
			float num2 = 1f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < numOctaves; i++)
			{
				num3 += num2;
				num4 += Noise(x * (float)num, y * (float)num) * num2;
				num2 *= 0.5f;
				num <<= 1;
			}
			return num4 / num3;
		}

		public static float FBM(float x, float y, int numLayers, float lacunarity, float gain)
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < numLayers; i++)
			{
				num3 += num2;
				num4 += Noise(x * num, y * num) * num2;
				num2 *= gain;
				num *= lacunarity;
			}
			return num4 / num3;
		}

		public static float Billow(float x, float y, int numLayers)
		{
			int num = 1;
			float num2 = 1f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < numLayers; i++)
			{
				num3 += num2;
				num4 += Math.Abs(2f * Noise(x * (float)num, y * (float)num) - 1f) * num2;
				num2 *= 0.5f;
				num <<= 1;
			}
			return num4 / num3;
		}

		public static float Marble(float x, float y, int numOctaves)
		{
			return ((float)Math.Sin(4f * (x + Fractal(x * 0.5f, y * 0.5f, numOctaves))) + 1f) * 0.5f;
		}

		public static float Wood(float x, float y, float scale)
		{
			float num = Noise(x, y) * scale;
			return num - (float)(int)num;
		}
	}
}
