using VRage.Game.Components;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Character.Components
{
	public abstract class MyCharacterComponent : MyEntityComponentBase
	{
		private bool m_needsUpdateAfterSimulation;

		private bool m_needsUpdateSimulation;

		private bool m_needsUpdateAfterSimulation10;

		private bool m_needsUpdateBeforeSimulation100;

		private bool m_needsUpdateBeforeSimulation;

		public bool NeedsUpdateAfterSimulation
		{
			get
			{
				return m_needsUpdateAfterSimulation;
			}
			set
			{
				m_needsUpdateAfterSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public bool NeedsUpdateSimulation
		{
			get
			{
				return m_needsUpdateSimulation;
			}
			set
			{
				m_needsUpdateSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.SIMULATE;
			}
		}

		public bool NeedsUpdateAfterSimulation10
		{
			get
			{
				return m_needsUpdateAfterSimulation10;
			}
			set
			{
				m_needsUpdateAfterSimulation10 = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		public bool NeedsUpdateBeforeSimulation100
		{
			get
			{
				return m_needsUpdateBeforeSimulation100;
			}
			set
			{
				m_needsUpdateBeforeSimulation100 = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		public bool NeedsUpdateBeforeSimulation
		{
			get
			{
				return m_needsUpdateBeforeSimulation;
			}
			set
			{
				m_needsUpdateBeforeSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public MyCharacter Character => (MyCharacter)base.Entity;

		public override string ComponentTypeDebugString => "Character Component";

		public virtual void UpdateAfterSimulation10()
		{
		}

		public virtual void UpdateBeforeSimulation()
		{
		}

		public virtual void Simulate()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
		}

		public virtual void UpdateBeforeSimulation100()
		{
		}

		public virtual void OnCharacterDead()
		{
		}
	}
}
