namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Delegate used for contract condition finished.
	/// </summary>
	/// <param name="conditionId">Condition id.</param>
	/// <param name="contractId">Contract id associated with the condition.</param>
	public delegate void MyContractConditionDelegate(long conditionId, long contractId);
}
