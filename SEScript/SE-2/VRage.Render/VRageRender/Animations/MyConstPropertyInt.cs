using System;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyInt : MyConstProperty<int>
	{
		public override string ValueType => "Int";

		public MyConstPropertyInt()
		{
		}

		public MyConstPropertyInt(string name)
			: base(name)
		{
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

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyInt myConstPropertyInt = new MyConstPropertyInt(base.Name);
			Duplicate(myConstPropertyInt);
			return myConstPropertyInt;
		}

		public static implicit operator int(MyConstPropertyInt f)
		{
			return f.GetValue<int>();
		}
	}
}
