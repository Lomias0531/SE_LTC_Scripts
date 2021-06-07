using System;

namespace VRage.Input
{
	[Flags]
	public enum RequestedJoystickAxis : byte
	{
		X = 0x1,
		Y = 0x2,
		Z = 0x4,
		NoZ = 0x3,
		All = 0x7
	}
}
