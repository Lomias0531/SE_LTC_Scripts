using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MySessionComponentTriggerSystem : MySessionComponentBase
	{
		private readonly Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>> m_triggers = new Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>>();

		private readonly FastResourceLock m_dictionaryLock = new FastResourceLock();

		public static MySessionComponentTriggerSystem Static;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public MyEntity GetTriggersEntity(string triggerName, out MyTriggerComponent foundTrigger)
		{
			foundTrigger = null;
			foreach (KeyValuePair<MyEntity, CachingHashSet<MyTriggerComponent>> trigger in m_triggers)
			{
				foreach (MyTriggerComponent item in trigger.Value)
				{
					MyAreaTriggerComponent myAreaTriggerComponent = item as MyAreaTriggerComponent;
					if (myAreaTriggerComponent != null && myAreaTriggerComponent.Name == triggerName)
					{
						foundTrigger = item;
						return trigger.Key;
					}
				}
			}
			return null;
		}

		public void AddTrigger(MyTriggerComponent trigger)
		{
			if (!Contains(trigger))
			{
				using (m_dictionaryLock.AcquireExclusiveUsing())
				{
					if (m_triggers.TryGetValue((MyEntity)trigger.Entity, out CachingHashSet<MyTriggerComponent> value))
					{
						value.Add(trigger);
					}
					else
					{
						m_triggers[(MyEntity)trigger.Entity] = new CachingHashSet<MyTriggerComponent>
						{
							trigger
						};
					}
				}
			}
		}

		public static void RemoveTrigger(MyEntity entity, MyTriggerComponent trigger)
		{
			if (Static != null)
			{
				Static.RemoveTriggerInternal(entity, trigger);
			}
		}

		private void RemoveTriggerInternal(MyEntity entity, MyTriggerComponent trigger)
		{
			using (m_dictionaryLock.AcquireExclusiveUsing())
			{
				if (m_triggers.TryGetValue(entity, out CachingHashSet<MyTriggerComponent> value))
				{
					value.Remove(trigger);
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					value.ApplyChanges();
					foreach (MyTriggerComponent item in value)
					{
						item.Update();
					}
				}
			}
		}

		public override void Draw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)
			{
				using (m_dictionaryLock.AcquireSharedUsing())
				{
					foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
					{
						foreach (MyTriggerComponent item in value)
						{
							item.DebugDraw();
						}
					}
				}
			}
		}

		public bool IsAnyTriggerActive(MyEntity entity)
		{
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				if (m_triggers.ContainsKey(entity))
				{
					foreach (MyTriggerComponent item in m_triggers[entity])
					{
						if (item.Enabled)
						{
							return true;
						}
					}
					return m_triggers[entity].Count == 0;
				}
			}
			return true;
		}

		public bool Contains(MyTriggerComponent trigger)
		{
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					if (value.Contains(trigger))
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<MyTriggerComponent> GetIntersectingTriggers(Vector3D position)
		{
			List<MyTriggerComponent> list = new List<MyTriggerComponent>();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					foreach (MyTriggerComponent item in value)
					{
						if (item.Contains(position))
						{
							list.Add(item);
						}
					}
				}
				return list;
			}
		}

		public List<MyTriggerComponent> GetAllTriggers()
		{
			List<MyTriggerComponent> list = new List<MyTriggerComponent>();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					foreach (MyTriggerComponent item in value)
					{
						list.Add(item);
					}
				}
				return list;
			}
		}
	}
}
