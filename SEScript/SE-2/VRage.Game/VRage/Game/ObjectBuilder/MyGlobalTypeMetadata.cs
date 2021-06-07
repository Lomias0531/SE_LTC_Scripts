using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace VRage.Game.ObjectBuilder
{
	public class MyGlobalTypeMetadata
	{
		/// <summary>
		/// Default type metadata manager.
		/// </summary>
		public static MyGlobalTypeMetadata Static = new MyGlobalTypeMetadata();

		private HashSet<Assembly> m_assemblies = new HashSet<Assembly>();

		private bool m_ready;

		public void RegisterAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					RegisterAssembly(assembly);
				}
			}
		}

		public void RegisterAssembly(Assembly assembly)
		{
			if (!(assembly == null))
			{
				m_assemblies.Add(assembly);
				MyObjectBuilderSerializer.RegisterFromAssembly(assembly);
				MyObjectBuilderType.RegisterFromAssembly(assembly, registerLegacyNames: true);
				MyXmlSerializerManager.RegisterFromAssembly(assembly);
				MyDefinitionManagerBase.RegisterTypesFromAssembly(assembly);
			}
		}

		public Type GetType(string fullName, bool throwOnError)
		{
			foreach (Assembly assembly in m_assemblies)
			{
				Type type;
				if ((type = assembly.GetType(fullName, throwOnError: false)) != null)
				{
					return type;
				}
			}
			if (throwOnError)
			{
				throw new TypeLoadException($"Type {fullName} was not found in any registered assembly!");
			}
			return null;
		}

		/// <summary>
		/// Initalize the registry with all the defautl game assemblies.
		/// </summary>
		public void Init(bool loadSerializersAsync = true)
		{
			if (!m_ready)
			{
				MyXmlSerializerManager.RegisterSerializableBaseType(typeof(MyObjectBuilder_Base));
				RegisterAssembly(GetType().Assembly);
				RegisterAssembly(MyPlugins.GameAssembly);
				RegisterAssembly(MyPlugins.SandboxGameAssembly);
				RegisterAssembly(MyPlugins.SandboxAssembly);
				RegisterAssembly(MyPlugins.UserAssemblies);
				RegisterAssembly(MyPlugins.GameBaseObjectBuildersAssembly);
				RegisterAssembly(MyPlugins.GameObjectBuildersAssembly);
				foreach (IPlugin plugin in MyPlugins.Plugins)
				{
					RegisterAssembly(plugin.GetType().Assembly);
				}
				if (loadSerializersAsync)
				{
					Task.Factory.StartNew(delegate
					{
						MyObjectBuilderSerializer.LoadSerializers();
					});
				}
				else
				{
					MyObjectBuilderSerializer.LoadSerializers();
				}
				m_ready = true;
			}
		}
	}
}
