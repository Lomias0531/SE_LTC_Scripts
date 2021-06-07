using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyVector3 : MyAnimatedProperty<Vector3>
	{
		public override string ValueType => "Vector3";

		public MyAnimatedPropertyVector3()
		{
		}

		public MyAnimatedPropertyVector3(string name)
			: this(name, interpolateAfterEnd: false, null)
		{
		}

		public MyAnimatedPropertyVector3(string name, bool interpolateAfterEnd, InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd, interpolator)
		{
		}

		protected override void Init()
		{
			Interpolator = MyVector3Interpolator.Lerp;
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyVector3 myAnimatedPropertyVector = new MyAnimatedPropertyVector3(base.Name);
			Duplicate(myAnimatedPropertyVector);
			return myAnimatedPropertyVector;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("X", ((Vector3)value).X.ToString());
			writer.WriteElementString("Y", ((Vector3)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector3)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector3 value2);
			value = value2;
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((Vector3)value1 - (Vector3)value2);
		}
	}
}
