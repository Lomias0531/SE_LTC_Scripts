using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a vector with four components.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct Vector4 : IEquatable<Vector4>
	{
		protected class VRageMath_Vector4_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector4, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4 owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4 owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector4_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector4, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4 owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4 owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector4_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector4, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4 owner, in float value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4 owner, out float value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Vector4_003C_003EW_003C_003EAccessor : IMemberAccessor<Vector4, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4 owner, in float value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4 owner, out float value)
			{
				value = owner.W;
			}
		}

		public static Vector4 Zero;

		public static Vector4 One;

		public static Vector4 UnitX;

		public static Vector4 UnitY;

		public static Vector4 UnitZ;

		public static Vector4 UnitW;

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

		/// <summary>
		/// Gets or sets the w-component of the vector.
		/// </summary>
		[ProtoMember(10)]
		public float W;

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return X;
				case 1:
					return Y;
				case 2:
					return Z;
				case 3:
					return W;
				default:
					throw new Exception("Index out of bounds");
				}
			}
			set
			{
				switch (index)
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
				case 3:
					W = value;
					break;
				default:
					throw new Exception("Index out of bounds");
				}
			}
		}

		static Vector4()
		{
			Zero = default(Vector4);
			One = new Vector4(1f, 1f, 1f, 1f);
			UnitX = new Vector4(1f, 0f, 0f, 0f);
			UnitY = new Vector4(0f, 1f, 0f, 0f);
			UnitZ = new Vector4(0f, 0f, 1f, 0f);
			UnitW = new Vector4(0f, 0f, 0f, 1f);
		}

		/// <summary>
		/// Initializes a new instance of Vector4.
		/// </summary>
		/// <param name="x">Initial value for the x-component of the vector.</param><param name="y">Initial value for the y-component of the vector.</param><param name="z">Initial value for the z-component of the vector.</param><param name="w">Initial value for the w-component of the vector.</param>
		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of Vector4.
		/// </summary>
		/// <param name="value">A vector containing the values to initialize x and y components with.</param><param name="z">Initial value for the z-component of the vector.</param><param name="w">Initial value for the w-component of the vector.</param>
		public Vector4(Vector2 value, float z, float w)
		{
			X = value.X;
			Y = value.Y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of Vector4.
		/// </summary>
		/// <param name="value">A vector containing the values to initialize x, y, and z components with.</param><param name="w">Initial value for the w-component of the vector.</param>
		public Vector4(Vector3 value, float w)
		{
			X = value.X;
			Y = value.Y;
			Z = value.Z;
			W = w;
		}

		/// <summary>
		/// Creates a new instance of Vector4.
		/// </summary>
		/// <param name="value">Value to initialize each component to.</param>
		public Vector4(float value)
		{
			X = (Y = (Z = (W = value)));
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector4 operator -(Vector4 value)
		{
			Vector4 result = default(Vector4);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			result.W = 0f - value.W;
			return result;
		}

		/// <summary>
		/// Tests vectors for equality.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static bool operator ==(Vector4 value1, Vector4 value2)
		{
			if ((double)value1.X == (double)value2.X && (double)value1.Y == (double)value2.Y && (double)value1.Z == (double)value2.Z)
			{
				return (double)value1.W == (double)value2.W;
			}
			return false;
		}

		/// <summary>
		/// Tests vectors for inequality.
		/// </summary>
		/// <param name="value1">Vector to compare.</param><param name="value2">Vector to compare.</param>
		public static bool operator !=(Vector4 value1, Vector4 value2)
		{
			if ((double)value1.X == (double)value2.X && (double)value1.Y == (double)value2.Y && (double)value1.Z == (double)value2.Z)
			{
				return (double)value1.W != (double)value2.W;
			}
			return true;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 operator +(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			result.W = value1.W + value2.W;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 operator -(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			result.W = value1.W - value2.W;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 operator *(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			result.W = value1.W * value2.W;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector4 operator *(Vector4 value1, float scaleFactor)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
			result.W = value1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="scaleFactor">Scalar value.</param><param name="value1">Source vector.</param>
		public static Vector4 operator *(float scaleFactor, Vector4 value1)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
			result.W = value1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector4 operator /(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			result.W = value1.W / value2.W;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector4 operator /(Vector4 value1, float divider)
		{
			float num = 1f / divider;
			Vector4 result = default(Vector4);
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
			result.W = value1.W * num;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector4 operator /(float lhs, Vector4 rhs)
		{
			Vector4 result = default(Vector4);
			result.X = lhs / rhs.X;
			result.Y = lhs / rhs.Y;
			result.Z = lhs / rhs.Z;
			result.W = lhs / rhs.W;
			return result;
		}

		public static Vector4 PackOrthoMatrix(Vector3 position, Vector3 forward, Vector3 up)
		{
			int direction = (int)Base6Directions.GetDirection(forward);
			int direction2 = (int)Base6Directions.GetDirection(up);
			return new Vector4(position, direction * 6 + direction2);
		}

		public static Vector4 PackOrthoMatrix(ref Matrix matrix)
		{
			int direction = (int)Base6Directions.GetDirection(matrix.Forward);
			int direction2 = (int)Base6Directions.GetDirection(matrix.Up);
			return new Vector4(matrix.Translation, direction * 6 + direction2);
		}

		public static Matrix UnpackOrthoMatrix(ref Vector4 packed)
		{
			int num = (int)packed.W;
			return Matrix.CreateWorld(new Vector3(packed), Base6Directions.GetVector(num / 6), Base6Directions.GetVector(num % 6));
		}

		public static void UnpackOrthoMatrix(ref Vector4 packed, out Matrix matrix)
		{
			int num = (int)packed.W;
			Vector3 position = new Vector3(packed);
			Vector3 forward = Base6Directions.GetVector(num / 6);
			Vector3 up = Base6Directions.GetVector(num % 6);
			Matrix.CreateWorld(ref position, ref forward, ref up, out matrix);
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(currentCulture), Y.ToString(currentCulture), Z.ToString(currentCulture), W.ToString(currentCulture));
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Vector4.
		/// </summary>
		/// <param name="other">The Vector4 to compare with the current Vector4.</param>
		public bool Equals(Vector4 other)
		{
			if ((double)X == (double)other.X && (double)Y == (double)other.Y && (double)Z == (double)other.Z)
			{
				return (double)W == (double)other.W;
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Vector4)
			{
				result = Equals((Vector4)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code of this object.
		/// </summary>
		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		public float Length()
		{
			return (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
		}

		/// <summary>
		/// Calculates the length of the vector squared.
		/// </summary>
		public float LengthSquared()
		{
			return (float)((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float Distance(Vector4 value1, Vector4 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			return (float)Math.Sqrt((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3 + (double)num4 * (double)num4);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the vectors.</param>
		public static void Distance(ref Vector4 value1, ref Vector4 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			float num5 = (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3 + (double)num4 * (double)num4);
			result = (float)Math.Sqrt(num5);
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static float DistanceSquared(Vector4 value1, Vector4 value2)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			return (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3 + (double)num4 * (double)num4);
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the two vectors squared.</param>
		public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out float result)
		{
			float num = value1.X - value2.X;
			float num2 = value1.Y - value2.Y;
			float num3 = value1.Z - value2.Z;
			float num4 = value1.W - value2.W;
			result = (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3 + (double)num4 * (double)num4);
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param>
		public static float Dot(Vector4 vector1, Vector4 vector2)
		{
			return (float)((double)vector1.X * (double)vector2.X + (double)vector1.Y * (double)vector2.Y + (double)vector1.Z * (double)vector2.Z + (double)vector1.W * (double)vector2.W);
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The dot product of the two vectors.</param>
		public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out float result)
		{
			result = (float)((double)vector1.X * (double)vector2.X + (double)vector1.Y * (double)vector2.Y + (double)vector1.Z * (double)vector2.Z + (double)vector1.W * (double)vector2.W);
		}

		/// <summary>
		/// Turns the current vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			float num = 1f / (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
			X *= num;
			Y *= num;
			Z *= num;
			W *= num;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector.
		/// </summary>
		/// <param name="vector">The source Vector4.</param>
		public static Vector4 Normalize(Vector4 vector)
		{
			float num = 1f / (float)Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y + (double)vector.Z * (double)vector.Z + (double)vector.W * (double)vector.W);
			Vector4 result = default(Vector4);
			result.X = vector.X * num;
			result.Y = vector.Y * num;
			result.Z = vector.Z * num;
			result.W = vector.W * num;
			return result;
		}

		/// <summary>
		/// Returns a normalized version of the specified vector.
		/// </summary>
		/// <param name="vector">Source vector.</param><param name="result">[OutAttribute] The normalized vector.</param>
		public static void Normalize(ref Vector4 vector, out Vector4 result)
		{
			float num = 1f / (float)Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y + (double)vector.Z * (double)vector.Z + (double)vector.W * (double)vector.W);
			result.X = vector.X * num;
			result.Y = vector.Y * num;
			result.Z = vector.Z * num;
			result.W = vector.W * num;
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 Min(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = (((double)value1.X < (double)value2.X) ? value1.X : value2.X);
			result.Y = (((double)value1.Y < (double)value2.Y) ? value1.Y : value2.Y);
			result.Z = (((double)value1.Z < (double)value2.Z) ? value1.Z : value2.Z);
			result.W = (((double)value1.W < (double)value2.W) ? value1.W : value2.W);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The minimized vector.</param>
		public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = (((double)value1.X < (double)value2.X) ? value1.X : value2.X);
			result.Y = (((double)value1.Y < (double)value2.Y) ? value1.Y : value2.Y);
			result.Z = (((double)value1.Z < (double)value2.Z) ? value1.Z : value2.Z);
			result.W = (((double)value1.W < (double)value2.W) ? value1.W : value2.W);
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 Max(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = (((double)value1.X > (double)value2.X) ? value1.X : value2.X);
			result.Y = (((double)value1.Y > (double)value2.Y) ? value1.Y : value2.Y);
			result.Z = (((double)value1.Z > (double)value2.Z) ? value1.Z : value2.Z);
			result.W = (((double)value1.W > (double)value2.W) ? value1.W : value2.W);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The maximized vector.</param>
		public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = (((double)value1.X > (double)value2.X) ? value1.X : value2.X);
			result.Y = (((double)value1.Y > (double)value2.Y) ? value1.Y : value2.Y);
			result.Z = (((double)value1.Z > (double)value2.Z) ? value1.Z : value2.Z);
			result.W = (((double)value1.W > (double)value2.W) ? value1.W : value2.W);
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param>
		public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
		{
			float x = value1.X;
			float num = ((double)x > (double)max.X) ? max.X : x;
			float x2 = ((double)num < (double)min.X) ? min.X : num;
			float y = value1.Y;
			float num2 = ((double)y > (double)max.Y) ? max.Y : y;
			float y2 = ((double)num2 < (double)min.Y) ? min.Y : num2;
			float z = value1.Z;
			float num3 = ((double)z > (double)max.Z) ? max.Z : z;
			float z2 = ((double)num3 < (double)min.Z) ? min.Z : num3;
			float w = value1.W;
			float num4 = ((double)w > (double)max.W) ? max.W : w;
			float w2 = ((double)num4 < (double)min.W) ? min.W : num4;
			Vector4 result = default(Vector4);
			result.X = x2;
			result.Y = y2;
			result.Z = z2;
			result.W = w2;
			return result;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param><param name="result">[OutAttribute] The clamped value.</param>
		public static void Clamp(ref Vector4 value1, ref Vector4 min, ref Vector4 max, out Vector4 result)
		{
			float x = value1.X;
			float num = ((double)x > (double)max.X) ? max.X : x;
			float x2 = ((double)num < (double)min.X) ? min.X : num;
			float y = value1.Y;
			float num2 = ((double)y > (double)max.Y) ? max.Y : y;
			float y2 = ((double)num2 < (double)min.Y) ? min.Y : num2;
			float z = value1.Z;
			float num3 = ((double)z > (double)max.Z) ? max.Z : z;
			float z2 = ((double)num3 < (double)min.Z) ? min.Z : num3;
			float w = value1.W;
			float num4 = ((double)w > (double)max.W) ? max.W : w;
			float w2 = ((double)num4 < (double)min.W) ? min.W : num4;
			result.X = x2;
			result.Y = y2;
			result.Z = z2;
			result.W = w2;
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			result.W = value1.W + (value2.W - value1.W) * amount;
			return result;
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param><param name="result">[OutAttribute] The result of the interpolation.</param>
		public static void Lerp(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
		{
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			result.W = value1.W + (value2.W - value1.W) * amount;
		}

		/// <summary>
		/// Returns a Vector4 containing the 4D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 4D triangle.
		/// </summary>
		/// <param name="value1">A Vector4 containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector4 containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector4 containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
		public static Vector4 Barycentric(Vector4 value1, Vector4 value2, Vector4 value3, float amount1, float amount2)
		{
			Vector4 result = default(Vector4);
			result.X = (float)((double)value1.X + (double)amount1 * ((double)value2.X - (double)value1.X) + (double)amount2 * ((double)value3.X - (double)value1.X));
			result.Y = (float)((double)value1.Y + (double)amount1 * ((double)value2.Y - (double)value1.Y) + (double)amount2 * ((double)value3.Y - (double)value1.Y));
			result.Z = (float)((double)value1.Z + (double)amount1 * ((double)value2.Z - (double)value1.Z) + (double)amount2 * ((double)value3.Z - (double)value1.Z));
			result.W = (float)((double)value1.W + (double)amount1 * ((double)value2.W - (double)value1.W) + (double)amount2 * ((double)value3.W - (double)value1.W));
			return result;
		}

		/// <summary>
		/// Returns a Vector4 containing the 4D Cartesian coordinates of a point specified in Barycentric (areal) coordinates relative to a 4D triangle.
		/// </summary>
		/// <param name="value1">A Vector4 containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector4 containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector4 containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param><param name="result">[OutAttribute] The 4D Cartesian coordinates of the specified point are placed in this Vector4 on exit.</param>
		public static void Barycentric(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, float amount1, float amount2, out Vector4 result)
		{
			result.X = (float)((double)value1.X + (double)amount1 * ((double)value2.X - (double)value1.X) + (double)amount2 * ((double)value3.X - (double)value1.X));
			result.Y = (float)((double)value1.Y + (double)amount1 * ((double)value2.Y - (double)value1.Y) + (double)amount2 * ((double)value3.Y - (double)value1.Y));
			result.Z = (float)((double)value1.Z + (double)amount1 * ((double)value2.Z - (double)value1.Z) + (double)amount2 * ((double)value3.Z - (double)value1.Z));
			result.W = (float)((double)value1.W + (double)amount1 * ((double)value2.W - (double)value1.W) + (double)amount2 * ((double)value3.W - (double)value1.W));
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static Vector4 SmoothStep(Vector4 value1, Vector4 value2, float amount)
		{
			amount = (((double)amount > 1.0) ? 1f : (((double)amount < 0.0) ? 0f : amount));
			amount = (float)((double)amount * (double)amount * (3.0 - 2.0 * (double)amount));
			Vector4 result = default(Vector4);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			result.W = value1.W + (value2.W - value1.W) * amount;
			return result;
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] The interpolated value.</param>
		public static void SmoothStep(ref Vector4 value1, ref Vector4 value2, float amount, out Vector4 result)
		{
			amount = (((double)amount > 1.0) ? 1f : (((double)amount < 0.0) ? 0f : amount));
			amount = (float)((double)amount * (double)amount * (3.0 - 2.0 * (double)amount));
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			result.W = value1.W + (value2.W - value1.W) * amount;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param>
		public static Vector4 CatmullRom(Vector4 value1, Vector4 value2, Vector4 value3, Vector4 value4, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			Vector4 result = default(Vector4);
			result.X = (float)(0.5 * (2.0 * (double)value2.X + (0.0 - (double)value1.X + (double)value3.X) * (double)amount + (2.0 * (double)value1.X - 5.0 * (double)value2.X + 4.0 * (double)value3.X - (double)value4.X) * (double)num + (0.0 - (double)value1.X + 3.0 * (double)value2.X - 3.0 * (double)value3.X + (double)value4.X) * (double)num2));
			result.Y = (float)(0.5 * (2.0 * (double)value2.Y + (0.0 - (double)value1.Y + (double)value3.Y) * (double)amount + (2.0 * (double)value1.Y - 5.0 * (double)value2.Y + 4.0 * (double)value3.Y - (double)value4.Y) * (double)num + (0.0 - (double)value1.Y + 3.0 * (double)value2.Y - 3.0 * (double)value3.Y + (double)value4.Y) * (double)num2));
			result.Z = (float)(0.5 * (2.0 * (double)value2.Z + (0.0 - (double)value1.Z + (double)value3.Z) * (double)amount + (2.0 * (double)value1.Z - 5.0 * (double)value2.Z + 4.0 * (double)value3.Z - (double)value4.Z) * (double)num + (0.0 - (double)value1.Z + 3.0 * (double)value2.Z - 3.0 * (double)value3.Z + (double)value4.Z) * (double)num2));
			result.W = (float)(0.5 * (2.0 * (double)value2.W + (0.0 - (double)value1.W + (double)value3.W) * (double)amount + (2.0 * (double)value1.W - 5.0 * (double)value2.W + 4.0 * (double)value3.W - (double)value4.W) * (double)num + (0.0 - (double)value1.W + 3.0 * (double)value2.W - 3.0 * (double)value3.W + (double)value4.W) * (double)num2));
			return result;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
		public static void CatmullRom(ref Vector4 value1, ref Vector4 value2, ref Vector4 value3, ref Vector4 value4, float amount, out Vector4 result)
		{
			float num = amount * amount;
			float num2 = amount * num;
			result.X = (float)(0.5 * (2.0 * (double)value2.X + (0.0 - (double)value1.X + (double)value3.X) * (double)amount + (2.0 * (double)value1.X - 5.0 * (double)value2.X + 4.0 * (double)value3.X - (double)value4.X) * (double)num + (0.0 - (double)value1.X + 3.0 * (double)value2.X - 3.0 * (double)value3.X + (double)value4.X) * (double)num2));
			result.Y = (float)(0.5 * (2.0 * (double)value2.Y + (0.0 - (double)value1.Y + (double)value3.Y) * (double)amount + (2.0 * (double)value1.Y - 5.0 * (double)value2.Y + 4.0 * (double)value3.Y - (double)value4.Y) * (double)num + (0.0 - (double)value1.Y + 3.0 * (double)value2.Y - 3.0 * (double)value3.Y + (double)value4.Y) * (double)num2));
			result.Z = (float)(0.5 * (2.0 * (double)value2.Z + (0.0 - (double)value1.Z + (double)value3.Z) * (double)amount + (2.0 * (double)value1.Z - 5.0 * (double)value2.Z + 4.0 * (double)value3.Z - (double)value4.Z) * (double)num + (0.0 - (double)value1.Z + 3.0 * (double)value2.Z - 3.0 * (double)value3.Z + (double)value4.Z) * (double)num2));
			result.W = (float)(0.5 * (2.0 * (double)value2.W + (0.0 - (double)value1.W + (double)value3.W) * (double)amount + (2.0 * (double)value1.W - 5.0 * (double)value2.W + 4.0 * (double)value3.W - (double)value4.W) * (double)num + (0.0 - (double)value1.W + 3.0 * (double)value2.W - 3.0 * (double)value3.W + (double)value4.W) * (double)num2));
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param>
		public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
		{
			float num = amount * amount;
			float num2 = amount * num;
			float num3 = (float)(2.0 * (double)num2 - 3.0 * (double)num + 1.0);
			float num4 = (float)(-2.0 * (double)num2 + 3.0 * (double)num);
			float num5 = num2 - 2f * num + amount;
			float num6 = num2 - num;
			Vector4 result = default(Vector4);
			result.X = (float)((double)value1.X * (double)num3 + (double)value2.X * (double)num4 + (double)tangent1.X * (double)num5 + (double)tangent2.X * (double)num6);
			result.Y = (float)((double)value1.Y * (double)num3 + (double)value2.Y * (double)num4 + (double)tangent1.Y * (double)num5 + (double)tangent2.Y * (double)num6);
			result.Z = (float)((double)value1.Z * (double)num3 + (double)value2.Z * (double)num4 + (double)tangent1.Z * (double)num5 + (double)tangent2.Z * (double)num6);
			result.W = (float)((double)value1.W * (double)num3 + (double)value2.W * (double)num4 + (double)tangent1.W * (double)num5 + (double)tangent2.W * (double)num6);
			return result;
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
		public static void Hermite(ref Vector4 value1, ref Vector4 tangent1, ref Vector4 value2, ref Vector4 tangent2, float amount, out Vector4 result)
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
			result.W = (float)((double)value1.W * (double)num3 + (double)value2.W * (double)num4 + (double)tangent1.W * (double)num5 + (double)tangent2.W * (double)num6);
		}

		/// <summary>
		/// Transforms a Vector2 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector2.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4 Transform(Vector2 position, Matrix matrix)
		{
			float x = (float)((double)position.X * (double)matrix.M11 + (double)position.Y * (double)matrix.M21) + matrix.M41;
			float y = (float)((double)position.X * (double)matrix.M12 + (double)position.Y * (double)matrix.M22) + matrix.M42;
			float z = (float)((double)position.X * (double)matrix.M13 + (double)position.Y * (double)matrix.M23) + matrix.M43;
			float w = (float)((double)position.X * (double)matrix.M14 + (double)position.Y * (double)matrix.M24) + matrix.M44;
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
			return result;
		}

		/// <summary>
		/// Transforms a Vector2 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector2.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector4 result)
		{
			float x = (float)((double)position.X * (double)matrix.M11 + (double)position.Y * (double)matrix.M21) + matrix.M41;
			float y = (float)((double)position.X * (double)matrix.M12 + (double)position.Y * (double)matrix.M22) + matrix.M42;
			float z = (float)((double)position.X * (double)matrix.M13 + (double)position.Y * (double)matrix.M23) + matrix.M43;
			float w = (float)((double)position.X * (double)matrix.M14 + (double)position.Y * (double)matrix.M24) + matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector3 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4 Transform(Vector3 position, Matrix matrix)
		{
			float x = (float)((double)position.X * (double)matrix.M11 + (double)position.Y * (double)matrix.M21 + (double)position.Z * (double)matrix.M31) + matrix.M41;
			float y = (float)((double)position.X * (double)matrix.M12 + (double)position.Y * (double)matrix.M22 + (double)position.Z * (double)matrix.M32) + matrix.M42;
			float z = (float)((double)position.X * (double)matrix.M13 + (double)position.Y * (double)matrix.M23 + (double)position.Z * (double)matrix.M33) + matrix.M43;
			float w = (float)((double)position.X * (double)matrix.M14 + (double)position.Y * (double)matrix.M24 + (double)position.Z * (double)matrix.M34) + matrix.M44;
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector4 result)
		{
			float x = (float)((double)position.X * (double)matrix.M11 + (double)position.Y * (double)matrix.M21 + (double)position.Z * (double)matrix.M31) + matrix.M41;
			float y = (float)((double)position.X * (double)matrix.M12 + (double)position.Y * (double)matrix.M22 + (double)position.Z * (double)matrix.M32) + matrix.M42;
			float z = (float)((double)position.X * (double)matrix.M13 + (double)position.Y * (double)matrix.M23 + (double)position.Z * (double)matrix.M33) + matrix.M43;
			float w = (float)((double)position.X * (double)matrix.M14 + (double)position.Y * (double)matrix.M24 + (double)position.Z * (double)matrix.M34) + matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector4 by the specified Matrix.
		/// </summary>
		/// <param name="vector">The source Vector4.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4 Transform(Vector4 vector, Matrix matrix)
		{
			float x = (float)((double)vector.X * (double)matrix.M11 + (double)vector.Y * (double)matrix.M21 + (double)vector.Z * (double)matrix.M31 + (double)vector.W * (double)matrix.M41);
			float y = (float)((double)vector.X * (double)matrix.M12 + (double)vector.Y * (double)matrix.M22 + (double)vector.Z * (double)matrix.M32 + (double)vector.W * (double)matrix.M42);
			float z = (float)((double)vector.X * (double)matrix.M13 + (double)vector.Y * (double)matrix.M23 + (double)vector.Z * (double)matrix.M33 + (double)vector.W * (double)matrix.M43);
			float w = (float)((double)vector.X * (double)matrix.M14 + (double)vector.Y * (double)matrix.M24 + (double)vector.Z * (double)matrix.M34 + (double)vector.W * (double)matrix.M44);
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
			return result;
		}

		/// <summary>
		/// Transforms a Vector4 by the given Matrix.
		/// </summary>
		/// <param name="vector">The source Vector4.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector4 vector, ref Matrix matrix, out Vector4 result)
		{
			float x = (float)((double)vector.X * (double)matrix.M11 + (double)vector.Y * (double)matrix.M21 + (double)vector.Z * (double)matrix.M31 + (double)vector.W * (double)matrix.M41);
			float y = (float)((double)vector.X * (double)matrix.M12 + (double)vector.Y * (double)matrix.M22 + (double)vector.Z * (double)matrix.M32 + (double)vector.W * (double)matrix.M42);
			float z = (float)((double)vector.X * (double)matrix.M13 + (double)vector.Y * (double)matrix.M23 + (double)vector.Z * (double)matrix.M33 + (double)vector.W * (double)matrix.M43);
			float w = (float)((double)vector.X * (double)matrix.M14 + (double)vector.Y * (double)matrix.M24 + (double)vector.Z * (double)matrix.M34 + (double)vector.W * (double)matrix.M44);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector2 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector2 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4 Transform(Vector2 value, Quaternion rotation)
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
			float x = (float)((double)value.X * (1.0 - (double)num10 - (double)num12) + (double)value.Y * ((double)num8 - (double)num6));
			float y = (float)((double)value.X * ((double)num8 + (double)num6) + (double)value.Y * (1.0 - (double)num7 - (double)num12));
			float z = (float)((double)value.X * ((double)num9 - (double)num5) + (double)value.Y * ((double)num11 + (double)num4));
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1f;
			return result;
		}

		/// <summary>
		/// Transforms a Vector2 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector2 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector4 result)
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
			float x = (float)((double)value.X * (1.0 - (double)num10 - (double)num12) + (double)value.Y * ((double)num8 - (double)num6));
			float y = (float)((double)value.X * ((double)num8 + (double)num6) + (double)value.Y * (1.0 - (double)num7 - (double)num12));
			float z = (float)((double)value.X * ((double)num9 - (double)num5) + (double)value.Y * ((double)num11 + (double)num4));
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1f;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector3 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4 Transform(Vector3 value, Quaternion rotation)
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
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1f;
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector3 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector4 result)
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
			result.W = 1f;
		}

		/// <summary>
		/// Transforms a Vector4 by a specified Quaternion.
		/// </summary>
		/// <param name="value">The Vector4 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4 Transform(Vector4 value, Quaternion rotation)
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
			Vector4 result = default(Vector4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = value.W;
			return result;
		}

		/// <summary>
		/// Transforms a Vector4 by a specified Quaternion.
		/// </summary>
		/// <param name="value">The Vector4 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector4 value, ref Quaternion rotation, out Vector4 result)
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
			result.W = value.W;
		}

		/// <summary>
		/// Transforms an array of Vector4s by a specified Matrix.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
		public static void Transform(Vector4[] sourceArray, ref Matrix matrix, Vector4[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				float x = sourceArray[i].X;
				float y = sourceArray[i].Y;
				float z = sourceArray[i].Z;
				float w = sourceArray[i].W;
				destinationArray[i].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31 + (double)w * (double)matrix.M41);
				destinationArray[i].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32 + (double)w * (double)matrix.M42);
				destinationArray[i].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33 + (double)w * (double)matrix.M43);
				destinationArray[i].W = (float)((double)x * (double)matrix.M14 + (double)y * (double)matrix.M24 + (double)z * (double)matrix.M34 + (double)w * (double)matrix.M44);
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector4s by a specified Matrix into a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s containing the range to transform.</param><param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param><param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param><param name="length">The number of Vector4s to transform.</param>
		public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Matrix matrix, Vector4[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				float x = sourceArray[sourceIndex].X;
				float y = sourceArray[sourceIndex].Y;
				float z = sourceArray[sourceIndex].Z;
				float w = sourceArray[sourceIndex].W;
				destinationArray[destinationIndex].X = (float)((double)x * (double)matrix.M11 + (double)y * (double)matrix.M21 + (double)z * (double)matrix.M31 + (double)w * (double)matrix.M41);
				destinationArray[destinationIndex].Y = (float)((double)x * (double)matrix.M12 + (double)y * (double)matrix.M22 + (double)z * (double)matrix.M32 + (double)w * (double)matrix.M42);
				destinationArray[destinationIndex].Z = (float)((double)x * (double)matrix.M13 + (double)y * (double)matrix.M23 + (double)z * (double)matrix.M33 + (double)w * (double)matrix.M43);
				destinationArray[destinationIndex].W = (float)((double)x * (double)matrix.M14 + (double)y * (double)matrix.M24 + (double)z * (double)matrix.M34 + (double)w * (double)matrix.M44);
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms an array of Vector4s by a specified Quaternion.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
		public static void Transform(Vector4[] sourceArray, ref Quaternion rotation, Vector4[] destinationArray)
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
				destinationArray[i].W = sourceArray[i].W;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector4s by a specified Quaternion into a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s containing the range to transform.</param><param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param><param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param><param name="length">The number of Vector4s to transform.</param>
		public static void Transform(Vector4[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector4[] destinationArray, int destinationIndex, int length)
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
				float w = sourceArray[sourceIndex].W;
				destinationArray[destinationIndex].X = (float)((double)x * (double)num13 + (double)y * (double)num14 + (double)z * (double)num15);
				destinationArray[destinationIndex].Y = (float)((double)x * (double)num16 + (double)y * (double)num17 + (double)z * (double)num18);
				destinationArray[destinationIndex].Z = (float)((double)x * (double)num19 + (double)y * (double)num20 + (double)z * (double)num21);
				destinationArray[destinationIndex].W = w;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector4 Negate(Vector4 value)
		{
			Vector4 result = default(Vector4);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			result.W = 0f - value.W;
			return result;
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
		public static void Negate(ref Vector4 value, out Vector4 result)
		{
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			result.W = 0f - value.W;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 Add(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			result.W = value1.W + value2.W;
			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] Sum of the source vectors.</param>
		public static void Add(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			result.Z = value1.Z + value2.Z;
			result.W = value1.W + value2.W;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 Subtract(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			result.W = value1.W - value2.W;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the subtraction.</param>
		public static void Subtract(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			result.Z = value1.Z - value2.Z;
			result.W = value1.W - value2.W;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4 Multiply(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			result.W = value1.W * value2.W;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			result.Z = value1.Z * value2.Z;
			result.W = value1.W * value2.W;
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector4 Multiply(Vector4 value1, float scaleFactor)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
			result.W = value1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector4 value1, float scaleFactor, out Vector4 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			result.Z = value1.Z * scaleFactor;
			result.W = value1.W * scaleFactor;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector4 Divide(Vector4 value1, Vector4 value2)
		{
			Vector4 result = default(Vector4);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			result.W = value1.W / value2.W;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			result.Z = value1.Z / value2.Z;
			result.W = value1.W / value2.W;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector4 Divide(Vector4 value1, float divider)
		{
			float num = 1f / divider;
			Vector4 result = default(Vector4);
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
			result.W = value1.W * num;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector4 value1, float divider, out Vector4 result)
		{
			float num = 1f / divider;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
			result.W = value1.W * num;
		}
	}
}
