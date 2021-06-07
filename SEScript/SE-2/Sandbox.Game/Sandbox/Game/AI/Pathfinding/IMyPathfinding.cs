using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyPathfinding
	{
		IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity relativeEntity);

		bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance);

		IMyPathfindingLog GetPathfindingLog();

		void Update();

		void UnloadData();

		void DebugDraw();
	}
}
