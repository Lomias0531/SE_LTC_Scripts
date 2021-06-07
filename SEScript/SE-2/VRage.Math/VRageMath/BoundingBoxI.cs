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
	public struct BoundingBoxI : IEquatable<BoundingBoxI>
	{
		protected class VRageMath_BoundingBoxI_003C_003EMin_003C_003EAccessor : IMemberAccessor<BoundingBoxI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBoxI owner, in Vector3I value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBoxI owner, out Vector3I value)
			{
				value = owner.Min;
			}
		}

		protected class VRageMath_BoundingBoxI_003C_003EMax_003C_003EAccessor : IMemberAccessor<BoundingBoxI, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingBoxI owner, in Vector3I value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingBoxI owner, out Vector3I value)
			{
				value = owner.Max;
			}
		}

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingBoxI.
		/// </summary>
		public const int CornerCount = 8;

		/// <summary>
		/// The minimum point the BoundingBoxI contains.
		/// </summary>
		[ProtoMember(1)]
		public Vector3I Min;

		/// <summary>
		/// The maximum point the BoundingBoxI contains.
		/// </summary>
		[ProtoMember(4)]
		public Vector3I Max;

		/// <summary>
		/// Calculates center
		/// </summary>
		public Vector3I Center => (Min + Max) / 2;

		public Vector3I HalfExtents => (Max - Min) / 2;

		/// <summary>
		/// Size
		/// </summary>
		/// <returns></returns>
		public Vector3I Size => Max - Min;

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

		public bool IsValid
		{
			get
			{
				if (Min.X <= Max.X && Min.Y <= Max.Y)
				{
					return Min.Z <= Max.Z;
				}
				return false;
			}
		}

		/// <summary>
		/// Creates an instance of BoundingBoxI.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBoxI includes.</param><param name="max">The maximum point the BoundingBoxI includes.</param>
		public BoundingBoxI(BoundingBox box)
		{
			Min = new Vector3I(box.Min);
			Max = new Vector3I(box.Max);
		}

		/// <summary>
		/// Creates an instance of BoundingBoxI.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBoxI includes.</param><param name="max">The maximum point the BoundingBoxI includes.</param>
		public BoundingBoxI(Vector3I min, Vector3I max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// Creates an instance of BoundingBoxI.
		/// </summary>
		/// <param name="min">The minimum point the BoundingBoxI includes.</param><param name="max">The maximum point the BoundingBoxI includes.</param>
		public BoundingBoxI(int min, int max)
		{
			Min = new Vector3I(min);
			Max = new Vector3I(max);
		}

		public static explicit operator BoundingBoxI(BoundingBoxD box)
		{
			return new BoundingBoxI((Vector3I)box.Min, (Vector3I)box.Max);
		}

		public static explicit operator BoundingBoxI(BoundingBox box)
		{
			return new BoundingBoxI((Vector3I)box.Min, (Vector3I)box.Max);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBoxI are equal.
		/// </summary>
		/// <param name="a">BoundingBoxI to compare.</param><param name="b">BoundingBoxI to compare.</param>
		public static bool operator ==(BoundingBoxI a, BoundingBoxI b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingBoxI are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(BoundingBoxI a, BoundingBoxI b)
		{
			if (!(a.Min != b.Min))
			{
				return a.Max != b.Max;
			}
			return true;
		}

		public static implicit operator BoundingBox(BoundingBoxI box)
		{
			return new BoundingBox(box.Min, box.Max);
		}

		public static implicit operator BoundingBoxD(BoundingBoxI box)
		{
			return new BoundingBoxD(box.Min, box.Max);
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingBoxI.
		/// </summary>
		public Vector3I[] GetCorners()
		{
			return new Vector3I[8]
			{
				new Vector3I(Min.X, Max.Y, Max.Z),
				new Vector3I(Max.X, Max.Y, Max.Z),
				new Vector3I(Max.X, Min.Y, Max.Z),
				new Vector3I(Min.X, Min.Y, Max.Z),
				new Vector3I(Min.X, Max.Y, Min.Z),
				new Vector3I(Max.X, Max.Y, Min.Z),
				new Vector3I(Max.X, Min.Y, Min.Z),
				new Vector3I(Min.X, Min.Y, Min.Z)
			};
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBoxI.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3I points where the corners of the BoundingBoxI are written.</param>
		public void GetCorners(Vector3I[] corners)
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
		/// Gets the array of points that make up the corners of the BoundingBoxI.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3I points where the corners of the BoundingBoxI are written.</param>
		public unsafe void GetCornersUnsafe(Vector3I* corners)
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
		/// Determines whether two instances of BoundingBoxI are equal.
		/// </summary>
		/// <param name="other">The BoundingBoxI to compare with the current BoundingBoxI.</param>
		public bool Equals(BoundingBoxI other)
		{
			if (Min == other.Min)
			{
				return Max == other.Max;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of BoundingBoxI are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingBoxI.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingBoxI)
			{
				result = Equals((BoundingBoxI)obj);
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
		/// Returns a String that represents the current BoundingBoxI.
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
		/// Creates the smallest BoundingBoxI that contains the two specified BoundingBoxI instances.
		/// </summary>
		/// <param name="original">One of the BoundingBoxIs to contain.</param><param name="additional">One of the BoundingBoxIs to contain.</param>
		public static BoundingBoxI CreateMerged(BoundingBoxI original, BoundingBoxI additional)
		{
			BoundingBoxI result = default(BoundingBoxI);
			Vector3I.Min(ref original.Min, ref additional.Min, out result.Min);
			Vector3I.Max(ref original.Max, ref additional.Max, out result.Max);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBoxI that contains the two specified BoundingBoxI instances.
		/// </summary>
		/// <param name="original">One of the BoundingBoxI instances to contain.</param><param name="additional">One of the BoundingBoxI instances to contain.</param><param name="result">[OutAttribute] The created BoundingBoxI.</param>
		public static void CreateMerged(ref BoundingBoxI original, ref BoundingBoxI additional, out BoundingBoxI result)
		{
			Vector3I.Min(ref original.Min, ref additional.Min, out Vector3I result2);
			Vector3I.Max(ref original.Max, ref additional.Max, out Vector3I result3);
			result.Min = result2;
			result.Max = result3;
		}

		/// <summary>
		/// Creates the smallest BoundingBoxI that will contain the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to contain.</param>
		public static BoundingBoxI CreateFromSphere(BoundingSphere sphere)
		{
			BoundingBoxI result = default(BoundingBoxI);
			result.Min.X = (int)(sphere.Center.X - sphere.Radius);
			result.Min.Y = (int)(sphere.Center.Y - sphere.Radius);
			result.Min.Z = (int)(sphere.Center.Z - sphere.Radius);
			result.Max.X = (int)(sphere.Center.X + sphere.Radius);
			result.Max.Y = (int)(sphere.Center.Y + sphere.Radius);
			result.Max.Z = (int)(sphere.Center.Z + sphere.Radius);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingBoxI that will contain the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to contain.</param><param name="result">[OutAttribute] The created BoundingBoxI.</param>
		public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBoxI result)
		{
			result.Min.X = (int)(sphere.Center.X - sphere.Radius);
			result.Min.Y = (int)(sphere.Center.Y - sphere.Radius);
			result.Min.Z = (int)(sphere.Center.Z - sphere.Radius);
			result.Max.X = (int)(sphere.Center.X + sphere.Radius);
			result.Max.Y = (int)(sphere.Center.Y + sphere.Radius);
			result.Max.Z = (int)(sphere.Center.Z + sphere.Radius);
		}

		/// <summary>
		/// Creates the smallest BoundingBoxI that will contain a group of points.
		/// </summary>
		/// <param name="points">A list of points the BoundingBoxI should contain.</param>
		public static BoundingBoxI CreateFromPoints(IEnumerable<Vector3I> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException();
			}
			bool flag = false;
			Vector3I value = new Vector3I(int.MaxValue);
			Vector3I value2 = new Vector3I(int.MinValue);
			foreach (Vector3I point in points)
			{
				Vector3I value3 = point;
				Vector3I.Min(ref value, ref value3, out value);
				Vector3I.Max(ref value2, ref value3, out value2);
				flag = true;
			}
			if (!flag)
			{
				throw new ArgumentException();
			}
			return new BoundingBoxI(value, value2);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public void IntersectWith(ref BoundingBoxI box)
		{
			Min.X = Math.Max(Min.X, box.Min.X);
			Min.Y = Math.Max(Min.Y, box.Min.Y);
			Min.Z = Math.Max(Min.Z, box.Min.Z);
			Max.X = Math.Min(Max.X, box.Max.X);
			Max.Y = Math.Min(Max.Y, box.Max.Y);
			Max.Z = Math.Min(Max.Z, box.Max.Z);
		}

		/// <summary>
		/// Returns bounding box which is intersection of this and box
		/// It's called 'Prunik'
		/// Result is invalid box when there's no intersection (Min &gt; Max)
		/// </summary>
		public BoundingBoxI Intersect(BoundingBoxI box)
		{
			BoundingBoxI result = default(BoundingBoxI);
			result.Min.X = Math.Max(Min.X, box.Min.X);
			result.Min.Y = Math.Max(Min.Y, box.Min.Y);
			result.Min.Z = Math.Max(Min.Z, box.Min.Z);
			result.Max.X = Math.Min(Max.X, box.Max.X);
			result.Max.Y = Math.Min(Max.Y, box.Max.Y);
			result.Max.Z = Math.Min(Max.Z, box.Max.Z);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingBoxI intersects another BoundingBoxI.
		/// </summary>
		/// <param name="box">The BoundingBoxI to check for intersection with.</param>
		public bool Intersects(BoundingBoxI box)
		{
			return Intersects(ref box);
		}

		public bool Intersects(ref BoundingBoxI box)
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
		/// Checks whether the current BoundingBoxI intersects another BoundingBoxI.
		/// </summary>
		/// <param name="box">The BoundingBoxI to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBoxI instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingBoxI box, out bool result)
		{
			result = false;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y) && !((double)Max.Z < (double)box.Min.Z) && !((double)Min.Z > (double)box.Max.Z))
			{
				result = true;
			}
		}

		public bool IntersectsTriangle(Vector3I v0, Vector3I v1, Vector3I v2)
		{
			return IntersectsTriangle(ref v0, ref v1, ref v2);
		}

		public bool IntersectsTriangle(ref Vector3I v0, ref Vector3I v1, ref Vector3I v2)
		{
			Vector3I.Min(ref v0, ref v1, out Vector3I result);
			Vector3I.Min(ref result, ref v2, out result);
			Vector3I.Max(ref v0, ref v1, out Vector3I result2);
			Vector3I.Max(ref result2, ref v2, out result2);
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
			Vector3I vector = v1 - v0;
			Vector3I vector2 = v2 - v1;
			Vector3I.Cross(ref vector, ref vector2, out Vector3I result3);
			Vector3I.Dot(ref v0, ref result3, out int dot);
			Plane plane = new Plane(result3, -dot);
			Intersects(ref plane, out PlaneIntersectionType result4);
			switch (result4)
			{
			case PlaneIntersectionType.Back:
				return false;
			case PlaneIntersectionType.Front:
				return false;
			default:
			{
				Vector3I center = Center;
				Vector3I halfExtents = new BoundingBoxI(Min - center, Max - center).HalfExtents;
				Vector3I vector3I = v0 - v2;
				Vector3I vector3I2 = v0 - center;
				Vector3I vector3I3 = v1 - center;
				Vector3I vector3I4 = v2 - center;
				float num = halfExtents.Y * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.Y);
				float val = vector3I2.Z * vector3I3.Y - vector3I2.Y * vector3I3.Z;
				float val2 = vector3I4.Z * vector.Y - vector3I4.Y * vector.Z;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Z) + halfExtents.Z * Math.Abs(vector.X);
				val = vector3I2.X * vector3I3.Z - vector3I2.Z * vector3I3.X;
				val2 = vector3I4.X * vector.Z - vector3I4.Z * vector.X;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector.Y) + halfExtents.Y * Math.Abs(vector.X);
				val = vector3I2.Y * vector3I3.X - vector3I2.X * vector3I3.Y;
				val2 = vector3I4.Y * vector.X - vector3I4.X * vector.Y;
				if (Math.Min(val, val2) > num || Math.Max(val, val2) < 0f - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.Y);
				float val3 = vector3I3.Z * vector3I4.Y - vector3I3.Y * vector3I4.Z;
				val = vector3I2.Z * vector2.Y - vector3I2.Y * vector2.Z;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.X);
				val3 = vector3I3.X * vector3I4.Z - vector3I3.Z * vector3I4.X;
				val = vector3I2.X * vector2.Z - vector3I2.Z * vector2.X;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector2.Y) + halfExtents.Y * Math.Abs(vector2.X);
				val3 = vector3I3.Y * vector3I4.X - vector3I3.X * vector3I4.Y;
				val = vector3I2.Y * vector2.X - vector3I2.X * vector2.Y;
				if (Math.Min(val3, val) > num || Math.Max(val3, val) < 0f - num)
				{
					return false;
				}
				num = halfExtents.Y * Math.Abs(vector3I.Z) + halfExtents.Z * Math.Abs(vector3I.Y);
				val2 = vector3I4.Z * vector3I2.Y - vector3I4.Y * vector3I2.Z;
				val3 = vector3I3.Z * vector3I.Y - vector3I3.Y * vector3I.Z;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3I.Z) + halfExtents.Z * Math.Abs(vector3I.X);
				val2 = vector3I4.X * vector3I2.Z - vector3I4.Z * vector3I2.X;
				val3 = vector3I3.X * vector3I.Z - vector3I3.Z * vector3I.X;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
				{
					return false;
				}
				num = halfExtents.X * Math.Abs(vector3I.Y) + halfExtents.Y * Math.Abs(vector3I.X);
				val2 = vector3I4.Y * vector3I2.X - vector3I4.X * vector3I2.Y;
				val3 = vector3I3.Y * vector3I.X - vector3I3.X * vector3I.Y;
				if (Math.Min(val2, val3) > num || Math.Max(val2, val3) < 0f - num)
				{
					return false;
				}
				return true;
			}
			}
		}

		/// <summary>
		/// Checks whether the current BoundingBoxI intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param>
		public PlaneIntersectionType Intersects(Plane plane)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = (((double)plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector3I.Y = (((double)plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector3I.Z = (((double)plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			Vector3I vector3I2 = default(Vector3I);
			vector3I2.X = (((double)plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector3I2.Y = (((double)plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector3I2.Z = (((double)plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			if ((double)plane.Normal.X * (double)vector3I.X + (double)plane.Normal.Y * (double)vector3I.Y + (double)plane.Normal.Z * (double)vector3I.Z + (double)plane.D > 0.0)
			{
				return PlaneIntersectionType.Front;
			}
			if (!((double)plane.Normal.X * (double)vector3I2.X + (double)plane.Normal.Y * (double)vector3I2.Y + (double)plane.Normal.Z * (double)vector3I2.Z + (double)plane.D < 0.0))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current BoundingBoxI intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingBoxI intersects the Plane.</param>
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = (((double)plane.Normal.X >= 0.0) ? Min.X : Max.X);
			vector3I.Y = (((double)plane.Normal.Y >= 0.0) ? Min.Y : Max.Y);
			vector3I.Z = (((double)plane.Normal.Z >= 0.0) ? Min.Z : Max.Z);
			Vector3I vector3I2 = default(Vector3I);
			vector3I2.X = (((double)plane.Normal.X >= 0.0) ? Max.X : Min.X);
			vector3I2.Y = (((double)plane.Normal.Y >= 0.0) ? Max.Y : Min.Y);
			vector3I2.Z = (((double)plane.Normal.Z >= 0.0) ? Max.Z : Min.Z);
			if ((double)plane.Normal.X * (double)vector3I.X + (double)plane.Normal.Y * (double)vector3I.Y + (double)plane.Normal.Z * (double)vector3I.Z + (double)plane.D > 0.0)
			{
				result = PlaneIntersectionType.Front;
			}
			else if ((double)plane.Normal.X * (double)vector3I2.X + (double)plane.Normal.Y * (double)vector3I2.Y + (double)plane.Normal.Z * (double)vector3I2.Z + (double)plane.D < 0.0)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
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
		/// Checks whether the current BoundingBoxI intersects a Ray.
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
				float num4 = ((float)Min.X - ray.Position.X) * num3;
				float num5 = ((float)Max.X - ray.Position.X) * num3;
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
				float num8 = ((float)Min.Y - ray.Position.Y) * num7;
				float num9 = ((float)Max.Y - ray.Position.Y) * num7;
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
				float num12 = ((float)Min.Z - ray.Position.Z) * num11;
				float num13 = ((float)Max.Z - ray.Position.Z) * num11;
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
		/// Checks whether the current BoundingBoxI intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBoxI, or null if there is no intersection.</param>
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
				float num4 = ((float)Min.X - ray.Position.X) * num3;
				float num5 = ((float)Max.X - ray.Position.X) * num3;
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
				float num8 = ((float)Min.Y - ray.Position.Y) * num7;
				float num9 = ((float)Max.Y - ray.Position.Y) * num7;
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
				float num12 = ((float)Min.Z - ray.Position.Z) * num11;
				float num13 = ((float)Max.Z - ray.Position.Z) * num11;
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
		/// Checks whether the current BoundingBoxI intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBoxI and BoundingSphere intersect; false otherwise.</param>
		public float Distance(Vector3I point)
		{
			return (Vector3I.Clamp(point, Min, Max) - point).Length();
		}

		/// <summary>
		/// Tests whether the BoundingBoxI contains another BoundingBoxI.
		/// </summary>
		/// <param name="box">The BoundingBoxI to test for overlap.</param>
		public ContainmentType Contains(BoundingBoxI box)
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
		/// Tests whether the BoundingBoxI contains a BoundingBoxI.
		/// </summary>
		/// <param name="box">The BoundingBoxI to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBoxI box, out ContainmentType result)
		{
			result = ContainmentType.Disjoint;
			if (!((double)Max.X < (double)box.Min.X) && !((double)Min.X > (double)box.Max.X) && !((double)Max.Y < (double)box.Min.Y) && !((double)Min.Y > (double)box.Max.Y) && !((double)Max.Z < (double)box.Min.Z) && !((double)Min.Z > (double)box.Max.Z))
			{
				result = ((!((double)Min.X > (double)box.Min.X) && !((double)box.Max.X > (double)Max.X) && !((double)Min.Y > (double)box.Min.Y) && !((double)box.Max.Y > (double)Max.Y) && !((double)Min.Z > (double)box.Min.Z) && !((double)box.Max.Z > (double)Max.Z)) ? ContainmentType.Contains : ContainmentType.Intersects);
			}
		}

		/// <summary>
		/// Tests whether the BoundingBoxI contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param>
		public ContainmentType Contains(Vector3I point)
		{
			if (!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y) && !((double)Min.Z > (double)point.Z) && !((double)point.Z > (double)Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		public ContainmentType Contains(Vector3 point)
		{
			if (!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y) && !((double)Min.Z > (double)point.Z) && !((double)point.Z > (double)Max.Z))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Tests whether the BoundingBoxI contains a point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3I point, out ContainmentType result)
		{
			result = ((!((double)Min.X > (double)point.X) && !((double)point.X > (double)Max.X) && !((double)Min.Y > (double)point.Y) && !((double)point.Y > (double)Max.Y) && !((double)Min.Z > (double)point.Z) && !((double)point.Z > (double)Max.Z)) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		/// <summary>
		/// Tests whether the BoundingBoxI contains a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		internal void SupportMapping(ref Vector3I v, out Vector3I result)
		{
			result.X = (((double)v.X >= 0.0) ? Max.X : Min.X);
			result.Y = (((double)v.Y >= 0.0) ? Max.Y : Min.Y);
			result.Z = (((double)v.Z >= 0.0) ? Max.Z : Min.Z);
		}

		/// <summary>
		/// Translate
		/// </summary>
		/// <param name="bbox"></param>
		/// <param name="vctTranlsation"></param>
		/// <returns></returns>
		public BoundingBoxI Translate(Vector3I vctTranlsation)
		{
			Min += vctTranlsation;
			Max += vctTranlsation;
			return this;
		}

		/// <summary>
		/// return expanded aabb (abb include point)
		/// </summary>
		/// <param name="bbox"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public BoundingBoxI Include(ref Vector3I point)
		{
			if (point.X < Min.X)
			{
				Min.X = point.X;
			}
			if (point.Y < Min.Y)
			{
				Min.Y = point.Y;
			}
			if (point.Z < Min.Z)
			{
				Min.Z = point.Z;
			}
			if (point.X > Max.X)
			{
				Max.X = point.X;
			}
			if (point.Y > Max.Y)
			{
				Max.Y = point.Y;
			}
			if (point.Z > Max.Z)
			{
				Max.Z = point.Z;
			}
			return this;
		}

		public BoundingBoxI GetIncluded(Vector3I point)
		{
			BoundingBoxI result = this;
			result.Include(point);
			return result;
		}

		public BoundingBoxI Include(Vector3I point)
		{
			return Include(ref point);
		}

		public BoundingBoxI Include(Vector3I p0, Vector3I p1, Vector3I p2)
		{
			return Include(ref p0, ref p1, ref p2);
		}

		public BoundingBoxI Include(ref Vector3I p0, ref Vector3I p1, ref Vector3I p2)
		{
			Include(ref p0);
			Include(ref p1);
			Include(ref p2);
			return this;
		}

		/// <summary>
		/// return expanded aabb (abb include point)
		/// </summary>
		/// <param name="bbox"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public BoundingBoxI Include(ref BoundingBoxI box)
		{
			Min = Vector3I.Min(Min, box.Min);
			Max = Vector3I.Max(Max, box.Max);
			return this;
		}

		public BoundingBoxI Include(BoundingBoxI box)
		{
			return Include(ref box);
		}

		public static BoundingBoxI CreateInvalid()
		{
			BoundingBoxI result = default(BoundingBoxI);
			Vector3I min = new Vector3I(int.MaxValue, int.MaxValue, int.MaxValue);
			Vector3I max = new Vector3I(int.MinValue, int.MinValue, int.MinValue);
			result.Min = min;
			result.Max = max;
			return result;
		}

		public float SurfaceArea()
		{
			Vector3I vector3I = Max - Min;
			return 2 * (vector3I.X * vector3I.Y + vector3I.X * vector3I.Z + vector3I.Y * vector3I.Z);
		}

		public float Volume()
		{
			Vector3I vector3I = Max - Min;
			return vector3I.X * vector3I.Y * vector3I.Z;
		}

		public void Inflate(int size)
		{
			Max += new Vector3I(size);
			Min -= new Vector3I(size);
		}

		public void InflateToMinimum(Vector3I minimumSize)
		{
			Vector3I center = Center;
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
			if (Size.Z < minimumSize.Z)
			{
				Min.Z = center.Z - minimumSize.Z / 2;
				Max.Z = center.Z + minimumSize.Z / 2;
			}
		}

		/// <summary>
		/// Iterate every cell contained in {left} - {right},
		/// where we interpret {box} as the set of all distinct Vector3I points inside a 'box'.
		///
		/// Containment is taken in a typical inclusive start, exclusive end fashion.
		/// </summary>
		/// <param name="left">The left bounding box.</param>
		/// <param name="right">The right bounding box.</param>
		/// <returns></returns>
		public static IEnumerable<Vector3I> IterateDifference(BoundingBoxI left, BoundingBoxI right)
		{
			Vector3I min2 = left.Min;
			Vector3I max2 = new Vector3I(Math.Min(left.Max.X, right.Min.X), left.Max.Y, left.Max.Z);
			Vector3I vec = default(Vector3I);
			vec.X = min2.X;
			while (vec.X < max2.X)
			{
				vec.Y = min2.Y;
				while (vec.Y < max2.Y)
				{
					vec.Z = min2.Z;
					while (vec.Z < max2.Z)
					{
						yield return vec;
						vec.Z++;
					}
					vec.Y++;
				}
				vec.X++;
			}
			min2 = new Vector3I(Math.Max(left.Min.X, right.Max.X), left.Min.Y, left.Min.Z);
			max2 = left.Max;
			vec.X = min2.X;
			while (vec.X < max2.X)
			{
				vec.Y = min2.Y;
				while (vec.Y < max2.Y)
				{
					vec.Z = min2.Z;
					while (vec.Z < max2.Z)
					{
						yield return vec;
						vec.Z++;
					}
					vec.Y++;
				}
				vec.X++;
			}
			left.Min.X = Math.Max(left.Min.X, right.Min.X);
			left.Max.X = Math.Min(left.Max.X, right.Max.X);
			min2 = left.Min;
			max2 = new Vector3I(left.Max.X, Math.Min(left.Max.Y, right.Min.Y), left.Max.Z);
			vec.Y = min2.Y;
			while (vec.Y < max2.Y)
			{
				vec.X = min2.X;
				while (vec.X < max2.X)
				{
					vec.Z = min2.Z;
					while (vec.Z < max2.Z)
					{
						yield return vec;
						vec.Z++;
					}
					vec.X++;
				}
				vec.Y++;
			}
			min2 = new Vector3I(left.Min.X, Math.Max(left.Min.Y, right.Max.Y), left.Min.Z);
			max2 = left.Max;
			vec.Y = min2.Y;
			while (vec.Y < max2.Y)
			{
				vec.X = min2.X;
				while (vec.X < max2.X)
				{
					vec.Z = min2.Z;
					while (vec.Z < max2.Z)
					{
						yield return vec;
						vec.Z++;
					}
					vec.X++;
				}
				vec.Y++;
			}
			left.Min.Y = Math.Max(left.Min.Y, right.Min.Y);
			left.Max.Y = Math.Min(left.Max.Y, right.Max.Y);
			min2 = left.Min;
			max2 = new Vector3I(left.Max.X, left.Max.Y, Math.Min(left.Max.Z, right.Min.Z));
			vec.Z = min2.Z;
			while (vec.Z < max2.Z)
			{
				vec.Y = min2.Y;
				while (vec.Y < max2.Y)
				{
					vec.X = min2.X;
					while (vec.X < max2.X)
					{
						yield return vec;
						vec.X++;
					}
					vec.Y++;
				}
				vec.Z++;
			}
			min2 = new Vector3I(left.Min.X, left.Min.Y, Math.Max(left.Min.Z, right.Max.Z));
			max2 = left.Max;
			vec.Z = min2.Z;
			while (vec.Z < max2.Z)
			{
				vec.Y = min2.Y;
				while (vec.Y < max2.Y)
				{
					vec.X = min2.X;
					while (vec.X < max2.X)
					{
						yield return vec;
						vec.X++;
					}
					vec.Y++;
				}
				vec.Z++;
			}
		}

		/// <summary>
		/// Enumerate all values in a integer interval (a cuboid).
		///
		/// This method is an allocating version of the Vector3I_RangeIterator.
		/// This once can be used in the foreach syntax though so it's more convenient for debug routines.
		/// </summary>
		/// <returns>An iterator for that range.</returns>
		public static IEnumerable<Vector3I> EnumeratePoints(BoundingBoxI rangeInclusive)
		{
			Vector3I vec = default(Vector3I);
			vec.Z = rangeInclusive.Min.Z;
			while (vec.Z < rangeInclusive.Max.Z)
			{
				vec.Y = rangeInclusive.Min.Y;
				while (vec.Y < rangeInclusive.Max.Y)
				{
					vec.X = rangeInclusive.Min.X;
					while (vec.X < rangeInclusive.Max.X)
					{
						yield return vec;
						vec.X++;
					}
					vec.Y++;
				}
				vec.Z++;
			}
		}
	}
}
