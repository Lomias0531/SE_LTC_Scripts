using System;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	public struct MyDebugDrawBatchAabbShaded : IMyDebugDrawBatchAabb, IDisposable
	{
		private MyRenderMessageDebugDrawTriangles m_msg;

		private MatrixD m_worldMatrix;

		private bool m_depthRead;

		internal MyDebugDrawBatchAabbShaded(MyRenderMessageDebugDrawTriangles msg, ref MatrixD worldMatrix, Color color, bool depthRead)
		{
			m_msg = msg;
			m_worldMatrix = worldMatrix;
			m_msg.Color = color;
			m_depthRead = depthRead;
		}

		public void Add(ref BoundingBoxD aabb, Color? color = null)
		{
			Color? color2 = color;
			if (!color2.HasValue)
			{
				_ = m_msg.Color;
			}
			else
			{
				color2.GetValueOrDefault();
			}
			int vertexCount = m_msg.VertexCount;
			m_msg.AddVertex(new Vector3D(aabb.Min.X, aabb.Min.Y, aabb.Min.Z));
			m_msg.AddVertex(new Vector3D(aabb.Max.X, aabb.Min.Y, aabb.Min.Z));
			m_msg.AddVertex(new Vector3D(aabb.Min.X, aabb.Min.Y, aabb.Max.Z));
			m_msg.AddVertex(new Vector3D(aabb.Max.X, aabb.Min.Y, aabb.Max.Z));
			m_msg.AddVertex(new Vector3D(aabb.Min.X, aabb.Max.Y, aabb.Min.Z));
			m_msg.AddVertex(new Vector3D(aabb.Max.X, aabb.Max.Y, aabb.Min.Z));
			m_msg.AddVertex(new Vector3D(aabb.Min.X, aabb.Max.Y, aabb.Max.Z));
			m_msg.AddVertex(new Vector3D(aabb.Max.X, aabb.Max.Y, aabb.Max.Z));
			m_msg.AddIndex(vertexCount + 1);
			m_msg.AddIndex(vertexCount);
			m_msg.AddIndex(vertexCount + 2);
			m_msg.AddIndex(vertexCount + 1);
			m_msg.AddIndex(vertexCount + 2);
			m_msg.AddIndex(vertexCount + 3);
			m_msg.AddIndex(vertexCount + 4);
			m_msg.AddIndex(vertexCount + 5);
			m_msg.AddIndex(vertexCount + 6);
			m_msg.AddIndex(vertexCount + 6);
			m_msg.AddIndex(vertexCount + 5);
			m_msg.AddIndex(vertexCount + 7);
			m_msg.AddIndex(vertexCount);
			m_msg.AddIndex(vertexCount + 1);
			m_msg.AddIndex(vertexCount + 4);
			m_msg.AddIndex(vertexCount + 4);
			m_msg.AddIndex(vertexCount + 1);
			m_msg.AddIndex(vertexCount + 5);
			m_msg.AddIndex(vertexCount + 3);
			m_msg.AddIndex(vertexCount + 2);
			m_msg.AddIndex(vertexCount + 6);
			m_msg.AddIndex(vertexCount + 3);
			m_msg.AddIndex(vertexCount + 6);
			m_msg.AddIndex(vertexCount + 7);
			m_msg.AddIndex(vertexCount + 1);
			m_msg.AddIndex(vertexCount + 3);
			m_msg.AddIndex(vertexCount + 5);
			m_msg.AddIndex(vertexCount + 5);
			m_msg.AddIndex(vertexCount + 3);
			m_msg.AddIndex(vertexCount + 7);
			m_msg.AddIndex(vertexCount + 4);
			m_msg.AddIndex(vertexCount + 2);
			m_msg.AddIndex(vertexCount);
			m_msg.AddIndex(vertexCount + 4);
			m_msg.AddIndex(vertexCount + 6);
			m_msg.AddIndex(vertexCount + 2);
		}

		public void Dispose()
		{
			MyRenderProxy.DebugDrawTriangles(m_msg, m_worldMatrix, m_depthRead);
		}
	}
}
