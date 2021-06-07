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
	public struct Matrix : IEquatable<Matrix>
	{
		private struct F16
		{
			public unsafe fixed float data[16];
		}

		protected class VRageMath_Matrix_003C_003EM_003C_003EAccessor : IMemberAccessor<Matrix, F16>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in F16 value)
			{
				owner.M = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out F16 value)
			{
				value = owner.M;
			}
		}

		protected class VRageMath_Matrix_003C_003EM11_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M11 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M11;
			}
		}

		protected class VRageMath_Matrix_003C_003EM12_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M12 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M12;
			}
		}

		protected class VRageMath_Matrix_003C_003EM13_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M13 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M13;
			}
		}

		protected class VRageMath_Matrix_003C_003EM14_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M14 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M14;
			}
		}

		protected class VRageMath_Matrix_003C_003EM21_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M21 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M21;
			}
		}

		protected class VRageMath_Matrix_003C_003EM22_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M22 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M22;
			}
		}

		protected class VRageMath_Matrix_003C_003EM23_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M23 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M23;
			}
		}

		protected class VRageMath_Matrix_003C_003EM24_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M24 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M24;
			}
		}

		protected class VRageMath_Matrix_003C_003EM31_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M31 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M31;
			}
		}

		protected class VRageMath_Matrix_003C_003EM32_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M32 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M32;
			}
		}

		protected class VRageMath_Matrix_003C_003EM33_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M33 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M33;
			}
		}

		protected class VRageMath_Matrix_003C_003EM34_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M34 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M34;
			}
		}

		protected class VRageMath_Matrix_003C_003EM41_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M41 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M41;
			}
		}

		protected class VRageMath_Matrix_003C_003EM42_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M42 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M42;
			}
		}

		protected class VRageMath_Matrix_003C_003EM43_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M43 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M43;
			}
		}

		protected class VRageMath_Matrix_003C_003EM44_003C_003EAccessor : IMemberAccessor<Matrix, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in float value)
			{
				owner.M44 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out float value)
			{
				value = owner.M44;
			}
		}

		protected class VRageMath_Matrix_003C_003EUp_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Up;
			}
		}

		protected class VRageMath_Matrix_003C_003EDown_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Down = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Down;
			}
		}

		protected class VRageMath_Matrix_003C_003ERight_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Right = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Right;
			}
		}

		protected class VRageMath_Matrix_003C_003ELeft_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Left = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Left;
			}
		}

		protected class VRageMath_Matrix_003C_003EForward_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Forward;
			}
		}

		protected class VRageMath_Matrix_003C_003EBackward_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Backward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Backward;
			}
		}

		protected class VRageMath_Matrix_003C_003ETranslation_003C_003EAccessor : IMemberAccessor<Matrix, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Matrix owner, in Vector3 value)
			{
				owner.Translation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Matrix owner, out Vector3 value)
			{
				value = owner.Translation;
			}
		}

		public static Matrix Identity;

		public static Matrix Zero;

		/// <summary>
		/// Matrix values
		/// </summary>
		[FieldOffset(0)]
		private F16 M;

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
		/// Value at row 1 column 4 of the matrix.
		/// </summary>
		[FieldOffset(12)]
		[ProtoMember(10)]
		public float M14;

		/// <summary>
		/// Value at row 2 column 1 of the matrix.
		/// </summary>
		[FieldOffset(16)]
		[ProtoMember(13)]
		public float M21;

		/// <summary>
		/// Value at row 2 column 2 of the matrix.
		/// </summary>
		[FieldOffset(20)]
		[ProtoMember(16)]
		public float M22;

		/// <summary>
		/// Value at row 2 column 3 of the matrix.
		/// </summary>
		[FieldOffset(24)]
		[ProtoMember(19)]
		public float M23;

		/// <summary>
		/// Value at row 2 column 4 of the matrix.
		/// </summary>
		[FieldOffset(28)]
		[ProtoMember(22)]
		public float M24;

		/// <summary>
		/// Value at row 3 column 1 of the matrix.
		/// </summary>
		[FieldOffset(32)]
		[ProtoMember(25)]
		public float M31;

		/// <summary>
		/// Value at row 3 column 2 of the matrix.
		/// </summary>
		[FieldOffset(36)]
		[ProtoMember(28)]
		public float M32;

		/// <summary>
		/// Value at row 3 column 3 of the matrix.
		/// </summary>
		[FieldOffset(40)]
		[ProtoMember(31)]
		public float M33;

		/// <summary>
		/// Value at row 3 column 4 of the matrix.
		/// </summary>
		[FieldOffset(44)]
		[ProtoMember(34)]
		public float M34;

		/// <summary>
		/// Value at row 4 column 1 of the matrix.
		/// </summary>
		[FieldOffset(48)]
		[ProtoMember(37)]
		public float M41;

		/// <summary>
		/// Value at row 4 column 2 of the matrix.
		/// </summary>
		[FieldOffset(52)]
		[ProtoMember(40)]
		public float M42;

		/// <summary>
		/// Value at row 4 column 3 of the matrix.
		/// </summary>
		[FieldOffset(56)]
		[ProtoMember(43)]
		public float M43;

		/// <summary>
		/// Value at row 4 column 4 of the matrix.
		/// </summary>
		[FieldOffset(60)]
		[ProtoMember(46)]
		public float M44;

		/// <summary>
		/// Gets and sets the up vector of the Matrix.
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
		/// Gets and sets the down vector of the Matrix.
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
		/// Gets and sets the right vector of the Matrix.
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
		/// Gets and sets the left vector of the Matrix.
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
		/// Gets and sets the forward vector of the Matrix.
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
		/// Gets and sets the backward vector of the Matrix.
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

		/// <summary>
		/// Gets and sets the translation vector of the Matrix.
		/// </summary>
		public Vector3 Translation
		{
			get
			{
				Vector3 result = default(Vector3);
				result.X = M41;
				result.Y = M42;
				result.Z = M43;
				return result;
			}
			set
			{
				M41 = value.X;
				M42 = value.Y;
				M43 = value.Z;
			}
		}

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
		/// Same result as Matrix.CreateScale(scale) * matrix, but much faster
		/// </summary>
		public static void Rescale(ref Matrix matrix, float scale)
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
		/// Same result as Matrix.CreateScale(scale) * matrix, but much faster
		/// </summary>
		public static void Rescale(ref Matrix matrix, ref Vector3 scale)
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

		public static Matrix Rescale(Matrix matrix, float scale)
		{
			Rescale(ref matrix, scale);
			return matrix;
		}

		public static Matrix Rescale(Matrix matrix, Vector3 scale)
		{
			Rescale(ref matrix, ref scale);
			return matrix;
		}

		static Matrix()
		{
			Identity = new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
			Zero = new Matrix(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
		}

		/// <summary>
		/// Initializes a new instance of Matrix.
		/// </summary>
		/// <param name="m11">Value to initialize m11 to.</param><param name="m12">Value to initialize m12 to.</param><param name="m13">Value to initialize m13 to.</param><param name="m14">Value to initialize m14 to.</param><param name="m21">Value to initialize m21 to.</param><param name="m22">Value to initialize m22 to.</param><param name="m23">Value to initialize m23 to.</param><param name="m24">Value to initialize m24 to.</param><param name="m31">Value to initialize m31 to.</param><param name="m32">Value to initialize m32 to.</param><param name="m33">Value to initialize m33 to.</param><param name="m34">Value to initialize m34 to.</param><param name="m41">Value to initialize m41 to.</param><param name="m42">Value to initialize m42 to.</param><param name="m43">Value to initialize m43 to.</param><param name="m44">Value to initialize m44 to.</param>
		public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = m14;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = m24;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = m34;
			M41 = m41;
			M42 = m42;
			M43 = m43;
			M44 = m44;
		}

		/// <summary>
		/// Initializes a new instance of Matrix with rotation data
		/// </summary>
		/// <param name="m11">Value to initialize m11 to.</param><param name="m12">Value to initialize m12 to.</param><param name="m13">Value to initialize m13 to.</param><param name="m14">Value to initialize m14 to.</param><param name="m21">Value to initialize m21 to.</param><param name="m22">Value to initialize m22 to.</param><param name="m23">Value to initialize m23 to.</param><param name="m24">Value to initialize m24 to.</param><param name="m31">Value to initialize m31 to.</param><param name="m32">Value to initialize m32 to.</param><param name="m33">Value to initialize m33 to.</param><param name="m34">Value to initialize m34 to.</param><param name="m41">Value to initialize m41 to.</param><param name="m42">Value to initialize m42 to.</param><param name="m43">Value to initialize m43 to.</param><param name="m44">Value to initialize m44 to.</param>
		public Matrix(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = 0f;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = 0f;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = 0f;
			M41 = 0f;
			M42 = 0f;
			M43 = 0f;
			M44 = 1f;
		}

		public Matrix(MatrixD other)
		{
			M11 = (float)other.M11;
			M12 = (float)other.M12;
			M13 = (float)other.M13;
			M14 = (float)other.M14;
			M21 = (float)other.M21;
			M22 = (float)other.M22;
			M23 = (float)other.M23;
			M24 = (float)other.M24;
			M31 = (float)other.M31;
			M32 = (float)other.M32;
			M33 = (float)other.M33;
			M34 = (float)other.M34;
			M41 = (float)other.M41;
			M42 = (float)other.M42;
			M43 = (float)other.M43;
			M44 = (float)other.M44;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param>
		public static Matrix operator -(Matrix matrix1)
		{
			Matrix result = default(Matrix);
			result.M11 = 0f - matrix1.M11;
			result.M12 = 0f - matrix1.M12;
			result.M13 = 0f - matrix1.M13;
			result.M14 = 0f - matrix1.M14;
			result.M21 = 0f - matrix1.M21;
			result.M22 = 0f - matrix1.M22;
			result.M23 = 0f - matrix1.M23;
			result.M24 = 0f - matrix1.M24;
			result.M31 = 0f - matrix1.M31;
			result.M32 = 0f - matrix1.M32;
			result.M33 = 0f - matrix1.M33;
			result.M34 = 0f - matrix1.M34;
			result.M41 = 0f - matrix1.M41;
			result.M42 = 0f - matrix1.M42;
			result.M43 = 0f - matrix1.M43;
			result.M44 = 0f - matrix1.M44;
			return result;
		}

		/// <summary>
		/// Compares a matrix for equality with another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static bool operator ==(Matrix matrix1, Matrix matrix2)
		{
			if ((double)matrix1.M11 == (double)matrix2.M11 && (double)matrix1.M22 == (double)matrix2.M22 && (double)matrix1.M33 == (double)matrix2.M33 && (double)matrix1.M44 == (double)matrix2.M44 && (double)matrix1.M12 == (double)matrix2.M12 && (double)matrix1.M13 == (double)matrix2.M13 && (double)matrix1.M14 == (double)matrix2.M14 && (double)matrix1.M21 == (double)matrix2.M21 && (double)matrix1.M23 == (double)matrix2.M23 && (double)matrix1.M24 == (double)matrix2.M24 && (double)matrix1.M31 == (double)matrix2.M31 && (double)matrix1.M32 == (double)matrix2.M32 && (double)matrix1.M34 == (double)matrix2.M34 && (double)matrix1.M41 == (double)matrix2.M41 && (double)matrix1.M42 == (double)matrix2.M42)
			{
				return (double)matrix1.M43 == (double)matrix2.M43;
			}
			return false;
		}

		/// <summary>
		/// Tests a matrix for inequality with another matrix.
		/// </summary>
		/// <param name="matrix1">The matrix on the left of the equal sign.</param><param name="matrix2">The matrix on the right of the equal sign.</param>
		public static bool operator !=(Matrix matrix1, Matrix matrix2)
		{
			if ((double)matrix1.M11 == (double)matrix2.M11 && (double)matrix1.M12 == (double)matrix2.M12 && (double)matrix1.M13 == (double)matrix2.M13 && (double)matrix1.M14 == (double)matrix2.M14 && (double)matrix1.M21 == (double)matrix2.M21 && (double)matrix1.M22 == (double)matrix2.M22 && (double)matrix1.M23 == (double)matrix2.M23 && (double)matrix1.M24 == (double)matrix2.M24 && (double)matrix1.M31 == (double)matrix2.M31 && (double)matrix1.M32 == (double)matrix2.M32 && (double)matrix1.M33 == (double)matrix2.M33 && (double)matrix1.M34 == (double)matrix2.M34 && (double)matrix1.M41 == (double)matrix2.M41 && (double)matrix1.M42 == (double)matrix2.M42 && (double)matrix1.M43 == (double)matrix2.M43)
			{
				return (double)matrix1.M44 != (double)matrix2.M44;
			}
			return true;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 + matrix2.M11;
			result.M12 = matrix1.M12 + matrix2.M12;
			result.M13 = matrix1.M13 + matrix2.M13;
			result.M14 = matrix1.M14 + matrix2.M14;
			result.M21 = matrix1.M21 + matrix2.M21;
			result.M22 = matrix1.M22 + matrix2.M22;
			result.M23 = matrix1.M23 + matrix2.M23;
			result.M24 = matrix1.M24 + matrix2.M24;
			result.M31 = matrix1.M31 + matrix2.M31;
			result.M32 = matrix1.M32 + matrix2.M32;
			result.M33 = matrix1.M33 + matrix2.M33;
			result.M34 = matrix1.M34 + matrix2.M34;
			result.M41 = matrix1.M41 + matrix2.M41;
			result.M42 = matrix1.M42 + matrix2.M42;
			result.M43 = matrix1.M43 + matrix2.M43;
			result.M44 = matrix1.M44 + matrix2.M44;
			return result;
		}

		/// <summary>
		/// Subtracts matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix operator -(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 - matrix2.M11;
			result.M12 = matrix1.M12 - matrix2.M12;
			result.M13 = matrix1.M13 - matrix2.M13;
			result.M14 = matrix1.M14 - matrix2.M14;
			result.M21 = matrix1.M21 - matrix2.M21;
			result.M22 = matrix1.M22 - matrix2.M22;
			result.M23 = matrix1.M23 - matrix2.M23;
			result.M24 = matrix1.M24 - matrix2.M24;
			result.M31 = matrix1.M31 - matrix2.M31;
			result.M32 = matrix1.M32 - matrix2.M32;
			result.M33 = matrix1.M33 - matrix2.M33;
			result.M34 = matrix1.M34 - matrix2.M34;
			result.M41 = matrix1.M41 - matrix2.M41;
			result.M42 = matrix1.M42 - matrix2.M42;
			result.M43 = matrix1.M43 - matrix2.M43;
			result.M44 = matrix1.M44 - matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = (float)((double)matrix1.M11 * (double)matrix2.M11 + (double)matrix1.M12 * (double)matrix2.M21 + (double)matrix1.M13 * (double)matrix2.M31 + (double)matrix1.M14 * (double)matrix2.M41);
			result.M12 = (float)((double)matrix1.M11 * (double)matrix2.M12 + (double)matrix1.M12 * (double)matrix2.M22 + (double)matrix1.M13 * (double)matrix2.M32 + (double)matrix1.M14 * (double)matrix2.M42);
			result.M13 = (float)((double)matrix1.M11 * (double)matrix2.M13 + (double)matrix1.M12 * (double)matrix2.M23 + (double)matrix1.M13 * (double)matrix2.M33 + (double)matrix1.M14 * (double)matrix2.M43);
			result.M14 = (float)((double)matrix1.M11 * (double)matrix2.M14 + (double)matrix1.M12 * (double)matrix2.M24 + (double)matrix1.M13 * (double)matrix2.M34 + (double)matrix1.M14 * (double)matrix2.M44);
			result.M21 = (float)((double)matrix1.M21 * (double)matrix2.M11 + (double)matrix1.M22 * (double)matrix2.M21 + (double)matrix1.M23 * (double)matrix2.M31 + (double)matrix1.M24 * (double)matrix2.M41);
			result.M22 = (float)((double)matrix1.M21 * (double)matrix2.M12 + (double)matrix1.M22 * (double)matrix2.M22 + (double)matrix1.M23 * (double)matrix2.M32 + (double)matrix1.M24 * (double)matrix2.M42);
			result.M23 = (float)((double)matrix1.M21 * (double)matrix2.M13 + (double)matrix1.M22 * (double)matrix2.M23 + (double)matrix1.M23 * (double)matrix2.M33 + (double)matrix1.M24 * (double)matrix2.M43);
			result.M24 = (float)((double)matrix1.M21 * (double)matrix2.M14 + (double)matrix1.M22 * (double)matrix2.M24 + (double)matrix1.M23 * (double)matrix2.M34 + (double)matrix1.M24 * (double)matrix2.M44);
			result.M31 = (float)((double)matrix1.M31 * (double)matrix2.M11 + (double)matrix1.M32 * (double)matrix2.M21 + (double)matrix1.M33 * (double)matrix2.M31 + (double)matrix1.M34 * (double)matrix2.M41);
			result.M32 = (float)((double)matrix1.M31 * (double)matrix2.M12 + (double)matrix1.M32 * (double)matrix2.M22 + (double)matrix1.M33 * (double)matrix2.M32 + (double)matrix1.M34 * (double)matrix2.M42);
			result.M33 = (float)((double)matrix1.M31 * (double)matrix2.M13 + (double)matrix1.M32 * (double)matrix2.M23 + (double)matrix1.M33 * (double)matrix2.M33 + (double)matrix1.M34 * (double)matrix2.M43);
			result.M34 = (float)((double)matrix1.M31 * (double)matrix2.M14 + (double)matrix1.M32 * (double)matrix2.M24 + (double)matrix1.M33 * (double)matrix2.M34 + (double)matrix1.M34 * (double)matrix2.M44);
			result.M41 = (float)((double)matrix1.M41 * (double)matrix2.M11 + (double)matrix1.M42 * (double)matrix2.M21 + (double)matrix1.M43 * (double)matrix2.M31 + (double)matrix1.M44 * (double)matrix2.M41);
			result.M42 = (float)((double)matrix1.M41 * (double)matrix2.M12 + (double)matrix1.M42 * (double)matrix2.M22 + (double)matrix1.M43 * (double)matrix2.M32 + (double)matrix1.M44 * (double)matrix2.M42);
			result.M43 = (float)((double)matrix1.M41 * (double)matrix2.M13 + (double)matrix1.M42 * (double)matrix2.M23 + (double)matrix1.M43 * (double)matrix2.M33 + (double)matrix1.M44 * (double)matrix2.M43);
			result.M44 = (float)((double)matrix1.M41 * (double)matrix2.M14 + (double)matrix1.M42 * (double)matrix2.M24 + (double)matrix1.M43 * (double)matrix2.M34 + (double)matrix1.M44 * (double)matrix2.M44);
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="scaleFactor">Scalar value.</param>
		public static Matrix operator *(Matrix matrix, float scaleFactor)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix.M11 * scaleFactor;
			result.M12 = matrix.M12 * scaleFactor;
			result.M13 = matrix.M13 * scaleFactor;
			result.M14 = matrix.M14 * scaleFactor;
			result.M21 = matrix.M21 * scaleFactor;
			result.M22 = matrix.M22 * scaleFactor;
			result.M23 = matrix.M23 * scaleFactor;
			result.M24 = matrix.M24 * scaleFactor;
			result.M31 = matrix.M31 * scaleFactor;
			result.M32 = matrix.M32 * scaleFactor;
			result.M33 = matrix.M33 * scaleFactor;
			result.M34 = matrix.M34 * scaleFactor;
			result.M41 = matrix.M41 * scaleFactor;
			result.M42 = matrix.M42 * scaleFactor;
			result.M43 = matrix.M43 * scaleFactor;
			result.M44 = matrix.M44 * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="scaleFactor">Scalar value.</param><param name="matrix">Source matrix.</param>
		public static Matrix operator *(float scaleFactor, Matrix matrix)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix.M11 * scaleFactor;
			result.M12 = matrix.M12 * scaleFactor;
			result.M13 = matrix.M13 * scaleFactor;
			result.M14 = matrix.M14 * scaleFactor;
			result.M21 = matrix.M21 * scaleFactor;
			result.M22 = matrix.M22 * scaleFactor;
			result.M23 = matrix.M23 * scaleFactor;
			result.M24 = matrix.M24 * scaleFactor;
			result.M31 = matrix.M31 * scaleFactor;
			result.M32 = matrix.M32 * scaleFactor;
			result.M33 = matrix.M33 * scaleFactor;
			result.M34 = matrix.M34 * scaleFactor;
			result.M41 = matrix.M41 * scaleFactor;
			result.M42 = matrix.M42 * scaleFactor;
			result.M43 = matrix.M43 * scaleFactor;
			result.M44 = matrix.M44 * scaleFactor;
			return result;
		}

		/// <summary>
		/// Divides the components of a matrix by the corresponding components of another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">The divisor.</param>
		public static Matrix operator /(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 / matrix2.M11;
			result.M12 = matrix1.M12 / matrix2.M12;
			result.M13 = matrix1.M13 / matrix2.M13;
			result.M14 = matrix1.M14 / matrix2.M14;
			result.M21 = matrix1.M21 / matrix2.M21;
			result.M22 = matrix1.M22 / matrix2.M22;
			result.M23 = matrix1.M23 / matrix2.M23;
			result.M24 = matrix1.M24 / matrix2.M24;
			result.M31 = matrix1.M31 / matrix2.M31;
			result.M32 = matrix1.M32 / matrix2.M32;
			result.M33 = matrix1.M33 / matrix2.M33;
			result.M34 = matrix1.M34 / matrix2.M34;
			result.M41 = matrix1.M41 / matrix2.M41;
			result.M42 = matrix1.M42 / matrix2.M42;
			result.M43 = matrix1.M43 / matrix2.M43;
			result.M44 = matrix1.M44 / matrix2.M44;
			return result;
		}

		/// <summary>
		/// Divides the components of a matrix by a scalar.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="divider">The divisor.</param>
		public static Matrix operator /(Matrix matrix1, float divider)
		{
			float num = 1f / divider;
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 * num;
			result.M12 = matrix1.M12 * num;
			result.M13 = matrix1.M13 * num;
			result.M14 = matrix1.M14 * num;
			result.M21 = matrix1.M21 * num;
			result.M22 = matrix1.M22 * num;
			result.M23 = matrix1.M23 * num;
			result.M24 = matrix1.M24 * num;
			result.M31 = matrix1.M31 * num;
			result.M32 = matrix1.M32 * num;
			result.M33 = matrix1.M33 * num;
			result.M34 = matrix1.M34 * num;
			result.M41 = matrix1.M41 * num;
			result.M42 = matrix1.M42 * num;
			result.M43 = matrix1.M43 * num;
			result.M44 = matrix1.M44 * num;
			return result;
		}

		/// <summary>
		/// Creates a spherical billboard that rotates around a specified object position.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="cameraUpVector">The up vector of the camera.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param>
		public static Matrix CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3? cameraForwardVector)
		{
			Vector3 vector = default(Vector3);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			if ((double)num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt(num), out vector);
			}
			Vector3.Cross(ref cameraUpVector, ref vector, out Vector3 result);
			result.Normalize();
			Vector3.Cross(ref vector, ref result, out Vector3 result2);
			Matrix result3 = default(Matrix);
			result3.M11 = result.X;
			result3.M12 = result.Y;
			result3.M13 = result.Z;
			result3.M14 = 0f;
			result3.M21 = result2.X;
			result3.M22 = result2.Y;
			result3.M23 = result2.Z;
			result3.M24 = 0f;
			result3.M31 = vector.X;
			result3.M32 = vector.Y;
			result3.M33 = vector.Z;
			result3.M34 = 0f;
			result3.M41 = objectPosition.X;
			result3.M42 = objectPosition.Y;
			result3.M43 = objectPosition.Z;
			result3.M44 = 1f;
			return result3;
		}

		/// <summary>
		/// Creates a spherical billboard that rotates around a specified object position.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="cameraUpVector">The up vector of the camera.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="result">[OutAttribute] The created billboard matrix.</param>
		public static void CreateBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, Vector3? cameraForwardVector, out Matrix result)
		{
			Vector3 vector = default(Vector3);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			if ((double)num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt(num), out vector);
			}
			Vector3.Cross(ref cameraUpVector, ref vector, out Vector3 result2);
			result2.Normalize();
			Vector3.Cross(ref vector, ref result2, out Vector3 result3);
			result.M11 = result2.X;
			result.M12 = result2.Y;
			result.M13 = result2.Z;
			result.M14 = 0f;
			result.M21 = result3.X;
			result.M22 = result3.Y;
			result.M23 = result3.Z;
			result.M24 = 0f;
			result.M31 = vector.X;
			result.M32 = vector.Y;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a cylindrical billboard that rotates around a specified axis.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="rotateAxis">Axis to rotate the billboard around.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="objectForwardVector">Optional forward vector of the object.</param>
		public static Matrix CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector)
		{
			Vector3 vector = default(Vector3);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			if ((double)num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt(num), out vector);
			}
			Vector3 vector2 = rotateAxis;
			Vector3.Dot(ref rotateAxis, ref vector, out float result);
			Vector3 vector3;
			Vector3 result2;
			if ((double)Math.Abs(result) > 0.998254656791687)
			{
				if (objectForwardVector.HasValue)
				{
					vector3 = objectForwardVector.Value;
					Vector3.Dot(ref rotateAxis, ref vector3, out result);
					if ((double)Math.Abs(result) > 0.998254656791687)
					{
						vector3 = (((double)Math.Abs((float)((double)rotateAxis.X * (double)Vector3.Forward.X + (double)rotateAxis.Y * (double)Vector3.Forward.Y + (double)rotateAxis.Z * (double)Vector3.Forward.Z)) > 0.998254656791687) ? Vector3.Right : Vector3.Forward);
					}
				}
				else
				{
					vector3 = (((double)Math.Abs((float)((double)rotateAxis.X * (double)Vector3.Forward.X + (double)rotateAxis.Y * (double)Vector3.Forward.Y + (double)rotateAxis.Z * (double)Vector3.Forward.Z)) > 0.998254656791687) ? Vector3.Right : Vector3.Forward);
				}
				Vector3.Cross(ref rotateAxis, ref vector3, out result2);
				result2.Normalize();
				Vector3.Cross(ref result2, ref rotateAxis, out vector3);
				vector3.Normalize();
			}
			else
			{
				Vector3.Cross(ref rotateAxis, ref vector, out result2);
				result2.Normalize();
				Vector3.Cross(ref result2, ref vector2, out vector3);
				vector3.Normalize();
			}
			Matrix result3 = default(Matrix);
			result3.M11 = result2.X;
			result3.M12 = result2.Y;
			result3.M13 = result2.Z;
			result3.M14 = 0f;
			result3.M21 = vector2.X;
			result3.M22 = vector2.Y;
			result3.M23 = vector2.Z;
			result3.M24 = 0f;
			result3.M31 = vector3.X;
			result3.M32 = vector3.Y;
			result3.M33 = vector3.Z;
			result3.M34 = 0f;
			result3.M41 = objectPosition.X;
			result3.M42 = objectPosition.Y;
			result3.M43 = objectPosition.Z;
			result3.M44 = 1f;
			return result3;
		}

		/// <summary>
		/// Creates a cylindrical billboard that rotates around a specified axis.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="rotateAxis">Axis to rotate the billboard around.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="objectForwardVector">Optional forward vector of the object.</param><param name="result">[OutAttribute] The created billboard matrix.</param>
		public static void CreateConstrainedBillboard(ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 rotateAxis, Vector3? cameraForwardVector, Vector3? objectForwardVector, out Matrix result)
		{
			Vector3 vector = default(Vector3);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			float num = vector.LengthSquared();
			if ((double)num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3.Forward);
			}
			else
			{
				Vector3.Multiply(ref vector, 1f / (float)Math.Sqrt(num), out vector);
			}
			Vector3 vector2 = rotateAxis;
			Vector3.Dot(ref rotateAxis, ref vector, out float result2);
			Vector3 vector3;
			Vector3 result3;
			if ((double)Math.Abs(result2) > 0.998254656791687)
			{
				if (objectForwardVector.HasValue)
				{
					vector3 = objectForwardVector.Value;
					Vector3.Dot(ref rotateAxis, ref vector3, out result2);
					if ((double)Math.Abs(result2) > 0.998254656791687)
					{
						vector3 = (((double)Math.Abs((float)((double)rotateAxis.X * (double)Vector3.Forward.X + (double)rotateAxis.Y * (double)Vector3.Forward.Y + (double)rotateAxis.Z * (double)Vector3.Forward.Z)) > 0.998254656791687) ? Vector3.Right : Vector3.Forward);
					}
				}
				else
				{
					vector3 = (((double)Math.Abs((float)((double)rotateAxis.X * (double)Vector3.Forward.X + (double)rotateAxis.Y * (double)Vector3.Forward.Y + (double)rotateAxis.Z * (double)Vector3.Forward.Z)) > 0.998254656791687) ? Vector3.Right : Vector3.Forward);
				}
				Vector3.Cross(ref rotateAxis, ref vector3, out result3);
				result3.Normalize();
				Vector3.Cross(ref result3, ref rotateAxis, out vector3);
				vector3.Normalize();
			}
			else
			{
				Vector3.Cross(ref rotateAxis, ref vector, out result3);
				result3.Normalize();
				Vector3.Cross(ref result3, ref vector2, out vector3);
				vector3.Normalize();
			}
			result.M11 = result3.X;
			result.M12 = result3.Y;
			result.M13 = result3.Z;
			result.M14 = 0f;
			result.M21 = vector2.X;
			result.M22 = vector2.Y;
			result.M23 = vector2.Z;
			result.M24 = 0f;
			result.M31 = vector3.X;
			result.M32 = vector3.Y;
			result.M33 = vector3.Z;
			result.M34 = 0f;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
		public static Matrix CreateTranslation(Vector3 position)
		{
			Matrix result = default(Matrix);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="position">Amounts to translate by on the x, y, and z axes.</param><param name="result">[OutAttribute] The created translation Matrix.</param>
		public static void CreateTranslation(ref Vector3 position, out Matrix result)
		{
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="xPosition">Value to translate by on the x-axis.</param><param name="yPosition">Value to translate by on the y-axis.</param><param name="zPosition">Value to translate by on the z-axis.</param>
		public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
		{
			Matrix result = default(Matrix);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="xPosition">Value to translate by on the x-axis.</param><param name="yPosition">Value to translate by on the y-axis.</param><param name="zPosition">Value to translate by on the z-axis.</param><param name="result">[OutAttribute] The created translation Matrix.</param>
		public static void CreateTranslation(float xPosition, float yPosition, float zPosition, out Matrix result)
		{
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param>
		public static Matrix CreateScale(float xScale, float yScale, float zScale)
		{
			Matrix result = default(Matrix);
			result.M11 = xScale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = yScale;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = zScale;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(float xScale, float yScale, float zScale, out Matrix result)
		{
			result.M11 = xScale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = yScale;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = zScale;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
		public static Matrix CreateScale(Vector3 scales)
		{
			float x = scales.X;
			float y = scales.Y;
			float z = scales.Z;
			Matrix result = default(Matrix);
			result.M11 = x;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = y;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = z;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(ref Vector3 scales, out Matrix result)
		{
			float x = scales.X;
			float y = scales.Y;
			float z = scales.Z;
			result.M11 = x;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = y;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = z;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scale">Amount to scale by.</param>
		public static Matrix CreateScale(float scale)
		{
			Matrix result = default(Matrix);
			float num = result.M11 = scale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scale">Value to scale by.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(float scale, out Matrix result)
		{
			float num = result.M11 = scale;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix CreateRotationX(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix result = default(Matrix);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = num2;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f - num2;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationX(float radians, out Matrix result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = 1f;
			result.M12 = 0f;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = num;
			result.M23 = num2;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f - num2;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix CreateRotationY(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix result = default(Matrix);
			result.M11 = num;
			result.M12 = 0f;
			result.M13 = 0f - num2;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = num2;
			result.M32 = 0f;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationY(float radians, out Matrix result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = num;
			result.M12 = 0f;
			result.M13 = 0f - num2;
			result.M14 = 0f;
			result.M21 = 0f;
			result.M22 = 1f;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = num2;
			result.M32 = 0f;
			result.M33 = num;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static Matrix CreateRotationZ(float radians)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Matrix result = default(Matrix);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f - num2;
			result.M22 = num;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The rotation matrix.</param>
		public static void CreateRotationZ(float radians, out Matrix result)
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0f;
			result.M14 = 0f;
			result.M21 = 0f - num2;
			result.M22 = num;
			result.M23 = 0f;
			result.M24 = 0f;
			result.M31 = 0f;
			result.M32 = 0f;
			result.M33 = 1f;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a new Matrix that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param>
		public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
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
			Matrix result = default(Matrix);
			result.M11 = num3 + num2 * (1f - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M14 = 0f;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M24 = 0f;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a new Matrix that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param><param name="result">[OutAttribute] The created Matrix.</param>
		public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix result)
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
			result.M14 = 0f;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1f - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M24 = 0f;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1f - num5);
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		public static void CreateRotationFromTwoVectors(ref Vector3 fromVector, ref Vector3 toVector, out Matrix resultMatrix)
		{
			Vector3 vector = Vector3.Normalize(fromVector);
			Vector3 vector2 = Vector3.Normalize(toVector);
			Vector3.Cross(ref vector, ref vector2, out Vector3 result);
			result.Normalize();
			Vector3.Cross(ref vector, ref result, out Vector3 result2);
			Matrix matrix = new Matrix(vector.X, result.X, result2.X, 0f, vector.Y, result.Y, result2.Y, 0f, vector.Z, result.Z, result2.Z, 0f, 0f, 0f, 0f, 1f);
			Vector3.Cross(ref vector2, ref result, out result2);
			Matrix matrix2 = new Matrix(vector2.X, vector2.Y, vector2.Z, 0f, result.X, result.Y, result.Z, 0f, result2.X, result2.Y, result2.Z, 0f, 0f, 0f, 0f, 1f);
			resultMatrix = matrix * matrix2;
		}

		/// <summary>
		/// Builds a perspective projection matrix based on a field of view and returns by value.
		/// </summary>
		/// <param name="fieldOfView">Field of view in the y direction, in radians.</param><param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param>
		public static Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
		{
			if ((double)fieldOfView <= 0.0 || (double)fieldOfView >= 3.14159274101257)
			{
				throw new ArgumentOutOfRangeException("fieldOfView", string.Format(CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
				{
					"fieldOfView"
				}));
			}
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			return result;
		}

		public static Matrix CreatePerspectiveFovRhComplementary(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
		{
			if ((double)fieldOfView <= 0.0 || (double)fieldOfView >= 3.14159274101257)
			{
				throw new ArgumentOutOfRangeException("fieldOfView", string.Format(CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
				{
					"fieldOfView"
				}));
			}
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = (0f - farPlaneDistance) / (nearPlaneDistance - farPlaneDistance) - 1f;
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = 0f - (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			return result;
		}

		public static Matrix CreatePerspectiveFovRhInfinite(float fieldOfView, float aspectRatio, float nearPlaneDistance)
		{
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = -1f;
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = 0f - nearPlaneDistance;
			return result;
		}

		public static Matrix CreatePerspectiveFovRhInfiniteComplementary(float fieldOfView, float aspectRatio, float nearPlaneDistance)
		{
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = 0f;
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = nearPlaneDistance;
			return result;
		}

		public static Matrix CreatePerspectiveFovRhInverse(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
		{
			float num = (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = aspectRatio * num;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = (result.M33 = 0f));
			result.M34 = (nearPlaneDistance - farPlaneDistance) / (nearPlaneDistance * farPlaneDistance);
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = -1f;
			result.M44 = 1f / nearPlaneDistance;
			return result;
		}

		public static Matrix CreatePerspectiveFovRhInfiniteInverse(float fieldOfView, float aspectRatio, float nearPlaneDistance)
		{
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = 0f;
			result.M34 = -1f / nearPlaneDistance;
			result.M41 = (result.M42 = 0f);
			result.M43 = -1f;
			result.M44 = 1f / nearPlaneDistance;
			return result;
		}

		public static Matrix CreatePerspectiveFovRhInfiniteComplementaryInverse(float fieldOfView, float aspectRatio, float nearPlaneDistance)
		{
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			Matrix result = default(Matrix);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = 0f;
			result.M34 = 1f / nearPlaneDistance;
			result.M41 = (result.M42 = 0f);
			result.M43 = -1f;
			result.M44 = 0f;
			return result;
		}

		public static Matrix CreateFromPerspectiveFieldOfView(ref Matrix proj, float nearPlaneDistance, float farPlaneDistance)
		{
			Matrix result = proj;
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			return result;
		}

		/// <summary>
		/// Builds a perspective projection matrix based on a field of view and returns by reference.
		/// </summary>
		/// <param name="fieldOfView">Field of view in the y direction, in radians.</param><param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param><param name="result">[OutAttribute] The perspective projection matrix.</param>
		public static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			if ((double)fieldOfView <= 0.0 || (double)fieldOfView >= 3.14159274101257)
			{
				throw new ArgumentOutOfRangeException("fieldOfView", string.Format(CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
				{
					"fieldOfView"
				}));
			}
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if ((double)nearPlaneDistance >= (double)farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			float num = 1f / (float)Math.Tan((double)fieldOfView * 0.5);
			float num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (result.M32 = 0f);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
		}

		/// <summary>
		/// Builds a perspective projection matrix and returns the result by value.
		/// </summary>
		/// <param name="width">Width of the view volume at the near view plane.</param><param name="height">Height of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param>
		public static Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
		{
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if ((double)nearPlaneDistance >= (double)farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			Matrix result = default(Matrix);
			result.M11 = 2f * nearPlaneDistance / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f * nearPlaneDistance / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M31 = (result.M32 = 0f);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			return result;
		}

		/// <summary>
		/// Builds a perspective projection matrix and returns the result by reference.
		/// </summary>
		/// <param name="width">Width of the view volume at the near view plane.</param><param name="height">Height of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if ((double)nearPlaneDistance >= (double)farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			result.M11 = 2f * nearPlaneDistance / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f * nearPlaneDistance / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M31 = (result.M32 = 0f);
			result.M34 = -1f;
			result.M41 = (result.M42 = (result.M44 = 0f));
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
		}

		/// <summary>
		/// Builds a customized, perspective projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume at the near view plane.</param><param name="right">Maximum x-value of the view volume at the near view plane.</param><param name="bottom">Minimum y-value of the view volume at the near view plane.</param><param name="top">Maximum y-value of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to of the far view plane.</param>
		public static Matrix CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
		{
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if ((double)nearPlaneDistance >= (double)farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			Matrix result = default(Matrix);
			result.M11 = (float)(2.0 * (double)nearPlaneDistance / ((double)right - (double)left));
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = (float)(2.0 * (double)nearPlaneDistance / ((double)top - (double)bottom));
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (float)(((double)left + (double)right) / ((double)right - (double)left));
			result.M32 = (float)(((double)top + (double)bottom) / ((double)top - (double)bottom));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			result.M41 = (result.M42 = (result.M44 = 0f));
			return result;
		}

		/// <summary>
		/// Builds a customized, perspective projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume at the near view plane.</param><param name="right">Maximum x-value of the view volume at the near view plane.</param><param name="bottom">Minimum y-value of the view volume at the near view plane.</param><param name="top">Maximum y-value of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to of the far view plane.</param><param name="result">[OutAttribute] The created projection matrix.</param>
		public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			if ((double)nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if ((double)farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if ((double)nearPlaneDistance >= (double)farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			result.M11 = (float)(2.0 * (double)nearPlaneDistance / ((double)right - (double)left));
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = (float)(2.0 * (double)nearPlaneDistance / ((double)top - (double)bottom));
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M31 = (float)(((double)left + (double)right) / ((double)right - (double)left));
			result.M32 = (float)(((double)top + (double)bottom) / ((double)top - (double)bottom));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1f;
			result.M43 = (float)((double)nearPlaneDistance * (double)farPlaneDistance / ((double)nearPlaneDistance - (double)farPlaneDistance));
			result.M41 = (result.M42 = (result.M44 = 0f));
		}

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// </summary>
		/// <param name="width">Width of the view volume.</param><param name="height">Height of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param>
		public static Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
		{
			Matrix result = default(Matrix);
			result.M11 = 2f / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
			result.M31 = (result.M32 = (result.M34 = 0f));
			result.M41 = (result.M42 = 0f);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// </summary>
		/// <param name="width">Width of the view volume.</param><param name="height">Height of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane, out Matrix result)
		{
			result.M11 = 2f / width;
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = 2f / height;
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
			result.M31 = (result.M32 = (result.M34 = 0f));
			result.M41 = (result.M42 = 0f);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
		}

		/// <summary>
		/// Builds a customized, orthogonal projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume.</param><param name="right">Maximum x-value of the view volume.</param><param name="bottom">Minimum y-value of the view volume.</param><param name="top">Maximum y-value of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param>
		public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
		{
			Matrix result = default(Matrix);
			result.M11 = (float)(2.0 / ((double)right - (double)left));
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = (float)(2.0 / ((double)top - (double)bottom));
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
			result.M31 = (result.M32 = (result.M34 = 0f));
			result.M41 = (float)(((double)left + (double)right) / ((double)left - (double)right));
			result.M42 = (float)(((double)top + (double)bottom) / ((double)bottom - (double)top));
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Builds a customized, orthogonal projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume.</param><param name="right">Maximum x-value of the view volume.</param><param name="bottom">Minimum y-value of the view volume.</param><param name="top">Maximum y-value of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out Matrix result)
		{
			result.M11 = (float)(2.0 / ((double)right - (double)left));
			result.M12 = (result.M13 = (result.M14 = 0f));
			result.M22 = (float)(2.0 / ((double)top - (double)bottom));
			result.M21 = (result.M23 = (result.M24 = 0f));
			result.M33 = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
			result.M31 = (result.M32 = (result.M34 = 0f));
			result.M41 = (float)(((double)left + (double)right) / ((double)left - (double)right));
			result.M42 = (float)(((double)top + (double)bottom) / ((double)bottom - (double)top));
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a view matrix.
		/// </summary>
		/// <param name="cameraPosition">The position of the camera.</param><param name="cameraTarget">The target towards which the camera is pointing.</param><param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
		public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			Matrix result = default(Matrix);
			result.M11 = vector2.X;
			result.M12 = vector3.X;
			result.M13 = vector.X;
			result.M14 = 0f;
			result.M21 = vector2.Y;
			result.M22 = vector3.Y;
			result.M23 = vector.Y;
			result.M24 = 0f;
			result.M31 = vector2.Z;
			result.M32 = vector3.Z;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = 0f - Vector3.Dot(vector2, cameraPosition);
			result.M42 = 0f - Vector3.Dot(vector3, cameraPosition);
			result.M43 = 0f - Vector3.Dot(vector, cameraPosition);
			result.M44 = 1f;
			return result;
		}

		public static Matrix CreateLookAtInverse(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
		{
			Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			Matrix result = default(Matrix);
			result.M11 = vector2.X;
			result.M12 = vector2.Y;
			result.M13 = vector2.Z;
			result.M14 = 0f;
			result.M21 = vector3.X;
			result.M22 = vector3.Y;
			result.M23 = vector3.Z;
			result.M24 = 0f;
			result.M31 = vector.X;
			result.M32 = vector.Y;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = cameraPosition.X;
			result.M42 = cameraPosition.Y;
			result.M43 = cameraPosition.Z;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a view matrix.
		/// </summary>
		/// <param name="cameraPosition">The position of the camera.</param><param name="cameraTarget">The target towards which the camera is pointing.</param><param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param><param name="result">[OutAttribute] The created view matrix.</param>
		public static void CreateLookAt(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result)
		{
			Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			result.M11 = vector2.X;
			result.M12 = vector3.X;
			result.M13 = vector.X;
			result.M14 = 0f;
			result.M21 = vector2.Y;
			result.M22 = vector3.Y;
			result.M23 = vector.Y;
			result.M24 = 0f;
			result.M31 = vector2.Z;
			result.M32 = vector3.Z;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = 0f - Vector3.Dot(vector2, cameraPosition);
			result.M42 = 0f - Vector3.Dot(vector3, cameraPosition);
			result.M43 = 0f - Vector3.Dot(vector, cameraPosition);
			result.M44 = 1f;
		}

		public static Matrix CreateWorld(Vector3 position)
		{
			return CreateWorld(position, Vector3.Forward, Vector3.Up);
		}

		/// <summary>
		/// Creates a world matrix with the specified parameters.
		/// </summary>
		/// <param name="position">Position of the object. This value is used in translation operations.</param><param name="forward">Forward direction of the object.</param><param name="up">Upward direction of the object; usually [0, 1, 0].</param>
		public static Matrix CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
		{
			Vector3 vector = Vector3.Normalize(-forward);
			Vector3.Cross(ref up, ref vector, out Vector3 result);
			Vector3.Normalize(ref result, out Vector3 result2);
			Vector3.Cross(ref vector, ref result2, out Vector3 result3);
			Matrix result4 = default(Matrix);
			result4.M11 = result2.X;
			result4.M12 = result2.Y;
			result4.M13 = result2.Z;
			result4.M14 = 0f;
			result4.M21 = result3.X;
			result4.M22 = result3.Y;
			result4.M23 = result3.Z;
			result4.M24 = 0f;
			result4.M31 = vector.X;
			result4.M32 = vector.Y;
			result4.M33 = vector.Z;
			result4.M34 = 0f;
			result4.M41 = position.X;
			result4.M42 = position.Y;
			result4.M43 = position.Z;
			result4.M44 = 1f;
			return result4;
		}

		/// <summary>
		/// Creates a world matrix with the specified parameters.
		/// </summary>
		/// <param name="position">Position of the object. This value is used in translation operations.</param><param name="forward">Forward direction of the object.</param><param name="up">Upward direction of the object; usually [0, 1, 0].</param><param name="result">[OutAttribute] The created world matrix.</param>
		public static void CreateWorld(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
		{
			Vector3 value = -forward;
			Vector3.Normalize(ref value, out Vector3 result2);
			Vector3.Cross(ref up, ref result2, out Vector3 result3);
			Vector3.Normalize(ref result3, out Vector3 result4);
			Vector3.Cross(ref result2, ref result4, out Vector3 result5);
			result.M11 = result4.X;
			result.M12 = result4.Y;
			result.M13 = result4.Z;
			result.M14 = 0f;
			result.M21 = result5.X;
			result.M22 = result5.Y;
			result.M23 = result5.Z;
			result.M24 = 0f;
			result.M31 = result2.X;
			result.M32 = result2.Y;
			result.M33 = result2.Z;
			result.M34 = 0f;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a rotation Matrix from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix from.</param>
		public static Matrix CreateFromQuaternion(Quaternion quaternion)
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
			Matrix result = default(Matrix);
			result.M11 = (float)(1.0 - 2.0 * ((double)num2 + (double)num3));
			result.M12 = (float)(2.0 * ((double)num4 + (double)num5));
			result.M13 = (float)(2.0 * ((double)num6 - (double)num7));
			result.M14 = 0f;
			result.M21 = (float)(2.0 * ((double)num4 - (double)num5));
			result.M22 = (float)(1.0 - 2.0 * ((double)num3 + (double)num));
			result.M23 = (float)(2.0 * ((double)num8 + (double)num9));
			result.M24 = 0f;
			result.M31 = (float)(2.0 * ((double)num6 + (double)num7));
			result.M32 = (float)(2.0 * ((double)num8 - (double)num9));
			result.M33 = (float)(1.0 - 2.0 * ((double)num2 + (double)num));
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Creates a rotation Matrix from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix from.</param><param name="result">[OutAttribute] The created Matrix.</param>
		public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix result)
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
			result.M11 = (float)(1.0 - 2.0 * ((double)num2 + (double)num3));
			result.M12 = (float)(2.0 * ((double)num4 + (double)num5));
			result.M13 = (float)(2.0 * ((double)num6 - (double)num7));
			result.M14 = 0f;
			result.M21 = (float)(2.0 * ((double)num4 - (double)num5));
			result.M22 = (float)(1.0 - 2.0 * ((double)num3 + (double)num));
			result.M23 = (float)(2.0 * ((double)num8 + (double)num9));
			result.M24 = 0f;
			result.M31 = (float)(2.0 * ((double)num6 + (double)num7));
			result.M32 = (float)(2.0 * ((double)num8 - (double)num9));
			result.M33 = (float)(1.0 - 2.0 * ((double)num2 + (double)num));
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Creates a new rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param>
		public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Quaternion result);
			CreateFromQuaternion(ref result, out Matrix result2);
			return result2;
		}

		/// <summary>
		/// Fills in a rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param><param name="result">[OutAttribute] An existing matrix filled in to represent the specified yaw, pitch, and roll.</param>
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
		{
			Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Quaternion result2);
			CreateFromQuaternion(ref result2, out result);
		}

		public static Matrix CreateFromTransformScale(Quaternion orientation, Vector3 position, Vector3 scale)
		{
			Matrix matrix = CreateFromQuaternion(orientation);
			matrix.Translation = position;
			Rescale(ref matrix, ref scale);
			return matrix;
		}

		/// <summary>
		/// Creates a Matrix that flattens geometry into a specified Plane as if casting a shadow from a specified light source.
		/// </summary>
		/// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param><param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
		public static Matrix CreateShadow(Vector3 lightDirection, Plane plane)
		{
			Plane.Normalize(ref plane, out Plane result);
			float num = (float)((double)result.Normal.X * (double)lightDirection.X + (double)result.Normal.Y * (double)lightDirection.Y + (double)result.Normal.Z * (double)lightDirection.Z);
			float num2 = 0f - result.Normal.X;
			float num3 = 0f - result.Normal.Y;
			float num4 = 0f - result.Normal.Z;
			float num5 = 0f - result.D;
			Matrix result2 = default(Matrix);
			result2.M11 = num2 * lightDirection.X + num;
			result2.M21 = num3 * lightDirection.X;
			result2.M31 = num4 * lightDirection.X;
			result2.M41 = num5 * lightDirection.X;
			result2.M12 = num2 * lightDirection.Y;
			result2.M22 = num3 * lightDirection.Y + num;
			result2.M32 = num4 * lightDirection.Y;
			result2.M42 = num5 * lightDirection.Y;
			result2.M13 = num2 * lightDirection.Z;
			result2.M23 = num3 * lightDirection.Z;
			result2.M33 = num4 * lightDirection.Z + num;
			result2.M43 = num5 * lightDirection.Z;
			result2.M14 = 0f;
			result2.M24 = 0f;
			result2.M34 = 0f;
			result2.M44 = num;
			return result2;
		}

		/// <summary>
		/// Fills in a Matrix to flatten geometry into a specified Plane as if casting a shadow from a specified light source.
		/// </summary>
		/// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param><param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param><param name="result">[OutAttribute] A Matrix that can be used to flatten geometry onto the specified plane from the specified direction.</param>
		public static void CreateShadow(ref Vector3 lightDirection, ref Plane plane, out Matrix result)
		{
			Plane.Normalize(ref plane, out Plane result2);
			float num = (float)((double)result2.Normal.X * (double)lightDirection.X + (double)result2.Normal.Y * (double)lightDirection.Y + (double)result2.Normal.Z * (double)lightDirection.Z);
			float num2 = 0f - result2.Normal.X;
			float num3 = 0f - result2.Normal.Y;
			float num4 = 0f - result2.Normal.Z;
			float num5 = 0f - result2.D;
			result.M11 = num2 * lightDirection.X + num;
			result.M21 = num3 * lightDirection.X;
			result.M31 = num4 * lightDirection.X;
			result.M41 = num5 * lightDirection.X;
			result.M12 = num2 * lightDirection.Y;
			result.M22 = num3 * lightDirection.Y + num;
			result.M32 = num4 * lightDirection.Y;
			result.M42 = num5 * lightDirection.Y;
			result.M13 = num2 * lightDirection.Z;
			result.M23 = num3 * lightDirection.Z;
			result.M33 = num4 * lightDirection.Z + num;
			result.M43 = num5 * lightDirection.Z;
			result.M14 = 0f;
			result.M24 = 0f;
			result.M34 = 0f;
			result.M44 = num;
		}

		/// <summary>
		/// Creates a Matrix that reflects the coordinate system about a specified Plane.
		/// </summary>
		/// <param name="value">The Plane about which to create a reflection.</param>
		public static Matrix CreateReflection(Plane value)
		{
			value.Normalize();
			float x = value.Normal.X;
			float y = value.Normal.Y;
			float z = value.Normal.Z;
			float num = -2f * x;
			float num2 = -2f * y;
			float num3 = -2f * z;
			Matrix result = default(Matrix);
			result.M11 = (float)((double)num * (double)x + 1.0);
			result.M12 = num2 * x;
			result.M13 = num3 * x;
			result.M14 = 0f;
			result.M21 = num * y;
			result.M22 = (float)((double)num2 * (double)y + 1.0);
			result.M23 = num3 * y;
			result.M24 = 0f;
			result.M31 = num * z;
			result.M32 = num2 * z;
			result.M33 = (float)((double)num3 * (double)z + 1.0);
			result.M34 = 0f;
			result.M41 = num * value.D;
			result.M42 = num2 * value.D;
			result.M43 = num3 * value.D;
			result.M44 = 1f;
			return result;
		}

		/// <summary>
		/// Fills in an existing Matrix so that it reflects the coordinate system about a specified Plane.
		/// </summary>
		/// <param name="value">The Plane about which to create a reflection.</param><param name="result">[OutAttribute] A Matrix that creates the reflection.</param>
		public static void CreateReflection(ref Plane value, out Matrix result)
		{
			Plane.Normalize(ref value, out Plane result2);
			value.Normalize();
			float x = result2.Normal.X;
			float y = result2.Normal.Y;
			float z = result2.Normal.Z;
			float num = -2f * x;
			float num2 = -2f * y;
			float num3 = -2f * z;
			result.M11 = (float)((double)num * (double)x + 1.0);
			result.M12 = num2 * x;
			result.M13 = num3 * x;
			result.M14 = 0f;
			result.M21 = num * y;
			result.M22 = (float)((double)num2 * (double)y + 1.0);
			result.M23 = num3 * y;
			result.M24 = 0f;
			result.M31 = num * z;
			result.M32 = num2 * z;
			result.M33 = (float)((double)num3 * (double)z + 1.0);
			result.M34 = 0f;
			result.M41 = num * result2.D;
			result.M42 = num2 * result2.D;
			result.M43 = num3 * result2.D;
			result.M44 = 1f;
		}

		/// <summary>
		/// Transforms a Matrix by applying a Quaternion rotation.
		/// </summary>
		/// <param name="value">The Matrix to transform.</param><param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
		public static Matrix Transform(Matrix value, Quaternion rotation)
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
			Matrix result = default(Matrix);
			result.M11 = (float)((double)value.M11 * (double)num13 + (double)value.M12 * (double)num14 + (double)value.M13 * (double)num15);
			result.M12 = (float)((double)value.M11 * (double)num16 + (double)value.M12 * (double)num17 + (double)value.M13 * (double)num18);
			result.M13 = (float)((double)value.M11 * (double)num19 + (double)value.M12 * (double)num20 + (double)value.M13 * (double)num21);
			result.M14 = value.M14;
			result.M21 = (float)((double)value.M21 * (double)num13 + (double)value.M22 * (double)num14 + (double)value.M23 * (double)num15);
			result.M22 = (float)((double)value.M21 * (double)num16 + (double)value.M22 * (double)num17 + (double)value.M23 * (double)num18);
			result.M23 = (float)((double)value.M21 * (double)num19 + (double)value.M22 * (double)num20 + (double)value.M23 * (double)num21);
			result.M24 = value.M24;
			result.M31 = (float)((double)value.M31 * (double)num13 + (double)value.M32 * (double)num14 + (double)value.M33 * (double)num15);
			result.M32 = (float)((double)value.M31 * (double)num16 + (double)value.M32 * (double)num17 + (double)value.M33 * (double)num18);
			result.M33 = (float)((double)value.M31 * (double)num19 + (double)value.M32 * (double)num20 + (double)value.M33 * (double)num21);
			result.M34 = value.M34;
			result.M41 = (float)((double)value.M41 * (double)num13 + (double)value.M42 * (double)num14 + (double)value.M43 * (double)num15);
			result.M42 = (float)((double)value.M41 * (double)num16 + (double)value.M42 * (double)num17 + (double)value.M43 * (double)num18);
			result.M43 = (float)((double)value.M41 * (double)num19 + (double)value.M42 * (double)num20 + (double)value.M43 * (double)num21);
			result.M44 = value.M44;
			return result;
		}

		/// <summary>
		/// Transforms a Matrix by applying a Quaternion rotation.
		/// </summary>
		/// <param name="value">The Matrix to transform.</param><param name="rotation">The rotation to apply, expressed as a Quaternion.</param><param name="result">[OutAttribute] An existing Matrix filled in with the result of the transform.</param>
		public static void Transform(ref Matrix value, ref Quaternion rotation, out Matrix result)
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
			float m = (float)((double)value.M11 * (double)num13 + (double)value.M12 * (double)num14 + (double)value.M13 * (double)num15);
			float m2 = (float)((double)value.M11 * (double)num16 + (double)value.M12 * (double)num17 + (double)value.M13 * (double)num18);
			float m3 = (float)((double)value.M11 * (double)num19 + (double)value.M12 * (double)num20 + (double)value.M13 * (double)num21);
			float m4 = value.M14;
			float m5 = (float)((double)value.M21 * (double)num13 + (double)value.M22 * (double)num14 + (double)value.M23 * (double)num15);
			float m6 = (float)((double)value.M21 * (double)num16 + (double)value.M22 * (double)num17 + (double)value.M23 * (double)num18);
			float m7 = (float)((double)value.M21 * (double)num19 + (double)value.M22 * (double)num20 + (double)value.M23 * (double)num21);
			float m8 = value.M24;
			float m9 = (float)((double)value.M31 * (double)num13 + (double)value.M32 * (double)num14 + (double)value.M33 * (double)num15);
			float m10 = (float)((double)value.M31 * (double)num16 + (double)value.M32 * (double)num17 + (double)value.M33 * (double)num18);
			float m11 = (float)((double)value.M31 * (double)num19 + (double)value.M32 * (double)num20 + (double)value.M33 * (double)num21);
			float m12 = value.M34;
			float m13 = (float)((double)value.M41 * (double)num13 + (double)value.M42 * (double)num14 + (double)value.M43 * (double)num15);
			float m14 = (float)((double)value.M41 * (double)num16 + (double)value.M42 * (double)num17 + (double)value.M43 * (double)num18);
			float m15 = (float)((double)value.M41 * (double)num19 + (double)value.M42 * (double)num20 + (double)value.M43 * (double)num21);
			float m16 = value.M44;
			result.M11 = m;
			result.M12 = m2;
			result.M13 = m3;
			result.M14 = m4;
			result.M21 = m5;
			result.M22 = m6;
			result.M23 = m7;
			result.M24 = m8;
			result.M31 = m9;
			result.M32 = m10;
			result.M33 = m11;
			result.M34 = m12;
			result.M41 = m13;
			result.M42 = m14;
			result.M43 = m15;
			result.M44 = m16;
		}

		public unsafe Vector4 GetRow(int row)
		{
			fixed (float* ptr = &M11)
			{
				float* ptr2 = ptr + row * 4;
				return new Vector4(*ptr2, ptr2[1], ptr2[2], ptr2[3]);
			}
		}

		public unsafe void SetRow(int row, Vector4 value)
		{
			fixed (float* ptr = &M11)
			{
				float* intPtr = ptr + row * 4;
				*intPtr = value.X;
				intPtr[1] = value.Y;
				intPtr[2] = value.Z;
				intPtr[3] = value.W;
			}
		}

		/// <summary>
		/// Retrieves a string representation of the current object.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return "{ " + string.Format(currentCulture, "{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", M11.ToString(currentCulture), M12.ToString(currentCulture), M13.ToString(currentCulture), M14.ToString(currentCulture)) + string.Format(currentCulture, "{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", M21.ToString(currentCulture), M22.ToString(currentCulture), M23.ToString(currentCulture), M24.ToString(currentCulture)) + string.Format(currentCulture, "{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", M31.ToString(currentCulture), M32.ToString(currentCulture), M33.ToString(currentCulture), M34.ToString(currentCulture)) + string.Format(currentCulture, "{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", M41.ToString(currentCulture), M42.ToString(currentCulture), M43.ToString(currentCulture), M44.ToString(currentCulture)) + "}";
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Matrix.
		/// </summary>
		/// <param name="other">The Object to compare with the current Matrix.</param>
		public bool Equals(Matrix other)
		{
			if ((double)M11 == (double)other.M11 && (double)M22 == (double)other.M22 && (double)M33 == (double)other.M33 && (double)M44 == (double)other.M44 && (double)M12 == (double)other.M12 && (double)M13 == (double)other.M13 && (double)M14 == (double)other.M14 && (double)M21 == (double)other.M21 && (double)M23 == (double)other.M23 && (double)M24 == (double)other.M24 && (double)M31 == (double)other.M31 && (double)M32 == (double)other.M32 && (double)M34 == (double)other.M34 && (double)M41 == (double)other.M41 && (double)M42 == (double)other.M42)
			{
				return (double)M43 == (double)other.M43;
			}
			return false;
		}

		/// <summary>
		/// Compares just position, forward and up
		/// </summary>
		public bool EqualsFast(ref Matrix other, float epsilon = 0.0001f)
		{
			float num = M21 - other.M21;
			float num2 = M22 - other.M22;
			float num3 = M23 - other.M23;
			float num4 = M31 - other.M31;
			float num5 = M32 - other.M32;
			float num6 = M33 - other.M33;
			float num7 = M41 - other.M41;
			float num8 = M42 - other.M42;
			float num9 = M43 - other.M43;
			float num10 = epsilon * epsilon;
			return num * num + num2 * num2 + num3 * num3 < num10 && num4 * num4 + num5 * num5 + num6 * num6 < num10 && num7 * num7 + num8 * num8 + num9 * num9 < num10;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Matrix)
			{
				result = Equals((Matrix)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code of this object.
		/// </summary>
		public override int GetHashCode()
		{
			return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() + M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() + M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() + M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
		}

		/// <summary>
		/// Transposes the rows and columns of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param>
		public static Matrix Transpose(Matrix matrix)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix.M11;
			result.M12 = matrix.M21;
			result.M13 = matrix.M31;
			result.M14 = matrix.M41;
			result.M21 = matrix.M12;
			result.M22 = matrix.M22;
			result.M23 = matrix.M32;
			result.M24 = matrix.M42;
			result.M31 = matrix.M13;
			result.M32 = matrix.M23;
			result.M33 = matrix.M33;
			result.M34 = matrix.M43;
			result.M41 = matrix.M14;
			result.M42 = matrix.M24;
			result.M43 = matrix.M34;
			result.M44 = matrix.M44;
			return result;
		}

		/// <summary>
		/// Transposes the rows and columns of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="result">[OutAttribute] Transposed matrix.</param>
		public static void Transpose(ref Matrix matrix, out Matrix result)
		{
			float m = matrix.M11;
			float m2 = matrix.M12;
			float m3 = matrix.M13;
			float m4 = matrix.M14;
			float m5 = matrix.M21;
			float m6 = matrix.M22;
			float m7 = matrix.M23;
			float m8 = matrix.M24;
			float m9 = matrix.M31;
			float m10 = matrix.M32;
			float m11 = matrix.M33;
			float m12 = matrix.M34;
			float m13 = matrix.M41;
			float m14 = matrix.M42;
			float m15 = matrix.M43;
			float m16 = matrix.M44;
			result.M11 = m;
			result.M12 = m5;
			result.M13 = m9;
			result.M14 = m13;
			result.M21 = m2;
			result.M22 = m6;
			result.M23 = m10;
			result.M24 = m14;
			result.M31 = m3;
			result.M32 = m7;
			result.M33 = m11;
			result.M34 = m15;
			result.M41 = m4;
			result.M42 = m8;
			result.M43 = m12;
			result.M44 = m16;
		}

		/// <summary>
		/// Transposes the rows and columns of a matrix that is assumed to be rotation only in place.
		/// </summary>
		public void TransposeRotationInPlace()
		{
			float m = M12;
			M12 = M21;
			M21 = m;
			m = M13;
			M13 = M31;
			M31 = m;
			m = M23;
			M23 = M32;
			M32 = m;
		}

		/// <summary>
		/// Calculates the determinant of the matrix.
		/// </summary>
		public float Determinant()
		{
			float m = M11;
			float m2 = M12;
			float m3 = M13;
			float m4 = M14;
			float m5 = M21;
			float m6 = M22;
			float m7 = M23;
			float m8 = M24;
			float m9 = M31;
			float m10 = M32;
			float m11 = M33;
			float m12 = M34;
			float m13 = M41;
			float m14 = M42;
			float m15 = M43;
			float m16 = M44;
			float num = (float)((double)m11 * (double)m16 - (double)m12 * (double)m15);
			float num2 = (float)((double)m10 * (double)m16 - (double)m12 * (double)m14);
			float num3 = (float)((double)m10 * (double)m15 - (double)m11 * (double)m14);
			float num4 = (float)((double)m9 * (double)m16 - (double)m12 * (double)m13);
			float num5 = (float)((double)m9 * (double)m15 - (double)m11 * (double)m13);
			float num6 = (float)((double)m9 * (double)m14 - (double)m10 * (double)m13);
			return (float)((double)m * ((double)m6 * (double)num - (double)m7 * (double)num2 + (double)m8 * (double)num3) - (double)m2 * ((double)m5 * (double)num - (double)m7 * (double)num4 + (double)m8 * (double)num5) + (double)m3 * ((double)m5 * (double)num2 - (double)m6 * (double)num4 + (double)m8 * (double)num6) - (double)m4 * ((double)m5 * (double)num3 - (double)m6 * (double)num5 + (double)m7 * (double)num6));
		}

		/// <summary>
		/// Calculates the inverse of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param>
		public static Matrix Invert(Matrix matrix)
		{
			return Invert(ref matrix);
		}

		public static Matrix Invert(ref Matrix matrix)
		{
			float m = matrix.M11;
			float m2 = matrix.M12;
			float m3 = matrix.M13;
			float m4 = matrix.M14;
			float m5 = matrix.M21;
			float m6 = matrix.M22;
			float m7 = matrix.M23;
			float m8 = matrix.M24;
			float m9 = matrix.M31;
			float m10 = matrix.M32;
			float m11 = matrix.M33;
			float m12 = matrix.M34;
			float m13 = matrix.M41;
			float m14 = matrix.M42;
			float m15 = matrix.M43;
			float m16 = matrix.M44;
			float num = (float)((double)m11 * (double)m16 - (double)m12 * (double)m15);
			float num2 = (float)((double)m10 * (double)m16 - (double)m12 * (double)m14);
			float num3 = (float)((double)m10 * (double)m15 - (double)m11 * (double)m14);
			float num4 = (float)((double)m9 * (double)m16 - (double)m12 * (double)m13);
			float num5 = (float)((double)m9 * (double)m15 - (double)m11 * (double)m13);
			float num6 = (float)((double)m9 * (double)m14 - (double)m10 * (double)m13);
			float num7 = (float)((double)m6 * (double)num - (double)m7 * (double)num2 + (double)m8 * (double)num3);
			float num8 = (float)(0.0 - ((double)m5 * (double)num - (double)m7 * (double)num4 + (double)m8 * (double)num5));
			float num9 = (float)((double)m5 * (double)num2 - (double)m6 * (double)num4 + (double)m8 * (double)num6);
			float num10 = (float)(0.0 - ((double)m5 * (double)num3 - (double)m6 * (double)num5 + (double)m7 * (double)num6));
			float num11 = (float)(1.0 / ((double)m * (double)num7 + (double)m2 * (double)num8 + (double)m3 * (double)num9 + (double)m4 * (double)num10));
			Matrix result = default(Matrix);
			result.M11 = num7 * num11;
			result.M21 = num8 * num11;
			result.M31 = num9 * num11;
			result.M41 = num10 * num11;
			result.M12 = (float)(0.0 - ((double)m2 * (double)num - (double)m3 * (double)num2 + (double)m4 * (double)num3)) * num11;
			result.M22 = (float)((double)m * (double)num - (double)m3 * (double)num4 + (double)m4 * (double)num5) * num11;
			result.M32 = (float)(0.0 - ((double)m * (double)num2 - (double)m2 * (double)num4 + (double)m4 * (double)num6)) * num11;
			result.M42 = (float)((double)m * (double)num3 - (double)m2 * (double)num5 + (double)m3 * (double)num6) * num11;
			float num12 = (float)((double)m7 * (double)m16 - (double)m8 * (double)m15);
			float num13 = (float)((double)m6 * (double)m16 - (double)m8 * (double)m14);
			float num14 = (float)((double)m6 * (double)m15 - (double)m7 * (double)m14);
			float num15 = (float)((double)m5 * (double)m16 - (double)m8 * (double)m13);
			float num16 = (float)((double)m5 * (double)m15 - (double)m7 * (double)m13);
			float num17 = (float)((double)m5 * (double)m14 - (double)m6 * (double)m13);
			result.M13 = (float)((double)m2 * (double)num12 - (double)m3 * (double)num13 + (double)m4 * (double)num14) * num11;
			result.M23 = (float)(0.0 - ((double)m * (double)num12 - (double)m3 * (double)num15 + (double)m4 * (double)num16)) * num11;
			result.M33 = (float)((double)m * (double)num13 - (double)m2 * (double)num15 + (double)m4 * (double)num17) * num11;
			result.M43 = (float)(0.0 - ((double)m * (double)num14 - (double)m2 * (double)num16 + (double)m3 * (double)num17)) * num11;
			float num18 = (float)((double)m7 * (double)m12 - (double)m8 * (double)m11);
			float num19 = (float)((double)m6 * (double)m12 - (double)m8 * (double)m10);
			float num20 = (float)((double)m6 * (double)m11 - (double)m7 * (double)m10);
			float num21 = (float)((double)m5 * (double)m12 - (double)m8 * (double)m9);
			float num22 = (float)((double)m5 * (double)m11 - (double)m7 * (double)m9);
			float num23 = (float)((double)m5 * (double)m10 - (double)m6 * (double)m9);
			result.M14 = (float)(0.0 - ((double)m2 * (double)num18 - (double)m3 * (double)num19 + (double)m4 * (double)num20)) * num11;
			result.M24 = (float)((double)m * (double)num18 - (double)m3 * (double)num21 + (double)m4 * (double)num22) * num11;
			result.M34 = (float)(0.0 - ((double)m * (double)num19 - (double)m2 * (double)num21 + (double)m4 * (double)num23)) * num11;
			result.M44 = (float)((double)m * (double)num20 - (double)m2 * (double)num22 + (double)m3 * (double)num23) * num11;
			return result;
		}

		/// <summary>
		/// Calculates the inverse of a matrix.
		/// </summary>
		/// <param name="matrix">The source matrix.</param><param name="result">[OutAttribute] The inverse of matrix. The same matrix can be used for both arguments.</param>
		public static void Invert(ref Matrix matrix, out Matrix result)
		{
			float m = matrix.M11;
			float m2 = matrix.M12;
			float m3 = matrix.M13;
			float m4 = matrix.M14;
			float m5 = matrix.M21;
			float m6 = matrix.M22;
			float m7 = matrix.M23;
			float m8 = matrix.M24;
			float m9 = matrix.M31;
			float m10 = matrix.M32;
			float m11 = matrix.M33;
			float m12 = matrix.M34;
			float m13 = matrix.M41;
			float m14 = matrix.M42;
			float m15 = matrix.M43;
			float m16 = matrix.M44;
			float num = (float)((double)m11 * (double)m16 - (double)m12 * (double)m15);
			float num2 = (float)((double)m10 * (double)m16 - (double)m12 * (double)m14);
			float num3 = (float)((double)m10 * (double)m15 - (double)m11 * (double)m14);
			float num4 = (float)((double)m9 * (double)m16 - (double)m12 * (double)m13);
			float num5 = (float)((double)m9 * (double)m15 - (double)m11 * (double)m13);
			float num6 = (float)((double)m9 * (double)m14 - (double)m10 * (double)m13);
			float num7 = (float)((double)m6 * (double)num - (double)m7 * (double)num2 + (double)m8 * (double)num3);
			float num8 = (float)(0.0 - ((double)m5 * (double)num - (double)m7 * (double)num4 + (double)m8 * (double)num5));
			float num9 = (float)((double)m5 * (double)num2 - (double)m6 * (double)num4 + (double)m8 * (double)num6);
			float num10 = (float)(0.0 - ((double)m5 * (double)num3 - (double)m6 * (double)num5 + (double)m7 * (double)num6));
			float num11 = (float)(1.0 / ((double)m * (double)num7 + (double)m2 * (double)num8 + (double)m3 * (double)num9 + (double)m4 * (double)num10));
			result.M11 = num7 * num11;
			result.M21 = num8 * num11;
			result.M31 = num9 * num11;
			result.M41 = num10 * num11;
			result.M12 = (float)(0.0 - ((double)m2 * (double)num - (double)m3 * (double)num2 + (double)m4 * (double)num3)) * num11;
			result.M22 = (float)((double)m * (double)num - (double)m3 * (double)num4 + (double)m4 * (double)num5) * num11;
			result.M32 = (float)(0.0 - ((double)m * (double)num2 - (double)m2 * (double)num4 + (double)m4 * (double)num6)) * num11;
			result.M42 = (float)((double)m * (double)num3 - (double)m2 * (double)num5 + (double)m3 * (double)num6) * num11;
			float num12 = (float)((double)m7 * (double)m16 - (double)m8 * (double)m15);
			float num13 = (float)((double)m6 * (double)m16 - (double)m8 * (double)m14);
			float num14 = (float)((double)m6 * (double)m15 - (double)m7 * (double)m14);
			float num15 = (float)((double)m5 * (double)m16 - (double)m8 * (double)m13);
			float num16 = (float)((double)m5 * (double)m15 - (double)m7 * (double)m13);
			float num17 = (float)((double)m5 * (double)m14 - (double)m6 * (double)m13);
			result.M13 = (float)((double)m2 * (double)num12 - (double)m3 * (double)num13 + (double)m4 * (double)num14) * num11;
			result.M23 = (float)(0.0 - ((double)m * (double)num12 - (double)m3 * (double)num15 + (double)m4 * (double)num16)) * num11;
			result.M33 = (float)((double)m * (double)num13 - (double)m2 * (double)num15 + (double)m4 * (double)num17) * num11;
			result.M43 = (float)(0.0 - ((double)m * (double)num14 - (double)m2 * (double)num16 + (double)m3 * (double)num17)) * num11;
			float num18 = (float)((double)m7 * (double)m12 - (double)m8 * (double)m11);
			float num19 = (float)((double)m6 * (double)m12 - (double)m8 * (double)m10);
			float num20 = (float)((double)m6 * (double)m11 - (double)m7 * (double)m10);
			float num21 = (float)((double)m5 * (double)m12 - (double)m8 * (double)m9);
			float num22 = (float)((double)m5 * (double)m11 - (double)m7 * (double)m9);
			float num23 = (float)((double)m5 * (double)m10 - (double)m6 * (double)m9);
			result.M14 = (float)(0.0 - ((double)m2 * (double)num18 - (double)m3 * (double)num19 + (double)m4 * (double)num20)) * num11;
			result.M24 = (float)((double)m * (double)num18 - (double)m3 * (double)num21 + (double)m4 * (double)num22) * num11;
			result.M34 = (float)(0.0 - ((double)m * (double)num19 - (double)m2 * (double)num21 + (double)m4 * (double)num23)) * num11;
			result.M44 = (float)((double)m * (double)num20 - (double)m2 * (double)num22 + (double)m3 * (double)num23) * num11;
		}

		/// <summary>
		/// Linearly interpolates between the corresponding values of two matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="amount">Interpolation value.</param>
		public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
			result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
			result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
			result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
			result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
			result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
			result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
			result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
			result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
			result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
			result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
			result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
			result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
			return result;
		}

		/// <summary>
		/// Linearly interpolates between the corresponding values of two matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="amount">Interpolation value.</param><param name="result">[OutAttribute] Resulting matrix.</param>
		public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
		{
			result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
			result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
			result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
			result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
			result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
			result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
			result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
			result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
			result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
			result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
			result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
			result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
			result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static void Slerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
		{
			Quaternion.CreateFromRotationMatrix(ref matrix1, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix2, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static void SlerpScale(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
		{
			Vector3 scale = matrix1.Scale;
			Vector3 scale2 = matrix2.Scale;
			if (scale.LengthSquared() < 1E-06f || scale2.LengthSquared() < 1E-06f)
			{
				result = Zero;
				return;
			}
			Matrix matrix3 = Normalize(matrix1);
			Matrix matrix4 = Normalize(matrix2);
			Quaternion.CreateFromRotationMatrix(ref matrix3, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix4, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
			Vector3 scale3 = Vector3.Lerp(scale, scale2, amount);
			Rescale(ref result, ref scale3);
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static void Slerp(Matrix matrix1, Matrix matrix2, float amount, out Matrix result)
		{
			Slerp(ref matrix1, ref matrix2, amount, out result);
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static Matrix Slerp(Matrix matrix1, Matrix matrix2, float amount)
		{
			Slerp(ref matrix1, ref matrix2, amount, out Matrix result);
			return result;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static void SlerpScale(Matrix matrix1, Matrix matrix2, float amount, out Matrix result)
		{
			SlerpScale(ref matrix1, ref matrix2, amount, out result);
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static Matrix SlerpScale(Matrix matrix1, Matrix matrix2, float amount)
		{
			SlerpScale(ref matrix1, ref matrix2, amount, out Matrix result);
			return result;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param>
		public static Matrix Negate(Matrix matrix)
		{
			Matrix result = default(Matrix);
			result.M11 = 0f - matrix.M11;
			result.M12 = 0f - matrix.M12;
			result.M13 = 0f - matrix.M13;
			result.M14 = 0f - matrix.M14;
			result.M21 = 0f - matrix.M21;
			result.M22 = 0f - matrix.M22;
			result.M23 = 0f - matrix.M23;
			result.M24 = 0f - matrix.M24;
			result.M31 = 0f - matrix.M31;
			result.M32 = 0f - matrix.M32;
			result.M33 = 0f - matrix.M33;
			result.M34 = 0f - matrix.M34;
			result.M41 = 0f - matrix.M41;
			result.M42 = 0f - matrix.M42;
			result.M43 = 0f - matrix.M43;
			result.M44 = 0f - matrix.M44;
			return result;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="result">[OutAttribute] Negated matrix.</param>
		public static void Negate(ref Matrix matrix, out Matrix result)
		{
			result.M11 = 0f - matrix.M11;
			result.M12 = 0f - matrix.M12;
			result.M13 = 0f - matrix.M13;
			result.M14 = 0f - matrix.M14;
			result.M21 = 0f - matrix.M21;
			result.M22 = 0f - matrix.M22;
			result.M23 = 0f - matrix.M23;
			result.M24 = 0f - matrix.M24;
			result.M31 = 0f - matrix.M31;
			result.M32 = 0f - matrix.M32;
			result.M33 = 0f - matrix.M33;
			result.M34 = 0f - matrix.M34;
			result.M41 = 0f - matrix.M41;
			result.M42 = 0f - matrix.M42;
			result.M43 = 0f - matrix.M43;
			result.M44 = 0f - matrix.M44;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix Add(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 + matrix2.M11;
			result.M12 = matrix1.M12 + matrix2.M12;
			result.M13 = matrix1.M13 + matrix2.M13;
			result.M14 = matrix1.M14 + matrix2.M14;
			result.M21 = matrix1.M21 + matrix2.M21;
			result.M22 = matrix1.M22 + matrix2.M22;
			result.M23 = matrix1.M23 + matrix2.M23;
			result.M24 = matrix1.M24 + matrix2.M24;
			result.M31 = matrix1.M31 + matrix2.M31;
			result.M32 = matrix1.M32 + matrix2.M32;
			result.M33 = matrix1.M33 + matrix2.M33;
			result.M34 = matrix1.M34 + matrix2.M34;
			result.M41 = matrix1.M41 + matrix2.M41;
			result.M42 = matrix1.M42 + matrix2.M42;
			result.M43 = matrix1.M43 + matrix2.M43;
			result.M44 = matrix1.M44 + matrix2.M44;
			return result;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Resulting matrix.</param>
		public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 + matrix2.M11;
			result.M12 = matrix1.M12 + matrix2.M12;
			result.M13 = matrix1.M13 + matrix2.M13;
			result.M14 = matrix1.M14 + matrix2.M14;
			result.M21 = matrix1.M21 + matrix2.M21;
			result.M22 = matrix1.M22 + matrix2.M22;
			result.M23 = matrix1.M23 + matrix2.M23;
			result.M24 = matrix1.M24 + matrix2.M24;
			result.M31 = matrix1.M31 + matrix2.M31;
			result.M32 = matrix1.M32 + matrix2.M32;
			result.M33 = matrix1.M33 + matrix2.M33;
			result.M34 = matrix1.M34 + matrix2.M34;
			result.M41 = matrix1.M41 + matrix2.M41;
			result.M42 = matrix1.M42 + matrix2.M42;
			result.M43 = matrix1.M43 + matrix2.M43;
			result.M44 = matrix1.M44 + matrix2.M44;
		}

		/// <summary>
		/// Subtracts matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 - matrix2.M11;
			result.M12 = matrix1.M12 - matrix2.M12;
			result.M13 = matrix1.M13 - matrix2.M13;
			result.M14 = matrix1.M14 - matrix2.M14;
			result.M21 = matrix1.M21 - matrix2.M21;
			result.M22 = matrix1.M22 - matrix2.M22;
			result.M23 = matrix1.M23 - matrix2.M23;
			result.M24 = matrix1.M24 - matrix2.M24;
			result.M31 = matrix1.M31 - matrix2.M31;
			result.M32 = matrix1.M32 - matrix2.M32;
			result.M33 = matrix1.M33 - matrix2.M33;
			result.M34 = matrix1.M34 - matrix2.M34;
			result.M41 = matrix1.M41 - matrix2.M41;
			result.M42 = matrix1.M42 - matrix2.M42;
			result.M43 = matrix1.M43 - matrix2.M43;
			result.M44 = matrix1.M44 - matrix2.M44;
			return result;
		}

		/// <summary>
		/// Subtracts matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the subtraction.</param>
		public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 - matrix2.M11;
			result.M12 = matrix1.M12 - matrix2.M12;
			result.M13 = matrix1.M13 - matrix2.M13;
			result.M14 = matrix1.M14 - matrix2.M14;
			result.M21 = matrix1.M21 - matrix2.M21;
			result.M22 = matrix1.M22 - matrix2.M22;
			result.M23 = matrix1.M23 - matrix2.M23;
			result.M24 = matrix1.M24 - matrix2.M24;
			result.M31 = matrix1.M31 - matrix2.M31;
			result.M32 = matrix1.M32 - matrix2.M32;
			result.M33 = matrix1.M33 - matrix2.M33;
			result.M34 = matrix1.M34 - matrix2.M34;
			result.M41 = matrix1.M41 - matrix2.M41;
			result.M42 = matrix1.M42 - matrix2.M42;
			result.M43 = matrix1.M43 - matrix2.M43;
			result.M44 = matrix1.M44 - matrix2.M44;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = (float)((double)matrix1.M11 * (double)matrix2.M11 + (double)matrix1.M12 * (double)matrix2.M21 + (double)matrix1.M13 * (double)matrix2.M31 + (double)matrix1.M14 * (double)matrix2.M41);
			result.M12 = (float)((double)matrix1.M11 * (double)matrix2.M12 + (double)matrix1.M12 * (double)matrix2.M22 + (double)matrix1.M13 * (double)matrix2.M32 + (double)matrix1.M14 * (double)matrix2.M42);
			result.M13 = (float)((double)matrix1.M11 * (double)matrix2.M13 + (double)matrix1.M12 * (double)matrix2.M23 + (double)matrix1.M13 * (double)matrix2.M33 + (double)matrix1.M14 * (double)matrix2.M43);
			result.M14 = (float)((double)matrix1.M11 * (double)matrix2.M14 + (double)matrix1.M12 * (double)matrix2.M24 + (double)matrix1.M13 * (double)matrix2.M34 + (double)matrix1.M14 * (double)matrix2.M44);
			result.M21 = (float)((double)matrix1.M21 * (double)matrix2.M11 + (double)matrix1.M22 * (double)matrix2.M21 + (double)matrix1.M23 * (double)matrix2.M31 + (double)matrix1.M24 * (double)matrix2.M41);
			result.M22 = (float)((double)matrix1.M21 * (double)matrix2.M12 + (double)matrix1.M22 * (double)matrix2.M22 + (double)matrix1.M23 * (double)matrix2.M32 + (double)matrix1.M24 * (double)matrix2.M42);
			result.M23 = (float)((double)matrix1.M21 * (double)matrix2.M13 + (double)matrix1.M22 * (double)matrix2.M23 + (double)matrix1.M23 * (double)matrix2.M33 + (double)matrix1.M24 * (double)matrix2.M43);
			result.M24 = (float)((double)matrix1.M21 * (double)matrix2.M14 + (double)matrix1.M22 * (double)matrix2.M24 + (double)matrix1.M23 * (double)matrix2.M34 + (double)matrix1.M24 * (double)matrix2.M44);
			result.M31 = (float)((double)matrix1.M31 * (double)matrix2.M11 + (double)matrix1.M32 * (double)matrix2.M21 + (double)matrix1.M33 * (double)matrix2.M31 + (double)matrix1.M34 * (double)matrix2.M41);
			result.M32 = (float)((double)matrix1.M31 * (double)matrix2.M12 + (double)matrix1.M32 * (double)matrix2.M22 + (double)matrix1.M33 * (double)matrix2.M32 + (double)matrix1.M34 * (double)matrix2.M42);
			result.M33 = (float)((double)matrix1.M31 * (double)matrix2.M13 + (double)matrix1.M32 * (double)matrix2.M23 + (double)matrix1.M33 * (double)matrix2.M33 + (double)matrix1.M34 * (double)matrix2.M43);
			result.M34 = (float)((double)matrix1.M31 * (double)matrix2.M14 + (double)matrix1.M32 * (double)matrix2.M24 + (double)matrix1.M33 * (double)matrix2.M34 + (double)matrix1.M34 * (double)matrix2.M44);
			result.M41 = (float)((double)matrix1.M41 * (double)matrix2.M11 + (double)matrix1.M42 * (double)matrix2.M21 + (double)matrix1.M43 * (double)matrix2.M31 + (double)matrix1.M44 * (double)matrix2.M41);
			result.M42 = (float)((double)matrix1.M41 * (double)matrix2.M12 + (double)matrix1.M42 * (double)matrix2.M22 + (double)matrix1.M43 * (double)matrix2.M32 + (double)matrix1.M44 * (double)matrix2.M42);
			result.M43 = (float)((double)matrix1.M41 * (double)matrix2.M13 + (double)matrix1.M42 * (double)matrix2.M23 + (double)matrix1.M43 * (double)matrix2.M33 + (double)matrix1.M44 * (double)matrix2.M43);
			result.M44 = (float)((double)matrix1.M41 * (double)matrix2.M14 + (double)matrix1.M42 * (double)matrix2.M24 + (double)matrix1.M43 * (double)matrix2.M34 + (double)matrix1.M44 * (double)matrix2.M44);
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the multiplication.</param>
		public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			float m = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			float m2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			float m3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			float m4 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			float m5 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			float m6 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			float m7 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			float m8 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			float m9 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			float m10 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			float m11 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			float m12 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			float m13 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			float m14 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			float m15 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			float m16 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
			result.M11 = m;
			result.M12 = m2;
			result.M13 = m3;
			result.M14 = m4;
			result.M21 = m5;
			result.M22 = m6;
			result.M23 = m7;
			result.M24 = m8;
			result.M31 = m9;
			result.M32 = m10;
			result.M33 = m11;
			result.M34 = m12;
			result.M41 = m13;
			result.M42 = m14;
			result.M43 = m15;
			result.M44 = m16;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix, only rotation parts.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the multiplication.</param>
		public static void MultiplyRotation(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
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
			result.M14 = 0f;
			result.M21 = m4;
			result.M22 = m5;
			result.M23 = m6;
			result.M24 = 0f;
			result.M31 = m7;
			result.M32 = m8;
			result.M33 = m9;
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="scaleFactor">Scalar value.</param>
		public static Matrix Multiply(Matrix matrix1, float scaleFactor)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 * scaleFactor;
			result.M12 = matrix1.M12 * scaleFactor;
			result.M13 = matrix1.M13 * scaleFactor;
			result.M14 = matrix1.M14 * scaleFactor;
			result.M21 = matrix1.M21 * scaleFactor;
			result.M22 = matrix1.M22 * scaleFactor;
			result.M23 = matrix1.M23 * scaleFactor;
			result.M24 = matrix1.M24 * scaleFactor;
			result.M31 = matrix1.M31 * scaleFactor;
			result.M32 = matrix1.M32 * scaleFactor;
			result.M33 = matrix1.M33 * scaleFactor;
			result.M34 = matrix1.M34 * scaleFactor;
			result.M41 = matrix1.M41 * scaleFactor;
			result.M42 = matrix1.M42 * scaleFactor;
			result.M43 = matrix1.M43 * scaleFactor;
			result.M44 = matrix1.M44 * scaleFactor;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="scaleFactor">Scalar value.</param><param name="result">[OutAttribute] The result of the multiplication.</param>
		public static void Multiply(ref Matrix matrix1, float scaleFactor, out Matrix result)
		{
			result.M11 = matrix1.M11 * scaleFactor;
			result.M12 = matrix1.M12 * scaleFactor;
			result.M13 = matrix1.M13 * scaleFactor;
			result.M14 = matrix1.M14 * scaleFactor;
			result.M21 = matrix1.M21 * scaleFactor;
			result.M22 = matrix1.M22 * scaleFactor;
			result.M23 = matrix1.M23 * scaleFactor;
			result.M24 = matrix1.M24 * scaleFactor;
			result.M31 = matrix1.M31 * scaleFactor;
			result.M32 = matrix1.M32 * scaleFactor;
			result.M33 = matrix1.M33 * scaleFactor;
			result.M34 = matrix1.M34 * scaleFactor;
			result.M41 = matrix1.M41 * scaleFactor;
			result.M42 = matrix1.M42 * scaleFactor;
			result.M43 = matrix1.M43 * scaleFactor;
			result.M44 = matrix1.M44 * scaleFactor;
		}

		/// <summary>
		/// Divides the components of a matrix by the corresponding components of another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">The divisor.</param>
		public static Matrix Divide(Matrix matrix1, Matrix matrix2)
		{
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 / matrix2.M11;
			result.M12 = matrix1.M12 / matrix2.M12;
			result.M13 = matrix1.M13 / matrix2.M13;
			result.M14 = matrix1.M14 / matrix2.M14;
			result.M21 = matrix1.M21 / matrix2.M21;
			result.M22 = matrix1.M22 / matrix2.M22;
			result.M23 = matrix1.M23 / matrix2.M23;
			result.M24 = matrix1.M24 / matrix2.M24;
			result.M31 = matrix1.M31 / matrix2.M31;
			result.M32 = matrix1.M32 / matrix2.M32;
			result.M33 = matrix1.M33 / matrix2.M33;
			result.M34 = matrix1.M34 / matrix2.M34;
			result.M41 = matrix1.M41 / matrix2.M41;
			result.M42 = matrix1.M42 / matrix2.M42;
			result.M43 = matrix1.M43 / matrix2.M43;
			result.M44 = matrix1.M44 / matrix2.M44;
			return result;
		}

		/// <summary>
		/// Divides the components of a matrix by the corresponding components of another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
		{
			result.M11 = matrix1.M11 / matrix2.M11;
			result.M12 = matrix1.M12 / matrix2.M12;
			result.M13 = matrix1.M13 / matrix2.M13;
			result.M14 = matrix1.M14 / matrix2.M14;
			result.M21 = matrix1.M21 / matrix2.M21;
			result.M22 = matrix1.M22 / matrix2.M22;
			result.M23 = matrix1.M23 / matrix2.M23;
			result.M24 = matrix1.M24 / matrix2.M24;
			result.M31 = matrix1.M31 / matrix2.M31;
			result.M32 = matrix1.M32 / matrix2.M32;
			result.M33 = matrix1.M33 / matrix2.M33;
			result.M34 = matrix1.M34 / matrix2.M34;
			result.M41 = matrix1.M41 / matrix2.M41;
			result.M42 = matrix1.M42 / matrix2.M42;
			result.M43 = matrix1.M43 / matrix2.M43;
			result.M44 = matrix1.M44 / matrix2.M44;
		}

		/// <summary>
		/// Divides the components of a matrix by a scalar.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="divider">The divisor.</param>
		public static Matrix Divide(Matrix matrix1, float divider)
		{
			float num = 1f / divider;
			Matrix result = default(Matrix);
			result.M11 = matrix1.M11 * num;
			result.M12 = matrix1.M12 * num;
			result.M13 = matrix1.M13 * num;
			result.M14 = matrix1.M14 * num;
			result.M21 = matrix1.M21 * num;
			result.M22 = matrix1.M22 * num;
			result.M23 = matrix1.M23 * num;
			result.M24 = matrix1.M24 * num;
			result.M31 = matrix1.M31 * num;
			result.M32 = matrix1.M32 * num;
			result.M33 = matrix1.M33 * num;
			result.M34 = matrix1.M34 * num;
			result.M41 = matrix1.M41 * num;
			result.M42 = matrix1.M42 * num;
			result.M43 = matrix1.M43 * num;
			result.M44 = matrix1.M44 * num;
			return result;
		}

		/// <summary>
		/// Divides the components of a matrix by a scalar.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="divider">The divisor.</param><param name="result">[OutAttribute] Result of the division.</param>
		public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
		{
			float num = 1f / divider;
			result.M11 = matrix1.M11 * num;
			result.M12 = matrix1.M12 * num;
			result.M13 = matrix1.M13 * num;
			result.M14 = matrix1.M14 * num;
			result.M21 = matrix1.M21 * num;
			result.M22 = matrix1.M22 * num;
			result.M23 = matrix1.M23 * num;
			result.M24 = matrix1.M24 * num;
			result.M31 = matrix1.M31 * num;
			result.M32 = matrix1.M32 * num;
			result.M33 = matrix1.M33 * num;
			result.M34 = matrix1.M34 * num;
			result.M41 = matrix1.M41 * num;
			result.M42 = matrix1.M42 * num;
			result.M43 = matrix1.M43 * num;
			result.M44 = matrix1.M44 * num;
		}

		/// <summary>
		/// Gets the orientation.
		/// </summary>
		/// <returns></returns>
		public Matrix GetOrientation()
		{
			Matrix identity = Identity;
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
			return (M11 + M12 + M13 + M14 + M21 + M22 + M23 + M24 + M31 + M32 + M33 + M34 + M41 + M42 + M43 + M44).IsValid();
		}

		public bool IsNan()
		{
			return float.IsNaN(M11 + M12 + M13 + M14 + M21 + M22 + M23 + M24 + M31 + M32 + M33 + M34 + M41 + M42 + M43 + M44);
		}

		public bool IsRotation()
		{
			float num = 0.01f;
			if (!HasNoTranslationOrPerspective())
			{
				return false;
			}
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

		/// <summary>
		/// Returns true if this matrix represents invertible (you can call Invert on it) linear (it does not contain translation or perspective transformation) transformation.
		/// Such matrix consist solely of rotations, shearing, mirroring and scaling. It can be orthogonalized to create an orthogonal rotation matrix.
		/// </summary>
		public bool HasNoTranslationOrPerspective()
		{
			float num = 0.0001f;
			if (M41 + M42 + M43 + M34 + M24 + M14 > num)
			{
				return false;
			}
			if (Math.Abs(M44 - 1f) > num)
			{
				return false;
			}
			return true;
		}

		public static Matrix CreateFromDir(Vector3 dir)
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
			Matrix identity = Identity;
			identity.Right = value;
			identity.Up = up;
			identity.Forward = dir;
			return identity;
		}

		public static Matrix CreateFromDir(Vector3 dir, Vector3 suggestedUp)
		{
			Vector3 up = Vector3.Cross(Vector3.Cross(dir, suggestedUp), dir);
			return CreateWorld(Vector3.Zero, dir, up);
		}

		public static Matrix Normalize(Matrix matrix)
		{
			Matrix result = matrix;
			result.Right = Vector3.Normalize(result.Right);
			result.Up = Vector3.Normalize(result.Up);
			result.Forward = Vector3.Normalize(result.Forward);
			return result;
		}

		public static Matrix Orthogonalize(Matrix rotationMatrix)
		{
			Matrix result = rotationMatrix;
			result.Right = Vector3.Normalize(result.Right);
			result.Up = Vector3.Normalize(result.Up - result.Right * result.Up.Dot(result.Right));
			result.Backward = Vector3.Normalize(result.Backward - result.Right * result.Backward.Dot(result.Right) - result.Up * result.Backward.Dot(result.Up));
			return result;
		}

		public static Matrix Round(ref Matrix matrix)
		{
			Matrix result = matrix;
			result.Right = Vector3I.Round(result.Right);
			result.Up = Vector3I.Round(result.Up);
			result.Forward = Vector3I.Round(result.Forward);
			return result;
		}

		public static Matrix AlignRotationToAxes(ref Matrix toAlign, ref Matrix axisDefinitionMatrix)
		{
			Matrix identity = Identity;
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

		public static bool GetEulerAnglesXYZ(ref Matrix mat, out Vector3 xyz)
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

		public static Matrix SwapYZCoordinates(Matrix m)
		{
			return m * CreateRotationX(MathHelper.ToRadians(-90f));
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
