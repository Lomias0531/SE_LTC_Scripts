using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game.Definitions;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	public abstract class MyDefinitionManagerBase
	{
		protected MyDefinitionSet m_definitions = new MyDefinitionSet();

		private static MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase> m_definitionFactory = new MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase>();

		protected static Dictionary<Type, MyDefinitionPostprocessor> m_postprocessorsByType = new Dictionary<Type, MyDefinitionPostprocessor>();

		protected static List<MyDefinitionPostprocessor> m_postProcessors = new List<MyDefinitionPostprocessor>();

		protected static HashSet<Assembly> m_registeredAssemblies = new HashSet<Assembly>();

		public static MyDefinitionManagerBase Static;

		private static readonly Dictionary<Type, HashSet<Type>> m_childDefinitionMap = new Dictionary<Type, HashSet<Type>>();

		private static ConcurrentDictionary<Type, Type> m_objectBuilderTypeCache = new ConcurrentDictionary<Type, Type>();

		private static HashSet<Assembly> m_registered = new HashSet<Assembly>();

		public MyDefinitionSet Definitions => m_definitions;

		public static MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase> GetObjectFactory()
		{
			return m_definitionFactory;
		}

		public static void RegisterTypesFromAssembly(Assembly assembly)
		{
			if (assembly == null || m_registeredAssemblies.Contains(assembly))
			{
				return;
			}
			m_registeredAssemblies.Add(assembly);
			if (m_registered.Contains(assembly))
			{
				return;
			}
			m_registered.Add(assembly);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyDefinitionTypeAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					continue;
				}
				if (!type.IsSubclassOf(typeof(MyDefinitionBase)) && type != typeof(MyDefinitionBase))
				{
					MyLog.Default.Error("Type {0} is not a definition.", type.Name);
					continue;
				}
				object[] array = customAttributes;
				for (int j = 0; j < array.Length; j++)
				{
					MyDefinitionTypeAttribute myDefinitionTypeAttribute = (MyDefinitionTypeAttribute)array[j];
					m_definitionFactory.RegisterDescriptor(myDefinitionTypeAttribute, type);
					MyDefinitionPostprocessor myDefinitionPostprocessor = (MyDefinitionPostprocessor)Activator.CreateInstance(myDefinitionTypeAttribute.PostProcessor);
					myDefinitionPostprocessor.DefinitionType = myDefinitionTypeAttribute.ObjectBuilderType;
					m_postProcessors.Add(myDefinitionPostprocessor);
					m_postprocessorsByType.Add(myDefinitionTypeAttribute.ObjectBuilderType, myDefinitionPostprocessor);
					MyXmlSerializerManager.RegisterSerializer(myDefinitionTypeAttribute.ObjectBuilderType);
				}
				Type type2 = type;
				while (type2 != typeof(MyDefinitionBase))
				{
					type2 = type2.BaseType;
					if (!m_childDefinitionMap.TryGetValue(type2, out HashSet<Type> value))
					{
						value = new HashSet<Type>();
						m_childDefinitionMap[type2] = value;
						value.Add(type2);
					}
					value.Add(type);
				}
			}
			m_postProcessors.Sort(MyDefinitionPostprocessor.Comparer);
		}

		public static MyDefinitionPostprocessor GetPostProcessor(Type obType)
		{
			m_postprocessorsByType.TryGetValue(obType, out MyDefinitionPostprocessor value);
			return value;
		}

		public static Type GetObjectBuilderType(Type defType)
		{
			if (m_objectBuilderTypeCache.TryGetValue(defType, out Type value))
			{
				return value;
			}
			object[] customAttributes = defType.GetCustomAttributes(typeof(MyDefinitionTypeAttribute), inherit: false);
			int num = 0;
			if (num < customAttributes.Length)
			{
				value = ((MyDefinitionTypeAttribute)customAttributes[num]).ObjectBuilderType;
				m_objectBuilderTypeCache.TryAdd(defType, value);
				return value;
			}
			return null;
		}

		public T GetDefinition<T>(string subtypeId) where T : MyDefinitionBase
		{
			return m_definitions.GetDefinition<T>(MyStringHash.GetOrCompute(subtypeId));
		}

		public T GetDefinition<T>(MyStringHash subtypeId) where T : MyDefinitionBase
		{
			return m_definitions.GetDefinition<T>(subtypeId);
		}

		public T GetDefinition<T>(MyDefinitionId subtypeId) where T : MyDefinitionBase
		{
			return m_definitions.GetDefinition<T>(subtypeId);
		}

		public IEnumerable<T> GetDefinitions<T>() where T : MyDefinitionBase
		{
			return m_definitions.GetDefinitionsOfType<T>();
		}

		public IEnumerable<T> GetAllDefinitions<T>() where T : MyDefinitionBase
		{
			return m_definitions.GetDefinitionsOfTypeAndSubtypes<T>();
		}

		public bool TryGetDefinition<T>(MyStringHash subtypeId, out T def) where T : MyDefinitionBase
		{
			if ((def = m_definitions.GetDefinition<T>(subtypeId)) != null)
			{
				return true;
			}
			return false;
		}

		public abstract MyDefinitionSet GetLoadingSet();

		public HashSet<Type> GetSubtypes<T>()
		{
			m_childDefinitionMap.TryGetValue(typeof(T), out HashSet<Type> value);
			return value;
		}
	}
}
