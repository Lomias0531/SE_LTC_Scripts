using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerUInt8 : MySerializer<byte>
	{
		public override void Clone(ref byte value)
		{
		}

		public override bool Equals(ref byte a, ref byte b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out byte value, MySerializeInfo info)
		{
			value = stream.ReadByte();
		}

		public override void Write(BitStream stream, ref byte value, MySerializeInfo info)
		{
			stream.WriteByte(value);
		}
	}
}
