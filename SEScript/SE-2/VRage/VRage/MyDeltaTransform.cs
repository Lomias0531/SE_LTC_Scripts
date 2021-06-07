using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
	/// <summary>
	/// Transform structure for delta-transforms.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct MyDeltaTransform
	{
		protected class VRage_MyDeltaTransform_003C_003EOrientationOffset_003C_003EAccessor : IMemberAccessor<MyDeltaTransform, Quaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDeltaTransform owner, in Quaternion value)
			{
				owner.OrientationOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDeltaTransform owner, out Quaternion value)
			{
				value = owner.OrientationOffset;
			}
		}

		protected class VRage_MyDeltaTransform_003C_003EPositionOffset_003C_003EAccessor : IMemberAccessor<MyDeltaTransform, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDeltaTransform owner, in Vector3 value)
			{
				owner.PositionOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDeltaTransform owner, out Vector3 value)
			{
				value = owner.PositionOffset;
			}
		}

		protected class VRage_MyDeltaTransform_003C_003EOrientationAsVector_003C_003EAccessor : IMemberAccessor<MyDeltaTransform, Vector4>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDeltaTransform owner, in Vector4 value)
			{
				owner.OrientationAsVector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDeltaTransform owner, out Vector4 value)
			{
				value = owner.OrientationAsVector;
			}
		}

		[NoSerialize]
		public Quaternion OrientationOffset;

		[Serialize]
		[ProtoMember(4)]
		public Vector3 PositionOffset;

		[Serialize]
		[ProtoMember(1)]
		public Vector4 OrientationAsVector
		{
			get
			{
				return OrientationOffset.ToVector4();
			}
			set
			{
				OrientationOffset = Quaternion.FromVector4(value);
			}
		}

		public bool IsZero
		{
			get
			{
				if (PositionOffset == Vector3.Zero)
				{
					return OrientationOffset == Quaternion.Zero;
				}
				return false;
			}
		}

		public static implicit operator Matrix(MyDeltaTransform transform)
		{
			Matrix.CreateFromQuaternion(ref transform.OrientationOffset, out Matrix result);
			result.Translation = transform.PositionOffset;
			return result;
		}

		public static implicit operator MyDeltaTransform(Matrix matrix)
		{
			MyDeltaTransform result = default(MyDeltaTransform);
			result.PositionOffset = matrix.Translation;
			Quaternion.CreateFromRotationMatrix(ref matrix, out result.OrientationOffset);
			return result;
		}

		public static implicit operator MatrixD(MyDeltaTransform transform)
		{
			MatrixD.CreateFromQuaternion(ref transform.OrientationOffset, out MatrixD result);
			result.Translation = transform.PositionOffset;
			return result;
		}

		public static implicit operator MyDeltaTransform(MatrixD matrix)
		{
			MyDeltaTransform result = default(MyDeltaTransform);
			result.PositionOffset = matrix.Translation;
			Quaternion.CreateFromRotationMatrix(ref matrix, out result.OrientationOffset);
			return result;
		}

		public static implicit operator MyPositionAndOrientation(MyDeltaTransform deltaTransform)
		{
			return new MyPositionAndOrientation(deltaTransform.PositionOffset, deltaTransform.OrientationOffset.Forward, deltaTransform.OrientationOffset.Up);
		}

		public static implicit operator MyDeltaTransform(MyPositionAndOrientation value)
		{
			MyDeltaTransform result = default(MyDeltaTransform);
			result.PositionOffset = (Vector3D)value.Position;
			result.OrientationOffset = Quaternion.CreateFromForwardUp(value.Forward, value.Up);
			return result;
		}
	}
}
