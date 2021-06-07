using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerOxygenBottles : MyStatBase
	{
		private static readonly MyDefinitionId OXYGEN_BOTTLE_ID = MyDefinitionId.Parse("MyObjectBuilder_OxygenContainerObject/OxygenBottle");

		private static readonly double CHECK_INTERVAL_MS = 1000.0;

		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private double m_lastCheck;

		public MyStatPlayerOxygenBottles()
		{
			base.Id = MyStringHash.GetOrCompute("player_oxygen_bottles");
			m_lastCheck = 0.0;
		}

		public override void Update()
		{
			if (TIMER.ElapsedTimeSpan.TotalMilliseconds - CHECK_INTERVAL_MS < m_lastCheck)
			{
				return;
			}
			m_lastCheck = TIMER.ElapsedTimeSpan.TotalMilliseconds;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				base.CurrentValue = 0f;
				return;
			}
			MyInventory inventory = localCharacter.GetInventory();
			if (inventory == null)
			{
				base.CurrentValue = 0f;
				return;
			}
			base.CurrentValue = 0f;
			foreach (MyPhysicalInventoryItem item in inventory.GetItems())
			{
				if (item.Content.GetId() == OXYGEN_BOTTLE_ID && ((MyObjectBuilder_OxygenContainerObject)item.Content).GasLevel > 1E-06f)
				{
					base.CurrentValue++;
				}
			}
		}
	}
}
