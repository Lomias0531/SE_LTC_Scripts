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
	public class MyObjectBuilder_ContractTypeRepairDefinition : MyObjectBuilder_ContractTypeDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMaxGridDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in double value)
			{
				owner.MaxGridDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out double value)
			{
				value = owner.MaxGridDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMinGridDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in double value)
			{
				owner.MinGridDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out double value)
			{
				value = owner.MinGridDistance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDuration_BaseTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in double value)
			{
				owner.Duration_BaseTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out double value)
			{
				value = owner.Duration_BaseTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDuration_TimePerMeter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in double value)
			{
				owner.Duration_TimePerMeter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out double value)
			{
				value = owner.Duration_TimePerMeter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EPrefabNames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MySerializableList<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in MySerializableList<string> value)
			{
				owner.PrefabNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out MySerializableList<string> value)
			{
				value = owner.PrefabNames;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EPriceToRewardCoeficient_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in float value)
			{
				owner.PriceToRewardCoeficient = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out float value)
			{
				value = owner.PriceToRewardCoeficient;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EPriceSpread_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in float value)
			{
				owner.PriceSpread = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out float value)
			{
				value = owner.PriceSpread;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003ETimeToPriceDenominator_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in float value)
			{
				owner.TimeToPriceDenominator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out float value)
			{
				value = owner.TimeToPriceDenominator;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMinimumReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMinimumMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMinStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDurationMultiplier_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EDurationMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in double value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out double value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EChancesPerFactionType_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EChancesPerFactionType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyContractChancePair[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in MyContractChancePair[] value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out MyContractChancePair[] value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeRepairDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeRepairDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeRepairDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeRepairDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractTypeRepairDefinition CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeRepairDefinition();
			}

			MyObjectBuilder_ContractTypeRepairDefinition IActivator<MyObjectBuilder_ContractTypeRepairDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public double MaxGridDistance;

		[ProtoMember(3)]
		public double MinGridDistance;

		[ProtoMember(5)]
		public double Duration_BaseTime;

		[ProtoMember(7)]
		public double Duration_TimePerMeter;

		[ProtoMember(9)]
		public MySerializableList<string> PrefabNames;

		[ProtoMember(11)]
		public float PriceToRewardCoeficient;

		[ProtoMember(13)]
		public float PriceSpread;

		[ProtoMember(15)]
		public float TimeToPriceDenominator;
	}
}
