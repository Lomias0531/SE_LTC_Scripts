using System;

namespace VRage.Game.Gui
{
	[Flags]
	public enum MyHudIndicatorFlagsEnum
	{
		NONE = 0x0,
		SHOW_TEXT = 0x1,
		SHOW_BORDER_INDICATORS = 0x2,
		SHOW_DISTANCE = 0x10,
		ALPHA_CORRECTION_BY_DISTANCE = 0x20,
		SHOW_ICON = 0x400,
		SHOW_FOCUS_MARK = 0x800,
		SHOW_ALL = -1
	}
}
