using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Meta
{
	/// <summary>
	/// The metadata system is responsible for indexing types and information about those types.
	/// </summary>
	public static class MyMetadataSystem
	{
		/// <summary>
		/// Mapping of attribute indexer types.
		/// </summary>
		private static readonly MyHashSetDictionary<Type, Type> AttributeIndexers = new MyHashSetDictionary<Type, Type>();

		/// <summary>
		/// List of indexer types.
		/// </summary>
		private static readonly List<Type> TypeIndexers = new List<Type>();

		/// <summary>
		/// Metadata context stack.
		/// </summary>
		private static readonly List<MyMetadataContext> Stack = new List<MyMetadataContext>();

		/// <summary>
		/// Active metadata context.
		///
		/// If there is no context set this will be null.
		/// </summary>
		public static MyMetadataContext ActiveContext => Stack.LastOrDefault();

		/// <summary>
		/// Metadata log, links to default for now.
		///
		/// When we introduce hierarchical logging this should be related to the correct system.
		/// </summary>
		public static MyLog Log => MyLog.Default;

		/// <summary>
		/// Load metadata from an assembly that is already loaded.
		/// </summary>
		/// <param name="assembly">Assembly to index.</param>
		/// <param name="batch">Whether to treat this load as part of a batch.</param>
		public static void LoadAssembly(Assembly assembly, bool batch = false)
		{
			if (ActiveContext != null)
			{
				ActiveContext.Index(assembly, batch);
			}
			else
			{
				Log.Error("Assembly {0} will not be indexed because there are no registered indexers.");
			}
		}

		/// <summary>
		/// Finish batch indexing of assemblies.
		///
		/// Batching is recommended when many assemblies are indexed at once.
		/// It significantly improves the performance of indexers that need to postprocess the full set of types.
		/// </summary>
		public static void FinishBatch()
		{
			if (ActiveContext != null)
			{
				ActiveContext.FinishBatch();
			}
		}

		/// <summary>
		/// Find a type amongst known assemblies.
		/// </summary>
		/// <param name="fullName">The full name of the type (i.e.: namespace + name).</param>
		/// <param name="throwOnError">Whether to throw exception or to return null if the type cannot be found.</param>
		/// <returns>The fount type or null.</returns>
		public static Type GetType(string fullName, bool throwOnError)
		{
			Type type = Type.GetType(fullName, throwOnError: false);
			if (type != null)
			{
				return type;
			}
			for (int i = 0; i < Stack.Count; i++)
			{
				foreach (Assembly item in Stack[i].Known)
				{
					if ((type = item.GetType(fullName, throwOnError: false)) != null)
					{
						return type;
					}
				}
			}
			if (throwOnError)
			{
				throw new TypeLoadException($"Type {fullName} was not found in any registered assembly!");
			}
			return null;
		}

		/// <summary>
		/// Push the provided metadata context to the top of the metadata text.
		/// </summary>
		/// <returns></returns>
		public static void PushMetadataContext(MyMetadataContext context)
		{
			context.AddIndexers(AttributeIndexers);
			context.AddIndexers(TypeIndexers);
			MyMetadataContext activeContext = ActiveContext;
			if (activeContext != null)
			{
				context.Hook(activeContext);
			}
			Stack.Add(context);
		}

		/// <summary>
		/// Create a new metadata context and set push it atop the metadata stack.
		/// </summary>
		/// <returns>The newly created context.</returns>
		public static MyMetadataContext PushMetadataContext()
		{
			MyMetadataContext myMetadataContext = new MyMetadataContext();
			PushMetadataContext(myMetadataContext);
			return myMetadataContext;
		}

		/// <summary>
		/// Pop the current metadata context, this will cause the context to be disposed.
		///
		/// This should update all global indexer references to the new context on the top of the stack.
		/// </summary>
		public static void PopContext()
		{
			if (Stack.Count == 0)
			{
				Log.Error("When popping metadata context: No context set.");
			}
			else
			{
				Stack.Pop().Close();
			}
		}

		/// <summary>
		/// Register an indexer for a given attribute type.
		///
		/// The provide indexer will be registered to all current and future metadata contexts.
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="indexerType"></param>
		public static void RegisterAttributeIndexer(Type attributeType, Type indexerType)
		{
			if (!typeof(IMyAttributeIndexer).IsAssignableFrom(indexerType))
			{
				Log.Error("Cannot register metadata indexer {0}, the type is not a IMyMetadataIndexer.", indexerType);
				return;
			}
			if (!indexerType.HasDefaultConstructor())
			{
				Log.Error("Cannot register metadata indexer {0}, the type does not define a parameterless constructor.", indexerType);
				return;
			}
			if (!typeof(Attribute).IsAssignableFrom(attributeType))
			{
				Log.Error("Cannot register metadata indexer {0}, the indexed attribute {1} is not actually an attribute.", indexerType, attributeType);
				return;
			}
			AttributeIndexers.Add(attributeType, indexerType);
			foreach (MyMetadataContext item in Stack)
			{
				item.AddIndexer(attributeType, indexerType);
			}
		}

		/// <summary>
		/// Register an indexer for a given attribute type.
		///
		/// The provide indexer will be registered to all current and future metadata contexts.
		/// </summary>
		/// <param name="indexerType"></param>
		public static void RegisterTypeIndexer(Type indexerType)
		{
			if (!typeof(IMyTypeIndexer).IsAssignableFrom(indexerType))
			{
				Log.Error("Cannot register metadata indexer {0}, the type is not a IMyMetadataIndexer.", indexerType);
				return;
			}
			if (!indexerType.HasDefaultConstructor())
			{
				Log.Error("Cannot register metadata indexer {0}, the type does not define a parameterless constructor.", indexerType);
				return;
			}
			TypeIndexers.Add(indexerType);
			foreach (MyMetadataContext item in Stack)
			{
				item.AddIndexer(indexerType);
			}
		}
	}
}
