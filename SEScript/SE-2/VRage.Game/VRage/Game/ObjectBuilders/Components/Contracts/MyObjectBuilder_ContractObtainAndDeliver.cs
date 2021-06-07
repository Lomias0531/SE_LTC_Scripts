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
	public class MyObjectBuilder_ContractObtainAndDeliver : MyObjectBuilder_Contract
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EIsPlayerMade_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EIsPlayerMade_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EState_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EState_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyContractStateEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in MyContractStateEnum value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out MyContractStateEnum value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EOwners_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EOwners_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in MySerializableList<long> value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out MySerializableList<long> value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ERewardMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ERewardReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EStartFaction_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartFaction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EStartStation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartStation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EStartBlock_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ECreation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ECreation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ETicksToDiscard_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ETicksToDiscard_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ERemainingTimeInS_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERemainingTimeInS_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in double? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out double? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EContractCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EContractCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyObjectBuilder_ContractCondition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in MyObjectBuilder_ContractCondition value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out MyObjectBuilder_ContractCondition value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractObtainAndDeliver>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractObtainAndDeliver();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractObtainAndDeliver CreateInstance()
			{
				return new MyObjectBuilder_ContractObtainAndDeliver();
			}

			MyObjectBuilder_ContractObtainAndDeliver IActivator<MyObjectBuilder_ContractObtainAndDeliver>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
