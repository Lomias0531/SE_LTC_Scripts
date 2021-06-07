using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a four-dimensional vector (x,y,z,w), which is used to efficiently rotate an object about the (x, y, z) vector by the angle theta, where w = cos(theta/2).
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct Quaternion : IEquatable<Quaternion>
	{
		protected class VRageMath_Quaternion_003C_003EX_003C_003EAccessor : IMemberAccessor<Quaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Quaternion owner, in float value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Quaternion owner, out float value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Quaternion_003C_003EY_003C_003EAccessor : IMemberAccessor<Quaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Quaternion owner, in float value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Quaternion owner, out float value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Quaternion_003C_003EZ_003C_003EAccessor : IMemberAccessor<Quaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Quaternion owner, in float value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Quaternion owner, out float value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Quaternion_003C_003EW_003C_003EAccessor : IMemberAccessor<Quaternion, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Quaternion owner, in float value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Quaternion owner, out float value)
			{
				value = owner.W;
			}
		}

		public static Quaternion Identity;

		public static Quaternion Zero;

		/// <summary>
		/// Specifies the x-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(1)]
		public float X;

		/// <summary>
		/// Specifies the y-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(4)]
		public float Y;

		/// <summary>
		/// Specifies the z-value of the vector component of the quaternion.
		/// </summary>
		[ProtoMember(7)]
		public float Z;

		/// <summary>
		/// Specifies the rotation component of the quaternion.
		/// </summary>
		[ProtoMember(10)]
		public float W;

		public Vector3 Forward
		{
			get
			{
				GetForward(ref this, out Vector3 result);
				return result;
			}
		}

		public Vector3 Right
		{
			get
			{
				GetRight(ref this, out Vector3 result);
				return result;
			}
		}

		public Vector3 Up
		{
			get
			{
				GetUp(ref this, out Vector3 result);
				return result;
			}
		}

		static Quaternion()
		{
			Identity = new Quaternion(0f, 0f, 0f, 1f);
			Zero = new Quaternion(0f, 0f, 0f, 0f);
		}

		/// <summary>
		/// Initializes a new instance of Quaternion.
		/// </summary>
		/// <param name="x">The x-value of the quaternion.</param><param name="y">The y-value of the quaternion.</param><param name="z">The z-value of the quaternion.</param><param name="w">The w-value of the quaternion.</param>
		public Quaternion(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Initializes a new instance of Quaternion.
		/// </summary>
		/// <param name="vectorPart">The vector component of the quaternion.</param><param name="scalarPart">The rotation component of the quaternion.</param>
		public Quaternion(Vector3 vectorPart, float scalarPart)
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
		public static Quaternion operator -(Quaternion quaternion)
		{
			Quaternion result = default(Quaternion);
			result.X = 0f - quaternion.X;
			result.Y = 0f - quaternion.Y;
			result.Z = 0f - quaternion.Z;
			result.W = 0f - quaternion.W;
			return result;
		}

		/// <summary>
		/// Compares two Quaternions for equality.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">Source Quaternion.</param>
		public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
		{
			if ((double)quaternion1.X == (double)quaternion2.X && (double)quaternion1.Y == (double)quaternion2.Y && (double)quaternion1.Z == (double)quaternion2.Z)
			{
				return (double)quaternion1.W == (double)quaternion2.W;
			}
			return false;
		}

		/// <summary>
		/// Compare two Quaternions for inequality.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">Source Quaternion.</param>
		public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
		{
			if ((double)quaternion1.X == (double)quaternion2.X && (double)quaternion1.Y == (double)quaternion2.Y && (double)quaternion1.Z == (double)quaternion2.Z)
			{
				return (double)quaternion1.W != (double)quaternion2.W;
			}
			return true;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Quaternion to add.</param><param name="quaternion2">Quaternion to add.</param>
		public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result = default(Quaternion);
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
		public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result = default(Quaternion);
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
		public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float x2 = quaternion2.X;
			float y2 = quaternion2.Y;
			float z2 = quaternion2.Z;
			float w2 = quaternion2.W;
			float num = (float)((double)y * (double)z2 - (double)z * (double)y2);
			float num2 = (float)((double)z * (double)x2 - (double)x * (double)z2);
			float num3 = (float)((double)x * (double)y2 - (double)y * (double)x2);
			float num4 = (float)((double)x * (double)x2 + (double)y * (double)y2 + (double)z * (double)z2);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)x * (double)w2 + (double)x2 * (double)w) + num;
			result.Y = (float)((double)y * (double)w2 + (double)y2 * (double)w) + num2;
			result.Z = (float)((double)z * (double)w2 + (double)z2 * (double)w) + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Multiplies a quaternion by a scalar value.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="scaleFactor">Scalar value.</param>
		public static Quaternion operator *(Quaternion quaternion1, float scaleFactor)
		{
			Quaternion result = default(Quaternion);
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides a Quaternion by another Quaternion.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">The divisor.</param>
		public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float num = 1f / (float)((double)quaternion2.X * (double)quaternion2.X + (double)quaternion2.Y * (double)quaternion2.Y + (double)quaternion2.Z * (double)quaternion2.Z + (double)quaternion2.W * (double)quaternion2.W);
			float num2 = (0f - quaternion2.X) * num;
			float num3 = (0f - quaternion2.Y) * num;
			float num4 = (0f - quaternion2.Z) * num;
			float num5 = quaternion2.W * num;
			float num6 = (float)((double)y * (double)num4 - (double)z * (double)num3);
			float num7 = (float)((double)z * (double)num2 - (double)x * (double)num4);
			float num8 = (float)((double)x * (double)num3 - (double)y * (double)num2);
			float num9 = (float)((double)x * (double)num2 + (double)y * (double)num3 + (double)z * (double)num4);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)x * (double)num5 + (double)num2 * (double)w) + num6;
			result.Y = (float)((double)y * (double)num5 + (double)num3 * (double)w) + num7;
			result.Z = (float)((double)z * (double)num5 + (double)num4 * (double)w) + num8;
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

		public string ToString(string format)
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", X.ToString(format, currentCulture), Y.ToString(format, currentCulture), Z.ToString(format, currentCulture), W.ToString(format, currentCulture));
		}

		public string ToStringAxisAngle(string format = "G")
		{
			GetAxisAngle(out Vector3 axis, out float angle);
			return string.Format(CultureInfo.CurrentCulture, "{{{0}/{1}}}", axis.ToString(format), angle.ToString(format));
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Quaternion.
		/// </summary>
		/// <param name="other">The Quaternion to compare with the current Quaternion.</param>
		public bool Equals(Quaternion other)
		{
			if ((double)X == (double)other.X && (double)Y == (double)other.Y && (double)Z == (double)other.Z)
			{
				return (double)W == (double)other.W;
			}
			return false;
		}

		public bool Equals(Quaternion value, float epsilon)
		{
			if (Math.Abs(X - value.X) < epsilon && Math.Abs(Y - value.Y) < epsilon && Math.Abs(Z - value.Z) < epsilon)
			{
				return Math.Abs(W - value.W) < epsilon;
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
			if (obj is Quaternion)
			{
				result = Equals((Quaternion)obj);
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
		/// Calculates the length squared of a Quaternion.
		/// </summary>
		public float LengthSquared()
		{
			return (float)((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
		}

		/// <summary>
		/// Calculates the length of a Quaternion.
		/// </summary>
		public float Length()
		{
			return (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
		}

		/// <summary>
		/// Divides each component of the quaternion by the length of the quaternion.
		/// </summary>
		public void Normalize()
		{
			float num = 1f / (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z + (double)W * (double)W);
			X *= num;
			Y *= num;
			Z *= num;
			W *= num;
		}

		public void GetAxisAngle(out Vector3 axis, out float angle)
		{
			axis.X = X;
			axis.Y = Y;
			axis.Z = Z;
			float num = axis.Length();
			float w = W;
			if (num != 0f)
			{
				axis.X /= num;
				axis.Y /= num;
				axis.Z /= num;
			}
			angle = (float)Math.Atan2(num, w) * 2f;
		}

		/// <summary>
		/// Divides each component of the quaternion by the length of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param>
		public static Quaternion Normalize(Quaternion quaternion)
		{
			float num = 1f / (float)Math.Sqrt((double)quaternion.X * (double)quaternion.X + (double)quaternion.Y * (double)quaternion.Y + (double)quaternion.Z * (double)quaternion.Z + (double)quaternion.W * (double)quaternion.W);
			Quaternion result = default(Quaternion);
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
		public static void Normalize(ref Quaternion quaternion, out Quaternion result)
		{
			float num = 1f / (float)Math.Sqrt((double)quaternion.X * (double)quaternion.X + (double)quaternion.Y * (double)quaternion.Y + (double)quaternion.Z * (double)quaternion.Z + (double)quaternion.W * (double)quaternion.W);
			result.X = quaternion.X * num;
			result.Y = quaternion.Y * num;
			result.Z = quaternion.Z * num;
			result.W = quaternion.W * num;
		}

		/// <summary>
		/// Transforms this Quaternion into its conjugate.
		/// </summary>
		public void Conjugate()
		{
			X = 0f - X;
			Y = 0f - Y;
			Z = 0f - Z;
		}

		/// <summary>
		/// Returns the conjugate of a specified Quaternion.
		/// </summary>
		/// <param name="value">The Quaternion of which to return the conjugate.</param>
		public static Quaternion Conjugate(Quaternion value)
		{
			Quaternion result = default(Quaternion);
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			result.W = value.W;
			return result;
		}

		/// <summary>
		/// Returns the conjugate of a specified Quaternion.
		/// </summary>
		/// <param name="value">The Quaternion of which to return the conjugate.</param><param name="result">[OutAttribute] An existing Quaternion filled in to be the conjugate of the specified one.</param>
		public static void Conjugate(ref Quaternion value, out Quaternion result)
		{
			result.X = 0f - value.X;
			result.Y = 0f - value.Y;
			result.Z = 0f - value.Z;
			result.W = value.W;
		}

		/// <summary>
		/// Returns the inverse of a Quaternion.
		/// </summary>
		/// <param name="quaternion">Source Quaternion.</param>
		public static Quaternion Inverse(Quaternion quaternion)
		{
			float num = 1f / (float)((double)quaternion.X * (double)quaternion.X + (double)quaternion.Y * (double)quaternion.Y + (double)quaternion.Z * (double)quaternion.Z + (double)quaternion.W * (double)quaternion.W);
			Quaternion result = default(Quaternion);
			result.X = (0f - quaternion.X) * num;
			result.Y = (0f - quaternion.Y) * num;
			result.Z = (0f - quaternion.Z) * num;
			result.W = quaternion.W * num;
			return result;
		}

		/// <summary>
		/// Returns the inverse of a Quaternion.
		/// </summary>
		/// <param name="quaternion">Source Quaternion.</param><param name="result">[OutAttribute] The inverse of the Quaternion.</param>
		public static void Inverse(ref Quaternion quaternion, out Quaternion result)
		{
			float num = 1f / (float)((double)quaternion.X * (double)quaternion.X + (double)quaternion.Y * (double)quaternion.Y + (double)quaternion.Z * (double)quaternion.Z + (double)quaternion.W * (double)quaternion.W);
			result.X = (0f - quaternion.X) * num;
			result.Y = (0f - quaternion.Y) * num;
			result.Z = (0f - quaternion.Z) * num;
			result.W = quaternion.W * num;
		}

		/// <summary>
		/// Creates a Quaternion from a vector and an angle to rotate about the vector.
		/// </summary>
		/// <param name="axis">The vector to rotate around.</param><param name="angle">The angle to rotate around the vector.</param>
		public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
		{
			float num = angle * 0.5f;
			float num2 = (float)Math.Sin(num);
			float w = (float)Math.Cos(num);
			Quaternion result = default(Quaternion);
			result.X = axis.X * num2;
			result.Y = axis.Y * num2;
			result.Z = axis.Z * num2;
			result.W = w;
			return result;
		}

		/// <summary>
		/// Creates a Quaternion from a vector and an angle to rotate about the vector.
		/// </summary>
		/// <param name="axis">The vector to rotate around.</param><param name="angle">The angle to rotate around the vector.</param><param name="result">[OutAttribute] The created Quaternion.</param>
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Quaternion result)
		{
			float num = angle * 0.5f;
			float num2 = (float)Math.Sin(num);
			float w = (float)Math.Cos(num);
			result.X = axis.X * num2;
			result.Y = axis.Y * num2;
			result.Z = axis.Z * num2;
			result.W = w;
		}

		/// <summary>
		/// Creates a new Quaternion from specified yaw, pitch, and roll angles.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the y-axis.</param><param name="pitch">The pitch angle, in radians, around the x-axis.</param><param name="roll">The roll angle, in radians, around the z-axis.</param>
		public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			float num = roll * 0.5f;
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			float num4 = pitch * 0.5f;
			float num5 = (float)Math.Sin(num4);
			float num6 = (float)Math.Cos(num4);
			float num7 = yaw * 0.5f;
			float num8 = (float)Math.Sin(num7);
			float num9 = (float)Math.Cos(num7);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)num9 * (double)num5 * (double)num3 + (double)num8 * (double)num6 * (double)num2);
			result.Y = (float)((double)num8 * (double)num6 * (double)num3 - (double)num9 * (double)num5 * (double)num2);
			result.Z = (float)((double)num9 * (double)num6 * (double)num2 - (double)num8 * (double)num5 * (double)num3);
			result.W = (float)((double)num9 * (double)num6 * (double)num3 + (double)num8 * (double)num5 * (double)num2);
			return result;
		}

		/// <summary>
		/// Creates a new Quaternion from specified yaw, pitch, and roll angles.
		/// </summary>
		/// <param name="yaw">The yaw angle, in radians, around the y-axis.</param><param name="pitch">The pitch angle, in radians, around the x-axis.</param><param name="roll">The roll angle, in radians, around the z-axis.</param><param name="result">[OutAttribute] An existing Quaternion filled in to express the specified yaw, pitch, and roll angles.</param>
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
		{
			float num = roll * 0.5f;
			float num2 = (float)Math.Sin(num);
			float num3 = (float)Math.Cos(num);
			float num4 = pitch * 0.5f;
			float num5 = (float)Math.Sin(num4);
			float num6 = (float)Math.Cos(num4);
			float num7 = yaw * 0.5f;
			float num8 = (float)Math.Sin(num7);
			float num9 = (float)Math.Cos(num7);
			result.X = (float)((double)num9 * (double)num5 * (double)num3 + (double)num8 * (double)num6 * (double)num2);
			result.Y = (float)((double)num8 * (double)num6 * (double)num3 - (double)num9 * (double)num5 * (double)num2);
			result.Z = (float)((double)num9 * (double)num6 * (double)num2 - (double)num8 * (double)num5 * (double)num3);
			result.W = (float)((double)num9 * (double)num6 * (double)num3 + (double)num8 * (double)num5 * (double)num2);
		}

		/// <summary>
		/// Works for normalized vectors only
		/// </summary>
		public static Quaternion CreateFromForwardUp(Vector3 forward, Vector3 up)
		{
			Vector3 vector = -forward;
			Vector3 vector2 = Vector3.Cross(up, vector);
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			float x = vector2.X;
			float y = vector2.Y;
			float z = vector2.Z;
			float x2 = vector3.X;
			float y2 = vector3.Y;
			float z2 = vector3.Z;
			float x3 = vector.X;
			float y3 = vector.Y;
			float z3 = vector.Z;
			float num = x + y2 + z3;
			Quaternion result = default(Quaternion);
			if (num > 0f)
			{
				float num2 = (float)Math.Sqrt(num + 1f);
				result.W = num2 * 0.5f;
				num2 = 0.5f / num2;
				result.X = (z2 - y3) * num2;
				result.Y = (x3 - z) * num2;
				result.Z = (y - x2) * num2;
				return result;
			}
			if (x >= y2 && x >= z3)
			{
				float num3 = (float)Math.Sqrt(1f + x - y2 - z3);
				float num4 = 0.5f / num3;
				result.X = 0.5f * num3;
				result.Y = (y + x2) * num4;
				result.Z = (z + x3) * num4;
				result.W = (z2 - y3) * num4;
				return result;
			}
			if (y2 > z3)
			{
				float num5 = (float)Math.Sqrt(1f + y2 - x - z3);
				float num6 = 0.5f / num5;
				result.X = (x2 + y) * num6;
				result.Y = 0.5f * num5;
				result.Z = (y3 + z2) * num6;
				result.W = (x3 - z) * num6;
				return result;
			}
			float num7 = (float)Math.Sqrt(1f + z3 - x - y2);
			float num8 = 0.5f / num7;
			result.X = (x3 + z) * num8;
			result.Y = (y3 + z2) * num8;
			result.Z = 0.5f * num7;
			result.W = (y - x2) * num8;
			return result;
		}

		public static Quaternion CreateFromRotationMatrix(MatrixD matrix)
		{
			return CreateFromRotationMatrix((Matrix)matrix);
		}

		/// <summary>
		/// Creates a Quaternion from a rotation Matrix.
		/// </summary>
		/// <param name="matrix">The rotation Matrix to create the Quaternion from.</param>
		public static Quaternion CreateFromRotationMatrix(Matrix matrix)
		{
			float num = matrix.M11 + matrix.M22 + matrix.M33;
			Quaternion result = default(Quaternion);
			if ((double)num > 0.0)
			{
				float num2 = (float)Math.Sqrt((double)num + 1.0);
				result.W = num2 * 0.5f;
				float num3 = 0.5f / num2;
				result.X = (matrix.M23 - matrix.M32) * num3;
				result.Y = (matrix.M31 - matrix.M13) * num3;
				result.Z = (matrix.M12 - matrix.M21) * num3;
			}
			else if ((double)matrix.M11 >= (double)matrix.M22 && (double)matrix.M11 >= (double)matrix.M33)
			{
				float num4 = (float)Math.Sqrt(1.0 + (double)matrix.M11 - (double)matrix.M22 - (double)matrix.M33);
				float num5 = 0.5f / num4;
				result.X = 0.5f * num4;
				result.Y = (matrix.M12 + matrix.M21) * num5;
				result.Z = (matrix.M13 + matrix.M31) * num5;
				result.W = (matrix.M23 - matrix.M32) * num5;
			}
			else if ((double)matrix.M22 > (double)matrix.M33)
			{
				float num6 = (float)Math.Sqrt(1.0 + (double)matrix.M22 - (double)matrix.M11 - (double)matrix.M33);
				float num7 = 0.5f / num6;
				result.X = (matrix.M21 + matrix.M12) * num7;
				result.Y = 0.5f * num6;
				result.Z = (matrix.M32 + matrix.M23) * num7;
				result.W = (matrix.M31 - matrix.M13) * num7;
			}
			else
			{
				float num8 = (float)Math.Sqrt(1.0 + (double)matrix.M33 - (double)matrix.M11 - (double)matrix.M22);
				float num9 = 0.5f / num8;
				result.X = (matrix.M31 + matrix.M13) * num9;
				result.Y = (matrix.M32 + matrix.M23) * num9;
				result.Z = 0.5f * num8;
				result.W = (matrix.M12 - matrix.M21) * num9;
			}
			return result;
		}

		public static void CreateFromRotationMatrix(ref MatrixD matrix, out Quaternion result)
		{
			Matrix matrix2 = matrix;
			CreateFromRotationMatrix(ref matrix2, out result);
		}

		public static void CreateFromTwoVectors(ref Vector3 firstVector, ref Vector3 secondVector, out Quaternion result)
		{
			Vector3.Cross(ref firstVector, ref secondVector, out Vector3 result2);
			result = new Quaternion(result2.X, result2.Y, result2.Z, Vector3.Dot(firstVector, secondVector));
			result.W += result.Length();
			result.Normalize();
		}

		public static Quaternion CreateFromTwoVectors(Vector3 firstVector, Vector3 secondVector)
		{
			CreateFromTwoVectors(ref firstVector, ref secondVector, out Quaternion result);
			return result;
		}

		/// <summary>
		/// Creates a Quaternion from a rotation Matrix.
		/// </summary>
		/// <param name="matrix">The rotation Matrix to create the Quaternion from.</param><param name="result">[OutAttribute] The created Quaternion.</param>
		public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
		{
			float num = matrix.M11 + matrix.M22 + matrix.M33;
			if ((double)num > 0.0)
			{
				float num2 = (float)Math.Sqrt((double)num + 1.0);
				result.W = num2 * 0.5f;
				float num3 = 0.5f / num2;
				result.X = (matrix.M23 - matrix.M32) * num3;
				result.Y = (matrix.M31 - matrix.M13) * num3;
				result.Z = (matrix.M12 - matrix.M21) * num3;
			}
			else if ((double)matrix.M11 >= (double)matrix.M22 && (double)matrix.M11 >= (double)matrix.M33)
			{
				float num4 = (float)Math.Sqrt(1.0 + (double)matrix.M11 - (double)matrix.M22 - (double)matrix.M33);
				float num5 = 0.5f / num4;
				result.X = 0.5f * num4;
				result.Y = (matrix.M12 + matrix.M21) * num5;
				result.Z = (matrix.M13 + matrix.M31) * num5;
				result.W = (matrix.M23 - matrix.M32) * num5;
			}
			else if ((double)matrix.M22 > (double)matrix.M33)
			{
				float num6 = (float)Math.Sqrt(1.0 + (double)matrix.M22 - (double)matrix.M11 - (double)matrix.M33);
				float num7 = 0.5f / num6;
				result.X = (matrix.M21 + matrix.M12) * num7;
				result.Y = 0.5f * num6;
				result.Z = (matrix.M32 + matrix.M23) * num7;
				result.W = (matrix.M31 - matrix.M13) * num7;
			}
			else
			{
				float num8 = (float)Math.Sqrt(1.0 + (double)matrix.M33 - (double)matrix.M11 - (double)matrix.M22);
				float num9 = 0.5f / num8;
				result.X = (matrix.M31 + matrix.M13) * num9;
				result.Y = (matrix.M32 + matrix.M23) * num9;
				result.Z = 0.5f * num8;
				result.W = (matrix.M12 - matrix.M21) * num9;
			}
		}

		/// <summary>
		/// Creates a Quaternion from a rotation Matrix.
		/// </summary>
		/// <param name="matrix">The rotation Matrix to create the Quaternion from.</param><param name="result">[OutAttribute] The created Quaternion.</param>
		public static void CreateFromRotationMatrix(ref Matrix3x3 matrix, out Quaternion result)
		{
			float num = matrix.M11 + matrix.M22 + matrix.M33;
			if ((double)num > 0.0)
			{
				float num2 = (float)Math.Sqrt((double)num + 1.0);
				result.W = num2 * 0.5f;
				float num3 = 0.5f / num2;
				result.X = (matrix.M23 - matrix.M32) * num3;
				result.Y = (matrix.M31 - matrix.M13) * num3;
				result.Z = (matrix.M12 - matrix.M21) * num3;
			}
			else if ((double)matrix.M11 >= (double)matrix.M22 && (double)matrix.M11 >= (double)matrix.M33)
			{
				float num4 = (float)Math.Sqrt(1.0 + (double)matrix.M11 - (double)matrix.M22 - (double)matrix.M33);
				float num5 = 0.5f / num4;
				result.X = 0.5f * num4;
				result.Y = (matrix.M12 + matrix.M21) * num5;
				result.Z = (matrix.M13 + matrix.M31) * num5;
				result.W = (matrix.M23 - matrix.M32) * num5;
			}
			else if ((double)matrix.M22 > (double)matrix.M33)
			{
				float num6 = (float)Math.Sqrt(1.0 + (double)matrix.M22 - (double)matrix.M11 - (double)matrix.M33);
				float num7 = 0.5f / num6;
				result.X = (matrix.M21 + matrix.M12) * num7;
				result.Y = 0.5f * num6;
				result.Z = (matrix.M32 + matrix.M23) * num7;
				result.W = (matrix.M31 - matrix.M13) * num7;
			}
			else
			{
				float num8 = (float)Math.Sqrt(1.0 + (double)matrix.M33 - (double)matrix.M11 - (double)matrix.M22);
				float num9 = 0.5f / num8;
				result.X = (matrix.M31 + matrix.M13) * num9;
				result.Y = (matrix.M32 + matrix.M23) * num9;
				result.Z = 0.5f * num8;
				result.W = (matrix.M12 - matrix.M21) * num9;
			}
		}

		/// <summary>
		/// Calculates the dot product of two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">Source Quaternion.</param>
		public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
		{
			return (float)((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W);
		}

		/// <summary>
		/// Calculates the dot product of two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">Source Quaternion.</param><param name="result">[OutAttribute] Dot product of the Quaternions.</param>
		public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out float result)
		{
			result = (float)((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W);
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value that indicates how far to interpolate between the quaternions.</param>
		public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			float num = (float)((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W);
			bool flag = false;
			if ((double)num < 0.0)
			{
				flag = true;
				num = 0f - num;
			}
			float num2;
			float num3;
			if ((double)num > 0.999998986721039)
			{
				num2 = 1f - amount;
				num3 = (flag ? (0f - amount) : amount);
			}
			else
			{
				float num4 = (float)Math.Acos(num);
				float num5 = (float)(1.0 / Math.Sin(num4));
				num2 = (float)Math.Sin((1.0 - (double)amount) * (double)num4) * num5;
				num3 = (flag ? ((float)(0.0 - Math.Sin((double)amount * (double)num4)) * num5) : ((float)Math.Sin((double)amount * (double)num4) * num5));
			}
			Quaternion result = default(Quaternion);
			result.X = (float)((double)num2 * (double)quaternion1.X + (double)num3 * (double)quaternion2.X);
			result.Y = (float)((double)num2 * (double)quaternion1.Y + (double)num3 * (double)quaternion2.Y);
			result.Z = (float)((double)num2 * (double)quaternion1.Z + (double)num3 * (double)quaternion2.Z);
			result.W = (float)((double)num2 * (double)quaternion1.W + (double)num3 * (double)quaternion2.W);
			return result;
		}

		/// <summary>
		/// Interpolates between two quaternions, using spherical linear interpolation.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value that indicates how far to interpolate between the quaternions.</param><param name="result">[OutAttribute] Result of the interpolation.</param>
		public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
		{
			float num = (float)((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W);
			bool flag = false;
			if ((double)num < 0.0)
			{
				flag = true;
				num = 0f - num;
			}
			float num2;
			float num3;
			if ((double)num > 0.999998986721039)
			{
				num2 = 1f - amount;
				num3 = (flag ? (0f - amount) : amount);
			}
			else
			{
				float num4 = (float)Math.Acos(num);
				float num5 = (float)(1.0 / Math.Sin(num4));
				num2 = (float)Math.Sin((1.0 - (double)amount) * (double)num4) * num5;
				num3 = (flag ? ((float)(0.0 - Math.Sin((double)amount * (double)num4)) * num5) : ((float)Math.Sin((double)amount * (double)num4) * num5));
			}
			result.X = (float)((double)num2 * (double)quaternion1.X + (double)num3 * (double)quaternion2.X);
			result.Y = (float)((double)num2 * (double)quaternion1.Y + (double)num3 * (double)quaternion2.Y);
			result.Z = (float)((double)num2 * (double)quaternion1.Z + (double)num3 * (double)quaternion2.Z);
			result.W = (float)((double)num2 * (double)quaternion1.W + (double)num3 * (double)quaternion2.W);
		}

		/// <summary>
		/// Linearly interpolates between two quaternions.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="quaternion2">Source quaternion.</param><param name="amount">Value indicating how far to interpolate between the quaternions.</param>
		public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
		{
			float num = 1f - amount;
			Quaternion result = default(Quaternion);
			if ((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W >= 0.0)
			{
				result.X = (float)((double)num * (double)quaternion1.X + (double)amount * (double)quaternion2.X);
				result.Y = (float)((double)num * (double)quaternion1.Y + (double)amount * (double)quaternion2.Y);
				result.Z = (float)((double)num * (double)quaternion1.Z + (double)amount * (double)quaternion2.Z);
				result.W = (float)((double)num * (double)quaternion1.W + (double)amount * (double)quaternion2.W);
			}
			else
			{
				result.X = (float)((double)num * (double)quaternion1.X - (double)amount * (double)quaternion2.X);
				result.Y = (float)((double)num * (double)quaternion1.Y - (double)amount * (double)quaternion2.Y);
				result.Z = (float)((double)num * (double)quaternion1.Z - (double)amount * (double)quaternion2.Z);
				result.W = (float)((double)num * (double)quaternion1.W - (double)amount * (double)quaternion2.W);
			}
			float num2 = 1f / (float)Math.Sqrt((double)result.X * (double)result.X + (double)result.Y * (double)result.Y + (double)result.Z * (double)result.Z + (double)result.W * (double)result.W);
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
		public static void Lerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
		{
			float num = 1f - amount;
			if ((double)quaternion1.X * (double)quaternion2.X + (double)quaternion1.Y * (double)quaternion2.Y + (double)quaternion1.Z * (double)quaternion2.Z + (double)quaternion1.W * (double)quaternion2.W >= 0.0)
			{
				result.X = (float)((double)num * (double)quaternion1.X + (double)amount * (double)quaternion2.X);
				result.Y = (float)((double)num * (double)quaternion1.Y + (double)amount * (double)quaternion2.Y);
				result.Z = (float)((double)num * (double)quaternion1.Z + (double)amount * (double)quaternion2.Z);
				result.W = (float)((double)num * (double)quaternion1.W + (double)amount * (double)quaternion2.W);
			}
			else
			{
				result.X = (float)((double)num * (double)quaternion1.X - (double)amount * (double)quaternion2.X);
				result.Y = (float)((double)num * (double)quaternion1.Y - (double)amount * (double)quaternion2.Y);
				result.Z = (float)((double)num * (double)quaternion1.Z - (double)amount * (double)quaternion2.Z);
				result.W = (float)((double)num * (double)quaternion1.W - (double)amount * (double)quaternion2.W);
			}
			float num2 = 1f / (float)Math.Sqrt((double)result.X * (double)result.X + (double)result.Y * (double)result.Y + (double)result.Z * (double)result.Z + (double)result.W * (double)result.W);
			result.X *= num2;
			result.Y *= num2;
			result.Z *= num2;
			result.W *= num2;
		}

		/// <summary>
		/// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
		/// </summary>
		/// <param name="value1">The first Quaternion rotation in the series.</param><param name="value2">The second Quaternion rotation in the series.</param>
		public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
		{
			float x = value2.X;
			float y = value2.Y;
			float z = value2.Z;
			float w = value2.W;
			float x2 = value1.X;
			float y2 = value1.Y;
			float z2 = value1.Z;
			float w2 = value1.W;
			float num = (float)((double)y * (double)z2 - (double)z * (double)y2);
			float num2 = (float)((double)z * (double)x2 - (double)x * (double)z2);
			float num3 = (float)((double)x * (double)y2 - (double)y * (double)x2);
			float num4 = (float)((double)x * (double)x2 + (double)y * (double)y2 + (double)z * (double)z2);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)x * (double)w2 + (double)x2 * (double)w) + num;
			result.Y = (float)((double)y * (double)w2 + (double)y2 * (double)w) + num2;
			result.Z = (float)((double)z * (double)w2 + (double)z2 * (double)w) + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Concatenates two Quaternions; the result represents the value1 rotation followed by the value2 rotation.
		/// </summary>
		/// <param name="value1">The first Quaternion rotation in the series.</param><param name="value2">The second Quaternion rotation in the series.</param><param name="result">[OutAttribute] The Quaternion rotation representing the concatenation of value1 followed by value2.</param>
		public static void Concatenate(ref Quaternion value1, ref Quaternion value2, out Quaternion result)
		{
			float x = value2.X;
			float y = value2.Y;
			float z = value2.Z;
			float w = value2.W;
			float x2 = value1.X;
			float y2 = value1.Y;
			float z2 = value1.Z;
			float w2 = value1.W;
			float num = (float)((double)y * (double)z2 - (double)z * (double)y2);
			float num2 = (float)((double)z * (double)x2 - (double)x * (double)z2);
			float num3 = (float)((double)x * (double)y2 - (double)y * (double)x2);
			float num4 = (float)((double)x * (double)x2 + (double)y * (double)y2 + (double)z * (double)z2);
			result.X = (float)((double)x * (double)w2 + (double)x2 * (double)w) + num;
			result.Y = (float)((double)y * (double)w2 + (double)y2 * (double)w) + num2;
			result.Z = (float)((double)z * (double)w2 + (double)z2 * (double)w) + num3;
			result.W = w * w2 - num4;
		}

		/// <summary>
		/// Flips the sign of each component of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param>
		public static Quaternion Negate(Quaternion quaternion)
		{
			Quaternion result = default(Quaternion);
			result.X = 0f - quaternion.X;
			result.Y = 0f - quaternion.Y;
			result.Z = 0f - quaternion.Z;
			result.W = 0f - quaternion.W;
			return result;
		}

		/// <summary>
		/// Flips the sign of each component of the quaternion.
		/// </summary>
		/// <param name="quaternion">Source quaternion.</param><param name="result">[OutAttribute] Negated quaternion.</param>
		public static void Negate(ref Quaternion quaternion, out Quaternion result)
		{
			result.X = 0f - quaternion.X;
			result.Y = 0f - quaternion.Y;
			result.Z = 0f - quaternion.Z;
			result.W = 0f - quaternion.W;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Quaternion to add.</param><param name="quaternion2">Quaternion to add.</param>
		public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result = default(Quaternion);
			result.X = quaternion1.X + quaternion2.X;
			result.Y = quaternion1.Y + quaternion2.Y;
			result.Z = quaternion1.Z + quaternion2.Z;
			result.W = quaternion1.W + quaternion2.W;
			return result;
		}

		/// <summary>
		/// Adds two Quaternions.
		/// </summary>
		/// <param name="quaternion1">Quaternion to add.</param><param name="quaternion2">Quaternion to add.</param><param name="result">[OutAttribute] Result of adding the Quaternions.</param>
		public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
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
		public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result = default(Quaternion);
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
		public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
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
		public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float x2 = quaternion2.X;
			float y2 = quaternion2.Y;
			float z2 = quaternion2.Z;
			float w2 = quaternion2.W;
			float num = (float)((double)y * (double)z2 - (double)z * (double)y2);
			float num2 = (float)((double)z * (double)x2 - (double)x * (double)z2);
			float num3 = (float)((double)x * (double)y2 - (double)y * (double)x2);
			float num4 = (float)((double)x * (double)x2 + (double)y * (double)y2 + (double)z * (double)z2);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)x * (double)w2 + (double)x2 * (double)w) + num;
			result.Y = (float)((double)y * (double)w2 + (double)y2 * (double)w) + num2;
			result.Z = (float)((double)z * (double)w2 + (double)z2 * (double)w) + num3;
			result.W = w * w2 - num4;
			return result;
		}

		/// <summary>
		/// Multiplies two quaternions.
		/// </summary>
		/// <param name="quaternion1">The quaternion on the left of the multiplication.</param><param name="quaternion2">The quaternion on the right of the multiplication.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float x2 = quaternion2.X;
			float y2 = quaternion2.Y;
			float z2 = quaternion2.Z;
			float w2 = quaternion2.W;
			float num = (float)((double)y * (double)z2 - (double)z * (double)y2);
			float num2 = (float)((double)z * (double)x2 - (double)x * (double)z2);
			float num3 = (float)((double)x * (double)y2 - (double)y * (double)x2);
			float num4 = (float)((double)x * (double)x2 + (double)y * (double)y2 + (double)z * (double)z2);
			result.X = (float)((double)x * (double)w2 + (double)x2 * (double)w) + num;
			result.Y = (float)((double)y * (double)w2 + (double)y2 * (double)w) + num2;
			result.Z = (float)((double)z * (double)w2 + (double)z2 * (double)w) + num3;
			result.W = w * w2 - num4;
		}

		/// <summary>
		/// Multiplies a quaternion by a scalar value.
		/// </summary>
		/// <param name="quaternion1">Source quaternion.</param><param name="scaleFactor">Scalar value.</param>
		public static Quaternion Multiply(Quaternion quaternion1, float scaleFactor)
		{
			Quaternion result = default(Quaternion);
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
		public static void Multiply(ref Quaternion quaternion1, float scaleFactor, out Quaternion result)
		{
			result.X = quaternion1.X * scaleFactor;
			result.Y = quaternion1.Y * scaleFactor;
			result.Z = quaternion1.Z * scaleFactor;
			result.W = quaternion1.W * scaleFactor;
		}

		/// <summary>
		/// Divides a Quaternion by another Quaternion.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">The divisor.</param>
		public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float num = 1f / (float)((double)quaternion2.X * (double)quaternion2.X + (double)quaternion2.Y * (double)quaternion2.Y + (double)quaternion2.Z * (double)quaternion2.Z + (double)quaternion2.W * (double)quaternion2.W);
			float num2 = (0f - quaternion2.X) * num;
			float num3 = (0f - quaternion2.Y) * num;
			float num4 = (0f - quaternion2.Z) * num;
			float num5 = quaternion2.W * num;
			float num6 = (float)((double)y * (double)num4 - (double)z * (double)num3);
			float num7 = (float)((double)z * (double)num2 - (double)x * (double)num4);
			float num8 = (float)((double)x * (double)num3 - (double)y * (double)num2);
			float num9 = (float)((double)x * (double)num2 + (double)y * (double)num3 + (double)z * (double)num4);
			Quaternion result = default(Quaternion);
			result.X = (float)((double)x * (double)num5 + (double)num2 * (double)w) + num6;
			result.Y = (float)((double)y * (double)num5 + (double)num3 * (double)w) + num7;
			result.Z = (float)((double)z * (double)num5 + (double)num4 * (double)w) + num8;
			result.W = w * num5 - num9;
			return result;
		}

		/// <summary>
		/// Divides a Quaternion by another Quaternion.
		/// </summary>
		/// <param name="quaternion1">Source Quaternion.</param><param name="quaternion2">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			float x = quaternion1.X;
			float y = quaternion1.Y;
			float z = quaternion1.Z;
			float w = quaternion1.W;
			float num = 1f / (float)((double)quaternion2.X * (double)quaternion2.X + (double)quaternion2.Y * (double)quaternion2.Y + (double)quaternion2.Z * (double)quaternion2.Z + (double)quaternion2.W * (double)quaternion2.W);
			float num2 = (0f - quaternion2.X) * num;
			float num3 = (0f - quaternion2.Y) * num;
			float num4 = (0f - quaternion2.Z) * num;
			float num5 = quaternion2.W * num;
			float num6 = (float)((double)y * (double)num4 - (double)z * (double)num3);
			float num7 = (float)((double)z * (double)num2 - (double)x * (double)num4);
			float num8 = (float)((double)x * (double)num3 - (double)y * (double)num2);
			float num9 = (float)((double)x * (double)num2 + (double)y * (double)num3 + (double)z * (double)num4);
			result.X = (float)((double)x * (double)num5 + (double)num2 * (double)w) + num6;
			result.Y = (float)((double)y * (double)num5 + (double)num3 * (double)w) + num7;
			result.Z = (float)((double)z * (double)num5 + (double)num4 * (double)w) + num8;
			result.W = w * num5 - num9;
		}

		public static Quaternion FromVector4(Vector4 v)
		{
			return new Quaternion(v.X, v.Y, v.Z, v.W);
		}

		public Vector4 ToVector4()
		{
			return new Vector4(X, Y, Z, W);
		}

		public static bool IsZero(Quaternion value)
		{
			return IsZero(value, 0.0001f);
		}

		public static bool IsZero(Quaternion value, float epsilon)
		{
			if (Math.Abs(value.X) < epsilon && Math.Abs(value.Y) < epsilon && Math.Abs(value.Z) < epsilon)
			{
				return Math.Abs(value.W) < epsilon;
			}
			return false;
		}

		/// <summary>
		/// Gets forward vector (0,0,-1) transformed by quaternion.
		/// </summary>
		public static void GetForward(ref Quaternion q, out Vector3 result)
		{
			float num = q.X + q.X;
			float num2 = q.Y + q.Y;
			float num3 = q.Z + q.Z;
			float num4 = q.W * num;
			float num5 = q.W * num2;
			float num6 = q.X * num;
			float num7 = q.X * num3;
			float num8 = q.Y * num2;
			float num9 = q.Y * num3;
			result.X = 0f - num7 - num5;
			result.Y = num4 - num9;
			result.Z = num6 + num8 - 1f;
		}

		/// <summary>
		/// Gets right vector (1,0,0) transformed by quaternion.
		/// </summary>
		public static void GetRight(ref Quaternion q, out Vector3 result)
		{
			float num = q.Y + q.Y;
			float num2 = q.Z + q.Z;
			float num3 = q.W * num;
			float num4 = q.W * num2;
			float num5 = q.X * num;
			float num6 = q.X * num2;
			float num7 = q.Y * num;
			float num8 = q.Z * num2;
			result.X = 1f - num7 - num8;
			result.Y = num5 + num4;
			result.Z = num6 - num3;
		}

		/// <summary>
		/// Gets up vector (0,1,0) transformed by quaternion.
		/// </summary>
		public static void GetUp(ref Quaternion q, out Vector3 result)
		{
			float num = q.X + q.X;
			float num2 = q.Y + q.Y;
			float num3 = q.Z + q.Z;
			float num4 = q.W * num;
			float num5 = q.W * num3;
			float num6 = q.X * num;
			float num7 = q.X * num2;
			float num8 = q.Y * num3;
			float num9 = q.Z * num3;
			result.X = num7 - num5;
			result.Y = 1f - num6 - num9;
			result.Z = num8 + num4;
		}

		public float GetComponent(int index)
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
				return 0f;
			}
		}

		public void SetComponent(int index, float value)
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
			}
		}

		public int FindLargestIndex()
		{
			int result = 0;
			float num = X;
			for (int i = 1; i < 4; i++)
			{
				float num2 = Math.Abs(GetComponent(i));
				if (num2 > num)
				{
					result = i;
					num = num2;
				}
			}
			return result;
		}
	}
}
