using System;
using VRage.Game.Components;

namespace Sandbox.Game.Entities.Cube
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyCubeGrids : MySessionComponentBase
	{
		private long Now => DateTime.Now.Ticks;

		public static event Action<MyCubeGrid, MySlimBlock> BlockBuilt;

		public static event Action<MyCubeGrid, MySlimBlock> BlockDestroyed;

		public static event Action<MyCubeGrid, MySlimBlock, bool> BlockFinished;

		public static event Action<MyCubeGrid, MySlimBlock, bool> BlockFunctional;

		internal static void NotifyBlockBuilt(MyCubeGrid grid, MySlimBlock block)
		{
			MyCubeGrids.BlockBuilt.InvokeIfNotNull(grid, block);
		}

		internal static void NotifyBlockDestroyed(MyCubeGrid grid, MySlimBlock block)
		{
			MyCubeGrids.BlockDestroyed.InvokeIfNotNull(grid, block);
		}

		internal static void NotifyBlockFinished(MyCubeGrid grid, MySlimBlock block, bool handWelded)
		{
			MyCubeGrids.BlockFinished.InvokeIfNotNull(grid, block, handWelded);
		}

		internal static void NotifyBlockFunctional(MyCubeGrid grid, MySlimBlock block, bool handWelded)
		{
			MyCubeGrids.BlockFunctional.InvokeIfNotNull(grid, block, handWelded);
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyCubeGrids.BlockBuilt = null;
			MyCubeGrids.BlockDestroyed = null;
		}
	}
}
