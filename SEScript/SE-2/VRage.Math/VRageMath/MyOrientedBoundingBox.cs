using System;

namespace VRageMath
{
	public struct MyOrientedBoundingBox : IEquatable<MyOrientedBoundingBox>
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

		public Vector3 Center;

		public Vector3 HalfExtent;

		public Quaternion Orientation;

		[ThreadStatic]
		private static Vector3[] m_cornersTmp;

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
		public MyOrientedBoundingBox(ref Matrix matrix)
		{
			Center = matrix.Translation;
			Vector3 value = new Vector3(matrix.Right.Length(), matrix.Up.Length(), matrix.Forward.Length());
			HalfExtent = value / 2f;
			matrix.Right /= value.X;
			matrix.Up /= value.Y;
			matrix.Forward /= value.Z;
			Quaternion.CreateFromRotationMatrix(ref matrix, out Orientation);
		}

		public MyOrientedBoundingBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
		{
			Center = center;
			HalfExtent = halfExtents;
			Orientation = orientation;
		}

		public static MyOrientedBoundingBox CreateFromBoundingBox(BoundingBox box)
		{
			Vector3 center = (box.Min + box.Max) * 0.5f;
			Vector3 halfExtents = (box.Max - box.Min) * 0.5f;
			return new MyOrientedBoundingBox(center, halfExtents, Quaternion.Identity);
		}

		public MyOrientedBoundingBox Transform(Quaternion rotation, Vector3 translation)
		{
			return new MyOrientedBoundingBox(Vector3.Transform(Center, rotation) + translation, HalfExtent, Orientation * rotation);
		}

		public MyOrientedBoundingBox Transform(float scale, Quaternion rotation, Vector3 translation)
		{
			return new MyOrientedBoundingBox(Vector3.Transform(Center * scale, rotation) + translation, HalfExtent * scale, Orientation * rotation);
		}

		public void Transform(Matrix matrix)
		{
			Center = Vector3.Transform(Center, matrix);
			Orientation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromQuaternion(Orientation) * matrix);
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

		public static bool operator ==(MyOrientedBoundingBox a, MyOrientedBoundingBox b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(MyOrientedBoundingBox a, MyOrientedBoundingBox b)
		{
			return !a.Equals(b);
		}

		public override string ToString()
		{
			return "{Center:" + Center.ToString() + " Extents:" + HalfExtent.ToString() + " Orientation:" + Orientation.ToString() + "}";
		}

		public bool Intersects(ref BoundingBox box)
		{
			Vector3 value = (box.Max + box.Min) * 0.5f;
			Vector3 hA = (box.Max - box.Min) * 0.5f;
			Matrix mB = Matrix.CreateFromQuaternion(Orientation);
			mB.Translation = Center - value;
			return ContainsRelativeBox(ref hA, ref HalfExtent, ref mB) != ContainmentType.Disjoint;
		}

		public ContainmentType Contains(ref BoundingBox box)
		{
			Vector3 value = (box.Max + box.Min) * 0.5f;
			Vector3 hB = (box.Max - box.Min) * 0.5f;
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			Matrix mB = Matrix.CreateFromQuaternion(result);
			mB.Translation = Vector3.TransformNormal(value - Center, mB);
			return ContainsRelativeBox(ref HalfExtent, ref hB, ref mB);
		}

		public static ContainmentType Contains(ref BoundingBox boxA, ref MyOrientedBoundingBox oboxB)
		{
			Vector3 hA = (boxA.Max - boxA.Min) * 0.5f;
			Vector3 value = (boxA.Max + boxA.Min) * 0.5f;
			Matrix mB = Matrix.CreateFromQuaternion(oboxB.Orientation);
			mB.Translation = oboxB.Center - value;
			return ContainsRelativeBox(ref hA, ref oboxB.HalfExtent, ref mB);
		}

		public bool Intersects(ref MyOrientedBoundingBox other)
		{
			return Contains(ref other) != ContainmentType.Disjoint;
		}

		public ContainmentType Contains(ref MyOrientedBoundingBox other)
		{
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			Quaternion.Multiply(ref result, ref other.Orientation, out Quaternion result2);
			Matrix mB = Matrix.CreateFromQuaternion(result2);
			mB.Translation = Vector3.Transform(other.Center - Center, result);
			return ContainsRelativeBox(ref HalfExtent, ref other.HalfExtent, ref mB);
		}

		public ContainmentType Contains(BoundingFrustum frustum)
		{
			return ConvertToFrustum().Contains(frustum);
		}

		public bool Intersects(BoundingFrustum frustum)
		{
			return Contains(frustum) != ContainmentType.Disjoint;
		}

		public static ContainmentType Contains(BoundingFrustum frustum, ref MyOrientedBoundingBox obox)
		{
			return frustum.Contains(obox.ConvertToFrustum());
		}

		public ContainmentType Contains(ref BoundingSphere sphere)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3 vector = Vector3.Transform(sphere.Center - Center, rotation);
			float num = Math.Abs(vector.X) - HalfExtent.X;
			float num2 = Math.Abs(vector.Y) - HalfExtent.Y;
			float num3 = Math.Abs(vector.Z) - HalfExtent.Z;
			float radius = sphere.Radius;
			if (num <= 0f - radius && num2 <= 0f - radius && num3 <= 0f - radius)
			{
				return ContainmentType.Contains;
			}
			num = Math.Max(num, 0f);
			num2 = Math.Max(num2, 0f);
			num3 = Math.Max(num3, 0f);
			if (num * num + num2 * num2 + num3 * num3 >= radius * radius)
			{
				return ContainmentType.Disjoint;
			}
			return ContainmentType.Intersects;
		}

