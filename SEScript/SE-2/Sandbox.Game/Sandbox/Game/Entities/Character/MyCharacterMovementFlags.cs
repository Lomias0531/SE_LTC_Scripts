using System;

namespace Sandbox.Game.Entities.Character
{
	[Flags]
	public enum MyCharacterMovementFlags : byte
	{
		Jump = 0x1,
		Sprint = 0x2,
		FlyUp = 0x4,
		FlyDown = 0x8,
		Crouch = 0x10,
		Walk = 0x20
	}
}
