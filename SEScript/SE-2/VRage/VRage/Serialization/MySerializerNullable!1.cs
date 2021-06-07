namespace VRage.Serialization
{
    using System;
    using System.Runtime.InteropServices;
    using VRage.Library.Collections;

    public class MySerializerNullable<T> : MySerializer<T?> where T: struct
    {
        private MySerializer<T> m_serializer;

        public MySerializerNullable()
        {
            this.m_serializer = MyFactory.GetSerializer<T>();
        }

        public override void Clone(ref T? value)
        {
            if (value.HasValue)
            {
                T local = value.Value;
                this.m_serializer.Clone(ref local);
                value = new T?(local);
            }
        }

        public override bool Equals(ref T? a, ref T? b)
        {
            if (a.HasValue != b.HasValue)
            {
                return false;
            }
            if (!a.HasValue)
            {
                return true;
            }
            T local = a.Value;
            T local2 = b.Value;
            return this.m_serializer.Equals(ref local, ref local2);
        }

        public override void Read(BitStream stream, out T? value, MySerializeInfo info)
        {
            if (stream.ReadBool())
            {
                this.m_serializer.Read(stream, out T local, info);
                value = new T?(local);
            }
            else
            {
                value = 0;
            }
        }

        public override void Write(BitStream stream, ref T? value, MySerializeInfo info)
        {
            if (value.HasValue)
            {
                T local = value.Value;
                stream.WriteBool(true);
                this.m_serializer.Write(stream, ref local, info);
            }
            else
            {
                stream.WriteBool(false);
            }
        }
    }
}

