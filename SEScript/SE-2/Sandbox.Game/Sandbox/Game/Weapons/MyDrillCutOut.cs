using VRageMath;

namespace Sandbox.Game.Weapons
{
	public class MyDrillCutOut
	{
		private float m_centerOffset;

		private float m_radius;

		protected BoundingSphereD m_sphere;

		public BoundingSphereD Sphere => m_sphere;

		public MyDrillCutOut(float centerOffset, float radius)
		{
			m_centerOffset = centerOffset;
			m_radius = radius;
			m_sphere = new BoundingSphereD(Vector3D.Zero, m_radius);
		}

		public void UpdatePosition(ref MatrixD worldMatrix)
		{
			m_sphere.Center = worldMatrix.Translation + worldMatrix.Forward * m_centerOffset;
		}
	}
}
