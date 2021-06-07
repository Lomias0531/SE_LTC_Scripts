using System;
using System.Reflection;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace VRage.Game.Components
{
	[PreloadRequired]
	public static class MyComponentFactory
	{
		private static MyObjectFactory<MyComponentBuilderAttribute, MyComponentBase> m_objectFactory;

		static MyComponentFactory()
		{
			m_objectFactory = new MyObjectFactory<MyComponentBuilderAttribute, MyComponentBase>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetExecutingAssembly());
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxGameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyComponentBase CreateInstanceByTypeId(MyObjectBuilderType type)
		{
			return m_objectFactory.CreateInstance(type);
		}

		public static MyObjectBuilder_ComponentBase CreateObjectBuilder(MyComponentBase instance)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ComponentBase>(instance);
		}

		public static MyComponentBase CreateInstanceByType(Type type)
		{
			if (type.IsAssignableFrom(typeof(MyComponentBase)))
			{
				return Activator.CreateInstance(type) as MyComponentBase;
			}
			return null;
		}

		public static Type GetCreatedInstanceType(MyObjectBuilderType type)
		{
			return m_objectFactory.GetProducedType(type);
		}

		public static Type TryGetCreatedInstanceType(MyObjectBuilderType type)
		{
			return m_objectFactory.TryGetProducedType(type);
		}
	}
}
