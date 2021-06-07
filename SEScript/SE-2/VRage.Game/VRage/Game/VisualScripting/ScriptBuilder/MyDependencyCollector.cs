using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using VRage.Collections;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyDependencyCollector
	{
		private HashSet<MetadataReference> m_references;

		public HashSetReader<MetadataReference> References => new HashSetReader<MetadataReference>(m_references);

		public MyDependencyCollector(IEnumerable<Assembly> assemblies)
			: this()
		{
			foreach (Assembly assembly in assemblies)
			{
				CollectReferences(assembly);
			}
		}

		public MyDependencyCollector()
		{
			m_references = new HashSet<MetadataReference>();
			try
			{
				Assembly assembly = Assembly.Load("netstandard");
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
			}
			catch
			{
			}
			m_references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
		}

		public void CollectReferences(Assembly assembly)
		{
			if (!(assembly == null))
			{
				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				for (int i = 0; i < referencedAssemblies.Length; i++)
				{
					Assembly assembly2 = Assembly.Load(referencedAssemblies[i]);
					m_references.Add(MetadataReference.CreateFromFile(assembly2.Location));
				}
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
			}
		}

		public void RegisterAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
			}
		}
	}
}
