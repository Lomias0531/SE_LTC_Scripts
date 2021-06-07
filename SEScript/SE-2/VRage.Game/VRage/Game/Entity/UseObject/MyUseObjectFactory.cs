using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VRage.FileSystem;
using VRage.ModAPI;
using VRage.Plugins;
using VRageRender.Import;

namespace VRage.Game.Entity.UseObject
{
	[PreloadRequired]
	public static class MyUseObjectFactory
	{
		private static Dictionary<string, Type> m_useObjectTypesByDummyName;

		static MyUseObjectFactory()
		{
			m_useObjectTypesByDummyName = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
			RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
			RegisterAssemblyTypes(MyPlugins.GameAssembly);
			RegisterAssemblyTypes(MyPlugins.SandboxAssembly);
			RegisterAssemblyTypes(MyPlugins.UserAssemblies);
			RegisterAssemblyTypes(Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, "Sandbox.Game.dll")));
		}

		private static void RegisterAssemblyTypes(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterAssemblyTypes(assemblies[i]);
				}
			}
		}

		private static void RegisterAssemblyTypes(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type typeFromHandle = typeof(IMyUseObject);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!typeFromHandle.IsAssignableFrom(type))
				{
					continue;
				}
				MyUseObjectAttribute[] array = (MyUseObjectAttribute[])type.GetCustomAttributes(typeof(MyUseObjectAttribute), inherit: false);
				if (!array.IsNullOrEmpty())
				{
					MyUseObjectAttribute[] array2 = array;
					foreach (MyUseObjectAttribute myUseObjectAttribute in array2)
					{
						m_useObjectTypesByDummyName[myUseObjectAttribute.DummyName] = type;
					}
				}
			}
		}

		[Conditional("DEBUG")]
		private static void AssertHasCorrectCtor(Type type)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			for (int i = 0; i < constructors.Length; i++)
			{
				ParameterInfo[] parameters = constructors[i].GetParameters();
				if (parameters.Length == 4 && parameters[0].ParameterType == typeof(IMyEntity) && parameters[1].ParameterType == typeof(string) && parameters[2].ParameterType == typeof(MyModelDummy) && parameters[3].ParameterType == typeof(uint))
				{
					break;
				}
			}
		}

		public static IMyUseObject CreateUseObject(string detectorName, IMyEntity owner, string dummyName, MyModelDummy dummyData, uint shapeKey)
		{
			if (!m_useObjectTypesByDummyName.TryGetValue(detectorName, out Type value) || value == null)
			{
				return null;
			}
			return (IMyUseObject)Activator.CreateInstance(value, owner, dummyName, dummyData, shapeKey);
		}
	}
}
