using System;

namespace VRageMath
{
	/// <summary>
	/// Contains commonly used precalculated values.
	/// </summary>
	public static class MathHelperD
	{
		/// <summary>
		/// Represents the mathematical constant e.
		/// </summary>
		public const double E = Math.E;

		/// <summary>
		/// Represents the value of pi.
		/// </summary>
		public const double Pi = Math.PI;

		/// <summary>
		/// Represents the value of pi times two.
		/// </summary>
		public const double TwoPi = Math.PI * 2.0;

		/// <summary>
		/// Represents the value of pi times four.
		/// </summary>
		public const double FourPi = Math.PI * 4.0;

		/// <summary>
		/// Represents the value of pi divided by two.
		/// </summary>
		public const double PiOver2 = Math.PI / 2.0;

		/// <summary>
		/// Represents the value of pi divided by four.
		/// </summary>
		public const double PiOver4 = Math.PI / 4.0;

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">The angle in degrees.</param>
		public static double ToRadians(double degrees)
		{
			return degrees / 180.0 * Math.PI;
		}

		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">The angle in radians.</param>
		public static double ToDegrees(double radians)
		{
			return radians * 180.0 / Math.PI;
		}

		/// <summary>
		/// Calculates the absolute value of the difference of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static double Distance(double value1, double value2)
		{
			return Math.Abs(value1 - value2);
		}

		/// <summary>
		/// Returns the lesser of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static double Min(double value1, double value2)
		{
			return Math.Min(value1, value2);
		}

		/// <summary>
		/// Returns the greater of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static double Max(double value1, double value2)
		{
			return Math.Max(value1, value2);
		}

		/// <summary>
		/// Restricts a value to be within a specified range. Reference page contains links to related code samples.
		/// </summary>
		/// <param name="value">The value to clamp.</param><param name="min">The minimum value. If value is less than min, min will be returned.</param><param name="max">The maximum value. If value is greater than max, max will be returned.</param>
		public static double Clamp(double value, double min, double max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
		}

		public static float MonotonicAcos(float cos)
		{
			if (cos > 1f)
			{
				return (float)Math.Acos(2f - cos);
			}
			return (float)(0.0 - Math.Acos(cos));
		}
	}
}
