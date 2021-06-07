using System;

namespace VRage.ModAPI
{
	[Flags]
	public enum VoxelOperatorFlags
	{
		Read = 0x1,
		Write = 0x2,
		WriteAll = 0x6,
		None = 0x0,
		ReadWrite = 0x3,
		Default = 0x3
	}
}
