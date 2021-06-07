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
	public struct MatrixD : IEquatable<MatrixD>
	{
		private struct F16
		{
			public unsafe fixed double data[16];
		}

		protected class VRageMath_MatrixD_003C_003EM11_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M11 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M11;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM12_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M12 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M12;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM13_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M13 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M13;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM14_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M14 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M14;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM21_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M21 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M21;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM22_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M22 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M22;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM23_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M23 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M23;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM24_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M24 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M24;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM31_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M31 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M31;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM32_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M32 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M32;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM33_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M33 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M33;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM34_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M34 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M34;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM41_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M41 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M41;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM42_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M42 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M42;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM43_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M43 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M43;
			}
		}

		protected class VRageMath_MatrixD_003C_003EM44_003C_003EAccessor : IMemberAccessor<MatrixD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in double value)
			{
				owner.M44 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out double value)
			{
				value = owner.M44;
			}
		}

		protected class VRageMath_MatrixD_003C_003EUp_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Up;
			}
		}

		protected class VRageMath_MatrixD_003C_003EDown_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Down = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Down;
			}
		}

		protected class VRageMath_MatrixD_003C_003ERight_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Right = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Right;
			}
		}

		protected class VRageMath_MatrixD_003C_003ELeft_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Left = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Left;
			}
		}

		protected class VRageMath_MatrixD_003C_003EForward_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Forward;
			}
		}

		protected class VRageMath_MatrixD_003C_003EBackward_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Backward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Backward;
			}
		}

		protected class VRageMath_MatrixD_003C_003ETranslation_003C_003EAccessor : IMemberAccessor<MatrixD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MatrixD owner, in Vector3D value)
			{
				owner.Translation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MatrixD owner, out Vector3D value)
			{
				value = owner.Translation;
			}
		}

		public static MatrixD Identity;

		public static MatrixD Zero;

		/// <summary>
		/// Value at row 1 column 1 of the matrix.
		/// </summary>
		[FieldOffset(0)]
		public double M11;

		/// <summary>
		/// Value at row 1 column 2 of the matrix.
		/// </summary>
		[FieldOffset(8)]
		public double M12;

		/// <summary>
		/// Value at row 1 column 3 of the matrix.
		/// </summary>
		[FieldOffset(16)]
		public double M13;

		/// <summary>
		/// Value at row 1 column 4 of the matrix.
		/// </summary>
		[FieldOffset(24)]
		public double M14;

		/// <summary>
		/// Value at row 2 column 1 of the matrix.
		/// </summary>
		[FieldOffset(32)]
		public double M21;

		/// <summary>
		/// Value at row 2 column 2 of the matrix.
		/// </summary>
		[FieldOffset(40)]
		public double M22;

		/// <summary>
		/// Value at row 2 column 3 of the matrix.
		/// </summary>
		[FieldOffset(48)]
		public double M23;

		/// <summary>
		/// Value at row 2 column 4 of the matrix.
		/// </summary>
		[FieldOffset(56)]
		public double M24;

		/// <summary>
		/// Value at row 3 column 1 of the matrix.
		/// </summary>
		[FieldOffset(64)]
		public double M31;

		/// <summary>
		/// Value at row 3 column 2 of the matrix.
		/// </summary>
		[FieldOffset(72)]
		public double M32;

		/// <summary>
		/// Value at row 3 column 3 of the matrix.
		/// </summary>
		[FieldOffset(80)]
		public double M33;

		/// <summary>
		/// Value at row 3 column 4 of the matrix.
		/// </summary>
		[FieldOffset(88)]
		public double M34;

		/// <summary>
		/// Value at row 4 column 1 of the matrix.
		/// </summary>
		[FieldOffset(96)]
		public double M41;

		/// <summary>
		/// Value at row 4 column 2 of the matrix.
		/// </summary>
		[FieldOffset(104)]
		public double M42;

		/// <summary>
		/// Value at row 4 column 3 of the matrix.
		/// </summary>
		[FieldOffset(112)]
		public double M43;

		/// <summary>
		/// Value at row 4 column 4 of the matrix.
		/// </summary>
		[FieldOffset(120)]
		public double M44;

		public Vector3D Col0
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = M11;
				result.Y = M21;
				result.Z = M31;
				return result;
			}
		}

		public Vector3D Col1
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = M12;
				result.Y = M22;
				result.Z = M32;
				return result;
			}
		}

		public Vector3D Col2
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = M13;
				result.Y = M23;
				result.Z = M33;
				return result;
			}
		}

		/// <summary>
		/// Gets and sets the up vector of the Matrix.
		/// </summary>
		public Vector3D Up
		{
			get
			{
				Vector3D result = default(Vector3D);
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
		public Vector3D Down
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = 0.0 - M21;
				result.Y = 0.0 - M22;
				result.Z = 0.0 - M23;
				return result;
			}
			set
			{
				M21 = 0.0 - value.X;
				M22 = 0.0 - value.Y;
				M23 = 0.0 - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the right vector of the Matrix.
		/// </summary>
		public Vector3D Right
		{
			get
			{
				Vector3D result = default(Vector3D);
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

		/// <summary>
		/// Gets and sets the left vector of the Matrix.
		/// </summary>
		public Vector3D Left
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = 0.0 - M11;
				result.Y = 0.0 - M12;
				result.Z = 0.0 - M13;
				return result;
			}
			set
			{
				M11 = 0.0 - value.X;
				M12 = 0.0 - value.Y;
				M13 = 0.0 - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the forward vector of the Matrix.
		/// </summary>
		public Vector3D Forward
		{
			get
			{
				Vector3D result = default(Vector3D);
				result.X = 0.0 - M31;
				result.Y = 0.0 - M32;
				result.Z = 0.0 - M33;
				return result;
			}
			set
			{
				M31 = 0.0 - value.X;
				M32 = 0.0 - value.Y;
				M33 = 0.0 - value.Z;
			}
		}

		/// <summary>
		/// Gets and sets the backward vector of the Matrix.
		/// </summary>
		public Vector3D Backward
		{
			get
			{
				Vector3D result = default(Vector3D);
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

		public Vector3D Scale => new Vector3D(Right.Length(), Up.Length(), Forward.Length());

		/// <summary>
		/// Gets and sets the translation vector of the Matrix.
		/// </summary>
		public Vector3D Translation
		{
			get
			{
				Vector3D result = default(Vector3D);
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

		public Matrix3x3 Rotation => new Matrix3x3((float)M11, (float)M12, (float)M13, (float)M21, (float)M22, (float)M23, (float)M31, (float)M32, (float)M33);

		public unsafe double this[int row, int column]
		{
			get
			{
				fixed (double* ptr = &M11)
				{
					return ptr[row * 4 + column];
				}
			}
			set
			{
				fixed (double* ptr = &M11)
				{
					ptr[row * 4 + column] = value;
				}
			}
		}

		/// <summary>
		/// Gets the base vector of the matrix, corresponding to the given direction
		/// </summary>
		public Vector3D GetDirectionVector(Base6Directions.Direction direction)
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
				return Vector3D.Zero;
			}
		}

		/// <summary>
		/// Sets the base vector of the matrix, corresponding to the given direction
		/// </summary>
		public void SetDirectionVector(Base6Directions.Direction direction, Vector3D newValue)
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

		public Base6Directions.Direction GetClosestDirection(Vector3D referenceVector)
		{
			return GetClosestDirection(ref referenceVector);
		}

		public Base6Directions.Direction GetClosestDirection(ref Vector3D referenceVector)
		{
			double num = Vector3D.Dot(referenceVector, Right);
			double num2 = Vector3D.Dot(referenceVector, Up);
			double num3 = Vector3D.Dot(referenceVector, Backward);
			double num4 = Math.Abs(num);
			double num5 = Math.Abs(num2);
			double num6 = Math.Abs(num3);
			if (num4 > num5)
			{
				if (num4 > num6)
				{
					if (num > 0.0)
					{
						return Base6Directions.Direction.Right;
					}
					return Base6Directions.Direction.Left;
				}
				if (num3 > 0.0)
				{
					return Base6Directions.Direction.Backward;
				}
				return Base6Directions.Direction.Forward;
			}
			if (num5 > num6)
			{
				if (num2 > 0.0)
				{
					return Base6Directions.Direction.Up;
				}
				return Base6Directions.Direction.Down;
			}
			if (num3 > 0.0)
			{
				return Base6Directions.Direction.Backward;
			}
			return Base6Directions.Direction.Forward;
		}

		/// <summary>
		/// Same result as Matrix.CreateScale(scale) * matrix, but much faster
		/// </summary>
		public static void Rescale(ref MatrixD matrix, double scale)
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
		public static void Rescale(ref MatrixD matrix, float scale)
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
		public static void Rescale(ref MatrixD matrix, ref Vector3D scale)
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

		public static MatrixD Rescale(MatrixD matrix, double scale)
		{
			Rescale(ref matrix, scale);
			return matrix;
		}

		public static MatrixD Rescale(MatrixD matrix, Vector3D scale)
		{
			Rescale(ref matrix, ref scale);
			return matrix;
		}

		static MatrixD()
		{
			Identity = new MatrixD(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
			Zero = new MatrixD(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
		}

		/// <summary>
		/// Initializes a new instance of Matrix.
		/// </summary>
		/// <param name="m11">Value to initialize m11 to.</param><param name="m12">Value to initialize m12 to.</param><param name="m13">Value to initialize m13 to.</param><param name="m14">Value to initialize m14 to.</param><param name="m21">Value to initialize m21 to.</param><param name="m22">Value to initialize m22 to.</param><param name="m23">Value to initialize m23 to.</param><param name="m24">Value to initialize m24 to.</param><param name="m31">Value to initialize m31 to.</param><param name="m32">Value to initialize m32 to.</param><param name="m33">Value to initialize m33 to.</param><param name="m34">Value to initialize m34 to.</param><param name="m41">Value to initialize m41 to.</param><param name="m42">Value to initialize m42 to.</param><param name="m43">Value to initialize m43 to.</param><param name="m44">Value to initialize m44 to.</param>
		public MatrixD(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
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
		public MatrixD(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
		{
			M11 = m11;
			M12 = m12;
			M13 = m13;
			M14 = 0.0;
			M21 = m21;
			M22 = m22;
			M23 = m23;
			M24 = 0.0;
			M31 = m31;
			M32 = m32;
			M33 = m33;
			M34 = 0.0;
			M41 = 0.0;
			M42 = 0.0;
			M43 = 0.0;
			M44 = 1.0;
		}

		public MatrixD(Matrix m)
		{
			M11 = m.M11;
			M12 = m.M12;
			M13 = m.M13;
			M14 = m.M14;
			M21 = m.M21;
			M22 = m.M22;
			M23 = m.M23;
			M24 = m.M24;
			M31 = m.M31;
			M32 = m.M32;
			M33 = m.M33;
			M34 = m.M34;
			M41 = m.M41;
			M42 = m.M42;
			M43 = m.M43;
			M44 = m.M44;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param>
		public static MatrixD operator -(MatrixD matrix1)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 0.0 - matrix1.M11;
			result.M12 = 0.0 - matrix1.M12;
			result.M13 = 0.0 - matrix1.M13;
			result.M14 = 0.0 - matrix1.M14;
			result.M21 = 0.0 - matrix1.M21;
			result.M22 = 0.0 - matrix1.M22;
			result.M23 = 0.0 - matrix1.M23;
			result.M24 = 0.0 - matrix1.M24;
			result.M31 = 0.0 - matrix1.M31;
			result.M32 = 0.0 - matrix1.M32;
			result.M33 = 0.0 - matrix1.M33;
			result.M34 = 0.0 - matrix1.M34;
			result.M41 = 0.0 - matrix1.M41;
			result.M42 = 0.0 - matrix1.M42;
			result.M43 = 0.0 - matrix1.M43;
			result.M44 = 0.0 - matrix1.M44;
			return result;
		}

		/// <summary>
		/// Compares a matrix for equality with another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static bool operator ==(MatrixD matrix1, MatrixD matrix2)
		{
			if (matrix1.M11 == matrix2.M11 && matrix1.M22 == matrix2.M22 && matrix1.M33 == matrix2.M33 && matrix1.M44 == matrix2.M44 && matrix1.M12 == matrix2.M12 && matrix1.M13 == matrix2.M13 && matrix1.M14 == matrix2.M14 && matrix1.M21 == matrix2.M21 && matrix1.M23 == matrix2.M23 && matrix1.M24 == matrix2.M24 && matrix1.M31 == matrix2.M31 && matrix1.M32 == matrix2.M32 && matrix1.M34 == matrix2.M34 && matrix1.M41 == matrix2.M41 && matrix1.M42 == matrix2.M42)
			{
				return matrix1.M43 == matrix2.M43;
			}
			return false;
		}

		/// <summary>
		/// Tests a matrix for inequality with another matrix.
		/// </summary>
		/// <param name="matrix1">The matrix on the left of the equal sign.</param><param name="matrix2">The matrix on the right of the equal sign.</param>
		public static bool operator !=(MatrixD matrix1, MatrixD matrix2)
		{
			if (matrix1.M11 == matrix2.M11 && matrix1.M12 == matrix2.M12 && matrix1.M13 == matrix2.M13 && matrix1.M14 == matrix2.M14 && matrix1.M21 == matrix2.M21 && matrix1.M22 == matrix2.M22 && matrix1.M23 == matrix2.M23 && matrix1.M24 == matrix2.M24 && matrix1.M31 == matrix2.M31 && matrix1.M32 == matrix2.M32 && matrix1.M33 == matrix2.M33 && matrix1.M34 == matrix2.M34 && matrix1.M41 == matrix2.M41 && matrix1.M42 == matrix2.M42 && matrix1.M43 == matrix2.M43)
			{
				return matrix1.M44 != matrix2.M44;
			}
			return true;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static MatrixD operator +(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
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
		public static MatrixD operator -(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
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
		public static MatrixD operator *(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
			result.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			result.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			result.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			result.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			result.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			result.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			result.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			result.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			result.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			result.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			result.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			result.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			result.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			result.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			result.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			result.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static MatrixD operator *(MatrixD matrix1, Matrix matrix2)
		{
			MatrixD result = default(MatrixD);
			result.M11 = matrix1.M11 * (double)matrix2.M11 + matrix1.M12 * (double)matrix2.M21 + matrix1.M13 * (double)matrix2.M31 + matrix1.M14 * (double)matrix2.M41;
			result.M12 = matrix1.M11 * (double)matrix2.M12 + matrix1.M12 * (double)matrix2.M22 + matrix1.M13 * (double)matrix2.M32 + matrix1.M14 * (double)matrix2.M42;
			result.M13 = matrix1.M11 * (double)matrix2.M13 + matrix1.M12 * (double)matrix2.M23 + matrix1.M13 * (double)matrix2.M33 + matrix1.M14 * (double)matrix2.M43;
			result.M14 = matrix1.M11 * (double)matrix2.M14 + matrix1.M12 * (double)matrix2.M24 + matrix1.M13 * (double)matrix2.M34 + matrix1.M14 * (double)matrix2.M44;
			result.M21 = matrix1.M21 * (double)matrix2.M11 + matrix1.M22 * (double)matrix2.M21 + matrix1.M23 * (double)matrix2.M31 + matrix1.M24 * (double)matrix2.M41;
			result.M22 = matrix1.M21 * (double)matrix2.M12 + matrix1.M22 * (double)matrix2.M22 + matrix1.M23 * (double)matrix2.M32 + matrix1.M24 * (double)matrix2.M42;
			result.M23 = matrix1.M21 * (double)matrix2.M13 + matrix1.M22 * (double)matrix2.M23 + matrix1.M23 * (double)matrix2.M33 + matrix1.M24 * (double)matrix2.M43;
			result.M24 = matrix1.M21 * (double)matrix2.M14 + matrix1.M22 * (double)matrix2.M24 + matrix1.M23 * (double)matrix2.M34 + matrix1.M24 * (double)matrix2.M44;
			result.M31 = matrix1.M31 * (double)matrix2.M11 + matrix1.M32 * (double)matrix2.M21 + matrix1.M33 * (double)matrix2.M31 + matrix1.M34 * (double)matrix2.M41;
			result.M32 = matrix1.M31 * (double)matrix2.M12 + matrix1.M32 * (double)matrix2.M22 + matrix1.M33 * (double)matrix2.M32 + matrix1.M34 * (double)matrix2.M42;
			result.M33 = matrix1.M31 * (double)matrix2.M13 + matrix1.M32 * (double)matrix2.M23 + matrix1.M33 * (double)matrix2.M33 + matrix1.M34 * (double)matrix2.M43;
			result.M34 = matrix1.M31 * (double)matrix2.M14 + matrix1.M32 * (double)matrix2.M24 + matrix1.M33 * (double)matrix2.M34 + matrix1.M34 * (double)matrix2.M44;
			result.M41 = matrix1.M41 * (double)matrix2.M11 + matrix1.M42 * (double)matrix2.M21 + matrix1.M43 * (double)matrix2.M31 + matrix1.M44 * (double)matrix2.M41;
			result.M42 = matrix1.M41 * (double)matrix2.M12 + matrix1.M42 * (double)matrix2.M22 + matrix1.M43 * (double)matrix2.M32 + matrix1.M44 * (double)matrix2.M42;
			result.M43 = matrix1.M41 * (double)matrix2.M13 + matrix1.M42 * (double)matrix2.M23 + matrix1.M43 * (double)matrix2.M33 + matrix1.M44 * (double)matrix2.M43;
			result.M44 = matrix1.M41 * (double)matrix2.M14 + matrix1.M42 * (double)matrix2.M24 + matrix1.M43 * (double)matrix2.M34 + matrix1.M44 * (double)matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static MatrixD operator *(Matrix matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
			result.M11 = (double)matrix1.M11 * matrix2.M11 + (double)matrix1.M12 * matrix2.M21 + (double)matrix1.M13 * matrix2.M31 + (double)matrix1.M14 * matrix2.M41;
			result.M12 = (double)matrix1.M11 * matrix2.M12 + (double)matrix1.M12 * matrix2.M22 + (double)matrix1.M13 * matrix2.M32 + (double)matrix1.M14 * matrix2.M42;
			result.M13 = (double)matrix1.M11 * matrix2.M13 + (double)matrix1.M12 * matrix2.M23 + (double)matrix1.M13 * matrix2.M33 + (double)matrix1.M14 * matrix2.M43;
			result.M14 = (double)matrix1.M11 * matrix2.M14 + (double)matrix1.M12 * matrix2.M24 + (double)matrix1.M13 * matrix2.M34 + (double)matrix1.M14 * matrix2.M44;
			result.M21 = (double)matrix1.M21 * matrix2.M11 + (double)matrix1.M22 * matrix2.M21 + (double)matrix1.M23 * matrix2.M31 + (double)matrix1.M24 * matrix2.M41;
			result.M22 = (double)matrix1.M21 * matrix2.M12 + (double)matrix1.M22 * matrix2.M22 + (double)matrix1.M23 * matrix2.M32 + (double)matrix1.M24 * matrix2.M42;
			result.M23 = (double)matrix1.M21 * matrix2.M13 + (double)matrix1.M22 * matrix2.M23 + (double)matrix1.M23 * matrix2.M33 + (double)matrix1.M24 * matrix2.M43;
			result.M24 = (double)matrix1.M21 * matrix2.M14 + (double)matrix1.M22 * matrix2.M24 + (double)matrix1.M23 * matrix2.M34 + (double)matrix1.M24 * matrix2.M44;
			result.M31 = (double)matrix1.M31 * matrix2.M11 + (double)matrix1.M32 * matrix2.M21 + (double)matrix1.M33 * matrix2.M31 + (double)matrix1.M34 * matrix2.M41;
			result.M32 = (double)matrix1.M31 * matrix2.M12 + (double)matrix1.M32 * matrix2.M22 + (double)matrix1.M33 * matrix2.M32 + (double)matrix1.M34 * matrix2.M42;
			result.M33 = (double)matrix1.M31 * matrix2.M13 + (double)matrix1.M32 * matrix2.M23 + (double)matrix1.M33 * matrix2.M33 + (double)matrix1.M34 * matrix2.M43;
			result.M34 = (double)matrix1.M31 * matrix2.M14 + (double)matrix1.M32 * matrix2.M24 + (double)matrix1.M33 * matrix2.M34 + (double)matrix1.M34 * matrix2.M44;
			result.M41 = (double)matrix1.M41 * matrix2.M11 + (double)matrix1.M42 * matrix2.M21 + (double)matrix1.M43 * matrix2.M31 + (double)matrix1.M44 * matrix2.M41;
			result.M42 = (double)matrix1.M41 * matrix2.M12 + (double)matrix1.M42 * matrix2.M22 + (double)matrix1.M43 * matrix2.M32 + (double)matrix1.M44 * matrix2.M42;
			result.M43 = (double)matrix1.M41 * matrix2.M13 + (double)matrix1.M42 * matrix2.M23 + (double)matrix1.M43 * matrix2.M33 + (double)matrix1.M44 * matrix2.M43;
			result.M44 = (double)matrix1.M41 * matrix2.M14 + (double)matrix1.M42 * matrix2.M24 + (double)matrix1.M43 * matrix2.M34 + (double)matrix1.M44 * matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="scaleFactor">Scalar value.</param>
		public static MatrixD operator *(MatrixD matrix, double scaleFactor)
		{
			MatrixD result = default(MatrixD);
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
		public static MatrixD operator *(double scaleFactor, MatrixD matrix)
		{
			MatrixD result = default(MatrixD);
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
		public static MatrixD operator /(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
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
		public static MatrixD operator /(MatrixD matrix1, double divider)
		{
			double num = 1.0 / divider;
			MatrixD result = default(MatrixD);
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
		public static MatrixD CreateBillboard(Vector3D objectPosition, Vector3D cameraPosition, Vector3D cameraUpVector, Vector3D? cameraForwardVector)
		{
			Vector3D vector = default(Vector3D);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			double num = vector.LengthSquared();
			if (num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3D.Forward);
			}
			else
			{
				Vector3D.Multiply(ref vector, 1.0 / Math.Sqrt(num), out vector);
			}
			Vector3D.Cross(ref cameraUpVector, ref vector, out Vector3D result);
			result.Normalize();
			Vector3D.Cross(ref vector, ref result, out Vector3D result2);
			MatrixD result3 = default(MatrixD);
			result3.M11 = result.X;
			result3.M12 = result.Y;
			result3.M13 = result.Z;
			result3.M14 = 0.0;
			result3.M21 = result2.X;
			result3.M22 = result2.Y;
			result3.M23 = result2.Z;
			result3.M24 = 0.0;
			result3.M31 = vector.X;
			result3.M32 = vector.Y;
			result3.M33 = vector.Z;
			result3.M34 = 0.0;
			result3.M41 = objectPosition.X;
			result3.M42 = objectPosition.Y;
			result3.M43 = objectPosition.Z;
			result3.M44 = 1.0;
			return result3;
		}

		/// <summary>
		/// Creates a spherical billboard that rotates around a specified object position.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="cameraUpVector">The up vector of the camera.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="result">[OutAttribute] The created billboard matrix.</param>
		public static void CreateBillboard(ref Vector3D objectPosition, ref Vector3D cameraPosition, ref Vector3D cameraUpVector, Vector3D? cameraForwardVector, out MatrixD result)
		{
			Vector3D vector = default(Vector3D);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			double num = vector.LengthSquared();
			if (num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3D.Forward);
			}
			else
			{
				Vector3D.Multiply(ref vector, 1.0 / Math.Sqrt(num), out vector);
			}
			Vector3D.Cross(ref cameraUpVector, ref vector, out Vector3D result2);
			result2.Normalize();
			Vector3D.Cross(ref vector, ref result2, out Vector3D result3);
			result.M11 = result2.X;
			result.M12 = result2.Y;
			result.M13 = result2.Z;
			result.M14 = 0.0;
			result.M21 = result3.X;
			result.M22 = result3.Y;
			result.M23 = result3.Z;
			result.M24 = 0.0;
			result.M31 = vector.X;
			result.M32 = vector.Y;
			result.M33 = vector.Z;
			result.M34 = 0.0;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a cylindrical billboard that rotates around a specified axis.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="rotateAxis">Axis to rotate the billboard around.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="objectForwardVector">Optional forward vector of the object.</param>
		public static MatrixD CreateConstrainedBillboard(Vector3D objectPosition, Vector3D cameraPosition, Vector3D rotateAxis, Vector3D? cameraForwardVector, Vector3D? objectForwardVector)
		{
			Vector3D vector = default(Vector3D);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			double num = vector.LengthSquared();
			if (num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3D.Forward);
			}
			else
			{
				Vector3D.Multiply(ref vector, 1.0 / Math.Sqrt(num), out vector);
			}
			Vector3D vector2 = rotateAxis;
			Vector3D.Dot(ref rotateAxis, ref vector, out double result);
			Vector3D vector3;
			Vector3D result2;
			if (Math.Abs(result) > 0.998254656791687)
			{
				if (objectForwardVector.HasValue)
				{
					vector3 = objectForwardVector.Value;
					Vector3D.Dot(ref rotateAxis, ref vector3, out result);
					if (Math.Abs(result) > 0.998254656791687)
					{
						vector3 = ((Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687) ? Vector3D.Right : Vector3D.Forward);
					}
				}
				else
				{
					vector3 = ((Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687) ? Vector3D.Right : Vector3D.Forward);
				}
				Vector3D.Cross(ref rotateAxis, ref vector3, out result2);
				result2.Normalize();
				Vector3D.Cross(ref result2, ref rotateAxis, out vector3);
				vector3.Normalize();
			}
			else
			{
				Vector3D.Cross(ref rotateAxis, ref vector, out result2);
				result2.Normalize();
				Vector3D.Cross(ref result2, ref vector2, out vector3);
				vector3.Normalize();
			}
			MatrixD result3 = default(MatrixD);
			result3.M11 = result2.X;
			result3.M12 = result2.Y;
			result3.M13 = result2.Z;
			result3.M14 = 0.0;
			result3.M21 = vector2.X;
			result3.M22 = vector2.Y;
			result3.M23 = vector2.Z;
			result3.M24 = 0.0;
			result3.M31 = vector3.X;
			result3.M32 = vector3.Y;
			result3.M33 = vector3.Z;
			result3.M34 = 0.0;
			result3.M41 = objectPosition.X;
			result3.M42 = objectPosition.Y;
			result3.M43 = objectPosition.Z;
			result3.M44 = 1.0;
			return result3;
		}

		/// <summary>
		/// Creates a cylindrical billboard that rotates around a specified axis.
		/// </summary>
		/// <param name="objectPosition">Position of the object the billboard will rotate around.</param><param name="cameraPosition">Position of the camera.</param><param name="rotateAxis">Axis to rotate the billboard around.</param><param name="cameraForwardVector">Optional forward vector of the camera.</param><param name="objectForwardVector">Optional forward vector of the object.</param><param name="result">[OutAttribute] The created billboard matrix.</param>
		public static void CreateConstrainedBillboard(ref Vector3D objectPosition, ref Vector3D cameraPosition, ref Vector3D rotateAxis, Vector3D? cameraForwardVector, Vector3D? objectForwardVector, out MatrixD result)
		{
			Vector3D vector = default(Vector3D);
			vector.X = objectPosition.X - cameraPosition.X;
			vector.Y = objectPosition.Y - cameraPosition.Y;
			vector.Z = objectPosition.Z - cameraPosition.Z;
			double num = vector.LengthSquared();
			if (num < 9.99999974737875E-05)
			{
				vector = (cameraForwardVector.HasValue ? (-cameraForwardVector.Value) : Vector3D.Forward);
			}
			else
			{
				Vector3D.Multiply(ref vector, 1.0 / Math.Sqrt(num), out vector);
			}
			Vector3D vector2 = rotateAxis;
			Vector3D.Dot(ref rotateAxis, ref vector, out double result2);
			Vector3D vector3;
			Vector3D result3;
			if (Math.Abs(result2) > 0.998254656791687)
			{
				if (objectForwardVector.HasValue)
				{
					vector3 = objectForwardVector.Value;
					Vector3D.Dot(ref rotateAxis, ref vector3, out result2);
					if (Math.Abs(result2) > 0.998254656791687)
					{
						vector3 = ((Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687) ? Vector3D.Right : Vector3D.Forward);
					}
				}
				else
				{
					vector3 = ((Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687) ? Vector3D.Right : Vector3D.Forward);
				}
				Vector3D.Cross(ref rotateAxis, ref vector3, out result3);
				result3.Normalize();
				Vector3D.Cross(ref result3, ref rotateAxis, out vector3);
				vector3.Normalize();
			}
			else
			{
				Vector3D.Cross(ref rotateAxis, ref vector, out result3);
				result3.Normalize();
				Vector3D.Cross(ref result3, ref vector2, out vector3);
				vector3.Normalize();
			}
			result.M11 = result3.X;
			result.M12 = result3.Y;
			result.M13 = result3.Z;
			result.M14 = 0.0;
			result.M21 = vector2.X;
			result.M22 = vector2.Y;
			result.M23 = vector2.Z;
			result.M24 = 0.0;
			result.M31 = vector3.X;
			result.M32 = vector3.Y;
			result.M33 = vector3.Z;
			result.M34 = 0.0;
			result.M41 = objectPosition.X;
			result.M42 = objectPosition.Y;
			result.M43 = objectPosition.Z;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="position">Amounts to translate by on the x, y, and z axes.</param>
		public static MatrixD CreateTranslation(Vector3D position)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1.0;
			return result;
		}

		public static MatrixD CreateTranslation(Vector3 position)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="position">Amounts to translate by on the x, y, and z axes.</param><param name="result">[OutAttribute] The created translation Matrix.</param>
		public static void CreateTranslation(ref Vector3D position, out MatrixD result)
		{
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="xPosition">Value to translate by on the x-axis.</param><param name="yPosition">Value to translate by on the y-axis.</param><param name="zPosition">Value to translate by on the z-axis.</param>
		public static MatrixD CreateTranslation(double xPosition, double yPosition, double zPosition)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a translation Matrix.
		/// </summary>
		/// <param name="xPosition">Value to translate by on the x-axis.</param><param name="yPosition">Value to translate by on the y-axis.</param><param name="zPosition">Value to translate by on the z-axis.</param><param name="result">[OutAttribute] The created translation Matrix.</param>
		public static void CreateTranslation(double xPosition, double yPosition, double zPosition, out MatrixD result)
		{
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = xPosition;
			result.M42 = yPosition;
			result.M43 = zPosition;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param>
		public static MatrixD CreateScale(double xScale, double yScale, double zScale)
		{
			MatrixD result = default(MatrixD);
			result.M11 = xScale;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = yScale;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = zScale;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="xScale">Value to scale by on the x-axis.</param><param name="yScale">Value to scale by on the y-axis.</param><param name="zScale">Value to scale by on the z-axis.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(double xScale, double yScale, double zScale, out MatrixD result)
		{
			result.M11 = xScale;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = yScale;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = zScale;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param>
		public static MatrixD CreateScale(Vector3D scales)
		{
			double x = scales.X;
			double y = scales.Y;
			double z = scales.Z;
			MatrixD result = default(MatrixD);
			result.M11 = x;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = y;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = z;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scales">Amounts to scale by on the x, y, and z axes.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(ref Vector3D scales, out MatrixD result)
		{
			double x = scales.X;
			double y = scales.Y;
			double z = scales.Z;
			result.M11 = x;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = y;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = z;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scale">Amount to scale by.</param>
		public static MatrixD CreateScale(double scale)
		{
			MatrixD result = default(MatrixD);
			double num = result.M11 = scale;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = num;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a scaling Matrix.
		/// </summary>
		/// <param name="scale">Value to scale by.</param><param name="result">[OutAttribute] The created scaling Matrix.</param>
		public static void CreateScale(double scale, out MatrixD result)
		{
			double num = result.M11 = scale;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = num;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static MatrixD CreateRotationX(double radians)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			MatrixD result = default(MatrixD);
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = num;
			result.M23 = num2;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0 - num2;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the x-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the x-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationX(double radians, out MatrixD result)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			result.M11 = 1.0;
			result.M12 = 0.0;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = num;
			result.M23 = num2;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0 - num2;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static MatrixD CreateRotationY(double radians)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			MatrixD result = default(MatrixD);
			result.M11 = num;
			result.M12 = 0.0;
			result.M13 = 0.0 - num2;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = num2;
			result.M32 = 0.0;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the y-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the y-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The matrix in which to place the calculated data.</param>
		public static void CreateRotationY(double radians, out MatrixD result)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			result.M11 = num;
			result.M12 = 0.0;
			result.M13 = 0.0 - num2;
			result.M14 = 0.0;
			result.M21 = 0.0;
			result.M22 = 1.0;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = num2;
			result.M32 = 0.0;
			result.M33 = num;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Returns a matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param>
		public static MatrixD CreateRotationZ(double radians)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			MatrixD result = default(MatrixD);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0 - num2;
			result.M22 = num;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Populates data into a user-specified matrix that can be used to rotate a set of vertices around the z-axis.
		/// </summary>
		/// <param name="radians">The amount, in radians, in which to rotate around the z-axis. Note that you can use ToRadians to convert degrees to radians.</param><param name="result">[OutAttribute] The rotation matrix.</param>
		public static void CreateRotationZ(double radians, out MatrixD result)
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			result.M11 = num;
			result.M12 = num2;
			result.M13 = 0.0;
			result.M14 = 0.0;
			result.M21 = 0.0 - num2;
			result.M22 = num;
			result.M23 = 0.0;
			result.M24 = 0.0;
			result.M31 = 0.0;
			result.M32 = 0.0;
			result.M33 = 1.0;
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a new Matrix that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param>
		public static MatrixD CreateFromAxisAngle(Vector3D axis, double angle)
		{
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			double num3 = x * x;
			double num4 = y * y;
			double num5 = z * z;
			double num6 = x * y;
			double num7 = x * z;
			double num8 = y * z;
			MatrixD result = default(MatrixD);
			result.M11 = num3 + num2 * (1.0 - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M14 = 0.0;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1.0 - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M24 = 0.0;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1.0 - num5);
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a new Matrix that rotates around an arbitrary vector.
		/// </summary>
		/// <param name="axis">The axis to rotate around.</param><param name="angle">The angle to rotate around the vector.</param><param name="result">[OutAttribute] The created Matrix.</param>
		public static void CreateFromAxisAngle(ref Vector3D axis, double angle, out MatrixD result)
		{
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			double num3 = x * x;
			double num4 = y * y;
			double num5 = z * z;
			double num6 = x * y;
			double num7 = x * z;
			double num8 = y * z;
			result.M11 = num3 + num2 * (1.0 - num3);
			result.M12 = num6 - num2 * num6 + num * z;
			result.M13 = num7 - num2 * num7 - num * y;
			result.M14 = 0.0;
			result.M21 = num6 - num2 * num6 - num * z;
			result.M22 = num4 + num2 * (1.0 - num4);
			result.M23 = num8 - num2 * num8 + num * x;
			result.M24 = 0.0;
			result.M31 = num7 - num2 * num7 + num * y;
			result.M32 = num8 - num2 * num8 - num * x;
			result.M33 = num5 + num2 * (1.0 - num5);
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Builds a perspective projection matrix based on a field of view and returns by value.
		/// </summary>
		/// <param name="fieldOfView">Field of view in the y direction, in radians.</param><param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param>
		public static MatrixD CreatePerspectiveFieldOfView(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
		{
			if (fieldOfView <= 0.0 || fieldOfView >= 3.14159274101257)
			{
				throw new ArgumentOutOfRangeException("fieldOfView", string.Format(CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
				{
					"fieldOfView"
				}));
			}
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			double num = 1.0 / Math.Tan(fieldOfView * 0.5);
			MatrixD result = default(MatrixD);
			double num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M31 = (result.M32 = 0.0);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1.0;
			result.M41 = (result.M42 = (result.M44 = 0.0));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			return result;
		}

		/// <summary>
		/// Builds a perspective projection matrix based on a field of view and returns by reference.
		/// </summary>
		/// <param name="fieldOfView">Field of view in the y direction, in radians.</param><param name="aspectRatio">Aspect ratio, defined as view space width divided by height. To match the aspect ratio of the viewport, the property AspectRatio.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param><param name="result">[OutAttribute] The perspective projection matrix.</param>
		public static void CreatePerspectiveFieldOfView(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance, out MatrixD result)
		{
			if (fieldOfView <= 0.0 || fieldOfView >= 3.14159274101257)
			{
				throw new ArgumentOutOfRangeException("fieldOfView", string.Format(CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
				{
					"fieldOfView"
				}));
			}
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			double num = 1.0 / Math.Tan(fieldOfView * 0.5);
			double num2 = result.M11 = num / aspectRatio;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = num;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M31 = (result.M32 = 0.0);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1.0;
			result.M41 = (result.M42 = (result.M44 = 0.0));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
		}

		/// <summary>
		/// Builds a perspective projection matrix and returns the result by value.
		/// </summary>
		/// <param name="width">Width of the view volume at the near view plane.</param><param name="height">Height of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param>
		public static MatrixD CreatePerspective(double width, double height, double nearPlaneDistance, double farPlaneDistance)
		{
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			MatrixD result = default(MatrixD);
			result.M11 = 2.0 * nearPlaneDistance / width;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 * nearPlaneDistance / height;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M31 = (result.M32 = 0.0);
			result.M34 = -1.0;
			result.M41 = (result.M42 = (result.M44 = 0.0));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			return result;
		}

		/// <summary>
		/// Builds a perspective projection matrix and returns the result by reference.
		/// </summary>
		/// <param name="width">Width of the view volume at the near view plane.</param><param name="height">Height of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to the far view plane.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreatePerspective(double width, double height, double nearPlaneDistance, double farPlaneDistance, out MatrixD result)
		{
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			result.M11 = 2.0 * nearPlaneDistance / width;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 * nearPlaneDistance / height;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M31 = (result.M32 = 0.0);
			result.M34 = -1.0;
			result.M41 = (result.M42 = (result.M44 = 0.0));
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
		}

		/// <summary>
		/// Builds a customized, perspective projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume at the near view plane.</param><param name="right">Maximum x-value of the view volume at the near view plane.</param><param name="bottom">Minimum y-value of the view volume at the near view plane.</param><param name="top">Maximum y-value of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to of the far view plane.</param>
		public static MatrixD CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
		{
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			MatrixD result = default(MatrixD);
			result.M11 = 2.0 * nearPlaneDistance / (right - left);
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 * nearPlaneDistance / (top - bottom);
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M31 = (left + right) / (right - left);
			result.M32 = (top + bottom) / (top - bottom);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1.0;
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M41 = (result.M42 = (result.M44 = 0.0));
			return result;
		}

		/// <summary>
		/// Builds a customized, perspective projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume at the near view plane.</param><param name="right">Maximum x-value of the view volume at the near view plane.</param><param name="bottom">Minimum y-value of the view volume at the near view plane.</param><param name="top">Maximum y-value of the view volume at the near view plane.</param><param name="nearPlaneDistance">Distance to the near view plane.</param><param name="farPlaneDistance">Distance to of the far view plane.</param><param name="result">[OutAttribute] The created projection matrix.</param>
		public static void CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance, out MatrixD result)
		{
			if (nearPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"nearPlaneDistance"
				}));
			}
			if (farPlaneDistance <= 0.0)
			{
				throw new ArgumentOutOfRangeException("farPlaneDistance", string.Format(CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
				{
					"farPlaneDistance"
				}));
			}
			if (nearPlaneDistance >= farPlaneDistance)
			{
				throw new ArgumentOutOfRangeException("nearPlaneDistance", "OppositePlanes");
			}
			result.M11 = 2.0 * nearPlaneDistance / (right - left);
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 * nearPlaneDistance / (top - bottom);
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M31 = (left + right) / (right - left);
			result.M32 = (top + bottom) / (top - bottom);
			result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M34 = -1.0;
			result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
			result.M41 = (result.M42 = (result.M44 = 0.0));
		}

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// </summary>
		/// <param name="width">Width of the view volume.</param><param name="height">Height of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param>
		public static MatrixD CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 2.0 / width;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 / height;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = 1.0 / (zNearPlane - zFarPlane);
			result.M31 = (result.M32 = (result.M34 = 0.0));
			result.M41 = (result.M42 = 0.0);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Builds an orthogonal projection matrix.
		/// </summary>
		/// <param name="width">Width of the view volume.</param><param name="height">Height of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane, out MatrixD result)
		{
			result.M11 = 2.0 / width;
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 / height;
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = 1.0 / (zNearPlane - zFarPlane);
			result.M31 = (result.M32 = (result.M34 = 0.0));
			result.M41 = (result.M42 = 0.0);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1.0;
		}

		/// <summary>
		/// Builds a customized, orthogonal projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume.</param><param name="right">Maximum x-value of the view volume.</param><param name="bottom">Minimum y-value of the view volume.</param><param name="top">Maximum y-value of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param>
		public static MatrixD CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 2.0 / (right - left);
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 / (top - bottom);
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = 1.0 / (zNearPlane - zFarPlane);
			result.M31 = (result.M32 = (result.M34 = 0.0));
			result.M41 = (left + right) / (left - right);
			result.M42 = (top + bottom) / (bottom - top);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Builds a customized, orthogonal projection matrix.
		/// </summary>
		/// <param name="left">Minimum x-value of the view volume.</param><param name="right">Maximum x-value of the view volume.</param><param name="bottom">Minimum y-value of the view volume.</param><param name="top">Maximum y-value of the view volume.</param><param name="zNearPlane">Minimum z-value of the view volume.</param><param name="zFarPlane">Maximum z-value of the view volume.</param><param name="result">[OutAttribute] The projection matrix.</param>
		public static void CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane, out MatrixD result)
		{
			result.M11 = 2.0 / (right - left);
			result.M12 = (result.M13 = (result.M14 = 0.0));
			result.M22 = 2.0 / (top - bottom);
			result.M21 = (result.M23 = (result.M24 = 0.0));
			result.M33 = 1.0 / (zNearPlane - zFarPlane);
			result.M31 = (result.M32 = (result.M34 = 0.0));
			result.M41 = (left + right) / (left - right);
			result.M42 = (top + bottom) / (bottom - top);
			result.M43 = zNearPlane / (zNearPlane - zFarPlane);
			result.M44 = 1.0;
		}

		public static MatrixD CreateLookAt(Vector3D cameraPosition, Vector3D cameraTarget, Vector3 cameraUpVector)
		{
			return CreateLookAt(cameraPosition, cameraTarget, (Vector3D)cameraUpVector);
		}

		/// <summary>
		/// Creates a view matrix.
		/// </summary>
		/// <param name="cameraPosition">The position of the camera.</param><param name="cameraTarget">The target towards which the camera is pointing.</param><param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
		public static MatrixD CreateLookAt(Vector3D cameraPosition, Vector3D cameraTarget, Vector3D cameraUpVector)
		{
			Vector3D vector3D = Vector3D.Normalize(cameraPosition - cameraTarget);
			Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D));
			Vector3D vector = Vector3D.Cross(vector3D, vector3D2);
			MatrixD result = default(MatrixD);
			result.M11 = vector3D2.X;
			result.M12 = vector.X;
			result.M13 = vector3D.X;
			result.M14 = 0.0;
			result.M21 = vector3D2.Y;
			result.M22 = vector.Y;
			result.M23 = vector3D.Y;
			result.M24 = 0.0;
			result.M31 = vector3D2.Z;
			result.M32 = vector.Z;
			result.M33 = vector3D.Z;
			result.M34 = 0.0;
			result.M41 = 0.0 - Vector3D.Dot(vector3D2, cameraPosition);
			result.M42 = 0.0 - Vector3D.Dot(vector, cameraPosition);
			result.M43 = 0.0 - Vector3D.Dot(vector3D, cameraPosition);
			result.M44 = 1.0;
			return result;
		}

		public static Matrix CreateLookAtInverse(Vector3D cameraPosition, Vector3D cameraTarget, Vector3D cameraUpVector)
		{
			Vector3D vector3D = Vector3D.Normalize(cameraPosition - cameraTarget);
			Vector3D vector = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D));
			Vector3D vector3D2 = Vector3D.Cross(vector3D, vector);
			MatrixD m = default(MatrixD);
			m.M11 = vector.X;
			m.M12 = vector.Y;
			m.M13 = vector.Z;
			m.M14 = 0.0;
			m.M21 = vector3D2.X;
			m.M22 = vector3D2.Y;
			m.M23 = vector3D2.Z;
			m.M24 = 0.0;
			m.M31 = vector3D.X;
			m.M32 = vector3D.Y;
			m.M33 = vector3D.Z;
			m.M34 = 0.0;
			m.M41 = cameraPosition.X;
			m.M42 = cameraPosition.Y;
			m.M43 = cameraPosition.Z;
			m.M44 = 1.0;
			return m;
		}

		/// <summary>
		/// Creates a view matrix.
		/// </summary>
		/// <param name="cameraPosition">The position of the camera.</param><param name="cameraTarget">The target towards which the camera is pointing.</param><param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param><param name="result">[OutAttribute] The created view matrix.</param>
		public static void CreateLookAt(ref Vector3D cameraPosition, ref Vector3D cameraTarget, ref Vector3D cameraUpVector, out MatrixD result)
		{
			Vector3D vector3D = Vector3D.Normalize(cameraPosition - cameraTarget);
			Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D));
			Vector3D vector = Vector3D.Cross(vector3D, vector3D2);
			result.M11 = vector3D2.X;
			result.M12 = vector.X;
			result.M13 = vector3D.X;
			result.M14 = 0.0;
			result.M21 = vector3D2.Y;
			result.M22 = vector.Y;
			result.M23 = vector3D.Y;
			result.M24 = 0.0;
			result.M31 = vector3D2.Z;
			result.M32 = vector.Z;
			result.M33 = vector3D.Z;
			result.M34 = 0.0;
			result.M41 = 0.0 - Vector3D.Dot(vector3D2, cameraPosition);
			result.M42 = 0.0 - Vector3D.Dot(vector, cameraPosition);
			result.M43 = 0.0 - Vector3D.Dot(vector3D, cameraPosition);
			result.M44 = 1.0;
		}

		public static MatrixD CreateWorld(Vector3D position, Vector3 forward, Vector3 up)
		{
			return CreateWorld(position, (Vector3D)forward, (Vector3D)up);
		}

		public static MatrixD CreateWorld(Vector3D position)
		{
			return CreateWorld(position, Vector3D.Forward, Vector3D.Up);
		}

		/// <summary>
		/// Creates a world matrix with the specified parameters.
		/// </summary>
		/// <param name="position">Position of the object. This value is used in translation operations.</param><param name="forward">Forward direction of the object.</param><param name="up">Upward direction of the object; usually [0, 1, 0].</param>
		public static MatrixD CreateWorld(Vector3D position, Vector3D forward, Vector3D up)
		{
			Vector3D vector3D = Vector3D.Normalize(-forward);
			Vector3D vector = Vector3D.Normalize(Vector3D.Cross(up, vector3D));
			Vector3D vector3D2 = Vector3D.Cross(vector3D, vector);
			MatrixD result = default(MatrixD);
			result.M11 = vector.X;
			result.M12 = vector.Y;
			result.M13 = vector.Z;
			result.M14 = 0.0;
			result.M21 = vector3D2.X;
			result.M22 = vector3D2.Y;
			result.M23 = vector3D2.Z;
			result.M24 = 0.0;
			result.M31 = vector3D.X;
			result.M32 = vector3D.Y;
			result.M33 = vector3D.Z;
			result.M34 = 0.0;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a world matrix with the specified parameters.
		/// </summary>
		/// <param name="position">Position of the object. This value is used in translation operations.</param><param name="forward">Forward direction of the object.</param><param name="up">Upward direction of the object; usually [0, 1, 0].</param><param name="result">[OutAttribute] The created world matrix.</param>
		public static void CreateWorld(ref Vector3D position, ref Vector3D forward, ref Vector3D up, out MatrixD result)
		{
			Vector3D vector3D = Vector3D.Normalize(-forward);
			Vector3D vector = Vector3D.Normalize(Vector3D.Cross(up, vector3D));
			Vector3D vector3D2 = Vector3D.Cross(vector3D, vector);
			result.M11 = vector.X;
			result.M12 = vector.Y;
			result.M13 = vector.Z;
			result.M14 = 0.0;
			result.M21 = vector3D2.X;
			result.M22 = vector3D2.Y;
			result.M23 = vector3D2.Z;
			result.M24 = 0.0;
			result.M31 = vector3D.X;
			result.M32 = vector3D.Y;
			result.M33 = vector3D.Z;
			result.M34 = 0.0;
			result.M41 = position.X;
			result.M42 = position.Y;
			result.M43 = position.Z;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a rotation Matrix from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix from.</param>
		public static MatrixD CreateFromQuaternion(Quaternion quaternion)
		{
			double num = quaternion.X * quaternion.X;
			double num2 = quaternion.Y * quaternion.Y;
			double num3 = quaternion.Z * quaternion.Z;
			double num4 = quaternion.X * quaternion.Y;
			double num5 = quaternion.Z * quaternion.W;
			double num6 = quaternion.Z * quaternion.X;
			double num7 = quaternion.Y * quaternion.W;
			double num8 = quaternion.Y * quaternion.Z;
			double num9 = quaternion.X * quaternion.W;
			MatrixD result = default(MatrixD);
			result.M11 = 1.0 - 2.0 * (num2 + num3);
			result.M12 = 2.0 * (num4 + num5);
			result.M13 = 2.0 * (num6 - num7);
			result.M14 = 0.0;
			result.M21 = 2.0 * (num4 - num5);
			result.M22 = 1.0 - 2.0 * (num3 + num);
			result.M23 = 2.0 * (num8 + num9);
			result.M24 = 0.0;
			result.M31 = 2.0 * (num6 + num7);
			result.M32 = 2.0 * (num8 - num9);
			result.M33 = 1.0 - 2.0 * (num2 + num);
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		public static MatrixD CreateFromQuaternion(QuaternionD quaternion)
		{
			double num = quaternion.X * quaternion.X;
			double num2 = quaternion.Y * quaternion.Y;
			double num3 = quaternion.Z * quaternion.Z;
			double num4 = quaternion.X * quaternion.Y;
			double num5 = quaternion.Z * quaternion.W;
			double num6 = quaternion.Z * quaternion.X;
			double num7 = quaternion.Y * quaternion.W;
			double num8 = quaternion.Y * quaternion.Z;
			double num9 = quaternion.X * quaternion.W;
			MatrixD result = default(MatrixD);
			result.M11 = 1.0 - 2.0 * (num2 + num3);
			result.M12 = 2.0 * (num4 + num5);
			result.M13 = 2.0 * (num6 - num7);
			result.M14 = 0.0;
			result.M21 = 2.0 * (num4 - num5);
			result.M22 = 1.0 - 2.0 * (num3 + num);
			result.M23 = 2.0 * (num8 + num9);
			result.M24 = 0.0;
			result.M31 = 2.0 * (num6 + num7);
			result.M32 = 2.0 * (num8 - num9);
			result.M33 = 1.0 - 2.0 * (num2 + num);
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Creates a rotation Matrix from a Quaternion.
		/// </summary>
		/// <param name="quaternion">Quaternion to create the Matrix from.</param><param name="result">[OutAttribute] The created Matrix.</param>
		public static void CreateFromQuaternion(ref Quaternion quaternion, out MatrixD result)
		{
			double num = quaternion.X * quaternion.X;
			double num2 = quaternion.Y * quaternion.Y;
			double num3 = quaternion.Z * quaternion.Z;
			double num4 = quaternion.X * quaternion.Y;
			double num5 = quaternion.Z * quaternion.W;
			double num6 = quaternion.Z * quaternion.X;
			double num7 = quaternion.Y * quaternion.W;
			double num8 = quaternion.Y * quaternion.Z;
			double num9 = quaternion.X * quaternion.W;
			result.M11 = 1.0 - 2.0 * (num2 + num3);
			result.M12 = 2.0 * (num4 + num5);
			result.M13 = 2.0 * (num6 - num7);
			result.M14 = 0.0;
			result.M21 = 2.0 * (num4 - num5);
			result.M22 = 1.0 - 2.0 * (num3 + num);
			result.M23 = 2.0 * (num8 + num9);
			result.M24 = 0.0;
			result.M31 = 2.0 * (num6 + num7);
			result.M32 = 2.0 * (num8 - num9);
			result.M33 = 1.0 - 2.0 * (num2 + num);
			result.M34 = 0.0;
			result.M41 = 0.0;
			result.M42 = 0.0;
			result.M43 = 0.0;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Creates a new rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param>
		public static MatrixD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
		{
			Quaternion.CreateFromYawPitchRoll((float)yaw, (float)pitch, (float)roll, out Quaternion result);
			CreateFromQuaternion(ref result, out MatrixD result2);
			return result2;
		}

		/// <summary>
		/// Fills in a rotation matrix from a specified yaw, pitch, and roll.
		/// </summary>
		/// <param name="yaw">Angle of rotation, in radians, around the y-axis.</param><param name="pitch">Angle of rotation, in radians, around the x-axis.</param><param name="roll">Angle of rotation, in radians, around the z-axis.</param><param name="result">[OutAttribute] An existing matrix filled in to represent the specified yaw, pitch, and roll.</param>
		public static void CreateFromYawPitchRoll(double yaw, double pitch, double roll, out MatrixD result)
		{
			Quaternion.CreateFromYawPitchRoll((float)yaw, (float)pitch, (float)roll, out Quaternion result2);
			CreateFromQuaternion(ref result2, out result);
		}

		public static MatrixD CreateFromTransformScale(Quaternion orientation, Vector3D position, Vector3D scale)
		{
			MatrixD matrix = CreateFromQuaternion(orientation);
			matrix.Translation = position;
			Rescale(ref matrix, ref scale);
			return matrix;
		}

		/// <summary>
		/// Creates a Matrix that flattens geometry into a specified Plane as if casting a shadow from a specified light source.
		/// </summary>
		/// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param><param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
		public static MatrixD CreateShadow(Vector3D lightDirection, Plane plane)
		{
			Plane.Normalize(ref plane, out Plane result);
			double num = (double)result.Normal.X * lightDirection.X + (double)result.Normal.Y * lightDirection.Y + (double)result.Normal.Z * lightDirection.Z;
			double num2 = 0f - result.Normal.X;
			double num3 = 0f - result.Normal.Y;
			double num4 = 0f - result.Normal.Z;
			double num5 = 0f - result.D;
			MatrixD result2 = default(MatrixD);
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
			result2.M14 = 0.0;
			result2.M24 = 0.0;
			result2.M34 = 0.0;
			result2.M44 = num;
			return result2;
		}

		/// <summary>
		/// Fills in a Matrix to flatten geometry into a specified Plane as if casting a shadow from a specified light source.
		/// </summary>
		/// <param name="lightDirection">A Vector3 specifying the direction from which the light that will cast the shadow is coming.</param><param name="plane">The Plane onto which the new matrix should flatten geometry so as to cast a shadow.</param><param name="result">[OutAttribute] A Matrix that can be used to flatten geometry onto the specified plane from the specified direction.</param>
		public static void CreateShadow(ref Vector3D lightDirection, ref Plane plane, out MatrixD result)
		{
			Plane.Normalize(ref plane, out Plane result2);
			double num = (double)result2.Normal.X * lightDirection.X + (double)result2.Normal.Y * lightDirection.Y + (double)result2.Normal.Z * lightDirection.Z;
			double num2 = 0f - result2.Normal.X;
			double num3 = 0f - result2.Normal.Y;
			double num4 = 0f - result2.Normal.Z;
			double num5 = 0f - result2.D;
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
			result.M14 = 0.0;
			result.M24 = 0.0;
			result.M34 = 0.0;
			result.M44 = num;
		}

		/// <summary>
		/// Creates a Matrix that reflects the coordinate system about a specified Plane.
		/// </summary>
		/// <param name="value">The Plane about which to create a reflection.</param>
		public static MatrixD CreateReflection(Plane value)
		{
			value.Normalize();
			double num = value.Normal.X;
			double num2 = value.Normal.Y;
			double num3 = value.Normal.Z;
			double num4 = -2.0 * num;
			double num5 = -2.0 * num2;
			double num6 = -2.0 * num3;
			MatrixD result = default(MatrixD);
			result.M11 = num4 * num + 1.0;
			result.M12 = num5 * num;
			result.M13 = num6 * num;
			result.M14 = 0.0;
			result.M21 = num4 * num2;
			result.M22 = num5 * num2 + 1.0;
			result.M23 = num6 * num2;
			result.M24 = 0.0;
			result.M31 = num4 * num3;
			result.M32 = num5 * num3;
			result.M33 = num6 * num3 + 1.0;
			result.M34 = 0.0;
			result.M41 = num4 * (double)value.D;
			result.M42 = num5 * (double)value.D;
			result.M43 = num6 * (double)value.D;
			result.M44 = 1.0;
			return result;
		}

		/// <summary>
		/// Fills in an existing Matrix so that it reflects the coordinate system about a specified Plane.
		/// </summary>
		/// <param name="value">The Plane about which to create a reflection.</param><param name="result">[OutAttribute] A Matrix that creates the reflection.</param>
		public static void CreateReflection(ref Plane value, out MatrixD result)
		{
			Plane.Normalize(ref value, out Plane result2);
			value.Normalize();
			double num = result2.Normal.X;
			double num2 = result2.Normal.Y;
			double num3 = result2.Normal.Z;
			double num4 = -2.0 * num;
			double num5 = -2.0 * num2;
			double num6 = -2.0 * num3;
			result.M11 = num4 * num + 1.0;
			result.M12 = num5 * num;
			result.M13 = num6 * num;
			result.M14 = 0.0;
			result.M21 = num4 * num2;
			result.M22 = num5 * num2 + 1.0;
			result.M23 = num6 * num2;
			result.M24 = 0.0;
			result.M31 = num4 * num3;
			result.M32 = num5 * num3;
			result.M33 = num6 * num3 + 1.0;
			result.M34 = 0.0;
			result.M41 = num4 * (double)result2.D;
			result.M42 = num5 * (double)result2.D;
			result.M43 = num6 * (double)result2.D;
			result.M44 = 1.0;
		}

		/// <summary>
		/// Transforms a Matrix by applying a Quaternion rotation.
		/// </summary>
		/// <param name="value">The Matrix to transform.</param><param name="rotation">The rotation to apply, expressed as a Quaternion.</param>
		public static MatrixD Transform(MatrixD value, Quaternion rotation)
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
			MatrixD result = default(MatrixD);
			result.M11 = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
			result.M12 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
			result.M13 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
			result.M14 = value.M14;
			result.M21 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
			result.M22 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
			result.M23 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
			result.M24 = value.M24;
			result.M31 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
			result.M32 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
			result.M33 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
			result.M34 = value.M34;
			result.M41 = value.M41 * num13 + value.M42 * num14 + value.M43 * num15;
			result.M42 = value.M41 * num16 + value.M42 * num17 + value.M43 * num18;
			result.M43 = value.M41 * num19 + value.M42 * num20 + value.M43 * num21;
			result.M44 = value.M44;
			return result;
		}

		/// <summary>
		/// Transforms a Matrix by applying a Quaternion rotation.
		/// </summary>
		/// <param name="value">The Matrix to transform.</param><param name="rotation">The rotation to apply, expressed as a Quaternion.</param><param name="result">[OutAttribute] An existing Matrix filled in with the result of the transform.</param>
		public static void Transform(ref MatrixD value, ref Quaternion rotation, out MatrixD result)
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
			double m = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
			double m2 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
			double m3 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
			double m4 = value.M14;
			double m5 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
			double m6 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
			double m7 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
			double m8 = value.M24;
			double m9 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
			double m10 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
			double m11 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
			double m12 = value.M34;
			double m13 = value.M41 * num13 + value.M42 * num14 + value.M43 * num15;
			double m14 = value.M41 * num16 + value.M42 * num17 + value.M43 * num18;
			double m15 = value.M41 * num19 + value.M42 * num20 + value.M43 * num21;
			double m16 = value.M44;
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
			fixed (double* ptr = &M11)
			{
				double* ptr2 = ptr + row * 4;
				return new Vector4((float)(*ptr2), (float)ptr2[1], (float)ptr2[2], (float)ptr2[3]);
			}
		}

		public unsafe void SetRow(int row, Vector4 value)
		{
			fixed (double* ptr = &M11)
			{
				double* intPtr = ptr + row * 4;
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
		public bool Equals(MatrixD other)
		{
			if (M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M44 == other.M44 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14 && M21 == other.M21 && M23 == other.M23 && M24 == other.M24 && M31 == other.M31 && M32 == other.M32 && M34 == other.M34 && M41 == other.M41 && M42 == other.M42)
			{
				return M43 == other.M43;
			}
			return false;
		}

		/// <summary>
		/// Compares just position, forward and up
		/// </summary>
		public bool EqualsFast(ref MatrixD other, double epsilon = 0.0001)
		{
			double num = M21 - other.M21;
			double num2 = M22 - other.M22;
			double num3 = M23 - other.M23;
			double num4 = M31 - other.M31;
			double num5 = M32 - other.M32;
			double num6 = M33 - other.M33;
			double num7 = M41 - other.M41;
			double num8 = M42 - other.M42;
			double num9 = M43 - other.M43;
			double num10 = epsilon * epsilon;
			return num * num + num2 * num2 + num3 * num3 < num10 && num4 * num4 + num5 * num5 + num6 * num6 < num10 && num7 * num7 + num8 * num8 + num9 * num9 < num10;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is MatrixD)
			{
				result = Equals((MatrixD)obj);
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
		public static MatrixD Transpose(MatrixD matrix)
		{
			MatrixD result = default(MatrixD);
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
		public static void Transpose(ref MatrixD matrix, out MatrixD result)
		{
			double m = matrix.M11;
			double m2 = matrix.M12;
			double m3 = matrix.M13;
			double m4 = matrix.M14;
			double m5 = matrix.M21;
			double m6 = matrix.M22;
			double m7 = matrix.M23;
			double m8 = matrix.M24;
			double m9 = matrix.M31;
			double m10 = matrix.M32;
			double m11 = matrix.M33;
			double m12 = matrix.M34;
			double m13 = matrix.M41;
			double m14 = matrix.M42;
			double m15 = matrix.M43;
			double m16 = matrix.M44;
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
		/// Calculates the determinant of the matrix.
		/// </summary>
		public double Determinant()
		{
			double m = M11;
			double m2 = M12;
			double m3 = M13;
			double m4 = M14;
			double m5 = M21;
			double m6 = M22;
			double m7 = M23;
			double m8 = M24;
			double m9 = M31;
			double m10 = M32;
			double m11 = M33;
			double m12 = M34;
			double m13 = M41;
			double m14 = M42;
			double m15 = M43;
			double m16 = M44;
			double num = m11 * m16 - m12 * m15;
			double num2 = m10 * m16 - m12 * m14;
			double num3 = m10 * m15 - m11 * m14;
			double num4 = m9 * m16 - m12 * m13;
			double num5 = m9 * m15 - m11 * m13;
			double num6 = m9 * m14 - m10 * m13;
			return m * (m6 * num - m7 * num2 + m8 * num3) - m2 * (m5 * num - m7 * num4 + m8 * num5) + m3 * (m5 * num2 - m6 * num4 + m8 * num6) - m4 * (m5 * num3 - m6 * num5 + m7 * num6);
		}

		/// <summary>
		/// Calculates the inverse of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param>
		public static MatrixD Invert(MatrixD matrix)
		{
			return Invert(ref matrix);
		}

		public static MatrixD Invert(ref MatrixD matrix)
		{
			double m = matrix.M11;
			double m2 = matrix.M12;
			double m3 = matrix.M13;
			double m4 = matrix.M14;
			double m5 = matrix.M21;
			double m6 = matrix.M22;
			double m7 = matrix.M23;
			double m8 = matrix.M24;
			double m9 = matrix.M31;
			double m10 = matrix.M32;
			double m11 = matrix.M33;
			double m12 = matrix.M34;
			double m13 = matrix.M41;
			double m14 = matrix.M42;
			double m15 = matrix.M43;
			double m16 = matrix.M44;
			double num = m11 * m16 - m12 * m15;
			double num2 = m10 * m16 - m12 * m14;
			double num3 = m10 * m15 - m11 * m14;
			double num4 = m9 * m16 - m12 * m13;
			double num5 = m9 * m15 - m11 * m13;
			double num6 = m9 * m14 - m10 * m13;
			double num7 = m6 * num - m7 * num2 + m8 * num3;
			double num8 = 0.0 - (m5 * num - m7 * num4 + m8 * num5);
			double num9 = m5 * num2 - m6 * num4 + m8 * num6;
			double num10 = 0.0 - (m5 * num3 - m6 * num5 + m7 * num6);
			double num11 = 1.0 / (m * num7 + m2 * num8 + m3 * num9 + m4 * num10);
			MatrixD result = default(MatrixD);
			result.M11 = num7 * num11;
			result.M21 = num8 * num11;
			result.M31 = num9 * num11;
			result.M41 = num10 * num11;
			result.M12 = (0.0 - (m2 * num - m3 * num2 + m4 * num3)) * num11;
			result.M22 = (m * num - m3 * num4 + m4 * num5) * num11;
			result.M32 = (0.0 - (m * num2 - m2 * num4 + m4 * num6)) * num11;
			result.M42 = (m * num3 - m2 * num5 + m3 * num6) * num11;
			double num12 = m7 * m16 - m8 * m15;
			double num13 = m6 * m16 - m8 * m14;
			double num14 = m6 * m15 - m7 * m14;
			double num15 = m5 * m16 - m8 * m13;
			double num16 = m5 * m15 - m7 * m13;
			double num17 = m5 * m14 - m6 * m13;
			result.M13 = (m2 * num12 - m3 * num13 + m4 * num14) * num11;
			result.M23 = (0.0 - (m * num12 - m3 * num15 + m4 * num16)) * num11;
			result.M33 = (m * num13 - m2 * num15 + m4 * num17) * num11;
			result.M43 = (0.0 - (m * num14 - m2 * num16 + m3 * num17)) * num11;
			double num18 = m7 * m12 - m8 * m11;
			double num19 = m6 * m12 - m8 * m10;
			double num20 = m6 * m11 - m7 * m10;
			double num21 = m5 * m12 - m8 * m9;
			double num22 = m5 * m11 - m7 * m9;
			double num23 = m5 * m10 - m6 * m9;
			result.M14 = (0.0 - (m2 * num18 - m3 * num19 + m4 * num20)) * num11;
			result.M24 = (m * num18 - m3 * num21 + m4 * num22) * num11;
			result.M34 = (0.0 - (m * num19 - m2 * num21 + m4 * num23)) * num11;
			result.M44 = (m * num20 - m2 * num22 + m3 * num23) * num11;
			return result;
		}

		/// <summary>
		/// Calculates the inverse of a matrix.
		/// </summary>
		/// <param name="matrix">The source matrix.</param><param name="result">[OutAttribute] The inverse of matrix. The same matrix can be used for both arguments.</param>
		public static void Invert(ref MatrixD matrix, out MatrixD result)
		{
			double m = matrix.M11;
			double m2 = matrix.M12;
			double m3 = matrix.M13;
			double m4 = matrix.M14;
			double m5 = matrix.M21;
			double m6 = matrix.M22;
			double m7 = matrix.M23;
			double m8 = matrix.M24;
			double m9 = matrix.M31;
			double m10 = matrix.M32;
			double m11 = matrix.M33;
			double m12 = matrix.M34;
			double m13 = matrix.M41;
			double m14 = matrix.M42;
			double m15 = matrix.M43;
			double m16 = matrix.M44;
			double num = m11 * m16 - m12 * m15;
			double num2 = m10 * m16 - m12 * m14;
			double num3 = m10 * m15 - m11 * m14;
			double num4 = m9 * m16 - m12 * m13;
			double num5 = m9 * m15 - m11 * m13;
			double num6 = m9 * m14 - m10 * m13;
			double num7 = m6 * num - m7 * num2 + m8 * num3;
			double num8 = 0.0 - (m5 * num - m7 * num4 + m8 * num5);
			double num9 = m5 * num2 - m6 * num4 + m8 * num6;
			double num10 = 0.0 - (m5 * num3 - m6 * num5 + m7 * num6);
			double num11 = 1.0 / (m * num7 + m2 * num8 + m3 * num9 + m4 * num10);
			result.M11 = num7 * num11;
			result.M21 = num8 * num11;
			result.M31 = num9 * num11;
			result.M41 = num10 * num11;
			result.M12 = (0.0 - (m2 * num - m3 * num2 + m4 * num3)) * num11;
			result.M22 = (m * num - m3 * num4 + m4 * num5) * num11;
			result.M32 = (0.0 - (m * num2 - m2 * num4 + m4 * num6)) * num11;
			result.M42 = (m * num3 - m2 * num5 + m3 * num6) * num11;
			double num12 = m7 * m16 - m8 * m15;
			double num13 = m6 * m16 - m8 * m14;
			double num14 = m6 * m15 - m7 * m14;
			double num15 = m5 * m16 - m8 * m13;
			double num16 = m5 * m15 - m7 * m13;
			double num17 = m5 * m14 - m6 * m13;
			result.M13 = (m2 * num12 - m3 * num13 + m4 * num14) * num11;
			result.M23 = (0.0 - (m * num12 - m3 * num15 + m4 * num16)) * num11;
			result.M33 = (m * num13 - m2 * num15 + m4 * num17) * num11;
			result.M43 = (0.0 - (m * num14 - m2 * num16 + m3 * num17)) * num11;
			double num18 = m7 * m12 - m8 * m11;
			double num19 = m6 * m12 - m8 * m10;
			double num20 = m6 * m11 - m7 * m10;
			double num21 = m5 * m12 - m8 * m9;
			double num22 = m5 * m11 - m7 * m9;
			double num23 = m5 * m10 - m6 * m9;
			result.M14 = (0.0 - (m2 * num18 - m3 * num19 + m4 * num20)) * num11;
			result.M24 = (m * num18 - m3 * num21 + m4 * num22) * num11;
			result.M34 = (0.0 - (m * num19 - m2 * num21 + m4 * num23)) * num11;
			result.M44 = (m * num20 - m2 * num22 + m3 * num23) * num11;
		}

		/// <summary>
		/// Linearly interpolates between the corresponding values of two matrices.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="amount">Interpolation value.</param>
		public static MatrixD Lerp(MatrixD matrix1, MatrixD matrix2, double amount)
		{
			MatrixD result = default(MatrixD);
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
		public static void Lerp(ref MatrixD matrix1, ref MatrixD matrix2, double amount, out MatrixD result)
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
		public static void Slerp(ref MatrixD matrix1, ref MatrixD matrix2, float amount, out MatrixD result)
		{
			Quaternion.CreateFromRotationMatrix(ref matrix1, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix2, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * (double)amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * (double)amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * (double)amount;
		}

		public bool IsOrthogonal()
		{
			double epsilon = 0.0001;
			return IsOrthogonal(epsilon);
		}

		public bool IsOrthogonal(double epsilon)
		{
			if (Math.Abs(Up.LengthSquared()) - 1.0 < epsilon && Math.Abs(Right.LengthSquared()) - 1.0 < epsilon && Math.Abs(Forward.LengthSquared()) - 1.0 < epsilon && Math.Abs(Right.Dot(Up)) < epsilon)
			{
				return Math.Abs(Right.Dot(Forward)) < epsilon;
			}
			return false;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static void SlerpScale(ref MatrixD matrix1, ref MatrixD matrix2, float amount, out MatrixD result)
		{
			Vector3D scale = matrix1.Scale;
			Vector3D scale2 = matrix2.Scale;
			if (scale.LengthSquared() < 0.0099999997764825821 || scale2.LengthSquared() < 0.0099999997764825821)
			{
				result = Zero;
				return;
			}
			MatrixD matrix3 = Normalize(matrix1);
			MatrixD matrix4 = Normalize(matrix2);
			Quaternion.CreateFromRotationMatrix(ref matrix3, out Quaternion result2);
			Quaternion.CreateFromRotationMatrix(ref matrix4, out Quaternion result3);
			Quaternion.Slerp(ref result2, ref result3, amount, out Quaternion result4);
			CreateFromQuaternion(ref result4, out result);
			Vector3D scale3 = Vector3D.Lerp(scale, scale2, amount);
			Rescale(ref result, ref scale3);
			result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * (double)amount;
			result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * (double)amount;
			result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * (double)amount;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static void Slerp(MatrixD matrix1, MatrixD matrix2, float amount, out MatrixD result)
		{
			Slerp(ref matrix1, ref matrix2, amount, out result);
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation.
		/// </summary>
		public static MatrixD Slerp(MatrixD matrix1, MatrixD matrix2, float amount)
		{
			Slerp(ref matrix1, ref matrix2, amount, out MatrixD result);
			return result;
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static void SlerpScale(MatrixD matrix1, MatrixD matrix2, float amount, out MatrixD result)
		{
			SlerpScale(ref matrix1, ref matrix2, amount, out result);
		}

		/// <summary>
		/// Performs spherical linear interpolation of position and rotation and scale.
		/// </summary>
		public static MatrixD SlerpScale(MatrixD matrix1, MatrixD matrix2, float amount)
		{
			SlerpScale(ref matrix1, ref matrix2, amount, out MatrixD result);
			return result;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param>
		public static MatrixD Negate(MatrixD matrix)
		{
			MatrixD result = default(MatrixD);
			result.M11 = 0.0 - matrix.M11;
			result.M12 = 0.0 - matrix.M12;
			result.M13 = 0.0 - matrix.M13;
			result.M14 = 0.0 - matrix.M14;
			result.M21 = 0.0 - matrix.M21;
			result.M22 = 0.0 - matrix.M22;
			result.M23 = 0.0 - matrix.M23;
			result.M24 = 0.0 - matrix.M24;
			result.M31 = 0.0 - matrix.M31;
			result.M32 = 0.0 - matrix.M32;
			result.M33 = 0.0 - matrix.M33;
			result.M34 = 0.0 - matrix.M34;
			result.M41 = 0.0 - matrix.M41;
			result.M42 = 0.0 - matrix.M42;
			result.M43 = 0.0 - matrix.M43;
			result.M44 = 0.0 - matrix.M44;
			return result;
		}

		/// <summary>
		/// Negates individual elements of a matrix.
		/// </summary>
		/// <param name="matrix">Source matrix.</param><param name="result">[OutAttribute] Negated matrix.</param>
		public static void Negate(ref MatrixD matrix, out MatrixD result)
		{
			result.M11 = 0.0 - matrix.M11;
			result.M12 = 0.0 - matrix.M12;
			result.M13 = 0.0 - matrix.M13;
			result.M14 = 0.0 - matrix.M14;
			result.M21 = 0.0 - matrix.M21;
			result.M22 = 0.0 - matrix.M22;
			result.M23 = 0.0 - matrix.M23;
			result.M24 = 0.0 - matrix.M24;
			result.M31 = 0.0 - matrix.M31;
			result.M32 = 0.0 - matrix.M32;
			result.M33 = 0.0 - matrix.M33;
			result.M34 = 0.0 - matrix.M34;
			result.M41 = 0.0 - matrix.M41;
			result.M42 = 0.0 - matrix.M42;
			result.M43 = 0.0 - matrix.M43;
			result.M44 = 0.0 - matrix.M44;
		}

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static MatrixD Add(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
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
		public static void Add(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
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
		public static void Subtract(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
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
		public static MatrixD Multiply(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
			result.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			result.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			result.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			result.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			result.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			result.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			result.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			result.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			result.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			result.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			result.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			result.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			result.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			result.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			result.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			result.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param>
		public static MatrixD Multiply(MatrixD matrix1, Matrix matrix2)
		{
			MatrixD result = default(MatrixD);
			result.M11 = matrix1.M11 * (double)matrix2.M11 + matrix1.M12 * (double)matrix2.M21 + matrix1.M13 * (double)matrix2.M31 + matrix1.M14 * (double)matrix2.M41;
			result.M12 = matrix1.M11 * (double)matrix2.M12 + matrix1.M12 * (double)matrix2.M22 + matrix1.M13 * (double)matrix2.M32 + matrix1.M14 * (double)matrix2.M42;
			result.M13 = matrix1.M11 * (double)matrix2.M13 + matrix1.M12 * (double)matrix2.M23 + matrix1.M13 * (double)matrix2.M33 + matrix1.M14 * (double)matrix2.M43;
			result.M14 = matrix1.M11 * (double)matrix2.M14 + matrix1.M12 * (double)matrix2.M24 + matrix1.M13 * (double)matrix2.M34 + matrix1.M14 * (double)matrix2.M44;
			result.M21 = matrix1.M21 * (double)matrix2.M11 + matrix1.M22 * (double)matrix2.M21 + matrix1.M23 * (double)matrix2.M31 + matrix1.M24 * (double)matrix2.M41;
			result.M22 = matrix1.M21 * (double)matrix2.M12 + matrix1.M22 * (double)matrix2.M22 + matrix1.M23 * (double)matrix2.M32 + matrix1.M24 * (double)matrix2.M42;
			result.M23 = matrix1.M21 * (double)matrix2.M13 + matrix1.M22 * (double)matrix2.M23 + matrix1.M23 * (double)matrix2.M33 + matrix1.M24 * (double)matrix2.M43;
			result.M24 = matrix1.M21 * (double)matrix2.M14 + matrix1.M22 * (double)matrix2.M24 + matrix1.M23 * (double)matrix2.M34 + matrix1.M24 * (double)matrix2.M44;
			result.M31 = matrix1.M31 * (double)matrix2.M11 + matrix1.M32 * (double)matrix2.M21 + matrix1.M33 * (double)matrix2.M31 + matrix1.M34 * (double)matrix2.M41;
			result.M32 = matrix1.M31 * (double)matrix2.M12 + matrix1.M32 * (double)matrix2.M22 + matrix1.M33 * (double)matrix2.M32 + matrix1.M34 * (double)matrix2.M42;
			result.M33 = matrix1.M31 * (double)matrix2.M13 + matrix1.M32 * (double)matrix2.M23 + matrix1.M33 * (double)matrix2.M33 + matrix1.M34 * (double)matrix2.M43;
			result.M34 = matrix1.M31 * (double)matrix2.M14 + matrix1.M32 * (double)matrix2.M24 + matrix1.M33 * (double)matrix2.M34 + matrix1.M34 * (double)matrix2.M44;
			result.M41 = matrix1.M41 * (double)matrix2.M11 + matrix1.M42 * (double)matrix2.M21 + matrix1.M43 * (double)matrix2.M31 + matrix1.M44 * (double)matrix2.M41;
			result.M42 = matrix1.M41 * (double)matrix2.M12 + matrix1.M42 * (double)matrix2.M22 + matrix1.M43 * (double)matrix2.M32 + matrix1.M44 * (double)matrix2.M42;
			result.M43 = matrix1.M41 * (double)matrix2.M13 + matrix1.M42 * (double)matrix2.M23 + matrix1.M43 * (double)matrix2.M33 + matrix1.M44 * (double)matrix2.M43;
			result.M44 = matrix1.M41 * (double)matrix2.M14 + matrix1.M42 * (double)matrix2.M24 + matrix1.M43 * (double)matrix2.M34 + matrix1.M44 * (double)matrix2.M44;
			return result;
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the multiplication.</param>
		public static void Multiply(ref MatrixD matrix1, ref Matrix matrix2, out MatrixD result)
		{
			double m = matrix1.M11 * (double)matrix2.M11 + matrix1.M12 * (double)matrix2.M21 + matrix1.M13 * (double)matrix2.M31 + matrix1.M14 * (double)matrix2.M41;
			double m2 = matrix1.M11 * (double)matrix2.M12 + matrix1.M12 * (double)matrix2.M22 + matrix1.M13 * (double)matrix2.M32 + matrix1.M14 * (double)matrix2.M42;
			double m3 = matrix1.M11 * (double)matrix2.M13 + matrix1.M12 * (double)matrix2.M23 + matrix1.M13 * (double)matrix2.M33 + matrix1.M14 * (double)matrix2.M43;
			double m4 = matrix1.M11 * (double)matrix2.M14 + matrix1.M12 * (double)matrix2.M24 + matrix1.M13 * (double)matrix2.M34 + matrix1.M14 * (double)matrix2.M44;
			double m5 = matrix1.M21 * (double)matrix2.M11 + matrix1.M22 * (double)matrix2.M21 + matrix1.M23 * (double)matrix2.M31 + matrix1.M24 * (double)matrix2.M41;
			double m6 = matrix1.M21 * (double)matrix2.M12 + matrix1.M22 * (double)matrix2.M22 + matrix1.M23 * (double)matrix2.M32 + matrix1.M24 * (double)matrix2.M42;
			double m7 = matrix1.M21 * (double)matrix2.M13 + matrix1.M22 * (double)matrix2.M23 + matrix1.M23 * (double)matrix2.M33 + matrix1.M24 * (double)matrix2.M43;
			double m8 = matrix1.M21 * (double)matrix2.M14 + matrix1.M22 * (double)matrix2.M24 + matrix1.M23 * (double)matrix2.M34 + matrix1.M24 * (double)matrix2.M44;
			double m9 = matrix1.M31 * (double)matrix2.M11 + matrix1.M32 * (double)matrix2.M21 + matrix1.M33 * (double)matrix2.M31 + matrix1.M34 * (double)matrix2.M41;
			double m10 = matrix1.M31 * (double)matrix2.M12 + matrix1.M32 * (double)matrix2.M22 + matrix1.M33 * (double)matrix2.M32 + matrix1.M34 * (double)matrix2.M42;
			double m11 = matrix1.M31 * (double)matrix2.M13 + matrix1.M32 * (double)matrix2.M23 + matrix1.M33 * (double)matrix2.M33 + matrix1.M34 * (double)matrix2.M43;
			double m12 = matrix1.M31 * (double)matrix2.M14 + matrix1.M32 * (double)matrix2.M24 + matrix1.M33 * (double)matrix2.M34 + matrix1.M34 * (double)matrix2.M44;
			double m13 = matrix1.M41 * (double)matrix2.M11 + matrix1.M42 * (double)matrix2.M21 + matrix1.M43 * (double)matrix2.M31 + matrix1.M44 * (double)matrix2.M41;
			double m14 = matrix1.M41 * (double)matrix2.M12 + matrix1.M42 * (double)matrix2.M22 + matrix1.M43 * (double)matrix2.M32 + matrix1.M44 * (double)matrix2.M42;
			double m15 = matrix1.M41 * (double)matrix2.M13 + matrix1.M42 * (double)matrix2.M23 + matrix1.M43 * (double)matrix2.M33 + matrix1.M44 * (double)matrix2.M43;
			double m16 = matrix1.M41 * (double)matrix2.M14 + matrix1.M42 * (double)matrix2.M24 + matrix1.M43 * (double)matrix2.M34 + matrix1.M44 * (double)matrix2.M44;
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

		public static void Multiply(ref Matrix matrix1, ref MatrixD matrix2, out MatrixD result)
		{
			double m = (double)matrix1.M11 * matrix2.M11 + (double)matrix1.M12 * matrix2.M21 + (double)matrix1.M13 * matrix2.M31 + (double)matrix1.M14 * matrix2.M41;
			double m2 = (double)matrix1.M11 * matrix2.M12 + (double)matrix1.M12 * matrix2.M22 + (double)matrix1.M13 * matrix2.M32 + (double)matrix1.M14 * matrix2.M42;
			double m3 = (double)matrix1.M11 * matrix2.M13 + (double)matrix1.M12 * matrix2.M23 + (double)matrix1.M13 * matrix2.M33 + (double)matrix1.M14 * matrix2.M43;
			double m4 = (double)matrix1.M11 * matrix2.M14 + (double)matrix1.M12 * matrix2.M24 + (double)matrix1.M13 * matrix2.M34 + (double)matrix1.M14 * matrix2.M44;
			double m5 = (double)matrix1.M21 * matrix2.M11 + (double)matrix1.M22 * matrix2.M21 + (double)matrix1.M23 * matrix2.M31 + (double)matrix1.M24 * matrix2.M41;
			double m6 = (double)matrix1.M21 * matrix2.M12 + (double)matrix1.M22 * matrix2.M22 + (double)matrix1.M23 * matrix2.M32 + (double)matrix1.M24 * matrix2.M42;
			double m7 = (double)matrix1.M21 * matrix2.M13 + (double)matrix1.M22 * matrix2.M23 + (double)matrix1.M23 * matrix2.M33 + (double)matrix1.M24 * matrix2.M43;
			double m8 = (double)matrix1.M21 * matrix2.M14 + (double)matrix1.M22 * matrix2.M24 + (double)matrix1.M23 * matrix2.M34 + (double)matrix1.M24 * matrix2.M44;
			double m9 = (double)matrix1.M31 * matrix2.M11 + (double)matrix1.M32 * matrix2.M21 + (double)matrix1.M33 * matrix2.M31 + (double)matrix1.M34 * matrix2.M41;
			double m10 = (double)matrix1.M31 * matrix2.M12 + (double)matrix1.M32 * matrix2.M22 + (double)matrix1.M33 * matrix2.M32 + (double)matrix1.M34 * matrix2.M42;
			double m11 = (double)matrix1.M31 * matrix2.M13 + (double)matrix1.M32 * matrix2.M23 + (double)matrix1.M33 * matrix2.M33 + (double)matrix1.M34 * matrix2.M43;
			double m12 = (double)matrix1.M31 * matrix2.M14 + (double)matrix1.M32 * matrix2.M24 + (double)matrix1.M33 * matrix2.M34 + (double)matrix1.M34 * matrix2.M44;
			double m13 = (double)matrix1.M41 * matrix2.M11 + (double)matrix1.M42 * matrix2.M21 + (double)matrix1.M43 * matrix2.M31 + (double)matrix1.M44 * matrix2.M41;
			double m14 = (double)matrix1.M41 * matrix2.M12 + (double)matrix1.M42 * matrix2.M22 + (double)matrix1.M43 * matrix2.M32 + (double)matrix1.M44 * matrix2.M42;
			double m15 = (double)matrix1.M41 * matrix2.M13 + (double)matrix1.M42 * matrix2.M23 + (double)matrix1.M43 * matrix2.M33 + (double)matrix1.M44 * matrix2.M43;
			double m16 = (double)matrix1.M41 * matrix2.M14 + (double)matrix1.M42 * matrix2.M24 + (double)matrix1.M43 * matrix2.M34 + (double)matrix1.M44 * matrix2.M44;
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
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="matrix2">Source matrix.</param><param name="result">[OutAttribute] Result of the multiplication.</param>
		public static void Multiply(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
		{
			double m = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
			double m2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
			double m3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
			double m4 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
			double m5 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
			double m6 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
			double m7 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
			double m8 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
			double m9 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
			double m10 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
			double m11 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
			double m12 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
			double m13 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
			double m14 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
			double m15 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
			double m16 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
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
		/// Multiplies a matrix by a scalar value.
		/// </summary>
		/// <param name="matrix1">Source matrix.</param><param name="scaleFactor">Scalar value.</param>
		public static MatrixD Multiply(MatrixD matrix1, double scaleFactor)
		{
			MatrixD result = default(MatrixD);
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
		public static void Multiply(ref MatrixD matrix1, double scaleFactor, out MatrixD result)
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
		public static MatrixD Divide(MatrixD matrix1, MatrixD matrix2)
		{
			MatrixD result = default(MatrixD);
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
		public static void Divide(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
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
		public static MatrixD Divide(MatrixD matrix1, double divider)
		{
			double num = 1.0 / divider;
			MatrixD result = default(MatrixD);
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
		public static void Divide(ref MatrixD matrix1, double divider, out MatrixD result)
		{
			double num = 1.0 / divider;
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
		public MatrixD GetOrientation()
		{
			MatrixD identity = Identity;
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
			return double.IsNaN(M11 + M12 + M13 + M14 + M21 + M22 + M23 + M24 + M31 + M32 + M33 + M34 + M41 + M42 + M43 + M44);
		}

		public bool IsRotation()
		{
			double num = 0.01;
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
			if (Math.Abs(Right.LengthSquared() - 1.0) > num)
			{
				return false;
			}
			if (Math.Abs(Up.LengthSquared() - 1.0) > num)
			{
				return false;
			}
			if (Math.Abs(Backward.LengthSquared() - 1.0) > num)
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
			double num = 9.9999997473787516E-05;
			if (M41 + M42 + M43 + M34 + M24 + M14 > num)
			{
				return false;
			}
			if (Math.Abs(M44 - 1.0) > num)
			{
				return false;
			}
			return true;
		}

		public static MatrixD CreateFromDir(Vector3D dir)
		{
			Vector3D value = new Vector3D(0.0, 0.0, 1.0);
			double z = dir.Z;
			Vector3D up;
			if (!(z > -0.99999) || !(z < 0.99999))
			{
				value = new Vector3D(dir.Z, 0.0, 0.0 - dir.X);
				up = new Vector3D(0.0, 1.0, 0.0);
			}
			else
			{
				value -= dir * z;
				value = Vector3D.Normalize(value);
				up = Vector3D.Cross(dir, value);
			}
			MatrixD identity = Identity;
			identity.Right = value;
			identity.Up = up;
			identity.Forward = dir;
			return identity;
		}

		public static MatrixD CreateFromDir(Vector3D dir, Vector3D suggestedUp)
		{
			Vector3D up = Vector3D.Cross(Vector3D.Cross(dir, suggestedUp), dir);
			return CreateWorld(Vector3D.Zero, dir, up);
		}

		public static MatrixD Normalize(MatrixD matrix)
		{
			MatrixD result = matrix;
			result.Right = Vector3D.Normalize(result.Right);
			result.Up = Vector3D.Normalize(result.Up);
			result.Forward = Vector3D.Normalize(result.Forward);
			return result;
		}

		public void Orthogonalize()
		{
			Vector3D vector3D = Vector3D.Normalize(Right);
			Vector3D vector3D2 = Vector3D.Normalize(Up - vector3D * Up.Dot(vector3D));
			Vector3D backward = Vector3D.Normalize(Backward - vector3D * Backward.Dot(vector3D) - vector3D2 * Backward.Dot(vector3D2));
			Right = vector3D;
			Up = vector3D2;
			Backward = backward;
		}

		public static MatrixD Orthogonalize(MatrixD rotationMatrix)
		{
			MatrixD result = rotationMatrix;
			result.Right = Vector3D.Normalize(result.Right);
			result.Up = Vector3D.Normalize(result.Up - result.Right * result.Up.Dot(result.Right));
			result.Backward = Vector3D.Normalize(result.Backward - result.Right * result.Backward.Dot(result.Right) - result.Up * result.Backward.Dot(result.Up));
			return result;
		}

		public static MatrixD AlignRotationToAxes(ref MatrixD toAlign, ref MatrixD axisDefinitionMatrix)
		{
			MatrixD identity = Identity;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			double num = toAlign.Right.Dot(axisDefinitionMatrix.Right);
			double num2 = toAlign.Right.Dot(axisDefinitionMatrix.Up);
			double num3 = toAlign.Right.Dot(axisDefinitionMatrix.Backward);
			if (Math.Abs(num) > Math.Abs(num2))
			{
				if (Math.Abs(num) > Math.Abs(num3))
				{
					identity.Right = ((num > 0.0) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
					flag = true;
				}
				else
				{
					identity.Right = ((num3 > 0.0) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
					flag3 = true;
				}
			}
			else if (Math.Abs(num2) > Math.Abs(num3))
			{
				identity.Right = ((num2 > 0.0) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
				flag2 = true;
			}
			else
			{
				identity.Right = ((num3 > 0.0) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
				flag3 = true;
			}
			num = toAlign.Up.Dot(axisDefinitionMatrix.Right);
			num2 = toAlign.Up.Dot(axisDefinitionMatrix.Up);
			num3 = toAlign.Up.Dot(axisDefinitionMatrix.Backward);
			if (flag2 || (Math.Abs(num) > Math.Abs(num2) && !flag))
			{
				if (Math.Abs(num) > Math.Abs(num3) || flag3)
				{
					identity.Up = ((num > 0.0) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
					flag = true;
				}
				else
				{
					identity.Up = ((num3 > 0.0) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
					flag3 = true;
				}
			}
			else if (Math.Abs(num2) > Math.Abs(num3) || flag3)
			{
				identity.Up = ((num2 > 0.0) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
				flag2 = true;
			}
			else
			{
				identity.Up = ((num3 > 0.0) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
				flag3 = true;
			}
			if (!flag)
			{
				num = toAlign.Backward.Dot(axisDefinitionMatrix.Right);
				identity.Backward = ((num > 0.0) ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left);
			}
			else if (!flag2)
			{
				num2 = toAlign.Backward.Dot(axisDefinitionMatrix.Up);
				identity.Backward = ((num2 > 0.0) ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down);
			}
			else
			{
				num3 = toAlign.Backward.Dot(axisDefinitionMatrix.Backward);
				identity.Backward = ((num3 > 0.0) ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward);
			}
			return identity;
		}

		public static bool GetEulerAnglesXYZ(ref MatrixD mat, out Vector3D xyz)
		{
			double x = mat.GetRow(0).X;
			double num = mat.GetRow(0).Y;
			double num2 = mat.GetRow(0).Z;
			double y = mat.GetRow(1).X;
			double x2 = mat.GetRow(1).Y;
			double num3 = mat.GetRow(1).Z;
			mat.GetRow(2);
			mat.GetRow(2);
			double x3 = mat.GetRow(2).Z;
			double num4 = num2;
			if (num4 < 1.0)
			{
				if (num4 > -1.0)
				{
					xyz = new Vector3D(Math.Atan2(0.0 - num3, x3), Math.Asin(num2), Math.Atan2(0.0 - num, x));
					return true;
				}
				xyz = new Vector3D(0.0 - Math.Atan2(y, x2), -1.570796012878418, 0.0);
				return false;
			}
			xyz = new Vector3D(Math.Atan2(y, x2), -1.570796012878418, 0.0);
			return false;
		}

		public static MatrixD SwapYZCoordinates(MatrixD m)
		{
			MatrixD result = m;
			Vector3D right = m.Right;
			Vector3D up = m.Up;
			Vector3D forward = m.Forward;
			result.Right = new Vector3D(right.X, right.Z, 0.0 - right.Y);
			result.Up = new Vector3D(forward.X, forward.Z, 0.0 - forward.Y);
			result.Forward = new Vector3D(0.0 - up.X, 0.0 - up.Z, up.Y);
			Vector3D translation = m.Translation;
			result.Translation = Vector3D.SwapYZCoordinates(translation);
			return result;
		}

		public bool IsMirrored()
		{
			return Determinant() < 0.0;
		}

		public static implicit operator Matrix(MatrixD m)
		{
			return new Matrix((float)m.M11, (float)m.M12, (float)m.M13, (float)m.M14, (float)m.M21, (float)m.M22, (float)m.M23, (float)m.M24, (float)m.M31, (float)m.M32, (float)m.M33, (float)m.M34, (float)m.M41, (float)m.M42, (float)m.M43, (float)m.M44);
		}

		public static implicit operator MatrixD(Matrix m)
		{
			return new MatrixD(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
		}
	}
}
