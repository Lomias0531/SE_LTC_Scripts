namespace VRage.ObjectBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using VRage;
    using VRage.Collections;

    public class MyObjectFactory<TAttribute, TCreatedObjectBase> where TAttribute: MyFactoryTagAttribute where TCreatedObjectBase: class
    {
        private readonly Dictionary<Type, TAttribute> m_attributesByProducedType;
        private readonly Dictionary<Type, TAttribute> m_attributesByObjectBuilder;
        private readonly FastResourceLock m_activatorsLock;
        private readonly Dictionary<Type, Func<object>> m_activators;

        public MyObjectFactory()
        {
            this.m_attributesByProducedType = new Dictionary<Type, TAttribute>();
            this.m_attributesByObjectBuilder = new Dictionary<Type, TAttribute>();
            this.m_activatorsLock = new FastResourceLock();
            this.m_activators = new Dictionary<Type, Func<object>>();
        }

        public TBase CreateInstance<TBase>() where TBase: class, TCreatedObjectBase, new() => 
            this.CreateInstance<TBase>((MyObjectBuilderType) typeof(TBase));

        public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType) where TBase: class, TCreatedObjectBase
        {
            if (!this.m_attributesByObjectBuilder.TryGetValue((Type) objectBuilderType, out TAttribute local))
            {
                return default(TBase);
            }
            using (this.m_activatorsLock.AcquireSharedUsing())
            {
                this.m_activators.TryGetValue(local.ProducedType, out Func<object> func);
            }
            if (func == null)
            {
                using (this.m_activatorsLock.AcquireExclusiveUsing())
                {
                    if (!this.m_activators.TryGetValue(local.ProducedType, out func))
                    {
                        func = ExpressionExtension.CreateActivator<object>(local.ProducedType);
                        this.m_activators.Add(local.ProducedType, func);
                    }
                }
            }
            return (func() as TBase);
        }

        public TCreatedObjectBase CreateInstance(MyObjectBuilderType objectBuilderType) => 
            this.CreateInstance<TCreatedObjectBase>(objectBuilderType);

        public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder: MyObjectBuilder_Base => 
            this.CreateObjectBuilder<TObjectBuilder>(instance.GetType());

        public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder: MyObjectBuilder_Base
        {
            if (!this.m_attributesByProducedType.TryGetValue(instanceType, out TAttribute local))
            {
                return default(TObjectBuilder);
            }
            return (MyObjectBuilderSerializer.CreateNewObject(local.ObjectBuilderType) as TObjectBuilder);
        }

        public Type GetProducedType(MyObjectBuilderType objectBuilderType) => 
            this.m_attributesByObjectBuilder[(Type) objectBuilderType].ProducedType;

        public void RegisterDescriptor(TAttribute descriptor, Type type)
        {
            descriptor.ProducedType = type;
            if (descriptor.IsMain)
            {
                this.m_attributesByProducedType.Add(descriptor.ProducedType, descriptor);
            }
            if (descriptor.ObjectBuilderType != null)
            {
                this.m_attributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
            }
            else if (typeof(MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
            {
                this.m_attributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
            }
        }

        public void RegisterFromAssembly(Assembly[] assemblies)
        {
            if (assemblies != null)
            {
                foreach (Assembly assembly in assemblies)
                {
                    this.RegisterFromAssembly(assembly);
                }
            }
        }

        public void RegisterFromAssembly(Assembly assembly)
        {
            if (assembly != null)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (TAttribute local in type.GetCustomAttributes((Type) typeof(TAttribute), false))
                    {
                        this.RegisterDescriptor(local, type);
                    }
                }
            }
        }

        public void RegisterFromCreatedObjectAssembly()
        {
            Assembly assembly = Assembly.GetAssembly((Type) typeof(TCreatedObjectBase));
            this.RegisterFromAssembly(assembly);
        }

        public Type TryGetProducedType(MyObjectBuilderType objectBuilderType)
        {
            TAttribute local = default(TAttribute);
            if (!this.m_attributesByObjectBuilder.TryGetValue((Type) objectBuilderType, out local))
            {
                return null;
            }
            return local.ProducedType;
        }

        public DictionaryValuesReader<Type, TAttribute> Attributes =>
            new DictionaryValuesReader<Type, TAttribute>(this.m_attributesByProducedType);
    }
}

