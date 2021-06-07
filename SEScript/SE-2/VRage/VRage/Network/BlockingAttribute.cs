using System;

namespace VRage.Network
{
	/// <summary>
	/// Indicates that event will be blocking all other events.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class BlockingAttribute : Attribute
	{
	}
}
