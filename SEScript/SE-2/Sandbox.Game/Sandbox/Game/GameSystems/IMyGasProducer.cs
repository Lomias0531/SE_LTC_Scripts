using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.GameSystems
{
	internal interface IMyGasProducer : IMyGasBlock, IMyConveyorEndpointBlock
	{
		float ProductionCapacity(float deltaTime);

		void Produce(float amount);

		int GetPriority();
	}
}
