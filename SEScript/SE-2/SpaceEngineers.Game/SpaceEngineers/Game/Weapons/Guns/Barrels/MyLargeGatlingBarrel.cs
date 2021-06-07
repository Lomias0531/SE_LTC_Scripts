using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
	internal class MyLargeGatlingBarrel : MyLargeBarrelBase
	{
		private Vector3D m_muzzleFlashPosition;

		private int m_nextNotificationTime;

		private MyHudNotification m_reloadNotification;

		private float m_rotationTimeout;

		private int m_shotsLeftInBurst;

		private int m_reloadCompletionTime;

		public int ShotsInBurst => m_gunBase.ShotsInBurst;

		public MyLargeGatlingBarrel()
		{
			m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);
		}

		public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			base.Init(entity, turretBase);
			m_shotsLeftInBurst = ShotsInBurst;
			if (!m_gunBase.HasDummies)
			{
				Vector3 position = 2.0 * entity.PositionComp.WorldMatrix.Forward;
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation(position));
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
			amount = MathHelper.SmoothStep(0f, 1f, amount);
			float num = amount * (MathF.PI * 4f) * 0.0166666675f;
			if (num != 0f)
			{
				base.Entity.PositionComp.LocalMatrix = Matrix.CreateRotationZ(num) * base.Entity.PositionComp.LocalMatrix;
			}
			if (m_shotSmoke != null)
			{
				m_shotSmoke.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				if (m_smokeToGenerate > 0)
				{
					m_shotSmoke.UserBirthMultiplier = m_smokeToGenerate;
				}
				else
				{
					m_shotSmoke.Stop(instant: false);
					m_shotSmoke = null;
				}
			}
			if (m_muzzleFlash != null)
			{
				if (m_smokeToGenerate == 0)
				{
					m_muzzleFlash.Stop();
					m_muzzleFlash = null;
				}
				else
				{
					m_muzzleFlash.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				}
			}
			UpdateReloadNotification();
		}

		public override void Draw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawLine3D(m_entity.PositionComp.GetPosition(), m_entity.PositionComp.GetPosition() + m_entity.WorldMatrix.Forward, Color.Green, Color.GreenYellow, depthRead: false);
				if (GetWeaponBase().Target != null)
				{
					MyRenderProxy.DebugDrawSphere(GetWeaponBase().Target.PositionComp.GetPosition(), 0.4f, Color.Green, 1f, depthRead: false);
				}
			}
		}

		public override bool StartShooting()
		{
			if (m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				return false;
			}
			if (!base.StartShooting())
			{
				return false;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
			{
				return false;
			}
			if (m_turretBase != null && !m_turretBase.IsTargetVisible())
			{
				return false;
			}
			if (m_lateStartRandom > m_currentLateStart && !m_dontTimeOffsetNextShot)
			{
				m_currentLateStart++;
				return false;
			}
			m_dontTimeOffsetNextShot = false;
			if (m_shotsLeftInBurst <= 0 && ShotsInBurst != 0)
			{
				return false;
			}
			m_muzzleFlashLength = MyUtils.GetRandomFloat(4f, 6f);
			m_muzzleFlashRadius = MyUtils.GetRandomFloat(1.2f, 2f);
			if (m_turretBase.IsControlledByLocalPlayer)
			{
				m_muzzleFlashRadius *= 0.33f;
			}
			IncreaseSmoke();
			m_muzzleFlashPosition = m_gunBase.GetMuzzleWorldPosition();
			if (m_shotSmoke == null)
			{
				if (m_smokeToGenerate > 0)
				{
					MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", MatrixD.CreateTranslation(m_muzzleFlashPosition), out m_shotSmoke);
				}
			}
			else if (m_shotSmoke.IsEmittingStopped)
			{
				m_shotSmoke.Play();
			}
			if (m_muzzleFlash == null)
			{
				MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", MatrixD.CreateTranslation(m_muzzleFlashPosition), out m_muzzleFlash);
			}
			if (m_shotSmoke != null)
			{
				m_shotSmoke.WorldMatrix = MatrixD.CreateTranslation(m_muzzleFlashPosition);
			}
			if (m_muzzleFlash != null)
			{
				m_muzzleFlash.WorldMatrix = MatrixD.CreateTranslation(m_muzzleFlashPosition);
			}
			GetWeaponBase().PlayShootingSound();
			Shoot(base.Entity.PositionComp.GetPosition());
			if (ShotsInBurst > 0)
			{
				m_shotsLeftInBurst--;
				if (m_shotsLeftInBurst <= 0)
				{
					m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
					m_turretBase.OnReloadStarted(m_gunBase.ReloadTime);
					m_shotsLeftInBurst = ShotsInBurst;
					if (m_muzzleFlash != null)
					{
						m_muzzleFlash.Stop();
						m_muzzleFlash = null;
					}
					m_currentLateStart = 0;
				}
			}
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			return true;
		}

		public override void StopShooting()
		{
			base.StopShooting();
			m_currentLateStart = 0;
			if (m_muzzleFlash != null)
			{
				m_muzzleFlash.Stop();
				m_muzzleFlash = null;
			}
		}

		public override void Close()
		{
			if (m_shotSmoke != null)
			{
				m_shotSmoke.Stop();
				m_shotSmoke = null;
			}
			if (m_muzzleFlash != null)
			{
				m_muzzleFlash.Stop();
				m_muzzleFlash = null;
			}
		}

		private void UpdateReloadNotification()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds > m_nextNotificationTime)
			{
				m_reloadNotification = null;
			}
			if (!m_gunBase.HasEnoughAmmunition() && MySession.Static.SurvivalMode)
			{
				MyHud.Notifications.Remove(m_reloadNotification);
				m_reloadNotification = null;
			}
			else if (!m_turretBase.IsControlledByLocalPlayer)
			{
				if (m_reloadNotification != null)
				{
					MyHud.Notifications.Remove(m_reloadNotification);
					m_reloadNotification = null;
				}
			}
			else if (m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				ShowReloadNotification(m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

		/// <summary>
		/// Will show the reload notification for the specified duration.
		/// </summary>
		/// <param name="duration">The time in MS it should show reloading.</param>
		private void ShowReloadNotification(int duration)
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
			if (m_reloadNotification == null)
			{
				duration = Math.Max(0, duration - 250);
				if (duration != 0)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					MyHud.Notifications.Add(m_reloadNotification);
					m_nextNotificationTime = num;
				}
			}
			else
			{
				int timeStep = num - m_nextNotificationTime;
				m_reloadNotification.AddAliveTime(timeStep);
				m_nextNotificationTime = num;
			}
		}
	}
}
