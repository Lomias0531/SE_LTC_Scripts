using VRageMath;

namespace VRageRender
{
	public interface IDrawTrianglesMessage
	{
		int VertexCount
		{
			get;
		}

		void AddIndex(int index);

		void AddVertex(Vector3D position);

		void AddTriangle(ref Vector3D v0, ref Vector3D v1, ref Vector3D v2);
	}
}
