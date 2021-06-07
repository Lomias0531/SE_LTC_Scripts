using System;

namespace Sandbox.ModAPI
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MyTerminalInterfaceAttribute : Attribute
	{
		public readonly Type[] LinkedTypes;

		public MyTerminalInterfaceAttribute(params Type[] linkedTypes)
		{
			LinkedTypes = linkedTypes;
		}
	}
}
