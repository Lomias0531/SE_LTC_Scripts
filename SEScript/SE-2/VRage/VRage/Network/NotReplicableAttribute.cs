using System;

namespace VRage.Network
{
	/// <summary>
	/// Marks class which should be never replicated.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class NotReplicableAttribute : Attribute
	{
	}
}
