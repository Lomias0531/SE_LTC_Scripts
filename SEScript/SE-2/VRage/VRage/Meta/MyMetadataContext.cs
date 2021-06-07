using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Library.Collections;

namespace VRage.Meta
{
	/// <summary>
	/// Base class for a metadata context.
	/// </summary>
	public class MyMetadataContext
	{
		protected readonly Dictionary<Type, IMyMetadataIndexer> Indexers = new Dictionary<Type, IMyMetadataIndexer>();

		protected readonly MyListDictionary<Type, IMyAttributeIndexer> AttributeIndexers = new MyListDictionary<Type, IMyAttributeIndexer>();

		protected readonly List<IMyTypeIndexer> TypeIndexers = new List<IMyTypeIndexer>();

		protected readonly HashSet<Assembly> KnownAssemblies = new HashSet<Assembly>();

		/// <summary>
		/// Whether this context should look for indexers while exploring the assembly.
		/// </summary>
		public bool RegisterIndexers = true;

		/// <summary>
		/// Set of know assemblies.
		/// </summary>
		public HashSetReader<Assembly> Known => KnownAssemblies;

		/// <summary>
		/// Invoked when this context becomes the top of the stack.
		/// </summary>
		protected internal virtual void Activate()
		{
			foreach (IMyMetadataIndexer value in Indexers.Values)
			{
				value.Activate();
			}
		}

		/// <summary>
		/// Invoked when this context is popped and it's data must be disposed.
		/// </summary>
		protected internal virtual void Close()
		{
			foreach (IMyMetadataIndexer value in Indexers.Values)
			{
				value.Close();
			}
			AttributeIndexers.Clear();
			KnownAssemblies.Clear();
			Indexers.Clear();
		}

		/// <summary>
		/// Index the given assembly.
		/// </summary>
		/// <param name="assembly">The assembly to index.</param>
		/// <param name="batch">Whether to process this addition as a part of a batch.</param>
		protected internal virtual void Index(Assembly assembly, bool batch = false)
		{
			if (KnownAssemblies.Contains(assembly))
			{
				return;
			}
			KnownAssemblies.Add(assembly);
			if (RegisterIndexers)
			{
				PreProcess(assembly);
			}
			Module[] loadedModules = assembly.GetLoadedModules();
			for (int i = 0; i < loadedModules.Length; i++)
			{
				Type[] types = loadedModules[i].GetTypes();
				foreach (Type type in types)
				{
					Attribute[] customAttributes = Attribute.GetCustomAttributes(type);
					for (int k = 0; k < customAttributes.Length; k++)
					{
						if (AttributeIndexers.TryGet(customAttributes[k].GetType(), out List<IMyAttributeIndexer> list))
						{
							foreach (IMyAttributeIndexer item in list)
							{
								item.Observe(customAttributes[k], type);
							}
						}
					}
					for (int l = 0; l < TypeIndexers.Count; l++)
					{
						TypeIndexers[l].Index(type);
					}
				}
			}
			if (!batch)
			{
				FinishBatch();
			}
		}

		/// <summary>
		/// Index a set of assemblies.
		/// </summary>
		/// <param name="assemblies">Collection of assemblies to index.</param>
		/// <param name="batch">Whether to process this addition as a part of a batch.</param>
		internal void Index(IEnumerable<Assembly> assemblies, bool batch = false)
		{
			foreach (Assembly assembly in assemblies)
			{
				Index(assembly);
			}
			if (!batch)
			{
				FinishBatch();
			}
		}

		/// <summary>
		/// Finish batch indexing of assemblies.
		///
		/// Batching is recommended when many assemblies are indexed at once.
		/// It significantly improves the performance of indexers that need to postprocess the full set of types.
		/// </summary>
		public void FinishBatch()
		{
			foreach (IMyMetadataIndexer value in Indexers.Values)
			{
				value.Process();
			}
		}

