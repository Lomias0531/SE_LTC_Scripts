using System;
using System.IO;
using System.Text;

namespace VRage.Library.Utils
{
	public static class MyHashRandomUtils
	{
		/// <summary>
		/// Create a [0, 1) float from it's mantissa.
		/// </summary>
		/// <param name="m">Mantissa bits.</param>
		/// <returns></returns>
		public unsafe static float CreateFloatFromMantissa(uint m)
		{
			m &= 0x7FFFFF;
			m |= 0x3F800000;
			float num = *(float*)(&m);
			return num - 1f;
		}

		public static uint JenkinsHash(uint x)
		{
			x += x << 10;
			x ^= x >> 6;
			x += x << 3;
			x ^= x >> 11;
			x += x << 15;
			return x;
		}

		/// <summary>
		/// Compute a float in the range [0, 1) created from the the seed.
		///
		/// For uniformly distributed seeds this method will produce uniformly distributed values.
		/// </summary>
		/// <param name="seed">Any integer to be used as a seed. The seed needs not be super uniform since it will be hashed.</param>
		/// <returns>A float in the range [0, 1)</returns>
		public static float UniformFloatFromSeed(int seed)
		{
			return CreateFloatFromMantissa(JenkinsHash((uint)seed));
		}

		public static void TestHashSample()
		{
			float[] array = new float[100000000];
			using (new MySimpleTestTimer("Int to sample fast"))
			{
				for (int i = 0; i < 100000000; i++)
				{
					array[i] = UniformFloatFromSeed(i);
				}
			}
			float num = 0f;
			float num2 = float.MaxValue;
			float num3 = float.MinValue;
			for (int j = 0; j < 100000000; j++)
			{
				num += array[j];
				if (num2 > array[j])
				{
					num2 = array[j];
				}
				if (num3 < array[j])
				{
					num3 = array[j];
				}
			}
			num /= 1E+08f;
			float num4 = 0f;
			for (int k = 0; k < 100000000; k++)
			{
				float num5 = array[k] - num;
				num4 += num5 * num5;
			}
			num4 = (float)Math.Sqrt(num4) / 1E+08f;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Min/Max/Avg: {0}/{1}/{2}\n", num2, num3, num);
			stringBuilder.AppendFormat("Std dev: {0}\n", num4);
			File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "perf.log"), stringBuilder.ToString());
		}
	}
}
