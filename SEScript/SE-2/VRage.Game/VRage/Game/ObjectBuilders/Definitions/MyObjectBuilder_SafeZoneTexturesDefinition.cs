using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SafeZoneTexturesDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EAlphamask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				owner.Alphamask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				value = owner.Alphamask;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003ENormalGloss_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				owner.NormalGloss = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				value = owner.NormalGloss;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EDisplayTextId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				owner.DisplayTextId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				value = owner.DisplayTextId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneTexturesDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneTexturesDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SafeZoneTexturesDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SafeZoneTexturesDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SafeZoneTexturesDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SafeZoneTexturesDefinition CreateInstance()
			{
				return new MyObjectBuilder_SafeZoneTexturesDefinition();
			}

			MyObjectBuilder_SafeZoneTexturesDefinition IActivator<MyObjectBuilder_SafeZoneTexturesDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[ModdableContentFile(new string[]
		{
			"dds",
			"png"
		})]
		public string Alphamask;

		[ProtoMember(2)]
		[ModdableContentFile(new string[]
		{
			"dds",
			"png"
		})]
		public string NormalGloss;

		[ProtoMember(3)]
		public string DisplayTextId;
	}
}
