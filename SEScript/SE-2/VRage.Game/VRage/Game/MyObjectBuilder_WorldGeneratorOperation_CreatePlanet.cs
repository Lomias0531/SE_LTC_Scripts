using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("CreatePlanet")]
	public class MyObjectBuilder_WorldGeneratorOperation_CreatePlanet : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EDefinitionName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in string value)
			{
				owner.DefinitionName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out string value)
			{
				value = owner.DefinitionName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EAddGPS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in bool value)
			{
				owner.AddGPS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out bool value)
			{
				value = owner.AddGPS;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EPositionMinCorner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in SerializableVector3D value)
			{
				owner.PositionMinCorner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out SerializableVector3D value)
			{
				value = owner.PositionMinCorner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EPositionCenter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in SerializableVector3D value)
			{
				owner.PositionCenter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out SerializableVector3D value)
			{
				value = owner.PositionCenter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EDiameter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in float value)
			{
				owner.Diameter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out float value)
			{
				value = owner.Diameter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_CreatePlanet();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_CreatePlanet CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_CreatePlanet();
			}

			MyObjectBuilder_WorldGeneratorOperation_CreatePlanet IActivator<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(172)]
		[XmlAttribute]
		public string DefinitionName;

		[ProtoMember(175)]
		[XmlAttribute]
		public bool AddGPS;

		[ProtoMember(178)]
		public SerializableVector3D PositionMinCorner;

		[ProtoMember(181)]
		public SerializableVector3D PositionCenter = new SerializableVector3D(Vector3.Invalid);

		[ProtoMember(184)]
		public float Diameter;
	}
}
