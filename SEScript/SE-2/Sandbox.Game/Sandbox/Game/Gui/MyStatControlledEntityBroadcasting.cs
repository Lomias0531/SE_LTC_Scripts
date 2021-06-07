using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityBroadcasting : MyStatBase
	{
		public MyStatControlledEntityBroadcasting()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_broadcasting");
		}

		public override void Update()
		{
			IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity != null)
			{
				if (myControllableEntity is MyLargeTurretBase)
				{
					myControllableEntity = (myControllableEntity as MyLargeTurretBase).PreviousControlledEntity;
				}
				MyCubeGrid myCubeGrid = myControllableEntity.Entity.Parent as MyCubeGrid;
				if (myCubeGrid != null)
				{
					base.CurrentValue = ((myCubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled == MyMultipleEnabledEnum.AllEnabled || myCubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled == MyMultipleEnabledEnum.Mixed) ? 1f : 0f);
				}
				else
				{
					base.CurrentValue = (myControllableEntity.EnabledBroadcasting ? 1f : 0f);
				}
			}
		}
	}
}
