using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledIsStatic : MyStatBase
	{
		public MyStatControlledIsStatic()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_is_static");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				MyCubeGrid myCubeGrid = controlledEntity.Entity as MyCubeGrid;
				if (myCubeGrid != null)
				{
					base.CurrentValue = (myCubeGrid.IsStatic ? 1 : 0);
					return;
				}
				MyCockpit myCockpit = controlledEntity.Entity as MyCockpit;
				if (myCockpit != null)
				{
					base.CurrentValue = (myCockpit.CubeGrid.IsStatic ? 1 : 0);
					return;
				}
				if (controlledEntity is MyLargeTurretBase)
				{
					base.CurrentValue = ((controlledEntity as MyLargeTurretBase).CubeGrid.IsStatic ? 1 : 0);
				}
			}
			base.CurrentValue = 0f;
		}
	}
}
