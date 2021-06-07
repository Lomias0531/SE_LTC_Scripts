using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a four-dimensional vector (x,y,z,w), which is used to efficiently rotate an object about the (x, y, z) vector by the angle theta, where w = cos(theta/2).
	/// Uses double precision floating point numbers for calculation and storage
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct QuaternionD
	{
		protected class VRageMath_QuaternionD_003C_003EX_003C_003EAccessor : IMemberAccessor<QuaternionD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref QuaternionD owner, in double value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref QuaternionD owner, out double value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_QuaternionD_003C_003EY_003C_003EAccessor : IMemberAccessor<QuaternionD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref QuaternionD owner, in double value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref QuaternionD owner, out double value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_QuaternionD_003C_003EZ_003C_003EAccessor : IMemberAccessor<QuaternionD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref QuaternionD owner, in double value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref QuaternionD owner, out double value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_QuaternionD_003C_003EW_003C_003EAccessor : IMemberAccessor<QuaternionD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref QuaternionD owner, in double value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref QuaternionD owner, out double value)
			{
				value = owner.W;
			}
		}

		public static QuaternionD Identity;

		/// <summary>
		/// Specifies the x-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(1)]
		public double X;

		/// <summary>
		/// Specifies the y-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(4)]
		public double Y;

		/// <summary>
		/// Specifies the z-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(7)]
		public double Z;

		/// <summary>
		/// Specifies the rotation component of the quaternion.
		/// </summary>
		[ProtoMember(10)]
		public double W;

		static QuaternionD()
		{
			Identity = new QuaternionD(0.0, 0.0, 0.0, 1.0);
		}

		/// <summary>
		/// Initializes a new instance of QuaternionD.
		/// </summary>
		/// <param name="x">The x-value of the quaternion.</param><param name="y">The y-value of the quaternion.</param><param name="z">The z-value of the quaternion.</param><param name="w">The w-value of the quaternion.</param>
		public QuaternionD(double x, double y, double z, double w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of QuaternionD.
		/// </summary>
		/// <param name="vectorPart">The vector component of the quaternion.</param><param name="scalarPart">The rotation component of the quaternion.</param>
		public QuaternionD(Vector3D vectorPart, double scalarPart)
		{
			X = vectorPart.X;
			Y = vectorPart.Y;
			Z = vectorPart.Z;
			W = scalarPart;
		}

		/// <summary>
		/// Flips the sign of each component of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param>
		public static QuaternionD operator -(QuaternionD quaternion)
		{
			QuaternionD result = default(QuaternionD);
			result.X = 0.0 - quaternion.X;
			result.Y = 0.0 - quaternion.Y;
			result.Z = 0.0 - quaternion.Z;
			result.W = 0.0 - quaternion.W;
			return result;
		}

		/// <summary>
		/// Compares two Quaternions for equality.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">Source QuaternionD.</param>
		public static bool operator ==(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			if (quaternion1.X == quaternion2.X && quaternion1.Y == quaternion2.Y && quaternion1.Z == quaternion2.Z)
			{
				return quaternion1.W == quaternion2.W;
			}
			return false;
		}

		/// <summary>
		/// Compare two Quaternions for inequality.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">Source QuaternionD.</param>
		public static bool operator !=(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			if (quaternion1.X == quaternion2.X && quaternion1.Y == quaternion2.Y && quaternion1.Z == quaternion2.Z)
			{
				return quaternion1.W != quaternion2.W;
			}
			return true;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">QuaternionD to add.</param><param name="quaternion2">QuaternionD to add.</param>
		public static QuaternionD operator +(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
			return result;
		}

		/// <summary>
		/// Subtracts a quaternion from another quaternion.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param>
		public static QuaternionD operator -(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X - quaternion2.X;
			result.Y = quaternion1.Y - quaternion2.Y;
			result.Z = quaternion1.Z - quaternion2.Z;
			result.W = quaternion1.W - quaternion2.W;
			return result;
		}

		/// <summary>
		/// Multiplies two quaternions.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param>
		public static QuaternionD operator *(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double x2 = quaternion2.X;
			double y2 = quaternion2.Y;
			double z2 = quaternion2.Z;
			double w2 = quaternion2.W;
			double num = y * z2 - z * y2;
			double num2 = z * x2 - x * z2;
			double num3 = x * y2 - y * x2;
			double num4 = x * x2 + y * y2 + z * z2;
			QuaternionD result = default(QuaternionD);
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Multiplies a quaternion by a scalar value.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="scaleFactor">Scalar value.</param>
		public static QuaternionD operator *(QuaternionD quaternion1, double scaleFactor)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides a QuaternionD by another QuaternionD.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">The divisor.</param>
		public static QuaternionD operator /(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double num = 1.0 / (quaternion2.X * quaternion2.X + quaternion2.Y * quaternion2.Y + quaternion2.Z * quaternion2.Z + quaternion2.W * quaternion2.W);
			double num2 = (0.0 - quaternion2.X) * num;
			double num3 = (0.0 - quaternion2.Y) * num;
			double num4 = (0.0 - quaternion2.Z) * num;
			double num5 = quaternion2.W * num;
			double num6 = y * num4 - z * num3;
			double num7 = z * num2 - x * num4;
			double num8 = x * num3 - y * num2;
			double num9 = x * num2 + y * num3 + z * num4;
			QuaternionD result = default(QuaternionD);
			result.X = x * num5 + num2 * w + num6;
			result.Y = y * num5 + num3 * w + num7;
			result.Z = z * num5 + num4 * w + num8;
			result.W = w * num5 - num9;
			return result;
		}

		/// <summary>
		/// Retireves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(currentCulture), Y.ToString(currentCulture), Z.ToString(currentCulture), W.ToString(currentCulture));
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the QuaternionD.
		/// </summary>
		/// <param name="other">The QuaternionD to compare with the current QuaternionD.</param>
		public bool Equals(QuaternionD other)
		{
			if (X == other.X && Y == other.Y && Z == other.Z)
			{
				return W == other.W;
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
			if (obj is QuaternionD)
			{
				result = Equals((QuaternionD)obj);
			}
			return result;
		}

		/// <summary>
		/// Get the hash code of this object.
		/// </summary>
		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
		}

		/// <summary>
		/// Calculates the length squared of a QuaternionD.
		/// </summary>
		public double LengthSquared()
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		/// <summary>
		/// Calculates the length of a QuaternionD.
		/// </summary>
		public double Length()
		{
			return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}

		/// <summary>
		/// Divides each component of the quaternion by the length of the quaternion.
		/// </summary>
		public void Normalize()
		{
			double num = 1.0 / Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
			X *= num;
			Y *= num;
			Z *= num;
			W *= num;
		}

		public void GetAxisAngle(out Vector3D axis, out double angle)
		{
			axis.X = X;
			axis.Y = Y;
			axis.Z = Z;
			double num = axis.Length();
			double w = W;
			if (num != 0.0)
			{
				axis.X /= num;
				axis.Y /= num;
				axis.Z /= num;
			}
			angle = Math.Atan2(num, w) * 2.0;
		}

		/// <summary>
		/// Divides each component of the quaternion by the length of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param>
		public static QuaternionD Normalize(QuaternionD quaternion)
		{
			double num = 1.0 / Math.Sqrt(quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);
			QuaternionD result = default(QuaternionD);
			result.X = quaternion.X * num;
			result.Y = quaternion.Y * num;
			result.Z = quaternion.Z * num;
			result.W = quaternion.W * num;
			return result;
		}

		/// <summary>
		/// Divides each component of the quaternion by the length of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param><param name="result">[OutAttribute] Normalized quaternion.</param>
		public static void Normalize(ref QuaternionD quaternion, out QuaternionD result)
		{
			double num = 1.0 / Math.Sqrt(quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);
			result.X = quaternion.X * num;
			result.Y = quaternion.Y * num;
			result.Z = quaternion.Z * num;
			result.W = quaternion.W * num;
		}

		/// <summary>
		/// Transforms this QuaternionD into its conjugate.
		/// </summary>
		public void Conjugate()
		{
			X = 0.0 - X;
			Y = 0.0 - Y;
			Z = 0.0 - Z;
		}

		/// <summary>
		/// Returns the conjugate of a specified QuaternionD.
		/// </summary>
		/// <param name="value">The QuaternionD of which to return the conjugate.</param>
		public static QuaternionD Conjugate(QuaternionD value)
		{
			QuaternionD result = default(QuaternionD);
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			result.Z = 0.0 - value.Z;
			result.W = value.W;
			return result;
		}

		/// <summary>
		/// Returns the conjugate of a specified QuaternionD.
		/// </summary>
		/// <param name="value">The QuaternionD of which to return the conjugate.</param><param name="result">[OutAttribute] An existing QuaternionD filled in to be the conjugate of the specified one.</param>
		public static void Conjugate(ref QuaternionD value, out QuaternionD result)
		{
			result.X = 0.0 - value.X;
			result.Y = 0.0 - value.Y;
			result.Z = 0.0 - value.Z;
			result.W = value.W;
		}

		/// <summary>
		/// Returns the inverse of a QuaternionD.
		/// </summary>
		/// <param name="quaternion">Source QuaternionD.</param>
		public static QuaternionD Inverse(QuaternionD quaternion)
		{
			double num = 1.0 / (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);
			QuaternionD result = default(QuaternionD);
			result.X = (0.0 - quaternion.X) * num;
			result.Y = (0.0 - quaternion.Y) * num;
			result.Z = (0.0 - quaternion.Z) * num;
			result.W = quaternion.W * num;
			return result;
		}

		/// <summary>
		/// Returns the inverse of a QuaternionD.
		/// </summary>
		/// <param name="quaternion">Source QuaternionD.</param><param name="result">[OutAttribute] The inverse of the QuaternionD.</param>
		public static void Inverse(ref QuaternionD quaternion, out QuaternionD result)
		{
			double num = 1.0 / (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z + quaternion.W * quaternion.W);
			result.X = (0.0 - quaternion.X) * num;
			result.Y = (0.0 - quaternion.Y) * num;
			result.Z = (0.0 - quaternion.Z) * num;
			result.W = quaternion.W * num;
		}

		/// <summary>
		/// Creates a QuaternionD from a vector and an angle to rotate about the vector.
		/// </summary>
		/// <param name="axis">The vector to rotate around.</param><param name="angle">The angle to rotate around the vector.</param>
		public static QuaternionD CreateFromAxisAngle(Vector3D axis, double angle)
		{
			double num = angle * 0.5;
			double num2 = Math.Sin(num);
			double w = Math.Cos(num);
			QuaternionD result = default(QuaternionD);
			result.X = axis.X * num2;
			result.Y = axis.Y * num2;
			result.Z = axis.Z * num2;
			result.W = w;
			return result;
		}

		/// <summary>
		/// Creates a QuaternionD from a vector and an angle to rotate about the vector.
		/// </summary>
		/// <param name="axis">The vector to rotate around.</param><param name="angle">The angle to rotate around the vector.</param><param name="result">[OutAttribute] The created QuaternionD.</param>
		public static void CreateFromAxisAngle(ref Vector3D axis, double angle, out QuaternionD result)
		{
			double num = angle * 0.5;
			double num2 = Math.Sin(num);
			double w = Math.Cos(num);
			result.X = axis.X * num2;
			result.Y = axis.Y * num2;
			result.Z = axis.Z * num2;
			result.W = w;
		}

		/// <summary>
		/// Creates a new QuaternionD from specified yaw, pitch, and roll angles.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the y-axis.</param><param name="pitch">The pitch angle, in radians, around the x-axis.</param><param name="roll">The roll angle, in radians, around the z-axis.</param>
		public static QuaternionD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
		{
			double num = roll * 0.5;
			double num2 = Math.Sin(num);
			double num3 = Math.Cos(num);
			double num4 = pitch * 0.5;
			double num5 = Math.Sin(num4);
			double num6 = Math.Cos(num4);
			double num7 = yaw * 0.5;
			double num8 = Math.Sin(num7);
			double num9 = Math.Cos(num7);
			QuaternionD result = default(QuaternionD);
			result.X = num9 * num5 * num3 + num8 * num6 * num2;
			result.Y = num8 * num6 * num3 - num9 * num5 * num2;
			result.Z = num9 * num6 * num2 - num8 * num5 * num3;
			result.W = num9 * num6 * num3 + num8 * num5 * num2;
			return result;
		}

		/// <summary>
		/// Creates a new QuaternionD from specified yaw, pitch, and roll angles.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the y-axis.</param><param name="pitch">The pitch angle, in radians, around the x-axis.</param><param name="roll">The roll angle, in radians, around the z-axis.</param><param name="result">[OutAttribute] An existing QuaternionD filled in to express the specified yaw, pitch, and roll angles.</param>
		public static void CreateFromYawPitchRoll(double yaw, double pitch, double roll, out QuaternionD result)
		{
			double num = roll * 0.5;
			double num2 = Math.Sin(num);
			double num3 = Math.Cos(num);
			double num4 = pitch * 0.5;
			double num5 = Math.Sin(num4);
			double num6 = Math.Cos(num4);
			double num7 = yaw * 0.5;
			double num8 = Math.Sin(num7);
			double num9 = Math.Cos(num7);
			result.X = num9 * num5 * num3 + num8 * num6 * num2;
			result.Y = num8 * num6 * num3 - num9 * num5 * num2;
			result.Z = num9 * num6 * num2 - num8 * num5 * num3;
			result.W = num9 * num6 * num3 + num8 * num5 * num2;
		}

		/// <summary>
		/// Works for normalized vectors only
		/// </summary>
		public static QuaternionD CreateFromForwardUp(Vector3D forward, Vector3D up)
		{
			Vector3D vector3D = -forward;
			Vector3D vector = Vector3D.Cross(up, vector3D);
			Vector3D vector3D2 = Vector3D.Cross(vector3D, vector);
			double x = vector.X;
			double y = vector.Y;
			double z = vector.Z;
			double x2 = vector3D2.X;
			double y2 = vector3D2.Y;
			double z2 = vector3D2.Z;
			double x3 = vector3D.X;
			double y3 = vector3D.Y;
			double z3 = vector3D.Z;
			double num = x + y2 + z3;
			QuaternionD result = default(QuaternionD);
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num + 1.0);
				result.W = num2 * 0.5;
				num2 = 0.5 / num2;
				result.X = (z2 - y3) * num2;
				result.Y = (x3 - z) * num2;
				result.Z = (y - x2) * num2;
				return result;
			}
			if (x >= y2 && x >= z3)
			{
				double num3 = Math.Sqrt(1.0 + x - y2 - z3);
				double num4 = 0.5 / num3;
				result.X = 0.5 * num3;
				result.Y = (y + x2) * num4;
				result.Z = (z + x3) * num4;
				result.W = (z2 - y3) * num4;
				return result;
			}
			if (y2 > z3)
			{
				double num5 = Math.Sqrt(1.0 + y2 - x - z3);
				double num6 = 0.5 / num5;
				result.X = (x2 + y) * num6;
				result.Y = 0.5 * num5;
				result.Z = (y3 + z2) * num6;
				result.W = (x3 - z) * num6;
				return result;
			}
			double num7 = Math.Sqrt(1.0 + z3 - x - y2);
			double num8 = 0.5 / num7;
			result.X = (x3 + z) * num8;
			result.Y = (y3 + z2) * num8;
			result.Z = 0.5 * num7;
			result.W = (y - x2) * num8;
			return result;
		}

		/// <summary>
		/// Creates a QuaternionD from a rotation MatrixD.
		/// </summary>
		/// <param name="matrix">The rotation MatrixD to create the QuaternionD from.</param>
		public static QuaternionD CreateFromRotationMatrix(MatrixD matrix)
		{
			double num = matrix.M11 + matrix.M22 + matrix.M33;
			QuaternionD result = default(QuaternionD);
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num + 1.0);
				result.W = num2 * 0.5;
				double num3 = 0.5 / num2;
				result.X = (matrix.M23 - matrix.M32) * num3;
				result.Y = (matrix.M31 - matrix.M13) * num3;
				result.Z = (matrix.M12 - matrix.M21) * num3;
			}
			else if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
			{
				double num4 = Math.Sqrt(1.0 + matrix.M11 - matrix.M22 - matrix.M33);
				double num5 = 0.5 / num4;
				result.X = 0.5 * num4;
				result.Y = (matrix.M12 + matrix.M21) * num5;
				result.Z = (matrix.M13 + matrix.M31) * num5;
				result.W = (matrix.M23 - matrix.M32) * num5;
			}
			else if (matrix.M22 > matrix.M33)
			{
				double num6 = Math.Sqrt(1.0 + matrix.M22 - matrix.M11 - matrix.M33);
				double num7 = 0.5 / num6;
				result.X = (matrix.M21 + matrix.M12) * num7;
				result.Y = 0.5 * num6;
				result.Z = (matrix.M32 + matrix.M23) * num7;
				result.W = (matrix.M31 - matrix.M13) * num7;
			}
			else
			{
				double num8 = Math.Sqrt(1.0 + matrix.M33 - matrix.M11 - matrix.M22);
				double num9 = 0.5 / num8;
				result.X = (matrix.M31 + matrix.M13) * num9;
				result.Y = (matrix.M32 + matrix.M23) * num9;
				result.Z = 0.5 * num8;
				result.W = (matrix.M12 - matrix.M21) * num9;
			}
			return result;
		}

		/// <summary>
		/// Creates a QuaternionD from a rotation MatrixD.
		/// </summary>
		/// <param name="matrix">The rotation MatrixD to create the QuaternionD from.</param><param name="result">[OutAttribute] The created QuaternionD.</param>
		public static void CreateFromRotationMatrix(ref MatrixD matrix, out QuaternionD result)
		{
			double num = matrix.M11 + matrix.M22 + matrix.M33;
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num + 1.0);
				result.W = num2 * 0.5;
				double num3 = 0.5 / num2;
				result.X = (matrix.M23 - matrix.M32) * num3;
				result.Y = (matrix.M31 - matrix.M13) * num3;
				result.Z = (matrix.M12 - matrix.M21) * num3;
			}
			else if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
			{
				double num4 = Math.Sqrt(1.0 + matrix.M11 - matrix.M22 - matrix.M33);
				double num5 = 0.5 / num4;
				result.X = 0.5 * num4;
				result.Y = (matrix.M12 + matrix.M21) * num5;
				result.Z = (matrix.M13 + matrix.M31) * num5;
				result.W = (matrix.M23 - matrix.M32) * num5;
			}
			else if (matrix.M22 > matrix.M33)
			{
				double num6 = Math.Sqrt(1.0 + matrix.M22 - matrix.M11 - matrix.M33);
				double num7 = 0.5 / num6;
				result.X = (matrix.M21 + matrix.M12) * num7;
				result.Y = 0.5 * num6;
				result.Z = (matrix.M32 + matrix.M23) * num7;
				result.W = (matrix.M31 - matrix.M13) * num7;
			}
			else
			{
				double num8 = Math.Sqrt(1.0 + matrix.M33 - matrix.M11 - matrix.M22);
				double num9 = 0.5 / num8;
				result.X = (matrix.M31 + matrix.M13) * num9;
				result.Y = (matrix.M32 + matrix.M23) * num9;
				result.Z = 0.5 * num8;
				result.W = (matrix.M12 - matrix.M21) * num9;
			}
		}

		/// <summary>
		/// Calculates the dot product of two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">Source QuaternionD.</param>
		public static double Dot(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			return quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
		}

		/// <summary>
		/// Calculates the dot product of two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">Source QuaternionD.</param><param name="result">[OutAttribute] Dot product of the Quaternions.</param>
		public static void Dot(ref QuaternionD quaternion1, ref QuaternionD quaternion2, out double result)
		{
			result = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value that indicates how far to interpolate between the quaternions.</param>
		public static QuaternionD Slerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
		{
			double num = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			bool flag = false;
			if (num < 0.0)
			{
				flag = true;
				num = 0.0 - num;
			}
			double num2;
			double num3;
			if (num > 0.999998986721039)
			{
				num2 = 1.0 - amount;
				num3 = (flag ? (0.0 - amount) : amount);
			}
			else
			{
				double num4 = Math.Acos(num);
				double num5 = 1.0 / Math.Sin(num4);
				num2 = Math.Sin((1.0 - amount) * num4) * num5;
				num3 = (flag ? ((0.0 - Math.Sin(amount * num4)) * num5) : (Math.Sin(amount * num4) * num5));
			}
			QuaternionD result = default(QuaternionD);
			result.X = num2 * quaternion1.X + num3 * quaternion2.X;
			result.Y = num2 * quaternion1.Y + num3 * quaternion2.Y;
			result.Z = num2 * quaternion1.Z + num3 * quaternion2.Z;
			result.W = num2 * quaternion1.W + num3 * quaternion2.W;
			return result;
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value that indicates how far to interpolate between the quaternions.</param><param name="result">[OutAttribute] Result of the interpolation.</param>
		public static void Slerp(ref QuaternionD quaternion1, ref QuaternionD quaternion2, double amount, out QuaternionD result)
		{
			double num = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;
			bool flag = false;
			if (num < 0.0)
			{
				flag = true;
				num = 0.0 - num;
			}
			double num2;
			double num3;
			if (num > 0.999998986721039)
			{
				num2 = 1.0 - amount;
				num3 = (flag ? (0.0 - amount) : amount);
			}
			else
			{
				double num4 = Math.Acos(num);
				double num5 = 1.0 / Math.Sin(num4);
				num2 = Math.Sin((1.0 - amount) * num4) * num5;
				num3 = (flag ? ((0.0 - Math.Sin(amount * num4)) * num5) : (Math.Sin(amount * num4) * num5));
			}
			result.X = num2 * quaternion1.X + num3 * quaternion2.X;
			result.Y = num2 * quaternion1.Y + num3 * quaternion2.Y;
			result.Z = num2 * quaternion1.Z + num3 * quaternion2.Z;
			result.W = num2 * quaternion1.W + num3 * quaternion2.W;
		}

		/// <summary>
		/// Linearly interpolates between two quaternions.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value indicating how far to interpolate between the quaternions.</param>
		public static QuaternionD Lerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount)
		{
			double num = 1.0 - amount;
			QuaternionD result = default(QuaternionD);
			if (quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W >= 0.0)
			{
				result.X = num * quaternion1.X + amount * quaternion2.X;
				result.Y = num * quaternion1.Y + amount * quaternion2.Y;
				result.Z = num * quaternion1.Z + amount * quaternion2.Z;
				result.W = num * quaternion1.W + amount * quaternion2.W;
			}
			else
			{
				result.X = num * quaternion1.X - amount * quaternion2.X;
				result.Y = num * quaternion1.Y - amount * quaternion2.Y;
				result.Z = num * quaternion1.Z - amount * quaternion2.Z;
				result.W = num * quaternion1.W - amount * quaternion2.W;
			}
			double num2 = 1.0 / Math.Sqrt(result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W);
			result.X *= num2;
			result.Y *= num2;
			result.Z *= num2;
			result.W *= num2;
			return result;
		}

		/// <summary>
		/// Linearly interpolates between two quaternions.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value indicating how far to interpolate between the quaternions.</param><param name="result">[OutAttribute] The resulting quaternion.</param>
		public static void Lerp(ref QuaternionD quaternion1, ref QuaternionD quaternion2, double amount, out QuaternionD result)
		{
			double num = 1.0 - amount;
			if (quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y + quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W >= 0.0)
			{
				result.X = num * quaternion1.X + amount * quaternion2.X;
				result.Y = num * quaternion1.Y + amount * quaternion2.Y;
				result.Z = num * quaternion1.Z + amount * quaternion2.Z;
				result.W = num * quaternion1.W + amount * quaternion2.W;
			}
			else
			{
				result.X = num * quaternion1.X - amount * quaternion2.X;
				result.Y = num * quaternion1.Y - amount * quaternion2.Y;
				result.Z = num * quaternion1.Z - amount * quaternion2.Z;
				result.W = num * quaternion1.W - amount * quaternion2.W;
			}
			double num2 = 1.0 / Math.Sqrt(result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W);
			result.X *= num2;
			result.Y *= num2;
			result.Z *= num2;
			result.W *= num2;
		}

		/// <summary>
		/// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
		/// </summary>
		/// <param name="value1">The first QuaternionD rotation in the series.</param><param name="value2">The second QuaternionD rotation in the series.</param>
		public static QuaternionD Concatenate(QuaternionD value1, QuaternionD value2)
		{
			double x = value2.X;
			double y = value2.Y;
			double z = value2.Z;
			double w = value2.W;
			double x2 = value1.X;
			double y2 = value1.Y;
			double z2 = value1.Z;
			double w2 = value1.W;
			double num = y * z2 - z * y2;
			double num2 = z * x2 - x * z2;
			double num3 = x * y2 - y * x2;
			double num4 = x * x2 + y * y2 + z * z2;
			QuaternionD result = default(QuaternionD);
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
		/// </summary>
		/// <param name="value1">The first QuaternionD rotation in the series.</param><param name="value2">The second QuaternionD rotation in the series.</param><param name="result">[OutAttribute] The QuaternionD rotation representing the concatenation of value1 followed by value2.</param>
		public static void Concatenate(ref QuaternionD value1, ref QuaternionD value2, out QuaternionD result)
		{
			double x = value2.X;
			double y = value2.Y;
			double z = value2.Z;
			double w = value2.W;
			double x2 = value1.X;
			double y2 = value1.Y;
			double z2 = value1.Z;
			double w2 = value1.W;
			double num = y * z2 - z * y2;
			double num2 = z * x2 - x * z2;
			double num3 = x * y2 - y * x2;
			double num4 = x * x2 + y * y2 + z * z2;
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
		}

		/// <summary>
		/// Flips the sign of each component of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param>
		public static QuaternionD Negate(QuaternionD quaternion)
		{
			QuaternionD result = default(QuaternionD);
			result.X = 0.0 - quaternion.X;
			result.Y = 0.0 - quaternion.Y;
			result.Z = 0.0 - quaternion.Z;
			result.W = 0.0 - quaternion.W;
			return result;
		}

		/// <summary>
		/// Flips the sign of each component of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param><param name="result">[OutAttribute] Negated quaternion.</param>
		public static void Negate(ref QuaternionD quaternion, out QuaternionD result)
		{
			result.X = 0.0 - quaternion.X;
			result.Y = 0.0 - quaternion.Y;
			result.Z = 0.0 - quaternion.Z;
			result.W = 0.0 - quaternion.W;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">QuaternionD to add.</param><param name="quaternion2">QuaternionD to add.</param>
		public static QuaternionD Add(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
			return result;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">QuaternionD to add.</param><param name="quaternion2">QuaternionD to add.</param><param name="result">[OutAttribute] Result of adding the Quaternions.</param>
		public static void Add(ref QuaternionD quaternion1, ref QuaternionD quaternion2, out QuaternionD result)
		{
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
		}

		/// <summary>
		/// Subtracts a quaternion from another quaternion.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param>
		public static QuaternionD Subtract(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X - quaternion2.X;
			result.Y = quaternion1.Y - quaternion2.Y;
			result.Z = quaternion1.Z - quaternion2.Z;
			result.W = quaternion1.W - quaternion2.W;
			return result;
		}

		/// <summary>
		/// Subtracts a quaternion from another quaternion.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="result">[OutAttribute] Result of the subtraction.</param>
		public static void Subtract(ref QuaternionD quaternion1, ref QuaternionD quaternion2, out QuaternionD result)
		{
			result.X = quaternion1.X - quaternion2.X;
			result.Y = quaternion1.Y - quaternion2.Y;
			result.Z = quaternion1.Z - quaternion2.Z;
			result.W = quaternion1.W - quaternion2.W;
		}

		/// <summary>
		/// Multiplies two quaternions.
		/// </summary>
		/// <param name="quaternion1">The quaternion on the left of the multiplication.</param><param name="quaternion2">The quaternion on the right of the multiplication.</param>
		public static QuaternionD Multiply(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double x2 = quaternion2.X;
			double y2 = quaternion2.Y;
			double z2 = quaternion2.Z;
			double w2 = quaternion2.W;
			double num = y * z2 - z * y2;
			double num2 = z * x2 - x * z2;
			double num3 = x * y2 - y * x2;
			double num4 = x * x2 + y * y2 + z * z2;
			QuaternionD result = default(QuaternionD);
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Multiplies two quaternions.
		/// </summary>
		/// <param name="quaternion1">The quaternion on the left of the multiplication.</param><param name="quaternion2">The quaternion on the right of the multiplication.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref QuaternionD quaternion1, ref QuaternionD quaternion2, out QuaternionD result)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double x2 = quaternion2.X;
			double y2 = quaternion2.Y;
			double z2 = quaternion2.Z;
			double w2 = quaternion2.W;
			double num = y * z2 - z * y2;
			double num2 = z * x2 - x * z2;
			double num3 = x * y2 - y * x2;
			double num4 = x * x2 + y * y2 + z * z2;
			result.X = x * w2 + x2 * w + num;
			result.Y = y * w2 + y2 * w + num2;
			result.Z = z * w2 + z2 * w + num3;
			result.W = w * w2 - num4;
		}

		/// <summary>
		/// Multiplies a quaternion by a scalar value.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="scaleFactor">Scalar value.</param>
		public static QuaternionD Multiply(QuaternionD quaternion1, double scaleFactor)
		{
			QuaternionD result = default(QuaternionD);
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a quaternion by a scalar value.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref QuaternionD quaternion1, double scaleFactor, out QuaternionD result)
		{
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
		}

		/// <summary>
		/// Divides a QuaternionD by another QuaternionD.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">The divisor.</param>
		public static QuaternionD Divide(QuaternionD quaternion1, QuaternionD quaternion2)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double num = 1.0 / (quaternion2.X * quaternion2.X + quaternion2.Y * quaternion2.Y + quaternion2.Z * quaternion2.Z + quaternion2.W * quaternion2.W);
			double num2 = (0.0 - quaternion2.X) * num;
			double num3 = (0.0 - quaternion2.Y) * num;
			double num4 = (0.0 - quaternion2.Z) * num;
			double num5 = quaternion2.W * num;
			double num6 = y * num4 - z * num3;
			double num7 = z * num2 - x * num4;
			double num8 = x * num3 - y * num2;
			double num9 = x * num2 + y * num3 + z * num4;
			QuaternionD result = default(QuaternionD);
			result.X = x * num5 + num2 * w + num6;
			result.Y = y * num5 + num3 * w + num7;
			result.Z = z * num5 + num4 * w + num8;
			result.W = w * num5 - num9;
			return result;
		}

		/// <summary>
		/// Divides a QuaternionD by another QuaternionD.
		/// </summary>
		/// <param name="quaternion1">Source QuaternionD.</param><param name="quaternion2">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref QuaternionD quaternion1, ref QuaternionD quaternion2, out QuaternionD result)
		{
			double x = quaternion1.X;
			double y = quaternion1.Y;
			double z = quaternion1.Z;
			double w = quaternion1.W;
			double num = 1.0 / (quaternion2.X * quaternion2.X + quaternion2.Y * quaternion2.Y + quaternion2.Z * quaternion2.Z + quaternion2.W * quaternion2.W);
			double num2 = (0.0 - quaternion2.X) * num;
			double num3 = (0.0 - quaternion2.Y) * num;
			double num4 = (0.0 - quaternion2.Z) * num;
			double num5 = quaternion2.W * num;
			double num6 = y * num4 - z * num3;
			double num7 = z * num2 - x * num4;
			double num8 = x * num3 - y * num2;
			double num9 = x * num2 + y * num3 + z * num4;
			result.X = x * num5 + num2 * w + num6;
			result.Y = y * num5 + num3 * w + num7;
			result.Z = z * num5 + num4 * w + num8;
			result.W = w * num5 - num9;
		}

		public static QuaternionD FromVector4(Vector4D v)
		{
			return new QuaternionD(v.X, v.Y, v.Z, v.W);
		}

		public Vector4D ToVector4()
		{
			return new Vector4D(X, Y, Z, W);
		}

		public static bool IsZero(QuaternionD value)
		{
			return IsZero(value, 0.0001);
		}

		public static bool IsZero(QuaternionD value, double epsilon)
		{
			if (Math.Abs(value.X) < epsilon && Math.Abs(value.Y) < epsilon && Math.Abs(value.Z) < epsilon)
			{
				return Math.Abs(value.W) < epsilon;
			}
			return false;
		}

		public static void CreateFromTwoVectors(ref Vector3D firstVector, ref Vector3D secondVector, out QuaternionD result)
		{
			Vector3D.Cross(ref firstVector, ref secondVector, out Vector3D result2);
			result = new QuaternionD(result2.X, result2.Y, result2.Z, Vector3D.Dot(firstVector, secondVector));
			result.W += result.Length();
			result.Normalize();
		}

		public static QuaternionD CreateFromTwoVectors(Vector3D firstVector, Vector3D secondVector)
		{
			CreateFromTwoVectors(ref firstVector, ref secondVector, out QuaternionD result);
			return result;
		}
	}
}
