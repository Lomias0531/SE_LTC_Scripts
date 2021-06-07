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
	public class MyObjectBuilder_Account : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct MyObjectBuilder_AccountLogEntry
		{
			protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EMyObjectBuilder_AccountLogEntry_003C_003EChangeIdentifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AccountLogEntry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_AccountLogEntry owner, in long value)
				{
					owner.ChangeIdentifier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_AccountLogEntry owner, out long value)
				{
					value = owner.ChangeIdentifier;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EMyObjectBuilder_AccountLogEntry_003C_003EAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AccountLogEntry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_AccountLogEntry owner, in long value)
				{
					owner.Amount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_AccountLogEntry owner, out long value)
				{
					value = owner.Amount;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EMyObjectBuilder_AccountLogEntry_003C_003EDateTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AccountLogEntry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyObjectBuilder_AccountLogEntry owner, in long value)
				{
					owner.DateTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyObjectBuilder_AccountLogEntry owner, out long value)
				{
					value = owner.DateTime;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EMyObjectBuilder_AccountLogEntry_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AccountLogEntry>
			{
				private sealed override object CreateInstance()
				{
					return default(MyObjectBuilder_AccountLogEntry);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyObjectBuilder_AccountLogEntry CreateInstance()
				{
					return (MyObjectBuilder_AccountLogEntry)(object)default(MyObjectBuilder_AccountLogEntry);
				}

				MyObjectBuilder_AccountLogEntry IActivator<MyObjectBuilder_AccountLogEntry>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long ChangeIdentifier;

			[ProtoMember(3)]
			public long Amount;

			[ProtoMember(5)]
			public long DateTime;
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EOwnerIdentifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Account, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Account owner, in long value)
			{
				owner.OwnerIdentifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Account owner, out long value)
			{
				value = owner.OwnerIdentifier;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EBalance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Account, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Account owner, in long value)
			{
				owner.Balance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Account owner, out long value)
			{
				value = owner.Balance;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003ELog_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Account, List<MyObjectBuilder_AccountLogEntry>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Account owner, in List<MyObjectBuilder_AccountLogEntry> value)
			{
				owner.Log = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Account owner, out List<MyObjectBuilder_AccountLogEntry> value)
			{
				value = owner.Log;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Account, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Account owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Account owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Account, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Account owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Account owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Account, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Account owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Account owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Account, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Account owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Account owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_BankingAndCurrency_MyObjectBuilder_Account_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Account>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Account();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Account CreateInstance()
			{
				return new MyObjectBuilder_Account();
			}

			MyObjectBuilder_Account IActivator<MyObjectBuilder_Account>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Identifier of the owner. Could be anything (player identityId, factionId)
		/// </summary>
		[ProtoMember(1)]
		public long OwnerIdentifier;

		/// <summary>
		/// Current balance of the account.
		/// </summary>
		[ProtoMember(3)]
		public long Balance;

		/// <summary>
		/// Log of changes on the account.
		/// </summary>
		[ProtoMember(5)]
		public List<MyObjectBuilder_AccountLogEntry> Log;
	}
}
