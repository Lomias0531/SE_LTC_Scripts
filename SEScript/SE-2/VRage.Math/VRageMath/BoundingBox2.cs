using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines an axis-aligned box-shaped 3D volume.
	/// </summary>
	[Serializable]
	public struct BoundingBox2 : IEquatable<BoundingBox2>
	{
		protected class VRageMath_BoundingBox2_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBox2, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2 owner, in Vector2 value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2 owner, out Vector2 value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBox2_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBox2, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2 owner, in Vector2 value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2 owner, out Vector2 value)
			{
				value = owner.Max;
			}
		}

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingBox2.
		/// </summary>
		public const int CornerCount = 8;

		/// <summary>
		/// The minimum point the BoundingBox2 contains.
		/// </summary>
		[ProtoMember(1)]
		public Vector2 Min;

		/// <summary>
		/// The maximum point the BoundingBox2 contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector2 Max;

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector2 Center => (Min + Max) / 2f;

		public Vector2 HalfExtents => (Max - Min) / 2f;

		public Vector2 Extents => Max - Min;

		public float Width => Max.X - Min.X;

		public float Height => Max.Y - Min.Y;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector2 Size => Max - Min;

		/// <summary>
		/// Creates an instance of BoundingBox2.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBox2 includes.</param><param name="max">The maximum point the BoundingBox2 includes.</param>
		public BoundingBox2(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2 are equal.
		/// </summary>
		/// <param name="a">BoundingBox2 to compare.</param><param name="b">BoundingBox2 to compare.</param>
		public static bool operator ==(BoundingBox2 a, BoundingBox2 b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2 are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBox2 a, BoundingBox2 b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBox2.
		/// </summary>
		public Vector2[] GetCorners()
		{
			return new Vector2[8]
			{
				new Vector2(Min.X, Max.Y),
				new Vector2(Max.X, Max.Y),
				new Vector2(Max.X, Min.Y),
				new Vector2(Min.X, Min.Y),
				new Vector2(Min.X, Max.Y),
				new Vector2(Max.X, Max.Y),
				new Vector2(Max.X, Min.Y),
				new Vector2(Min.X, Min.Y)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox2.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2 points where the corners of the BoundingBox2 are written.</param>
		public void GetCorners(Vector2[] corners)
		{
			corners[0].X = Min.X;
			corners[0].Y = Max.Y;
			corners[1].X = Max.X;
			corners[1].Y = Max.Y;
			corners[2].X = Max.X;
			corners[2].Y = Min.Y;
			corners[3].X = Min.X;
			corners[3].Y = Min.Y;
			corners[4].X = Min.X;
			corners[4].Y = Max.Y;
			corners[5].X = Max.X;
			corners[5].Y = Max.Y;
			corners[6].X = Max.X;
			corners[6].Y = Min.Y;
			corners[7].X = Min.X;
			corners[7].Y = Min.Y;
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox2.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2 points where the corners of the BoundingBox2 are written.</param>
		public unsafe void GetCornersUnsafe(Vector2* corners)
		{
			corners->X = Min.X;
			corners->Y = Max.Y;
			corners[1].X = Max.X;
			corners[1].Y = Max.Y;
			corners[2].X = Max.X;
			corners[2].Y = Min.Y;
			corners[3].X = Min.X;
			corners[3].Y = Min.Y;
			corners[4].X = Min.X;
			corners[4].Y = Max.Y;
			corners[5].X = Max.X;
			corners[5].Y = Max.Y;
			corners[6].X = Max.X;
			corners[6].Y = Min.Y;
			corners[7].X = Min.X;
			corners[7].Y = Min.Y;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2 are equal.
		/// </summary>
		/// <param name="other">The BoundingBox2 to compare with the current BoundingBox2.</param>
		public bool Equals(BoundingBox2 other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2 are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingBox2.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingBox2)
			{
				result = Equals((BoundingBox2)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return Min.GetHashCode() + Max.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current BoundingBox2.
		/// </summary>
		public override string ToString()
		{
			return $"Min:{Min} Max:{Max}";
		}

		/// <summary>
		/// Creates the smallest BoundingBox2 that contains the two specified BoundingBox2 instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2s to contain.</param><param name="additional">One of the BoundingBox2s to contain.</param>
		public static BoundingBox2 CreateMerged(BoundingBox2 original, BoundingBox2 additional)
		{
			BoundingBox2 result = default(BoundingBox2);
			Vector2.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector2.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2 that contains the two specified BoundingBox2 instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2 instances to contain.</param><param name="additional">One of the BoundingBox2 instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox2.</param>
		public static void CreateMerged(ref BoundingBox2 original, ref BoundingBox2 additional, out BoundingBox2 result)
		{
			Vector2.Min(ref original.Min, ref additional.Min, out Vector2 result2);
			Vector2.Max(ref original.Max, ref additional.Max, out Vector2 result3);
			result.Min = result2;
			result.Max = result3;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2 that will contain a group of points.
		/// </summary>
		/// <param name="points">A list of points the BoundingBox2 should contain.</param>
		public static BoundingBox2 CreateFromPoints(IEnumerable<Vector2> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector2 value = new Vector2(float.MaxValue);
			Vector2 value2 = new Vector2(float.MinValue);
			foreach (Vector2 point in points)
			{
				Vector2 value3 = point;
				Vector2.Min(ref value, ref value3, out value);
				Vector2.Max(ref value2, ref value3, out value2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBox2(value, value2);
		}

		public static BoundingBox2 CreateFromHalfExtent(Vector2 center, float halfExtent)
		{
			return CreateFromHalfExtent(center, new Vector2(halfExtent));
		}

		public static BoundingBox2 CreateFromHalfExtent(Vector2 center, Vector2 halfExtent)
		{
			return new BoundingBox2(center - halfExtent, center + halfExtent);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBox2 Intersect(BoundingBox2 box)
		{
			BoundingBox2 result = default(BoundingBox2);
			result.Min.X = Math.Max(Min.X, box.Min.X);
			result.Min.Y = Math.Max(Min.Y, box.Min.Y);
			result.Max.X = Math.Min(Max.X, box.Max.X);
			result.Max.Y = Math.Min(Max.Y, box.Max.Y);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingBox2 intersects another BoundingBox2.
		/// </summary>
		/// <param name="box">The BoundingBox2 to check for intersection with.</param>
		public bool Intersects(BoundingBox2 box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBox2 box)
		{
			if ((double)Max.X >= (double)box.Min.X && (double)Min.X <= (double)box.Max.X)
			{
				if ((double)Max.Y >= (double)box.Min.Y)
				{
					return (double)Min.Y <= (double)box.Max.Y;
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Checks whether the current BoundingBox2 intersects another BoundingBox2.
		/// </summary>
		/// <param name="box">The BoundingBox2 to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox2 instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox2 box, out bool result)
		{
			result = false;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y))
			{
				result = true;
			}
		}

		public float Distance(Vector2 point)
		{
			return Vector2.Distance(Vector2.Clamp(point, Min, Max), point);
		}

		/// <summary>
		/// Tests whether the BoundingBox2 contains another BoundingBox2.
		/// </summary>
		/// <param name="box">The BoundingBox2 to test for overlap.</param>
		public ContainmentType Contains(BoundingBox2 box)
		{
			if ((double)Max.X < (double)box.Min.X || (double)Min.X > (double)box.Max.X || (double)Max.Y < (double)box.Min.Y || (double)Min.Y > (double)box.Max.Y)
			{
				return ContainmentType.Disjoint;
			}
			if (!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox2 contains a BoundingBox2.
		/// </summary>
		/// <param name="box">The BoundingBox2 to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox2 box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y))
			{
				result = ((!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox2 contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param>
		public ContainmentType Contains(Vector2 point)
		{
			if (!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBox2 contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector2 point, out ContainmentType result)
		{
			result = ((!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		internal void SupportMapping(ref Vector2 v, out Vector2 result)
		{
			result.X = (((double)v.X >= 0.0) ? Max.X : Min.X);
			result.Y = (((double)v.Y >= 0.0) ? Max.Y : Min.Y);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="vctTranlsation"></param>
		/// <returns></returns>
		public BoundingBox2 Translate(Vector2 vctTranlsation)
		{
			Min += vctTranlsation;
			Max += vctTranlsation;
			return this;
		}

		/// <summary>
		/// return expanded aabb (abb include point)
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public BoundingBox2 Include(ref Vector2 point)
		{
			Min.X = Math.Min(point.X, Min.X);
			Min.Y = Math.Min(point.Y, Min.Y);
			Max.X = Math.Max(point.X, Max.X);
			Max.Y = Math.Max(point.Y, Max.Y);
			return this;
		}

		public BoundingBox2 GetIncluded(Vector2 point)
		{
			BoundingBox2 result = this;
			result.Include(point);
			return result;
		}

		public BoundingBox2 Include(Vector2 point)
		{
			return Include(ref point);
		}

		public BoundingBox2 Include(Vector2 p0, Vector2 p1, Vector2 p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBox2 Include(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2)
		{
			Include(ref p0);
			Include(ref p1);
			Include(ref p2);
			return this;
		}

		/// <summary>
		/// return expanded aabb (abb include point)
		/// </summary>
		/// <param name="box"></param>
		/// <returns></returns>
		public BoundingBox2 Include(ref BoundingBox2 box)
		{
			Min = Vector2.Min(Min, box.Min);
			Max = Vector2.Max(Max, box.Max);
			return this;
		}

		public BoundingBox2 Include(BoundingBox2 box)
		{
			return Include(ref box);
		}

		public static BoundingBox2 CreateInvalid()
		{
			BoundingBox2 result = default(BoundingBox2);
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);
			result.Min = min;
			result.Max = max;
			return result;
		}

		public float Perimeter()
		{
			Vector2 vector = Max - Min;
			return 2f * (vector.X = vector.Y);
		}

		public float Area()
		{
			Vector2 vector = Max - Min;
			return vector.X * vector.Y;
		}

		public void Inflate(float size)
		{
			Max += new Vector2(size);
			Min -= new Vector2(size);
		}

		public void InflateToMinimum(Vector2 minimumSize)
		{
			Vector2 center = Center;
			if (Size.X < minimumSize.X)
			{
				Min.X = center.X - minimumSize.X / 2f;
				Max.X = center.X + minimumSize.X / 2f;
			}
			if (Size.Y < minimumSize.Y)
			{
				Min.Y = center.Y - minimumSize.Y / 2f;
				Max.Y = center.Y + minimumSize.Y / 2f;
			}
		}

		public void Scale(Vector2 scale)
		{
			Vector2 center = Center;
			Vector2 value = HalfExtents * scale;
			Min = center - value;
			Max = center + value;
		}
	}
}
