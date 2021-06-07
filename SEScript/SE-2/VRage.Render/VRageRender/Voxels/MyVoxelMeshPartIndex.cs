namespace VRageRender.Voxels
{
	/// <summary>
	/// Describes the start and cout of indices of a mesh part for the present materials.
	/// </summary>
	public struct MyVoxelMeshPartIndex
	{
		/// <summary>
		/// Material triple for this part.
		/// </summary>
		public MyVoxelMaterialTriple Materials;

		/// <summary>
		/// First index in this part.
		/// </summary>
		public int StartIndex;

		/// <summary>
		/// Number of indices contained in this part.
		/// </summary>
		public int IndexCount;

		public override string ToString()
		{
			return $"{Materials} {StartIndex}:{IndexCount}";
		}
	}
}
