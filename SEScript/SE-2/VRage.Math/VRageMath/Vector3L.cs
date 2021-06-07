using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[Serializable]
	[ProtoContract]
	public struct Vector3L : IEquatable<Vector3L>, IComparable<Vector3L>
	{
		public class EqualityComparer : IEqualityComparer<Vector3L>, IComparer<Vector3L>
		{
			public bool Equals(Vector3L x, Vector3L y)
			{
				return (x.X == y.X) & (x.Y == y.Y) & (x.Z == y.Z);
			}

			public int GetHashCode(Vector3L obj)
			{
				return (int)((((obj.X * 397) ^ obj.Y) * 397) ^ obj.Z);
			}

			public int Compare(Vector3L x, Vector3L y)
			{
				return x.CompareTo(y);
			}
		}

		protected class VRageMath_Vector3L_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3L, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3L owner, in long value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3L owner, out long value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3L_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3L, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3L owner, in long value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3L owner, out long value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3L_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3L, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3L owner, in long value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3L owner, out long value)
			{
				value = owner.Z;
			}
		}

		public static readonly EqualityComparer Comparer = new EqualityComparer();

		public static Vector3L UnitX = new Vector3L(1L, 0L, 0L);

		public static Vector3L UnitY = new Vector3L(0L, 1L, 0L);

		public static Vector3L UnitZ = new Vector3L(0L, 0L, 1L);

		public static Vector3L Zero = new Vector3L(0L, 0L, 0L);

		public static Vector3L MaxValue = new Vector3L(long.MaxValue, long.MaxValue, long.MaxValue);

		public static Vector3L MinValue = new Vector3L(long.MinValue, long.MinValue, long.MinValue);

		public static Vector3L Up = new Vector3L(0L, 1L, 0L);

		public static Vector3L Down = new Vector3L(0L, -1L, 0L);

		public static Vector3L Right = new Vector3L(1L, 0L, 0L);

		public static Vector3L Left = new Vector3L(-1L, 0L, 0L);

		public static Vector3L Forward = new Vector3L(0L, 0L, -1L);

		public static Vector3L Backward = new Vector3L(0L, 0L, 1L);

		public static Vector3L One = new Vector3L(1L, 1L, 1L);

		[ProtoMember(1)]
		public long X;

		[ProtoMember(4)]
		public long Y;

		[ProtoMember(7)]
		public long Z;

		public long this[long index]
		{
			get
			{
				if ((ulong)index <= 2uL)
				{
					switch (index)
					{
					case 0L:
						return X;
					case 1L:
						return Y;
					case 2L:
						return Z;
					}
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if ((ulong)index <= 2uL)
				{
					switch (index)
					{
					case 0L:
						X = value;
						return;
					case 1L:
						Y = value;
						return;
					case 2L:
						Z = value;
						return;
					}
				}
				throw new IndexOutOfRangeException();
			}
		}

		/// <summary>
		/// How many cubes are in block with this size
		/// </summary>
		/// <returns></returns>
		public long Size => Math.Abs(X * Y * Z);

		public long SizeLong => Math.Abs(X * Y * Z);

		public Vector3L(long xyz)
		{
			X = xyz;
			Y = xyz;
			Z = xyz;
		}

		public Vector3L(long x, long y, long z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3L(Vector3 xyz)
		{
			X = (long)xyz.X;
			Y = (long)xyz.Y;
			Z = (long)xyz.Z;
		}

		public Vector3L(Vector3D xyz)
		{
			X = (long)xyz.X;
			Y = (long)xyz.Y;
			Z = (long)xyz.Z;
		}

		public Vector3L(Vector3S xyz)
		{
			X = xyz.X;
			Y = xyz.Y;
			Z = xyz.Z;
		}

		public Vector3L(float x, float y, float z)
		{
			X = (long)x;
			Y = (long)y;
			Z = (long)z;
		}

		public override string ToString()
		{
			return $"[X:{X}, Y:{Y}, Z:{Z}]";
		}

		public bool Equals(Vector3L other)
		{
			if (other.X == X && other.Y == Y)
			{
				return other.Z == Z;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(Vector3L))
			{
				return false;
			}
			return Equals((Vector3L)obj);
		}

		public override int GetHashCode()
		{
			return (int)((((X * 397) ^ Y) * 397) ^ Z);
		}

		public bool IsInsideInclusiveEnd(ref Vector3L min, ref Vector3L max)
		{
			if (min.X <= X && X <= max.X && min.Y <= Y && Y <= max.Y && min.Z <= Z)
			{
				return Z <= max.Z;
			}
			return false;
		}

		public bool IsInsideInclusiveEnd(Vector3L min, Vector3L max)
		{
			return IsInsideInclusiveEnd(ref min, ref max);
		}

		public bool IsInside(ref Vector3L inclusiveMin, ref Vector3L exclusiveMax)
		{
			if (inclusiveMin.X <= X && X < exclusiveMax.X && inclusiveMin.Y <= Y && Y < exclusiveMax.Y && inclusiveMin.Z <= Z)
			{
				return Z < exclusiveMax.Z;
			}
			return false;
		}

		public bool IsInside(Vector3L inclusiveMin, Vector3L exclusiveMax)
		{
			return IsInside(ref inclusiveMin, ref exclusiveMax);
		}

		/// <summary>
		/// Calculates rectangular distance.
		/// It's how many sectors you have to travel to get to other sector from current sector.
		/// </summary>
		public long RectangularDistance(Vector3L otherVector)
		{
			return Math.Abs(X - otherVector.X) + Math.Abs(Y - otherVector.Y) + Math.Abs(Z - otherVector.Z);
		}

		/// <summary>
		/// Calculates rectangular distance of this vector, longerpreted as a polong, from the origin.
		/// </summary>
		/// <returns></returns>
		public long RectangularLength()
		{
			return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
		}

		public long Length()
		{
			return (long)Math.Sqrt(Dot(this, this));
		}

		public static bool Boxlongersects(Vector3L minA, Vector3L maxA, Vector3L minB, Vector3L maxB)
		{
			return Boxlongersects(ref minA, ref maxA, ref minB, ref maxB);
		}

		public static bool Boxlongersects(ref Vector3L minA, ref Vector3L maxA, ref Vector3L minB, ref Vector3L maxB)
		{
			if (maxA.X >= minB.X && minA.X <= maxB.X && maxA.Y >= minB.Y && minA.Y <= maxB.Y)
			{
				if (maxA.Z >= minB.Z)
				{
					return minA.Z <= maxB.Z;
				}
				return false;
			}
			return false;
		}

		public static bool BoxContains(Vector3L boxMin, Vector3L boxMax, Vector3L pt)
		{
			if (boxMax.X >= pt.X && boxMin.X <= pt.X && boxMax.Y >= pt.Y && boxMin.Y <= pt.Y)
			{
				if (boxMax.Z >= pt.Z)
				{
					return boxMin.Z <= pt.Z;
				}
				return false;
			}
			return false;
		}

		public static bool BoxContains(ref Vector3L boxMin, ref Vector3L boxMax, ref Vector3L pt)
		{
			if (boxMax.X >= pt.X && boxMin.X <= pt.X && boxMax.Y >= pt.Y && boxMin.Y <= pt.Y)
			{
				if (boxMax.Z >= pt.Z)
				{
					return boxMin.Z <= pt.Z;
				}
				return false;
			}
			return false;
		}

		public static Vector3L operator *(Vector3L a, Vector3L b)
		{
			return new Vector3L(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}

		public static bool operator ==(Vector3L a, Vector3L b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3L a, Vector3L b)
		{
			return !(a == b);
		}

		public static Vector3 operator +(Vector3L a, float b)
		{
			return new Vector3((float)a.X + b, (float)a.Y + b, (float)a.Z + b);
		}

		public static Vector3 operator *(Vector3L a, Vector3 b)
		{
			return new Vector3((float)a.X * b.X, (float)a.Y * b.Y, (float)a.Z * b.Z);
		}

		public static Vector3 operator *(Vector3 a, Vector3L b)
		{
			return new Vector3(a.X * (float)b.X, a.Y * (float)b.Y, a.Z * (float)b.Z);
		}

		public static Vector3 operator *(float num, Vector3L b)
		{
			return new Vector3(num * (float)b.X, num * (float)b.Y, num * (float)b.Z);
		}

		public static Vector3 operator *(Vector3L a, float num)
		{
			return new Vector3(num * (float)a.X, num * (float)a.Y, num * (float)a.Z);
		}

		public static Vector3D operator *(double num, Vector3L b)
		{
			return new Vector3D(num * (double)b.X, num * (double)b.Y, num * (double)b.Z);
		}

		public static Vector3D operator *(Vector3L a, double num)
		{
			return new Vector3D(num * (double)a.X, num * (double)a.Y, num * (double)a.Z);
		}

		public static Vector3 operator /(Vector3L a, float num)
		{
			return new Vector3((float)a.X / num, (float)a.Y / num, (float)a.Z / num);
		}

		public static Vector3 operator /(float num, Vector3L a)
		{
			return new Vector3(num / (float)a.X, num / (float)a.Y, num / (float)a.Z);
		}

		public static Vector3L operator /(Vector3L a, long num)
		{
			return new Vector3L(a.X / num, a.Y / num, a.Z / num);
		}

		public static Vector3L operator /(Vector3L a, Vector3L b)
		{
			return new Vector3L(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
		}

		public static Vector3L operator %(Vector3L a, long num)
		{
			return new Vector3L(a.X % num, a.Y % num, a.Z % num);
		}

		public static Vector3L operator >>(Vector3L v, int shift)
		{
			return new Vector3L(v.X >> shift, v.Y >> shift, v.Z >> shift);
		}

		public static Vector3L operator <<(Vector3L v, int shift)
		{
			return new Vector3L(v.X << shift, v.Y << shift, v.Z << shift);
		}

		public static Vector3L operator &(Vector3L v, long mask)
		{
			return new Vector3L(v.X & mask, v.Y & mask, v.Z & mask);
		}

		public static Vector3L operator |(Vector3L v, long mask)
		{
			return new Vector3L(v.X | mask, v.Y | mask, v.Z | mask);
		}

		public static Vector3L operator ^(Vector3L v, long mask)
		{
			return new Vector3L(v.X ^ mask, v.Y ^ mask, v.Z ^ mask);
		}

		public static Vector3L operator ~(Vector3L v)
		{
			return new Vector3L(~v.X, ~v.Y, ~v.Z);
		}

		public static Vector3L operator *(long num, Vector3L b)
		{
			return new Vector3L(num * b.X, num * b.Y, num * b.Z);
		}

		public static Vector3L operator *(Vector3L a, long num)
		{
			return new Vector3L(num * a.X, num * a.Y, num * a.Z);
		}

		public static Vector3L operator +(Vector3L a, Vector3L b)
		{
			return new Vector3L(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3L operator +(Vector3L a, long b)
		{
			return new Vector3L(a.X + b, a.Y + b, a.Z + b);
		}

		public static Vector3L operator -(Vector3L a, Vector3L b)
		{
			return new Vector3L(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Vector3L operator -(Vector3L a, long b)
		{
			return new Vector3L(a.X - b, a.Y - b, a.Z - b);
		}

		public static Vector3L operator -(Vector3L a)
		{
			return new Vector3L(-a.X, -a.Y, -a.Z);
		}

		public static Vector3L Min(Vector3L value1, Vector3L value2)
		{
			Vector3L result = default(Vector3L);
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		public static void Min(ref Vector3L value1, ref Vector3L value2, out Vector3L result)
		{
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is smallest of all the three components.
		/// </summary>
		public long AbsMin()
		{
			if (Math.Abs(X) < Math.Abs(Y))
			{
				if (Math.Abs(X) < Math.Abs(Z))
				{
					return Math.Abs(X);
				}
				return Math.Abs(Z);
			}
			if (Math.Abs(Y) < Math.Abs(Z))
			{
				return Math.Abs(Y);
			}
			return Math.Abs(Z);
		}

		public static Vector3L Max(Vector3L value1, Vector3L value2)
		{
			Vector3L result = default(Vector3L);
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		public static void Max(ref Vector3L value1, ref Vector3L value2, out Vector3L result)
		{
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is largest of all the three components.
		/// </summary>
		public long AbsMax()
		{
			if (Math.Abs(X) > Math.Abs(Y))
			{
				if (Math.Abs(X) > Math.Abs(Z))
				{
					return Math.Abs(X);
				}
				return Math.Abs(Z);
			}
			if (Math.Abs(Y) > Math.Abs(Z))
			{
				return Math.Abs(Y);
			}
			return Math.Abs(Z);
		}

		public long AxisValue(Base6Directions.Axis axis)
		{
			switch (axis)
			{
			case Base6Directions.Axis.ForwardBackward:
				return Z;
			case Base6Directions.Axis.LeftRight:
				return X;
			default:
				return Y;
			}
		}

		public static CubeFace GetDominantDirection(Vector3L val)
		{
			if (Math.Abs(val.X) > Math.Abs(val.Y))
			{
				if (Math.Abs(val.X) > Math.Abs(val.Z))
				{
					if (val.X > 0)
					{
						return CubeFace.Right;
					}
					return CubeFace.Left;
				}
				if (val.Z > 0)
				{
					return CubeFace.Backward;
				}
				return CubeFace.Forward;
			}
			if (Math.Abs(val.Y) > Math.Abs(val.Z))
			{
				if (val.Y > 0)
				{
					return CubeFace.Up;
				}
				return CubeFace.Down;
			}
			if (val.Z > 0)
			{
				return CubeFace.Backward;
			}
			return CubeFace.Forward;
		}

		public static Vector3L GetDominantDirectionVector(Vector3L val)
		{
			if (Math.Abs(val.X) > Math.Abs(val.Y))
			{
				val.Y = 0L;
				if (Math.Abs(val.X) > Math.Abs(val.Z))
				{
					val.Z = 0L;
					if (val.X > 0)
					{
						val.X = 1L;
					}
					else
					{
						val.X = -1L;
					}
				}
				else
				{
					val.X = 0L;
					if (val.Z > 0)
					{
						val.Z = 1L;
					}
					else
					{
						val.Z = -1L;
					}
				}
			}
			else
			{
				val.X = 0L;
				if (Math.Abs(val.Y) > Math.Abs(val.Z))
				{
					val.Z = 0L;
					if (val.Y > 0)
					{
						val.Y = 1L;
					}
					else
					{
						val.Y = -1L;
					}
				}
				else
				{
					val.Y = 0L;
					if (val.Z > 0)
					{
						val.Z = 1L;
					}
					else
					{
						val.Z = -1L;
					}
				}
			}
			return val;
		}

		/// <summary>
		/// Returns a vector that is equal to the projection of the input vector to the coordinate axis that corresponds
		/// to the original vector's largest value.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		public static Vector3L DominantAxisProjection(Vector3L value1)
		{
			if (Math.Abs(value1.X) > Math.Abs(value1.Y))
			{
				value1.Y = 0L;
				if (Math.Abs(value1.X) > Math.Abs(value1.Z))
				{
					value1.Z = 0L;
				}
				else
				{
					value1.X = 0L;
				}
			}
			else
			{
				value1.X = 0L;
				if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
				{
					value1.Z = 0L;
				}
				else
				{
					value1.Y = 0L;
				}
			}
			return value1;
		}

		/// <summary>
		/// Calculates a vector that is equal to the projection of the input vector to the coordinate axis that corresponds
		/// to the original vector's largest value. The result is saved longo a user-specified variable.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="result">[OutAttribute] The projected vector.</param>
		public static void DominantAxisProjection(ref Vector3L value1, out Vector3L result)
		{
			if (Math.Abs(value1.X) > Math.Abs(value1.Y))
			{
				if (Math.Abs(value1.X) > Math.Abs(value1.Z))
				{
					result = new Vector3L(value1.X, 0L, 0L);
				}
				else
				{
					result = new Vector3L(0L, 0L, value1.Z);
				}
			}
			else if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
			{
				result = new Vector3L(0L, value1.Y, 0L);
			}
			else
			{
				result = new Vector3L(0L, 0L, value1.Z);
			}
		}

		public static Vector3L Sign(Vector3 value)
		{
			return new Vector3L(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));
		}

		public static Vector3L Sign(Vector3L value)
		{
			return new Vector3L(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));
		}

		public static Vector3L Round(Vector3 value)
		{
			Round(ref value, out Vector3L r);
			return r;
		}

		public static Vector3L Round(Vector3D value)
		{
			Round(ref value, out Vector3L r);
			return r;
		}

		public static void Round(ref Vector3 v, out Vector3L r)
		{
			r.X = (long)Math.Round(v.X, MidpointRounding.AwayFromZero);
			r.Y = (long)Math.Round(v.Y, MidpointRounding.AwayFromZero);
			r.Z = (long)Math.Round(v.Z, MidpointRounding.AwayFromZero);
		}

		public static void Round(ref Vector3D v, out Vector3L r)
		{
			r.X = (long)Math.Round(v.X, MidpointRounding.AwayFromZero);
			r.Y = (long)Math.Round(v.Y, MidpointRounding.AwayFromZero);
			r.Z = (long)Math.Round(v.Z, MidpointRounding.AwayFromZero);
		}

		public static Vector3L Floor(Vector3 value)
		{
			return new Vector3L((long)Math.Floor(value.X), (long)Math.Floor(value.Y), (long)Math.Floor(value.Z));
		}

		public static Vector3L Floor(Vector3D value)
		{
			return new Vector3L((long)Math.Floor(value.X), (long)Math.Floor(value.Y), (long)Math.Floor(value.Z));
		}

		public static void Floor(ref Vector3 v, out Vector3L r)
		{
			r.X = (long)Math.Floor(v.X);
			r.Y = (long)Math.Floor(v.Y);
			r.Z = (long)Math.Floor(v.Z);
		}

		public static void Floor(ref Vector3D v, out Vector3L r)
		{
			r.X = (long)Math.Floor(v.X);
			r.Y = (long)Math.Floor(v.Y);
			r.Z = (long)Math.Floor(v.Z);
		}

		public static Vector3L Ceiling(Vector3 value)
		{
			return new Vector3L((long)Math.Ceiling(value.X), (long)Math.Ceiling(value.Y), (long)Math.Ceiling(value.Z));
		}

		public static Vector3L Trunc(Vector3 value)
		{
			return new Vector3L((long)value.X, (long)value.Y, (long)value.Z);
		}

		public static Vector3L Shift(Vector3L value)
		{
			return new Vector3L(value.Z, value.X, value.Y);
		}

		public static implicit operator Vector3(Vector3L value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		public static implicit operator Vector3D(Vector3L value)
		{
			return new Vector3D(value.X, value.Y, value.Z);
		}

		public static explicit operator Vector3I(Vector3L value)
		{
			return new Vector3I((int)value.X, (int)value.Y, (int)value.Z);
		}

		/// <summary>
		/// Transforms a Vector3L by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3L.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The transformed vector.</param>
		public static void Transform(ref Vector3L position, ref Matrix matrix, out Vector3L result)
		{
			long x = position.X * (long)Math.Round(matrix.M11) + position.Y * (long)Math.Round(matrix.M21) + position.Z * (long)Math.Round(matrix.M31) + (long)Math.Round(matrix.M41);
			long y = position.X * (long)Math.Round(matrix.M12) + position.Y * (long)Math.Round(matrix.M22) + position.Z * (long)Math.Round(matrix.M32) + (long)Math.Round(matrix.M42);
			long z = position.X * (long)Math.Round(matrix.M13) + position.Y * (long)Math.Round(matrix.M23) + position.Z * (long)Math.Round(matrix.M33) + (long)Math.Round(matrix.M43);
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static void Transform(ref Vector3L value, ref Quaternion rotation, out Vector3L result)
		{
			float num = rotation.X + rotation.X;
			float num2 = rotation.Y + rotation.Y;
			float num3 = rotation.Z + rotation.Z;
			float num4 = rotation.W * num;
			float num5 = rotation.W * num2;
			float num6 = rotation.W * num3;
			float num7 = rotation.X * num;
			float num8 = rotation.X * num2;
			float num9 = rotation.X * num3;
			float num10 = rotation.Y * num2;
			float num11 = rotation.Y * num3;
			float num12 = rotation.Z * num3;
			float num13 = (float)((double)value.X * (1.0 - (double)num10 - (double)num12) + (double)value.Y * ((double)num8 - (double)num6) + (double)value.Z * ((double)num9 + (double)num5));
			float num14 = (float)((double)value.X * ((double)num8 + (double)num6) + (double)value.Y * (1.0 - (double)num7 - (double)num12) + (double)value.Z * ((double)num11 - (double)num4));
			float num15 = (float)((double)value.X * ((double)num9 - (double)num5) + (double)value.Y * ((double)num11 + (double)num4) + (double)value.Z * (1.0 - (double)num7 - (double)num10));
			result.X = (long)Math.Round(num13);
			result.Y = (long)Math.Round(num14);
			result.Z = (long)Math.Round(num15);
		}

		public static Vector3L Transform(Vector3L value, Quaternion rotation)
		{
			Transform(ref value, ref rotation, out Vector3L result);
			return result;
		}

		/// <summary>
		/// Transforms a vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector3 resulting from the transformation.</param>
		public static void TransformNormal(ref Vector3L normal, ref Matrix matrix, out Vector3L result)
		{
			long x = normal.X * (long)Math.Round(matrix.M11) + normal.Y * (long)Math.Round(matrix.M21) + normal.Z * (long)Math.Round(matrix.M31);
			long y = normal.X * (long)Math.Round(matrix.M12) + normal.Y * (long)Math.Round(matrix.M22) + normal.Z * (long)Math.Round(matrix.M32);
			long z = normal.X * (long)Math.Round(matrix.M13) + normal.Y * (long)Math.Round(matrix.M23) + normal.Z * (long)Math.Round(matrix.M33);
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The cross product of the vectors.</param>
		public static void Cross(ref Vector3L vector1, ref Vector3L vector2, out Vector3L result)
		{
			long x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
			long y = vector1.Z * vector2.X - vector1.X * vector2.Z;
			long z = vector1.X * vector2.Y - vector1.Y * vector2.X;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public int CompareTo(Vector3L other)
		{
			long num = X - other.X;
			long num2 = Y - other.Y;
			long value = Z - other.Z;
			if (num == 0L)
			{
				if (num2 == 0L)
				{
					return Math.Sign(value);
				}
				return Math.Sign(num2);
			}
			return Math.Sign(num);
		}

		public static Vector3L Abs(Vector3L value)
		{
			return new Vector3L(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
		}

		public static void Abs(ref Vector3L value, out Vector3L result)
		{
			result.X = Math.Abs(value.X);
			result.Y = Math.Abs(value.Y);
			result.Z = Math.Abs(value.Z);
		}

		public static Vector3L Clamp(Vector3L value1, Vector3L min, Vector3L max)
		{
			Clamp(ref value1, ref min, ref max, out Vector3L result);
			return result;
		}

		public static void Clamp(ref Vector3L value1, ref Vector3L min, ref Vector3L max, out Vector3L result)
		{
			long x = value1.X;
			long num = (x > max.X) ? max.X : x;
			result.X = ((num < min.X) ? min.X : num);
			long y = value1.Y;
			long num2 = (y > max.Y) ? max.Y : y;
			result.Y = ((num2 < min.Y) ? min.Y : num2);
			long z = value1.Z;
			long num3 = (z > max.Z) ? max.Z : z;
			result.Z = ((num3 < min.Z) ? min.Z : num3);
		}

		/// <summary>
		/// Manhattan distance (cube distance)
		/// X + Y + Z of Abs(first - second)
		/// </summary>
		public static long DistanceManhattan(Vector3L first, Vector3L second)
		{
			Vector3L vector3L = Abs(first - second);
			return vector3L.X + vector3L.Y + vector3L.Z;
		}

		public long Dot(ref Vector3L v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public static long Dot(Vector3L vector1, Vector3L vector2)
		{
			return Dot(ref vector1, ref vector2);
		}

		public static long Dot(ref Vector3L vector1, ref Vector3L vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static void Dot(ref Vector3L vector1, ref Vector3L vector2, out long dot)
		{
			dot = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static bool TryParseFromString(string p, out Vector3L vec)
		{
			string[] array = p.Split(new char[1]
			{
				';'
			});
			if (array.Length != 3)
			{
				vec = Zero;
				return false;
			}
			try
			{
				vec.X = long.Parse(array[0]);
				vec.Y = long.Parse(array[1]);
				vec.Z = long.Parse(array[2]);
			}
			catch (FormatException)
			{
				vec = Zero;
				return false;
			}
			return true;
		}

		public long Volume()
		{
			return X * Y * Z;
		}

		/// <summary>
		/// Enumerate all values in a longeger longerval (a cuboid).
		///
		/// This method is an allocating version of the Vector3L_RangeIterator.
		/// This once can be used in the foreach syntax though so it's more convenient for debug routines.
		/// </summary>
		/// <param name="minInclusive">Minimum range (inclusive)</param>
		/// <param name="maxExclusive">Maximum range (exclusive)</param>
		/// <returns>An iterator for that range.</returns>
		public static IEnumerable<Vector3L> EnumerateRange(Vector3L minInclusive, Vector3L maxExclusive)
		{
			Vector3L vec = default(Vector3L);
			vec.Z = minInclusive.Z;
			while (vec.Z < maxExclusive.Z)
			{
				vec.Y = minInclusive.Y;
				while (vec.Y < maxExclusive.Y)
				{
					vec.X = minInclusive.X;
					while (vec.X < maxExclusive.X)
					{
						yield return vec;
						vec.X++;
					}
					vec.Y++;
				}
				vec.Z++;
			}
		}

		public void ToBytes(List<byte> result)
		{
			result.AddRange(BitConverter.GetBytes(X));
			result.AddRange(BitConverter.GetBytes(Y));
			result.AddRange(BitConverter.GetBytes(Z));
		}
	}
}
