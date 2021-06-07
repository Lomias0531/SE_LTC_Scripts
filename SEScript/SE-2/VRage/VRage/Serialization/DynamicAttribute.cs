using System;

namespace VRage.Serialization
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class DynamicAttribute : SerializeAttribute
	{
		public DynamicAttribute(Type dynamicSerializerType, bool defaultTypeCommon = false)
		{
			Flags = (defaultTypeCommon ? MyObjectFlags.DynamicDefault : MyObjectFlags.Dynamic);
			DynamicSerializerType = dynamicSerializerType;
		}
	}
}
