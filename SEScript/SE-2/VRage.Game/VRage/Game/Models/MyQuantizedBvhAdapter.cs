using BulletXNA.BulletCollision;
using BulletXNA.LinearMath;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Import;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
	public class MyQuantizedBvhAdapter : IMyTriangePruningStructure
	{
		private readonly GImpactQuantizedBvh m_bvh;

		private readonly MyModel m_model;

		[ThreadStatic]
		private static MyQuantizedBvhResult m_resultThreadStatic;

		[ThreadStatic]
		private static MyQuantizedBvhAllTrianglesResult m_resultAllThreadStatic;

		private BoundingBox[] m_bounds;

		private Plane[] m_planes;

		private static MyQuantizedBvhResult Result => m_resultThreadStatic ?? (m_resultThreadStatic = new MyQuantizedBvhResult());

		private static MyQuantizedBvhAllTrianglesResult ResultAll => m_resultAllThreadStatic ?? (m_resultAllThreadStatic = new MyQuantizedBvhAllTrianglesResult());

		public int Size => m_bvh.Size;

		public MyQuantizedBvhAdapter(GImpactQuantizedBvh bvh, MyModel model)
		{
			m_bvh = bvh;
			m_model = model;
		}

		public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity entity, ref LineD line, IntersectionFlags flags)
		{
			BoundingSphereD boundingSphere = entity.PositionComp.WorldVolume;
			if (!MyUtils.IsLineIntersectingBoundingSphere(ref line, ref boundingSphere))
			{
				return null;
			}
			MatrixD customInvMatrix = entity.PositionComp.WorldMatrixInvScaled;
			return GetIntersectionWithLine(entity, ref line, ref customInvMatrix, flags);
		}

		public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(IMyEntity entity, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags)
		{
			Line line2 = new Line(Vector3D.Transform(line.From, ref customInvMatrix), Vector3D.Transform(line.To, ref customInvMatrix));
			UpdateCache();
			Result.Start(m_model, line2, m_planes, flags);
			IndexedVector3 ray_dir = line2.Direction.ToBullet();
			IndexedVector3 ray_origin = line2.From.ToBullet();
			m_bvh.RayQueryClosest(ref ray_dir, ref ray_origin, Result.ProcessTriangleHandler);
			if (Result.Result.HasValue)
			{
				return new MyIntersectionResultLineTriangleEx(Result.Result.Value, entity, ref line2);
			}
			return null;
		}

		public void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
		{
			if (retTriangles.Count != maxNeighbourTriangles)
			{
				UpdateCache();
				BoundingBox boundingBox = BoundingBox.CreateInvalid();
				BoundingSphere sphereLocal = sphere;
				boundingBox.Include(ref sphere);
				AABB box = new AABB(boundingBox.Min.ToBullet(), boundingBox.Max.ToBullet());
				m_bvh.BoxQuery(ref box, delegate(int triangleIndex)
				{
					if (CheckSphereTriangleIntersection(ref sphereLocal, triangleIndex) && (!referenceNormalVector.HasValue || !maxAngle.HasValue || MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, m_planes[triangleIndex].Normal) <= maxAngle))
					{
						MyTriangleVertexIndices myTriangleVertexIndices = m_model.Triangles[triangleIndex];
						MyTriangle_Vertex_Normals myTriangle_Vertex_Normals = default(MyTriangle_Vertex_Normals);
						myTriangle_Vertex_Normals.Vertices = new MyTriangle_Vertices
						{
							Vertex0 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I0].Position),
							Vertex1 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I1].Position),
							Vertex2 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I2].Position)
						};
						myTriangle_Vertex_Normals.Normals.Normal0 = m_model.GetVertexNormal(myTriangleVertexIndices.I0);
						myTriangle_Vertex_Normals.Normals.Normal1 = m_model.GetVertexNormal(myTriangleVertexIndices.I2);
						myTriangle_Vertex_Normals.Normals.Normal2 = m_model.GetVertexNormal(myTriangleVertexIndices.I1);
						MyTriangle_Vertex_Normals item = myTriangle_Vertex_Normals;
						retTriangles.Add(item);
						if (retTriangles.Count == maxNeighbourTriangles)
						{
							return true;
						}
					}
					return false;
				});
			}
		}

		public bool GetIntersectionWithAABB(IMyEntity entity, ref BoundingBoxD aabb)
		{
			UpdateCache();
			MatrixD matrix = entity.PositionComp.WorldMatrixNormalizedInv;
			Vector3D value = Vector3D.Transform(aabb.Center, ref matrix);
			BoundingBoxD boundingBoxD = aabb;
			boundingBoxD.Translate(value - aabb.Center);
			AABB box = new AABB(boundingBoxD.Min.ToBullet(), boundingBoxD.Max.ToBullet());
			return m_bvh.BoxQuery(ref box, (int triangleIndex) => true);
		}

		public bool GetIntersectionWithSphere(IMyEntity entity, ref BoundingSphereD sphere)
		{
			MatrixD matrix = entity.PositionComp.WorldMatrixNormalizedInv;
			Vector3 center = Vector3D.Transform(sphere.Center, ref matrix);
			BoundingSphere sphereInObjectSpace = new BoundingSphere(center, (float)sphere.Radius);
			return GetIntersectionWithSphere(ref sphereInObjectSpace);
		}

		private void UpdateCache()
		{
			if (m_bounds == null)
			{
				lock (m_bvh)
				{
					if (m_bounds == null)
					{
						int trianglesCount = m_model.GetTrianglesCount();
						m_bounds = new BoundingBox[trianglesCount];
						m_planes = new Plane[trianglesCount];
						for (int i = 0; i < trianglesCount; i++)
						{
							MyTriangleVertexIndices myTriangleVertexIndices = m_model.Triangles[i];
							MyTriangle_Vertices myTriangle_Vertices = default(MyTriangle_Vertices);
							myTriangle_Vertices.Vertex0 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I0].Position);
							myTriangle_Vertices.Vertex1 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I1].Position);
							myTriangle_Vertices.Vertex2 = PositionPacker.UnpackPosition(ref m_model.Vertices[myTriangleVertexIndices.I2].Position);
							MyTriangle_Vertices myTriangle_Vertices2 = myTriangle_Vertices;
							m_bounds[i].Min = (m_bounds[i].Max = myTriangle_Vertices2.Vertex0);
							m_bounds[i].Include(ref myTriangle_Vertices2.Vertex1);
							m_bounds[i].Include(ref myTriangle_Vertices2.Vertex2);
							m_planes[i] = new Plane(ref myTriangle_Vertices2.Vertex0, ref myTriangle_Vertices2.Vertex1, ref myTriangle_Vertices2.Vertex2);
						}
					}
				}
			}
		}

		public bool GetIntersectionWithSphere(ref BoundingSphere sphereInObjectSpace)
		{
			UpdateCache();
			BoundingBox boundingBox = BoundingBox.CreateInvalid();
			BoundingSphere sphereF = sphereInObjectSpace;
			boundingBox.Include(ref sphereInObjectSpace);
			AABB box = new AABB(boundingBox.Min.ToBullet(), boundingBox.Max.ToBullet());
			return m_bvh.BoxQuery(ref box, (int triangleIndex) => CheckSphereTriangleIntersection(ref sphereF, triangleIndex));
		}

		private bool CheckSphereTriangleIntersection(ref BoundingSphere sphereF, int triangleIndex)
		{
			if (m_bounds[triangleIndex].Intersects(ref sphereF))
			{
				MyTriangleVertexIndices myTriangleVertexIndices = m_model.Triangles[triangleIndex];
				MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
				m_model.GetVertex(myTriangleVertexIndices.I0, myTriangleVertexIndices.I1, myTriangleVertexIndices.I2, out triangle.Vertex0, out triangle.Vertex1, out triangle.Vertex2);
				if (MyUtils.GetSphereTriangleIntersection(ref sphereF, ref m_planes[triangleIndex], ref triangle).HasValue)
				{
					return true;
				}
			}
			return false;
		}

		public void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result)
		{
			MatrixD customInvMatrix = entity.PositionComp.WorldMatrixNormalizedInv;
			GetTrianglesIntersectingLine(entity, ref line, ref customInvMatrix, flags, result);
		}

		public void GetTrianglesIntersectingLine(IMyEntity entity, ref LineD line, ref MatrixD customInvMatrix, IntersectionFlags flags, List<MyIntersectionResultLineTriangleEx> result)
		{
			UpdateCache();
			Line line2 = new Line(Vector3D.Transform(line.From, ref customInvMatrix), Vector3D.Transform(line.To, ref customInvMatrix));
			ResultAll.Start(m_model, line2, m_planes, flags);
			IndexedVector3 ray_dir = line2.Direction.ToBullet();
			IndexedVector3 ray_origin = line2.From.ToBullet();
			m_bvh.RayQuery(ref ray_dir, ref ray_origin, ResultAll.ProcessTriangleHandler);
			ResultAll.End();
			foreach (MyIntersectionResultLineTriangle item in ResultAll.Result)
			{
				result.Add(new MyIntersectionResultLineTriangleEx(item, entity, ref line2));
			}
		}

		public void GetTrianglesIntersectingAABB(ref BoundingBox aabb, List<MyTriangle_Vertex_Normal> retTriangles, int maxNeighbourTriangles)
		{
			if (retTriangles.Count != maxNeighbourTriangles)
			{
				UpdateCache();
				IndexedVector3 min = aabb.Min.ToBullet();
				IndexedVector3 max = aabb.Max.ToBullet();
				AABB box = new AABB(ref min, ref max);
				m_bvh.BoxQuery(ref box, delegate(int triangleIndex)
				{
					MyTriangleVertexIndices myTriangleVertexIndices = m_model.Triangles[triangleIndex];
					MyTriangle_Vertices vertexes = default(MyTriangle_Vertices);
					m_model.GetVertex(myTriangleVertexIndices.I0, myTriangleVertexIndices.I1, myTriangleVertexIndices.I2, out vertexes.Vertex0, out vertexes.Vertex1, out vertexes.Vertex2);
					MyTriangle_Vertex_Normal item = default(MyTriangle_Vertex_Normal);
					item.Vertexes = vertexes;
					item.Normal = Vector3.Forward;
					retTriangles.Add(item);
					return retTriangles.Count == maxNeighbourTriangles;
				});
			}
		}

		public void Close()
		{
		}
	}
}
