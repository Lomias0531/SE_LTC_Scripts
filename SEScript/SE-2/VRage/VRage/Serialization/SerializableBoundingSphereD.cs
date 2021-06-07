using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Serialization
{
	[ProtoContract]
	public struct SerializableBoundingSphereD
	{
		protected class VRage_Serialization_SerializableBoundingSphereD_003C_003ECenter_003C_003EAccessor : IMemberAccessor<SerializableBoundingSphereD, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBoundingSphereD owner, in SerializableVector3D value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBoundingSphereD owner, out SerializableVector3D value)
			{
				value = owner.Center;
			}
		}

		protected class VRage_Serialization_SerializableBoundingSphereD_003C_003ERadius_003C_003EAccessor : IMemberAccessor<SerializableBoundingSphereD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBoundingSphereD owner, in double value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBoundingSphereD owner, out double value)
			{
				value = owner.Radius;
			}
		}

		[ProtoMember(1)]
		public SerializableVector3D Center;

		[ProtoMember(4)]
		public double Radius;

		public static implicit operator BoundingSphereD(SerializableBoundingSphereD v)
		{
			return new BoundingSphereD(v.Center, v.Radius);
		}

		public static implicit operator SerializableBoundingSphereD(BoundingSphereD v)
		{
			SerializableBoundingSphereD result = default(SerializableBoundingSphereD);
			result.Center = v.Center;
			result.Radius = v.Radius;
			return result;
		}
	}
}
