using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("AlternativeImpactSound")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public sealed class AlternativeImpactSounds
	{
		protected class VRage_Game_AlternativeImpactSounds_003C_003Emass_003C_003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AlternativeImpactSounds owner, in float value)
			{
				owner.mass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AlternativeImpactSounds owner, out float value)
			{
				value = owner.mass;
			}
		}

		protected class VRage_Game_AlternativeImpactSounds_003C_003EsoundCue_003C_003EAccessor : IMemberAccessor<AlternativeImpactSounds, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AlternativeImpactSounds owner, in string value)
			{
				owner.soundCue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AlternativeImpactSounds owner, out string value)
			{
				value = owner.soundCue;
			}
		}

		protected class VRage_Game_AlternativeImpactSounds_003C_003EminVelocity_003C_003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AlternativeImpactSounds owner, in float value)
			{
				owner.minVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AlternativeImpactSounds owner, out float value)
			{
				value = owner.minVelocity;
			}
		}

		protected class VRage_Game_AlternativeImpactSounds_003C_003EmaxVolumeVelocity_003C_003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AlternativeImpactSounds owner, in float value)
			{
				owner.maxVolumeVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AlternativeImpactSounds owner, out float value)
			{
				value = owner.maxVolumeVelocity;
			}
		}

		private class VRage_Game_AlternativeImpactSounds_003C_003EActor : IActivator, IActivator<AlternativeImpactSounds>
		{
			private sealed override object CreateInstance()
			{
				return new AlternativeImpactSounds();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override AlternativeImpactSounds CreateInstance()
			{
				return new AlternativeImpactSounds();
			}

			AlternativeImpactSounds IActivator<AlternativeImpactSounds>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(31)]
		[XmlAttribute]
		public float mass;

		[ProtoMember(34)]
		[XmlAttribute]
		public string soundCue = "";

		[ProtoMember(37)]
		[XmlAttribute]
		public float minVelocity;

		[ProtoMember(40)]
		[XmlAttribute]
		public float maxVolumeVelocity;
	}
}
