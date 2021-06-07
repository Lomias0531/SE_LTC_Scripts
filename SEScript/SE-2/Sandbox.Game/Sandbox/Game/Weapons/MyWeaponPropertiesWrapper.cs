using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;

namespace Sandbox.Game.Weapons
{
	public class MyWeaponPropertiesWrapper
	{
		private MyWeaponDefinition m_weaponDefinition;

		private MyAmmoDefinition m_ammoDefinition;

		private MyAmmoMagazineDefinition m_ammoMagazineDefinition;

		public MyDefinitionId WeaponDefinitionId
		{
			get;
			private set;
		}

		public MyDefinitionId AmmoMagazineId
		{
			get;
			private set;
		}

		public MyDefinitionId AmmoDefinitionId
		{
			get;
			private set;
		}

		public MyAmmoDefinition AmmoDefinition => m_ammoDefinition;

		public MyWeaponDefinition WeaponDefinition => m_weaponDefinition;

		public MyAmmoMagazineDefinition AmmoMagazineDefinition => m_ammoMagazineDefinition;

		public int AmmoMagazinesCount => WeaponDefinition.AmmoMagazinesId.Length;

		public bool IsAmmoProjectile => AmmoDefinition.AmmoType == MyAmmoType.HighSpeed;

		public bool IsAmmoMissile => AmmoDefinition.AmmoType == MyAmmoType.Missile;

		public bool IsDeviated => WeaponDefinition.DeviateShotAngle != 0f;

		public int CurrentWeaponRateOfFire => m_weaponDefinition.WeaponAmmoDatas[(int)AmmoDefinition.AmmoType].RateOfFire;

		public int ShotsInBurst => m_weaponDefinition.WeaponAmmoDatas[(int)AmmoDefinition.AmmoType].ShotsInBurst;

		public int ReloadTime => m_weaponDefinition.ReloadTime;

		public int CurrentWeaponShootIntervalInMiliseconds => m_weaponDefinition.WeaponAmmoDatas[(int)AmmoDefinition.AmmoType].ShootIntervalInMiliseconds;

		public MySoundPair CurrentWeaponShootSound => m_weaponDefinition.WeaponAmmoDatas[(int)AmmoDefinition.AmmoType].ShootSound;

		public MyWeaponPropertiesWrapper(MyDefinitionId weaponDefinitionId)
		{
			WeaponDefinitionId = weaponDefinitionId;
			MyDefinitionManager.Static.TryGetWeaponDefinition(WeaponDefinitionId, out m_weaponDefinition);
		}

		public bool CanChangeAmmoMagazine(MyDefinitionId newAmmoMagazineId)
		{
			return WeaponDefinition.IsAmmoMagazineCompatible(newAmmoMagazineId);
		}

		public void ChangeAmmoMagazine(MyDefinitionId newAmmoMagazineId)
		{
			AmmoMagazineId = newAmmoMagazineId;
			m_ammoMagazineDefinition = MyDefinitionManager.Static.GetAmmoMagazineDefinition(AmmoMagazineId);
			AmmoDefinitionId = AmmoMagazineDefinition.AmmoDefinitionId;
			m_ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(AmmoDefinitionId);
		}

		public T GetCurrentAmmoDefinitionAs<T>() where T : MyAmmoDefinition
		{
			return AmmoDefinition as T;
		}
	}
}
