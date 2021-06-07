using Sandbox.Definitions;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal struct OverlapResult
	{
		public Vector3I Position;

		public MyCubeBlock FatBlock;

		public MyBlockOrientation Orientation;

		public MyCubeBlockDefinition Definition;
	}
}
