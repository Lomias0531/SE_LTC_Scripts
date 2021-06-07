using VRageMath;

namespace VRage
{
	public struct MyTriangle_Vertices
	{
		public Vector3 Vertex0;

		public Vector3 Vertex1;

		public Vector3 Vertex2;

		public void Transform(ref Matrix transform)
		{
			Vertex0 = Vector3.Transform(Vertex0, ref transform);
			Vertex1 = Vector3.Transform(Vertex1, ref transform);
			Vertex2 = Vector3.Transform(Vertex2, ref transform);
		}
	}
}
