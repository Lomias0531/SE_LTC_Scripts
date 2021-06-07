using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_HierarchyComponentBase), true)]
	public class MyHierarchyComponentBase : MyEntityComponentBase
	{
		private class VRage_Game_Components_MyHierarchyComponentBase_003C_003EActor : IActivator, IActivator<MyHierarchyComponentBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyHierarchyComponentBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHierarchyComponentBase CreateInstance()
			{
				return new MyHierarchyComponentBase();
			}

			MyHierarchyComponentBase IActivator<MyHierarchyComponentBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly List<MyHierarchyComponentBase> m_children = new List<MyHierarchyComponentBase>();

		private readonly List<MyHierarchyComponentBase> m_childrenNeedingWorldMatrix = new List<MyHierarchyComponentBase>();

		private readonly List<MyEntity> m_deserializedEntities = new List<MyEntity>();

		/// Identifier for the parent hierarchy.
		///
		/// This is should be reliably unique within a hierarchy level but only usable by the parent.
		public long ChildId;

		private MyEntityComponentContainer m_parentContainer;

		private MyHierarchyComponentBase m_parent;

		/// <summary>
		/// Gets the children collection.
		/// </summary>
		public ListReader<MyHierarchyComponentBase> Children => m_children;

		/// <summary>
		/// Gets the children collection for spatial updates.
		/// </summary>
		public ListReader<MyHierarchyComponentBase> ChildrenNeedingWorldMatrix => m_childrenNeedingWorldMatrix;

		public MyHierarchyComponentBase Parent
		{
			get
			{
				return m_parent;
			}
			set
			{
				MyHierarchyComponentBase parent = m_parent;
				if (m_parentContainer != null)
				{
					m_parentContainer.ComponentAdded -= Container_ComponentAdded;
					m_parentContainer.ComponentRemoved -= Container_ComponentRemoved;
					m_parentContainer = null;
				}
				m_parent = value;
				if (m_parent != null)
				{
					m_parentContainer = m_parent.Container;
					m_parentContainer.ComponentAdded += Container_ComponentAdded;
					m_parentContainer.ComponentRemoved += Container_ComponentRemoved;
				}
				this.OnParentChanged.InvokeIfNotNull(parent, m_parent);
			}
		}

		public override string ComponentTypeDebugString => "Hierarchy";

		public event Action<IMyEntity> OnChildRemoved;

		public event Action<MyHierarchyComponentBase, MyHierarchyComponentBase> OnParentChanged;

		/// <summary>
		/// Return top most parent of this entity
		/// </summary>
		/// <returns></returns>
		public MyHierarchyComponentBase GetTopMostParent(Type type = null)
		{
			MyHierarchyComponentBase myHierarchyComponentBase = this;
			while (myHierarchyComponentBase.Parent != null && (type == null || !myHierarchyComponentBase.Container.Contains(type)))
			{
				myHierarchyComponentBase = myHierarchyComponentBase.Parent;
			}
			return myHierarchyComponentBase;
		}

		private void Container_ComponentRemoved(Type arg1, MyEntityComponentBase arg2)
		{
			if (arg2 == m_parent)
			{
				m_parent = null;
			}
		}

		private void Container_ComponentAdded(Type arg1, MyEntityComponentBase arg2)
		{
			if (typeof(MyHierarchyComponentBase).IsAssignableFrom(arg1))
			{
				m_parent = (arg2 as MyHierarchyComponentBase);
			}
		}

		/// <summary>
		/// Adds the child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="preserveWorldPos">if set to <c>true</c> [preserve absolute position].</param>
		public void AddChild(IMyEntity child, bool preserveWorldPos = false, bool insertIntoSceneIfNeeded = true)
		{
			MyHierarchyComponentBase myHierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
			if (!m_children.Contains(myHierarchyComponentBase))
			{
				MatrixD worldMatrix = child.WorldMatrix;
				myHierarchyComponentBase.Parent = this;
				m_children.Add(myHierarchyComponentBase);
				if (child.NeedsWorldMatrix)
				{
					m_childrenNeedingWorldMatrix.Add(myHierarchyComponentBase);
				}
				if (preserveWorldPos)
				{
					child.PositionComp.SetWorldMatrix(worldMatrix, base.Entity, forceUpdate: true, updateChildren: true, updateLocal: true, skipTeleportCheck: false, forceUpdateAllChildren: false, ignoreAssert: true);
				}
				else
				{
					MyPositionComponentBase myPositionComponentBase = base.Container.Get<MyPositionComponentBase>();
					MyPositionComponentBase myPositionComponentBase2 = child.Components.Get<MyPositionComponentBase>();
					MatrixD parentWorldMatrix = myPositionComponentBase.WorldMatrix;
					myPositionComponentBase2.UpdateWorldMatrix(ref parentWorldMatrix);
				}
				if (base.Container.Entity.InScene && !child.InScene && insertIntoSceneIfNeeded)
				{
					child.OnAddedToScene(base.Container.Entity);
				}
			}
		}

		internal void UpdateNeedsWorldMatrix()
		{
			if (base.Entity.Parent == null)
			{
				return;
			}
			if (base.Entity.NeedsWorldMatrix && base.Entity.Parent.Hierarchy.m_children.Contains(this))
			{
				if (!base.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Contains(this))
				{
					base.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Add(this);
				}
			}
			else
			{
				base.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Remove(this);
			}
		}

		public void AddChildWithMatrix(IMyEntity child, ref Matrix childLocalMatrix, bool insertIntoSceneIfNeeded = true)
		{
			MyHierarchyComponentBase myHierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
			myHierarchyComponentBase.Parent = this;
			m_children.Add(myHierarchyComponentBase);
			child.WorldMatrix = (MatrixD)childLocalMatrix * base.Container.Get<MyPositionComponentBase>().WorldMatrix;
			if (child.NeedsWorldMatrix)
			{
				m_childrenNeedingWorldMatrix.Add(myHierarchyComponentBase);
			}
			if (base.Container.Entity.InScene && !child.InScene && insertIntoSceneIfNeeded)
			{
				child.OnAddedToScene(this);
			}
		}

		/// <summary>
		/// Removes the child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="preserveWorldPos">if set to <c>true</c> [preserve absolute position].</param>
		public void RemoveChild(IMyEntity child, bool preserveWorldPos = false)
		{
			MyHierarchyComponentBase myHierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
			MatrixD worldMatrix = default(MatrixD);
			if (preserveWorldPos)
			{
				worldMatrix = child.WorldMatrix;
			}
			if (child.InScene)
			{
				child.OnRemovedFromScene(this);
			}
			m_children.Remove(myHierarchyComponentBase);
			m_childrenNeedingWorldMatrix.Remove(myHierarchyComponentBase);
			if (preserveWorldPos)
			{
				child.WorldMatrix = worldMatrix;
			}
			myHierarchyComponentBase.Parent = null;
			this.OnChildRemoved.InvokeIfNotNull(child);
		}

		public void GetChildrenRecursive(HashSet<IMyEntity> result)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				MyHierarchyComponentBase myHierarchyComponentBase = Children[i];
				result.Add(myHierarchyComponentBase.Container.Entity);
				myHierarchyComponentBase.GetChildrenRecursive(result);
			}
		}

		public void RemoveByJN(MyHierarchyComponentBase childHierarchy)
		{
			m_children.Remove(childHierarchy);
			m_childrenNeedingWorldMatrix.Remove(childHierarchy);
		}

		public void Delete()
		{
			for (int num = m_children.Count - 1; num >= 0; num--)
			{
				m_children[num].Container.Entity.Delete();
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			if (m_parentContainer != null && !m_parentContainer.Entity.MarkedForClose)
			{
				m_parentContainer.ComponentAdded -= Container_ComponentAdded;
				m_parentContainer.ComponentRemoved -= Container_ComponentRemoved;
			}
			m_parent = null;
			m_parentContainer = null;
			base.OnBeforeRemovedFromContainer();
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			if (Children.Count == 0)
			{
				return null;
			}
			MyObjectBuilder_HierarchyComponentBase myObjectBuilder_HierarchyComponentBase = new MyObjectBuilder_HierarchyComponentBase();
			foreach (MyHierarchyComponentBase child in Children)
			{
				if (child.Entity.Save)
				{
					myObjectBuilder_HierarchyComponentBase.Children.Add(child.Entity.GetObjectBuilder(copy));
				}
			}
			if (myObjectBuilder_HierarchyComponentBase.Children.Count <= 0)
			{
				return null;
			}
			return myObjectBuilder_HierarchyComponentBase;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_HierarchyComponentBase myObjectBuilder_HierarchyComponentBase = builder as MyObjectBuilder_HierarchyComponentBase;
			if (myObjectBuilder_HierarchyComponentBase != null)
			{
				m_deserializedEntities.Clear();
				foreach (MyObjectBuilder_EntityBase child in myObjectBuilder_HierarchyComponentBase.Children)
				{
					if (!MyEntityIdentifier.ExistsById(child.EntityId))
					{
						MyEntity myEntity = MyEntity.MyEntitiesCreateFromObjectBuilderExtCallback(child, arg2: true);
						if (myEntity != null)
						{
							m_deserializedEntities.Add(myEntity);
						}
					}
				}
				foreach (MyEntity deserializedEntity in m_deserializedEntities)
				{
					AddChild(deserializedEntity, preserveWorldPos: true, insertIntoSceneIfNeeded: false);
				}
			}
		}
	}
}
