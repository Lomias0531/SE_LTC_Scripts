using VRage.Game;

namespace Sandbox.Game.Entities.Cube
{
	public interface IMyBlockAdditionalModelGenerator
	{
		bool Initialize(MyCubeGrid grid, MyCubeSize gridSizeEnum);

		void Close();

		void EnableGenerator(bool enable);

		void BlockAddedToMergedGrid(MySlimBlock block);

		void GenerateBlocks(MySlimBlock generatingBlock);

		void UpdateAfterSimulation();

		void UpdateBeforeSimulation();

		void UpdateAfterGridSpawn(MySlimBlock block);

		MySlimBlock GetGeneratingBlock(MySlimBlock generatedBlock);
	}
}
