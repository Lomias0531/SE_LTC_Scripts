using System;
using System.Runtime.InteropServices;
using VRage;

namespace VRageMath
{
	/// <summary>
	/// Contains commonly used precalculated values.
	/// </summary>
	public static class MathHelper
	{
		/// <summary>
		/// Represents the mathematical constant e.
		/// </summary>
		public const float E = 2.718282f;

		/// <summary>
		/// Represents the log base two of e.
		/// </summary>
		public const float Log2E = 1.442695f;

		/// <summary>
		/// Represents the log base ten of e.
		/// </summary>
		public const float Log10E = 0.4342945f;

		/// <summary>
		/// Represents the value of pi.
		/// </summary>
		public const float Pi = 3.141593f;

		/// <summary>
		/// Represents the value of pi times two.
		/// </summary>
		public const float TwoPi = MathF.PI * 2f;

		/// <summary>
		/// Represents the value of pi times two.
		/// </summary>
		public const float FourPi = MathF.PI * 4f;

		/// <summary>
		/// Represents the value of pi divided by two.
		/// </summary>
		public const float PiOver2 = MathF.E * 449f / 777f;

		/// <summary>
		/// Represents the value of pi divided by four.
		/// </summary>
		public const float PiOver4 = MathF.PI / 4f;

		/// <summary>
		/// Represents the value of the square root of two
		/// </summary>
		public const float Sqrt2 = 1.41421354f;

		/// <summary>
		/// Represents the value of the square root of three
		/// </summary>
		public const float Sqrt3 = 1.73205078f;

		/// <summary>
		/// 60 / 2*pi
		/// </summary>
		public const float RadiansPerSecondToRPM = 30f / MathF.PI;

		/// <summary>
		/// 2*pi / 60
		/// </summary>
		public const float RPMToRadiansPerSecond = MathF.PI / 30f;

		/// <summary>
		/// 2*pi / 60000
		/// </summary>
		public const float RPMToRadiansPerMillisec = 0.000104719758f;

		public const float EPSILON = 1E-05f;

		public const float EPSILON10 = 1E-06f;

		private static readonly int[] lof2floor_lut = new int[32]
		{
			0,
			9,
			1,
			10,
			13,
			21,
			2,
			29,
			11,
			14,
			16,
			18,
			22,
			25,
			3,
			30,
			8,
			12,
			20,
			28,
			15,
			17,
			24,
			7,
			19,
			27,
			23,
			6,
			26,
			5,
			4,
			31
		};

		private const float SMOOTHING = 0.95f;

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">The angle in degrees.</param>
		public static float ToRadians(float degrees)
		{
			return degrees * (MathF.PI / 180f);
		}

		public static Vector3 ToRadians(Vector3 v)
		{
			return v * (MathF.PI / 180f);
		}

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">The angle in degrees.</param>
		public static double ToRadians(double degrees)
		{
			return degrees * (Math.PI / 180.0);
		}

		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">The angle in radians.</param>
		public static float ToDegrees(float radians)
		{
			return radians * 57.29578f;
		}

		public static double ToDegrees(double radians)
		{
			return radians * (180.0 / Math.PI);
		}

		/// <summary>
		/// Calculates the absolute value of the difference of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static float Distance(float value1, float value2)
		{
			return Math.Abs(value1 - value2);
		}

		/// <summary>
		/// Returns the lesser of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static float Min(float value1, float value2)
		{
			return Math.Min(value1, value2);
		}

