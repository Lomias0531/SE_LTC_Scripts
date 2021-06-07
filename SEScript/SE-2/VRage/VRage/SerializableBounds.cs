using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage
{
	[ProtoContract]
	public struct SerializableBounds
	{
		protected class VRage_SerializableBounds_003C_003EMin_003C_003EAccessor : IMemberAccessor<SerializableBounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBounds owner, in float value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBounds owner, out float value)
			{
				value = owner.Min;
			}
		}

		protected class VRage_SerializableBounds_003C_003EMax_003C_003EAccessor : IMemberAccessor<SerializableBounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBounds owner, in float value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBounds owner, out float value)
			{
				value = owner.Max;
			}
		}

		protected class VRage_SerializableBounds_003C_003EDefault_003C_003EAccessor : IMemberAccessor<SerializableBounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableBounds owner, in float value)
			{
				owner.Default = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableBounds owner, out float value)
			{
				value = owner.Default;
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public float Min;

		[ProtoMember(4)]
		[XmlAttribute]
		public float Max;

		[ProtoMember(7)]
		[XmlAttribute]
		public float Default;

		public SerializableBounds(float min, float max, float def)
		{
			Min = min;
			Max = max;
			Default = def;
		}

		public static implicit operator MyBounds(SerializableBounds v)
		{
			return new MyBounds(v.Min, v.Max, v.Default);
		}

		public static implicit operator SerializableBounds(MyBounds v)
		{
			return new SerializableBounds(v.Min, v.Max, v.Default);
		}
	}
}
