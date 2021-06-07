using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Utils
{
	/// <summary>
	/// MyFileSystemUtils
	/// </summary>
	public static class MyUtils
	{
		private struct Edge : IEquatable<Edge>
		{
			public int I0;

			public int I1;

			public bool Equals(Edge other)
			{
				return object.Equals(other.GetHashCode(), GetHashCode());
			}

			public override int GetHashCode()
			{
				if (I0 >= I1)
				{
					return (I1.GetHashCode() * 397) ^ I0.GetHashCode();
				}
				return (I0.GetHashCode() * 397) ^ I1.GetHashCode();
			}
		}

		public struct ClearCollectionToken<TCollection, TElement> : IDisposable where TCollection : class, ICollection<TElement>, new()
		{
			public readonly TCollection Collection;

			public ClearCollectionToken(TCollection collection)
			{
				Collection = collection;
			}

			public void Dispose()
			{
				Collection.Clear();
			}
		}

		public struct ClearRangeToken<T> : IDisposable
		{
			public struct OffsetEnumerator : IEnumerator<T>, IEnumerator, IDisposable
			{
				private readonly int End;

				private int Index;

				private readonly List<T> List;

				public T Current => List[Index];

				object IEnumerator.Current => List[Index];

				public OffsetEnumerator(List<T> list, int begin)
				{
					List = list;
					End = list.Count;
					Index = begin - 1;
				}

				public bool MoveNext()
				{
					Index++;
					return Index < End;
				}

				public void Dispose()
				{
				}

				public void Reset()
				{
					throw new NotImplementedException();
				}
			}

			public readonly int Begin;

			public readonly List<T> Collection;

			public ClearRangeToken(List<T> collection)
			{
				Collection = collection;
				Begin = collection.Count;
			}

			public void Dispose()
			{
				int count = Collection.Count - Begin;
				Collection.RemoveRange(Begin, count);
			}

			public void Add(T element)
			{
				Collection.Add(element);
			}

			public OffsetEnumerator GetEnumerator()
			{
				return new OffsetEnumerator(Collection, Begin);
			}
		}

		private const int HashSeed = -2128831035;

		[ThreadStatic]
		private static Random m_secretRandom;

		private static byte[] m_randomBuffer = new byte[8];

		public const string C_CRLF = "\r\n";

		/// <summary>
		/// Default number suffix, k = thousand, m = million, g/b = billion
		/// </summary>
		public static Tuple<string, float>[] DefaultNumberSuffix = new Tuple<string, float>[4]
		{
			new Tuple<string, float>("k", 1000f),
			new Tuple<string, float>("m", 1000000f),
			new Tuple<string, float>("g", 1E+09f),
			new Tuple<string, float>("b", 1E+09f)
		};

		public static readonly StringBuilder EmptyStringBuilder = new StringBuilder();

		public static readonly Matrix ZeroMatrix = new Matrix(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

		private static readonly string[] BYTE_SIZE_PREFIX = new string[5]
		{
			"",
			"K",
			"M",
			"G",
			"T"
		};

		private static Random Instance
		{
			get
			{
				if (m_secretRandom == null)
				{
					if (MyRandom.EnableDeterminism)
					{
						m_secretRandom = new Random(1);
					}
					else
					{
						m_secretRandom = new Random();
					}
				}
				return m_secretRandom;
			}
		}

		public static Thread MainThread
		{
			get;
			set;
		}

		/// <summary>
		/// Vytvori zadany adresar. Automaticky povytvara celu adresarovu strukturu, teda ak chcem vytvorit c:\volaco\opica
		/// a c:\volaco zatial neexistuje, tak tato metoda ho vytvori.
		/// </summary>
		/// <param name="folderPath"></param>
		public static void CreateFolder(string folderPath)
		{
			Directory.CreateDirectory(folderPath);
		}

		public static void CopyDirectory(string source, string destination)
		{
			if (Directory.Exists(source))
			{
				if (!Directory.Exists(destination))
				{
					Directory.CreateDirectory(destination);
				}
				string[] files = Directory.GetFiles(source);
				foreach (string obj in files)
				{
					string fileName = Path.GetFileName(obj);
					string destFileName = Path.Combine(destination, fileName);
					File.Copy(obj, destFileName, overwrite: true);
				}
			}
		}

		public static string StripInvalidChars(string filename)
		{
			return Path.GetInvalidFileNameChars().Aggregate(filename, (string current, char c) => current.Replace(c.ToString(), string.Empty));
		}

		private static int HashStep(int value, int hash)
		{
			hash ^= value;
			hash *= 16777619;
			return hash;
		}

		public unsafe static int GetHash(double d, int hash = -2128831035)
		{
			if (d == 0.0)
			{
				return hash;
			}
			ulong num = (ulong)(*(long*)(&d));
			hash = HashStep((int)num, HashStep((int)(num >> 32), hash));
			return hash;
		}

		public static int GetHash(string str, int hash = -2128831035)
		{
			if (str != null)
			{
				int i;
				for (i = 0; i < str.Length - 1; i += 2)
				{
					hash = HashStep((int)(((uint)str[i] << 16) + str[i + 1]), hash);
				}
				if ((str.Length & 1) != 0)
				{
					hash = HashStep(str[i], hash);
				}
			}
			return hash;
		}

		public static int GetHash(string str, int start, int length, int hash = -2128831035)
		{
			if (str == null)
			{
				return 0;
			}
			if (length < 0)
			{
				length = str.Length - start;
			}
			if (length <= 0)
			{
				return 0;
			}
			int num = start + length - 1;
			int i;
			for (i = start; i < num; i += 2)
			{
				hash = HashStep((int)(((uint)str[i] << 16) + str[i + 1]), hash);
			}
			if ((length & 1) != 0)
			{
				hash = HashStep(str[i], hash);
			}
			return hash;
		}

		public static int GetHashUpperCase(string str, int start, int length, int hash = -2128831035)
		{
			if (str == null)
			{
				return 0;
			}
			if (length < 0)
			{
				length = str.Length - start;
			}
			if (length <= 0)
			{
				return 0;
			}
			int num = start + length - 1;
			int i;
			for (i = start; i < num; i += 2)
			{
				hash = HashStep((int)(((uint)char.ToUpperInvariant(str[i]) << 16) + char.ToUpperInvariant(str[i + 1])), hash);
			}
			if ((length & 1) != 0)
			{
				hash = HashStep(char.ToUpperInvariant(str[i]), hash);
			}
			return hash;
		}

		public static void GetOpenBoundaries(Vector3[] vertices, int[] indices, List<Vector3> openBoundaries)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			for (int i = 0; i < vertices.Length; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (IsEqual(vertices[j], vertices[i]))
					{
						if (!dictionary.ContainsKey(j))
						{
							dictionary[j] = new List<int>();
						}
						dictionary[j].Add(i);
						break;
					}
				}
			}
			foreach (KeyValuePair<int, List<int>> item in dictionary)
			{
				foreach (int item2 in item.Value)
				{
					for (int k = 0; k < indices.Length; k++)
					{
						if (indices[k] == item2)
						{
							indices[k] = item.Key;
						}
					}
				}
			}
			Dictionary<Edge, int> dictionary2 = new Dictionary<Edge, int>();
			for (int l = 0; l < indices.Length; l += 3)
			{
				AddEdge(indices[l], indices[l + 1], dictionary2);
				AddEdge(indices[l + 1], indices[l + 2], dictionary2);
				AddEdge(indices[l + 2], indices[l], dictionary2);
			}
			openBoundaries.Clear();
			foreach (KeyValuePair<Edge, int> item3 in dictionary2)
			{
				if (item3.Value == 1)
				{
					openBoundaries.Add(vertices[item3.Key.I0]);
					openBoundaries.Add(vertices[item3.Key.I1]);
				}
			}
		}

		private static void AddEdge(int i0, int i1, Dictionary<Edge, int> edgeCounts)
		{
			Edge edge = default(Edge);
			edge.I0 = i0;
			edge.I1 = i1;
			Edge key = edge;
			key.GetHashCode();
			if (edgeCounts.ContainsKey(key))
			{
				edgeCounts[key]++;
			}
			else
			{
				edgeCounts[key] = 1;
			}
		}

		public static int GetRandomInt(int maxValue)
		{
			return Instance.Next(maxValue);
		}

		public static int GetRandomInt(int minValue, int maxValue)
		{
			return Instance.Next(Math.Min(minValue, maxValue), Math.Max(minValue, maxValue));
		}

		/// <summary>
		/// Returns a uniformly-distributed random vector from inside of a box (-1,-1,-1), (1, 1, 1)
		/// </summary>
		public static Vector3 GetRandomVector3()
		{
			return new Vector3(GetRandomFloat(-1f, 1f), GetRandomFloat(-1f, 1f), GetRandomFloat(-1f, 1f));
		}

		/// <summary>
		/// Returns a uniformly-distributed random vector from inside of a box (-1,-1,-1), (1, 1, 1)
		/// </summary>
		public static Vector3D GetRandomVector3D()
		{
			return new Vector3D(GetRandomDouble(-1.0, 1.0), GetRandomDouble(-1.0, 1.0), GetRandomDouble(-1.0, 1.0));
		}

		public static Vector3 GetRandomPerpendicularVector(in Vector3 axis)
		{
			Vector3D axis2 = axis;
			return GetRandomPerpendicularVector(ref axis2);
		}

		public static Vector3D GetRandomPerpendicularVector(ref Vector3D axis)
		{
			Vector3D vector = Vector3D.CalculatePerpendicularVector(axis);
			Vector3D.Cross(ref axis, ref vector, out Vector3D result);
			double randomDouble = GetRandomDouble(0.0, 6.2831859588623047);
			return Math.Cos(randomDouble) * vector + Math.Sin(randomDouble) * result;
		}

		public static Vector3D GetRandomDiscPosition(ref Vector3D center, double radius, ref Vector3D tangent, ref Vector3D bitangent)
		{
			double scaleFactor = Math.Sqrt(GetRandomDouble(0.0, 1.0) * radius * radius);
			double randomDouble = GetRandomDouble(0.0, 6.2831859588623047);
			return center + scaleFactor * (Math.Cos(randomDouble) * tangent + Math.Sin(randomDouble) * bitangent);
		}

		public static Vector3D GetRandomDiscPosition(ref Vector3D center, double minRadius, double maxRadius, ref Vector3D tangent, ref Vector3D bitangent)
		{
			double scaleFactor = Math.Sqrt(GetRandomDouble(minRadius * minRadius, maxRadius * maxRadius));
			double randomDouble = GetRandomDouble(0.0, 6.2831859588623047);
			return center + scaleFactor * (Math.Cos(randomDouble) * tangent + Math.Sin(randomDouble) * bitangent);
		}

		public static Vector3 GetRandomBorderPosition(ref BoundingSphere sphere)
		{
			return sphere.Center + GetRandomVector3Normalized() * sphere.Radius;
		}

		public static Vector3D GetRandomBorderPosition(ref BoundingSphereD sphere)
		{
			return sphere.Center + GetRandomVector3Normalized() * (float)sphere.Radius;
		}

		public static Vector3 GetRandomPosition(ref BoundingBox box)
		{
			return box.Center + GetRandomVector3() * box.HalfExtents;
		}

		public static Vector3D GetRandomPosition(ref BoundingBoxD box)
		{
			return box.Center + (Vector3D)GetRandomVector3() * box.HalfExtents;
		}

		public static Vector3 GetRandomBorderPosition(ref BoundingBox box)
		{
			BoundingBoxD box2 = box;
			return GetRandomBorderPosition(ref box2);
		}

		public static Vector3D GetRandomBorderPosition(ref BoundingBoxD box)
		{
			Vector3D size = box.Size;
			double num = 2.0 / box.SurfaceArea;
			double num2 = size.X * size.Y * num;
			double num3 = size.X * size.Z * num;
			double num4 = 1.0 - num2 - num3;
			double num5 = Instance.NextDouble();
			if (num5 < num2)
			{
				if (num5 < num2 * 0.5)
				{
					size.Z = box.Min.Z;
				}
				else
				{
					size.Z = box.Max.Z;
				}
				size.X = GetRandomDouble(box.Min.X, box.Max.X);
				size.Y = GetRandomDouble(box.Min.Y, box.Max.Y);
				return size;
			}
			num5 -= num2;
			if (num5 < num3)
			{
				if (num5 < num3 * 0.5)
				{
					size.Y = box.Min.Y;
				}
				else
				{
					size.Y = box.Max.Y;
				}
				size.X = GetRandomDouble(box.Min.X, box.Max.X);
				size.Z = GetRandomDouble(box.Min.Z, box.Max.Z);
				return size;
			}
			num5 -= num4;
			if (num5 < num4 * 0.5)
			{
				size.X = box.Min.X;
			}
			else
			{
				size.X = box.Max.X;
			}
			size.Y = GetRandomDouble(box.Min.Y, box.Max.Y);
			size.Z = GetRandomDouble(box.Min.Z, box.Max.Z);
			return size;
		}

		public static Vector3 GetRandomVector3Normalized()
		{
			float randomRadian = GetRandomRadian();
			float randomFloat = GetRandomFloat(-1f, 1f);
			float num = (float)Math.Sqrt(1.0 - (double)(randomFloat * randomFloat));
			return new Vector3((double)num * Math.Cos(randomRadian), (double)num * Math.Sin(randomRadian), randomFloat);
		}

		public static Vector3 GetRandomVector3HemisphereNormalized(Vector3 normal)
		{
			Vector3 randomVector3Normalized = GetRandomVector3Normalized();
			if (Vector3.Dot(randomVector3Normalized, normal) < 0f)
			{
				return -randomVector3Normalized;
			}
			return randomVector3Normalized;
		}

		public static Vector3 GetRandomVector3MaxAngle(float maxAngle)
		{
			float randomFloat = GetRandomFloat(0f - maxAngle, maxAngle);
			float randomFloat2 = GetRandomFloat(0f, MathF.PI * 2f);
			return -new Vector3(MyMath.FastSin(randomFloat) * MyMath.FastCos(randomFloat2), MyMath.FastSin(randomFloat) * MyMath.FastSin(randomFloat2), MyMath.FastCos(randomFloat));
		}

		public static Vector3 GetRandomVector3CircleNormalized()
		{
			float randomRadian = GetRandomRadian();
			return new Vector3((float)Math.Sin(randomRadian), 0f, (float)Math.Cos(randomRadian));
		}

		public static float GetRandomSign()
		{
			return Math.Sign((float)Instance.NextDouble() - 0.5f);
		}

		public static float GetRandomFloat()
		{
			return (float)Instance.NextDouble();
		}

		public static float GetRandomFloat(float minValue, float maxValue)
		{
			return MyRandom.Instance.NextFloat() * (maxValue - minValue) + minValue;
		}

		public static double GetRandomDouble(double minValue, double maxValue)
		{
			return Instance.NextDouble() * (maxValue - minValue) + minValue;
		}

		public static float GetRandomRadian()
		{
			return GetRandomFloat(0f, 6.283186f);
		}

		public static long GetRandomLong()
		{
			Instance.NextBytes(m_randomBuffer);
			return BitConverter.ToInt64(m_randomBuffer, 0);
		}

		/// <summary>
		/// Returns a random TimeSpan between begin (inclusive) and end (exclusive).
		/// </summary>
		public static TimeSpan GetRandomTimeSpan(TimeSpan begin, TimeSpan end)
		{
			long randomLong = GetRandomLong();
			return new TimeSpan(begin.Ticks + randomLong % (end.Ticks - begin.Ticks));
		}

		/// <summary>
		/// Returns intersection point between sphere and its edges. But only if there is intersection between sphere and one of the edges.
		/// If sphere intersects somewhere inside the triangle, this method will not detect it.
		/// </summary>
		public static Vector3? GetEdgeSphereCollision(ref Vector3 sphereCenter, float sphereRadius, ref MyTriangle_Vertices triangle)
		{
			Vector3 closestPointOnLine = GetClosestPointOnLine(ref triangle.Vertex0, ref triangle.Vertex1, ref sphereCenter);
			if (Vector3.Distance(closestPointOnLine, sphereCenter) < sphereRadius)
			{
				return closestPointOnLine;
			}
			closestPointOnLine = GetClosestPointOnLine(ref triangle.Vertex1, ref triangle.Vertex2, ref sphereCenter);
			if (Vector3.Distance(closestPointOnLine, sphereCenter) < sphereRadius)
			{
				return closestPointOnLine;
			}
			closestPointOnLine = GetClosestPointOnLine(ref triangle.Vertex2, ref triangle.Vertex0, ref sphereCenter);
			if (Vector3.Distance(closestPointOnLine, sphereCenter) < sphereRadius)
			{
				return closestPointOnLine;
			}
			return null;
		}

		/// <summary>
		/// Return true if point is inside the triangle.
		/// </summary>
		public static bool GetInsidePolygonForSphereCollision(ref Vector3D point, ref MyTriangle_Vertices triangle)
		{
			if ((double)(0f + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex0 - (Vector3)point, triangle.Vertex1 - (Vector3)point) + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex1 - (Vector3)point, triangle.Vertex2 - (Vector3)point) + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex2 - (Vector3)point, triangle.Vertex0 - (Vector3)point)) >= 6.2203541591948124)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Return true if point is inside the triangle.
		/// </summary>
		public static bool GetInsidePolygonForSphereCollision(ref Vector3 point, ref MyTriangle_Vertices triangle)
		{
			if ((double)(0f + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex0 - point, triangle.Vertex1 - point) + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex1 - point, triangle.Vertex2 - point) + GetAngleBetweenVectorsForSphereCollision(triangle.Vertex2 - point, triangle.Vertex0 - point)) >= 6.2203541591948124)
			{
				return true;
			}
			return false;
		}

		public static float GetAngleBetweenVectorsForSphereCollision(Vector3 vector1, Vector3 vector2)
		{
			float num = Vector3.Dot(vector1, vector2);
			float num2 = vector1.Length() * vector2.Length();
			float num3 = (float)Math.Acos(num / num2);
			if (float.IsNaN(num3))
			{
				return 0f;
			}
			return num3;
		}

		/// <summary>
		/// Checks whether a ray intersects a triangleVertexes. This uses the algorithm
		/// developed by Tomas Moller and Ben Trumbore, which was published in the
		/// Journal of Graphics Tools, pitch 2, "Fast, Minimum Storage Ray-Triangle
		/// Intersection".
		///
		/// This method is implemented using the pass-by-reference versions of the
		/// XNA math functions. Using these overloads is generally not recommended,
		/// because they make the code less readable than the normal pass-by-value
		/// versions. This method can be called very frequently in a tight inner loop,
		/// however, so in this particular case the performance benefits from passing
		/// everything by reference outweigh the loss of readability.
		/// </summary>
		public static float? GetLineTriangleIntersection(ref Line line, ref MyTriangle_Vertices triangle)
		{
			Vector3.Subtract(ref triangle.Vertex1, ref triangle.Vertex0, out Vector3 result);
			Vector3.Subtract(ref triangle.Vertex2, ref triangle.Vertex0, out Vector3 result2);
			Vector3.Cross(ref line.Direction, ref result2, out Vector3 result3);
			Vector3.Dot(ref result, ref result3, out float result4);
			if (result4 > -1.401298E-45f && result4 < float.Epsilon)
			{
				return null;
			}
			float num = 1f / result4;
			Vector3.Subtract(ref line.From, ref triangle.Vertex0, out Vector3 result5);
			Vector3.Dot(ref result5, ref result3, out float result6);
			result6 *= num;
			if (result6 < 0f || result6 > 1f)
			{
				return null;
			}
			Vector3.Cross(ref result5, ref result, out Vector3 result7);
			Vector3.Dot(ref line.Direction, ref result7, out float result8);
			result8 *= num;
			if (result8 < 0f || result6 + result8 > 1f)
			{
				return null;
			}
			Vector3.Dot(ref result2, ref result7, out float result9);
			result9 *= num;
			if (result9 < 0f)
			{
				return null;
			}
			if (result9 > line.Length)
			{
				return null;
			}
			return result9;
		}

		public static Vector3 GetNormalVectorFromTriangle(ref MyTriangle_Vertices inputTriangle)
		{
			return Vector3.Normalize(Vector3.Cross(inputTriangle.Vertex2 - inputTriangle.Vertex0, inputTriangle.Vertex1 - inputTriangle.Vertex0));
		}

		/// <summary>
		/// Method returns intersection point between sphere and triangle (which is defined by vertexes and plane).
		/// If no intersection found, method returns null.
		/// See below how intersection point can be calculated, because it's not so easy - for example sphere vs. triangle will 
		/// hardly generate just intersection point... more like intersection area or something.
		/// </summary>
		public static Vector3? GetSphereTriangleIntersection(ref BoundingSphere sphere, ref Plane trianglePlane, ref MyTriangle_Vertices triangle)
		{
			if (GetSpherePlaneIntersection(ref sphere, ref trianglePlane, out float distanceFromPlaneToSphere) == MySpherePlaneIntersectionEnum.INTERSECTS)
			{
				Vector3 vector = trianglePlane.Normal * distanceFromPlaneToSphere;
				Vector3 point = default(Vector3);
				point.X = sphere.Center.X - vector.X;
				point.Y = sphere.Center.Y - vector.Y;
				point.Z = sphere.Center.Z - vector.Z;
				if (GetInsidePolygonForSphereCollision(ref point, ref triangle))
				{
					return point;
				}
				Vector3? edgeSphereCollision = GetEdgeSphereCollision(ref sphere.Center, sphere.Radius / 1f, ref triangle);
				if (edgeSphereCollision.HasValue)
				{
					return edgeSphereCollision.Value;
				}
			}
			return null;
		}

		public static string AlignIntToRight(int value, int charsCount, char ch)
		{
			string text = value.ToString();
			int length = text.Length;
			if (length > charsCount)
			{
				return text;
			}
			return new string(ch, charsCount - length) + text;
		}

		public static bool TryParseWithSuffix(this string text, NumberStyles numberStyle, IFormatProvider formatProvider, out float value, Tuple<string, float>[] suffix = null)
		{
			Tuple<string, float>[] array = suffix ?? DefaultNumberSuffix;
			foreach (Tuple<string, float> tuple in array)
			{
				if (text.EndsWith(tuple.Item1, StringComparison.InvariantCultureIgnoreCase))
				{
					bool result = float.TryParse(text.Substring(0, text.Length - tuple.Item1.Length), numberStyle, formatProvider, out value);
					value *= tuple.Item2;
					return result;
				}
			}
			return float.TryParse(text, out value);
		}

		/// <summary>
		/// Aligns rectangle, works in screen/texture/pixel coordinates, not normalized coordinates.
		/// </summary>
		/// <returns>Pixel coordinates for texture.</returns>
		public static Vector2 GetCoordAligned(Vector2 coordScreen, Vector2 size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return coordScreen;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return coordScreen - size * 0.5f;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return coordScreen - size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return coordScreen - size * new Vector2(0.5f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return coordScreen - size;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return coordScreen - size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return coordScreen - size * new Vector2(1f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return coordScreen - size * new Vector2(0f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return coordScreen - size * new Vector2(1f, 0f);
			default:
				throw new InvalidBranchException();
			}
		}

		public static Vector2 AlignCoord(Vector2 coordScreen, Vector2 size, MyGuiDrawAlignEnum drawAlignEnum)
		{
			switch (drawAlignEnum)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return coordScreen;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return coordScreen + size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return coordScreen + size * new Vector2(0f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return coordScreen + size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return coordScreen + size * new Vector2(0.5f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return coordScreen + size * new Vector2(0.5f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return coordScreen + size * new Vector2(1f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return coordScreen + size * new Vector2(1f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return coordScreen + size * new Vector2(1f, 1f);
			default:
				throw new ArgumentOutOfRangeException("drawAlignEnum", drawAlignEnum, null);
			}
		}

		/// <summary>
		/// Modifies input coordinate (in center) using alignment and
		/// size of the rectangle. Result is at position inside rectangle
		/// specified by alignment.
		/// </summary>
		public static Vector2 GetCoordAlignedFromCenter(Vector2 coordCenter, Vector2 size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return coordCenter + size * new Vector2(-0.5f, -0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return coordCenter + size * new Vector2(-0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return coordCenter + size * new Vector2(-0.5f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return coordCenter + size * new Vector2(0f, -0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return coordCenter;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return coordCenter + size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return coordCenter + size * new Vector2(0.5f, -0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return coordCenter + size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return coordCenter + size * new Vector2(0.5f, 0.5f);
			default:
				throw new InvalidBranchException();
			}
		}

		public static Vector2 GetCoordAlignedFromTopLeft(Vector2 topLeft, Vector2 size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return topLeft;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return topLeft + size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return topLeft + size * new Vector2(0f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return topLeft + size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return topLeft + size * new Vector2(0.5f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return topLeft + size * new Vector2(0.5f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return topLeft + size * new Vector2(1f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return topLeft + size * new Vector2(1f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return topLeft + size * new Vector2(1f, 1f);
			default:
				return topLeft;
			}
		}

		/// <summary>
		/// Reverses effect of alignment to compute top-left corner coordinate.
		/// </summary>
		public static Vector2 GetCoordTopLeftFromAligned(Vector2 alignedCoord, Vector2 size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return alignedCoord;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return alignedCoord - size * 0.5f;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return alignedCoord - size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return alignedCoord - size * new Vector2(0.5f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return alignedCoord - size;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return alignedCoord - size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return alignedCoord - size * new Vector2(1f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return alignedCoord - size * new Vector2(0f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return alignedCoord - size * new Vector2(1f, 0f);
			default:
				throw new InvalidBranchException();
			}
		}

		/// <summary>
		/// Reverses effect of alignment to compute top-left corner coordinate.
		/// </summary>
		public static Vector2I GetCoordTopLeftFromAligned(Vector2I alignedCoord, Vector2I size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return alignedCoord;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return new Vector2I(alignedCoord.X, alignedCoord.Y - size.Y / 2);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return new Vector2I(alignedCoord.X, alignedCoord.Y - size.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y - size.Y / 2);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y - size.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y - size.Y / 2);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y - size.Y);
			default:
				throw new InvalidBranchException();
			}
		}

		/// <summary>
		/// Reverses effect of alignment to compute center coordinate.
		/// </summary>
		public static Vector2 GetCoordCenterFromAligned(Vector2 alignedCoord, Vector2 size, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return alignedCoord + size * 0.5f;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return alignedCoord;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return alignedCoord + size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return alignedCoord - size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return alignedCoord - size * 0.5f;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return alignedCoord + size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return alignedCoord - size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return alignedCoord + size * new Vector2(0.5f, -0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return alignedCoord + size * new Vector2(-0.5f, 0.5f);
			default:
				throw new InvalidBranchException();
			}
		}

		/// <summary>
		/// Returns coordinate within given rectangle specified by draw align. Rectangle position should be
		/// upper left corner. Conversion assumes that Y coordinates increase downwards.
		/// </summary>
		public static Vector2 GetCoordAlignedFromRectangle(ref RectangleF rect, MyGuiDrawAlignEnum drawAlign)
		{
			switch (drawAlign)
			{
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
				return rect.Position;
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				return rect.Position + rect.Size * new Vector2(0f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				return rect.Position + rect.Size * new Vector2(0f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				return rect.Position + rect.Size * new Vector2(0.5f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				return rect.Position + rect.Size * 0.5f;
			case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				return rect.Position + rect.Size * new Vector2(0.5f, 1f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				return rect.Position + rect.Size * new Vector2(1f, 0f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				return rect.Position + rect.Size * new Vector2(1f, 0.5f);
			case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
				return rect.Position + rect.Size * 1f;
			default:
				throw new InvalidBranchException();
			}
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Matrix matrix)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(MatrixD matrix)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3 vec)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3D vec)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector3? vec)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Vector2 vec)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(float f)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(double f)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValid(Quaternion q)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertIsValidOrZero(Matrix matrix)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertLengthValid(ref Vector3 vec)
		{
		}

		[Conditional("DEBUG")]
		public static void AssertLengthValid(ref Vector3D vec)
		{
		}

		public static bool HasValidLength(Vector3 vec)
		{
			return vec.Length() > 1E-06f;
		}

		public static bool HasValidLength(Vector3D vec)
		{
			return vec.Length() > 9.9999999747524271E-07;
		}

		public static bool IsEqual(float value1, float value2)
		{
			return IsZero(value1 - value2);
		}

		public static bool IsEqual(Vector2 value1, Vector2 value2)
		{
			if (IsZero(value1.X - value2.X))
			{
				return IsZero(value1.Y - value2.Y);
			}
			return false;
		}

		public static bool IsEqual(Vector3 value1, Vector3 value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y))
			{
				return IsZero(value1.Z - value2.Z);
			}
			return false;
		}

		public static bool IsEqual(Quaternion value1, Quaternion value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y) && IsZero(value1.Z - value2.Z))
			{
				return IsZero(value1.W - value2.W);
			}
			return false;
		}

		public static bool IsEqual(QuaternionD value1, QuaternionD value2)
		{
			if (IsZero(value1.X - value2.X) && IsZero(value1.Y - value2.Y) && IsZero(value1.Z - value2.Z))
			{
				return IsZero(value1.W - value2.W);
			}
			return false;
		}

		public static bool IsEqual(Matrix value1, Matrix value2)
		{
			if (IsZero(value1.Left - value2.Left) && IsZero(value1.Up - value2.Up) && IsZero(value1.Forward - value2.Forward))
			{
				return IsZero(value1.Translation - value2.Translation);
			}
			return false;
		}

		public static bool IsValid(Matrix matrix)
		{
			if (matrix.Up.IsValid() && matrix.Left.IsValid() && matrix.Forward.IsValid() && matrix.Translation.IsValid())
			{
				return matrix != Matrix.Zero;
			}
			return false;
		}

		public static bool IsValid(MatrixD matrix)
		{
			if (matrix.Up.IsValid() && matrix.Left.IsValid() && matrix.Forward.IsValid() && matrix.Translation.IsValid())
			{
				return matrix != MatrixD.Zero;
			}
			return false;
		}

		public static bool IsValid(Vector3 vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector3D vec)
		{
			if (IsValid(vec.X) && IsValid(vec.Y))
			{
				return IsValid(vec.Z);
			}
			return false;
		}

		public static bool IsValid(Vector2 vec)
		{
			if (IsValid(vec.X))
			{
				return IsValid(vec.Y);
			}
			return false;
		}

		public static bool IsValid(float f)
		{
			if (!float.IsNaN(f))
			{
				return !float.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(double f)
		{
			if (!double.IsNaN(f))
			{
				return !double.IsInfinity(f);
			}
			return false;
		}

		public static bool IsValid(Vector3? vec)
		{
			if (vec.HasValue)
			{
				if (IsValid(vec.Value.X) && IsValid(vec.Value.Y))
				{
					return IsValid(vec.Value.Z);
				}
				return false;
			}
			return true;
		}

		public static bool IsValid(Quaternion q)
		{
			if (IsValid(q.X) && IsValid(q.Y) && IsValid(q.Z) && IsValid(q.W))
			{
				return !IsZero(q);
			}
			return false;
		}

		public static bool IsValidNormal(Vector3 vec)
		{
			float num = vec.LengthSquared();
			if (vec.IsValid() && num > 0.999f)
			{
				return num < 1.001f;
			}
			return false;
		}

		public static bool IsValidOrZero(Matrix matrix)
		{
			if (IsValid(matrix.Up) && IsValid(matrix.Left) && IsValid(matrix.Forward))
			{
				return IsValid(matrix.Translation);
			}
			return false;
		}

		public static bool IsZero(float value, float epsilon = 1E-05f)
		{
			if (value > 0f - epsilon)
			{
				return value < epsilon;
			}
			return false;
		}

		public static bool IsZero(double value, float epsilon = 1E-05f)
		{
			if (value > (double)(0f - epsilon))
			{
				return value < (double)epsilon;
			}
			return false;
		}

		public static bool IsZero(Vector3 value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(ref Vector3 value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(Vector3D value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(ref Vector3D value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon))
			{
				return IsZero(value.Z, epsilon);
			}
			return false;
		}

		public static bool IsZero(Quaternion value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon) && IsZero(value.Z, epsilon))
			{
				return IsZero(value.W, epsilon);
			}
			return false;
		}

		public static bool IsZero(ref Quaternion value, float epsilon = 1E-05f)
		{
			if (IsZero(value.X, epsilon) && IsZero(value.Y, epsilon) && IsZero(value.Z, epsilon))
			{
				return IsZero(value.W, epsilon);
			}
			return false;
		}

		public static bool IsZero(Vector4 value)
		{
			if (IsZero(value.X) && IsZero(value.Y) && IsZero(value.Z))
			{
				return IsZero(value.W);
			}
			return false;
		}

		public static void CheckFloatValues(object graph, string name, ref double? min, ref double? max)
		{
			_ = new StackTrace().FrameCount;
			_ = 1000;
			if (graph == null)
			{
				return;
			}
			if (graph is float)
			{
				float num = (float)graph;
				if (float.IsInfinity(num) || float.IsNaN(num))
				{
					throw new InvalidOperationException("Invalid value: " + name);
				}
				if (!min.HasValue || (double)num < min)
				{
					min = num;
				}
				if (!max.HasValue || (double)num > max)
				{
					max = num;
				}
			}
			if (graph is double)
			{
				double num2 = (double)graph;
				if (double.IsInfinity(num2) || double.IsNaN(num2))
				{
					throw new InvalidOperationException("Invalid value: " + name);
				}
				if (!min.HasValue || num2 < min)
				{
					min = num2;
				}
				if (!max.HasValue || num2 > max)
				{
					max = num2;
				}
			}
			if (graph.GetType().IsPrimitive || graph is string || graph is DateTime)
			{
				return;
			}
			if (graph is IEnumerable)
			{
				foreach (object item in graph as IEnumerable)
				{
					CheckFloatValues(item, name + "[]", ref min, ref max);
				}
				return;
			}
			FieldInfo[] fields = graph.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				CheckFloatValues(fieldInfo.GetValue(graph), name + "." + fieldInfo.Name, ref min, ref max);
			}
			PropertyInfo[] properties = graph.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				CheckFloatValues(propertyInfo.GetValue(graph, null), name + "." + propertyInfo.Name, ref min, ref max);
			}
		}

		public static void DeserializeValue(XmlReader reader, out Vector3 value)
		{
			Vector3 vector = default(Vector3);
			vector.X = reader.ReadElementContentAsFloat();
			vector.Y = reader.ReadElementContentAsFloat();
			vector.Z = reader.ReadElementContentAsFloat();
			value = vector;
		}

		public static void DeserializeValue(XmlReader reader, out Vector4 value)
		{
			Vector4 vector = default(Vector4);
			vector.W = reader.ReadElementContentAsFloat();
			vector.X = reader.ReadElementContentAsFloat();
			vector.Y = reader.ReadElementContentAsFloat();
			vector.Z = reader.ReadElementContentAsFloat();
			value = vector;
		}

		public static string FormatByteSizePrefix(ref double byteSize)
		{
			long num = 1L;
			for (int i = 0; i < BYTE_SIZE_PREFIX.Length; i++)
			{
				num *= 1024;
				if (byteSize < (double)num)
				{
					byteSize /= num / 1024;
					return BYTE_SIZE_PREFIX[i];
				}
			}
			return string.Empty;
		}

		public static Color[] GenerateBoxColors()
		{
			List<Color> list = new List<Color>();
			for (float num = 0f; num < 1f; num += 0.2f)
			{
				for (float num2 = 0f; num2 < 1f; num2 += 0.33f)
				{
					for (float num3 = 0f; num3 < 1f; num3 += 0.33f)
					{
						float x = MathHelper.Lerp(0.5f, 7f / 12f, num);
						float y = MathHelper.Lerp(0.4f, 0.9f, num2);
						float z = MathHelper.Lerp(0.4f, 1f, num3);
						list.Add(new Vector3(x, y, z).HSVtoColor());
					}
				}
			}
			list.ShuffleList();
			return list.ToArray();
		}

		/// <summary>
		/// Generate oriented quad by matrix
		/// </summary>
		public static void GenerateQuad(out MyQuadD quad, ref Vector3D position, float width, float height, ref MatrixD matrix)
		{
			Vector3D value = matrix.Left * width;
			Vector3D value2 = matrix.Up * height;
			quad.Point0 = position + value + value2;
			quad.Point1 = position + value - value2;
			quad.Point2 = position - value - value2;
			quad.Point3 = position - value + value2;
		}

		/// <summary>
		/// Calculating the Angle between two Vectors (return in radians).
		/// </summary>
		public static float GetAngleBetweenVectors(Vector3 vectorA, Vector3 vectorB)
		{
			float num = Vector3.Dot(vectorA, vectorB);
			if (num > 1f && num <= 1.0001f)
			{
				num = 1f;
			}
			if (num < -1f && num >= -1.0001f)
			{
				num = -1f;
			}
			return (float)Math.Acos(num);
		}

		public static float GetAngleBetweenVectorsAndNormalise(Vector3 vectorA, Vector3 vectorB)
		{
			return GetAngleBetweenVectors(Vector3.Normalize(vectorA), Vector3.Normalize(vectorB));
		}

		public static bool GetBillboardQuadAdvancedRotated(out MyQuadD quad, Vector3D position, float radiusX, float radiusY, float angle, Vector3D cameraPosition)
		{
			Vector3D direction = default(Vector3D);
			direction.X = position.X - cameraPosition.X;
			direction.Y = position.Y - cameraPosition.Y;
			direction.Z = position.Z - cameraPosition.Z;
			if (direction.LengthSquared() <= 9.9999997473787516E-06)
			{
				quad = default(MyQuadD);
				return false;
			}
			direction = Normalize(direction);
			Vector3D.Reject(ref Vector3D.Up, ref direction, out Vector3D result);
			Vector3D normalized;
			if (result.LengthSquared() <= 9.9999994396249292E-11)
			{
				normalized = Vector3D.Forward;
			}
			else
			{
				Normalize(ref result, out normalized);
			}
			Vector3D.Cross(ref normalized, ref direction, out Vector3D result2);
			result2 = Normalize(result2);
			float num = (float)Math.Cos(angle);
			float num2 = (float)Math.Sin(angle);
			Vector3D vector3D = default(Vector3D);
			vector3D.X = (double)(radiusX * num) * result2.X + (double)(radiusY * num2) * normalized.X;
			vector3D.Y = (double)(radiusX * num) * result2.Y + (double)(radiusY * num2) * normalized.Y;
			vector3D.Z = (double)(radiusX * num) * result2.Z + (double)(radiusY * num2) * normalized.Z;
			Vector3D vector3D2 = default(Vector3D);
			vector3D2.X = (double)((0f - radiusX) * num2) * result2.X + (double)(radiusY * num) * normalized.X;
			vector3D2.Y = (double)((0f - radiusX) * num2) * result2.Y + (double)(radiusY * num) * normalized.Y;
			vector3D2.Z = (double)((0f - radiusX) * num2) * result2.Z + (double)(radiusY * num) * normalized.Z;
			quad.Point0.X = position.X + vector3D.X + vector3D2.X;
			quad.Point0.Y = position.Y + vector3D.Y + vector3D2.Y;
			quad.Point0.Z = position.Z + vector3D.Z + vector3D2.Z;
			quad.Point1.X = position.X - vector3D.X + vector3D2.X;
			quad.Point1.Y = position.Y - vector3D.Y + vector3D2.Y;
			quad.Point1.Z = position.Z - vector3D.Z + vector3D2.Z;
			quad.Point2.X = position.X - vector3D.X - vector3D2.X;
			quad.Point2.Y = position.Y - vector3D.Y - vector3D2.Y;
			quad.Point2.Z = position.Z - vector3D.Z - vector3D2.Z;
			quad.Point3.X = position.X + vector3D.X - vector3D2.X;
			quad.Point3.Y = position.Y + vector3D.Y - vector3D2.Y;
			quad.Point3.Z = position.Z + vector3D.Z - vector3D2.Z;
			return true;
		}

		public static bool GetBillboardQuadAdvancedRotated(out MyQuadD quad, Vector3D position, float radius, float angle, Vector3D cameraPosition)
		{
			return GetBillboardQuadAdvancedRotated(out quad, position, radius, radius, angle, cameraPosition);
		}

		/// <summary>
		/// This billboard isn't facing the camera. It's always oriented in specified direction. May be used as thrusts, or inner light of reflector.
		/// </summary>
		public static void GetBillboardQuadOriented(out MyQuadD quad, ref Vector3D position, float width, float height, ref Vector3 leftVector, ref Vector3 upVector)
		{
			Vector3D value = leftVector * width;
			Vector3D value2 = upVector * height;
			quad.Point0 = position + value2 + value;
			quad.Point1 = position + value2 - value;
			quad.Point2 = position - value2 - value;
			quad.Point3 = position - value2 + value;
		}

		public static bool? GetBoolFromString(string s)
		{
			if (!bool.TryParse(s, out bool result))
			{
				return null;
			}
			return result;
		}

		public static bool GetBoolFromString(string s, bool defaultValue)
		{
			return GetBoolFromString(s) ?? defaultValue;
		}

		public static BoundingSphereD GetBoundingSphereFromBoundingBox(ref BoundingBoxD box)
		{
			BoundingSphereD result = default(BoundingSphereD);
			result.Center = (box.Max + box.Min) / 2.0;
			result.Radius = Vector3D.Distance(result.Center, box.Max);
			return result;
		}

		public static byte? GetByteFromString(string s)
		{
			if (!byte.TryParse(s, out byte result))
			{
				return null;
			}
			return result;
		}

		public static Vector3 GetCartesianCoordinatesFromSpherical(float angleHorizontal, float angleVertical, float radius)
		{
			angleVertical = MathF.E * 449f / 777f - angleVertical;
			angleHorizontal = 3.141593f - angleHorizontal;
			return new Vector3((float)((double)radius * Math.Sin(angleVertical) * Math.Sin(angleHorizontal)), (float)((double)radius * Math.Cos(angleVertical)), (float)((double)radius * Math.Sin(angleVertical) * Math.Cos(angleHorizontal)));
		}

		public static int GetClampInt(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static Vector3 GetClosestPointOnLine(ref Vector3 linePointA, ref Vector3 linePointB, ref Vector3 point)
		{
			float dist = 0f;
			return GetClosestPointOnLine(ref linePointA, ref linePointB, ref point, out dist);
		}

		public static Vector3D GetClosestPointOnLine(ref Vector3D linePointA, ref Vector3D linePointB, ref Vector3D point)
		{
			double dist = 0.0;
			return GetClosestPointOnLine(ref linePointA, ref linePointB, ref point, out dist);
		}

		public static Vector3 GetClosestPointOnLine(ref Vector3 linePointA, ref Vector3 linePointB, ref Vector3 point, out float dist)
		{
			Vector3 vector = point - linePointA;
			Vector3 vector2 = Normalize(linePointB - linePointA);
			float num = Vector3.Distance(linePointA, linePointB);
			float num2 = dist = Vector3.Dot(vector2, vector);
			if (num2 <= 0f)
			{
				return linePointA;
			}
			if (num2 >= num)
			{
				return linePointB;
			}
			Vector3 value = vector2 * num2;
			return linePointA + value;
		}

		public static Vector3D GetClosestPointOnLine(ref Vector3D linePointA, ref Vector3D linePointB, ref Vector3D point, out double dist)
		{
			Vector3D vector = point - linePointA;
			Vector3D vector3D = Normalize(linePointB - linePointA);
			double num = Vector3D.Distance(linePointA, linePointB);
			double num2 = dist = Vector3D.Dot(vector3D, vector);
			if (num2 <= 0.0)
			{
				return linePointA;
			}
			if (num2 >= num)
			{
				return linePointB;
			}
			Vector3D value = vector3D * num2;
			return linePointA + value;
		}

		public static float? GetFloatFromString(string s)
		{
			if (!float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out float result))
			{
				return null;
			}
			return result;
		}

		public static float GetFloatFromString(string s, float defaultValue)
		{
			float? floatFromString = GetFloatFromString(s);
			if (floatFromString.HasValue)
			{
				return floatFromString.Value;
			}
			return defaultValue;
		}

		public static int? GetInt32FromString(string s)
		{
			if (int.TryParse(s, out int result))
			{
				return result;
			}
			return null;
		}

		public static int? GetIntFromString(string s)
		{
			if (!int.TryParse(s, out int result))
			{
				return null;
			}
			return result;
		}

		public static int GetIntFromString(string s, int defaultValue)
		{
			int? intFromString = GetIntFromString(s);
			if (intFromString.HasValue)
			{
				return intFromString.Value;
			}
			return defaultValue;
		}

		/// <summary>
		/// Distance between "from" and opposite side of the "sphere". Always positive.
		/// </summary>
		public static double GetLargestDistanceToSphere(ref Vector3D from, ref BoundingSphereD sphere)
		{
			return Vector3D.Distance(from, sphere.Center) + sphere.Radius;
		}

		/// <summary>
		/// Calculates intersection between line and bounding box and if found, distance is returned. Otherwise null is returned.
		/// </summary>
		public static float? GetLineBoundingBoxIntersection(ref Line line, ref BoundingBox boundingBox)
		{
			Ray ray = new Ray(line.From, line.Direction);
			float? num = boundingBox.Intersects(ray);
			if (!num.HasValue)
			{
				return null;
			}
			if (num.Value <= line.Length)
			{
				return num.Value;
			}
			return null;
		}

		public static int GetMaxValueFromEnum<T>()
		{
			Array values = Enum.GetValues(typeof(T));
			int num = int.MinValue;
			Type underlyingType = Enum.GetUnderlyingType(typeof(T));
			if (underlyingType == typeof(byte))
			{
				foreach (byte item in values)
				{
					if (item > num)
					{
						num = item;
					}
				}
				return num;
			}
			if (underlyingType == typeof(short))
			{
				foreach (short item2 in values)
				{
					if (item2 > num)
					{
						num = item2;
					}
				}
				return num;
			}
			if (underlyingType == typeof(ushort))
			{
				foreach (ushort item3 in values)
				{
					if (item3 > num)
					{
						num = item3;
					}
				}
				return num;
			}
			if (underlyingType == typeof(int))
			{
				foreach (int item4 in values)
				{
					if (item4 > num)
					{
						num = item4;
					}
				}
				return num;
			}
			throw new InvalidBranchException();
		}

		public static double GetPointLineDistance(ref Vector3D linePointA, ref Vector3D linePointB, ref Vector3D point)
		{
			Vector3D vector = linePointB - linePointA;
			return Vector3D.Cross(vector, point - linePointA).Length() / vector.Length();
		}

		public static void GetPolyLineQuad(out MyQuadD retQuad, ref MyPolyLineD polyLine, Vector3D cameraPosition)
		{
			Vector3D vector = Normalize(cameraPosition - polyLine.Point0);
			Vector3D vector3Scaled = GetVector3Scaled(Vector3D.Cross(polyLine.LineDirectionNormalized, vector), polyLine.Thickness);
			retQuad.Point0 = polyLine.Point0 - vector3Scaled;
			retQuad.Point1 = polyLine.Point1 - vector3Scaled;
			retQuad.Point2 = polyLine.Point1 + vector3Scaled;
			retQuad.Point3 = polyLine.Point0 + vector3Scaled;
		}

		public static T GetRandomItem<T>(this T[] list)
		{
			return list[GetRandomInt(list.Length)];
		}

		public static T GetRandomItemFromList<T>(this List<T> list)
		{
			return list[GetRandomInt(list.Count)];
		}

		/// <summary>
		/// Calculates distance from point 'from' to boundary of 'sphere'. If point is inside the sphere, distance will be negative.
		/// </summary>
		public static double GetSmallestDistanceToSphere(ref Vector3D from, ref BoundingSphereD sphere)
		{
			return Vector3D.Distance(from, sphere.Center) - sphere.Radius;
		}

		public static double GetSmallestDistanceToSphereAlwaysPositive(ref Vector3D from, ref BoundingSphereD sphere)
		{
			double num = GetSmallestDistanceToSphere(ref from, ref sphere);
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}

		/// <summary>
		/// This tells if a sphere is BEHIND, in FRONT, or INTERSECTS a plane, also it's distance
		/// </summary>
		public static MySpherePlaneIntersectionEnum GetSpherePlaneIntersection(ref BoundingSphereD sphere, ref PlaneD plane, out double distanceFromPlaneToSphere)
		{
			double d = plane.D;
			distanceFromPlaneToSphere = plane.Normal.X * sphere.Center.X + plane.Normal.Y * sphere.Center.Y + plane.Normal.Z * sphere.Center.Z + d;
			if (Math.Abs(distanceFromPlaneToSphere) < sphere.Radius)
			{
				return MySpherePlaneIntersectionEnum.INTERSECTS;
			}
			if (distanceFromPlaneToSphere >= sphere.Radius)
			{
				return MySpherePlaneIntersectionEnum.FRONT;
			}
			return MySpherePlaneIntersectionEnum.BEHIND;
		}

		/// <summary>
		/// This tells if a sphere is BEHIND, in FRONT, or INTERSECTS a plane, also it's distance
		/// </summary>
		public static MySpherePlaneIntersectionEnum GetSpherePlaneIntersection(ref BoundingSphere sphere, ref Plane plane, out float distanceFromPlaneToSphere)
		{
			float d = plane.D;
			distanceFromPlaneToSphere = plane.Normal.X * sphere.Center.X + plane.Normal.Y * sphere.Center.Y + plane.Normal.Z * sphere.Center.Z + d;
			if (Math.Abs(distanceFromPlaneToSphere) < sphere.Radius)
			{
				return MySpherePlaneIntersectionEnum.INTERSECTS;
			}
			if (distanceFromPlaneToSphere >= sphere.Radius)
			{
				return MySpherePlaneIntersectionEnum.FRONT;
			}
			return MySpherePlaneIntersectionEnum.BEHIND;
		}

		public static Vector3D GetTransformNormalNormalized(Vector3D vec, ref MatrixD matrix)
		{
			Vector3D.TransformNormal(ref vec, ref matrix, out Vector3D result);
			return Normalize(result);
		}

		public static Vector3D GetVector3Scaled(Vector3D originalVector, float newLength)
		{
			if (newLength == 0f)
			{
				return Vector3D.Zero;
			}
			double num = originalVector.Length();
			if (num == 0.0)
			{
				return Vector3D.Zero;
			}
			double num2 = (double)newLength / num;
			return new Vector3D(originalVector.X * num2, originalVector.Y * num2, originalVector.Z * num2);
		}

		/// <summary>
		/// Check intersection between line and bounding sphere
		/// We don't use BoundingSphere.Contains(Ray ...) because ray doesn't have an end, but line does, so we need
		/// to check if line really intersects the sphere.
		/// </summary>
		public static bool IsLineIntersectingBoundingSphere(ref LineD line, ref BoundingSphereD boundingSphere)
		{
			RayD ray = new RayD(ref line.From, ref line.Direction);
			double? num = boundingSphere.Intersects(ray);
			if (!num.HasValue)
			{
				return false;
			}
			if (num.Value <= line.Length)
			{
				return true;
			}
			return false;
		}

		public static bool IsWrongTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
		{
			if ((vertex2 - vertex0).LengthSquared() <= 9.99999944E-11f)
			{
				return true;
			}
			if ((vertex1 - vertex0).LengthSquared() <= 9.99999944E-11f)
			{
				return true;
			}
			if ((vertex1 - vertex2).LengthSquared() <= 9.99999944E-11f)
			{
				return true;
			}
			return false;
		}

		public static Vector3D LinePlaneIntersection(Vector3D planePoint, Vector3 planeNormal, Vector3D lineStart, Vector3 lineDir)
		{
			double num = Vector3D.Dot(planePoint - lineStart, planeNormal);
			float num2 = Vector3.Dot(lineDir, planeNormal);
			return lineStart + (Vector3D)lineDir * (num / (double)num2);
		}

		/// <summary>
		/// Protected normalize with assert
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector3 Normalize(Vector3 vec)
		{
			return Vector3.Normalize(vec);
		}

		public static Vector3D Normalize(Vector3D vec)
		{
			return Vector3D.Normalize(vec);
		}

		/// <summary>
		/// Protected normalize with assert
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void Normalize(ref Vector3 vec, out Vector3 normalized)
		{
			Vector3.Normalize(ref vec, out normalized);
		}

		public static void Normalize(ref Vector3D vec, out Vector3D normalized)
		{
			Vector3D.Normalize(ref vec, out normalized);
		}

		/// <summary>
		/// Protected normalize with assert
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static void Normalize(ref Matrix m, out Matrix normalized)
		{
			normalized = Matrix.CreateWorld(m.Translation, Normalize(m.Forward), Normalize(m.Up));
		}

		public static void Normalize(ref MatrixD m, out MatrixD normalized)
		{
			normalized = MatrixD.CreateWorld(m.Translation, Normalize(m.Forward), Normalize(m.Up));
		}

		public static void RotationMatrixToYawPitchRoll(ref Matrix mx, out float yaw, out float pitch, out float roll)
		{
			float num = mx.M32;
			if (num > 1f)
			{
				num = 1f;
			}
			else if (num < -1f)
			{
				num = -1f;
			}
			pitch = (float)Math.Asin(0f - num);
			float num2 = 0.001f;
			if ((float)Math.Cos(pitch) > num2)
			{
				roll = (float)Math.Atan2(mx.M12, mx.M22);
				yaw = (float)Math.Atan2(mx.M31, mx.M33);
			}
			else
			{
				roll = (float)Math.Atan2(0f - mx.M21, mx.M11);
				yaw = 0f;
			}
		}

		public static void SerializeValue(XmlWriter writer, Vector3 v)
		{
			writer.WriteValue(v.X.ToString(CultureInfo.InvariantCulture) + " " + v.Y.ToString(CultureInfo.InvariantCulture) + " " + v.Z.ToString(CultureInfo.InvariantCulture));
		}

		public static void SerializeValue(XmlWriter writer, Vector4 v)
		{
			writer.WriteValue(v.X.ToString(CultureInfo.InvariantCulture) + " " + v.Y.ToString(CultureInfo.InvariantCulture) + " " + v.Z.ToString(CultureInfo.InvariantCulture) + " " + v.W.ToString(CultureInfo.InvariantCulture));
		}

		public static void ShuffleList<T>(this IList<T> list, int offset = 0, int? count = null)
		{
			int num = count ?? (list.Count - offset);
			while (num > 1)
			{
				num--;
				int randomInt = GetRandomInt(num + 1);
				T value = list[offset + randomInt];
				list[offset + randomInt] = list[offset + num];
				list[offset + num] = value;
			}
		}

		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T val = lhs;
			lhs = rhs;
			rhs = val;
		}

		public static void VectorPlaneRotation(Vector3D xVector, Vector3D yVector, out Vector3D xOut, out Vector3D yOut, float angle)
		{
			Vector3D vector3D = xVector * Math.Cos(angle) + yVector * Math.Sin(angle);
			Vector3D vector3D2 = xVector * Math.Cos((double)angle + Math.PI / 2.0) + yVector * Math.Sin((double)angle + Math.PI / 2.0);
			xOut = vector3D;
			yOut = vector3D2;
		}

		/// <summary>
		/// When location is null, creates new instance, stores it in location and returns it.
		/// When location is not null, returns it.
		/// </summary>
		public static T Init<T>(ref T location) where T : class, new()
		{
			return location ?? (location = new T());
		}

		public static TCollection PrepareCollection<TCollection, TElement>(ref TCollection collection) where TCollection : class, ICollection<TElement>, new()
		{
			if (collection == null)
			{
				collection = new TCollection();
			}
			else if (collection.Count != 0)
			{
				collection.Clear();
			}
			return collection;
		}

		public static ClearCollectionToken<List<TElement>, TElement> ReuseCollection<TElement>(ref List<TElement> collection)
		{
			return ReuseCollection<List<TElement>, TElement>(ref collection);
		}

		public static ClearCollectionToken<MyList<TElement>, TElement> ReuseCollection<TElement>(ref MyList<TElement> collection)
		{
			return ReuseCollection<MyList<TElement>, TElement>(ref collection);
		}

		public static ClearCollectionToken<HashSet<TElement>, TElement> ReuseCollection<TElement>(ref HashSet<TElement> collection)
		{
			return ReuseCollection<HashSet<TElement>, TElement>(ref collection);
		}

		public static ClearCollectionToken<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>> ReuseCollection<TKey, TValue>(ref Dictionary<TKey, TValue> collection)
		{
			return ReuseCollection<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>(ref collection);
		}

		public static ClearCollectionToken<TCollection, TElement> ReuseCollection<TCollection, TElement>(ref TCollection collection) where TCollection : class, ICollection<TElement>, new()
		{
			PrepareCollection<TCollection, TElement>(ref collection);
			return new ClearCollectionToken<TCollection, TElement>(collection);
		}

		public static ClearRangeToken<TElement> ReuseCollectionNested<TElement>(ref List<TElement> collection)
		{
			if (collection == null)
			{
				collection = new List<TElement>();
			}
			return new ClearRangeToken<TElement>(collection);
		}

		public static void InterlockedMax(ref long storage, long value)
		{
			long num = Interlocked.Read(ref storage);
			while (value > num)
			{
				Interlocked.CompareExchange(ref storage, value, num);
				num = Interlocked.Read(ref storage);
			}
		}

		[Conditional("DEBUG")]
		public static void CheckMainThread()
		{
		}
	}
}
