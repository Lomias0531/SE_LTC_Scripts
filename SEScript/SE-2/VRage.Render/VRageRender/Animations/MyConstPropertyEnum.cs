using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyConstPropertyEnum : MyConstPropertyInt, IMyConstProperty
	{
		private Type m_enumType;

		private List<string> m_enumStrings;

		public override string BaseValueType => "Enum";

		public MyConstPropertyEnum()
		{
		}

		public MyConstPropertyEnum(string name)
			: this(name, null, null)
		{
		}

		public MyConstPropertyEnum(string name, Type enumType, List<string> enumStrings)
			: base(name)
		{
			m_enumType = enumType;
			m_enumStrings = enumStrings;
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

		public Type GetEnumType()
		{
			return m_enumType;
		}

		public List<string> GetEnumStrings()
		{
			return m_enumStrings;
		}

		public override IMyConstProperty Duplicate()
		{
			MyConstPropertyEnum myConstPropertyEnum = new MyConstPropertyEnum(base.Name);
			Duplicate(myConstPropertyEnum);
			myConstPropertyEnum.m_enumType = m_enumType;
			myConstPropertyEnum.m_enumStrings = m_enumStrings;
			return myConstPropertyEnum;
		}

		Type IMyConstProperty.GetValueType()
		{
			return m_enumType;
		}

		public override void SetValue(object val)
		{
			int value = Convert.ToInt32(val);
			SetValue(value);
		}
	}
}
