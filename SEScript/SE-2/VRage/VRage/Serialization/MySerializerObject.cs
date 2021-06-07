using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerObject<T> : MySerializer<T>
	{
		private MyMemberSerializer<T>[] m_memberSerializers;

		public MySerializerObject()
		{
			IEnumerable<MemberInfo> source = (from s in typeof(T).GetDataMembers(fields: true, properties: true, nonPublic: true, inherited: true, _static: false, instance: true, read: true, write: true)
				where !Attribute.IsDefined(s, typeof(NoSerializeAttribute))
				where Attribute.IsDefined(s, typeof(SerializeAttribute)) || s.IsMemberPublic()
				select s).Where(Filter);
			m_memberSerializers = source.Select((MemberInfo s) => MyFactory.CreateMemberSerializer<T>(s)).ToArray();
		}

		private bool Filter(MemberInfo info)
		{
			if (info.MemberType == MemberTypes.Field)
			{
				return true;
			}
			if (info.MemberType == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = (PropertyInfo)info;
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					return propertyInfo.GetIndexParameters().Length == 0;
				}
				return false;
			}
			return false;
		}

		public override void Clone(ref T value)
		{
			T clone = Activator.CreateInstance<T>();
			MyMemberSerializer<T>[] memberSerializers = m_memberSerializers;
			for (int i = 0; i < memberSerializers.Length; i++)
			{
				memberSerializers[i].Clone(ref value, ref clone);
			}
			value = clone;
		}

		public override bool Equals(ref T a, ref T b)
		{
			if (!typeof(T).IsValueType)
			{
				if ((object)a == (object)b)
				{
					return true;
				}
				if (MySerializer.AnyNull(a, b))
				{
					return false;
				}
			}
			MyMemberSerializer<T>[] memberSerializers = m_memberSerializers;
			for (int i = 0; i < memberSerializers.Length; i++)
			{
				if (!memberSerializers[i].Equals(ref a, ref b))
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(BitStream stream, out T value, MySerializeInfo info)
		{
			value = Activator.CreateInstance<T>();
			MyMemberSerializer<T>[] memberSerializers = m_memberSerializers;
			for (int i = 0; i < memberSerializers.Length; i++)
			{
				memberSerializers[i].Read(stream, ref value, info.ItemInfo);
			}
		}

		public override void Write(BitStream stream, ref T value, MySerializeInfo info)
		{
			MyMemberSerializer<T>[] memberSerializers = m_memberSerializers;
			for (int i = 0; i < memberSerializers.Length; i++)
			{
				memberSerializers[i].Write(stream, ref value, info.ItemInfo);
			}
		}
	}
}
