using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyGridPhysicalHierarchyData : IGroupData<MyCubeGrid>
	{
		public MyCubeGrid m_root;

		private MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group m_group;

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = (group as MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group);
		}

		public void OnRelease()
		{
			m_root = null;
			m_group = null;
		}

		public void OnNodeAdded(MyCubeGrid entity)
		{
		}

		public void OnNodeRemoved(MyCubeGrid entity)
		{
		}
	}
}
