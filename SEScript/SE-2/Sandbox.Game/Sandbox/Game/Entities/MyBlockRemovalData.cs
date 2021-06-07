using Sandbox.Game.Entities.Cube;

namespace Sandbox.Game.Entities
{
	public class MyBlockRemovalData
	{
		public MySlimBlock Block;

		public ushort? BlockIdInCompound;

		public bool CheckExisting;

		public MyBlockRemovalData(MySlimBlock block, ushort? blockIdInCompound = null, bool checkExisting = false)
		{
			Block = block;
			BlockIdInCompound = blockIdInCompound;
			CheckExisting = checkExisting;
		}
	}
}
