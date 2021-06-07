using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels.Planet
{
	public class SphereOcclusionHelper
	{
		private float m_minRadius;

		private float m_maxRadius;

		private float m_baseRadius;

		private Vector3 m_lastUpdatePosition;

		public float OcclusionRange
		{
			get;
			private set;
		}

		public float OcclusionAngleCosine
		{
			get;
			private set;
		}

		public float OcclusionDistance
		{
			get;
			private set;
		}

		public SphereOcclusionHelper(float minRadius, float maxRadius)
		{
			m_minRadius = minRadius;
			m_maxRadius = maxRadius;
		}

		public void CalculateOcclusion(Vector3 position)
		{
		}

		public void DebugDraw(MatrixD worldMatrix)
		{
			Vector3D translation = worldMatrix.Translation;
			MyRenderProxy.DebugDrawSphere(translation, m_minRadius, Color.Red, 0.2f, depthRead: true, smooth: true);
			MyRenderProxy.DebugDrawSphere(translation, m_maxRadius, Color.Red, 0.2f, depthRead: true, smooth: true);
			float num = m_lastUpdatePosition.Length();
			Vector3 vector = m_lastUpdatePosition / num;
			MyRenderProxy.DebugDrawLine3D(translation, translation + OcclusionDistance * vector, Color.Green, Color.Green, depthRead: true);
			MyRenderProxy.DebugDrawCone(translation + vector * (num - OcclusionDistance), baseVec: Vector3D.CalculatePerpendicularVector(vector) * m_baseRadius, directionVec: vector * OcclusionDistance, color: Color.Blue, depthRead: true);
		}
	}
}
