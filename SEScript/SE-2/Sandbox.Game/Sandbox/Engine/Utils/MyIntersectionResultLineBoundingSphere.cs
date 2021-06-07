using VRage.Game.Entity;

namespace Sandbox.Engine.Utils
{
	internal struct MyIntersectionResultLineBoundingSphere
	{
		public readonly float Distance;

		public readonly MyEntity PhysObject;

		public MyIntersectionResultLineBoundingSphere(float distance, MyEntity physObject)
		{
			Distance = distance;
			PhysObject = physObject;
		}

		public static MyIntersectionResultLineBoundingSphere? GetCloserIntersection(ref MyIntersectionResultLineBoundingSphere? a, ref MyIntersectionResultLineBoundingSphere? b)
		{
			if ((!a.HasValue && b.HasValue) || (a.HasValue && b.HasValue && b.Value.Distance < a.Value.Distance))
			{
				return b;
			}
			return a;
		}
	}
}
