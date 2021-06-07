using BulletXNA.BulletCollision;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
	public class MyQuantizedBvhAllTrianglesResult
	{
		private MyModel m_model;

		private readonly List<MyIntersectionResultLineTriangle> m_result = new List<MyIntersectionResultLineTriangle>();

		private Line m_line;

		private IntersectionFlags m_flags;

		private readonly MyResultComparer m_comparer = new MyResultComparer();

		public readonly ProcessCollisionHandler ProcessTriangleHandler;

		private Plane[] m_planes;

		public List<MyIntersectionResultLineTriangle> Result => m_result;

		public MyQuantizedBvhAllTrianglesResult()
		{
			ProcessTriangleHandler = ProcessTriangle;
		}

		public void Start(MyModel model, Line line, Plane[] planes, IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES)
		{
			m_result.Clear();
			m_model = model;
			m_line = line;
			m_flags = flags;
			m_planes = planes;
		}

		private float? ProcessTriangle(int triangleIndex)
		{
			MyTriangleVertexIndices myTriangleVertexIndices = m_model.Triangles[triangleIndex];
			MyTriangle_Vertices triangle = default(MyTriangle_Vertices);
			m_model.GetVertex(myTriangleVertexIndices.I0, myTriangleVertexIndices.I2, myTriangleVertexIndices.I1, out triangle.Vertex0, out triangle.Vertex1, out triangle.Vertex2);
			Vector3 triangleNormal = m_planes[triangleIndex].Normal;
			if ((m_flags & IntersectionFlags.FLIPPED_TRIANGLES) == 0 && m_line.Direction.Dot(ref triangleNormal) > 0f)
			{
				return null;
			}
			float? lineTriangleIntersection = MyUtils.GetLineTriangleIntersection(ref m_line, ref triangle);
			if (lineTriangleIntersection.HasValue)
			{
				MyTriangle_BoneIndicesWeigths? boneWeigths = m_model.GetBoneIndicesWeights(triangleIndex);
				MyIntersectionResultLineTriangle item = new MyIntersectionResultLineTriangle(triangleIndex, ref triangle, ref boneWeigths, ref triangleNormal, lineTriangleIntersection.Value);
				m_result.Add(item);
			}
			return lineTriangleIntersection;
		}

		public void End()
		{
			m_result.Sort(m_comparer);
		}
	}
}
