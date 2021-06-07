namespace Sandbox.Game.Entities.Blocks
{
	public enum MyContractResults
	{
		Success,
		Error_Unknown,
		Error_MissingKeyStructure,
		Error_InvalidData,
		Fail_CannotAccess,
		Fail_NotPossible,
		Fail_ActivationConditionsNotMet,
		Fail_ActivationConditionsNotMet_InsufficientFunds,
		Fail_ActivationConditionsNotMet_InsufficientSpace,
		Fail_FinishConditionsNotMet,
		Fail_FinishConditionsNotMet_MissingPackage,
		Fail_FinishConditionsNotMet_IncorrectTargetEntity,
		Fail_ContractNotFound_Activation,
		Fail_ContractNotFound_Abandon,
		Fail_ContractNotFound_Finish,
		Fail_FinishConditionsNotMet_NotEnoughItems,
		Fail_ActivationConditionsNotMet_ContractLimitReachedHard,
		Fail_ActivationConditionsNotMet_TargetOffline,
		Fail_FinishConditionsNotMet_NotEnoughSpace,
		Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt
	}
}
