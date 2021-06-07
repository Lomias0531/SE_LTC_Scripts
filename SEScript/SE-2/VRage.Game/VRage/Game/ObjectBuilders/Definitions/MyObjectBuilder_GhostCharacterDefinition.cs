using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_GhostCharacterDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003ELeftHandWeapons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.LeftHandWeapons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.LeftHandWeapons;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003ERightHandWeapons_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, SerializableDefinitionId[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in SerializableDefinitionId[] value)
			{
				owner.RightHandWeapons = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out SerializableDefinitionId[] value)
			{
				value = owner.RightHandWeapons;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GhostCharacterDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GhostCharacterDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GhostCharacterDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GhostCharacterDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GhostCharacterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GhostCharacterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GhostCharacterDefinition CreateInstance()
			{
				return new MyObjectBuilder_GhostCharacterDefinition();
			}

			MyObjectBuilder_GhostCharacterDefinition IActivator<MyObjectBuilder_GhostCharacterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("WeaponId")]
		[ProtoMember(1)]
		public SerializableDefinitionId[] LeftHandWeapons;

		[XmlArrayItem("WeaponId")]
		[ProtoMember(4)]
		public SerializableDefinitionId[] RightHandWeapons;
	}
}
