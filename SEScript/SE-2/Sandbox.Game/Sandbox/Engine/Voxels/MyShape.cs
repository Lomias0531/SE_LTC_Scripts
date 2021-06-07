using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public abstract class MyShape : IMyVoxelShape
	{
		protected MatrixD m_transformation = MatrixD.Identity;

		protected MatrixD m_inverse = MatrixD.Identity;

		protected bool m_inverseIsDirty;

		public MatrixD Transformation
		{
			get
			{
				return m_transformation;
			}
			set
			{
				m_transformation = value;
				m_inverseIsDirty = true;
			}
		}

		public MatrixD InverseTransformation
		{
			get
			{
				if (m_inverseIsDirty)
				{
					MatrixD.Invert(ref m_transformation, out m_inverse);
					m_inverseIsDirty = false;
				}
				return m_inverse;
			}
		}

		MatrixD IMyVoxelShape.Transform
		{
			get
			{
				return Transformation;
			}
			set
			{
				Transformation = value;
			}
		}

		public abstract BoundingBoxD GetWorldBoundaries();

		public abstract BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition);

		public abstract BoundingBoxD GetLocalBounds();

		public abstract float GetVolume(ref Vector3D voxelPosition);

		protected float SignedDistanceToDensity(float signedDistance)
		{
			return MathHelper.Clamp(0f - signedDistance, -1f, 1f) * 0.5f + 0.5f;
		}

		public abstract void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex);

		public abstract void SendCutOutRequest(MyVoxelBase voxelbool);

		public virtual void SendDrillCutOutRequest(MyVoxelBase voxel, bool damage = false)
		{
		}

		public abstract void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex);

		public abstract void SendRevertRequest(MyVoxelBase voxel);

		public abstract MyShape Clone();

		BoundingBoxD IMyVoxelShape.GetWorldBoundary()
		{
			return GetWorldBoundaries();
		}

		BoundingBoxD IMyVoxelShape.PeekWorldBoundary(ref Vector3D targetPosition)
		{
			return PeekWorldBoundaries(ref targetPosition);
		}

		float IMyVoxelShape.GetIntersectionVolume(ref Vector3D voxelPosition)
		{
			return GetVolume(ref voxelPosition);
		}
	}
}
