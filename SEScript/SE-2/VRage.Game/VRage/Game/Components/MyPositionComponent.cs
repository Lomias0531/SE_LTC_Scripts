using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
	public class MyPositionComponent : MyPositionComponentBase
	{
		private class VRage_Game_Components_MyPositionComponent_003C_003EActor : IActivator, IActivator<MyPositionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyPositionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPositionComponent CreateInstance()
			{
				return new MyPositionComponent();
			}

			MyPositionComponent IActivator<MyPositionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Action<object> WorldPositionChanged;

		private MySyncComponentBase m_syncObject;

		private MyPhysicsComponentBase m_physics;

		private MyHierarchyComponentBase m_hierarchy;

		public static bool SynchronizationEnabled = true;

		private List<MyEntity> entities = new List<MyEntity>();

		private Dictionary<string, int> types = new Dictionary<string, int>();

		/// <summary>
		/// Sets the local aabb.
		/// </summary>
		/// <value>
		/// The local aabb.
		/// </value>
		public override BoundingBox LocalAABB
		{
			get
			{
				return m_localAABB;
			}
			set
			{
				base.LocalAABB = value;
				base.Container.Entity.UpdateGamePruningStructure();
			}
		}

		protected override bool ShouldSync
		{
			get
			{
				if (SynchronizationEnabled && base.Container.Get<MySyncComponentBase>() != null)
				{
					return m_syncObject != null;
				}
				return false;
			}
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_syncObject = base.Container.Get<MySyncComponentBase>();
			m_physics = base.Container.Get<MyPhysicsComponentBase>();
			m_hierarchy = base.Container.Get<MyHierarchyComponentBase>();
			base.Container.ComponentAdded += container_ComponentAdded;
			base.Container.ComponentRemoved += container_ComponentRemoved;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			base.Container.ComponentAdded -= container_ComponentAdded;
			base.Container.ComponentRemoved -= container_ComponentRemoved;
		}

		private void container_ComponentAdded(Type type, MyEntityComponentBase comp)
		{
			if (type == typeof(MySyncComponentBase))
			{
				m_syncObject = (comp as MySyncComponentBase);
			}
			else if (type == typeof(MyPhysicsComponentBase))
			{
				m_physics = (comp as MyPhysicsComponentBase);
			}
			else if (type == typeof(MyHierarchyComponentBase))
			{
				m_hierarchy = (comp as MyHierarchyComponentBase);
			}
		}

		private void container_ComponentRemoved(Type type, MyEntityComponentBase comp)
		{
			if (type == typeof(MySyncComponentBase))
			{
				m_syncObject = null;
			}
			else if (type == typeof(MyPhysicsComponentBase))
			{
				m_physics = null;
			}
			else if (type == typeof(MyHierarchyComponentBase))
			{
				m_hierarchy = null;
			}
		}

		/// <summary>
		/// Updates the childs of this entity.
		/// </summary>
		protected virtual void UpdateChildren(object source, bool forceUpdateAllChildren)
		{
			MatrixD parentWorldMatrix = base.WorldMatrix;
			foreach (MyHierarchyComponentBase item in forceUpdateAllChildren ? m_hierarchy.Children : m_hierarchy.ChildrenNeedingWorldMatrix)
			{
				item.Container.Entity.PositionComp.UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren: true, forceUpdateAllChildren);
			}
		}

		/// <summary>
		/// Called when [world position changed].
		/// </summary>
		/// <param name="source">The source object that caused this event.</param>
		protected override void OnWorldPositionChanged(object source, bool updateChildren, bool forceUpdateAllChildren)
		{
			base.Container.Entity.UpdateGamePruningStructure();
			if (updateChildren && m_hierarchy != null && m_hierarchy.Children.Count > 0)
			{
				UpdateChildren(source, forceUpdateAllChildren);
			}
			m_worldVolumeDirty = true;
			m_worldAABBDirty = true;
			m_normalizedInvMatrixDirty = true;
			m_invScaledMatrixDirty = true;
			if (m_physics != null && m_physics != source && m_physics.Enabled)
			{
				m_physics.OnWorldPositionChanged(source);
			}
			RaiseOnPositionChanged(this);
			WorldPositionChanged.InvokeIfNotNull(source);
			if (base.Container.Entity.Render != null && (base.Entity.Flags & EntityFlags.InvalidateOnMove) != 0)
			{
				base.Container.Entity.Render.InvalidateRenderObjects();
			}
		}
	}
}
