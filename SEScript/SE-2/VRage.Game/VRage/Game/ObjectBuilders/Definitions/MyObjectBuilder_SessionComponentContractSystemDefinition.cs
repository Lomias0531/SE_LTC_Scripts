using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SessionComponentContractSystemDefinition : MyObjectBuilder_SessionComponentDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EContractPerPlayerLimit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in int value)
			{
				owner.ContractPerPlayerLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out int value)
			{
				value = owner.ContractPerPlayerLimit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SessionComponentContractSystemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentContractSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContractSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionComponentContractSystemDefinition CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContractSystemDefinition();
			}

			MyObjectBuilder_SessionComponentContractSystemDefinition IActivator<MyObjectBuilder_SessionComponentContractSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int ContractPerPlayerLimit;
	}
}
