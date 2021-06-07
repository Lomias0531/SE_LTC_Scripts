using System;

namespace Sandbox.Game.Screens.Helpers
{
	[Flags]
	public enum MySupportKeysEnum : byte
	{
		NONE = 0x0,
		CTRL = 0x1,
		ALT = 0x2,
		SHIFT = 0x4,
		CTRL_ALT = 0x3,
		CTRL_SHIFT = 0x5,
		ALT_SHIFT = 0x6,
		CTRL_ALT_SHIFT = 0x7
	}
}
