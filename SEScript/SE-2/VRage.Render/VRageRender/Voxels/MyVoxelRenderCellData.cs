using System;
using VRage.Library;
using VRageMath;

namespace VRageRender.Voxels
{
	public struct MyVoxelRenderCellData : IDisposable
	{
		/// <summary>
		/// Complete list of vertices.
		/// </summary>
		public NativeArray Vertices;

		/// <summary>
		/// Complete list of normals.
		/// </summary>
		public NativeArray Normals;

		/// <summary>
		/// Complete list of mesh indices.
		/// </summary>
		public NativeArray Indices;

		public bool ShortIndices;

		/// <summary>
		/// List of mesh parts according to material separation.
		/// </summary>
		public MyVoxelMeshPartIndex[] Parts;

		/// <summary>
		/// Bounding box of the whole mesh.
		/// </summary>
		public BoundingBox CellBounds;

		/// <summary>
		/// Number of vertices in the cell data.
		/// </summary>
		public int VertexCount;

		/// <summary>
		/// Number of indices in the cell data.
		/// </summary>
		public int IndexCount;

		/// <inheritdoc />
		public void Dispose()
		{
			if (Vertices != null)
			{
				Vertices.Dispose();
			}
			if (Normals != null)
			{
				Normals.Dispose();
			}
			if (Indices != null)
			{
				Indices.Dispose();
			}
		}
	}
}
