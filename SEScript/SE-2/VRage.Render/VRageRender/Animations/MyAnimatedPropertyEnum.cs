using System;
using System.Collections.Generic;

namespace VRageRender.Animations
{
	public class MyAnimatedPropertyEnum : MyAnimatedPropertyInt
	{
		private Type m_enumType;

		private List<string> m_enumStrings;

		public override string BaseValueType => "Enum";

		public MyAnimatedPropertyEnum()
		{
		}

		public MyAnimatedPropertyEnum(string name)
			: this(name, null, null)
		{
		}

		public MyAnimatedPropertyEnum(string name, Type enumType, List<string> enumStrings)
			: this(name, null, enumType, enumStrings)
		{
		}

		public MyAnimatedPropertyEnum(string name, InterpolatorDelegate interpolator, Type enumType, List<string> enumStrings)
			: base(name, interpolator)
		{
			m_enumType = enumType;
			m_enumStrings = enumStrings;
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
			MyAnimatedPropertyEnum myAnimatedPropertyEnum = new MyAnimatedPropertyEnum(base.Name);
			Duplicate(myAnimatedPropertyEnum);
			myAnimatedPropertyEnum.m_enumType = m_enumType;
			myAnimatedPropertyEnum.m_enumStrings = m_enumStrings;
			return myAnimatedPropertyEnum;
		}
	}
}
