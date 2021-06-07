using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
	/// <summary>
	/// Packed vector type containing four 16-bit floating-point values.
	/// </summary>
	[Serializable]
	public struct HalfVector3
	{
		protected class VRageMath_PackedVector_HalfVector3_003C_003EX_003C_003EAccessor : IMemberAccessor<HalfVector3, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector3 owner, in ushort value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector3 owner, out ushort value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_PackedVector_HalfVector3_003C_003EY_003C_003EAccessor : IMemberAccessor<HalfVector3, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector3 owner, in ushort value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector3 owner, out ushort value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_PackedVector_HalfVector3_003C_003EZ_003C_003EAccessor : IMemberAccessor<HalfVector3, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector3 owner, in ushort value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector3 owner, out ushort value)
			{
				value = owner.Z;
			}
		}

		public ushort X;

		public ushort Y;

		public ushort Z;

		/// <summary>
		/// Initializes a new instance of the HalfVector3 class.
		/// </summary>
		/// <param name="x">Initial value for the x component.</param><param name="y">Initial value for the y component.</param><param name="z">Initial value for the z component.</param><param name="w">Initial value for the w component.</param>
		public HalfVector3(float x, float y, float z)
		{
			X = HalfUtils.Pack(x);
			Y = HalfUtils.Pack(y);
			Z = HalfUtils.Pack(z);
		}

		/// <summary>
		/// Initializes a new instance of the HalfVector3 structure.
		/// </summary>
		/// <param name="vector">A vector containing the initial values for the components of the HalfVector3 structure.</param>
		public HalfVector3(Vector3 vector)
			: this(vector.X, vector.Y, vector.Z)
		{
		}

		/// <summary>
		/// Expands the packed representation into a Vector4.
		/// </summary>
		public Vector3 ToVector3()
		{
			Vector3 result = default(Vector3);
			result.X = HalfUtils.Unpack(X);
			result.Y = HalfUtils.Unpack(Y);
			result.Z = HalfUtils.Unpack(Z);
			return result;
		}

		public HalfVector4 ToHalfVector4()
		{
			HalfVector4 result = default(HalfVector4);
			result.PackedValue = (X | ((ulong)Y << 16) | ((ulong)Z << 32));
			return result;
		}

		public static implicit operator HalfVector3(Vector3 v)
		{
			return new HalfVector3(v);
		}

		public static implicit operator Vector3(HalfVector3 v)
		{
			return v.ToVector3();
		}

		/// <summary>
		/// Returns a string representation of the current instance.
		/// </summary>
		public override string ToString()
		{
			return ToVector3().ToString();
		}
	}
}
