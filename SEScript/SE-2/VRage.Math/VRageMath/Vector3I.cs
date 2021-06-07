using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	[Serializable]
	[ProtoContract]
	public struct Vector3I : IEquatable<Vector3I>, IComparable<Vector3I>
	{
		public class EqualityComparer : IEqualityComparer<Vector3I>, IComparer<Vector3I>
		{
			public bool Equals(Vector3I x, Vector3I y)
			{
				return (x.X == y.X) & (x.Y == y.Y) & (x.Z == y.Z);
			}

			public int GetHashCode(Vector3I obj)
			{
				return (((obj.X * 397) ^ obj.Y) * 397) ^ obj.Z;
			}

			public int Compare(Vector3I x, Vector3I y)
			{
				return x.CompareTo(y);
			}
		}

		protected class VRageMath_Vector3I_003C_003EX_003C_003EAccessor : IMemberAccessor<Vector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3I owner, in int value)
			{
				owner.X = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3I owner, out int value)
			{
				value = owner.X;
			}
		}

		protected class VRageMath_Vector3I_003C_003EY_003C_003EAccessor : IMemberAccessor<Vector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3I owner, in int value)
			{
				owner.Y = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3I owner, out int value)
			{
				value = owner.Y;
			}
		}

		protected class VRageMath_Vector3I_003C_003EZ_003C_003EAccessor : IMemberAccessor<Vector3I, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Vector3I owner, in int value)
			{
				owner.Z = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Vector3I owner, out int value)
			{
				value = owner.Z;
			}
		}

		public static readonly EqualityComparer Comparer = new EqualityComparer();

		public static Vector3I UnitX = new Vector3I(1, 0, 0);

		public static Vector3I UnitY = new Vector3I(0, 1, 0);

		public static Vector3I UnitZ = new Vector3I(0, 0, 1);

		public static Vector3I Zero = new Vector3I(0, 0, 0);

		public static Vector3I MaxValue = new Vector3I(int.MaxValue, int.MaxValue, int.MaxValue);

		public static Vector3I MinValue = new Vector3I(int.MinValue, int.MinValue, int.MinValue);

		public static Vector3I Up = new Vector3I(0, 1, 0);

		public static Vector3I Down = new Vector3I(0, -1, 0);

		public static Vector3I Right = new Vector3I(1, 0, 0);

		public static Vector3I Left = new Vector3I(-1, 0, 0);

		public static Vector3I Forward = new Vector3I(0, 0, -1);

		public static Vector3I Backward = new Vector3I(0, 0, 1);

		[ProtoMember(1)]
		public int X;

		[ProtoMember(4)]
		public int Y;

		[ProtoMember(7)]
		public int Z;

		public static Vector3I One = new Vector3I(1, 1, 1);

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
				default:
					throw new IndexOutOfRangeException();
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
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		public bool IsPowerOfTwo
		{
			get
			{
				if (MathHelper.IsPowerOfTwo(X) && MathHelper.IsPowerOfTwo(Y))
				{
					return MathHelper.IsPowerOfTwo(Z);
				}
				return false;
			}
		}

		/// <summary>
		/// How many cubes are in block with this size
		/// </summary>
		/// <returns></returns>
		public int Size => Math.Abs(X * Y * Z);

		public long SizeLong => Math.Abs((long)X * (long)Y * Z);

		public Vector3I(int xyz)
		{
			X = xyz;
			Y = xyz;
			Z = xyz;
		}

		public Vector3I(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3I(Vector2I xy, int z)
		{
			X = xy.X;
			Y = xy.Y;
			Z = z;
		}

		public Vector3I(Vector3 xyz)
		{
			X = (int)xyz.X;
			Y = (int)xyz.Y;
			Z = (int)xyz.Z;
		}

		public Vector3I(Vector3D xyz)
		{
			X = (int)xyz.X;
			Y = (int)xyz.Y;
			Z = (int)xyz.Z;
		}

		public Vector3I(Vector3S xyz)
		{
			X = xyz.X;
			Y = xyz.Y;
			Z = xyz.Z;
		}

		public Vector3I(float x, float y, float z)
		{
			X = (int)x;
			Y = (int)y;
			Z = (int)z;
		}

		public Vector3I(byte[] bytes, int index)
		{
			X = BitConverter.ToInt32(bytes, index);
			Y = BitConverter.ToInt32(bytes, index + 4);
			Z = BitConverter.ToInt32(bytes, index + 8);
		}

		public static explicit operator Vector3I(Vector3 value)
		{
			return new Vector3I((int)value.X, (int)value.Y, (int)value.Z);
		}

		public override string ToString()
		{
			return $"[X:{X}, Y:{Y}, Z:{Z}]";
		}

		public bool Equals(Vector3I other)
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
			if (obj.GetType() != typeof(Vector3I))
			{
				return false;
			}
			return Equals((Vector3I)obj);
		}

		public override int GetHashCode()
		{
			return (((X * 397) ^ Y) * 397) ^ Z;
		}

		public bool IsInsideInclusiveEnd(ref Vector3I min, ref Vector3I max)
		{
			if (min.X <= X && X <= max.X && min.Y <= Y && Y <= max.Y && min.Z <= Z)
			{
				return Z <= max.Z;
			}
			return false;
		}

		public bool IsInsideInclusiveEnd(Vector3I min, Vector3I max)
		{
			return IsInsideInclusiveEnd(ref min, ref max);
		}

		public bool IsInside(ref Vector3I inclusiveMin, ref Vector3I exclusiveMax)
		{
			if (inclusiveMin.X <= X && X < exclusiveMax.X && inclusiveMin.Y <= Y && Y < exclusiveMax.Y && inclusiveMin.Z <= Z)
			{
				return Z < exclusiveMax.Z;
			}
			return false;
		}

		public bool IsInside(Vector3I inclusiveMin, Vector3I exclusiveMax)
		{
			return IsInside(ref inclusiveMin, ref exclusiveMax);
		}

		/// <summary>
		/// Calculates rectangular distance.
		/// It's how many sectors you have to travel to get to other sector from current sector.
		/// </summary>
		public int RectangularDistance(Vector3I otherVector)
		{
			return Math.Abs(X - otherVector.X) + Math.Abs(Y - otherVector.Y) + Math.Abs(Z - otherVector.Z);
		}

		/// <summary>
		/// Calculates rectangular distance of this vector, interpreted as a point, from the origin.
		/// </summary>
		/// <returns></returns>
		public int RectangularLength()
		{
			return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
		}

		public int Length()
		{
			return (int)Math.Sqrt(Dot(this, this));
		}

		public static bool BoxIntersects(Vector3I minA, Vector3I maxA, Vector3I minB, Vector3I maxB)
		{
			return BoxIntersects(ref minA, ref maxA, ref minB, ref maxB);
		}

		public static bool BoxIntersects(ref Vector3I minA, ref Vector3I maxA, ref Vector3I minB, ref Vector3I maxB)
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

		public static bool BoxContains(Vector3I boxMin, Vector3I boxMax, Vector3I pt)
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

		public static bool BoxContains(ref Vector3I boxMin, ref Vector3I boxMax, ref Vector3I pt)
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

		public static Vector3I operator *(Vector3I a, Vector3I b)
		{
			return new Vector3I(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}

		public static bool operator ==(Vector3I a, Vector3I b)
		{
			if (a.X == b.X && a.Y == b.Y)
			{
				return a.Z == b.Z;
			}
			return false;
		}

		public static bool operator !=(Vector3I a, Vector3I b)
		{
			return !(a == b);
		}

		public static Vector3 operator +(Vector3I a, float b)
		{
			return new Vector3((float)a.X + b, (float)a.Y + b, (float)a.Z + b);
		}

		public static Vector3 operator *(Vector3I a, Vector3 b)
		{
			return new Vector3((float)a.X * b.X, (float)a.Y * b.Y, (float)a.Z * b.Z);
		}

		public static Vector3 operator *(Vector3 a, Vector3I b)
		{
			return new Vector3(a.X * (float)b.X, a.Y * (float)b.Y, a.Z * (float)b.Z);
		}

		public static Vector3D operator *(Vector3I a, Vector3D b)
		{
			return new Vector3D((double)a.X * b.X, (double)a.Y * b.Y, (double)a.Z * b.Z);
		}

		public static Vector3D operator *(Vector3D a, Vector3I b)
		{
			return new Vector3D(a.X * (double)b.X, a.Y * (double)b.Y, a.Z * (double)b.Z);
		}

		public static Vector3 operator *(float num, Vector3I b)
		{
			return new Vector3(num * (float)b.X, num * (float)b.Y, num * (float)b.Z);
		}

		public static Vector3 operator *(Vector3I a, float num)
		{
			return new Vector3(num * (float)a.X, num * (float)a.Y, num * (float)a.Z);
		}

		public static Vector3D operator *(double num, Vector3I b)
		{
			return new Vector3D(num * (double)b.X, num * (double)b.Y, num * (double)b.Z);
		}

		public static Vector3D operator *(Vector3I a, double num)
		{
			return new Vector3D(num * (double)a.X, num * (double)a.Y, num * (double)a.Z);
		}

		public static Vector3 operator /(Vector3I a, float num)
		{
			return new Vector3((float)a.X / num, (float)a.Y / num, (float)a.Z / num);
		}

		public static Vector3 operator /(float num, Vector3I a)
		{
			return new Vector3(num / (float)a.X, num / (float)a.Y, num / (float)a.Z);
		}

		public static Vector3I operator /(Vector3I a, int num)
		{
			return new Vector3I(a.X / num, a.Y / num, a.Z / num);
		}

		public static Vector3I operator /(Vector3I a, Vector3I b)
		{
			return new Vector3I(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
		}

		public static Vector3I operator %(Vector3I a, int num)
		{
			return new Vector3I(a.X % num, a.Y % num, a.Z % num);
		}

		public static Vector3I operator >>(Vector3I v, int shift)
		{
			return new Vector3I(v.X >> shift, v.Y >> shift, v.Z >> shift);
		}

		public static Vector3I operator <<(Vector3I v, int shift)
		{
			return new Vector3I(v.X << shift, v.Y << shift, v.Z << shift);
		}

		public static Vector3I operator &(Vector3I v, int mask)
		{
			return new Vector3I(v.X & mask, v.Y & mask, v.Z & mask);
		}

		public static Vector3I operator |(Vector3I v, int mask)
		{
			return new Vector3I(v.X | mask, v.Y | mask, v.Z | mask);
		}

		public static Vector3I operator ^(Vector3I v, int mask)
		{
			return new Vector3I(v.X ^ mask, v.Y ^ mask, v.Z ^ mask);
		}

		public static Vector3I operator ~(Vector3I v)
		{
			return new Vector3I(~v.X, ~v.Y, ~v.Z);
		}

		public static Vector3I operator *(int num, Vector3I b)
		{
			return new Vector3I(num * b.X, num * b.Y, num * b.Z);
		}

		public static Vector3I operator *(Vector3I a, int num)
		{
			return new Vector3I(num * a.X, num * a.Y, num * a.Z);
		}

		public static Vector3I operator +(Vector3I a, Vector3I b)
		{
			return new Vector3I(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3I operator +(Vector3I a, int b)
		{
			return new Vector3I(a.X + b, a.Y + b, a.Z + b);
		}

		public static Vector3I operator -(Vector3I a, Vector3I b)
		{
			return new Vector3I(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Vector3I operator -(Vector3I a, int b)
		{
			return new Vector3I(a.X - b, a.Y - b, a.Z - b);
		}

		public static Vector3I operator -(Vector3I a)
		{
			return new Vector3I(-a.X, -a.Y, -a.Z);
		}

		public static Vector3I Min(Vector3I value1, Vector3I value2)
		{
			Vector3I result = default(Vector3I);
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		public static void Min(ref Vector3I value1, ref Vector3I value2, out Vector3I result)
		{
			result.X = ((value1.X < value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is smallest of all the three components.
		/// </summary>
		public int AbsMin()
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

		public static Vector3I Max(Vector3I value1, Vector3I value2)
		{
			Vector3I result = default(Vector3I);
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
			return result;
		}

		public static void Max(ref Vector3I value1, ref Vector3I value2, out Vector3I result)
		{
			result.X = ((value1.X > value2.X) ? value1.X : value2.X);
			result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
			result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
		}

		/// <summary>
		/// Returns the component of the vector, whose absolute value is largest of all the three components.
		/// </summary>
		public int AbsMax()
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

		/// <summary>
		/// Separates minimal and maximal values of any two input vectors
		/// </summary>
		/// <param name="min">minimal values of the two vectors</param>
		/// <param name="max">maximal values of the two vectors</param>
		public static void MinMax(ref Vector3I min, ref Vector3I max)
		{
			if (min.X > max.X)
			{
				int x = min.X;
				min.X = max.X;
				max.X = x;
			}
			if (min.Y > max.Y)
			{
				int x = min.Y;
				min.Y = max.Y;
				max.Y = x;
			}
			if (min.Z > max.Z)
			{
				int x = min.Z;
				min.Z = max.Z;
				max.Z = x;
			}
		}

		public int AxisValue(Base6Directions.Axis axis)
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

		public static CubeFace GetDominantDirection(Vector3I val)
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

		public static Vector3I GetDominantDirectionVector(Vector3I val)
		{
			if (Math.Abs(val.X) > Math.Abs(val.Y))
			{
				val.Y = 0;
				if (Math.Abs(val.X) > Math.Abs(val.Z))
				{
					val.Z = 0;
					if (val.X > 0)
					{
						val.X = 1;
					}
					else
					{
						val.X = -1;
					}
				}
				else
				{
					val.X = 0;
					if (val.Z > 0)
					{
						val.Z = 1;
					}
					else
					{
						val.Z = -1;
					}
				}
			}
			else
			{
				val.X = 0;
				if (Math.Abs(val.Y) > Math.Abs(val.Z))
				{
					val.Z = 0;
					if (val.Y > 0)
					{
						val.Y = 1;
					}
					else
					{
						val.Y = -1;
					}
				}
				else
				{
					val.Y = 0;
					if (val.Z > 0)
					{
						val.Z = 1;
					}
					else
					{
						val.Z = -1;
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
		public static Vector3I DominantAxisProjection(Vector3I value1)
		{
			if (Math.Abs(value1.X) > Math.Abs(value1.Y))
			{
				value1.Y = 0;
				if (Math.Abs(value1.X) > Math.Abs(value1.Z))
				{
					value1.Z = 0;
				}
				else
				{
					value1.X = 0;
				}
			}
			else
			{
				value1.X = 0;
				if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
				{
					value1.Z = 0;
				}
				else
				{
					value1.Y = 0;
				}
			}
			return value1;
		}

		/// <summary>
		/// Calculates a vector that is equal to the projection of the input vector to the coordinate axis that corresponds
		/// to the original vector's largest value. The result is saved into a user-specified variable.
		/// </summary>
		/// <param name="value1">Source vector.</param><param name="result">[OutAttribute] The projected vector.</param>
		public static void DominantAxisProjection(ref Vector3I value1, out Vector3I result)
		{
			if (Math.Abs(value1.X) > Math.Abs(value1.Y))
			{
				if (Math.Abs(value1.X) > Math.Abs(value1.Z))
				{
					result = new Vector3I(value1.X, 0, 0);
				}
				else
				{
					result = new Vector3I(0, 0, value1.Z);
				}
			}
			else if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
			{
				result = new Vector3I(0, value1.Y, 0);
			}
			else
			{
				result = new Vector3I(0, 0, value1.Z);
			}
		}

		public static Vector3I Sign(Vector3 value)
		{
			return new Vector3I(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));
		}

		public static Vector3I Sign(Vector3I value)
		{
			return new Vector3I(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));
		}

		public static Vector3I Round(Vector3 value)
		{
			Round(ref value, out Vector3I r);
			return r;
		}

		public static Vector3I Round(Vector3D value)
		{
			Round(ref value, out Vector3I r);
			return r;
		}

		public static void Round(ref Vector3 v, out Vector3I r)
		{
			r.X = (int)Math.Round(v.X, MidpointRounding.AwayFromZero);
			r.Y = (int)Math.Round(v.Y, MidpointRounding.AwayFromZero);
			r.Z = (int)Math.Round(v.Z, MidpointRounding.AwayFromZero);
		}

		public static void Round(ref Vector3D v, out Vector3I r)
		{
			r.X = (int)Math.Round(v.X, MidpointRounding.AwayFromZero);
			r.Y = (int)Math.Round(v.Y, MidpointRounding.AwayFromZero);
			r.Z = (int)Math.Round(v.Z, MidpointRounding.AwayFromZero);
		}

		public static Vector3I Floor(Vector3 value)
		{
			return new Vector3I((int)Math.Floor(value.X), (int)Math.Floor(value.Y), (int)Math.Floor(value.Z));
		}

		public static Vector3I Floor(Vector3D value)
		{
			return new Vector3I((int)Math.Floor(value.X), (int)Math.Floor(value.Y), (int)Math.Floor(value.Z));
		}

		public static void Floor(ref Vector3 v, out Vector3I r)
		{
			r.X = (int)Math.Floor(v.X);
			r.Y = (int)Math.Floor(v.Y);
			r.Z = (int)Math.Floor(v.Z);
		}

		public static void Floor(ref Vector3D v, out Vector3I r)
		{
			r.X = (int)Math.Floor(v.X);
			r.Y = (int)Math.Floor(v.Y);
			r.Z = (int)Math.Floor(v.Z);
		}

		public static Vector3I Ceiling(Vector3 value)
		{
			return new Vector3I((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y), (int)Math.Ceiling(value.Z));
		}

		public static Vector3I Trunc(Vector3 value)
		{
			return new Vector3I((int)value.X, (int)value.Y, (int)value.Z);
		}

		public static Vector3I Shift(Vector3I value)
		{
			return new Vector3I(value.Z, value.X, value.Y);
		}

		public static implicit operator Vector3(Vector3I value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		public static implicit operator Vector3D(Vector3I value)
		{
			return new Vector3D(value.X, value.Y, value.Z);
		}

		public static implicit operator Vector3L(Vector3I value)
		{
			return new Vector3L(value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Transforms a Vector3I by the given Matrix.
		/// </summary>
		/// <param name="position">The source Vector3I.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The transformed vector.</param>
		public static void Transform(ref Vector3I position, ref Matrix matrix, out Vector3I result)
		{
			int x = position.X * (int)Math.Round(matrix.M11) + position.Y * (int)Math.Round(matrix.M21) + position.Z * (int)Math.Round(matrix.M31) + (int)Math.Round(matrix.M41);
			int y = position.X * (int)Math.Round(matrix.M12) + position.Y * (int)Math.Round(matrix.M22) + position.Z * (int)Math.Round(matrix.M32) + (int)Math.Round(matrix.M42);
			int z = position.X * (int)Math.Round(matrix.M13) + position.Y * (int)Math.Round(matrix.M23) + position.Z * (int)Math.Round(matrix.M33) + (int)Math.Round(matrix.M43);
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static void Transform(ref Vector3I value, ref Quaternion rotation, out Vector3I result)
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
			result.X = (int)Math.Round(num13);
			result.Y = (int)Math.Round(num14);
			result.Z = (int)Math.Round(num15);
		}

		public static Vector3I Transform(Vector3I value, Quaternion rotation)
		{
			Transform(ref value, ref rotation, out Vector3I result);
			return result;
		}

		public static void Transform(ref Vector3I value, ref MatrixI matrix, out Vector3I result)
		{
			result = value.X * Base6Directions.GetIntVector(matrix.Right) + value.Y * Base6Directions.GetIntVector(matrix.Up) + value.Z * Base6Directions.GetIntVector(matrix.Backward) + matrix.Translation;
		}

		public static Vector3I Transform(Vector3I value, MatrixI transformation)
		{
			Transform(ref value, ref transformation, out Vector3I result);
			return result;
		}

		public static Vector3I Transform(Vector3I value, ref MatrixI transformation)
		{
			Transform(ref value, ref transformation, out Vector3I result);
			return result;
		}

		public static Vector3I TransformNormal(Vector3I value, ref MatrixI transformation)
		{
			TransformNormal(ref value, ref transformation, out Vector3I result);
			return result;
		}

		/// <summary>
		/// Transforms a vector normal by a matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param><param name="matrix">The transformation Matrix.</param><param name="result">[OutAttribute] The Vector3 resulting from the transformation.</param>
		public static void TransformNormal(ref Vector3I normal, ref Matrix matrix, out Vector3I result)
		{
			int x = normal.X * (int)Math.Round(matrix.M11) + normal.Y * (int)Math.Round(matrix.M21) + normal.Z * (int)Math.Round(matrix.M31);
			int y = normal.X * (int)Math.Round(matrix.M12) + normal.Y * (int)Math.Round(matrix.M22) + normal.Z * (int)Math.Round(matrix.M32);
			int z = normal.X * (int)Math.Round(matrix.M13) + normal.Y * (int)Math.Round(matrix.M23) + normal.Z * (int)Math.Round(matrix.M33);
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public static void TransformNormal(ref Vector3I normal, ref MatrixI matrix, out Vector3I result)
		{
			result = normal.X * Base6Directions.GetIntVector(matrix.Right) + normal.Y * Base6Directions.GetIntVector(matrix.Up) + normal.Z * Base6Directions.GetIntVector(matrix.Backward);
		}

		/// <summary>
		/// Calculates the cross product of two vectors.
		/// </summary>
		/// <param name="vector1">Source vector.</param><param name="vector2">Source vector.</param><param name="result">[OutAttribute] The cross product of the vectors.</param>
		public static void Cross(ref Vector3I vector1, ref Vector3I vector2, out Vector3I result)
		{
			int x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
			int y = vector1.Z * vector2.X - vector1.X * vector2.Z;
			int z = vector1.X * vector2.Y - vector1.Y * vector2.X;
			result.X = x;
			result.Y = y;
			result.Z = z;
		}

		public int CompareTo(Vector3I other)
		{
			int num = X - other.X;
			int num2 = Y - other.Y;
			int result = Z - other.Z;
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

		public static Vector3I Abs(Vector3I value)
		{
			return new Vector3I(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
		}

		public static void Abs(ref Vector3I value, out Vector3I result)
		{
			result.X = Math.Abs(value.X);
			result.Y = Math.Abs(value.Y);
			result.Z = Math.Abs(value.Z);
		}

		public static Vector3I Clamp(Vector3I value1, Vector3I min, Vector3I max)
		{
			Clamp(ref value1, ref min, ref max, out Vector3I result);
			return result;
		}

		public static void Clamp(ref Vector3I value1, ref Vector3I min, ref Vector3I max, out Vector3I result)
		{
			int x = value1.X;
			int num = (x > max.X) ? max.X : x;
			result.X = ((num < min.X) ? min.X : num);
			int y = value1.Y;
			int num2 = (y > max.Y) ? max.Y : y;
			result.Y = ((num2 < min.Y) ? min.Y : num2);
			int z = value1.Z;
			int num3 = (z > max.Z) ? max.Z : z;
			result.Z = ((num3 < min.Z) ? min.Z : num3);
		}

		/// <summary>
		/// Manhattan distance (cube distance)
		/// X + Y + Z of Abs(first - second)
		/// </summary>
		public static int DistanceManhattan(Vector3I first, Vector3I second)
		{
			Vector3I vector3I = Abs(first - second);
			return vector3I.X + vector3I.Y + vector3I.Z;
		}

		public int Dot(ref Vector3I v)
		{
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public static int Dot(Vector3I vector1, Vector3I vector2)
		{
			return Dot(ref vector1, ref vector2);
		}

		public static int Dot(ref Vector3I vector1, ref Vector3I vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static void Dot(ref Vector3I vector1, ref Vector3I vector2, out int dot)
		{
			dot = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static bool TryParseFromString(string p, out Vector3I vec)
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
				vec.X = int.Parse(array[0]);
				vec.Y = int.Parse(array[1]);
				vec.Z = int.Parse(array[2]);
			}
			catch (FormatException)
			{
				vec = Zero;
				return false;
			}
			return true;
		}

		public int Volume()
		{
			return X * Y * Z;
		}

		/// <summary>
		/// Enumerate all values in a integer interval (a cuboid).
		///
		/// This method is an allocating version of the Vector3I_RangeIterator.
		/// This once can be used in the foreach syntax though so it's more convenient for debug routines.
		/// </summary>
		/// <param name="minInclusive">Minimum range (inclusive)</param>
		/// <param name="maxExclusive">Maximum range (exclusive)</param>
		/// <returns>An iterator for that range.</returns>
		public static IEnumerable<Vector3I> EnumerateRange(Vector3I minInclusive, Vector3I maxExclusive)
		{
			Vector3I vec = default(Vector3I);
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

		public bool IsAxisAligned()
		{
			int num = 0;
			if (X == 0)
			{
				num++;
			}
			if (Y == 0)
			{
				num++;
			}
			if (Z == 0)
			{
				num++;
			}
			return num == 2;
		}
	}
}
