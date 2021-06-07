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
	public struct BoundingBox2I : IEquatable<BoundingBox2I>
	{
		protected class VRageMath_BoundingBox2I_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBox2I, Vector2I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2I owner, in Vector2I value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2I owner, out Vector2I value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBox2I_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBox2I, Vector2I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2I owner, in Vector2I value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2I owner, out Vector2I value)
			{
				value = owner.Max;
			}
		}

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingBox2I.
		/// </summary>
		public const int CornerCount = 8;

		/// <summary>
		/// The minimum point the BoundingBox2I contains.
		/// </summary>
		[ProtoMember(1)]
		public Vector2I Min;

		/// <summary>
		/// The maximum point the BoundingBox2I contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector2I Max;

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector2I Center => (Min + Max) / 2;

		public Vector2I HalfExtents => (Max - Min) / 2;

		public Vector2I Extents => Max - Min;

		public float Width => Max.X - Min.X;

		public float Height => Max.Y - Min.Y;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector2I Size => Max - Min;

		/// <summary>
		/// Creates an instance of BoundingBox2I.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBox2I includes.</param><param name="max">The maximum point the BoundingBox2I includes.</param>
		public BoundingBox2I(Vector2I min, Vector2I max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2I are equal.
		/// </summary>
		/// <param name="a">BoundingBox2I to compare.</param><param name="b">BoundingBox2I to compare.</param>
		public static bool operator ==(BoundingBox2I a, BoundingBox2I b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2I are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBox2I a, BoundingBox2I b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBox2I.
		/// </summary>
		public Vector2I[] GetCorners()
		{
			return new Vector2I[8]
			{
				new Vector2I(Min.X, Max.Y),
				new Vector2I(Max.X, Max.Y),
				new Vector2I(Max.X, Min.Y),
				new Vector2I(Min.X, Min.Y),
				new Vector2I(Min.X, Max.Y),
				new Vector2I(Max.X, Max.Y),
				new Vector2I(Max.X, Min.Y),
				new Vector2I(Min.X, Min.Y)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox2I.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2I points where the corners of the BoundingBox2I are written.</param>
		public void GetCorners(Vector2I[] corners)
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
		/// Gets the array of points that make up the corners of the BoundingBox2I.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2I points where the corners of the BoundingBox2I are written.</param>
		public unsafe void GetCornersUnsafe(Vector2I* corners)
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
		/// Determines whether two instances of BoundingBox2I are equal.
		/// </summary>
		/// <param name="other">The BoundingBox2I to compare with the current BoundingBox2I.</param>
		public bool Equals(BoundingBox2I other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2I are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingBox2I.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingBox2I)
			{
				result = Equals((BoundingBox2I)obj);
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
		/// Returns a String that represents the current BoundingBox2I.
		/// </summary>
		public override string ToString()
		{
			return $"Min:{Min} Max:{Max}";
		}

		/// <summary>
		/// Creates the smallest BoundingBox2I that contains the two specified BoundingBox2I instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2Is to contain.</param><param name="additional">One of the BoundingBox2Is to contain.</param>
		public static BoundingBox2I CreateMerged(BoundingBox2I original, BoundingBox2I additional)
		{
			BoundingBox2I result = default(BoundingBox2I);
			Vector2I.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector2I.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2I that contains the two specified BoundingBox2I instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2I instances to contain.</param><param name="additional">One of the BoundingBox2I instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox2I.</param>
		public static void CreateMerged(ref BoundingBox2I original, ref BoundingBox2I additional, out BoundingBox2I result)
		{
			Vector2I.Min(ref original.Min, ref additional.Min, out Vector2I min);
			Vector2I.Max(ref original.Max, ref additional.Max, out Vector2I max);
			result.Min = min;
			result.Max = max;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2I that will contain a group of points.
		/// </summary>
		/// <param name="points">A list of points the BoundingBox2I should contain.</param>
		public static BoundingBox2I CreateFromPoints(IEnumerable<Vector2I> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector2I v = new Vector2I(int.MaxValue);
			Vector2I v2 = new Vector2I(int.MinValue);
			foreach (Vector2I point in points)
			{
				Vector2I v3 = point;
				Vector2I.Min(ref v, ref v3, out v);
				Vector2I.Max(ref v2, ref v3, out v2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBox2I(v, v2);
		}

		public static BoundingBox2I CreateFromHalfExtent(Vector2I center, int halfExtent)
		{
			return CreateFromHalfExtent(center, new Vector2I(halfExtent));
		}

		public static BoundingBox2I CreateFromHalfExtent(Vector2I center, Vector2I halfExtent)
		{
			return new BoundingBox2I(center - halfExtent, center + halfExtent);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBox2I Intersect(BoundingBox2I box)
		{
			BoundingBox2I result = default(BoundingBox2I);
			result.Min.X = Math.Max(Min.X, box.Min.X);
			result.Min.Y = Math.Max(Min.Y, box.Min.Y);
			result.Max.X = Math.Min(Max.X, box.Max.X);
			result.Max.Y = Math.Min(Max.Y, box.Max.Y);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingBox2I intersects another BoundingBox2I.
		/// </summary>
		/// <param name="box">The BoundingBox2I to check for intersection with.</param>
		public bool Intersects(BoundingBox2I box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBox2I box)
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
		/// Checks whether the current BoundingBox2I intersects another BoundingBox2I.
		/// </summary>
		/// <param name="box">The BoundingBox2I to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox2I instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox2I box, out bool result)
		{
			result = false;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y))
			{
				result = true;
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox2I contains another BoundingBox2I.
		/// </summary>
		/// <param name="box">The BoundingBox2I to test for overlap.</param>
		public ContainmentType Contains(BoundingBox2I box)
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
		/// Tests whether the BoundingBox2I contains a BoundingBox2I.
		/// </summary>
		/// <param name="box">The BoundingBox2I to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox2I box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y))
			{
				result = ((!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox2I contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param>
		public ContainmentType Contains(Vector2I point)
		{
			if (!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBox2I contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector2I point, out ContainmentType result)
		{
			result = ((!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		internal void SupportMapping(ref Vector2I v, out Vector2I result)
		{
			result.X = (((double)v.X >= 0.0) ? Max.X : Min.X);
			result.Y = (((double)v.Y >= 0.0) ? Max.Y : Min.Y);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="vctTranlsation"></param>
		/// <returns></returns>
		public BoundingBox2I Translate(Vector2I vctTranlsation)
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
		public BoundingBox2I Include(ref Vector2I point)
		{
			Min.X = Math.Min(point.X, Min.X);
			Min.Y = Math.Min(point.Y, Min.Y);
			Max.X = Math.Max(point.X, Max.X);
			Max.Y = Math.Max(point.Y, Max.Y);
			return this;
		}

		public BoundingBox2I GetIncluded(Vector2I point)
		{
			BoundingBox2I result = this;
			result.Include(point);
			return result;
		}

		public BoundingBox2I Include(Vector2I point)
		{
			return Include(ref point);
		}

		public BoundingBox2I Include(Vector2I p0, Vector2I p1, Vector2I p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBox2I Include(ref Vector2I p0, ref Vector2I p1, ref Vector2I p2)
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
		public BoundingBox2I Include(ref BoundingBox2I box)
		{
			Min = Vector2I.Min(Min, box.Min);
			Max = Vector2I.Max(Max, box.Max);
			return this;
		}

		public BoundingBox2I Include(BoundingBox2I box)
		{
			return Include(ref box);
		}

		public static BoundingBox2I CreateInvalid()
		{
			BoundingBox2I result = default(BoundingBox2I);
			Vector2I min = new Vector2I(int.MaxValue, int.MaxValue);
			Vector2I max = new Vector2I(int.MinValue, int.MinValue);
			result.Min = min;
			result.Max = max;
			return result;
		}

		public float Perimeter()
		{
			Vector2I vector2I = Max - Min;
			return 2 * (vector2I.X = vector2I.Y);
		}

		public float Area()
		{
			Vector2I vector2I = Max - Min;
			return vector2I.X * vector2I.Y;
		}

		public void Inflate(int size)
		{
			Max += new Vector2I(size);
			Min -= new Vector2I(size);
		}

		public void InflateToMinimum(Vector2I minimumSize)
		{
			Vector2I center = Center;
			if (Size.X < minimumSize.X)
			{
				Min.X = center.X - minimumSize.X / 2;
				Max.X = center.X + minimumSize.X / 2;
			}
			if (Size.Y < minimumSize.Y)
			{
				Min.Y = center.Y - minimumSize.Y / 2;
				Max.Y = center.Y + minimumSize.Y / 2;
			}
		}

		public void Scale(Vector2I scale)
		{
			Vector2I center = Center;
			Vector2I halfExtents = HalfExtents;
			halfExtents.X *= scale.X;
			halfExtents.Y *= scale.Y;
			Min = center - halfExtents;
			Max = center + halfExtents;
		}
	}
}
