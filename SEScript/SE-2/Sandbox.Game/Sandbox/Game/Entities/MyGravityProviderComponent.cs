using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public abstract class MyGravityProviderComponent : MyEntityComponentBase, IMyGravityProvider
	{
		public override string ComponentTypeDebugString => GetType().Name;

		public abstract bool IsWorking
		{
			get;
		}

		public abstract Vector3 GetWorldGravity(Vector3D worldPoint);

		public abstract bool IsPositionInRange(Vector3D worldPoint);

		public abstract float GetGravityMultiplier(Vector3D worldPoint);

		public abstract void GetProxyAABB(out BoundingBoxD aabb);
	}
}
