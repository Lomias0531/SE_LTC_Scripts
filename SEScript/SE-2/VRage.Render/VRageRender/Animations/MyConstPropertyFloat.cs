using System;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyFloat : MyConstProperty<float>
	{
		public override string ValueType => "Float";

		public MyConstPropertyFloat()
		{
		}

		public MyConstPropertyFloat(string name)
			: base(name)
		{
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

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyFloat myConstPropertyFloat = new MyConstPropertyFloat(base.Name);
			Duplicate(myConstPropertyFloat);
			return myConstPropertyFloat;
		}

		public static implicit operator float(MyConstPropertyFloat f)
		{
			return f.GetValue<float>();
		}
	}
}
