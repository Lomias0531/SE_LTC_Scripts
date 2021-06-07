using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666)]
	public class MyUniformGravityProviderComponent : MySessionComponentBase, IMyGravityProvider
	{
		public readonly Vector3 Gravity = Vector3.Down * 9.81f;

		public bool IsWorking => true;

		public override void LoadData()
		{
			MyGravityProviderSystem.AddNaturalGravityProvider(this);
		}

		protected override void UnloadData()
		{
			MyGravityProviderSystem.RemoveNaturalGravityProvider(this);
		}

		public Vector3 GetWorldGravity(Vector3D worldPoint)
		{
			return Gravity;
		}

		public bool IsPositionInRange(Vector3D worldPoint)
		{
			return true;
		}

		public Vector3 GetWorldGravityGrid(Vector3D worldPoint)
		{
			return Gravity;
		}

		public bool IsPositionInRangeGrid(Vector3D worldPoint)
		{
			return true;
		}

		public float GetGravityMultiplier(Vector3D worldPoint)
		{
			return 1f;
		}

		public void GetProxyAABB(out BoundingBoxD aabb)
		{
			throw new NotSupportedException();
		}
	}
}
