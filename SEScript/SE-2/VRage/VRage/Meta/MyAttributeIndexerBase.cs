using System;
using System.Collections.Generic;

namespace VRage.Meta
{
	/// <summary>
	/// Base class for a sample implementation of an attribute observer.
	///
	/// Sufficient for most usages.
	/// </summary>
	/// <typeparam name="TAttribute"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public class MyAttributeIndexerBase<TAttribute, TKey> : IMyAttributeIndexer, IMyMetadataIndexer where TAttribute : Attribute, IMyKeyAttribute<TKey>
	{
		protected Dictionary<TKey, Type> IndexedTypes = new Dictionary<TKey, Type>();

		protected MyAttributeIndexerBase<TAttribute, TKey> Parent;

		/// <summary>
		/// Static reference to the topmost indexer.
		/// </summary>
		public static MyAttributeIndexerBase<TAttribute, TKey> Static;

		public bool TryGetType(TKey key, out Type indexedType)
		{
			if (IndexedTypes.TryGetValue(key, out indexedType))
			{
				return true;
			}
			if (Parent != null)
			{
				return Parent.TryGetType(key, out indexedType);
			}
			return false;
		}

		public virtual void SetParent(IMyMetadataIndexer indexer)
		{
			Parent = (MyAttributeIndexerBase<TAttribute, TKey>)indexer;
		}

		public virtual void Activate()
		{
			Static = this;
		}

		public virtual void Close()
		{
			IndexedTypes.Clear();
		}

		public virtual void Process()
		{
		}

		public virtual void Observe(Attribute attribute, Type type)
		{
			Observe((TAttribute)attribute, type);
		}

		protected virtual void Observe(TAttribute attribute, Type type)
		{
			IndexedTypes.Add(attribute.Key, type);
		}
	}
}
