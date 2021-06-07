using ProtoBuf;
using System;
using System.Collections.Generic;
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
	public struct BoundingBox : IEquatable<BoundingBox>
	{
		public class ComparerType : IEqualityComparer<BoundingBoxD>
		{
			public bool Equals(BoundingBoxD x, BoundingBoxD y)
			{
				if (x.Min == y.Min)
				{
					return x.Max == y.Max;
				}
				return false;
			}

			public int GetHashCode(BoundingBoxD obj)
			{
				return obj.Min.GetHashCode() ^ obj.Max.GetHashCode();
			}
		}

		protected class VRageMath_BoundingBox_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBox, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox owner, in Vector3 value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox owner, out Vector3 value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBox_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBox, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox owner, in Vector3 value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox owner, out Vector3 value)
			{
				value = owner.Max;
			}
		}

		protected class VRageMath_BoundingBox_003C_003ECorners_003C_003EAccessor : IMemberAccessor<BoundingBox, BoxCornerEnumerator>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBox owner, in BoxCornerEnumerator value)
			{
				owner.Corners = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBox owner, out BoxCornerEnumerator value)
			{
				value = owner.Corners;
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
		public Vector3 Min;

		/// <summary>
		/// The maximum point the BoundingBox contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector3 Max;

		public static readonly ComparerType Comparer = new ComparerType();

		public BoxCornerEnumerator Corners
		{
			get
			{
				return new BoxCornerEnumerator(Min, Max);
			}
			set
			{
			}
		}

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector3 Center => (Min + Max) * 0.5f;

		public Vector3 HalfExtents => (Max - Min) * 0.5f;

		public Vector3 Extents => Max - Min;

		public float Width => Max.X - Min.X;

		public float Height => Max.Y - Min.Y;

		public float Depth => Max.Z - Min.Z;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector3 Size => Vector3.Abs(Max - Min);

		/// <summary>
		/// Matrix of AABB, respection center and size
		/// </summary>
		public Matrix Matrix
		{
			get
			{
				Vector3 position = Center;
				Vector3 scale = Size;
				Matrix.CreateTranslation(ref position, out Matrix result);
				Matrix.Rescale(ref result, ref scale);
				return result;
			}
		}

		/// <summary>
		/// return perimeter of edges
		/// </summary>
		/// <returns></returns>
		public float Perimeter
		{
			get
			{
				float num = Max.X - Min.X;
				float num2 = Max.Y - Min.Y;
				float num3 = Max.Z - Min.Z;
				return 4f * (num + num2 + num3);
			}
		}

		/// <summary>
		/// Creates an instance of BoundingBox.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBox includes.</param><param name="max">The maximum point the BoundingBox includes.</param>
		public BoundingBox(Vector3 min, Vector3 max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// Creates an instance of BoundingBox from BoundingBoxD (helper for transformed BBs)
		/// </summary>
		/// <param name="bbd"></param>
		public BoundingBox(BoundingBoxD bbd)
		{
			Min = bbd.Min;
			Max = bbd.Max;
		}

		public BoundingBox(BoundingBoxI bbd)
		{
			Min = bbd.Min;
			Max = bbd.Max;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are equal.
		/// </summary>
		/// <param name="a">BoundingBox to compare.</param><param name="b">BoundingBox to compare.</param>
		public static bool operator ==(BoundingBox a, BoundingBox b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBox are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBox a, BoundingBox b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBox. ALLOCATION!
		/// </summary>
		public Vector3[] GetCorners()
		{
			return new Vector3[8]
			{
				new Vector3(Min.X, Max.Y, Max.Z),
				new Vector3(Max.X, Max.Y, Max.Z),
				new Vector3(Max.X, Min.Y, Max.Z),
				new Vector3(Min.X, Min.Y, Max.Z),
				new Vector3(Min.X, Max.Y, Min.Z),
				new Vector3(Max.X, Max.Y, Min.Z),
				new Vector3(Max.X, Min.Y, Min.Z),
				new Vector3(Min.X, Min.Y, Min.Z)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
		public void GetCorners(Vector3[] corners)
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
		public unsafe void GetCornersUnsafe(Vector3* corners)
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
		public bool Equals(BoundingBox other)
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
			if (obj is BoundingBox)
			{
				return Equals((BoundingBox)obj);
			}
			return false;
		}

		public bool Equals(BoundingBox other, float epsilon)
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
		public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
		{
			BoundingBox result = default(BoundingBox);
			Vector3.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector3.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
		/// </summary>
		/// <param name="original">One of the BoundingBox instances to contain.</param><param name="additional">One of the BoundingBox instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox.</param>
		public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
		{
			Vector3.Min(ref original.Min, ref additional.Min, out Vector3 result2);
			Vector3.Max(ref original.Max, ref additional.Max, out Vector3 result3);
			result.Min = result2;
			result.Max = result3;
		}

		/// <summary>
		/// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to contain.</param>
		public static BoundingBox CreateFromSphere(BoundingSphere sphere)
		{
			BoundingBox result = default(BoundingBox);
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
		public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
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
		public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector3 value = new Vector3(float.MaxValue);
			Vector3 value2 = new Vector3(float.MinValue);
			foreach (Vector3 point in points)
			{
				Vector3 value3 = point;
				Vector3.Min(ref value, ref value3, out value);
				Vector3.Max(ref value2, ref value3, out value2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBox(value, value2);
		}

		public static BoundingBox CreateFromHalfExtent(Vector3 center, float halfExtent)
		{
			return CreateFromHalfExtent(center, new Vector3(halfExtent));
		}

		public static BoundingBox CreateFromHalfExtent(Vector3 center, Vector3 halfExtent)
		{
			return new BoundingBox(center - halfExtent, center + halfExtent);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBox Intersect(BoundingBox box)
		{
			BoundingBox result = default(BoundingBox);
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
		public bool Intersects(BoundingBox box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBox box)
		{
			if ((double)Max.X >= (double)box.Min.X && (double)Min.X <= (double)box.Max.X && (double)Max.Y >= (double)box.Min.Y && (double)Min.Y <= (double)box.Max.Y)
			{
				if ((double)Max.Z >= (double)box.Min.Z)
				{
					return (double)Min.Z <= (double)box.Max.Z;
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects another BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox box, out bool result)
		{
			result = false;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y) && !((double)Max.Z < (double)box.Min.Z) && !((double)Min.Z > (double)box.Max.Z))
			{
				result = true;
			}
		}

		public bool IntersectsTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			return IntersectsTriangle(ref v0, ref v1, ref v2);
		}

		public bool IntersectsTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
		{
			Vector3.Min(ref v0, ref v1, out Vector3 result);
			Vector3.Min(ref result, ref v2, out result);
			Vector3.Max(ref v0, ref v1, out Vector3 result2);
			Vector3.Max(ref result2, ref v2, out result2);
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
			Vector3 vector = v1 - v0;
			Vector3 vector2 = v2 - v1;
			Vector3.Cross(ref vector, ref vector2, out Vector3 result3);
			Vector3.Dot(ref v0, ref result3, out float result4);
			Plane plane = new Plane(result3, 0f - result4);
			Intersects(ref plane, out PlaneIntersectionType result5);
			switch (result5)
			{
			case PlaneIntersectionType.Back:
				return false;
			case PlaneIntersectionType.Front:
				return false;
			default:
			{
				Vector3 center = Center;
				Vector3 halfExtents = new BoundingBox(Min - center, Max - center).HalfExtents;
				Vector3 vector3 = v0 - v2;
				Vector3 vector4 = v0 - center;
				Vector3 vector5 = v1 - center;
				Vector3 vector6 = v2 - center;
				float num = halfExtents.Y * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.Y);
				float val = vector4.Z * vector5.Y - vector4.Y * vector5.Z;
				float val2 = vector6.Z * vector.Y - vector6.Y * vector.Z;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.X);
				val = vector4.X * vector5.Z - vector4.Z * vector5.X;
				val2 = vector6.X * vector.Z - vector6.Z * vector.X;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Y) + halfExtents.Y * Math.Abs(vector.X);
				val = vector4.Y * vector5.X - vector4.X * vector5.Y;
				val2 = vector6.Y * vector.X - vector6.X * vector.Y;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.Y);
				float val3 = vector5.Z * vector6.Y - vector5.Y * vector6.Z;
				val = vector4.Z * vector2.Y - vector4.Y * vector2.Z;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.X);
				val3 = vector5.X * vector6.Z - vector5.Z * vector6.X;
				val = vector4.X * vector2.Z - vector4.Z * vector2.X;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Y) + halfExtents.Y * Math.Abs(vector2.X);
				val3 = vector5.Y * vector6.X - vector5.X * vector6.Y;
				val = vector4.Y * vector2.X - vector4.X * vector2.Y;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector3.Z) + halfExtents.Z * Math.Abs(vector3.Y);
				val2 = vector6.Z * vector4.Y - vector6.Y * vector4.Z;
				val3 = vector5.Z * vector3.Y - vector5.Y * vector3.Z;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3.Z) + halfExtents.Z * Math.Abs(vector3.X);
				val2 = vector6.X * vector4.Z - vector6.Z * vector4.X;
				val3 = vector5.X * vector3.Z - vector5.Z * vector3.X;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3.Y) + halfExtents.Y * Math.Abs(vector3.X);
				val2 = vector6.Y * vector4.X - vector6.X * vector4.Y;
				val3 = vector5.Y * vector3.X - vector5.X * vector3.Y;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
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
		public bool Intersects(BoundingFrustum frustum)
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
		public PlaneIntersectionType Intersects(Plane plane)
		{
			Vector3 vector = default(Vector3);
			vector.X = (((double)plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector.Y = (((double)plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector.Z = (((double)plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			if ((double)(plane.Normal.X * vector.X + plane.Normal.Y * vector.Y + plane.Normal.Z * vector.Z + plane.D) > 0.0)
			{
				return PlaneIntersectionType.Front;
			}
			Vector3 vector2 = default(Vector3);
			vector2.X = (((double)plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector2.Y = (((double)plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector2.Z = (((double)plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			if (!((double)(plane.Normal.X * vector2.X + plane.Normal.Y * vector2.Y + plane.Normal.Z * vector2.Z + plane.D) < 0.0))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingBox intersects the Plane.</param>
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			Vector3 vector = default(Vector3);
			vector.X = (((double)plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector.Y = (((double)plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector.Z = (((double)plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			if ((double)(plane.Normal.X * vector.X + plane.Normal.Y * vector.Y + plane.Normal.Z * vector.Z + plane.D) > 0.0)
			{
				result = PlaneIntersectionType.Front;
			}
			Vector3 vector2 = default(Vector3);
			vector2.X = (((double)plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector2.Y = (((double)plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector2.Z = (((double)plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			result = (((double)(plane.Normal.X * vector2.X + plane.Normal.Y * vector2.Y + plane.Normal.Z * vector2.Z + plane.D) < 0.0) ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting);
		}

		public bool Intersects(Line line, out float distance)
		{
			distance = 0f;
			float? num = Intersects(new Ray(line.From, line.Direction));
			if (!num.HasValue)
			{
				return false;
			}
			if (num.Value < 0f)
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

		/// <summary>
		/// Checks whether the current BoundingBox intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param>
		public float? Intersects(Ray ray)
		{
			float num = 0f;
			float num2 = float.MaxValue;
			if ((double)Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.X < (double)Min.X || (double)ray.Position.X > (double)Max.X)
				{
					return null;
				}
			}
			else
			{
				float num3 = 1f / ray.Direction.X;
				float num4 = (Min.X - ray.Position.X) * num3;
				float num5 = (Max.X - ray.Position.X) * num3;
				if ((double)num4 > (double)num5)
				{
					float num6 = num4;
					num4 = num5;
					num5 = num6;
				}
				num = MathHelper.Max(num4, num);
				num2 = MathHelper.Min(num5, num2);
				if ((double)num > (double)num2)
				{
					return null;
				}
			}
			if ((double)Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.Y < (double)Min.Y || (double)ray.Position.Y > (double)Max.Y)
				{
					return null;
				}
			}
			else
			{
				float num7 = 1f / ray.Direction.Y;
				float num8 = (Min.Y - ray.Position.Y) * num7;
				float num9 = (Max.Y - ray.Position.Y) * num7;
				if ((double)num8 > (double)num9)
				{
					float num10 = num8;
					num8 = num9;
					num9 = num10;
				}
				num = MathHelper.Max(num8, num);
				num2 = MathHelper.Min(num9, num2);
				if ((double)num > (double)num2)
				{
					return null;
				}
			}
			if ((double)Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.Z < (double)Min.Z || (double)ray.Position.Z > (double)Max.Z)
				{
					return null;
				}
			}
			else
			{
				float num11 = 1f / ray.Direction.Z;
				float num12 = (Min.Z - ray.Position.Z) * num11;
				float num13 = (Max.Z - ray.Position.Z) * num11;
				if ((double)num12 > (double)num13)
				{
					float num14 = num12;
					num12 = num13;
					num13 = num14;
				}
				num = MathHelper.Max(num12, num);
				float num15 = MathHelper.Min(num13, num2);
				if ((double)num > (double)num15)
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
		public void Intersects(ref Ray ray, out float? result)
		{
			result = null;
			float num = 0f;
			float num2 = float.MaxValue;
			if ((double)Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.X < (double)Min.X || (double)ray.Position.X > (double)Max.X)
				{
					return;
				}
			}
			else
			{
				float num3 = 1f / ray.Direction.X;
				float num4 = (Min.X - ray.Position.X) * num3;
				float num5 = (Max.X - ray.Position.X) * num3;
				if ((double)num4 > (double)num5)
				{
					float num6 = num4;
					num4 = num5;
					num5 = num6;
				}
				num = MathHelper.Max(num4, num);
				num2 = MathHelper.Min(num5, num2);
				if ((double)num > (double)num2)
				{
					return;
				}
			}
			if ((double)Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.Y < (double)Min.Y || (double)ray.Position.Y > (double)Max.Y)
				{
					return;
				}
			}
			else
			{
				float num7 = 1f / ray.Direction.Y;
				float num8 = (Min.Y - ray.Position.Y) * num7;
				float num9 = (Max.Y - ray.Position.Y) * num7;
				if ((double)num8 > (double)num9)
				{
					float num10 = num8;
					num8 = num9;
					num9 = num10;
				}
				num = MathHelper.Max(num8, num);
				num2 = MathHelper.Min(num9, num2);
				if ((double)num > (double)num2)
				{
					return;
				}
			}
			if ((double)Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
			{
				if ((double)ray.Position.Z < (double)Min.Z || (double)ray.Position.Z > (double)Max.Z)
				{
					return;
				}
			}
			else
			{
				float num11 = 1f / ray.Direction.Z;
				float num12 = (Min.Z - ray.Position.Z) * num11;
				float num13 = (Max.Z - ray.Position.Z) * num11;
				if ((double)num12 > (double)num13)
				{
					float num14 = num12;
					num12 = num13;
					num13 = num14;
				}
				num = MathHelper.Max(num12, num);
				float num15 = MathHelper.Min(num13, num2);
				if ((double)num > (double)num15)
				{
					return;
				}
			}
			result = num;
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param>
		public bool Intersects(BoundingSphere sphere)
		{
			return Intersects(ref sphere);
		}

		/// <summary>
		/// Checks whether the current BoundingBox intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox and BoundingSphere intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3 result2);
			Vector3.DistanceSquared(ref sphere.Center, ref result2, out float result3);
			result = ((double)result3 <= (double)sphere.Radius * (double)sphere.Radius);
		}

		public bool Intersects(ref BoundingSphere sphere)
		{
			Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3 result);
			Vector3.DistanceSquared(ref sphere.Center, ref result, out float result2);
			return result2 <= sphere.Radius * sphere.Radius;
		}

		public bool Intersects(ref BoundingSphereD sphere)
		{
			Vector3 value = sphere.Center;
			Vector3.Clamp(ref value, ref Min, ref Max, out Vector3 result);
			Vector3.DistanceSquared(ref value, ref result, out float result2);
			return (double)result2 <= sphere.Radius * sphere.Radius;
		}

		public float Distance(Vector3 point)
		{
			if (Contains(point) == ContainmentType.Contains)
			{
				return 0f;
			}
			return Vector3.Distance(Vector3.Clamp(point, Min, Max), point);
		}

		public float DistanceSquared(Vector3 point)
		{
			if (Contains(point) == ContainmentType.Contains)
			{
				return 0f;
			}
			return Vector3.DistanceSquared(Vector3.Clamp(point, Min, Max), point);
		}

		/// <summary>
		/// Tests whether the BoundingBox contains another BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param>
		public ContainmentType Contains(BoundingBox box)
		{
			if ((double)Max.X < (double)box.Min.X || (double)Min.X > (double)box.Max.X || (double)Max.Y < (double)box.Min.Y || (double)Min.Y > (double)box.Max.Y || (double)Max.Z < (double)box.Min.Z || (double)Min.Z > (double)box.Max.Z)
			{
				return ContainmentType.Disjoint;
			}
			if (!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y) && !((double)Min.Z > (double)box.Min.Z) && !((double)box.Max.Z > (double)Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y) && !((double)Max.Z < (double)box.Min.Z) && !((double)Min.Z > (double)box.Max.Z))
			{
				result = ((!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y) && !((double)Min.Z > (double)box.Min.Z) && !((double)box.Max.Z > (double)Max.Z)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to test for overlap.</param>
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			if (!frustum.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			Vector3[] cornerArray = frustum.cornerArray;
			foreach (Vector3 point in cornerArray)
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
		public ContainmentType Contains(Vector3 point)
		{
			if (!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y) && !((double)Min.Z > (double)point.Z) && !((double)point.Z > (double)Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		public ContainmentType Contains(Vector3D point)
		{
			if (!((double)Min.X > point.X) && !(point.X > (double)Max.X) && !((double)Min.Y > point.Y) && !(point.Y > (double)Max.Y) && !((double)Min.Z > point.Z) && !(point.Z > (double)Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			result = ((!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y) && !((double)Min.Z > (double)point.Z) && !((double)point.Z > (double)Max.Z)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param>
		public ContainmentType Contains(BoundingSphere sphere)
		{
			Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3 result);
			Vector3.DistanceSquared(ref sphere.Center, ref result, out float result2);
			float radius = sphere.Radius;
			if ((double)result2 > (double)radius * (double)radius)
			{
				return ContainmentType.Disjoint;
			}
			if (!((double)Min.X + (double)radius > (double)sphere.Center.X) && !((double)sphere.Center.X > (double)Max.X - (double)radius) && !((double)Max.X - (double)Min.X <= (double)radius) && !((double)Min.Y + (double)radius > (double)sphere.Center.Y) && !((double)sphere.Center.Y > (double)Max.Y - (double)radius) && !((double)Max.Y - (double)Min.Y <= (double)radius) && !((double)Min.Z + (double)radius > (double)sphere.Center.Z) && !((double)sphere.Center.Z > (double)Max.Z - (double)radius) && !((double)Max.X - (double)Min.X <= (double)radius))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Tests whether the BoundingBox contains a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			Vector3.Clamp(ref sphere.Center, ref Min, ref Max, out Vector3 result2);
			Vector3.DistanceSquared(ref sphere.Center, ref result2, out float result3);
			float radius = sphere.Radius;
			if ((double)result3 > (double)radius * (double)radius)
			{
				result = ContainmentType.Disjoint;
			}
			else
			{
				result = ((!((double)Min.X + (double)radius > (double)sphere.Center.X) && !((double)sphere.Center.X > (double)Max.X - (double)radius) && !((double)Max.X - (double)Min.X <= (double)radius) && !((double)Min.Y + (double)radius > (double)sphere.Center.Y) && !((double)sphere.Center.Y > (double)Max.Y - (double)radius) && !((double)Max.Y - (double)Min.Y <= (double)radius) && !((double)Min.Z + (double)radius > (double)sphere.Center.Z) && !((double)sphere.Center.Z > (double)Max.Z - (double)radius) && !((double)Max.X - (double)Min.X <= (double)radius)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		internal void SupportMapping(ref Vector3 v, out Vector3 result)
		{
			result.X = (((double)v.X >= 0.0) ? Max.X : Min.X);
			result.Y = (((double)v.Y >= 0.0) ? Max.Y : Min.Y);
			result.Z = (((double)v.Z >= 0.0) ? Max.Z : Min.Z);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="worldMatrix"></param>
		/// <returns></returns>
		public BoundingBox Translate(Matrix worldMatrix)
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
		public BoundingBox Translate(Vector3 vctTranlsation)
		{
			Min += vctTranlsation;
			Max += vctTranlsation;
			return this;
		}

		public BoundingBox Transform(Matrix worldMatrix)
		{
			return Transform(ref worldMatrix);
		}

		public BoundingBoxD Transform(MatrixD worldMatrix)
		{
			return Transform(ref worldMatrix);
		}

		public BoundingBox Transform(ref Matrix m)
		{
			BoundingBox bb = CreateInvalid();
			Transform(ref m, ref bb);
			return bb;
		}

		public void Transform(ref Matrix m, ref BoundingBox bb)
		{
			bb.Min = (bb.Max = m.Translation);
			Vector3 min = m.Right * Min.X;
			Vector3 max = m.Right * Max.X;
			Vector3.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
			min = m.Up * Min.Y;
			max = m.Up * Max.Y;
			Vector3.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
			min = m.Backward * Min.Z;
			max = m.Backward * Max.Z;
			Vector3.MinMax(ref min, ref max);
			bb.Min += min;
			bb.Max += max;
		}

		public BoundingBoxD Transform(ref MatrixD m)
		{
			BoundingBoxD bb = BoundingBoxD.CreateInvalid();
			Transform(ref m, ref bb);
			return bb;
		}

		public void Transform(ref MatrixD m, ref BoundingBoxD bb)
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
		public BoundingBox Include(ref Vector3 point)
		{
			Min.X = Math.Min(point.X, Min.X);
			Min.Y = Math.Min(point.Y, Min.Y);
			Min.Z = Math.Min(point.Z, Min.Z);
			Max.X = Math.Max(point.X, Max.X);
			Max.Y = Math.Max(point.Y, Max.Y);
			Max.Z = Math.Max(point.Z, Max.Z);
			return this;
		}

		public BoundingBox GetIncluded(Vector3 point)
		{
			BoundingBox result = this;
			result.Include(point);
			return result;
		}

		public BoundingBox Include(Vector3 point)
		{
			return Include(ref point);
		}

		public BoundingBox Include(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBox Include(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
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
		public BoundingBox Include(ref BoundingBox box)
		{
			Min = Vector3.Min(Min, box.Min);
			Max = Vector3.Max(Max, box.Max);
			return this;
		}

		public BoundingBox Include(BoundingBox box)
		{
			return Include(ref box);
		}

		public void Include(ref Line line)
		{
			Include(ref line.From);
			Include(ref line.To);
		}

		public BoundingBox Include(BoundingSphere sphere)
		{
			return Include(ref sphere);
		}

		public BoundingBox Include(ref BoundingSphere sphere)
		{
			Vector3 value = new Vector3(sphere.Radius);
			Vector3 value2 = sphere.Center;
			Vector3 value3 = sphere.Center;
			Vector3.Subtract(ref value2, ref value, out value2);
			Vector3.Add(ref value3, ref value, out value3);
			Include(ref value2);
			Include(ref value3);
			return this;
		}

		public unsafe BoundingBox Include(ref BoundingFrustum frustum)
		{
			Vector3* ptr = stackalloc Vector3[8];
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

		public static BoundingBox CreateInvalid()
		{
			BoundingBox result = default(BoundingBox);
			Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			result.Min = min;
			result.Max = max;
			return result;
		}

		public float SurfaceArea()
		{
			Vector3 vector = Max - Min;
			return 2f * (vector.X * vector.Y + vector.X * vector.Z + vector.Y * vector.Z);
		}

		public float Volume()
		{
			Vector3 vector = Max - Min;
			return vector.X * vector.Y * vector.Z;
		}

		public float ProjectedArea(Vector3 viewDir)
		{
			Vector3 vector = Max - Min;
			Vector3 v = new Vector3(vector.Y, vector.Z, vector.X) * new Vector3(vector.Z, vector.X, vector.Y);
			return Vector3.Abs(viewDir).Dot(v);
		}

		public void Inflate(float size)
		{
			Max += new Vector3(size);
			Min -= new Vector3(size);
		}

		public void Inflate(Vector3 size)
		{
			Max += size;
			Min -= size;
		}

		public void InflateToMinimum(Vector3 minimumSize)
		{
			Vector3 center = Center;
			if (Size.X < minimumSize.X)
			{
				Min.X = center.X - minimumSize.X * 0.5f;
				Max.X = center.X + minimumSize.X * 0.5f;
			}
			if (Size.Y < minimumSize.Y)
			{
				Min.Y = center.Y - minimumSize.Y * 0.5f;
				Max.Y = center.Y + minimumSize.Y * 0.5f;
			}
			if (Size.Z < minimumSize.Z)
			{
				Min.Z = center.Z - minimumSize.Z * 0.5f;
				Max.Z = center.Z + minimumSize.Z * 0.5f;
			}
		}

		public void Scale(Vector3 scale)
		{
			Vector3 center = Center;
			Vector3 value = HalfExtents * scale;
			Min = center - value;
			Max = center + value;
		}

		public BoundingBox Round(int decimals)
		{
			return new BoundingBox(Vector3.Round(Min, decimals), Vector3.Round(Max, decimals));
		}

		public BoundingBoxI Round()
		{
			return new BoundingBoxI(Vector3D.Round(Min), Vector3D.Round(Max));
		}
	}
}
