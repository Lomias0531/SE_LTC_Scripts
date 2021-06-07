using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContractCustom : MyObjectBuilder_Contract
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EDefinitionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractCustom, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in SerializableDefinitionId value)
			{
				owner.DefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out SerializableDefinitionId value)
			{
				value = owner.DefinitionId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EContractName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in string value)
			{
				owner.ContractName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out string value)
			{
				value = owner.ContractName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EContractDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in string value)
			{
				owner.ContractDescription = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out string value)
			{
				value = owner.ContractDescription;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EIsPlayerMade_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EIsPlayerMade_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EState_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EState_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, MyContractStateEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in MyContractStateEnum value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out MyContractStateEnum value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EOwners_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EOwners_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in MySerializableList<long> value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out MySerializableList<long> value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ERewardMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ERewardReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EStartFaction_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartFaction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EStartStation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartStation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EStartBlock_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ECreation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ECreation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ETicksToDiscard_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ETicksToDiscard_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in int? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out int? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ERemainingTimeInS_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERemainingTimeInS_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in double? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out double? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EContractCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EContractCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, MyObjectBuilder_ContractCondition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in MyObjectBuilder_ContractCondition value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out MyObjectBuilder_ContractCondition value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractCustom, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractCustom owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractCustom owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCustom_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractCustom>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractCustom();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractCustom CreateInstance()
			{
				return new MyObjectBuilder_ContractCustom();
			}

			MyObjectBuilder_ContractCustom IActivator<MyObjectBuilder_ContractCustom>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableDefinitionId DefinitionId;

		[ProtoMember(3)]
		public string ContractName;

		[ProtoMember(5)]
		public string ContractDescription;
	}
}
