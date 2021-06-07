using VRageMath;

namespace VRage.Game.Models
{
	public struct MyIntersectionResultLineTriangle
	{
		public float Distance;

		public MyTriangle_Vertices InputTriangle;

		public MyTriangle_BoneIndicesWeigths? BoneWeights;

		public Vector3 InputTriangleNormal;

		public int TriangleIndex;

		public MyIntersectionResultLineTriangle(int triangleIndex, ref MyTriangle_Vertices triangle, ref Vector3 triangleNormal, float distance)
		{
			InputTriangle = triangle;
			InputTriangleNormal = triangleNormal;
			Distance = distance;
			BoneWeights = null;
			TriangleIndex = triangleIndex;
		}

		public MyIntersectionResultLineTriangle(int triangleIndex, ref MyTriangle_Vertices triangle, ref MyTriangle_BoneIndicesWeigths? boneWeigths, ref Vector3 triangleNormal, float distance)
		{
			InputTriangle = triangle;
			InputTriangleNormal = triangleNormal;
			Distance = distance;
			BoneWeights = boneWeigths;
			TriangleIndex = triangleIndex;
		}

		public static MyIntersectionResultLineTriangle? GetCloserIntersection(ref MyIntersectionResultLineTriangle? a, ref MyIntersectionResultLineTriangle? b)
		{
			if ((!a.HasValue && b.HasValue) || (a.HasValue && b.HasValue && b.Value.Distance < a.Value.Distance))
			{
				return b;
			}
			return a;
		}
	}
}
