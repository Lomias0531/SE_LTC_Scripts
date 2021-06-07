using System;

namespace VRage.Mod.Io.Data
{
	[Flags]
	internal enum ModContentWarnings
	{
		None = 0x0,
		Alcohol = 0x1,
		Drugs = 0x2,
		Violence = 0x4,
		Explicit = 0x8
	}
}
