using System;

namespace Sandbox.Game.Entities.Cube
{
	[Flags]
	public enum MySymmetrySettingModeEnum
	{
		Disabled = 0x0,
		NoPlane = 0x1,
		XPlane = 0x2,
		XPlaneOdd = 0x4,
		YPlane = 0x8,
		YPlaneOdd = 0x10,
		ZPlane = 0x20,
		ZPlaneOdd = 0x40
	}
}
