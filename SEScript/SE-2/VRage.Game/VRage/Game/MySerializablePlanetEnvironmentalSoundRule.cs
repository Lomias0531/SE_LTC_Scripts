using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public class MySerializablePlanetEnvironmentalSoundRule
	{
		protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule_003C_003EHeight_003C_003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializablePlanetEnvironmentalSoundRule owner, in SerializableRange value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializablePlanetEnvironmentalSoundRule owner, out SerializableRange value)
			{
				value = owner.Height;
			}
		}

		protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule_003C_003ELatitude_003C_003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SymmetricSerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializablePlanetEnvironmentalSoundRule owner, in SymmetricSerializableRange value)
			{
				owner.Latitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializablePlanetEnvironmentalSoundRule owner, out SymmetricSerializableRange value)
			{
				value = owner.Latitude;
			}
		}

		protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule_003C_003ESunAngleFromZenith_003C_003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializablePlanetEnvironmentalSoundRule owner, in SerializableRange value)
			{
				owner.SunAngleFromZenith = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializablePlanetEnvironmentalSoundRule owner, out SerializableRange value)
			{
				value = owner.SunAngleFromZenith;
			}
		}

		protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule_003C_003EEnvironmentSound_003C_003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializablePlanetEnvironmentalSoundRule owner, in string value)
			{
				owner.EnvironmentSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializablePlanetEnvironmentalSoundRule owner, out string value)
			{
				value = owner.EnvironmentSound;
			}
		}

		private class VRage_Game_MySerializablePlanetEnvironmentalSoundRule_003C_003EActor : IActivator, IActivator<MySerializablePlanetEnvironmentalSoundRule>
		{
			private sealed override object CreateInstance()
			{
				return new MySerializablePlanetEnvironmentalSoundRule();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySerializablePlanetEnvironmentalSoundRule CreateInstance()
			{
				return new MySerializablePlanetEnvironmentalSoundRule();
			}

			MySerializablePlanetEnvironmentalSoundRule IActivator<MySerializablePlanetEnvironmentalSoundRule>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(47)]
		public SerializableRange Height = new SerializableRange(0f, 1f);

		[ProtoMember(48)]
		public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);

		[ProtoMember(49)]
		public SerializableRange SunAngleFromZenith = new SerializableRange(0f, 180f);

		[ProtoMember(50)]
		public string EnvironmentSound;
	}
}
