using System;

namespace VRageMath
{
	public struct MyOrientedBoundingBoxD : IEquatable<MyOrientedBoundingBox>
	{
		public const int CornerCount = 8;

		private const float RAY_EPSILON = 1E-20f;

		public static readonly int[] StartVertices = new int[12]
		{
			0,
			1,
			5,
			4,
			3,
			2,
			6,
			7,
			0,
			1,
			5,
			4
		};

		public static readonly int[] EndVertices = new int[12]
		{
			1,
			5,
			4,
			0,
			2,
			6,
			7,
			3,
			3,
			2,
			6,
			7
		};

		public static readonly int[] StartXVertices = new int[4]
		{
			0,
			4,
			7,
			3
		};

		public static readonly int[] EndXVertices = new int[4]
		{
			1,
			5,
			6,
			2
		};

		public static readonly int[] StartYVertices = new int[4]
		{
			0,
			1,
			5,
			4
		};

		public static readonly int[] EndYVertices = new int[4]
		{
			3,
			2,
			6,
			7
		};

		public static readonly int[] StartZVertices = new int[4]
		{
			0,
			3,
			2,
			1
		};

		public static readonly int[] EndZVertices = new int[4]
		{
			4,
			7,
			6,
			5
		};

		public static readonly Vector3[] XNeighbourVectorsBack = new Vector3[4]
		{
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, -1f, 0f)
		};

