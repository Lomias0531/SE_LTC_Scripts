using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContractConditionDeliverItems : MyObjectBuilder_ContractCondition
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EItemType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in SerializableDefinitionId value)
			{
				owner.ItemType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out SerializableDefinitionId value)
			{
				value = owner.ItemType;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EItemAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in int value)
			{
				owner.ItemAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out int value)
			{
				value = owner.ItemAmount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EItemVolume_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in float value)
			{
				owner.ItemVolume = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out float value)
			{
				value = owner.ItemVolume;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003ETransferItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in bool value)
			{
				owner.TransferItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out bool value)
			{
				value = owner.TransferItems;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EIsFinished_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EIsFinished_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EContractId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EContractId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003ESubId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003ESubId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EStationEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EStationEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EFactionEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EFactionEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EBlockEndId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003C_003EBlockEndId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractCondition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractCondition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractConditionDeliverItems owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractConditionDeliverItems owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractConditionDeliverItems>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractConditionDeliverItems();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractConditionDeliverItems CreateInstance()
			{
				return new MyObjectBuilder_ContractConditionDeliverItems();
			}

			MyObjectBuilder_ContractConditionDeliverItems IActivator<MyObjectBuilder_ContractConditionDeliverItems>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId ItemType;

		[ProtoMember(3)]
		public int ItemAmount;

		[ProtoMember(4)]
		public float ItemVolume;

		[ProtoMember(5)]
		public bool TransferItems;
	}
}
