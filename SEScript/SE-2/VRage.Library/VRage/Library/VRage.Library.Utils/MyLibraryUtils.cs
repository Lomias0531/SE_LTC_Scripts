using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VRage.Library.Utils
{
	public class MyLibraryUtils
	{
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void AssertBlittable<T>()
		{
			try
			{
				if (default(T) != null)
				{
					GCHandle.Alloc(default(T), GCHandleType.Pinned).Free();
				}
			}
			catch
			{
			}
		}

		public static void ThrowNonBlittable<T>()
		{
			try
			{
				if (default(T) == null)
				{
					throw new InvalidOperationException("Class is never blittable");
				}
				GCHandle.Alloc(default(T), GCHandleType.Pinned).Free();
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(string.Concat("Type '", typeof(T), "' is not blittable"), innerException);
			}
		}

		/// <summary>
		/// Normalizes uniform-spaced float within min/max into uint with specified number of bits.
		/// This does not preserve 0 when min = -max
		/// </summary>
		public static uint NormalizeFloat(float value, float min, float max, int bits)
		{
			int num = (1 << bits) - 1;
			value = (value - min) / (max - min);
			return (uint)(value * (float)num + 0.5f);
		}

		/// <summary>
		/// Denormalizes uint with specified number of bits into uniform-space float within min/max.
		/// This does not preserve 0 when min = -max
		/// </summary>
		public static float DenormalizeFloat(uint value, float min, float max, int bits)
		{
			int num = (1 << bits) - 1;
			float num2 = (float)value / (float)num;
			return min + num2 * (max - min);
		}

		/// <summary>
		/// Normalizes uniform-spaced float within min/max into uint with specified number of bits.
		/// This preserves 0 when min = -max
		/// </summary>
		public static uint NormalizeFloatCenter(float value, float min, float max, int bits)
		{
			int num = (1 << bits) - 2;
			value = (value - min) / (max - min);
			return (uint)(value * (float)num + 0.5f);
		}

		/// <summary>
		/// Denormalizes uint with specified number of bits into uniform-space float within min/max.
		/// This preserves 0 when min = -max
		/// </summary>
		public static float DenormalizeFloatCenter(uint value, float min, float max, int bits)
		{
			int num = (1 << bits) - 2;
			float num2 = (float)value / (float)num;
			return min + num2 * (max - min);
		}

		public static int GetDivisionCeil(int num, int div)
		{
			return (num - 1) / div + 1;
		}
	}
}
