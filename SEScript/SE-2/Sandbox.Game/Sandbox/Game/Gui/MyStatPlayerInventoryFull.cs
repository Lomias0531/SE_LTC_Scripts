using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerInventoryFull : MyStatBase
	{
		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private static readonly double VISIBLE_TIME_MS = 5000.0;

		private double m_visibleFromMs;

		private bool m_inventoryFull;

		public bool InventoryFull
		{
			get
			{
				return m_inventoryFull;
			}
			set
			{
				if (value)
				{
					m_visibleFromMs = TIMER.ElapsedTimeSpan.TotalMilliseconds;
				}
				m_inventoryFull = value;
				base.CurrentValue = (value ? 1 : 0);
			}
		}

		public MyStatPlayerInventoryFull()
		{
			base.Id = MyStringHash.GetOrCompute("player_inventory_full");
		}

		public override void Update()
		{
			if (m_inventoryFull && TIMER.ElapsedTimeSpan.TotalMilliseconds - m_visibleFromMs > VISIBLE_TIME_MS)
			{
				m_inventoryFull = false;
				base.CurrentValue = 0f;
			}
		}
	}
}
