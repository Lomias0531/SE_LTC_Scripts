using System;

namespace VRageRender.Messages
{
	[Flags]
	public enum GPUEmitterFlags : uint
	{
		Streaks = 0x1,
		Collide = 0x2,
		SleepState = 0x4,
		Dead = 0x8,
		Light = 0x10,
		VolumetricLight = 0x20,
		FreezeSimulate = 0x80,
		FreezeEmit = 0x100,
		RandomRotationEnabled = 0x200,
		LocalRotation = 0x400,
		LocalAndCameraRotation = 0x800,
		UseEmissivityChannel = 0x1000,
		UseAlphaAnisotropy = 0x2000
	}
}
