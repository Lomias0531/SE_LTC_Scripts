using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Components
{
	public static class MyEntityContainerEventExtensions
	{
		public class EntityEventParams
		{
		}

		public class ControlAcquiredParams : EntityEventParams
		{
			public MyEntity Owner;

			public ControlAcquiredParams(MyEntity owner)
			{
				Owner = owner;
			}
		}

		public class ControlReleasedParams : EntityEventParams
		{
			public MyEntity Owner;

			public ControlReleasedParams(MyEntity owner)
			{
				Owner = owner;
			}
		}

		public class ModelChangedParams : EntityEventParams
		{
			public Vector3 Size;

			public float Mass;

			public float Volume;

			public string Model;

			public string DisplayName;

			public string[] Icons;

			public ModelChangedParams(string model, Vector3 size, float mass, float volume, string displayName, string[] icons)
			{
				Model = model;
				Size = size;
				Mass = mass;
				Volume = volume;
				DisplayName = displayName;
				Icons = icons;
			}
		}

		public class InventoryChangedParams : EntityEventParams
		{
			public uint ItemId;

			public float Amount;

			public MyInventoryBase Inventory;

			public InventoryChangedParams(uint itemId, MyInventoryBase inventory, float amount)
			{
				ItemId = itemId;
				Inventory = inventory;
				Amount = amount;
			}
		}

		public class HitParams : EntityEventParams
		{
			public MyStringHash HitEntity;

			public MyStringHash HitAction;

			public HitParams(MyStringHash hitAction, MyStringHash hitEntity)
			{
				HitEntity = hitEntity;
				HitAction = hitAction;
			}
		}

		public delegate void EntityEventHandler(EntityEventParams eventParams);

		private class RegisteredComponent
		{
			public MyComponentBase Component;

			public EntityEventHandler Handler;

			public RegisteredComponent(MyComponentBase component, EntityEventHandler handler)
			{
				Component = component;
				Handler = handler;
			}
		}

		private class RegisteredEvents : Dictionary<MyStringHash, List<RegisteredComponent>>
		{
			public RegisteredEvents(MyStringHash eventType, MyComponentBase component, EntityEventHandler handler)
			{
				base[eventType] = new List<RegisteredComponent>();
				base[eventType].Add(new RegisteredComponent(component, handler));
			}
		}

		private static Dictionary<long, RegisteredEvents> RegisteredListeners = new Dictionary<long, RegisteredEvents>();

		private static Dictionary<MyComponentBase, List<long>> ExternalListeners = new Dictionary<MyComponentBase, List<long>>();

		private static HashSet<long> ExternalyListenedEntities = new HashSet<long>();

		private static List<RegisteredComponent> m_tmpList = new List<RegisteredComponent>();

		private static List<MyComponentBase> m_tmpCompList = new List<MyComponentBase>();

		private static bool ProcessingEvents;

		private static bool HasPostponedOperations;

		private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>> PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>>();

		private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>> PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();

		private static List<long> PostPonedRegisteredListenersRemoval = new List<long>();

		private static int m_debugCounter;

		public static void InitEntityEvents()
		{
			RegisteredListeners = new Dictionary<long, RegisteredEvents>();
			ExternalListeners = new Dictionary<MyComponentBase, List<long>>();
			ExternalyListenedEntities = new HashSet<long>();
			PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>>();
			PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();
			ProcessingEvents = false;
			HasPostponedOperations = false;
		}

		public static void RegisterForEntityEvent(this MyEntityComponentBase component, MyStringHash eventType, EntityEventHandler handler)
		{
			if (ProcessingEvents)
			{
				AddPostponedRegistration(component, component.Entity as MyEntity, eventType, handler);
			}
			else
			{
				if (component.Entity == null)
				{
					return;
				}
				component.BeforeRemovedFromContainer += RegisteredComponentBeforeRemovedFromContainer;
				component.Entity.OnClose += RegisteredEntityOnClose;
				if (RegisteredListeners.ContainsKey(component.Entity.EntityId))
				{
					RegisteredEvents registeredEvents = RegisteredListeners[component.Entity.EntityId];
					if (registeredEvents.ContainsKey(eventType))
					{
						if (registeredEvents[eventType].Find((RegisteredComponent x) => x.Handler == handler) == null)
						{
							registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
						}
					}
					else
					{
						registeredEvents[eventType] = new List<RegisteredComponent>();
						registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
					}
				}
				else
				{
					RegisteredListeners[component.Entity.EntityId] = new RegisteredEvents(eventType, component, handler);
				}
			}
		}

		public static void RegisterForEntityEvent(this MyEntityComponentBase component, MyEntity entity, MyStringHash eventType, EntityEventHandler handler)
		{
			if (ProcessingEvents)
			{
				AddPostponedRegistration(component, entity, eventType, handler);
			}
			else if (component.Entity == entity)
			{
				component.RegisterForEntityEvent(eventType, handler);
			}
			else
			{
				if (entity == null)
				{
					return;
				}
				component.BeforeRemovedFromContainer += RegisteredComponentBeforeRemovedFromContainer;
				entity.OnClose += RegisteredEntityOnClose;
				if (RegisteredListeners.ContainsKey(entity.EntityId))
				{
					RegisteredEvents registeredEvents = RegisteredListeners[entity.EntityId];
					if (registeredEvents.ContainsKey(eventType))
					{
						if (registeredEvents[eventType].Find((RegisteredComponent x) => x.Handler == handler) == null)
						{
							registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
						}
					}
					else
					{
						registeredEvents[eventType] = new List<RegisteredComponent>();
						registeredEvents[eventType].Add(new RegisteredComponent(component, handler));
					}
				}
				else
				{
					RegisteredListeners[entity.EntityId] = new RegisteredEvents(eventType, component, handler);
				}
				if (ExternalListeners.ContainsKey(component) && !ExternalListeners[component].Contains(entity.EntityId))
				{
					ExternalListeners[component].Add(entity.EntityId);
				}
				else
				{
					ExternalListeners[component] = new List<long>
					{
						entity.EntityId
					};
				}
				ExternalyListenedEntities.Add(entity.EntityId);
			}
		}

		public static void UnregisterForEntityEvent(this MyEntityComponentBase component, MyEntity entity, MyStringHash eventType)
		{
			if (ProcessingEvents)
			{
				AddPostponedUnregistration(component, entity, eventType);
			}
			else
			{
				if (entity == null)
				{
					return;
				}
				bool flag = true;
				if (RegisteredListeners.ContainsKey(entity.EntityId))
				{
					if (RegisteredListeners[entity.EntityId].ContainsKey(eventType))
					{
						RegisteredListeners[entity.EntityId][eventType].RemoveAll((RegisteredComponent x) => x.Component == component);
						if (RegisteredListeners[entity.EntityId][eventType].Count == 0)
						{
							RegisteredListeners[entity.EntityId].Remove(eventType);
						}
					}
					if (RegisteredListeners[entity.EntityId].Count == 0)
					{
						RegisteredListeners.Remove(entity.EntityId);
						ExternalyListenedEntities.Remove(entity.EntityId);
						flag = false;
					}
				}
				if (ExternalListeners.ContainsKey(component) && ExternalListeners[component].Contains(entity.EntityId))
				{
					ExternalListeners[component].Remove(entity.EntityId);
					if (ExternalListeners[component].Count == 0)
					{
						ExternalListeners.Remove(component);
					}
				}
				if (!flag)
				{
					entity.OnClose -= RegisteredEntityOnClose;
				}
			}
		}

		private static void RegisteredEntityOnClose(IMyEntity entity)
		{
			entity.OnClose -= RegisteredEntityOnClose;
			if (RegisteredListeners.ContainsKey(entity.EntityId))
			{
				if (ProcessingEvents)
				{
					AddPostponedListenerRemoval(entity.EntityId);
				}
				else
				{
					RegisteredListeners.Remove(entity.EntityId);
				}
			}
			if (ExternalyListenedEntities.Contains(entity.EntityId))
			{
				ExternalyListenedEntities.Remove(entity.EntityId);
				m_tmpCompList.Clear();
				foreach (KeyValuePair<MyComponentBase, List<long>> externalListener in ExternalListeners)
				{
					externalListener.Value.Remove(entity.EntityId);
					if (externalListener.Value.Count == 0)
					{
						m_tmpCompList.Add(externalListener.Key);
					}
				}
				foreach (MyComponentBase tmpComp in m_tmpCompList)
				{
					ExternalListeners.Remove(tmpComp);
				}
			}
		}

		private static void RegisteredComponentBeforeRemovedFromContainer(MyEntityComponentBase component)
		{
			component.BeforeRemovedFromContainer -= RegisteredComponentBeforeRemovedFromContainer;
			if (component.Entity != null)
			{
				if (RegisteredListeners.ContainsKey(component.Entity.EntityId))
				{
					m_tmpList.Clear();
					foreach (KeyValuePair<MyStringHash, List<RegisteredComponent>> item in RegisteredListeners[component.Entity.EntityId])
					{
						item.Value.RemoveAll((RegisteredComponent x) => x.Component == component);
					}
				}
				if (ExternalListeners.ContainsKey(component))
				{
					foreach (long item2 in ExternalListeners[component])
					{
						if (RegisteredListeners.ContainsKey(item2))
						{
							foreach (KeyValuePair<MyStringHash, List<RegisteredComponent>> item3 in RegisteredListeners[item2])
							{
								item3.Value.RemoveAll((RegisteredComponent x) => x.Component == component);
							}
						}
					}
					ExternalListeners.Remove(component);
				}
			}
		}

		public static void RaiseEntityEvent(this MyEntity entity, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (entity.Components != null)
			{
				InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
			}
		}

		public static void RaiseEntityEventOn(MyEntity entity, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (entity.Components != null)
			{
				InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
			}
		}

		public static void RaiseEntityEvent(this MyEntityComponentBase component, MyStringHash eventType, EntityEventParams eventParams)
		{
			if (component.Entity != null)
			{
				InvokeEventOnListeners(component.Entity.EntityId, eventType, eventParams);
			}
		}

		private static void InvokeEventOnListeners(long entityId, MyStringHash eventType, EntityEventParams eventParams)
		{
			bool processingEvents = ProcessingEvents;
			if (processingEvents)
			{
				m_debugCounter++;
			}
			if (m_debugCounter <= 5)
			{
				ProcessingEvents = true;
				if (RegisteredListeners.ContainsKey(entityId) && RegisteredListeners[entityId].ContainsKey(eventType))
				{
					foreach (RegisteredComponent item in RegisteredListeners[entityId][eventType])
					{
						try
						{
							item.Handler(eventParams);
						}
						catch (Exception)
						{
						}
					}
				}
				ProcessingEvents = processingEvents;
				if (!ProcessingEvents)
				{
					m_debugCounter = 0;
				}
				if (HasPostponedOperations && !ProcessingEvents)
				{
					ProcessPostponedRegistrations();
				}
			}
		}

		private static void ProcessPostponedRegistrations()
		{
			foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler> item in PostponedRegistration)
			{
				item.Item1.RegisterForEntityEvent(item.Item2, item.Item3, item.Item4);
			}
			foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash> item2 in PostponedUnregistration)
			{
				item2.Item1.UnregisterForEntityEvent(item2.Item2, item2.Item3);
			}
			foreach (long item3 in PostPonedRegisteredListenersRemoval)
			{
				RegisteredListeners.Remove(item3);
			}
			PostponedRegistration.Clear();
			PostponedUnregistration.Clear();
			PostPonedRegisteredListenersRemoval.Clear();
			HasPostponedOperations = false;
		}

		private static void AddPostponedRegistration(MyEntityComponentBase component, MyEntity entity, MyStringHash eventType, EntityEventHandler handler)
		{
			PostponedRegistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash, EntityEventHandler>(component, entity, eventType, handler));
			HasPostponedOperations = true;
		}

		private static void AddPostponedUnregistration(MyEntityComponentBase component, MyEntity entity, MyStringHash eventType)
		{
			PostponedUnregistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash>(component, entity, eventType));
			HasPostponedOperations = true;
		}

		private static void AddPostponedListenerRemoval(long id)
		{
			PostPonedRegisteredListenersRemoval.Add(id);
			HasPostponedOperations = true;
		}
	}
}
