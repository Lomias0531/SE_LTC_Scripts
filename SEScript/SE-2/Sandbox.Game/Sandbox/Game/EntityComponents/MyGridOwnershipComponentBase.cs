using Sandbox.Game.Entities.Cube;
using VRage.Game.Components;

namespace Sandbox.Game.EntityComponents
{
	public abstract class MyGridOwnershipComponentBase : MyEntityComponentBase
	{
		public override string ComponentTypeDebugString => "Ownership";

		public abstract long GetBlockOwnerId(MySlimBlock block);
	}
}
