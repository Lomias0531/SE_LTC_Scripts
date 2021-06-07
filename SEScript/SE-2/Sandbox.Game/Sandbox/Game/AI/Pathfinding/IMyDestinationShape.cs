using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public interface IMyDestinationShape
	{
		void SetRelativeTransform(MatrixD invWorldTransform);

		void UpdateWorldTransform(MatrixD worldTransform);

		float PointAdmissibility(Vector3D position, float tolerance);

		Vector3D GetClosestPoint(Vector3D queryPoint);

		Vector3D GetBestPoint(Vector3D queryPoint);

		Vector3D GetDestination();

		void DebugDraw();
	}
}
