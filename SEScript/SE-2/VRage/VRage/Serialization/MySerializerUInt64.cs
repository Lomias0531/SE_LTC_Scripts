using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerUInt64 : MySerializer<ulong>
	{
		public override void Clone(ref ulong value)
		{
		}

		public override bool Equals(ref ulong a, ref ulong b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out ulong value, MySerializeInfo info)
		{
			if (info.IsVariant || info.IsVariantSigned)
			{
				value = stream.ReadUInt64Variant();
			}
			else
			{
				value = stream.ReadUInt64();
			}
		}

		public override void Write(BitStream stream, ref ulong value, MySerializeInfo info)
		{
			if (info.IsVariant || info.IsVariantSigned)
			{
				stream.WriteVariant(value);
			}
			else
			{
				stream.WriteUInt64(value);
			}
		}
	}
}
