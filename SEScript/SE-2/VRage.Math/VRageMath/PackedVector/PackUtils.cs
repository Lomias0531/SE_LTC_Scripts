using System;

namespace VRageMath.PackedVector
{
	internal static class PackUtils
	{
		public static uint PackUnsigned(float bitmask, float value)
		{
			return (uint)ClampAndRound(value, 0f, bitmask);
		}

		public static uint PackSigned(uint bitmask, float value)
		{
			float num = bitmask >> 1;
			float min = (float)(0.0 - (double)num - 1.0);
			return (uint)((int)ClampAndRound(value, min, num) & (int)bitmask);
		}

		public static uint PackUNorm(float bitmask, float value)
		{
			value *= bitmask;
			return (uint)ClampAndRound(value, 0f, bitmask);
		}

		public static float UnpackUNorm(uint bitmask, uint value)
		{
			value &= bitmask;
			return (float)value / (float)bitmask;
		}

		public static uint PackSNorm(uint bitmask, float value)
		{
			float num = bitmask >> 1;
			value *= num;
			return (uint)((int)ClampAndRound(value, 0f - num, num) & (int)bitmask);
		}

		public static float UnpackSNorm(uint bitmask, uint value)
		{
			uint num = bitmask + 1 >> 1;
			if ((value & num) != 0)
			{
				if ((value & bitmask) == num)
				{
					return -1f;
				}
				value |= ~bitmask;
			}
			else
			{
				value &= bitmask;
			}
			float num2 = bitmask >> 1;
			return (float)(int)value / num2;
		}

		private static double ClampAndRound(float value, float min, float max)
		{
			if (float.IsNaN(value))
			{
				return 0.0;
			}
			if (float.IsInfinity(value))
			{
				if (!float.IsNegativeInfinity(value))
				{
					return max;
				}
				return min;
			}
			if ((double)value < (double)min)
			{
				return min;
			}
			if ((double)value > (double)max)
			{
				return max;
			}
			return Math.Round(value);
		}
	}
}
