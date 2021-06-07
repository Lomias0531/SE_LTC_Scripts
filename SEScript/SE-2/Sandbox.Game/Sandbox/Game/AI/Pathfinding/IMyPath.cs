using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyPath
	{
		IMyDestinationShape Destination
		{
			get;
		}

		IMyEntity EndEntity
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		bool PathCompleted
		{
			get;
		}

		void Invalidate();

		bool GetNextTarget(Vector3D position, out Vector3D target, out float targetRadius, out IMyEntity relativeEntity);

		void Reinit(Vector3D position);

		void DebugDraw();
	}
}
