using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerDouble : MySerializer<double>
	{
		public override void Clone(ref double value)
		{
		}

		public override bool Equals(ref double a, ref double b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out double value, MySerializeInfo info)
		{
			value = stream.ReadDouble();
		}

		public override void Write(BitStream stream, ref double value, MySerializeInfo info)
		{
			stream.WriteDouble(value);
		}
	}
}
