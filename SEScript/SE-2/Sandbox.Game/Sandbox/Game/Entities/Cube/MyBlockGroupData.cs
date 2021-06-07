using VRage.Groups;

namespace Sandbox.Game.Entities.Cube
{
	public class MyBlockGroupData : IGroupData<MySlimBlock>
	{
		public void OnRelease()
		{
		}

		public void OnNodeAdded(MySlimBlock entity)
		{
		}

		public void OnNodeRemoved(MySlimBlock entity)
		{
		}

		public void OnCreate<TGroupData>(MyGroups<MySlimBlock, TGroupData>.Group group) where TGroupData : IGroupData<MySlimBlock>, new()
		{
		}
	}
}
