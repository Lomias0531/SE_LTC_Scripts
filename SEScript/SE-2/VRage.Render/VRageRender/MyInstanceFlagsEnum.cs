using System;

namespace VRageRender
{
	[Flags]
	public enum MyInstanceFlagsEnum : byte
	{
		CastShadows = 0x1,
		ShowLod1 = 0x2,
		EnableColorMask = 0x4
	}
}
