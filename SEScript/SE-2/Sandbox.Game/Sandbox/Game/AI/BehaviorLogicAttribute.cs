using System;

namespace Sandbox.Game.AI
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class BehaviorLogicAttribute : Attribute
	{
		public readonly string BehaviorSubtype;

		public BehaviorLogicAttribute(string behaviorSubtype)
		{
			BehaviorSubtype = behaviorSubtype;
		}
	}
}
