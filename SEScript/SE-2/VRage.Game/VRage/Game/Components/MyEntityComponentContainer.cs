using System;
using VRage.ModAPI;

namespace VRage.Game.Components
{
	public class MyEntityComponentContainer : MyComponentContainer, IMyComponentContainer
	{
		private IMyEntity m_entity;

		public IMyEntity Entity
		{
			get
			{
				return m_entity;
			}
			private set
			{
				m_entity = value;
			}
		}

		public event Action<Type, MyEntityComponentBase> ComponentAdded;

		public event Action<Type, MyEntityComponentBase> ComponentRemoved;

		public MyEntityComponentContainer(IMyEntity entity)
		{
			Entity = entity;
		}

		public override void Init(MyContainerDefinition definition)
		{
			if (definition.Flags.HasValue)
			{
				Entity.Flags |= definition.Flags.Value;
			}
		}

		protected override void OnComponentAdded(Type t, MyComponentBase component)
		{
			base.OnComponentAdded(t, component);
			MyEntityComponentBase myEntityComponentBase = component as MyEntityComponentBase;
			Action<Type, MyEntityComponentBase> componentAdded = this.ComponentAdded;
			if (componentAdded != null && myEntityComponentBase != null)
			{
				componentAdded(t, myEntityComponentBase);
			}
		}

		protected override void OnComponentRemoved(Type t, MyComponentBase component)
		{
			base.OnComponentRemoved(t, component);
			MyEntityComponentBase myEntityComponentBase = component as MyEntityComponentBase;
			if (myEntityComponentBase != null)
			{
				this.ComponentRemoved.InvokeIfNotNull(t, myEntityComponentBase);
			}
		}
	}
}
