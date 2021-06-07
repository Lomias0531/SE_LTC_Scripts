using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableOrientedBoundingBoxD
	{
		protected class VRage_SerializableOrientedBoundingBoxD_003C_003ECenter_003C_003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableOrientedBoundingBoxD owner, in SerializableVector3D value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableOrientedBoundingBoxD owner, out SerializableVector3D value)
			{
				value = owner.Center;
			}
		}

		protected class VRage_SerializableOrientedBoundingBoxD_003C_003EHalfExtent_003C_003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableOrientedBoundingBoxD owner, in SerializableVector3D value)
			{
				owner.HalfExtent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableOrientedBoundingBoxD owner, out SerializableVector3D value)
			{
				value = owner.HalfExtent;
			}
		}

		protected class VRage_SerializableOrientedBoundingBoxD_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<SerializableOrientedBoundingBoxD, SerializableQuaternion>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableOrientedBoundingBoxD owner, in SerializableQuaternion value)
			{
				owner.Orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableOrientedBoundingBoxD owner, out SerializableQuaternion value)
			{
				value = owner.Orientation;
			}
		}

		[ProtoMember(1)]
		public SerializableVector3D Center;

		[ProtoMember(4)]
		public SerializableVector3D HalfExtent;

		[ProtoMember(7)]
		public SerializableQuaternion Orientation;

		public static implicit operator MyOrientedBoundingBoxD(SerializableOrientedBoundingBoxD v)
		{
			return new MyOrientedBoundingBoxD(v.Center, v.HalfExtent, v.Orientation);
		}

		public static implicit operator SerializableOrientedBoundingBoxD(MyOrientedBoundingBoxD v)
		{
			SerializableOrientedBoundingBoxD result = default(SerializableOrientedBoundingBoxD);
			result.Center = v.Center;
			result.HalfExtent = v.HalfExtent;
			result.Orientation = v.Orientation;
			return result;
		}
	}
}
