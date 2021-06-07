using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerInventoryCapacityChanged : MyStatBase
	{
		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private static readonly double VISIBLE_TIME_MS = 3000.0;

		private int m_lastVolume;

		private double m_timeToggled;

		public MyStatPlayerInventoryCapacityChanged()
		{
			base.Id = MyStringHash.GetOrCompute("player_inventory_capacity_changed");
		}

		public override void Update()
		{
			double totalMilliseconds = TIMER.ElapsedTimeSpan.TotalMilliseconds;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				int num = MyFixedPoint.MultiplySafe(localCharacter.GetInventory().CurrentVolume, 1000).ToIntSafe();
				if (m_lastVolume != num)
				{
					base.CurrentValue = 1f;
					m_timeToggled = totalMilliseconds;
					m_lastVolume = num;
				}
			}
			if (base.CurrentValue == 1f && totalMilliseconds - m_timeToggled > VISIBLE_TIME_MS)
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
