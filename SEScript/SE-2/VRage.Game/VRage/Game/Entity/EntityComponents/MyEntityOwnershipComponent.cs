using System;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game.Entity.EntityComponents
{
	[MyComponentType(typeof(MyEntityOwnershipComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_EntityOwnershipComponent), true)]
	public class MyEntityOwnershipComponent : MyEntityComponentBase
	{
		private class VRage_Game_Entity_EntityComponents_MyEntityOwnershipComponent_003C_003EActor : IActivator, IActivator<MyEntityOwnershipComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityOwnershipComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityOwnershipComponent CreateInstance()
			{
				return new MyEntityOwnershipComponent();
			}

			MyEntityOwnershipComponent IActivator<MyEntityOwnershipComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private long m_ownerId;

		private MyOwnershipShareModeEnum m_shareMode = MyOwnershipShareModeEnum.All;

		public Action<long, long> OwnerChanged;

		public Action<MyOwnershipShareModeEnum> ShareModeChanged;

		public long OwnerId
		{
			get
			{
				return m_ownerId;
			}
			set
			{
				if (m_ownerId != value && OwnerChanged != null)
				{
					OwnerChanged(m_ownerId, value);
				}
				m_ownerId = value;
			}
		}

		public MyOwnershipShareModeEnum ShareMode
		{
			get
			{
				return m_shareMode;
			}
			set
			{
				if (m_shareMode != value && ShareModeChanged != null)
				{
					ShareModeChanged(value);
				}
				m_shareMode = value;
			}
		}

		public override string ComponentTypeDebugString => GetType().Name;

		public override bool IsSerialized()
		{
			return true;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_EntityOwnershipComponent obj = base.Serialize() as MyObjectBuilder_EntityOwnershipComponent;
			obj.OwnerId = m_ownerId;
			obj.ShareMode = m_shareMode;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_EntityOwnershipComponent myObjectBuilder_EntityOwnershipComponent = builder as MyObjectBuilder_EntityOwnershipComponent;
			m_ownerId = myObjectBuilder_EntityOwnershipComponent.OwnerId;
			m_shareMode = myObjectBuilder_EntityOwnershipComponent.ShareMode;
		}
	}
}
