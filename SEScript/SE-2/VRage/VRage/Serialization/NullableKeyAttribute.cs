using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class NullableKeyAttribute : SerializeAttribute
	{
		public NullableKeyAttribute()
		{
			Flags = MyObjectFlags.DefaultZero;
			Kind = MySerializeKind.Key;
		}
	}
}
