using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyShapeSphere : MyShape, IMyVoxelShapeSphere, IMyVoxelShape
	{
		public Vector3D Center;

		public float Radius;

		Vector3D IMyVoxelShapeSphere.Center
		{
			get
			{
				return Center;
			}
			set
			{
				Center = value;
			}
		}

		float IMyVoxelShapeSphere.Radius
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

		public MyShapeSphere()
		{
		}

		public MyShapeSphere(Vector3D center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public override BoundingBoxD GetWorldBoundaries()
		{
			return new BoundingBoxD(Center - Radius, Center + Radius).TransformFast(base.Transformation);
		}

		public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
		{
			return new BoundingBoxD(targetPosition - Radius, targetPosition + Radius);
		}

		public override BoundingBoxD GetLocalBounds()
		{
			return new BoundingBoxD(Center - Radius, Center + Radius);
		}

		public override float GetVolume(ref Vector3D voxelPosition)
		{
			if (m_inverseIsDirty)
			{
				MatrixD.Invert(ref m_transformation, out m_inverse);
				m_inverseIsDirty = false;
			}
			Vector3D.Transform(ref voxelPosition, ref m_inverse, out voxelPosition);
			float signedDistance = (float)(voxelPosition - Center).Length() - Radius;
			return SignedDistanceToDensity(signedDistance);
		}

		public override void SendDrillCutOutRequest(MyVoxelBase voxel, bool damage = false)
		{
			voxel.RequestVoxelCutoutSphere(Center, Radius, createDebris: false, damage);
		}

		public override void SendCutOutRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationSphere(Center, Radius, 0, MyVoxelBase.OperationType.Cut);
		}

		public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationSphere(Center, Radius, newMaterialIndex, MyVoxelBase.OperationType.Paint);
		}

		public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex)
		{
			voxel.RequestVoxelOperationSphere(Center, Radius, newMaterialIndex, MyVoxelBase.OperationType.Fill);
		}

		public override void SendRevertRequest(MyVoxelBase voxel)
		{
			voxel.RequestVoxelOperationSphere(Center, Radius, 0, MyVoxelBase.OperationType.Revert);
		}

		public override MyShape Clone()
		{
			return new MyShapeSphere
			{
				Transformation = base.Transformation,
				Center = Center,
				Radius = Radius
			};
		}
	}
}
