using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class MyBehaviorTreeActionAttribute : Attribute
	{
		public readonly string ActionName;

		public readonly MyBehaviorTreeActionType ActionType;

		public bool ReturnsRunning;

		public MyBehaviorTreeActionAttribute(string actionName)
			: this(actionName, MyBehaviorTreeActionType.BODY)
		{
		}

		public MyBehaviorTreeActionAttribute(string actionName, MyBehaviorTreeActionType type)
		{
			ActionName = actionName;
			ActionType = type;
			ReturnsRunning = true;
		}
	}
}
