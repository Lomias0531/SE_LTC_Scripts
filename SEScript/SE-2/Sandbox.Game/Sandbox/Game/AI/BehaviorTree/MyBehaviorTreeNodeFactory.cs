using System;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.AI.BehaviorTree
{
	internal static class MyBehaviorTreeNodeFactory
	{
		private static MyObjectFactory<MyBehaviorTreeNodeTypeAttribute, MyBehaviorTreeNode> m_objectFactory;

		static MyBehaviorTreeNodeFactory()
		{
			m_objectFactory = new MyObjectFactory<MyBehaviorTreeNodeTypeAttribute, MyBehaviorTreeNode>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyBehaviorTreeNode)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyBehaviorTreeNode CreateBTNode(MyObjectBuilder_BehaviorTreeNode builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}

		public static Type GetProducedType(MyObjectBuilderType objectBuilderType)
		{
			return m_objectFactory.GetProducedType(objectBuilderType);
		}
	}
}
