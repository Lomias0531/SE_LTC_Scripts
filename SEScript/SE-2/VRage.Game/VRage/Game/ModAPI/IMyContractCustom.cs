namespace VRage.Game.ModAPI
{
	public interface IMyContractCustom : IMyContract
	{
		MyDefinitionId DefinitionId
		{
			get;
		}

		long? EndBlockId
		{
			get;
		}

		string Name
		{
			get;
		}

		string Description
		{
			get;
		}

		int ReputationReward
		{
			get;
		}

		int FailReputationPrice
		{
			get;
		}
	}
}
