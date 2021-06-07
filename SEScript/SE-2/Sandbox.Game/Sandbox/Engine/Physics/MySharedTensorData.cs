using Sandbox.Game.Entities;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	internal class MySharedTensorData : IGroupData<MyCubeGrid>
	{
		public MyGroups<MyCubeGrid, MySharedTensorData>.Group m_group
		{
			get;
			set;
		}

		public void OnNodeAdded(MyCubeGrid grid)
		{
			MarkDirty();
			MarkGridTensorDirty(grid);
		}

		public void OnNodeRemoved(MyCubeGrid grid)
		{
			MarkDirty();
			MarkGridTensorDirty(grid);
		}

		public void MarkDirty()
		{
			foreach (MyGroups<MyCubeGrid, MySharedTensorData>.Node node in m_group.Nodes)
			{
				MarkGridTensorDirty(node.NodeData);
			}
		}

		public static void MarkGridTensorDirty(MyCubeGrid grid)
		{
			grid.Physics?.Shape.MarkSharedTensorDirty();
		}

		public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
		{
			m_group = (group as MyGroups<MyCubeGrid, MySharedTensorData>.Group);
		}

		public void OnRelease()
		{
			m_group = null;
		}
	}
}
