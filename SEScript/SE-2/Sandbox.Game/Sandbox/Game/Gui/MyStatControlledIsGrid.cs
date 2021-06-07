using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledIsGrid : MyStatBase
	{
		public MyStatControlledIsGrid()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_is_grid");
		}

		public override void Update()
		{
			IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity == null)
			{
				base.CurrentValue = 0f;
			}
			if (myControllableEntity is MyLargeTurretBase)
			{
				myControllableEntity = (myControllableEntity as MyLargeTurretBase).PreviousControlledEntity;
			}
			base.CurrentValue = ((myControllableEntity is MyShipController) ? 1 : 0);
		}
	}
}
