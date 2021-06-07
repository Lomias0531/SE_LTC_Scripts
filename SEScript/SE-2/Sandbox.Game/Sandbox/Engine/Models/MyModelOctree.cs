using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Models
{
	internal class MyModelOctree : IMyTriangePruningStructure
	{
		private MyModel m_model;

		private MyModelOctreeNode m_rootNode;

		public int Size => 0;

		private MyModelOctree()
		{
		}

		public MyModelOctree(MyModel model)
		{
			m_model = model;
			m_rootNode = new MyModelOctreeNode(model.BoundingBox);
			for (int i = 0; i < m_model.Triangles.Length; i++)
			{
				m_rootNode.AddTriangle(model, i, 0);
			}
			m_rootNode.OptimizeChilds();
		}

		public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity physObject, ref LineD line, IntersectionFlags flags)
		{
			BoundingSphereD boundingSphere = physObject.WorldVolume;
			if (!MyUtils.IsLineIntersectingBoundingSphere(ref line, ref boundingSphere))
			{
				return null;
			}
			MatrixD customInvMatrix = physObject.GetWorldMatrixNormalizedInv();
			return GetIntersectionWithLine(physObject, ref line, ref customInvMatrix, flags);
		}

		public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity physObject, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags)
		{
			Line line2 = new Line(Vector3D.Transform(line.From, ref customInvMatrix), Vector3D.Transform(line.To, ref customInvMatrix));
			return m_rootNode.GetIntersectionWithLine(physObject, m_model, ref line2, null, flags);
		}

		public void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result)
		{
			MatrixD customInvMatrix = entity.GetWorldMatrixNormalizedInv();
			GetTrianglesIntersectingLine(entity, ref line, ref customInvMatrix, flags, result);
		}

		public void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result)
		{
		}

		public void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
		{
			m_rootNode.GetTrianglesIntersectingSphere(m_model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
		}

		public bool GetIntersectionWithSphere(IMyEntity physObject, ref BoundingSphereD sphere)
		{
			MatrixD matrix = physObject.GetWorldMatrixNormalizedInv();
			Vector3D v = Vector3D.Transform(sphere.Center, ref matrix);
			BoundingSphere sphere2 = new BoundingSphere(v, (float)sphere.Radius);
			return m_rootNode.GetIntersectionWithSphere(m_model, ref sphere2);
		}

		public bool GetIntersectionWithSphere(ref BoundingSphere sphere)
		{
			return m_rootNode.GetIntersectionWithSphere(m_model, ref sphere);
		}

		public bool GetIntersectionWithAABB(IMyEntity physObject, ref BoundingBoxD aabb)
		{
			return false;
		}

		public void GetTrianglesIntersectingAABB(ref BoundingBox box, List<MyTriangle_Vertex_Normal> retTriangles, int maxNeighbourTriangles)
		{
		}

		public void Close()
		{
		}
	}
}
