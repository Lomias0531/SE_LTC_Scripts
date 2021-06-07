using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerInt8 : MySerializer<sbyte>
	{
		public override void Clone(ref sbyte value)
		{
		}

		public override bool Equals(ref sbyte a, ref sbyte b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out sbyte value, MySerializeInfo info)
		{
			value = stream.ReadSByte();
		}

		public override void Write(BitStream stream, ref sbyte value, MySerializeInfo info)
		{
			stream.WriteSByte(value);
		}
	}
}
