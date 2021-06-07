using System.Diagnostics;

namespace System
{
	public static class FloatExtensions
	{
		/// <summary>
		/// Returns true if float is not NaN or infinity.
		/// </summary>
		public static bool IsValid(this float f)
		{
			if (!float.IsNaN(f))
			{
				return !float.IsInfinity(f);
			}
			return false;
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(this float f)
		{
		}

		public static bool IsEqual(this float f, float other, float epsilon = 0.0001f)
		{
			if (f != other)
			{
				return (f - other).IsZero(epsilon);
			}
			return true;
		}

		public static bool IsZero(this float f, float epsilon = 0.0001f)
		{
			return Math.Abs(f) < epsilon;
		}

		public static bool IsInt(this float f, float epsilon = 0.0001f)
		{
			return Math.Abs(f % 1f) <= epsilon;
		}
	}
}
