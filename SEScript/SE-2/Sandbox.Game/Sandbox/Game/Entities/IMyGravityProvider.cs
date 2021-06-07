using VRageMath;

namespace Sandbox.Game.Entities
{
	public interface IMyGravityProvider
	{
		bool IsWorking
		{
			get;
		}

		Vector3 GetWorldGravity(Vector3D worldPoint);

		bool IsPositionInRange(Vector3D worldPoint);

		float GetGravityMultiplier(Vector3D worldPoint);

		void GetProxyAABB(out BoundingBoxD aabb);
	}
}
