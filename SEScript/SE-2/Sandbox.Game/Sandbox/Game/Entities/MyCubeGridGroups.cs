using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyCubeGridGroups : IMySceneComponent
	{
		public static MyCubeGridGroups Static;

		private MyGroupsBase<MyCubeGrid>[] m_groupsByType;

		public MyGroups<MyCubeGrid, MyGridLogicalGroupData> Logical = new MyGroups<MyCubeGrid, MyGridLogicalGroupData>(supportOphrans: true);

		public MyGroups<MyCubeGrid, MyGridPhysicalGroupData> Physical = new MyGroups<MyCubeGrid, MyGridPhysicalGroupData>(supportOphrans: true, MyGridPhysicalGroupData.IsMajorGroup);

		public MyGroups<MyCubeGrid, MyGridNoDamageGroupData> NoContactDamage = new MyGroups<MyCubeGrid, MyGridNoDamageGroupData>(supportOphrans: true);

		public MyGroups<MyCubeGrid, MyGridMechanicalGroupData> Mechanical = new MyGroups<MyCubeGrid, MyGridMechanicalGroupData>(supportOphrans: true);

		public MyGroups<MySlimBlock, MyBlockGroupData> SmallToLargeBlockConnections = new MyGroups<MySlimBlock, MyBlockGroupData>();

		public MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData> PhysicalDynamic = new MyGroups<MyCubeGrid, MyGridPhysicalDynamicGroupData>();

		private static readonly HashSet<object> m_tmpBlocksDebugHelper = new HashSet<object>();

		public MyCubeGridGroups()
		{
			m_groupsByType = new MyGroupsBase<MyCubeGrid>[4];
			m_groupsByType[0] = Logical;
			m_groupsByType[1] = Physical;
			m_groupsByType[2] = NoContactDamage;
			m_groupsByType[3] = Mechanical;
		}

		public void AddNode(GridLinkTypeEnum type, MyCubeGrid grid)
		{
			GetGroups(type).AddNode(grid);
		}

		public void RemoveNode(GridLinkTypeEnum type, MyCubeGrid grid)
		{
			GetGroups(type).RemoveNode(grid);
		}

		public void CreateLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			GetGroups(type).CreateLink(linkId, parent, child);
			if (type == GridLinkTypeEnum.Physical && !parent.Physics.IsStatic && !child.Physics.IsStatic)
			{
				PhysicalDynamic.CreateLink(linkId, parent, child);
			}
		}

		public bool BreakLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child = null)
		{
			if (type == GridLinkTypeEnum.Physical)
			{
				PhysicalDynamic.BreakLink(linkId, parent, child);
			}
			return GetGroups(type).BreakLink(linkId, parent, child);
		}

		public void UpdateDynamicState(MyCubeGrid grid)
		{
			bool flag = PhysicalDynamic.GetGroup(grid) != null;
			bool flag2 = !grid.IsStatic;
			if (flag && !flag2)
			{
				PhysicalDynamic.BreakAllLinks(grid);
			}
			else if (!flag && flag2)
			{
				MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node = Physical.GetNode(grid);
				if (node != null)
				{
					foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> childLink in node.ChildLinks)
					{
						if (!childLink.Value.NodeData.IsStatic)
						{
							PhysicalDynamic.CreateLink(childLink.Key, grid, childLink.Value.NodeData);
						}
					}
					foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node> parentLink in node.ParentLinks)
					{
						if (!parentLink.Value.NodeData.IsStatic)
						{
							PhysicalDynamic.CreateLink(parentLink.Key, parentLink.Value.NodeData, grid);
						}
					}
				}
			}
		}

		public MyGroupsBase<MyCubeGrid> GetGroups(GridLinkTypeEnum type)
		{
			return m_groupsByType[(int)type];
		}

		void IMySceneComponent.Load()
		{
			Static = new MyCubeGridGroups();
		}

		void IMySceneComponent.Unload()
		{
			Static = null;
		}

		internal static void DebugDrawBlockGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups) where TNode : MySlimBlock where TGroupData : IGroupData<TNode>, new()
		{
			int num = 0;
			foreach (MyGroups<TNode, TGroupData>.Group group in groups.Groups)
			{
				Color color = new Vector3((float)(num++ % 15) / 15f, 1f, 1f).HSVtoColor();
				foreach (MyGroups<TNode, TGroupData>.Node node2 in group.Nodes)
				{
					try
					{
						node2.NodeData.GetWorldBoundingBox(out BoundingBoxD aabb);
						foreach (MyGroups<TNode, TGroupData>.Node child in node2.Children)
						{
							m_tmpBlocksDebugHelper.Add(child);
						}
						foreach (object item in m_tmpBlocksDebugHelper)
						{
							MyGroups<TNode, TGroupData>.Node node = null;
							int num2 = 0;
							foreach (MyGroups<TNode, TGroupData>.Node child2 in node2.Children)
							{
								if (item == child2)
								{
									node = child2;
									num2++;
								}
							}
							node.NodeData.GetWorldBoundingBox(out BoundingBoxD aabb2);
							MyRenderProxy.DebugDrawLine3D(aabb.Center, aabb2.Center, color, color, depthRead: false);
							MyRenderProxy.DebugDrawText3D((aabb.Center + aabb2.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
						}
						Color color2 = new Color(color.ToVector3() + 0.25f);
						MyRenderProxy.DebugDrawSphere(aabb.Center, 0.2f, color2.ToVector3(), 0.5f, depthRead: false, smooth: true);
						MyRenderProxy.DebugDrawText3D(aabb.Center, node2.LinkCount.ToString(), color2, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					finally
					{
						m_tmpBlocksDebugHelper.Clear();
					}
				}
			}
		}
	}
}
