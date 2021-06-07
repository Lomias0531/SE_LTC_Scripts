using VRageMath;

namespace Sandbox.Definitions
{
	public class MyEdgeOrientationInfo
	{
		public readonly Matrix Orientation;

		public readonly MyCubeEdgeType EdgeType;

		public MyEdgeOrientationInfo(Matrix localMatrix, MyCubeEdgeType edgeType)
		{
			Orientation = localMatrix;
			EdgeType = edgeType;
		}
	}
}
