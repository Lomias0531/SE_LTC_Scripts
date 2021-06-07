using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageRender.Animations;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("ParticleSound")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class ParticleSound
	{
		protected class VRage_Game_ParticleSound_003C_003EName_003C_003EAccessor : IMemberAccessor<ParticleSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleSound owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleSound owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ParticleSound_003C_003EVersion_003C_003EAccessor : IMemberAccessor<ParticleSound, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleSound owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleSound owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Game_ParticleSound_003C_003EProperties_003C_003EAccessor : IMemberAccessor<ParticleSound, List<GenerationProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleSound owner, in List<GenerationProperty> value)
			{
				owner.Properties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleSound owner, out List<GenerationProperty> value)
			{
				value = owner.Properties;
			}
		}

		private class VRage_Game_ParticleSound_003C_003EActor : IActivator, IActivator<ParticleSound>
		{
			private sealed override object CreateInstance()
			{
				return new ParticleSound();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ParticleSound CreateInstance()
			{
				return new ParticleSound();
			}

			ParticleSound IActivator<ParticleSound>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(67)]
		[XmlAttribute("Name")]
		public string Name = "";

		[ProtoMember(70)]
		[XmlAttribute("Version")]
		public int Version;

		[ProtoMember(73)]
		public List<GenerationProperty> Properties;
	}
}
