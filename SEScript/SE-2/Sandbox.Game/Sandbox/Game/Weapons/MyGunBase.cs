using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	public class MyGunBase : MyDeviceBase
	{
		public class DummyContainer
		{
			public List<MatrixD> Dummies = new List<MatrixD>();

			public int DummyIndex;

			public MatrixD DummyInWorld = Matrix.Identity;

			public bool Dirty = true;

			public MatrixD DummyToUse => Dummies[DummyIndex];
		}

		private class WeaponEffect
		{
			public string EffectName;

			public string DummyName;

			public Matrix LocalMatrix;

			public MyWeaponDefinition.WeaponEffectAction Action;

			public MyParticleEffect Effect;

			public bool InstantStop;

			public WeaponEffect(string effectName, string dummyName, Matrix localMatrix, MyWeaponDefinition.WeaponEffectAction action, MyParticleEffect effect, bool instantStop)
			{
				EffectName = effectName;
				DummyName = dummyName;
				Effect = effect;
				Action = action;
				LocalMatrix = localMatrix;
				InstantStop = instantStop;
			}
		}

		protected class m_cachedAmmunitionAmount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType cachedAmmunitionAmount;
				ISyncType result = cachedAmmunitionAmount = new Sync<int, SyncDirection.FromServer>(P_1, P_2);
				((MyGunBase)P_0).m_cachedAmmunitionAmount = (Sync<int, SyncDirection.FromServer>)cachedAmmunitionAmount;
				return result;
			}
		}

		public const int AMMO_PER_SHOOT = 1;

		protected MyWeaponPropertiesWrapper m_weaponProperties;

		protected Dictionary<MyDefinitionId, int> m_remainingAmmos;

		protected Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;

		protected Dictionary<int, DummyContainer> m_dummiesByAmmoType;

		protected MatrixD m_worldMatrix;

		protected IMyGunBaseUser m_user;

		private List<WeaponEffect> m_activeEffects = new List<WeaponEffect>();

		public Matrix m_holdingDummyMatrix;

		private int m_shotProjectiles;

		private Dictionary<string, MyModelDummy> m_dummies;

		public int CurrentAmmo
		{
			get;
			set;
		}

		public MyWeaponPropertiesWrapper WeaponProperties => m_weaponProperties;

		public MyAmmoMagazineDefinition CurrentAmmoMagazineDefinition => WeaponProperties.AmmoMagazineDefinition;

		public MyDefinitionId CurrentAmmoMagazineId => WeaponProperties.AmmoMagazineId;

		public MyAmmoDefinition CurrentAmmoDefinition => WeaponProperties.AmmoDefinition;

		public float BackkickForcePerSecond
		{
			get
			{
				if (WeaponProperties != null && WeaponProperties.AmmoDefinition != null)
				{
					return WeaponProperties.AmmoDefinition.BackkickForce;
				}
				return 0f;
			}
		}

		public bool HasMissileAmmoDefined => m_weaponProperties.WeaponDefinition.HasMissileAmmoDefined;

		public bool HasProjectileAmmoDefined => m_weaponProperties.WeaponDefinition.HasProjectileAmmoDefined;

		public int MuzzleFlashLifeSpan => m_weaponProperties.WeaponDefinition.MuzzleFlashLifeSpan;

		public int ShootIntervalInMiliseconds
		{
			get
			{
				if (ShootIntervalModifier != 1f)
				{
					return (int)(ShootIntervalModifier * (float)m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds);
				}
				return m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds;
			}
		}

		public float ShootIntervalModifier
		{
			get;
			set;
		}

		public float ReleaseTimeAfterFire => m_weaponProperties.WeaponDefinition.ReleaseTimeAfterFire;

		public MySoundPair ShootSound => m_weaponProperties.CurrentWeaponShootSound;

		public MySoundPair NoAmmoSound => m_weaponProperties.WeaponDefinition.NoAmmoSound;

		public MySoundPair ReloadSound => m_weaponProperties.WeaponDefinition.ReloadSound;

		public MySoundPair SecondarySound => m_weaponProperties.WeaponDefinition.SecondarySound;

		public bool UseDefaultMuzzleFlash => m_weaponProperties.WeaponDefinition.UseDefaultMuzzleFlash;

		public float MechanicalDamage
		{
			get
			{
				if (WeaponProperties.AmmoDefinition != null)
				{
					return m_weaponProperties.AmmoDefinition.GetDamageForMechanicalObjects();
				}
				return 0f;
			}
		}

		public float DeviateAngle => m_weaponProperties.WeaponDefinition.DeviateShotAngle;

		public bool HasAmmoMagazines => m_weaponProperties.WeaponDefinition.HasAmmoMagazines();

		public bool IsAmmoProjectile => m_weaponProperties.IsAmmoProjectile;

		public bool IsAmmoMissile => m_weaponProperties.IsAmmoMissile;

		public int ShotsInBurst => WeaponProperties.ShotsInBurst;

		public int ReloadTime => WeaponProperties.ReloadTime;

		public bool HasDummies => m_dummiesByAmmoType.Count > 0;

		public MatrixD WorldMatrix
		{
			get
			{
				return m_worldMatrix;
			}
			set
			{
				m_worldMatrix = value;
				RecalculateMuzzles();
			}
		}

		public DateTime LastShootTime
		{
			get;
			private set;
		}

		public int RemainingAmmo
		{
			set
			{
				m_cachedAmmunitionAmount.SetLocalValue(value);
			}
		}

		public int DummiesPerType(MyAmmoType ammoType)
		{
			if (m_dummiesByAmmoType.ContainsKey((int)ammoType))
			{
				return m_dummiesByAmmoType[(int)ammoType].Dummies.Count;
			}
			return 0;
		}

		public MyGunBase()
		{
			m_dummiesByAmmoType = new Dictionary<int, DummyContainer>();
			m_remainingAmmos = new Dictionary<MyDefinitionId, int>();
			ShootIntervalModifier = 1f;
		}

		public MyObjectBuilder_GunBase GetObjectBuilder()
		{
			MyObjectBuilder_GunBase myObjectBuilder_GunBase = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GunBase>();
			myObjectBuilder_GunBase.CurrentAmmoMagazineName = CurrentAmmoMagazineId.SubtypeName;
			myObjectBuilder_GunBase.RemainingAmmo = CurrentAmmo;
			myObjectBuilder_GunBase.LastShootTime = LastShootTime.Ticks;
			myObjectBuilder_GunBase.RemainingAmmosList = new List<MyObjectBuilder_GunBase.RemainingAmmoIns>();
			foreach (KeyValuePair<MyDefinitionId, int> remainingAmmo in m_remainingAmmos)
			{
				MyObjectBuilder_GunBase.RemainingAmmoIns remainingAmmoIns = new MyObjectBuilder_GunBase.RemainingAmmoIns();
				remainingAmmoIns.SubtypeName = remainingAmmo.Key.SubtypeName;
				remainingAmmoIns.Amount = remainingAmmo.Value;
				myObjectBuilder_GunBase.RemainingAmmosList.Add(remainingAmmoIns);
			}
			myObjectBuilder_GunBase.InventoryItemId = base.InventoryItemId;
			return myObjectBuilder_GunBase;
		}

		public void Init(MyObjectBuilder_GunBase objectBuilder, MyCubeBlockDefinition cubeBlockDefinition, IMyGunBaseUser gunBaseUser)
		{
			if (cubeBlockDefinition is MyWeaponBlockDefinition)
			{
				MyWeaponBlockDefinition myWeaponBlockDefinition = cubeBlockDefinition as MyWeaponBlockDefinition;
				Init(objectBuilder, myWeaponBlockDefinition.WeaponDefinitionId, gunBaseUser);
			}
			else
			{
				MyDefinitionId backwardCompatibleDefinitionId = GetBackwardCompatibleDefinitionId(cubeBlockDefinition.Id.TypeId);
				Init(objectBuilder, backwardCompatibleDefinitionId, gunBaseUser);
			}
		}

		public void Init(MyObjectBuilder_GunBase objectBuilder, MyDefinitionId weaponDefinitionId, IMyGunBaseUser gunBaseUser)
		{
			if (objectBuilder != null)
			{
				Init(objectBuilder);
			}
			m_user = gunBaseUser;
			m_weaponProperties = new MyWeaponPropertiesWrapper(weaponDefinitionId);
			m_remainingAmmos = new Dictionary<MyDefinitionId, int>(WeaponProperties.AmmoMagazinesCount);
			if (objectBuilder != null)
			{
				MyDefinitionId newAmmoMagazineId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), objectBuilder.CurrentAmmoMagazineName);
				if (m_weaponProperties.CanChangeAmmoMagazine(newAmmoMagazineId))
				{
					CurrentAmmo = objectBuilder.RemainingAmmo;
					m_weaponProperties.ChangeAmmoMagazine(newAmmoMagazineId);
				}
				else if (WeaponProperties.WeaponDefinition.HasAmmoMagazines())
				{
					m_weaponProperties.ChangeAmmoMagazine(m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
				}
				foreach (MyObjectBuilder_GunBase.RemainingAmmoIns remainingAmmos in objectBuilder.RemainingAmmosList)
				{
					m_remainingAmmos.Add(new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), remainingAmmos.SubtypeName), remainingAmmos.Amount);
				}
				LastShootTime = new DateTime(objectBuilder.LastShootTime);
			}
			else
			{
				if (WeaponProperties.WeaponDefinition.HasAmmoMagazines())
				{
					m_weaponProperties.ChangeAmmoMagazine(m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
				}
				LastShootTime = new DateTime(0L);
			}
			if (m_user.AmmoInventory != null)
			{
				if (m_user.PutConstraint())
				{
					m_user.AmmoInventory.Constraint = CreateAmmoInventoryConstraints(m_user.ConstraintDisplayName);
				}
				RefreshAmmunitionAmount();
			}
			if (m_user.Weapon != null)
			{
				m_user.Weapon.OnClosing += Weapon_OnClosing;
			}
		}

		private void Weapon_OnClosing(MyEntity obj)
		{
			if (m_user.Weapon != null)
			{
				m_user.Weapon.OnClosing -= Weapon_OnClosing;
			}
		}

		public Vector3 GetDeviatedVector(float deviateAngle, Vector3 direction)
		{
			return MyUtilRandomVector3ByDeviatingVector.GetRandom(direction, deviateAngle);
		}

		private void AddProjectile(MyWeaponPropertiesWrapper weaponProperties, Vector3D initialPosition, Vector3D initialVelocity, Vector3D direction, MyEntity owner)
		{
			Vector3 directionNormalized = direction;
			if (weaponProperties.IsDeviated)
			{
				directionNormalized = GetDeviatedVector(weaponProperties.WeaponDefinition.DeviateShotAngle, direction);
				directionNormalized.Normalize();
			}
			if (directionNormalized.IsValid())
			{
				m_shotProjectiles++;
				MyProjectiles.Add(weaponProperties, initialPosition, initialVelocity, directionNormalized, m_user, owner);
			}
		}

		private void AddMissile(MyWeaponPropertiesWrapper weaponProperties, Vector3D initialPosition, Vector3 initialVelocity, Vector3 direction, MyEntity controller)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MyMissileAmmoDefinition currentAmmoDefinitionAs = weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
			Vector3 vector = direction;
			if (weaponProperties.IsDeviated)
			{
				vector = GetDeviatedVector(weaponProperties.WeaponDefinition.DeviateShotAngle, direction);
				vector.Normalize();
			}
			if (vector.IsValid())
			{
				initialVelocity += vector * currentAmmoDefinitionAs.MissileInitialSpeed;
				long owner = 0L;
				if (m_user.OwnerId != 0L)
				{
					owner = m_user.OwnerId;
				}
				else if (controller != null)
				{
					owner = controller.EntityId;
				}
				MyObjectBuilder_Missile builder = MyMissile.PrepareBuilder(weaponProperties, initialPosition, initialVelocity, vector, owner, m_user.Owner.EntityId, (m_user.Launcher as MyEntity).EntityId);
				m_user.Launcher.ShootMissile(builder);
			}
		}

		public void Shoot(Vector3 initialVelocity, MyEntity owner = null)
		{
			MatrixD muzzleWorldMatrix = GetMuzzleWorldMatrix();
			Shoot(muzzleWorldMatrix.Translation, initialVelocity, muzzleWorldMatrix.Forward, owner);
		}

		public void Shoot(Vector3 initialVelocity, Vector3 direction, MyEntity owner = null)
		{
			Shoot(GetMuzzleWorldMatrix().Translation, initialVelocity, direction, owner);
		}

		public void ShootWithOffset(Vector3 initialVelocity, Vector3 direction, float offset, MyEntity owner = null)
		{
			Shoot(GetMuzzleWorldMatrix().Translation + direction * offset, initialVelocity, direction, owner);
		}

		public void Shoot(Vector3D initialPosition, Vector3 initialVelocity, Vector3 direction, MyEntity owner = null)
		{
			MyAmmoDefinition ammoDefinition = m_weaponProperties.AmmoDefinition;
			switch (ammoDefinition.AmmoType)
			{
			case MyAmmoType.HighSpeed:
			{
				int projectileCount = (ammoDefinition as MyProjectileAmmoDefinition).ProjectileCount;
				for (int i = 0; i < projectileCount; i++)
				{
					AddProjectile(m_weaponProperties, initialPosition, initialVelocity, direction, owner);
				}
				break;
			}
			case MyAmmoType.Missile:
				if (Sync.IsServer)
				{
					AddMissile(m_weaponProperties, initialPosition, initialVelocity, direction, owner);
				}
				break;
			}
			MoveToNextMuzzle(ammoDefinition.AmmoType);
			CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
			LastShootTime = DateTime.UtcNow;
		}

		public void CreateEffects(MyWeaponDefinition.WeaponEffectAction action)
		{
			if (m_dummies == null || m_dummies.Count <= 0 || WeaponProperties.WeaponDefinition.WeaponEffects.Length == 0)
			{
				return;
			}
			for (int i = 0; i < WeaponProperties.WeaponDefinition.WeaponEffects.Length; i++)
			{
				if (WeaponProperties.WeaponDefinition.WeaponEffects[i].Action != action || !m_dummies.TryGetValue(WeaponProperties.WeaponDefinition.WeaponEffects[i].Dummy, out MyModelDummy value))
				{
					continue;
				}
				bool flag = true;
				string empty = string.Empty;
				empty = WeaponProperties.WeaponDefinition.WeaponEffects[i].Particle;
				if (WeaponProperties.WeaponDefinition.WeaponEffects[i].Loop)
				{
					for (int j = 0; j < m_activeEffects.Count; j++)
					{
						if (m_activeEffects[j].DummyName == value.Name && m_activeEffects[j].EffectName == empty)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					MatrixD matrixD = MatrixD.Normalize(value.Matrix);
					if (MyParticlesManager.TryCreateParticleEffect(empty, MatrixD.Multiply(matrixD, WorldMatrix), out MyParticleEffect effect) && WeaponProperties.WeaponDefinition.WeaponEffects[i].Loop)
					{
						m_activeEffects.Add(new WeaponEffect(empty, value.Name, matrixD, action, effect, WeaponProperties.WeaponDefinition.WeaponEffects[i].InstantStop));
					}
				}
			}
		}

		public void UpdateEffects()
		{
			for (int i = 0; i < m_activeEffects.Count; i++)
			{
				if (!m_activeEffects[i].Effect.IsStopped || m_activeEffects[i].Effect.GetParticlesCount() > 0)
				{
					m_activeEffects[i].Effect.WorldMatrix = MatrixD.Multiply(m_activeEffects[i].LocalMatrix, WorldMatrix);
					continue;
				}
				m_activeEffects.RemoveAt(i);
				i--;
			}
		}

		public void RemoveOldEffects(MyWeaponDefinition.WeaponEffectAction action = MyWeaponDefinition.WeaponEffectAction.Shoot)
		{
			for (int i = 0; i < m_activeEffects.Count; i++)
			{
				if (m_activeEffects[i].Action == action)
				{
					m_activeEffects[i].Effect.Stop(m_activeEffects[i].InstantStop);
					m_activeEffects[i].Effect.Close(notify: true, m_activeEffects[i].InstantStop);
					m_activeEffects.RemoveAt(i);
					i--;
				}
			}
		}

		public MyInventoryConstraint CreateAmmoInventoryConstraints(string displayName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(MyTexts.GetString(MySpaceTexts.ToolTipItemFilter_AmmoMagazineInput), displayName);
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(stringBuilder.ToString());
			MyDefinitionId[] ammoMagazinesId = m_weaponProperties.WeaponDefinition.AmmoMagazinesId;
			foreach (MyDefinitionId id in ammoMagazinesId)
			{
				myInventoryConstraint.Add(id);
			}
			return myInventoryConstraint;
		}

		public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineId)
		{
			return WeaponProperties.CanChangeAmmoMagazine(ammoMagazineId);
		}

		public override bool CanSwitchAmmoMagazine()
		{
			if (m_weaponProperties != null)
			{
				return m_weaponProperties.WeaponDefinition.HasAmmoMagazines();
			}
			return false;
		}

		public bool SwitchAmmoMagazine(MyDefinitionId ammoMagazineId)
		{
			m_remainingAmmos[CurrentAmmoMagazineId] = CurrentAmmo;
			WeaponProperties.ChangeAmmoMagazine(ammoMagazineId);
			int value = 0;
			m_remainingAmmos.TryGetValue(ammoMagazineId, out value);
			CurrentAmmo = value;
			RefreshAmmunitionAmount();
			return ammoMagazineId == WeaponProperties.AmmoMagazineId;
		}

		public override bool SwitchAmmoMagazineToNextAvailable()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			if (!weaponDefinition.HasAmmoMagazines())
			{
				return false;
			}
			int ammoMagazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(CurrentAmmoMagazineId);
			int num = weaponDefinition.AmmoMagazinesId.Length;
			int num2 = ammoMagazineIdArrayIndex + 1;
			for (int i = 0; i != num; i++)
			{
				if (num2 == num)
				{
					num2 = 0;
				}
				if (weaponDefinition.AmmoMagazinesId[num2].SubtypeId != CurrentAmmoMagazineId.SubtypeId)
				{
					if (MySession.Static.InfiniteAmmo)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
					int value = 0;
					if (m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[num2], out value) && value > 0)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
					if (m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[num2]) > 0)
					{
						return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[num2]);
					}
				}
				num2++;
			}
			return false;
		}

		public override bool SwitchToNextAmmoMagazine()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			int ammoMagazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(CurrentAmmoMagazineId);
			int num = weaponDefinition.AmmoMagazinesId.Length;
			ammoMagazineIdArrayIndex++;
			if (ammoMagazineIdArrayIndex == num)
			{
				ammoMagazineIdArrayIndex = 0;
			}
			return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[ammoMagazineIdArrayIndex]);
		}

		public bool SwitchAmmoMagazineToFirstAvailable()
		{
			MyWeaponDefinition weaponDefinition = WeaponProperties.WeaponDefinition;
			for (int i = 0; i < WeaponProperties.AmmoMagazinesCount; i++)
			{
				int value = 0;
				if (m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[i], out value) && value > 0)
				{
					return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[i]);
				}
				if (m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[i]) > 0)
				{
					return SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[i]);
				}
			}
			return false;
		}

		public bool HasEnoughAmmunition()
		{
			if (MySession.Static.InfiniteAmmo)
			{
				return true;
			}
			if (!Sync.IsServer)
			{
				return (int)m_cachedAmmunitionAmount > 0;
			}
			if (CurrentAmmo < 1)
			{
				if (m_user == null || m_user.AmmoInventory == null)
				{
					string msg = string.Format("Error: {0} should not be null!", (m_user == null) ? "User" : "AmmoInventory");
					MyLog.Default.WriteLine(msg);
					return false;
				}
				return m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId) > 0;
			}
			return true;
		}

		public void ConsumeAmmo()
		{
			if (Sync.IsServer)
			{
				CurrentAmmo--;
				if (CurrentAmmo < 0 && HasEnoughAmmunition())
				{
					CurrentAmmo = WeaponProperties.AmmoMagazineDefinition.Capacity - 1;
					if (!MySession.Static.InfiniteAmmo)
					{
						m_user.AmmoInventory.RemoveItemsOfType(1, CurrentAmmoMagazineId);
					}
				}
				RefreshAmmunitionAmount();
			}
			MyInventory ammoInventory = m_user.AmmoInventory;
			if (ammoInventory == null)
			{
				return;
			}
			MyPhysicalInventoryItem? myPhysicalInventoryItem = null;
			if (base.InventoryItemId.HasValue)
			{
				myPhysicalInventoryItem = ammoInventory.GetItemByID(base.InventoryItemId.Value);
			}
			else
			{
				myPhysicalInventoryItem = ammoInventory.FindUsableItem(m_user.PhysicalItemId);
				if (myPhysicalInventoryItem.HasValue)
				{
					base.InventoryItemId = myPhysicalInventoryItem.Value.ItemId;
				}
			}
			if (!myPhysicalInventoryItem.HasValue)
			{
				return;
			}
			MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject = myPhysicalInventoryItem.Value.Content as MyObjectBuilder_PhysicalGunObject;
			if (myObjectBuilder_PhysicalGunObject == null)
			{
				return;
			}
			IMyObjectBuilder_GunObject<MyObjectBuilder_GunBase> myObjectBuilder_GunObject = myObjectBuilder_PhysicalGunObject.GunEntity as IMyObjectBuilder_GunObject<MyObjectBuilder_GunBase>;
			if (myObjectBuilder_GunObject != null)
			{
				if (myObjectBuilder_GunObject.DeviceBase == null)
				{
					myObjectBuilder_GunObject.InitializeDeviceBase(GetObjectBuilder());
				}
				else
				{
					myObjectBuilder_GunObject.GetDevice().RemainingAmmo = CurrentAmmo;
				}
			}
		}

		public void StopShoot()
		{
			m_shotProjectiles = 0;
		}

		public int GetTotalAmmunitionAmount()
		{
			return m_cachedAmmunitionAmount;
		}

		public int GetInventoryAmmoMagazinesCount()
		{
			return (int)m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId);
		}

		public void RefreshAmmunitionAmount()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if (MySession.Static.InfiniteAmmo)
			{
				m_cachedAmmunitionAmount.Value = CurrentAmmo;
			}
			else if (m_user != null && m_user.AmmoInventory != null && m_weaponProperties.WeaponDefinition.HasAmmoMagazines())
			{
				if (!HasEnoughAmmunition())
				{
					SwitchAmmoMagazineToFirstAvailable();
				}
				m_cachedAmmunitionAmount.Value = CurrentAmmo + (int)m_user.AmmoInventory.GetItemAmount(CurrentAmmoMagazineId) * m_weaponProperties.AmmoMagazineDefinition.Capacity;
			}
			else
			{
				m_cachedAmmunitionAmount.Value = 0;
			}
		}

		private MyDefinitionId GetBackwardCompatibleDefinitionId(MyObjectBuilderType typeId)
		{
			if (typeId == typeof(MyObjectBuilder_LargeGatlingTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeGatlingTurret");
			}
			if (typeId == typeof(MyObjectBuilder_LargeMissileTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeMissileTurret");
			}
			if (typeId == typeof(MyObjectBuilder_InteriorTurret))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "LargeInteriorTurret");
			}
			if (typeId == typeof(MyObjectBuilder_SmallMissileLauncher) || typeId == typeof(MyObjectBuilder_SmallMissileLauncherReload))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "SmallMissileLauncher");
			}
			if (typeId == typeof(MyObjectBuilder_SmallGatlingGun))
			{
				return new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "GatlingGun");
			}
			return default(MyDefinitionId);
		}

		public void AddMuzzleMatrix(MyAmmoType ammoType, Matrix localMatrix)
		{
			if (!m_dummiesByAmmoType.ContainsKey((int)ammoType))
			{
				m_dummiesByAmmoType[(int)ammoType] = new DummyContainer();
			}
			m_dummiesByAmmoType[(int)ammoType].Dummies.Add(MatrixD.Normalize(localMatrix));
		}

		public void LoadDummies(Dictionary<string, MyModelDummy> dummies)
		{
			m_dummies = dummies;
			m_dummiesByAmmoType.Clear();
			foreach (KeyValuePair<string, MyModelDummy> dummy in dummies)
			{
				if (dummy.Key.ToLower().Contains("muzzle_projectile"))
				{
					AddMuzzleMatrix(MyAmmoType.HighSpeed, dummy.Value.Matrix);
					m_holdingDummyMatrix = dummy.Value.Matrix;
					m_holdingDummyMatrix = Matrix.CreateScale(1f / dummy.Value.Matrix.Scale) * m_holdingDummyMatrix;
					m_holdingDummyMatrix = Matrix.Invert(m_holdingDummyMatrix);
				}
				else if (dummy.Key.ToLower().Contains("muzzle_missile"))
				{
					AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix);
				}
				else if (dummy.Key.ToLower().Contains("holding_dummy") || dummy.Key.ToLower().Contains("holdingdummy"))
				{
					m_holdingDummyMatrix = dummy.Value.Matrix;
					m_holdingDummyMatrix = Matrix.Normalize(m_holdingDummyMatrix);
				}
			}
		}

		public override Vector3D GetMuzzleLocalPosition()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return Vector3D.Zero;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out DummyContainer value))
			{
				return value.DummyToUse.Translation;
			}
			return Vector3D.Zero;
		}

		public MatrixD GetMuzzleLocalMatrix()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return MatrixD.Identity;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out DummyContainer value))
			{
				return value.DummyToUse;
			}
			return MatrixD.Identity;
		}

		public override Vector3D GetMuzzleWorldPosition()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return m_worldMatrix.Translation;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out DummyContainer value))
			{
				if (value.Dirty)
				{
					value.DummyInWorld = value.DummyToUse * m_worldMatrix;
					value.Dirty = false;
				}
				return value.DummyInWorld.Translation;
			}
			return m_worldMatrix.Translation;
		}

		public MatrixD GetMuzzleWorldMatrix()
		{
			if (m_weaponProperties.AmmoDefinition == null)
			{
				return m_worldMatrix;
			}
			if (m_dummiesByAmmoType.TryGetValue((int)m_weaponProperties.AmmoDefinition.AmmoType, out DummyContainer value))
			{
				if (value.Dirty)
				{
					value.DummyInWorld = value.DummyToUse * m_worldMatrix;
					value.Dirty = false;
				}
				return value.DummyInWorld;
			}
			return m_worldMatrix;
		}

		private void MoveToNextMuzzle(MyAmmoType ammoType)
		{
			if (m_dummiesByAmmoType.TryGetValue((int)ammoType, out DummyContainer value) && value.Dummies.Count > 1)
			{
				value.DummyIndex++;
				if (value.DummyIndex == value.Dummies.Count)
				{
					value.DummyIndex = 0;
				}
				value.Dirty = true;
			}
		}

		private void RecalculateMuzzles()
		{
			foreach (DummyContainer value in m_dummiesByAmmoType.Values)
			{
				value.Dirty = true;
			}
		}

		public void StartShootSound(MyEntity3DSoundEmitter soundEmitter, bool force2D = false)
		{
			if (ShootSound == null || soundEmitter == null)
			{
				return;
			}
			if (soundEmitter.IsPlaying)
			{
				if (!soundEmitter.Loop)
				{
					soundEmitter.PlaySound(ShootSound, stopPrevious: false, skipIntro: false, force2D);
				}
			}
			else
			{
				soundEmitter.PlaySound(ShootSound, stopPrevious: true, skipIntro: false, force2D);
			}
		}

		internal void StartNoAmmoSound(MyEntity3DSoundEmitter soundEmitter)
		{
			if (NoAmmoSound != null && soundEmitter != null)
			{
				soundEmitter.StopSound(forced: true);
				soundEmitter.PlaySingleSound(NoAmmoSound, stopPrevious: true);
			}
		}
	}
}
