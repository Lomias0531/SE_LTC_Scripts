namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Delegate used for contract activation.
	/// </summary>
	/// <param name="contractId">Activated contract id.</param>
	/// <param name="identityId">Identity id associated with activated contract.</param>
	public delegate void MyContractActivateDelegate(long contractId, long identityId);
}
