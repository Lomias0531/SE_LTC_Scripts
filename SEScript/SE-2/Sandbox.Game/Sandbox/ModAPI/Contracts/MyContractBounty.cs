using System;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
	public class MyContractBounty : IMyContractBounty, IMyContract
	{
		public long TargetIdentityId
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

		public MyContractBounty(long startBlockId, int moneyReward, int collateral, int duration, long targetIdentityId)
		{
			StartBlockId = startBlockId;
			MoneyReward = moneyReward;
			Collateral = collateral;
			Duration = duration;
			TargetIdentityId = targetIdentityId;
		}
	}
}
