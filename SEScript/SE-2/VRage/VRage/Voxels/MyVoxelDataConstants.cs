namespace VRage.Voxels
{
	public static class MyVoxelDataConstants
	{
		public const string StorageV2Extension = ".vx2";

		public const byte IsoLevel = 127;

		public const byte ContentEmpty = 0;

		public const byte ContentFull = byte.MaxValue;

		public const float HalfContent = 127.5f;

		public const float HalfContentReciprocal = 2f / 255f;

		public const float ContentReciprocal = 0.003921569f;

		/// <summary>
		/// The byte describing the absence of material.
		///
		/// This byte should never accur in areas where content is non-zero.
		/// </summary>
		public const byte NullMaterial = byte.MaxValue;

		/// <summary>
		/// Array of default values for voxel data, this array can be indexed by the integer value of MyStorageDataTypeEnum.
		/// </summary>
		private static readonly byte[] Defaults = new byte[2]
		{
			0,
			255
		};

		/// <summary>
		/// There are 16 lods, 0 through 15.
		/// </summary>
		public const int LodCount = 16;

		/// <summary>
		/// Lookup the default value for a given storage data type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static byte DefaultValue(MyStorageDataTypeEnum type)
		{
			return Defaults[(int)type];
		}
	}
}
