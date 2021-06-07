using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerInt64 : MySerializer<long>
	{
		public override void Clone(ref long value)
		{
		}

		public override bool Equals(ref long a, ref long b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out long value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				value = (long)stream.ReadUInt64Variant();
			}
			else if (info.IsVariantSigned)
			{
				value = stream.ReadInt64Variant();
			}
			else
			{
				value = stream.ReadInt64();
			}
		}

		public override void Write(BitStream stream, ref long value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				stream.WriteVariant((ulong)value);
			}
			else if (info.IsVariantSigned)
			{
				stream.WriteVariantSigned(value);
			}
			else
			{
				stream.WriteInt64(value);
			}
		}
	}
}
