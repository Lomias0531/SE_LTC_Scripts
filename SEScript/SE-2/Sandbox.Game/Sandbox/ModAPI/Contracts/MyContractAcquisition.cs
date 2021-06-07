using System;
using VRage.Game;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
	public class MyContractAcquisition : IMyContractAcquisition, IMyContract
	{
		public long EndBlockId
		{
			get;
			private set;
		}

		public MyDefinitionId ItemTypeId
		{
			get;
			private set;
		}

		public int ItemAmount
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

		public MyContractAcquisition(long startBlockId, int moneyReward, int collateral, int duration, long endBlockId, MyDefinitionId itemTypeId, int itemAmount)
		{
			StartBlockId = startBlockId;
			MoneyReward = moneyReward;
			Collateral = collateral;
			Duration = duration;
			EndBlockId = endBlockId;
			ItemTypeId = itemTypeId;
			ItemAmount = itemAmount;
		}
	}
}
