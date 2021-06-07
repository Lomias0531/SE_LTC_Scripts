using System;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.AI.BehaviorTree
{
	internal static class MyBehaviorTreeNodeMemoryFactory
	{
		private static MyObjectFactory<MyBehaviorTreeNodeMemoryTypeAttribute, MyBehaviorTreeNodeMemory> m_objectFactory;

		static MyBehaviorTreeNodeMemoryFactory()
		{
			m_objectFactory = new MyObjectFactory<MyBehaviorTreeNodeMemoryTypeAttribute, MyBehaviorTreeNodeMemory>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyBehaviorTreeNodeMemory)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyBehaviorTreeNodeMemory CreateNodeMemory(MyObjectBuilder_BehaviorTreeNodeMemory builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}

		public static MyObjectBuilder_BehaviorTreeNodeMemory CreateObjectBuilder(MyBehaviorTreeNodeMemory cubeBlock)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_BehaviorTreeNodeMemory>(cubeBlock);
		}

		public static Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			return m_objectFactory.GetProducedType(objectBuilderType);
		}
	}
}
