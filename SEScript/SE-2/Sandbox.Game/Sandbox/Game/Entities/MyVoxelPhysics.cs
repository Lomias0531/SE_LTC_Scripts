using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using VRage;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal class MyVoxelPhysics : MyVoxelBase
	{
		private class Sandbox_Game_Entities_MyVoxelPhysics_003C_003EActor : IActivator, IActivator<MyVoxelPhysics>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelPhysics();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelPhysics CreateInstance()
			{
				return new MyVoxelPhysics();
			}

			MyVoxelPhysics IActivator<MyVoxelPhysics>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyPlanet m_parent;

		internal new MyVoxelPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyVoxelPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public override MyVoxelBase RootVoxel => m_parent;

		public bool Valid
		{
			get;
			set;
		}

		public new MyPlanet Parent => m_parent;

		public MyVoxelPhysics()
		{
			AddDebugRenderComponent(new MyDebugRenderComponentVoxelMap(this));
		}

		public override void Init(MyObjectBuilder_EntityBase builder, IMyStorage storage)
		{
		}

		public void Init(IMyStorage storage, Vector3D positionMinCorner, Vector3I storageMin, Vector3I storageMax, MyPlanet parent)
		{
			PositionLeftBottomCorner = positionMinCorner;
			m_storageMax = storageMax;
			m_storageMin = storageMin;
			base.m_storage = storage;
			base.SizeInMetres = base.Size * 1f;
			base.SizeInMetresHalf = base.SizeInMetres / 2f;
			MatrixD worldMatrix = MatrixD.CreateTranslation(positionMinCorner + base.SizeInMetresHalf);
			Init(storage, worldMatrix, storageMin, storageMax, parent);
		}

		public void Init(IMyStorage storage, MatrixD worldMatrix, Vector3I storageMin, Vector3I storageMax, MyPlanet parent)
		{
			m_parent = parent;
			long num = storageMin.X;
			num = ((num * 397) ^ storageMin.Y);
			num = ((num * 397) ^ storageMin.Z);
			num = ((num * 397) ^ parent.EntityId);
			base.EntityId = MyEntityIdentifier.ConstructId(MyEntityIdentifier.ID_OBJECT_TYPE.VOXEL_PHYSICS, num & 0xFFFFFFFFFFFFFF);
			base.Init(null);
			InitVoxelMap(worldMatrix, base.Size, useOffset: false);
			Valid = true;
		}

		protected override void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
		{
			base.InitVoxelMap(worldMatrix, size, useOffset);
			Physics = new MyVoxelPhysicsBody(this, 1.5f, 7f);
			Physics.Enabled = true;
		}

		public void OnStorageChanged(Vector3I minChanged, Vector3I maxChanged, MyStorageDataTypeFlags dataChanged)
		{
			minChanged = Vector3I.Clamp(minChanged, m_storageMin, m_storageMax);
			maxChanged = Vector3I.Clamp(maxChanged, m_storageMin, m_storageMax);
			if ((dataChanged & MyStorageDataTypeFlags.Content) != 0 && Physics != null)
			{
				Physics.InvalidateRange(minChanged, maxChanged);
				RaisePhysicsChanged();
			}
		}

		public void RefreshPhysics(IMyStorage storage)
		{
			base.m_storage = storage;
			OnStorageChanged(m_storageMin, m_storageMax, MyStorageDataTypeFlags.Content);
			Valid = true;
		}

		protected override void BeforeDelete()
		{
			base.BeforeDelete();
			base.m_storage = null;
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (Physics != null)
			{
				Physics.UpdateBeforeSimulation10();
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (Physics != null)
			{
				Physics.UpdateAfterSimulation10();
			}
		}

		public override int GetOrePriority()
		{
			return 0;
		}

		public void PrefetchShapeOnRay(ref LineD ray)
		{
			if (Physics != null)
			{
				Physics.PrefetchShapeOnRay(ref ray);
			}
		}
	}
}
