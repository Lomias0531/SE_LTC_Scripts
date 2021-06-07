using System;

namespace Sandbox.Game.Weapons
{
	[Flags]
	public enum MyTurretTargetFlags : ushort
	{
		Players = 0x1,
		SmallShips = 0x2,
		LargeShips = 0x4,
		Stations = 0x8,
		Asteroids = 0x10,
		Missiles = 0x20,
		Moving = 0x40,
		NotNeutrals = 0x80
	}
}
