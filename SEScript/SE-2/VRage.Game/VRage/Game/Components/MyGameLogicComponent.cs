using System.Xml.Serialization;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
	public abstract class MyGameLogicComponent : MyEntityComponentBase, IMyGameLogicComponent
	{
		private MyEntityUpdateEnum m_needsUpdate;

		private bool m_entityUpdate;

		bool IMyGameLogicComponent.EntityUpdate
		{
			get
			{
				return m_entityUpdate;
			}
			set
			{
				m_entityUpdate = value;
			}
		}

		public MyEntityUpdateEnum NeedsUpdate
		{
			get
			{
				if (m_entityUpdate)
				{
					MyEntityUpdateEnum myEntityUpdateEnum = MyEntityUpdateEnum.NONE;
					if ((base.Entity.Flags & EntityFlags.NeedsUpdate) != 0)
					{
						myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_FRAME;
					}
					if ((base.Entity.Flags & EntityFlags.NeedsUpdate10) != 0)
					{
						myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
					}
					if ((base.Entity.Flags & EntityFlags.NeedsUpdate100) != 0)
					{
						myEntityUpdateEnum |= MyEntityUpdateEnum.EACH_100TH_FRAME;
					}
					if ((base.Entity.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != 0)
					{
						myEntityUpdateEnum |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					}
					return myEntityUpdateEnum;
				}
				return m_needsUpdate;
			}
			set
			{
				if (value == NeedsUpdate)
				{
					return;
				}
				if (m_entityUpdate)
				{
					if (base.Entity.InScene)
					{
						MyAPIGatewayShortcuts.UnregisterEntityUpdate(base.Entity, arg2: false);
					}
					base.Entity.Flags &= ~EntityFlags.NeedsUpdateBeforeNextFrame;
					base.Entity.Flags &= ~EntityFlags.NeedsUpdate;
					base.Entity.Flags &= ~EntityFlags.NeedsUpdate10;
					base.Entity.Flags &= ~EntityFlags.NeedsUpdate100;
					if ((value & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
					{
						base.Entity.Flags |= EntityFlags.NeedsUpdateBeforeNextFrame;
					}
					if ((value & MyEntityUpdateEnum.EACH_FRAME) != 0)
					{
						base.Entity.Flags |= EntityFlags.NeedsUpdate;
					}
					if ((value & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
					{
						base.Entity.Flags |= EntityFlags.NeedsUpdate10;
					}
					if ((value & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
					{
						base.Entity.Flags |= EntityFlags.NeedsUpdate100;
					}
					if (base.Entity.InScene)
					{
						MyAPIGatewayShortcuts.RegisterEntityUpdate(base.Entity);
					}
				}
				else
				{
					if (base.Entity.InScene)
					{
						MyGameLogic.ChangeUpdate(this, value);
					}
					m_needsUpdate = value;
				}
			}
		}

		[XmlIgnore]
		public bool Closed
		{
			get;
			protected set;
		}

		[XmlIgnore]
		public bool MarkedForClose
		{
			get;
			protected set;
		}

		public override string ComponentTypeDebugString => "Game Logic";

		void IMyGameLogicComponent.UpdateOnceBeforeFrame(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateOnceBeforeFrame();
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateBeforeSimulation();
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation10(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateBeforeSimulation10();
			}
		}

		void IMyGameLogicComponent.UpdateBeforeSimulation100(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateBeforeSimulation100();
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateAfterSimulation();
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation10(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateAfterSimulation10();
			}
		}

		void IMyGameLogicComponent.UpdateAfterSimulation100(bool entityUpdate)
		{
			if (entityUpdate == m_entityUpdate)
			{
				UpdateAfterSimulation100();
			}
		}

		void IMyGameLogicComponent.Close()
		{
			MyGameLogic.UnregisterForUpdate(this);
			Close();
		}

		void IMyGameLogicComponent.RegisterForUpdate()
		{
			MyGameLogic.RegisterForUpdate(this);
		}

		void IMyGameLogicComponent.UnregisterForUpdate()
		{
			MyGameLogic.UnregisterForUpdate(this);
		}

		public virtual void UpdateOnceBeforeFrame()
		{
		}

		public virtual void UpdateBeforeSimulation()
		{
		}

		public virtual void UpdateBeforeSimulation10()
		{
		}

		public virtual void UpdateBeforeSimulation100()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
		}

		public virtual void UpdateAfterSimulation10()
		{
		}

		public virtual void UpdateAfterSimulation100()
		{
		}

		public virtual void UpdatingStopped()
		{
		}

		public virtual void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
		}

		public virtual MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			return null;
		}

		public virtual void MarkForClose()
		{
		}

		public virtual void Close()
		{
		}
	}
}
