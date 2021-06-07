using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;
using VRageMath.PackedVector;

namespace System
{
	public static class SerializationExtensionsMath
	{
		public static void Serialize(this BitStream stream, ref Vector2 vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
		}

		public static void Serialize(this BitStream stream, ref Vector3 vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
		}

		public static void Serialize(this BitStream stream, ref Vector4 vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
			stream.Serialize(ref vec.W);
		}

		public static Quaternion ReadQuaternion(this BitStream stream)
		{
			Quaternion result = default(Quaternion);
			result.X = stream.ReadFloat();
			result.Y = stream.ReadFloat();
			result.Z = stream.ReadFloat();
			result.W = stream.ReadFloat();
			return result;
		}

		public static void WriteQuaternion(this BitStream stream, Quaternion q)
		{
			stream.WriteFloat(q.X);
			stream.WriteFloat(q.Y);
			stream.WriteFloat(q.Z);
			stream.WriteFloat(q.W);
		}

		public static void Serialize(this BitStream stream, ref Quaternion quat)
		{
			stream.Serialize(ref quat.X);
			stream.Serialize(ref quat.Y);
			stream.Serialize(ref quat.Z);
			stream.Serialize(ref quat.W);
		}

		/// <summary>
		/// Serializes normalized quaternion into 52 bits
		/// </summary>
		public static Quaternion ReadQuaternionNorm(this BitStream stream)
		{
			bool num = stream.ReadBool();
			bool num2 = stream.ReadBool();
			bool flag = stream.ReadBool();
			bool flag2 = stream.ReadBool();
			ushort num3 = stream.ReadUInt16();
			ushort num4 = stream.ReadUInt16();
			ushort num5 = stream.ReadUInt16();
			Quaternion result = default(Quaternion);
			result.X = (float)((double)(int)num3 / 65535.0);
			result.Y = (float)((double)(int)num4 / 65535.0);
			result.Z = (float)((double)(int)num5 / 65535.0);
			if (num2)
			{
				result.X = 0f - result.X;
			}
			if (flag)
			{
				result.Y = 0f - result.Y;
			}
			if (flag2)
			{
				result.Z = 0f - result.Z;
			}
			float num6 = 1f - result.X * result.X - result.Y * result.Y - result.Z * result.Z;
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			result.W = (float)Math.Sqrt(num6);
			if (num)
			{
				result.W = 0f - result.W;
			}
			return result;
		}

		/// <summary>
		/// Serializes normalized quaternion into 52 bits
		/// </summary>
		public static void WriteQuaternionNorm(this BitStream stream, Quaternion value)
		{
			stream.WriteBool(value.W < 0f);
			stream.WriteBool(value.X < 0f);
			stream.WriteBool(value.Y < 0f);
			stream.WriteBool(value.Z < 0f);
			stream.WriteUInt16((ushort)((double)Math.Abs(value.X) * 65535.0));
			stream.WriteUInt16((ushort)((double)Math.Abs(value.Y) * 65535.0));
			stream.WriteUInt16((ushort)((double)Math.Abs(value.Z) * 65535.0));
		}

		/// <summary>
		/// Serializes normalized quaternion into 52 bits
		/// </summary>
		public static void SerializeNorm(this BitStream stream, ref Quaternion quat)
		{
			if (stream.Reading)
			{
				quat = stream.ReadQuaternionNorm();
			}
			else
			{
				stream.WriteQuaternionNorm(quat);
			}
		}

		/// <summary>
		/// 64 bits
		/// </summary>
		public static void Serialize(this BitStream stream, ref HalfVector4 vec)
		{
			stream.Serialize(ref vec.PackedValue);
		}

		/// <summary>
		/// 48 bits
		/// </summary>
		public static void Serialize(this BitStream stream, ref HalfVector3 vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
		}

		public static void Serialize(this BitStream stream, ref Vector3D vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
		}

		public static void Serialize(this BitStream stream, ref Vector4D vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
			stream.Serialize(ref vec.W);
		}

