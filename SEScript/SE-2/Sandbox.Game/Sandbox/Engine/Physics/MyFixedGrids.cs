using Sandbox.Game.Entities;
using System.Collections.Generic;
using System.Threading;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyFixedGrids : MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>, IMySceneComponent
	{
		public class MyFixedGridsGroupData : IGroupData<MyCubeGrid>
		{
			private Group m_group;

			private int m_rootedGrids;

			public bool IsRooted => m_rootedGrids > 0;

			public void OnNodeAdded(MyCubeGrid grid)
			{
				bool flag = false;
				if (Static.m_roots.Contains(grid))
				{
					OnRootAdded();
					flag = true;
				}
				if (flag | (m_rootedGrids != 0))
				{
					ConvertGrid(grid, @static: true);
				}
			}

			public void OnNodeRemoved(MyCubeGrid grid)
			{
				if (Static.m_roots.Contains(grid))
				{
					OnRootRemoved();
				}
				else if (m_rootedGrids != 0)
				{
					ConvertGrid(grid, @static: false);
				}
			}

			public void OnRootAdded()
			{
				if (m_rootedGrids++ == 0)
				{
					Convert(@static: true);
				}
			}

			public void OnRootRemoved()
			{
				if (--m_rootedGrids == 0)
				{
					Convert(@static: false);
				}
			}

			private void Convert(bool @static)
			{
				foreach (Node node in m_group.Nodes)
				{
					ConvertGrid(node.NodeData, @static);
				}
			}

			public static void ConvertGrid(MyCubeGrid grid, bool @static)
			{
				grid.IsMarkedForEarlyDeactivation = @static;
			}

			public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
			{
				m_group = (group as Group);
			}

			public void OnRelease()
			{
				m_group = null;
			}

			public static bool MajorSelector(Group major, Group minor)
			{
				bool num = major.GroupData.m_rootedGrids > 0;
				bool flag = minor.GroupData.m_rootedGrids > 0;
				if (num)
				{
					if (!flag)
					{
						return true;
					}
				}
				else if (flag)
				{
					return false;
				}
				return major.Nodes.Count >= minor.Nodes.Count;
			}
		}

		private static MyFixedGrids m_static;

		private HashSet<MyCubeGrid> m_roots = new HashSet<MyCubeGrid>();

		private static MyFixedGrids Static => m_static;

		public MyFixedGrids()
			: base(supportOphrans: false, (MajorGroupComparer)MyFixedGridsGroupData.MajorSelector)
		{
			base.SupportsChildToChild = true;
		}

		public void Load()
		{
			m_static = this;
		}

		public void Unload()
		{
			m_static = null;
		}

		private static void AssertThread()
		{
			_ = MySandboxGame.Static.UpdateThread;
			_ = Thread.CurrentThread;
		}

		public static void MarkGridRoot(MyCubeGrid grid)
		{
			AssertThread();
			if (Static.m_roots.Add(grid))
			{
				Group group = Static.GetGroup(grid);
				if (group == null)
				{
					MyFixedGridsGroupData.ConvertGrid(grid, @static: true);
				}
				else
				{
					group.GroupData.OnRootAdded();
				}
			}
		}

		public static void UnmarkGridRoot(MyCubeGrid grid)
		{
			AssertThread();
			if (Static.m_roots.Remove(grid))
			{
				Group group = Static.GetGroup(grid);
				if (group == null)
				{
					MyFixedGridsGroupData.ConvertGrid(grid, @static: false);
				}
				else
				{
					group.GroupData.OnRootRemoved();
				}
			}
		}

		public static void Link(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			AssertThread();
			Static.CreateLink(linkingBlock.EntityId, parent, child);
		}

		public static void BreakLink(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
		{
			AssertThread();
			Static.BreakLink(linkingBlock.EntityId, parent, child);
		}

		public static bool IsRooted(MyCubeGrid grid)
		{
			if (!MyPhysics.InsideSimulation)
			{
				AssertThread();
			}
			if (Static.m_roots.Contains(grid))
			{
				return true;
			}
			return Static.GetGroup(grid)?.GroupData.IsRooted ?? false;
		}
	}
}
