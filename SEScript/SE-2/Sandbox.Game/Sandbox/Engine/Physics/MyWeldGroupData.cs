using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyWeldGroupData : IGroupData<MyEntity>
	{
		private MyGroups<MyEntity, MyWeldGroupData>.Group m_group;

		private MyEntity m_weldParent;

		public MyEntity Parent => m_weldParent;

		public void OnRelease()
		{
			m_group = null;
			m_weldParent = null;
		}

		public void OnNodeAdded(MyEntity entity)
		{
			if (entity.MarkedForClose)
			{
				return;
			}
			if (m_weldParent == null)
			{
				m_weldParent = entity;
			}
			else
			{
				MyPhysicsBody myPhysicsBody = m_weldParent.Physics as MyPhysicsBody;
				if (myPhysicsBody.IsStatic)
				{
					myPhysicsBody.Weld(entity.Physics as MyPhysicsBody);
				}
				else if (entity.Physics.IsStatic || (myPhysicsBody.RigidBody2 == null && entity.Physics.RigidBody2 != null))
				{
					ReplaceParent(entity);
				}
				else
				{
					myPhysicsBody.Weld(entity.Physics as MyPhysicsBody);
				}
			}
			if (m_weldParent.Physics != null && m_weldParent.Physics.RigidBody != null)
			{
				m_weldParent.Physics.RigidBody.Activate();
			}
			m_weldParent.RaisePhysicsChanged();
		}

		public void OnNodeRemoved(MyEntity entity)
		{
			if (m_weldParent == null)
			{
				return;
			}
			if (m_weldParent == entity)
			{
				if ((m_group.Nodes.Count != 1 || !m_group.Nodes.First().NodeData.MarkedForClose) && m_group.Nodes.Count > 0)
				{
					ReplaceParent(null);
				}
			}
			else if (m_weldParent.Physics != null && !entity.MarkedForClose)
			{
				(m_weldParent.Physics as MyPhysicsBody).Unweld(entity.Physics as MyPhysicsBody);
			}
			if (m_weldParent != null && m_weldParent.Physics != null && m_weldParent.Physics.RigidBody != null)
			{
				m_weldParent.Physics.RigidBody.Activate();
				m_weldParent.RaisePhysicsChanged();
			}
			entity.RaisePhysicsChanged();
		}

		private void ReplaceParent(MyEntity newParent)
		{
			m_weldParent = MyWeldingGroups.ReplaceParent(m_group, m_weldParent, newParent);
		}

		public void OnCreate<TGroupData>(MyGroups<MyEntity, TGroupData>.Group group) where TGroupData : IGroupData<MyEntity>, new()
		{
			m_group = (group as MyGroups<MyEntity, MyWeldGroupData>.Group);
		}

		public bool UpdateParent(MyEntity oldParent)
		{
			MyPhysicsBody physicsBody = oldParent.GetPhysicsBody();
			if (physicsBody.WeldedRigidBody.IsFixed)
			{
				return false;
			}
			MyPhysicsBody myPhysicsBody = physicsBody;
			foreach (MyPhysicsBody child in physicsBody.WeldInfo.Children)
			{
				if (child.WeldedRigidBody.IsFixed)
				{
					myPhysicsBody = child;
					break;
				}
				if (!myPhysicsBody.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC) && child.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC))
				{
					myPhysicsBody = child;
				}
			}
			if (myPhysicsBody == physicsBody)
			{
				return false;
			}
			ReplaceParent((MyEntity)myPhysicsBody.Entity);
			myPhysicsBody.Weld(physicsBody);
			return true;
		}
	}
}
