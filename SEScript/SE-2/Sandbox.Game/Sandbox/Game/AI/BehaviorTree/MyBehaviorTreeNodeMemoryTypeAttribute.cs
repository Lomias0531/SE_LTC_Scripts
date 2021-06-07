using System;
using VRage.Game.Common;

namespace Sandbox.Game.AI.BehaviorTree
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MyBehaviorTreeNodeMemoryTypeAttribute : MyFactoryTagAttribute
	{
		public MyBehaviorTreeNodeMemoryTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
