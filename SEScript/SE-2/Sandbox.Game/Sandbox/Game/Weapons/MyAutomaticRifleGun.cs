using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_AutomaticRifle), true)]
	public class MyAutomaticRifleGun : MyEntity, IMyHandheldGunObject<MyGunBase>, IMyGunObject<MyGunBase>, IMyGunBaseUser, IMyEventProxy, IMyEventOwner, IMyAutomaticRifleGun, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyMissileGunObject, IMySyncedEntity
	{
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MyAutomaticRifleGun, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAutomaticRifleGun @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MyAutomaticRifleGun, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyAutomaticRifleGun @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		private class Sandbox_Game_Weapons_MyAutomaticRifleGun_003C_003EActor : IActivator, IActivator<MyAutomaticRifleGun>
		{
			private sealed override object CreateInstance()
			{
				return new MyAutomaticRifleGun();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAutomaticRifleGun CreateInstance()
			{
				return new MyAutomaticRifleGun();
			}

			MyAutomaticRifleGun IActivator<MyAutomaticRifleGun>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_lastTimeShoot;

		public static float RIFLE_MAX_SHAKE = 0.5f;

		public static float RIFLE_FOV_SHAKE = 0.0065f;

		private int m_lastDirectionChangeAnnounce;

		private MyParticleEffect m_smokeEffect;

		private bool m_firstDraw;

		private MyGunBase m_gunBase;

		private static MyDefinitionId m_handItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "AutomaticRifleGun");

		private MyPhysicalItemDefinition m_physicalItemDef;

		private MyCharacter m_owner;

		protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();

		private int m_shootingCounter;

		private bool m_canZoom = true;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private int m_shotsFiredInBurst;

		private MyHudNotification m_outOfAmmoNotification;

		private MyHudNotification m_safezoneNotification;

		private bool m_isAfterReleaseFire;

		private MyEntity[] m_shootIgnoreEntities;

		public int LastTimeShoot => m_lastTimeShoot;

		public MyCharacter Owner => m_owner;

		public long OwnerId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.EntityId;
				}
				return 0L;
			}
		}

		public long OwnerIdentityId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetPlayerIdentityId();
				}
				return 0L;
			}
		}

		public bool IsShooting
		{
			get;
			private set;
		}

		public int ShootDirectionUpdateTime => 200;

		public bool ForceAnimationInsteadOfIK => false;

		public bool IsBlocking => false;

		public MyObjectBuilder_PhysicalGunObject PhysicalObject
		{
			get;
			set;
		}

		public SyncType SyncType
		{
			get;
			set;
		}

		public bool IsSkinnable => true;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount
		{
			get;
			protected set;
		}

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => m_handItemDefId;

		public MyGunBase GunBase => m_gunBase;

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon => this;

		MyEntity IMyGunBaseUser.Owner => m_owner;

		IMyMissileGunObject IMyGunBaseUser.Launcher => this;

		MyInventory IMyGunBaseUser.AmmoInventory
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetInventory();
				}
				return null;
			}
		}

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => m_physicalItemDef.Id;

		MyInventory IMyGunBaseUser.WeaponInventory
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.GetInventory();
				}
				return null;
			}
		}

		long IMyGunBaseUser.OwnerId
		{
			get
			{
				if (m_owner != null)
				{
					return m_owner.ControllerInfo.ControllingIdentityId;
				}
				return 0L;
			}
		}

		string IMyGunBaseUser.ConstraintDisplayName => null;

		public MyPhysicalItemDefinition PhysicalItemDefinition => m_physicalItemDef;

		public int CurrentAmmunition
		{
			get
			{
				return m_gunBase.GetTotalAmmunitionAmount();
			}
			set
			{
				m_gunBase.RemainingAmmo = value;
			}
		}

		public int CurrentMagazineAmmunition
		{
			get
			{
				return m_gunBase.CurrentAmmo;
			}
			set
			{
				m_gunBase.CurrentAmmo = value;
			}
		}

		public MyAutomaticRifleGun()
		{
			m_shootIgnoreEntities = new MyEntity[1]
			{
				this
			};
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME);
			base.Render.NeedsDraw = true;
			m_gunBase = new MyGunBase();
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = WorldPositionChanged;
			base.Render = new MyRenderComponentAutomaticRifle();
			SyncType = SyncHelpers.Compose(this);
			SyncType.Append(m_gunBase);
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
			{
				m_handItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_AutomaticRifle), objectBuilder.SubtypeName);
			}
			MyObjectBuilder_AutomaticRifle myObjectBuilder_AutomaticRifle = (MyObjectBuilder_AutomaticRifle)objectBuilder;
			MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref m_handItemDefId);
			m_physicalItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(m_handItemDefId);
			MyDefinitionId weaponDefinitionId = (m_physicalItemDef is MyWeaponItemDefinition) ? (m_physicalItemDef as MyWeaponItemDefinition).WeaponDefinitionId : new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), "AutomaticRifleGun");
			m_gunBase.Init(myObjectBuilder_AutomaticRifle.GunBase, weaponDefinitionId, this);
			base.Init(objectBuilder);
			Init(MyTexts.Get(MySpaceTexts.DisplayName_Rifle), m_physicalItemDef.Model, null, null);
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(m_physicalItemDef.Model);
			m_gunBase.LoadDummies(modelOnlyDummies.Dummies);
			if (!m_gunBase.HasDummies)
			{
				Matrix localMatrix = Matrix.CreateTranslation(myHandItemDefinition.MuzzlePosition);
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, localMatrix);
			}
			PhysicalObject = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(m_physicalItemDef.Id.TypeId, m_physicalItemDef.Id.SubtypeName);
			PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)myObjectBuilder_AutomaticRifle.Clone();
			PhysicalObject.GunEntity.EntityId = base.EntityId;
			CurrentAmmunition = myObjectBuilder_AutomaticRifle.CurrentAmmo;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_AutomaticRifle obj = (MyObjectBuilder_AutomaticRifle)base.GetObjectBuilder(copy);
			obj.SubtypeName = DefinitionId.SubtypeName;
			obj.GunBase = m_gunBase.GetObjectBuilder();
			obj.CurrentAmmo = CurrentAmmunition;
			return obj;
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			MyCharacterWeaponPositionComponent myCharacterWeaponPositionComponent = Owner.Components.Get<MyCharacterWeaponPositionComponent>();
			Vector3D v = (myCharacterWeaponPositionComponent == null || !Sandbox.Engine.Platform.Game.IsDedicated) ? Vector3D.Normalize(target - base.PositionComp.WorldMatrix.Translation) : Vector3D.Normalize(target - myCharacterWeaponPositionComponent.LogicalPositionWorld);
			return v;
		}

		public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (m_owner == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(m_owner, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				if (!m_gunBase.HasAmmoMagazines)
				{
					status = MyGunStatusEnum.Failed;
					return false;
				}
				if (m_gunBase.ShotsInBurst > 0 && m_shotsFiredInBurst >= m_gunBase.ShotsInBurst)
				{
					status = MyGunStatusEnum.BurstLimit;
					return false;
				}
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
				{
					status = MyGunStatusEnum.Cooldown;
					return false;
				}
				if (m_owner.GetCurrentMovementState() == MyCharacterMovementEnum.Sprinting)
				{
					status = MyGunStatusEnum.Failed;
					return false;
				}
				if (!MySession.Static.CreativeMode && (!(m_owner.CurrentWeapon is MyAutomaticRifleGun) || !m_gunBase.HasEnoughAmmunition()))
				{
					status = MyGunStatusEnum.OutOfAmmo;
					return false;
				}
				status = MyGunStatusEnum.OK;
				return true;
			case MyShootActionEnum.SecondaryAction:
				if (!m_canZoom)
				{
					status = MyGunStatusEnum.Cooldown;
					return false;
				}
				return true;
			default:
				status = MyGunStatusEnum.Failed;
				return false;
			}
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				Shoot(direction, overrideWeaponPos);
				m_shotsFiredInBurst++;
				IsShooting = true;
				if (m_owner.ControllerInfo.IsLocallyControlled() && !MySession.Static.IsCameraUserAnySpectator())
				{
					MySector.MainCamera.CameraShake.AddShake(RIFLE_MAX_SHAKE);
					MySector.MainCamera.AddFovSpring(RIFLE_FOV_SHAKE);
				}
				break;
			case MyShootActionEnum.SecondaryAction:
				if (MySession.Static.ControlledEntity == m_owner)
				{
					m_owner.Zoom(newKeyPress: true);
					m_canZoom = false;
				}
				break;
			}
		}

		public void BeginShoot(MyShootActionEnum action)
		{
		}

		public void EndShoot(MyShootActionEnum action)
		{
			switch (action)
			{
			case MyShootActionEnum.PrimaryAction:
				IsShooting = false;
				m_shotsFiredInBurst = 0;
				m_gunBase.StopShoot();
				break;
			case MyShootActionEnum.SecondaryAction:
				m_canZoom = true;
				break;
			}
			m_isActionDoubleClicked[action] = false;
		}

		private void Shoot(Vector3 direction, Vector3D? overrideWeaponPos)
		{
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (!overrideWeaponPos.HasValue || m_gunBase.DummiesPerType(MyAmmoType.HighSpeed) > 1)
			{
				if (m_owner != null)
				{
					m_gunBase.ShootWithOffset(m_owner.Physics.LinearVelocity, direction, -0.25f, m_owner);
				}
				else
				{
					m_gunBase.ShootWithOffset(Vector3.Zero, direction, -0.25f);
				}
			}
			else
			{
				m_gunBase.Shoot(overrideWeaponPos.Value + direction * -0.25f, m_owner.Physics.LinearVelocity, direction, m_owner);
			}
			m_isAfterReleaseFire = false;
			if (m_gunBase.ShootSound != null)
			{
				StartLoopSound(m_gunBase.ShootSound);
			}
			m_gunBase.ConsumeAmmo();
		}

		private void CreateSmokeEffect()
		{
			if (m_smokeEffect == null && MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) < 150.0 && MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", base.PositionComp.WorldMatrix, out m_smokeEffect))
			{
				m_smokeEffect.OnDelete += OnSmokeEffectDelete;
			}
		}

		private void OnSmokeEffectDelete(object sender, EventArgs eventArgs)
		{
			m_smokeEffect = null;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_smokeEffect != null)
			{
				float num = 0.2f;
				m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(m_gunBase.GetMuzzleWorldPosition() + base.PositionComp.WorldMatrix.Forward * num);
				m_smokeEffect.UserBirthMultiplier = 50f;
			}
			m_gunBase.UpdateEffects();
			if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) > m_gunBase.ReleaseTimeAfterFire && !m_isAfterReleaseFire)
			{
				StopLoopSound();
				if (m_smokeEffect != null)
				{
					m_smokeEffect.Stop();
				}
				m_isAfterReleaseFire = true;
				m_gunBase.RemoveOldEffects();
			}
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo)
			{
				m_gunBase.StartNoAmmoSound(m_soundEmitter);
			}
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.Failed || status == MyGunStatusEnum.SafeZoneDenied)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
			if (status == MyGunStatusEnum.OutOfAmmo)
			{
				if (m_outOfAmmoNotification == null)
				{
					m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
				}
				m_outOfAmmoNotification.SetTextFormatArguments(base.DisplayName);
				MyHud.Notifications.Add(m_outOfAmmoNotification);
			}
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				if (m_safezoneNotification == null)
				{
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_ShootingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		public void StartLoopSound(MySoundPair cueEnum)
		{
			bool force2D = m_owner != null && m_owner.IsInFirstPersonView && m_owner == MySession.Static.LocalCharacter;
			m_gunBase.StartShootSound(m_soundEmitter, force2D);
			UpdateSoundEmitter();
		}

		public void StopLoopSound()
		{
			if (m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		private void WorldPositionChanged(object source)
		{
			m_gunBase.WorldMatrix = base.WorldMatrix;
		}

		protected override void Closing()
		{
			IsShooting = false;
			m_gunBase.RemoveOldEffects();
			if (m_smokeEffect != null)
			{
				m_smokeEffect.Stop();
				m_smokeEffect = null;
			}
			if (m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: false);
			}
			base.Closing();
		}

		public void OnControlAcquired(MyCharacter owner)
		{
			m_owner = owner;
			if (m_owner != null)
			{
				m_shootIgnoreEntities = new MyEntity[2]
				{
					this,
					m_owner
				};
				MyInventory inventory = m_owner.GetInventory();
				if (inventory != null)
				{
					inventory.ContentsChanged += MyAutomaticRifleGun_ContentsChanged;
				}
			}
			m_gunBase.RefreshAmmunitionAmount();
			m_firstDraw = true;
		}

		private void MyAutomaticRifleGun_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount();
		}

		public void OnControlReleased()
		{
			if (m_owner != null)
			{
				MyInventory inventory = m_owner.GetInventory();
				if (inventory != null)
				{
					inventory.ContentsChanged -= MyAutomaticRifleGun_ContentsChanged;
				}
			}
			m_owner = null;
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			MyHud.BlockInfo.Visible = true;
			if (m_firstDraw)
			{
				MyHud.BlockInfo.MissingComponentIndex = -1;
				MyHud.BlockInfo.BlockName = PhysicalItemDefinition.DisplayNameText;
				MyHud.BlockInfo.PCUCost = 0;
				MyHud.BlockInfo.BlockIcons = PhysicalItemDefinition.Icons;
				MyHud.BlockInfo.BlockIntegrity = 1f;
				MyHud.BlockInfo.CriticalIntegrity = 0f;
				MyHud.BlockInfo.CriticalComponentIndex = 0;
				MyHud.BlockInfo.OwnershipIntegrity = 0f;
				MyHud.BlockInfo.BlockBuiltBy = 0L;
				MyHud.BlockInfo.GridSize = MyCubeSize.Small;
				MyHud.BlockInfo.Components.Clear();
				MyHud.BlockInfo.SetContextHelp(PhysicalItemDefinition);
				m_firstDraw = false;
			}
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			UpdateSoundEmitter();
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				if (m_owner != null)
				{
					Vector3 velocityVector = Vector3.Zero;
					m_owner.GetLinearVelocity(ref velocityVector);
					m_soundEmitter.SetVelocity(velocityVector);
				}
				m_soundEmitter.Update();
			}
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		public bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return false;
		}

		public void DoubleClicked(MyShootActionEnum action)
		{
			m_isActionDoubleClicked[action] = true;
		}

		public void MissileShootEffect()
		{
			m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MyAutomaticRifleGun x) => x.OnShootMissile, builder);
		}

		[Event(null, 704)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Add(builder);
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyAutomaticRifleGun x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 717)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Remove(entityId);
		}
	}
}
