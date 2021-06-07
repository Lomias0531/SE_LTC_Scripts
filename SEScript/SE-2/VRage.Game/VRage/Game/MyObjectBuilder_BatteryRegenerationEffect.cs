using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_BatteryRegenerationEffect
	{
		protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect_003C_003EChargePerSecond_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BatteryRegenerationEffect owner, in float value)
			{
				owner.ChargePerSecond = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BatteryRegenerationEffect owner, out float value)
			{
				value = owner.ChargePerSecond;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect_003C_003ERemainingTimeInMiliseconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BatteryRegenerationEffect owner, in long value)
			{
				owner.RemainingTimeInMiliseconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BatteryRegenerationEffect owner, out long value)
			{
				value = owner.RemainingTimeInMiliseconds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect_003C_003ELastRegenTimeInMiliseconds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BatteryRegenerationEffect owner, in long value)
			{
				owner.LastRegenTimeInMiliseconds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BatteryRegenerationEffect owner, out long value)
			{
				value = owner.LastRegenTimeInMiliseconds;
			}
		}

		private class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BatteryRegenerationEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BatteryRegenerationEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BatteryRegenerationEffect CreateInstance()
			{
				return new MyObjectBuilder_BatteryRegenerationEffect();
			}

			MyObjectBuilder_BatteryRegenerationEffect IActivator<MyObjectBuilder_BatteryRegenerationEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float ChargePerSecond;

		[ProtoMember(3)]
		public long RemainingTimeInMiliseconds;

		[ProtoMember(5)]
		public long LastRegenTimeInMiliseconds;
	}
}
