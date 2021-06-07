using Sandbox.Game.Entities;
using System;
using System.Text;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Components
{
	public class MyEntityGameLogic : MyGameLogicComponent
	{
		private class Sandbox_Game_Components_MyEntityGameLogic_003C_003EActor : IActivator, IActivator<MyEntityGameLogic>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityGameLogic();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityGameLogic CreateInstance()
			{
				return new MyEntityGameLogic();
			}

			MyEntityGameLogic IActivator<MyEntityGameLogic>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected MyEntity m_entity;

		public MyGameLogicComponent GameLogic
		{
			get;
			set;
		}

		public event Action<MyEntity> OnMarkForClose;

		public event Action<MyEntity> OnClose;

		public event Action<MyEntity> OnClosing;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_entity = (base.Container.Entity as MyEntity);
		}

		public MyEntityGameLogic()
		{
			GameLogic = new MyNullGameLogicComponent();
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			if (objectBuilder != null)
			{
				if (objectBuilder.PositionAndOrientation.HasValue)
				{
					MyPositionAndOrientation value = objectBuilder.PositionAndOrientation.Value;
					MatrixD worldMatrix = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
					base.Container.Entity.PositionComp.SetWorldMatrix(worldMatrix);
				}
				if (objectBuilder.EntityId != 0L)
				{
					base.Container.Entity.EntityId = objectBuilder.EntityId;
				}
				base.Container.Entity.Name = objectBuilder.Name;
				base.Container.Entity.Render.PersistentFlags = objectBuilder.PersistentFlags;
			}
			AllocateEntityID();
			base.Container.Entity.InScene = false;
			MyEntities.SetEntityName(m_entity, possibleRename: false);
			if (m_entity.SyncFlag)
			{
				m_entity.CreateSync();
			}
			GameLogic.Init(objectBuilder);
		}

		public void Init(StringBuilder displayName, string model, MyEntity parentObject, float? scale, string modelCollision = null)
		{
			base.Container.Entity.DisplayName = displayName?.ToString();
			m_entity.RefreshModels(model, modelCollision);
			parentObject?.Hierarchy.AddChild(base.Container.Entity, preserveWorldPos: false, insertIntoSceneIfNeeded: false);
			base.Container.Entity.PositionComp.Scale = scale;
			AllocateEntityID();
		}

		private void AllocateEntityID()
		{
			if (base.Container.Entity.EntityId == 0L && !MyEntityIdentifier.AllocationSuspended)
			{
				base.Container.Entity.EntityId = MyEntityIdentifier.AllocateId();
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyEntityFactory.CreateObjectBuilder(base.Container.Entity as MyEntity);
			myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation
			{
				Position = base.Container.Entity.PositionComp.GetPosition(),
				Up = (Vector3)base.Container.Entity.WorldMatrix.Up,
				Forward = (Vector3)base.Container.Entity.WorldMatrix.Forward
			};
			myObjectBuilder_EntityBase.EntityId = base.Container.Entity.EntityId;
			myObjectBuilder_EntityBase.Name = base.Container.Entity.Name;
			myObjectBuilder_EntityBase.PersistentFlags = base.Container.Entity.Render.PersistentFlags;
			return myObjectBuilder_EntityBase;
		}

		public override void UpdateOnceBeforeFrame()
		{
			GameLogic.UpdateOnceBeforeFrame();
		}

		public override void UpdateBeforeSimulation()
		{
			GameLogic.UpdateBeforeSimulation();
		}

		public override void UpdateAfterSimulation()
		{
			GameLogic.UpdateAfterSimulation();
		}

		public override void UpdatingStopped()
		{
		}

		public override void UpdateBeforeSimulation10()
		{
			GameLogic.UpdateBeforeSimulation10();
		}

		public override void UpdateAfterSimulation10()
		{
			GameLogic.UpdateAfterSimulation10();
		}

		public override void UpdateBeforeSimulation100()
		{
			GameLogic.UpdateBeforeSimulation100();
		}

		public override void UpdateAfterSimulation100()
		{
			GameLogic.UpdateAfterSimulation100();
		}

		public override void MarkForClose()
		{
			base.MarkedForClose = true;
			MyEntities.Close(m_entity);
			GameLogic.MarkForClose();
			this.OnMarkForClose?.Invoke(m_entity);
		}

		public override void Close()
		{
			((IMyGameLogicComponent)GameLogic).Close();
			MyHierarchyComponent<MyEntity> hierarchy = m_entity.Hierarchy;
			while (hierarchy != null && hierarchy.Children.Count > 0)
			{
				MyHierarchyComponentBase myHierarchyComponentBase = hierarchy.Children[hierarchy.Children.Count - 1];
				myHierarchyComponentBase.Container.Entity.Close();
				hierarchy.RemoveByJN(myHierarchyComponentBase);
			}
			CallAndClearOnClosing();
			MyEntities.RemoveName(m_entity);
			MyEntities.RemoveFromClosedEntities(m_entity);
			if (m_entity.Physics != null)
			{
				m_entity.Physics.Close();
				m_entity.Physics = null;
				m_entity.RaisePhysicsChanged();
			}
			MyEntities.UnregisterForUpdate(m_entity, immediate: true);
			MyEntities.UnregisterForDraw(m_entity);
			if (hierarchy == null || hierarchy.Parent == null)
			{
				MyEntities.Remove(m_entity);
			}
			else
			{
				m_entity.Parent.Hierarchy.RemoveByJN(hierarchy);
				if (m_entity.Parent.InScene)
				{
					m_entity.OnRemovedFromScene(m_entity);
				}
				MyEntities.RaiseEntityRemove(m_entity);
			}
			if (m_entity.EntityId != 0L)
			{
				MyEntityIdentifier.RemoveEntity(m_entity.EntityId);
			}
			CallAndClearOnClose();
			base.Closed = true;
		}

		protected void CallAndClearOnClose()
		{
			if (this.OnClose != null)
			{
				this.OnClose(m_entity);
			}
			this.OnClose = null;
		}

		protected void CallAndClearOnClosing()
		{
			if (this.OnClosing != null)
			{
				this.OnClosing(m_entity);
			}
			this.OnClosing = null;
		}
	}
}
