using VRageMath;

namespace VRageRender.Import
{
	public class MyModelInfo
	{
		public int TrianglesCount;

		public int VerticesCount;

		public Vector3 BoundingBoxSize;

		public MyModelInfo(int triCnt, int VertCnt, Vector3 BBsize)
		{
			TrianglesCount = triCnt;
			VerticesCount = VertCnt;
			BoundingBoxSize = BBsize;
		}
	}
}
