using System;

namespace VRage.Game.ObjectBuilders.Components
{
	[Flags]
	public enum MySafeZoneAction
	{
		Damage = 0x1,
		Shooting = 0x2,
		Drilling = 0x4,
		Welding = 0x8,
		Grinding = 0x10,
		VoxelHand = 0x20,
		Building = 0x40,
		LandingGearLock = 0x80,
		ConvertToStation = 0x100,
		All = 0x1FF,
		AdminIgnore = 0x17E
	}
}
