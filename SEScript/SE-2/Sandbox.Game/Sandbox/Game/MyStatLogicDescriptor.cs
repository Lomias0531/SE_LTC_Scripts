using System;

namespace Sandbox.Game
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MyStatLogicDescriptor : Attribute
	{
		public string ComponentName;

		public MyStatLogicDescriptor(string componentName)
		{
			ComponentName = componentName;
		}
	}
}
