using VRage.Library.Collections;

namespace VRage.Serialization
{
	public class MySerializerFloat : MySerializer<float>
	{
		public override void Clone(ref float value)
		{
		}

		public override bool Equals(ref float a, ref float b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out float value, MySerializeInfo info)
		{
			if (info.IsNormalized && info.IsFixed8)
			{
				value = (float)(int)stream.ReadByte() / 255f;
			}
			else if (info.IsNormalized && info.IsFixed16)
			{
				value = (float)(int)stream.ReadUInt16() / 65535f;
			}
			else
			{
				value = stream.ReadFloat();
			}
		}

		public override void Write(BitStream stream, ref float value, MySerializeInfo info)
		{
			if (info.IsNormalized && info.IsFixed8)
			{
				stream.WriteByte((byte)(value * 255f));
			}
			else if (info.IsNormalized && info.IsFixed16)
			{
				stream.WriteUInt16((ushort)(value * 65535f));
			}
			else
			{
				stream.WriteFloat(value);
			}
		}
	}
}
