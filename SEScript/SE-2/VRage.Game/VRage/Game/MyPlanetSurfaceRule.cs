using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetSurfaceRule : ICloneable
	{
		protected class VRage_Game_MyPlanetSurfaceRule_003C_003EHeight_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value)
			{
				value = owner.Height;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceRule_003C_003ELatitude_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SymmetricSerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceRule owner, in SymmetricSerializableRange value)
			{
				owner.Latitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceRule owner, out SymmetricSerializableRange value)
			{
				value = owner.Latitude;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceRule_003C_003ELongitude_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value)
			{
				owner.Longitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value)
			{
				value = owner.Longitude;
			}
		}

		protected class VRage_Game_MyPlanetSurfaceRule_003C_003ESlope_003C_003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value)
			{
				owner.Slope = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value)
			{
				value = owner.Slope;
			}
		}

		private class VRage_Game_MyPlanetSurfaceRule_003C_003EActor : IActivator, IActivator<MyPlanetSurfaceRule>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetSurfaceRule();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetSurfaceRule CreateInstance()
			{
				return new MyPlanetSurfaceRule();
			}

			MyPlanetSurfaceRule IActivator<MyPlanetSurfaceRule>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(11)]
		public SerializableRange Height = new SerializableRange(0f, 1f);

		[ProtoMember(12)]
		public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);

		[ProtoMember(13)]
		public SerializableRange Longitude = new SerializableRange(-180f, 180f);

		[ProtoMember(14)]
		public SerializableRange Slope = new SerializableRange(0f, 90f);

		/// Check that a rule matches terrain properties.
		///
		/// @param height Height ration to the height map.
		/// @param latitude Latitude cosine
		/// @param slope Surface dominant angle cosine.
		public bool Check(float height, float latitude, float longitude, float slope)
		{
			if (Height.ValueBetween(height) && Latitude.ValueBetween(latitude) && Longitude.ValueBetween(longitude))
			{
				return Slope.ValueBetween(slope);
			}
			return false;
		}

		public object Clone()
		{
			return new MyPlanetSurfaceRule
			{
				Height = Height,
				Latitude = Latitude,
				Longitude = Longitude,
				Slope = Slope
			};
		}
	}
}
