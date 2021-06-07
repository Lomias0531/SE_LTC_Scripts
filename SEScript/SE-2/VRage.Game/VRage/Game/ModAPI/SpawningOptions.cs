using System;

namespace VRage.Game.ModAPI
{
	[Flags]
	public enum SpawningOptions
	{
		None = 0x0,
		RotateFirstCockpitTowardsDirection = 0x2,
		SpawnRandomCargo = 0x4,
		DisableDampeners = 0x8,
		SetNeutralOwner = 0x10,
		TurnOffReactors = 0x20,
		DisableSave = 0x40,
		UseGridOrigin = 0x80,
		SetAuthorship = 0x100,
		ReplaceColor = 0x200,
		UseOnlyWorldMatrix = 0x400
	}
}
