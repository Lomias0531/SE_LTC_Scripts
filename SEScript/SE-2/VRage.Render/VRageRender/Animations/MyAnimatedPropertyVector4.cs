using System.Xml;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyVector4 : MyAnimatedProperty<Vector4>
	{
		public override string ValueType => "Vector4";

		public MyAnimatedPropertyVector4()
		{
		}

		public MyAnimatedPropertyVector4(string name)
			: this(name, null)
		{
		}

		public MyAnimatedPropertyVector4(string name, InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd: false, interpolator)
		{
		}

		protected override void Init()
		{
			Interpolator = MyVector4Interpolator.Lerp;
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyVector4 myAnimatedPropertyVector = new MyAnimatedPropertyVector4(base.Name);
			Duplicate(myAnimatedPropertyVector);
			return myAnimatedPropertyVector;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteElementString("W", ((Vector4)value).W.ToString());
			writer.WriteElementString("X", ((Vector4)value).X.ToString());
			writer.WriteElementString("Y", ((Vector4)value).Y.ToString());
			writer.WriteElementString("Z", ((Vector4)value).Z.ToString());
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyUtils.DeserializeValue(reader, out Vector4 value2);
			value = value2;
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((Vector4)value1 - (Vector4)value2);
		}
	}
}
