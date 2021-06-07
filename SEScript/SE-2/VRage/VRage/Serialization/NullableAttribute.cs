using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class NullableAttribute : SerializeAttribute
	{
		public NullableAttribute()
		{
			Flags = MyObjectFlags.DefaultZero;
		}
	}
}
