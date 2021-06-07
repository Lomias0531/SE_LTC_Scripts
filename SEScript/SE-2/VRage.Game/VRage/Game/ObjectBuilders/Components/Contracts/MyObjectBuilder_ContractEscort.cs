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
	public class MyObjectBuilder_ContractEscort : MyObjectBuilder_Contract
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EGridId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.GridId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.GridId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EStartPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in SerializableVector3D value)
			{
				owner.StartPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out SerializableVector3D value)
			{
				value = owner.StartPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EEndPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in SerializableVector3D value)
			{
				owner.EndPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out SerializableVector3D value)
			{
				value = owner.EndPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EPathLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in double value)
			{
				owner.PathLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out double value)
			{
				value = owner.PathLength;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ERewardRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in double value)
			{
				owner.RewardRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out double value)
			{
				value = owner.RewardRadius;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ETriggerEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.TriggerEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.TriggerEntityId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ETriggerRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in double value)
			{
				owner.TriggerRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out double value)
			{
				value = owner.TriggerRadius;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EDroneAttackPeriod_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.DroneAttackPeriod = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.DroneAttackPeriod;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EDroneFirstDelay_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.DroneFirstDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.DroneFirstDelay;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EInnerTimer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.InnerTimer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.InnerTimer;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EInitialDelay_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.InitialDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.InitialDelay;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EDronesPerWave_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in int value)
			{
				owner.DronesPerWave = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out int value)
			{
				value = owner.DronesPerWave;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EIsBehaviorAttached_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in bool value)
			{
				owner.IsBehaviorAttached = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out bool value)
			{
				value = owner.IsBehaviorAttached;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EWaveFactionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.WaveFactionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.WaveFactionId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EDrones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MySerializableList<long> value)
			{
				owner.Drones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MySerializableList<long> value)
			{
				value = owner.Drones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EEscortShipOwner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				owner.EscortShipOwner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				value = owner.EscortShipOwner;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EDestinationReached_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in bool value)
			{
				owner.DestinationReached = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out bool value)
			{
				value = owner.DestinationReached;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EIsPlayerMade_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EIsPlayerMade_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EState_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EState_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyContractStateEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MyContractStateEnum value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MyContractStateEnum value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EOwners_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EOwners_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MySerializableList<long> value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MySerializableList<long> value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ERewardMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ERewardReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EStartFaction_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartFaction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EStartStation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartStation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EStartBlock_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ECreation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ECreation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ETicksToDiscard_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ETicksToDiscard_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in int? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out int? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ERemainingTimeInS_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERemainingTimeInS_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in double? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out double? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EContractCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EContractCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyObjectBuilder_ContractCondition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MyObjectBuilder_ContractCondition value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MyObjectBuilder_ContractCondition value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractEscort owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractEscort owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractEscort>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractEscort();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractEscort CreateInstance()
			{
				return new MyObjectBuilder_ContractEscort();
			}

			MyObjectBuilder_ContractEscort IActivator<MyObjectBuilder_ContractEscort>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long GridId;

		[ProtoMember(3)]
		public SerializableVector3D StartPosition;

		[ProtoMember(5)]
		public SerializableVector3D EndPosition;

		[ProtoMember(7)]
		public double PathLength;

		[ProtoMember(10)]
		public double RewardRadius;

		[ProtoMember(11)]
		public long TriggerEntityId;

		[ProtoMember(13)]
		public double TriggerRadius;

		[ProtoMember(15)]
		public long DroneAttackPeriod;

		[ProtoMember(17)]
		public long DroneFirstDelay;

		[ProtoMember(19)]
		public long InnerTimer;

		[ProtoMember(21)]
		public long InitialDelay;

		[ProtoMember(22)]
		public int DronesPerWave;

		[ProtoMember(23)]
		public bool IsBehaviorAttached;

		[ProtoMember(24)]
		public long WaveFactionId;

		[ProtoMember(25)]
		public MySerializableList<long> Drones;

		[ProtoMember(27)]
		public long EscortShipOwner;

		[ProtoMember(29)]
		public bool DestinationReached;
	}
}
