using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerInt16 : MySerializer<short>
	{
		public override void Clone(ref short value)
		{
		}

		public override bool Equals(ref short a, ref short b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out short value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				value = (short)stream.ReadUInt32Variant();
			}
			else if (info.IsVariantSigned)
			{
				value = (short)stream.ReadInt32Variant();
			}
			else
			{
				value = stream.ReadInt16();
			}
		}

		public override void Write(BitStream stream, ref short value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				stream.WriteVariant((ushort)value);
			}
			else if (info.IsVariantSigned)
			{
				stream.WriteVariantSigned(value);
			}
			else
			{
				stream.WriteInt16(value);
			}
		}
	}
}
