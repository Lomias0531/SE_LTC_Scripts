using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	public class MyGridPhysicalHierarchy : MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>, IMySceneComponent
	{
		public delegate void MyActionWithData(MyCubeGrid grid, ref object data);

		public static MyGridPhysicalHierarchy Static;

		private readonly Dictionary<long, HashSet<MyEntity>> m_nonGridChildren = new Dictionary<long, HashSet<MyEntity>>();

		public void Load()
		{
			Static = this;
			base.SupportsOphrans = true;
			base.SupportsChildToChild = true;
		}

		public void Unload()
		{
			Static = null;
		}

		public override void AddNode(MyCubeGrid nodeToAdd)
		{
			base.AddNode(nodeToAdd);
			UpdateRoot(nodeToAdd);
		}

		public override void CreateLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode)
		{
			base.CreateLink(linkId, parentNode, childNode);
			UpdateRoot(parentNode);
		}

		public override bool BreakLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode = null)
		{
			if (childNode == null)
			{
				childNode = GetNode(parentNode).m_children[linkId].NodeData;
			}
			bool flag = base.BreakLink(linkId, parentNode, childNode);
			if (!flag)
			{
				flag = base.BreakLink(linkId, childNode, parentNode);
			}
			if (flag)
			{
				UpdateRoot(parentNode);
				if (GetGroup(parentNode) != GetGroup(childNode))
				{
					UpdateRoot(childNode);
				}
			}
			return flag;
		}

		public MyCubeGrid GetParent(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node == null)
			{
				return null;
			}
			return GetParent(node);
		}

		public MyCubeGrid GetParent(Node node)
		{
			if (node.m_parents.Count == 0)
			{
				return null;
			}
			return node.m_parents.FirstPair().Value.NodeData;
		}

		public long GetParentLinkId(Node node)
		{
			if (node.m_parents.Count == 0)
			{
				return 0L;
			}
			return node.m_parents.FirstPair().Key;
		}

		public bool IsEntityParent(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return true;
			}
			return GetParent(myCubeGrid) == null;
		}

		public MyCubeGrid GetRoot(MyCubeGrid grid)
		{
			Group group = GetGroup(grid);
			if (group == null)
			{
				return grid;
			}
			MyCubeGrid myCubeGrid = group.GroupData.m_root;
			if (myCubeGrid == null)
			{
				myCubeGrid = grid;
			}
			return myCubeGrid;
		}

		public MyEntity GetEntityConnectingToParent(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node == null)
			{
				return null;
			}
			if (node.m_parents.Count == 0)
			{
				return null;
			}
			return MyEntities.GetEntityById(node.m_parents.FirstPair().Key);
		}

		public bool HasChildren(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node != null)
			{
				return node.Children.Count > 0;
			}
			return false;
		}

		public bool IsCyclic(MyCubeGrid grid)
		{
			Node node = GetNode(grid);
			if (node != null && node.Children.Count > 0)
			{
				foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
				{
					if (GetParentLinkId(childLink.Value) != childLink.Key)
					{
						return true;
					}
					if (IsCyclic(childLink.Value.NodeData))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void ApplyOnChildren(MyCubeGrid grid, Action<MyCubeGrid> action)
		{
			Node node = GetNode(grid);
			if (node != null && node.Children.Count > 0)
			{
				foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
				{
					if (GetParentLinkId(childLink.Value) == childLink.Key)
					{
						action(childLink.Value.NodeData);
					}
				}
			}
		}

		public void ApplyOnChildren(MyCubeGrid grid, ref object data, MyActionWithData action)
		{
			Node node = GetNode(grid);
			if (node != null && node.Children.Count > 0)
			{
				foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
				{
					if (GetParentLinkId(childLink.Value) == childLink.Key)
					{
						action(childLink.Value.NodeData, ref data);
					}
				}
			}
		}

		public void ApplyOnAllChildren(MyEntity entity, Action<MyEntity> action)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				Node node = GetNode(myCubeGrid);
				if (node != null && node.Children.Count > 0)
				{
					foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
					{
						if (GetParentLinkId(childLink.Value) == childLink.Key)
						{
							action(childLink.Value.NodeData);
						}
					}
				}
				if (node != null && m_nonGridChildren.TryGetValue(myCubeGrid.EntityId, out HashSet<MyEntity> value))
				{
					foreach (MyEntity item in value)
					{
						action(item);
					}
				}
			}
		}

		public bool InSameHierarchy(MyCubeGrid first, MyCubeGrid second)
		{
			MyCubeGrid root = GetRoot(first);
			MyCubeGrid root2 = GetRoot(second);
			return root == root2;
		}

		public bool IsChildOf(MyCubeGrid parentGrid, MyEntity entity)
		{
			Node node = GetNode(parentGrid);
			if (node != null && node.Children.Count > 0)
			{
				foreach (KeyValuePair<long, Node> childLink in node.ChildLinks)
				{
					if (GetParentLinkId(childLink.Value) == childLink.Key && childLink.Value.NodeData == entity)
					{
						return true;
					}
				}
			}
			if (node != null && m_nonGridChildren.TryGetValue(parentGrid.EntityId, out HashSet<MyEntity> value))
			{
				return value.Contains(entity);
			}
			return false;
		}

		public void UpdateRoot(MyCubeGrid node)
		{
			if (MyEntities.IsClosingAll)
			{
				return;
			}
			Group group = GetGroup(node);
			if (group != null)
			{
				MyCubeGrid myCubeGrid = CalculateNewRoot(group);
				group.GroupData.m_root = myCubeGrid;
				if (myCubeGrid != null)
				{
					ReplaceRoot(myCubeGrid);
					foreach (Node node2 in group.Nodes)
					{
						node2.NodeData.HierarchyUpdated(myCubeGrid);
					}
				}
			}
		}

		private MyCubeGrid CalculateNewRoot(Group group)
		{
			if (group.m_members.Count == 1)
			{
				return group.m_members.FirstElement().NodeData;
			}
			Node node = null;
			float num = 0f;
			List<Node> list = new List<Node>();
			if (group.m_members.Count == 1)
			{
				return group.m_members.FirstElement().NodeData;
			}
			bool flag = false;
			long num2 = long.MaxValue;
			foreach (Node node4 in group.Nodes)
			{
				if (node4.NodeData.IsStatic || MyFixedGrids.IsRooted(node4.NodeData))
				{
					if (!flag)
					{
						list.Clear();
						node = null;
						flag = true;
					}
					list.Add(node4);
				}
				if (!flag)
				{
					if (IsGridControlled(node4.NodeData) && node4.NodeData.EntityId < num2)
					{
						node = node4;
						num2 = node4.NodeData.EntityId;
					}
					if (node4.NodeData.Physics != null)
					{
						float num3 = 0f;
						HkMassProperties? massProperties = node4.NodeData.Physics.Shape.MassProperties;
						if (massProperties.HasValue)
						{
							num3 = massProperties.Value.Mass;
						}
						if (num3 > num)
						{
							num = num3;
							list.Clear();
							list.Add(node4);
						}
						else if (num3 == num)
						{
							list.Add(node4);
						}
					}
				}
			}
			Node node2 = null;
			if (list.Count == 1)
			{
				node2 = list[0];
			}
			else if (list.Count > 1)
			{
				long entityId = list[0].NodeData.EntityId;
				Node node3 = list[0];
				foreach (Node item in list)
				{
					if (MyWeldingGroups.Static.IsEntityParent(item.NodeData) && entityId > item.NodeData.EntityId)
					{
						entityId = item.NodeData.EntityId;
						node3 = item;
					}
				}
				node2 = node3;
			}
			if (node != null)
			{
				if (node2 == null)
				{
					node2 = node;
				}
				else if (node.NodeData.Physics != null && node2.NodeData.Physics != null)
				{
					float num4 = 0f;
					HkMassProperties? massProperties2 = node.NodeData.Physics.Shape.MassProperties;
					if (massProperties2.HasValue)
					{
						num4 = massProperties2.Value.Mass;
					}
					float num5 = 0f;
					massProperties2 = node2.NodeData.Physics.Shape.MassProperties;
					if (massProperties2.HasValue)
					{
						num5 = massProperties2.Value.Mass;
					}
					if (num5 / num4 < 2f)
					{
						node2 = node;
					}
				}
				else
				{
					node2 = node;
				}
			}
			return node2?.NodeData;
		}

		private bool IsGridControlled(MyCubeGrid grid)
		{
			MyShipController shipController = grid.GridSystems.ControlSystem.GetShipController();
			if (shipController != null)
			{
				return shipController.CubeGrid == grid;
			}
			return false;
		}

		public Vector3? GetPivot(MyCubeGrid grid, bool parent = false)
		{
			return (GetEntityConnectingToParent(grid) as MyMechanicalConnectionBlockBase)?.GetConstraintPosition(grid, parent);
		}

		public void AddNonGridNode(MyCubeGrid parent, MyEntity entity)
		{
			if (GetGroup(parent) != null)
			{
				if (!m_nonGridChildren.TryGetValue(parent.EntityId, out HashSet<MyEntity> value))
				{
					value = new HashSet<MyEntity>();
					m_nonGridChildren.Add(parent.EntityId, value);
					parent.OnClose += RemoveAllNonGridNodes;
				}
				value.Add(entity);
			}
		}

		public void RemoveNonGridNode(MyCubeGrid parent, MyEntity entity)
		{
			if (GetGroup(parent) != null && m_nonGridChildren.TryGetValue(parent.EntityId, out HashSet<MyEntity> value))
			{
				value.Remove(entity);
				if (value.Count == 0)
				{
					m_nonGridChildren.Remove(parent.EntityId);
					parent.OnClose -= RemoveAllNonGridNodes;
				}
			}
		}

		private void RemoveAllNonGridNodes(MyEntity parent)
		{
			m_nonGridChildren.Remove(parent.EntityId);
			parent.OnClose -= RemoveAllNonGridNodes;
		}

		public bool NonGridLinkExists(long parentId, MyEntity child)
		{
			if (m_nonGridChildren.TryGetValue(parentId, out HashSet<MyEntity> value))
			{
				return value.Contains(child);
			}
			return false;
		}

		public int GetNodeChainLength(MyCubeGrid grid)
		{
			return GetNode(grid)?.ChainLength ?? 0;
		}

		public void Log(MyCubeGrid grid)
		{
			MyLog.Default.IncreaseIndent();
			MyLog.Default.WriteLine(string.Format("{0}: name={1} physics={2} mass={3} static={4} controlled={5}", grid.EntityId, grid.DisplayName, grid.Physics != null, (grid.Physics != null && grid.Physics.Shape.MassProperties.HasValue) ? grid.Physics.Shape.MassProperties.Value.Mass.ToString() : "None", grid.IsStatic, IsGridControlled(grid)));
			ApplyOnChildren(grid, Log);
			MyLog.Default.DecreaseIndent();
		}

		public void Draw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_HIERARCHY)
			{
				ApplyOnNodes(DrawNode);
			}
		}

		private void DrawNode(MyCubeGrid grid, Node node)
		{
			if (node.m_parents.Count > 0)
			{
				MyRenderProxy.DebugDrawArrow3D(grid.PositionComp.GetPosition(), node.m_parents.FirstPair().Value.NodeData.PositionComp.GetPosition(), Color.Orange);
			}
			else
			{
				MyRenderProxy.DebugDrawAxis(grid.PositionComp.WorldMatrix, 1f, depthRead: false);
			}
		}

		public MyGridPhysicalHierarchy()
			: base(supportOphrans: false, (MajorGroupComparer)null)
		{
		}
	}
}
