using Sandbox.Game.Entities;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatToolbarSymmetryActive : MyStatBase
	{
		public MyStatToolbarSymmetryActive()
		{
			base.Id = MyStringHash.GetOrCompute("toolbar_symmetry");
		}

		public override void Update()
		{
			base.CurrentValue = (MyCubeBuilder.Static.UseSymmetry ? 1 : 0);
		}
	}
}
