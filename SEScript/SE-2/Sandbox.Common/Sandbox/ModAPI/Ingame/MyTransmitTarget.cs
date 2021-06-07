using System;

namespace Sandbox.ModAPI.Ingame
{
	[Flags]
	public enum MyTransmitTarget
	{
		None = 0x0,
		Owned = 0x1,
		Ally = 0x2,
		Neutral = 0x4,
		Enemy = 0x8,
		Everyone = 0xF,
		Default = 0x3
	}
}
