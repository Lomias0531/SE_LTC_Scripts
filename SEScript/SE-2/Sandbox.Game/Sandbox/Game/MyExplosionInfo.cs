using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game
{
	public struct MyExplosionInfo
	{
		public float PlayerDamage;

		public float Damage;

		public BoundingSphereD ExplosionSphere;

		public float StrengthImpulse;

		public float StrengthAngularImpulse;

		public MyEntity ExcludedEntity;

		public MyEntity OwnerEntity;

		public MyEntity HitEntity;

		public MyExplosionFlags ExplosionFlags;

		public MyExplosionTypeEnum ExplosionType;

		public int LifespanMiliseconds;

		public int ObjectsRemoveDelayInMiliseconds;

		public float ParticleScale;

		public float VoxelCutoutScale;

		public Vector3? Direction;

		public Vector3D VoxelExplosionCenter;

		public bool PlaySound;

		public bool CheckIntersections;

		public Vector3 Velocity;

		public long OriginEntity;

		public string CustomEffect;

		public MySoundPair CustomSound;

		public bool KeepAffectedBlocks;

		public bool AffectVoxels
		{
			get
			{
				return HasFlag(MyExplosionFlags.AFFECT_VOXELS);
			}
			set
			{
				SetFlag(MyExplosionFlags.AFFECT_VOXELS, value);
			}
		}

		public bool CreateDebris
		{
			get
			{
				return HasFlag(MyExplosionFlags.CREATE_DEBRIS);
			}
			set
			{
				SetFlag(MyExplosionFlags.CREATE_DEBRIS, value);
			}
		}

		public bool CreateParticleDebris
		{
			get
			{
				return HasFlag(MyExplosionFlags.CREATE_PARTICLE_DEBRIS);
			}
			set
			{
				SetFlag(MyExplosionFlags.CREATE_PARTICLE_DEBRIS, value);
			}
		}

		public bool ApplyForceAndDamage
		{
			get
			{
				return HasFlag(MyExplosionFlags.APPLY_FORCE_AND_DAMAGE);
			}
			set
			{
				SetFlag(MyExplosionFlags.APPLY_FORCE_AND_DAMAGE, value);
			}
		}

		public bool CreateDecals
		{
			get
			{
				return HasFlag(MyExplosionFlags.CREATE_DECALS);
			}
			set
			{
				SetFlag(MyExplosionFlags.CREATE_DECALS, value);
			}
		}

		public bool ForceDebris
		{
			get
			{
				return HasFlag(MyExplosionFlags.FORCE_DEBRIS);
			}
			set
			{
				SetFlag(MyExplosionFlags.FORCE_DEBRIS, value);
			}
		}

		public bool CreateParticleEffect
		{
			get
			{
				return HasFlag(MyExplosionFlags.CREATE_PARTICLE_EFFECT);
			}
			set
			{
				SetFlag(MyExplosionFlags.CREATE_PARTICLE_EFFECT, value);
			}
		}

		public bool CreateShrapnels
		{
			get
			{
				return HasFlag(MyExplosionFlags.CREATE_SHRAPNELS);
			}
			set
			{
				SetFlag(MyExplosionFlags.CREATE_SHRAPNELS, value);
			}
		}

		public MyExplosionInfo(float playerDamage, float damage, BoundingSphereD explosionSphere, MyExplosionTypeEnum type, bool playSound, bool checkIntersection = true)
		{
			PlayerDamage = playerDamage;
			Damage = damage;
			ExplosionSphere = explosionSphere;
			StrengthImpulse = (StrengthAngularImpulse = 0f);
			ExcludedEntity = (OwnerEntity = (HitEntity = null));
			ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.APPLY_DEFORMATION);
			ExplosionType = type;
			LifespanMiliseconds = 700;
			ObjectsRemoveDelayInMiliseconds = 0;
			ParticleScale = 1f;
			VoxelCutoutScale = 1f;
			Direction = null;
			VoxelExplosionCenter = explosionSphere.Center;
			PlaySound = playSound;
			CheckIntersections = checkIntersection;
			Velocity = Vector3.Zero;
			OriginEntity = 0L;
			CustomEffect = "";
			CustomSound = null;
			KeepAffectedBlocks = false;
		}

		private void SetFlag(MyExplosionFlags flag, bool value)
		{
			if (value)
			{
				ExplosionFlags |= flag;
			}
			else
			{
				ExplosionFlags &= ~flag;
			}
		}

		private bool HasFlag(MyExplosionFlags flag)
		{
			return (ExplosionFlags & flag) == flag;
		}
	}
}
