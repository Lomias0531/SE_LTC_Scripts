using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Network;

namespace Sandbox.Game.GameSystems
{
	public class MyGridSelectionSystem
	{
		private HashSet<IMyGunObject<MyDeviceBase>> m_currentGuns = new HashSet<IMyGunObject<MyDeviceBase>>();

		private MyDefinitionId? m_gunId;

		private bool m_useSingleGun;

		private MyShipController m_shipController;

		private MyGridWeaponSystem m_weaponSystem;

		private int m_gunTimer;

		private int m_gunTimer_Max;

		private static int PerFrameMax = 10;

		private int m_curentDrawHudIndex;

		public MyGridWeaponSystem WeaponSystem
		{
			get
			{
				return m_weaponSystem;
			}
			set
			{
				if (m_weaponSystem != value)
				{
					if (m_weaponSystem != null)
					{
						m_weaponSystem.WeaponRegistered -= WeaponSystem_WeaponRegistered;
						m_weaponSystem.WeaponUnregistered -= WeaponSystem_WeaponUnregistered;
					}
					m_weaponSystem = value;
					if (m_weaponSystem != null)
					{
						m_weaponSystem.WeaponRegistered += WeaponSystem_WeaponRegistered;
						m_weaponSystem.WeaponUnregistered += WeaponSystem_WeaponUnregistered;
					}
				}
			}
		}

		public MyGridSelectionSystem(MyShipController shipController)
		{
			m_shipController = shipController;
		}

		private void WeaponSystem_WeaponRegistered(MyGridWeaponSystem sender, MyGridWeaponSystem.EventArgs args)
		{
			if (m_shipController.Pilot == null)
			{
				return;
			}
			MyDefinitionId definitionId = args.Weapon.DefinitionId;
			MyDefinitionId? gunId = m_gunId;
			if (!(definitionId == gunId))
			{
				return;
			}
			if (m_useSingleGun)
			{
				if (m_currentGuns.Count < 1)
				{
					args.Weapon.OnControlAcquired(m_shipController.Pilot);
					m_currentGuns.Add(args.Weapon);
				}
			}
			else
			{
				args.Weapon.OnControlAcquired(m_shipController.Pilot);
				m_currentGuns.Add(args.Weapon);
			}
		}

		private void WeaponSystem_WeaponUnregistered(MyGridWeaponSystem sender, MyGridWeaponSystem.EventArgs args)
		{
			if (m_shipController.Pilot != null)
			{
				MyDefinitionId definitionId = args.Weapon.DefinitionId;
				MyDefinitionId? gunId = m_gunId;
				if (definitionId == gunId && m_currentGuns.Contains(args.Weapon))
				{
					args.Weapon.OnControlReleased();
					m_currentGuns.Remove(args.Weapon);
				}
			}
		}

		internal bool CanShoot(MyShootActionEnum action, out MyGunStatusEnum status, out IMyGunObject<MyDeviceBase> FailedGun)
		{
			FailedGun = null;
			if (m_currentGuns == null)
			{
				status = MyGunStatusEnum.NotSelected;
				return false;
			}
			bool flag = false;
			status = MyGunStatusEnum.OK;
			foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
			{
				flag |= currentGun.CanShoot(action, (m_shipController.ControllerInfo.Controller != null) ? m_shipController.ControllerInfo.Controller.Player.Identity.IdentityId : m_shipController.OwnerId, out MyGunStatusEnum status2);
				if (status2 != 0)
				{
					FailedGun = currentGun;
					status = status2;
				}
			}
			return flag;
		}

		internal void Shoot(MyShootActionEnum action)
		{
			foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
			{
				MyGunStatusEnum status;
				if (!currentGun.EnabledInWorldRules)
				{
					if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
					}
				}
				else if (currentGun.CanShoot(action, (m_shipController.ControllerInfo.Controller != null) ? m_shipController.ControllerInfo.ControllingIdentityId : m_shipController.OwnerId, out status))
				{
					currentGun.Shoot(action, ((MyEntity)currentGun).WorldMatrix.Forward, null);
				}
			}
		}

		internal void BeginShoot(MyShootActionEnum action)
		{
			foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
			{
				if (!currentGun.EnabledInWorldRules)
				{
					if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
					}
				}
				else
				{
					currentGun.BeginShoot(action);
				}
			}
		}

		internal void EndShoot(MyShootActionEnum action)
		{
			foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
			{
				if (!currentGun.EnabledInWorldRules)
				{
					if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
					}
				}
				else
				{
					currentGun.EndShoot(action);
				}
			}
		}

		public bool CanSwitchAmmoMagazine()
		{
			bool flag = true;
			if (m_currentGuns != null)
			{
				foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
				{
					if (currentGun.GunBase == null)
					{
						return false;
					}
					flag &= currentGun.GunBase.CanSwitchAmmoMagazine();
				}
				return flag;
			}
			return flag;
		}

		internal void SwitchAmmoMagazine()
		{
			foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
			{
				if (!currentGun.EnabledInWorldRules)
				{
					if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
					}
				}
				else
				{
					currentGun.GunBase.SwitchToNextAmmoMagazine();
				}
			}
		}

		internal void SwitchTo(MyDefinitionId? gunId, bool useSingle = false)
		{
			m_gunId = gunId;
			m_useSingleGun = useSingle;
			if (m_currentGuns != null)
			{
				foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
				{
					currentGun.OnControlReleased();
				}
			}
			if (!gunId.HasValue)
			{
				m_currentGuns.Clear();
				return;
			}
			m_currentGuns.Clear();
			if (useSingle)
			{
				IMyGunObject<MyDeviceBase> gunWithAmmo = WeaponSystem.GetGunWithAmmo(gunId.Value, m_shipController.OwnerId);
				if (gunWithAmmo != null)
				{
					m_currentGuns.Add(gunWithAmmo);
				}
			}
			else
			{
				HashSet<IMyGunObject<MyDeviceBase>> gunsById = WeaponSystem.GetGunsById(gunId.Value);
				if (gunsById != null)
				{
					foreach (IMyGunObject<MyDeviceBase> item in gunsById)
					{
						if (item != null)
						{
							m_currentGuns.Add(item);
						}
					}
				}
			}
			foreach (IMyGunObject<MyDeviceBase> currentGun2 in m_currentGuns)
			{
				currentGun2.OnControlAcquired(m_shipController.Pilot);
			}
		}

		public MyDefinitionId? GetGunId()
		{
			return m_gunId;
		}

		internal void DrawHud(IMyCameraController camera, long playerId)
		{
			if (m_currentGuns != null)
			{
				if (m_gunTimer <= 0)
				{
					m_gunTimer_Max = m_currentGuns.Count / PerFrameMax + 1;
					m_gunTimer = m_gunTimer_Max;
				}
				m_gunTimer--;
				foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
				{
					currentGun.DrawHud(camera, playerId, (currentGun.GetHashCode() + (int)MySandboxGame.Static.SimulationFrameCounter) % m_gunTimer_Max == 0);
				}
			}
		}

		internal void OnControlAcquired()
		{
			if (m_currentGuns != null)
			{
				SwitchTo(m_gunId, m_useSingleGun);
				foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
				{
					currentGun.OnControlAcquired(m_shipController.Pilot);
				}
			}
		}

		internal void OnControlReleased()
		{
			if (m_currentGuns != null)
			{
				foreach (IMyGunObject<MyDeviceBase> currentGun in m_currentGuns)
				{
					currentGun.OnControlReleased();
				}
			}
		}
	}
}
