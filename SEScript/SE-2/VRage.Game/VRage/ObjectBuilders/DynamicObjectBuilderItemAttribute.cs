using VRage.Serialization;

namespace VRage.ObjectBuilders
{
	public class DynamicObjectBuilderItemAttribute : DynamicItemAttribute
	{
		public DynamicObjectBuilderItemAttribute(bool defaultTypeCommon = false)
			: base(typeof(MyObjectBuilderDynamicSerializer), defaultTypeCommon)
		{
		}
	}
}
