using System;

namespace Sandbox.Game.AI
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class TargetTypeAttribute : Attribute
	{
		public readonly string TargetType;

		public TargetTypeAttribute(string targetType)
		{
			TargetType = targetType;
		}
	}
}
