using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EventAttribute : Attribute
	{
		public readonly int Order;

		public readonly string Serialization;

		public EventAttribute(string serializationMethod = null, [CallerLineNumber] int order = 0)
		{
			Order = order;
			Serialization = serializationMethod;
		}
	}
}
