using System;

namespace Sandbox.Game.AI
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class BehaviorActionImplAttribute : Attribute
	{
		public readonly Type LogicType;

		public BehaviorActionImplAttribute(Type logicType)
		{
			LogicType = logicType;
		}
	}
}
