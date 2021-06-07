using System;
using System.Collections.Generic;
using System.Xml;

namespace VRageRender.Animations
{
	public class MyAnimatedProperty2DEnum : MyAnimatedProperty2DInt
	{
		private Type m_enumType;

		private List<string> m_enumStrings;

		public override string BaseValueType => "Enum";

		public MyAnimatedProperty2DEnum(string name)
			: this(name, null, null)
		{
		}

		public MyAnimatedProperty2DEnum(string name, Type enumType, List<string> enumStrings)
			: this(name, null, enumType, enumStrings)
		{
		}

		public MyAnimatedProperty2DEnum(string name, MyAnimatedProperty<int>.InterpolatorDelegate interpolator, Type enumType, List<string> enumStrings)
			: base(name, interpolator)
		{
			m_enumType = enumType;
			m_enumStrings = enumStrings;
		}

		public override void DeserializeValue(XmlReader reader, out object value)
		{
			MyAnimatedPropertyInt myAnimatedPropertyInt = new MyAnimatedPropertyInt(base.Name, m_interpolator2);
			myAnimatedPropertyInt.Deserialize(reader);
			value = myAnimatedPropertyInt;
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
			MyAnimatedProperty2DEnum myAnimatedProperty2DEnum = new MyAnimatedProperty2DEnum(base.Name);
			Duplicate(myAnimatedProperty2DEnum);
			myAnimatedProperty2DEnum.m_enumType = m_enumType;
			myAnimatedProperty2DEnum.m_enumStrings = m_enumStrings;
			return myAnimatedProperty2DEnum;
		}
	}
}
