namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
	/// <summary>
	/// Voxel penetration definition
	/// </summary>
	public struct VoxelPlacementSettings
	{
		public VoxelPlacementMode PlacementMode;

		/// <summary>
		/// Maximum amount in % of block being inside voxel (where 1 - 100% to 0 - 0%)
		/// </summary>
		public float MaxAllowed;

		/// <summary>
		/// Minimum amount in % of block being inside voxel (where 1 - 100% to 0 - 0%)
		/// </summary>
		public float MinAllowed;
	}
}
