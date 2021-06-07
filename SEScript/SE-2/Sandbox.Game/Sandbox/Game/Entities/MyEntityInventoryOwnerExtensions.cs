using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
	public static class MyEntityInventoryOwnerExtensions
	{
		[Obsolete("IMyInventoryOwner interface and MyInventoryOwnerTypeEnum enum is obsolete. Use type checking and inventory methods on MyEntity or MyInventory. Inventories will have this attribute as member.")]
		public static MyInventoryOwnerTypeEnum InventoryOwnerType(this MyEntity entity)
		{
			if (IsSameOrSubclass(typeof(MyUserControllableGun), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyProductionBlock), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyConveyorSorter), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.Storage;
			}
			if (IsSameOrSubclass(typeof(MyGasGenerator), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyShipToolBase), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyGasTank), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyReactor), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.Energy;
			}
			if (IsSameOrSubclass(typeof(MyCollector), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.Storage;
			}
			if (IsSameOrSubclass(typeof(MyCargoContainer), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.Storage;
			}
			if (IsSameOrSubclass(typeof(MyShipDrill), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.System;
			}
			if (IsSameOrSubclass(typeof(MyCharacter), entity.GetType()))
			{
				return MyInventoryOwnerTypeEnum.Character;
			}
			return MyInventoryOwnerTypeEnum.Storage;
		}

		private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
		{
			if (!potentialDescendant.IsSubclassOf(potentialBase))
			{
				return potentialDescendant == potentialBase;
			}
			return true;
		}
	}
}
