using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a sphere.
	/// </summary>
	[Serializable]
	public struct BoundingSphere : IEquatable<BoundingSphere>
	{
		protected class VRageMath_BoundingSphere_003C_003ECenter_003C_003EAccessor : IMemberAccessor<BoundingSphere, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingSphere owner, in Vector3 value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingSphere owner, out Vector3 value)
			{
				value = owner.Center;
			}
		}

		protected class VRageMath_BoundingSphere_003C_003ERadius_003C_003EAccessor : IMemberAccessor<BoundingSphere, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingSphere owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingSphere owner, out float value)
			{
				value = owner.Radius;
			}
		}

		/// <summary>
		/// The center point of the sphere.
		/// </summary>
		public Vector3 Center;

		/// <summary>
		/// The radius of the sphere.
		/// </summary>
		public float Radius;

		/// <summary>
		/// Creates a new instance of BoundingSphere.
		/// </summary>
		/// <param name="center">Center point of the sphere.</param><param name="radius">Radius of the sphere.</param>
		public BoundingSphere(Vector3 center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		/// <summary>
		/// Determines whether two instances of BoundingSphere are equal.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(BoundingSphere a, BoundingSphere b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingSphere are not equal.
		/// </summary>
		/// <param name="a">The BoundingSphere to the left of the inequality operator.</param><param name="b">The BoundingSphere to the right of the inequality operator.</param>
		public static bool operator !=(BoundingSphere a, BoundingSphere b)
		{
			if (!(a.Center != b.Center))
			{
				return (double)a.Radius != (double)b.Radius;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified BoundingSphere is equal to the current BoundingSphere.
		/// </summary>
		/// <param name="other">The BoundingSphere to compare with the current BoundingSphere.</param>
		public bool Equals(BoundingSphere other)
		{
			if (Center == other.Center)
			{
				return (double)Radius == (double)other.Radius;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the BoundingSphere.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingSphere.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingSphere)
			{
				result = Equals((BoundingSphere)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return Center.GetHashCode() + Radius.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current BoundingSphere.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{Center:{0} Radius:{1}}}", new object[2]
			{
				Center.ToString(),
				Radius.ToString(currentCulture)
			});
		}

		/// <summary>
		/// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
		/// </summary>
		/// <param name="original">BoundingSphere to be merged.</param><param name="additional">BoundingSphere to be merged.</param>
		public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
		{
			Vector3.Subtract(ref additional.Center, ref original.Center, out Vector3 result);
			float num = result.Length();
			float radius = original.Radius;
			float radius2 = additional.Radius;
			if ((double)radius + (double)radius2 >= (double)num)
			{
				if ((double)radius - (double)radius2 >= (double)num)
				{
					return original;
				}
				if ((double)radius2 - (double)radius >= (double)num)
				{
					return additional;
				}
			}
			Vector3 value = result * (1f / num);
			float num2 = MathHelper.Min(0f - radius, num - radius2);
			float num3 = (float)(((double)MathHelper.Max(radius, num + radius2) - (double)num2) * 0.5);
			BoundingSphere result2 = default(BoundingSphere);
			result2.Center = original.Center + value * (num3 + num2);
			result2.Radius = num3;
			return result2;
		}

		/// <summary>
		/// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
		/// </summary>
		/// <param name="original">BoundingSphere to be merged.</param><param name="additional">BoundingSphere to be merged.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
		public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
		{
			Vector3.Subtract(ref additional.Center, ref original.Center, out Vector3 result2);
			float num = result2.Length();
			float radius = original.Radius;
			float radius2 = additional.Radius;
			if ((double)radius + (double)radius2 >= (double)num)
			{
				if ((double)radius - (double)radius2 >= (double)num)
				{
					result = original;
					return;
				}
				if ((double)radius2 - (double)radius >= (double)num)
				{
					result = additional;
					return;
				}
			}
			Vector3 value = result2 * (1f / num);
			float num2 = MathHelper.Min(0f - radius, num - radius2);
			float num3 = (float)(((double)MathHelper.Max(radius, num + radius2) - (double)num2) * 0.5);
			result.Center = original.Center + value * (num3 + num2);
			result.Radius = num3;
		}

		/// <summary>
		/// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to create the BoundingSphere from.</param>
		public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
		{
			BoundingSphere result = default(BoundingSphere);
			result.Center = (box.Min + box.Max) * 0.5f;
			Vector3.Distance(ref result.Center, ref box.Max, out result.Radius);
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to create the BoundingSphere from.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
		public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
		{
			result.Center = (box.Min + box.Max) * 0.5f;
			Vector3.Distance(ref result.Center, ref box.Max, out result.Radius);
		}

		/// <summary>
		/// Creates a BoundingSphere that can contain a specified list of points.
		/// </summary>
		/// <param name="points">List of points the BoundingSphere must contain.</param>
		public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
		{
			IEnumerator<Vector3> enumerator = points.GetEnumerator();
			enumerator.MoveNext();
			Vector3 value5;
			Vector3 value4;
			Vector3 value3;
			Vector3 value2;
			Vector3 value;
			Vector3 value6 = value5 = (value4 = (value3 = (value2 = (value = enumerator.Current))));
			foreach (Vector3 point in points)
			{
				if ((double)point.X < (double)value6.X)
				{
					value6 = point;
				}
				if ((double)point.X > (double)value5.X)
				{
					value5 = point;
				}
				if ((double)point.Y < (double)value4.Y)
				{
					value4 = point;
				}
				if ((double)point.Y > (double)value3.Y)
				{
					value3 = point;
				}
				if ((double)point.Z < (double)value2.Z)
				{
					value2 = point;
				}
				if ((double)point.Z > (double)value.Z)
				{
					value = point;
				}
			}
			Vector3.Distance(ref value5, ref value6, out float result);
			Vector3.Distance(ref value3, ref value4, out float result2);
			Vector3.Distance(ref value, ref value2, out float result3);
			Vector3 result4;
			float num;
			if ((double)result > (double)result2)
			{
				if ((double)result > (double)result3)
				{
					Vector3.Lerp(ref value5, ref value6, 0.5f, out result4);
					num = result * 0.5f;
				}
				else
				{
					Vector3.Lerp(ref value, ref value2, 0.5f, out result4);
					num = result3 * 0.5f;
				}
			}
			else if ((double)result2 > (double)result3)
			{
				Vector3.Lerp(ref value3, ref value4, 0.5f, out result4);
				num = result2 * 0.5f;
			}
			else
			{
				Vector3.Lerp(ref value, ref value2, 0.5f, out result4);
				num = result3 * 0.5f;
			}
			foreach (Vector3 point2 in points)
			{
				Vector3 value7 = default(Vector3);
				value7.X = point2.X - result4.X;
				value7.Y = point2.Y - result4.Y;
				value7.Z = point2.Z - result4.Z;
				float num2 = value7.Length();
				if ((double)num2 > (double)num)
				{
					num = (float)(((double)num + (double)num2) * 0.5);
					result4 += (float)(1.0 - (double)num / (double)num2) * value7;
				}
			}
			BoundingSphere result5 = default(BoundingSphere);
			result5.Center = result4;
			result5.Radius = num;
			return result5;
		}

		/// <summary>
		/// Creates the smallest BoundingSphere that can contain a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to create the BoundingSphere with.</param>
		public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			return CreateFromPoints(frustum.cornerArray);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects with a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with the current BoundingSphere.</param>
		public bool Intersects(BoundingBox box)
		{
			Vector3.Clamp(ref Center, ref box.Min, ref box.Max, out Vector3 result);
			Vector3.DistanceSquared(ref Center, ref result, out float result2);
			return (double)result2 <= (double)Radius * (double)Radius;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere and BoundingBox intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox box, out bool result)
		{
			Vector3.Clamp(ref Center, ref box.Min, ref box.Max, out Vector3 result2);
			Vector3.DistanceSquared(ref Center, ref result2, out float result3);
			result = ((double)result3 <= (double)Radius * (double)Radius);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
		public bool Intersects(BoundingFrustum frustum)
		{
			frustum.Intersects(ref this, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects with a specified Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with the current BoundingSphere.</param>
		public PlaneIntersectionType Intersects(Plane plane)
		{
			return plane.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingSphere intersects the Plane.</param>
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			plane.Intersects(ref this, out result);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects with a specified Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with the current BoundingSphere.</param>
		public float? Intersects(Ray ray)
		{
			return ray.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
		public void Intersects(ref Ray ray, out float? result)
		{
			ray.Intersects(ref this, out result);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects with a specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with the current BoundingSphere.</param>
		public bool Intersects(BoundingSphere sphere)
		{
			Vector3.DistanceSquared(ref Center, ref sphere.Center, out float result);
			float radius = Radius;
			float radius2 = sphere.Radius;
			return (double)radius * (double)radius + 2.0 * (double)radius * (double)radius2 + (double)radius2 * (double)radius2 > (double)result;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere intersects another BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			Vector3.DistanceSquared(ref Center, ref sphere.Center, out float result2);
			float radius = Radius;
			float radius2 = sphere.Radius;
			result = ((double)radius * (double)radius + 2.0 * (double)radius * (double)radius2 + (double)radius2 * (double)radius2 > (double)result2);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check against the current BoundingSphere.</param>
		public ContainmentType Contains(BoundingBox box)
		{
			if (!box.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			float num = Radius * Radius;
			Vector3 vector = default(Vector3);
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Min.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Min.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Min.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return ContainmentType.Intersects;
			}
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Min.Z;
			if (!((double)vector.LengthSquared() > (double)num))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			box.Intersects(ref this, out bool result2);
			if (!result2)
			{
				result = ContainmentType.Disjoint;
				return;
			}
			float num = Radius * Radius;
			result = ContainmentType.Intersects;
			Vector3 vector = default(Vector3);
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Max.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Min.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Min.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Max.Y;
			vector.Z = Center.Z - box.Min.Z;
			if ((double)vector.LengthSquared() > (double)num)
			{
				return;
			}
			vector.X = Center.X - box.Max.X;
			vector.Y = Center.Y - box.Min.Y;
			vector.Z = Center.Z - box.Min.Z;
			if (!((double)vector.LengthSquared() > (double)num))
			{
				vector.X = Center.X - box.Min.X;
				vector.Y = Center.Y - box.Min.Y;
				vector.Z = Center.Z - box.Min.Z;
				if (!((double)vector.LengthSquared() > (double)num))
				{
					result = ContainmentType.Contains;
				}
			}
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check against the current BoundingSphere.</param>
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			if (!frustum.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			float num = Radius * Radius;
			Vector3[] cornerArray = frustum.cornerArray;
			Vector3 vector2 = default(Vector3);
			for (int i = 0; i < cornerArray.Length; i++)
			{
				Vector3 vector = cornerArray[i];
				vector2.X = vector.X - Center.X;
				vector2.Y = vector.Y - Center.Y;
				vector2.Z = vector.Z - Center.Z;
				if ((double)vector2.LengthSquared() > (double)num)
				{
					return ContainmentType.Intersects;
				}
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified point.
		/// </summary>
		/// <param name="point">The point to check against the current BoundingSphere.</param>
		public ContainmentType Contains(Vector3 point)
		{
			if (!((double)Vector3.DistanceSquared(point, Center) >= (double)Radius * (double)Radius))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			Vector3.DistanceSquared(ref point, ref Center, out float result2);
			result = (((double)result2 < (double)Radius * (double)Radius) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check against the current BoundingSphere.</param>
		public ContainmentType Contains(BoundingSphere sphere)
		{
			Vector3.Distance(ref Center, ref sphere.Center, out float result);
			float radius = Radius;
			float radius2 = sphere.Radius;
			if ((double)radius + (double)radius2 < (double)result)
			{
				return ContainmentType.Disjoint;
			}
			if (!((double)radius - (double)radius2 < (double)result))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Checks whether the current BoundingSphere contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			Vector3.Distance(ref Center, ref sphere.Center, out float result2);
			float radius = Radius;
			float radius2 = sphere.Radius;
			result = (((double)radius + (double)radius2 >= (double)result2) ? (((double)radius - (double)radius2 >= (double)result2) ? ContainmentType.Contains : ContainmentType.Intersects) : ContainmentType.Disjoint);
		}

		internal void SupportMapping(ref Vector3 v, out Vector3 result)
		{
			float num = Radius / v.Length();
			result.X = Center.X + v.X * num;
			result.Y = Center.Y + v.Y * num;
			result.Z = Center.Z + v.Z * num;
		}

		/// <summary>
		/// Translates and scales the BoundingSphere using a given Matrix.
		/// </summary>
		/// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
		public BoundingSphere Transform(Matrix matrix)
		{
			BoundingSphere result = default(BoundingSphere);
			result.Center = Vector3.Transform(Center, matrix);
			float num = Math.Max((float)((double)matrix.M11 * (double)matrix.M11 + (double)matrix.M12 * (double)matrix.M12 + (double)matrix.M13 * (double)matrix.M13), Math.Max((float)((double)matrix.M21 * (double)matrix.M21 + (double)matrix.M22 * (double)matrix.M22 + (double)matrix.M23 * (double)matrix.M23), (float)((double)matrix.M31 * (double)matrix.M31 + (double)matrix.M32 * (double)matrix.M32 + (double)matrix.M33 * (double)matrix.M33)));
			result.Radius = Radius * (float)Math.Sqrt(num);
			return result;
		}

		/// <summary>
		/// Translates and scales the BoundingSphere using a given Matrix.
		/// </summary>
		/// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param><param name="result">[OutAttribute] The transformed BoundingSphere.</param>
		public void Transform(ref Matrix matrix, out BoundingSphere result)
		{
			result.Center = Vector3.Transform(Center, matrix);
			float num = Math.Max((float)((double)matrix.M11 * (double)matrix.M11 + (double)matrix.M12 * (double)matrix.M12 + (double)matrix.M13 * (double)matrix.M13), Math.Max((float)((double)matrix.M21 * (double)matrix.M21 + (double)matrix.M22 * (double)matrix.M22 + (double)matrix.M23 * (double)matrix.M23), (float)((double)matrix.M31 * (double)matrix.M31 + (double)matrix.M32 * (double)matrix.M32 + (double)matrix.M33 * (double)matrix.M33)));
			result.Radius = Radius * (float)Math.Sqrt(num);
		}

		public BoundingSphere Translate(ref Vector3 translation)
		{
			return new BoundingSphere(Center + translation, Radius);
		}

		public bool IntersectRaySphere(Ray ray, out float tmin, out float tmax)
		{
			tmin = 0f;
			tmax = 0f;
			Vector3 v = ray.Position - Center;
			float num = ray.Direction.Dot(ray.Direction);
			float num2 = 2f * v.Dot(ray.Direction);
			float num3 = v.Dot(v) - Radius * Radius;
			float num4 = num2 * num2 - 4f * num * num3;
			if (num4 < 0f)
			{
				return false;
			}
			tmin = (0f - num2 - (float)Math.Sqrt(num4)) / (2f * num);
			tmax = (0f - num2 + (float)Math.Sqrt(num4)) / (2f * num);
			if (tmin > tmax)
			{
				float num5 = tmin;
				tmin = tmax;
				tmax = num5;
			}
			return true;
		}

		public BoundingSphere Include(BoundingSphere sphere)
		{
			Include(ref this, ref sphere);
			return this;
		}

		public static void Include(ref BoundingSphere sphere, ref BoundingSphere otherSphere)
		{
			if (sphere.Radius == float.MinValue)
			{
				sphere.Center = otherSphere.Center;
				sphere.Radius = otherSphere.Radius;
				return;
			}
			float num = Vector3.Distance(sphere.Center, otherSphere.Center);
			if (!(num + otherSphere.Radius <= sphere.Radius))
			{
				if (num + sphere.Radius <= otherSphere.Radius)
				{
					sphere = otherSphere;
					return;
				}
				float amount = (num + otherSphere.Radius - sphere.Radius) / (2f * num);
				Vector3 center = Vector3.Lerp(sphere.Center, otherSphere.Center, amount);
				float radius = (num + sphere.Radius + otherSphere.Radius) / 2f;
				sphere.Center = center;
				sphere.Radius = radius;
			}
		}

		public static BoundingSphere CreateInvalid()
		{
			return new BoundingSphere(Vector3.Zero, float.MinValue);
		}
	}
}
