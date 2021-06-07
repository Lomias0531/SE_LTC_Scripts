using System;

namespace VRageRender.Messages
{
	[Flags]
	public enum MyTextureType
	{
		Unspecified = 0x0,
		ColorMetal = 0x1,
		NormalGloss = 0x2,
		Extensions = 0x4,
		Alphamask = 0x8
	}
}
