using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[ProtoContract]
	public struct Vector3Ushort
	{
		protected class VRageMath_Vector3Ushort_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3Ushort owner, in ushort value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3Ushort owner, out ushort value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3Ushort_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3Ushort owner, in ushort value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3Ushort owner, out ushort value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3Ushort_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3Ushort owner, in ushort value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3Ushort owner, out ushort value)
			{
				value = owner.Z;
			}
		}

		[ProtoMember(1)]
		public ushort X;

		[ProtoMember(4)]
		public ushort Y;

		[ProtoMember(7)]
		public ushort Z;

		public Vector3Ushort(ushort x, ushort y, ushort z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString()
		{
			return X + ", " + Y + ", " + Z;
		}

		public static Vector3Ushort operator *(Vector3Ushort v, ushort t)
		{
			return new Vector3Ushort((ushort)(t * v.X), (ushort)(t * v.Y), (ushort)(t * v.Z));
		}

		public static Vector3 operator *(Vector3 vector, Vector3Ushort ushortVector)
		{
			return ushortVector * vector;
		}

		public static Vector3 operator *(Vector3Ushort ushortVector, Vector3 vector)
		{
			return new Vector3((float)(int)ushortVector.X * vector.X, (float)(int)ushortVector.Y * vector.Y, (float)(int)ushortVector.Z * vector.Z);
		}

		public static explicit operator Vector3(Vector3Ushort v)
		{
			return new Vector3((int)v.X, (int)v.Y, (int)v.Z);
		}
	}
}
