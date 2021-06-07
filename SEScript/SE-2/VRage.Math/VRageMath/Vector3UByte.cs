using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector3UByte
	{
		public class EqualityComparer : IEqualityComparer<Vector3UByte>, IComparer<Vector3UByte>
		{
			public bool Equals(Vector3UByte x, Vector3UByte y)
			{
				return (x.X == y.X) & (x.Y == y.Y) & (x.Z == y.Z);
			}

			public int GetHashCode(Vector3UByte obj)
			{
				return (((obj.X * 397) ^ obj.Y) * 397) ^ obj.Z;
			}

			public int Compare(Vector3UByte a, Vector3UByte b)
			{
				int num = a.X - b.X;
				int num2 = a.Y - b.Y;
				int result = a.Z - b.Z;
				if (num == 0)
				{
					if (num2 == 0)
					{
						return result;
					}
					return num2;
				}
				return num;
			}
		}

		protected class VRageMath_Vector3UByte_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3UByte owner, in byte value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3UByte owner, out byte value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3UByte_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3UByte owner, in byte value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3UByte owner, out byte value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3UByte_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3UByte, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3UByte owner, in byte value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3UByte owner, out byte value)
			{
				value = owner.Z;
			}
		}

		public static readonly EqualityComparer Comparer = new EqualityComparer();

		public static Vector3UByte Zero = new Vector3UByte(0, 0, 0);

		[ProtoMember(1)]
		public byte X;

		[ProtoMember(4)]
		public byte Y;

		[ProtoMember(7)]
		public byte Z;

		private static Vector3 m_clampBoundary = new Vector3(255f);

		public Vector3UByte(byte x, byte y, byte z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3UByte(Vector3I vec)
		{
			X = (byte)vec.X;
			Y = (byte)vec.Y;
			Z = (byte)vec.Z;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z;
		}

		public override int GetHashCode()
		{
			return (Z << 16) | (Y << 8) | X;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				Vector3UByte? vector3UByte = obj as Vector3UByte?;
				if (vector3UByte.HasValue)
				{
					return this == vector3UByte.Value;
				}
			}
			return false;
		}

		public static bool operator ==(Vector3UByte a, Vector3UByte b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3UByte a, Vector3UByte b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z != b.Z;
			}
			return true;
		}

		public static Vector3UByte Round(Vector3 vec)
		{
			return new Vector3UByte((byte)Math.Round(vec.X), (byte)Math.Round(vec.Y), (byte)Math.Round(vec.Z));
		}

		public static Vector3UByte Floor(Vector3 vec)
		{
			return new Vector3UByte((byte)Math.Floor(vec.X), (byte)Math.Floor(vec.Y), (byte)Math.Floor(vec.Z));
		}

		public static implicit operator Vector3I(Vector3UByte vec)
		{
			return new Vector3I(vec.X, vec.Y, vec.Z);
		}

		public int LengthSquared()
		{
			return X * X + Y * Y + Z * Z;
		}

		/// <summary>
		/// Returns true when all components are 127
		/// </summary>
		public static bool IsMiddle(Vector3UByte vec)
		{
			if (vec.X == 127 && vec.Y == 127)
			{
				return vec.Z == 127;
			}
			return false;
		}

		/// <summary>
		/// Normalizes Vector3 into Vector4UByte, scales vector from (-range, range) to (0, 255).
		/// Unsafe for values "range &gt;= any_vec_value / 257";
		/// </summary>
		public static Vector3UByte Normalize(Vector3 vec, float range)
		{
			Vector3 value = (vec / range / 2f + new Vector3(0.5f)) * 255f;
			Vector3.Clamp(ref value, ref Vector3.Zero, ref m_clampBoundary, out value);
			return new Vector3UByte((byte)value.X, (byte)value.Y, (byte)value.Z);
		}

		/// <summary>
		/// Unpacks Vector3 from Vector3UByte, scales vector from (0, 255) to (-range, range)
		/// </summary>
		public static Vector3 Denormalize(Vector3UByte vec, float range)
		{
			float num = 0.00196078443f;
			return (new Vector3((int)vec.X, (int)vec.Y, (int)vec.Z) / 255f - new Vector3(0.5f - num)) * 2f * range;
		}
	}
}
