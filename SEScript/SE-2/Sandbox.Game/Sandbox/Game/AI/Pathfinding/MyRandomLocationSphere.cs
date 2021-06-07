using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyRandomLocationSphere : IMyDestinationShape
	{
		private Vector3D m_center;

		private Vector3D m_relativeCenter;

		private Vector3D m_desiredDirection;

		private float m_radius;

		public MyRandomLocationSphere(Vector3D worldCenter, float radius, Vector3D direction)
		{
			Init(ref worldCenter, radius, direction);
		}

		public void Init(ref Vector3D worldCenter, float radius, Vector3D direction)
		{
			m_center = worldCenter;
			m_radius = radius;
			m_desiredDirection = direction;
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
			Vector3D vector3D = position - m_center;
			float num = (float)vector3D.Normalize();
			if (num < m_radius + tolerance || vector3D.Dot(ref m_desiredDirection) < 0.9)
			{
				return float.PositiveInfinity;
			}
			return num;
		}

		public Vector3D GetClosestPoint(Vector3D queryPoint)
		{
			Vector3D v = queryPoint - m_center;
			if (v.Normalize() > (double)m_radius)
			{
				return queryPoint;
			}
			if (m_desiredDirection.Dot(ref v) > 0.9)
			{
				return m_center + v * m_radius;
			}
			return m_center + m_desiredDirection * m_radius;
		}

		public Vector3D GetBestPoint(Vector3D queryPoint)
		{
			if ((queryPoint - m_center).Length() > (double)m_radius)
			{
				return queryPoint;
			}
			return m_center + m_desiredDirection * m_radius;
		}

		public Vector3D GetDestination()
		{
			return m_center + m_desiredDirection * m_radius;
		}

		public void DebugDraw()
		{
			MyRenderProxy.DebugDrawSphere(m_center, m_radius, Color.Gainsboro);
			MyRenderProxy.DebugDrawSphere(m_center + m_desiredDirection * m_radius, 4f, Color.Aqua);
		}
	}
}
