using System;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyDestinationSphere : IMyDestinationShape
	{
		private float m_radius;

		private Vector3D m_center;

		private Vector3D m_relativeCenter;

		public MyDestinationSphere(ref Vector3D worldCenter, float radius)
		{
			Init(ref worldCenter, radius);
		}

		public void Init(ref Vector3D worldCenter, float radius)
		{
			m_radius = radius;
			m_center = worldCenter;
		}

		public void SetRelativeTransform(MatrixD invWorldTransform)
		{
			Vector3D.Transform(ref m_center, ref invWorldTransform, out m_relativeCenter);
		}

		public void UpdateWorldTransform(MatrixD worldTransform)
		{
			Vector3D.Transform(ref m_relativeCenter, ref worldTransform, out m_center);
		}

		public float PointAdmissibility(Vector3D position, float tolerance)
		{
			float num = (float)Vector3D.Distance(position, m_center);
			if (num <= m_radius + tolerance)
			{
				return num;
			}
			return float.PositiveInfinity;
		}

		public Vector3D GetClosestPoint(Vector3D queryPoint)
		{
			Vector3D value = queryPoint - m_center;
			double num = value.Length();
			if (num < (double)m_radius)
			{
				return queryPoint;
			}
			return m_center + value / num * m_radius;
		}

		public Vector3D GetBestPoint(Vector3D queryPoint)
		{
			return m_center;
		}

		public Vector3D GetDestination()
		{
			return m_center;
		}

		public void DebugDraw()
		{
			MyRenderProxy.DebugDrawSphere(m_center, Math.Max(m_radius, 0.05f), Color.Pink, 1f, depthRead: false);
			MyRenderProxy.DebugDrawSphere(m_center, m_radius, Color.Pink, 1f, depthRead: false);
			MyRenderProxy.DebugDrawText3D(m_center, "Destination", Color.Pink, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}
	}
}
