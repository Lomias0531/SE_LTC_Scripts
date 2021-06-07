namespace Sandbox.Game.World.Generator
{
	public enum MyContractCreationResults
	{
		Success,
		Fail_Common,
		Fail_Impossible,
		Fail_NoAccess,
		Fail_GridNotFound,
		Fail_BlockNotFound,
		Error,
		Error_MissingKeyStructure,
		Fail_NotAnOwnerOfBlock,
		Fail_NotAnOwnerOfGrid,
		Fail_NotEnoughFunds,
		Fail_CreationLimitHard
	}
}
