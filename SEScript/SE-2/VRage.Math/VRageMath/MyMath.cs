using System;

namespace VRageMath
{
	public static class MyMath
	{
		private const float Size = 10000f;

		private static int ANGLE_GRANULARITY = 0;

		private static float[] m_precomputedValues = null;

		private static Vector3[] m_corners = new Vector3[8];

		private static readonly float OneOverRoot3 = (float)Math.Pow(3.0, -0.5);

		public static Vector3 Vector3One = Vector3.One;

		public static void InitializeFastSin()
		{
			if (m_precomputedValues == null)
			{
				ANGLE_GRANULARITY = 62830;
				m_precomputedValues = new float[ANGLE_GRANULARITY];
				for (int i = 0; i < ANGLE_GRANULARITY; i++)
				{
					m_precomputedValues[i] = (float)Math.Sin((float)i / 10000f);
				}
			}
		}

		public static float FastSin(float angle)
		{
			int num = (int)(angle * 10000f);
			num %= ANGLE_GRANULARITY;
			if (num < 0)
			{
				num += ANGLE_GRANULARITY;
			}
			return m_precomputedValues[num];
		}

		public static float FastCos(float angle)
		{
			return FastSin(angle + MathF.E * 449f / 777f);
		}

		/// <summary>
		/// Fast approximation of Hyperbolic tangent
		/// Max deviation is &lt;3%
		/// </summary>
		public static float FastTanH(float x)
		{
			if (x < -3f)
			{
				return -1f;
			}
			if (x > 3f)
			{
				return 1f;
			}
			return x * (27f + x * x) / (27f + 9f * x * x);
		}

		public static float NormalizeAngle(float angle, float center = 0f)
		{
			return angle - MathF.PI * 2f * (float)Math.Floor((angle + 3.141593f - center) / (MathF.PI * 2f));
		}

		/// <summary>
		/// ArcTanAngle
		/// </summary>
		/// <returns>ArcTan angle between x and y</returns>
		public static float ArcTanAngle(float x, float y)
		{
			if (x == 0f)
			{
				if (y == 1f)
				{
					return MathF.E * 449f / 777f;
				}
				return MathF.E * -449f / 777f;
			}
			if (x > 0f)
			{
				return (float)Math.Atan(y / x);
			}
			if (x < 0f)
			{
				if (y > 0f)
				{
					return (float)Math.Atan(y / x) + 3.141593f;
				}
				return (float)Math.Atan(y / x) - 3.141593f;
			}
			return 0f;
		}

		public static Vector3 Abs(ref Vector3 vector)
		{
			return new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
		}

		/// <summary>
		/// Return vector with each component max
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3 MaxComponents(ref Vector3 a, ref Vector3 b)
		{
			return new Vector3(MathHelper.Max(a.X, b.X), MathHelper.Max(a.Y, b.Y), MathHelper.Max(a.Z, b.Z));
		}

		/// <summary>
		/// AngleTo 
		/// </summary>
		/// <returns>Angle between the vector lines</returns>
		public static Vector3 AngleTo(Vector3 From, Vector3 Location)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 vector = Vector3.Normalize(Location - From);
			zero.X = (float)Math.Asin(vector.Y);
			zero.Y = ArcTanAngle(0f - vector.Z, 0f - vector.X);
			return zero;
		}

		public static float AngleBetween(Vector3 a, Vector3 b)
		{
			float num = Vector3.Dot(a, b);
			float num2 = a.Length() * b.Length();
			float num3 = num / num2;
			if (Math.Abs(1f - num3) < 0.001f)
			{
				return 0f;
			}
			return (float)Math.Acos(num3);
		}

		public static float CosineDistance(ref Vector3 a, ref Vector3 b)
		{
			Vector3.Dot(ref a, ref b, out float result);
			return result / (a.Length() * b.Length());
		}

		public static double CosineDistance(ref Vector3D a, ref Vector3D b)
		{
			Vector3D.Dot(ref a, ref b, out double result);
			return result / (a.Length() * b.Length());
		}

		public static int Mod(int x, int m)
		{
			return (x % m + m) % m;
		}

		public static long Mod(long x, int m)
		{
			return (x % m + m) % m;
		}

		/// <summary>
		/// QuaternionToEuler 
		/// </summary>
		/// <returns>Converted quaternion to the euler pitch, rot, yaw</returns>
		public static Vector3 QuaternionToEuler(Quaternion Rotation)
		{
			Vector3 location = Vector3.Transform(Vector3.Forward, Rotation);
			Vector3 position = Vector3.Transform(Vector3.Up, Rotation);
			Vector3 result = AngleTo(default(Vector3), location);
			if (result.X == MathF.E * 449f / 777f)
			{
				result.Y = ArcTanAngle(position.Z, position.X);
				result.Z = 0f;
			}
			else if (result.X == MathF.E * -449f / 777f)
			{
				result.Y = ArcTanAngle(0f - position.Y, 0f - position.X);
				result.Z = 0f;
			}
			else
			{
				position = Vector3.Transform(position, Matrix.CreateRotationY(0f - result.Y));
				position = Vector3.Transform(position, Matrix.CreateRotationX(0f - result.X));
				result.Z = ArcTanAngle(position.Y, 0f - position.X);
			}
			return result;
		}

		/// <summary>
		/// This projection results to initial velocity of non-engine objects, which parents move in some velocity
		/// We want to add only forward speed of the parent to the forward direction of the object, and if parent
		/// is going backward, no speed is added.
		/// </summary>
		/// <param name="forwardVector"></param>
		/// <param name="projectedVector"></param>
		/// <returns></returns>
		public static Vector3 ForwardVectorProjection(Vector3 forwardVector, Vector3 projectedVector)
		{
			if (Vector3.Dot(projectedVector, forwardVector) > 0f)
			{
				return forwardVector.Project(projectedVector + forwardVector);
			}
			return Vector3.Zero;
		}

		public static BoundingBox CreateFromInsideRadius(float radius)
		{
			float value = OneOverRoot3 * radius;
			return new BoundingBox(-new Vector3(value), new Vector3(value));
		}

		/// <summary>
		/// Calculates color from vector
		/// </summary>
		public static Vector3 VectorFromColor(byte red, byte green, byte blue)
		{
			return new Vector3((float)(int)red / 255f, (float)(int)green / 255f, (float)(int)blue / 255f);
		}

		public static Vector4 VectorFromColor(byte red, byte green, byte blue, byte alpha)
		{
			return new Vector4((float)(int)red / 255f, (float)(int)green / 255f, (float)(int)blue / 255f, (float)(int)alpha / 255f);
		}

		/// <summary>
		/// Return minimum distance between line segment v-w and point p.
		/// </summary>
		public static float DistanceSquaredFromLineSegment(Vector3 v, Vector3 w, Vector3 p)
		{
			Vector3 vector = w - v;
			float num = vector.LengthSquared();
			if (num == 0f)
			{
				return Vector3.DistanceSquared(p, v);
			}
			float num2 = Vector3.Dot(p - v, vector);
			if (num2 <= 0f)
			{
				return Vector3.DistanceSquared(p, v);
			}
			if (num2 >= num)
			{
				return Vector3.DistanceSquared(p, w);
			}
			return Vector3.DistanceSquared(p, v + num2 / num * vector);
		}

		/// Clamp the provided value to an interval.
		public static float Clamp(float val, float min, float max)
		{
			if (val < min)
			{
				return min;
			}
			if (val > max)
			{
				return max;
			}
			return val;
		}
	}
}
