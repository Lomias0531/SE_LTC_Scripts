using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SmallMissileLauncher))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySmallMissileLauncher),
		typeof(Sandbox.ModAPI.Ingame.IMySmallMissileLauncher)
	})]
	public class MySmallMissileLauncher : MyUserControllableGun, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallMissileLauncher, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallMissileLauncher
	{
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallMissileLauncher, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallMissileLauncher @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MySmallMissileLauncher, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallMissileLauncher @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MySmallMissileLauncher)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_MySmallMissileLauncher_003C_003EActor : IActivator, IActivator<MySmallMissileLauncher>
		{
			private sealed override object CreateInstance()
			{
				return new MySmallMissileLauncher();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySmallMissileLauncher CreateInstance()
			{
				return new MySmallMissileLauncher();
			}

			MySmallMissileLauncher IActivator<MySmallMissileLauncher>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected int m_shotsLeftInBurst;

		protected int m_nextShootTime;

		private int m_nextNotificationTime;

		private MyHudNotification m_reloadNotification;

		private MyGunBase m_gunBase;

		private bool m_shoot;

		private Vector3 m_shootDirection;

		private int m_currentBarrel;

		private int m_lateStartRandom = MyUtils.GetRandomInt(0, 3);

		private int m_currentLateStart;

		private Vector3D m_targetLocal = Vector3.Zero;

		private MyEntity[] m_shootIgnoreEntities;

		private MyHudNotification m_safezoneNotification;

		private MyMultilineConveyorEndpoint m_endpoint;

		protected Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		protected MyHudNotification ReloadNotification
		{
			get
			{
				if (m_reloadNotification == null)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.MissileLauncherReloadingNotification, m_gunBase.ReloadTime - 250, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
				}
				return m_reloadNotification;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_endpoint;

		public bool IsSkinnable => false;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount
		{
			get;
			protected set;
		}

		public bool IsControlled => Controller != null;

		public MyCharacter Controller
		{
			get;
			protected set;
		}

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => base.BlockDefinition.Id;

		public bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		public bool IsShooting => m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds;

		public int ShootDirectionUpdateTime => 0;

		public MyGunBase GunBase => m_gunBase;

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon => base.Parent;

		MyEntity IMyGunBaseUser.Owner => base.Parent;

		IMyMissileGunObject IMyGunBaseUser.Launcher => this;

		MyInventory IMyGunBaseUser.AmmoInventory => this.GetInventory();

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => default(MyDefinitionId);

		MyInventory IMyGunBaseUser.WeaponInventory => null;

		long IMyGunBaseUser.OwnerId => base.OwnerId;

		string IMyGunBaseUser.ConstraintDisplayName => base.BlockDefinition.DisplayNameText;

		bool Sandbox.ModAPI.Ingame.IMySmallMissileLauncher.UseConveyorSystem => m_useConveyorSystem;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public void InitializeConveyorEndpoint()
		{
			m_endpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_endpoint));
		}

		public override bool IsStationary()
		{
			return true;
		}

		public override Vector3D GetWeaponMuzzleWorldPosition()
		{
			if (m_gunBase != null)
			{
				return m_gunBase.GetMuzzleWorldPosition();
			}
			return base.GetWeaponMuzzleWorldPosition();
		}

		public MySmallMissileLauncher()
		{
			m_shootIgnoreEntities = new MyEntity[1]
			{
				this
			};
			CreateTerminalControls();
			m_gunBase = new MyGunBase();
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			base.SyncType.Append(m_gunBase);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySmallMissileLauncher>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MySmallMissileLauncher> obj = new MyTerminalControlOnOffSwitch<MySmallMissileLauncher>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MySmallMissileLauncher x) => x.UseConveyorSystem),
					Setter = delegate(MySmallMissileLauncher x, bool v)
					{
						x.UseConveyorSystem = v;
					},
					Visible = ((MySmallMissileLauncher x) => x.CubeGrid.GridSizeEnum == MyCubeSize.Large)
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_SmallMissileLauncher myObjectBuilder_SmallMissileLauncher = builder as MyObjectBuilder_SmallMissileLauncher;
			MyWeaponBlockDefinition myWeaponBlockDefinition = base.BlockDefinition as MyWeaponBlockDefinition;
			MyStringHash group;
			if (myWeaponBlockDefinition != null && this.GetInventory() == null)
			{
				MyInventory component = new MyInventory(myWeaponBlockDefinition.InventoryMaxVolume, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive);
				base.Components.Add((MyInventoryBase)component);
				group = myWeaponBlockDefinition.ResourceSinkGroup;
			}
			else
			{
				if (this.GetInventory() == null)
				{
					MyInventory myInventory = null;
					myInventory = ((cubeGrid.GridSizeEnum != MyCubeSize.Small) ? new MyInventory(1.14f, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive) : new MyInventory(0.24f, new Vector3(1.2f, 0.45f, 0.45f), MyInventoryFlags.CanReceive));
					base.Components.Add(myInventory);
				}
				group = MyStringHash.GetOrCompute("Defense");
			}
			this.GetInventory();
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(group, 0.0002f, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			m_gunBase.Init(myObjectBuilder_SmallMissileLauncher.GunBase, base.BlockDefinition, this);
			base.Init(builder, cubeGrid);
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			base.ResourceSink.Update();
			this.GetInventory().Init(myObjectBuilder_SmallMissileLauncher.Inventory);
			m_shotsLeftInBurst = m_gunBase.ShotsInBurst;
			AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(base.ResourceSink, this));
			if (base.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				m_useConveyorSystem.SetLocalValue(myObjectBuilder_SmallMissileLauncher.UseConveyorSystem);
			}
			else
			{
				m_useConveyorSystem.SetLocalValue(newValue: false);
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			LoadDummies();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += m_ammoInventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= m_ammoInventory_ContentsChanged;
			}
		}

		private void LoadDummies()
		{
			MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(base.BlockDefinition.Model);
			m_gunBase.LoadDummies(modelOnlyDummies.Dummies);
			if (!m_gunBase.HasDummies)
			{
				foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
				{
					if (dummy.Key.ToLower().Contains("barrel"))
					{
						m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix);
					}
				}
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void m_ammoInventory_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SmallMissileLauncher obj = (MyObjectBuilder_SmallMissileLauncher)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			obj.GunBase = m_gunBase.GetObjectBuilder();
			return obj;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		private Vector3 GetSmokePosition()
		{
			return m_gunBase.GetMuzzleWorldPosition() - base.WorldMatrix.Forward * 0.5;
		}

		public void OnControlAcquired(MyCharacter owner)
		{
			Controller = owner;
		}

		public void OnControlReleased()
		{
			Controller = null;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (Sync.IsServer && base.IsFunctional && UseConveyorSystem && MySession.Static.SurvivalMode)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.VolumeFillFactor * MySession.Static.BlocksInventorySizeMultiplier < 1f)
				{
					int i = m_gunBase.WeaponProperties.CurrentWeaponRateOfFire / 36 + 1;
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(m_gunBase.CurrentAmmoMagazineId, i, this, inventory, remove: false, calcImmediately: false);
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_shoot)
			{
				ShootMissile();
			}
			UpdateReloadNotification();
			m_shoot = false;
			base.NeedsUpdate &= (MyEntityUpdateEnum)(-1);
		}

		public bool Zoom(bool newKeyPress)
		{
			return false;
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out MyGunStatusEnum status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				if (fullUpdate)
				{
					MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
					Vector3D translation = muzzleWorldMatrix.Translation;
					Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
					Vector3D target = Vector3D.Zero;
					if (MyHudCrosshair.GetTarget(translation, to, ref target))
					{
						MatrixD matrix = base.WorldMatrix;
						MatrixD.Invert(ref matrix, out MatrixD result);
						Vector3D.Transform(ref target, ref result, out m_targetLocal);
					}
					else
					{
						m_targetLocal = Vector3.Zero;
					}
				}
				if (!Vector3.IsZero(m_targetLocal))
				{
					Vector3D result2 = Vector3.Zero;
					MatrixD matrix2 = base.WorldMatrix;
					Vector3D.Transform(ref m_targetLocal, ref matrix2, out result2);
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, result2);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, fullUpdate ? Vector4.One : new Vector4(0.6f, 0.6f, 0.6f, 0.6f), result2, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
			MyHud.BlockInfo.Visible = true;
			MyHud.BlockInfo.MissingComponentIndex = -1;
			MyHud.BlockInfo.BlockName = base.BlockDefinition.DisplayNameText;
			MyHud.BlockInfo.SetContextHelp(base.BlockDefinition);
			MyHud.BlockInfo.PCUCost = 0;
			MyHud.BlockInfo.BlockIcons = base.BlockDefinition.Icons;
			MyHud.BlockInfo.BlockIntegrity = 1f;
			MyHud.BlockInfo.CriticalIntegrity = 0f;
			MyHud.BlockInfo.CriticalComponentIndex = 0;
			MyHud.BlockInfo.OwnershipIntegrity = 0f;
			MyHud.BlockInfo.BlockBuiltBy = 0L;
			MyHud.BlockInfo.GridSize = MyCubeSize.Small;
			MyHud.BlockInfo.Components.Clear();
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out MyGunStatusEnum status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				MatrixD muzzleWorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				Vector3D translation = muzzleWorldMatrix.Translation;
				Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
				Vector3D target = Vector3D.Zero;
				if (MyHudCrosshair.GetTarget(translation, to, ref target))
				{
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, target);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, Vector4.One, target, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
		}

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		private void StartSound(MySoundPair cueEnum)
		{
			m_gunBase.StartShootSound(m_soundEmitter);
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return base.WorldMatrix.Forward;
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (action != 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			if (!m_gunBase.HasAmmoMagazines)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				status = MyGunStatusEnum.Cooldown;
				return false;
			}
			if (m_shotsLeftInBurst == 0 && m_gunBase.ShotsInBurst > 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!HasPlayerAccess(shooter))
			{
				status = MyGunStatusEnum.AccessDenied;
				return false;
			}
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				status = MyGunStatusEnum.OutOfPower;
				return false;
			}
			if (!base.IsFunctional)
			{
				status = MyGunStatusEnum.NotFunctional;
				return false;
			}
			if (!base.Enabled)
			{
				status = MyGunStatusEnum.Disabled;
				return false;
			}
			if (!MySession.Static.CreativeMode && !m_gunBase.HasEnoughAmmunition())
			{
				status = MyGunStatusEnum.OutOfAmmo;
				return false;
			}
			return true;
		}

		public virtual void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (m_shootingBegun && m_lateStartRandom > m_currentLateStart)
			{
				m_currentLateStart++;
				return;
			}
			m_shoot = true;
			m_shootDirection = direction;
			m_gunBase.ConsumeAmmo();
			m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ShootIntervalInMiliseconds;
			if (m_gunBase.ShotsInBurst > 0)
			{
				m_shotsLeftInBurst--;
				if (m_shotsLeftInBurst <= 0)
				{
					m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
					m_shotsLeftInBurst = m_gunBase.ShotsInBurst;
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public new void EndShoot(MyShootActionEnum action)
		{
			base.EndShoot(action);
			m_currentLateStart = 0;
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode)
			{
				m_gunBase.StartNoAmmoSound(m_soundEmitter);
			}
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				if (m_safezoneNotification == null)
				{
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_ShootingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void ShootMissile()
		{
			if (m_gunBase == null)
			{
				MySandboxGame.Log.WriteLine("Missile launcher barrel null");
				return;
			}
			if (base.Parent.Physics == null || base.Parent.Physics.RigidBody == null)
			{
				MySandboxGame.Log.WriteLine("Missile launcher parent physics null");
				return;
			}
			Vector3 linearVelocity = base.Parent.Physics.LinearVelocity;
			ShootMissile(linearVelocity);
		}

		public void ShootMissile(Vector3 velocity)
		{
			StartSound(m_gunBase.ShootSound);
			if (Sync.IsServer)
			{
				m_gunBase.Shoot(velocity, (Controller != null) ? Controller.IsUsing : null);
			}
		}

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			m_gunBase.WorldMatrix = base.WorldMatrix;
		}

		private void UpdateReloadNotification()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds > m_nextNotificationTime)
			{
				m_reloadNotification = null;
			}
			if (Controller != MySession.Static.LocalCharacter)
			{
				if (m_reloadNotification != null)
				{
					MyHud.Notifications.Remove(m_reloadNotification);
					m_reloadNotification = null;
				}
			}
			else if (m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds && m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds > m_gunBase.ShootIntervalInMiliseconds)
			{
				ShowReloadNotification(m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

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

		public override bool CanOperate()
		{
			return CheckIsWorking();
		}

		public override void ShootFromTerminal(Vector3 direction)
		{
			base.ShootFromTerminal(direction);
			Shoot(MyShootActionEnum.PrimaryAction, direction, null, null);
		}

		public override void StopShootFromTerminal()
		{
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		public void MissileShootEffect()
		{
			m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallMissileLauncher x) => x.OnShootMissile, builder);
		}

		[Event(null, 854)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Add(builder);
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallMissileLauncher x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 865)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Remove(entityId);
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public PullInformation GetPullInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
