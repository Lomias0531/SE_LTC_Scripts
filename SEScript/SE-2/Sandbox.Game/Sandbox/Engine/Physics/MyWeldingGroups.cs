using Sandbox.Game.Entities;
using System.Threading;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyWeldingGroups : MyGroups<MyEntity, MyWeldGroupData>, IMySceneComponent
	{
		private static MyWeldingGroups m_static;

		public static MyWeldingGroups Static => m_static;

		public void Load()
		{
			m_static = this;
			base.SupportsOphrans = true;
		}

		public void Unload()
		{
			m_static = null;
		}

		public static MyEntity ReplaceParent(Group group, MyEntity oldParent, MyEntity newParent)
		{
			if (oldParent != null && oldParent.Physics != null)
			{
				oldParent.GetPhysicsBody().UnweldAll(insertInWorld: false);
			}
			else
			{
				if (group == null)
				{
					return oldParent;
				}
				foreach (Node node in group.Nodes)
				{
					if (!node.NodeData.MarkedForClose)
					{
						node.NodeData.GetPhysicsBody().Unweld(insertInWorld: false);
					}
				}
			}
			if (group == null)
			{
				return oldParent;
			}
			if (newParent == null)
			{
				foreach (Node node2 in group.Nodes)
				{
					if (!node2.NodeData.MarkedForClose && node2.NodeData != oldParent)
					{
						if (node2.NodeData.Physics.IsStatic)
						{
							newParent = node2.NodeData;
							break;
						}
						if (node2.NodeData.Physics.RigidBody2 != null)
						{
							newParent = node2.NodeData;
						}
					}
				}
			}
			foreach (Node node3 in group.Nodes)
			{
				if (!node3.NodeData.MarkedForClose && newParent != node3.NodeData)
				{
					if (newParent == null)
					{
						newParent = node3.NodeData;
					}
					else
					{
						newParent.GetPhysicsBody().Weld(node3.NodeData.Physics, recreateShape: false);
					}
				}
			}
			if (newParent != null && !newParent.Physics.IsInWorld)
			{
				newParent.Physics.Activate();
			}
			return newParent;
		}

		public override void CreateLink(long linkId, MyEntity parentNode, MyEntity childNode)
		{
			if (MySandboxGame.Static.UpdateThread == Thread.CurrentThread)
			{
				base.CreateLink(linkId, parentNode, childNode);
			}
		}

		public bool IsEntityParent(MyEntity entity)
		{
			Group group = GetGroup(entity);
			if (group == null)
			{
				return true;
			}
			return entity == group.GroupData.Parent;
		}

		public MyWeldingGroups()
			: base(supportOphrans: false, (MajorGroupComparer)null)
		{
		}
	}
}
