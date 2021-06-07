using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerGuid : MySerializer<Guid>
	{
		public override void Clone(ref Guid value)
		{
		}

		public override bool Equals(ref Guid a, ref Guid b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out Guid value, MySerializeInfo info)
		{
			string g = stream.ReadPrefixLengthString(info.Encoding);
			value = new Guid(g);
		}

		public override void Write(BitStream stream, ref Guid value, MySerializeInfo info)
		{
			string text = value.ToString();
			stream.WritePrefixLengthString(text, 0, text.Length, info.Encoding);
		}
	}
}
