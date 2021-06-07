namespace VRage.Voxels
{
	public static class MyVoxelrequestFlagsExtensions
	{
		public static bool HasFlags(this MyVoxelRequestFlags self, MyVoxelRequestFlags other)
		{
			return (self & other) == other;
		}
	}
}
