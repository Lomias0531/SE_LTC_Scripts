using Sandbox.Game.Entities;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyPathfindingLog
	{
		void LogStorageWrite(MyVoxelBase map, MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax);
	}
}
