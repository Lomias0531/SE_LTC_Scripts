using ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a vector with two components.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct Vector2D : IEquatable<Vector2D>
	{
		protected class VRageMath_Vector2D_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector2D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector2D owner, in double value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector2D owner, out double value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector2D_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector2D, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector2D owner, in double value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector2D owner, out double value)
			{
				value = owner.Y;
			}
		}

		public static Vector2D Zero;

		public static Vector2D One;

		public static Vector2D UnitX;

		public static Vector2D UnitY;

		public static Vector2D PositiveInfinity;

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

		public double this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return X;
				case 1:
					return Y;
				default:
					throw new ArgumentException();
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
				default:
					throw new ArgumentException();
				}
			}
		}

		static Vector2D()
		{
			Zero = default(Vector2D);
			One = new Vector2D(1.0, 1.0);
			UnitX = new Vector2D(1.0, 0.0);
			UnitY = new Vector2D(0.0, 1.0);
			PositiveInfinity = One * double.PositiveInfinity;
		}

		/// <summary>
		/// Initializes a new instance of Vector2D.
		/// </summary>
		/// <param name="x">Initial value for the x-component of the vector.</param><param name="y">Initial value for the y-component of the vector.</param>
		public Vector2D(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new instance of Vector2D.
		/// </summary>
		/// <param name="value">Value to initialize both components to.</param>
		public Vector2D(double value)
		{
			X = (Y = value);
		}

		public static explicit operator Vector2I(Vector2D vector)
		{
			return new Vector2I(vector);
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector2D operator -(Vector2D value)
		{
			Vector2D result = default(Vector2D);
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			return result;
		}

		/// <summary>
		/// Tests vectors for equality.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static bool operator ==(Vector2D value1, Vector2D value2)
		{
			if (value1.X == value2.X)
			{
				return value1.Y == value2.Y;
			}
			return false;
		}

		/// <summary>
		/// Tests vectors for inequality.
		/// </summary>
		/// <param name="value1">Vector to compare.</param><param name="value2">Vector to compare.</param>
		public static bool operator !=(Vector2D value1, Vector2D value2)
		{
			if (value1.X == value2.X)
			{
				return value1.Y != value2.Y;
			}
			return true;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D operator +(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			return result;
		}

		/// <summary>
		/// Adds double to each component of a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source double.</param>
		public static Vector2D operator +(Vector2D value1, double value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X + value2;
			result.Y = value1.Y + value2;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">source vector.</param>
		public static Vector2D operator -(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">source vector.</param>
		public static Vector2D operator -(Vector2D value1, double value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X - value2;
			result.Y = value1.Y - value2;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D operator *(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector2D operator *(Vector2D value, double scaleFactor)
		{
			Vector2D result = default(Vector2D);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="scaleFactor">Scalar value.</param><param name="value">Source vector.</param>
		public static Vector2D operator *(double scaleFactor, Vector2D value)
		{
			Vector2D result = default(Vector2D);
			result.X = value.X * scaleFactor;
			result.Y = value.Y * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector2D operator /(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector2D operator /(Vector2D value1, double divider)
		{
			double num = 1.0 / divider;
			Vector2D result = default(Vector2D);
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			return result;
		}

		/// <summary>
		/// Divides a scalar value by a vector.
		/// </summary>
		public static Vector2D operator /(double value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1 / value2.X;
			result.Y = value1 / value2.Y;
			return result;
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[2]
			{
				X.ToString(currentCulture),
				Y.ToString(currentCulture)
			});
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Vector2D.
		/// </summary>
		/// <param name="other">The Object to compare with the current Vector2D.</param>
		public bool Equals(Vector2D other)
		{
			if (X == other.X)
			{
				return Y == other.Y;
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
			if (obj is Vector2D)
			{
				result = Equals((Vector2D)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code of the vector object.
		/// </summary>
		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode();
		}

		public bool IsValid()
		{
			return (X * Y).IsValid();
		}

		[Conditional("DEBUG")]
		public void AssertIsValid()
		{
		}

		/// <summary>
		/// Calculates the length of the vector.
		/// </summary>
		public double Length()
		{
			return Math.Sqrt(X * X + Y * Y);
		}

		/// <summary>
		/// Calculates the length of the vector squared.
		/// </summary>
		public double LengthSquared()
		{
			return X * X + Y * Y;
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static double Distance(Vector2D value1, Vector2D value2)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			return Math.Sqrt(num * num + num2 * num2);
		}

		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the vectors.</param>
		public static void Distance(ref Vector2D value1, ref Vector2D value2, out double result)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num3 = num * num + num2 * num2;
			result = Math.Sqrt(num3);
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static double DistanceSquared(Vector2D value1, Vector2D value2)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			return num * num + num2 * num2;
		}

		/// <summary>
		/// Calculates the distance between two vectors squared.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The distance between the vectors squared.</param>
		public static void DistanceSquared(ref Vector2D value1, ref Vector2D value2, out double result)
		{
			double num = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			result = num * num + num2 * num2;
		}

		/// <summary>
		/// Calculates the dot product of two vectors. If the two vectors are unit vectors, the dot product returns a doubleing point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static double Dot(Vector2D value1, Vector2D value2)
		{
			return value1.X * value2.X + value1.Y * value2.Y;
		}

		/// <summary>
		/// Calculates the dot product of two vectors and writes the result to a user-specified variable. If the two vectors are unit vectors, the dot product returns a doubleing point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The dot product of the two vectors.</param>
		public static void Dot(ref Vector2D value1, ref Vector2D value2, out double result)
		{
			result = value1.X * value2.X + value1.Y * value2.Y;
		}

		/// <summary>
		/// Turns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		public void Normalize()
		{
			double num = 1.0 / Math.Sqrt(X * X + Y * Y);
			X *= num;
			Y *= num;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		/// <param name="value">Source Vector2D.</param>
		public static Vector2D Normalize(Vector2D value)
		{
			double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y);
			Vector2D result = default(Vector2D);
			result.X = value.X * num;
			result.Y = value.Y * num;
			return result;
		}

		/// <summary>
		/// Creates a unit vector from the specified vector, writing the result to a user-specified variable. The result is a vector one unit in length pointing in the same direction as the original vector.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] Normalized vector.</param>
		public static void Normalize(ref Vector2D value, out Vector2D result)
		{
			double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y);
			result.X = value.X * num;
			result.Y = value.Y * num;
		}

		/// <summary>
		/// Determines the reflect vector of the given vector and normal.
		/// </summary>
		/// <param name="vector">Source vector.</param><param name="normal">Normal of vector.</param>
		public static Vector2D Reflect(Vector2D vector, Vector2D normal)
		{
			double num = vector.X * normal.X + vector.Y * normal.Y;
			Vector2D result = default(Vector2D);
			result.X = vector.X - 2.0 * num * normal.X;
			result.Y = vector.Y - 2.0 * num * normal.Y;
			return result;
		}

		/// <summary>
		/// Determines the reflect vector of the given vector and normal.
		/// </summary>
		/// <param name="vector">Source vector.</param><param name="normal">Normal of vector.</param><param name="result">[OutAttribute] The created reflect vector.</param>
		public static void Reflect(ref Vector2D vector, ref Vector2D normal, out Vector2D result)
		{
			double num = vector.X * normal.X + vector.Y * normal.Y;
			result.X = vector.X - 2.0 * num * normal.X;
			result.Y = vector.Y - 2.0 * num * normal.Y;
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D Min(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the lowest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The minimized vector.</param>
		public static void Min(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D Max(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			return result;
		}

		/// <summary>
		/// Returns a vector that contains the highest value from each matching pair of components.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The maximized vector.</param>
		public static void Max(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param>
		public static Vector2D Clamp(Vector2D value1, Vector2D min, Vector2D max)
		{
			double x = value1.X;
			double num = (x > max.X) ? max.X : x;
			double x2 = (num < min.X) ? min.X : num;
			double y = value1.Y;
			double num2 = (y > max.Y) ? max.Y : y;
			double y2 = (num2 < min.Y) ? min.Y : num2;
			Vector2D result = default(Vector2D);
			result.X = x2;
			result.Y = y2;
			return result;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value1">The value to clamp.</param><param name="min">The minimum value.</param><param name="max">The maximum value.</param><param name="result">[OutAttribute] The clamped value.</param>
		public static void Clamp(ref Vector2D value1, ref Vector2D min, ref Vector2D max, out Vector2D result)
		{
			double x = value1.X;
			double num = (x > max.X) ? max.X : x;
			double x2 = (num < min.X) ? min.X : num;
			double y = value1.Y;
			double num2 = (y > max.Y) ? max.Y : y;
			double y2 = (num2 < min.Y) ? min.Y : num2;
			result.X = x2;
			result.Y = y2;
		}

		public static Vector2D ClampToSphere(Vector2D vector, double radius)
		{
			double num = vector.LengthSquared();
			double num2 = radius * radius;
			if (num > num2)
			{
				return vector * Math.Sqrt(num2 / num);
			}
			return vector;
		}

		public static void ClampToSphere(ref Vector2D vector, double radius)
		{
			double num = vector.LengthSquared();
			double num2 = radius * radius;
			if (num > num2)
			{
				vector *= Math.Sqrt(num2 / num);
			}
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		public static Vector2D Lerp(Vector2D value1, Vector2D value2, double amount)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			return result;
		}

		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="amount">Value between 0 and 1 indicating the weight of value2.</param><param name="result">[OutAttribute] The result of the interpolation.</param>
		public static void Lerp(ref Vector2D value1, ref Vector2D value2, double amount, out Vector2D result)
		{
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
		}

		/// <summary>
		/// Returns a Vector2D containing the 2D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 2D triangle.
		/// </summary>
		/// <param name="value1">A Vector2D containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector2D containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector2D containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
		public static Vector2D Barycentric(Vector2D value1, Vector2D value2, Vector2D value3, double amount1, double amount2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
			result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
			return result;
		}

		/// <summary>
		/// Returns a Vector2D containing the 2D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 2D triangle.
		/// </summary>
		/// <param name="value1">A Vector2D containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param><param name="value2">A Vector2D containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param><param name="value3">A Vector2D containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param><param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param><param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param><param name="result">[OutAttribute] The 2D Cartesian coordinates of the specified point are placed in this Vector2D on exit.</param>
		public static void Barycentric(ref Vector2D value1, ref Vector2D value2, ref Vector2D value3, double amount1, double amount2, out Vector2D result)
		{
			result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
			result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param>
		public static Vector2D SmoothStep(Vector2D value1, Vector2D value2, double amount)
		{
			amount = ((amount > 1.0) ? 1.0 : ((amount < 0.0) ? 0.0 : amount));
			amount = amount * amount * (3.0 - 2.0 * amount);
			Vector2D result = default(Vector2D);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
			return result;
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param><param name="value2">Source value.</param><param name="amount">Weighting value.</param><param name="result">[OutAttribute] The interpolated value.</param>
		public static void SmoothStep(ref Vector2D value1, ref Vector2D value2, double amount, out Vector2D result)
		{
			amount = ((amount > 1.0) ? 1.0 : ((amount < 0.0) ? 0.0 : amount));
			amount = amount * amount * (3.0 - 2.0 * amount);
			result.X = value1.X + (value2.X - value1.X) * amount;
			result.Y = value1.Y + (value2.Y - value1.Y) * amount;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param>
		public static Vector2D CatmullRom(Vector2D value1, Vector2D value2, Vector2D value3, Vector2D value4, double amount)
		{
			double num = amount * amount;
			double num2 = amount * num;
			Vector2D result = default(Vector2D);
			result.X = 0.5 * (2.0 * value2.X + (0.0 - value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num + (0.0 - value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
			result.Y = 0.5 * (2.0 * value2.Y + (0.0 - value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num + (0.0 - value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
			return result;
		}

		/// <summary>
		/// Performs a Catmull-Rom interpolation using the specified positions.
		/// </summary>
		/// <param name="value1">The first position in the interpolation.</param><param name="value2">The second position in the interpolation.</param><param name="value3">The third position in the interpolation.</param><param name="value4">The fourth position in the interpolation.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
		public static void CatmullRom(ref Vector2D value1, ref Vector2D value2, ref Vector2D value3, ref Vector2D value4, double amount, out Vector2D result)
		{
			double num = amount * amount;
			double num2 = amount * num;
			result.X = 0.5 * (2.0 * value2.X + (0.0 - value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num + (0.0 - value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
			result.Y = 0.5 * (2.0 * value2.Y + (0.0 - value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num + (0.0 - value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param>
		public static Vector2D Hermite(Vector2D value1, Vector2D tangent1, Vector2D value2, Vector2D tangent2, double amount)
		{
			double num = amount * amount;
			double num2 = amount * num;
			double num3 = 2.0 * num2 - 3.0 * num + 1.0;
			double num4 = -2.0 * num2 + 3.0 * num;
			double num5 = num2 - 2.0 * num + amount;
			double num6 = num2 - num;
			Vector2D result = default(Vector2D);
			result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
			result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
			return result;
		}

		/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position vector.</param><param name="tangent1">Source tangent vector.</param><param name="value2">Source position vector.</param><param name="tangent2">Source tangent vector.</param><param name="amount">Weighting factor.</param><param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
		public static void Hermite(ref Vector2D value1, ref Vector2D tangent1, ref Vector2D value2, ref Vector2D tangent2, double amount, out Vector2D result)
		{
			double num = amount * amount;
			double num2 = amount * num;
			double num3 = 2.0 * num2 - 3.0 * num + 1.0;
			double num4 = -2.0 * num2 + 3.0 * num;
			double num5 = num2 - 2.0 * num + amount;
			double num6 = num2 - num;
			result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
			result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
		}

		/// <summary>
		/// Transforms the vector (x, y, 0, 1) by the specified matrix.
		/// </summary>
		/// <param name="position">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector2D Transform(Vector2D position, Matrix matrix)
		{
			double x = position.X * (double)matrix.M11 + position.Y * (double)matrix.M21 + (double)matrix.M41;
			double y = position.X * (double)matrix.M12 + position.Y * (double)matrix.M22 + (double)matrix.M42;
			Vector2D result = default(Vector2D);
			result.X = x;
			result.Y = y;
			return result;
		}

		/// <summary>
		/// Transforms a Vector2D by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector2D.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector2D resulting from the transformation.</param>
		public static void Transform(ref Vector2D position, ref Matrix matrix, out Vector2D result)
		{
			double x = position.X * (double)matrix.M11 + position.Y * (double)matrix.M21 + (double)matrix.M41;
			double y = position.X * (double)matrix.M12 + position.Y * (double)matrix.M22 + (double)matrix.M42;
			result.X = x;
			result.Y = y;
		}

		/// <summary>
		/// Transforms a 2D vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation matrix.</param>
		public static Vector2D TransformNormal(Vector2D normal, Matrix matrix)
		{
			double x = normal.X * (double)matrix.M11 + normal.Y * (double)matrix.M21;
			double y = normal.X * (double)matrix.M12 + normal.Y * (double)matrix.M22;
			Vector2D result = default(Vector2D);
			result.X = x;
			result.Y = y;
			return result;
		}

		/// <summary>
		/// Transforms a vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation matrix.</param><param name="result">[OutAttribute] The Vector2D resulting from the transformation.</param>
		public static void TransformNormal(ref Vector2D normal, ref Matrix matrix, out Vector2D result)
		{
			double x = normal.X * (double)matrix.M11 + normal.Y * (double)matrix.M21;
			double y = normal.X * (double)matrix.M12 + normal.Y * (double)matrix.M22;
			result.X = x;
			result.Y = y;
		}

		/// <summary>
		/// Transforms a single Vector2D, or the vector normal (x, y, 0, 0), by a specified Quaternion rotation.
		/// </summary>
		/// <param name="value">The vector to rotate.</param><param name="rotation">The Quaternion rotation to apply.</param>
		public static Vector2D Transform(Vector2D value, Quaternion rotation)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num3;
			double num5 = (double)rotation.X * num;
			double num6 = (double)rotation.X * num2;
			double num7 = (double)rotation.Y * num2;
			double num8 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num7 - num8) + value.Y * (num6 - num4);
			double y = value.X * (num6 + num4) + value.Y * (1.0 - num5 - num8);
			Vector2D result = default(Vector2D);
			result.X = x;
			result.Y = y;
			return result;
		}

		/// <summary>
		/// Transforms a Vector2D, or the vector normal (x, y, 0, 0), by a specified Quaternion rotation.
		/// </summary>
		/// <param name="value">The vector to rotate.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="result">[OutAttribute] An existing Vector2D filled in with the result of the rotation.</param>
		public static void Transform(ref Vector2D value, ref Quaternion rotation, out Vector2D result)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num3;
			double num5 = (double)rotation.X * num;
			double num6 = (double)rotation.X * num2;
			double num7 = (double)rotation.Y * num2;
			double num8 = (double)rotation.Z * num3;
			double x = value.X * (1.0 - num7 - num8) + value.Y * (num6 - num4);
			double y = value.X * (num6 + num4) + value.Y * (1.0 - num5 - num8);
			result.X = x;
			result.Y = y;
		}

		/// <summary>
		/// Transforms an array of Vector2s by a specified Matrix.
		/// </summary>
		/// <param name="sourceArray">The array of Vector2s to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
		public static void Transform(Vector2D[] sourceArray, ref Matrix matrix, Vector2D[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				double x = sourceArray[i].X;
				double y = sourceArray[i].Y;
				destinationArray[i].X = x * (double)matrix.M11 + y * (double)matrix.M21 + (double)matrix.M41;
				destinationArray[i].Y = x * (double)matrix.M12 + y * (double)matrix.M22 + (double)matrix.M42;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector2s by a specified Matrix and places the results in a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="sourceIndex">The index of the first Vector2D to transform in the source array.</param><param name="matrix">The Matrix to transform by.</param><param name="destinationArray">The destination array into which the resulting Vector2s will be written.</param><param name="destinationIndex">The index of the position in the destination array where the first result Vector2D should be written.</param><param name="length">The number of Vector2s to be transformed.</param>
		public static void Transform(Vector2D[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2D[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				double x = sourceArray[sourceIndex].X;
				double y = sourceArray[sourceIndex].Y;
				destinationArray[destinationIndex].X = x * (double)matrix.M11 + y * (double)matrix.M21 + (double)matrix.M41;
				destinationArray[destinationIndex].Y = x * (double)matrix.M12 + y * (double)matrix.M22 + (double)matrix.M42;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms an array of Vector2D vector normals by a specified Matrix.
		/// </summary>
		/// <param name="sourceArray">The array of vector normals to transform.</param><param name="matrix">The transform Matrix to apply.</param><param name="destinationArray">An existing array into which the transformed vector normals are written.</param>
		public static void TransformNormal(Vector2D[] sourceArray, ref Matrix matrix, Vector2D[] destinationArray)
		{
			for (int i = 0; i < sourceArray.Length; i++)
			{
				double x = sourceArray[i].X;
				double y = sourceArray[i].Y;
				destinationArray[i].X = x * (double)matrix.M11 + y * (double)matrix.M21;
				destinationArray[i].Y = x * (double)matrix.M12 + y * (double)matrix.M22;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector2D vector normals by a specified Matrix and places the results in a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="sourceIndex">The index of the first Vector2D to transform in the source array.</param><param name="matrix">The Matrix to apply.</param><param name="destinationArray">The destination array into which the resulting Vector2s are written.</param><param name="destinationIndex">The index of the position in the destination array where the first result Vector2D should be written.</param><param name="length">The number of vector normals to be transformed.</param>
		public static void TransformNormal(Vector2D[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2D[] destinationArray, int destinationIndex, int length)
		{
			while (length > 0)
			{
				double x = sourceArray[sourceIndex].X;
				double y = sourceArray[sourceIndex].Y;
				destinationArray[destinationIndex].X = x * (double)matrix.M11 + y * (double)matrix.M21;
				destinationArray[destinationIndex].Y = x * (double)matrix.M12 + y * (double)matrix.M22;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Transforms an array of Vector2s by a specified Quaternion.
		/// </summary>
		/// <param name="sourceArray">The array of Vector2s to transform.</param><param name="rotation">The transform Matrix to use.</param><param name="destinationArray">An existing array into which the transformed Vector2s are written.</param>
		public static void Transform(Vector2D[] sourceArray, ref Quaternion rotation, Vector2D[] destinationArray)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num3;
			double num5 = (double)rotation.X * num;
			double num6 = (double)rotation.X * num2;
			double num7 = (double)rotation.Y * num2;
			double num8 = (double)rotation.Z * num3;
			double num9 = 1.0 - num7 - num8;
			double num10 = num6 - num4;
			double num11 = num6 + num4;
			double num12 = 1.0 - num5 - num8;
			for (int i = 0; i < sourceArray.Length; i++)
			{
				double x = sourceArray[i].X;
				double y = sourceArray[i].Y;
				destinationArray[i].X = x * num9 + y * num10;
				destinationArray[i].Y = x * num11 + y * num12;
			}
		}

		/// <summary>
		/// Transforms a specified range in an array of Vector2s by a specified Quaternion and places the results in a specified range in a destination array.
		/// </summary>
		/// <param name="sourceArray">The source array.</param><param name="sourceIndex">The index of the first Vector2D to transform in the source array.</param><param name="rotation">The Quaternion rotation to apply.</param><param name="destinationArray">The destination array into which the resulting Vector2s are written.</param><param name="destinationIndex">The index of the position in the destination array where the first result Vector2D should be written.</param><param name="length">The number of Vector2s to be transformed.</param>
		public static void Transform(Vector2D[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2D[] destinationArray, int destinationIndex, int length)
		{
			double num = rotation.X + rotation.X;
			double num2 = rotation.Y + rotation.Y;
			double num3 = rotation.Z + rotation.Z;
			double num4 = (double)rotation.W * num3;
			double num5 = (double)rotation.X * num;
			double num6 = (double)rotation.X * num2;
			double num7 = (double)rotation.Y * num2;
			double num8 = (double)rotation.Z * num3;
			double num9 = 1.0 - num7 - num8;
			double num10 = num6 - num4;
			double num11 = num6 + num4;
			double num12 = 1.0 - num5 - num8;
			while (length > 0)
			{
				double x = sourceArray[sourceIndex].X;
				double y = sourceArray[sourceIndex].Y;
				destinationArray[destinationIndex].X = x * num9 + y * num10;
				destinationArray[destinationIndex].Y = x * num11 + y * num12;
				sourceIndex++;
				destinationIndex++;
				length--;
			}
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param>
		public static Vector2D Negate(Vector2D value)
		{
			Vector2D result = default(Vector2D);
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			return result;
		}

		/// <summary>
		/// Returns a vector pointing in the opposite direction.
		/// </summary>
		/// <param name="value">Source vector.</param><param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
		public static void Negate(ref Vector2D value, out Vector2D result)
		{
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D Add(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] Sum of the source vectors.</param>
		public static void Add(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D Subtract(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
			return result;
		}

		/// <summary>
		/// Subtracts a vector from a vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the subtraction.</param>
		public static void Subtract(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param>
		public static Vector2D Multiply(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
			return result;
		}

		/// <summary>
		/// Multiplies the components of two vectors by each other.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Source vector.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param>
		public static Vector2D Multiply(Vector2D value1, double scaleFactor)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Vector2D value1, double scaleFactor, out Vector2D result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">Divisor vector.</param>
		public static Vector2D Divide(Vector2D value1, Vector2D value2)
		{
			Vector2D result = default(Vector2D);
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
			return result;
		}

		/// <summary>
		/// Divides the components of a vector by the components of another vector.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="value2">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param>
		public static Vector2D Divide(Vector2D value1, double divider)
		{
			double num = 1.0 / divider;
			Vector2D result = default(Vector2D);
			result.X = value1.X * num;
			result.Y = value1.Y * num;
			return result;
		}

		/// <summary>
		/// Divides a vector by a scalar value.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="divider">The divisor.</param><param name="result">[OutAttribute] The result of the division.</param>
		public static void Divide(ref Vector2D value1, double divider, out Vector2D result)
		{
			double num = 1.0 / divider;
			result.X = value1.X * num;
			result.Y = value1.Y * num;
		}

		public bool Between(ref Vector2D start, ref Vector2D end)
		{
			if (!(X >= start.X) || !(X <= end.X))
			{
				if (Y >= start.Y)
				{
					return Y <= end.Y;
				}
				return false;
			}
			return true;
		}

		public static Vector2D Floor(Vector2D position)
		{
			return new Vector2D(Math.Floor(position.X), Math.Floor(position.Y));
		}

		public void Rotate(double angle)
		{
			double x = X;
			X = X * Math.Cos(angle) - Y * Math.Sin(angle);
			Y = Y * Math.Cos(angle) + x * Math.Sin(angle);
		}
	}
}
