using System;
using System.Reflection;

namespace VRage
{
	public static class MyStructDefault
	{
		public static FieldInfo GetDefaultFieldInfo(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsInitOnly && fieldInfo.GetCustomAttribute(typeof(StructDefaultAttribute)) != null)
				{
					return fieldInfo;
				}
			}
			return null;
		}

		public static T GetDefaultValue<T>(Type type) where T : struct
		{
			FieldInfo defaultFieldInfo = GetDefaultFieldInfo(typeof(T));
			if (defaultFieldInfo == null)
			{
				return new T();
			}
			return (T)defaultFieldInfo.GetValue(null);
		}
	}
}
