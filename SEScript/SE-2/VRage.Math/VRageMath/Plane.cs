using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a plane.
	/// </summary>
	[Serializable]
	public struct Plane : IEquatable<Plane>
	{
		protected class VRageMath_Plane_003C_003ENormal_003C_003EAccessor : IMemberAccessor<Plane, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Plane owner, in Vector3 value)
			{
				owner.Normal = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Plane owner, out Vector3 value)
			{
				value = owner.Normal;
			}
		}

		protected class VRageMath_Plane_003C_003ED_003C_003EAccessor : IMemberAccessor<Plane, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Plane owner, in float value)
			{
				owner.D = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Plane owner, out float value)
			{
				value = owner.D;
			}
		}

		/// <summary>
		/// The normal vector of the Plane.
		/// </summary>
		public Vector3 Normal;

		/// <summary>
		/// The distance of the Plane along its normal from the origin.
		/// Note: Be careful! The distance is signed and is the opposite of what people usually expect.
		///       If you look closely at the plane equation: (n dot P) - D = 0, you'll realize that D = - (n dot P) (that is, negative instead of positive)
		/// </summary>
		public float D;

		private static MyRandom _random;

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="a">X component of the normal defining the Plane.</param><param name="b">Y component of the normal defining the Plane.</param><param name="c">Z component of the normal defining the Plane.</param><param name="d">Distance of the origin from the plane along its normal.</param>
		public Plane(float a, float b, float c, float d)
		{
			Normal.X = a;
			Normal.Y = b;
			Normal.Z = c;
			D = d;
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="normal">The normal vector to the Plane.</param><param name="d">Distance of the origin from the plane along its normal.</param>
		public Plane(Vector3 normal, float d)
		{
			Normal = normal;
			D = d;
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="position">A point that lies on the Plane</param><param name="normal">The normal vector to the Plane.</param>
		public Plane(Vector3 position, Vector3 normal)
		{
			Normal = normal;
			D = 0f - Vector3.Dot(position, normal);
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="value">Vector4 with X, Y, and Z components defining the normal of the Plane. The W component defines the distance of the origin from the plane along its normal.</param>
		public Plane(Vector4 value)
		{
			Normal.X = value.X;
			Normal.Y = value.Y;
			Normal.Z = value.Z;
			D = value.W;
		}

		/// <summary>
		/// Creates a new instance of Plane.
		/// </summary>
		/// <param name="point1">One point of a triangle defining the Plane.</param><param name="point2">One point of a triangle defining the Plane.</param><param name="point3">One point of a triangle defining the Plane.</param>
		public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			float num = point2.X - point1.X;
			float num2 = point2.Y - point1.Y;
			float num3 = point2.Z - point1.Z;
			float num4 = point3.X - point1.X;
			float num5 = point3.Y - point1.Y;
			float num6 = point3.Z - point1.Z;
			float num7 = num2 * num6 - num3 * num5;
			float num8 = num3 * num4 - num * num6;
			float num9 = num * num5 - num2 * num4;
			float num10 = 1f / (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
			Normal.X = num7 * num10;
			Normal.Y = num8 * num10;
			Normal.Z = num9 * num10;
			D = 0f - (Normal.X * point1.X + Normal.Y * point1.Y + Normal.Z * point1.Z);
		}

		public Plane(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3)
		{
			float num = point2.X - point1.X;
			float num2 = point2.Y - point1.Y;
			float num3 = point2.Z - point1.Z;
			float num4 = point3.X - point1.X;
			float num5 = point3.Y - point1.Y;
			float num6 = point3.Z - point1.Z;
			float num7 = num2 * num6 - num3 * num5;
			float num8 = num3 * num4 - num * num6;
			float num9 = num * num5 - num2 * num4;
			float num10 = 1f / (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
			Normal.X = num7 * num10;
			Normal.Y = num8 * num10;
			Normal.Z = num9 * num10;
			D = 0f - (Normal.X * point1.X + Normal.Y * point1.Y + Normal.Z * point1.Z);
		}

		/// <summary>
		/// Determines whether two instances of Plane are equal.
		/// </summary>
		/// <param name="lhs">The object to the left of the equality operator.</param><param name="rhs">The object to the right of the equality operator.</param>
		public static bool operator ==(Plane lhs, Plane rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Determines whether two instances of Plane are not equal.
		/// </summary>
		/// <param name="lhs">The object to the left of the inequality operator.</param><param name="rhs">The object to the right of the inequality operator.</param>
		public static bool operator !=(Plane lhs, Plane rhs)
		{
			if ((double)lhs.Normal.X == (double)rhs.Normal.X && (double)lhs.Normal.Y == (double)rhs.Normal.Y && (double)lhs.Normal.Z == (double)rhs.Normal.Z)
			{
				return (double)lhs.D != (double)rhs.D;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified Plane is equal to the Plane.
		/// </summary>
		/// <param name="other">The Plane to compare with the current Plane.</param>
		public bool Equals(Plane other)
		{
			if ((double)Normal.X == (double)other.Normal.X && (double)Normal.Y == (double)other.Normal.Y && (double)Normal.Z == (double)other.Normal.Z)
			{
				return (double)D == (double)other.D;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the Plane.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Plane.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Plane)
			{
				result = Equals((Plane)obj);
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
		/// Returns a String that represents the current Plane.
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
		/// Changes the coefficients of the Normal vector of this Plane to make it of unit length.
		/// </summary>
		public void Normalize()
		{
			float num = (float)((double)Normal.X * (double)Normal.X + (double)Normal.Y * (double)Normal.Y + (double)Normal.Z * (double)Normal.Z);
			if (!((double)Math.Abs(num - 1f) < 1.19209289550781E-07))
			{
				float num2 = 1f / (float)Math.Sqrt(num);
				Normal.X *= num2;
				Normal.Y *= num2;
				Normal.Z *= num2;
				D *= num2;
			}
		}

		/// <summary>
		/// Changes the coefficients of the Normal vector of a Plane to make it of unit length.
		/// </summary>
		/// <param name="value">The Plane to normalize.</param>
		public static Plane Normalize(Plane value)
		{
			float num = (float)((double)value.Normal.X * (double)value.Normal.X + (double)value.Normal.Y * (double)value.Normal.Y + (double)value.Normal.Z * (double)value.Normal.Z);
			if ((double)Math.Abs(num - 1f) < 1.19209289550781E-07)
			{
				Plane result = default(Plane);
				result.Normal = value.Normal;
				result.D = value.D;
				return result;
			}
			float num2 = 1f / (float)Math.Sqrt(num);
			Plane result2 = default(Plane);
			result2.Normal.X = value.Normal.X * num2;
			result2.Normal.Y = value.Normal.Y * num2;
			result2.Normal.Z = value.Normal.Z * num2;
			result2.D = value.D * num2;
			return result2;
		}

		/// <summary>
		/// Changes the coefficients of the Normal vector of a Plane to make it of unit length.
		/// </summary>
		/// <param name="value">The Plane to normalize.</param><param name="result">[OutAttribute] An existing plane Plane filled in with a normalized version of the specified plane.</param>
		public static void Normalize(ref Plane value, out Plane result)
		{
			float num = (float)((double)value.Normal.X * (double)value.Normal.X + (double)value.Normal.Y * (double)value.Normal.Y + (double)value.Normal.Z * (double)value.Normal.Z);
			if ((double)Math.Abs(num - 1f) < 1.19209289550781E-07)
			{
				result.Normal = value.Normal;
				result.D = value.D;
				return;
			}
			float num2 = 1f / (float)Math.Sqrt(num);
			result.Normal.X = value.Normal.X * num2;
			result.Normal.Y = value.Normal.Y * num2;
			result.Normal.Z = value.Normal.Z * num2;
			result.D = value.D * num2;
		}

		/// <summary>
		/// Transforms a normalized Plane by a Matrix.
		/// </summary>
		/// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param><param name="matrix">The transform Matrix to apply to the Plane.</param>
		public static Plane Transform(Plane plane, Matrix matrix)
		{
			Transform(ref plane, ref matrix, out Plane result);
			return result;
		}

		/// <summary>
		/// Transforms a normalized Plane by a Matrix.
		/// </summary>
		/// <param name="plane">The normalized Plane to transform. This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param><param name="matrix">The transform Matrix to apply to the Plane.</param><param name="result">[OutAttribute] An existing Plane filled in with the results of applying the transform.</param>
		public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
		{
			result = default(Plane);
			Vector3 position = -plane.Normal * plane.D;
			Vector3.TransformNormal(ref plane.Normal, ref matrix, out result.Normal);
			Vector3.Transform(ref position, ref matrix, out position);
			Vector3.Dot(ref position, ref result.Normal, out result.D);
			result.D = 0f - result.D;
		}

		/// <summary>
		/// Calculates the dot product of a specified Vector4 and this Plane.
		/// </summary>
		/// <param name="value">The Vector4 to multiply this Plane by.</param>
		public float Dot(Vector4 value)
		{
			return (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z + (double)D * (double)value.W);
		}

		/// <summary>
		/// Calculates the dot product of a specified Vector4 and this Plane.
		/// </summary>
		/// <param name="value">The Vector4 to multiply this Plane by.</param><param name="result">[OutAttribute] The dot product of the specified Vector4 and this Plane.</param>
		public void Dot(ref Vector4 value, out float result)
		{
			result = (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z + (double)D * (double)value.W);
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the Normal vector of this Plane plus the distance (D) value of the Plane.
		/// </summary>
		/// <param name="value">The Vector3 to multiply by.</param>
		public float DotCoordinate(Vector3 value)
		{
			return (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z) + D;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the Normal vector of this Plane plus the distance (D) value of the Plane.
		/// </summary>
		/// <param name="value">The Vector3 to multiply by.</param><param name="result">[OutAttribute] The resulting value.</param>
		public void DotCoordinate(ref Vector3 value, out float result)
		{
			result = (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z) + D;
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the Normal vector of this Plane.
		/// </summary>
		/// <param name="value">The Vector3 to multiply by.</param>
		public float DotNormal(Vector3 value)
		{
			return (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z);
		}

		/// <summary>
		/// Returns the dot product of a specified Vector3 and the Normal vector of this Plane.
		/// </summary>
		/// <param name="value">The Vector3 to multiply by.</param><param name="result">[OutAttribute] The resulting dot product.</param>
		public void DotNormal(ref Vector3 value, out float result)
		{
			result = (float)((double)Normal.X * (double)value.X + (double)Normal.Y * (double)value.Y + (double)Normal.Z * (double)value.Z);
		}

		/// <summary>
		/// Checks whether the current Plane intersects a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingBox box)
		{
			Vector3 vector = default(Vector3);
			vector.X = (((double)Normal.X >= 0.0) ? box.Min.X : box.Max.X);
			vector.Y = (((double)Normal.Y >= 0.0) ? box.Min.Y : box.Max.Y);
			vector.Z = (((double)Normal.Z >= 0.0) ? box.Min.Z : box.Max.Z);
			Vector3 vector2 = default(Vector3);
			vector2.X = (((double)Normal.X >= 0.0) ? box.Max.X : box.Min.X);
			vector2.Y = (((double)Normal.Y >= 0.0) ? box.Max.Y : box.Min.Y);
			vector2.Z = (((double)Normal.Z >= 0.0) ? box.Max.Z : box.Min.Z);
			if ((double)Normal.X * (double)vector.X + (double)Normal.Y * (double)vector.Y + (double)Normal.Z * (double)vector.Z + (double)D > 0.0)
			{
				return PlaneIntersectionType.Front;
			}
			if (!((double)Normal.X * (double)vector2.X + (double)Normal.Y * (double)vector2.Y + (double)Normal.Z * (double)vector2.Z + (double)D < 0.0))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current Plane intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the Plane intersects the BoundingBox.</param>
		public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
		{
			Vector3 vector = default(Vector3);
			vector.X = (((double)Normal.X >= 0.0) ? box.Min.X : box.Max.X);
			vector.Y = (((double)Normal.Y >= 0.0) ? box.Min.Y : box.Max.Y);
			vector.Z = (((double)Normal.Z >= 0.0) ? box.Min.Z : box.Max.Z);
			Vector3 vector2 = default(Vector3);
			vector2.X = (((double)Normal.X >= 0.0) ? box.Max.X : box.Min.X);
			vector2.Y = (((double)Normal.Y >= 0.0) ? box.Max.Y : box.Min.Y);
			vector2.Z = (((double)Normal.Z >= 0.0) ? box.Max.Z : box.Min.Z);
			if ((double)Normal.X * (double)vector.X + (double)Normal.Y * (double)vector.Y + (double)Normal.Z * (double)vector.Z + (double)D > 0.0)
			{
				result = PlaneIntersectionType.Front;
			}
			else if ((double)Normal.X * (double)vector2.X + (double)Normal.Y * (double)vector2.Y + (double)Normal.Z * (double)vector2.Z + (double)D < 0.0)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
		}

		/// <summary>
		/// Checks whether the current Plane intersects a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingFrustum frustum)
		{
			return frustum.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current Plane intersects a specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param>
		public PlaneIntersectionType Intersects(BoundingSphere sphere)
		{
			float num = (float)((double)sphere.Center.X * (double)Normal.X + (double)sphere.Center.Y * (double)Normal.Y + (double)sphere.Center.Z * (double)Normal.Z) + D;
			if ((double)num > (double)sphere.Radius)
			{
				return PlaneIntersectionType.Front;
			}
			if (!((double)num < 0.0 - (double)sphere.Radius))
			{
				return PlaneIntersectionType.Intersecting;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current Plane intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the Plane intersects the BoundingSphere.</param>
		public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
		{
			float num = (float)((double)sphere.Center.X * (double)Normal.X + (double)sphere.Center.Y * (double)Normal.Y + (double)sphere.Center.Z * (double)Normal.Z) + D;
			if ((double)num > (double)sphere.Radius)
			{
				result = PlaneIntersectionType.Front;
			}
			else if ((double)num < 0.0 - (double)sphere.Radius)
			{
				result = PlaneIntersectionType.Back;
			}
			else
			{
				result = PlaneIntersectionType.Intersecting;
			}
		}

		public Vector3 RandomPoint()
		{
			if (_random == null)
			{
				_random = new MyRandom();
			}
			Vector3 vector = default(Vector3);
			Vector3 vector2;
			do
			{
				vector.X = 2f * (float)_random.NextDouble() - 1f;
				vector.Y = 2f * (float)_random.NextDouble() - 1f;
				vector.Z = 2f * (float)_random.NextDouble() - 1f;
				vector2 = Vector3.Cross(vector, Normal);
			}
			while (vector2 == Vector3.Zero);
			vector2.Normalize();
			return vector2 * (float)Math.Sqrt(_random.NextDouble());
		}
	}
}
