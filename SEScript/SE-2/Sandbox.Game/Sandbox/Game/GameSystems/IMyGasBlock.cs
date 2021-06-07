using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.GameSystems
{
	public interface IMyGasBlock : IMyConveyorEndpointBlock
	{
		bool CanPressurizeRoom
		{
			get;
		}

		bool IsWorking();
	}
}
