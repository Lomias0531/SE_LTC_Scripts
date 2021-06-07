using System;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	public struct MyDebugDrawBatchAabbLines : IMyDebugDrawBatchAabb, IDisposable
	{
		private MyRenderMessageDebugDrawLine3DBatch m_msg;

		private Color m_color;

		internal MyDebugDrawBatchAabbLines(MyRenderMessageDebugDrawLine3DBatch msg, ref MatrixD worldMatrix, Color color, bool depthRead)
		{
			m_msg = msg;
			m_msg.WorldMatrix = worldMatrix;
			m_msg.DepthRead = depthRead;
			m_color = color;
		}

		public void Add(ref BoundingBoxD aabb, Color? color = null)
		{
			Color color2 = color ?? m_color;
			Vector3D pointFrom = new Vector3D(aabb.Min.X, aabb.Min.Y, aabb.Min.Z);
			Vector3D vector3D = new Vector3D(aabb.Max.X, aabb.Min.Y, aabb.Min.Z);
			Vector3D vector3D2 = new Vector3D(aabb.Min.X, aabb.Min.Y, aabb.Max.Z);
			Vector3D vector3D3 = new Vector3D(aabb.Max.X, aabb.Min.Y, aabb.Max.Z);
			Vector3D vector3D4 = new Vector3D(aabb.Min.X, aabb.Max.Y, aabb.Min.Z);
			Vector3D vector3D5 = new Vector3D(aabb.Max.X, aabb.Max.Y, aabb.Min.Z);
			Vector3D vector3D6 = new Vector3D(aabb.Min.X, aabb.Max.Y, aabb.Max.Z);
			Vector3D pointTo = new Vector3D(aabb.Max.X, aabb.Max.Y, aabb.Max.Z);
			m_msg.AddLine(pointFrom, color2, vector3D, color2);
			m_msg.AddLine(vector3D2, color2, vector3D3, color2);
			m_msg.AddLine(vector3D4, color2, vector3D5, color2);
			m_msg.AddLine(vector3D6, color2, pointTo, color2);
			m_msg.AddLine(pointFrom, color2, vector3D2, color2);
			m_msg.AddLine(vector3D, color2, vector3D3, color2);
			m_msg.AddLine(vector3D4, color2, vector3D6, color2);
			m_msg.AddLine(vector3D5, color2, pointTo, color2);
			m_msg.AddLine(pointFrom, color2, vector3D4, color2);
			m_msg.AddLine(vector3D, color2, vector3D5, color2);
			m_msg.AddLine(vector3D2, color2, vector3D6, color2);
			m_msg.AddLine(vector3D3, color2, pointTo, color2);
		}

		public void Dispose()
		{
			MyRenderProxy.DebugDrawLine3DSubmitBatch(m_msg);
		}
	}
}
