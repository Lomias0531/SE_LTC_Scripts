using Havok;
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
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
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

namespace Sandbox.Game.Weapons
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SmallGatlingGun))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySmallGatlingGun),
		typeof(Sandbox.ModAPI.Ingame.IMySmallGatlingGun)
	})]
	public class MySmallGatlingGun : MyUserControllableGun, IMyGunObject<MyGunBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallGatlingGun, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallGatlingGun, IMyMissileGunObject
	{
		protected sealed class OnShootMissile_003C_003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallGatlingGun, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallGatlingGun @this, in MyObjectBuilder_Missile builder, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnShootMissile(builder);
			}
		}

		protected sealed class OnRemoveMissile_003C_003ESystem_Int64 : ICallSite<MySmallGatlingGun, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySmallGatlingGun @this, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveMissile(entityId);
			}
		}

		protected class m_lateStartRandom_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lateStartRandom;
				ISyncType result = lateStartRandom = new Sync<int, SyncDirection.FromServer>(P_1, P_2);
				((MySmallGatlingGun)P_0).m_lateStartRandom = (Sync<int, SyncDirection.FromServer>)lateStartRandom;
				return result;
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MySmallGatlingGun)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private class Sandbox_Game_Weapons_MySmallGatlingGun_003C_003EActor : IActivator, IActivator<MySmallGatlingGun>
		{
			private sealed override object CreateInstance()
			{
				return new MySmallGatlingGun();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySmallGatlingGun CreateInstance()
			{
				return new MySmallGatlingGun();
			}

			MySmallGatlingGun IActivator<MySmallGatlingGun>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const int SMOKE_OVERTIME_LENGTH = 120;

		private int m_lastTimeShoot;

		private float m_rotationTimeout;

		private bool m_cannonMotorEndPlayed;

		private ShootStateEnum currentState;

		private int m_shootOvertime;

		private int m_smokeOvertime;

		private readonly Sync<int, SyncDirection.FromServer> m_lateStartRandom;

		private int m_currentLateStart;

		private float m_muzzleFlashLength;

		private float m_muzzleFlashRadius;

		private int m_smokesToGenerate;

		private MyEntity3DSoundEmitter m_soundEmitterRotor;

		private MyEntity m_barrel;

		private MyParticleEffect m_smokeEffect;

		private MyParticleEffect m_flashEffect;

		private MyGunBase m_gunBase;

		private Vector3D m_targetLocal = Vector3.Zero;

		private List<HkHitInfo> m_hits = new List<HkHitInfo>();

		private MyHudNotification m_safezoneNotification;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private MyEntity[] m_shootIgnoreEntities;

		public int LastTimeShoot => m_lastTimeShoot;

		public int LateStartRandom => m_lateStartRandom.Value;

		public float MuzzleFlashLength => m_muzzleFlashLength;

		public float MuzzleFlashRadius => m_muzzleFlashRadius;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool IsSkinnable => false;

		public float BackkickForcePerSecond => m_gunBase.BackkickForcePerSecond;

		public float ShakeAmount
		{
			get;
			protected set;
		}

		public float ProjectileCountMultiplier => 0f;

		public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

		public new MyDefinitionId DefinitionId => base.BlockDefinition.Id;

		public bool IsShooting => MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds * 2;

		public int ShootDirectionUpdateTime => 0;

		private bool UseConveyorSystem
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

		bool Sandbox.ModAPI.Ingame.IMySmallGatlingGun.UseConveyorSystem => m_useConveyorSystem;

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
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
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

		public MySmallGatlingGun()
		{
			m_shootIgnoreEntities = new MyEntity[1]
			{
				this
			};
			CreateTerminalControls();
			m_lastTimeShoot = -60000;
			m_smokesToGenerate = 0;
			m_cannonMotorEndPlayed = true;
			m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_gunBase = new MyGunBase();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME);
			base.Render = new MyRenderComponentSmallGatlingGun();
			AddDebugRenderComponent(new MyDebugRenderComponentSmallGatlingGun(this));
			base.SyncType.Append(m_gunBase);
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySmallGatlingGun>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MySmallGatlingGun> obj = new MyTerminalControlOnOffSwitch<MySmallGatlingGun>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MySmallGatlingGun x) => x.UseConveyorSystem),
					Setter = delegate(MySmallGatlingGun x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SmallGatlingGun obj = (MyObjectBuilder_SmallGatlingGun)base.GetObjectBuilderCubeBlock(copy);
			obj.GunBase = m_gunBase.GetObjectBuilder();
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_SmallGatlingGun myObjectBuilder_SmallGatlingGun = objectBuilder as MyObjectBuilder_SmallGatlingGun;
			MyWeaponBlockDefinition myWeaponBlockDefinition = base.BlockDefinition as MyWeaponBlockDefinition;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			m_soundEmitterRotor = new MyEntity3DSoundEmitter(this);
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = ((myWeaponBlockDefinition == null) ? new MyInventory(0.064f, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive) : new MyInventory(myWeaponBlockDefinition.InventoryMaxVolume, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive));
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_SmallGatlingGun.Inventory);
			}
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(myWeaponBlockDefinition.ResourceSinkGroup, 0.0002f, () => base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			m_gunBase.Init(myObjectBuilder_SmallGatlingGun.GunBase, base.BlockDefinition, this);
			base.Init(objectBuilder, cubeGrid);
			if (Sync.IsServer)
			{
				m_lateStartRandom.Value = MyUtils.GetRandomInt(0, 30);
			}
			base.ResourceSink.Update();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(base.ResourceSink, this));
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_SmallGatlingGun.UseConveyorSystem);
			base.IsWorkingChanged += MySmallGatlingGun_IsWorkingChanged;
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += AmmoInventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= AmmoInventory_ContentsChanged;
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		private void AmmoInventory_ContentsChanged(MyInventoryBase obj)
		{
			m_gunBase.RefreshAmmunitionAmount();
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (m_soundEmitterRotor != null)
			{
				m_soundEmitterRotor.StopSound(forced: true);
			}
			if (m_smokeEffect != null)
			{
				m_smokeEffect.Stop(instant: false);
				m_smokeEffect = null;
			}
			if (m_flashEffect != null)
			{
				m_flashEffect.Stop();
				m_flashEffect = null;
			}
			base.Closing();
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

		protected override void WorldPositionChanged(object source)
		{
			base.WorldPositionChanged(source);
			if (m_barrel != null)
			{
				m_gunBase.WorldMatrix = m_barrel.PositionComp.WorldMatrix;
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			if (base.Subparts.TryGetValue("Barrel", out MyEntitySubpart value))
			{
				m_barrel = value;
			}
		}

		private void MySmallGatlingGun_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.IsWorking)
			{
				if (currentState == ShootStateEnum.Continuous)
				{
					StartLoopSound();
				}
			}
			else
			{
				StopLoopSound();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.PositionComp == null)
			{
				return;
			}
			bool flag = m_flashEffect == null;
			bool flag2 = currentState != ShootStateEnum.Off;
			if (flag == flag2)
			{
				if (flag)
				{
					MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", base.PositionComp.WorldMatrix, out m_flashEffect);
					if (currentState == ShootStateEnum.Once)
					{
						m_smokesToGenerate = 10;
						m_shootOvertime = 5;
						currentState = ShootStateEnum.Off;
					}
				}
				else if (m_shootOvertime <= 0)
				{
					if (m_flashEffect != null)
					{
						m_flashEffect.Stop();
						m_flashEffect = null;
					}
				}
				else
				{
					m_shootOvertime--;
				}
			}
			float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
			amount = MathHelper.SmoothStep(0f, 1f, amount);
			float num = amount * (MathF.PI * 4f) * 0.0166666675f;
			if (num != 0f && m_barrel != null && m_barrel.PositionComp != null)
			{
				m_barrel.PositionComp.LocalMatrix = Matrix.CreateRotationZ(num) * m_barrel.PositionComp.LocalMatrix;
			}
			if (num == 0f && !base.HasDamageEffect && m_smokeOvertime <= 0 && currentState == ShootStateEnum.Off)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			SmokesToGenerateDecrease();
			if (m_smokesToGenerate > 0)
			{
				m_smokeOvertime = 120;
				if (MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) < 150.0)
				{
					if (m_smokeEffect == null)
					{
						MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", base.PositionComp.WorldMatrix, out m_smokeEffect);
					}
					else if (m_smokeEffect.IsEmittingStopped)
					{
						m_smokeEffect.Play();
						m_smokeEffect.WorldMatrix = base.PositionComp.WorldMatrix;
					}
				}
			}
			else
			{
				m_smokeOvertime--;
				if (m_smokeEffect != null && !m_smokeEffect.IsEmittingStopped)
				{
					m_smokeEffect.StopEmitting();
				}
				if (m_flashEffect != null)
				{
					m_flashEffect.Stop();
					m_flashEffect = null;
				}
			}
			if (m_smokeEffect != null)
			{
				m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(GetWeaponMuzzleWorldPosition());
				m_smokeEffect.UserBirthMultiplier = m_smokesToGenerate / 10 * 10;
			}
			if (m_flashEffect != null)
			{
				m_flashEffect.WorldMatrix = base.PositionComp.WorldMatrix;
				m_flashEffect.SetTranslation(GetWeaponMuzzleWorldPosition());
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (!MySession.Static.SurvivalMode || !Sync.IsServer || !base.IsWorking || !m_useConveyorSystem || !(this.GetInventory().VolumeFillFactor < 0.6f))
			{
				return;
			}
			MyAmmoMagazineDefinition currentAmmoMagazineDefinition = m_gunBase.CurrentAmmoMagazineDefinition;
			if (currentAmmoMagazineDefinition != null)
			{
				MyInventory inventory = this.GetInventory();
				MyFixedPoint myFixedPoint = MyFixedPoint.Floor((inventory.MaxVolume - inventory.CurrentVolume) * (1f / currentAmmoMagazineDefinition.Volume));
				if (!(myFixedPoint == 0))
				{
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(m_gunBase.CurrentAmmoMagazineId, myFixedPoint, this, inventory, remove: false, calcImmediately: false);
				}
			}
		}

		private void ClampSmokesToGenerate()
		{
			m_smokesToGenerate = MyUtils.GetClampInt(m_smokesToGenerate, 0, 50);
		}

		private void SmokesToGenerateIncrease()
		{
			m_smokesToGenerate += 19;
			ClampSmokesToGenerate();
		}

		private void SmokesToGenerateDecrease()
		{
			m_smokesToGenerate--;
			ClampSmokesToGenerate();
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return base.PositionComp.WorldMatrix.Forward;
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			status = MyGunStatusEnum.OK;
			if (!MySessionComponentSafeZones.IsActionAllowed(base.CubeGrid, MySafeZoneAction.Shooting, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			if (action != 0)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (base.Parent == null || base.Parent.Physics == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!m_gunBase.HasAmmoMagazines)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
			{
				status = MyGunStatusEnum.Cooldown;
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
			if (m_barrel == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			return true;
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if (base.Parent.Physics == null)
			{
				return;
			}
			if (m_shootingBegun && (int)m_lateStartRandom > m_currentLateStart && currentState == ShootStateEnum.Continuous)
			{
				m_currentLateStart++;
				return;
			}
			if (currentState == ShootStateEnum.Off)
			{
				currentState = ShootStateEnum.Once;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			m_muzzleFlashLength = MyUtils.GetRandomFloat(3f, 4f) * base.CubeGrid.GridSize;
			m_muzzleFlashRadius = MyUtils.GetRandomFloat(0.9f, 1.5f) * base.CubeGrid.GridSize;
			base.Render.NeedsDrawFromParent = true;
			SmokesToGenerateIncrease();
			PlayShotSound();
			m_gunBase.Shoot(base.Parent.Physics.LinearVelocity);
			m_gunBase.ConsumeAmmo();
			if (BackkickForcePerSecond > 0f && !base.CubeGrid.Physics.IsStatic)
			{
				base.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, -direction * BackkickForcePerSecond, base.PositionComp.GetPosition(), null);
			}
			m_cannonMotorEndPlayed = false;
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void BeginShoot(MyShootActionEnum action)
		{
			currentState = ShootStateEnum.Continuous;
			base.BeginShoot(action);
			StartLoopSound();
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			currentState = ShootStateEnum.Off;
			base.EndShoot(action);
			m_currentLateStart = 0;
			StopLoopSound();
			if (m_flashEffect != null)
			{
				m_flashEffect.Stop();
				m_flashEffect = null;
			}
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode && this.GetInventory().GetItemAmount(m_gunBase.CurrentAmmoMagazineId) < 1)
			{
				StartNoAmmoSound();
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

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
		}

		public void OnControlAcquired(MyCharacter owner)
		{
		}

		public void OnControlReleased()
		{
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			CanShoot(MyShootActionEnum.PrimaryAction, playerId, out MyGunStatusEnum status);
			if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
			{
				if (fullUpdate)
				{
					Vector3D from = base.PositionComp.GetPosition() + base.PositionComp.WorldMatrix.Forward;
					Vector3D to = base.PositionComp.GetPosition() + 200.0 * base.PositionComp.WorldMatrix.Forward;
					Vector3D target = Vector3D.Zero;
					if (MyHudCrosshair.GetTarget(from, to, ref target))
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
				Vector3D from = base.PositionComp.GetPosition() + base.PositionComp.WorldMatrix.Forward;
				Vector3D to = base.PositionComp.GetPosition() + 200.0 * base.PositionComp.WorldMatrix.Forward;
				Vector3D target = Vector3D.Zero;
				if (MyHudCrosshair.GetTarget(from, to, ref target))
				{
					float num = (float)Vector3D.Distance(MySector.MainCamera.Position, target);
					MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, new Vector4(1f, 1f, 1f, 1f), target, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
		}

		private void UpdatePower()
		{
			base.ResourceSink.Update();
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
		}

		public void StartNoAmmoSound()
		{
			m_gunBase.StartNoAmmoSound(m_soundEmitter);
		}

		private void StopLoopSound()
		{
			if (m_soundEmitter != null && m_soundEmitter.IsPlaying && m_soundEmitter.Loop)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (m_soundEmitterRotor != null && m_soundEmitterRotor.IsPlaying && m_soundEmitterRotor.Loop)
			{
				m_soundEmitterRotor.StopSound(forced: true);
				m_soundEmitterRotor.PlaySound(m_gunBase.SecondarySound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: true);
			}
		}

		private void PlayShotSound()
		{
			m_gunBase.StartShootSound(m_soundEmitter);
		}

		private void StartLoopSound()
		{
			if (m_soundEmitterRotor != null && m_gunBase.SecondarySound != MySoundPair.Empty && (!m_soundEmitterRotor.IsPlaying || !m_soundEmitterRotor.Loop) && base.IsWorking)
			{
				if (m_soundEmitterRotor.IsPlaying)
				{
					m_soundEmitterRotor.StopSound(forced: true);
				}
				m_soundEmitterRotor.PlaySound(m_gunBase.SecondarySound, stopPrevious: true);
			}
		}

		public int GetAmmunitionAmount()
		{
			return m_gunBase.GetTotalAmmunitionAmount();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				GetBarrelAndMuzzle();
			}
			else
			{
				m_barrel = null;
			}
		}

		private void GetBarrelAndMuzzle()
		{
			if (!base.Subparts.TryGetValue("Barrel", out MyEntitySubpart value))
			{
				return;
			}
			m_barrel = value;
			MyModel model = m_barrel.Model;
			m_gunBase.LoadDummies(model.Dummies);
			if (!m_gunBase.HasDummies)
			{
				if (model.Dummies.ContainsKey("Muzzle"))
				{
					m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, model.Dummies["Muzzle"].Matrix);
					return;
				}
				Matrix localMatrix = Matrix.CreateTranslation(new Vector3(0f, 0f, -1f));
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, localMatrix);
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

		public void UpdateSoundEmitter()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public bool SupressShootAnimation()
		{
			return false;
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

		public void MissileShootEffect()
		{
			m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
		}

		public void ShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallGatlingGun x) => x.OnShootMissile, builder);
		}

		[Event(null, 1065)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnShootMissile(MyObjectBuilder_Missile builder)
		{
			MyMissiles.Add(builder);
		}

		public void RemoveMissile(long entityId)
		{
			MyMultiplayer.RaiseEvent(this, (MySmallGatlingGun x) => x.OnRemoveMissile, entityId);
		}

		[Event(null, 1078)]
		[Reliable]
		[Broadcast]
		private void OnRemoveMissile(long entityId)
		{
			MyMissiles.Remove(entityId);
		}
	}
}
