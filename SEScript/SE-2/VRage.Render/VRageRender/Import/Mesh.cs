using VRageMath;

namespace VRageRender.Import
{
	public class Mesh
	{
		public Matrix AbsoluteMatrix = Matrix.Identity;

		public int MeshIndex;

		/// <summary>
		/// Offset on the vertex buffer
		/// </summary>
		public int VertexOffset = -1;

		public int VertexCount = -1;

		/// <summary>
		/// Offset on the indices buffer
		/// </summary>
		public int StartIndex = -1;

		public int IndexCount = -1;
	}
}
