using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityHandbreak : MyStatBase
	{
		public MyStatControlledEntityHandbreak()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_handbreak");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				MyCubeGrid myCubeGrid = controlledEntity.Entity.Parent as MyCubeGrid;
				if (myCubeGrid != null)
				{
					base.CurrentValue = (myCubeGrid.GridSystems.WheelSystem.HandBrake ? 1f : 0f);
				}
			}
		}
	}
}
