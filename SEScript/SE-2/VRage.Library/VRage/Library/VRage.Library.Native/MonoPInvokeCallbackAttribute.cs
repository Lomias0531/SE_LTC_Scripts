using System;

namespace VRage.Library.Native
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MonoPInvokeCallbackAttribute : Attribute
	{
		public MonoPInvokeCallbackAttribute(Type t)
		{
		}
	}
}
