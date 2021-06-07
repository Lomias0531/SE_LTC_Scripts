using ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a matrix.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	[ProtoContract]
	public struct Matrix3x3 : IEquatable<Matrix3x3>
	{
		private struct F9
		{
			public unsafe fixed float data[9];
		}

		protected class VRageMath_Matrix3x3_003C_003EM_003C_003EAccessor : IMemberAccessor<Matrix3x3, F9>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in F9 value)
			{
				owner.M = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out F9 value)
			{
				value = owner.M;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM11_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M11 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M11;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM12_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M12 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M12;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM13_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M13 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M13;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM21_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M21 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M21;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM22_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M22 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M22;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM23_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M23 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M23;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM31_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M31 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M31;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM32_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M32 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M32;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EM33_003C_003EAccessor : IMemberAccessor<Matrix3x3, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in float value)
			{
				owner.M33 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out float value)
			{
				value = owner.M33;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EUp_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Up;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EDown_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Down = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Down;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003ERight_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Right = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Right;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003ELeft_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Left = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Left;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EForward_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Forward;
			}
		}

		protected class VRageMath_Matrix3x3_003C_003EBackward_003C_003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix3x3 owner, in Vector3 value)
			{
				owner.Backward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix3x3 owner, out Vector3 value)
			{
				value = owner.Backward;
			}
		}

		public static Matrix3x3 Identity;

		public static Matrix3x3 Zero;

		/// <summary>
		/// Matrix3x3 values
		/// </summary>
		[FieldOffset(0)]
		private F9 M;

		/// <summary>
		/// Value at row 1 column 1 of the matrix.
		/// </summary>
		[FieldOffset(0)]
		[ProtoMember(1)]
		public float M11;

		/// <summary>
		/// Value at row 1 column 2 of the matrix.
		/// </summary>
		[FieldOffset(4)]
		[ProtoMember(4)]
		public float M12;

		/// <summary>
		/// Value at row 1 column 3 of the matrix.
		/// </summary>
		[FieldOffset(8)]
		[ProtoMember(7)]
		public float M13;

		/// <summary>
		/// Value at row 2 column 1 of the matrix.
		/// </summary>
		[FieldOffset(12)]
		[ProtoMember(10)]
		public float M21;

		/// <summary>
		/// Value at row 2 column 2 of the matrix.
		/// </summary>
		[FieldOffset(16)]
		[ProtoMember(13)]
		public float M22;

		/// <summary>
		/// Value at row 2 column 3 of the matrix.
		/// </summary>
		[FieldOffset(20)]
		[ProtoMember(16)]
		public float M23;

		/// <summary>
		/// Value at row 3 column 1 of the matrix.
		/// </summary>
		[FieldOffset(24)]
		[ProtoMember(19)]
		public float M31;

		/// <summary>
		/// Value at row 3 column 2 of the matrix.
		/// </summary>
		[FieldOffset(28)]
		[ProtoMember(22)]
		public float M32;

		/// <summary>
		/// Value at row 3 column 3 of the matrix.
		/// </summary>
		[FieldOffset(32)]
		[ProtoMember(25)]
		public float M33;

		/// <summary>
		/// Gets and sets the up vector of the Matrix3x3.
		/// </summary>
		public Vector3 Up
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M21;
				result.Y = M22;
				result.Z = M23;
				return result;
			}
			set
			{
				M21 = value.X;
				M22 = value.Y;
				M23 = value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the down vector of the Matrix3x3.
		/// </summary>
		public Vector3 Down
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = 0f - M21;
				result.Y = 0f - M22;
				result.Z = 0f - M23;
				return result;
			}
			set
			{
				M21 = 0f - value.X;
				M22 = 0f - value.Y;
				M23 = 0f - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the right vector of the Matrix3x3.
		/// </summary>
		public Vector3 Right
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M11;
				result.Y = M12;
				result.Z = M13;
				return result;
			}
			set
			{
				M11 = value.X;
				M12 = value.Y;
				M13 = value.Z;
			}
		}

		public Vector3 Col0
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M11;
				result.Y = M21;
				result.Z = M31;
				return result;
			}
		}

		public Vector3 Col1
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M12;
				result.Y = M22;
				result.Z = M32;
				return result;
			}
		}

		public Vector3 Col2
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M13;
				result.Y = M23;
				result.Z = M33;
				return result;
			}
		}

		/// <summary>
		/// Gets and sets the left vector of the Matrix3x3.
		/// </summary>
		public Vector3 Left
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = 0f - M11;
				result.Y = 0f - M12;
				result.Z = 0f - M13;
				return result;
			}
			set
			{
				M11 = 0f - value.X;
				M12 = 0f - value.Y;
				M13 = 0f - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the forward vector of the Matrix3x3.
		/// </summary>
		public Vector3 Forward
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = 0f - M31;
				result.Y = 0f - M32;
				result.Z = 0f - M33;
				return result;
			}
			set
			{
				M31 = 0f - value.X;
				M32 = 0f - value.Y;
				M33 = 0f - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the backward vector of the Matrix3x3.
		/// </summary>
		public Vector3 Backward
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M31;
				result.Y = M32;
				result.Z = M33;
				return result;
			}
			set
			{
				M31 = value.X;
				M32 = value.Y;
				M33 = value.Z;
			}
		}

		public Vector3 Scale => new Vector3(Right.Length(), Up.Length(), Forward.Length());

		public unsafe float this[int row, int column]
		{
			get
			{
				fixed (float* ptr = &M11)
				{
					return ptr[row * 4 + column];
				}
			}
			set
			{
				fixed (float* ptr = &M11)
				{
					ptr[row * 4 + column] = value;
				}
			}
		}

		/// <summary>
		/// Gets the base vector of the matrix, corresponding to the given direction
		/// </summary>
		public Vector3 GetDirectionVector(Base6Directions.Direction direction)
		{
			switch (direction)
			{
			case Base6Directions.Direction.Forward:
				return Forward;
			case Base6Directions.Direction.Backward:
				return Backward;
			case Base6Directions.Direction.Left:
				return Left;
			case Base6Directions.Direction.Right:
				return Right;
			case Base6Directions.Direction.Up:
				return Up;
			case Base6Directions.Direction.Down:
				return Down;
			default:
				return Vector3.Zero;
			}
		}

		/// <summary>
		/// Sets the base vector of the matrix, corresponding to the given direction
		/// </summary>
		public void SetDirectionVector(Base6Directions.Direction direction, Vector3 newValue)
		{
			switch (direction)
			{
			case Base6Directions.Direction.Forward:
				Forward = newValue;
				break;
			case Base6Directions.Direction.Backward:
				Backward = newValue;
				break;
			case Base6Directions.Direction.Left:
				Left = newValue;
				break;
			case Base6Directions.Direction.Right:
				Right = newValue;
				break;
			case Base6Directions.Direction.Up:
				Up = newValue;
				break;
			case Base6Directions.Direction.Down:
				Down = newValue;
				break;
			}
		}

		public Base6Directions.Direction GetClosestDirection(Vector3 referenceVector)
		{
			return GetClosestDirection(ref referenceVector);
		}

		public Base6Directions.Direction GetClosestDirection(ref Vector3 referenceVector)
		{
			float num = Vector3.Dot(referenceVector, Right);
			float num2 = Vector3.Dot(referenceVector, Up);
			float num3 = Vector3.Dot(referenceVector, Backward);
			float num4 = Math.Abs(num);
			float num5 = Math.Abs(num2);
			float num6 = Math.Abs(num3);
			if (num4 > num5)
			{
				if (num4 > num6)
				{
					if (num > 0f)
					{
						return Base6Directions.Direction.Right;
					}
					return Base6Directions.Direction.Left;
				}
				if (num3 > 0f)
				{
					return Base6Directions.Direction.Backward;
				}
				return Base6Directions.Direction.Forward;
			}
			if (num5 > num6)
			{
				if (num2 > 0f)
				{
					return Base6Directions.Direction.Up;
				}
				return Base6Directions.Direction.Down;
			}
			if (num3 > 0f)
			{
				return Base6Directions.Direction.Backward;
			}
			return Base6Directions.Direction.Forward;
		}

		/// <summary>
		/// Same result as Matrix3x3.CreateScale(scale) * matrix, but much faster
		/// </summary>
		public static void Rescale(ref Matrix3x3 matrix, float scale)
		{
			matrix.M11 *= scale;
			matrix.M12 *= scale;
			matrix.M13 *= scale;
			matrix.M21 *= scale;
			matrix.M22 *= scale;
			matrix.M23 *= scale;
			matrix.M31 *= scale;
			matrix.M32 *= scale;
			matrix.M33 *= scale;
		}

		/// <summary>
		/// Same result as Matrix3x3.CreateScale(scale) * matrix, but much faster
		/// </summary>
		public static void Rescale(ref Matrix3x3 matrix, ref Vector3 scale)
		{
			matrix.M11 *= scale.X;
			matrix.M12 *= scale.X;
			matrix.M13 *= scale.X;
			matrix.M21 *= scale.Y;
			matrix.M22 *= scale.Y;
			matrix.M23 *= scale.Y;
			matrix.M31 *= scale.Z;
			matrix.M32 *= scale.Z;
			matrix.M33 *= scale.Z;
		}

		public static Matrix3x3 Rescale(Matrix3x3 matrix, float scale)
		{
			Rescale(ref matrix, scale);
			return matrix;
		}

		public static Matrix3x3 Rescale(Matrix3x3 matrix, Vector3 scale)
		{
			Rescale(ref matrix, ref scale);
			return matrix;
		}

		static Matrix3x3()
		{
			Identity = new Matrix3x3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
			Zero = new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
		}

		/// <summary>
		/// Initializes a new instance of Matrix3x3.
		/// </summary>
		/// <param name="m11">Value to initialize m11 to.</param><param name="m12">Value to initialize m12 to.</param><param name="m13">Value to initialize m13 to.</param><param name="m14">Value to initialize m14 to.</param><param name="m21">Value to initialize m21 to.</param><param name="m22">Value to initialize m22 to.</param><param name="m23">Value to initialize m23 to.</param><param name="m24">Value to initialize m24 to.</param><param name="m31">Value to initialize m31 to.</param><param name="m32">Value to initialize m32 to.</param><param name="m33">Value to initialize m33 to.</param><param name="m34">Value to initialize m34 to.</param><param name="m41">Value to initialize m41 to.</param><param name="m42">Value to initialize m42 to.</param><param name="m43">Value to initialize m43 to.</param><param name="m44">Value to initialize m44 to.</param>
		public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M31 = m31;
			M32 = m32;
			M33 = m33;
		}

		public Matrix3x3(Matrix3x3 other)
		{
			M11 = other.M11;
			M12 = other.M12;
			M13 = other.M13;
			M21 = other.M21;
			M22 = other.M22;
			M23 = other.M23;
			M31 = other.M31;
			M32 = other.M32;
			M33 = other.M33;
		}

		public Matrix3x3(MatrixD other)
		{
			M11 = (float)other.M11;
			M12 = (float)other.M12;
			M13 = (float)other.M13;
			M21 = (float)other.M21;
			M22 = (float)other.M22;
			M23 = (float)other.M23;
			M31 = (float)other.M31;
			M32 = (float)other.M32;
			M33 = (float)other.M33;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param>
		public static Matrix3x3 CreateScale(float xScale, float yScale, float zScale)
		{
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = xScale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = yScale;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = zScale;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param><param name="result">[OutAttribute] The created scaling Matrix3x3.</param>
		public static void CreateScale(float xScale, float yScale, float zScale, out Matrix3x3 result)
		{
			result.M11 = xScale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = yScale;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = zScale;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
		public static Matrix3x3 CreateScale(Vector3 scales)
		{
			float x = scales.X;
			float y = scales.Y;
			float z = scales.Z;
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = x;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = y;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = z;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param><param name="result">[OutAttribute] The created scaling Matrix3x3.</param>
		public static void CreateScale(ref Vector3 scales, out Matrix3x3 result)
		{
			float x = scales.X;
			float y = scales.Y;
			float z = scales.Z;
			result.M11 = x;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = y;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = z;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="scale">Amount to scale by.</param>
		public static Matrix3x3 CreateScale(float scale)
		{
			Matrix3x3 result = default(Matrix3x3);
			float num = result.M11 = scale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = num;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix3x3.
		/// </summary>
		/// <param name="scale">Value to scale by.</param><param name="result">[OutAttribute] The created scaling Matrix3x3.</param>
		public static void CreateScale(float scale, out Matrix3x3 result)
		{
			float num = result.M11 = scale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = num;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix3x3 CreateRotationX(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = num2;
			result.M31 = 0f;
			result.M32 = 0f - num2;
			result.M33 = num;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationX(float radians, out Matrix3x3 result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = num2;
			result.M31 = 0f;
			result.M32 = 0f - num2;
			result.M33 = num;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix3x3 CreateRotationY(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = num;
			result.M12 = 0f;
			result.M13 = 0f - num2;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M31 = num2;
			result.M32 = 0f;
			result.M33 = num;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationY(float radians, out Matrix3x3 result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = num;
			result.M12 = 0f;
			result.M13 = 0f - num2;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M31 = num2;
			result.M32 = 0f;
			result.M33 = num;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix3x3 CreateRotationZ(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0f;
			result.M21 = 0f - num2;
			result.M22 = num;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The rotation matrix.</param>
		public static void CreateRotationZ(float radians, out Matrix3x3 result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0f;
			result.M21 = 0f - num2;
			result.M22 = num;
			result.M23 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
		}

		/// <summary>
		/// Creates a new Matrix3x3 that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param>
		public static Matrix3x3 CreateFromAxisAngle(Vector3 axis, float angle)
		{
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num = (float)Math.Sin(angle);
			float num2 = (float)Math.Cos(angle);
			float num3 = x * x;
			float num4 = y * y;
			float num5 = z * z;
			float num6 = x * y;
			float num7 = x * z;
			float num8 = y * z;
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = num3 + num2 * (1f - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
			return result;
		}

		/// <summary>
		/// Creates a new Matrix3x3 that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param><param name="result">[OutAttribute] The created Matrix3x3.</param>
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix3x3 result)
		{
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num = (float)Math.Sin(angle);
			float num2 = (float)Math.Cos(angle);
			float num3 = x * x;
			float num4 = y * y;
			float num5 = z * z;
			float num6 = x * y;
			float num7 = x * z;
			float num8 = y * z;
			result.M11 = num3 + num2 * (1f - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
		}

		public static void CreateRotationFromTwoVectors(ref Vector3 fromVector, ref Vector3 toVector, out Matrix3x3 resultMatrix)
		{
			Vector3 vector = Vector3.Normalize(fromVector);
			Vector3 vector2 = Vector3.Normalize(toVector);
			Vector3.Cross(ref vector, ref vector2, out Vector3 result);
			result.Normalize();
			Vector3.Cross(ref vector, ref result, out Vector3 result2);
			Matrix3x3 matrix = new Matrix3x3(vector.X, result.X, result2.X, vector.Y, result.Y, result2.Y, vector.Z, result.Z, result2.Z);
			Vector3.Cross(ref vector2, ref result, out result2);
			Matrix3x3 matrix2 = new Matrix3x3(vector2.X, vector2.Y, vector2.Z, result.X, result.Y, result.Z, result2.X, result2.Y, result2.Z);
			Multiply(ref matrix, ref matrix2, out resultMatrix);
		}

		/// <summary>
		/// Creates a rotation Matrix3x3 from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix3x3 from.</param>
		public static Matrix3x3 CreateFromQuaternion(Quaternion quaternion)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			Matrix3x3 result = default(Matrix3x3);
			result.M11 = (float)(1.0 - 2.0 * (double)(num2 + num3));
			result.M12 = (float)(2.0 * (double)(num4 + num5));
			result.M13 = (float)(2.0 * (double)(num6 - num7));
			result.M21 = (float)(2.0 * (double)(num4 - num5));
			result.M22 = (float)(1.0 - 2.0 * (double)(num3 + num));
			result.M23 = (float)(2.0 * (double)(num8 + num9));
			result.M31 = (float)(2.0 * (double)(num6 + num7));
			result.M32 = (float)(2.0 * (double)(num8 - num9));
			result.M33 = (float)(1.0 - 2.0 * (double)(num2 + num));
			return result;
		}

		/// <summary>
		/// Creates a rotation Matrix3x3 from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix3x3 from.</param><param name="result">[OutAttribute] The created Matrix3x3.</param>
		public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix3x3 result)
		{
			float num = quaternion.X * quaternion.X;
			float num2 = quaternion.Y * quaternion.Y;
			float num3 = quaternion.Z * quaternion.Z;
			float num4 = quaternion.X * quaternion.Y;
			float num5 = quaternion.Z * quaternion.W;
			float num6 = quaternion.Z * quaternion.X;
			float num7 = quaternion.Y * quaternion.W;
			float num8 = quaternion.Y * quaternion.Z;
			float num9 = quaternion.X * quaternion.W;
			result.M11 = (float)(1.0 - 2.0 * (double)(num2 + num3));
			result.M12 = (float)(2.0 * (double)(num4 + num5));
			result.M13 = (float)(2.0 * (double)(num6 - num7));
			result.M21 = (float)(2.0 * (double)(num4 - num5));
			result.M22 = (float)(1.0 - 2.0 * (double)(num3 + num));
			result.M23 = (float)(2.0 * (double)(num8 + num9));
			result.M31 = (float)(2.0 * (double)(num6 + num7));
			result.M32 = (float)(2.0 * (double)(num8 - num9));
			result.M33 = (float)(1.0 - 2.0 * (double)(num2 + num));
		}

		/// <summary>
		/// Creates a new rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param>
		public static Matrix3x3 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Quaternion result);
			CreateFromQuaternion(ref result, out Matrix3x3 result2);
			return result2;
		}

		/// <summary>
		/// Fills in a rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param><param name="result">[OutAttribute] An existing matrix filled in to represent the specified yaw, pitch, and roll.</param>
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Matrix3x3 result)
		{
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Quaternion result2);
			CreateFromQuaternion(ref result2, out result);
		}

		/// <summary>
		/// Transforms a Matrix3x3 by applying a Quaternion rotation.
		/// </summary>
		/// <param name="value">The Matrix3x3 to transform.</param><param name="rotation">The rotation to apply, expressed as a Quaternion.</param><param name="result">[OutAttribute] An existing Matrix3x3 filled in with the result of the transform.</param>
		public static void Transform(ref Matrix3x3 value, ref Quaternion rotation, out Matrix3x3 result)
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
			float m = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
			float m2 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
			float m3 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
			float m4 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
			float m5 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
			float m6 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
			float m7 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
			float m8 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
			float m9 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
			result.M11 = m;
			result.M12 = m2;
			result.M13 = m3;
			result.M21 = m4;
			result.M22 = m5;
			result.M23 = m6;
			result.M31 = m7;
			result.M32 = m8;
			result.M33 = m9;
		}

		public unsafe Vector3 GetRow(int row)
		{
			fixed (float* ptr = &M11)
			{
				float* ptr2 = ptr + row * 4;
				return new Vector3(*ptr2, ptr2[1], ptr2[2]);
			}
		}

		public unsafe void SetRow(int row, Vector3 value)
		{
			fixed (float* ptr = &M11)
			{
				float* intPtr = ptr + row * 4;
				*intPtr = value.X;
				intPtr[1] = value.Y;
				intPtr[2] = value.Z;
			}
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return "{ " + string.Format(currentCulture, "{{M11:{0} M12:{1} M13:{2}}} ", M11.ToString(currentCulture), M12.ToString(currentCulture), string.Concat((object)M13.ToString(currentCulture), (object)string.Format(currentCulture, "{{M21:{0} M22:{1} M23:{2}}} ", M21.ToString(currentCulture), M22.ToString(currentCulture), string.Concat((object)M23.ToString(currentCulture), (object)string.Format(currentCulture, "{{M31:{0} M32:{1} M33:{2}}} ", M31.ToString(currentCulture), M32.ToString(currentCulture), M33.ToString(currentCulture)))))) + "}";
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Matrix3x3.
		/// </summary>
		/// <param name="other">The Object to compare with the current Matrix3x3.</param>
		public bool Equals(Matrix3x3 other)
		{
			if (M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M12 == other.M12 && M13 == other.M13 && M21 == other.M21 && M23 == other.M23 && M31 == other.M31)
			{
				return M32 == other.M32;
			}
			return false;
		}

		/// <summary>
		/// Compares just position, forward and up
		/// </summary>
		public bool EqualsFast(ref Matrix3x3 other, float epsilon = 0.0001f)
		{
			float num = M21 - other.M21;
			float num2 = M22 - other.M22;
			float num3 = M23 - other.M23;
			float num4 = M31 - other.M31;
			float num5 = M32 - other.M32;
			float num6 = M33 - other.M33;
			float num7 = epsilon * epsilon;
			return num * num + num2 * num2 + num3 * num3 < num7 && num4 * num4 + num5 * num5 + num6 * num6 < num7;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Matrix3x3)
			{
				result = Equals((Matrix3x3)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code of this object.
		/// </summary>
		public override int GetHashCode()
		{
			return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode();
		}

		/// <summary>
		/// Transposes the rows and columns of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="result">[OutAttribute] Transposed matrix.</param>
		public static void Transpose(ref Matrix3x3 matrix, out Matrix3x3 result)
		{
			float m = matrix.M11;
			float m2 = matrix.M12;
			float m3 = matrix.M13;
			float m4 = matrix.M21;
			float m5 = matrix.M22;
			float m6 = matrix.M23;
			float m7 = matrix.M31;
			float m8 = matrix.M32;
			float m9 = matrix.M33;
			result.M11 = m;
			result.M12 = m4;
			result.M13 = m7;
			result.M21 = m2;
			result.M22 = m5;
			result.M23 = m8;
			result.M31 = m3;
			result.M32 = m6;
			result.M33 = m9;
		}

		public void Transpose()
		{
			float m = M12;
			float m2 = M13;
			float m3 = M21;
			float m4 = M23;
			float m5 = M31;
			float m6 = M32;
			M12 = m3;
			M13 = m5;
			M21 = m;
			M23 = m6;
			M31 = m2;
			M32 = m4;
		}

		public float Determinant()
		{
			return M11 * (M22 * M33 - M32 * M23) - M12 * (M21 * M33 - M23 * M31) + M13 * (M21 * M32 - M22 * M31);
		}

		/// <summary>
		/// Calculates the inverse of a matrix.
		/// </summary>
		/// <param name="matrix">The source matrix.</param><param name="result">[OutAttribute] The inverse of matrix. The same matrix can be used for both arguments.</param>
		public static void Invert(ref Matrix3x3 matrix, out Matrix3x3 result)
		{
			float num = matrix.Determinant();
			float num2 = 1f / num;
			result.M11 = (matrix.M22 * matrix.M33 - matrix.M32 * matrix.M23) * num2;
			result.M12 = (matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33) * num2;
			result.M13 = (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22) * num2;
			result.M21 = (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33) * num2;
			result.M22 = (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31) * num2;
			result.M23 = (matrix.M21 * matrix.M13 - matrix.M11 * matrix.M23) * num2;
			result.M31 = (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22) * num2;
			result.M32 = (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32) * num2;
			result.M33 = (matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12) * num2;
		}

		/// <summary>
		/// Linearly interpolates between the corresponding values of two matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="amount">Interpolation value.</param><param name="result">[OutAttribute] Resulting matrix.</param>
		public static void Lerp(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, float amount, out Matrix3x3 result)
		{
			result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
			result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
			result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
			result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
			result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
			result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
			result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
			result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
			result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static void Slerp(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, float amount, out Matrix3x3 result)
		{
			Quaternion.CreateFromRotationMatrix(ref matrix1, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix2, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static void SlerpScale(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, float amount, out Matrix3x3 result)
		{
			Vector3 scale = matrix1.Scale;
			Vector3 scale2 = matrix2.Scale;
			if (scale.LengthSquared() < 1E-06f || scale2.LengthSquared() < 1E-06f)
			{
				result = Zero;
				return;
			}
			Matrix3x3 matrix3 = Normalize(matrix1);
			Matrix3x3 matrix4 = Normalize(matrix2);
			Quaternion.CreateFromRotationMatrix(ref matrix3, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix4, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
			Vector3 scale3 = Vector3.Lerp(scale, scale2, amount);
			Rescale(ref result, ref scale3);
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="result">[OutAttribute] Negated matrix.</param>
		public static void Negate(ref Matrix3x3 matrix, out Matrix3x3 result)
		{
			result.M11 = 0f - matrix.M11;
			result.M12 = 0f - matrix.M12;
			result.M13 = 0f - matrix.M13;
			result.M21 = 0f - matrix.M21;
			result.M22 = 0f - matrix.M22;
			result.M23 = 0f - matrix.M23;
			result.M31 = 0f - matrix.M31;
			result.M32 = 0f - matrix.M32;
			result.M33 = 0f - matrix.M33;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Resulting matrix.</param>
		public static void Add(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
		{
			result.M11 = matrix1.M11 + matrix2.M11;
			result.M12 = matrix1.M12 + matrix2.M12;
			result.M13 = matrix1.M13 + matrix2.M13;
			result.M21 = matrix1.M21 + matrix2.M21;
			result.M22 = matrix1.M22 + matrix2.M22;
			result.M23 = matrix1.M23 + matrix2.M23;
			result.M31 = matrix1.M31 + matrix2.M31;
			result.M32 = matrix1.M32 + matrix2.M32;
			result.M33 = matrix1.M33 + matrix2.M33;
		}

		/// <summary>
		/// Subtracts matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the subtraction.</param>
		public static void Subtract(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
		{
			result.M11 = matrix1.M11 - matrix2.M11;
			result.M12 = matrix1.M12 - matrix2.M12;
			result.M13 = matrix1.M13 - matrix2.M13;
			result.M21 = matrix1.M21 - matrix2.M21;
			result.M22 = matrix1.M22 - matrix2.M22;
			result.M23 = matrix1.M23 - matrix2.M23;
			result.M31 = matrix1.M31 - matrix2.M31;
			result.M32 = matrix1.M32 - matrix2.M32;
			result.M33 = matrix1.M33 - matrix2.M33;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the multiplication.</param>
		public static void Multiply(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
		{
			float m = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31;
			float m2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32;
			float m3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33;
			float m4 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31;
			float m5 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32;
			float m6 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33;
			float m7 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31;
			float m8 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32;
			float m9 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33;
			result.M11 = m;
			result.M12 = m2;
			result.M13 = m3;
			result.M21 = m4;
			result.M22 = m5;
			result.M23 = m6;
			result.M31 = m7;
			result.M32 = m8;
			result.M33 = m9;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Matrix3x3 matrix1, float scaleFactor, out Matrix3x3 result)
		{
			result.M11 = matrix1.M11 * scaleFactor;
			result.M12 = matrix1.M12 * scaleFactor;
			result.M13 = matrix1.M13 * scaleFactor;
			result.M21 = matrix1.M21 * scaleFactor;
			result.M22 = matrix1.M22 * scaleFactor;
			result.M23 = matrix1.M23 * scaleFactor;
			result.M31 = matrix1.M31 * scaleFactor;
			result.M32 = matrix1.M32 * scaleFactor;
			result.M33 = matrix1.M33 * scaleFactor;
		}

		/// <summary>
		/// Divides the components of a matrix by the corresponding components of another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
		{
			result.M11 = matrix1.M11 / matrix2.M11;
			result.M12 = matrix1.M12 / matrix2.M12;
			result.M13 = matrix1.M13 / matrix2.M13;
			result.M21 = matrix1.M21 / matrix2.M21;
			result.M22 = matrix1.M22 / matrix2.M22;
			result.M23 = matrix1.M23 / matrix2.M23;
			result.M31 = matrix1.M31 / matrix2.M31;
			result.M32 = matrix1.M32 / matrix2.M32;
			result.M33 = matrix1.M33 / matrix2.M33;
		}

		/// <summary>
		/// Divides the components of a matrix by a scalar.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="divider">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref Matrix3x3 matrix1, float divider, out Matrix3x3 result)
		{
			float num = 1f / divider;
			result.M11 = matrix1.M11 * num;
			result.M12 = matrix1.M12 * num;
			result.M13 = matrix1.M13 * num;
			result.M21 = matrix1.M21 * num;
			result.M22 = matrix1.M22 * num;
			result.M23 = matrix1.M23 * num;
			result.M31 = matrix1.M31 * num;
			result.M32 = matrix1.M32 * num;
			result.M33 = matrix1.M33 * num;
		}

		/// <summary>
		/// Gets the orientation.
		/// </summary>
		/// <returns></returns>
		public Matrix3x3 GetOrientation()
		{
			Matrix3x3 identity = Identity;
			identity.Forward = Forward;
			identity.Up = Up;
			identity.Right = Right;
			return identity;
		}

		[Conditional("DEBUG")]
		public void AssertIsValid()
		{
		}

		public bool IsValid()
		{
			return (M11 + M12 + M13 + M21 + M22 + M23 + M31 + M32 + M33).IsValid();
		}

		public bool IsNan()
		{
			return float.IsNaN(M11 + M12 + M13 + M21 + M22 + M23 + M31 + M32 + M33);
		}

		public bool IsRotation()
		{
			float num = 0.01f;
			if (Math.Abs(Right.Dot(Up)) > num)
			{
				return false;
			}
			if (Math.Abs(Right.Dot(Backward)) > num)
			{
				return false;
			}
			if (Math.Abs(Up.Dot(Backward)) > num)
			{
				return false;
			}
			if (Math.Abs(Right.LengthSquared() - 1f) > num)
			{
				return false;
			}
			if (Math.Abs(Up.LengthSquared() - 1f) > num)
			{
				return false;
			}
			if (Math.Abs(Backward.LengthSquared() - 1f) > num)
			{
				return false;
			}
			return true;
		}

		public static Matrix3x3 CreateFromDir(Vector3 dir)
		{
			Vector3 value = new Vector3(0f, 0f, 1f);
			float z = dir.Z;
			Vector3 up;
			if (!((double)z > -0.99999) || !((double)z < 0.99999))
			{
				value = new Vector3(dir.Z, 0f, 0f - dir.X);
				up = new Vector3(0f, 1f, 0f);
			}
			else
			{
				value -= dir * z;
				value = Vector3.Normalize(value);
				up = Vector3.Cross(dir, value);
			}
			Matrix3x3 identity = Identity;
			identity.Right = value;
			identity.Up = up;
			identity.Forward = dir;
			return identity;
		}

		/// <summary>
		/// Creates a world matrix with the specified parameters.
		/// </summary>
		/// <param name="forward">Forward direction of the object.</param><param name="up">Upward direction of the object; usually [0, 1, 0].</param>
		public static Matrix3x3 CreateWorld(ref Vector3 forward, ref Vector3 up)
		{
			Vector3.Normalize(ref forward, out Vector3 result);
			result = -result;
			Vector3.Cross(ref up, ref result, out Vector3 result2);
			Vector3.Normalize(ref result2, out Vector3 result3);
			Vector3.Cross(ref result, ref result3, out Vector3 result4);
			Matrix3x3 result5 = default(Matrix3x3);
			result5.M11 = result3.X;
			result5.M12 = result3.Y;
			result5.M13 = result3.Z;
			result5.M21 = result4.X;
			result5.M22 = result4.Y;
			result5.M23 = result4.Z;
			result5.M31 = result.X;
			result5.M32 = result.Y;
			result5.M33 = result.Z;
			return result5;
		}

		public static Matrix3x3 CreateFromDir(Vector3 dir, Vector3 suggestedUp)
		{
			Vector3 up = Vector3.Cross(Vector3.Cross(dir, suggestedUp), dir);
			return CreateWorld(ref dir, ref up);
		}

		public static Matrix3x3 Normalize(Matrix3x3 matrix)
		{
			Matrix3x3 result = matrix;
			result.Right = Vector3.Normalize(result.Right);
			result.Up = Vector3.Normalize(result.Up);
			result.Forward = Vector3.Normalize(result.Forward);
			return result;
		}

		public static Matrix3x3 Orthogonalize(Matrix3x3 rotationMatrix)
		{
			Matrix3x3 result = rotationMatrix;
			result.Right = Vector3.Normalize(result.Right);
			result.Up = Vector3.Normalize(result.Up - result.Right * result.Up.Dot(result.Right));
			result.Backward = Vector3.Normalize(result.Backward - result.Right * result.Backward.Dot(result.Right) - result.Up * result.Backward.Dot(result.Up));
			return result;
		}

		public static Matrix3x3 Round(ref Matrix3x3 matrix)
		{
			Matrix3x3 result = matrix;
			result.Right = Vector3I.Round(result.Right);
			result.Up = Vector3I.Round(result.Up);
			result.Forward = Vector3I.Round(result.Forward);
			return result;
		}

		public static Matrix3x3 AlignRotationToAxes(ref Matrix3x3 toAlign, ref Matrix3x3 axisDefinitionMatrix)
		{
			Matrix3x3 identity = Identity;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			float num = toAlign.Right.Dot(axisDefinitionMatrix.Right);
			float num2 = toAlign.Right.Dot(axisDefinitionMatrix.Up);
			float num3 = toAlign.Right.Dot(axisDefinitionMatrix.Backward);
			if (Math.Abs(num) > Math.Abs(num2))
			{
				if (Math.Abs(num) > Math.Abs(num3))
				{
					identity.Right = ((num > 0f) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
					flag = true;
				}
				else
				{
					identity.Right = ((num3 > 0f) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
					flag3 = true;
				}
			}
			else if (Math.Abs(num2) > Math.Abs(num3))
			{
				identity.Right = ((num2 > 0f) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
				flag2 = true;
			}
			else
			{
				identity.Right = ((num3 > 0f) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
				flag3 = true;
			}
			num = toAlign.Up.Dot(axisDefinitionMatrix.Right);
			num2 = toAlign.Up.Dot(axisDefinitionMatrix.Up);
			num3 = toAlign.Up.Dot(axisDefinitionMatrix.Backward);
			if (flag2 || (Math.Abs(num) > Math.Abs(num2) && !flag))
			{
				if (Math.Abs(num) > Math.Abs(num3) || flag3)
				{
					identity.Up = ((num > 0f) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
					flag = true;
				}
				else
				{
					identity.Up = ((num3 > 0f) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
					flag3 = true;
				}
			}
			else if (Math.Abs(num2) > Math.Abs(num3) || flag3)
			{
				identity.Up = ((num2 > 0f) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
				flag2 = true;
			}
			else
			{
				identity.Up = ((num3 > 0f) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
				flag3 = true;
			}
			if (!flag)
			{
				num = toAlign.Backward.Dot(axisDefinitionMatrix.Right);
				identity.Backward = ((num > 0f) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
			}
			else if (!flag2)
			{
				num2 = toAlign.Backward.Dot(axisDefinitionMatrix.Up);
				identity.Backward = ((num2 > 0f) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
			}
			else
			{
				num3 = toAlign.Backward.Dot(axisDefinitionMatrix.Backward);
				identity.Backward = ((num3 > 0f) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
			}
			return identity;
		}

		public static bool GetEulerAnglesXYZ(ref Matrix3x3 mat, out Vector3 xyz)
		{
			float x = mat.GetRow(0).X;
			float y = mat.GetRow(0).Y;
			float z = mat.GetRow(0).Z;
			float x2 = mat.GetRow(1).X;
			float y2 = mat.GetRow(1).Y;
			float z2 = mat.GetRow(1).Z;
			mat.GetRow(2);
			mat.GetRow(2);
			float z3 = mat.GetRow(2).Z;
			float num = z;
			if (num < 1f)
			{
				if (num > -1f)
				{
					xyz = new Vector3((float)Math.Atan2(0f - z2, z3), (float)Math.Asin(z), (float)Math.Atan2(0f - y, x));
					return true;
				}
				xyz = new Vector3((float)(0.0 - Math.Atan2(x2, y2)), MathF.E * -449f / 777f, 0f);
				return false;
			}
			xyz = new Vector3((float)Math.Atan2(x2, y2), MathF.E * -449f / 777f, 0f);
			return false;
		}

		public bool IsMirrored()
		{
			return Determinant() < 0f;
		}

		public bool IsOrthogonal()
		{
			if (Math.Abs(Up.LengthSquared()) - 1f < 0.0001f && Math.Abs(Right.LengthSquared()) - 1f < 0.0001f && Math.Abs(Forward.LengthSquared()) - 1f < 0.0001f && Math.Abs(Right.Dot(Up)) < 0.0001f)
			{
				return Math.Abs(Right.Dot(Forward)) < 0.0001f;
			}
			return false;
		}
	}
}