		public static readonly Vector3[] XNeighbourVectorsForw = new Vector3[4]
		{
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 1f, 0f)
		};

		public static readonly Vector3[] YNeighbourVectorsBack = new Vector3[4]
		{
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 0f, -1f)
		};

		public static readonly Vector3[] YNeighbourVectorsForw = new Vector3[4]
		{
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 1f)
		};

		public static readonly Vector3[] ZNeighbourVectorsBack = new Vector3[4]
		{
			new Vector3(0f, 1f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, -1f, 0f),
			new Vector3(-1f, 0f, 0f)
		};

		public static readonly Vector3[] ZNeighbourVectorsForw = new Vector3[4]
		{
			new Vector3(0f, -1f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(1f, 0f, 0f)
		};

		public Vector3D Center;

		public Vector3D HalfExtent;

		public Quaternion Orientation;

		[ThreadStatic]
		private static Vector3D[] m_cornersTmp;

		/// <summary>
		/// Returns normal between two cube edge of same direction
		/// </summary>
		/// <param name="axis">Edge direction axis (0 = X, 1 = Y, 2 = Z)</param>
		/// <param name="edge0"></param>
		/// <param name="edge1"></param>
		/// <param name="normal"></param>
		/// <returns>false if edges are not neighbors</returns>
		public static bool GetNormalBetweenEdges(int axis, int edge0, int edge1, out Vector3 normal)
		{
			Vector3[] array = null;
			Vector3[] array2 = null;
			normal = Vector3.Zero;
			switch (axis)
			{
			case 0:
				_ = StartXVertices;
				_ = EndXVertices;
				array = XNeighbourVectorsForw;
				array2 = XNeighbourVectorsBack;
				break;
			case 1:
				_ = StartYVertices;
				_ = EndYVertices;
				array = YNeighbourVectorsForw;
				array2 = YNeighbourVectorsBack;
				break;
			case 2:
				_ = StartZVertices;
				_ = EndZVertices;
				array = ZNeighbourVectorsForw;
				array2 = ZNeighbourVectorsBack;
				break;
			default:
				return false;
			}
			if (edge0 == -1)
			{
				edge0 = 3;
			}
			if (edge0 == 4)
			{
				edge0 = 0;
			}
			if (edge1 == -1)
			{
				edge1 = 3;
			}
			if (edge1 == 4)
			{
				edge1 = 0;
			}
			if (edge0 == 3 && edge1 == 0)
			{
				normal = array[3];
				return true;
			}
			if (edge0 == 0 && edge1 == 3)
			{
				normal = array2[3];
				return true;
			}
			if (edge0 + 1 == edge1)
			{
				normal = array[edge0];
				return true;
			}
			if (edge0 == edge1 + 1)
			{
				normal = array2[edge1];
				return true;
			}
			return false;
		}

		/// <summary>
		/// Initializes a new instance of the MyOrientedBoundingBox.
		/// Scale of matrix is size of box
		/// </summary>
		public MyOrientedBoundingBoxD(MatrixD matrix)
		{
			Center = matrix.Translation;
			Vector3D value = new Vector3D(matrix.Right.Length(), matrix.Up.Length(), matrix.Forward.Length());
			HalfExtent = value / 2.0;
			matrix.Right /= value.X;
			matrix.Up /= value.Y;
			matrix.Forward /= value.Z;
			Quaternion.CreateFromRotationMatrix(ref matrix, out Orientation);
		}

		public MyOrientedBoundingBoxD(Vector3D center, Vector3D halfExtents, Quaternion orientation)
		{
			Center = center;
			HalfExtent = halfExtents;
			Orientation = orientation;
		}

		public MyOrientedBoundingBoxD(BoundingBoxD box, MatrixD transform)
		{
			Center = (box.Min + box.Max) * 0.5;
			HalfExtent = (box.Max - box.Min) * 0.5;
			Center = Vector3D.Transform(Center, transform);
			Orientation = Quaternion.CreateFromRotationMatrix(transform);
		}

		public static MyOrientedBoundingBoxD CreateFromBoundingBox(BoundingBoxD box)
		{
			Vector3D center = (box.Min + box.Max) * 0.5;
			Vector3D halfExtents = (box.Max - box.Min) * 0.5;
			return new MyOrientedBoundingBoxD(center, halfExtents, Quaternion.Identity);
		}

		public MyOrientedBoundingBoxD Transform(Quaternion rotation, Vector3D translation)
		{
			return new MyOrientedBoundingBoxD(Vector3D.Transform(Center, rotation) + translation, HalfExtent, Orientation * rotation);
		}

		public MyOrientedBoundingBoxD Transform(float scale, Quaternion rotation, Vector3D translation)
		{
			return new MyOrientedBoundingBoxD(Vector3D.Transform(Center * scale, rotation) + translation, HalfExtent * scale, Orientation * rotation);
		}

		public void Transform(MatrixD matrix)
		{
			Center = Vector3D.Transform(Center, matrix);
			Orientation = Quaternion.CreateFromRotationMatrix(MatrixD.CreateFromQuaternion(Orientation) * matrix);
		}

		public bool Equals(MyOrientedBoundingBox other)
		{
			if (Center == other.Center && HalfExtent == other.HalfExtent)
			{
				return Orientation == other.Orientation;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is MyOrientedBoundingBox)
			{
				MyOrientedBoundingBox myOrientedBoundingBox = (MyOrientedBoundingBox)obj;
				if (Center == myOrientedBoundingBox.Center && HalfExtent == myOrientedBoundingBox.HalfExtent)
				{
					return Orientation == myOrientedBoundingBox.Orientation;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Center.GetHashCode() ^ HalfExtent.GetHashCode() ^ Orientation.GetHashCode();
		}

		public static bool operator ==(MyOrientedBoundingBoxD a, MyOrientedBoundingBoxD b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(MyOrientedBoundingBoxD a, MyOrientedBoundingBoxD b)
		{
			return !a.Equals(b);
		}

		public override string ToString()
		{
			return "{Center:" + Center.ToString() + " Extents:" + HalfExtent.ToString() + " Orientation:" + Orientation.ToString() + "}";
		}

		public bool Intersects(ref BoundingBox box)
		{
			Vector3D value = (box.Max + box.Min) * 0.5f;
			Vector3D hA = (box.Max - box.Min) * 0.5f;
			MatrixD mB = MatrixD.CreateFromQuaternion(Orientation);
			mB.Translation = Center - value;
			return ContainsRelativeBox(ref hA, ref HalfExtent, ref mB) != ContainmentType.Disjoint;
		}

		public bool Intersects(ref BoundingBoxD box)
		{
			Vector3D value = (box.Max + box.Min) * 0.5;
			Vector3D hA = (box.Max - box.Min) * 0.5;
			MatrixD mB = MatrixD.CreateFromQuaternion(Orientation);
			mB.Translation = Center - value;
			return ContainsRelativeBox(ref hA, ref HalfExtent, ref mB) != ContainmentType.Disjoint;
		}

		public ContainmentType Contains(ref BoundingBox box)
		{
			BoundingBoxD box2 = box;
			return Contains(ref box2);
		}

		public ContainmentType Contains(ref BoundingBoxD box)
		{
			Vector3D value = (box.Max + box.Min) * 0.5;
			Vector3D hB = (box.Max - box.Min) * 0.5;
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			MatrixD mB = MatrixD.CreateFromQuaternion(result);
			mB.Translation = Vector3D.TransformNormal(value - Center, mB);
			return ContainsRelativeBox(ref HalfExtent, ref hB, ref mB);
		}

		public static ContainmentType Contains(ref BoundingBox boxA, ref MyOrientedBoundingBox oboxB)
		{
			Vector3 hA = (boxA.Max - boxA.Min) * 0.5f;
			Vector3 value = (boxA.Max + boxA.Min) * 0.5f;
			Matrix mB = Matrix.CreateFromQuaternion(oboxB.Orientation);
			mB.Translation = oboxB.Center - value;
			return MyOrientedBoundingBox.ContainsRelativeBox(ref hA, ref oboxB.HalfExtent, ref mB);
		}

		public bool Intersects(ref MyOrientedBoundingBoxD other)
		{
			return Contains(ref other) != ContainmentType.Disjoint;
		}

		public ContainmentType Contains(ref MyOrientedBoundingBoxD other)
		{
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			Quaternion.Multiply(ref result, ref other.Orientation, out Quaternion result2);
			MatrixD mB = MatrixD.CreateFromQuaternion(result2);
			mB.Translation = Vector3D.Transform(other.Center - Center, result);
			return ContainsRelativeBox(ref HalfExtent, ref other.HalfExtent, ref mB);
		}

		public ContainmentType Contains(BoundingFrustumD frustum)
		{
			return ConvertToFrustum().Contains(frustum);
		}

		public bool Intersects(BoundingFrustumD frustum)
		{
			return Contains(frustum) != ContainmentType.Disjoint;
		}

		public static ContainmentType Contains(BoundingFrustumD frustum, ref MyOrientedBoundingBoxD obox)
		{
			return frustum.Contains(obox.ConvertToFrustum());
		}

		public ContainmentType Contains(ref BoundingSphereD sphere)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3 vector = Vector3.Transform(sphere.Center - Center, rotation);
			double num = (double)Math.Abs(vector.X) - HalfExtent.X;
			double num2 = (double)Math.Abs(vector.Y) - HalfExtent.Y;
			double num3 = (double)Math.Abs(vector.Z) - HalfExtent.Z;
			double radius = sphere.Radius;
			if (num <= 0.0 - radius && num2 <= 0.0 - radius && num3 <= 0.0 - radius)
			{
				return ContainmentType.Contains;
			}
			num = Math.Max(num, 0.0);
			num2 = Math.Max(num2, 0.0);
			num3 = Math.Max(num3, 0.0);
			if (num * num + num2 * num2 + num3 * num3 >= radius * radius)
			{
				return ContainmentType.Disjoint;
			}
			return ContainmentType.Intersects;
		}

		public bool Intersects(ref BoundingSphereD sphere)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3 vector = Vector3.Transform(sphere.Center - Center, rotation);
			double val = (double)Math.Abs(vector.X) - HalfExtent.X;
			double val2 = (double)Math.Abs(vector.Y) - HalfExtent.Y;
			double val3 = (double)Math.Abs(vector.Z) - HalfExtent.Z;
			val = Math.Max(val, 0.0);
			val2 = Math.Max(val2, 0.0);
			val3 = Math.Max(val3, 0.0);
			double radius = sphere.Radius;
			return val * val + val2 * val2 + val3 * val3 < radius * radius;
		}

		public static ContainmentType Contains(ref BoundingSphere sphere, ref MyOrientedBoundingBox box)
		{
			Quaternion rotation = Quaternion.Conjugate(box.Orientation);
			Vector3 value = Vector3.Transform(sphere.Center - box.Center, rotation);
			value.X = Math.Abs(value.X);
			value.Y = Math.Abs(value.Y);
			value.Z = Math.Abs(value.Z);
			float num = sphere.Radius * sphere.Radius;
			if ((value + box.HalfExtent).LengthSquared() <= num)
			{
				return ContainmentType.Contains;
			}
			Vector3 vector = value - box.HalfExtent;
			vector.X = Math.Max(vector.X, 0f);
			vector.Y = Math.Max(vector.Y, 0f);
			vector.Z = Math.Max(vector.Z, 0f);
			if (vector.LengthSquared() >= num)
			{
				return ContainmentType.Disjoint;
			}
			return ContainmentType.Intersects;
		}

		public bool Contains(ref Vector3 point)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3 vector = Vector3D.Transform(point - Center, rotation);
			if ((double)Math.Abs(vector.X) <= HalfExtent.X && (double)Math.Abs(vector.Y) <= HalfExtent.Y)
			{
				return (double)Math.Abs(vector.Z) <= HalfExtent.Z;
			}
			return false;
		}

		public bool Contains(ref Vector3D point)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3D vector3D = Vector3D.Transform(point - Center, rotation);
			if (Math.Abs(vector3D.X) <= HalfExtent.X && Math.Abs(vector3D.Y) <= HalfExtent.Y)
			{
				return Math.Abs(vector3D.Z) <= HalfExtent.Z;
			}
			return false;
		}

		public double? Intersects(ref RayD ray)
		{
			MatrixD matrixD = Matrix.CreateFromQuaternion(Orientation);
			Vector3D vector = Center - ray.Position;
			double num = double.MinValue;
			double num2 = double.MaxValue;
			double num3 = Vector3D.Dot(matrixD.Right, vector);
			double num4 = Vector3D.Dot(matrixD.Right, ray.Direction);
			if (num4 >= -9.9999996826552254E-21 && num4 <= 9.9999996826552254E-21)
			{
				if (0.0 - num3 - HalfExtent.X > 0.0 || 0.0 - num3 + HalfExtent.X < 0.0)
				{
					return null;
				}
			}
			else
			{
				double num5 = (num3 - HalfExtent.X) / num4;
				double num6 = (num3 + HalfExtent.X) / num4;
				if (num5 > num6)
				{
					double num7 = num5;
					num5 = num6;
					num6 = num7;
				}
				if (num5 > num)
				{
					num = num5;
				}
				if (num6 < num2)
				{
					num2 = num6;
				}
				if (num2 < 0.0 || num > num2)
				{
					return null;
				}
			}
			num3 = Vector3D.Dot(matrixD.Up, vector);
			num4 = Vector3D.Dot(matrixD.Up, ray.Direction);
			if (num4 >= -9.9999996826552254E-21 && num4 <= 9.9999996826552254E-21)
			{
				if (0.0 - num3 - HalfExtent.Y > 0.0 || 0.0 - num3 + HalfExtent.Y < 0.0)
				{
					return null;
				}
			}
			else
			{
				double num8 = (num3 - HalfExtent.Y) / num4;
				double num9 = (num3 + HalfExtent.Y) / num4;
				if (num8 > num9)
				{
					double num10 = num8;
					num8 = num9;
					num9 = num10;
				}
				if (num8 > num)
				{
					num = num8;
				}
				if (num9 < num2)
				{
					num2 = num9;
				}
				if (num2 < 0.0 || num > num2)
				{
					return null;
				}
			}
			num3 = Vector3D.Dot(matrixD.Forward, vector);
			num4 = Vector3D.Dot(matrixD.Forward, ray.Direction);
			if (num4 >= -9.9999996826552254E-21 && num4 <= 9.9999996826552254E-21)
			{
				if (0.0 - num3 - HalfExtent.Z > 0.0 || 0.0 - num3 + HalfExtent.Z < 0.0)
				{
					return null;
				}
			}
			else
			{
				double num11 = (num3 - HalfExtent.Z) / num4;
				double num12 = (num3 + HalfExtent.Z) / num4;
				if (num11 > num12)
				{
					double num13 = num11;
					num11 = num12;
					num12 = num13;
				}
				if (num11 > num)
				{
					num = num11;
				}
				if (num12 < num2)
				{
					num2 = num12;
				}
				if (num2 < 0.0 || num > num2)
				{
					return null;
				}
			}
			return num;
		}

		public double? Intersects(ref LineD line)
		{
			if (Contains(ref line.From))
			{
				RayD ray = new RayD(line.To, -line.Direction);
				double? num = Intersects(ref ray);
				if (num.HasValue)
				{
					double num2 = line.Length - num.Value;
					if (num2 < 0.0)
					{
						return null;
					}
					if (num2 > line.Length)
					{
						return null;
					}
					return num2;
				}
				return null;
			}
			RayD ray2 = new RayD(line.From, line.Direction);
			double? num3 = Intersects(ref ray2);
			if (num3.HasValue)
			{
				if (num3.Value < 0.0)
				{
					return null;
				}
				if (num3.Value > line.Length)
				{
					return null;
				}
				return num3.Value;
			}
			return null;
		}

		public PlaneIntersectionType Intersects(ref PlaneD plane)
		{
			double num = plane.DotCoordinate(Center);
			Vector3D vector3D = Vector3D.Transform(plane.Normal, Quaternion.Conjugate(Orientation));
			double num2 = Math.Abs(HalfExtent.X * vector3D.X) + Math.Abs(HalfExtent.Y * vector3D.Y) + Math.Abs(HalfExtent.Z * vector3D.Z);
			if (num > num2)
			{
				return PlaneIntersectionType.Front;
			}
			if (num < 0.0 - num2)
			{
				return PlaneIntersectionType.Back;
			}
			return PlaneIntersectionType.Intersecting;
		}

		public void GetCorners(Vector3D[] corners, int startIndex)
		{
			MatrixD matrixD = MatrixD.CreateFromQuaternion(Orientation);
			Vector3D value = matrixD.Left * HalfExtent.X;
			Vector3D value2 = matrixD.Up * HalfExtent.Y;
			Vector3D value3 = matrixD.Backward * HalfExtent.Z;
			int num = startIndex;
			corners[num++] = Center - value + value2 + value3;
			corners[num++] = Center + value + value2 + value3;
			corners[num++] = Center + value - value2 + value3;
			corners[num++] = Center - value - value2 + value3;
			corners[num++] = Center - value + value2 - value3;
			corners[num++] = Center + value + value2 - value3;
			corners[num++] = Center + value - value2 - value3;
			corners[num++] = Center - value - value2 - value3;
		}

		public static ContainmentType ContainsRelativeBox(ref Vector3D hA, ref Vector3D hB, ref MatrixD mB)
		{
			Vector3D translation = mB.Translation;
			Vector3D vector3D = new Vector3D(Math.Abs(translation.X), Math.Abs(translation.Y), Math.Abs(translation.Z));
			Vector3D right = mB.Right;
			Vector3D up = mB.Up;
			Vector3D backward = mB.Backward;
			Vector3D vector = right * hB.X;
			Vector3D vector2 = up * hB.Y;
			Vector3D vector3 = backward * hB.Z;
			double num = Math.Abs(vector.X) + Math.Abs(vector2.X) + Math.Abs(vector3.X);
			double num2 = Math.Abs(vector.Y) + Math.Abs(vector2.Y) + Math.Abs(vector3.Y);
			double num3 = Math.Abs(vector.Z) + Math.Abs(vector2.Z) + Math.Abs(vector3.Z);
			if (vector3D.X + num <= hA.X && vector3D.Y + num2 <= hA.Y && vector3D.Z + num3 <= hA.Z)
			{
				return ContainmentType.Contains;
			}
			if (vector3D.X > hA.X + Math.Abs(vector.X) + Math.Abs(vector2.X) + Math.Abs(vector3.X))
			{
				return ContainmentType.Disjoint;
			}
			if (vector3D.Y > hA.Y + Math.Abs(vector.Y) + Math.Abs(vector2.Y) + Math.Abs(vector3.Y))
			{
				return ContainmentType.Disjoint;
			}
			if (vector3D.Z > hA.Z + Math.Abs(vector.Z) + Math.Abs(vector2.Z) + Math.Abs(vector3.Z))
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3D.Dot(translation, right)) > Math.Abs(hA.X * right.X) + Math.Abs(hA.Y * right.Y) + Math.Abs(hA.Z * right.Z) + hB.X)
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3D.Dot(translation, up)) > Math.Abs(hA.X * up.X) + Math.Abs(hA.Y * up.Y) + Math.Abs(hA.Z * up.Z) + hB.Y)
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3D.Dot(translation, backward)) > Math.Abs(hA.X * backward.X) + Math.Abs(hA.Y * backward.Y) + Math.Abs(hA.Z * backward.Z) + hB.Z)
			{
				return ContainmentType.Disjoint;
			}
			Vector3D vector3D2 = new Vector3D(0.0, 0.0 - right.Z, right.Y);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector2)) + Math.Abs(Vector3D.Dot(vector3D2, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(0.0, 0.0 - up.Z, up.Y);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector3)) + Math.Abs(Vector3D.Dot(vector3D2, vector)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(0.0, 0.0 - backward.Z, backward.Y);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector)) + Math.Abs(Vector3D.Dot(vector3D2, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(right.Z, 0.0, 0.0 - right.X);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector2)) + Math.Abs(Vector3D.Dot(vector3D2, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(up.Z, 0.0, 0.0 - up.X);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector3)) + Math.Abs(Vector3D.Dot(vector3D2, vector)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(backward.Z, 0.0, 0.0 - backward.X);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector)) + Math.Abs(Vector3D.Dot(vector3D2, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(0.0 - right.Y, right.X, 0.0);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector2)) + Math.Abs(Vector3D.Dot(vector3D2, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(0.0 - up.Y, up.X, 0.0);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector3)) + Math.Abs(Vector3D.Dot(vector3D2, vector)))
			{
				return ContainmentType.Disjoint;
			}
			vector3D2 = new Vector3D(0.0 - backward.Y, backward.X, 0.0);
			if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector)) + Math.Abs(Vector3D.Dot(vector3D2, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			return ContainmentType.Intersects;
		}

		public BoundingFrustumD ConvertToFrustum()
		{
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			double num = 1.0 / HalfExtent.X;
			double num2 = 1.0 / HalfExtent.Y;
			double num3 = 0.5 / HalfExtent.Z;
			MatrixD.CreateFromQuaternion(ref result, out MatrixD result2);
			result2.M11 *= num;
			result2.M21 *= num;
			result2.M31 *= num;
			result2.M12 *= num2;
			result2.M22 *= num2;
			result2.M32 *= num2;
			result2.M13 *= num3;
			result2.M23 *= num3;
			result2.M33 *= num3;
			result2.Translation = Vector3.UnitZ * 0.5f + Vector3D.TransformNormal(-Center, result2);
			return new BoundingFrustumD(result2);
		}

		public BoundingBoxD GetAABB()
		{
			if (m_cornersTmp == null)
			{
				m_cornersTmp = new Vector3D[8];
			}
			GetCorners(m_cornersTmp, 0);
			BoundingBoxD result = BoundingBoxD.CreateInvalid();
			for (int i = 0; i < 8; i++)
			{
				result.Include(m_cornersTmp[i]);
			}
			return result;
		}

		public static MyOrientedBoundingBoxD Create(BoundingBoxD boundingBox, MatrixD matrix)
		{
			MyOrientedBoundingBoxD result = new MyOrientedBoundingBoxD(boundingBox.Center, boundingBox.HalfExtents, Quaternion.Identity);
			result.Transform(matrix);
			return result;
		}
	}
}
