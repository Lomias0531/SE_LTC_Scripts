using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a ray.
	/// </summary>
	[Serializable]
	public struct RayD : IEquatable<RayD>
	{
		protected class VRageMath_RayD_003C_003EPosition_003C_003EAccessor : IMemberAccessor<RayD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RayD owner, in Vector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RayD owner, out Vector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRageMath_RayD_003C_003EDirection_003C_003EAccessor : IMemberAccessor<RayD, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref RayD owner, in Vector3D value)
			{
				owner.Direction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref RayD owner, out Vector3D value)
			{
				value = owner.Direction;
			}
		}

		/// <summary>
		/// Specifies the starting point of the Ray.
		/// </summary>
		public Vector3D Position;

		/// <summary>
		/// Unit vector specifying the direction the Ray is pointing.
		/// </summary>
		public Vector3D Direction;

		/// <summary>
		/// Creates a new instance of Ray.
		/// </summary>
		/// <param name="position">The starting point of the Ray.</param><param name="direction">Unit vector describing the direction of the Ray.</param>
		public RayD(Vector3D position, Vector3D direction)
		{
			Position = position;
			Direction = direction;
		}

		public RayD(ref Vector3D position, ref Vector3D direction)
		{
			Position = position;
			Direction = direction;
		}

		/// <summary>
		/// Determines whether two instances of Ray are equal.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(RayD a, RayD b)
		{
			if (a.Position.X == b.Position.X && a.Position.Y == b.Position.Y && a.Position.Z == b.Position.Z && a.Direction.X == b.Direction.X && a.Direction.Y == b.Direction.Y)
			{
				return a.Direction.Z == b.Direction.Z;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of Ray are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(RayD a, RayD b)
		{
			if (a.Position.X == b.Position.X && a.Position.Y == b.Position.Y && a.Position.Z == b.Position.Z && a.Direction.X == b.Direction.X && a.Direction.Y == b.Direction.Y)
			{
				return a.Direction.Z != b.Direction.Z;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified Ray is equal to the current Ray.
		/// </summary>
		/// <param name="other">The Ray to compare with the current Ray.</param>
		public bool Equals(RayD other)
		{
			if (Position.X == other.Position.X && Position.Y == other.Position.Y && Position.Z == other.Position.Z && Direction.X == other.Direction.X && Direction.Y == other.Direction.Y)
			{
				return Direction.Z == other.Direction.Z;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of Ray are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Ray.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj != null && obj is RayD)
			{
				result = Equals((RayD)obj);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return Position.GetHashCode() + Direction.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current Ray.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Position:{0} Direction:{1}}}", new object[2]
			{
				Position.ToString(),
				Direction.ToString()
			});
		}

		/// <summary>
		/// Checks whether the Ray intersects a specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with the Ray.</param>
		public double? Intersects(BoundingBoxD box)
		{
			return box.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current Ray intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox or null if there is no intersection.</param>
		public void Intersects(ref BoundingBoxD box, out double? result)
		{
			box.Intersects(ref this, out result);
		}

		/// <summary>
		/// Checks whether the Ray intersects a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with the Ray.</param>
		public double? Intersects(BoundingFrustumD frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			return frustum.Intersects(this);
		}

		/// <summary>
		/// Determines whether this Ray intersects a specified Plane.
		/// </summary>
		/// <param name="plane">The Plane with which to calculate this Ray's intersection.</param>
		public double? Intersects(PlaneD plane)
		{
			double num = plane.Normal.X * Direction.X + plane.Normal.Y * Direction.Y + plane.Normal.Z * Direction.Z;
			if (Math.Abs(num) < 9.99999974737875E-06)
			{
				return null;
			}
			double num2 = (float)(plane.Normal.X * Position.X + plane.Normal.Y * Position.Y + plane.Normal.Z * Position.Z);
			double num3 = (0.0 - plane.D - num2) / num;
			if (num3 < 0.0)
			{
				if (num3 < -9.99999974737875E-06)
				{
					return null;
				}
				num3 = 0.0;
			}
			return num3;
		}

		/// <summary>
		/// Determines whether this Ray intersects a specified Plane.
		/// </summary>
		/// <param name="plane">The Plane with which to calculate this Ray's intersection.</param><param name="result">[OutAttribute] The distance at which this Ray intersects the specified Plane, or null if there is no intersection.</param>
		public void Intersects(ref PlaneD plane, out double? result)
		{
			double num = plane.Normal.X * Direction.X + plane.Normal.Y * Direction.Y + plane.Normal.Z * Direction.Z;
			if (Math.Abs(num) < 9.99999974737875E-06)
			{
				result = null;
				return;
			}
			double num2 = plane.Normal.X * Position.X + plane.Normal.Y * Position.Y + plane.Normal.Z * Position.Z;
			double num3 = (0.0 - plane.D - num2) / num;
			if (num3 < 0.0)
			{
				if (num3 < -9.99999974737875E-06)
				{
					result = null;
					return;
				}
				result = 0.0;
			}
			result = num3;
		}

		/// <summary>
		/// Checks whether the Ray intersects a specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with the Ray.</param>
		public double? Intersects(BoundingSphereD sphere)
		{
			double num = sphere.Center.X - Position.X;
			double num2 = sphere.Center.Y - Position.Y;
			double num3 = sphere.Center.Z - Position.Z;
			double num4 = num * num + num2 * num2 + num3 * num3;
			double num5 = sphere.Radius * sphere.Radius;
			if (num4 <= num5)
			{
				return 0.0;
			}
			double num6 = num * Direction.X + num2 * Direction.Y + num3 * Direction.Z;
			if (num6 < 0.0)
			{
				return null;
			}
			double num7 = num4 - num6 * num6;
			if (num7 > num5)
			{
				return null;
			}
			double num8 = Math.Sqrt(num5 - num7);
			return num6 - num8;
		}

		/// <summary>
		/// Checks whether the current Ray intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
		public void Intersects(ref BoundingSphere sphere, out double? result)
		{
			double num = (double)sphere.Center.X - Position.X;
			double num2 = (double)sphere.Center.Y - Position.Y;
			double num3 = (double)sphere.Center.Z - Position.Z;
			double num4 = num * num + num2 * num2 + num3 * num3;
			double num5 = sphere.Radius * sphere.Radius;
			if (num4 <= num5)
			{
				result = 0.0;
				return;
			}
			result = null;
			double num6 = num * Direction.X + num2 * Direction.Y + num3 * Direction.Z;
			if (!(num6 < 0.0))
			{
				double num7 = num4 - num6 * num6;
				if (!(num7 > num5))
				{
					double num8 = Math.Sqrt(num5 - num7);
					result = num6 - num8;
				}
			}
		}
	}
}
