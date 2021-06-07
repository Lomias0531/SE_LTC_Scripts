using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;
using VRage.FileSystem;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyVSCompiler
	{
		public static MyDependencyCollector DependencyCollector;

		private static readonly CSharpCompilationOptions m_defaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOverflowChecks(enabled: true).WithOptimizationLevel(OptimizationLevel.Release);

		public readonly List<string> SourceFiles = new List<string>();

		public readonly List<string> SourceTexts = new List<string>();

		private CSharpCompilation m_compilation;

		private Assembly m_compiledAndLoadedAssembly;

		public string AssemblyName
		{
			get;
			private set;
		}

		public Assembly Assembly => m_compiledAndLoadedAssembly;

		public MyVSCompiler(string assemblyName, IEnumerable<string> sourceFiles)
			: this(assemblyName)
		{
			SourceFiles.AddRange(sourceFiles);
		}

		public MyVSCompiler(string assemblyName)
		{
			AssemblyName = assemblyName;
		}

		/// <summary>
		/// Creates a fresh new compilation of source files.
		/// Does not load any assembly.
		/// </summary>
		/// <returns>Success if no compilation errors were encountered.</returns>
		public bool Compile()
		{
			if (SourceFiles.Count == 0 && SourceTexts.Count == 0)
			{
				return false;
			}
			SyntaxTree[] array = new SyntaxTree[SourceFiles.Count + SourceTexts.Count];
			int num = 0;
			try
			{
				foreach (string sourceFile in SourceFiles)
				{
					using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(sourceFile)))
					{
						string text = streamReader.ReadToEnd();
						array[num] = CSharpSyntaxTree.ParseText(text);
						num++;
					}
				}
				foreach (string sourceText in SourceTexts)
				{
					array[num++] = CSharpSyntaxTree.ParseText(sourceText);
				}
				m_compilation = CSharpCompilation.Create(AssemblyName, array, DependencyCollector.References, m_defaultCompilationOptions);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public string GetDiagnosticMessage()
		{
			ImmutableArray<Diagnostic> diagnostics = m_compilation.GetDiagnostics();
			if (!diagnostics.IsDefaultOrEmpty)
			{
				StringBuilder stringBuilder = new StringBuilder();
				ImmutableArray<Diagnostic>.Enumerator enumerator = diagnostics.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Diagnostic current = enumerator.Current;
					if (current.Severity == DiagnosticSeverity.Error)
					{
						stringBuilder.AppendLine(current.ToString());
					}
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		public bool WriteAssembly(string outputPath)
		{
			string path = Path.ChangeExtension(outputPath, ".pdb");
			using (FileStream assemblyOutputStream = File.OpenWrite(outputPath))
			{
				using (FileStream symbolsOutputStream = File.OpenWrite(path))
				{
					return Emit(assemblyOutputStream, symbolsOutputStream);
				}
			}
		}

		/// <summary>
		/// Loads assembly.
		/// <returns>Loading success.</returns>
		/// </summary>
		public bool LoadAssembly(string outputPath = null)
		{
			try
			{
				if (outputPath == null)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							if (Emit(memoryStream, memoryStream2))
							{
								m_compiledAndLoadedAssembly = Assembly.Load(memoryStream.ToArray(), memoryStream2.ToArray());
							}
						}
					}
				}
				else
				{
					m_compiledAndLoadedAssembly = Assembly.LoadFrom(outputPath);
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Write the final assembly and it's symbols to the provided streams.
		/// </summary>
		/// <param name="assemblyOutputStream"></param>
		/// <param name="symbolsOutputStream"></param>
		/// <returns></returns>
		private bool Emit(Stream assemblyOutputStream, Stream symbolsOutputStream)
		{
			EmitResult emitResult = m_compilation.Emit(assemblyOutputStream, symbolsOutputStream);
			if (!emitResult.Success)
			{
				StringBuilder stringBuilder = new StringBuilder();
				ImmutableArray<Diagnostic>.Enumerator enumerator = emitResult.Diagnostics.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Diagnostic current = enumerator.Current;
					string arg = string.Empty;
					if (current.Location != Location.None)
					{
						foreach (SyntaxNode item in current.Location.SourceTree.GetRoot().DescendantNodes())
						{
							ClassDeclarationSyntax classDeclarationSyntax;
							if ((classDeclarationSyntax = (item as ClassDeclarationSyntax)) != null)
							{
								arg = classDeclarationSyntax.Identifier.Text;
								break;
							}
						}
						stringBuilder.AppendLine(arg + ": " + current);
					}
					else
					{
						stringBuilder.AppendLine(current.ToString());
					}
				}
				return false;
			}
			return true;
		}

		public List<IMyLevelScript> GetLevelScriptInstances()
		{
			List<IMyLevelScript> list = new List<IMyLevelScript>();
			if (m_compiledAndLoadedAssembly == null)
			{
				return list;
			}
			Type[] types = m_compiledAndLoadedAssembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(IMyLevelScript).IsAssignableFrom(type))
				{
					list.Add((IMyLevelScript)Activator.CreateInstance(type));
				}
			}
			return list;
		}
	}
}
