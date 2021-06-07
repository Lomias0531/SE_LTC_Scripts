using System;
using System.Globalization;
using System.Xml;
using VRage.Utils;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyFloat : MyAnimatedProperty<float>
	{
		public override string ValueType => "Float";

		public MyAnimatedPropertyFloat()
		{
		}

		public MyAnimatedPropertyFloat(string name)
			: this(name, interpolateAfterEnd: false, null)
		{
		}

		public MyAnimatedPropertyFloat(string name, bool interpolateAfterEnd, InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd, interpolator)
		{
		}

		protected override void Init()
		{
			Interpolator = MyFloatInterpolator.Lerp;
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyFloat myAnimatedPropertyFloat = new MyAnimatedPropertyFloat(base.Name);
			Duplicate(myAnimatedPropertyFloat);
			return myAnimatedPropertyFloat;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((float)value).ToString(CultureInfo.InvariantCulture));
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = Convert.ToSingle(value, CultureInfo.InvariantCulture);
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return MyUtils.IsZero((float)value1 - (float)value2);
		}
	}
}
