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
	public struct Ray : IEquatable<Ray>
	{
		protected class VRageMath_Ray_003C_003EPosition_003C_003EAccessor : IMemberAccessor<Ray, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Ray owner, in Vector3 value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Ray owner, out Vector3 value)
			{
				value = owner.Position;
			}
		}

		protected class VRageMath_Ray_003C_003EDirection_003C_003EAccessor : IMemberAccessor<Ray, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Ray owner, in Vector3 value)
			{
				owner.Direction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Ray owner, out Vector3 value)
			{
				value = owner.Direction;
			}
		}

		/// <summary>
		/// Specifies the starting point of the Ray.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// Unit vector specifying the direction the Ray is pointing.
		/// </summary>
		public Vector3 Direction;

		/// <summary>
		/// Creates a new instance of Ray.
		/// </summary>
		/// <param name="position">The starting point of the Ray.</param><param name="direction">Unit vector describing the direction of the Ray.</param>
		public Ray(Vector3 position, Vector3 direction)
		{
			Position = position;
			Direction = direction;
		}

		/// <summary>
		/// Determines whether two instances of Ray are equal.
		/// </summary>
		/// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
		public static bool operator ==(Ray a, Ray b)
		{
			if ((double)a.Position.X == (double)b.Position.X && (double)a.Position.Y == (double)b.Position.Y && (double)a.Position.Z == (double)b.Position.Z && (double)a.Direction.X == (double)b.Direction.X && (double)a.Direction.Y == (double)b.Direction.Y)
			{
				return (double)a.Direction.Z == (double)b.Direction.Z;
			}
			return false;
		}

		/// <summary>
		/// Determines whether two instances of Ray are not equal.
		/// </summary>
		/// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
		public static bool operator !=(Ray a, Ray b)
		{
			if ((double)a.Position.X == (double)b.Position.X && (double)a.Position.Y == (double)b.Position.Y && (double)a.Position.Z == (double)b.Position.Z && (double)a.Direction.X == (double)b.Direction.X && (double)a.Direction.Y == (double)b.Direction.Y)
			{
				return (double)a.Direction.Z != (double)b.Direction.Z;
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified Ray is equal to the current Ray.
		/// </summary>
		/// <param name="other">The Ray to compare with the current Ray.</param>
		public bool Equals(Ray other)
		{
			if ((double)Position.X == (double)other.Position.X && (double)Position.Y == (double)other.Position.Y && (double)Position.Z == (double)other.Position.Z && (double)Direction.X == (double)other.Direction.X && (double)Direction.Y == (double)other.Direction.Y)
			{
				return (double)Direction.Z == (double)other.Direction.Z;
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
			if (obj != null && obj is Ray)
			{
				result = Equals((Ray)obj);
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
		public float? Intersects(BoundingBox box)
		{
			return box.Intersects(this);
		}

		/// <summary>
		/// Checks whether the current Ray intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox or null if there is no intersection.</param>
		public void Intersects(ref BoundingBox box, out float? result)
		{
			box.Intersects(ref this, out result);
		}

		/// <summary>
		/// Checks whether the Ray intersects a specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection with the Ray.</param>
		public float? Intersects(BoundingFrustum frustum)
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
		public float? Intersects(Plane plane)
		{
			float num = (float)((double)plane.Normal.X * (double)Direction.X + (double)plane.Normal.Y * (double)Direction.Y + (double)plane.Normal.Z * (double)Direction.Z);
			if ((double)Math.Abs(num) < 9.99999974737875E-06)
			{
				return null;
			}
			float num2 = (float)((double)plane.Normal.X * (double)Position.X + (double)plane.Normal.Y * (double)Position.Y + (double)plane.Normal.Z * (double)Position.Z);
			float num3 = (0f - plane.D - num2) / num;
			if ((double)num3 < 0.0)
			{
				if ((double)num3 < -9.99999974737875E-06)
				{
					return null;
				}
				num3 = 0f;
			}
			return num3;
		}

		/// <summary>
		/// Determines whether this Ray intersects a specified Plane.
		/// </summary>
		/// <param name="plane">The Plane with which to calculate this Ray's intersection.</param><param name="result">[OutAttribute] The distance at which this Ray intersects the specified Plane, or null if there is no intersection.</param>
		public void Intersects(ref Plane plane, out float? result)
		{
			float num = (float)((double)plane.Normal.X * (double)Direction.X + (double)plane.Normal.Y * (double)Direction.Y + (double)plane.Normal.Z * (double)Direction.Z);
			if ((double)Math.Abs(num) < 9.99999974737875E-06)
			{
				result = null;
				return;
			}
			float num2 = (float)((double)plane.Normal.X * (double)Position.X + (double)plane.Normal.Y * (double)Position.Y + (double)plane.Normal.Z * (double)Position.Z);
			float num3 = (0f - plane.D - num2) / num;
			if ((double)num3 < 0.0)
			{
				if ((double)num3 < -9.99999974737875E-06)
				{
					result = null;
					return;
				}
				result = 0f;
			}
			result = num3;
		}

		/// <summary>
		/// Checks whether the Ray intersects a specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with the Ray.</param>
		public float? Intersects(BoundingSphere sphere)
		{
			float num = sphere.Center.X - Position.X;
			float num2 = sphere.Center.Y - Position.Y;
			float num3 = sphere.Center.Z - Position.Z;
			float num4 = (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3);
			float num5 = sphere.Radius * sphere.Radius;
			if ((double)num4 <= (double)num5)
			{
				return 0f;
			}
			float num6 = (float)((double)num * (double)Direction.X + (double)num2 * (double)Direction.Y + (double)num3 * (double)Direction.Z);
			if ((double)num6 < 0.0)
			{
				return null;
			}
			float num7 = num4 - num6 * num6;
			if ((double)num7 > (double)num5)
			{
				return null;
			}
			float num8 = (float)Math.Sqrt((double)num5 - (double)num7);
			return num6 - num8;
		}

		/// <summary>
		/// Checks whether the current Ray intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
		public void Intersects(ref BoundingSphere sphere, out float? result)
		{
			float num = sphere.Center.X - Position.X;
			float num2 = sphere.Center.Y - Position.Y;
			float num3 = sphere.Center.Z - Position.Z;
			float num4 = (float)((double)num * (double)num + (double)num2 * (double)num2 + (double)num3 * (double)num3);
			float num5 = sphere.Radius * sphere.Radius;
			if ((double)num4 <= (double)num5)
			{
				result = 0f;
				return;
			}
			result = null;
			float num6 = (float)((double)num * (double)Direction.X + (double)num2 * (double)Direction.Y + (double)num3 * (double)Direction.Z);
			if (!((double)num6 < 0.0))
			{
				float num7 = num4 - num6 * num6;
				if (!((double)num7 > (double)num5))
				{
					float num8 = (float)Math.Sqrt((double)num5 - (double)num7);
					result = num6 - num8;
				}
			}
		}
	}
}
