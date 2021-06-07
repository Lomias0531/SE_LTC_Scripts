using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class NullableItemAttribute : SerializeAttribute
	{
		public NullableItemAttribute()
		{
			Flags = MyObjectFlags.DefaultZero;
			Kind = MySerializeKind.Item;
		}
	}
}
