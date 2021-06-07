using Sandbox.Game.Entities;
using System;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyShapeRamp : MyShape, IMyVoxelShapeRamp, IMyVoxelShape
	{
		public BoundingBoxD Boundaries;

		public Vector3D RampNormal;

		public double RampNormalW;

		BoundingBoxD IMyVoxelShapeRamp.Boundaries
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

		Vector3D IMyVoxelShapeRamp.RampNormal
		{
			get
			{
				return RampNormal;
			}
			set
			{
				RampNormal = value;
			}
		}

		double IMyVoxelShapeRamp.RampNormalW
		{
			get
			{
				return RampNormalW;
			}
			set
			{
				RampNormalW = value;
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
			Vector3D vector3D = Vector3D.Abs(voxelPosition) - Boundaries.HalfExtents;
			double num = Vector3D.Dot(voxelPosition, RampNormal) + RampNormalW;
			return SignedDistanceToDensity((float)Math.Max(vector3D.Max(), 0.0 - num));
		}

		public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationRamp(Boundaries, RampNormal, RampNormalW, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);
		}

		public override void SendCutOutRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationRamp(Boundaries, RampNormal, RampNormalW, base.Transformation, 0, MyVoxelBase.OperationType.Cut);
		}

		public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationRamp(Boundaries, RampNormal, RampNormalW, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);
		}

		public override void SendRevertRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationRamp(Boundaries, RampNormal, RampNormalW, base.Transformation, 0, MyVoxelBase.OperationType.Revert);
		}

		public override MyShape Clone()
		{
			return new MyShapeRamp
			{
				Transformation = base.Transformation,
				Boundaries = Boundaries,
				RampNormal = RampNormal,
				RampNormalW = RampNormalW
			};
		}
	}
}
