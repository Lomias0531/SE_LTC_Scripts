using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Serialization
{
	public class MySerializerMyStringHash : MySerializer<MyStringHash>
	{
		public override void Clone(ref MyStringHash value)
		{
		}

		public override bool Equals(ref MyStringHash a, ref MyStringHash b)
		{
			return a.Equals(b);
		}

		public override void Read(BitStream stream, out MyStringHash value, MySerializeInfo info)
		{
			value = MyStringHash.TryGet(stream.ReadInt32());
		}

		public override void Write(BitStream stream, ref MyStringHash value, MySerializeInfo info)
		{
			stream.WriteInt32((int)value);
		}
	}
}
