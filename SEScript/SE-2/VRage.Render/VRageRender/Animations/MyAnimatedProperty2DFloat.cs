using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2DFloat : MyAnimatedProperty2D<MyAnimatedPropertyFloat, float, float>
	{
		public override string ValueType => "Float";

		public MyAnimatedProperty2DFloat(string name)
			: this(name, null)
		{
		}

		public MyAnimatedProperty2DFloat(string name, MyAnimatedProperty<float>.InterpolatorDelegate interpolator)
			: base(name, interpolator)
		{
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat(base.Name, interpolateAfterEnd: false, m_interpolator2);
			myAnimatedPropertyFloat.Deserialize(reader);
			value = myAnimatedPropertyFloat;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedProperty2DFloat myAnimatedProperty2DFloat = new MyAnimatedProperty2DFloat(base.Name);
			Duplicate(myAnimatedProperty2DFloat);
			return myAnimatedProperty2DFloat;
		}

		public override void ApplyVariance(ref float interpolatedValue, ref float variance, float multiplier, out float value)
		{
			if (variance != 0f || multiplier != 1f)
			{
				interpolatedValue = MyUtils.GetRandomFloat(interpolatedValue - variance, interpolatedValue + variance) * multiplier;
			}
			value = interpolatedValue;
		}
	}
}
