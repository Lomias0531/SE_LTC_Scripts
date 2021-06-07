using Sandbox.Game.Gui;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MystatControlledEntityHydrogenEstimatedTimeRemaining : MyStatBase
	{
		private MyStatBase m_usageStat;

		public MystatControlledEntityHydrogenEstimatedTimeRemaining()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_estimated_time_remaining_hydrogen");
		}

		public override void Update()
		{
			if (m_usageStat == null)
			{
				m_usageStat = MyHud.Stats.GetStat<MyStatControlledEntityHydrogenCapacity>();
			}
			else
			{
				base.CurrentValue = m_usageStat.CurrentValue / m_usageStat.MaxValue;
			}
		}
	}
}
