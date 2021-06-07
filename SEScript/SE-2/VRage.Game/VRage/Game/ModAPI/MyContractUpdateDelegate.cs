using VRage.Library.Utils;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Delegate used for custom update event.
	/// </summary>
	/// <param name="contractId">Contract id.</param>
	/// <param name="newState">New state of the contract.</param>
	/// <param name="currentTime">Current time.</param>
	public delegate void MyContractUpdateDelegate(long contractId, MyCustomContractStateEnum newState, MyTimeSpan currentTime);
}
