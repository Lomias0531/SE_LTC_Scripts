namespace VRage.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using VRage.Meta;
    using VRage.ObjectBuilders;
    using VRage.Utils;

    public class MyObjectFactory<TAttribute, TCreatedObjectBase> : IMyAttributeIndexer, IMyMetadataIndexer where TAttribute: MyFactoryTagAttribute where TCreatedObjectBase: class
    {
        protected Dictionary<Type, TAttribute> AttributesByProducedType;
        protected Dictionary<Type, TAttribute> AttributesByObjectBuilder;
        protected VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase> Parent;
        private static VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase> m_instance;

        public MyObjectFactory()
        {
            this.AttributesByProducedType = new Dictionary<Type, TAttribute>();
            this.AttributesByObjectBuilder = new Dictionary<Type, TAttribute>();
        }

        public virtual void Activate()
        {
            VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase>.m_instance = (VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase>) this;
        }

        public virtual void Close()
        {
            this.AttributesByObjectBuilder.Clear();
            this.AttributesByProducedType.Clear();
        }

        public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType) where TBase: class, TCreatedObjectBase
        {
            if (!this.TryGetProducedType(objectBuilderType, out Type type))
            {
                return default(TBase);
            }
            object obj1 = Activator.CreateInstance(type);
            TBase local = obj1 as TBase;
            if ((obj1 != null) && (local == null))
            {
                object[] args = new object[] { objectBuilderType.FullName, typeof(TBase).FullName };
                MyLog.Default.Critical("Factory product {0} is not an instance of {1}", args);
                return default(TBase);
            }
            return local;
        }

        public TCreatedObjectBase CreateInstance(MyObjectBuilderType objectBuilderType) => 
            this.CreateInstance<TCreatedObjectBase>(objectBuilderType);

        public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder: MyObjectBuilder_Base => 
            this.CreateObjectBuilder<TObjectBuilder>(instance.GetType());

        public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder: MyObjectBuilder_Base
        {
            if (!this.TryGetAttribute(instanceType, out TAttribute local))
            {
                return default(TObjectBuilder);
            }
            return (MyObjectBuilderSerializer.CreateNewObject(local.ObjectBuilderType) as TObjectBuilder);
        }

        public static VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase> Get() => 
            VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase>.m_instance;

        protected TAttribute GetAttribute(Type instanceType, bool inherited = false)
        {
            if (inherited)
            {
                TAttribute attr = default(TAttribute);
                while ((instanceType != null) && !this.TryGetAttribute(instanceType, out attr))
                {
                    instanceType = instanceType.get_BaseType();
                }
                return attr;
            }
            this.TryGetAttribute(instanceType, out TAttribute local2);
            return local2;
        }

        public MyObjectBuilderType GetObjectBuilderType(Type type)
        {
            if (this.TryGetAttribute(type, out TAttribute local))
            {
                return local.ObjectBuilderType;
            }
            return 0;
        }

        public Type GetProducedType(MyObjectBuilderType objectBuilderType)
        {
            if (this.TryGetAttribute(objectBuilderType, out TAttribute local))
            {
                return local.ProducedType;
            }
            return null;
        }

        public virtual void Observe(Attribute attribute, Type type)
        {
            this.RegisterDescriptor((TAttribute) attribute, type);
        }

        public virtual void Process()
        {
        }

        protected virtual void RegisterDescriptor(TAttribute descriptor, Type type)
        {
            descriptor.ProducedType = type;
            if (!typeof(TCreatedObjectBase).IsAssignableFrom(type))
            {
                object[] args = new object[] { type, typeof(TAttribute), typeof(TCreatedObjectBase) };
                MyLog.Default.Critical("Type {0} cannot have factory tag attribute {1}, because it's not assignable to {2}", args);
            }
            else
            {
                if (descriptor.IsMain)
                {
                    if (this.AttributesByProducedType.TryGetValue(descriptor.ProducedType, out TAttribute local))
                    {
                        MyLog.Default.Critical($"Duplicate factory tag attribute {typeof(TAttribute)} on type {type}. Either remove the duplicate instances or mark only one of the attributes as the main one main.", Array.Empty<object>());
                        return;
                    }
                    this.AttributesByProducedType.Add(descriptor.ProducedType, descriptor);
                }
                if (descriptor.ObjectBuilderType != null)
                {
                    if (this.AttributesByObjectBuilder.TryGetValue(descriptor.ObjectBuilderType, out local))
                    {
                        object[] args = new object[] { descriptor.ObjectBuilderType, descriptor.ProducedType, local.ProducedType };
                        MyLog.Default.Critical("Cannot associate OB {0} with type {1} because it's already associated with {2}.", args);
                    }
                    else
                    {
                        this.AttributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
                    }
                }
                else if (typeof(MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
                {
                    this.AttributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
                }
            }
        }

        [Obsolete]
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

        [Obsolete]
        public void RegisterFromCreatedObjectAssembly()
        {
        }

        public virtual void SetParent(IMyMetadataIndexer indexer)
        {
            this.Parent = (VRage.Factory.MyObjectFactory<TAttribute, TCreatedObjectBase>) indexer;
        }

        public bool TryGetAttribute(Type instanceType, out TAttribute attr) => 
            (this.AttributesByProducedType.TryGetValue(instanceType, out attr) || ((this.Parent != null) && this.Parent.TryGetAttribute(instanceType, out attr)));

        public bool TryGetAttribute(MyObjectBuilderType builderType, out TAttribute attr) => 
            (this.AttributesByObjectBuilder.TryGetValue((Type) builderType, out attr) || ((this.Parent != null) && this.Parent.TryGetAttribute(builderType, out attr)));

        public bool TryGetObjectBuilderType(Type type, out MyObjectBuilderType objectBuilderType)
        {
            if (this.TryGetAttribute(type, out TAttribute local))
            {
                objectBuilderType = local.ObjectBuilderType;
                return true;
            }
            objectBuilderType = 0;
            return false;
        }

        public bool TryGetProducedType(MyObjectBuilderType objectBuilderType, out Type type)
        {
            if (this.TryGetAttribute(objectBuilderType, out TAttribute local))
            {
                type = local.ProducedType;
                return true;
            }
            type = null;
            return false;
        }

        public IEnumerable<TAttribute> Attributes
        {
            get
            {
                if (this.Parent != null)
                {
                    return Enumerable.Concat<TAttribute>((IEnumerable<TAttribute>) this.AttributesByProducedType.get_Values(), this.Parent.Attributes);
                }
                return (IEnumerable<TAttribute>) this.AttributesByProducedType.get_Values();
            }
        }
    }
}

