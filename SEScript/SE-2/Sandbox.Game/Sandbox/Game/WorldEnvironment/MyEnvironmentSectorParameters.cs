using Sandbox.Game.WorldEnvironment.Definitions;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public struct MyEnvironmentSectorParameters
	{
		public long EntityId;

		public BoundingBox2I DataRange;

		public Vector3 SurfaceBasisX;

		public Vector3 SurfaceBasisY;

		public Vector3D Center;

		public Vector3D[] Bounds;

		public MyWorldEnvironmentDefinition Environment;

		public IMyEnvironmentDataProvider Provider;

		public long SectorId;
	}
}
