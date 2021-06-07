using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NoSerializeAttribute : Attribute
	{
	}
}
