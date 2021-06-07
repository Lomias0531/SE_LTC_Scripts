using System;

namespace VRage.Game.Components
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class MyComponentTypeAttribute : Attribute
	{
		public readonly Type ComponentType;

		public MyComponentTypeAttribute(Type componentType)
		{
			ComponentType = componentType;
		}
	}
}
