using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VRage.Utils
{
	public static class MyEnumDuplicitiesTester
	{
		private const string m_keenSWHCompanyName = "Keen Software House";

		[Conditional("DEBUG")]
		public static void CheckEnumNotDuplicitiesInRunningApplication()
		{
			CheckEnumNotDuplicities("Keen Software House");
		}

		private static void CheckEnumNotDuplicities(string companyName)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
			List<Assembly> list = new List<Assembly>(assemblies.Length + files.Length);
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				if (companyName == null || GetCompanyNameOfAssembly(assembly) == companyName)
				{
					list.Add(assembly);
				}
			}
			string[] array2 = files;
			foreach (string text in array2)
			{
				if (!IsLoaded(assemblies, text) && (companyName == null || FileVersionInfo.GetVersionInfo(text).CompanyName == companyName))
				{
					list.Add(Assembly.LoadFrom(text));
				}
			}
			HashSet<object> hashSet = new HashSet<object>();
			foreach (Assembly item in list)
			{
				TestEnumNotDuplicitiesInAssembly(item, hashSet);
			}
		}

		private static bool IsLoaded(Assembly[] assemblies, string assemblyPath)
		{
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.IsDynamic || (!string.IsNullOrEmpty(assembly.Location) && Path.GetFullPath(assembly.Location) == assemblyPath))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// The company name of the calling assembly.
		/// </summary>
		private static string GetCompanyNameOfAssembly(Assembly assembly)
		{
			AssemblyCompanyAttribute assemblyCompanyAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute), inherit: false) as AssemblyCompanyAttribute;
			if (assemblyCompanyAttribute == null)
			{
				return string.Empty;
			}
			return assemblyCompanyAttribute.Company;
		}

		private static void TestEnumNotDuplicitiesInAssembly(Assembly assembly, HashSet<object> hashSet)
		{
		}

		private static void AssertEnumNotDuplicities(Type enumType, HashSet<object> hashSet)
		{
			hashSet.Clear();
			foreach (object value in Enum.GetValues(enumType))
			{
				if (!hashSet.Add(value))
				{
					throw new Exception(string.Concat("Duplicate enum found: ", value, " in ", enumType.AssemblyQualifiedName));
				}
			}
		}
	}
}
