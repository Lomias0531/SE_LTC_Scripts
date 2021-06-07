using System;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
	public class MyContractSearch : IMyContractSearch, IMyContract
	{
		public long TargetGridId
		{
			get;
			private set;
		}

		public double SearchRadius
		{
			get;
			private set;
		}

		public long StartBlockId
		{
			get;
			private set;
		}

		public int MoneyReward
		{
			get;
			private set;
		}

		public int Collateral
		{
			get;
			private set;
		}

		public int Duration
		{
			get;
			private set;
		}

		public Action<long> OnContractAcquired
		{
			get;
			set;
		}

		public Action OnContractSucceeded
		{
			get;
			set;
		}

		public Action OnContractFailed
		{
			get;
			set;
		}

		public MyContractSearch(long startBlockId, int moneyReward, int collateral, int duration, long targetGridId, double searchRadius)
		{
			StartBlockId = startBlockId;
			MoneyReward = moneyReward;
			Collateral = collateral;
			Duration = duration;
			TargetGridId = targetGridId;
			SearchRadius = searchRadius;
		}
	}
}