		/// <summary>
		/// Add a single indexer for the provided attribute type.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to index.</param>
		/// <param name="indexerType">The type of the indexer to create.</param>
		internal void AddIndexer(Type attributeType, Type indexerType)
		{
			IMyMetadataIndexer metaIndexer = GetMetaIndexer(indexerType);
			AttributeIndexers.Add(attributeType, (IMyAttributeIndexer)metaIndexer);
			metaIndexer.Activate();
		}

		private IMyMetadataIndexer GetMetaIndexer(Type indexerType)
		{
			if (!Indexers.TryGetValue(indexerType, out IMyMetadataIndexer value))
			{
				value = (IMyMetadataIndexer)Activator.CreateInstance(indexerType);
				Indexers.Add(indexerType, value);
			}
			return value;
		}

		/// <summary>
		/// Add several indexers at once from a set of mappings.
		/// </summary>
		/// <param name="indexerTypes">collection of attribute to indexer type mappings</param>
		internal void AddIndexers(IEnumerable<KeyValuePair<Type, HashSet<Type>>> indexerTypes)
		{
			foreach (KeyValuePair<Type, HashSet<Type>> indexerType in indexerTypes)
			{
				foreach (Type item in indexerType.Value)
				{
					AddIndexer(indexerType.Key, item);
				}
			}
		}

		/// <summary>
		/// Add a type indexer.
		///
		/// This may throw exception if the provided type is not valid.
		/// </summary>
		/// <param name="typeIndexer"></param>
		internal void AddIndexer(Type typeIndexer)
		{
			IMyMetadataIndexer metaIndexer = GetMetaIndexer(typeIndexer);
			TypeIndexers.Add((IMyTypeIndexer)metaIndexer);
			metaIndexer.Activate();
		}

		/// <summary>
		/// Add several type indexers.
		///
		/// This may throw exception if one of the provided types is not valid.
		/// </summary>
		/// <param name="typeIndexers"></param>
		internal void AddIndexers(IEnumerable<Type> typeIndexers)
		{
			foreach (Type typeIndexer in typeIndexers)
			{
				AddIndexer(typeIndexer);
			}
		}

		/// <summary>
		/// Hook indexers from parent to child.
		/// </summary>
		/// <param name="parent">Parent metadata context.</param>
		public void Hook(MyMetadataContext parent)
		{
			foreach (KeyValuePair<Type, IMyMetadataIndexer> indexer in Indexers)
			{
				if (parent.Indexers.TryGetValue(indexer.Key, out IMyMetadataIndexer value))
				{
					indexer.Value.SetParent(value);
				}
			}
		}

		/// <summary>
		/// Pre-process an assembly.
		///
		/// This will discover indexers and run any static constructors that are needed.
		/// </summary>
		/// <param name="assembly"></param>
		private void PreProcess(Assembly assembly)
		{
			Module[] loadedModules = assembly.GetLoadedModules();
			for (int i = 0; i < loadedModules.Length; i++)
			{
				Type[] types = loadedModules[i].GetTypes();
				foreach (Type type in types)
				{
					if (type.HasAttribute<PreloadRequiredAttribute>())
					{
						RuntimeHelpers.RunClassConstructor(type.TypeHandle);
					}
					foreach (MyAttributeMetadataIndexerAttributeBase customAttribute in type.GetCustomAttributes<MyAttributeMetadataIndexerAttributeBase>())
					{
						Type attributeType = customAttribute.AttributeType;
						Type indexerType = customAttribute.TargetType ?? type;
						MyMetadataSystem.RegisterAttributeIndexer(attributeType, indexerType);
					}
					if (type.GetCustomAttribute<MyTypeMetadataIndexerAttribute>() != null)
					{
						MyMetadataSystem.RegisterTypeIndexer(type);
					}
				}
			}
		}

		/// <summary>
		/// Tries to get indexer for the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="indexer"></param>
		/// <returns></returns>
		public bool TryGetIndexer(Type type, out IMyMetadataIndexer indexer)
		{
			return Indexers.TryGetValue(type, out indexer);
		}
	}
}
