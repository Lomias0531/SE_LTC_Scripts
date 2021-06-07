using Sandbox.Definitions;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public struct ConnectivityResult
	{
		public Vector3I Position;

		public MyCubeBlock FatBlock;

		public MyBlockOrientation Orientation;

		public MyCubeBlockDefinition Definition;
	}
}
