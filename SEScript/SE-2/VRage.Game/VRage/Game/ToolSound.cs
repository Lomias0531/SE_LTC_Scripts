using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlType("ToolSound")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public struct ToolSound
	{
		protected class VRage_Game_ToolSound_003C_003Etype_003C_003EAccessor : IMemberAccessor<ToolSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolSound owner, in string value)
			{
				owner.type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolSound owner, out string value)
			{
				value = owner.type;
			}
		}

		protected class VRage_Game_ToolSound_003C_003Esubtype_003C_003EAccessor : IMemberAccessor<ToolSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolSound owner, in string value)
			{
				owner.subtype = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolSound owner, out string value)
			{
				value = owner.subtype;
			}
		}

		protected class VRage_Game_ToolSound_003C_003Esound_003C_003EAccessor : IMemberAccessor<ToolSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ToolSound owner, in string value)
			{
				owner.sound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ToolSound owner, out string value)
			{
				value = owner.sound;
			}
		}

		private class VRage_Game_ToolSound_003C_003EActor : IActivator, IActivator<ToolSound>
		{
			private sealed override object CreateInstance()
			{
				return default(ToolSound);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ToolSound CreateInstance()
			{
				return (ToolSound)(object)default(ToolSound);
			}

			ToolSound IActivator<ToolSound>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(169)]
		[XmlAttribute]
		public string type;

		[ProtoMember(172)]
		[XmlAttribute]
		public string subtype;

		[ProtoMember(175)]
		[XmlAttribute]
		public string sound;
	}
}
