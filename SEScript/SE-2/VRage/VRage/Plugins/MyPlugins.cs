#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VRage.Collections;
using VRage.FileSystem;

namespace VRage.Plugins
{
	public class MyPlugins : IDisposable
	{
		private static List<IPlugin> m_plugins = new List<IPlugin>();

		private static List<IHandleInputPlugin> m_handleInputPlugins = new List<IHandleInputPlugin>();

		private static Assembly m_gamePluginAssembly;

		private static List<Assembly> m_userPluginAssemblies;

		private static Assembly m_sandboxAssembly;

		private static Assembly m_sandboxGameAssembly;

		private static Assembly m_gameObjBuildersPlugin;

		private static Assembly m_gameBaseObjBuildersPlugin;

		private static MyPlugins m_instance;

		public static bool Loaded => m_instance != null;

		public static ListReader<IPlugin> Plugins => m_plugins;

		public static ListReader<IHandleInputPlugin> HandleInputPlugins => m_handleInputPlugins;

		public static Assembly GameAssembly
		{
			get
			{
				if (!GameAssemblyReady)
				{
					return null;
				}
				return m_gamePluginAssembly;
			}
		}

		public static Assembly GameObjectBuildersAssembly
		{
			get
			{
				if (!GameObjectBuildersAssemblyReady)
				{
					return null;
				}
				return m_gameObjBuildersPlugin;
			}
		}

		public static Assembly GameBaseObjectBuildersAssembly
		{
			get
			{
				if (!GameBaseObjectBuildersAssemblyReady)
				{
					return null;
				}
				return m_gameBaseObjBuildersPlugin;
			}
		}

		public static Assembly[] UserAssemblies
		{
			get
			{
				if (!UserAssembliesReady)
				{
					return null;
				}
				return m_userPluginAssemblies.ToArray();
			}
		}

		public static Assembly SandboxAssembly
		{
			get
			{
				if (!SandboxAssemblyReady)
				{
					return null;
				}
				return m_sandboxAssembly;
			}
		}

		public static Assembly SandboxGameAssembly
		{
			get
			{
				if (m_sandboxGameAssembly == null)
				{
					return null;
				}
				return m_sandboxGameAssembly;
			}
		}

		public static bool GameAssemblyReady => m_gamePluginAssembly != null;

		public static bool GameObjectBuildersAssemblyReady => m_gameObjBuildersPlugin != null;

		public static bool GameBaseObjectBuildersAssemblyReady => m_gameBaseObjBuildersPlugin != null;

		public static bool UserAssembliesReady => m_userPluginAssemblies != null;

		public static bool SandboxAssemblyReady => m_sandboxAssembly != null;

		public static void RegisterFromArgs(string[] args)
		{
			m_userPluginAssemblies = null;
			if (args == null)
			{
				return;
			}
			List<string> list = new List<string>();
			if (args.Contains("-plugin"))
			{
				for (int i = args.ToList().IndexOf("-plugin"); i + 1 < args.Length && !args[i + 1].StartsWith("-"); i++)
				{
					list.Add(args[i + 1]);
				}
			}
			if (list.Count > 0)
			{
				m_userPluginAssemblies = new List<Assembly>(list.Count);
				for (int j = 0; j < list.Count; j++)
				{
					m_userPluginAssemblies.Add(Assembly.LoadFrom(list[j]));
				}
			}
		}

		public static void RegisterUserAssemblyFiles(List<string> userAssemblyFiles)
		{
			if (userAssemblyFiles != null)
			{
				if (m_userPluginAssemblies == null)
				{
					m_userPluginAssemblies = new List<Assembly>(userAssemblyFiles.Count);
				}
				foreach (string userAssemblyFile in userAssemblyFiles)
				{
					if (!string.IsNullOrEmpty(userAssemblyFile))
					{
						m_userPluginAssemblies.Add(Assembly.LoadFrom(userAssemblyFile));
					}
				}
			}
		}

		public static void RegisterGameAssemblyFile(string gameAssemblyFile)
		{
			if (gameAssemblyFile != null)
			{
				m_gamePluginAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameAssemblyFile));
			}
		}

		public static void RegisterGameObjectBuildersAssemblyFile(string gameObjBuildersAssemblyFile)
		{
			if (gameObjBuildersAssemblyFile != null)
			{
				m_gameObjBuildersPlugin = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameObjBuildersAssemblyFile));
			}
		}

		public static void RegisterBaseGameObjectBuildersAssemblyFile(string gameBaseObjBuildersAssemblyFile)
		{
			if (gameBaseObjBuildersAssemblyFile != null)
			{
				m_gameBaseObjBuildersPlugin = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameBaseObjBuildersAssemblyFile));
			}
		}

		public static void RegisterSandboxAssemblyFile(string sandboxAssemblyFile)
		{
			if (sandboxAssemblyFile != null)
			{
				m_sandboxAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, sandboxAssemblyFile));
			}
		}

		public static void RegisterSandboxGameAssemblyFile(string sandboxAssemblyFile)
		{
			if (sandboxAssemblyFile != null)
			{
				m_sandboxGameAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, sandboxAssemblyFile));
			}
		}

		public static void Load()
		{
			try
			{
				if (m_gamePluginAssembly != null)
				{
					LoadPlugins(new List<Assembly>
					{
						m_gamePluginAssembly
					});
				}
				if (m_userPluginAssemblies != null)
				{
					LoadPlugins(m_userPluginAssemblies);
				}
			}
			catch (Exception ex)
			{
				if (ex is ReflectionTypeLoadException)
				{
					_ = (ex as ReflectionTypeLoadException).LoaderExceptions;
				}
				throw;
			}
			m_instance = new MyPlugins();
		}

		private static void LoadPlugins(List<Assembly> assemblies)
		{
			foreach (Assembly assembly in assemblies)
			{
				foreach (Type item in from s in assembly.GetTypes()
					where s.GetInterfaces().Contains(typeof(IPlugin)) && !s.IsAbstract
					select s)
				{
					try
					{
						IPlugin plugin = (IPlugin)Activator.CreateInstance(item);
						m_plugins.Add(plugin);
						if (plugin is IHandleInputPlugin)
						{
							m_handleInputPlugins.Add((IHandleInputPlugin)plugin);
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Trace.Fail("Cannot create instance of '" + item.FullName + "': " + ex.ToString());
					}
				}
			}
		}

		public static void Unload()
		{
			foreach (IPlugin plugin in m_plugins)
			{
				plugin.Dispose();
			}
			m_plugins.Clear();
			m_handleInputPlugins.Clear();
			m_instance.Dispose();
			m_instance = null;
			m_gamePluginAssembly = null;
			m_userPluginAssemblies = null;
			m_sandboxAssembly = null;
			m_sandboxGameAssembly = null;
			m_gameObjBuildersPlugin = null;
			m_gameBaseObjBuildersPlugin = null;
		}

		private MyPlugins()
		{
		}

		~MyPlugins()
		{
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
