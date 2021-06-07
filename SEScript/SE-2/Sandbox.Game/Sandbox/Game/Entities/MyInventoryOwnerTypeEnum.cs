using System;

namespace Sandbox.Game.Entities
{
	[Obsolete("IMyInventoryOwner interface and MyInventoryOwnerTypeEnum enum is obsolete. Use type checking and inventory methods on MyEntity.")]
	public enum MyInventoryOwnerTypeEnum
	{
		Character,
		Storage,
		Energy,
		System,
		Conveyor
	}
}
