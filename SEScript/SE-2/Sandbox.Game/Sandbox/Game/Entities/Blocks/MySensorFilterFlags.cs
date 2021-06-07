using System;

namespace Sandbox.Game.Entities.Blocks
{
	[Flags]
	public enum MySensorFilterFlags : ushort
	{
		Players = 0x1,
		FloatingObjects = 0x2,
		SmallShips = 0x4,
		LargeShips = 0x8,
		Stations = 0x10,
		Asteroids = 0x20,
		Subgrids = 0x40,
		Owner = 0x100,
		Friendly = 0x200,
		Neutral = 0x400,
		Enemy = 0x800
	}
}
