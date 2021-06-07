namespace VRage.Voxels
{
	public static class MyVoxelEnumExtensions
	{
		public static bool Requests(this MyStorageDataTypeFlags self, MyStorageDataTypeEnum value)
		{
			return ((int)self & (1 << (int)value)) != 0;
		}

		public static MyStorageDataTypeFlags Without(this MyStorageDataTypeFlags self, MyStorageDataTypeEnum value)
		{
			return (MyStorageDataTypeFlags)((int)self & (int)(byte)(~(byte)(1 << (int)value)) & 3);
		}

		public static MyStorageDataTypeFlags ToFlags(this MyStorageDataTypeEnum self)
		{
			return (MyStorageDataTypeFlags)(1 << (int)self);
		}
	}
}
