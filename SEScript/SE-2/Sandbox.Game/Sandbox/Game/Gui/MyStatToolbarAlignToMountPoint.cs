using Sandbox.Game.Entities;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatToolbarAlignToMountPoint : MyStatBase
	{
		public MyStatToolbarAlignToMountPoint()
		{
			base.Id = MyStringHash.GetOrCompute("toolbar_align_to_mountpoint");
		}

		public override void Update()
		{
			base.CurrentValue = (MyCubeBuilder.Static.AlignToDefault ? 1 : 0);
		}
	}
}
