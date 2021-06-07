using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyShapeBox : MyShape, IMyVoxelShapeBox, IMyVoxelShape
	{
		public BoundingBoxD Boundaries;

		BoundingBoxD IMyVoxelShapeBox.Boundaries
		{
			get
			{
				return Boundaries;
			}
			set
			{
				Boundaries = value;
			}
		}

		public override BoundingBoxD GetWorldBoundaries()
		{
			return Boundaries.TransformFast(base.Transformation);
		}

		public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
		{
			MatrixD transformation = base.Transformation;
			transformation.Translation = targetPosition;
			return Boundaries.TransformFast(transformation);
		}

		public override BoundingBoxD GetLocalBounds()
		{
			return Boundaries;
		}

		public override float GetVolume(ref Vector3D voxelPosition)
		{
			if (m_inverseIsDirty)
			{
				m_inverse = MatrixD.Invert(m_transformation);
				m_inverseIsDirty = false;
			}
			voxelPosition = Vector3D.Transform(voxelPosition, m_inverse);
			Vector3D center = Boundaries.Center;
			return SignedDistanceToDensity((float)(Vector3D.Abs(voxelPosition - center) - (center - Boundaries.Min)).Max());
		}

		public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationBox(Boundaries, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);
		}

		public override void SendCutOutRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationBox(Boundaries, base.Transformation, 0, MyVoxelBase.OperationType.Cut);
		}

		public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationBox(Boundaries, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);
		}

		public override void SendRevertRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationBox(Boundaries, base.Transformation, 0, MyVoxelBase.OperationType.Revert);
		}

		public override MyShape Clone()
		{
			return new MyShapeBox
			{
				Transformation = base.Transformation,
				Boundaries = Boundaries
			};
		}
	}
}
