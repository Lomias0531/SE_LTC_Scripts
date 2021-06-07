using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerList<TItem> : MySerializer<List<TItem>>
	{
		private MySerializer<TItem> m_itemSerializer = MyFactory.GetSerializer<TItem>();

		public override void Clone(ref List<TItem> value)
		{
			List<TItem> list = new List<TItem>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				TItem value2 = value[i];
				m_itemSerializer.Clone(ref value2);
				list.Add(value2);
			}
			value = list;
		}

		public override bool Equals(ref List<TItem> a, ref List<TItem> b)
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
			for (int i = 0; i < a.Count; i++)
			{
				TItem a2 = a[i];
				TItem b2 = b[i];
				if (!m_itemSerializer.Equals(ref a2, ref b2))
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(BitStream stream, out List<TItem> value, MySerializeInfo info)
		{
			int num = (int)stream.ReadUInt32Variant();
			value = new List<TItem>(num);
			for (int i = 0; i < num; i++)
			{
				MySerializationHelpers.CreateAndRead(stream, out TItem result, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
				value.Add(result);
			}
		}

		public override void Write(BitStream stream, ref List<TItem> value, MySerializeInfo info)
		{
			int count = value.Count;
			stream.WriteVariant((uint)count);
			for (int i = 0; i < count; i++)
			{
				TItem value2 = value[i];
				MySerializationHelpers.Write(stream, ref value2, m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
			}
		}
	}
}
