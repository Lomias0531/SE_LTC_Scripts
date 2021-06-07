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
	public class MyObjectBuilder_ContractTypeHuntDefinition : MyObjectBuilder_ContractTypeDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003ERemarkPeriodInS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in int value)
			{
				owner.RemarkPeriodInS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out int value)
			{
				value = owner.RemarkPeriodInS;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003ERemarkVariance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in float value)
			{
				owner.RemarkVariance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out float value)
			{
				value = owner.RemarkVariance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EKillRange_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in double value)
			{
				owner.KillRange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out double value)
			{
				value = owner.KillRange;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EKillRangeMultiplier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in float value)
			{
				owner.KillRangeMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out float value)
			{
				value = owner.KillRangeMultiplier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EReputationLossForTarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in int value)
			{
				owner.ReputationLossForTarget = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out int value)
			{
				value = owner.ReputationLossForTarget;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003ERewardRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in double value)
			{
				owner.RewardRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out double value)
			{
				value = owner.RewardRadius;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDuration_BaseTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in double value)
			{
				owner.Duration_BaseTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out double value)
			{
				value = owner.Duration_BaseTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EMinimumReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EMinimumMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EMinStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDurationMultiplier_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EDurationMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in double value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out double value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EChancesPerFactionType_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EChancesPerFactionType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyContractChancePair[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in MyContractChancePair[] value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out MyContractChancePair[] value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeHuntDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeHuntDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeHuntDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeHuntDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractTypeHuntDefinition CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeHuntDefinition();
			}

			MyObjectBuilder_ContractTypeHuntDefinition IActivator<MyObjectBuilder_ContractTypeHuntDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int RemarkPeriodInS;

		[ProtoMember(3)]
		public float RemarkVariance;

		[ProtoMember(5)]
		public double KillRange;

		[ProtoMember(7)]
		public float KillRangeMultiplier;

		[ProtoMember(8)]
		public int ReputationLossForTarget;

		[ProtoMember(9)]
		public double RewardRadius;

		[ProtoMember(11)]
		public double Duration_BaseTime;
	}
}
