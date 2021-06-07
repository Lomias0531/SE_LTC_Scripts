using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public abstract class MyEnvironmentDataView
	{
		public Vector2I Start;

		public Vector2I End;

		public int Lod;

		public MyList<ItemInfo> Items;

		public MyEnvironmentSector Listener;

		public List<int> SectorOffsets;

		public List<int> IntraSectorOffsets;

		public List<MyLogicalEnvironmentSectorBase> LogicalSectors;

		public abstract void Close();

		public void GetLogicalSector(int item, out int logicalItem, out MyLogicalEnvironmentSectorBase sector)
		{
			int index = SectorOffsets.BinaryIntervalSearch(item) - 1;
			logicalItem = item - SectorOffsets[index] + IntraSectorOffsets[index];
			sector = LogicalSectors[index];
		}
	}
}
