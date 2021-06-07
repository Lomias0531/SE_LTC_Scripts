using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	public class MyTextSurfaceScriptFactory
	{
		public struct ScriptInfo
		{
			public string Id;

			public MyStringId DisplayName;

			public Type Type;
		}

		private static MyTextSurfaceScriptFactory m_instance;

		private Dictionary<string, ScriptInfo> m_scripts = new Dictionary<string, ScriptInfo>();

		public DictionaryReader<string, ScriptInfo> Scripts => m_scripts;

		public static MyTextSurfaceScriptFactory Instance => m_instance;

		public static void LoadScripts()
		{
			if (m_instance == null)
			{
				m_instance = new MyTextSurfaceScriptFactory();
			}
			m_instance.m_scripts.Clear();
			m_instance.RegisterFromAssembly(Assembly.GetExecutingAssembly());
			m_instance.RegisterFromAssembly(MySession.Static.ScriptManager.Scripts.Values);
			m_instance.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_instance.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static void UnloadScripts()
		{
			if (m_instance != null)
			{
				m_instance.m_scripts.Clear();
				m_instance = null;
			}
		}

		public void RegisterFromAssembly(IEnumerable<Assembly> assemblies)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					RegisterFromAssembly(assembly);
				}
			}
		}

		public void RegisterFromAssembly(Assembly assembly)
		{
			foreach (TypeInfo definedType in assembly.DefinedTypes)
			{
				if (definedType.ImplementedInterfaces.Contains(typeof(IMyTextSurfaceScript)) && !(definedType == typeof(MyTextSurfaceScriptBase)))
				{
					MyTextSurfaceScriptAttribute customAttribute = definedType.GetCustomAttribute<MyTextSurfaceScriptAttribute>();
					if (customAttribute != null)
					{
						m_scripts[customAttribute.Id] = new ScriptInfo
						{
							Id = customAttribute.Id,
							DisplayName = MyStringId.GetOrCompute(customAttribute.DisplayName),
							Type = definedType.AsType()
						};
					}
				}
			}
		}

		public static IMyTextSurfaceScript CreateScript(string id, IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
		{
			if (m_instance == null)
			{
				return null;
			}
			if (!m_instance.Scripts.TryGetValue(id, out ScriptInfo value))
			{
				return null;
			}
			return (IMyTextSurfaceScript)Activator.CreateInstance(value.Type, surface, block, size);
		}
	}
}
