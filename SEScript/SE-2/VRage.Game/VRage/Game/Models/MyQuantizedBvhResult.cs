using BulletXNA.BulletCollision;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
	public class MyQuantizedBvhResult
	{
		private MyModel m_model;

		private MyIntersectionResultLineTriangle? m_result;

		private Line m_line;

		private IntersectionFlags m_flags;

		public readonly ProcessCollisionHandler ProcessTriangleHandler;

		private Plane[] m_planes;

		public MyIntersectionResultLineTriangle? Result => m_result;

		public MyQuantizedBvhResult()
		{
			ProcessTriangleHandler = ProcessTriangle;
		}

		public void Start(MyModel model, Line line, Plane[] planes, IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES)
		{
			m_result = null;
			m_model = model;
			m_planes = planes;
			m_line = line;
			m_flags = flags;
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
			if (lineTriangleIntersection.HasValue && float.IsNaN(lineTriangleIntersection.Value))
			{
				MyLog.Default.Warning("Invalid triangle in " + m_model.AssetName);
			}
			if (lineTriangleIntersection.HasValue && !float.IsNaN(lineTriangleIntersection.Value) && (!m_result.HasValue || lineTriangleIntersection.Value < m_result.Value.Distance))
			{
				MyTriangle_BoneIndicesWeigths? boneWeigths = m_model.GetBoneIndicesWeights(triangleIndex);
				m_result = new MyIntersectionResultLineTriangle(triangleIndex, ref triangle, ref boneWeigths, ref triangleNormal, lineTriangleIntersection.Value);
				return lineTriangleIntersection.Value;
			}
			return null;
		}
	}
}
