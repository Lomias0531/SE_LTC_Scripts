using Sandbox;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
	internal class MyLargeMissileBarrel : MyLargeBarrelBase
	{
		private int m_reloadCompletionTime;

		private int m_nextShootTime;

		private int m_shotsLeftInBurst;

		private int m_nextNotificationTime;

		private MyHudNotification m_reloadNotification;

		private MyEntity3DSoundEmitter m_soundEmitter;

		public int ShotsInBurst => m_gunBase.ShotsInBurst;

		public MyLargeMissileBarrel()
		{
			m_soundEmitter = new MyEntity3DSoundEmitter(m_entity);
		}

		public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			base.Init(entity, turretBase);
			if (!m_gunBase.HasDummies)
			{
				Matrix identity = Matrix.Identity;
				identity.Translation += entity.PositionComp.WorldMatrix.Forward * 3.0;
				m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, identity);
			}
			m_shotsLeftInBurst = ShotsInBurst;
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Entity = turretBase;
			}
		}

		public void Init(Matrix localMatrix, MyLargeTurretBase parentObject)
		{
			m_shotsLeftInBurst = ShotsInBurst;
		}

		public override bool StartShooting()
		{
			if (m_turretBase == null || m_turretBase.Parent == null || m_turretBase.Parent.Physics == null)
			{
				return false;
			}
			bool dontTimeOffsetNextShot = m_dontTimeOffsetNextShot;
			if (Sync.IsServer)
			{
				if (m_lateStartRandom > m_currentLateStart && !m_dontTimeOffsetNextShot && !m_turretBase.IsControlled)
				{
					m_currentLateStart++;
					return false;
				}
				m_dontTimeOffsetNextShot = false;
			}
			if (m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				return false;
			}
			if (m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				return false;
			}
			if ((m_shotsLeftInBurst > 0 || ShotsInBurst == 0) && (m_turretBase.Target != null || m_turretBase.IsControlled || dontTimeOffsetNextShot))
			{
				if (Sync.IsServer)
				{
					GetWeaponBase().RemoveAmmoPerShot();
				}
				m_gunBase.Shoot(m_turretBase.Parent.Physics.LinearVelocity);
				m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ShootIntervalInMiliseconds;
				if (ShotsInBurst > 0)
				{
					m_shotsLeftInBurst--;
					if (m_shotsLeftInBurst <= 0)
					{
						m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
						m_turretBase.OnReloadStarted(m_gunBase.ReloadTime);
						m_shotsLeftInBurst = ShotsInBurst;
					}
				}
			}
			return true;
		}

		public override void StopShooting()
		{
			base.StopShooting();
			m_currentLateStart = 0;
			m_soundEmitter.StopSound(forced: true);
		}

		private void StartSound()
		{
			m_gunBase.StartShootSound(m_soundEmitter);
		}

		public override void Close()
		{
			base.Close();
			m_soundEmitter.StopSound(forced: true);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			UpdateReloadNotification();
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
