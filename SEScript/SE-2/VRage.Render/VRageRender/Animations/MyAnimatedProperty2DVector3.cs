using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2DVector3 : MyAnimatedProperty2D<MyAnimatedPropertyVector3, Vector3, Vector3>
	{
		public override string ValueType => "Vector3";

		public MyAnimatedProperty2DVector3(string name)
			: this(name, null)
		{
		}

		public MyAnimatedProperty2DVector3(string name, MyAnimatedProperty<Vector3>.InterpolatorDelegate interpolator)
			: base(name, interpolator)
		{
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyVector3 myAnimatedPropertyVector = new MyAnimatedPropertyVector3(base.Name, interpolateAfterEnd: false, m_interpolator2);
			myAnimatedPropertyVector.Deserialize(reader);
			value = myAnimatedPropertyVector;
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedProperty2DVector3 myAnimatedProperty2DVector = new MyAnimatedProperty2DVector3(base.Name);
			Duplicate(myAnimatedProperty2DVector);
			return myAnimatedProperty2DVector;
		}

		public override void ApplyVariance(ref Vector3 interpolatedValue, ref Vector3 variance, float multiplier, out Vector3 value)
		{
			if (variance != Vector3.Zero || multiplier != 1f)
			{
				value.X = MyUtils.GetRandomFloat(interpolatedValue.X - variance.X, interpolatedValue.X + variance.X) * multiplier;
				value.Y = MyUtils.GetRandomFloat(interpolatedValue.Y - variance.Y, interpolatedValue.Y + variance.Y) * multiplier;
				value.Z = MyUtils.GetRandomFloat(interpolatedValue.Z - variance.Z, interpolatedValue.Z + variance.Z) * multiplier;
			}
			value = interpolatedValue;
		}
	}
}
