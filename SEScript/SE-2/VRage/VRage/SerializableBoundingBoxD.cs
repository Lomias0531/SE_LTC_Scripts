using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableBoundingBoxD
	{
		protected class VRage_SerializableBoundingBoxD_003C_003EMin_003C_003EAccessor : IMemberAccessor<SerializableBoundingBoxD, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBoundingBoxD owner, in SerializableVector3D value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBoundingBoxD owner, out SerializableVector3D value)
			{
				value = owner.Min;
			}
		}

		protected class VRage_SerializableBoundingBoxD_003C_003EMax_003C_003EAccessor : IMemberAccessor<SerializableBoundingBoxD, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBoundingBoxD owner, in SerializableVector3D value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBoundingBoxD owner, out SerializableVector3D value)
			{
				value = owner.Max;
			}
		}

		[ProtoMember(1)]
		public SerializableVector3D Min;

		[ProtoMember(4)]
		public SerializableVector3D Max;

		public static implicit operator BoundingBoxD(SerializableBoundingBoxD v)
		{
			return new BoundingBoxD(v.Min, v.Max);
		}

		public static implicit operator SerializableBoundingBoxD(BoundingBoxD v)
		{
			SerializableBoundingBoxD result = default(SerializableBoundingBoxD);
			result.Min = v.Min;
			result.Max = v.Max;
			return result;
		}
	}
}
