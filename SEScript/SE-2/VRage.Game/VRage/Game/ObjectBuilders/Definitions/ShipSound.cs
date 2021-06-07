using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	public class ShipSound
	{
		protected class VRage_Game_ObjectBuilders_Definitions_ShipSound_003C_003ESoundType_003C_003EAccessor : IMemberAccessor<ShipSound, ShipSystemSoundsEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShipSound owner, in ShipSystemSoundsEnum value)
			{
				owner.SoundType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShipSound owner, out ShipSystemSoundsEnum value)
			{
				value = owner.SoundType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_ShipSound_003C_003ESoundName_003C_003EAccessor : IMemberAccessor<ShipSound, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShipSound owner, in string value)
			{
				owner.SoundName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShipSound owner, out string value)
			{
				value = owner.SoundName;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_ShipSound_003C_003EActor : IActivator, IActivator<ShipSound>
		{
			private sealed override object CreateInstance()
			{
				return new ShipSound();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ShipSound CreateInstance()
			{
				return new ShipSound();
			}

			ShipSound IActivator<ShipSound>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(76)]
		[XmlAttribute("Type")]
		public ShipSystemSoundsEnum SoundType = ShipSystemSoundsEnum.MainLoopMedium;

		[ProtoMember(79)]
		[XmlAttribute("SoundName")]
		public string SoundName = "";
	}
}
