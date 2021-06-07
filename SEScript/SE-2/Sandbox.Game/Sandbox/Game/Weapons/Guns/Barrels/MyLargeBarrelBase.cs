using Sandbox.Definitions;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons.Guns.Barrels
{
	public abstract class MyLargeBarrelBase
	{
		protected MyGunBase m_gunBase;

		protected Matrix m_renderLocal;

		protected int m_lastTimeShoot;

		private int m_lastTimeSmooke;

		protected int m_lateStartRandom;

		protected int m_currentLateStart;

		private float m_barrelElevationMin;

		private float m_barrelSinElevationMin;

		protected MyParticleEffect m_shotSmoke;

		protected MyParticleEffect m_muzzleFlash;

		protected bool m_dontTimeOffsetNextShot;

		protected int m_smokeLastTime;

		protected int m_smokeToGenerate;

		protected float m_muzzleFlashLength;

		protected float m_muzzleFlashRadius;

		protected MyEntity m_entity;

		protected MyLargeTurretBase m_turretBase;

		public MyGunBase GunBase => m_gunBase;

		public MyModelDummy CameraDummy
		{
			get;
			private set;
		}

		public int LateTimeRandom
		{
			get
			{
				return m_lateStartRandom;
			}
			set
			{
				m_lateStartRandom = value;
			}
		}

		public float BarrelElevationMin
		{
			get
			{
				return m_barrelElevationMin;
			}
			protected set
			{
				m_barrelElevationMin = value;
				m_barrelSinElevationMin = (float)Math.Sin(m_barrelSinElevationMin);
			}
		}

		public float BarrelSinElevationMin => m_barrelSinElevationMin;

		public bool NeedsPerFrameUpdate => m_smokeToGenerate > 0;

		public MyEntity Entity => m_entity;

		public void ResetCurrentLateStart()
		{
			m_currentLateStart = 0;
		}

		public void DontTimeOffsetNextShot()
		{
			m_dontTimeOffsetNextShot = true;
		}

		public MyLargeBarrelBase()
		{
			m_lastTimeShoot = 0;
			m_lastTimeSmooke = 0;
			BarrelElevationMin = -0.6f;
		}

		public virtual void Draw()
		{
		}

		public virtual void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			m_entity = entity;
			m_turretBase = turretBase;
			m_gunBase = turretBase.GunBase;
			m_lateStartRandom = turretBase.LateStartRandom;
			if (m_entity.Model != null)
			{
				if (m_entity.Model.Dummies.ContainsKey("camera"))
				{
					CameraDummy = m_entity.Model.Dummies["camera"];
				}
				m_gunBase.LoadDummies(m_entity.Model.Dummies);
			}
			m_entity.OnClose += m_entity_OnClose;
		}

		private void m_entity_OnClose(MyEntity obj)
		{
			if (m_shotSmoke != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_shotSmoke);
				m_shotSmoke = null;
			}
			if (m_muzzleFlash != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_muzzleFlash);
				m_muzzleFlash = null;
			}
		}

		public virtual bool StartShooting()
		{
			m_turretBase.Render.NeedsDrawFromParent = true;
			return true;
		}

		public virtual void StopShooting()
		{
			m_turretBase.Render.NeedsDrawFromParent = false;
			GetWeaponBase().StopShootingSound();
		}

		protected MyLargeTurretBase GetWeaponBase()
		{
			return m_turretBase;
		}

		protected void Shoot(Vector3 muzzlePosition)
		{
			if (m_turretBase.Parent.Physics != null)
			{
				_ = (Vector3)m_entity.WorldMatrix.Forward;
				Vector3 linearVelocity = m_turretBase.Parent.Physics.LinearVelocity;
				GetWeaponBase().RemoveAmmoPerShot();
				m_gunBase.Shoot(linearVelocity);
			}
		}

		private void DrawCrossHair()
		{
		}

		public bool IsControlledByPlayer()
		{
			return MySession.Static.ControlledEntity == this;
		}

		protected void IncreaseSmoke()
		{
			m_smokeToGenerate += 19;
			m_smokeToGenerate = MyUtils.GetClampInt(m_smokeToGenerate, 0, 50);
		}

		protected void DecreaseSmoke()
		{
			m_smokeToGenerate--;
			m_smokeToGenerate = MyUtils.GetClampInt(m_smokeToGenerate, 0, 50);
		}

		public virtual void UpdateAfterSimulation()
		{
			DecreaseSmoke();
		}

		public void RemoveSmoke()
		{
			m_smokeToGenerate = 0;
		}

		public virtual void Close()
		{
		}

		public void WorldPositionChanged()
		{
			WorldPositionChanged(ref m_renderLocal);
		}

		public void WorldPositionChanged(ref Matrix renderLocal)
		{
			m_gunBase.WorldMatrix = Entity.PositionComp.WorldMatrix;
			m_renderLocal = renderLocal;
		}

		public void ShootEffect()
		{
			m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
		}
	}
}
