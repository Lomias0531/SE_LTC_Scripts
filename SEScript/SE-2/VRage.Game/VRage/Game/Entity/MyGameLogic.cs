using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ModAPI;

namespace VRage.Game.Entity
{
	/// <summary>
	/// This is mostly a copy of the updating system present in MyEntities.
	///
	/// The old style of GameLogic component was tied to entity updates. This meant that
	/// when the entity removed one of its update flags, the component would stop updating 
	/// with no warning.
	///
	/// Here we update GameLogic components separately from the containing entity.
	/// </summary>
	public static class MyGameLogic
	{
		private static CachingList<MyGameLogicComponent> m_componentsForUpdateOnce = new CachingList<MyGameLogicComponent>();

		private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(1);

		private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate10 = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(10);

		private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate100 = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(100);

		/// <summary>
		/// Registers a component to the update system.
		/// Only use for first-time registration! If changing update flags, use ChangeUpdate!
		/// </summary>
		/// <param name="component"></param>
		public static void RegisterForUpdate(MyGameLogicComponent component)
		{
			if (!((IMyGameLogicComponent)component).EntityUpdate)
			{
				MyEntityUpdateEnum needsUpdate = component.NeedsUpdate;
				if ((needsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
				{
					m_componentsForUpdateOnce.Add(component);
				}
				if ((needsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
				{
					m_componentsForUpdate.List.Add(component);
				}
				if ((needsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
				{
					m_componentsForUpdate10.List.Add(component);
				}
				if ((needsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
				{
					m_componentsForUpdate100.List.Add(component);
				}
			}
		}

		/// <summary>
		/// Unregisters a component from the update system.
		/// Only use when disposing a component! If changing update flags, use ChangeUpdate!
		/// </summary>
		/// <param name="component"></param>
		/// <param name="immediate"></param>
		public static void UnregisterForUpdate(MyGameLogicComponent component)
		{
			MyEntityUpdateEnum needsUpdate = component.NeedsUpdate;
			if ((needsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
			{
				m_componentsForUpdateOnce.Remove(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
			{
				m_componentsForUpdate.List.Remove(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
			{
				m_componentsForUpdate10.List.Remove(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
			{
				m_componentsForUpdate100.List.Remove(component);
			}
		}

		/// <summary>
		/// Modifies a component's update flags without modifying lists the component is already in.
		/// Much more performant than Unregister/Register pattern!
		/// </summary>
		/// <param name="component"></param>
		/// <param name="newUpdate"></param>
		/// <param name="immediate"></param>
		public static void ChangeUpdate(MyGameLogicComponent component, MyEntityUpdateEnum newUpdate, bool immediate = false)
		{
			if (((IMyGameLogicComponent)component).EntityUpdate)
			{
				return;
			}
			MyEntityUpdateEnum needsUpdate = component.NeedsUpdate;
			if (needsUpdate == newUpdate)
			{
				return;
			}
			if ((needsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
			{
				if ((newUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) == 0)
				{
					if (immediate)
					{
						m_componentsForUpdateOnce.ApplyChanges();
					}
					m_componentsForUpdateOnce.Remove(component, immediate);
				}
			}
			else if ((newUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != 0)
			{
				m_componentsForUpdateOnce.Add(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
			{
				if ((newUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0)
				{
					if (immediate)
					{
						m_componentsForUpdate.List.ApplyChanges();
					}
					m_componentsForUpdate.List.Remove(component, immediate);
				}
			}
			else if ((newUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
			{
				m_componentsForUpdate.List.Add(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
			{
				if ((newUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) == 0)
				{
					if (immediate)
					{
						m_componentsForUpdate10.List.ApplyChanges();
					}
					m_componentsForUpdate10.List.Remove(component, immediate);
				}
			}
			else if ((newUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != 0)
			{
				m_componentsForUpdate10.List.Add(component);
			}
			if ((needsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
			{
				if ((newUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) == 0)
				{
					if (immediate)
					{
						m_componentsForUpdate100.List.ApplyChanges();
					}
					m_componentsForUpdate100.List.Remove(component, immediate);
				}
			}
			else if ((newUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != 0)
			{
				m_componentsForUpdate100.List.Add(component);
			}
		}

		public static void UpdateOnceBeforeFrame()
		{
			m_componentsForUpdateOnce.ApplyChanges();
			foreach (MyGameLogicComponent item in m_componentsForUpdateOnce)
			{
				item.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				if (!item.MarkedForClose && !item.Closed)
				{
					((IMyGameLogicComponent)item).UpdateOnceBeforeFrame(entityUpdate: false);
				}
			}
		}

		public static void UpdateBeforeSimulation()
		{
			UpdateOnceBeforeFrame();
			m_componentsForUpdate.List.ApplyChanges();
			m_componentsForUpdate.Update();
			m_componentsForUpdate.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateBeforeSimulation(entityUpdate: false);
				}
			});
			m_componentsForUpdate10.List.ApplyChanges();
			m_componentsForUpdate10.Update();
			m_componentsForUpdate10.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateBeforeSimulation10(entityUpdate: false);
				}
			});
			m_componentsForUpdate100.List.ApplyChanges();
			m_componentsForUpdate100.Update();
			m_componentsForUpdate100.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateBeforeSimulation100(entityUpdate: false);
				}
			});
		}

		public static void UpdateAfterSimulation()
		{
			m_componentsForUpdate.List.ApplyChanges();
			m_componentsForUpdate.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateAfterSimulation(entityUpdate: false);
				}
			});
			m_componentsForUpdate10.List.ApplyChanges();
			m_componentsForUpdate10.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateAfterSimulation10(entityUpdate: false);
				}
			});
			m_componentsForUpdate100.List.ApplyChanges();
			m_componentsForUpdate100.Iterate(delegate(MyGameLogicComponent c)
			{
				if (!c.MarkedForClose && !c.Closed)
				{
					((IMyGameLogicComponent)c).UpdateAfterSimulation100(entityUpdate: false);
				}
			});
		}

		public static void UpdatingStopped()
		{
			foreach (MyGameLogicComponent item in m_componentsForUpdate.List)
			{
				if (!item.MarkedForClose && !item.Closed)
				{
					item.UpdatingStopped();
				}
			}
		}
	}
}
