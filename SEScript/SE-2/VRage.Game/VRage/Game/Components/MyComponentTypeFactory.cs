using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Plugins;
using VRage.Utils;

namespace VRage.Game.Components
{
	[PreloadRequired]
	public static class MyComponentTypeFactory
	{
		private static Dictionary<MyStringId, Type> m_idToType;

		private static Dictionary<Type, MyStringId> m_typeToId;

		private static Dictionary<Type, Type> m_typeToContainerComponentType;

		static MyComponentTypeFactory()
		{
			m_idToType = new Dictionary<MyStringId, Type>(MyStringId.Comparer);
			m_typeToId = new Dictionary<Type, MyStringId>();
			m_typeToContainerComponentType = new Dictionary<Type, Type>();
			RegisterFromAssembly(typeof(MyComponentTypeFactory).Assembly);
			RegisterFromAssembly(MyPlugins.SandboxAssembly);
			RegisterFromAssembly(MyPlugins.GameAssembly);
			RegisterFromAssembly(MyPlugins.SandboxGameAssembly);
			RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		private static void RegisterFromAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterFromAssembly(assemblies[i]);
				}
			}
		}

		private static void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type typeFromHandle = typeof(MyComponentBase);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeFromHandle.IsAssignableFrom(type))
				{
					AddId(type, MyStringId.GetOrCompute(type.Name));
					RegisterComponentTypeAttribute(type);
				}
			}
		}

		private static void RegisterComponentTypeAttribute(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(MyComponentTypeAttribute), inherit: true);
			Type type2 = null;
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				MyComponentTypeAttribute myComponentTypeAttribute = (MyComponentTypeAttribute)array[i];
				if (myComponentTypeAttribute.ComponentType != null && type2 == null)
				{
					type2 = myComponentTypeAttribute.ComponentType;
					break;
				}
			}
			if (type2 != null)
			{
				m_typeToContainerComponentType.Add(type, type2);
			}
		}

		private static void AddId(Type type, MyStringId id)
		{
			m_idToType[id] = type;
			m_typeToId[type] = id;
		}

		public static MyStringId GetId(Type type)
		{
			return m_typeToId[type];
		}

		public static Type GetType(MyStringId id)
		{
			return m_idToType[id];
		}

		public static Type GetType(string typeId)
		{
			if (MyStringId.TryGet(typeId, out MyStringId id))
			{
				return m_idToType[id];
			}
			throw new Exception("Unregistered component typeId! : " + typeId);
		}

		public static Type GetComponentType(Type type)
		{
			if (m_typeToContainerComponentType.TryGetValue(type, out Type value))
			{
				return value;
			}
			return null;
		}
	}
}
