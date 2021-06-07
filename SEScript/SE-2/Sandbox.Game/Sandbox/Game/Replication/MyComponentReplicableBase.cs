using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
	public abstract class MyComponentReplicableBase<T> : MyExternalReplicableEvent<T> where T : MyEntityComponentBase, IMyEventProxy
	{
		private readonly Action<MyEntity> m_raiseDestroyedHandler;

		public override bool HasToBeChild => true;

		protected MyComponentReplicableBase()
		{
			m_raiseDestroyedHandler = delegate
			{
				RaiseDestroyed();
			};
		}

		protected override void OnHook()
		{
			base.OnHook();
			if (base.Instance != null)
			{
				((MyEntity)base.Instance.Entity).OnClose += m_raiseDestroyedHandler;
				base.Instance.BeforeRemovedFromContainer += delegate
				{
					OnComponentRemovedFromContainer();
				};
			}
		}

		private void OnComponentRemovedFromContainer()
		{
			if (base.Instance != null && base.Instance.Entity != null)
			{
				((MyEntity)base.Instance.Entity).OnClose -= m_raiseDestroyedHandler;
				RaiseDestroyed();
			}
		}

		protected override void OnLoad(BitStream stream, Action<T> loadingDoneHandler)
		{
			MySerializer.CreateAndRead(stream, out long entityId);
			MyEntities.CallAsync(delegate
			{
				LoadAsync(entityId, loadingDoneHandler);
			});
		}

		private void LoadAsync(long entityId, Action<T> loadingDoneHandler)
		{
			Type componentType = MyComponentTypeFactory.GetComponentType(typeof(T));
			MyEntity entity = null;
			MyComponentBase component = null;
			if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				entity.Components.TryGet(componentType, out component);
			}
			loadingDoneHandler(component as T);
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			long value = base.Instance.Entity.EntityId;
			MySerializer.Write(stream, ref value);
			return true;
		}

		public override void OnDestroyClient()
		{
			if (base.Instance != null && base.Instance.Entity != null)
			{
				((MyEntity)base.Instance.Entity).OnClose -= m_raiseDestroyedHandler;
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
		}

		public override BoundingBoxD GetAABB()
		{
			return BoundingBoxD.CreateInvalid();
		}
	}
}
