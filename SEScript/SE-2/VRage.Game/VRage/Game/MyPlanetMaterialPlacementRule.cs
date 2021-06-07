using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	/// Important!
	///
	/// Due to the geometry in question the slope is stored as the cosine (used in dot product).
	///
	/// Meanwhile the dot product for the latitude yields the cosine of the modulus of the compliment of our angle.
	/// This means after the maths are done that what we have is the *sine*, so the latitude is stored as the sine.
	[ProtoContract]
	public class MyPlanetMaterialPlacementRule : MyPlanetMaterialDefinition, ICloneable
	{
		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003EHeight_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value)
			{
				owner.Height = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value)
			{
				value = owner.Height;
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003ELatitude_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SymmetricSerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in SymmetricSerializableRange value)
			{
				owner.Latitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out SymmetricSerializableRange value)
			{
				value = owner.Latitude;
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003ELongitude_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value)
			{
				owner.Longitude = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value)
			{
				value = owner.Longitude;
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003ESlope_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialPlacementRule, SerializableRange>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in SerializableRange value)
			{
				owner.Slope = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out SerializableRange value)
			{
				value = owner.Slope;
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003EMaterial_003C_003EAccessor : VRage_Game_MyPlanetMaterialDefinition_003C_003EMaterial_003C_003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in string value)
			{
				Set(ref *(MyPlanetMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out string value)
			{
				Get(ref *(MyPlanetMaterialDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003EValue_003C_003EAccessor : VRage_Game_MyPlanetMaterialDefinition_003C_003EValue_003C_003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in byte value)
			{
				Set(ref *(MyPlanetMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out byte value)
			{
				Get(ref *(MyPlanetMaterialDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003EMaxDepth_003C_003EAccessor : VRage_Game_MyPlanetMaterialDefinition_003C_003EMaxDepth_003C_003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in float value)
			{
				Set(ref *(MyPlanetMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out float value)
			{
				Get(ref *(MyPlanetMaterialDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyPlanetMaterialPlacementRule_003C_003ELayers_003C_003EAccessor : VRage_Game_MyPlanetMaterialDefinition_003C_003ELayers_003C_003EAccessor, IMemberAccessor<MyPlanetMaterialPlacementRule, MyPlanetMaterialLayer[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyPlanetMaterialPlacementRule owner, in MyPlanetMaterialLayer[] value)
			{
				Set(ref *(MyPlanetMaterialDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyPlanetMaterialPlacementRule owner, out MyPlanetMaterialLayer[] value)
			{
				Get(ref *(MyPlanetMaterialDefinition*)(&owner), out value);
			}
		}

		private class VRage_Game_MyPlanetMaterialPlacementRule_003C_003EActor : IActivator, IActivator<MyPlanetMaterialPlacementRule>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetMaterialPlacementRule();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaterialPlacementRule CreateInstance()
			{
				return new MyPlanetMaterialPlacementRule();
			}

			MyPlanetMaterialPlacementRule IActivator<MyPlanetMaterialPlacementRule>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(7)]
		public SerializableRange Height = new SerializableRange(0f, 1f);

		[ProtoMember(8)]
		public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);

		[ProtoMember(9)]
		public SerializableRange Longitude = new SerializableRange(-180f, 180f);

		[ProtoMember(10)]
		public SerializableRange Slope = new SerializableRange(0f, 90f);

		public override bool IsRule => true;

		public MyPlanetMaterialPlacementRule()
		{
		}

		public MyPlanetMaterialPlacementRule(MyPlanetMaterialPlacementRule copyFrom)
		{
			Height = copyFrom.Height;
			Latitude = copyFrom.Latitude;
			Longitude = copyFrom.Longitude;
			Slope = copyFrom.Slope;
			Material = copyFrom.Material;
			Value = copyFrom.Value;
			MaxDepth = copyFrom.MaxDepth;
			Layers = copyFrom.Layers;
		}

		/// Check that a rule matches terrain properties.
		///
		/// @param height Height ration to the height map.
		/// @param latitude Latitude cosine
		/// @param slope Surface dominant angle sine.
		public bool Check(float height, float latitude, float slope)
		{
			if (Height.ValueBetween(height) && Latitude.ValueBetween(latitude))
			{
				return Slope.ValueBetween(slope);
			}
			return false;
		}

		public new object Clone()
		{
			return new MyPlanetMaterialPlacementRule(this);
		}
	}
}
