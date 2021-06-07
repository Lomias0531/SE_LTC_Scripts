using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyEntityTracker
	{
		public BoundingSphereD BoundingVolume = new BoundingSphereD(Vector3D.PositiveInfinity, 0.0);

		public MyEntity Entity
		{
			get;
			private set;
		}

		public Vector3D CurrentPosition => Entity.PositionComp.WorldAABB.Center;

		public Vector3D LastPosition
		{
			get
			{
				return BoundingVolume.Center;
			}
			private set
			{
				BoundingVolume.Center = value;
			}
		}

		public double Radius
		{
			get
			{
				return BoundingVolume.Radius;
			}
			set
			{
				Tolerance = MathHelper.Clamp(value / 2.0, 128.0, 512.0);
				BoundingVolume.Radius = value + Tolerance;
			}
		}

		public double Tolerance
		{
			get;
			private set;
		}

		public MyEntityTracker(MyEntity entity, double radius)
		{
			Entity = entity;
			Radius = radius;
		}

		public bool ShouldGenerate()
		{
			if (!Entity.Closed && Entity.Save && (CurrentPosition - LastPosition).Length() > Tolerance)
			{
				if (Entity.Physics != null)
				{
					return !Entity.Physics.IsStatic;
				}
				return true;
			}
			return false;
		}

		public void UpdateLastPosition()
		{
			LastPosition = CurrentPosition;
		}

		public override string ToString()
		{
			return string.Concat(Entity, ", ", BoundingVolume, ", ", Tolerance);
		}
	}
}
