using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class DynamicKeyAttribute : SerializeAttribute
	{
		public DynamicKeyAttribute(Type dynamicSerializerType, bool defaultTypeCommon = false)
		{
			Flags = (defaultTypeCommon ? MyObjectFlags.DynamicDefault : MyObjectFlags.Dynamic);
			DynamicSerializerType = dynamicSerializerType;
			Kind = MySerializeKind.Key;
		}
	}
}
