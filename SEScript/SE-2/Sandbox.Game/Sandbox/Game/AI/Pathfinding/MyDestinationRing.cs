using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyDestinationRing : IMyDestinationShape
	{
		private float m_innerRadius;

		private float m_outerRadius;

		private Vector3D m_center;

		private Vector3D m_relativeCenter;

		public MyDestinationRing(ref Vector3D worldCenter, float innerRadius, float outerRadius)
		{
			Init(ref worldCenter, innerRadius, outerRadius);
		}

		public void Init(ref Vector3D worldCenter, float innerRadius, float outerRadius)
		{
			m_center = worldCenter;
			m_innerRadius = innerRadius;
			m_outerRadius = outerRadius;
		}

		public void Reinit(ref Vector3D worldCenter)
		{
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
			if (num < Math.Min(m_innerRadius - tolerance, 0f) || num > m_outerRadius + tolerance)
			{
				return float.PositiveInfinity;
			}
			return num;
		}

		public Vector3D GetClosestPoint(Vector3D queryPoint)
		{
			Vector3D value = queryPoint - m_center;
			double num = value.Length();
			if (num < (double)m_innerRadius)
			{
				return m_center + value / num * m_innerRadius;
			}
			if (num > (double)m_outerRadius)
			{
				return m_center + value / num * m_outerRadius;
			}
			return queryPoint;
		}

		public Vector3D GetBestPoint(Vector3D queryPoint)
		{
			Vector3D value = Vector3D.Normalize(queryPoint - m_center);
			return m_center + value * ((m_innerRadius + m_outerRadius) * 0.5f);
		}

		public Vector3D GetDestination()
		{
			return m_center;
		}

		public void DebugDraw()
		{
			MyRenderProxy.DebugDrawSphere(m_center, m_innerRadius, Color.RoyalBlue, 0.4f);
			MyRenderProxy.DebugDrawSphere(m_center, m_outerRadius, Color.Aqua, 0.4f);
		}
	}
}
