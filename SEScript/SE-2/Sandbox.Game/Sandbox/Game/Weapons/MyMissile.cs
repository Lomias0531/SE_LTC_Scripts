using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_Missile), true)]
	public sealed class MyMissile : MyAmmoBase, IMyEventProxy, IMyEventOwner, IMyDestroyableObject
	{
		private class Sandbox_Game_Weapons_MyMissile_003C_003EActor : IActivator, IActivator<MyMissile>
		{
			private sealed override object CreateInstance()
			{
				return new MyMissile();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMissile CreateInstance()
			{
				return new MyMissile();
			}

			MyMissile IActivator<MyMissile>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyMissileAmmoDefinition m_missileAmmoDefinition;

		private float m_maxTrajectory;

		private MyParticleEffect m_smokeEffect;

		private MyExplosionTypeEnum m_explosionType;

		private MyEntity m_collidedEntity;

		private Vector3D? m_collisionPoint;

		private Vector3 m_collisionNormal;

		private long m_owner;

		private readonly float m_smokeEffectOffsetMultiplier = 0.4f;

		private Vector3 m_linearVelocity;

		private MyWeaponPropertiesWrapper m_weaponProperties;

		private long m_launcherId;

		public static bool DEBUG_DRAW_MISSILE_TRAJECTORY;

		internal int m_pruningProxyId = -1;

		private readonly MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_removed;

		public SerializableDefinitionId AmmoMagazineId => m_weaponProperties.AmmoMagazineId;

		public SerializableDefinitionId WeaponDefinitionId => m_weaponProperties.WeaponDefinitionId;

		private bool UseDamageSystem
		{
			get;
			set;
		}

		public long Owner => m_owner;

		float IMyDestroyableObject.Integrity => 1f;

		bool IMyDestroyableObject.UseDamageSystem => UseDamageSystem;

		public MyMissile()
		{
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
			if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
			{
				Func<bool> entity = () => MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity is MyCharacter && MySession.Static.ControlledEntity.Entity == m_collidedEntity;
				m_soundEmitter.EmitterMethods[1].Add(entity);
				m_soundEmitter.EmitterMethods[0].Add(entity);
			}
			base.Flags |= EntityFlags.IsNotGamePrunningStructureObject;
			if (Sync.IsDedicated)
			{
				base.Flags &= ~EntityFlags.UpdateRender;
				base.InvalidateOnMove = false;
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyObjectBuilder_Missile myObjectBuilder_Missile = (MyObjectBuilder_Missile)objectBuilder;
			base.Init(objectBuilder);
			m_weaponProperties = new MyWeaponPropertiesWrapper(myObjectBuilder_Missile.WeaponDefinitionId);
			m_weaponProperties.ChangeAmmoMagazine(myObjectBuilder_Missile.AmmoMagazineId);
			m_missileAmmoDefinition = m_weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
			Init(m_weaponProperties, m_missileAmmoDefinition.MissileModelName, spherePhysics: false, capsulePhysics: true, bulletType: true, Sync.IsServer);
			UseDamageSystem = true;
			m_maxTrajectory = m_missileAmmoDefinition.MaxTrajectory;
			base.SyncFlag = true;
			m_collisionPoint = null;
			m_owner = myObjectBuilder_Missile.Owner;
			m_originEntity = myObjectBuilder_Missile.OriginEntity;
			m_linearVelocity = myObjectBuilder_Missile.LinearVelocity;
			m_launcherId = myObjectBuilder_Missile.LauncherId;
			base.OnPhysicsChanged += OnMissilePhysicsChanged;
		}

		private void OnMissilePhysicsChanged(MyEntity entity)
		{
			if (base.Physics != null && base.Physics.RigidBody != null)
			{
				base.Physics.RigidBody.CallbackLimit = 1;
			}
		}

		public void UpdateData(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyObjectBuilder_Missile myObjectBuilder_Missile = (MyObjectBuilder_Missile)objectBuilder;
			if (objectBuilder.PositionAndOrientation.HasValue)
			{
				MyPositionAndOrientation value = objectBuilder.PositionAndOrientation.Value;
				MatrixD worldMatrix = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
				base.PositionComp.SetWorldMatrix(worldMatrix, null, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: false, forceUpdateAllChildren: false, ignoreAssert: true);
			}
			base.EntityId = myObjectBuilder_Missile.EntityId;
			m_owner = myObjectBuilder_Missile.Owner;
			m_originEntity = myObjectBuilder_Missile.OriginEntity;
			m_linearVelocity = myObjectBuilder_Missile.LinearVelocity;
			m_launcherId = myObjectBuilder_Missile.LauncherId;
			m_collisionPoint = null;
			m_markedToDestroy = false;
			m_removed = false;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_Missile obj = (MyObjectBuilder_Missile)base.GetObjectBuilder(copy);
			obj.LinearVelocity = base.Physics.LinearVelocity;
			obj.AmmoMagazineId = m_weaponProperties.AmmoMagazineId;
			obj.WeaponDefinitionId = m_weaponProperties.WeaponDefinitionId;
			obj.Owner = m_owner;
			obj.OriginEntity = m_originEntity;
			obj.LauncherId = m_launcherId;
			return obj;
		}

		public static MyObjectBuilder_Missile PrepareBuilder(MyWeaponPropertiesWrapper weaponProperties, Vector3D position, Vector3D initialVelocity, Vector3D direction, long owner, long originEntity, long launcherId)
		{
			MyObjectBuilder_Missile myObjectBuilder_Missile = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Missile>();
			myObjectBuilder_Missile.LinearVelocity = initialVelocity;
			myObjectBuilder_Missile.AmmoMagazineId = weaponProperties.AmmoMagazineId;
			myObjectBuilder_Missile.WeaponDefinitionId = weaponProperties.WeaponDefinitionId;
			myObjectBuilder_Missile.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			Vector3D vector3D = position + direction * 4.0;
			if (!MyPhysics.CastRay(position, vector3D).HasValue)
			{
				position = vector3D;
			}
			myObjectBuilder_Missile.PositionAndOrientation = new MyPositionAndOrientation(position, direction, Vector3D.CalculatePerpendicularVector(direction));
			myObjectBuilder_Missile.Owner = owner;
			myObjectBuilder_Missile.OriginEntity = originEntity;
			myObjectBuilder_Missile.LauncherId = launcherId;
			myObjectBuilder_Missile.EntityId = MyEntityIdentifier.AllocateId();
			return myObjectBuilder_Missile;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			m_shouldExplode = false;
			Start(base.PositionComp.GetPosition(), m_linearVelocity, base.WorldMatrix.Forward);
			if (m_physicsEnabled)
			{
				base.Physics.RigidBody.MaxLinearVelocity = m_missileAmmoDefinition.DesiredSpeed;
				base.Physics.RigidBody.Layer = 8;
				base.Physics.CanUpdateAccelerations = false;
			}
			m_explosionType = MyExplosionTypeEnum.MISSILE_EXPLOSION;
			if (!Sync.IsDedicated)
			{
				MySoundPair shootSound = m_weaponDefinition.WeaponAmmoDatas[1].ShootSound;
				if (shootSound != null)
				{
					m_soundEmitter.PlaySingleSound(shootSound, stopPrevious: true);
				}
				MatrixD worldMatrix = base.PositionComp.WorldMatrix;
				worldMatrix.Translation -= worldMatrix.Forward * m_smokeEffectOffsetMultiplier;
				Vector3D worldPosition = worldMatrix.Translation;
				MyParticlesManager.TryCreateParticleEffect("Smoke_Missile", ref MatrixD.Identity, ref worldPosition, base.Render.GetRenderObjectID(), out m_smokeEffect);
				(MyEntities.GetEntityById(m_launcherId) as IMyMissileGunObject)?.MissileShootEffect();
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (m_smokeEffect != null)
			{
				m_smokeEffect.Stop(instant: false);
				m_smokeEffect = null;
			}
			m_soundEmitter.StopSound(forced: true);
		}

		public override void UpdateBeforeSimulation()
		{
			if (m_shouldExplode)
			{
				ExecuteExplosion();
				return;
			}
			base.UpdateBeforeSimulation();
			Vector3D position = base.PositionComp.GetPosition();
			if (m_physicsEnabled)
			{
				m_linearVelocity = base.Physics.LinearVelocity;
				base.Physics.AngularVelocity = Vector3.Zero;
			}
			if (m_missileAmmoDefinition.MissileSkipAcceleration)
			{
				m_linearVelocity = base.WorldMatrix.Forward * m_missileAmmoDefinition.DesiredSpeed;
			}
			else
			{
				m_linearVelocity += base.PositionComp.WorldMatrix.Forward * m_missileAmmoDefinition.MissileAcceleration * 0.01666666753590107;
			}
			if (m_physicsEnabled)
			{
				base.Physics.LinearVelocity = m_linearVelocity;
			}
			else
			{
				Vector3.ClampToSphere(ref m_linearVelocity, m_missileAmmoDefinition.DesiredSpeed);
				base.PositionComp.SetPosition(base.PositionComp.GetPosition() + m_linearVelocity * 0.0166666675f);
			}
			if (Vector3.DistanceSquared(base.PositionComp.GetPosition(), m_origin) >= m_maxTrajectory * m_maxTrajectory)
			{
				MarkForExplosion();
			}
			if (DEBUG_DRAW_MISSILE_TRAJECTORY)
			{
				MyRenderProxy.DebugDrawLine3D(position, base.PositionComp.GetPosition(), Color.AliceBlue, Color.AliceBlue, depthRead: true);
			}
			MyMissiles.OnMissileMoved(this, ref m_linearVelocity);
		}

		private void ExecuteExplosion()
		{
			if (!Sync.IsServer)
			{
				Return();
				return;
			}
			PlaceDecal();
			float missileExplosionRadius = m_missileAmmoDefinition.MissileExplosionRadius;
			BoundingSphereD explosionSphere = new BoundingSphereD(base.PositionComp.GetPosition(), missileExplosionRadius);
			MyEntity myEntity = null;
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(m_owner);
			if (myIdentity != null)
			{
				myEntity = myIdentity.Character;
			}
			else
			{
				MyEntity entity = null;
				MyEntities.TryGetEntityById(m_owner, out entity);
				myEntity = entity;
			}
			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = 0f;
			myExplosionInfo.Damage = m_missileAmmoDefinition.MissileExplosionDamage;
			myExplosionInfo.ExplosionType = m_explosionType;
			myExplosionInfo.ExplosionSphere = explosionSphere;
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.HitEntity = m_collidedEntity;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.OwnerEntity = myEntity;
			myExplosionInfo.Direction = Vector3.Normalize(base.PositionComp.GetPosition() - m_origin);
			myExplosionInfo.VoxelExplosionCenter = explosionSphere.Center + missileExplosionRadius * base.WorldMatrix.Forward * 0.25;
			myExplosionInfo.ExplosionFlags = (MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS);
			myExplosionInfo.VoxelCutoutScale = 0.3f;
			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;
			myExplosionInfo.OriginEntity = m_originEntity;
			myExplosionInfo.KeepAffectedBlocks = true;
			MyExplosionInfo explosionInfo = myExplosionInfo;
			if (m_collidedEntity != null && m_collidedEntity.Physics != null)
			{
				explosionInfo.Velocity = m_collidedEntity.Physics.LinearVelocity;
			}
			if (!m_markedToDestroy)
			{
				explosionInfo.ExplosionFlags |= MyExplosionFlags.CREATE_PARTICLE_EFFECT;
			}
			MyExplosions.AddExplosion(ref explosionInfo);
			if (m_collidedEntity != null && !(m_collidedEntity is MyAmmoBase) && m_collidedEntity.Physics != null && !m_collidedEntity.Physics.IsStatic)
			{
				m_collidedEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, 100f * base.Physics.LinearVelocity, m_collisionPoint, null);
			}
			Return();
		}

		private void Done()
		{
			if (m_collidedEntity != null)
			{
				m_collidedEntity.Unpin();
				m_collidedEntity = null;
			}
		}

		private void Return()
		{
			Done();
			MyMissiles.Return(this);
		}

		private void PlaceDecal()
		{
			if (m_collidedEntity != null && m_collisionPoint.HasValue)
			{
				MyHitInfo myHitInfo = default(MyHitInfo);
				myHitInfo.Position = m_collisionPoint.Value;
				myHitInfo.Normal = m_collisionNormal;
				MyHitInfo hitInfo = myHitInfo;
				MyDecals.HandleAddDecal(m_collidedEntity, hitInfo, m_missileAmmoDefinition.PhysicalMaterial);
			}
		}

		private void MarkForExplosion()
		{
			if (m_markedToDestroy)
			{
				Return();
			}
			else
			{
				m_shouldExplode = true;
			}
			if (Sync.IsServer && !m_removed)
			{
				(MyEntities.GetEntityById(m_launcherId) as IMyMissileGunObject)?.RemoveMissile(base.EntityId);
				m_removed = true;
			}
		}

		public override void MarkForDestroy()
		{
			Return();
		}

		protected override void Closing()
		{
			base.Closing();
			Done();
		}

		protected override void OnContactStart(ref MyPhysics.MyContactPointEvent value)
		{
			if (base.MarkedForClose || m_collidedEntity != null)
			{
				return;
			}
			MyEntity myEntity = value.ContactPointEvent.GetOtherEntity(this) as MyEntity;
			if (myEntity != null)
			{
				myEntity.Pin();
				m_collidedEntity = myEntity;
				m_collisionPoint = value.Position;
				m_collisionNormal = value.Normal;
				if (!Sync.IsServer)
				{
					PlaceDecal();
				}
				else
				{
					MarkForExplosion();
				}
			}
		}

		private void DoDamage(float damage, MyStringHash damageType, bool sync, long attackerId)
		{
			if (sync)
			{
				if (Sync.IsServer)
				{
					MySyncDamage.DoDamageSynced(this, damage, damageType, attackerId);
				}
				return;
			}
			if (UseDamageSystem)
			{
				MyDamageSystem.Static.RaiseDestroyed(this, new MyDamageInformation(isDeformation: false, damage, damageType, attackerId));
			}
			MarkForExplosion();
		}

		void IMyDestroyableObject.OnDestroy()
		{
		}

		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			DoDamage(damage, damageType, sync, attackerId);
			return true;
		}
	}
}
