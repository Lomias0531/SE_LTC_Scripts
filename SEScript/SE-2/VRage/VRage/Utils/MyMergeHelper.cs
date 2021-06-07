using System;
using System.Reflection;

namespace VRage.Utils
{
	/// <summary>
	/// Helper class for merge funcionality. Performs comparison between
	/// source and other values and set on self if value is different
	/// </summary>
	public static class MyMergeHelper
	{
		public static void Merge<T>(T self, T source, T other) where T : class
		{
			if (self == null)
			{
				MyLog.Default.WriteLine("self cannot be null!!! type: " + typeof(T));
			}
			if (source == null)
			{
				MyLog.Default.WriteLine("Source cannot be null!!! type: " + typeof(T));
			}
			if (other == null)
			{
				MyLog.Default.WriteLine("Other cannot be null!!! type: " + typeof(T));
			}
			object self2 = self;
			object source2 = source;
			object other2 = other;
			MergeInternal(typeof(T), ref self2, ref source2, ref other2);
		}

		public static void Merge<T>(ref T self, ref T source, ref T other) where T : struct
		{
			object self2 = self;
			object source2 = source;
			object other2 = other;
			MergeInternal(typeof(T), ref self2, ref source2, ref other2);
			self = (T)self2;
		}

		private static void MergeInternal(Type type, ref object self, ref object source, ref object other)
		{
			if (type == null)
			{
				MyLog.Default.WriteLine(string.Concat("type cannot be null!!! self: ", self, " source: ", source, " other: ", other));
			}
			if (self == null)
			{
				self = Activator.CreateInstance(type);
			}
			if (source == null)
			{
				source = Activator.CreateInstance(type);
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object source2 = fieldInfo.GetValue(source);
				object other2 = fieldInfo.GetValue(other);
				if (source2 == other2)
				{
					continue;
				}
				if (source2 == null)
				{
					MyLog.Default.WriteLine(string.Concat("ERROR: Error detected related to the following resource: ", other2, " Please check your definition files and reload"));
					MyLog.Default.WriteLine(string.Concat("More info MergeInternal: field: ", fieldInfo, " source: ", source, " , other: ", other, " , valueOther: ", other2));
					continue;
				}
				bool flag = false;
				if ((IsPrimitive(fieldInfo.FieldType) && !(flag = source2.Equals(other2))) || (source2 != null && other2 == null))
				{
					fieldInfo.SetValue(self, source2);
				}
				else if (!flag)
				{
					object self2 = fieldInfo.GetValue(self);
					MergeInternal(fieldInfo.FieldType, ref self2, ref source2, ref other2);
					fieldInfo.SetValue(self, self2);
				}
			}
		}

		private static bool IsPrimitive(Type type)
		{
			if (!type.IsPrimitive && !(type == typeof(string)))
			{
				return type == typeof(Type);
			}
			return true;
		}
	}
}
