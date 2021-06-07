using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerDictionary<TKey, TValue> : MySerializer<Dictionary<TKey, TValue>>
	{
		private MySerializer<TKey> m_keySerializer = MyFactory.GetSerializer<TKey>();

		private MySerializer<TValue> m_valueSerializer = MyFactory.GetSerializer<TValue>();

		public override void Clone(ref Dictionary<TKey, TValue> obj)
		{
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(obj.Count);
			foreach (KeyValuePair<TKey, TValue> item in obj)
			{
				TKey value = item.Key;
				TValue value2 = item.Value;
				m_keySerializer.Clone(ref value);
				m_valueSerializer.Clone(ref value2);
				dictionary.Add(value, value2);
			}
			obj = dictionary;
		}

		public override bool Equals(ref Dictionary<TKey, TValue> a, ref Dictionary<TKey, TValue> b)
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
			foreach (KeyValuePair<TKey, TValue> item in a)
			{
				TValue a2 = item.Value;
				if (!b.TryGetValue(item.Key, out TValue value) || !m_valueSerializer.Equals(ref a2, ref value))
				{
					return false;
				}
			}
			return true;
		}

		public override void Read(BitStream stream, out Dictionary<TKey, TValue> obj, MySerializeInfo info)
		{
			int num = (int)stream.ReadUInt32Variant();
			obj = new Dictionary<TKey, TValue>(num);
			for (int i = 0; i < num; i++)
			{
				MySerializationHelpers.CreateAndRead(stream, out TKey result, m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
				MySerializationHelpers.CreateAndRead(stream, out TValue result2, m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
				obj.Add(result, result2);
			}
		}

		public override void Write(BitStream stream, ref Dictionary<TKey, TValue> obj, MySerializeInfo info)
		{
			int count = obj.Count;
			stream.WriteVariant((uint)count);
			foreach (KeyValuePair<TKey, TValue> item in obj)
			{
				TKey value = item.Key;
				TValue value2 = item.Value;
				MySerializationHelpers.Write(stream, ref value, m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
				MySerializationHelpers.Write(stream, ref value2, m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
			}
		}
	}
}
