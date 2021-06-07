using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyString : MyConstProperty<string>
	{
		public override string ValueType => "String";

		public MyConstPropertyString()
		{
		}

		public MyConstPropertyString(string name)
			: base(name)
		{
		}

		public override void SerializeValue(XmlWriter writer, object value)
		{
			writer.WriteValue((string)value);
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			base.DeserializeValue(reader, out value);
			value = value.ToString();
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyString myConstPropertyString = new MyConstPropertyString(base.Name);
			Duplicate(myConstPropertyString);
			return myConstPropertyString;
		}

		public static implicit operator string(MyConstPropertyString f)
		{
			return f.GetValue<string>();
		}
	}
}
