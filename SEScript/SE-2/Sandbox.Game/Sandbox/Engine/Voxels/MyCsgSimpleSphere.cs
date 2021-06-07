using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	internal class MyCsgSimpleSphere : MyCsgShapeBase
	{
		private Vector3 m_translation;

		private float m_radius;

		public MyCsgSimpleSphere(Vector3 translation, float radius)
		{
			m_translation = translation;
			m_radius = radius;
		}

		internal override ContainmentType Contains(ref BoundingBox queryAabb, ref BoundingSphere querySphere, float lodVoxelSize)
		{
			BoundingSphere boundingSphere = new BoundingSphere(m_translation, m_radius + lodVoxelSize);
			boundingSphere.Contains(ref queryAabb, out ContainmentType result);
			if (result == ContainmentType.Disjoint)
			{
				return ContainmentType.Disjoint;
			}
			boundingSphere.Radius = m_radius - lodVoxelSize;
			boundingSphere.Contains(ref queryAabb, out ContainmentType result2);
			if (result2 == ContainmentType.Contains)
			{
				return ContainmentType.Contains;
			}
			return ContainmentType.Intersects;
		}

		internal override float SignedDistance(ref Vector3 position, float lodVoxelSize, IMyModule macroModulator, IMyModule detailModulator)
		{
			float num = (position - m_translation).Length();
			if (m_radius - lodVoxelSize > num)
			{
				return -1f;
			}
			if (m_radius + lodVoxelSize < num)
			{
				return 1f;
			}
			return SignedDistanceInternal(lodVoxelSize, num);
		}

		private float SignedDistanceInternal(float lodVoxelSize, float distance)
		{
			return (distance - m_radius) / lodVoxelSize;
		}

		internal override float SignedDistanceUnchecked(ref Vector3 position, float lodVoxelSize, IMyModule macroModulator, IMyModule detailModulator)
		{
			float distance = (position - m_translation).Length();
			return SignedDistanceInternal(lodVoxelSize, distance);
		}

		internal override void DebugDraw(ref MatrixD worldTranslation, Color color)
		{
			MyRenderProxy.DebugDrawSphere(Vector3D.Transform(m_translation, worldTranslation), m_radius, color.ToVector3(), 0.5f);
		}

		internal override MyCsgShapeBase DeepCopy()
		{
			return new MyCsgSimpleSphere(m_translation, m_radius);
		}

		internal override void ShrinkTo(float percentage)
		{
			m_radius *= percentage;
		}

		internal override Vector3 Center()
		{
			return m_translation;
		}
	}
}
