using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines an axis-aligned box-shaped 3D volume.
	/// </summary>
	[Serializable]
	[ProtoContract]
	public struct BoundingBoxD : IEquatable<BoundingBoxD>
	{
		public class ComparerType : IEqualityComparer<BoundingBox>
		{
			public bool Equals(BoundingBox x, BoundingBox y)
			{
				if (x.Min == y.Min)
				{
					return x.Max == y.Max;
				}
				return false;
			}

			public int GetHashCode(BoundingBox obj)
			{
				return obj.Min.GetHashCode() ^ obj.Max.GetHashCode();
			}
		}

		protected class VRageMath_BoundingBoxD_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBoxD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBoxD owner, in Vector3D value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBoxD owner, out Vector3D value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBoxD_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBoxD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBoxD owner, in Vector3D value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBoxD owner, out Vector3D value)
			{
				value = owner.Max;
			}
		}

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingBox.
		/// </summary>
		public const int CornerCount = 8;

		/// <summary>
		/// The minimum point the BoundingBox contains.
		/// </summary>
		[ProtoMember(1)]
		public Vector3D Min;

		/// <summary>
		/// The maximum point the BoundingBox contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector3D Max;

		public static readonly ComparerType Comparer = new ComparerType();

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector3D Center => (Min + Max) * 0.5;

		public Vector3D HalfExtents => (Max - Min) * 0.5;

		public Vector3D Extents => Max - Min;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector3D Size => Max - Min;

		/// <summary>
		/// Matrix of AABB, respection center and size
		/// </summary>
		public MatrixD Matrix
		{
			get
			{
				Vector3D position = Center;
				Vector3D scale = Size;
				MatrixD.CreateTranslation(ref position, out MatrixD result);
				MatrixD.Rescale(ref result, ref scale);
				return result;
			}
		}

		public double SurfaceArea
		{
			get
			{
				Vector3D vector3D = Max - Min;
				return 2.0 * (vector3D.X * vector3D.Y + vector3D.X * vector3D.Z + vector3D.Y * vector3D.Z);
			}
		}

		public double Volume
		{
			get
			{
				Vector3D vector3D = Max - Min;
				return vector3D.X * vector3D.Y * vector3D.Z;
			}
		}

		/// <summary>
		/// return perimeter of edges
		/// </summary>
		/// <returns></returns>
		public double Perimeter
		{
			get
			{
				double num = Max.X - Min.X;
				double num2 = Max.Y - Min.Y;
				double num3 = Max.Z - Min.Z;
				return 4.0 * (num + num2 + num3);
			}
		}

		public bool Valid
		{
			get
			{
				if (Min != new Vector3D(double.MaxValue))
				{
					return Max != new Vector3D(double.MinValue);
				}
				return false;
			}
		}

		/// <summary>
		/// Creates an instance of BoundingBox.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBox includes.</param><param name="max">The maximum point the BoundingBox includes.</param>
		public BoundingBoxD(Vector3D min, Vector3D max)
		{
			Min = min;
			Max = max;
		}

		public static implicit operator BoundingBoxD(BoundingBoxI box)
		{
			return new BoundingBoxD(box.Min, box.Max);
		}

		public static implicit operator BoundingBoxD(BoundingBox box)
		{
			return new BoundingBoxD(box.Min, box.Max);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are equal.
		/// </summary>
		/// <param name="a">BoundingBox to compare.</param><param name="b">BoundingBox to compare.</param>
		public static bool operator ==(BoundingBoxD a, BoundingBoxD b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBoxD a, BoundingBoxD b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		public static BoundingBoxD operator +(BoundingBoxD a, Vector3D b)
		{
			BoundingBoxD result = default(BoundingBoxD);
			result.Max = a.Max + b;
			result.Min = a.Min + b;
			return result;
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBox. ALLOCATION!
		/// </summary>
		public Vector3D[] GetCorners()
		{
			return new Vector3D[8]
			{
				new Vector3D(Min.X, Max.Y, Max.Z),
				new Vector3D(Max.X, Max.Y, Max.Z),
				new Vector3D(Max.X, Min.Y, Max.Z),
				new Vector3D(Min.X, Min.Y, Max.Z),
				new Vector3D(Min.X, Max.Y, Min.Z),
				new Vector3D(Max.X, Max.Y, Min.Z),
				new Vector3D(Max.X, Min.Y, Min.Z),
				new Vector3D(Min.X, Min.Y, Min.Z)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
		public void GetCorners(Vector3D[] corners)
		{
			corners[0].X = Min.X;
			corners[0].Y = Max.Y;
			corners[0].Z = Max.Z;
			corners[1].X = Max.X;
			corners[1].Y = Max.Y;
			corners[1].Z = Max.Z;
			corners[2].X = Max.X;
			corners[2].Y = Min.Y;
			corners[2].Z = Max.Z;
			corners[3].X = Min.X;
			corners[3].Y = Min.Y;
			corners[3].Z = Max.Z;
			corners[4].X = Min.X;
			corners[4].Y = Max.Y;
			corners[4].Z = Min.Z;
			corners[5].X = Max.X;
			corners[5].Y = Max.Y;
			corners[5].Z = Min.Z;
			corners[6].X = Max.X;
			corners[6].Y = Min.Y;
			corners[6].Z = Min.Z;
			corners[7].X = Min.X;
			corners[7].Y = Min.Y;
			corners[7].Z = Min.Z;
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
		public unsafe void GetCornersUnsafe(Vector3D* corners)
		{
			corners->X = Min.X;
			corners->Y = Max.Y;
			corners->Z = Max.Z;
			corners[1].X = Max.X;
			corners[1].Y = Max.Y;
			corners[1].Z = Max.Z;
			corners[2].X = Max.X;
			corners[2].Y = Min.Y;
			corners[2].Z = Max.Z;
			corners[3].X = Min.X;
			corners[3].Y = Min.Y;
			corners[3].Z = Max.Z;
			corners[4].X = Min.X;
			corners[4].Y = Max.Y;
			corners[4].Z = Min.Z;
			corners[5].X = Max.X;
			corners[5].Y = Max.Y;
			corners[5].Z = Min.Z;
			corners[6].X = Max.X;
			corners[6].Y = Min.Y;
			corners[6].Z = Min.Z;
			corners[7].X = Min.X;
			corners[7].Y = Min.Y;
			corners[7].Z = Min.Z;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are equal.
		/// </summary>
		/// <param name="other">The BoundingBox to compare with the current BoundingBox.</param>
		public bool Equals(BoundingBoxD other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingBox.</param>
		public override bool Equals(object obj)
		{
			if (obj is BoundingBoxD)
			{
				return Equals((BoundingBoxD)obj);
			}
			return false;
		}

		public bool Equals(BoundingBoxD other, double epsilon)
		{
			if (Min.Equals(other.Min, epsilon))
			{
				return Max.Equals(other.Max, epsilon);
			}
			return false;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return Min.GetHashCode() + Max.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current BoundingBox.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[2]
			{
				Min.ToString(),
				Max.ToString()
			});
		}

		/// <summary>
		/// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
		/// </summary>
		/// <param name="original">One of the BoundingBoxs to contain.</param><param name="additional">One of the BoundingBoxs to contain.</param>
		public static BoundingBoxD CreateMerged(BoundingBoxD original, BoundingBoxD additional)
		{
			BoundingBoxD result = default(BoundingBoxD);
			Vector3D.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector3D.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox instances to contain.</param><param name="additional">One of the BoundingBox instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox.</param>
		public static void CreateMerged(ref BoundingBoxD original, ref BoundingBoxD additional, out BoundingBoxD result)
		{
			Vector3D.Min(ref original.Min, ref additional.Min, out Vector3D result2);
			Vector3D.Max(ref original.Max, ref additional.Max, out Vector3D result3);
			result.Min = result2;
			result.Max = result3;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to contain.</param>
		public static BoundingBoxD CreateFromSphere(BoundingSphereD sphere)
		{
			BoundingBoxD result = default(BoundingBoxD);
			result.Min.X = sphere.Center.X - sphere.Radius;
			result.Min.Y = sphere.Center.Y - sphere.Radius;
			result.Min.Z = sphere.Center.Z - sphere.Radius;
			result.Max.X = sphere.Center.X + sphere.Radius;
			result.Max.Y = sphere.Center.Y + sphere.Radius;
			result.Max.Z = sphere.Center.Z + sphere.Radius;
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to contain.</param><param name="result">[OutAttribute] The created BoundingBox.</param>
		public static void CreateFromSphere(ref BoundingSphereD sphere, out BoundingBoxD result)
		{
			result.Min.X = sphere.Center.X - sphere.Radius;
			result.Min.Y = sphere.Center.Y - sphere.Radius;
			result.Min.Z = sphere.Center.Z - sphere.Radius;
			result.Max.X = sphere.Center.X + sphere.Radius;
			result.Max.Y = sphere.Center.Y + sphere.Radius;
			result.Max.Z = sphere.Center.Z + sphere.Radius;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that will contain a group of points.
		/// </summary>
		/// <param name="points">A list of points the BoundingBox should contain.</param>
		public static BoundingBoxD CreateFromPoints(IEnumerable<Vector3D> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector3D value = new Vector3D(double.MaxValue);
			Vector3D value2 = new Vector3D(double.MinValue);
			foreach (Vector3D point in points)
			{
				Vector3D value3 = point;
				Vector3D.Min(ref value, ref value3, out value);
				Vector3D.Max(ref value2, ref value3, out value2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBoxD(value, value2);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBoxD Intersect(BoundingBoxD box)
		{
			BoundingBoxD result = default(BoundingBoxD);
			result.Min.X = Math.Max(Min.X, box.Min.X);
			result.Min.Y = Math.Max(Min.Y, box.Min.Y);
			result.Min.Z = Math.Max(Min.Z, box.Min.Z);
			result.Max.X = Math.Min(Max.X, box.Max.X);
			result.Max.Y = Math.Min(Max.Y, box.Max.Y);
			result.Max.Z = Math.Min(Max.Z, box.Max.Z);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects another BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param>
		public bool Intersects(BoundingBoxD box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBoxD box)
		{
			if (Max.X >= box.Min.X && Min.X <= box.Max.X && Max.Y >= box.Min.Y && Min.Y <= box.Max.Y)
			{
				if (Max.Z >= box.Min.Z)
				{
					return Min.Z <= box.Max.Z;
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects another BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBoxD box, out bool result)
		{
			result = false;
			if (!(Max.X < box.Min.X) && !(Min.X > box.Max.X) && !(Max.Y < box.Min.Y) && !(Min.Y > box.Max.Y) && !(Max.Z < box.Min.Z) && !(Min.Z > box.Max.Z))
			{
				result = true;
			}
		}

		public void Intersects(ref BoundingBox box, out bool result)
		{
			result = false;
			if (!(Max.X < (double)box.Min.X) && !(Min.X > (double)box.Max.X) && !(Max.Y < (double)box.Min.Y) && !(Min.Y > (double)box.Max.Y) && !(Max.Z < (double)box.Min.Z) && !(Min.Z > (double)box.Max.Z))
			{
				result = true;
			}
		}

		public bool IntersectsTriangle(Vector3D v0, Vector3D v1, Vector3D v2)
		{
			return IntersectsTriangle(ref v0, ref v1, ref v2);
		}

		public bool IntersectsTriangle(ref Vector3D v0, ref Vector3D v1, ref Vector3D v2)
		{
			Vector3D.Min(ref v0, ref v1, out Vector3D result);
			Vector3D.Min(ref result, ref v2, out result);
			Vector3D.Max(ref v0, ref v1, out Vector3D result2);
			Vector3D.Max(ref result2, ref v2, out result2);
			if (result.X > Max.X)
			{
				return false;
			}
			if (result2.X < Min.X)
			{
				return false;
			}
			if (result.Y > Max.Y)
			{
				return false;
			}
			if (result2.Y < Min.Y)
			{
				return false;
			}
			if (result.Z > Max.Z)
			{
				return false;
			}
			if (result2.Z < Min.Z)
			{
				return false;
			}
			Vector3D vector = v1 - v0;
			Vector3D vector2 = v2 - v1;
			Vector3D.Cross(ref vector, ref vector2, out Vector3D result3);
			Vector3D.Dot(ref v0, ref result3, out double result4);
			PlaneD plane = new PlaneD(result3, 0.0 - result4);
			Intersects(ref plane, out PlaneIntersectionType result5);
			switch (result5)
			{
			case PlaneIntersectionType.Back:
				return false;
			case PlaneIntersectionType.Front:
				return false;
			default:
			{
				Vector3D center = Center;
				Vector3D halfExtents = new BoundingBoxD(Min - center, Max - center).HalfExtents;
				Vector3D vector3D = v0 - v2;
				Vector3D vector3D2 = v0 - center;
				Vector3D vector3D3 = v1 - center;
				Vector3D vector3D4 = v2 - center;
				double num = halfExtents.Y * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.Y);
				double val = vector3D2.Z * vector3D3.Y - vector3D2.Y * vector3D3.Z;
				double val2 = vector3D4.Z * vector.Y - vector3D4.Y * vector.Z;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.X);
				val = vector3D2.X * vector3D3.Z - vector3D2.Z * vector3D3.X;
				val2 = vector3D4.X * vector.Z - vector3D4.Z * vector.X;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Y) + halfExtents.Y * Math.Abs(vector.X);
				val = vector3D2.Y * vector3D3.X - vector3D2.X * vector3D3.Y;
				val2 = vector3D4.Y * vector.X - vector3D4.X * vector.Y;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.Y);
				double val3 = vector3D3.Z * vector3D4.Y - vector3D3.Y * vector3D4.Z;
				val = vector3D2.Z * vector2.Y - vector3D2.Y * vector2.Z;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.X);
				val3 = vector3D3.X * vector3D4.Z - vector3D3.Z * vector3D4.X;
				val = vector3D2.X * vector2.Z - vector3D2.Z * vector2.X;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Y) + halfExtents.Y * Math.Abs(vector2.X);
				val3 = vector3D3.Y * vector3D4.X - vector3D3.X * vector3D4.Y;
				val = vector3D2.Y * vector2.X - vector3D2.X * vector2.Y;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector3D.Z) + halfExtents.Z * Math.Abs(vector3D.Y);
				val2 = vector3D4.Z * vector3D2.Y - vector3D4.Y * vector3D2.Z;
				val3 = vector3D3.Z * vector3D.Y - vector3D3.Y * vector3D.Z;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3D.Z) + halfExtents.Z * Math.Abs(vector3D.X);
				val2 = vector3D4.X * vector3D2.Z - vector3D4.Z * vector3D2.X;
				val3 = vector3D3.X * vector3D.Z - vector3D3.Z * vector3D.X;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0.0 - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3D.Y) + halfExtents.Y * Math.Abs(vector3D.X);
				val2 = vector3D4.Y * vector3D2.X - vector3D4.X * vector3D2.Y;
				val3 = vector3D3.Y * vector3D.X - vector3D3.X * vector3D.Y;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0.0 - num)
				{
					return false;
				}
				return true;
			}
			}
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
		public bool Intersects(BoundingFrustumD frustum)
		{
			if (null == frustum)
			{
				throw new ArgumentNullException("frustum");
			}
			return frustum.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param>
		public PlaneIntersectionType Intersects(PlaneD plane)
		{
			Vector3D vector3D = default(Vector3D);
			vector3D.X = ((plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector3D.Y = ((plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector3D.Z = ((plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			if (plane.Normal.X * vector3D.X + plane.Normal.Y * vector3D.Y + plane.Normal.Z * vector3D.Z + plane.D > 0.0)
			{
				return PlaneIntersectionType.Front;
			}
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = ((plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector3D2.Y = ((plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector3D2.Z = ((plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			if (!(plane.Normal.X * vector3D2.X + plane.Normal.Y * vector3D2.Y + plane.Normal.Z * vector3D2.Z + plane.D < 0.0))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingBox intersects the Plane.</param>
		public void Intersects(ref PlaneD plane, out PlaneIntersectionType result)
		{
			Vector3D vector3D = default(Vector3D);
			vector3D.X = ((plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector3D.Y = ((plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector3D.Z = ((plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = ((plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector3D2.Y = ((plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector3D2.Z = ((plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			if (plane.Normal.X * vector3D.X + plane.Normal.Y * vector3D.Y + plane.Normal.Z * vector3D.Z + plane.D > 0.0)
			{
				result = PlaneIntersectionType.Front;
			}
			else if (plane.Normal.X * vector3D2.X + plane.Normal.Y * vector3D2.Y + plane.Normal.Z * vector3D2.Z + plane.D < 0.0)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
		}

		public bool Intersects(ref LineD line)
		{
			double? num = Intersects(new RayD(line.From, line.Direction));
			if (!num.HasValue)
			{
				return false;
			}
			if (num.Value < 0.0)
			{
				return false;
			}
			if (num.Value > line.Length)
			{
				return false;
			}
			return true;
		}

		public bool Intersects(ref LineD line, out double distance)
		{
			distance = 0.0;
			double? num = Intersects(new RayD(line.From, line.Direction));
			if (!num.HasValue)
			{
				return false;
			}
			if (num.Value < 0.0)
			{
				return false;
			}
			if (num.Value > line.Length)
			{
				return false;
			}
			distance = num.Value;
			return true;
		}

		public double? Intersects(Ray ray)
		{
			RayD ray2 = new RayD(ray.Position, ray.Direction);
			return Intersects(ray2);
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param>
		public double? Intersects(RayD ray)
		{
			double num = 0.0;
			double num2 = double.MaxValue;
			if (Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
			{
				if (ray.Position.X < Min.X || ray.Position.X > Max.X)
				{
					return null;
				}
			}
			else
			{
				double num3 = 1.0 / ray.Direction.X;
				double num4 = (Min.X - ray.Position.X) * num3;
				double num5 = (Max.X - ray.Position.X) * num3;
				if (num4 > num5)
				{
					double num6 = num4;
					num4 = num5;
					num5 = num6;
				}
				num = MathHelper.Max(num4, num);
				num2 = MathHelper.Min(num5, num2);
				if (num > num2)
				{
					return null;
				}
			}
			if (Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
			{
				if (ray.Position.Y < Min.Y || ray.Position.Y > Max.Y)
				{
					return null;
				}
			}
			else
			{
				double num7 = 1.0 / ray.Direction.Y;
				double num8 = (Min.Y - ray.Position.Y) * num7;
				double num9 = (Max.Y - ray.Position.Y) * num7;
				if (num8 > num9)
				{
					double num10 = num8;
					num8 = num9;
					num9 = num10;
				}
				num = MathHelper.Max(num8, num);
				num2 = MathHelper.Min(num9, num2);
				if (num > num2)
				{
					return null;
				}
			}
			if (Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
			{
				if (ray.Position.Z < Min.Z || ray.Position.Z > Max.Z)
				{
					return null;
				}
			}
			else
			{
				double num11 = 1.0 / ray.Direction.Z;
				double num12 = (Min.Z - ray.Position.Z) * num11;
				double num13 = (Max.Z - ray.Position.Z) * num11;
				if (num12 > num13)
				{
					double num14 = num12;
					num12 = num13;
					num13 = num14;
				}
				num = MathHelper.Max(num12, num);
				double num15 = MathHelper.Min(num13, num2);
				if (num > num15)
				{
					return null;
				}
			}
			return num;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox, or null if there is no intersection.</param>
		public void Intersects(ref RayD ray, out double? result)
		{
			result = null;
			double num = 0.0;
			double num2 = double.MaxValue;
			if (Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
			{
				if (ray.Position.X < Min.X || ray.Position.X > Max.X)
				{
					return;
				}
			}
			else
			{
				double num3 = 1.0 / ray.Direction.X;
				double num4 = (Min.X - ray.Position.X) * num3;
				double num5 = (Max.X - ray.Position.X) * num3;
				if (num4 > num5)
				{
					double num6 = num4;
					num4 = num5;
					num5 = num6;
				}
				num = MathHelper.Max(num4, num);
				num2 = MathHelper.Min(num5, num2);
				if (num > num2)
				{
					return;
				}
			}
			if (Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
			{
				if (ray.Position.Y < Min.Y || ray.Position.Y > Max.Y)
				{
					return;
				}
			}
			else
			{
				double num7 = 1.0 / ray.Direction.Y;
				double num8 = (Min.Y - ray.Position.Y) * num7;
				double num9 = (Max.Y - ray.Position.Y) * num7;
				if (num8 > num9)
				{
					double num10 = num8;
					num8 = num9;
					num9 = num10;
				}
				num = MathHelper.Max(num8, num);
				num2 = MathHelper.Min(num9, num2);
				if (num > num2)
				{
					return;
				}
			}
			if (Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
			{
				if (ray.Position.Z < Min.Z || ray.Position.Z > Max.Z)
				{
					return;
				}
			}
			else
			{
				double num11 = 1.0 / ray.Direction.Z;
				double num12 = (Min.Z - ray.Position.Z) * num11;
				double num13 = (Max.Z - ray.Position.Z) * num11;
				if (num12 > num13)
				{
					double num14 = num12;
					num12 = num13;
					num13 = num14;
				}
				num = MathHelper.Max(num12, num);
				double num15 = MathHelper.Min(num13, num2);
				if (num > num15)
				{
					return;
				}
			}
			result = num;
		}

		public bool Intersect(ref LineD line, out LineD intersectedLine)
		{
			RayD ray = new RayD(line.From, line.Direction);
			if (!Intersect(ref ray, out double tmin, out double tmax))
			{
				intersectedLine = line;
				return false;
			}
			tmin = Math.Max(tmin, 0.0);
			tmax = Math.Min(tmax, line.Length);
			intersectedLine.From = line.From + line.Direction * tmin;
			intersectedLine.To = line.From + line.Direction * tmax;
			intersectedLine.Direction = line.Direction;
			intersectedLine.Length = tmax - tmin;
			return true;
		}

		public bool Intersect(ref LineD line, out double t1, out double t2)
		{
			RayD ray = new RayD(line.From, line.Direction);
			return Intersect(ref ray, out t1, out t2);
		}

		public bool Intersect(ref RayD ray, out double tmin, out double tmax)
		{
			double num = 1.0 / ray.Direction.X;
			double num2 = 1.0 / ray.Direction.Y;
			double num3 = 1.0 / ray.Direction.Z;
			double val = (Min.X - ray.Position.X) * num;
			double val2 = (Max.X - ray.Position.X) * num;
			double val3 = (Min.Y - ray.Position.Y) * num2;
			double val4 = (Max.Y - ray.Position.Y) * num2;
			double val5 = (Min.Z - ray.Position.Z) * num3;
			double val6 = (Max.Z - ray.Position.Z) * num3;
			tmin = Math.Max(Math.Max(Math.Min(val, val2), Math.Min(val3, val4)), Math.Min(val5, val6));
			tmax = Math.Min(Math.Min(Math.Max(val, val2), Math.Max(val3, val4)), Math.Max(val5, val6));
			if (tmax < 0.0)
			{
				return false;
			}
			if (tmin > tmax)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param>
		public bool Intersects(BoundingSphereD sphere)
		{
			return Intersects(ref sphere);
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox and BoundingSphere intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphereD sphere, out bool result)
		{
			Vector3D.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3D result2);
			Vector3D.DistanceSquared(ref sphere.Center, ref result2, out double result3);
			result = (result3 <= sphere.Radius * sphere.Radius);
		}

		public bool Intersects(ref BoundingSphereD sphere)
		{
			Vector3D.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3D result);
			Vector3D.DistanceSquared(ref sphere.Center, ref result, out double result2);
			return result2 <= sphere.Radius * sphere.Radius;
		}

		public double Distance(Vector3D point)
		{
			if (Contains(point) == ContainmentType.Contains)
			{
				return 0.0;
			}
			return Vector3D.Distance(Vector3D.Clamp(point, Min, Max), point);
		}

		public double DistanceSquared(Vector3D point)
		{
			return DistanceSquared(ref point);
		}

		public double DistanceSquared(ref Vector3D point)
		{
			Vector3D.Clamp(ref point, ref Min, ref Max, out Vector3D result);
			Vector3D.DistanceSquared(ref result, ref point, out double result2);
			return result2;
		}

		public double Distance(ref BoundingBoxD other)
		{
			return Math.Sqrt(DistanceSquared(ref other));
		}

		public double DistanceSquared(ref BoundingBoxD other)
		{
			Vector3D min = Min;
			Vector3D min2 = other.Min;
			Vector3D max = Max;
			Vector3D max2 = other.Max;
			double num = 0.0;
			if (max2.X < min.X)
			{
				double num2 = min.X - max2.X;
				num += num2 * num2;
			}
			else if (max.X < min2.X)
			{
				double num3 = min2.X - max.X;
				num += num3 * num3;
			}
			if (max2.Y < min.Y)
			{
				double num4 = min.Y - max2.Y;
				num += num4 * num4;
			}
			else if (max.Y < min2.Y)
			{
				double num5 = min2.Y - max.Y;
				num += num5 * num5;
			}
			if (max2.Z < min.Z)
			{
				double num6 = min.Z - max2.Z;
				num += num6 * num6;
			}
			else if (max.Z < min2.Z)
			{
				double num7 = min2.Z - max.Z;
				num += num7 * num7;
			}
			return num;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains another BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param>
		public ContainmentType Contains(BoundingBoxD box)
		{
			if (Max.X < box.Min.X || Min.X > box.Max.X || Max.Y < box.Min.Y || Min.Y > box.Max.Y || Max.Z < box.Min.Z || Min.Z > box.Max.Z)
			{
				return ContainmentType.Disjoint;
			}
			if (!(Min.X > box.Min.X) && !(box.Max.X > Max.X) && !(Min.Y > box.Min.Y) && !(box.Max.Y > Max.Y) && !(Min.Z > box.Min.Z) && !(box.Max.Z > Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBoxD box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!(Max.X < box.Min.X) && !(Min.X > box.Max.X) && !(Max.Y < box.Min.Y) && !(Min.Y > box.Max.Y) && !(Max.Z < box.Min.Z) && !(Min.Z > box.Max.Z))
			{
				result = ((!(Min.X > box.Min.X) && !(box.Max.X > Max.X) && !(Min.Y > box.Min.Y) && !(box.Max.Y > Max.Y) && !(Min.Z > box.Min.Z) && !(box.Max.Z > Max.Z)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to test for overlap.</param>
		public ContainmentType Contains(BoundingFrustumD frustum)
		{
			if (!frustum.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			Vector3D[] cornerArray = frustum.CornerArray;
			foreach (Vector3D point in cornerArray)
			{
				if (Contains(point) == ContainmentType.Disjoint)
				{
					return ContainmentType.Intersects;
				}
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param>
		public ContainmentType Contains(Vector3D point)
		{
			if (!(Min.X > point.X) && !(point.X > Max.X) && !(Min.Y > point.Y) && !(point.Y > Max.Y) && !(Min.Z > point.Z) && !(point.Z > Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3D point, out ContainmentType result)
		{
			result = ((!(Min.X > point.X) && !(point.X > Max.X) && !(Min.Y > point.Y) && !(point.Y > Max.Y) && !(Min.Z > point.Z) && !(point.Z > Max.Z)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param>
		public ContainmentType Contains(BoundingSphereD sphere)
		{
			Vector3D.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3D result);
			Vector3D.DistanceSquared(ref sphere.Center, ref result, out double result2);
			double radius = sphere.Radius;
			if (result2 > radius * radius)
			{
				return ContainmentType.Disjoint;
			}
			if (!(Min.X + radius > sphere.Center.X) && !(sphere.Center.X > Max.X - radius) && !(Max.X - Min.X <= radius) && !(Min.Y + radius > sphere.Center.Y) && !(sphere.Center.Y > Max.Y - radius) && !(Max.Y - Min.Y <= radius) && !(Min.Z + radius > sphere.Center.Z) && !(sphere.Center.Z > Max.Z - radius) && !(Max.X - Min.X <= radius))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
		{
			Vector3D.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3D result2);
			Vector3D.DistanceSquared(ref sphere.Center, ref result2, out double result3);
			double radius = sphere.Radius;
			if (result3 > radius * radius)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				result = ((!(Min.X + radius > sphere.Center.X) && !(sphere.Center.X > Max.X - radius) && !(Max.X - Min.X <= radius) && !(Min.Y + radius > sphere.Center.Y) && !(sphere.Center.Y > Max.Y - radius) && !(Max.Y - Min.Y <= radius) && !(Min.Z + radius > sphere.Center.Z) && !(sphere.Center.Z > Max.Z - radius) && !(Max.X - Min.X <= radius)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		internal void SupportMapping(ref Vector3D v, out Vector3D result)
		{
			result.X = ((v.X >= 0.0) ? Max.X : Min.X);
			result.Y = ((v.Y >= 0.0) ? Max.Y : Min.Y);
			result.Z = ((v.Z >= 0.0) ? Max.Z : Min.Z);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="worldMatrix"></param>
		/// <returns></returns>
		public BoundingBoxD Translate(MatrixD worldMatrix)
		{
			Min += worldMatrix.Translation;
			Max += worldMatrix.Translation;
			return this;
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="vctTranlsation"></param>
		/// <returns></returns>
		public BoundingBoxD Translate(Vector3D vctTranlsation)
		{
			Min += vctTranlsation;
			Max += vctTranlsation;
			return this;
		}

		/// <summary>
		/// Transform this AABB by matrix.
		/// </summary>
		/// <param name="m">transformation matrix</param>
		/// <returns>transformed aabb</returns>
		public BoundingBoxD TransformSlow(MatrixD m)
		{
			return TransformSlow(ref m);
		}

		/// <summary>
		/// Transform this AABB by matrix.
		/// </summary>
		/// <param name="m">transformation matrix</param>
		/// <returns>transformed aabb</returns>
		public unsafe BoundingBoxD TransformSlow(ref MatrixD worldMatrix)
		{
			BoundingBoxD result = CreateInvalid();
			Vector3D* ptr = stackalloc Vector3D[8];
			GetCornersUnsafe(ptr);
			for (int i = 0; i < 8; i++)
			{
				Vector3D point = Vector3D.Transform(ptr[i], worldMatrix);
				result = result.Include(ref point);
			}
			return result;
		}

		/// <summary>
		/// Transform this AABB by matrix. Matrix has to be only rotation and translation.
		/// </summary>
		/// <param name="m">transformation matrix</param>
		/// <returns>transformed aabb</returns>
		public BoundingBoxD TransformFast(MatrixD m)
		{
			BoundingBoxD bb = CreateInvalid();
			TransformFast(ref m, ref bb);
			return bb;
		}

		/// <summary>
		/// Transform this AABB by matrix. Matrix has to be only rotation and translation.
		/// </summary>
		/// <param name="m">transformation matrix</param>
		/// <returns>transformed aabb</returns>
		public BoundingBoxD TransformFast(ref MatrixD m)
		{
			BoundingBoxD bb = CreateInvalid();
			TransformFast(ref m, ref bb);
			return bb;
		}

		/// <summary>
		/// Transform this AABB by matrix. Matrix has to be only rotation and translation.
		/// </summary>
		/// <param name="m">transformation matrix</param>
		/// <param name="bb">output transformed aabb</param>
		public void TransformFast(ref MatrixD m, ref BoundingBoxD bb)
		{
			bb.Min = (bb.Max = m.Translation);
			Vector3D min = m.Right * Min.X;
			Vector3D max = m.Right * Max.X;
			Vector3D.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
			min = m.Up * Min.Y;
			max = m.Up * Max.Y;
			Vector3D.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
			min = m.Backward * Min.Z;
			max = m.Backward * Max.Z;
			Vector3D.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
		}

		/// <summary>
		/// return expanded aabb (aabb include point)
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public BoundingBoxD Include(ref Vector3D point)
		{
			Min.X = Math.Min(point.X, Min.X);
			Min.Y = Math.Min(point.Y, Min.Y);
			Min.Z = Math.Min(point.Z, Min.Z);
			Max.X = Math.Max(point.X, Max.X);
			Max.Y = Math.Max(point.Y, Max.Y);
			Max.Z = Math.Max(point.Z, Max.Z);
			return this;
		}

		public BoundingBoxD Include(Vector3D point)
		{
			return Include(ref point);
		}

		public BoundingBoxD Include(Vector3D p0, Vector3D p1, Vector3D p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBoxD Include(ref Vector3D p0, ref Vector3D p1, ref Vector3D p2)
		{
			Include(ref p0);
			Include(ref p1);
			Include(ref p2);
			return this;
		}

		/// <summary>
		/// return expanded aabb (aabb include aabb)
		/// </summary>
		/// <param name="box"></param>
		/// <returns></returns>
		public BoundingBoxD Include(ref BoundingBoxD box)
		{
			Min = Vector3D.Min(Min, box.Min);
			Max = Vector3D.Max(Max, box.Max);
			return this;
		}

		public BoundingBoxD Include(BoundingBoxD box)
		{
			return Include(ref box);
		}

		public void Include(ref LineD line)
		{
			Include(ref line.From);
			Include(ref line.To);
		}

		public BoundingBoxD Include(BoundingSphereD sphere)
		{
			return Include(ref sphere);
		}

		public BoundingBoxD Include(ref BoundingSphereD sphere)
		{
			Vector3D value = new Vector3D(sphere.Radius);
			Vector3D value2 = sphere.Center;
			Vector3D value3 = sphere.Center;
			Vector3D.Subtract(ref value2, ref value, out value2);
			Vector3D.Add(ref value3, ref value, out value3);
			Include(ref value2);
			Include(ref value3);
			return this;
		}

		public unsafe BoundingBoxD Include(ref BoundingFrustumD frustum)
		{
			Vector3D* ptr = stackalloc Vector3D[8];
			frustum.GetCornersUnsafe(ptr);
			Include(ref *ptr);
			Include(ref ptr[1]);
			Include(ref ptr[2]);
			Include(ref ptr[3]);
			Include(ref ptr[4]);
			Include(ref ptr[5]);
			Include(ref ptr[6]);
			Include(ref ptr[7]);
			return this;
		}

		public static BoundingBoxD CreateInvalid()
		{
			return new BoundingBoxD(new Vector3D(double.MaxValue), new Vector3D(double.MinValue));
		}

		public double ProjectedArea(Vector3D viewDir)
		{
			Vector3D vector3D = Max - Min;
			Vector3D v = new Vector3D(vector3D.Y, vector3D.Z, vector3D.X) * new Vector3D(vector3D.Z, vector3D.X, vector3D.Y);
			return Vector3D.Abs(viewDir).Dot(v);
		}

		public BoundingBoxD Inflate(double size)
		{
			Max += new Vector3D(size);
			Min -= new Vector3D(size);
			return this;
		}

		public BoundingBoxD Inflate(Vector3D size)
		{
			Max += size;
			Min -= size;
			return this;
		}

		public BoundingBoxD GetInflated(double size)
		{
			BoundingBoxD result = this;
			result.Inflate(size);
			return result;
		}

		public BoundingBoxD GetInflated(Vector3 size)
		{
			BoundingBoxD result = this;
			result.Inflate(size);
			return result;
		}

		public BoundingBoxD GetInflated(Vector3D size)
		{
			BoundingBoxD result = this;
			result.Inflate(size);
			return result;
		}

		public static explicit operator BoundingBox(BoundingBoxD b)
		{
			return new BoundingBox(b.Min, b.Max);
		}

		public void InflateToMinimum(Vector3D minimumSize)
		{
			Vector3D center = Center;
			if (Size.X < minimumSize.X)
			{
				Min.X = center.X - minimumSize.X * 0.5;
				Max.X = center.X + minimumSize.X * 0.5;
			}
			if (Size.Y < minimumSize.Y)
			{
				Min.Y = center.Y - minimumSize.Y * 0.5;
				Max.Y = center.Y + minimumSize.Y * 0.5;
			}
			if (Size.Z < minimumSize.Z)
			{
				Min.Z = center.Z - minimumSize.Z * 0.5;
				Max.Z = center.Z + minimumSize.Z * 0.5;
			}
		}

		public void InflateToMinimum(double minimumSize)
		{
			Vector3D center = Center;
			if (Size.X < minimumSize)
			{
				Min.X = center.X - minimumSize * 0.5;
				Max.X = center.X + minimumSize * 0.5;
			}
			if (Size.Y < minimumSize)
			{
				Min.Y = center.Y - minimumSize * 0.5;
				Max.Y = center.Y + minimumSize * 0.5;
			}
			if (Size.Z < minimumSize)
			{
				Min.Z = center.Z - minimumSize * 0.5;
				Max.Z = center.Z + minimumSize * 0.5;
			}
		}

		public BoundingBoxD Round(int decimals)
		{
			return new BoundingBoxD(Vector3D.Round(Min, decimals), Vector3D.Round(Max, decimals));
		}

		public BoundingBoxI Round()
		{
			return new BoundingBoxI(Vector3D.Round(Min), Vector3D.Round(Max));
		}

		[Conditional("DEBUG")]
		public void AssertIsValid()
		{
		}
	}
}
