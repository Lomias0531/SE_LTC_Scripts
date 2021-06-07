namespace VRage.Voxels.Mesh
{
	public static class VrMeshExtensions
	{
		public static bool IsEmpty(this VrVoxelMesh self)
		{
			if (self != null)
			{
				return self.TriangleCount == 0;
			}
			return true;
		}
	}
}
