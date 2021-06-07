using System;

namespace Sandbox.Game.World
{
	[Flags]
	public enum AdminSettingsEnum
	{
		None = 0x0,
		Invulnerable = 0x1,
		ShowPlayers = 0x2,
		UseTerminals = 0x4,
		Untargetable = 0x8,
		KeepOriginalOwnershipOnPaste = 0x10,
		IgnoreSafeZones = 0x20,
		IgnorePcu = 0x40,
		AdminOnly = 0x6D
	}
}
