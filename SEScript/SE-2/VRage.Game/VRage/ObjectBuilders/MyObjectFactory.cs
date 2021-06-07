using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Collections;
using VRage.Game.Common;

namespace VRage.ObjectBuilders
{
	public class MyObjectFactory<TAttribute, TCreatedObjectBase> where TAttribute : MyFactoryTagAttribute where TCreatedObjectBase : class
	{
		private readonly Dictionary<Type, TAttribute> m_attributesByProducedType = new Dictionary<Type, TAttribute>();

		private readonly Dictionary<Type, TAttribute> m_attributesByObjectBuilder = new Dictionary<Type, TAttribute>();

		private readonly FastResourceLock m_activatorsLock = new FastResourceLock();

		private readonly Dictionary<Type, Func<object>> m_activators = new Dictionary<Type, Func<object>>();

		public DictionaryValuesReader<Type, TAttribute> Attributes => new DictionaryValuesReader<Type, TAttribute>(m_attributesByProducedType);

		public void RegisterFromCreatedObjectAssembly()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(TCreatedObjectBase));
			RegisterFromAssembly(assembly);
		}

		public void RegisterDescriptor(TAttribute descriptor, Type type)
		{
			descriptor.ProducedType = type;
			if (descriptor.IsMain)
			{
				m_attributesByProducedType.Add(descriptor.ProducedType, descriptor);
			}
			if (descriptor.ObjectBuilderType != null)
			{
				m_attributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
			}
			else if (typeof(MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
			{
				m_attributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
			}
		}

		public void RegisterFromAssembly(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					RegisterFromAssembly(assembly);
				}
			}
		}

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

		public TCreatedObjectBase CreateInstance(MyObjectBuilderType objectBuilderType)
		{
			return CreateInstance<TCreatedObjectBase>(objectBuilderType);
		}

		public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType) where TBase : class, TCreatedObjectBase
		{
			if (m_attributesByObjectBuilder.TryGetValue(objectBuilderType, out TAttribute value))
			{
				Func<object> value2;
				using (m_activatorsLock.AcquireSharedUsing())
				{
					m_activators.TryGetValue(value.ProducedType, out value2);
				}
				if (value2 == null)
				{
					using (m_activatorsLock.AcquireExclusiveUsing())
					{
						if (!m_activators.TryGetValue(value.ProducedType, out value2))
						{
							value2 = ExpressionExtension.CreateActivator<object>(value.ProducedType);
							m_activators.Add(value.ProducedType, value2);
						}
					}
				}
				return value2() as TBase;
			}
			return null;
		}

		public TBase CreateInstance<TBase>() where TBase : class, TCreatedObjectBase, new()
		{
			return CreateInstance<TBase>(typeof(TBase));
		}

		public Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			return m_attributesByObjectBuilder[objectBuilderType].ProducedType;
		}

		public Type TryGetProducedType(MyObjectBuilderType objectBuilderType)
		{
			TAttribute value = null;
			if (!m_attributesByObjectBuilder.TryGetValue(objectBuilderType, out value))
			{
				return null;
			}
			return value.ProducedType;
		}

		public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder : MyObjectBuilder_Base
		{
			return CreateObjectBuilder<TObjectBuilder>(instance.GetType());
		}

		public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder : MyObjectBuilder_Base
		{
			if (!m_attributesByProducedType.TryGetValue(instanceType, out TAttribute value))
			{
				return null;
			}
			return MyObjectBuilderSerializer.CreateNewObject(value.ObjectBuilderType) as TObjectBuilder;
		}
	}
}
