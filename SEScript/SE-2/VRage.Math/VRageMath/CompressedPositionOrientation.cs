using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
	/// <summary>
	/// Defines a matrix.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct CompressedPositionOrientation
	{
		protected class VRageMath_CompressedPositionOrientation_003C_003EPosition_003C_003EAccessor : IMemberAccessor<CompressedPositionOrientation, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CompressedPositionOrientation owner, in Vector3 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CompressedPositionOrientation owner, out Vector3 value)
			{
				value = owner.Position;
			}
		}

		protected class VRageMath_CompressedPositionOrientation_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<CompressedPositionOrientation, HalfVector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CompressedPositionOrientation owner, in HalfVector4 value)
			{
				owner.Orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CompressedPositionOrientation owner, out HalfVector4 value)
			{
				value = owner.Orientation;
			}
		}

		protected class VRageMath_CompressedPositionOrientation_003C_003EMatrix_003C_003EAccessor : IMemberAccessor<CompressedPositionOrientation, Matrix>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CompressedPositionOrientation owner, in Matrix value)
			{
				owner.Matrix = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CompressedPositionOrientation owner, out Matrix value)
			{
				value = owner.Matrix;
			}
		}

		public Vector3 Position;

		public HalfVector4 Orientation;

		public Matrix Matrix
		{
			get
			{
				ToMatrix(out Matrix result);
				return result;
			}
			set
			{
				FromMatrix(ref value);
			}
		}

		public CompressedPositionOrientation(ref Matrix matrix)
		{
			Position = matrix.Translation;
			Quaternion.CreateFromRotationMatrix(ref matrix, out Quaternion result);
			Orientation = new HalfVector4(result.ToVector4());
		}

		public void FromMatrix(ref Matrix matrix)
		{
			Position = matrix.Translation;
			Quaternion.CreateFromRotationMatrix(ref matrix, out Quaternion result);
			Orientation = new HalfVector4(result.ToVector4());
		}

		public void ToMatrix(out Matrix result)
		{
			Quaternion quaternion = Quaternion.FromVector4(Orientation.ToVector4());
			Matrix.CreateFromQuaternion(ref quaternion, out result);
			result.Translation = Position;
		}
	}
}
