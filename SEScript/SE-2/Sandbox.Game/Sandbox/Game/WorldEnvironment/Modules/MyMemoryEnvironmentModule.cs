using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	public class MyMemoryEnvironmentModule : IMyEnvironmentModule
	{
		private MyLogicalEnvironmentSectorBase m_sector;

		private readonly HashSet<int> m_disabledItems = new HashSet<int>();

		public bool NeedToSave => m_disabledItems.Count > 0;

		public void ProcessItems(Dictionary<short, MyLodEnvironmentItemSet> items, int changedLodMin, int changedLodMax)
		{
			foreach (int disabledItem in m_disabledItems)
			{
				m_sector.InvalidateItem(disabledItem);
			}
		}

		public void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
		{
			if (ob != null)
			{
				m_disabledItems.UnionWith(((MyObjectBuilder_DummyEnvironmentModule)ob).DisabledItems);
			}
			m_sector = sector;
		}

		public void Close()
		{
		}

		public MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder()
		{
			if (m_disabledItems.Count > 0)
			{
				return new MyObjectBuilder_DummyEnvironmentModule
				{
					DisabledItems = m_disabledItems
				};
			}
			return null;
		}

		public void OnItemEnable(int itemId, bool enabled)
		{
			if (enabled)
			{
				m_disabledItems.Remove(itemId);
			}
			else
			{
				m_disabledItems.Add(itemId);
			}
			m_sector.InvalidateItem(itemId);
		}

		public void HandleSyncEvent(int logicalItem, object data, bool fromClient)
		{
		}

		public void DebugDraw()
		{
		}
	}
}
