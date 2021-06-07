using Sandbox.Game.Entities;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyShapeEllipsoid : MyShape
	{
		private BoundingBoxD m_boundaries;

		private Matrix m_scaleMatrix = Matrix.Identity;

		private Matrix m_scaleMatrixInverse = Matrix.Identity;

		private Vector3 m_radius;

		public Vector3 Radius
		{
			get
			{
				return m_radius;
			}
			set
			{
				m_radius = value;
				m_scaleMatrix = Matrix.CreateScale(m_radius);
				m_scaleMatrixInverse = Matrix.Invert(m_scaleMatrix);
				m_boundaries = new BoundingBoxD(-Radius, Radius);
			}
		}

		public BoundingBoxD Boundaries => m_boundaries;

		public override BoundingBoxD GetWorldBoundaries()
		{
			return m_boundaries.TransformFast(base.Transformation);
		}

		public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
		{
			MatrixD transformation = base.Transformation;
			transformation.Translation = targetPosition;
			return m_boundaries.TransformFast(transformation);
		}

		public override BoundingBoxD GetLocalBounds()
		{
			return m_boundaries;
		}

		public override float GetVolume(ref Vector3D voxelPosition)
		{
			if (m_inverseIsDirty)
			{
				m_inverse = MatrixD.Invert(m_transformation);
				m_inverseIsDirty = false;
			}
			voxelPosition = Vector3D.Transform(voxelPosition, m_inverse);
			Vector3 position = Vector3D.Transform(voxelPosition, m_scaleMatrixInverse);
			position.Normalize();
			Vector3 vector = Vector3.Transform(position, m_scaleMatrix);
			float signedDistance = (float)(voxelPosition.Length() - (double)vector.Length());
			return SignedDistanceToDensity(signedDistance);
		}

		public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationElipsoid(Radius, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);
		}

		public override void SendCutOutRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationElipsoid(Radius, base.Transformation, 0, MyVoxelBase.OperationType.Cut);
		}

		public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationElipsoid(Radius, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);
		}

		public override void SendRevertRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationElipsoid(Radius, base.Transformation, 0, MyVoxelBase.OperationType.Revert);
		}

		public override MyShape Clone()
		{
			return new MyShapeEllipsoid
			{
				Transformation = base.Transformation,
				Radius = Radius
			};
		}
	}
}
