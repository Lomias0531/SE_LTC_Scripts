using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2DInt : MyAnimatedProperty2D<MyAnimatedPropertyInt, int, int>
	{
		public override string ValueType => "Int";

		public MyAnimatedProperty2DInt(string name)
			: this(name, null)
		{
		}

		public MyAnimatedProperty2DInt(string name, MyAnimatedProperty<int>.InterpolatorDelegate interpolator)
			: base(name, interpolator)
		{
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyInt myAnimatedPropertyInt = new MyAnimatedPropertyInt(base.Name, m_interpolator2);
			myAnimatedPropertyInt.Deserialize(reader);
			value = myAnimatedPropertyInt;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedProperty2DInt myAnimatedProperty2DInt = new MyAnimatedProperty2DInt(base.Name);
			Duplicate(myAnimatedProperty2DInt);
			return myAnimatedProperty2DInt;
		}

		public override void ApplyVariance(ref int interpolatedValue, ref int variance, float multiplier, out int value)
		{
			if (variance != 0 || multiplier != 1f)
			{
				interpolatedValue = (int)((float)MyUtils.GetRandomInt(interpolatedValue - variance, interpolatedValue + variance) * multiplier);
			}
			value = interpolatedValue;
		}
	}
}
