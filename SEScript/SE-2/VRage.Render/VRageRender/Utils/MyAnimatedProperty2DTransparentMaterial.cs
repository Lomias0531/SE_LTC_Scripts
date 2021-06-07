using System.Xml;
using VRageRender.Animations;

namespace VRageRender.Utils
{
	public class MyAnimatedProperty2DTransparentMaterial : MyAnimatedProperty2D<MyAnimatedPropertyTransparentMaterial, MyTransparentMaterial, int>
	{
		public override string ValueType => "String";

		public override string BaseValueType => "MyTransparentMaterial";

		public MyAnimatedProperty2DTransparentMaterial(string name)
			: this(name, null)
		{
		}

		public MyAnimatedProperty2DTransparentMaterial(string name, MyAnimatedProperty<MyTransparentMaterial>.InterpolatorDelegate interpolator)
			: base(name, interpolator)
		{
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyTransparentMaterial myAnimatedPropertyTransparentMaterial = new MyAnimatedPropertyTransparentMaterial(base.Name, m_interpolator2);
			myAnimatedPropertyTransparentMaterial.Deserialize(reader);
			value = myAnimatedPropertyTransparentMaterial;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedProperty2DTransparentMaterial myAnimatedProperty2DTransparentMaterial = new MyAnimatedProperty2DTransparentMaterial(base.Name);
			Duplicate(myAnimatedProperty2DTransparentMaterial);
			return myAnimatedProperty2DTransparentMaterial;
		}

		public override void ApplyVariance(ref MyTransparentMaterial interpolatedValue, ref int variance, float multiplier, out MyTransparentMaterial value)
		{
			value = interpolatedValue;
		}
	}
}
