using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageRender.Animations;

namespace VRage.Game
{
	[ProtoContract]
	public class ParticleEmitter
	{
		protected class VRage_Game_ParticleEmitter_003C_003EVersion_003C_003EAccessor : IMemberAccessor<ParticleEmitter, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleEmitter owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleEmitter owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Game_ParticleEmitter_003C_003EProperties_003C_003EAccessor : IMemberAccessor<ParticleEmitter, List<GenerationProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleEmitter owner, in List<GenerationProperty> value)
			{
				owner.Properties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleEmitter owner, out List<GenerationProperty> value)
			{
				value = owner.Properties;
			}
		}

		private class VRage_Game_ParticleEmitter_003C_003EActor : IActivator, IActivator<ParticleEmitter>
		{
			private sealed override object CreateInstance()
			{
				return new ParticleEmitter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ParticleEmitter CreateInstance()
			{
				return new ParticleEmitter();
			}

			ParticleEmitter IActivator<ParticleEmitter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(52)]
		[XmlAttribute("Version")]
		public int Version;

		[ProtoMember(55)]
		public List<GenerationProperty> Properties;
	}
}
