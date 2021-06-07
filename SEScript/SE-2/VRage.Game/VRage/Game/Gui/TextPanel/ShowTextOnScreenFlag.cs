using System;

namespace VRage.Game.GUI.TextPanel
{
	[Flags]
	public enum ShowTextOnScreenFlag : byte
	{
		NONE = 0x0,
		PUBLIC = 0x2,
		PRIVATE = 0x4
	}
}
