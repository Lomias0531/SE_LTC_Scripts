using System;
using VRage.ModAPI;
using VRage.Network;

namespace VRage.Game.Components
{
	[GenerateActivator]
	public abstract class MyEntityComponentBase : MyComponentBase
	{
		public MyEntityComponentContainer Container => base.ContainerBase as MyEntityComponentContainer;

		public IMyEntity Entity => (base.ContainerBase as MyEntityComponentContainer)?.Entity;

		/// <summary>
		/// Name of the base component type for debug purposes (e.g.: "Position")
		/// </summary>
		public abstract string ComponentTypeDebugString
		{
			get;
		}

		public virtual bool AttachSyncToEntity => true;

		public static event Action<MyEntityComponentBase> OnAfterAddedToContainer;

		public event Action<MyEntityComponentBase> BeforeRemovedFromContainer;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			MyEntityComponentBase.OnAfterAddedToContainer?.Invoke(this);
			if (Entity != null)
			{
				IMySyncedEntity mySyncedEntity = Entity as IMySyncedEntity;
				if (mySyncedEntity != null && mySyncedEntity.SyncType != null && AttachSyncToEntity)
				{
					mySyncedEntity.SyncType.Append(this);
				}
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			this.BeforeRemovedFromContainer?.Invoke(this);
		}
	}
}
