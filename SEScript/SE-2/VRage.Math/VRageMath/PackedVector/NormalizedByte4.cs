using System;
using System.Globalization;

namespace VRageMath.PackedVector
{
	/// <summary>
	/// Packed vector type containing four 8-bit signed normalized values, ranging from −1 to 1.
	/// </summary>
	public struct NormalizedByte4 : IPackedVector<uint>, IPackedVector, IEquatable<NormalizedByte4>
	{
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
		/// Initializes a new instance of the NormalizedByte4 class.
		/// </summary>
		/// <param name="x">Initial value for the x component.</param><param name="y">Initial value for the y component.</param><param name="z">Initial value for the z component.</param><param name="w">Initial value for the w component.</param>
		public NormalizedByte4(float x, float y, float z, float w)
		{
			packedValue = PackHelper(x, y, z, w);
		}

		/// <summary>
		/// Initializes a new instance of the NormalizedByte4 structure.
		/// </summary>
		/// <param name="vector">A vector containing the initial values for the components of the NormalizedByte4 structure.</param>
		public NormalizedByte4(Vector4 vector)
		{
			packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are the same.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(NormalizedByte4 a, NormalizedByte4 b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Compares the current instance of a class to another instance to determine whether they are different.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator !=(NormalizedByte4 a, NormalizedByte4 b)
		{
			return !a.Equals(b);
		}

		void IPackedVector.PackFromVector4(Vector4 vector)
		{
			packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
		}

		private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
		{
			return PackUtils.PackSNorm(255u, vectorX) | (PackUtils.PackSNorm(255u, vectorY) << 8) | (PackUtils.PackSNorm(255u, vectorZ) << 16) | (PackUtils.PackSNorm(255u, vectorW) << 24);
		}

		/// <summary>
		/// Expands the packed representation into a Vector4.
		/// </summary>
		public Vector4 ToVector4()
		{
			Vector4 result = default(Vector4);
			result.X = PackUtils.UnpackSNorm(255u, packedValue);
			result.Y = PackUtils.UnpackSNorm(255u, packedValue >> 8);
			result.Z = PackUtils.UnpackSNorm(255u, packedValue >> 16);
			result.W = PackUtils.UnpackSNorm(255u, packedValue >> 24);
			return result;
		}

		/// <summary>
		/// Returns a string representation of the current instance.
		/// </summary>
		public override string ToString()
		{
			return packedValue.ToString("X8", CultureInfo.InvariantCulture);
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
			if (obj is NormalizedByte4)
			{
				return Equals((NormalizedByte4)obj);
			}
			return false;
		}

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object.
		/// </summary>
		/// <param name="other">The object with which to make the comparison.</param>
		public bool Equals(NormalizedByte4 other)
		{
			return packedValue.Equals(other.packedValue);
		}
	}
}
