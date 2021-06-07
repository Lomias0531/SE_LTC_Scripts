using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerInt32 : MySerializer<int>
	{
		public override void Clone(ref int value)
		{
		}

		public override bool Equals(ref int a, ref int b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out int value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				value = (int)stream.ReadUInt32Variant();
			}
			else if (info.IsVariantSigned)
			{
				value = stream.ReadInt32Variant();
			}
			else
			{
				value = stream.ReadInt32();
			}
		}

		public override void Write(BitStream stream, ref int value, MySerializeInfo info)
		{
			if (info.IsVariant)
			{
				stream.WriteVariant((uint)value);
			}
			else if (info.IsVariantSigned)
			{
				stream.WriteVariantSigned(value);
			}
			else
			{
				stream.WriteInt32(value);
			}
		}
	}
}
