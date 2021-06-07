namespace Sandbox.Game.Contracts
{
	public enum MyActivationResults
	{
		Success,
		Fail_General,
		Fail_InsufficientFunds,
		Fail_InsufficientInventorySpace,
		Fail_ContractLimitReachedHard,
		Fail_TargetNotOnline,
		Fail,
		Error,
		Fail_YouAreTargetOfThisHunt
	}
}
