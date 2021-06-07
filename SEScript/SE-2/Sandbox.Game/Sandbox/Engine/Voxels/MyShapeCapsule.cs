using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyShapeCapsule : MyShape, IMyVoxelShapeCapsule, IMyVoxelShape
	{
		public Vector3D A;

		public Vector3D B;

		public float Radius;

		Vector3D IMyVoxelShapeCapsule.A
		{
			get
			{
				return A;
			}
			set
			{
				A = value;
			}
		}

		Vector3D IMyVoxelShapeCapsule.B
		{
			get
			{
				return B;
			}
			set
			{
				B = value;
			}
		}

		float IMyVoxelShapeCapsule.Radius
		{
			get
			{
				return Radius;
			}
			set
			{
				Radius = value;
			}
		}

		public override BoundingBoxD GetWorldBoundaries()
		{
			return new BoundingBoxD(A - Radius, B + Radius).TransformFast(base.Transformation);
		}

		public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
		{
			MatrixD transformation = base.Transformation;
			transformation.Translation = targetPosition;
			return new BoundingBoxD(A - Radius, B + Radius).TransformFast(transformation);
		}

		public override BoundingBoxD GetLocalBounds()
		{
			return new BoundingBoxD(A - Radius, B + Radius);
		}

		public override float GetVolume(ref Vector3D voxelPosition)
		{
			if (m_inverseIsDirty)
			{
				m_inverse = MatrixD.Invert(m_transformation);
				m_inverseIsDirty = false;
			}
			voxelPosition = Vector3D.Transform(voxelPosition, m_inverse);
			Vector3D value = voxelPosition - A;
			Vector3D v = B - A;
			double scaleFactor = MathHelper.Clamp(value.Dot(ref v) / v.LengthSquared(), 0.0, 1.0);
			float signedDistance = (float)((value - v * scaleFactor).Length() - (double)Radius);
			return SignedDistanceToDensity(signedDistance);
		}

		public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationCapsule(A, B, Radius, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);
		}

		public override void SendCutOutRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationCapsule(A, B, Radius, base.Transformation, 0, MyVoxelBase.OperationType.Cut);
		}

		public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationCapsule(A, B, Radius, base.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);
		}

		public override void SendRevertRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationCapsule(A, B, Radius, base.Transformation, 0, MyVoxelBase.OperationType.Revert);
		}

		public override MyShape Clone()
		{
			return new MyShapeCapsule
			{
				Transformation = base.Transformation,
				A = A,
				B = B,
				Radius = Radius
			};
		}
	}
}
