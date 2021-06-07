using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerHashSet<TItem> : MySerializer<HashSet<TItem>>
	{
		private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

		public override void Clone(ref HashSet<TItem> value)
		{
			HashSet<TItem> hashSet = new HashSet<TItem>();
			foreach (TItem item in value)
			{
				TItem value2 = item;
				m_itemSerializer.Clone(ref value2);
				hashSet.Add(value2);
			}
			value = hashSet;
		}

		public override bool Equals(ref HashSet<TItem> a, ref HashSet<TItem> b)
		{
			if (a == b)
			{
				return true;
			}
			if (MySerializer.AnyNull(a, b))
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			foreach (TItem item in a)
			{
				if (!b.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(BitStream stream, out HashSet<TItem> value, MySerializeInfo info)
		{
			int num = (int)stream.ReadUInt32Variant();
			value = new HashSet<TItem>();
			for (int i = 0; i < num; i++)
			{
				MySerializationHelpers.CreateAndRead(stream, out TItem result, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
				value.Add(result);
			}
		}

		public override void Write(BitStream stream, ref HashSet<TItem> value, MySerializeInfo info)
		{
			int count = value.Count;
			stream.WriteVariant((uint)count);
			foreach (TItem item in value)
			{
				TItem value2 = item;
				MySerializationHelpers.Write(stream, ref value2, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
			}
		}
	}
}
