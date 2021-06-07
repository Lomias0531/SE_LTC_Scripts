using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector4I : IComparable<Vector4I>
	{
		public class EqualityComparer : IEqualityComparer<Vector4I>, IComparer<Vector4I>
		{
			public bool Equals(Vector4I x, Vector4I y)
			{
				if (x.X == y.X && x.Y == y.Y && x.Z == y.Z)
				{
					return x.W == y.W;
				}
				return false;
			}

			public int GetHashCode(Vector4I obj)
			{
				return (((((obj.X * 397) ^ obj.Y) * 397) ^ obj.Z) * 397) ^ obj.W;
			}

			public int Compare(Vector4I x, Vector4I y)
			{
				return x.CompareTo(y);
			}
		}

		protected class VRageMath_Vector4I_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector4I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4I owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4I owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector4I_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector4I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4I owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4I owner, out int value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector4I_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector4I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4I owner, in int value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4I owner, out int value)
			{
				value = owner.Z;
			}
		}

		protected class VRageMath_Vector4I_003C_003EW_003C_003EAccessor : IMemberAccessor<Vector4I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector4I owner, in int value)
			{
				owner.W = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector4I owner, out int value)
			{
				value = owner.W;
			}
		}

		[ProtoMember(1)]
		public int X;

		[ProtoMember(4)]
		public int Y;

		[ProtoMember(7)]
		public int Z;

		[ProtoMember(10)]
		public int W;

		public static readonly EqualityComparer Comparer = new EqualityComparer();

		public int this[int index]
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

		public Vector4I(int x, int y, int z, int w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Vector4I(Vector3I xyz, int w)
		{
			X = xyz.X;
			Y = xyz.Y;
			Z = xyz.Z;
			W = w;
		}

		public static explicit operator Byte4(Vector4I xyzw)
		{
			return new Byte4(xyzw.X, xyzw.Y, xyzw.Z, xyzw.W);
		}

		public int CompareTo(Vector4I other)
		{
			int num = X - other.X;
			int num2 = Y - other.Y;
			int num3 = Z - other.Z;
			int result = W - other.W;
			if (num == 0)
			{
				if (num2 == 0)
				{
					if (num3 == 0)
					{
						return result;
					}
					return num3;
				}
				return num2;
			}
			return num;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z + ", " + W;
		}
	}
}
