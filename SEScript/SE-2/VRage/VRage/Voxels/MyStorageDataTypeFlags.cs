using System;

namespace VRage.Voxels
{
	[Flags]
	public enum MyStorageDataTypeFlags : byte
	{
		None = 0x0,
		Content = 0x1,
		Material = 0x2,
		ContentAndMaterial = 0x3,
		All = 0x3
	}
}
