using ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a vector with three components.
	/// </summary>
	[Serializable]
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Math.XmlSerializers")]
	public struct Vector3 : IEquatable<Vector3>
	{
		protected class VRageMath_Vector3_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3 owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3 owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3 owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3 owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3 owner, in float value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3 owner, out float value)
			{
				value = owner.Z;
			}
		}

		public static Vector3 Zero;

		public static Vector3 One;

		public static Vector3 MinusOne;

		public static Vector3 Half;

		public static Vector3 PositiveInfinity;

		public static Vector3 NegativeInfinity;

		public static Vector3 UnitX;

		public static Vector3 UnitY;

		public static Vector3 UnitZ;

		public static Vector3 Up;

		public static Vector3 Down;

		public static Vector3 Right;

		public static Vector3 Left;

		public static Vector3 Forward;

		public static Vector3 Backward;

		public static Vector3 MaxValue;

		public static Vector3 MinValue;

		public static Vector3 Invalid;

		/// <summary>
		/// Gets or sets the x-component of the vector.
		/// </summary>
		[ProtoMember(1)]
		public float X;

		/// <summary>
		/// Gets or sets the y-component of the vector.
		/// </summary>
		[ProtoMember(4)]
		public float Y;

		/// <summary>
		/// Gets or sets the z-component of the vector.
		/// </summary>
		[ProtoMember(7)]
		public float Z;

		public float Sum => X + Y + Z;

		public float Volume => X * Y * Z;

		static Vector3()
		{
			Zero = default(Vector3);
			One = new Vector3(1f, 1f, 1f);
			MinusOne = new Vector3(-1f, -1f, -1f);
			Half = new Vector3(0.5f, 0.5f, 0.5f);
			PositiveInfinity = new Vector3(float.PositiveInfinity);
			NegativeInfinity = new Vector3(float.NegativeInfinity);
			UnitX = new Vector3(1f, 0f, 0f);
			UnitY = new Vector3(0f, 1f, 0f);
			UnitZ = new Vector3(0f, 0f, 1f);
			Up = new Vector3(0f, 1f, 0f);
			Down = new Vector3(0f, -1f, 0f);
			Right = new Vector3(1f, 0f, 0f);
			Left = new Vector3(-1f, 0f, 0f);
			Forward = new Vector3(0f, 0f, -1f);
			Backward = new Vector3(0f, 0f, 1f);
			MaxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			MinValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Invalid = new Vector3(float.NaN);
		}

		/// <summary>
		/// Initializes a new instance of Vector3.
		/// </summary>
		/// <param name="x">Initial value for the x-component of the vector.</param><param name="y">Initial value for the y-component of the vector.</param><param name="z">Initial value for the z-component of the vector.</param>
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3(double x, double y, double z)
		{
			X = (float)x;
			Y = (float)y;
			Z = (float)z;
		}

		/// <summary>
		/// Creates a new instance of Vector3.
		/// </summary>
		/// <param name="value">Value to initialize each component to.</param>
		public Vector3(float value)
		{
			X = (Y = (Z = value));
		}

		/// <summary>
		/// Initializes a new instance of Vector3.
		/// </summary>
		/// <param name="value">A vector containing the values to initialize x and y components with.</param><param name="z">Initial value for the z-component of the vector.</param>
		public Vector3(Vector2 value, float z)
		{
			X = value.X;
			Y = value.Y;
			Z = z;
		}

		public Vector3(Vector4 xyz)
		{
			X = xyz.X;
			Y = xyz.Y;
			Z = xyz.Z;
		}

		public Vector3(ref Vector3I value)
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
		}

		public Vector3(Vector3I value)
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector3 operator -(Vector3 value)
		{
			Vector3 result = default(Vector3);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			return result;
		}

		/// <summary>
		/// Tests vectors for equality.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static bool operator ==(Vector3 value1, Vector3 value2)
		{
			if (value1.X == value2.X && value1.Y == value2.Y)
			{
				return value1.Z == value2.Z;
			}
			return false;
		}

		/// <summary>
		/// Tests vectors for inequality.
		/// </summary>
		/// <param name="value1">Vector to compare.</param><param name="value2">Vector to compare.</param>
		public static bool operator !=(Vector3 value1, Vector3 value2)
		{
			if (value1.X == value2.X && value1.Y == value2.Y)
			{
				return value1.Z != value2.Z;
			}
			return true;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 operator +(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			return result;
		}

		public static Vector3 operator +(Vector3 value1, float value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X + value2;
			result.Y = value1.Y + value2;
			result.Z = value1.Z + value2;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 operator -(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			return result;
		}

		public static Vector3 operator -(Vector3 value1, float value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X - value2;
			result.Y = value1.Y - value2;
			result.Z = value1.Z - value2;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 operator *(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector3 operator *(Vector3 value, float scaleFactor)
		{
			Vector3 result = default(Vector3);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			result.Z = value.Z * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="scaleFactor">Scalar value.</param><param name="value">Source vector.</param>
		public static Vector3 operator *(float scaleFactor, Vector3 value)
		{
			Vector3 result = default(Vector3);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			result.Z = value.Z * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector3 operator /(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector3 operator /(Vector3 value, float divider)
		{
			float num = 1f / divider;
			Vector3 result = default(Vector3);
			result.X = value.X * num;
			result.Y = value.Y * num;
			result.Z = value.Z * num;
			return result;
		}

		public static Vector3 operator /(float value, Vector3 divider)
		{
			Vector3 result = default(Vector3);
			result.X = value / divider.X;
			result.Y = value / divider.Y;
			result.Z = value / divider.Z;
			return result;
		}

		public void Divide(float divider)
		{
			float num = 1f / divider;
			X *= num;
			Y *= num;
			Z *= num;
		}

		public void Multiply(float scale)
		{
			X *= scale;
			Y *= scale;
			Z *= scale;
		}

		public void Add(Vector3 other)
		{
			X += other.X;
			Y += other.Y;
			Z += other.Z;
		}

		public static Vector3 Abs(Vector3 value)
		{
			return new Vector3(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
		}

		public static Vector3 Sign(Vector3 value)
		{
			return new Vector3(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));
		}

		private static float Sign(float v, float epsilon)
		{
			if (!(v > epsilon))
			{
				if (!(v < 0f - epsilon))
				{
					return 0f;
				}
				return -1f;
			}
			return 1f;
		}

		public static Vector3 Sign(Vector3 value, float epsilon)
		{
			return new Vector3(Sign(value.X, epsilon), Sign(value.Y, epsilon), Sign(value.Z, epsilon));
		}

		/// <summary>
		/// Returns per component sign, never returns zero.
		/// For zero component returns sign 1.
		/// Faster than Sign.
		/// </summary>
		public static Vector3 SignNonZero(Vector3 value)
		{
			return new Vector3((!(value.X < 0f)) ? 1 : (-1), (!(value.Y < 0f)) ? 1 : (-1), (!(value.Z < 0f)) ? 1 : (-1));
		}

		public void Interpolate3(Vector3 v0, Vector3 v1, float rt)
		{
			float num = 1f - rt;
			X = num * v0.X + rt * v1.X;
			Y = num * v0.Y + rt * v1.Y;
			Z = num * v0.Z + rt * v1.Z;
		}

		public bool IsValid()
		{
			return (X * Y * Z).IsValid();
		}

		[Conditional("DEBUG")]
		public void AssertIsValid()
		{
		}

		public static bool IsUnit(ref Vector3 value)
		{
			float num = value.LengthSquared();
			if (num >= 0.9999f)
			{
				return num < 1.0001f;
			}
			return false;
		}

		public static bool ArePerpendicular(in Vector3 a, in Vector3 b)
		{
			float num = a.Dot(b);
			return num * num < 1E-08f * a.LengthSquared() * b.LengthSquared();
		}

		public static bool ArePerpendicular(Vector3 a, Vector3 b)
		{
			float num = a.Dot(b);
			return num * num < 1E-08f * a.LengthSquared() * b.LengthSquared();
		}

		public static bool IsZero(Vector3 value)
		{
			return IsZero(value, 0.0001f);
		}

		public static bool IsZero(Vector3 value, float epsilon)
		{
			if (Math.Abs(value.X) < epsilon && Math.Abs(value.Y) < epsilon)
			{
				return Math.Abs(value.Z) < epsilon;
			}
			return false;
		}

		public static Vector3 IsZeroVector(Vector3 value)
		{
			return new Vector3((value.X == 0f) ? 1 : 0, (value.Y == 0f) ? 1 : 0, (value.Z == 0f) ? 1 : 0);
		}

		public static Vector3 IsZeroVector(Vector3 value, float epsilon)
		{
			return new Vector3((Math.Abs(value.X) < epsilon) ? 1 : 0, (Math.Abs(value.Y) < epsilon) ? 1 : 0, (Math.Abs(value.Z) < epsilon) ? 1 : 0);
		}

		public static Vector3 Step(Vector3 value)
		{
			return new Vector3((value.X > 0f) ? 1 : ((value.X < 0f) ? (-1) : 0), (value.Y > 0f) ? 1 : ((value.Y < 0f) ? (-1) : 0), (value.Z > 0f) ? 1 : ((value.Z < 0f) ? (-1) : 0));
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", X.ToString(currentCulture), Y.ToString(currentCulture), Z.ToString(currentCulture));
		}

		public string ToString(string format)
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", X.ToString(format, currentCulture), Y.ToString(format, currentCulture), Z.ToString(format, currentCulture));
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Vector3.
		/// </summary>
		/// <param name="other">The Vector3 to compare with the current Vector3.</param>
		public bool Equals(Vector3 other)
		{
			if (X == other.X && Y == other.Y)
			{
				return Z == other.Z;
			}
			return false;
		}

		public bool Equals(Vector3 other, float epsilon)
		{
			if (Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon)
			{
				return Math.Abs(Z - other.Z) < epsilon;
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Vector3)
			{
				result = Equals((Vector3)obj);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return ((((int)(X * 997f) * 397) ^ (int)(Y * 997f)) * 397) ^ (int)(Z * 997f);
		}

		/// <summary>
		/// Gets the hash code of the vector object.
		/// </summary>
		public long GetHash()
		{
			int num = 0;
			long num2 = (long)Math.Round(Math.Abs(X * 1000f));
			num = 2;
			long num3 = (num2 * 397) ^ (long)Math.Round(Math.Abs(Y * 1000f));
			num += 4;
			long num4 = (num3 * 397) ^ (long)Math.Round(Math.Abs(Z * 1000f));
			num += 16;
			long num5 = (num4 * 397) ^ (Math.Sign(X) + 5);
			num += 256;
			long num6 = (num5 * 397) ^ (Math.Sign(Y) + 7);
			num += 65536;
			long num7 = (num6 * 397) ^ (Math.Sign(Z) + 11);
			num++;
			return (num7 * 397) ^ num;
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		public float Length()
		{
			return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		/// <summary>
		/// Calculates the length of the vector squared.
		/// </summary>
		public float LengthSquared()
		{
			return X * X + Y * Y + Z * Z;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float Distance(Vector3 value1, Vector3 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the vectors.</param>
		public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = num * num + num2 * num2 + num3 * num3;
			result = (float)Math.Sqrt(num4);
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float DistanceSquared(Vector3 value1, Vector3 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			return num * num + num2 * num2 + num3 * num3;
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the two vectors squared.</param>
		public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			result = num * num + num2 * num2 + num3 * num3;
		}

		/// <summary>
		/// Calculates rectangular distance (a.k.a. Manhattan distance or Chessboard distace) between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float RectangularDistance(Vector3 value1, Vector3 value2)
		{
			Vector3 vector = Abs(value1 - value2);
			return vector.X + vector.Y + vector.Z;
		}

		/// <summary>
		/// Calculates rectangular distance (a.k.a. Manhattan distance or Chessboard distace) between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float RectangularDistance(ref Vector3 value1, ref Vector3 value2)
		{
			Vector3 vector = Abs(value1 - value2);
			return vector.X + vector.Y + vector.Z;
		}

		/// <summary>
		/// Calculates the dot product of two vectors. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param>
		public static float Dot(Vector3 vector1, Vector3 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		/// <summary>
		/// Calculates the dot product of two vectors and writes the result to a user-specified variable. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The dot product of the two vectors.</param>
		public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
		{
			result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public float Dot(Vector3 v)
		{
			return Dot(ref v);
		}

		public float Dot(ref Vector3 v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public Vector3 Cross(Vector3 v)
		{
			return Cross(this, v);
		}

		/// <summary>
		/// Turns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		/// returns length
		public float Normalize()
		{
			float num = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
			float num2 = 1f / num;
			X *= num2;
			Y *= num2;
			Z *= num2;
			return num;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		/// <param name="value">The source Vector3.</param>
		public static Vector3 Normalize(Vector3 value)
		{
			float num = 1f / (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
			Vector3 result = default(Vector3);
			result.X = value.X * num;
			result.Y = value.Y * num;
			result.Z = value.Z * num;
			return result;
		}

		public static Vector3 Normalize(Vector3D value)
		{
			float num = 1f / (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
			Vector3 result = default(Vector3);
			result.X = (float)value.X * num;
			result.Y = (float)value.Y * num;
			result.Z = (float)value.Z * num;
			return result;
		}

		public static bool GetNormalized(ref Vector3 value)
		{
			float num = (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
			if (num > 0.001f)
			{
				float num2 = 1f / num;
				Vector3 vector = default(Vector3);
				vector.X = value.X * num2;
				vector.Y = value.Y * num2;
				vector.Z = value.Z * num2;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector, writing the result to a user-specified variable. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] The normalized vector.</param>
		public static void Normalize(ref Vector3 value, out Vector3 result)
		{
			float num = 1f / (float)Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
			result.X = value.X * num;
			result.Y = value.Y * num;
			result.Z = value.Z * num;
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param>
		public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
		{
			Vector3 result = default(Vector3);
			result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
			result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
			result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
			return result;
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The cross product of the vectors.</param>
		public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
		{
			float x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
			float y = vector1.Z * vector2.X - vector1.X * vector2.Z;
			float z = vector1.X * vector2.Y - vector1.Y * vector2.X;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.
		/// </summary>
		/// <param name="vector">Source vector.</param><param name="normal">Normal of the surface.</param>
		public static Vector3 Reflect(Vector3 vector, Vector3 normal)
		{
			float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			Vector3 result = default(Vector3);
			result.X = vector.X - 2f * num * normal.X;
			result.Y = vector.Y - 2f * num * normal.Y;
			result.Z = vector.Z - 2f * num * normal.Z;
			return result;
		}

		/// <summary>
		/// Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.
		/// </summary>
		/// <param name="vector">Source vector.</param><param name="normal">Normal of the surface.</param><param name="result">[OutAttribute] The reflected vector.</param>
		public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
		{
			float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			result.X = vector.X - 2f * num * normal.X;
			result.Y = vector.Y - 2f * num * normal.Y;
			result.Z = vector.Z - 2f * num * normal.Z;
		}

		/// <summary>
		/// Returns the rejection of vector from direction, i.e. projection of vector onto the plane defined by origin and direction
		/// </summary>
		/// <param name="vector">Vector which is to be rejected</param>
		/// <param name="direction">Direction from which the input vector will be rejected</param>
		/// <returns>Rejection of the vector from the given direction</returns>
		public static Vector3 Reject(Vector3 vector, Vector3 direction)
		{
			Reject(ref vector, ref direction, out Vector3 result);
			return result;
		}

		/// <summary>
		/// Returns the rejection of vector from direction, i.e. projection of vector onto the plane defined by origin and direction
		/// </summary>
		/// <param name="vector">Vector which is to be rejected</param>
		/// <param name="direction">Direction from which the input vector will be rejected</param>
		/// <param name="result">Rejection of the vector from the given direction</param>
		public static void Reject(ref Vector3 vector, ref Vector3 direction, out Vector3 result)
		{
			Dot(ref direction, ref direction, out float result2);
			result2 = 1f / result2;
			Dot(ref direction, ref vector, out float result3);
			result3 *= result2;
			Vector3 vector2 = default(Vector3);
			vector2.X = direction.X * result2;
			vector2.Y = direction.Y * result2;
			vector2.Z = direction.Z * result2;
			result.X = vector.X - result3 * vector2.X;
			result.Y = vector.Y - result3 * vector2.Y;
			result.Z = vector.Z - result3 * vector2.Z;
		}

		/// <summary>
		/// Returns the component of the vector that is smallest of all the three components.
		/// </summary>
		public float Min()
		{
			if (X < Y)
			{
				if (X < Z)
				{
					return X;
				}
				return Z;
			}
			if (Y < Z)
			{
				return Y;
			}
			return Z;
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is smallest of all the three components.
		/// </summary>
		public float AbsMin()
		{
			if (Math.Abs(X) < Math.Abs(Y))
			{
				if (Math.Abs(X) < Math.Abs(Z))
				{
					return Math.Abs(X);
				}
				return Math.Abs(Z);
			}
			if (Math.Abs(Y) < Math.Abs(Z))
			{
				return Math.Abs(Y);
			}
			return Math.Abs(Z);
		}

		/// <summary>
		/// Returns the component of the vector that is largest of all the three components.
		/// </summary>
		public float Max()
		{
			if (X > Y)
			{
				if (X > Z)
				{
					return X;
				}
				return Z;
			}
			if (Y > Z)
			{
				return Y;
			}
			return Z;
		}

		/// <summary>
		/// Keeps only component with maximal absolute, others are set to zero.
		/// </summary>
		public Vector3 MaxAbsComponent()
		{
			Vector3 vector = this;
			Vector3 vector2 = Abs(vector);
			float num = vector2.Max();
			if (vector2.X != num)
			{
				vector.X = 0f;
			}
			if (vector2.Y != num)
			{
				vector.Y = 0f;
			}
			if (vector2.Z != num)
			{
				vector.Z = 0f;
			}
			return vector;
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is largest of all the three components.
		/// </summary>
		public float AbsMax()
		{
			if (Math.Abs(X) > Math.Abs(Y))
			{
				if (Math.Abs(X) > Math.Abs(Z))
				{
					return Math.Abs(X);
				}
				return Math.Abs(Z);
			}
			if (Math.Abs(Y) > Math.Abs(Z))
			{
				return Math.Abs(Y);
			}
			return Math.Abs(Z);
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 Min(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The minimized vector.</param>
		public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 Max(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The maximized vector.</param>
		public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Separates minimal and maximal values of any two input vectors
		/// </summary>
		/// <param name="min">minimal values of the two vectors</param>
		/// <param name="max">maximal values of the two vectors</param>
		public static void MinMax(ref Vector3 min, ref Vector3 max)
		{
			if (min.X > max.X)
			{
				float x = min.X;
				min.X = max.X;
				max.X = x;
			}
			if (min.Y > max.Y)
			{
				float x = min.Y;
				min.Y = max.Y;
				max.Y = x;
			}
			if (min.Z > max.Z)
			{
				float x = min.Z;
				min.Z = max.Z;
				max.Z = x;
			}
		}

		/// <summary>
		/// Returns a vector that is equal to the projection of the input vector to the coordinate axis that corresponds
		/// to the original vector's largest value.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		public static Vector3 DominantAxisProjection(Vector3 value1)
		{
			DominantAxisProjection(ref value1, out Vector3 result);
			return result;
		}

		/// <summary>
		/// Calculates a vector that is equal to the projection of the input vector to the coordinate axis that corresponds
		/// to the original vector's largest value. The result is saved into a user-specified variable.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="result">[OutAttribute] The projected vector.</param>
		public static void DominantAxisProjection(ref Vector3 value1, out Vector3 result)
		{
			if (Math.Abs(value1.X) > Math.Abs(value1.Y))
			{
				result = ((Math.Abs(value1.X) > Math.Abs(value1.Z)) ? new Vector3(value1.X, 0f, 0f) : new Vector3(0f, 0f, value1.Z));
			}
			else
			{
				result = ((Math.Abs(value1.Y) > Math.Abs(value1.Z)) ? new Vector3(0f, value1.Y, 0f) : new Vector3(0f, 0f, value1.Z));
			}
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param>
		public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
		{
			Vector3 result = value1;
			if (value1.X > max.X)
			{
				result.X = max.X;
			}
			else if (value1.X < min.X)
			{
				result.X = min.X;
			}
			if (value1.Y > max.Y)
			{
				result.Y = max.Y;
			}
			else if (value1.Y < min.Y)
			{
				result.Y = min.Y;
			}
			if (value1.Z > max.Z)
			{
				result.Z = max.Z;
			}
			else if (value1.Z < min.Z)
			{
				result.Z = min.Z;
			}
			return result;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param><param name="result">[OutAttribute] The clamped value.</param>
		public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			result = value1;
			if (value1.X > max.X)
			{
				result.X = max.X;
			}
			else if (value1.X < min.X)
			{
				result.X = min.X;
			}
			if (value1.Y > max.Y)
			{
				result.Y = max.Y;
			}
			else if (value1.Y < min.Y)
			{
				result.Y = min.Y;
			}
			if (value1.Z > max.Z)
			{
				result.Z = max.Z;
			}
			else if (value1.Z < min.Z)
			{
				result.Z = min.Z;
			}
		}

		public static Vector3 ClampToSphere(Vector3 vector, float radius)
		{
			float num = vector.LengthSquared();
			float num2 = radius * radius;
			if (num > num2)
			{
				return vector * (float)Math.Sqrt(num2 / num);
			}
			return vector;
		}

		public static void ClampToSphere(ref Vector3 vector, float radius)
		{
			float num = vector.LengthSquared();
			float num2 = radius * radius;
			if (num > num2)
			{
				vector *= (float)Math.Sqrt(num2 / num);
			}
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			return result;
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param><param name="result">[OutAttribute] The result of the interpolation.</param>
		public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
		{
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
		}

		/// <summary>
		/// Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle.
		/// </summary>
		/// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
		public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
		{
			Vector3 result = default(Vector3);
			result.X = (float)((double)value1.X + (double)amount1 * ((double)value2.X - (double)value1.X) + (double)amount2 * ((double)value3.X - (double)value1.X));
			result.Y = (float)((double)value1.Y + (double)amount1 * ((double)value2.Y - (double)value1.Y) + (double)amount2 * ((double)value3.Y - (double)value1.Y));
			result.Z = (float)((double)value1.Z + (double)amount1 * ((double)value2.Z - (double)value1.Z) + (double)amount2 * ((double)value3.Z - (double)value1.Z));
			return result;
		}

		/// <summary>
		/// Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 3D triangle.
		/// </summary>
		/// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param><param name="result">[OutAttribute] The 3D Cartesian coordinates of the specified point are placed in this Vector3 on exit.</param>
		public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
		{
			result.X = (float)((double)value1.X + (double)amount1 * ((double)value2.X - (double)value1.X) + (double)amount2 * ((double)value3.X - (double)value1.X));
			result.Y = (float)((double)value1.Y + (double)amount1 * ((double)value2.Y - (double)value1.Y) + (double)amount2 * ((double)value3.Y - (double)value1.Y));
			result.Z = (float)((double)value1.Z + (double)amount1 * ((double)value2.Z - (double)value1.Z) + (double)amount2 * ((double)value3.Z - (double)value1.Z));
		}

		/// <summary>
		/// Compute barycentric coordinates (u, v, w) for point p with respect to triangle (a, b, c)
		/// From : Real-Time Collision Detection, Christer Ericson, CRC Press
		/// 3.4 Barycentric Coordinates
		/// </summary>
		public static void Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c, out float u, out float v, out float w)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			Vector3 vector3 = p - a;
			float num = Dot(vector, vector);
			float num2 = Dot(vector, vector2);
			float num3 = Dot(vector2, vector2);
			float num4 = Dot(vector3, vector);
			float num5 = Dot(vector3, vector2);
			float num6 = num * num3 - num2 * num2;
			v = (num3 * num4 - num2 * num5) / num6;
			w = (num * num5 - num2 * num4) / num6;
			u = 1f - v - w;
		}

		public static float TriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			return Cross(v2 - v1, v3 - v1).Length() * 0.5f;
		}

		public static float TriangleArea(ref Vector3 v1, ref Vector3 v2, ref Vector3 v3)
		{
			Subtract(ref v2, ref v1, out Vector3 result);
			Subtract(ref v3, ref v1, out Vector3 result2);
			Cross(ref result, ref result2, out Vector3 result3);
			return result3.Length() * 0.5f;
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
		{
			amount = (((double)amount > 1.0) ? 1f : (((double)amount < 0.0) ? 0f : amount));
			amount = (float)((double)amount * (double)amount * (3.0 - 2.0 * (double)amount));
			Vector3 result = default(Vector3);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			return result;
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Weighting value.</param><param name="result">[OutAttribute] The interpolated value.</param>
		public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
		{
			amount = (((double)amount > 1.0) ? 1f : (((double)amount < 0.0) ? 0f : amount));
			amount = (float)((double)amount * (double)amount * (3.0 - 2.0 * (double)amount));
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param>
		public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			Vector3 result = default(Vector3);
			result.X = (float)(0.5 * (2.0 * (double)value2.X + (0.0 - (double)value1.X + (double)value3.X) * (double)amount + (2.0 * (double)value1.X - 5.0 * (double)value2.X + 4.0 * (double)value3.X - (double)value4.X) * (double)num + (0.0 - (double)value1.X + 3.0 * (double)value2.X - 3.0 * (double)value3.X + (double)value4.X) * (double)num2));
			result.Y = (float)(0.5 * (2.0 * (double)value2.Y + (0.0 - (double)value1.Y + (double)value3.Y) * (double)amount + (2.0 * (double)value1.Y - 5.0 * (double)value2.Y + 4.0 * (double)value3.Y - (double)value4.Y) * (double)num + (0.0 - (double)value1.Y + 3.0 * (double)value2.Y - 3.0 * (double)value3.Y + (double)value4.Y) * (double)num2));
			result.Z = (float)(0.5 * (2.0 * (double)value2.Z + (0.0 - (double)value1.Z + (double)value3.Z) * (double)amount + (2.0 * (double)value1.Z - 5.0 * (double)value2.Z + 4.0 * (double)value3.Z - (double)value4.Z) * (double)num + (0.0 - (double)value1.Z + 3.0 * (double)value2.Z - 3.0 * (double)value3.Z + (double)value4.Z) * (double)num2));
			return result;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
		public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
		{
			float num = amount * amount;
			float num2 = amount * num;
			result.X = (float)(0.5 * (2.0 * (double)value2.X + (0.0 - (double)value1.X + (double)value3.X) * (double)amount + (2.0 * (double)value1.X - 5.0 * (double)value2.X + 4.0 * (double)value3.X - (double)value4.X) * (double)num + (0.0 - (double)value1.X + 3.0 * (double)value2.X - 3.0 * (double)value3.X + (double)value4.X) * (double)num2));
			result.Y = (float)(0.5 * (2.0 * (double)value2.Y + (0.0 - (double)value1.Y + (double)value3.Y) * (double)amount + (2.0 * (double)value1.Y - 5.0 * (double)value2.Y + 4.0 * (double)value3.Y - (double)value4.Y) * (double)num + (0.0 - (double)value1.Y + 3.0 * (double)value2.Y - 3.0 * (double)value3.Y + (double)value4.Y) * (double)num2));
			result.Z = (float)(0.5 * (2.0 * (double)value2.Z + (0.0 - (double)value1.Z + (double)value3.Z) * (double)amount + (2.0 * (double)value1.Z - 5.0 * (double)value2.Z + 4.0 * (double)value3.Z - (double)value4.Z) * (double)num + (0.0 - (double)value1.Z + 3.0 * (double)value2.Z - 3.0 * (double)value3.Z + (double)value4.Z) * (double)num2));
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param>
		public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			float num3 = (float)(2.0 * (double)num2 - 3.0 * (double)num + 1.0);
			float num4 = (float)(-2.0 * (double)num2 + 3.0 * (double)num);
			float num5 = num2 - 2f * num + amount;
			float num6 = num2 - num;
			Vector3 result = default(Vector3);
			result.X = (float)((double)value1.X * (double)num3 + (double)value2.X * (double)num4 + (double)tangent1.X * (double)num5 + (double)tangent2.X * (double)num6);
			result.Y = (float)((double)value1.Y * (double)num3 + (double)value2.Y * (double)num4 + (double)tangent1.Y * (double)num5 + (double)tangent2.Y * (double)num6);
			result.Z = (float)((double)value1.Z * (double)num3 + (double)value2.Z * (double)num4 + (double)tangent1.Z * (double)num5 + (double)tangent2.Z * (double)num6);
			return result;
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
		public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
		{
			float num = amount * amount;
			float num2 = amount * num;
			float num3 = (float)(2.0 * (double)num2 - 3.0 * (double)num + 1.0);
			float num4 = (float)(-2.0 * (double)num2 + 3.0 * (double)num);
			float num5 = num2 - 2f * num + amount;
			float num6 = num2 - num;
			result.X = (float)((double)value1.X * (double)num3 + (double)value2.X * (double)num4 + (double)tangent1.X * (double)num5 + (double)tangent2.X * (double)num6);
			result.Y = (float)((double)value1.Y * (double)num3 + (double)value2.Y * (double)num4 + (double)tangent1.Y * (double)num5 + (double)tangent2.Y * (double)num6);
			result.Z = (float)((double)value1.Z * (double)num3 + (double)value2.Z * (double)num4 + (double)tangent1.Z * (double)num5 + (double)tangent2.Z * (double)num6);
		}

		/// <summary>
		/// Transforms a 3D vector by the given matrix.
		/// </summary>
		/// <param name="position">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector3 Transform(Vector3 position, Matrix matrix)
		{
			float num = (float)((double)position.X * (double)matrix.M11 + (double)position.Y * (double)matrix.M21 + (double)position.Z * (double)matrix.M31) + matrix.M41;
			float num2 = (float)((double)position.X * (double)matrix.M12 + (double)position.Y * (double)matrix.M22 + (double)position.Z * (double)matrix.M32) + matrix.M42;
			float num3 = (float)((double)position.X * (double)matrix.M13 + (double)position.Y * (double)matrix.M23 + (double)position.Z * (double)matrix.M33) + matrix.M43;
			float num4 = 1f / (position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
			Vector3 result = default(Vector3);
			result.X = num * num4;
			result.Y = num2 * num4;
			result.Z = num3 * num4;
			return result;
		}

		/// <summary>
		/// Transforms a 3D vector by the given matrix.
		/// </summary>
		/// <param name="position">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector3D Transform(Vector3 position, MatrixD matrix)
		{
			double num = (double)position.X * matrix.M11 + (double)position.Y * matrix.M21 + (double)position.Z * matrix.M31 + matrix.M41;
			double num2 = (double)position.X * matrix.M12 + (double)position.Y * matrix.M22 + (double)position.Z * matrix.M32 + matrix.M42;
			double num3 = (double)position.X * matrix.M13 + (double)position.Y * matrix.M23 + (double)position.Z * matrix.M33 + matrix.M43;
			double num4 = 1.0 / ((double)position.X * matrix.M14 + (double)position.Y * matrix.M24 + (double)position.Z * matrix.M34 + matrix.M44);
			Vector3D result = default(Vector3D);
			result.X = num * num4;
			result.Y = num2 * num4;
			result.Z = num3 * num4;
			return result;
		}

		public static Vector3 Transform(Vector3 position, ref Matrix matrix)
		{
			Transform(ref position, ref matrix, out Vector3 result);
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The transformed vector.</param>
		public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
		{
			float num = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
			float num2 = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
			float num3 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
			float num4 = 1f / (position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
			result.X = num * num4;
			result.Y = num2 * num4;
			result.Z = num3 * num4;
		}

		public static void Transform(ref Vector3 position, ref MatrixI matrix, out Vector3 result)
		{
			result = position.X * Base6Directions.GetVector(matrix.Right) + position.Y * Base6Directions.GetVector(matrix.Up) + position.Z * Base6Directions.GetVector(matrix.Backward) + matrix.Translation;
		}

		/// <summary>
		/// Transforms a Vector3 by the given projection matrix (both ortho and perspective are supported)
		/// </summary>
		/// <param name="position">The source Vector3.</param><param name="matrix">The projection Matrix.</param><param name="result">[OutAttribute] The transformed vector.</param>
		public static void TransformProjection(ref Vector3 position, ref Matrix matrix, out Vector3 result)
		{
			float num = position.X * matrix.M11;
			float num2 = position.Y * matrix.M22;
			float num3 = position.Z * matrix.M33 + matrix.M43;
			float num4 = 1f / (position.Z * matrix.M34 + matrix.M44);
			result.X = num * num4;
			result.Y = num2 * num4;
			result.Z = num3 * num4;
		}

		/// Transform the provided vector only about the rotation, scale and translation terms of a matrix.
		///
		/// This effectively treats the matrix as a 3x4 matrix and the input vector as a 4 dimensional vector with unit W coordinate.
		public static void TransformNoProjection(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
		{
			float x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41;
			float y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42;
			float z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		/// Transform the provided vector only about the rotation and scale terms of a matrix.
		public static void RotateAndScale(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
		{
			float x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31;
			float y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32;
			float z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static Vector3 RotateAndScale(Vector3 vector, Matrix matrix)
		{
			RotateAndScale(ref vector, ref matrix, out Vector3 result);
			return result;
		}

		/// <summary>
		/// Transforms a 3D vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
		{
			float x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
			float y = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
			float z = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
			Vector3 result = default(Vector3);
			result.X = x;
			result.Y = y;
			result.Z = z;
			return result;
		}

		/// <summary>
		/// Transforms a 3D vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector3 TransformNormal(Vector3 normal, MatrixD matrix)
		{
			float x = (float)((double)normal.X * matrix.M11 + (double)normal.Y * matrix.M21 + (double)normal.Z * matrix.M31);
			float y = (float)((double)normal.X * matrix.M12 + (double)normal.Y * matrix.M22 + (double)normal.Z * matrix.M32);
			float z = (float)((double)normal.X * matrix.M13 + (double)normal.Y * matrix.M23 + (double)normal.Z * matrix.M33);
			Vector3 result = default(Vector3);
			result.X = x;
			result.Y = y;
			result.Z = z;
			return result;
		}

		/// <summary>
		/// Transforms a 3D vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector3 TransformNormal(Vector3D normal, Matrix matrix)
		{
			float x = (float)(normal.X * (double)matrix.M11 + normal.Y * (double)matrix.M21 + normal.Z * (double)matrix.M31);
			float y = (float)(normal.X * (double)matrix.M12 + normal.Y * (double)matrix.M22 + normal.Z * (double)matrix.M32);
			float z = (float)(normal.X * (double)matrix.M13 + normal.Y * (double)matrix.M23 + normal.Z * (double)matrix.M33);
			Vector3 result = default(Vector3);
			result.X = x;
			result.Y = y;
			result.Z = z;
			return result;
		}

		/// <summary>
		/// Transforms a vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector3 resulting from the transformation.</param>
		public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
		{
			float x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
			float y = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
			float z = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static void TransformNormal(ref Vector3 normal, ref MatrixD matrix, out Vector3 result)
		{
			float x = (float)((double)normal.X * matrix.M11 + (double)normal.Y * matrix.M21 + (double)normal.Z * matrix.M31);
			float y = (float)((double)normal.X * matrix.M12 + (double)normal.Y * matrix.M22 + (double)normal.Z * matrix.M32);
			float z = (float)((double)normal.X * matrix.M13 + (double)normal.Y * matrix.M23 + (double)normal.Z * matrix.M33);
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static void TransformNormal(ref Vector3 normal, ref MatrixI matrix, out Vector3 result)
		{
			result = normal.X * Base6Directions.GetVector(matrix.Right) + normal.Y * Base6Directions.GetVector(matrix.Up) + normal.Z * Base6Directions.GetVector(matrix.Backward);
		}

		public static Vector3 TransformNormal(Vector3 normal, MyBlockOrientation orientation)
		{
			TransformNormal(ref normal, orientation, out Vector3 result);
			return result;
		}

		public static void TransformNormal(ref Vector3 normal, MyBlockOrientation orientation, out Vector3 result)
		{
			result = (0f - normal.X) * Base6Directions.GetVector(orientation.Left) + normal.Y * Base6Directions.GetVector(orientation.Up) - normal.Z * Base6Directions.GetVector(orientation.Forward);
		}

		public static Vector3 TransformNormal(Vector3 normal, ref Matrix matrix)
		{
			TransformNormal(ref normal, ref matrix, out Vector3 result);
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion rotation.
		/// </summary>
		/// <param name="value">The Vector3 to rotate.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector3 Transform(Vector3 value, Quaternion rotation)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			float x = (float)((double)value.X * (1.0 - (double)num10 - (double)num12) + (double)value.Y * ((double)num8 - (double)num6) + (double)value.Z * ((double)num9 + (double)num5));
			float y = (float)((double)value.X * ((double)num8 + (double)num6) + (double)value.Y * (1.0 - (double)num7 - (double)num12) + (double)value.Z * ((double)num11 - (double)num4));
			float z = (float)((double)value.X * ((double)num9 - (double)num5) + (double)value.Y * ((double)num11 + (double)num4) + (double)value.Z * (1.0 - (double)num7 - (double)num10));
			Vector3 result = default(Vector3);
			result.X = x;
			result.Y = y;
			result.Z = z;
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion rotation.
		/// </summary>
		/// <param name="value">The Vector3 to rotate.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] An existing Vector3 filled in with the results of the rotation.</param>
		public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			float x = (float)((double)value.X * (1.0 - (double)num10 - (double)num12) + (double)value.Y * ((double)num8 - (double)num6) + (double)value.Z * ((double)num9 + (double)num5));
			float y = (float)((double)value.X * ((double)num8 + (double)num6) + (double)value.Y * (1.0 - (double)num7 - (double)num12) + (double)value.Z * ((double)num11 - (double)num4));
			float z = (float)((double)value.X * ((double)num9 - (double)num5) + (double)value.Y * ((double)num11 + (double)num4) + (double)value.Z * (1.0 - (double)num7 - (double)num10));
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		/// <summary>
		/// Transforms a source array of Vector3s by a specified Matrix and writes the results to an existing destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
		public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				float x = sourceArray[i].X;
				float y = sourceArray[i].Y;
				float z = sourceArray[i].Z;
				destinationArray[i].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31) + matrix.M41;
				destinationArray[i].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32) + matrix.M42;
				destinationArray[i].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33) + matrix.M43;
			}
		}

		/// <summary>
		/// Applies a specified transform Matrix to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="sourceIndex">The index in the source array at which to start.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The existing destination array.</param><param name="destinationIndex">The index in the destination array at which to start.</param><param name="length">The number of Vector3s to transform.</param>
		public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				float x = sourceArray[sourceIndex].X;
				float y = sourceArray[sourceIndex].Y;
				float z = sourceArray[sourceIndex].Z;
				destinationArray[destinationIndex].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31) + matrix.M41;
				destinationArray[destinationIndex].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32) + matrix.M42;
				destinationArray[destinationIndex].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33) + matrix.M43;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms an array of 3D vector normals by a specified Matrix.
		/// </summary>
		/// <param name="sourceArray">The array of Vector3 normals to transform.</param><param name="matrix">The transform matrix to apply.</param><param name="destinationArray">An existing Vector3 array into which the results of the transforms are written.</param>
		public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				float x = sourceArray[i].X;
				float y = sourceArray[i].Y;
				float z = sourceArray[i].Z;
				destinationArray[i].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31);
				destinationArray[i].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32);
				destinationArray[i].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33);
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of 3D vector normals by a specified Matrix and writes the results to a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array of Vector3 normals.</param><param name="sourceIndex">The starting index in the source array.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The destination Vector3 array.</param><param name="destinationIndex">The starting index in the destination array.</param><param name="length">The number of vectors to transform.</param>
		public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				float x = sourceArray[sourceIndex].X;
				float y = sourceArray[sourceIndex].Y;
				float z = sourceArray[sourceIndex].Z;
				destinationArray[destinationIndex].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31);
				destinationArray[destinationIndex].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32);
				destinationArray[destinationIndex].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33);
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms a source array of Vector3s by a specified Quaternion rotation and writes the results to an existing destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
		public static void Transform(Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			float num13 = 1f - num10 - num12;
			float num14 = num8 - num6;
			float num15 = num9 + num5;
			float num16 = num8 + num6;
			float num17 = 1f - num7 - num12;
			float num18 = num11 - num4;
			float num19 = num9 - num5;
			float num20 = num11 + num4;
			float num21 = 1f - num7 - num10;
			for (int i = 0; i < sourceArray.Length; i++)
			{
				float x = sourceArray[i].X;
				float y = sourceArray[i].Y;
				float z = sourceArray[i].Z;
				destinationArray[i].X = (float)((double)x * (double)num13 + (double)y * (double)num14 + (double)z * (double)num15);
				destinationArray[i].Y = (float)((double)x * (double)num16 + (double)y * (double)num17 + (double)z * (double)num18);
				destinationArray[i].Z = (float)((double)x * (double)num19 + (double)y * (double)num20 + (double)z * (double)num21);
			}
		}

		/// <summary>
		/// Applies a specified Quaternion rotation to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="sourceIndex">The index in the source array at which to start.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The existing destination array.</param><param name="destinationIndex">The index in the destination array at which to start.</param><param name="length">The number of Vector3s to transform.</param>
		public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			float num13 = 1f - num10 - num12;
			float num14 = num8 - num6;
			float num15 = num9 + num5;
			float num16 = num8 + num6;
			float num17 = 1f - num7 - num12;
			float num18 = num11 - num4;
			float num19 = num9 - num5;
			float num20 = num11 + num4;
			float num21 = 1f - num7 - num10;
			while (length > 0)
			{
				float x = sourceArray[sourceIndex].X;
				float y = sourceArray[sourceIndex].Y;
				float z = sourceArray[sourceIndex].Z;
				destinationArray[destinationIndex].X = (float)((double)x * (double)num13 + (double)y * (double)num14 + (double)z * (double)num15);
				destinationArray[destinationIndex].Y = (float)((double)x * (double)num16 + (double)y * (double)num17 + (double)z * (double)num18);
				destinationArray[destinationIndex].Z = (float)((double)x * (double)num19 + (double)y * (double)num20 + (double)z * (double)num21);
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector3 Negate(Vector3 value)
		{
			Vector3 result = default(Vector3);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			return result;
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
		public static void Negate(ref Vector3 value, out Vector3 result)
		{
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 Add(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] Sum of the source vectors.</param>
		public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 Subtract(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the subtraction.</param>
		public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector3 Multiply(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector3 Multiply(Vector3 value1, float scaleFactor)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector3 Divide(Vector3 value1, Vector3 value2)
		{
			Vector3 result = default(Vector3);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">The divisor.</param>
		public static Vector3 Divide(Vector3 value1, float value2)
		{
			float num = 1f / value2;
			Vector3 result = default(Vector3);
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
		{
			float num = 1f / value2;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
		}

		public static Vector3 CalculatePerpendicularVector(Vector3 v)
		{
			v.CalculatePerpendicularVector(out Vector3 result);
			return result;
		}

		public void CalculatePerpendicularVector(out Vector3 result)
		{
			if (Math.Abs(Y + Z) > 0.0001f || Math.Abs(X) > 0.0001f)
			{
				result = new Vector3(0f - (Y + Z), X, X);
			}
			else
			{
				result = new Vector3(Z, Z, 0f - (X + Y));
			}
			Normalize(ref result, out result);
		}

		public static void GetAzimuthAndElevation(Vector3 v, out float azimuth, out float elevation)
		{
			Dot(ref v, ref Up, out float result);
			v.Y = 0f;
			v.Normalize();
			Dot(ref v, ref Forward, out float result2);
			elevation = (float)Math.Asin(result);
			if (v.X >= 0f)
			{
				azimuth = 0f - (float)Math.Acos(result2);
			}
			else
			{
				azimuth = (float)Math.Acos(result2);
			}
		}

		public static void CreateFromAzimuthAndElevation(float azimuth, float elevation, out Vector3 direction)
		{
			Matrix matrix = Matrix.CreateRotationY(azimuth);
			Matrix matrix2 = Matrix.CreateRotationX(elevation);
			direction = Forward;
			TransformNormal(ref direction, ref matrix2, out direction);
			TransformNormal(ref direction, ref matrix, out direction);
		}

		public long VolumeInt(float multiplier)
		{
			return (long)(X * multiplier) * (long)(Y * multiplier) * (long)(Z * multiplier);
		}

		public bool IsInsideInclusive(ref Vector3 min, ref Vector3 max)
		{
			if (min.X <= X && X <= max.X && min.Y <= Y && Y <= max.Y && min.Z <= Z)
			{
				return Z <= max.Z;
			}
			return false;
		}

		public static Vector3 SwapYZCoordinates(Vector3 v)
		{
			return new Vector3(v.X, v.Z, 0f - v.Y);
		}

		public float GetDim(int i)
		{
			switch (i)
			{
			case 0:
				return X;
			case 1:
				return Y;
			case 2:
				return Z;
			default:
				return GetDim((i % 3 + 3) % 3);
			}
		}

		public void SetDim(int i, float value)
		{
			switch (i)
			{
			case 0:
				X = value;
				break;
			case 1:
				Y = value;
				break;
			case 2:
				Z = value;
				break;
			default:
				SetDim((i % 3 + 3) % 3, value);
				break;
			}
		}

		public static Vector3 Ceiling(Vector3 v)
		{
			return new Vector3(Math.Ceiling(v.X), Math.Ceiling(v.Y), Math.Ceiling(v.Z));
		}

		public static Vector3 Floor(Vector3 v)
		{
			return new Vector3(Math.Floor(v.X), Math.Floor(v.Y), Math.Floor(v.Z));
		}

		public static Vector3 Round(Vector3 v)
		{
			return new Vector3(Math.Round(v.X), Math.Round(v.Y), Math.Round(v.Z));
		}

		public static Vector3 Round(Vector3 v, int numDecimals)
		{
			return new Vector3(Math.Round(v.X, numDecimals), Math.Round(v.Y, numDecimals), Math.Round(v.Z, numDecimals));
		}

		/// <summary>
		/// Projects given vector on plane specified by it's normal.
		/// </summary>
		/// <param name="vec">Vector which is to be projected</param>
		/// <param name="planeNormal">Plane normal (may or may not be normalized)</param>
		/// <returns>Vector projected on plane</returns>
		public static Vector3 ProjectOnPlane(ref Vector3 vec, ref Vector3 planeNormal)
		{
			float num = vec.Dot(planeNormal);
			float num2 = planeNormal.LengthSquared();
			return vec - num / num2 * planeNormal;
		}

		/// <summary>
		/// Projects vector on another vector resulting in new vector in guided vector's direction with different length.
		/// </summary>
		/// <param name="vec">Vector which is to be projected</param>
		/// <param name="guideVector">Guide vector (may or may not be normalized)</param>
		/// <returns>Vector projected on guide vector</returns>
		public static Vector3 ProjectOnVector(ref Vector3 vec, ref Vector3 guideVector)
		{
			if (IsZero(ref vec) || IsZero(ref guideVector))
			{
				return Zero;
			}
			return guideVector * Dot(vec, guideVector) / guideVector.LengthSquared();
		}

		public static bool IsZero(ref Vector3 vec)
		{
			if (IsZero(vec.X) && IsZero(vec.Y))
			{
				return IsZero(vec.Z);
			}
			return false;
		}

		private static bool IsZero(float d)
		{
			if (d > -1E-05f)
			{
				return d < 1E-05f;
			}
			return false;
		}
	}
}
