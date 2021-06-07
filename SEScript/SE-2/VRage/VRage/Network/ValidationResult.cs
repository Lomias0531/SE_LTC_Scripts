using System;

namespace VRage.Network
{
	[Flags]
	public enum ValidationResult
	{
		Passed = 0x0,
		Kick = 0x1,
		Access = 0x2,
		Controlled = 0x4,
		Ownership = 0x8,
		BigOwner = 0x10,
		BigOwnerSpaceMaster = 0x20
	}
}
