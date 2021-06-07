using System;

namespace VRage.Game
{
	/// <summary>
	/// Represents a set of maps.
	/// </summary>
	[Flags]
	public enum MyPlanetMapTypeSet
	{
		Material = 0x1,
		Biome = 0x2,
		Ore = 0x4
	}
}
