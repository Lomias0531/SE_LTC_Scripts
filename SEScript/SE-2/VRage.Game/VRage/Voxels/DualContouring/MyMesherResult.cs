namespace VRage.Voxels.DualContouring
{
	/// <summary>
	/// Struct describing the result of contouring a range of voxels.
	///
	/// The range can either be totaly empty (all isos &lt; 0), totally full (all isos &gt; 0), or mixed.
	/// Only the later will
	/// </summary>
	public struct MyMesherResult
	{
		/// <summary>
		/// The constitution of the content for the range of voxels that was contoured.
		///
		/// If this is not mixed then the mesh should be null.
		/// </summary>
		public readonly MyVoxelContentConstitution Constitution;

		/// <summary>
		/// The generated mesh if any.
		/// </summary>
		public readonly VrVoxelMesh Mesh;

		public static MyMesherResult Empty = new MyMesherResult(MyVoxelContentConstitution.Empty);

		/// <summary>
		/// Whether this result contains a mesh.
		/// </summary>
		public bool MeshProduced => Mesh != null;

		internal MyMesherResult(VrVoxelMesh mesh)
		{
			Constitution = MyVoxelContentConstitution.Mixed;
			Mesh = mesh;
		}

		internal MyMesherResult(MyVoxelContentConstitution constitution)
		{
			Constitution = constitution;
			Mesh = null;
		}
	}
}
