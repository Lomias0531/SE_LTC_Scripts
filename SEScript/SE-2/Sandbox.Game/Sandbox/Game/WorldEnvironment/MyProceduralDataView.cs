using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyProceduralDataView : MyEnvironmentDataView
	{
		private readonly MyProceduralEnvironmentProvider m_provider;

		public MyProceduralDataView(MyProceduralEnvironmentProvider provider, int lod, ref Vector2I start, ref Vector2I end)
		{
			m_provider = provider;
			Start = start;
			End = end;
			Lod = lod;
			int capacity = (end - start + 1).Size();
			SectorOffsets = new List<int>(capacity);
			LogicalSectors = new List<MyLogicalEnvironmentSectorBase>(capacity);
			IntraSectorOffsets = new List<int>(capacity);
			Items = new MyList<ItemInfo>();
		}

		public override void Close()
		{
			m_provider.CloseView(this);
		}

		public int GetSectorIndex(int x, int y)
		{
			return x - Start.X + (y - Start.Y) * (End.X - Start.X + 1);
		}

		public void AddSector(MyProceduralLogicalSector sector)
		{
			SectorOffsets.Add(Items.Count);
			LogicalSectors.Add(sector);
			IntraSectorOffsets.Add(0);
		}
	}
}
