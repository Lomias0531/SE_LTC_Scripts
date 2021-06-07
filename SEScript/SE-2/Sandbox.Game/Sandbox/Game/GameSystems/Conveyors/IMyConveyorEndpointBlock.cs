namespace Sandbox.Game.GameSystems.Conveyors
{
	public interface IMyConveyorEndpointBlock
	{
		IMyConveyorEndpoint ConveyorEndpoint
		{
			get;
		}

		void InitializeConveyorEndpoint();

		bool AllowSelfPulling();

		PullInformation GetPullInformation();

		PullInformation GetPushInformation();
	}
}
