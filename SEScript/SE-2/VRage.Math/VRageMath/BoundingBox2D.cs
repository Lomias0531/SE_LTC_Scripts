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
	public struct BoundingBox2D : IEquatable<BoundingBox2D>
	{
		protected class VRageMath_BoundingBox2D_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBox2D, Vector2D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2D owner, in Vector2D value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2D owner, out Vector2D value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBox2D_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBox2D, Vector2D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox2D owner, in Vector2D value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox2D owner, out Vector2D value)
			{
				value = owner.Max;
			}
		}

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingBox2D.
		/// </summary>
		public const int CornerCount = 8;

		/// <summary>
		/// The minimum point the BoundingBox2D contains.
		/// </summary>
		[ProtoMember(1)]
		public Vector2D Min;

		/// <summary>
		/// The maximum point the BoundingBox2D contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector2D Max;

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector2D Center => (Min + Max) / 2.0;

		public Vector2D HalfExtents => (Max - Min) / 2.0;

		public Vector2D Extents => Max - Min;

		public double Width => Max.X - Min.X;

		public double Height => Max.Y - Min.Y;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector2D Size => Max - Min;

		/// <summary>
		/// Creates an instance of BoundingBox2D.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBox2D includes.</param><param name="max">The maximum point the BoundingBox2D includes.</param>
		public BoundingBox2D(Vector2D min, Vector2D max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2D are equal.
		/// </summary>
		/// <param name="a">BoundingBox2D to compare.</param><param name="b">BoundingBox2D to compare.</param>
		public static bool operator ==(BoundingBox2D a, BoundingBox2D b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2D are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBox2D a, BoundingBox2D b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBox2D.
		/// </summary>
		public Vector2D[] GetCorners()
		{
			return new Vector2D[8]
			{
				new Vector2D(Min.X, Max.Y),
				new Vector2D(Max.X, Max.Y),
				new Vector2D(Max.X, Min.Y),
				new Vector2D(Min.X, Min.Y),
				new Vector2D(Min.X, Max.Y),
				new Vector2D(Max.X, Max.Y),
				new Vector2D(Max.X, Min.Y),
				new Vector2D(Min.X, Min.Y)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox2D.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2D points where the corners of the BoundingBox2D are written.</param>
		public void GetCorners(Vector2D[] corners)
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
		/// Gets the array of points that make up the corners of the BoundingBox2D.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector2D points where the corners of the BoundingBox2D are written.</param>
		public unsafe void GetCornersUnsafe(Vector2D* corners)
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
		/// Determines whether two instances of BoundingBox2D are equal.
		/// </summary>
		/// <param name="other">The BoundingBox2D to compare with the current BoundingBox2D.</param>
		public bool Equals(BoundingBox2D other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox2D are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingBox2D.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingBox2D)
			{
				result = Equals((BoundingBox2D)obj);
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
		/// Returns a String that represents the current BoundingBox2D.
		/// </summary>
		public override string ToString()
		{
			return $"Min:{Min} Max:{Max}";
		}

		/// <summary>
		/// Creates the smallest BoundingBox2D that contains the two specified BoundingBox2D instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2Ds to contain.</param><param name="additional">One of the BoundingBox2Ds to contain.</param>
		public static BoundingBox2D CreateMerged(BoundingBox2D original, BoundingBox2D additional)
		{
			BoundingBox2D result = default(BoundingBox2D);
			Vector2D.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector2D.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2D that contains the two specified BoundingBox2D instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox2D instances to contain.</param><param name="additional">One of the BoundingBox2D instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox2D.</param>
		public static void CreateMerged(ref BoundingBox2D original, ref BoundingBox2D additional, out BoundingBox2D result)
		{
			Vector2D.Min(ref original.Min, ref additional.Min, out Vector2D result2);
			Vector2D.Max(ref original.Max, ref additional.Max, out Vector2D result3);
			result.Min = result2;
			result.Max = result3;
		}

		/// <summary>
		/// Creates the smallest BoundingBox2D that will contain a group of points.
		/// </summary>
		/// <param name="points">A list of points the BoundingBox2D should contain.</param>
		public static BoundingBox2D CreateFromPoints(IEnumerable<Vector2D> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector2D value = new Vector2D(double.MaxValue);
			Vector2D value2 = new Vector2D(double.MinValue);
			foreach (Vector2D point in points)
			{
				Vector2D value3 = point;
				Vector2D.Min(ref value, ref value3, out value);
				Vector2D.Max(ref value2, ref value3, out value2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBox2D(value, value2);
		}

		public static BoundingBox2D CreateFromHalfExtent(Vector2D center, double halfExtent)
		{
			return CreateFromHalfExtent(center, new Vector2D(halfExtent));
		}

		public static BoundingBox2D CreateFromHalfExtent(Vector2D center, Vector2D halfExtent)
		{
			return new BoundingBox2D(center - halfExtent, center + halfExtent);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBox2D Intersect(BoundingBox2D box)
		{
			BoundingBox2D result = default(BoundingBox2D);
			result.Min.X = Math.Max(Min.X, box.Min.X);
			result.Min.Y = Math.Max(Min.Y, box.Min.Y);
			result.Max.X = Math.Min(Max.X, box.Max.X);
			result.Max.Y = Math.Min(Max.Y, box.Max.Y);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingBox2D intersects another BoundingBox2D.
		/// </summary>
		/// <param name="box">The BoundingBox2D to check for intersection with.</param>
		public bool Intersects(BoundingBox2D box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBox2D box)
		{
			if (Max.X >= box.Min.X && Min.X <= box.Max.X)
			{
				if (Max.Y >= box.Min.Y)
				{
					return Min.Y <= box.Max.Y;
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Checks whether the current BoundingBox2D intersects another BoundingBox2D.
		/// </summary>
		/// <param name="box">The BoundingBox2D to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox2D instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox2D box, out bool result)
		{
			result = false;
			if (!(Max.X < box.Min.X) && !(Min.X > box.Max.X) && !(Max.Y < box.Min.Y) && !(Min.Y > box.Max.Y))
			{
				result = true;
			}
		}

		public double Distance(Vector2D point)
		{
			return Vector2D.Distance(Vector2D.Clamp(point, Min, Max), point);
		}

		/// <summary>
		/// Tests whether the BoundingBox2D contains another BoundingBox2D.
		/// </summary>
		/// <param name="box">The BoundingBox2D to test for overlap.</param>
		public ContainmentType Contains(BoundingBox2D box)
		{
			if (Max.X < box.Min.X || Min.X > box.Max.X || Max.Y < box.Min.Y || Min.Y > box.Max.Y)
			{
				return ContainmentType.Disjoint;
			}
			if (!(Min.X > box.Min.X) && !(box.Max.X > Max.X) && !(Min.Y > box.Min.Y) && !(box.Max.Y > Max.Y))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox2D contains a BoundingBox2D.
		/// </summary>
		/// <param name="box">The BoundingBox2D to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox2D box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!(Max.X < box.Min.X) && !(Min.X > box.Max.X) && !(Max.Y < box.Min.Y) && !(Min.Y > box.Max.Y))
			{
				result = ((!(Min.X > box.Min.X) && !(box.Max.X > Max.X) && !(Min.Y > box.Min.Y) && !(box.Max.Y > Max.Y)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox2D contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param>
		public ContainmentType Contains(Vector2D point)
		{
			if (!(Min.X > point.X) && !(point.X > Max.X) && !(Min.Y > point.Y) && !(point.Y > Max.Y))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBox2D contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector2D point, out ContainmentType result)
		{
			result = ((!(Min.X > point.X) && !(point.X > Max.X) && !(Min.Y > point.Y) && !(point.Y > Max.Y)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		internal void SupportMapping(ref Vector2D v, out Vector2D result)
		{
			result.X = ((v.X >= 0.0) ? Max.X : Min.X);
			result.Y = ((v.Y >= 0.0) ? Max.Y : Min.Y);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="vctTranlsation"></param>
		/// <returns></returns>
		public BoundingBox2D Translate(Vector2D vctTranlsation)
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
		public BoundingBox2D Include(ref Vector2D point)
		{
			Min.X = Math.Min(point.X, Min.X);
			Min.Y = Math.Min(point.Y, Min.Y);
			Max.X = Math.Max(point.X, Max.X);
			Max.Y = Math.Max(point.Y, Max.Y);
			return this;
		}

		public BoundingBox2D GetIncluded(Vector2D point)
		{
			BoundingBox2D result = this;
			result.Include(point);
			return result;
		}

		public BoundingBox2D Include(Vector2D point)
		{
			return Include(ref point);
		}

		public BoundingBox2D Include(Vector2D p0, Vector2D p1, Vector2D p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBox2D Include(ref Vector2D p0, ref Vector2D p1, ref Vector2D p2)
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
		public BoundingBox2D Include(ref BoundingBox2D box)
		{
			Min = Vector2D.Min(Min, box.Min);
			Max = Vector2D.Max(Max, box.Max);
			return this;
		}

		public BoundingBox2D Include(BoundingBox2D box)
		{
			return Include(ref box);
		}

		public static BoundingBox2D CreateInvalid()
		{
			BoundingBox2D result = default(BoundingBox2D);
			Vector2D min = new Vector2D(double.MaxValue, double.MaxValue);
			Vector2D max = new Vector2D(double.MinValue, double.MinValue);
			result.Min = min;
			result.Max = max;
			return result;
		}

		public double Perimeter()
		{
			Vector2D vector2D = Max - Min;
			return 2.0 * (vector2D.X = vector2D.Y);
		}

		public double Area()
		{
			Vector2D vector2D = Max - Min;
			return vector2D.X * vector2D.Y;
		}

		public void Inflate(double size)
		{
			Max += new Vector2D(size);
			Min -= new Vector2D(size);
		}

		public void InflateToMinimum(Vector2D minimumSize)
		{
			Vector2D center = Center;
			if (Size.X < minimumSize.X)
			{
				Min.X = center.X - minimumSize.X / 2.0;
				Max.X = center.X + minimumSize.X / 2.0;
			}
			if (Size.Y < minimumSize.Y)
			{
				Min.Y = center.Y - minimumSize.Y / 2.0;
				Max.Y = center.Y + minimumSize.Y / 2.0;
			}
		}

		public void Scale(Vector2D scale)
		{
			Vector2D center = Center;
			Vector2D value = HalfExtents * scale;
			Min = center - value;
			Max = center + value;
		}
	}
}
