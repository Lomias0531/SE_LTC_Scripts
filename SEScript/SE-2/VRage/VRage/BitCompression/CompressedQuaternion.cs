using System;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.BitCompression
{
	public static class CompressedQuaternion
	{
		private const float MIN_QF_LENGTH = -0.707106531f;

		private const float MAX_QF_LENGTH = 0.707106531f;

		private const int QF_BITS = 9;

		private const int QF_VALUE = 511;

		private const float QF_SCALE = 511f;

		private const float QF_SCALE_INV = 0.00195694715f;

		public static void Write(BitStream stream, Quaternion value)
		{
			value.Normalize();
			int num = value.FindLargestIndex();
			if (value.GetComponent(num) < 0f)
			{
				value = -value;
			}
			stream.WriteInt32(num, 2);
			for (int i = 0; i < 4; i++)
			{
				if (i != num)
				{
					uint value2 = (uint)Math.Floor((value.GetComponent(i) - -0.707106531f) / 1.41421306f * 511f + 0.5f);
					stream.WriteUInt32(value2, 9);
				}
			}
		}

		public static Quaternion Read(BitStream stream)
		{
			Quaternion identity = Quaternion.Identity;
			int num = stream.ReadInt32(2);
			float num2 = 0f;
			for (int i = 0; i < 4; i++)
			{
				if (i != num)
				{
					float num3 = (float)stream.ReadInt32(9) * 0.00195694715f * 1.41421306f + -0.707106531f;
					identity.SetComponent(i, num3);
					num2 += num3 * num3;
				}
			}
			identity.SetComponent(num, (float)Math.Sqrt(1f - num2));
			identity.Normalize();
			return identity;
		}

		public static bool CompressedQuaternionUnitTest()
		{
			BitStream bitStream = new BitStream();
			bitStream.ResetWrite();
			Quaternion identity = Quaternion.Identity;
			bitStream.WriteQuaternionNormCompressed(identity);
			bitStream.ResetRead();
			Quaternion value = bitStream.ReadQuaternionNormCompressed();
			bool flag = !identity.Equals(value, 0.00195694715f);
			bitStream.ResetWrite();
			identity = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathF.PI / 3f);
			bitStream.WriteQuaternionNormCompressed(identity);
			bitStream.ResetRead();
			value = bitStream.ReadQuaternionNormCompressed();
			flag |= !identity.Equals(value, 0.00195694715f);
			bitStream.ResetWrite();
			Vector3 axis = new Vector3(1f, -1f, 3f);
			axis.Normalize();
			identity = Quaternion.CreateFromAxisAngle(axis, MathF.PI / 3f);
			bitStream.WriteQuaternionNormCompressed(identity);
			bitStream.ResetRead();
			value = bitStream.ReadQuaternionNormCompressed();
			return flag | !identity.Equals(value, 0.00195694715f);
		}
	}
}
