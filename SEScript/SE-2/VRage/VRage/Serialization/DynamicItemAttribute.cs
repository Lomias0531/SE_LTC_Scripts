using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class DynamicItemAttribute : SerializeAttribute
	{
		public DynamicItemAttribute(Type dynamicSerializerType, bool defaultTypeCommon = false)
		{
			Flags = (defaultTypeCommon ? MyObjectFlags.DynamicDefault : MyObjectFlags.Dynamic);
			DynamicSerializerType = dynamicSerializerType;
			Kind = MySerializeKind.Item;
		}
	}
}
