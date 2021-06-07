using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerBool : MySerializer<bool>
	{
		public override void Clone(ref bool value)
		{
		}

		public override bool Equals(ref bool a, ref bool b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out bool value, MySerializeInfo info)
		{
			value = stream.ReadBool();
		}

		public override void Write(BitStream stream, ref bool value, MySerializeInfo info)
		{
			stream.WriteBool(value);
		}
	}
}
