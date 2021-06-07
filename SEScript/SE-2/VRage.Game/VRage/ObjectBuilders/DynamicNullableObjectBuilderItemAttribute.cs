using VRage.Serialization;

namespace VRage.ObjectBuilders
{
	public class DynamicNullableObjectBuilderItemAttribute : DynamicNullableItemAttribute
	{
		public DynamicNullableObjectBuilderItemAttribute(bool defaultTypeCommon = false)
			: base(typeof(MyObjectBuilderDynamicSerializer), defaultTypeCommon)
		{
		}
	}
}
