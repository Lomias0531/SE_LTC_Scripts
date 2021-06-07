using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
	public static class MySessionComponentMapping
	{
		private static Dictionary<Type, MyObjectBuilderType> m_objectBuilderTypeByType = new Dictionary<Type, MyObjectBuilderType>();

		private static Dictionary<MyObjectBuilderType, Type> m_typeByObjectBuilderType = new Dictionary<MyObjectBuilderType, Type>();

		private static Dictionary<Type, MyObjectBuilder_SessionComponent> m_sessionObjectBuilderByType = new Dictionary<Type, MyObjectBuilder_SessionComponent>();

		public static bool Map(Type type, MyObjectBuilderType objectBuilderType)
		{
			if (!type.IsSubclassOf(typeof(MySessionComponentBase)))
			{
				return false;
			}
			if (!m_objectBuilderTypeByType.ContainsKey(type))
			{
				m_objectBuilderTypeByType.Add(type, objectBuilderType);
				if (!m_typeByObjectBuilderType.ContainsKey(objectBuilderType))
				{
					m_typeByObjectBuilderType.Add(objectBuilderType, type);
					return true;
				}
				return false;
			}
			return false;
		}

		public static Type TryGetMappedSessionComponentType(MyObjectBuilderType objectBuilderType)
		{
			Type value = null;
			m_typeByObjectBuilderType.TryGetValue(objectBuilderType, out value);
			return value;
		}

		public static MyObjectBuilderType TryGetMappedObjectBuilderType(Type type)
		{
			MyObjectBuilderType value = null;
			m_objectBuilderTypeByType.TryGetValue(type, out value);
			return value;
		}

		public static void Clear()
		{
			m_objectBuilderTypeByType.Clear();
			m_typeByObjectBuilderType.Clear();
			m_sessionObjectBuilderByType.Clear();
		}

		public static Dictionary<Type, MyObjectBuilder_SessionComponent> GetMappedSessionObjectBuilders(List<MyObjectBuilder_SessionComponent> objectBuilders)
		{
			m_sessionObjectBuilderByType.Clear();
			foreach (MyObjectBuilder_SessionComponent objectBuilder in objectBuilders)
			{
				if (m_typeByObjectBuilderType.ContainsKey(objectBuilder.GetType()))
				{
					Type key = m_typeByObjectBuilderType[objectBuilder.GetType()];
					m_sessionObjectBuilderByType[key] = objectBuilder;
				}
			}
			return m_sessionObjectBuilderByType;
		}
	}
}
