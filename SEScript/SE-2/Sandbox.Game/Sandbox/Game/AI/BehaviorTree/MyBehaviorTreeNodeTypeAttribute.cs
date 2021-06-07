using System;
using VRage.Game.Common;

namespace Sandbox.Game.AI.BehaviorTree
{
	public class MyBehaviorTreeNodeTypeAttribute : MyFactoryTagAttribute
	{
		public readonly Type MemoryType;

		public MyBehaviorTreeNodeTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
			MemoryType = typeof(MyBehaviorTreeNodeMemory);
		}

		public MyBehaviorTreeNodeTypeAttribute(Type objectBuilderType, Type memoryType)
			: base(objectBuilderType)
		{
			MemoryType = memoryType;
		}
	}
}
