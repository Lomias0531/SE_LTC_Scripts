using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.GameSystems
{
	public interface IMyGasConsumer : IMyGasBlock, IMyConveyorEndpointBlock
	{
		float ConsumptionNeed(float deltaTime);

		void Consume(float amount);

		int GetPriority();
	}
}
