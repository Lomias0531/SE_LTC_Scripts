using System;

namespace VRage.Network
{
	/// <summary>
	/// Event which is sent reliably, use with caution and only when necessary!
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ReliableAttribute : Attribute
	{
	}
}
