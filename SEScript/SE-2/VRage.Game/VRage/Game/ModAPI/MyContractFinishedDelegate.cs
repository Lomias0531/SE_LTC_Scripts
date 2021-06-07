namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Delegate used for event triggered when contract is finished.
	/// </summary>
	/// <param name="contractId">Contract id.</param>
	/// <param name="identityId">Identity id associated with the contract.</param>
	/// <param name="rewardeeCount">Amount of rewardees.</param>
	public delegate void MyContractFinishedDelegate(long contractId, long identityId, int rewardeeCount);
}
