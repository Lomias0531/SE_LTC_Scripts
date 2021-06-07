using System;

namespace VRage.Utils
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class MyFlagEnumAttribute : Attribute
	{
		private readonly Type m_enumType;

		public Type EnumType => m_enumType;

		public MyFlagEnumAttribute(Type enumType)
		{
			m_enumType = enumType;
		}
	}
}
