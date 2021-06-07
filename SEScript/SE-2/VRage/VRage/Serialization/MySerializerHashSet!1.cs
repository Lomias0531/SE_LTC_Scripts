namespace VRage.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using VRage.Library.Collections;

    public class MySerializerHashSet<TItem> : MySerializer<HashSet<TItem>>
    {
        private MySerializer<TItem> m_itemSerializer;

        public MySerializerHashSet()
        {
            this.m_itemSerializer = MyFactory.GetSerializer<TItem>();
        }

        public override void Clone(ref HashSet<TItem> value)
        {
            HashSet<TItem> set = new HashSet<TItem>();
            foreach (TItem local in value)
            {
                this.m_itemSerializer.Clone(ref local);
                set.Add(local);
            }
            value = set;
        }

        public override bool Equals(ref HashSet<TItem> a, ref HashSet<TItem> b)
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
                foreach (TItem local in a)
                {
                    if (!b.Contains(local))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void Read(BitStream stream, out HashSet<TItem> value, MySerializeInfo info)
        {
            int num = (int) stream.ReadUInt32Variant();
            value = new HashSet<TItem>();
            for (int i = 0; i < num; i++)
            {
                MySerializationHelpers.CreateAndRead<TItem>(stream, out TItem local, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
                value.Add(local);
            }
        }

        public override void Write(BitStream stream, ref HashSet<TItem> value, MySerializeInfo info)
        {
            int count = value.Count;
            stream.WriteVariant((uint) count);
            foreach (TItem local in value)
            {
                MySerializationHelpers.Write<TItem>(stream, ref local, this.m_itemSerializer, info.ItemInfo ?? MySerializeInfo.Default);
            }
        }
    }
}

