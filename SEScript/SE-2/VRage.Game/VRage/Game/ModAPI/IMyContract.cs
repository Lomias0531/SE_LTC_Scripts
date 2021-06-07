using System;

namespace VRage.Game.ModAPI
{
	public interface IMyContract
	{
		long StartBlockId
		{
			get;
		}

		int MoneyReward
		{
			get;
		}

		int Collateral
		{
			get;
		}

		int Duration
		{
			get;
		}

		Action<long> OnContractAcquired
		{
			get;
			set;
		}

		Action OnContractSucceeded
		{
			get;
			set;
		}

		Action OnContractFailed
		{
			get;
			set;
		}
	}
}