		public static void Serialize(this BitStream stream, ref Vector3I vec)
		{
			stream.Serialize(ref vec.X);
			stream.Serialize(ref vec.Y);
			stream.Serialize(ref vec.Z);
		}

		public static void SerializeVariant(this BitStream stream, ref Vector3I vec)
		{
			stream.SerializeVariant(ref vec.X);
			stream.SerializeVariant(ref vec.Y);
			stream.SerializeVariant(ref vec.Z);
		}

		/// <summary>
		/// 64 bits
		/// </summary>
		public static void Write(this BitStream stream, HalfVector4 vec)
		{
			stream.WriteUInt64(vec.PackedValue);
		}

		/// <summary>
		/// 48 bits
		/// </summary>
		public static void Write(this BitStream stream, HalfVector3 vec)
		{
			stream.WriteUInt16(vec.X);
			stream.WriteUInt16(vec.Y);
			stream.WriteUInt16(vec.Z);
		}

		public static void Write(this BitStream stream, Vector3 vec)
		{
			stream.WriteFloat(vec.X);
			stream.WriteFloat(vec.Y);
			stream.WriteFloat(vec.Z);
		}

		public static void Write(this BitStream stream, Vector3D vec)
		{
			stream.WriteDouble(vec.X);
			stream.WriteDouble(vec.Y);
			stream.WriteDouble(vec.Z);
		}

		public static void Write(this BitStream stream, Vector4 vec)
		{
			stream.WriteFloat(vec.X);
			stream.WriteFloat(vec.Y);
			stream.WriteFloat(vec.Z);
			stream.WriteFloat(vec.W);
		}

		public static void Write(this BitStream stream, Vector4D vec)
		{
			stream.WriteDouble(vec.X);
			stream.WriteDouble(vec.Y);
			stream.WriteDouble(vec.Z);
			stream.WriteDouble(vec.W);
		}

		public static void Write(this BitStream stream, Vector3I vec)
		{
			stream.WriteInt32(vec.X);
			stream.WriteInt32(vec.Y);
			stream.WriteInt32(vec.Z);
		}

		/// <summary>
		/// Writes Vector3 with -1, 1 range (uniform-spacing) with specified bit precision.
		/// </summary>
		public static void WriteNormalizedSignedVector3(this BitStream stream, Vector3 vec, int bitCount)
		{
			vec = Vector3.Clamp(vec, Vector3.MinusOne, Vector3.One);
			stream.WriteNormalizedSignedFloat(vec.X, bitCount);
			stream.WriteNormalizedSignedFloat(vec.Y, bitCount);
			stream.WriteNormalizedSignedFloat(vec.Z, bitCount);
		}

		public static void WriteVariant(this BitStream stream, Vector3I vec)
		{
			stream.WriteVariantSigned(vec.X);
			stream.WriteVariantSigned(vec.Y);
			stream.WriteVariantSigned(vec.Z);
		}

		/// <summary>
		/// 64 bits
		/// </summary>
		public static HalfVector4 ReadHalfVector4(this BitStream stream)
		{
			HalfVector4 result = default(HalfVector4);
			result.PackedValue = stream.ReadUInt64();
			return result;
		}

		/// <summary>
		/// 48 bits
		/// </summary>
		public static HalfVector3 ReadHalfVector3(this BitStream stream)
		{
			HalfVector3 result = default(HalfVector3);
			result.X = stream.ReadUInt16();
			result.Y = stream.ReadUInt16();
			result.Z = stream.ReadUInt16();
			return result;
		}

		/// <summary>
		/// Reads Vector3 with -1, 1 range (uniform-spacing) with specified bit precision.
		/// </summary>
		public static Vector3 ReadNormalizedSignedVector3(this BitStream stream, int bitCount)
		{
			Vector3 result = default(Vector3);
			result.X = stream.ReadNormalizedSignedFloat(bitCount);
			result.Y = stream.ReadNormalizedSignedFloat(bitCount);
			result.Z = stream.ReadNormalizedSignedFloat(bitCount);
			return result;
		}

