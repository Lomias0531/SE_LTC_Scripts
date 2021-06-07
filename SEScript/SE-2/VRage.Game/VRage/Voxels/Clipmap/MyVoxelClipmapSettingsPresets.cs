namespace VRage.Voxels.Clipmap
{
	public class MyVoxelClipmapSettingsPresets
	{
		/// <summary>
		/// Settings for normal stones.
		/// </summary>
		public static MyVoxelClipmapSettings[] NormalSettings = new MyVoxelClipmapSettings[4]
		{
			MyVoxelClipmapSettings.Create(4, 3, 2f, 4, 16384),
			MyVoxelClipmapSettings.Create(5, 3, 2f, 4, 16384),
			MyVoxelClipmapSettings.Create(5, 3, 3f),
			MyVoxelClipmapSettings.Create(5, 4000, 9f)
		};

		/// <summary>
		/// Settings for planets
		/// </summary>
		public static MyVoxelClipmapSettings[] PlanetSettings = new MyVoxelClipmapSettings[4]
		{
			MyVoxelClipmapSettings.Create(4, 2, 2f),
			MyVoxelClipmapSettings.Create(5, 2, 2f),
			MyVoxelClipmapSettings.Create(5, 3, 2f),
			MyVoxelClipmapSettings.Create(5, 3, 3f)
		};
	}
}
