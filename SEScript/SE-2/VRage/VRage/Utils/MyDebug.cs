#define TRACE
using System.Diagnostics;
using VRageMath;

namespace VRage.Utils
{
	public class MyDebug
	{
		/// <summary>
		/// This "assert" is executed in DEBUG and RELEASE modes. Use it in code that that won't suffer from more work (e.g. loading), not in frequently used loops
		/// </summary>
		/// <param name="condition"></param>
		public static void AssertRelease(bool condition)
		{
			AssertRelease(condition, "Assertion failed");
		}

		/// <summary>
		/// This "assert" is executed in DEBUG and RELEASE modes. Use it in code that that won't suffer from more work (e.g. loading), not in frequently used loops
		/// </summary>
		/// <param name="condition"></param>
		public static void AssertRelease(bool condition, string assertMessage)
		{
			if (!condition)
			{
				MyLog.Default.WriteLine("Assert: " + assertMessage);
				System.Diagnostics.Trace.Fail(assertMessage);
			}
		}

		/// <summary>
		/// Logs the message on release and also displays a message on DEBUG.
		/// </summary>
		/// <param name="message"></param>
		public static void FailRelease(string message)
		{
			MyLog.Default.WriteLine("Assert Fail: " + message);
			System.Diagnostics.Trace.Fail(message);
		}

		public static void FailRelease(string format, params object[] args)
		{
			string text = string.Format(format, args);
			MyLog.Default.WriteLine("Assert Fail: " + text);
			System.Diagnostics.Trace.Fail(text);
		}

		/// <summary>
		/// This "assert" is executed in DEBUG mode. Because people dont know how to use AssertRelease!
		/// </summary>
		/// <param name="condition"></param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void AssertDebug(bool condition)
		{
		}

		/// <summary>
		/// This "assert" is executed in DEBUG mode. Because people dont know how to use AssertRelease!
		/// </summary>
		/// <param name="condition"></param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void AssertDebug(bool condition, string assertMessage)
		{
		}

		/// <summary>
		/// Returns true if float is valid
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public static bool IsValid(float f)
		{
			if (!float.IsNaN(f))
			{
				return !float.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(double d)
		{
			if (!double.IsNaN(d))
			{
				return !double.IsInfinity(d);
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsValid(Vector3 vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
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

		public static bool IsValid(Vector3D vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector3D? vec)
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

		public static bool IsValidNormal(Vector3 vec)
		{
			float num = vec.LengthSquared();
			if (IsValid(vec) && num > 0.999f)
			{
				return num < 1.001f;
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector2 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static bool IsValid(Vector2 vec)
		{
			if (IsValid(vec.X))
			{
				return IsValid(vec.Y);
			}
			return false;
		}

		public static bool IsValid(Matrix matrix)
		{
			if (IsValid(matrix.Up) && IsValid(matrix.Left) && IsValid(matrix.Forward) && IsValid(matrix.Translation))
			{
				return matrix != Matrix.Zero;
			}
			return false;
		}

		public static bool IsValid(Quaternion q)
		{
			if (IsValid(q.X) && IsValid(q.Y) && IsValid(q.Z) && IsValid(q.W))
			{
				return !MyUtils.IsZero(q);
			}
			return false;
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void AssertIsValid(Vector3D vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void AssertIsValid(Vector3D? vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void AssertIsValid(Vector3 vec)
		{
		}

		/// <summary>
		/// Returns true if Vector3 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void AssertIsValid(Vector3? vec)
		{
		}

		/// <summary>
		/// Returns true if Vector2 is valid
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void AssertIsValid(Vector2 vec)
		{
		}

		/// <summary>
		/// Returns true if float is valid
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public static void AssertIsValid(float f)
		{
		}

		public static void AssertIsValid(Matrix matrix)
		{
		}

		public static void AssertIsValid(Quaternion q)
		{
		}
	}
}
