using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatToolbarGridSize : MyStatBase
	{
		public MyStatToolbarGridSize()
		{
			base.Id = MyStringHash.GetOrCompute("toolbar_grid_size");
		}

		public override void Update()
		{
			if (!MyCubeBuilder.Static.IsActivated || MyCubeBuilder.Static.ToolbarBlockDefinition == null)
			{
				base.CurrentValue = -1f;
			}
			else
			{
				base.CurrentValue = ((MyCubeBuilder.Static.ToolbarBlockDefinition.CubeSize != MyCubeSize.Small) ? 1 : 0);
			}
		}
	}
}
