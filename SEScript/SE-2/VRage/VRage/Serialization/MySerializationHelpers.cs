using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public static class MySerializationHelpers
	{
		public static bool CreateAndRead<TMember>(BitStream stream, out TMember result, MySerializer<TMember> serializer, MySerializeInfo info)
		{
			if (ReadNullable(stream, info.IsNullable))
			{
				if (MySerializer<TMember>.IsClass && info.IsDynamic)
				{
					Type t = typeof(TMember);
					bool flag = true;
					if (info.IsDynamicDefault)
					{
						flag = stream.ReadBool();
					}
					if (flag)
					{
						t = stream.ReadDynamicType(typeof(TMember), info.DynamicSerializer);
					}
					MyFactory.GetSerializer(t).Read(stream, out object value, info);
					result = (TMember)value;
				}
				else
				{
					serializer.Read(stream, out result, info);
				}
				return true;
			}
			result = default(TMember);
			return false;
		}

		public static void Write<TMember>(BitStream stream, ref TMember value, MySerializer<TMember> serializer, MySerializeInfo info)
		{
			if (!WriteNullable(stream, ref value, info.IsNullable, serializer))
			{
				return;
			}
			if (MySerializer<TMember>.IsClass && info.IsDynamic)
			{
				Type typeFromHandle = typeof(TMember);
				Type type = value.GetType();
				bool flag = true;
				if (info.IsDynamicDefault)
				{
					flag = (typeFromHandle != type);
					stream.WriteBool(flag);
				}
				if (flag)
				{
					stream.WriteDynamicType(typeFromHandle, type, info.DynamicSerializer);
				}
				MyFactory.GetSerializer(value.GetType()).Write(stream, value, info);
			}
			else
			{
				if (!MySerializer<TMember>.IsValueType && !(value.GetType() == typeof(TMember)))
				{
					throw new MySerializeException(MySerializeErrorEnum.DynamicNotAllowed);
				}
				serializer.Write(stream, ref value, info);
			}
		}

		private static bool ReadNullable(BitStream stream, bool isNullable)
		{
			if (isNullable)
			{
				return stream.ReadBool();
			}
			return true;
		}

		private static bool WriteNullable<T>(BitStream stream, ref T value, bool isNullable, MySerializer<T> serializer)
		{
			if (isNullable)
			{
				T b = default(T);
				bool flag = !serializer.Equals(ref value, ref b);
				stream.WriteBool(flag);
				return flag;
			}
			if (!typeof(T).IsValueType && value == null)
			{
				throw new MySerializeException(MySerializeErrorEnum.NullNotAllowed);
			}
			return true;
		}
	}
}
