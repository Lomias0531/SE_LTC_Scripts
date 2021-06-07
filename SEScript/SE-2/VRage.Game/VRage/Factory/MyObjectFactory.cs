using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Game.Common;
using VRage.Meta;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Factory
{
	/// <summary>
	/// Base class for automatic object factory.
	///
	/// Object factories are created and set up automatically.
	/// </summary>
	/// <typeparam name="TAttribute"></typeparam>
	/// <typeparam name="TCreatedObjectBase"></typeparam>
	public class MyObjectFactory<TAttribute, TCreatedObjectBase> : IMyAttributeIndexer, IMyMetadataIndexer where TAttribute : MyFactoryTagAttribute where TCreatedObjectBase : class
	{
		protected Dictionary<Type, TAttribute> AttributesByProducedType = new Dictionary<Type, TAttribute>();

		protected Dictionary<Type, TAttribute> AttributesByObjectBuilder = new Dictionary<Type, TAttribute>();

		protected MyObjectFactory<TAttribute, TCreatedObjectBase> Parent;

		private static MyObjectFactory<TAttribute, TCreatedObjectBase> m_instance;

		/// <summary>
		/// Enumerate all know attributes. And therefore all indexed types.
		/// </summary>
		public IEnumerable<TAttribute> Attributes
		{
			get
			{
				if (Parent != null)
				{
					return AttributesByProducedType.Values.Concat(Parent.Attributes);
				}
				return AttributesByProducedType.Values;
			}
		}

		/// <summary>
		/// Register a type with descriptor.
		/// </summary>
		/// <param name="descriptor">Descriptor</param>
		/// <param name="type">Type</param>
		protected virtual void RegisterDescriptor(TAttribute descriptor, Type type)
		{
			descriptor.ProducedType = type;
			if (!typeof(TCreatedObjectBase).IsAssignableFrom(type))
			{
				MyLog.Default.Critical("Type {0} cannot have factory tag attribute {1}, because it's not assignable to {2}", type, typeof(TAttribute), typeof(TCreatedObjectBase));
				return;
			}
			TAttribute value;
			if (descriptor.IsMain)
			{
				if (AttributesByProducedType.TryGetValue(descriptor.ProducedType, out value))
				{
					MyLog.Default.Critical($"Duplicate factory tag attribute {typeof(TAttribute)} on type {type}. Either remove the duplicate instances or mark only one of the attributes as the main one main.");
					return;
				}
				AttributesByProducedType.Add(descriptor.ProducedType, descriptor);
			}
			if (descriptor.ObjectBuilderType != null)
			{
				if (AttributesByObjectBuilder.TryGetValue(descriptor.ObjectBuilderType, out value))
				{
					MyLog.Default.Critical("Cannot associate OB {0} with type {1} because it's already associated with {2}.", descriptor.ObjectBuilderType, descriptor.ProducedType, value.ProducedType);
				}
				else
				{
					AttributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
				}
			}
			else if (typeof(MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
			{
				AttributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
			}
		}

		/// <summary>
		/// Create default instance from object builder.
		/// </summary>
		/// <param name="objectBuilderType">The object builder type that maps to the object we want to create.</param>
		/// <returns></returns>
		public TCreatedObjectBase CreateInstance(MyObjectBuilderType objectBuilderType)
		{
			return CreateInstance<TCreatedObjectBase>(objectBuilderType);
		}

		public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType) where TBase : class, TCreatedObjectBase
		{
			if (!TryGetProducedType(objectBuilderType, out Type type))
			{
				return null;
			}
			object obj = Activator.CreateInstance(type);
			TBase val = obj as TBase;
			if (obj != null && val == null)
			{
				MyLog.Default.Critical("Factory product {0} is not an instance of {1}", ((Type)objectBuilderType).FullName, typeof(TBase).FullName);
				return null;
			}
			return val;
		}

		public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder : MyObjectBuilder_Base
		{
			return CreateObjectBuilder<TObjectBuilder>(instance.GetType());
		}

		public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder : MyObjectBuilder_Base
		{
			if (!TryGetAttribute(instanceType, out TAttribute attr))
			{
				return null;
			}
			return MyObjectBuilderSerializer.CreateNewObject(attr.ObjectBuilderType) as TObjectBuilder;
		}

		public Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			if (TryGetAttribute(objectBuilderType, out TAttribute attr))
			{
				return attr.ProducedType;
			}
			return null;
		}

		public MyObjectBuilderType GetObjectBuilderType(Type type)
		{
			if (TryGetAttribute(type, out TAttribute attr))
			{
				return attr.ObjectBuilderType;
			}
			return null;
		}

		public bool TryGetProducedType(MyObjectBuilderType objectBuilderType, out Type type)
		{
			if (TryGetAttribute(objectBuilderType, out TAttribute attr))
			{
				type = attr.ProducedType;
				return true;
			}
			type = null;
			return false;
		}

		public bool TryGetObjectBuilderType(Type type, out MyObjectBuilderType objectBuilderType)
		{
			if (TryGetAttribute(type, out TAttribute attr))
			{
				objectBuilderType = attr.ObjectBuilderType;
				return true;
			}
			objectBuilderType = null;
			return false;
		}

		/// <summary>
		/// Get an attribute for it's generated type.
		///
		/// This may optionally check for attributes from parent classes in case the factory attribute allows inheritance.
		/// </summary>
		/// <param name="instanceType">The type to query.</param>
		/// <param name="inherited">Check for inheritance</param>
		/// <returns></returns>
		protected TAttribute GetAttribute(Type instanceType, bool inherited = false)
		{
			if (inherited)
			{
				TAttribute attr = null;
				while (instanceType != null && !TryGetAttribute(instanceType, out attr))
				{
					instanceType = instanceType.BaseType;
				}
				return attr;
			}
			TryGetAttribute(instanceType, out TAttribute attr2);
			return attr2;
		}

		public bool TryGetAttribute(Type instanceType, out TAttribute attr)
		{
			if (AttributesByProducedType.TryGetValue(instanceType, out attr))
			{
				return true;
			}
			if (Parent != null)
			{
				return Parent.TryGetAttribute(instanceType, out attr);
			}
			return false;
		}

		public bool TryGetAttribute(MyObjectBuilderType builderType, out TAttribute attr)
		{
			if (AttributesByObjectBuilder.TryGetValue(builderType, out attr))
			{
				return true;
			}
			if (Parent != null)
			{
				return Parent.TryGetAttribute(builderType, out attr);
			}
			return false;
		}

		public static MyObjectFactory<TAttribute, TCreatedObjectBase> Get()
		{
			return m_instance;
		}

		[Obsolete]
		public void RegisterFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(TAttribute), inherit: false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					TAttribute descriptor = (TAttribute)customAttributes[j];
					RegisterDescriptor(descriptor, type);
				}
			}
		}

		[Obsolete]
		public void RegisterFromCreatedObjectAssembly()
		{
		}

		public virtual void SetParent(IMyMetadataIndexer indexer)
		{
			Parent = (MyObjectFactory<TAttribute, TCreatedObjectBase>)indexer;
		}

		public virtual void Activate()
		{
			m_instance = this;
		}

		public virtual void Observe(Attribute attribute, Type type)
		{
			RegisterDescriptor((TAttribute)attribute, type);
		}

		public virtual void Close()
		{
			AttributesByObjectBuilder.Clear();
			AttributesByProducedType.Clear();
		}

		public virtual void Process()
		{
		}
	}
}
