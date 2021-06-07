using VRage.Serialization;

namespace VRage.ObjectBuilders
{
	public class DynamicObjectBuilderAttribute : DynamicAttribute
	{
		public DynamicObjectBuilderAttribute(bool defaultTypeCommon = false)
			: base(typeof(MyObjectBuilderDynamicSerializer), defaultTypeCommon)
		{
		}
	}
}
