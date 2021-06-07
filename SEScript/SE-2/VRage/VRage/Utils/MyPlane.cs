using VRageMath;

namespace VRage.Utils
{
	public struct MyPlane
	{
		public Vector3 Point;

		public Vector3 Normal;

		public MyPlane(Vector3 point, Vector3 normal)
		{
			Point = point;
			Normal = normal;
		}

		public MyPlane(ref Vector3 point, ref Vector3 normal)
		{
			Point = point;
			Normal = normal;
		}

		public MyPlane(ref MyTriangle_Vertices triangle)
		{
			Point = triangle.Vertex0;
			Normal = MyUtils.Normalize(Vector3.Cross(triangle.Vertex1 - triangle.Vertex0, triangle.Vertex2 - triangle.Vertex0));
		}

		public float GetPlaneDistance()
		{
			return 0f - (Normal.X * Point.X + Normal.Y * Point.Y + Normal.Z * Point.Z);
		}
	}
}
