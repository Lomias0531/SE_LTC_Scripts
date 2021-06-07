using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.BankingAndCurrency
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_BankingSystem : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public struct MyObjectBuilder_AccountEntry
		{
			protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EMyObjectBuilder_AccountEntry_003C_003EOwnerIdentifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AccountEntry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_AccountEntry owner, in long value)
				{
					owner.OwnerIdentifier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_AccountEntry owner, out long value)
				{
					value = owner.OwnerIdentifier;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EMyObjectBuilder_AccountEntry_003C_003EAccount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AccountEntry, MyObjectBuilder_Account>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_AccountEntry owner, in MyObjectBuilder_Account value)
				{
					owner.Account = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_AccountEntry owner, out MyObjectBuilder_Account value)
				{
					value = owner.Account;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EMyObjectBuilder_AccountEntry_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AccountEntry>
			{
				private sealed override object CreateInstance()
				{
					return default(MyObjectBuilder_AccountEntry);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyObjectBuilder_AccountEntry CreateInstance()
				{
					return (MyObjectBuilder_AccountEntry)(object)default(MyObjectBuilder_AccountEntry);
				}

				MyObjectBuilder_AccountEntry IActivator<MyObjectBuilder_AccountEntry>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long OwnerIdentifier;

			[ProtoMember(3)]
			public MyObjectBuilder_Account Account;
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EAccounts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem, List<MyObjectBuilder_AccountEntry>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in List<MyObjectBuilder_AccountEntry> value)
			{
				owner.Accounts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out List<MyObjectBuilder_AccountEntry> value)
			{
				value = owner.Accounts;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EOverallBalance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BankingSystem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in long value)
			{
				owner.OverallBalance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out long value)
			{
				value = owner.OverallBalance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BankingSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BankingSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BankingSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_BankingSystem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BankingSystem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BankingSystem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BankingSystem CreateInstance()
			{
				return new MyObjectBuilder_BankingSystem();
			}

			MyObjectBuilder_BankingSystem IActivator<MyObjectBuilder_BankingSystem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<MyObjectBuilder_AccountEntry> Accounts;

		[ProtoMember(3)]
		public long OverallBalance;
	}
}
