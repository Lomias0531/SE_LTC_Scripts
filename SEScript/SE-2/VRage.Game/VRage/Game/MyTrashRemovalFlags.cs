using System;

namespace VRage.Game
{
	[Flags]
	public enum MyTrashRemovalFlags
	{
		None = 0x0,
		Default = 0x1E1A,
		Fixed = 0x1,
		Stationary = 0x2,
		Linear = 0x8,
		Accelerating = 0x10,
		Powered = 0x20,
		Controlled = 0x40,
		WithProduction = 0x80,
		WithMedBay = 0x100,
		WithBlockCount = 0x200,
		DistanceFromPlayer = 0x400,
		RevertMaterials = 0x800,
		RevertAsteroids = 0x1000,
		RevertWithFloatingsPresent = 0x2000,
		Indestructible = 0x4000
	}
}
