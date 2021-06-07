using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Contract : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EIsPlayerMade_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in bool value)
			{
				owner.IsPlayerMade = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out bool value)
			{
				value = owner.IsPlayerMade;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EState_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MyContractStateEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in MyContractStateEnum value)
			{
				owner.State = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out MyContractStateEnum value)
			{
				value = owner.State;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EOwners_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MySerializableList<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in MySerializableList<long> value)
			{
				owner.Owners = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out MySerializableList<long> value)
			{
				value = owner.Owners;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardMoney_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.RewardMoney = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.RewardMoney;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERewardReputation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in int value)
			{
				owner.RewardReputation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out int value)
			{
				value = owner.RewardReputation;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartingDeposit_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.StartingDeposit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.StartingDeposit;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EFailReputationPrice_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in int value)
			{
				owner.FailReputationPrice = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out int value)
			{
				value = owner.FailReputationPrice;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartFaction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.StartFaction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.StartFaction;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.StartStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.StartStation;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EStartBlock_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.StartBlock = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.StartBlock;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ECreation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in long value)
			{
				owner.Creation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out long value)
			{
				value = owner.Creation;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ETicksToDiscard_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in int? value)
			{
				owner.TicksToDiscard = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out int? value)
			{
				value = owner.TicksToDiscard;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ERemainingTimeInS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, double?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in double? value)
			{
				owner.RemainingTimeInS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out double? value)
			{
				value = owner.RemainingTimeInS;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EContractCondition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MyObjectBuilder_ContractCondition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Contract owner, in MyObjectBuilder_ContractCondition value)
			{
				owner.ContractCondition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Contract owner, out MyObjectBuilder_ContractCondition value)
			{
				value = owner.ContractCondition;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Contract owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Contract owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Contract owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Contract owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Contract owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Contract owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Contract owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Contract owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Contract>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Contract();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Contract CreateInstance()
			{
				return new MyObjectBuilder_Contract();
			}

			MyObjectBuilder_Contract IActivator<MyObjectBuilder_Contract>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long Id;

		[ProtoMember(2)]
		public bool IsPlayerMade;

		[ProtoMember(3)]
		public MyContractStateEnum State;

		[ProtoMember(5)]
		public MySerializableList<long> Owners;

		[ProtoMember(7)]
		public long RewardMoney;

		[ProtoMember(9)]
		public int RewardReputation;

		[ProtoMember(11)]
		public long StartingDeposit;

		[ProtoMember(13)]
		public int FailReputationPrice;

		[ProtoMember(15)]
		public long StartFaction;

		[ProtoMember(17)]
		public long StartStation;

		[ProtoMember(18)]
		public long StartBlock;

		[ProtoMember(19)]
		public long Creation;

		[ProtoMember(21)]
		public int? TicksToDiscard;

		[ProtoMember(23)]
		public double? RemainingTimeInS;

		[ProtoMember(25)]
		[Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))]
		[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_ContractCondition>))]
		public MyObjectBuilder_ContractCondition ContractCondition;

		public MyObjectBuilder_Contract()
		{
			Owners = new MySerializableList<long>();
		}
	}
}
