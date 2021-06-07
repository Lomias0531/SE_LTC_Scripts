using System;

namespace VRage.Game
{
	[Flags]
	public enum MyEnvironmentTypes
	{
		None = 0x0,
		Space = 0x1,
		PlanetWithAtmosphere = 0x2,
		PlanetWithoutAtmosphere = 0x4,
		All = 0x7
	}
}
