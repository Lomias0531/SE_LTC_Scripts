using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Components.BankingAndCurrency;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
	public class MyAccount
	{
		public List<MyAccountLogEntry> Log = new List<MyAccountLogEntry>();

		public long OwnerIdentifier
		{
			get;
			private set;
		}

		public long Balance
		{
			get;
			private set;
		}

		public MyAccount()
		{
		}

		public MyAccount(long ownerIdentifier, long startingBalance)
		{
			OwnerIdentifier = ownerIdentifier;
			Balance = startingBalance;
		}

		public void Init(MyObjectBuilder_Account obAccount)
		{
			OwnerIdentifier = obAccount.OwnerIdentifier;
			Balance = obAccount.Balance;
			Log = new List<MyAccountLogEntry>();
			foreach (MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry item2 in obAccount.Log)
			{
				MyAccountLogEntry item = default(MyAccountLogEntry);
				item.Amount = item2.Amount;
				item.ChangeIdentifier = item2.ChangeIdentifier;
				item.DateTime = new DateTime(item2.DateTime);
				Log.Add(item);
			}
		}

		public MyObjectBuilder_Account GetObjectBuilder()
		{
			MyObjectBuilder_Account myObjectBuilder_Account = new MyObjectBuilder_Account();
			myObjectBuilder_Account.OwnerIdentifier = OwnerIdentifier;
			myObjectBuilder_Account.Balance = Balance;
			myObjectBuilder_Account.Log = new List<MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry>();
			foreach (MyAccountLogEntry item2 in Log)
			{
				MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry item = default(MyObjectBuilder_Account.MyObjectBuilder_AccountLogEntry);
				item.Amount = item2.Amount;
				item.ChangeIdentifier = item2.ChangeIdentifier;
				item.DateTime = item2.DateTime.Ticks;
				myObjectBuilder_Account.Log.Add(item);
			}
			return myObjectBuilder_Account;
		}

		internal void Add(long valueToAdd)
		{
			Balance += valueToAdd;
		}

		internal void Subtract(long valueToSubtract)
		{
			Balance -= valueToSubtract;
		}

		internal MyAccountInfo GetAccountInfo()
		{
			MyAccountInfo myAccountInfo = default(MyAccountInfo);
			myAccountInfo.Balance = Balance;
			myAccountInfo.OwnerIdentifier = OwnerIdentifier;
			MyAccountInfo result = myAccountInfo;
			result.Log = Log.ToArray();
			return result;
		}

		internal void ResetBalance(long newBalance)
		{
			Balance = newBalance;
		}

		public static long operator +(MyAccount account, long addValue)
		{
			return account.Balance + addValue;
		}

		public static long operator +(MyAccount account1, MyAccount account2)
		{
			return account1.Balance + account2.Balance;
		}

		public static long operator -(MyAccount account, long subtractValue)
		{
			return account.Balance - subtractValue;
		}

		public static long operator -(MyAccount account1, MyAccount account2)
		{
			return account1.Balance - account2.Balance;
		}
	}
}
