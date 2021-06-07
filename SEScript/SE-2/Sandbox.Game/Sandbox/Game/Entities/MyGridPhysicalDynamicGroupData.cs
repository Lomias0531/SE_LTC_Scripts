using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public class MyGridPhysicalDynamicGroupData : IGroupData<MyCubeGrid>
	{
		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}

		public void OnRelease()
		{
		}

		public void OnNodeAdded(MyCubeGrid entity)
		{
		}

		public void OnNodeRemoved(MyCubeGrid entity)
		{
		}
	}
}
