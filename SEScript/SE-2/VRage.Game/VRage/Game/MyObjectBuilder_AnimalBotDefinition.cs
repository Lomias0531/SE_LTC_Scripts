using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimalBotDefinition : MyObjectBuilder_AgentDefinition
	{
		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EBotModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBotModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ETargetType_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ETargetType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EInventoryContentGenerated_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContentGenerated_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EInventoryContainerTypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContainerTypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ERemoveAfterDeath_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveAfterDeath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ERespawnTimeMs_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERespawnTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ERemoveTimeMs_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EBotBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBotBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, BotBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in BotBehavior value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out BotBehavior value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EBehaviorType_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EBehaviorSubtype_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorSubtype_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ECommandable_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003ECommandable_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AnimalBotDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimalBotDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimalBotDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimalBotDefinition CreateInstance()
			{
				return new MyObjectBuilder_AnimalBotDefinition();
			}

			MyObjectBuilder_AnimalBotDefinition IActivator<MyObjectBuilder_AnimalBotDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
