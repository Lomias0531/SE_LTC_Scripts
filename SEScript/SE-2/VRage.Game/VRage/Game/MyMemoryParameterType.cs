using System;

namespace VRage.Game
{
	[Flags]
	public enum MyMemoryParameterType : byte
	{
		IN = 0x1,
		OUT = 0x2,
		IN_OUT = 0x3,
		PARAMETER = 0x4
	}
}
