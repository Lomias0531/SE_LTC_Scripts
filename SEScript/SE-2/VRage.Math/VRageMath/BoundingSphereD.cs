using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a sphere.
	/// </summary>
	[Serializable]
	public struct BoundingSphereD : IEquatable<BoundingSphereD>
	{
		protected class VRageMath_BoundingSphereD_003C_003ECenter_003C_003EAccessor : IMemberAccessor<BoundingSphereD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingSphereD owner, in Vector3D value)
			{
				owner.Center = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingSphereD owner, out Vector3D value)
			{
				value = owner.Center;
			}
		}

		protected class VRageMath_BoundingSphereD_003C_003ERadius_003C_003EAccessor : IMemberAccessor<BoundingSphereD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingSphereD owner, in double value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingSphereD owner, out double value)
			{
				value = owner.Radius;
			}
		}

		/// <summary>
		/// The center point of the sphere.
		/// </summary>
		public Vector3D Center;

		/// <summary>
		/// The radius of the sphere.
		/// </summary>
		public double Radius;

		/// <summary>
		/// Creates a new instance of BoundingSphereD.
		/// </summary>
		/// <param name="center">Center point of the sphere.</param><param name="radius">Radius of the sphere.</param>
		public BoundingSphereD(Vector3D center, double radius)
		{
			Center = center;
			Radius = radius;
		}

		/// <summary>
		/// Determines whether two instances of BoundingSphereD are equal.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(BoundingSphereD a, BoundingSphereD b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingSphereD are not equal.
		/// </summary>
		/// <param name="a">The BoundingSphereD to the left of the inequality operator.</param><param name="b">The BoundingSphereD to the right of the inequality operator.</param>
		public static bool operator !=(BoundingSphereD a, BoundingSphereD b)
		{
			if (!(a.Center != b.Center))
			{
				return a.Radius != b.Radius;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified BoundingSphereD is equal to the current BoundingSphereD.
		/// </summary>
		/// <param name="other">The BoundingSphereD to compare with the current BoundingSphereD.</param>
		public bool Equals(BoundingSphereD other)
		{
			if (Center == other.Center)
			{
				return Radius == other.Radius;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the BoundingSphereD.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingSphereD.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BoundingSphereD)
			{
				result = Equals((BoundingSphereD)obj);
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
		/// Returns a String that represents the current BoundingSphereD.
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
		/// Creates a BoundingSphereD that contains the two specified BoundingSphereD instances.
		/// </summary>
		/// <param name="original">BoundingSphereD to be merged.</param><param name="additional">BoundingSphereD to be merged.</param>
		public static BoundingSphereD CreateMerged(BoundingSphereD original, BoundingSphereD additional)
		{
			Vector3D.Subtract(ref additional.Center, ref original.Center, out Vector3D result);
			double num = result.Length();
			double radius = original.Radius;
			double radius2 = additional.Radius;
			if (radius + radius2 >= num)
			{
				if (radius - radius2 >= num)
				{
					return original;
				}
				if (radius2 - radius >= num)
				{
					return additional;
				}
			}
			Vector3D value = result * (1.0 / num);
			double num2 = MathHelper.Min(0.0 - radius, num - radius2);
			double num3 = (MathHelper.Max(radius, num + radius2) - num2) * 0.5;
			BoundingSphereD result2 = default(BoundingSphereD);
			result2.Center = original.Center + value * (num3 + num2);
			result2.Radius = num3;
			return result2;
		}

		/// <summary>
		/// Creates a BoundingSphereD that contains the two specified BoundingSphereD instances.
		/// </summary>
		/// <param name="original">BoundingSphereD to be merged.</param><param name="additional">BoundingSphereD to be merged.</param><param name="result">[OutAttribute] The created BoundingSphereD.</param>
		public static void CreateMerged(ref BoundingSphereD original, ref BoundingSphereD additional, out BoundingSphereD result)
		{
			Vector3D.Subtract(ref additional.Center, ref original.Center, out Vector3D result2);
			double num = result2.Length();
			double radius = original.Radius;
			double radius2 = additional.Radius;
			if (radius + radius2 >= num)
			{
				if (radius - radius2 >= num)
				{
					result = original;
					return;
				}
				if (radius2 - radius >= num)
				{
					result = additional;
					return;
				}
			}
			Vector3D value = result2 * (1.0 / num);
			double num2 = MathHelper.Min(0.0 - radius, num - radius2);
			double num3 = (MathHelper.Max(radius, num + radius2) - num2) * 0.5;
			result.Center = original.Center + value * (num3 + num2);
			result.Radius = num3;
		}

		/// <summary>
		/// Creates the smallest BoundingSphereD that can contain a specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to create the BoundingSphereD from.</param>
		public static BoundingSphereD CreateFromBoundingBox(BoundingBoxD box)
		{
			BoundingSphereD result = default(BoundingSphereD);
			Vector3D.Lerp(ref box.Min, ref box.Max, 0.5, out result.Center);
			Vector3D.Distance(ref box.Min, ref box.Max, out double result2);
			result.Radius = result2 * 0.5;
			return result;
		}

		/// <summary>
		/// Creates the smallest BoundingSphereD that can contain a specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to create the BoundingSphereD from.</param><param name="result">[OutAttribute] The created BoundingSphereD.</param>
		public static void CreateFromBoundingBox(ref BoundingBoxD box, out BoundingSphereD result)
		{
			Vector3D.Lerp(ref box.Min, ref box.Max, 0.5, out result.Center);
			Vector3D.Distance(ref box.Min, ref box.Max, out double result2);
			result.Radius = result2 * 0.5;
		}

		/// <summary>
		/// Creates a BoundingSphereD that can contain a specified list of points.
		/// </summary>
		/// <param name="points">List of points the BoundingSphereD must contain.</param>
		public static BoundingSphereD CreateFromPoints(Vector3D[] points)
		{
			Vector3D value5;
			Vector3D value4;
			Vector3D value3;
			Vector3D value2;
			Vector3D value;
			Vector3D value6 = value5 = (value4 = (value3 = (value2 = (value = points[0]))));
			Vector3D[] array = points;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3D vector3D = array[i];
				if (vector3D.X < value6.X)
				{
					value6 = vector3D;
				}
				if (vector3D.X > value5.X)
				{
					value5 = vector3D;
				}
				if (vector3D.Y < value4.Y)
				{
					value4 = vector3D;
				}
				if (vector3D.Y > value3.Y)
				{
					value3 = vector3D;
				}
				if (vector3D.Z < value2.Z)
				{
					value2 = vector3D;
				}
				if (vector3D.Z > value.Z)
				{
					value = vector3D;
				}
			}
			Vector3D.Distance(ref value5, ref value6, out double result);
			Vector3D.Distance(ref value3, ref value4, out double result2);
			Vector3D.Distance(ref value, ref value2, out double result3);
			Vector3D result4;
			double num;
			if (result > result2)
			{
				if (result > result3)
				{
					Vector3D.Lerp(ref value5, ref value6, 0.5, out result4);
					num = result * 0.5;
				}
				else
				{
					Vector3D.Lerp(ref value, ref value2, 0.5, out result4);
					num = result3 * 0.5;
				}
			}
			else if (result2 > result3)
			{
				Vector3D.Lerp(ref value3, ref value4, 0.5, out result4);
				num = result2 * 0.5;
			}
			else
			{
				Vector3D.Lerp(ref value, ref value2, 0.5, out result4);
				num = result3 * 0.5;
			}
			array = points;
			Vector3D value7 = default(Vector3D);
			for (int i = 0; i < array.Length; i++)
			{
				Vector3D vector3D2 = array[i];
				value7.X = vector3D2.X - result4.X;
				value7.Y = vector3D2.Y - result4.Y;
				value7.Z = vector3D2.Z - result4.Z;
				double num2 = value7.Length();
				if (num2 > num)
				{
					num = (num + num2) * 0.5;
					result4 += (1.0 - num / num2) * value7;
				}
			}
			BoundingSphereD result5 = default(BoundingSphereD);
			result5.Center = result4;
			result5.Radius = num;
			return result5;
		}

		/// <summary>
		/// Creates the smallest BoundingSphereD that can contain a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to create the BoundingSphereD with.</param>
		public static BoundingSphereD CreateFromFrustum(BoundingFrustumD frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			return CreateFromPoints(frustum.CornerArray);
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD intersects with a specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check for intersection with the current BoundingSphereD.</param>
		public bool Intersects(BoundingBoxD box)
		{
			Vector3D.Clamp(ref Center, ref box.Min, ref box.Max, out Vector3D result);
			Vector3D.DistanceSquared(ref Center, ref result, out double result2);
			return result2 <= Radius * Radius;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD intersects a BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphereD and BoundingBoxD intersect; false otherwise.</param>
		public void Intersects(ref BoundingBoxD box, out bool result)
		{
			Vector3D.Clamp(ref Center, ref box.Min, ref box.Max, out Vector3D result2);
			Vector3D.DistanceSquared(ref Center, ref result2, out double result3);
			result = (result3 <= Radius * Radius);
		}

		public double? Intersects(RayD ray)
		{
			return ray.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD intersects with a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphereD.</param>
		public bool Intersects(BoundingFrustumD frustum)
		{
			frustum.Intersects(ref this, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD intersects with a specified BoundingSphereD.
		/// </summary>
		/// <param name="sphere">The BoundingSphereD to check for intersection with the current BoundingSphereD.</param>
		public bool Intersects(BoundingSphereD sphere)
		{
			Vector3D.DistanceSquared(ref Center, ref sphere.Center, out double result);
			double radius = Radius;
			double radius2 = sphere.Radius;
			return radius * radius + 2.0 * radius * radius2 + radius2 * radius2 > result;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD intersects another BoundingSphereD.
		/// </summary>
		/// <param name="sphere">The BoundingSphereD to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphereD instances intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphereD sphere, out bool result)
		{
			Vector3D.DistanceSquared(ref Center, ref sphere.Center, out double result2);
			double radius = Radius;
			double radius2 = sphere.Radius;
			result = (radius * radius + 2.0 * radius * radius2 + radius2 * radius2 > result2);
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check against the current BoundingSphereD.</param>
		public ContainmentType Contains(BoundingBoxD box)
		{
			if (!box.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			double num = Radius * Radius;
			Vector3D vector3D = default(Vector3D);
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (vector3D.LengthSquared() > num)
			{
				return ContainmentType.Intersects;
			}
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (!(vector3D.LengthSquared() > num))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBoxD box, out ContainmentType result)
		{
			box.Intersects(ref this, out bool result2);
			if (!result2)
			{
				result = ContainmentType.Disjoint;
				return;
			}
			double num = Radius * Radius;
			result = ContainmentType.Intersects;
			Vector3D vector3D = default(Vector3D);
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Max.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Min.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Max.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (vector3D.LengthSquared() > num)
			{
				return;
			}
			vector3D.X = Center.X - box.Max.X;
			vector3D.Y = Center.Y - box.Min.Y;
			vector3D.Z = Center.Z - box.Min.Z;
			if (!(vector3D.LengthSquared() > num))
			{
				vector3D.X = Center.X - box.Min.X;
				vector3D.Y = Center.Y - box.Min.Y;
				vector3D.Z = Center.Z - box.Min.Z;
				if (!(vector3D.LengthSquared() > num))
				{
					result = ContainmentType.Contains;
				}
			}
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check against the current BoundingSphereD.</param>
		public ContainmentType Contains(BoundingFrustumD frustum)
		{
			if (!frustum.Intersects(this))
			{
				return ContainmentType.Disjoint;
			}
			double num = Radius * Radius;
			Vector3D[] cornerArray = frustum.CornerArray;
			Vector3D vector3D2 = default(Vector3D);
			for (int i = 0; i < cornerArray.Length; i++)
			{
				Vector3D vector3D = cornerArray[i];
				vector3D2.X = vector3D.X - Center.X;
				vector3D2.Y = vector3D.Y - Center.Y;
				vector3D2.Z = vector3D.Z - Center.Z;
				if (vector3D2.LengthSquared() > num)
				{
					return ContainmentType.Intersects;
				}
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified point.
		/// </summary>
		/// <param name="point">The point to check against the current BoundingSphereD.</param>
		public ContainmentType Contains(Vector3D point)
		{
			if (!(Vector3D.DistanceSquared(point, Center) >= Radius * Radius))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Disjoint;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3D point, out ContainmentType result)
		{
			Vector3D.DistanceSquared(ref point, ref Center, out double result2);
			result = ((result2 < Radius * Radius) ? ContainmentType.Contains : ContainmentType.Disjoint);
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified BoundingSphereD.
		/// </summary>
		/// <param name="sphere">The BoundingSphereD to check against the current BoundingSphereD.</param>
		public ContainmentType Contains(BoundingSphereD sphere)
		{
			Vector3D.Distance(ref Center, ref sphere.Center, out double result);
			double radius = Radius;
			double radius2 = sphere.Radius;
			if (radius + radius2 < result)
			{
				return ContainmentType.Disjoint;
			}
			if (!(radius - radius2 < result))
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Checks whether the current BoundingSphereD contains the specified BoundingSphereD.
		/// </summary>
		/// <param name="sphere">The BoundingSphereD to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
		{
			Vector3D.Distance(ref Center, ref sphere.Center, out double result2);
			double radius = Radius;
			double radius2 = sphere.Radius;
			result = ((radius + radius2 >= result2) ? ((radius - radius2 >= result2) ? ContainmentType.Contains : ContainmentType.Intersects) : ContainmentType.Disjoint);
		}

		internal void SupportMapping(ref Vector3D v, out Vector3D result)
		{
			double num = Radius / v.Length();
			result.X = Center.X + v.X * num;
			result.Y = Center.Y + v.Y * num;
			result.Z = Center.Z + v.Z * num;
		}

		/// <summary>
		/// Translates and scales the BoundingSphereD using a given Matrix.
		/// </summary>
		/// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphereD.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
		public BoundingSphereD Transform(MatrixD matrix)
		{
			BoundingSphereD result = default(BoundingSphereD);
			result.Center = Vector3D.Transform(Center, matrix);
			double num = Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33));
			result.Radius = Radius * Math.Sqrt(num);
			return result;
		}

		/// <summary>
		/// Translates and scales the BoundingSphereD using a given Matrix.
		/// </summary>
		/// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphereD.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param><param name="result">[OutAttribute] The transformed BoundingSphereD.</param>
		public void Transform(ref MatrixD matrix, out BoundingSphereD result)
		{
			result.Center = Vector3D.Transform(Center, matrix);
			double num = Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33));
			result.Radius = Radius * Math.Sqrt(num);
		}

		public bool IntersectRaySphere(RayD ray, out double tmin, out double tmax)
		{
			tmin = 0.0;
			tmax = 0.0;
			Vector3D v = ray.Position - Center;
			double num = ray.Direction.Dot(ray.Direction);
			double num2 = 2.0 * v.Dot(ray.Direction);
			double num3 = v.Dot(v) - Radius * Radius;
			double num4 = num2 * num2 - 4.0 * num * num3;
			if (num4 < 0.0)
			{
				return false;
			}
			tmin = (0.0 - num2 - Math.Sqrt(num4)) / (2.0 * num);
			tmax = (0.0 - num2 + Math.Sqrt(num4)) / (2.0 * num);
			if (tmin > tmax)
			{
				double num5 = tmin;
				tmin = tmax;
				tmax = num5;
			}
			return true;
		}

		public BoundingSphereD Include(BoundingSphereD sphere)
		{
			Include(ref this, ref sphere);
			return this;
		}

		public static void Include(ref BoundingSphereD sphere, ref BoundingSphereD otherSphere)
		{
			if (sphere.Radius == double.MinValue)
			{
				sphere.Center = otherSphere.Center;
				sphere.Radius = otherSphere.Radius;
				return;
			}
			double num = Vector3D.Distance(sphere.Center, otherSphere.Center);
			if (!(num + otherSphere.Radius <= sphere.Radius))
			{
				if (num + sphere.Radius <= otherSphere.Radius)
				{
					sphere = otherSphere;
					return;
				}
				double amount = (num + otherSphere.Radius - sphere.Radius) / (2.0 * num);
				Vector3D center = Vector3D.Lerp(sphere.Center, otherSphere.Center, amount);
				double radius = (num + sphere.Radius + otherSphere.Radius) / 2.0;
				sphere.Center = center;
				sphere.Radius = radius;
			}
		}

		public static BoundingSphereD CreateInvalid()
		{
			return new BoundingSphereD(Vector3D.Zero, double.MinValue);
		}

		public static implicit operator BoundingSphereD(BoundingSphere b)
		{
			return new BoundingSphereD(b.Center, b.Radius);
		}

		public static implicit operator BoundingSphere(BoundingSphereD b)
		{
			return new BoundingSphere(b.Center, (float)b.Radius);
		}

		public Vector3D RandomToUniformPointInSphere(double ranX, double ranY, double ranZ)
		{
			double num = Math.PI * 2.0 * ranX;
			double num2 = Math.Acos(2.0 * ranY - 1.0);
			double num3 = Math.Pow(ranZ, 0.33333333333333331);
			double num4 = Radius * num3;
			return new Vector3D(num4 * Math.Cos(num) * Math.Sin(num2), num4 * Math.Sin(num) * Math.Sin(num2), num4 * Math.Cos(num2)) + Center;
		}

		/// <summary>
		///  Similar to RandomToUniformPointInSphere(...) but excludes points within distance of cutoutRadius from center. (Results are randomly distributed in the shape that remains from sphere that had another sphere cut out from center. )
		/// </summary>
		/// <param name="sphere"></param>
		/// <param name="ranX"></param>
		/// <param name="ranY"></param>
		/// <param name="ranZ"></param>
		/// <param name="cutoutRadius"></param>
		/// <returns></returns>
		public Vector3D? RandomToUniformPointInSphereWithInnerCutout(double ranX, double ranY, double ranZ, double cutoutRadius)
		{
			if (cutoutRadius < 0.0 || cutoutRadius >= Radius || Radius <= 0.0)
			{
				return null;
			}
			double num = cutoutRadius / Radius;
			double num2 = (Radius - cutoutRadius) / Radius;
			double ranZ2 = ranZ * num2 + num;
			return RandomToUniformPointInSphere(ranX, ranY, ranZ2);
		}

		public Vector3D RandomToUniformPointOnSphere(double ranX, double ranY)
		{
			double num = Math.PI * 2.0 * ranX;
			double num2 = Math.Acos(2.0 * ranY - 1.0);
			double radius = Radius;
			return new Vector3D(radius * Math.Cos(num) * Math.Sin(num2), radius * Math.Sin(num) * Math.Sin(num2), radius * Math.Cos(num2)) + Center;
		}
	}
}
