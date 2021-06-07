using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage
{
	[ProtoContract]
	[XmlType("DistantSound")]
	public sealed class DistantSound
	{
		protected class VRage_DistantSound_003C_003EDistance_003C_003EAccessor : IMemberAccessor<DistantSound, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DistantSound owner, in float value)
			{
				owner.Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DistantSound owner, out float value)
			{
				value = owner.Distance;
			}
		}

		protected class VRage_DistantSound_003C_003EDistanceCrossfade_003C_003EAccessor : IMemberAccessor<DistantSound, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DistantSound owner, in float value)
			{
				owner.DistanceCrossfade = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DistantSound owner, out float value)
			{
				value = owner.DistanceCrossfade;
			}
		}

		protected class VRage_DistantSound_003C_003ESound_003C_003EAccessor : IMemberAccessor<DistantSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DistantSound owner, in string value)
			{
				owner.Sound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DistantSound owner, out string value)
			{
				value = owner.Sound;
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public float Distance = 50f;

		[ProtoMember(4)]
		[XmlAttribute]
		public float DistanceCrossfade = -1f;

		[ProtoMember(7)]
		[XmlAttribute]
		public string Sound = "";
	}
}