		public static Vector3 ReadVector3(this BitStream stream)
		{
			Vector3 result = default(Vector3);
			result.X = stream.ReadFloat();
			result.Y = stream.ReadFloat();
			result.Z = stream.ReadFloat();
			return result;
		}

		public static Vector3D ReadVector3D(this BitStream stream)
		{
			Vector3D result = default(Vector3D);
			result.X = stream.ReadDouble();
			result.Y = stream.ReadDouble();
			result.Z = stream.ReadDouble();
			return result;
		}

		public static Vector4 ReadVector4(this BitStream stream)
		{
			Vector4 result = default(Vector4);
			result.X = stream.ReadFloat();
			result.Y = stream.ReadFloat();
			result.Z = stream.ReadFloat();
			result.W = stream.ReadFloat();
			return result;
		}

		public static Vector4D ReadVector4D(this BitStream stream)
		{
			Vector4D result = default(Vector4D);
			result.X = stream.ReadDouble();
			result.Y = stream.ReadDouble();
			result.Z = stream.ReadDouble();
			result.W = stream.ReadDouble();
			return result;
		}

		public static Vector3I ReadVector3I(this BitStream stream)
		{
			Vector3I result = default(Vector3I);
			result.X = stream.ReadInt32();
			result.Y = stream.ReadInt32();
			result.Z = stream.ReadInt32();
			return result;
		}

		public static Vector3I ReadVector3IVariant(this BitStream stream)
		{
			Vector3I result = default(Vector3I);
			result.X = stream.ReadInt32Variant();
			result.Y = stream.ReadInt32Variant();
			result.Z = stream.ReadInt32Variant();
			return result;
		}

		/// <summary>
		/// Serializes only position and orientation, 12 + 6.5 = 18.5 bytes
		/// </summary>
		public static void SerializePositionOrientation(this BitStream stream, ref Matrix m)
		{
			if (stream.Writing)
			{
				Vector3 vec = m.Translation;
				Quaternion.CreateFromRotationMatrix(ref m, out Quaternion result);
				stream.Serialize(ref vec);
				stream.SerializeNorm(ref result);
			}
			else
			{
				Vector3 vec2 = default(Vector3);
				Quaternion quat = default(Quaternion);
				stream.Serialize(ref vec2);
				stream.SerializeNorm(ref quat);
				Matrix.CreateFromQuaternion(ref quat, out m);
				m.Translation = vec2;
			}
		}

		/// <summary>
		/// Serializes only position and orientation, 24 + 6.5 = 30.5 bytes
		/// </summary>
		public static void SerializePositionOrientation(this BitStream stream, ref MatrixD m)
		{
			if (stream.Writing)
			{
				Vector3D vec = m.Translation;
				Quaternion.CreateFromRotationMatrix(ref m, out Quaternion result);
				stream.Serialize(ref vec);
				stream.SerializeNorm(ref result);
			}
			else
			{
				Vector3D vec2 = default(Vector3D);
				Quaternion quat = default(Quaternion);
				stream.Serialize(ref vec2);
				stream.SerializeNorm(ref quat);
				MatrixD.CreateFromQuaternion(ref quat, out m);
				m.Translation = vec2;
			}
		}

		public unsafe static void Serialize(this BitStream stream, ref MyBlockOrientation orientation)
		{
			MyBlockOrientation myBlockOrientation = orientation;
			stream.SerializeMemory(&myBlockOrientation, sizeof(MyBlockOrientation) * 8);
			orientation = myBlockOrientation;
		}

		public static void SerializeList(this BitStream stream, ref List<Vector3D> list)
		{
			stream.SerializeList(ref list, delegate(BitStream bs, ref Vector3D vec)
			{
				bs.Serialize(ref vec);
			});
		}

		public static void Serialize(this BitStream stream, ref BoundingBox bb)
		{
			stream.Serialize(ref bb.Min);
			stream.Serialize(ref bb.Max);
		}

		public static void Serialize(this BitStream stream, ref BoundingBoxD bb)
		{
			stream.Serialize(ref bb.Min);
			stream.Serialize(ref bb.Max);
		}
	}
}
