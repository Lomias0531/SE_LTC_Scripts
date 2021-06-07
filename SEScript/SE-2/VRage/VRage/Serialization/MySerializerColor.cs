using VRage.Library.Collections;
using VRageMath;

namespace VRage.Serialization
{
	public class MySerializerColor : MySerializer<Color>
	{
		public override void Clone(ref Color value)
		{
		}

		public override bool Equals(ref Color a, ref Color b)
		{
			return a.PackedValue == b.PackedValue;
		}

		public override void Read(BitStream stream, out Color value, MySerializeInfo info)
		{
			value.PackedValue = stream.ReadUInt32();
		}

		public override void Write(BitStream stream, ref Color value, MySerializeInfo info)
		{
			stream.WriteUInt32(value.PackedValue);
		}
	}
}
