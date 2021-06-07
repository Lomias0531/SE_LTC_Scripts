using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VRage.FileSystem;
using VRage.Plugins;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public static class MyVSAssemblyProvider
	{
		public readonly struct Arguments
		{
			public readonly IEnumerable<string> SourceFiles;

			public readonly string BaseContentPath;

			public readonly Assembly ReferenceProvider;

			public readonly string OutputPath;

			public readonly bool GenerateDummy;

			public readonly bool LoadAssembly;

			public Arguments(IEnumerable<string> sourceFiles, string baseContentPath, Assembly referenceProvider, string outputPath, bool generateDummy, bool loadAssembly = true)
			{
				SourceFiles = sourceFiles;
				BaseContentPath = baseContentPath;
				ReferenceProvider = referenceProvider;
				OutputPath = outputPath;
				GenerateDummy = generateDummy;
				LoadAssembly = loadAssembly;
			}
		}

		private static MyVSPreprocessor m_defaultPreprocessor = new MyVSPreprocessor();

		private static MyVSCompiler m_defaultCompiler;

		private static Assembly m_assembly;

		public static void Init(IEnumerable<string> fileNames, string localModPath)
		{
			Arguments args = new Arguments(fileNames, localModPath, MyPlugins.GameAssembly, null, generateDummy: false);
			Init(in args);
		}

		public static void Init(in Arguments args)
		{
			if (MyVSCompiler.DependencyCollector == null)
			{
				MyVSCompiler.DependencyCollector = new MyDependencyCollector();
				MyVSCompiler.DependencyCollector.CollectReferences(args.ReferenceProvider);
			}
			m_defaultPreprocessor.Clear();
			foreach (string sourceFile in args.SourceFiles)
			{
				m_defaultPreprocessor.AddFile(sourceFile, args.BaseContentPath);
			}
			List<string> list = new List<string>();
			string[] fileSet = m_defaultPreprocessor.FileSet;
			MyVisualScriptBuilder myVisualScriptBuilder = new MyVisualScriptBuilder();
			string[] array = fileSet;
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = myVisualScriptBuilder.ScriptFilePath = array[i];
				if (myVisualScriptBuilder.Load() && myVisualScriptBuilder.Build())
				{
					list.Add(Path.Combine(Path.GetTempPath(), myVisualScriptBuilder.ScriptName + ".cs"));
					File.WriteAllText(list[list.Count - 1], myVisualScriptBuilder.Syntax);
				}
			}
			if (args.GenerateDummy)
			{
				GenerateDummy(out string path);
				list.Add(path);
			}
			m_defaultCompiler = new MyVSCompiler((args.OutputPath != null) ? Path.GetFileNameWithoutExtension(args.OutputPath) : "MyVSDefaultAssembly", list);
			if (fileSet.Length != 0 && m_defaultCompiler.Compile())
			{
				if (!string.IsNullOrEmpty(args.OutputPath))
				{
					m_defaultCompiler.WriteAssembly(args.OutputPath);
				}
				if (args.LoadAssembly)
				{
					bool flag = m_defaultCompiler.LoadAssembly(args.OutputPath);
				}
			}
			m_assembly = m_defaultCompiler.Assembly;
			foreach (string item in list)
			{
				File.Delete(item);
			}
		}

		private static void GenerateDummy(out string path)
		{
			string arg = string.Join("\n", m_defaultPreprocessor.ClassNames.Select((string x) => $"           new {x}();"));
			string contents = $"namespace VisualScripting.CustomScripts\r\n{{\r\n    public class __Dummy\r\n    {{\r\n        public __Dummy()\r\n        {{\r\n{arg}\r\n        }}\r\n    }}\r\n}}";
			path = Path.Combine(Path.GetTempPath(), "__Dummy.cs");
			File.WriteAllText(path, contents);
		}

		public static bool TryLoad(string name, bool checkExists)
		{
			if (checkExists && !MyFileSystem.FileExists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".dll")))
			{
				return false;
			}
			m_assembly = Assembly.Load(name);
			return true;
		}

		public static Type GetType(string typeName)
		{
			if (m_assembly == null)
			{
				return null;
			}
			return m_assembly.GetType(typeName);
		}

		public static List<IMyLevelScript> GetLevelScriptInstances(HashSet<string> scriptNames = null)
		{
			List<IMyLevelScript> list = new List<IMyLevelScript>();
			if (m_assembly == null)
			{
				return list;
			}
			Type[] types = m_assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(IMyLevelScript).IsAssignableFrom(type) && (scriptNames == null || scriptNames.Contains(type.Name)))
				{
					list.Add((IMyLevelScript)Activator.CreateInstance(type));
				}
			}
			return list;
		}
	}
}
