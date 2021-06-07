using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContractRepair : MyObjectBuilder_Contract
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EGridPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in SerializableVector3D value)
			{
				owner.GridPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out SerializableVector3D value)
			{
				value = owner.GridPosition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EGridId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				owner.GridId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				value = owner.GridId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EPrefabName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in string value)
			{
				owner.PrefabName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out string value)
			{
				value = owner.PrefabName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EBlocksToRepair_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, MySerializableList<Vector3I>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MySerializableList<Vector3I> value)
			{
				owner.BlocksToRepair = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MySerializableList<Vector3I> value)
			{
				value = owner.BlocksToRepair;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EKeepGridAtTheEnd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in bool value)
			{
				owner.KeepGridAtTheEnd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out bool value)
			{
				value = owner.KeepGridAtTheEnd;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EUnrepairedBlockCount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in int value)
			{
				owner.UnrepairedBlockCount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out int value)
			{
				value = owner.UnrepairedBlockCount;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EId_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EIsPlayerMade_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EIsPlayerMade_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EState_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EState_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyContractStateEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MyContractStateEnum value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MyContractStateEnum value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EOwners_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EOwners_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MySerializableList<long> value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MySerializableList<long> value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ERewardMoney_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardMoney_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ERewardReputation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardReputation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EStartingDeposit_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartingDeposit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EFailReputationPrice_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EFailReputationPrice_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in int value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out int value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EStartFaction_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartFaction_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EStartStation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartStation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EStartBlock_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartBlock_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ECreation_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ECreation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in long value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out long value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ETicksToDiscard_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ETicksToDiscard_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in int? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out int? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ERemainingTimeInS_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERemainingTimeInS_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in double? value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out double? value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EContractCondition_003C_003EAccessor : VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EContractCondition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyObjectBuilder_ContractCondition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MyObjectBuilder_ContractCondition value)
			{
				Set(ref *(MyObjectBuilder_Contract*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MyObjectBuilder_ContractCondition value)
			{
				Get(ref *(MyObjectBuilder_Contract*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContractRepair owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContractRepair owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContractRepair>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContractRepair();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContractRepair CreateInstance()
			{
				return new MyObjectBuilder_ContractRepair();
			}

			MyObjectBuilder_ContractRepair IActivator<MyObjectBuilder_ContractRepair>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public SerializableVector3D GridPosition;

		[ProtoMember(3)]
		public long GridId;

		[ProtoMember(5)]
		public string PrefabName;

		[ProtoMember(7)]
		public MySerializableList<Vector3I> BlocksToRepair;

		[ProtoMember(9)]
		public bool KeepGridAtTheEnd;

		[ProtoMember(11)]
		public int UnrepairedBlockCount;
	}
}
