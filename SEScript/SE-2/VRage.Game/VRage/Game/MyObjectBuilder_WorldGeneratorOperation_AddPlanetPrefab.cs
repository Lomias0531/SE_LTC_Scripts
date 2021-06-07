using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("AddPlanetPrefab")]
	public class MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EPrefabName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in string value)
			{
				owner.PrefabName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out string value)
			{
				value = owner.PrefabName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EDefinitionName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in string value)
			{
				owner.DefinitionName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out string value)
			{
				value = owner.DefinitionName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EAddGPS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in bool value)
			{
				owner.AddGPS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out bool value)
			{
				value = owner.AddGPS;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in SerializableVector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out SerializableVector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab();
			}

			MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(160)]
		[XmlAttribute]
		public string PrefabName;

		[ProtoMember(163)]
		[XmlAttribute]
		public string DefinitionName;

		[ProtoMember(166)]
		[XmlAttribute]
		public bool AddGPS;

		[ProtoMember(169)]
		public SerializableVector3D Position;
	}
}
