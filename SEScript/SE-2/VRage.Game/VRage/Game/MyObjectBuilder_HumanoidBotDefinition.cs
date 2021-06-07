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
	public class MyObjectBuilder_HumanoidBotDefinition : MyObjectBuilder_AgentDefinition
	{
		[ProtoContract]
		public class Item
		{
			protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EItem_003C_003EType_003C_003EAccessor : IMemberAccessor<Item, MyObjectBuilderType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Item owner, in MyObjectBuilderType value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Item owner, out MyObjectBuilderType value)
				{
					value = owner.Type;
				}
			}

			protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EItem_003C_003ESubtype_003C_003EAccessor : IMemberAccessor<Item, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Item owner, in string value)
				{
					owner.Subtype = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Item owner, out string value)
				{
					value = owner.Subtype;
				}
			}

			private class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EItem_003C_003EActor : IActivator, IActivator<Item>
			{
				private sealed override object CreateInstance()
				{
					return new Item();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Item CreateInstance()
				{
					return new Item();
				}

				Item IActivator<Item>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlIgnore]
			public MyObjectBuilderType Type = typeof(MyObjectBuilder_PhysicalGunObject);

			[XmlAttribute]
			[ProtoMember(1)]
			public string Subtype;
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EStartingItem_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, Item>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in Item value)
			{
				owner.StartingItem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out Item value)
			{
				value = owner.StartingItem;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EInventoryItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, Item[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in Item[] value)
			{
				owner.InventoryItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out Item[] value)
			{
				value = owner.InventoryItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EBotModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBotModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ETargetType_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ETargetType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EInventoryContentGenerated_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContentGenerated_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EInventoryContainerTypeId_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContainerTypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ERemoveAfterDeath_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveAfterDeath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ERespawnTimeMs_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERespawnTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ERemoveTimeMs_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveTimeMs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_AgentDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_AgentDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EBotBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBotBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, BotBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in BotBehavior value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out BotBehavior value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EBehaviorType_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EBehaviorSubtype_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorSubtype_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ECommandable_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003ECommandable_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_BotDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_BotDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_HumanoidBotDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_HumanoidBotDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_HumanoidBotDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_HumanoidBotDefinition CreateInstance()
			{
				return new MyObjectBuilder_HumanoidBotDefinition();
			}

			MyObjectBuilder_HumanoidBotDefinition IActivator<MyObjectBuilder_HumanoidBotDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public Item StartingItem;

		[XmlArrayItem("Item")]
		[ProtoMember(7)]
		public Item[] InventoryItems;
	}
}
