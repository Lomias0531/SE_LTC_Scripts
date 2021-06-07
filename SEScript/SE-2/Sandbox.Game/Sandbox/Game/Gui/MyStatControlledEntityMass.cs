using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityMass : MyStatBase
	{
		public override float MaxValue => 0f;

		public MyStatControlledEntityMass()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_mass");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				MyCubeGrid myCubeGrid = null;
				MyCockpit myCockpit = controlledEntity.Entity as MyCockpit;
				if (myCockpit != null)
				{
					myCubeGrid = myCockpit.CubeGrid;
				}
				else
				{
					MyRemoteControl myRemoteControl = controlledEntity as MyRemoteControl;
					if (myRemoteControl != null)
					{
						myCubeGrid = myRemoteControl.CubeGrid;
					}
					else
					{
						MyLargeTurretBase myLargeTurretBase = controlledEntity as MyLargeTurretBase;
						if (myLargeTurretBase != null)
						{
							myCubeGrid = myLargeTurretBase.CubeGrid;
						}
					}
				}
				base.CurrentValue = (myCubeGrid?.GetCurrentMass() ?? 0);
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
