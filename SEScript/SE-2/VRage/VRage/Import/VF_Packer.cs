using System;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Import
{
	public class VF_Packer
	{
		public static short PackAmbientAndAlpha(float ambient, byte alpha)
		{
			return (short)((short)(ambient * 8191f) + (short)(alpha << 13));
		}

		public static float UnpackAmbient(float packed)
		{
			return packed % 8192f / 8191f;
		}

		public static float UnpackAmbient(short packed)
		{
			return (float)packed % 8192f / 8191f;
		}

		public static byte UnpackAlpha(short packed)
		{
			return (byte)Math.Abs(packed / 8192);
		}

		public static byte UnpackAlpha(float packed)
		{
			return (byte)Math.Abs(packed / 8192f);
		}

		public static uint PackNormal(Vector3 normal)
		{
			return PackNormal(ref normal);
		}

		public static uint PackNormal(ref Vector3 normal)
		{
			Vector3 vector = normal;
			vector.X = 0.5f * (vector.X + 1f);
			vector.Y = 0.5f * (vector.Y + 1f);
			ushort num = (ushort)(vector.X * 32767f);
			uint num2 = (ushort)(vector.Y * 32767f);
			ushort num3 = (ushort)((vector.Z > 0f) ? 1 : 0);
			return (uint)(num | (ushort)(num3 << 15) | (int)(num2 << 16));
		}

		public static uint PackTangentSign(ref Vector4 tangent)
		{
			Vector4 vector = tangent;
			vector.X = 0.5f * (vector.X + 1f);
			vector.Y = 0.5f * (vector.Y + 1f);
			ushort num = (ushort)(vector.X * 32767f);
			uint num2 = (ushort)(vector.Y * 32767f);
			ushort num3 = (ushort)((vector.Z > 0f) ? 1 : 0);
			int num4 = num | (ushort)(num3 << 15);
			num2 |= (ushort)(((vector.W > 0f) ? 1 : 0) << 15);
			return (uint)(num4 | (int)(num2 << 16));
		}

		public static Vector4 UnpackTangentSign(ref Byte4 packedTangent)
		{
			Vector4 vector = packedTangent.ToVector4();
			float num = (vector.Y > 127.5f) ? 1f : (-1f);
			float num2 = (vector.W > 127.5f) ? 1f : (-1f);
			if (num > 0f)
			{
				vector.Y -= 128f;
			}
			if (num2 > 0f)
			{
				vector.W -= 128f;
			}
			float num3 = vector.X + 256f * vector.Y;
			float num4 = vector.Z + 256f * vector.W;
			num3 /= 32767f;
			num4 /= 32767f;
			float num5 = 2f * num3 - 1f;
			float num6 = 2f * num4 - 1f;
			float num7 = Math.Max(0f, 1f - num5 * num5 - num6 * num6);
			float z = num * (float)Math.Sqrt(num7);
			return new Vector4(num5, num6, z, num2);
		}

		public static Byte4 PackNormalB4(ref Vector3 normal)
		{
			uint packedValue = PackNormal(ref normal);
			Byte4 result = default(Byte4);
			result.PackedValue = packedValue;
			return result;
		}

		public static Byte4 PackTangentSignB4(ref Vector4 tangentW)
		{
			uint packedValue = PackTangentSign(ref tangentW);
			Byte4 result = default(Byte4);
			result.PackedValue = packedValue;
			return result;
		}

		public static Vector3 UnpackNormal(ref uint packedNormal)
		{
			Byte4 packedNormal2 = default(Byte4);
			packedNormal2.PackedValue = packedNormal;
			return UnpackNormal(ref packedNormal2);
		}

		public static Vector3 UnpackNormal(uint packedNormal)
		{
			Byte4 packedNormal2 = default(Byte4);
			packedNormal2.PackedValue = packedNormal;
			return UnpackNormal(ref packedNormal2);
		}

		public static Vector3 UnpackNormal(Byte4 packedNormal)
		{
			return UnpackNormal(ref packedNormal);
		}

		public static Vector3 UnpackNormal(ref Byte4 packedNormal)
		{
			Vector4 vector = packedNormal.ToVector4();
			float num = (vector.Y > 127.5f) ? 1f : (-1f);
			if (num > 0f)
			{
				vector.Y -= 128f;
			}
			float num2 = vector.X + 256f * vector.Y;
			float num3 = vector.Z + 256f * vector.W;
			num2 /= 32767f;
			num3 /= 32767f;
			float num4 = 2f * num2 - 1f;
			float num5 = 2f * num3 - 1f;
			float num6 = Math.Max(0f, 1f - num4 * num4 - num5 * num5);
			float z = num * (float)Math.Sqrt(num6);
			return new Vector3(num4, num5, z);
		}

		public static HalfVector4 PackPosition(Vector3 position)
		{
			Vector3 position2 = position;
			return PositionPacker.PackPosition(ref position2);
		}

		public static HalfVector4 PackPosition(ref Vector3 position)
		{
			return PositionPacker.PackPosition(ref position);
		}

		public static Vector3 UnpackPosition(ref HalfVector4 position)
		{
			return PositionPacker.UnpackPosition(ref position);
		}

		public static Vector3 UnpackPosition(HalfVector4 position)
		{
			return PositionPacker.UnpackPosition(ref position);
		}

		public static Vector3 RepackModelPosition(ref Vector3 position)
		{
			HalfVector4 position2 = PackPosition(ref position);
			return UnpackPosition(ref position2);
		}
	}
}
