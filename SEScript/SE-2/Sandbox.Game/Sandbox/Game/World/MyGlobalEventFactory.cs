using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.World
{
	public class MyGlobalEventFactory
	{
		private static readonly Dictionary<MyDefinitionId, MethodInfo> m_typesToHandlers;

		private static MyObjectFactory<MyEventTypeAttribute, MyGlobalEventBase> m_globalEventFactory;

		static MyGlobalEventFactory()
		{
			m_typesToHandlers = new Dictionary<MyDefinitionId, MethodInfo>();
			m_globalEventFactory = new MyObjectFactory<MyEventTypeAttribute, MyGlobalEventBase>();
			RegisterEventTypesAndHandlers(Assembly.GetAssembly(typeof(MyGlobalEventBase)));
			RegisterEventTypesAndHandlers(MyPlugins.GameAssembly);
			RegisterEventTypesAndHandlers(MyPlugins.SandboxAssembly);
		}

		private static void RegisterEventTypesAndHandlers(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				MethodInfo[] methods = types[i].GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					if (!methodInfo.IsPublic || !methodInfo.IsStatic)
					{
						continue;
					}
					object[] customAttributes = methodInfo.GetCustomAttributes(typeof(MyGlobalEventHandler), inherit: false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						object[] array = customAttributes;
						for (int k = 0; k < array.Length; k++)
						{
							RegisterHandler(((MyGlobalEventHandler)array[k]).EventDefinitionId, methodInfo);
						}
					}
				}
			}
			m_globalEventFactory.RegisterFromAssembly(assembly);
		}

		private static void RegisterHandler(MyDefinitionId eventDefinitionId, MethodInfo handler)
		{
			m_typesToHandlers[eventDefinitionId] = handler;
		}

		public static MethodInfo GetEventHandler(MyDefinitionId eventDefinitionId)
		{
			MethodInfo value = null;
			m_typesToHandlers.TryGetValue(eventDefinitionId, out value);
			return value;
		}

		public static EventDataType CreateEvent<EventDataType>(MyDefinitionId id) where EventDataType : MyGlobalEventBase, new()
		{
			MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(id);
			if (eventDefinition == null)
			{
				return null;
			}
			EventDataType val = new EventDataType();
			val.InitFromDefinition(eventDefinition);
			return val;
		}

		public static MyGlobalEventBase CreateEvent(MyDefinitionId id)
		{
			MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(id);
			if (eventDefinition == null)
			{
				return null;
			}
			MyGlobalEventBase myGlobalEventBase = m_globalEventFactory.CreateInstance(id.TypeId);
			if (myGlobalEventBase == null)
			{
				return myGlobalEventBase;
			}
			myGlobalEventBase.InitFromDefinition(eventDefinition);
			return myGlobalEventBase;
		}

		public static MyGlobalEventBase CreateEvent(MyObjectBuilder_GlobalEventBase ob)
		{
			if (ob.DefinitionId.HasValue)
			{
				if (ob.DefinitionId.Value.TypeId == MyObjectBuilderType.Invalid)
				{
					return CreateEventObsolete(ob);
				}
				ob.SubtypeName = ob.DefinitionId.Value.SubtypeName;
			}
			if (MyDefinitionManager.Static.GetEventDefinition(ob.GetId()) == null)
			{
				return null;
			}
			MyGlobalEventBase myGlobalEventBase = CreateEvent(ob.GetId());
			myGlobalEventBase.Init(ob);
			return myGlobalEventBase;
		}

		private static MyGlobalEventBase CreateEventObsolete(MyObjectBuilder_GlobalEventBase ob)
		{
			MyGlobalEventBase myGlobalEventBase = CreateEvent(GetEventDefinitionObsolete(ob.EventType));
			myGlobalEventBase.SetActivationTime(TimeSpan.FromMilliseconds(ob.ActivationTimeMs));
			myGlobalEventBase.Enabled = ob.Enabled;
			return myGlobalEventBase;
		}

		private static MyDefinitionId GetEventDefinitionObsolete(MyGlobalEventTypeEnum eventType)
		{
			switch (eventType)
			{
			case MyGlobalEventTypeEnum.SpawnNeutralShip:
			case MyGlobalEventTypeEnum.SpawnCargoShip:
				return new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip");
			case MyGlobalEventTypeEnum.MeteorWave:
				return new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "MeteorWave");
			case MyGlobalEventTypeEnum.April2014:
				return new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "April2014");
			default:
				return new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase));
			}
		}
	}
}
