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
	public struct Vector4D : IEquatable<Vector4>
	{
		protected class VRageMath_Vector4D_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector4D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4D owner, in double value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4D owner, out double value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector4D_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector4D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4D owner, in double value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4D owner, out double value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector4D_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector4D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4D owner, in double value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4D owner, out double value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Vector4D_003C_003EW_003C_003EAccessor : IMemberAccessor<Vector4D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4D owner, in double value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4D owner, out double value)
			{
				value = owner.W;
			}
		}

		public static Vector4D Zero;

		public static Vector4D One;

		public static Vector4D UnitX;

		public static Vector4D UnitY;

		public static Vector4D UnitZ;

		public static Vector4D UnitW;

		/// <summary>
		/// Gets or sets the x-component of the vector.
		/// </summary>
		[ProtoMember(1)]
		public double X;

		/// <summary>
		/// Gets or sets the y-component of the vector.
		/// </summary>
		[ProtoMember(4)]
		public double Y;

		/// <summary>
		/// Gets or sets the z-component of the vector.
		/// </summary>
		[ProtoMember(7)]
		public double Z;

		/// <summary>
		/// Gets or sets the w-component of the vector.
		/// </summary>
		[ProtoMember(10)]
		public double W;

		static Vector4D()
		{
			Zero = default(Vector4D);
			One = new Vector4D(1.0, 1.0, 1.0, 1.0);
			UnitX = new Vector4D(1.0, 0.0, 0.0, 0.0);
			UnitY = new Vector4D(0.0, 1.0, 0.0, 0.0);
			UnitZ = new Vector4D(0.0, 0.0, 1.0, 0.0);
			UnitW = new Vector4D(0.0, 0.0, 0.0, 1.0);
		}

		/// <summary>
		/// Initializes a new instance of Vector4.
		/// </summary>
		/// <param name="x">Initial value for the x-component of the vector.</param><param name="y">Initial value for the y-component of the vector.</param><param name="z">Initial value for the z-component of the vector.</param><param name="w">Initial value for the w-component of the vector.</param>
		public Vector4D(double x, double y, double z, double w)
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
		public Vector4D(Vector2 value, double z, double w)
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
		public Vector4D(Vector3D value, double w)
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
		public Vector4D(double value)
		{
			X = (Y = (Z = (W = value)));
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector4D operator -(Vector4D value)
		{
			Vector4D result = default(Vector4D);
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			result.Z = 0.0 - value.Z;
			result.W = 0.0 - value.W;
			return result;
		}

		/// <summary>
		/// Tests vectors for equality.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static bool operator ==(Vector4D value1, Vector4D value2)
		{
			if (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z)
			{
				return value1.W == value2.W;
			}
			return false;
		}

		/// <summary>
		/// Tests vectors for inequality.
		/// </summary>
		/// <param name="value1">Vector to compare.</param><param name="value2">Vector to compare.</param>
		public static bool operator !=(Vector4D value1, Vector4D value2)
		{
			if (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z)
			{
				return value1.W != value2.W;
			}
			return true;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4D operator +(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator -(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator *(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator *(Vector4D value1, double scaleFactor)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator *(double scaleFactor, Vector4D value1)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator /(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator /(Vector4D value1, double divider)
		{
			double num = 1.0 / divider;
			Vector4D result = default(Vector4D);
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
		public static Vector4D operator /(double lhs, Vector4D rhs)
		{
			Vector4D result = default(Vector4D);
			result.X = lhs / rhs.X;
			result.Y = lhs / rhs.Y;
			result.Z = lhs / rhs.Z;
			result.W = lhs / rhs.W;
			return result;
		}

		public static Vector4D PackOrthoMatrix(Vector3D position, Vector3 forward, Vector3 up)
		{
			int direction = (int)Base6Directions.GetDirection(forward);
			int direction2 = (int)Base6Directions.GetDirection(up);
			return new Vector4D(position, direction * 6 + direction2);
		}

		public static Vector4D PackOrthoMatrix(ref MatrixD matrix)
		{
			int direction = (int)Base6Directions.GetDirection(matrix.Forward);
			int direction2 = (int)Base6Directions.GetDirection(matrix.Up);
			return new Vector4D(matrix.Translation, direction * 6 + direction2);
		}

		public static MatrixD UnpackOrthoMatrix(ref Vector4D packed)
		{
			int num = (int)packed.W;
			return MatrixD.CreateWorld(new Vector3(packed), Base6Directions.GetVector(num / 6), Base6Directions.GetVector(num % 6));
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
			if (X == (double)other.X && Y == (double)other.Y && Z == (double)other.Z)
			{
				return W == (double)other.W;
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
		public double Length()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}

		/// <summary>
		/// Calculates the length of the vector squared.
		/// </summary>
		public double LengthSquared()
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static double Distance(Vector4 value1, Vector4 value2)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num3 = value1.Z - value2.Z;
			double num4 = value1.W - value2.W;
			return Math.Sqrt(num * num + num2 * num2 + num3 * num3 + num4 * num4);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the vectors.</param>
		public static void Distance(ref Vector4 value1, ref Vector4 value2, out double result)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num3 = value1.Z - value2.Z;
			double num4 = value1.W - value2.W;
			double num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
			result = Math.Sqrt(num5);
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static double DistanceSquared(Vector4 value1, Vector4 value2)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num3 = value1.Z - value2.Z;
			double num4 = value1.W - value2.W;
			return num * num + num2 * num2 + num3 * num3 + num4 * num4;
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the two vectors squared.</param>
		public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out double result)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num3 = value1.Z - value2.Z;
			double num4 = value1.W - value2.W;
			result = num * num + num2 * num2 + num3 * num3 + num4 * num4;
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param>
		public static double Dot(Vector4 vector1, Vector4 vector2)
		{
			return (double)vector1.X * (double)vector2.X + (double)vector1.Y * (double)vector2.Y + (double)vector1.Z * (double)vector2.Z + (double)vector1.W * (double)vector2.W;
		}

		/// <summary>
		/// Calculates the dot product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The dot product of the two vectors.</param>
		public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out double result)
		{
			result = (double)vector1.X * (double)vector2.X + (double)vector1.Y * (double)vector2.Y + (double)vector1.Z * (double)vector2.Z + (double)vector1.W * (double)vector2.W;
		}

		/// <summary>
		/// Turns the current vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			double num = 1.0 / Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
			X *= num;
			Y *= num;
			Z *= num;
			W *= num;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector.
		/// </summary>
		/// <param name="vector">The source Vector4.</param>
		public static Vector4D Normalize(Vector4D vector)
		{
			double num = 1.0 / Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W);
			Vector4D result = default(Vector4D);
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
		public static void Normalize(ref Vector4D vector, out Vector4D result)
		{
			double num = 1.0 / Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W);
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
		public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max)
		{
			double x = value1.X;
			double num = (x > max.X) ? max.X : x;
			double x2 = (num < min.X) ? min.X : num;
			double y = value1.Y;
			double num2 = (y > max.Y) ? max.Y : y;
			double y2 = (num2 < min.Y) ? min.Y : num2;
			double z = value1.Z;
			double num3 = (z > max.Z) ? max.Z : z;
			double z2 = (num3 < min.Z) ? min.Z : num3;
			double w = value1.W;
			double num4 = (w > max.W) ? max.W : w;
			double w2 = (num4 < min.W) ? min.W : num4;
			Vector4D result = default(Vector4D);
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
		public static void Clamp(ref Vector4D value1, ref Vector4D min, ref Vector4D max, out Vector4D result)
		{
			double x = value1.X;
			double num = (x > max.X) ? max.X : x;
			double x2 = (num < min.X) ? min.X : num;
			double y = value1.Y;
			double num2 = (y > max.Y) ? max.Y : y;
			double y2 = (num2 < min.Y) ? min.Y : num2;
			double z = value1.Z;
			double num3 = (z > max.Z) ? max.Z : z;
			double z2 = (num3 < min.Z) ? min.Z : num3;
			double w = value1.W;
			double num4 = (w > max.W) ? max.W : w;
			double w2 = (num4 < min.W) ? min.W : num4;
			result.X = x2;
			result.Y = y2;
			result.Z = z2;
			result.W = w2;
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount)
		{
			Vector4D result = default(Vector4D);
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
		public static void Lerp(ref Vector4D value1, ref Vector4D value2, double amount, out Vector4D result)
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
		public static Vector4D Barycentric(Vector4D value1, Vector4D value2, Vector4D value3, double amount1, double amount2)
		{
			Vector4D result = default(Vector4D);
			result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
			result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
			result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
			result.W = value1.W + amount1 * (value2.W - value1.W) + amount2 * (value3.W - value1.W);
			return result;
		}

		/// <summary>
		/// Returns a Vector4 containing the 4D Cartesian coordinates of a point specified in Barycentric (areal) coordinates relative to a 4D triangle.
		/// </summary>
		/// <param name="value1">A Vector4 containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector4 containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector4 containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param><param name="result">[OutAttribute] The 4D Cartesian coordinates of the specified point are placed in this Vector4 on exit.</param>
		public static void Barycentric(ref Vector4D value1, ref Vector4D value2, ref Vector4D value3, double amount1, double amount2, out Vector4D result)
		{
			result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
			result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
			result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
			result.W = value1.W + amount1 * (value2.W - value1.W) + amount2 * (value3.W - value1.W);
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static Vector4D SmoothStep(Vector4D value1, Vector4D value2, double amount)
		{
			amount = ((amount > 1.0) ? 1.0 : ((amount < 0.0) ? 0.0 : amount));
			amount = amount * amount * (3.0 - 2.0 * amount);
			Vector4D result = default(Vector4D);
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
		public static void SmoothStep(ref Vector4D value1, ref Vector4D value2, double amount, out Vector4D result)
		{
			amount = ((amount > 1.0) ? 1.0 : ((amount < 0.0) ? 0.0 : amount));
			amount = amount * amount * (3.0 - 2.0 * amount);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			result.Z = value1.Z + (value2.Z - value1.Z) * amount;
			result.W = value1.W + (value2.W - value1.W) * amount;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param>
		public static Vector4D CatmullRom(Vector4D value1, Vector4D value2, Vector4D value3, Vector4D value4, double amount)
		{
			double num = amount * amount;
			double num2 = amount * num;
			Vector4D result = default(Vector4D);
			result.X = 0.5 * (2.0 * value2.X + (0.0 - value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num + (0.0 - value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
			result.Y = 0.5 * (2.0 * value2.Y + (0.0 - value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num + (0.0 - value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
			result.Z = 0.5 * (2.0 * value2.Z + (0.0 - value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num + (0.0 - value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
			result.W = 0.5 * (2.0 * value2.W + (0.0 - value1.W + value3.W) * amount + (2.0 * value1.W - 5.0 * value2.W + 4.0 * value3.W - value4.W) * num + (0.0 - value1.W + 3.0 * value2.W - 3.0 * value3.W + value4.W) * num2);
			return result;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
		public static void CatmullRom(ref Vector4D value1, ref Vector4D value2, ref Vector4D value3, ref Vector4D value4, double amount, out Vector4D result)
		{
			double num = amount * amount;
			double num2 = amount * num;
			result.X = 0.5 * (2.0 * value2.X + (0.0 - value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num + (0.0 - value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
			result.Y = 0.5 * (2.0 * value2.Y + (0.0 - value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num + (0.0 - value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
			result.Z = 0.5 * (2.0 * value2.Z + (0.0 - value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num + (0.0 - value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
			result.W = 0.5 * (2.0 * value2.W + (0.0 - value1.W + value3.W) * amount + (2.0 * value1.W - 5.0 * value2.W + 4.0 * value3.W - value4.W) * num + (0.0 - value1.W + 3.0 * value2.W - 3.0 * value3.W + value4.W) * num2);
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param>
		public static Vector4D Hermite(Vector4D value1, Vector4D tangent1, Vector4D value2, Vector4D tangent2, double amount)
		{
			double num = amount * amount;
			double num2 = amount * num;
			double num3 = 2.0 * num2 - 3.0 * num + 1.0;
			double num4 = -2.0 * num2 + 3.0 * num;
			double num5 = num2 - 2.0 * num + amount;
			double num6 = num2 - num;
			Vector4D result = default(Vector4D);
			result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
			result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
			result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
			result.W = value1.W * num3 + value2.W * num4 + tangent1.W * num5 + tangent2.W * num6;
			return result;
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
		public static void Hermite(ref Vector4D value1, ref Vector4D tangent1, ref Vector4D value2, ref Vector4D tangent2, double amount, out Vector4D result)
		{
			double num = amount * amount;
			double num2 = amount * num;
			double num3 = 2.0 * num2 - 3.0 * num + 1.0;
			double num4 = -2.0 * num2 + 3.0 * num;
			double num5 = num2 - 2.0 * num + amount;
			double num6 = num2 - num;
			result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
			result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
			result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
			result.W = value1.W * num3 + value2.W * num4 + tangent1.W * num5 + tangent2.W * num6;
		}

		/// <summary>
		/// Transforms a Vector2 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector2.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4D Transform(Vector2 position, MatrixD matrix)
		{
			double x = (double)position.X * matrix.M11 + (double)position.Y * matrix.M21 + matrix.M41;
			double y = (double)position.X * matrix.M12 + (double)position.Y * matrix.M22 + matrix.M42;
			double z = (double)position.X * matrix.M13 + (double)position.Y * matrix.M23 + matrix.M43;
			double w = (double)position.X * matrix.M14 + (double)position.Y * matrix.M24 + matrix.M44;
			Vector4D result = default(Vector4D);
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
		public static void Transform(ref Vector2 position, ref MatrixD matrix, out Vector4D result)
		{
			double x = (double)position.X * matrix.M11 + (double)position.Y * matrix.M21 + matrix.M41;
			double y = (double)position.X * matrix.M12 + (double)position.Y * matrix.M22 + matrix.M42;
			double z = (double)position.X * matrix.M13 + (double)position.Y * matrix.M23 + matrix.M43;
			double w = (double)position.X * matrix.M14 + (double)position.Y * matrix.M24 + matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector3 by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4D Transform(Vector3D position, MatrixD matrix)
		{
			double x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
			double y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
			double z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
			double w = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
			Vector4D result = default(Vector4D);
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
		public static void Transform(ref Vector3D position, ref MatrixD matrix, out Vector4D result)
		{
			double x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
			double y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
			double z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
			double w = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector4 by the specified Matrix.
		/// </summary>
		/// <param name="vector">The source Vector4.</param><param name="matrix">The transformation Matrix.</param>
		public static Vector4D Transform(Vector4D vector, MatrixD matrix)
		{
			double x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41;
			double y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42;
			double z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43;
			double w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44;
			Vector4D result = default(Vector4D);
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
		public static void Transform(ref Vector4D vector, ref MatrixD matrix, out Vector4D result)
		{
			double x = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41;
			double y = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42;
			double z = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43;
			double w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44;
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = w;
		}

		/// <summary>
		/// Transforms a Vector2 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector2 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4D Transform(Vector2 value, Quaternion rotation)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = (double)value.X * (1.0 - num10 - num12) + (double)value.Y * (num8 - num6);
			double y = (double)value.X * (num8 + num6) + (double)value.Y * (1.0 - num7 - num12);
			double z = (double)value.X * (num9 - num5) + (double)value.Y * (num11 + num4);
			Vector4D result = default(Vector4D);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1.0;
			return result;
		}

		/// <summary>
		/// Transforms a Vector2 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector2 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector4D result)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = (double)value.X * (1.0 - num10 - num12) + (double)value.Y * (num8 - num6);
			double y = (double)value.X * (num8 + num6) + (double)value.Y * (1.0 - num7 - num12);
			double z = (double)value.X * (num9 - num5) + (double)value.Y * (num11 + num4);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1.0;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector3 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4D Transform(Vector3D value, Quaternion rotation)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
			double y = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
			double z = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
			Vector4D result = default(Vector4D);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1.0;
			return result;
		}

		/// <summary>
		/// Transforms a Vector3 by a specified Quaternion into a Vector4.
		/// </summary>
		/// <param name="value">The Vector3 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] The Vector4 resulting from the transformation.</param>
		public static void Transform(ref Vector3D value, ref Quaternion rotation, out Vector4D result)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
			double y = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
			double z = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = 1.0;
		}

		/// <summary>
		/// Transforms a Vector4 by a specified Quaternion.
		/// </summary>
		/// <param name="value">The Vector4 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector4D Transform(Vector4D value, Quaternion rotation)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
			double y = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
			double z = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
			Vector4D result = default(Vector4D);
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
		public static void Transform(ref Vector4D value, ref Quaternion rotation, out Vector4D result)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
			double y = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
			double z = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
			result.X = x;
			result.Y = y;
			result.Z = z;
			result.W = value.W;
		}

		/// <summary>
		/// Transforms an array of Vector4s by a specified Matrix.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
		public static void Transform(Vector4D[] sourceArray, ref MatrixD matrix, Vector4D[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				double x = sourceArray[i].X;
				double y = sourceArray[i].Y;
				double z = sourceArray[i].Z;
				double w = sourceArray[i].W;
				destinationArray[i].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + w * matrix.M41;
				destinationArray[i].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + w * matrix.M42;
				destinationArray[i].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + w * matrix.M43;
				destinationArray[i].W = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + w * matrix.M44;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector4s by a specified Matrix into a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s containing the range to transform.</param><param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param><param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param><param name="length">The number of Vector4s to transform.</param>
		public static void Transform(Vector4D[] sourceArray, int sourceIndex, ref MatrixD matrix, Vector4D[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				double x = sourceArray[sourceIndex].X;
				double y = sourceArray[sourceIndex].Y;
				double z = sourceArray[sourceIndex].Z;
				double w = sourceArray[sourceIndex].W;
				destinationArray[destinationIndex].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + w * matrix.M41;
				destinationArray[destinationIndex].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + w * matrix.M42;
				destinationArray[destinationIndex].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + w * matrix.M43;
				destinationArray[destinationIndex].W = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + w * matrix.M44;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms an array of Vector4s by a specified Quaternion.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The existing destination array into which the transformed Vector4s are written.</param>
		public static void Transform(Vector4D[] sourceArray, ref Quaternion rotation, Vector4D[] destinationArray)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double num13 = 1.0 - num10 - num12;
			double num14 = num8 - num6;
			double num15 = num9 + num5;
			double num16 = num8 + num6;
			double num17 = 1.0 - num7 - num12;
			double num18 = num11 - num4;
			double num19 = num9 - num5;
			double num20 = num11 + num4;
			double num21 = 1.0 - num7 - num10;
			for (int i = 0; i < sourceArray.Length; i++)
			{
				double x = sourceArray[i].X;
				double y = sourceArray[i].Y;
				double z = sourceArray[i].Z;
				destinationArray[i].X = x * num13 + y * num14 + z * num15;
				destinationArray[i].Y = x * num16 + y * num17 + z * num18;
				destinationArray[i].Z = x * num19 + y * num20 + z * num21;
				destinationArray[i].W = sourceArray[i].W;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector4s by a specified Quaternion into a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The array of Vector4s containing the range to transform.</param><param name="sourceIndex">The index in the source array of the first Vector4 to transform.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The existing destination array of Vector4s into which to write the results.</param><param name="destinationIndex">The index in the destination array of the first result Vector4 to write.</param><param name="length">The number of Vector4s to transform.</param>
		public static void Transform(Vector4D[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector4D[] destinationArray, int destinationIndex, int length)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num;
			double num5 = (double)rotation.W * num2;
			double num6 = (double)rotation.W * num3;
			double num7 = (double)rotation.X * num;
			double num8 = (double)rotation.X * num2;
			double num9 = (double)rotation.X * num3;
			double num10 = (double)rotation.Y * num2;
			double num11 = (double)rotation.Y * num3;
			double num12 = (double)rotation.Z * num3;
			double num13 = 1.0 - num10 - num12;
			double num14 = num8 - num6;
			double num15 = num9 + num5;
			double num16 = num8 + num6;
			double num17 = 1.0 - num7 - num12;
			double num18 = num11 - num4;
			double num19 = num9 - num5;
			double num20 = num11 + num4;
			double num21 = 1.0 - num7 - num10;
			while (length > 0)
			{
				double x = sourceArray[sourceIndex].X;
				double y = sourceArray[sourceIndex].Y;
				double z = sourceArray[sourceIndex].Z;
				double w = sourceArray[sourceIndex].W;
				destinationArray[destinationIndex].X = x * num13 + y * num14 + z * num15;
				destinationArray[destinationIndex].Y = x * num16 + y * num17 + z * num18;
				destinationArray[destinationIndex].Z = x * num19 + y * num20 + z * num21;
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
		public static Vector4D Negate(Vector4D value)
		{
			Vector4D result = default(Vector4D);
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			result.Z = 0.0 - value.Z;
			result.W = 0.0 - value.W;
			return result;
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
		public static void Negate(ref Vector4D value, out Vector4D result)
		{
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			result.Z = 0.0 - value.Z;
			result.W = 0.0 - value.W;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector4D Add(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static void Add(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
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
		public static void Subtract(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
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
		public static Vector4D Multiply(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static Vector4D Multiply(Vector4D value1, double scaleFactor)
		{
			Vector4D result = default(Vector4D);
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
		public static void Multiply(ref Vector4D value1, double scaleFactor, out Vector4D result)
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
		public static Vector4D Divide(Vector4D value1, Vector4D value2)
		{
			Vector4D result = default(Vector4D);
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
		public static void Divide(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
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
		public static Vector4D Divide(Vector4D value1, double divider)
		{
			double num = 1.0 / divider;
			Vector4D result = default(Vector4D);
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
		public static void Divide(ref Vector4D value1, double divider, out Vector4D result)
		{
			double num = 1.0 / divider;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			result.Z = value1.Z * num;
			result.W = value1.W * num;
		}

		public static implicit operator Vector4(Vector4D v)
		{
			return new Vector4((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);
		}

		public static implicit operator Vector4D(Vector4 v)
		{
			return new Vector4D(v.X, v.Y, v.Z, v.W);
		}
	}
}
