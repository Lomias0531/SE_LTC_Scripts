using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Replication;
using VRageMath;

namespace Sandbox.Game.Replication
{
	public abstract class MyExternalReplicable : IMyReplicable, IMyNetObject, IMyEventOwner
	{
		protected IMyStateGroup m_physicsSync;

		protected IMyReplicable m_parent;

		private static readonly MyConcurrentDictionary<object, MyExternalReplicable> m_objectExternalReplicables = new MyConcurrentDictionary<object, MyExternalReplicable>();

		public IMyStateGroup PhysicsSync => m_physicsSync;

		public virtual string InstanceName => "";

		public virtual bool IsReadyForReplication
		{
			get
			{
				if (HasToBeChild)
				{
					if (GetParent() != null)
					{
						return GetParent().IsReadyForReplication;
					}
					return false;
				}
				return true;
			}
		}

		public virtual Dictionary<IMyReplicable, Action> ReadyForReplicationAction => GetParent()?.ReadyForReplicationAction;

		public virtual bool PriorityUpdate => true;

		public virtual bool IncludeInIslands => true;

		public abstract bool IsValid
		{
			get;
		}

		public abstract bool HasToBeChild
		{
			get;
		}

		public virtual bool IsSpatial => !HasToBeChild;

		public Action<IMyReplicable> OnAABBChanged
		{
			get;
			set;
		}

		public static event Action<MyExternalReplicable> Destroyed;

		public static MyExternalReplicable FindByObject(object obj)
		{
			return m_objectExternalReplicables.GetValueOrDefault(obj, null);
		}

		public virtual void Hook(object obj)
		{
			m_objectExternalReplicables[obj] = this;
		}

		public virtual void OnServerReplicate()
		{
		}

		public virtual bool ShouldReplicate(MyClientInfo client)
		{
			return true;
		}

		protected virtual void RaiseDestroyed()
		{
			ReadyForReplicationAction?.Remove(this);
			object instance = GetInstance();
			if (instance != null)
			{
				m_objectExternalReplicables.Remove(instance);
			}
			MyExternalReplicable.Destroyed?.Invoke(this);
		}

		protected abstract object GetInstance();

		protected abstract void OnHook();

		public abstract IMyReplicable GetParent();

		public abstract bool OnSave(BitStream stream, Endpoint clientEndpoint);

		public abstract void OnLoad(BitStream stream, Action<bool> loadingDoneHandler);

		public abstract void Reload(Action<bool> loadingDoneHandler);

		public abstract void OnDestroyClient();

		public abstract void OnRemovedFromReplication();

		public abstract void GetStateGroups(List<IMyStateGroup> resultList);

		public abstract BoundingBoxD GetAABB();

		public virtual HashSet<IMyReplicable> GetDependencies(bool forPlayer)
		{
			return null;
		}

		public virtual HashSet<IMyReplicable> GetPhysicalDependencies(MyTimeSpan timeStamp, MyReplicablesBase replicables)
		{
			return null;
		}

		public abstract bool CheckConsistency();

		public virtual ValidationResult HasRights(EndpointId endpointId, ValidationType validationFlags)
		{
			return ValidationResult.Passed;
		}
	}
	public abstract class MyExternalReplicable<T> : MyExternalReplicable
	{
		public T Instance
		{
			get;
			private set;
		}

		public override string InstanceName
		{
			get
			{
				if (Instance == null)
				{
					return "";
				}
				return Instance.ToString();
			}
		}

		public override bool IsReadyForReplication
		{
			get
			{
				MyEntity myEntity = Instance as MyEntity;
				MyEntityComponentBase myEntityComponentBase = Instance as MyEntityComponentBase;
				if (myEntity != null)
				{
					return myEntity.IsReadyForReplication;
				}
				if (myEntityComponentBase != null)
				{
					return ((MyEntity)myEntityComponentBase.Entity).IsReadyForReplication;
				}
				return base.IsReadyForReplication;
			}
		}

		public override Dictionary<IMyReplicable, Action> ReadyForReplicationAction
		{
			get
			{
				MyEntity myEntity = Instance as MyEntity;
				MyEntityComponentBase myEntityComponentBase = Instance as MyEntityComponentBase;
				if (myEntity != null)
				{
					return myEntity.ReadyForReplicationAction;
				}
				if (myEntityComponentBase != null)
				{
					if ((MyEntity)myEntityComponentBase.Entity != null)
					{
						return ((MyEntity)myEntityComponentBase.Entity).ReadyForReplicationAction;
					}
					return base.ReadyForReplicationAction;
				}
				return base.ReadyForReplicationAction;
			}
		}

		protected abstract void OnLoad(BitStream stream, Action<T> loadingDoneHandler);

		protected sealed override object GetInstance()
		{
			return Instance;
		}

		protected override void RaiseDestroyed()
		{
			base.RaiseDestroyed();
		}

		protected void OnLoadDone(T instance, Action<bool> loadingDoneHandler)
		{
			if (instance != null)
			{
				HookInternal(instance);
				loadingDoneHandler(obj: true);
			}
			else
			{
				loadingDoneHandler(obj: false);
			}
		}

		public sealed override void OnLoad(BitStream stream, Action<bool> loadingDoneHandler)
		{
			OnLoad(stream, delegate(T instance)
			{
				OnLoadDone(instance, loadingDoneHandler);
			});
		}

		public sealed override void Reload(Action<bool> loadingDoneHandler)
		{
			OnLoad(null, delegate(T instance)
			{
				OnLoadDone(instance, loadingDoneHandler);
			});
		}

		public sealed override void OnRemovedFromReplication()
		{
			Instance = default(T);
		}

		public sealed override void Hook(object obj)
		{
			HookInternal(obj);
		}

		private void HookInternal(object obj)
		{
			Instance = (T)obj;
			base.Hook(obj);
			OnHook();
		}

		public sealed override bool CheckConsistency()
		{
			return Instance != null;
		}
	}
}
