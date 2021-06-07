using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class DynamicNullableItemAttribute : SerializeAttribute
	{
		public DynamicNullableItemAttribute(Type dynamicSerializerType, bool defaultTypeCommon = false)
		{
			Flags = (defaultTypeCommon ? MyObjectFlags.DynamicDefault : MyObjectFlags.Dynamic);
			Flags |= MyObjectFlags.DefaultZero;
			DynamicSerializerType = dynamicSerializerType;
			Kind = MySerializeKind.Item;
		}
	}
}
