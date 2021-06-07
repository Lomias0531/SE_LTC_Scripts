using Sandbox.Game.Screens.Helpers;
using VRage.Game;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
	public struct ToolbarItemCache
	{
		private MyToolbarItem m_cachedItem;

		private ToolbarItem m_item;

		public ToolbarItem Item
		{
			get
			{
				return m_item;
			}
			set
			{
				m_item = value;
				m_cachedItem = null;
			}
		}

		[NoSerialize]
		public MyToolbarItem CachedItem
		{
			get
			{
				if (m_cachedItem == null)
				{
					m_cachedItem = ToolbarItem.ToItem(Item);
				}
				return m_cachedItem;
			}
		}

		public MyObjectBuilder_ToolbarItem ToObjectBuilder()
		{
			return m_cachedItem?.GetObjectBuilder();
		}

		public void SetToToolbar(MyToolbar toolbar, int index)
		{
			MyToolbarItem cachedItem = m_cachedItem;
			if (cachedItem != null)
			{
				toolbar.SetItemAtIndex(index, cachedItem);
			}
		}

		public static implicit operator ToolbarItemCache(ToolbarItem item)
		{
			ToolbarItemCache result = default(ToolbarItemCache);
			result.m_item = item;
			return result;
		}
	}
}
