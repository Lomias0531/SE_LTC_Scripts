using System;

namespace Sandbox.Game.GameSystems.BankingAndCurrency
{
	public struct MyAccountLogEntry
	{
		public long ChangeIdentifier
		{
			get;
			set;
		}

		public long Amount
		{
			get;
			set;
		}

		public DateTime DateTime
		{
			get;
			set;
		}
	}
}
