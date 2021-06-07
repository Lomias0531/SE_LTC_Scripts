using System;

namespace VRage.Network
{
	[Flags]
	public enum ValidationType
	{
		None = 0x0,
		Access = 0x1,
		Controlled = 0x2,
		Ownership = 0x4,
		BigOwner = 0x8,
		BigOwnerSpaceMaster = 0x10,
		IgnoreDLC = 0x20
	}
}
