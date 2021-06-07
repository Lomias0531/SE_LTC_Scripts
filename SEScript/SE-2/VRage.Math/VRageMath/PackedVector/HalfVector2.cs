using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
	/// <summary>
	/// Packed vector type containing two 16-bit floating-point values.
	/// </summary>
	[Serializable]
	public struct HalfVector2 : IPackedVector<uint>, IPackedVector, IEquatable<HalfVector2>
	{
		protected class VRageMath_PackedVector_HalfVector2_003C_003EpackedValue_003C_003EAccessor : IMemberAccessor<HalfVector2, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector2 owner, in uint value)
			{
				owner.packedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector2 owner, out uint value)
			{
				value = owner.packedValue;
			}
		}

		protected class VRageMath_PackedVector_HalfVector2_003C_003EPackedValue_003C_003EAccessor : IMemberAccessor<HalfVector2, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref HalfVector2 owner, in uint value)
			{
				owner.PackedValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref HalfVector2 owner, out uint value)
			{
				value = owner.PackedValue;
			}
		}

		private uint packedValue;

		/// <summary>
		/// Directly gets or sets the packed representation of the value.
		/// </summary>
		public uint PackedValue
		{
			get
			{
				return packedValue;
			}
			set
			{
				packedValue = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the HalfVector2 structure.
		/// </summary>
		/// <param name="x">Initial value for the x component.</param><param name="y">Initial value for the y component.</param>
		public HalfVector2(float x, float y)
		{
			packedValue = PackHelper(x, y);
		}

		/// <summary>
		/// Initializes a new instance of the HalfVector2 structure.
		/// </summary>
		/// <param name="vector">A vector containing the initial values for the components of the HalfVector2 structure.</param>
		public HalfVector2(Vector2 vector)
		{
			packedValue = PackHelper(vector.X, vector.Y);
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are the same.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(HalfVector2 a, HalfVector2 b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are different.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator !=(HalfVector2 a, HalfVector2 b)
		{
			return !a.Equals(b);
		}

		void IPackedVector.PackFromVector4(Vector4 vector)
		{
			packedValue = PackHelper(vector.X, vector.Y);
		}

		private static uint PackHelper(float vectorX, float vectorY)
		{
			return (uint)(HalfUtils.Pack(vectorX) | (HalfUtils.Pack(vectorY) << 16));
		}

		/// <summary>
		/// Expands the HalfVector2 to a Vector2.
		/// </summary>
		public Vector2 ToVector2()
		{
			Vector2 result = default(Vector2);
			result.X = HalfUtils.Unpack((ushort)packedValue);
			result.Y = HalfUtils.Unpack((ushort)(packedValue >> 16));
			return result;
		}

		Vector4 IPackedVector.ToVector4()
		{
			Vector2 vector = ToVector2();
			return new Vector4(vector.X, vector.Y, 0f, 1f);
		}

		/// <summary>
		/// Returns a string representation of the current instance.
		/// </summary>
		public override string ToString()
		{
			return ToVector2().ToString();
		}

		/// <summary>
		/// Gets the hash code for the current instance.
		/// </summary>
		public override int GetHashCode()
		{
			return packedValue.GetHashCode();
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object with which to make the comparison.</param>
		public override bool Equals(object obj)
		{
			if (obj is HalfVector2)
			{
				return Equals((HalfVector2)obj);
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="other">The object with which to make the comparison.</param>
		public bool Equals(HalfVector2 other)
		{
			return packedValue.Equals(other.packedValue);
		}
	}
}
