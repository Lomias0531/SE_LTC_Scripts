using System;

namespace VRage.Game.ModAPI
{
	public interface IMyContractSystem
	{
		/// <summary>
		/// The operation of finishing Condition itself
		/// Returns whether it succeeded or not
		/// </summary>                       
		Func<long, long, bool> CustomFinishCondition
		{
			get;
			set;
		}

		Func<long, long, MyActivationCustomResults> CustomCanActivateContract
		{
			get;
			set;
		}

		Func<long, bool> CustomNeedsUpdate
		{
			get;
			set;
		}

		/// <summary>
		/// Called after Condition has been successfully finished.
		/// </summary>
		event MyContractConditionDelegate CustomConditionFinished;

		/// <summary>
		/// Called when contract is activated.
		/// </summary>
		event MyContractActivateDelegate CustomActivateContract;

		/// <summary>
		/// Called when contract has failed. Has additional information associated with the contract.
		/// </summary>
		event MyContractFailedDelegate CustomFailFor;

		/// <summary>
		/// Called when contract was finished. Has additional information associated with the contract.
		/// </summary>
		event MyContractFinishedDelegate CustomFinishFor;

		/// <summary>
		/// Called when contract was finished.
		/// </summary>
		event MyContractChangeDelegate CustomFinish;

		/// <summary>
		/// Called when contract failed.
		/// </summary>
		event MyContractChangeDelegate CustomFail;

		/// <summary>
		/// Called when contract ended due to any reason. Use to clean up/remove anything.
		/// </summary>
		event MyContractChangeDelegate CustomCleanUp;

		/// <summary>
		/// Called immidietly just before contract faield.
		/// </summary>
		event MyContractChangeDelegate CustomTimeRanOut;

		/// <summary>
		/// Called every frame if marked for it by CustomNeedsUpdate.
		/// </summary>
		event MyContractUpdateDelegate CustomUpdate;

		/// <summary>
		/// Adds the contract
		/// </summary>
		/// <param name="contract">contract id</param>
		/// <returns>information about new contract</returns>
		MyAddContractResultWrapper AddContract(IMyContract contract);

		/// <summary>
		/// Removes the contract
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>True if contract was removed</returns>
		bool RemoveContract(long contractId);

		/// <summary>
		/// Checks if specified contract is in active state
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>True if contract is active</returns>
		bool IsContractActive(long contractId);

		/// <summary>
		/// Gets Contract State
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>state of the contract</returns>
		MyCustomContractStateEnum GetContractState(long contractId);

		/// <summary>
		/// Tries to set the contract to finish state
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>True if contract was finished</returns>
		bool TryFinishCustomContract(long contractId);

		/// <summary>
		/// Tries to set the contract to fail state
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>True if contract was failed</returns>
		bool TryFailCustomContract(long contractId);

		/// <summary>
		/// Tries to abandon the contract
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <param name="playerId">player id who has the contract</param>
		/// <returns>True if contract was abandoned</returns>
		bool TryAbandonCustomContract(long contractId, long playerId);

		/// <summary>
		/// Gets Contracts Definition Id
		/// </summary>
		/// <param name="contractId">contract id</param>
		/// <returns>Definition Id of the contract</returns>
		MyDefinitionId? GetContractDefinitionId(long contractId);
	}
}
