using System;

namespace VRage.Game.Entity.UseObject
{
	[Flags]
	public enum UseActionEnum
	{
		None = 0x0,
		Manipulate = 0x1,
		OpenTerminal = 0x2,
		OpenInventory = 0x4,
		UseFinished = 0x8,
		Close = 0x10,
		PickUp = 0x20,
		BuildPlanner = 0x40
	}
}
