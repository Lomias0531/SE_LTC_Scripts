using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public class MyGridMechanicalGroupData : IGroupData<MyCubeGrid>
	{
		public void OnRelease()
		{
		}

		public void OnNodeAdded(MyCubeGrid entity)
		{
		}

		public void OnNodeRemoved(MyCubeGrid entity)
		{
		}

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
		}
	}
}
