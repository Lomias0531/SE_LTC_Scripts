using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyGridWeaponSystem
	{
		public struct EventArgs
		{
			public IMyGunObject<MyDeviceBase> Weapon;
		}

		private Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> m_gunsByDefId;

		public event Action<MyGridWeaponSystem, EventArgs> WeaponRegistered;

		public event Action<MyGridWeaponSystem, EventArgs> WeaponUnregistered;

		static MyGridWeaponSystem()
		{
		}

		public MyGridWeaponSystem()
		{
			m_gunsByDefId = new Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>>(5, MyDefinitionId.Comparer);
		}

		public IMyGunObject<MyDeviceBase> GetGun(MyDefinitionId defId)
		{
			if (m_gunsByDefId.ContainsKey(defId))
			{
				return m_gunsByDefId[defId].FirstOrDefault();
			}
			return null;
		}

		public Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> GetGunSets()
		{
			return m_gunsByDefId;
		}

		public bool HasGunsOfId(MyDefinitionId defId)
		{
			if (m_gunsByDefId.ContainsKey(defId))
			{
				return m_gunsByDefId[defId].Count > 0;
			}
			return false;
		}

		internal void Register(IMyGunObject<MyDeviceBase> gun)
		{
			if (!m_gunsByDefId.ContainsKey(gun.DefinitionId))
			{
				m_gunsByDefId.Add(gun.DefinitionId, new HashSet<IMyGunObject<MyDeviceBase>>());
			}
			m_gunsByDefId[gun.DefinitionId].Add(gun);
			if (this.WeaponRegistered != null)
			{
				this.WeaponRegistered(this, new EventArgs
				{
					Weapon = gun
				});
			}
		}

		internal void Unregister(IMyGunObject<MyDeviceBase> gun)
		{
			if (!m_gunsByDefId.ContainsKey(gun.DefinitionId))
			{
				MyDebug.FailRelease(string.Concat("deinition ID ", gun.DefinitionId, " not in m_gunsByDefId"));
				return;
			}
			m_gunsByDefId[gun.DefinitionId].Remove(gun);
			if (this.WeaponUnregistered != null)
			{
				this.WeaponUnregistered(this, new EventArgs
				{
					Weapon = gun
				});
			}
		}

		public HashSet<IMyGunObject<MyDeviceBase>> GetGunsById(MyDefinitionId gunId)
		{
			if (m_gunsByDefId.ContainsKey(gunId))
			{
				return m_gunsByDefId[gunId];
			}
			return null;
		}

		internal IMyGunObject<MyDeviceBase> GetGunWithAmmo(MyDefinitionId gunId, long shooter)
		{
			if (!m_gunsByDefId.ContainsKey(gunId))
			{
				return null;
			}
			IMyGunObject<MyDeviceBase> result = m_gunsByDefId[gunId].FirstOrDefault();
			foreach (IMyGunObject<MyDeviceBase> item in m_gunsByDefId[gunId])
			{
				if (item.CanShoot(MyShootActionEnum.PrimaryAction, shooter, out MyGunStatusEnum _))
				{
					return item;
				}
			}
			return result;
		}
	}
}
