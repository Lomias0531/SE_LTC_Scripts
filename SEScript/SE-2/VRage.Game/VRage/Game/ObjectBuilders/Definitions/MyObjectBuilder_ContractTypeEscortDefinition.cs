using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContractTypeEscortDefinition : MyObjectBuilder_ContractTypeDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003ERewardRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in double value)
			{
				owner.RewardRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out double value)
			{
				value = owner.RewardRadius;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003ETriggerRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in double value)
			{
				owner.TriggerRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out double value)
			{
				value = owner.TriggerRadius;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003ETravelDistanceMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in double value)
			{
				owner.TravelDistanceMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out double value)
			{
				value = owner.TravelDistanceMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003ETravelDistanceMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in double value)
			{
				owner.TravelDistanceMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out double value)
			{
				value = owner.TravelDistanceMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDroneFirstDelayInS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				owner.DroneFirstDelayInS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				value = owner.DroneFirstDelayInS;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDroneAttackPeriodInS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				owner.DroneAttackPeriodInS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				value = owner.DroneAttackPeriodInS;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EInitialDelayInS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				owner.InitialDelayInS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				value = owner.InitialDelayInS;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDronesPerWave_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				owner.DronesPerWave = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				value = owner.DronesPerWave;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EPrefabsAttackDrones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MySerializableList<string> value)
			{
				owner.PrefabsAttackDrones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MySerializableList<string> value)
			{
				value = owner.PrefabsAttackDrones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EPrefabsEscortShips_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MySerializableList<string> value)
			{
				owner.PrefabsEscortShips = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MySerializableList<string> value)
			{
				value = owner.PrefabsEscortShips;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDroneBehaviours_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MySerializableList<string> value)
			{
				owner.DroneBehaviours = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MySerializableList<string> value)
			{
				value = owner.DroneBehaviours;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDuration_BaseTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in float value)
			{
				owner.Duration_BaseTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out float value)
			{
				value = owner.Duration_BaseTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDuration_FlightTimeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in float value)
			{
				owner.Duration_FlightTimeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out float value)
			{
				value = owner.Duration_FlightTimeMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EMinimumReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EMinimumMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EMinStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDurationMultiplier_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EDurationMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in double value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out double value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EChancesPerFactionType_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EChancesPerFactionType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyContractChancePair[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MyContractChancePair[] value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MyContractChancePair[] value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeEscortDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeEscortDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeEscortDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeEscortDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractTypeEscortDefinition CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeEscortDefinition();
			}

			MyObjectBuilder_ContractTypeEscortDefinition IActivator<MyObjectBuilder_ContractTypeEscortDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public double RewardRadius;

		[ProtoMember(3)]
		public double TriggerRadius;

		[ProtoMember(5)]
		public double TravelDistanceMax;

		[ProtoMember(7)]
		public double TravelDistanceMin;

		[ProtoMember(9)]
		public int DroneFirstDelayInS;

		[ProtoMember(11)]
		public int DroneAttackPeriodInS;

		[ProtoMember(13)]
		public int InitialDelayInS;

		[ProtoMember(14)]
		public int DronesPerWave;

		[ProtoMember(15)]
		public MySerializableList<string> PrefabsAttackDrones;

		[ProtoMember(17)]
		public MySerializableList<string> PrefabsEscortShips;

		[ProtoMember(19)]
		public MySerializableList<string> DroneBehaviours;

		[ProtoMember(21)]
		public float Duration_BaseTime;

		[ProtoMember(23)]
		public float Duration_FlightTimeMultiplier;
	}
}
