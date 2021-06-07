using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector3S
	{
		protected class VRageMath_Vector3S_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3S, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3S owner, in short value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3S owner, out short value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3S_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3S, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3S owner, in short value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3S owner, out short value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3S_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3S, short>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3S owner, in short value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3S owner, out short value)
			{
				value = owner.Z;
			}
		}

		[ProtoMember(1)]
		public short X;

		[ProtoMember(4)]
		public short Y;

		[ProtoMember(7)]
		public short Z;

		public static Vector3S Up = new Vector3S(0, 1, 0);

		public static Vector3S Down = new Vector3S(0, -1, 0);

		public static Vector3S Right = new Vector3S(1, 0, 0);

		public static Vector3S Left = new Vector3S(-1, 0, 0);

		public static Vector3S Forward = new Vector3S(0, 0, -1);

		public static Vector3S Backward = new Vector3S(0, 0, 1);

		public Vector3S(Vector3I vec)
			: this(ref vec)
		{
		}

		public Vector3S(ref Vector3I vec)
		{
			X = (short)vec.X;
			Y = (short)vec.Y;
			Z = (short)vec.Z;
		}

		public Vector3S(short x, short y, short z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3S(float x, float y, float z)
		{
			X = (short)x;
			Y = (short)y;
			Z = (short)z;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z;
		}

		public override int GetHashCode()
		{
			return (((X * 397) ^ Y) * 397) ^ Z;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				Vector3S? vector3S = obj as Vector3S?;
				if (vector3S.HasValue)
				{
					return this == vector3S.Value;
				}
			}
			return false;
		}

		public static Vector3S operator *(Vector3S v, short t)
		{
			return new Vector3S((short)(t * v.X), (short)(t * v.Y), (short)(t * v.Z));
		}

		public static Vector3 operator *(Vector3S v, float t)
		{
			return new Vector3(t * (float)v.X, t * (float)v.Y, t * (float)v.Z);
		}

		public static Vector3 operator *(Vector3 vector, Vector3S shortVector)
		{
			return shortVector * vector;
		}

		public static Vector3 operator *(Vector3S shortVector, Vector3 vector)
		{
			return new Vector3((float)shortVector.X * vector.X, (float)shortVector.Y * vector.Y, (float)shortVector.Z * vector.Z);
		}

		public static bool operator ==(Vector3S v1, Vector3S v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y)
			{
				return v1.Z == v2.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3S v1, Vector3S v2)
		{
			if (v1.X == v2.X && v1.Y == v2.Y)
			{
				return v1.Z != v2.Z;
			}
			return true;
		}

		public static Vector3S Round(Vector3 v)
		{
			return new Vector3S((short)Math.Round(v.X), (short)Math.Round(v.Y), (short)Math.Round(v.Z));
		}

		public static implicit operator Vector3I(Vector3S me)
		{
			return new Vector3I(me.X, me.Y, me.Z);
		}

		public static Vector3I operator -(Vector3S op1, Vector3B op2)
		{
			return new Vector3I(op1.X - op2.X, op1.Y - op2.Y, op1.Z - op2.Z);
		}
	}
}
