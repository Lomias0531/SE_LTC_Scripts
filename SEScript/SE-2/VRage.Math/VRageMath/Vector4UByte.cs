using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector4UByte
	{
		protected class VRageMath_Vector4UByte_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector4UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4UByte owner, in byte value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4UByte owner, out byte value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector4UByte_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector4UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4UByte owner, in byte value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4UByte owner, out byte value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector4UByte_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector4UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4UByte owner, in byte value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4UByte owner, out byte value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Vector4UByte_003C_003EW_003C_003EAccessor : IMemberAccessor<Vector4UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4UByte owner, in byte value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4UByte owner, out byte value)
			{
				value = owner.W;
			}
		}

		[ProtoMember(1)]
		public byte X;

		[ProtoMember(4)]
		public byte Y;

		[ProtoMember(7)]
		public byte Z;

		[ProtoMember(10)]
		public byte W;

		public byte this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return X;
				case 1:
					return Y;
				case 2:
					return Z;
				case 3:
					return W;
				default:
					throw new Exception("Index out of bounds");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					X = value;
					break;
				case 1:
					Y = value;
					break;
				case 2:
					Z = value;
					break;
				case 3:
					W = value;
					break;
				default:
					throw new Exception("Index out of bounds");
				}
			}
		}

		public Vector4UByte(byte x, byte y, byte z, byte w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z + ", " + W;
		}

		public static Vector4UByte Round(Vector3 vec)
		{
			return Round(new Vector4(vec.X, vec.Y, vec.Z, 0f));
		}

		public static Vector4UByte Round(Vector4 vec)
		{
			return new Vector4UByte((byte)Math.Round(vec.X), (byte)Math.Round(vec.Y), (byte)Math.Round(vec.Z), 0);
		}

		/// <summary>
		/// Normalizes Vector3 into Vector4UByte, scales vector from (-range, range) to (0, 255)
		/// </summary>
		public static Vector4UByte Normalize(Vector3 vec, float range)
		{
			return Round((vec / range / 2f + new Vector3(0.5f)) * 255f);
		}
	}
}
