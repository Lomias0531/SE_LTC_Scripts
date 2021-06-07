using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.Collections;
using VRage.Plugins;

namespace Sandbox.Game.Replication
{
	public class MyReplicableFactory
	{
		private readonly MyConcurrentDictionary<Type, Type> m_objTypeToExternalReplicableType = new MyConcurrentDictionary<Type, Type>(32);

		public MyReplicableFactory()
		{
			Assembly[] array = new Assembly[4]
			{
				typeof(MySandboxGame).Assembly,
				MyPlugins.GameAssembly,
				MyPlugins.SandboxAssembly,
				MyPlugins.SandboxGameAssembly
			};
			if (MyPlugins.UserAssemblies != null)
			{
				array = array.Union(MyPlugins.UserAssemblies).ToArray();
			}
			RegisterFromAssemblies(array);
		}

		private void RegisterFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			foreach (Assembly item in assemblies.Where((Assembly x) => x != null))
			{
				RegisterFromAssembly(item);
			}
		}

		private void RegisterFromAssembly(Assembly assembly)
		{
			foreach (Type item in from t in assembly.GetTypes()
				where typeof(MyExternalReplicable).IsAssignableFrom(t) && !t.IsAbstract
				select t)
			{
				Type type = item.FindGenericBaseTypeArgument(typeof(MyExternalReplicable<>));
				if (type != null && !m_objTypeToExternalReplicableType.ContainsKey(type))
				{
					m_objTypeToExternalReplicableType.TryAdd(type, item);
				}
			}
		}

		public Type FindTypeFor(object obj)
		{
			Type type = obj.GetType();
			if (type.IsValueType)
			{
				throw new InvalidOperationException("obj cannot be value type");
			}
			Type value = null;
			Type type2 = type;
			while (type2 != typeof(object) && !m_objTypeToExternalReplicableType.TryGetValue(type2, out value))
			{
				type2 = type2.BaseType;
			}
			if (type != type2)
			{
				m_objTypeToExternalReplicableType.TryAdd(type, value);
			}
			return value;
		}
	}
}
