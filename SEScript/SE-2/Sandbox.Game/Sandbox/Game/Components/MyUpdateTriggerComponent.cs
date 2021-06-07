using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_UpdateTrigger), true)]
	public class MyUpdateTriggerComponent : MyTriggerComponent
	{
		private class Sandbox_Game_Components_MyUpdateTriggerComponent_003C_003EActor : IActivator, IActivator<MyUpdateTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyUpdateTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUpdateTriggerComponent CreateInstance()
			{
				return new MyUpdateTriggerComponent();
			}

			MyUpdateTriggerComponent IActivator<MyUpdateTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_size = 100;

		private Dictionary<MyEntity, MyEntityUpdateEnum> m_needsUpdate = new Dictionary<MyEntity, MyEntityUpdateEnum>();

		public int Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
				if (base.Entity != null)
				{
					m_AABB.Inflate(value / 2);
				}
			}
		}

		public override string ComponentTypeDebugString => "Pirate update trigger";

		public MyUpdateTriggerComponent()
		{
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_UpdateTrigger obj = base.Serialize(copy) as MyObjectBuilder_UpdateTrigger;
			obj.Size = m_size;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_UpdateTrigger myObjectBuilder_UpdateTrigger = builder as MyObjectBuilder_UpdateTrigger;
			m_size = myObjectBuilder_UpdateTrigger.Size;
		}

		private void grid_OnBlockOwnershipChanged(MyCubeGrid obj)
		{
			bool flag = false;
			foreach (long bigOwner in obj.BigOwners)
			{
				MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(bigOwner);
				if (playerFaction != null && !playerFaction.IsEveryoneNpc())
				{
					flag = true;
					break;
				}
			}
			foreach (long smallOwner in obj.SmallOwners)
			{
				MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(smallOwner);
				if (playerFaction2 != null && !playerFaction2.IsEveryoneNpc())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				obj.Components.Remove<MyUpdateTriggerComponent>();
				obj.OnBlockOwnershipChanged -= grid_OnBlockOwnershipChanged;
			}
		}

		public MyUpdateTriggerComponent(int triggerSize)
		{
			m_size = triggerSize;
		}

		protected override void UpdateInternal()
		{
			if (base.Entity.Physics == null)
			{
				return;
			}
			m_AABB = base.Entity.PositionComp.WorldAABB.Inflate(m_size / 2);
			bool flag = m_needsUpdate.Count != 0;
			for (int num = base.QueryResult.Count - 1; num >= 0; num--)
			{
				MyEntity myEntity = base.QueryResult[num];
				if (!myEntity.Closed && myEntity.PositionComp.WorldAABB.Intersects(m_AABB) && !(myEntity is MyMeteor))
				{
					break;
				}
				base.QueryResult.RemoveAtFast(num);
			}
			base.DoQuery = (base.QueryResult.Count == 0);
			base.UpdateInternal();
			if (base.QueryResult.Count == 0)
			{
				if (!flag)
				{
					DisableRecursively((MyEntity)base.Entity);
				}
			}
			else if (flag)
			{
				EnableRecursively((MyEntity)base.Entity);
				m_needsUpdate.Clear();
			}
		}

		protected override bool QueryEvaluator(MyEntity entity)
		{
			if (entity.Physics == null || entity.Physics.IsStatic)
			{
				return false;
			}
			if (entity is MyFloatingObject || entity is MyDebrisBase)
			{
				return false;
			}
			if (entity == base.Entity.GetTopMostParent())
			{
				return false;
			}
			return true;
		}

		private void DisableRecursively(MyEntity entity)
		{
			Enabled = false;
			m_needsUpdate[entity] = entity.NeedsUpdate;
			entity.NeedsUpdate = MyEntityUpdateEnum.NONE;
			entity.Render.Visible = false;
			if (entity.Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
				{
					DisableRecursively((MyEntity)child.Entity);
				}
			}
		}

		private void EnableRecursively(MyEntity entity)
		{
			Enabled = true;
			if (m_needsUpdate.ContainsKey(entity))
			{
				entity.NeedsUpdate = m_needsUpdate[entity];
			}
			entity.Render.Visible = true;
			if (entity.Hierarchy != null)
			{
				foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
				{
					EnableRecursively((MyEntity)child.Entity);
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			if (base.Entity != null && !base.Entity.MarkedForClose && base.QueryResult.Count != 0)
			{
				EnableRecursively((MyEntity)base.Entity);
				m_needsUpdate.Clear();
			}
			m_needsUpdate.Clear();
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.OnBlockOwnershipChanged += grid_OnBlockOwnershipChanged;
			}
		}
	}
}
