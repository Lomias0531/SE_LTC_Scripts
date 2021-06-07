using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a PlaneD.
	/// </summary>
	[Serializable]
	public struct PlaneD : IEquatable<PlaneD>
	{
		protected class VRageMath_PlaneD_003C_003ENormal_003C_003EAccessor : IMemberAccessor<PlaneD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlaneD owner, in Vector3D value)
			{
				owner.Normal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlaneD owner, out Vector3D value)
			{
				value = owner.Normal;
			}
		}

		protected class VRageMath_PlaneD_003C_003ED_003C_003EAccessor : IMemberAccessor<PlaneD, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlaneD owner, in double value)
			{
				owner.D = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlaneD owner, out double value)
			{
				value = owner.D;
			}
		}

		/// <summary>
		/// The normal vector of the PlaneD.
		/// </summary>
		public Vector3D Normal;

		/// <summary>
		/// The distance of the PlaneD along its normal from the origin.
		/// Note: Be careful! The distance is signed and is the opposite of what people usually expect.
		///       If you look closely at the plane equation: (n dot P) + D = 0, you'll realize that D = - (n dot P) (that is, negative instead of positive)
		/// </summary>
		public double D;

		private static MyRandom _random;

		/// <summary>
		/// Creates a new instance of PlaneD.
		/// </summary>
		/// <param name="a">X component of the normal defining the PlaneD.</param><param name="b">Y component of the normal defining the PlaneD.</param><param name="c">Z component of the normal defining the PlaneD.</param><param name="d">Distance of the origin from the PlaneD along its normal.</param>
		public PlaneD(double a, double b, double c, double d)
		{
			Normal.X = a;
			Normal.Y = b;
			Normal.Z = c;
			D = d;
		}

		/// <summary>
		/// Creates a new instance of PlaneD.
		/// </summary>
		/// <param name="normal">The normal vector to the PlaneD.</param><param name="d">The distance of the origin from the PlaneD along its normal.</param>
		public PlaneD(Vector3D normal, double d)
		{
			Normal = normal;
			D = d;
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="position">A point that lies on the Plane</param><param name="normal">The normal vector to the Plane.</param>
		public PlaneD(Vector3D position, Vector3D normal)
		{
			Normal = normal;
			D = 0.0 - Vector3D.Dot(position, normal);
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="position">A point that lies on the Plane</param><param name="normal">The normal vector to the Plane.</param>
		public PlaneD(Vector3D position, Vector3 normal)
		{
			Normal = normal;
			D = 0.0 - Vector3D.Dot(position, normal);
		}

		/// <summary>
		/// Creates a new instance of PlaneD.
		/// </summary>
		/// <param name="value">Vector4 with X, Y, and Z components defining the normal of the PlaneD. The W component defines the distance of the origin from the PlaneD along its normal.</param>
		public PlaneD(Vector4 value)
		{
			Normal.X = value.X;
			Normal.Y = value.Y;
			Normal.Z = value.Z;
			D = value.W;
		}

		/// <summary>
		/// Creates a new instance of PlaneD.
		/// </summary>
		/// <param name="point1">One point of a triangle defining the PlaneD.</param><param name="point2">One point of a triangle defining the PlaneD.</param><param name="point3">One point of a triangle defining the PlaneD.</param>
		public PlaneD(Vector3D point1, Vector3D point2, Vector3D point3)
		{
			double num = point2.X - point1.X;
			double num2 = point2.Y - point1.Y;
			double num3 = point2.Z - point1.Z;
			double num4 = point3.X - point1.X;
			double num5 = point3.Y - point1.Y;
			double num6 = point3.Z - point1.Z;
			double num7 = num2 * num6 - num3 * num5;
			double num8 = num3 * num4 - num * num6;
			double num9 = num * num5 - num2 * num4;
			double num10 = 1.0 / Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
			Normal.X = num7 * num10;
			Normal.Y = num8 * num10;
			Normal.Z = num9 * num10;
			D = 0.0 - (Normal.X * point1.X + Normal.Y * point1.Y + Normal.Z * point1.Z);
		}

		/// <summary>
		/// Determines whether two instances of PlaneD are equal.
		/// </summary>
		/// <param name="lhs">The object to the left of the equality operator.</param><param name="rhs">The object to the right of the equality operator.</param>
		public static bool operator ==(PlaneD lhs, PlaneD rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Determines whether two instances of PlaneD are not equal.
		/// </summary>
		/// <param name="lhs">The object to the left of the inequality operator.</param><param name="rhs">The object to the right of the inequality operator.</param>
		public static bool operator !=(PlaneD lhs, PlaneD rhs)
		{
			if (lhs.Normal.X == rhs.Normal.X && lhs.Normal.Y == rhs.Normal.Y && lhs.Normal.Z == rhs.Normal.Z)
			{
				return lhs.D != rhs.D;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified PlaneD is equal to the PlaneD.
		/// </summary>
		/// <param name="other">The PlaneD to compare with the current PlaneD.</param>
		public bool Equals(PlaneD other)
		{
			if (Normal.X == other.Normal.X && Normal.Y == other.Normal.Y && Normal.Z == other.Normal.Z)
			{
				return D == other.D;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the PlaneD.
		/// </summary>
		/// <param name="obj">The Object to compare with the current PlaneD.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is PlaneD)
			{
				result = Equals((PlaneD)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this object.
		/// </summary>
		public override int GetHashCode()
		{
			return Normal.GetHashCode() + D.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current PlaneD.
		/// </summary>
		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{Normal:{0} D:{1}}}", new object[2]
			{
				Normal.ToString(),
				D.ToString(currentCulture)
			});
		}

		/// <summary>
		/// Changes the coefficients of the Normal vector of this PlaneD to make it of unit length.
		/// </summary>
		public void Normalize()
		{
			double num = Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z;
			if (!(Math.Abs(num - 1.0) < 1.19209289550781E-07))
			{
				double num2 = 1.0 / Math.Sqrt(num);
				Normal.X *= num2;
				Normal.Y *= num2;
				Normal.Z *= num2;
				D *= num2;
			}
		}

		/// <summary>
		/// Changes the coefficients of the Normal vector of a PlaneD to make it of unit length.
		/// </summary>
		/// <param name="value">The PlaneD to normalize.</param>
		public static PlaneD Normalize(PlaneD value)
		{
			double num = value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z;
			if (Math.Abs(num - 1.0) < 1.19209289550781E-07)
			{
				PlaneD result = default(PlaneD);
				result.Normal = value.Normal;
				result.D = value.D;
				return result;
			}
			double num2 = 1.0 / Math.Sqrt(num);
			PlaneD result2 = default(PlaneD);
			result2.Normal.X = value.Normal.X * num2;
			result2.Normal.Y = value.Normal.Y * num2;
			result2.Normal.Z = value.Normal.Z * num2;
			result2.D = value.D * num2;
			return result2;
		}

		/// <summary>
		/// Changes the coefficients of the Normal vector of a PlaneD to make it of unit length.
		/// </summary>
		/// <param name="value">The PlaneD to normalize.</param><param name="result">[OutAttribute] An existing PlaneD PlaneD filled in with a normalized version of the specified PlaneD.</param>
		public static void Normalize(ref PlaneD value, out PlaneD result)
		{
			double num = value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z;
			if (Math.Abs(num - 1.0) < 1.19209289550781E-07)
			{
				result.Normal = value.Normal;
				result.D = value.D;
				return;
			}
			double num2 = 1.0 / Math.Sqrt(num);
			result.Normal.X = value.Normal.X * num2;
			result.Normal.Y = value.Normal.Y * num2;
			result.Normal.Z = value.Normal.Z * num2;
			result.D = value.D * num2;
		}

		/// <summary>
		/// Transforms a normalized PlaneD by a Matrix.
		/// </summary>
		/// <param name="PlaneD">The normalized PlaneD to transform. This PlaneD must already be normalized, so that its Normal vector is of unit length, before this method is called.</param><param name="matrix">The transform Matrix to apply to the PlaneD.</param>
		public static PlaneD Transform(PlaneD plane, MatrixD matrix)
		{
			Transform(ref plane, ref matrix, out PlaneD result);
			return result;
		}

		/// <summary>
		/// Transforms a normalized PlaneD by a Matrix.
		/// </summary>
		/// <param name="PlaneD">The normalized PlaneD to transform. This PlaneD must already be normalized, so that its Normal vector is of unit length, before this method is called.</param><param name="matrix">The transform Matrix to apply to the PlaneD.</param><param name="result">[OutAttribute] An existing PlaneD filled in with the results of applying the transform.</param>
		public static void Transform(ref PlaneD plane, ref MatrixD matrix, out PlaneD result)
		{
			result = default(PlaneD);
			Vector3D position = -plane.Normal * plane.D;
			Vector3D.TransformNormal(ref plane.Normal, ref matrix, out result.Normal);
			Vector3D.Transform(ref position, ref matrix, out position);
			Vector3D.Dot(ref position, ref result.Normal, out result.D);
			result.D = 0.0 - result.D;
		}

		/// <summary>
		/// Calculates the dot product of a specified Vector4 and this PlaneD.
		/// </summary>
		/// <param name="value">The Vector4 to multiply this PlaneD by.</param>
		public double Dot(Vector4 value)
		{
			return Normal.X * (double)value.X + Normal.Y * (double)value.Y + Normal.Z * (double)value.Z + D * (double)value.W;
		}

		/// <summary>
		/// Calculates the dot product of a specified Vector4 and this PlaneD.
		/// </summary>
		/// <param name="value">The Vector4 to multiply this PlaneD by.</param><param name="result">[OutAttribute] The dot product of the specified Vector4 and this PlaneD.</param>
		public void Dot(ref Vector4 value, out double result)
		{
			result = Normal.X * (double)value.X + Normal.Y * (double)value.Y + Normal.Z * (double)value.Z + D * (double)value.W;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3D and the Normal vector of this PlaneD plus the distance (D) value of the PlaneD.
		/// </summary>
		/// <param name="value">The Vector3D to multiply by.</param>
		public double DotCoordinate(Vector3D value)
		{
			return Normal.X * value.X + Normal.Y * value.Y + Normal.Z * value.Z + D;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3D and the Normal vector of this PlaneD plus the distance (D) value of the PlaneD.
		/// </summary>
		/// <param name="value">The Vector3D to multiply by.</param><param name="result">[OutAttribute] The resulting value.</param>
		public void DotCoordinate(ref Vector3D value, out double result)
		{
			result = Normal.X * value.X + Normal.Y * value.Y + Normal.Z * value.Z + D;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3D and the Normal vector of this PlaneD.
		/// </summary>
		/// <param name="value">The Vector3D to multiply by.</param>
		public double DotNormal(Vector3D value)
		{
			return Normal.X * value.X + Normal.Y * value.Y + Normal.Z * value.Z;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3D and the Normal vector of this PlaneD.
		/// </summary>
		/// <param name="value">The Vector3D to multiply by.</param><param name="result">[OutAttribute] The resulting dot product.</param>
		public void DotNormal(ref Vector3D value, out double result)
		{
			result = Normal.X * value.X + Normal.Y * value.Y + Normal.Z * value.Z;
		}

		/// <summary>
		/// Checks whether the current PlaneD intersects a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingBoxD box)
		{
			Vector3D vector3D = default(Vector3D);
			vector3D.X = ((Normal.X >= 0.0) ? box.Min.X : box.Max.X);
			vector3D.Y = ((Normal.Y >= 0.0) ? box.Min.Y : box.Max.Y);
			vector3D.Z = ((Normal.Z >= 0.0) ? box.Min.Z : box.Max.Z);
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = ((Normal.X >= 0.0) ? box.Max.X : box.Min.X);
			vector3D2.Y = ((Normal.Y >= 0.0) ? box.Max.Y : box.Min.Y);
			vector3D2.Z = ((Normal.Z >= 0.0) ? box.Max.Z : box.Min.Z);
			if (Normal.X * vector3D.X + Normal.Y * vector3D.Y + Normal.Z * vector3D.Z + D > 0.0)
			{
				return PlaneIntersectionType.Front;
			}
			if (!(Normal.X * vector3D2.X + Normal.Y * vector3D2.Y + Normal.Z * vector3D2.Z + D < 0.0))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current PlaneD intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the PlaneD intersects the BoundingBox.</param>
		public void Intersects(ref BoundingBoxD box, out PlaneIntersectionType result)
		{
			Vector3D vector3D = default(Vector3D);
			vector3D.X = ((Normal.X >= 0.0) ? box.Min.X : box.Max.X);
			vector3D.Y = ((Normal.Y >= 0.0) ? box.Min.Y : box.Max.Y);
			vector3D.Z = ((Normal.Z >= 0.0) ? box.Min.Z : box.Max.Z);
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = ((Normal.X >= 0.0) ? box.Max.X : box.Min.X);
			vector3D2.Y = ((Normal.Y >= 0.0) ? box.Max.Y : box.Min.Y);
			vector3D2.Z = ((Normal.Z >= 0.0) ? box.Max.Z : box.Min.Z);
			if (Normal.X * vector3D.X + Normal.Y * vector3D.Y + Normal.Z * vector3D.Z + D > 0.0)
			{
				result = PlaneIntersectionType.Front;
			}
			else if (Normal.X * vector3D2.X + Normal.Y * vector3D2.Y + Normal.Z * vector3D2.Z + D < 0.0)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
		}

		/// <summary>
		/// Checks whether the current PlaneD intersects a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingFrustumD frustum)
		{
			return frustum.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current PlaneD intersects a specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingSphereD sphere)
		{
			double num = sphere.Center.X * Normal.X + sphere.Center.Y * Normal.Y + sphere.Center.Z * Normal.Z + D;
			if (num > sphere.Radius)
			{
				return PlaneIntersectionType.Front;
			}
			if (!(num < 0.0 - sphere.Radius))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current PlaneD intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the PlaneD intersects the BoundingSphere.</param>
		public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
		{
			double num = (double)sphere.Center.X * Normal.X + (double)sphere.Center.Y * Normal.Y + (double)sphere.Center.Z * Normal.Z + D;
			if (num > (double)sphere.Radius)
			{
				result = PlaneIntersectionType.Front;
			}
			else if (num < 0.0 - (double)sphere.Radius)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
		}

		public Vector3D RandomPoint()
		{
			if (_random == null)
			{
				_random = new MyRandom();
			}
			Vector3D vector = default(Vector3D);
			Vector3D vector3D;
			do
			{
				vector.X = 2.0 * _random.NextDouble() - 1.0;
				vector.Y = 2.0 * _random.NextDouble() - 1.0;
				vector.Z = 2.0 * _random.NextDouble() - 1.0;
				vector3D = Vector3D.Cross(vector, Normal);
			}
			while (vector3D == Vector3D.Zero);
			vector3D.Normalize();
			return vector3D * Math.Sqrt(_random.NextDouble());
		}

		public double DistanceToPoint(Vector3D point)
		{
			return Vector3D.Dot(Normal, point) + D;
		}

		public double DistanceToPoint(ref Vector3D point)
		{
			return Vector3D.Dot(Normal, point) + D;
		}

		public Vector3D ProjectPoint(ref Vector3D point)
		{
			return point - Normal * DistanceToPoint(ref point);
		}

		/// <summary>
		/// Gets intersection point in Plane.
		/// </summary>
		/// <param name="from">Starting point of a ray.</param>
		/// <param name="direction">Ray direction.</param>
		/// <returns>Point of intersection.</returns>
		public Vector3D Intersection(ref Vector3D from, ref Vector3D direction)
		{
			double scaleFactor = (0.0 - (DotNormal(from) + D)) / DotNormal(direction);
			return from + scaleFactor * direction;
		}
	}
}
