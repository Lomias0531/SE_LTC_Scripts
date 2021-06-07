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
	public class BoundingFrustumD : IEquatable<BoundingFrustumD>
	{
		protected class VRageMath_BoundingFrustumD_003C_003Em_planes_003C_003EAccessor : IMemberAccessor<BoundingFrustumD, PlaneD[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustumD owner, in PlaneD[] value)
			{
				owner.m_planes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustumD owner, out PlaneD[] value)
			{
				value = owner.m_planes;
			}
		}

		protected class VRageMath_BoundingFrustumD_003C_003ECornerArray_003C_003EAccessor : IMemberAccessor<BoundingFrustumD, Vector3D[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustumD owner, in Vector3D[] value)
			{
				owner.CornerArray = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustumD owner, out Vector3D[] value)
			{
				value = owner.CornerArray;
			}
		}

		protected class VRageMath_BoundingFrustumD_003C_003Ematrix_003C_003EAccessor : IMemberAccessor<BoundingFrustumD, MatrixD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustumD owner, in MatrixD value)
			{
				owner.matrix = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustumD owner, out MatrixD value)
			{
				value = owner.matrix;
			}
		}

		protected class VRageMath_BoundingFrustumD_003C_003Egjk_003C_003EAccessor : IMemberAccessor<BoundingFrustumD, GjkD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustumD owner, in GjkD value)
			{
				owner.gjk = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustumD owner, out GjkD value)
			{
				value = owner.gjk;
			}
		}

		protected class VRageMath_BoundingFrustumD_003C_003EMatrix_003C_003EAccessor : IMemberAccessor<BoundingFrustumD, MatrixD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref BoundingFrustumD owner, in MatrixD value)
			{
				owner.Matrix = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref BoundingFrustumD owner, out MatrixD value)
			{
				value = owner.Matrix;
			}
		}

		private readonly PlaneD[] m_planes = new PlaneD[6];

		internal readonly Vector3D[] CornerArray = new Vector3D[8];

		/// <summary>
		/// Specifies the total number of corners (8) in the BoundingFrustumD.
		/// </summary>
		public const int CornerCount = 8;

		private const int NearPlaneIndex = 0;

		private const int FarPlaneIndex = 1;

		private const int LeftPlaneIndex = 2;

		private const int RightPlaneIndex = 3;

		private const int TopPlaneIndex = 4;

		private const int BottomPlaneIndex = 5;

		private const int NumPlanes = 6;

		private MatrixD matrix;

		private GjkD gjk;

		public PlaneD this[int index] => m_planes[index];

		/// <summary>
		/// Gets the near plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Near => m_planes[0];

		/// <summary>
		/// Gets the far plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Far => m_planes[1];

		/// <summary>
		/// Gets the left plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Left => m_planes[2];

		/// <summary>
		/// Gets the right plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Right => m_planes[3];

		/// <summary>
		/// Gets the top plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Top => m_planes[4];

		/// <summary>
		/// Gets the bottom plane of the BoundingFrustumD.
		/// </summary>
		public PlaneD Bottom => m_planes[5];

		/// <summary>
		/// Gets or sets the Matrix that describes this bounding frustum.
		/// </summary>
		public MatrixD Matrix
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

		public BoundingFrustumD()
		{
		}

		/// <summary>
		/// Creates a new instance of BoundingFrustumD.
		/// </summary>
		/// <param name="value">Combined matrix that usually takes view × projection matrix.</param>
		public BoundingFrustumD(MatrixD value)
		{
			SetMatrix(ref value);
		}

		/// <summary>
		/// Determines whether two instances of BoundingFrustumD are equal.
		/// </summary>
		/// <param name="a">The BoundingFrustumD to the left of the equality operator.</param><param name="b">The BoundingFrustumD to the right of the equality operator.</param>
		public static bool operator ==(BoundingFrustumD a, BoundingFrustumD b)
		{
			return object.Equals(a, b);
		}

		/// <summary>
		/// Determines whether two instances of BoundingFrustumD are not equal.
		/// </summary>
		/// <param name="a">The BoundingFrustumD to the left of the inequality operator.</param><param name="b">The BoundingFrustumD to the right of the inequality operator.</param>
		public static bool operator !=(BoundingFrustumD a, BoundingFrustumD b)
		{
			return !object.Equals(a, b);
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingFrustumD. ALLOCATION!
		/// </summary>
		public Vector3D[] GetCorners()
		{
			return (Vector3D[])CornerArray.Clone();
		}

		/// <summary>
		/// Gets an array of points that make up the corners of the BoundingFrustumD.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3D points where the corners of the BoundingFrustumD are written.</param>
		public void GetCorners(Vector3D[] corners)
		{
			CornerArray.CopyTo(corners, 0);
		}

		/// <summary>
		/// Gets the array of points that make up the corners of the BoundingBox.
		/// </summary>
		/// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
		public unsafe void GetCornersUnsafe(Vector3D* corners)
		{
			*corners = CornerArray[0];
			corners[1] = CornerArray[1];
			corners[2] = CornerArray[2];
			corners[3] = CornerArray[3];
			corners[4] = CornerArray[4];
			corners[5] = CornerArray[5];
			corners[6] = CornerArray[6];
			corners[7] = CornerArray[7];
		}

		/// <summary>
		/// Determines whether the specified BoundingFrustumD is equal to the current BoundingFrustumD.
		/// </summary>
		/// <param name="other">The BoundingFrustumD to compare with the current BoundingFrustumD.</param>
		public bool Equals(BoundingFrustumD other)
		{
			if (other == null)
			{
				return false;
			}
			return matrix == other.matrix;
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the BoundingFrustumD.
		/// </summary>
		/// <param name="obj">The Object to compare with the current BoundingFrustumD.</param>
		public override bool Equals(object obj)
		{
			bool result = false;
			BoundingFrustumD boundingFrustumD = obj as BoundingFrustumD;
			if (boundingFrustumD != null)
			{
				result = (matrix == boundingFrustumD.matrix);
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
		/// Returns a String that represents the current BoundingFrustumD.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Near:{0} Far:{1} Left:{2} Right:{3} Top:{4} Bottom:{5}}}", Near.ToString(), Far.ToString(), Left.ToString(), Right.ToString(), Top.ToString(), Bottom.ToString());
		}

		private void SetMatrix(ref MatrixD value)
		{
			matrix = value;
			m_planes[2].Normal.X = 0.0 - value.M14 - value.M11;
			m_planes[2].Normal.Y = 0.0 - value.M24 - value.M21;
			m_planes[2].Normal.Z = 0.0 - value.M34 - value.M31;
			m_planes[2].D = 0.0 - value.M44 - value.M41;
			m_planes[3].Normal.X = 0.0 - value.M14 + value.M11;
			m_planes[3].Normal.Y = 0.0 - value.M24 + value.M21;
			m_planes[3].Normal.Z = 0.0 - value.M34 + value.M31;
			m_planes[3].D = 0.0 - value.M44 + value.M41;
			m_planes[4].Normal.X = 0.0 - value.M14 + value.M12;
			m_planes[4].Normal.Y = 0.0 - value.M24 + value.M22;
			m_planes[4].Normal.Z = 0.0 - value.M34 + value.M32;
			m_planes[4].D = 0.0 - value.M44 + value.M42;
			m_planes[5].Normal.X = 0.0 - value.M14 - value.M12;
			m_planes[5].Normal.Y = 0.0 - value.M24 - value.M22;
			m_planes[5].Normal.Z = 0.0 - value.M34 - value.M32;
			m_planes[5].D = 0.0 - value.M44 - value.M42;
			m_planes[0].Normal.X = 0.0 - value.M13;
			m_planes[0].Normal.Y = 0.0 - value.M23;
			m_planes[0].Normal.Z = 0.0 - value.M33;
			m_planes[0].D = 0.0 - value.M43;
			m_planes[1].Normal.X = 0.0 - value.M14 + value.M13;
			m_planes[1].Normal.Y = 0.0 - value.M24 + value.M23;
			m_planes[1].Normal.Z = 0.0 - value.M34 + value.M33;
			m_planes[1].D = 0.0 - value.M44 + value.M43;
			for (int i = 0; i < 6; i++)
			{
				double num = m_planes[i].Normal.Length();
				m_planes[i].Normal = m_planes[i].Normal / num;
				m_planes[i].D = m_planes[i].D / num;
			}
			RayD ray = ComputeIntersectionLine(ref m_planes[0], ref m_planes[2]);
			CornerArray[0] = ComputeIntersection(ref m_planes[4], ref ray);
			CornerArray[3] = ComputeIntersection(ref m_planes[5], ref ray);
			RayD ray2 = ComputeIntersectionLine(ref m_planes[3], ref m_planes[0]);
			CornerArray[1] = ComputeIntersection(ref m_planes[4], ref ray2);
			CornerArray[2] = ComputeIntersection(ref m_planes[5], ref ray2);
			ray2 = ComputeIntersectionLine(ref m_planes[2], ref m_planes[1]);
			CornerArray[4] = ComputeIntersection(ref m_planes[4], ref ray2);
			CornerArray[7] = ComputeIntersection(ref m_planes[5], ref ray2);
			ray2 = ComputeIntersectionLine(ref m_planes[1], ref m_planes[3]);
			CornerArray[5] = ComputeIntersection(ref m_planes[4], ref ray2);
			CornerArray[6] = ComputeIntersection(ref m_planes[5], ref ray2);
		}

		private static RayD ComputeIntersectionLine(ref PlaneD p1, ref PlaneD p2)
		{
			RayD result = default(RayD);
			result.Direction = Vector3D.Cross(p1.Normal, p2.Normal);
			double divider = result.Direction.LengthSquared();
			result.Position = Vector3D.Cross((0.0 - p1.D) * p2.Normal + p2.D * p1.Normal, result.Direction) / divider;
			return result;
		}

		private static Vector3D ComputeIntersection(ref PlaneD plane, ref RayD ray)
		{
			double scaleFactor = (0.0 - plane.D - Vector3D.Dot(plane.Normal, ray.Position)) / Vector3D.Dot(plane.Normal, ray.Direction);
			return ray.Position + ray.Direction * scaleFactor;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects the specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check for intersection.</param>
		public bool Intersects(BoundingBoxD box)
		{
			Intersects(ref box, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects a BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustumD and BoundingBoxD intersect; false otherwise.</param>
		public void Intersects(ref BoundingBoxD box, out bool result)
		{
			if (gjk == null)
			{
				gjk = new GjkD();
			}
			gjk.Reset();
			Vector3D.Subtract(ref CornerArray[0], ref box.Min, out Vector3D result2);
			if (result2.LengthSquared() < 9.99999974737875E-06)
			{
				Vector3D.Subtract(ref CornerArray[0], ref box.Max, out result2);
			}
			double num = double.MaxValue;
			result = false;
			Vector3D v = default(Vector3D);
			double num3;
			do
			{
				v.X = 0.0 - result2.X;
				v.Y = 0.0 - result2.Y;
				v.Z = 0.0 - result2.Z;
				SupportMapping(ref v, out Vector3D result3);
				box.SupportMapping(ref result2, out Vector3D result4);
				Vector3D.Subtract(ref result3, ref result4, out Vector3D result5);
				if (result2.X * result5.X + result2.Y * result5.Y + result2.Z * result5.Z > 0.0)
				{
					return;
				}
				gjk.AddSupportPoint(ref result5);
				result2 = gjk.ClosestPoint;
				double num2 = num;
				num = result2.LengthSquared();
				if (num2 - num <= 9.99999974737875E-06 * num2)
				{
					return;
				}
				num3 = 3.9999998989515007E-05 * gjk.MaxLengthSquared;
			}
			while (!gjk.FullSimplex && num >= num3);
			result = true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects the specified BoundingFrustumD.
		/// </summary>
		/// <param name="frustum">The BoundingFrustumD to check for intersection.</param>
		public bool Intersects(BoundingFrustumD frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			if (gjk == null)
			{
				gjk = new GjkD();
			}
			gjk.Reset();
			Vector3D.Subtract(ref CornerArray[0], ref frustum.CornerArray[0], out Vector3D result);
			if (result.LengthSquared() < 9.99999974737875E-06)
			{
				Vector3D.Subtract(ref CornerArray[0], ref frustum.CornerArray[1], out result);
			}
			double num = double.MaxValue;
			Vector3D v = default(Vector3D);
			double num3;
			do
			{
				v.X = 0.0 - result.X;
				v.Y = 0.0 - result.Y;
				v.Z = 0.0 - result.Z;
				SupportMapping(ref v, out Vector3D result2);
				frustum.SupportMapping(ref result, out Vector3D result3);
				Vector3D.Subtract(ref result2, ref result3, out Vector3D result4);
				if (result.X * result4.X + result.Y * result4.Y + result.Z * result4.Z > 0.0)
				{
					return false;
				}
				gjk.AddSupportPoint(ref result4);
				result = gjk.ClosestPoint;
				double num2 = num;
				num = result.LengthSquared();
				num3 = 3.9999998989515007E-05 * gjk.MaxLengthSquared;
				if (num2 - num <= 9.99999974737875E-06 * num2)
				{
					return false;
				}
			}
			while (!gjk.FullSimplex && num >= num3);
			return true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects the specified Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection.</param>
		public PlaneIntersectionType Intersects(PlaneD plane)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3D.Dot(ref CornerArray[i], ref plane.Normal, out double result);
				num = ((!(result + plane.D > 0.0)) ? (num | 2) : (num | 1));
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
		/// Checks whether the current BoundingFrustumD intersects a Plane.
		/// </summary>
		/// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingFrustumD intersects the Plane.</param>
		public void Intersects(ref PlaneD plane, out PlaneIntersectionType result)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3D.Dot(ref CornerArray[i], ref plane.Normal, out double result2);
				num = ((!(result2 + plane.D > 0.0)) ? (num | 2) : (num | 1));
				if (num == 3)
				{
					result = PlaneIntersectionType.Intersecting;
					return;
				}
			}
			result = ((num != 1) ? PlaneIntersectionType.Back : PlaneIntersectionType.Front);
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects the specified Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection.</param>
		public double? Intersects(RayD ray)
		{
			Intersects(ref ray, out double? result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects a Ray.
		/// </summary>
		/// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingFrustumD or null if there is no intersection.</param>
		public void Intersects(ref RayD ray, out double? result)
		{
			Contains(ref ray.Position, out ContainmentType result2);
			if (result2 == ContainmentType.Contains)
			{
				result = 0.0;
				return;
			}
			double num = double.MinValue;
			double num2 = double.MaxValue;
			result = null;
			PlaneD[] planes = m_planes;
			for (int i = 0; i < planes.Length; i++)
			{
				PlaneD planeD = planes[i];
				Vector3D vector = planeD.Normal;
				Vector3D.Dot(ref ray.Direction, ref vector, out double result3);
				Vector3D.Dot(ref ray.Position, ref vector, out double result4);
				result4 += planeD.D;
				if (Math.Abs(result3) < 9.99999974737875E-06)
				{
					if (result4 > 0.0)
					{
						return;
					}
					continue;
				}
				double num3 = (0.0 - result4) / result3;
				if (result3 < 0.0)
				{
					if (num3 > num2)
					{
						return;
					}
					if (num3 > num)
					{
						num = num3;
					}
				}
				else
				{
					if (num3 < num)
					{
						return;
					}
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
			}
			double num4 = (num >= 0.0) ? num : num2;
			if (!(num4 < 0.0))
			{
				result = num4;
			}
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection.</param>
		public bool Intersects(BoundingSphereD sphere)
		{
			Intersects(ref sphere, out bool result);
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD intersects a BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustumD and BoundingSphere intersect; false otherwise.</param>
		public void Intersects(ref BoundingSphereD sphere, out bool result)
		{
			if (gjk == null)
			{
				gjk = new GjkD();
			}
			gjk.Reset();
			Vector3D.Subtract(ref CornerArray[0], ref sphere.Center, out Vector3D result2);
			if (result2.LengthSquared() < 9.99999974737875E-06)
			{
				result2 = Vector3D.UnitX;
			}
			double num = double.MaxValue;
			result = false;
			Vector3D v = default(Vector3D);
			double num3;
			do
			{
				v.X = 0.0 - result2.X;
				v.Y = 0.0 - result2.Y;
				v.Z = 0.0 - result2.Z;
				SupportMapping(ref v, out Vector3D result3);
				sphere.SupportMapping(ref result2, out Vector3D result4);
				Vector3D.Subtract(ref result3, ref result4, out Vector3D result5);
				if (result2.X * result5.X + result2.Y * result5.Y + result2.Z * result5.Z > 0.0)
				{
					return;
				}
				gjk.AddSupportPoint(ref result5);
				result2 = gjk.ClosestPoint;
				double num2 = num;
				num = result2.LengthSquared();
				if (num2 - num <= 9.99999974737875E-06 * num2)
				{
					return;
				}
				num3 = 3.9999998989515007E-05 * gjk.MaxLengthSquared;
			}
			while (!gjk.FullSimplex && num >= num3);
			result = true;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD contains the specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to check against the current BoundingFrustumD.</param>
		public ContainmentType Contains(BoundingBoxD box)
		{
			bool flag = false;
			PlaneD[] planes = m_planes;
			foreach (PlaneD plane in planes)
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
		/// Checks whether the current BoundingFrustumD contains the specified BoundingBoxD.
		/// </summary>
		/// <param name="box">The BoundingBoxD to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingBoxD box, out ContainmentType result)
		{
			bool flag = false;
			PlaneD[] planes = m_planes;
			foreach (PlaneD plane in planes)
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
		/// Checks whether the current BoundingFrustumD contains the specified BoundingFrustumD.
		/// </summary>
		/// <param name="frustum">The BoundingFrustumD to check against the current BoundingFrustumD.</param>
		public ContainmentType Contains(BoundingFrustumD frustum)
		{
			if (frustum == null)
			{
				throw new ArgumentNullException("frustum");
			}
			ContainmentType result = ContainmentType.Disjoint;
			if (Intersects(frustum))
			{
				result = ContainmentType.Contains;
				for (int i = 0; i < CornerArray.Length; i++)
				{
					if (Contains(frustum.CornerArray[i]) == ContainmentType.Disjoint)
					{
						result = ContainmentType.Intersects;
						break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD contains the specified point.
		/// </summary>
		/// <param name="point">The point to check against the current BoundingFrustumD.</param>
		public ContainmentType Contains(Vector3D point)
		{
			PlaneD[] planes = m_planes;
			for (int i = 0; i < planes.Length; i++)
			{
				PlaneD planeD = planes[i];
				if (planeD.Normal.X * point.X + planeD.Normal.Y * point.Y + planeD.Normal.Z * point.Z + planeD.D > 9.99999974737875E-06)
				{
					return ContainmentType.Disjoint;
				}
			}
			return ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD contains the specified point.
		/// </summary>
		/// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref Vector3D point, out ContainmentType result)
		{
			PlaneD[] planes = m_planes;
			for (int i = 0; i < planes.Length; i++)
			{
				PlaneD planeD = planes[i];
				if (planeD.Normal.X * point.X + planeD.Normal.Y * point.Y + planeD.Normal.Z * point.Z + planeD.D > 9.99999974737875E-06)
				{
					result = ContainmentType.Disjoint;
					return;
				}
			}
			result = ContainmentType.Contains;
		}

		/// <summary>
		/// Checks whether the current BoundingFrustumD contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to check against the current BoundingFrustumD.</param>
		public ContainmentType Contains(BoundingSphereD sphere)
		{
			Vector3D center = sphere.Center;
			double radius = sphere.Radius;
			int num = 0;
			PlaneD[] planes = m_planes;
			for (int i = 0; i < planes.Length; i++)
			{
				PlaneD planeD = planes[i];
				double num2 = planeD.Normal.X * center.X + planeD.Normal.Y * center.Y + planeD.Normal.Z * center.Z + planeD.D;
				if (num2 > radius)
				{
					return ContainmentType.Disjoint;
				}
				if (num2 < 0.0 - radius)
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
		/// Checks whether the current BoundingFrustumD contains the specified BoundingSphere.
		/// </summary>
		/// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
		public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
		{
			Vector3D center = sphere.Center;
			double radius = sphere.Radius;
			int num = 0;
			PlaneD[] planes = m_planes;
			for (int i = 0; i < planes.Length; i++)
			{
				PlaneD planeD = planes[i];
				double num2 = planeD.Normal.X * center.X + planeD.Normal.Y * center.Y + planeD.Normal.Z * center.Z + planeD.D;
				if (num2 > radius)
				{
					result = ContainmentType.Disjoint;
					return;
				}
				if (num2 < 0.0 - radius)
				{
					num++;
				}
			}
			result = ((num == 6) ? ContainmentType.Contains : ContainmentType.Intersects);
		}

		internal void SupportMapping(ref Vector3D v, out Vector3D result)
		{
			int num = 0;
			Vector3D.Dot(ref CornerArray[0], ref v, out double result2);
			for (int i = 1; i < CornerArray.Length; i++)
			{
				Vector3D.Dot(ref CornerArray[i], ref v, out double result3);
				if (result3 > result2)
				{
					num = i;
					result2 = result3;
				}
			}
			result = CornerArray[num];
		}
	}
}
