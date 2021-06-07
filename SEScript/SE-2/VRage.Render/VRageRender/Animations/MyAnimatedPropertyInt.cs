using System;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyInt : MyAnimatedProperty<int>
	{
		public override string ValueType => "Int";

		public MyAnimatedPropertyInt()
		{
		}

		public MyAnimatedPropertyInt(string name)
			: this(name, null)
		{
		}

		public MyAnimatedPropertyInt(string name, InterpolatorDelegate interpolator)
			: base(name, interpolateAfterEnd: false, interpolator)
		{
		}

		protected override void Init()
		{
			Interpolator = MyIntInterpolator.Lerp;
			base.Init();
		}

		public override IMyConstProperty Duplicate()
		{
			MyAnimatedPropertyInt myAnimatedPropertyInt = new MyAnimatedPropertyInt(base.Name);
			Duplicate(myAnimatedPropertyInt);
			return myAnimatedPropertyInt;
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue(((int)value).ToString(CultureInfo.InvariantCulture));
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}

		protected override bool EqualsValues(object value1, object value2)
		{
			return (int)value1 == (int)value2;
		}
	}
}
