using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Models
{
	internal class MyModelOctreeNode
	{
		private const int OCTREE_CHILDS_COUNT = 8;

		private const int MAX_RECURSIVE_LEVEL = 8;

		private const float CHILD_BOUNDING_BOX_EXPAND = 0.3f;

		private List<MyModelOctreeNode> m_childs;

		private BoundingBox m_boundingBox;

		private BoundingBox m_realBoundingBox;

		private List<int> m_triangleIndices;

		private MyModelOctreeNode()
		{
		}

		public MyModelOctreeNode(BoundingBox boundingBox)
		{
			m_childs = new List<MyModelOctreeNode>(8);
			for (int i = 0; i < 8; i++)
			{
				m_childs.Add(null);
			}
			m_boundingBox = boundingBox;
			m_realBoundingBox = BoundingBox.CreateInvalid();
			m_triangleIndices = new List<int>();
		}

		public void OptimizeChilds()
		{
			m_boundingBox = m_realBoundingBox;
			for (int i = 0; i < m_childs.Count; i++)
			{
				if (m_childs[i] != null)
				{
					m_childs[i].OptimizeChilds();
				}
			}
			while (m_childs.Remove(null))
			{
			}
			while (m_childs != null && m_childs.Count == 1)
			{
				foreach (int triangleIndex in m_childs[0].m_triangleIndices)
				{
					m_triangleIndices.Add(triangleIndex);
				}
				m_childs = m_childs[0].m_childs;
			}
			if (m_childs != null && m_childs.Count == 0)
			{
				m_childs = null;
			}
		}

		public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity physObject, MyModel model, ref Line line, double? minDistanceUntilNow, IntersectionFlags flags)
		{
			MyIntersectionResultLineTriangle? intersectionWithLineRecursive = GetIntersectionWithLineRecursive(model, ref line, minDistanceUntilNow);
			if (intersectionWithLineRecursive.HasValue)
			{
				return new MyIntersectionResultLineTriangleEx(intersectionWithLineRecursive.Value, physObject, ref line);
			}
			return null;
		}

		private MyIntersectionResultLineTriangle? GetIntersectionWithLineRecursive(MyModel model, ref Line line, double? minDistanceUntilNow)
		{
			double? num = MyUtils.GetLineBoundingBoxIntersection(ref line, ref m_boundingBox);
			if (!num.HasValue || (minDistanceUntilNow.HasValue && minDistanceUntilNow < num.Value))
			{
				return null;
			}
			MyIntersectionResultLineTriangle? a = null;
			BoundingBox boundingBox = default(BoundingBox);
			BoundingBox box = BoundingBox.CreateInvalid().Include(line.From).Include(line.To);
			MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
			for (int i = 0; i < m_triangleIndices.Count; i++)
			{
				int num2 = m_triangleIndices[i];
				model.GetTriangleBoundingBox(num2, ref boundingBox);
				if (boundingBox.Intersects(ref box))
				{
					MyTriangleVertexIndices myTriangleVertexIndices = model.Triangles[num2];
					triangle.Vertex0 = model.GetVertex(myTriangleVertexIndices.I0);
					triangle.Vertex1 = model.GetVertex(myTriangleVertexIndices.I2);
					triangle.Vertex2 = model.GetVertex(myTriangleVertexIndices.I1);
					float? lineTriangleIntersection = MyUtils.GetLineTriangleIntersection(ref line, ref triangle);
					if (lineTriangleIntersection.HasValue && (!a.HasValue || lineTriangleIntersection.Value < a.Value.Distance))
					{
						Vector3 triangleNormal = MyUtils.GetNormalVectorFromTriangle(ref triangle);
						a = new MyIntersectionResultLineTriangle(num2, ref triangle, ref triangleNormal, lineTriangleIntersection.Value);
					}
				}
			}
			if (m_childs != null)
			{
				for (int j = 0; j < m_childs.Count; j++)
				{
					MyIntersectionResultLineTriangle? b = m_childs[j].GetIntersectionWithLineRecursive(model, ref line, (!a.HasValue) ? null : new double?(a.Value.Distance));
					a = MyIntersectionResultLineTriangle.GetCloserIntersection(ref a, ref b);
				}
			}
			return a;
		}

		public void GetTrianglesIntersectingSphere(MyModel model, ref BoundingSphereD sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normal> retTriangles, int maxNeighbourTriangles)
		{
			BoundingSphere sphere2 = sphere;
			if (!m_boundingBox.Intersects(ref sphere))
			{
				return;
			}
			BoundingBox boundingBox = default(BoundingBox);
			MyTriangle_Vertices inputTriangle = default(MyTriangle_Vertices);
			MyTriangle_Vertex_Normal item = default(MyTriangle_Vertex_Normal);
			for (int i = 0; i < m_triangleIndices.Count; i++)
			{
				if (retTriangles.Count == maxNeighbourTriangles)
				{
					return;
				}
				int num = m_triangleIndices[i];
				model.GetTriangleBoundingBox(num, ref boundingBox);
				if (!boundingBox.Intersects(ref sphere))
				{
					continue;
				}
				MyTriangleVertexIndices myTriangleVertexIndices = model.Triangles[num];
				inputTriangle.Vertex0 = model.GetVertex(myTriangleVertexIndices.I0);
				inputTriangle.Vertex1 = model.GetVertex(myTriangleVertexIndices.I2);
				inputTriangle.Vertex2 = model.GetVertex(myTriangleVertexIndices.I1);
				Vector3 normalVectorFromTriangle = MyUtils.GetNormalVectorFromTriangle(ref inputTriangle);
				Plane trianglePlane = new Plane(inputTriangle.Vertex0, inputTriangle.Vertex1, inputTriangle.Vertex2);
				if (MyUtils.GetSphereTriangleIntersection(ref sphere2, ref trianglePlane, ref inputTriangle).HasValue)
				{
					Vector3 normalVectorFromTriangle2 = MyUtils.GetNormalVectorFromTriangle(ref inputTriangle);
					if (!referenceNormalVector.HasValue || !maxAngle.HasValue || MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, normalVectorFromTriangle2) <= maxAngle)
					{
						item.Vertexes = inputTriangle;
						item.Normal = normalVectorFromTriangle;
						retTriangles.Add(item);
					}
				}
			}
			if (m_childs != null)
			{
				for (int j = 0; j < m_childs.Count; j++)
				{
					m_childs[j].GetTrianglesIntersectingSphere(model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
				}
			}
		}

		public void GetTrianglesIntersectingSphere(MyModel model, ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
		{
			if (!m_boundingBox.Intersects(ref sphere))
			{
				return;
			}
			BoundingBox boundingBox = default(BoundingBox);
			MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
			MyTriangle_Normals normals = default(MyTriangle_Normals);
			MyTriangle_Vertex_Normals item = default(MyTriangle_Vertex_Normals);
			for (int i = 0; i < m_triangleIndices.Count; i++)
			{
				if (retTriangles.Count == maxNeighbourTriangles)
				{
					return;
				}
				int num = m_triangleIndices[i];
				model.GetTriangleBoundingBox(num, ref boundingBox);
				if (!boundingBox.Intersects(ref sphere))
				{
					continue;
				}
				MyTriangleVertexIndices myTriangleVertexIndices = model.Triangles[num];
				triangle.Vertex0 = model.GetVertex(myTriangleVertexIndices.I0);
				triangle.Vertex1 = model.GetVertex(myTriangleVertexIndices.I2);
				triangle.Vertex2 = model.GetVertex(myTriangleVertexIndices.I1);
				normals.Normal0 = model.GetVertexNormal(myTriangleVertexIndices.I0);
				normals.Normal1 = model.GetVertexNormal(myTriangleVertexIndices.I2);
				normals.Normal2 = model.GetVertexNormal(myTriangleVertexIndices.I1);
				Plane trianglePlane = new Plane(triangle.Vertex0, triangle.Vertex1, triangle.Vertex2);
				if (MyUtils.GetSphereTriangleIntersection(ref sphere, ref trianglePlane, ref triangle).HasValue)
				{
					Vector3 normalVectorFromTriangle = MyUtils.GetNormalVectorFromTriangle(ref triangle);
					if (!referenceNormalVector.HasValue || !maxAngle.HasValue || MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, normalVectorFromTriangle) <= maxAngle)
					{
						item.Vertices = triangle;
						item.Normals = normals;
						retTriangles.Add(item);
					}
				}
			}
			if (m_childs != null)
			{
				for (int j = 0; j < m_childs.Count; j++)
				{
					m_childs[j].GetTrianglesIntersectingSphere(model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
				}
			}
		}

		public bool GetIntersectionWithSphere(MyModel model, ref BoundingSphere sphere)
		{
			if (!m_boundingBox.Intersects(ref sphere))
			{
				return false;
			}
			BoundingBox boundingBox = default(BoundingBox);
			MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
			for (int i = 0; i < m_triangleIndices.Count; i++)
			{
				int num = m_triangleIndices[i];
				model.GetTriangleBoundingBox(num, ref boundingBox);
				if (boundingBox.Intersects(ref sphere))
				{
					MyTriangleVertexIndices myTriangleVertexIndices = model.Triangles[num];
					triangle.Vertex0 = model.GetVertex(myTriangleVertexIndices.I0);
					triangle.Vertex1 = model.GetVertex(myTriangleVertexIndices.I2);
					triangle.Vertex2 = model.GetVertex(myTriangleVertexIndices.I1);
					Plane trianglePlane = new Plane(triangle.Vertex0, triangle.Vertex1, triangle.Vertex2);
					if (MyUtils.GetSphereTriangleIntersection(ref sphere, ref trianglePlane, ref triangle).HasValue)
					{
						return true;
					}
				}
			}
			if (m_childs != null)
			{
				for (int j = 0; j < m_childs.Count; j++)
				{
					if (m_childs[j].GetIntersectionWithSphere(model, ref sphere))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddTriangle(MyModel model, int triangleIndex, int recursiveLevel)
		{
			BoundingBox boundingBox = default(BoundingBox);
			model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
			if (recursiveLevel != 8)
			{
				for (int i = 0; i < 8; i++)
				{
					BoundingBox childBoundingBox = GetChildBoundingBox(m_boundingBox, i);
					if (childBoundingBox.Contains(boundingBox) == ContainmentType.Contains)
					{
						if (m_childs[i] == null)
						{
							m_childs[i] = new MyModelOctreeNode(childBoundingBox);
						}
						m_childs[i].AddTriangle(model, triangleIndex, recursiveLevel + 1);
						m_realBoundingBox = m_realBoundingBox.Include(ref boundingBox.Min);
						m_realBoundingBox = m_realBoundingBox.Include(ref boundingBox.Max);
						return;
					}
				}
			}
			m_triangleIndices.Add(triangleIndex);
			m_realBoundingBox = m_realBoundingBox.Include(ref boundingBox.Min);
			m_realBoundingBox = m_realBoundingBox.Include(ref boundingBox.Max);
		}

		private BoundingBox GetChildBoundingBox(BoundingBox parentBoundingBox, int childIndex)
		{
			Vector3 value;
			switch (childIndex)
			{
			case 0:
				value = new Vector3(0f, 0f, 0f);
				break;
			case 1:
				value = new Vector3(1f, 0f, 0f);
				break;
			case 2:
				value = new Vector3(1f, 0f, 1f);
				break;
			case 3:
				value = new Vector3(0f, 0f, 1f);
				break;
			case 4:
				value = new Vector3(0f, 1f, 0f);
				break;
			case 5:
				value = new Vector3(1f, 1f, 0f);
				break;
			case 6:
				value = new Vector3(1f, 1f, 1f);
				break;
			case 7:
				value = new Vector3(0f, 1f, 1f);
				break;
			default:
				throw new InvalidBranchException();
			}
			Vector3 vector = (parentBoundingBox.Max - parentBoundingBox.Min) / 2f;
			BoundingBox result = default(BoundingBox);
			result.Min = parentBoundingBox.Min + value * vector;
			result.Max = result.Min + vector;
			result.Min -= vector * 0.3f;
			result.Max += vector * 0.3f;
			result.Min = Vector3.Max(result.Min, parentBoundingBox.Min);
			result.Max = Vector3.Min(result.Max, parentBoundingBox.Max);
			return result;
		}
	}
}
