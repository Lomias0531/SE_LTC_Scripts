using System;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyGenerationIndex : MyConstPropertyInt
	{
		public override string BaseValueType => "GenerationIndex";

		public MyConstPropertyGenerationIndex()
		{
		}

		public MyConstPropertyGenerationIndex(string name)
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
			MyConstPropertyGenerationIndex myConstPropertyGenerationIndex = new MyConstPropertyGenerationIndex(base.Name);
			Duplicate(myConstPropertyGenerationIndex);
			return myConstPropertyGenerationIndex;
		}
	}
}
