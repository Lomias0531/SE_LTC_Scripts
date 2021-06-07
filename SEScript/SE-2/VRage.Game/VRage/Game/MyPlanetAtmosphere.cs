using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetAtmosphere
	{
		protected class VRage_Game_MyPlanetAtmosphere_003C_003EBreathable_003C_003EAccessor : IMemberAccessor<MyPlanetAtmosphere, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAtmosphere owner, in bool value)
			{
				owner.Breathable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAtmosphere owner, out bool value)
			{
				value = owner.Breathable;
			}
		}

		protected class VRage_Game_MyPlanetAtmosphere_003C_003EOxygenDensity_003C_003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAtmosphere owner, in float value)
			{
				owner.OxygenDensity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAtmosphere owner, out float value)
			{
				value = owner.OxygenDensity;
			}
		}

		protected class VRage_Game_MyPlanetAtmosphere_003C_003EDensity_003C_003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAtmosphere owner, in float value)
			{
				owner.Density = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAtmosphere owner, out float value)
			{
				value = owner.Density;
			}
		}

		protected class VRage_Game_MyPlanetAtmosphere_003C_003ELimitAltitude_003C_003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAtmosphere owner, in float value)
			{
				owner.LimitAltitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAtmosphere owner, out float value)
			{
				value = owner.LimitAltitude;
			}
		}

		protected class VRage_Game_MyPlanetAtmosphere_003C_003EMaxWindSpeed_003C_003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAtmosphere owner, in float value)
			{
				owner.MaxWindSpeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAtmosphere owner, out float value)
			{
				value = owner.MaxWindSpeed;
			}
		}

		private class VRage_Game_MyPlanetAtmosphere_003C_003EActor : IActivator, IActivator<MyPlanetAtmosphere>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetAtmosphere();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetAtmosphere CreateInstance()
			{
				return new MyPlanetAtmosphere();
			}

			MyPlanetAtmosphere IActivator<MyPlanetAtmosphere>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(74)]
		[XmlElement]
		public bool Breathable;

		[ProtoMember(75)]
		[XmlElement]
		public float OxygenDensity = 1f;

		[ProtoMember(76)]
		[XmlElement]
		public float Density = 1f;

		[ProtoMember(77)]
		[XmlElement]
		public float LimitAltitude = 2f;

		[XmlElement]
		[ProtoMember(78)]
		public float MaxWindSpeed;
	}
}
