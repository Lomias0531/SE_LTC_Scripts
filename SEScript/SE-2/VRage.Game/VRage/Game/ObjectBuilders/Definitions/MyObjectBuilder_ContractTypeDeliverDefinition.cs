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
	public class MyObjectBuilder_ContractTypeDeliverDefinition : MyObjectBuilder_ContractTypeDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDuration_BaseTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in double value)
			{
				owner.Duration_BaseTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out double value)
			{
				value = owner.Duration_BaseTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDuration_TimePerJumpDist_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in double value)
			{
				owner.Duration_TimePerJumpDist = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out double value)
			{
				value = owner.Duration_TimePerJumpDist;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDuration_TimePerMeter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in double value)
			{
				owner.Duration_TimePerMeter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out double value)
			{
				value = owner.Duration_TimePerMeter;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EMinimumReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EMinimumMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinimumMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMoneyReputationCoeficient_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EMinStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMinStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EMaxStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in long value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out long value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDurationMultiplier_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EDurationMultiplier_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in double value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out double value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EChancesPerFactionType_003C_003EAccessor : VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition_003C_003EChancesPerFactionType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, MyContractChancePair[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in MyContractChancePair[] value)
			{
				Set(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out MyContractChancePair[] value)
			{
				Get(ref *(MyObjectBuilder_ContractTypeDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeDeliverDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractTypeDeliverDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDeliverDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeDeliverDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeDeliverDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractTypeDeliverDefinition CreateInstance()
			{
				return new MyObjectBuilder_ContractTypeDeliverDefinition();
			}

			MyObjectBuilder_ContractTypeDeliverDefinition IActivator<MyObjectBuilder_ContractTypeDeliverDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public double Duration_BaseTime;

		[ProtoMember(3)]
		public double Duration_TimePerJumpDist;

		[ProtoMember(5)]
		public double Duration_TimePerMeter;
	}
}
