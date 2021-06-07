using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerNullable<T> : MySerializer<T?> where T : struct
	{
		private MySerializer<T> m_serializer = MyFactory.GetSerializer<T>();

		public override void Clone(ref T? value)
		{
			if (value.HasValue)
			{
				T value2 = value.Value;
				m_serializer.Clone(ref value2);
				value = value2;
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
			T a2 = a.Value;
			T b2 = b.Value;
			return m_serializer.Equals(ref a2, ref b2);
		}

		public override void Read(BitStream stream, out T? value, MySerializeInfo info)
		{
			if (stream.ReadBool())
			{
				m_serializer.Read(stream, out T value2, info);
				value = value2;
			}
			else
			{
				value = null;
			}
		}

		public override void Write(BitStream stream, ref T? value, MySerializeInfo info)
		{
			if (value.HasValue)
			{
				T value2 = value.Value;
				stream.WriteBool(value: true);
				m_serializer.Write(stream, ref value2, info);
			}
			else
			{
				stream.WriteBool(value: false);
			}
		}
	}
}