		/// <summary>
		/// Returns the greater of two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param>
		public static float Max(float value1, float value2)
		{
			return Math.Max(value1, value2);
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
		public static float Clamp(float value, float min, float max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
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

		/// <summary>
		/// Restricts a value to be within a specified range. Reference page contains links to related code samples.
		/// </summary>
		/// <param name="value">The value to clamp.</param><param name="min">The minimum value. If value is less than min, min will be returned.</param><param name="max">The maximum value. If value is greater than max, max will be returned.</param>
		public static MyFixedPoint Clamp(MyFixedPoint value, MyFixedPoint min, MyFixedPoint max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
		}

		/// <summary>
		/// Restricts a value to be within a specified range. Reference page contains links to related code samples.
		/// </summary>
		/// <param name="value">The value to clamp.</param><param name="min">The minimum value. If value is less than min, min will be returned.</param><param name="max">The maximum value. If value is greater than max, max will be returned.</param>
		public static int Clamp(int value, int min, int max)
		{
			value = ((value > max) ? max : value);
			value = ((value < min) ? min : value);
			return value;
		}

		/// <summary>
		/// Linearly interpolates between two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		/// <summary>
		/// Linearly interpolates between two values.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static double Lerp(double value1, double value2, double amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		/// <summary>
		/// Performs interpolation on logarithmic scale.
		/// </summary>
		public static float InterpLog(float value, float amount1, float amount2)
		{
			return (float)(Math.Pow(amount1, 1.0 - (double)value) * Math.Pow(amount2, value));
		}

		public static float InterpLogInv(float value, float amount1, float amount2)
		{
			return (float)Math.Log(value / amount1, amount2 / amount1);
		}

		/// <summary>
		/// Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two normalized barycentric (areal) coordinates.
		/// </summary>
		/// <param name="value1">The coordinate on one axis of vertex 1 of the defining triangle.</param><param name="value2">The coordinate on the same axis of vertex 2 of the defining triangle.</param><param name="value3">The coordinate on the same axis of vertex 3 of the defining triangle.</param><param name="amount1">The normalized barycentric (areal) coordinate b2, equal to the weighting factor for vertex 2, the coordinate of which is specified in value2.</param><param name="amount2">The normalized barycentric (areal) coordinate b3, equal to the weighting factor for vertex 3, the coordinate of which is specified in value3.</param>
		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
		{
			return (float)((double)value1 + (double)amount1 * ((double)value2 - (double)value1) + (double)amount2 * ((double)value3 - (double)value1));
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static float SmoothStep(float value1, float value2, float amount)
		{
			return Lerp(value1, value2, SCurve3(amount));
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static double SmoothStep(double value1, double value2, double amount)
		{
			return Lerp(value1, value2, SCurve3(amount));
		}

		/// <summary>
		/// Interpolates between zero and one using cubic equiation, solved by de Casteljau.
		/// </summary>
		/// <param name="amount">Weighting value [0..1].</param>
		public static float SmoothStepStable(float amount)
		{
			float num = 1f - amount;
			float num2 = amount * amount;
			float num3 = amount * num + amount;
			return num2 * num + num3 * amount;
		}

		/// <summary>
		/// Interpolates between zero and one using cubic equiation, solved by de Casteljau.
		/// </summary>
		/// <param name="amount">Weighting value [0..1].</param>
		public static double SmoothStepStable(double amount)
		{
			double num = 1.0 - amount;
			double num2 = amount * amount;
			double num3 = amount * num + amount;
			return num2 * num + num3 * amount;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param>
		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			return (float)(0.5 * (2.0 * (double)value2 + (0.0 - (double)value1 + (double)value3) * (double)amount + (2.0 * (double)value1 - 5.0 * (double)value2 + 4.0 * (double)value3 - (double)value4) * (double)num + (0.0 - (double)value1 + 3.0 * (double)value2 - 3.0 * (double)value3 + (double)value4) * (double)num2));
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position.</param><param name="tangent1">Source tangent.</param><param name="value2">Source position.</param><param name="tangent2">Source tangent.</param><param name="amount">Weighting factor.</param>
		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			float num3 = (float)(2.0 * (double)num2 - 3.0 * (double)num + 1.0);
			float num4 = (float)(-2.0 * (double)num2 + 3.0 * (double)num);
			float num5 = num2 - 2f * num + amount;
			float num6 = num2 - num;
			return (float)((double)value1 * (double)num3 + (double)value2 * (double)num4 + (double)tangent1 * (double)num5 + (double)tangent2 * (double)num6);
		}

		public static Vector3D CalculateBezierPoint(double t, Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3)
		{
			double num = 1.0 - t;
			double num2 = t * t;
			double num3 = num * num;
			double scaleFactor = num3 * num;
			double scaleFactor2 = num2 * t;
			return scaleFactor * p0 + 3.0 * num3 * t * p1 + 3.0 * num * num2 * p2 + scaleFactor2 * p3;
		}

		/// <summary>
		/// Reduces a given angle to a value between π and -π.
		/// </summary>
		/// <param name="angle">The angle to reduce, in radians.</param>
		public static float WrapAngle(float angle)
		{
			angle = (float)Math.IEEERemainder(angle, 6.28318548202515);
			if ((double)angle <= -3.14159274101257)
			{
				angle += 6.283185f;
			}
			else if ((double)angle > 3.14159274101257)
			{
				angle -= 6.283185f;
			}
			return angle;
		}

		public static int GetNearestBiggerPowerOfTwo(int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}

		public static uint GetNearestBiggerPowerOfTwo(uint v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}

		public static int GetNumberOfMipmaps(int v)
		{
			int num = 0;
			while (v > 0)
			{
				v >>= 1;
				num++;
			}
			return num;
		}

		/// <summary>
		/// Returns nearest bigger power of two
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public static int GetNearestBiggerPowerOfTwo(float f)
		{
			int num = 1;
			while ((float)num < f)
			{
				num <<= 1;
			}
			return num;
		}

		public static int GetNearestBiggerPowerOfTwo(double f)
		{
			int num = 1;
			while ((double)num < f)
			{
				num <<= 1;
			}
			return num;
		}

		public static float Max(float a, float b, float c)
		{
			float num = (a > b) ? a : b;
			if (!(num > c))
			{
				return c;
			}
			return num;
		}

		public static int Max(int a, int b, int c)
		{
			int num = (a > b) ? a : b;
			if (num <= c)
			{
				return c;
			}
			return num;
		}

		public static float Min(float a, float b, float c)
		{
			float num = (a < b) ? a : b;
			if (!(num < c))
			{
				return c;
			}
			return num;
		}

		public static double Max(double a, double b, double c)
		{
			double num = (a > b) ? a : b;
			if (!(num > c))
			{
				return c;
			}
			return num;
		}

		public static double Min(double a, double b, double c)
		{
			double num = (a < b) ? a : b;
			if (!(num < c))
			{
				return c;
			}
			return num;
		}

		public unsafe static int ComputeHashFromBytes(byte[] bytes)
		{
			int num = bytes.Length;
			num -= num % 4;
			GCHandle gCHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			int num2 = 0;
			try
			{
				int* ptr = (int*)gCHandle.AddrOfPinnedObject().ToPointer();
				int num3 = 0;
				while (num3 < num)
				{
					num2 ^= *ptr;
					num3 += 4;
					ptr++;
				}
				return num2;
			}
			finally
			{
				gCHandle.Free();
			}
		}

		public static float RoundOn2(float x)
		{
			return (float)(int)(x * 100f) / 100f;
		}

		public static int RoundToInt(float x)
		{
			return (int)Math.Round(x);
		}

		public static int RoundToInt(double x)
		{
			return (int)Math.Round(x);
		}

		public static int FloorToInt(float x)
		{
			return (int)Math.Floor(x);
		}

		public static int FloorToInt(double x)
		{
			return (int)Math.Floor(x);
		}

		public static int CeilToInt(float x)
		{
			return (int)Math.Ceiling(x);
		}

		public static int CeilToInt(double x)
		{
			return (int)Math.Ceiling(x);
		}

		/// <summary>
		/// Returns true if value is power of two
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static bool IsPowerOfTwo(int x)
		{
			if (x > 0)
			{
				return (x & (x - 1)) == 0;
			}
			return false;
		}

		public static float SCurve3(float t)
		{
			return t * t * (3f - 2f * t);
		}

		public static double SCurve3(double t)
		{
			return t * t * (3.0 - 2.0 * t);
		}

		public static float SCurve5(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		public static double SCurve5(double t)
		{
			return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
		}

		public static float Saturate(float n)
		{
			if (!(n < 0f))
			{
				if (!(n > 1f))
				{
					return n;
				}
				return 1f;
			}
			return 0f;
		}

		public static double Saturate(double n)
		{
			if (!(n < 0.0))
			{
				if (!(n > 1.0))
				{
					return n;
				}
				return 1.0;
			}
			return 0.0;
		}

		public static int Floor(float n)
		{
			if (!(n < 0f))
			{
				return (int)n;
			}
			return (int)n - 1;
		}

		public static int Floor(double n)
		{
			if (!(n < 0.0))
			{
				return (int)n;
			}
			return (int)n - 1;
		}

		/// Fast integer Floor(Log2(value)).
		///
		/// Uses a DeBruijn-like method to find quickly the MSB.
		///
		/// Algorithm:
		/// https://en.wikipedia.org/wiki/De_Bruijn_sequence#Uses
		///
		/// This implementation:
		/// http://stackoverflow.com/a/11398748
		public static int Log2Floor(int value)
		{
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;
			return lof2floor_lut[(uint)(value * 130329821) >> 27];
		}

		/// Based on the above and this discussion:
		/// http://stackoverflow.com/questions/3272424/compute-fast-log-base-2-ceiling
		public static int Log2Ceiling(int value)
		{
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;
			value = lof2floor_lut[(uint)(value * 130329821) >> 27];
			if ((value & (value - 1)) == 0)
			{
				return value;
			}
			return value + 1;
		}

		public static int Log2(int n)
		{
			int num = 0;
			while ((n >>= 1) > 0)
			{
				num++;
			}
			return num;
		}

		public static int Log2(uint n)
		{
			int num = 0;
			while ((n >>= 1) != 0)
			{
				num++;
			}
			return num;
		}

		/// <summary>
		/// Returns 2^n
		/// </summary>
		public static int Pow2(int n)
		{
			return 1 << n;
		}

		public static double CubicInterp(double p0, double p1, double p2, double p3, double t)
		{
			double num = p3 - p2 - (p0 - p1);
			double num2 = p0 - p1 - num;
			double num3 = t * t;
			return num * num3 * t + num2 * num3 + (p2 - p0) * t + p1;
		}

		/// <summary>
		/// Returns angle in range 0..2*PI
		/// </summary>
		/// <param name="angle">in radians</param>
		public static void LimitRadians2PI(ref double angle)
		{
			if (angle > 6.2831854820251465)
			{
				angle %= 6.2831854820251465;
			}
			else if (angle < 0.0)
			{
				angle = angle % 6.2831854820251465 + 6.2831854820251465;
			}
		}

		/// <summary>
		/// Returns angle in range 0..2*PI
		/// </summary>
		/// <param name="angle">in radians</param>
		public static void LimitRadians(ref float angle)
		{
			if (angle > MathF.PI * 2f)
			{
				angle %= MathF.PI * 2f;
			}
			else if (angle < 0f)
			{
				angle = angle % (MathF.PI * 2f) + MathF.PI * 2f;
			}
		}

		/// <summary>
		/// Returns angle in range -PI..PI
		/// </summary>
		/// <param name="angle">radians</param>
		public static void LimitRadiansPI(ref double angle)
		{
			if (angle > 3.1415929794311523)
			{
				angle = angle % 3.1415929794311523 - 3.1415929794311523;
			}
			else if (angle < -3.1415929794311523)
			{
				angle = angle % 3.1415929794311523 + 3.1415929794311523;
			}
		}

		/// <summary>
		/// Returns angle in range -PI..PI
		/// </summary>
		/// <param name="angle">radians</param>
		public static void LimitRadiansPI(ref float angle)
		{
			if (angle > 3.141593f)
			{
				angle = angle % 3.141593f - 3.141593f;
			}
			else if (angle < 3.141593f)
			{
				angle = angle % 3.141593f + 3.141593f;
			}
		}

		public static Vector3 CalculateVectorOnSphere(Vector3 northPoleDir, float phi, float theta)
		{
			double num = Math.Sin(theta);
			return Vector3.TransformNormal(new Vector3(Math.Cos(phi) * num, Math.Sin(phi) * num, Math.Cos(theta)), Matrix.CreateFromDir(northPoleDir));
		}

		/// <summary>
		/// Calculate the monotonic cosine of a value.
		///
		/// Monotonic cosine is an alternative cosine encoding that is monotonic in the [-pi, pi] interval.
		/// We use this when some parameter of an onject in a planet is constrained by latitude.
		///
		/// The 'monotonicity' is guaranteed by subtracting the cosine value from 2 if the angle is positive.
		/// So for instance MonotonicCos(pi/2) = 2.
		///
		/// This only works in the above interval of course.
		///
		/// </summary>
		/// <param name="radians">The angle in radians.</param>
		/// <returns>The cosine of the angle if it is &gt; 0, 2 - that value otherwise.</returns>
		public static float MonotonicCosine(float radians)
		{
			if (radians > 0f)
			{
				return 2f - (float)Math.Cos(radians);
			}
			return (float)Math.Cos(radians);
		}

		public static float MonotonicAcos(float cos)
		{
			if (cos > 1f)
			{
				return (float)Math.Acos(2f - cos);
			}
			return (float)(0.0 - Math.Acos(cos));
		}

		/// <summary>
		/// Faster Atan implementation.
		///
		/// Good only in the [-pi/2, pi/2] range.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static float Atan(float x)
		{
			return 0.785375f * x - x * (Math.Abs(x) - 1f) * (0.2447f + 0.0663f * Math.Abs(x));
		}

		/// <summary>
		/// Faster Atan implementation.
		///
		/// Good only in the [-pi/2, pi/2] range.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static double Atan(double x)
		{
			return 0.785375 * x - x * (Math.Abs(x) - 1.0) * (0.2447 + 0.0663 * Math.Abs(x));
		}

		public static bool IsEqual(float value1, float value2)
		{
			return IsZero(value1 - value2);
		}

		public static bool IsEqual(Vector2 value1, Vector2 value2)
		{
			if (IsZero(value1.X - value2.X))
			{
				return IsZero(value1.Y - value2.Y);
			}
			return false;
		}

		public static bool IsEqual(Vector3 value1, Vector3 value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y))
			{
				return IsZero(value1.Z - value2.Z);
			}
			return false;
		}

		public static bool IsEqual(Quaternion value1, Quaternion value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y) && IsZero(value1.Z - value2.Z))
			{
				return IsZero(value1.W - value2.W);
			}
			return false;
		}

		public static bool IsEqual(QuaternionD value1, QuaternionD value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y) && IsZero(value1.Z - value2.Z))
			{
				return IsZero(value1.W - value2.W);
			}
			return false;
		}

