using System;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.Serialization
{
	public class MySerializerQuaternion : MySerializer<Quaternion>
	{
		public override void Clone(ref Quaternion value)
		{
		}

		public override bool Equals(ref Quaternion a, ref Quaternion b)
		{
			return a == b;
		}

		public override void Read(BitStream stream, out Quaternion value, MySerializeInfo info)
		{
			if (info.IsNormalized)
			{
				bool num = stream.ReadBool();
				bool num2 = stream.ReadBool();
				bool flag = stream.ReadBool();
				bool flag2 = stream.ReadBool();
				ushort num3 = stream.ReadUInt16();
				ushort num4 = stream.ReadUInt16();
				ushort num5 = stream.ReadUInt16();
				value.X = (float)((double)(int)num3 / 65535.0);
				value.Y = (float)((double)(int)num4 / 65535.0);
				value.Z = (float)((double)(int)num5 / 65535.0);
				if (num2)
				{
					value.X = 0f - value.X;
				}
				if (flag)
				{
					value.Y = 0f - value.Y;
				}
				if (flag2)
				{
					value.Z = 0f - value.Z;
				}
				float num6 = 1f - value.X * value.X - value.Y * value.Y - value.Z * value.Z;
				if (num6 < 0f)
				{
					num6 = 0f;
				}
				value.W = (float)Math.Sqrt(num6);
				if (num)
				{
					value.W = 0f - value.W;
				}
			}
			else
			{
				value.X = stream.ReadFloat();
				value.Y = stream.ReadFloat();
				value.Z = stream.ReadFloat();
				value.W = stream.ReadFloat();
			}
		}

		public override void Write(BitStream stream, ref Quaternion value, MySerializeInfo info)
		{
			if (info.IsNormalized)
			{
				stream.WriteBool(value.W < 0f);
				stream.WriteBool(value.X < 0f);
				stream.WriteBool(value.Y < 0f);
				stream.WriteBool(value.Z < 0f);
				stream.WriteUInt16((ushort)((double)Math.Abs(value.X) * 65535.0));
				stream.WriteUInt16((ushort)((double)Math.Abs(value.Y) * 65535.0));
				stream.WriteUInt16((ushort)((double)Math.Abs(value.Z) * 65535.0));
			}
			else
			{
				stream.WriteFloat(value.X);
				stream.WriteFloat(value.Y);
				stream.WriteFloat(value.Z);
				stream.WriteFloat(value.W);
			}
		}
	}
}
