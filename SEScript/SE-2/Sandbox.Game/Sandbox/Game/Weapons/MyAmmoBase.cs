using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public class MyAmmoBase : MyEntity
	{
		private class Sandbox_Game_Weapons_MyAmmoBase_003C_003EActor : IActivator, IActivator<MyAmmoBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyAmmoBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAmmoBase CreateInstance()
			{
				return new MyAmmoBase();
			}

			MyAmmoBase IActivator<MyAmmoBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected Vector3D m_origin;

		protected MyWeaponDefinition m_weaponDefinition;

		protected long m_originEntity;

		private float m_ammoOffsetSize;

		protected bool m_shouldExplode;

		protected bool m_markedToDestroy;

		protected bool m_physicsEnabled;

		public new MyPhysicsBody Physics => base.Physics as MyPhysicsBody;

		protected MyAmmoBase()
		{
			base.Save = false;
		}

		protected void Init(MyWeaponPropertiesWrapper weaponProperties, string modelName, bool spherePhysics = true, bool capsulePhysics = false, bool bulletType = false, bool physics = true)
		{
			bool allocationSuspended = MyEntityIdentifier.AllocationSuspended;
			MyEntityIdentifier.AllocationSuspended = true;
			base.Init(null, modelName, null, null);
			m_weaponDefinition = weaponProperties.WeaponDefinition;
			m_physicsEnabled = physics;
			if (physics)
			{
				if (spherePhysics)
				{
					this.InitSpherePhysics(MyMaterialType.AMMO, base.Model, 100f, MyPerGameSettings.DefaultLinearDamping, MyPerGameSettings.DefaultAngularDamping, 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
				}
				else if (capsulePhysics)
				{
					this.InitCapsulePhysics(MyMaterialType.AMMO, new Vector3(0f, 0f, (0f - base.Model.BoundingBox.HalfExtents.Z) * 0.8f), new Vector3(0f, 0f, base.Model.BoundingBox.HalfExtents.Z * 0.8f), 0.1f, 10f, 0f, 0f, 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
					m_ammoOffsetSize = base.Model.BoundingBox.HalfExtents.Z * 0.8f + 0.1f;
				}
				else
				{
					this.InitBoxPhysics(MyMaterialType.AMMO, base.Model, 1f, MyPerGameSettings.DefaultAngularDamping, 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
				}
				Physics.RigidBody.ContactPointCallbackEnabled = true;
				Physics.ContactPointCallback += OnContactPointCallback;
			}
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			base.Render.CastShadows = false;
			MyEntityIdentifier.AllocationSuspended = allocationSuspended;
		}

		protected void Start(Vector3D position, Vector3D initialVelocity, Vector3D direction)
		{
			m_shouldExplode = false;
			m_origin = position + direction * m_ammoOffsetSize;
			m_markedToDestroy = false;
			MatrixD worldMatrix = MatrixD.CreateWorld(m_origin, direction, Vector3D.CalculatePerpendicularVector(direction));
			base.PositionComp.SetWorldMatrix(worldMatrix);
			if (m_physicsEnabled)
			{
				Physics.Clear();
				Physics.Enabled = true;
				Physics.LinearVelocity = initialVelocity;
			}
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
		}

		public virtual void MarkForDestroy()
		{
			m_markedToDestroy = true;
			Close();
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_physicsEnabled)
			{
				Physics.ContactPointCallback -= OnContactPointCallback;
			}
		}

		private void OnContactPointCallback(ref MyPhysics.MyContactPointEvent value)
		{
			if (value.ContactPointEvent.EventType != HkContactPointEvent.Type.ManifoldAtEndOfStep)
			{
				OnContactStart(ref value);
			}
		}

		protected virtual void OnContactStart(ref MyPhysics.MyContactPointEvent value)
		{
		}
	}
}
