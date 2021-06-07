namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Delegate used for event of contract failure.
	/// </summary>
	/// <param name="contractId">Id of contract that failed.</param>
	/// <param name="identityId">Identity id associated with the contract.</param>
	/// <param name="isAbandon">True if contract was abandond. Otherwise false.</param>
	public delegate void MyContractFailedDelegate(long contractId, long identityId, bool isAbandon);
}
