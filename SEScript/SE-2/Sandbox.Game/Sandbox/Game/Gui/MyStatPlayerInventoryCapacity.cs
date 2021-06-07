using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerInventoryCapacity : MyStatBase
	{
		private float m_max;

		public override float MaxValue => m_max;

		public MyStatPlayerInventoryCapacity()
		{
			base.Id = MyStringHash.GetOrCompute("player_inventory_capacity");
		}

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				MyInventory inventory = localCharacter.GetInventory();
				if (inventory != null)
				{
					m_max = MyFixedPoint.MultiplySafe(inventory.MaxVolume, 1000).ToIntSafe();
					base.CurrentValue = MyFixedPoint.MultiplySafe(inventory.CurrentVolume, 1000).ToIntSafe();
				}
			}
		}
	}
}
