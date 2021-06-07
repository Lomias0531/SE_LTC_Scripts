using VRageMath;

namespace Sandbox.Game.Entities
{
	public interface IMyGizmoDrawableObject
	{
		Color GetGizmoColor();

		bool CanBeDrawn();

		BoundingBox? GetBoundingBox();

		float GetRadius();

		MatrixD GetWorldMatrix();

		Vector3 GetPositionInGrid();

		bool EnableLongDrawDistance();
	}
}