		public static bool IsEqual(Matrix value1, Matrix value2)
		{
			if (IsZero(value1.Left - value2.Left) && IsZero(value1.Up - value2.Up) && IsZero(value1.Forward - value2.Forward))
			{
				return IsZero(value1.Translation - value2.Translation);
			}
			return false;
		}

		public static bool IsValid(Matrix matrix)
		{
			if (matrix.Up.IsValid() && matrix.Left.IsValid() && matrix.Forward.IsValid() && matrix.Translation.IsValid())
			{
				return matrix != Matrix.Zero;
			}
			return false;
		}

		public static bool IsValid(MatrixD matrix)
		{
			if (matrix.Up.IsValid() && matrix.Left.IsValid() && matrix.Forward.IsValid() && matrix.Translation.IsValid())
			{
				return matrix != MatrixD.Zero;
			}
			return false;
		}

		public static bool IsValid(Vector3 vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector3D vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector2 vec)
		{
			if (IsValid(vec.X))
			{
				return IsValid(vec.Y);
			}
			return false;
		}

		public static bool IsValid(float f)
		{
			if (!float.IsNaN(f))
			{
				return !float.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(double f)
		{
			if (!double.IsNaN(f))
			{
				return !double.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(Vector3? vec)
		{
			if (vec.HasValue)
			{
				if (IsValid(vec.Value.X) && IsValid(vec.Value.Y))
				{
					return IsValid(vec.Value.Z);
				}
				return false;
			}
			return true;
		}

		public static bool IsValid(Quaternion q)
		{
			if (IsValid(q.X) && IsValid(q.Y) && IsValid(q.Z) && IsValid(q.W))
			{
				return !IsZero(q);
			}
			return false;
		}

		public static bool IsValidNormal(Vector3 vec)
		{
			float num = vec.LengthSquared();
			if (vec.IsValid() && num > 0.999f)
			{
				return num < 1.001f;
			}
			return false;
		}

		public static bool IsValidOrZero(Matrix matrix)
		{
			if (IsValid(matrix.Up) && IsValid(matrix.Left) && IsValid(matrix.Forward))
			{
				return IsValid(matrix.Translation);
			}
			return false;
		}

		public static bool IsZero(float value, float epsilon = 1E-05f)
		{
			if (value > 0f - epsilon)
			{
				return value < epsilon;
			}
			return false;
		}

		public static bool IsZero(double value, float epsilon = 1E-05f)
		{
			if (value > (double)(0f - epsilon))
			{
				return value < (double)epsilon;
			}
			return false;
		}

		public static bool IsZero(Vector3 value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(Vector3D value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(Quaternion value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon) && IsZero(value.Z, epsilon))
			{
				return IsZero(value.W, epsilon);
			}
			return false;
		}

		public static bool IsZero(Vector4 value)
		{
			if (IsZero(value.X) && IsZero(value.Y) && IsZero(value.Z))
			{
				return IsZero(value.W);
			}
			return false;
		}

		public static int Smooth(int newValue, int lastSmooth)
		{
			return (int)((float)lastSmooth * 0.95f + (float)newValue * 0.0500000119f);
		}

		public static float Smooth(float newValue, float lastSmooth)
		{
			return lastSmooth * 0.95f + newValue * 0.0500000119f;
		}
	}
}
