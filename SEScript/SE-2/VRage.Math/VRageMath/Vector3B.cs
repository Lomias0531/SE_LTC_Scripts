using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector3B
	{
		protected class VRageMath_Vector3B_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3B, sbyte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3B owner, in sbyte value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3B owner, out sbyte value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3B_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3B, sbyte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3B owner, in sbyte value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3B owner, out sbyte value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3B_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3B, sbyte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3B owner, in sbyte value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3B owner, out sbyte value)
			{
				value = owner.Z;
			}
		}

		[ProtoMember(1)]
		public sbyte X;

		[ProtoMember(4)]
		public sbyte Y;

		[ProtoMember(7)]
		public sbyte Z;

		public static readonly Vector3B Zero = default(Vector3B);

		public static Vector3B Up = new Vector3B(0, 1, 0);

		public static Vector3B Down = new Vector3B(0, -1, 0);

		public static Vector3B Right = new Vector3B(1, 0, 0);

		public static Vector3B Left = new Vector3B(-1, 0, 0);

		public static Vector3B Forward = new Vector3B(0, 0, -1);

		public static Vector3B Backward = new Vector3B(0, 0, 1);

		public Vector3B(sbyte x, sbyte y, sbyte z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3B(Vector3I vec)
		{
			X = (sbyte)vec.X;
			Y = (sbyte)vec.Y;
			Z = (sbyte)vec.Z;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z;
		}

		public override int GetHashCode()
		{
			return ((byte)Z << 16) | ((byte)Y << 8) | (byte)X;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				Vector3B? vector3B = obj as Vector3B?;
				if (vector3B.HasValue)
				{
					return this == vector3B.Value;
				}
			}
			return false;
		}

		public static Vector3 operator *(Vector3 vector, Vector3B shortVector)
		{
			return shortVector * vector;
		}

		public static Vector3 operator *(Vector3B shortVector, Vector3 vector)
		{
			return new Vector3((float)shortVector.X * vector.X, (float)shortVector.Y * vector.Y, (float)shortVector.Z * vector.Z);
		}

		public static implicit operator Vector3I(Vector3B vec)
		{
			return new Vector3I(vec.X, vec.Y, vec.Z);
		}

		public static Vector3B Round(Vector3 vec)
		{
			return new Vector3B((sbyte)Math.Round(vec.X), (sbyte)Math.Round(vec.Y), (sbyte)Math.Round(vec.Z));
		}

		public static bool operator ==(Vector3B a, Vector3B b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3B a, Vector3B b)
		{
			return !(a == b);
		}

		public static Vector3B operator -(Vector3B me)
		{
			return new Vector3B((sbyte)(-me.X), (sbyte)(-me.Y), (sbyte)(-me.Z));
		}

		/// <summary>
		/// Puts Vector3 into Vector3B, value -127 represents -range, 128 represents range
		/// </summary>
		public static Vector3B Fit(Vector3 vec, float range)
		{
			return Round(vec / range * 128f);
		}
	}
}