		public bool Intersects(ref BoundingSphere sphere)
		{
			Quaternion rotation = Quaternion.Conjugate(Orientation);
			Vector3 vector = Vector3.Transform(sphere.Center - Center, rotation);
			float val = Math.Abs(vector.X) - HalfExtent.X;
			float val2 = Math.Abs(vector.Y) - HalfExtent.Y;
			float val3 = Math.Abs(vector.Z) - HalfExtent.Z;
			val = Math.Max(val, 0f);
			val2 = Math.Max(val2, 0f);
			val3 = Math.Max(val3, 0f);
			float radius = sphere.Radius;
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
			Vector3 vector = Vector3.Transform(point - Center, rotation);
			if (Math.Abs(vector.X) <= HalfExtent.X && Math.Abs(vector.Y) <= HalfExtent.Y)
			{
				return Math.Abs(vector.Z) <= HalfExtent.Z;
			}
			return false;
		}

		public float? Intersects(ref Ray ray)
		{
			Matrix matrix = Matrix.CreateFromQuaternion(Orientation);
			Vector3 vector = Center - ray.Position;
			float num = float.MinValue;
			float num2 = float.MaxValue;
			float num3 = Vector3.Dot(matrix.Right, vector);
			float num4 = Vector3.Dot(matrix.Right, ray.Direction);
			if (num4 >= -1E-20f && num4 <= 1E-20f)
			{
				if ((double)(0f - num3 - HalfExtent.X) > 0.0 || 0f - num3 + HalfExtent.X < 0f)
				{
					return null;
				}
			}
			else
			{
				float num5 = (num3 - HalfExtent.X) / num4;
				float num6 = (num3 + HalfExtent.X) / num4;
				if (num5 > num6)
				{
					float num7 = num5;
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
				if (num2 < 0f || num > num2)
				{
					return null;
				}
			}
			num3 = Vector3.Dot(matrix.Up, vector);
			num4 = Vector3.Dot(matrix.Up, ray.Direction);
			if (num4 >= -1E-20f && num4 <= 1E-20f)
			{
				if ((double)(0f - num3 - HalfExtent.Y) > 0.0 || 0f - num3 + HalfExtent.Y < 0f)
				{
					return null;
				}
			}
			else
			{
				float num8 = (num3 - HalfExtent.Y) / num4;
				float num9 = (num3 + HalfExtent.Y) / num4;
				if (num8 > num9)
				{
					float num10 = num8;
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
				if (num2 < 0f || num > num2)
				{
					return null;
				}
			}
			num3 = Vector3.Dot(matrix.Forward, vector);
			num4 = Vector3.Dot(matrix.Forward, ray.Direction);
			if (num4 >= -1E-20f && num4 <= 1E-20f)
			{
				if ((double)(0f - num3 - HalfExtent.Z) > 0.0 || 0f - num3 + HalfExtent.Z < 0f)
				{
					return null;
				}
			}
			else
			{
				float num11 = (num3 - HalfExtent.Z) / num4;
				float num12 = (num3 + HalfExtent.Z) / num4;
				if (num11 > num12)
				{
					float num13 = num11;
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
				if (num2 < 0f || num > num2)
				{
					return null;
				}
			}
			return num;
		}

		public float? Intersects(ref Line line)
		{
			if (Contains(ref line.From))
			{
				Ray ray = new Ray(line.To, -line.Direction);
				float? num = Intersects(ref ray);
				if (num.HasValue)
				{
					float num2 = line.Length - num.Value;
					if (num2 < 0f)
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
			Ray ray2 = new Ray(line.From, line.Direction);
			float? num3 = Intersects(ref ray2);
			if (num3.HasValue)
			{
				if (num3.Value < 0f)
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

		public PlaneIntersectionType Intersects(ref Plane plane)
		{
			float num = plane.DotCoordinate(Center);
			Vector3 vector = Vector3.Transform(plane.Normal, Quaternion.Conjugate(Orientation));
			float num2 = Math.Abs(HalfExtent.X * vector.X) + Math.Abs(HalfExtent.Y * vector.Y) + Math.Abs(HalfExtent.Z * vector.Z);
			if (num > num2)
			{
				return PlaneIntersectionType.Front;
			}
			if (num < 0f - num2)
			{
				return PlaneIntersectionType.Back;
			}
			return PlaneIntersectionType.Intersecting;
		}

		public void GetCorners(Vector3[] corners, int startIndex)
		{
			Matrix matrix = Matrix.CreateFromQuaternion(Orientation);
			Vector3 value = matrix.Left * HalfExtent.X;
			Vector3 value2 = matrix.Up * HalfExtent.Y;
			Vector3 value3 = matrix.Backward * HalfExtent.Z;
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

		public static ContainmentType ContainsRelativeBox(ref Vector3 hA, ref Vector3 hB, ref Matrix mB)
		{
			Vector3 translation = mB.Translation;
			Vector3 vector = new Vector3(Math.Abs(translation.X), Math.Abs(translation.Y), Math.Abs(translation.Z));
			Vector3 right = mB.Right;
			Vector3 up = mB.Up;
			Vector3 backward = mB.Backward;
			Vector3 vector2 = right * hB.X;
			Vector3 vector3 = up * hB.Y;
			Vector3 vector4 = backward * hB.Z;
			float num = Math.Abs(vector2.X) + Math.Abs(vector3.X) + Math.Abs(vector4.X);
			float num2 = Math.Abs(vector2.Y) + Math.Abs(vector3.Y) + Math.Abs(vector4.Y);
			float num3 = Math.Abs(vector2.Z) + Math.Abs(vector3.Z) + Math.Abs(vector4.Z);
			if (vector.X + num <= hA.X && vector.Y + num2 <= hA.Y && vector.Z + num3 <= hA.Z)
			{
				return ContainmentType.Contains;
			}
			if (vector.X > hA.X + Math.Abs(vector2.X) + Math.Abs(vector3.X) + Math.Abs(vector4.X))
			{
				return ContainmentType.Disjoint;
			}
			if (vector.Y > hA.Y + Math.Abs(vector2.Y) + Math.Abs(vector3.Y) + Math.Abs(vector4.Y))
			{
				return ContainmentType.Disjoint;
			}
			if (vector.Z > hA.Z + Math.Abs(vector2.Z) + Math.Abs(vector3.Z) + Math.Abs(vector4.Z))
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3.Dot(translation, right)) > Math.Abs(hA.X * right.X) + Math.Abs(hA.Y * right.Y) + Math.Abs(hA.Z * right.Z) + hB.X)
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3.Dot(translation, up)) > Math.Abs(hA.X * up.X) + Math.Abs(hA.Y * up.Y) + Math.Abs(hA.Z * up.Z) + hB.Y)
			{
				return ContainmentType.Disjoint;
			}
			if (Math.Abs(Vector3.Dot(translation, backward)) > Math.Abs(hA.X * backward.X) + Math.Abs(hA.Y * backward.Y) + Math.Abs(hA.Z * backward.Z) + hB.Z)
			{
				return ContainmentType.Disjoint;
			}
			Vector3 vector5 = new Vector3(0f, 0f - right.Z, right.Y);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Y * vector5.Y) + Math.Abs(hA.Z * vector5.Z) + Math.Abs(Vector3.Dot(vector5, vector3)) + Math.Abs(Vector3.Dot(vector5, vector4)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(0f, 0f - up.Z, up.Y);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Y * vector5.Y) + Math.Abs(hA.Z * vector5.Z) + Math.Abs(Vector3.Dot(vector5, vector4)) + Math.Abs(Vector3.Dot(vector5, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(0f, 0f - backward.Z, backward.Y);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Y * vector5.Y) + Math.Abs(hA.Z * vector5.Z) + Math.Abs(Vector3.Dot(vector5, vector2)) + Math.Abs(Vector3.Dot(vector5, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(right.Z, 0f, 0f - right.X);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Z * vector5.Z) + Math.Abs(hA.X * vector5.X) + Math.Abs(Vector3.Dot(vector5, vector3)) + Math.Abs(Vector3.Dot(vector5, vector4)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(up.Z, 0f, 0f - up.X);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Z * vector5.Z) + Math.Abs(hA.X * vector5.X) + Math.Abs(Vector3.Dot(vector5, vector4)) + Math.Abs(Vector3.Dot(vector5, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(backward.Z, 0f, 0f - backward.X);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.Z * vector5.Z) + Math.Abs(hA.X * vector5.X) + Math.Abs(Vector3.Dot(vector5, vector2)) + Math.Abs(Vector3.Dot(vector5, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(0f - right.Y, right.X, 0f);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.X * vector5.X) + Math.Abs(hA.Y * vector5.Y) + Math.Abs(Vector3.Dot(vector5, vector3)) + Math.Abs(Vector3.Dot(vector5, vector4)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(0f - up.Y, up.X, 0f);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.X * vector5.X) + Math.Abs(hA.Y * vector5.Y) + Math.Abs(Vector3.Dot(vector5, vector4)) + Math.Abs(Vector3.Dot(vector5, vector2)))
			{
				return ContainmentType.Disjoint;
			}
			vector5 = new Vector3(0f - backward.Y, backward.X, 0f);
			if (Math.Abs(Vector3.Dot(translation, vector5)) > Math.Abs(hA.X * vector5.X) + Math.Abs(hA.Y * vector5.Y) + Math.Abs(Vector3.Dot(vector5, vector2)) + Math.Abs(Vector3.Dot(vector5, vector3)))
			{
				return ContainmentType.Disjoint;
			}
			return ContainmentType.Intersects;
		}

		public BoundingFrustum ConvertToFrustum()
		{
			Quaternion.Conjugate(ref Orientation, out Quaternion result);
			float num = 1f / HalfExtent.X;
			float num2 = 1f / HalfExtent.Y;
			float num3 = 0.5f / HalfExtent.Z;
			Matrix.CreateFromQuaternion(ref result, out Matrix result2);
			result2.M11 *= num;
			result2.M21 *= num;
			result2.M31 *= num;
			result2.M12 *= num2;
			result2.M22 *= num2;
			result2.M32 *= num2;
			result2.M13 *= num3;
			result2.M23 *= num3;
			result2.M33 *= num3;
			result2.Translation = Vector3.UnitZ * 0.5f + Vector3.TransformNormal(-Center, result2);
			return new BoundingFrustum(result2);
		}

		public BoundingBox GetAABB()
		{
			if (m_cornersTmp == null)
			{
				m_cornersTmp = new Vector3[8];
			}
			GetCorners(m_cornersTmp, 0);
			BoundingBox result = BoundingBox.CreateInvalid();
			for (int i = 0; i < 8; i++)
			{
				result.Include(m_cornersTmp[i]);
			}
			return result;
		}

		public static MyOrientedBoundingBox Create(BoundingBox boundingBox, Matrix matrix)
		{
			MyOrientedBoundingBox result = new MyOrientedBoundingBox(boundingBox.Center, boundingBox.HalfExtents, Quaternion.Identity);
			result.Transform(matrix);
			return result;
		}
	}
}
