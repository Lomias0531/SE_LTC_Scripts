using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public static class MyEnvironmentSectorExtensions
	{
		public static bool HasWorkPending(this MyEnvironmentSector self)
		{
			if (!self.HasSerialWorkPending && !self.HasParallelWorkPending)
			{
				return self.HasParallelWorkInProgress;
			}
			return true;
		}

		public static void DisableItemsInBox(this MyEnvironmentSector sector, ref BoundingBoxD box)
		{
			if (sector.DataView != null)
			{
				for (int i = 0; i < sector.DataView.LogicalSectors.Count; i++)
				{
					sector.DataView.LogicalSectors[i].DisableItemsInBox(sector.SectorCenter, ref box);
				}
			}
		}

		public static void GetItemsInAabb(this MyEnvironmentSector sector, ref BoundingBoxD aabb, List<int> itemsInBox)
		{
			if (sector.DataView != null)
			{
				aabb.Translate(-sector.SectorCenter);
				for (int i = 0; i < sector.DataView.LogicalSectors.Count; i++)
				{
					sector.DataView.LogicalSectors[i].GetItemsInAabb(ref aabb, itemsInBox);
				}
			}
		}
	}
}
