using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerUInt32 : MySerializer<uint>
	{
		public override void Clone(ref uint value)
		{
		}

		public override bool Equals(ref uint a, ref uint b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out uint value, MySerializeInfo info)
		{
			if (info.IsVariant || info.IsVariantSigned)
			{
				value = stream.ReadUInt32Variant();
			}
			else
			{
				value = stream.ReadUInt32();
			}
		}

		public override void Write(BitStream stream, ref uint value, MySerializeInfo info)
		{
			if (info.IsVariant || info.IsVariantSigned)
			{
				stream.WriteVariant(value);
			}
			else
			{
				stream.WriteUInt32(value);
			}
		}
	}
}
