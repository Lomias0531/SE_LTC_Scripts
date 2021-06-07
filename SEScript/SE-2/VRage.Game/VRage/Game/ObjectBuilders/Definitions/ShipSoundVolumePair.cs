using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	public class ShipSoundVolumePair
	{
		protected class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair_003C_003ESpeed_003C_003EAccessor : IMemberAccessor<ShipSoundVolumePair, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShipSoundVolumePair owner, in float value)
			{
				owner.Speed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShipSoundVolumePair owner, out float value)
			{
				value = owner.Speed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair_003C_003EVolume_003C_003EAccessor : IMemberAccessor<ShipSoundVolumePair, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ShipSoundVolumePair owner, in float value)
			{
				owner.Volume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ShipSoundVolumePair owner, out float value)
			{
				value = owner.Volume;
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair_003C_003EActor : IActivator, IActivator<ShipSoundVolumePair>
		{
			private sealed override object CreateInstance()
			{
				return new ShipSoundVolumePair();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ShipSoundVolumePair CreateInstance()
			{
				return new ShipSoundVolumePair();
			}

			ShipSoundVolumePair IActivator<ShipSoundVolumePair>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(82)]
		[XmlAttribute("Speed")]
		public float Speed;

		[ProtoMember(85)]
		[XmlAttribute("Volume")]
		public float Volume;
	}
}
