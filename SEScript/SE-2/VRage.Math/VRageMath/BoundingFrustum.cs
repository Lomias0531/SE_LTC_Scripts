using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
	/// <summary>
	/// Defines a frustum and helps determine whether forms intersect with it.
	/// </summary>
	[Serializable]
	public class BoundingFrustum : IEquatable<BoundingFrustum>
	{
		protected class VRageMath_BoundingFrustum_003C_003Eplanes_003C_003EAccessor : IMemberAccessor<BoundingFrustum, Plane[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustum owner, in Plane[] value)
			{
				owner.planes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustum owner, out Plane[] value)
			{
				value = owner.planes;
			}
		}

		protected class VRageMath_BoundingFrustum_003C_003EcornerArray_003C_003EAccessor : IMemberAccessor<BoundingFrustum, Vector3[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustum owner, in Vector3[] value)
			{
				owner.cornerArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustum owner, out Vector3[] value)
			{
				value = owner.cornerArray;
			}
		}

		protected class VRageMath_BoundingFrustum_003C_003Ematrix_003C_003EAccessor : IMemberAccessor<BoundingFrustum, Matrix>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustum owner, in Matrix value)
			{
				owner.matrix = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustum owner, out Matrix value)
			{
				value = owner.matrix;
			}
		}

		protected class VRageMath_BoundingFrustum_003C_003Egjk_003C_003EAccessor : IMemberAccessor<BoundingFrustum, Gjk>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustum owner, in Gjk value)
			{
				owner.gjk = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustum owner, out Gjk value)
			{
				value = owner.gjk;
			}
		}

		protected class VRageMath_BoundingFrustum_003C_003EMatrix_003C_003EAccessor : IMemberAccessor<BoundingFrustum, Matrix>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustum owner, in Matrix value)
			{
				owner.Matrix = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustum owner, out Matrix value)
			{
				value = owner.Matrix;
			}
		}

		private Plane[] planes = new Plane[6];

		internal Vector3[] cornerArray = new Vector3[8];

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingFrustum.
		/// </summary>
		public const int CornerCount = 8;

		private const int NearPlaneIndex = 0;

		private const int FarPlaneIndex = 1;

		private const int LeftPlaneIndex = 2;

		private const int RightPlaneIndex = 3;

		private const int TopPlaneIndex = 4;

		private const int BottomPlaneIndex = 5;

		private const int NumPlanes = 6;

		private Matrix matrix;

		private Gjk gjk;

		public Plane[] Planes => planes;

		public Plane this[int index] => planes[index];

		/// <summary>
		/// Gets the near plane of the BoundingFrustum.
		/// </summary>
		public Plane Near => planes[0];

		/// <summary>
		/// Gets the far plane of the BoundingFrustum.
		/// </summary>
		public Plane Far => planes[1];

		/// <summary>
		/// Gets the left plane of the BoundingFrustum.
		/// </summary>
		public Plane Left => planes[2];

		/// <summary>
		/// Gets the right plane of the BoundingFrustum.
		/// </summary>
		public Plane Right => planes[3];

		/// <summary>
		/// Gets the top plane of the BoundingFrustum.
		/// </summary>
		public Plane Top => planes[4];

		/// <summary>
		/// Gets the bottom plane of the BoundingFrustum.
		/// </summary>
		public Plane Bottom => planes[5];

		/// <summary>
		/// Gets or sets the Matrix that describes this bounding frustum.
		/// </summary>
		public Matrix Matrix
		{
			get
			{
				return matrix;
			}
			set
			{
				SetMatrix(ref value);
			}
		}

		public BoundingFrustum()
		{
		}

		/// <summary>
		/// Creates a new instance of BoundingFrustum.
		/// </summary>
		/// <param name="value">Combined matrix that usually takes view × projection matrix.</param>
		public BoundingFrustum(Matrix value)
		{
			SetMatrix(ref value);
		}

		/// <summary>
		/// Determines whether two instances of BoundingFrustum are equal.
		/// </summary>
		/// <param name="a">The BoundingFrustum to the left of the equality operator.</param><param name="b">The BoundingFrustum to the right of the equality operator.</param>
		public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
		{
			return object.Equals(a, b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingFrustum are not equal.
		/// </summary>
		/// <param name="a">The BoundingFrustum to the left of the inequality operator.</param><param name="b">The BoundingFrustum to the right of the inequality operator.</param>
		public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
		{
			return !object.Equals(a, b);
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingFrustum. ALLOCATION!
		/// </summary>
		public Vector3[] GetCorners()
		{
			return (Vector3[])cornerArray.Clone();
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingFrustum.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingFrustum are written.</param>
		public void GetCorners(Vector3[] corners)
		{
			cornerArray.CopyTo(corners, 0);
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
		public unsafe void GetCornersUnsafe(Vector3* corners)
		{
			*corners = cornerArray[0];
			corners[1] = cornerArray[1];
			corners[2] = cornerArray[2];
			corners[3] = cornerArray[3];
			corners[4] = cornerArray[4];
			corners[5] = cornerArray[5];
			corners[6] = cornerArray[6];
			corners[7] = cornerArray[7];
		}

		/// <summary>
		/// Determines whether the specified BoundingFrustum is equal to the current BoundingFrustum.
		/// </summary>
		/// <param name="other">The BoundingFrustum to compare with the current BoundingFrustum.</param>
		public bool Equals(BoundingFrustum other)
		{
			if (other == null)
			{
				return false;
			}
			return matrix == other.matrix;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the BoundingFrustum.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingFrustum.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			BoundingFrustum boundingFrustum = obj as BoundingFrustum;
			if (boundingFrustum != null)
			{
				result = (matrix == boundingFrustum.matrix);
			}
			return result;
		}

		/// <summary>
		/// Gets the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return matrix.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current BoundingFrustum.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Near:{0} Far:{1} Left:{2} Right:{3} Top:{4} Bottom:{5}}}", Near.ToString(), Far.ToString(), Left.ToString(), Right.ToString(), Top.ToString(), Bottom.ToString());
		}

		private void SetMatrix(ref Matrix value)
		{
			matrix = value;
			planes[2].Normal.X = 0f - value.M14 - value.M11;
			planes[2].Normal.Y = 0f - value.M24 - value.M21;
			planes[2].Normal.Z = 0f - value.M34 - value.M31;
			planes[2].D = 0f - value.M44 - value.M41;
			planes[3].Normal.X = 0f - value.M14 + value.M11;
			planes[3].Normal.Y = 0f - value.M24 + value.M21;
			planes[3].Normal.Z = 0f - value.M34 + value.M31;
			planes[3].D = 0f - value.M44 + value.M41;
			planes[4].Normal.X = 0f - value.M14 + value.M12;
			planes[4].Normal.Y = 0f - value.M24 + value.M22;
			planes[4].Normal.Z = 0f - value.M34 + value.M32;
			planes[4].D = 0f - value.M44 + value.M42;
			planes[5].Normal.X = 0f - value.M14 - value.M12;
			planes[5].Normal.Y = 0f - value.M24 - value.M22;
			planes[5].Normal.Z = 0f - value.M34 - value.M32;
			planes[5].D = 0f - value.M44 - value.M42;
			planes[0].Normal.X = 0f - value.M13;
			planes[0].Normal.Y = 0f - value.M23;
			planes[0].Normal.Z = 0f - value.M33;
			planes[0].D = 0f - value.M43;
			planes[1].Normal.X = 0f - value.M14 + value.M13;
			planes[1].Normal.Y = 0f - value.M24 + value.M23;
			planes[1].Normal.Z = 0f - value.M34 + value.M33;
			planes[1].D = 0f - value.M44 + value.M43;
			for (int i = 0; i < 6; i++)
			{
				float num = planes[i].Normal.Length();
				planes[i].Normal /= num;
				planes[i].D /= num;
			}
			Ray ray = ComputeIntersectionLine(ref planes[0], ref planes[2]);
			cornerArray[0] = ComputeIntersection(ref planes[4], ref ray);
			cornerArray[3] = ComputeIntersection(ref planes[5], ref ray);
			Ray ray2 = ComputeIntersectionLine(ref planes[3], ref planes[0]);
			cornerArray[1] = ComputeIntersection(ref planes[4], ref ray2);
			cornerArray[2] = ComputeIntersection(ref planes[5], ref ray2);
			ray2 = ComputeIntersectionLine(ref planes[2], ref planes[1]);
			cornerArray[4] = ComputeIntersection(ref planes[4], ref ray2);
			cornerArray[7] = ComputeIntersection(ref planes[5], ref ray2);
			ray2 = ComputeIntersectionLine(ref planes[1], ref planes[3]);
			cornerArray[5] = ComputeIntersection(ref planes[4], ref ray2);
			cornerArray[6] = ComputeIntersection(ref planes[5], ref ray2);
		}

		private static Ray ComputeIntersectionLine(ref Plane p1, ref Plane p2)
		{
			Ray result = default(Ray);
			result.Direction = Vector3.Cross(p1.Normal, p2.Normal);
			float divider = result.Direction.LengthSquared();
			result.Position = Vector3.Cross((0f - p1.D) * p2.Normal + p2.D * p1.Normal, result.Direction) / divider;
			return result;
		}

		private static Vector3 ComputeIntersection(ref Plane plane, ref Ray ray)
		{
			float scaleFactor = (0f - plane.D - Vector3.Dot(plane.Normal, ray.Position)) / Vector3.Dot(plane.Normal, ray.Direction);
			return ray.Position + ray.Direction * scaleFactor;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects the specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection.</param>
		public bool Intersects(BoundingBox box)
		{
			Intersects(ref box, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects a BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustum and BoundingBox intersect; false otherwise.</param>
		public void Intersects(ref BoundingBox box, out bool result)
		{
			if (gjk == null)
			{
				gjk = new Gjk();
			}
			gjk.Reset();
			Vector3.Subtract(ref cornerArray[0], ref box.Min, out Vector3 result2);
			if ((double)result2.LengthSquared() < 9.99999974737875E-06)
			{
				Vector3.Subtract(ref cornerArray[0], ref box.Max, out result2);
			}
			float num = float.MaxValue;
			result = false;
			Vector3 v = default(Vector3);
			float num3;
			do
			{
				v.X = 0f - result2.X;
				v.Y = 0f - result2.Y;
				v.Z = 0f - result2.Z;
				SupportMapping(ref v, out Vector3 result3);
				box.SupportMapping(ref result2, out Vector3 result4);
				Vector3.Subtract(ref result3, ref result4, out Vector3 result5);
				if ((double)result2.X * (double)result5.X + (double)result2.Y * (double)result5.Y + (double)result2.Z * (double)result5.Z > 0.0)
				{
					return;
				}
				gjk.AddSupportPoint(ref result5);
				result2 = gjk.ClosestPoint;
				float num2 = num;
				num = result2.LengthSquared();
				if ((double)num2 - (double)num <= 9.99999974737875E-06 * (double)num2)
				{
					return;
				}
				num3 = 4E-05f * gjk.MaxLengthSquared;
			}
			while (!gjk.FullSimplex && (double)num >= (double)num3);
			result = true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects the specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check for intersection.</param>
		public bool Intersects(BoundingFrustum frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			if (gjk == null)
			{
				gjk = new Gjk();
			}
			gjk.Reset();
			Vector3.Subtract(ref cornerArray[0], ref frustum.cornerArray[0], out Vector3 result);
			if ((double)result.LengthSquared() < 9.99999974737875E-06)
			{
				Vector3.Subtract(ref cornerArray[0], ref frustum.cornerArray[1], out result);
			}
			float num = float.MaxValue;
			Vector3 v = default(Vector3);
			float num3;
			do
			{
				v.X = 0f - result.X;
				v.Y = 0f - result.Y;
				v.Z = 0f - result.Z;
				SupportMapping(ref v, out Vector3 result2);
				frustum.SupportMapping(ref result, out Vector3 result3);
				Vector3.Subtract(ref result2, ref result3, out Vector3 result4);
				if ((double)result.X * (double)result4.X + (double)result.Y * (double)result4.Y + (double)result.Z * (double)result4.Z > 0.0)
				{
					return false;
				}
				gjk.AddSupportPoint(ref result4);
				result = gjk.ClosestPoint;
				float num2 = num;
				num = result.LengthSquared();
				num3 = 4E-05f * gjk.MaxLengthSquared;
				if ((double)num2 - (double)num <= 9.99999974737875E-06 * (double)num2)
				{
					return false;
				}
			}
			while (!gjk.FullSimplex && (double)num >= (double)num3);
			return true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects the specified Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection.</param>
		public PlaneIntersectionType Intersects(Plane plane)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3.Dot(ref cornerArray[i], ref plane.Normal, out float result);
				num = ((!((double)result + (double)plane.D > 0.0)) ? (num | 2) : (num | 1));
				if (num == 3)
				{
					return PlaneIntersectionType.Intersecting;
				}
			}
			if (num == 1)
			{
				return PlaneIntersectionType.Front;
			}
			return PlaneIntersectionType.Back;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingFrustum intersects the Plane.</param>
		public void Intersects(ref Plane plane, out PlaneIntersectionType result)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3.Dot(ref cornerArray[i], ref plane.Normal, out float result2);
				num = ((!((double)result2 + (double)plane.D > 0.0)) ? (num | 2) : (num | 1));
				if (num == 3)
				{
					result = PlaneIntersectionType.Intersecting;
					return;
				}
			}
			result = ((num != 1) ? PlaneIntersectionType.Back : PlaneIntersectionType.Front);
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects the specified Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection.</param>
		public float? Intersects(Ray ray)
		{
			Intersects(ref ray, out float? result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingFrustum or null if there is no intersection.</param>
		public void Intersects(ref Ray ray, out float? result)
		{
			Contains(ref ray.Position, out ContainmentType result2);
			if (result2 == ContainmentType.Contains)
			{
				result = 0f;
				return;
			}
			float num = float.MinValue;
			float num2 = float.MaxValue;
			result = null;
			Plane[] array = planes;
			for (int i = 0; i < array.Length; i++)
			{
				Plane plane = array[i];
				Vector3 vector = plane.Normal;
				Vector3.Dot(ref ray.Direction, ref vector, out float result3);
				Vector3.Dot(ref ray.Position, ref vector, out float result4);
				result4 += plane.D;
				if ((double)Math.Abs(result3) < 9.99999974737875E-06)
				{
					if ((double)result4 > 0.0)
					{
						return;
					}
					continue;
				}
				float num3 = (0f - result4) / result3;
				if ((double)result3 < 0.0)
				{
					if ((double)num3 > (double)num2)
					{
						return;
					}
					if ((double)num3 > (double)num)
					{
						num = num3;
					}
				}
				else
				{
					if ((double)num3 < (double)num)
					{
						return;
					}
					if ((double)num3 < (double)num2)
					{
						num2 = num3;
					}
				}
			}
			float num4 = ((double)num >= 0.0) ? num : num2;
			if (!((double)num4 < 0.0))
			{
				result = num4;
			}
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection.</param>
		public bool Intersects(BoundingSphere sphere)
		{
			Intersects(ref sphere, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustum and BoundingSphere intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphere sphere, out bool result)
		{
			if (gjk == null)
			{
				gjk = new Gjk();
			}
			gjk.Reset();
			Vector3.Subtract(ref cornerArray[0], ref sphere.Center, out Vector3 result2);
			if ((double)result2.LengthSquared() < 9.99999974737875E-06)
			{
				result2 = Vector3.UnitX;
			}
			float num = float.MaxValue;
			result = false;
			Vector3 v = default(Vector3);
			float num3;
			do
			{
				v.X = 0f - result2.X;
				v.Y = 0f - result2.Y;
				v.Z = 0f - result2.Z;
				SupportMapping(ref v, out Vector3 result3);
				sphere.SupportMapping(ref result2, out Vector3 result4);
				Vector3.Subtract(ref result3, ref result4, out Vector3 result5);
				if ((double)result2.X * (double)result5.X + (double)result2.Y * (double)result5.Y + (double)result2.Z * (double)result5.Z > 0.0)
				{
					return;
				}
				gjk.AddSupportPoint(ref result5);
				result2 = gjk.ClosestPoint;
				float num2 = num;
				num = result2.LengthSquared();
				if ((double)num2 - (double)num <= 9.99999974737875E-06 * (double)num2)
				{
					return;
				}
				num3 = 4E-05f * gjk.MaxLengthSquared;
			}
			while (!gjk.FullSimplex && (double)num >= (double)num3);
			result = true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to check against the current BoundingFrustum.</param>
		public ContainmentType Contains(ref BoundingBox box)
		{
			bool flag = false;
			Plane[] array = planes;
			foreach (Plane plane in array)
			{
				switch (box.Intersects(plane))
				{
				case PlaneIntersectionType.Front:
					return ContainmentType.Disjoint;
				case PlaneIntersectionType.Intersecting:
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return ContainmentType.Intersects;
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified BoundingBox.
		/// </summary>
		/// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBox box, out ContainmentType result)
		{
			bool flag = false;
			Plane[] array = planes;
			foreach (Plane plane in array)
			{
				switch (box.Intersects(plane))
				{
				case PlaneIntersectionType.Front:
					result = ContainmentType.Disjoint;
					return;
				case PlaneIntersectionType.Intersecting:
					flag = true;
					break;
				}
			}
			result = ((!flag) ? ContainmentType.Contains : ContainmentType.Intersects);
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified BoundingFrustum.
		/// </summary>
		/// <param name="frustum">The BoundingFrustum to check against the current BoundingFrustum.</param>
		public ContainmentType Contains(BoundingFrustum frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			ContainmentType result = ContainmentType.Disjoint;
			if (Intersects(frustum))
			{
				result = ContainmentType.Contains;
				for (int i = 0; i < cornerArray.Length; i++)
				{
					if (Contains(frustum.cornerArray[i]) == ContainmentType.Disjoint)
					{
						result = ContainmentType.Intersects;
						break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified point.
		/// </summary>
		/// <param name="point">The point to check against the current BoundingFrustum.</param>
		public ContainmentType Contains(Vector3 point)
		{
			Plane[] array = planes;
			for (int i = 0; i < array.Length; i++)
			{
				Plane plane = array[i];
				if ((double)((float)((double)plane.Normal.X * (double)point.X + (double)plane.Normal.Y * (double)point.Y + (double)plane.Normal.Z * (double)point.Z) + plane.D) > 9.99999974737875E-06)
				{
					return ContainmentType.Disjoint;
				}
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3 point, out ContainmentType result)
		{
			Plane[] array = planes;
			for (int i = 0; i < array.Length; i++)
			{
				Plane plane = array[i];
				if ((double)((float)((double)plane.Normal.X * (double)point.X + (double)plane.Normal.Y * (double)point.Y + (double)plane.Normal.Z * (double)point.Z) + plane.D) > 9.99999974737875E-06)
				{
					result = ContainmentType.Disjoint;
					return;
				}
			}
			result = ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check against the current BoundingFrustum.</param>
		public ContainmentType Contains(BoundingSphere sphere)
		{
			Vector3 center = sphere.Center;
			float radius = sphere.Radius;
			int num = 0;
			Plane[] array = planes;
			for (int i = 0; i < array.Length; i++)
			{
				Plane plane = array[i];
				float num2 = (float)((double)plane.Normal.X * (double)center.X + (double)plane.Normal.Y * (double)center.Y + (double)plane.Normal.Z * (double)center.Z) + plane.D;
				if ((double)num2 > (double)radius)
				{
					return ContainmentType.Disjoint;
				}
				if ((double)num2 < 0.0 - (double)radius)
				{
					num++;
				}
			}
			if (num == 6)
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphere sphere, out ContainmentType result)
		{
			Vector3 center = sphere.Center;
			float radius = sphere.Radius;
			int num = 0;
			Plane[] array = planes;
			for (int i = 0; i < array.Length; i++)
			{
				Plane plane = array[i];
				float num2 = (float)((double)plane.Normal.X * (double)center.X + (double)plane.Normal.Y * (double)center.Y + (double)plane.Normal.Z * (double)center.Z) + plane.D;
				if ((double)num2 > (double)radius)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				if ((double)num2 < 0.0 - (double)radius)
				{
					num++;
				}
			}
			result = ((num == 6) ? ContainmentType.Contains : ContainmentType.Intersects);
		}

		internal void SupportMapping(ref Vector3 v, out Vector3 result)
		{
			int num = 0;
			Vector3.Dot(ref cornerArray[0], ref v, out float result2);
			for (int i = 1; i < cornerArray.Length; i++)
			{
				Vector3.Dot(ref cornerArray[i], ref v, out float result3);
				if ((double)result3 > (double)result2)
				{
					num = i;
					result2 = result3;
				}
			}
			result = cornerArray[num];
		}
	}
}
