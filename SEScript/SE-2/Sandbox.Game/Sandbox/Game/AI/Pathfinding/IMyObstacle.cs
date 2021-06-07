using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyObstacle
	{
		bool Contains(ref Vector3D point);

		void Update();

		void DebugDraw();
	}
}
