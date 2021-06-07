using System;

namespace VRage.Game.GUI.TextPanel
{
	[Flags]
	public enum TextPanelAccessFlag : byte
	{
		NONE = 0x0,
		READ_FACTION = 0x2,
		WRITE_FACTION = 0x4,
		READ_AND_WRITE_FACTION = 0x6,
		READ_ENEMY = 0x8,
		WRITE_ENEMY = 0x10,
		READ_ALL = 0xA,
		WRITE_ALL = 0x14,
		READ_AND_WRITE_ALL = 0x1E
	}
}
