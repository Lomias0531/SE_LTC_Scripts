using System;
using VRage.Game;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
	public class MyContractCustom : IMyContractCustom, IMyContract
	{
		public MyDefinitionId DefinitionId
		{
			get;
			private set;
		}

		public long? EndBlockId
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public int ReputationReward
		{
			get;
			private set;
		}

		public int FailReputationPrice
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

		public MyContractCustom(MyDefinitionId definitionId, long startBlockId, int moneyReward, int collateral, int duration, string name, string description, int reputationReward, int failReputationPrice, long? endBlockId)
		{
			DefinitionId = definitionId;
			EndBlockId = endBlockId;
			Name = name;
			Description = description;
			ReputationReward = reputationReward;
			FailReputationPrice = failReputationPrice;
			StartBlockId = startBlockId;
			MoneyReward = moneyReward;
			Collateral = collateral;
			Duration = duration;
		}
	}
}
