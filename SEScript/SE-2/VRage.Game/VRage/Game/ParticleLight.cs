using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageRender.Animations;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("ParticleLight")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class ParticleLight
	{
		protected class VRage_Game_ParticleLight_003C_003EName_003C_003EAccessor : IMemberAccessor<ParticleLight, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleLight owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleLight owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ParticleLight_003C_003EVersion_003C_003EAccessor : IMemberAccessor<ParticleLight, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleLight owner, in int value)
			{
				owner.Version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleLight owner, out int value)
			{
				value = owner.Version;
			}
		}

		protected class VRage_Game_ParticleLight_003C_003EProperties_003C_003EAccessor : IMemberAccessor<ParticleLight, List<GenerationProperty>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ParticleLight owner, in List<GenerationProperty> value)
			{
				owner.Properties = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ParticleLight owner, out List<GenerationProperty> value)
			{
				value = owner.Properties;
			}
		}

		private class VRage_Game_ParticleLight_003C_003EActor : IActivator, IActivator<ParticleLight>
		{
			private sealed override object CreateInstance()
			{
				return new ParticleLight();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ParticleLight CreateInstance()
			{
				return new ParticleLight();
			}

			ParticleLight IActivator<ParticleLight>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(58)]
		[XmlAttribute("Name")]
		public string Name = "";

		[ProtoMember(61)]
		[XmlAttribute("Version")]
		public int Version;

		[ProtoMember(64)]
		public List<GenerationProperty> Properties;
	}
}
