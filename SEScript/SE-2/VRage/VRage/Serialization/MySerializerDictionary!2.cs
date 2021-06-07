namespace VRage.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using VRage.Library.Collections;

    public class MySerializerDictionary<TKey, TValue> : MySerializer<Dictionary<TKey, TValue>>
    {
        private MySerializer<TKey> m_keySerializer;
        private MySerializer<TValue> m_valueSerializer;

        public MySerializerDictionary()
        {
            this.m_keySerializer = MyFactory.GetSerializer<TKey>();
            this.m_valueSerializer = MyFactory.GetSerializer<TValue>();
        }

        public override void Clone(ref Dictionary<TKey, TValue> obj)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(obj.Count);
            foreach (KeyValuePair<TKey, TValue> pair in obj)
            {
                TKey key = pair.Key;
                TValue local2 = pair.Value;
                this.m_keySerializer.Clone(ref key);
                this.m_valueSerializer.Clone(ref local2);
                dictionary.Add(key, local2);
            }
            obj = dictionary;
        }

        public override bool Equals(ref Dictionary<TKey, TValue> a, ref Dictionary<TKey, TValue> b)
        {
            if (a != b)
            {
                if (AnyNull(a, b))
                {
                    return false;
                }
                if (a.Count != b.Count)
                {
                    return false;
                }
                foreach (KeyValuePair<TKey, TValue> pair in a)
                {
                    TValue local = pair.Value;
                    if (!b.TryGetValue(pair.Key, out TValue local2) || !this.m_valueSerializer.Equals(ref local, ref local2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void Read(BitStream stream, out Dictionary<TKey, TValue> obj, MySerializeInfo info)
        {
            int capacity = (int) stream.ReadUInt32Variant();
            obj = new Dictionary<TKey, TValue>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                MySerializationHelpers.CreateAndRead<TKey>(stream, out TKey local, this.m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
                MySerializationHelpers.CreateAndRead<TValue>(stream, out TValue local2, this.m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
                obj.Add(local, local2);
            }
        }

        public override void Write(BitStream stream, ref Dictionary<TKey, TValue> obj, MySerializeInfo info)
        {
            int count = obj.Count;
            stream.WriteVariant((uint) count);
            foreach (KeyValuePair<TKey, TValue> pair in obj)
            {
                TKey key = pair.Key;
                TValue local2 = pair.Value;
                MySerializationHelpers.Write<TKey>(stream, ref key, this.m_keySerializer, info.KeyInfo ?? MySerializeInfo.Default);
                MySerializationHelpers.Write<TValue>(stream, ref local2, this.m_valueSerializer, info.ItemInfo ?? MySerializeInfo.Default);
            }
        }
    }
}

