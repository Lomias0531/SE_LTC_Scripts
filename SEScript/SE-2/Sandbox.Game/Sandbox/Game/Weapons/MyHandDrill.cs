#define VRAGE
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_HandDrill), true)]
	public class MyHandDrill : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IMyGunBaseUser, IMyHandDrill, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		private class Sandbox_Game_Weapons_MyHandDrill_003C_003EActor : IActivator, IActivator<MyHandDrill>
		{
			private sealed override object CreateInstance()
			{
				return new MyHandDrill();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHandDrill CreateInstance()
			{
				return new MyHandDrill();
			}

			MyHandDrill IActivator<MyHandDrill>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float SPIKE_THRUST_DISTANCE_HALF = 0.03f;

		private const float SPIKE_THRUST_PERIOD_IN_SECONDS = 0.06f;

		private const float SPIKE_SLOWDOWN_TIME_IN_SECONDS = 0.5f;

		private const float SPIKE_MAX_ROTATION_SPEED = -25f;

		private int m_lastTimeDrilled;

		private MyDrillBase m_drillBase;

		private MyCharacter m_owner;

		private MyDefinitionId m_handItemDefId;

		private MyStringHash m_drillMat;

		private MyEntitySubpart m_spike;

		private Vector3 m_spikeBasePos;

		private float m_spikeRotationAngle;

		private float m_spikeThrustPosition;

		private int m_spikeLastUpdateTime;

		private MyOreDetectorComponent m_oreDetectorBase = new MyOreDetectorComponent();

		private MyEntity[] m_shootIgnoreEntities;

		protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();

		private float m_speedMultiplier = 1f;

		private MyHudNotification m_safezoneNotification;

		private bool m_firstDraw;

		private MyResourceSinkComponent m_sinkComp;

		private MyPhysicalItemDefinition m_physItemDef;

		private static MyDefinitionId m_physicalItemId;

		private bool m_tryingToDrill;

		private bool m_firstTimeHeatup = true;

		private bool m_objectInDrillingRange;

		public MyResourceSinkComponent SinkComp
		{
			get
			{
				return m_sinkComp;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceSinkComponent)))
				{
					base.Components.Remove<MyResourceSinkComponent>();
				}
				base.Components.Add(value);
				m_sinkComp = value;
			}
		}

		public float BackkickForcePerSecond => 0f;

		public float ShakeAmount
		{
			get
			{
				return 2.5f;
			}
			protected set
			{
			}
		}

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

		public bool EnabledInWorldRules => true;

		public MyObjectBuilder_PhysicalGunObject PhysicalObject
		{
			get;
			private set;
		}

		public bool IsShooting => m_drillBase.IsDrilling;

		public MyEntity DrilledEntity => m_drillBase.DrilledEntity;

		public bool CollectingOre => m_drillBase.CollectingOre;

		public bool ForceAnimationInsteadOfIK => false;

		public bool IsBlocking => false;

		public int ShootDirectionUpdateTime => 200;

		public bool IsSkinnable => true;

		public new MyDefinitionId DefinitionId => m_handItemDefId;

		public MyToolBase GunBase => null;

		public MyPhysicalItemDefinition PhysicalItemDefinition => m_physItemDef;

		public int CurrentAmmunition
		{
			get;
			set;
		}

		public int CurrentMagazineAmmunition
		{
			get;
			set;
		}

		MyEntity[] IMyGunBaseUser.IgnoreEntities => m_shootIgnoreEntities;

		MyEntity IMyGunBaseUser.Weapon => this;

		MyEntity IMyGunBaseUser.Owner => m_owner;

		IMyMissileGunObject IMyGunBaseUser.Launcher => null;

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

		MyDefinitionId IMyGunBaseUser.PhysicalItemId => default(MyDefinitionId);

		MyInventory IMyGunBaseUser.WeaponInventory => null;

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

		static MyHandDrill()
		{
			m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "HandDrillItem");
		}

		public MyHandDrill()
		{
			m_shootIgnoreEntities = new MyEntity[1]
			{
				this
			};
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "HandDrillItem");
			if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
			{
				m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
			}
			PhysicalObject = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(m_physicalItemId);
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = WorldPositionChanged;
			m_handItemDefId = objectBuilder.GetId();
			MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref m_handItemDefId);
			MyHandDrillDefinition myHandDrillDefinition = myHandItemDefinition as MyHandDrillDefinition;
			m_drillMat = myHandDrillDefinition.ToolMaterial;
			m_speedMultiplier = 1f / myHandDrillDefinition.SpeedMultiplier;
			m_drillBase = new MyDrillBase(this, "Smoke_HandDrillDust", "Smoke_HandDrillDustStones", "Collision_Sparks_HandDrill", new MyDrillSensorRayCast(-0.5f, 2.15f, PhysicalItemDefinition), new MyDrillCutOut(1f, 0.35f * myHandDrillDefinition.DistanceMultiplier), 0.5f, -0.25f, 0.35f);
			m_drillBase.VoxelHarvestRatio = 0.009f * myHandDrillDefinition.HarvestRatioMultiplier * MySession.Static.Settings.HarvestRatioMultiplier;
			m_drillBase.ParticleOffset = myHandDrillDefinition.ParticleOffset;
			AddDebugRenderComponent(new MyDebugRenderCompomentDrawDrillBase(m_drillBase));
			base.Init(objectBuilder);
			m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(m_physicalItemId);
			Init(null, m_physItemDef.Model, null, null);
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			m_spike = base.Subparts["Spike"];
			m_spikeBasePos = m_spike.PositionComp.LocalMatrix.Translation;
			m_drillBase.IgnoredEntities.Add(this);
			m_drillBase.UpdatePosition(base.PositionComp.WorldMatrix);
			PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)objectBuilder.Clone();
			PhysicalObject.GunEntity.EntityId = base.EntityId;
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_oreDetectorBase.DetectionRadius = 20f;
			m_oreDetectorBase.OnCheckControl = OnCheckControl;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), 4E-05f, () => (!m_tryingToDrill) ? 1E-06f : SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			SinkComp = myResourceSinkComponent;
			foreach (ToolSound toolSound in myHandItemDefinition.ToolSounds)
			{
				if (toolSound.type != null && toolSound.subtype != null && toolSound.sound != null && toolSound.type.Equals("Main"))
				{
					if (toolSound.subtype.Equals("Idle"))
					{
						m_drillBase.m_idleSoundLoop = new MySoundPair(toolSound.sound);
					}
					if (toolSound.subtype.Equals("Soundset"))
					{
						m_drillBase.m_drillMaterial = MyStringHash.GetOrCompute(toolSound.sound);
					}
				}
			}
		}

		public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeDrilled) < 1000f * m_speedMultiplier)
			{
				status = MyGunStatusEnum.Cooldown;
				m_firstTimeHeatup = false;
				return false;
			}
			if (Owner == null)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(Owner, MySafeZoneAction.Drilling, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			SinkComp.Update();
			if (!SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				status = MyGunStatusEnum.OutOfPower;
				return false;
			}
			status = MyGunStatusEnum.OK;
			return true;
		}

		public void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			MyAnalyticsHelper.ReportActivityStartIf(!IsShooting, Owner, "Drilling", "Character", "HandTools", "HandDrill");
			if (!DoDrillAction(action == MyShootActionEnum.PrimaryAction) && IsShooting && Owner != null)
			{
				Owner.EndShoot(action);
			}
		}

		public void BeginShoot(MyShootActionEnum action)
		{
		}

		public void EndShoot(MyShootActionEnum action)
		{
			MyAnalyticsHelper.ReportActivityEnd(Owner, "Drilling");
			m_drillBase.StopDrill();
			m_tryingToDrill = false;
			m_firstTimeHeatup = true;
			m_objectInDrillingRange = false;
			SinkComp.Update();
			m_lastTimeDrilled = MySandboxGame.TotalGamePlayTimeInMilliseconds - (int)(1000f * m_speedMultiplier);
			m_isActionDoubleClicked[action] = false;
		}

		private bool DoDrillAction(bool collectOre)
		{
			m_tryingToDrill = true;
			SinkComp.Update();
			if (!SinkComp.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 4E-05f))
			{
				m_tryingToDrill = false;
				return false;
			}
			m_lastTimeDrilled = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_objectInDrillingRange = m_drillBase.Drill(collectOre, speedMultiplier: m_speedMultiplier, performCutout: !m_firstTimeHeatup, assignDamagedMaterial: true);
			m_spikeLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			return true;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_drillBase.UpdateAfterSimulation();
			if (IsShooting)
			{
				CreateCollisionSparks();
			}
			if (!m_tryingToDrill && !(m_drillBase.AnimationMaxSpeedRatio > 0f))
			{
				return;
			}
			float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_spikeLastUpdateTime) / 1000f;
			if (m_objectInDrillingRange && Owner != null && Owner.ControllerInfo.IsLocallyControlled() && !MySession.Static.IsCameraUserAnySpectator())
			{
				m_drillBase.PerformCameraShake();
			}
			m_spikeRotationAngle += num * m_drillBase.AnimationMaxSpeedRatio * -25f;
			if (m_spikeRotationAngle > MathF.PI * 2f)
			{
				m_spikeRotationAngle -= MathF.PI * 2f;
			}
			if (m_spikeRotationAngle < MathF.PI * 2f)
			{
				m_spikeRotationAngle += MathF.PI * 2f;
			}
			m_spikeThrustPosition += num * m_drillBase.AnimationMaxSpeedRatio / 0.06f;
			if (m_spikeThrustPosition > 1f)
			{
				m_spikeThrustPosition -= 2f;
				if (Owner != null && m_objectInDrillingRange)
				{
					Owner.WeaponPosition.AddBackkick(0.035f);
				}
			}
			m_spikeLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_spike.PositionComp.LocalMatrix = Matrix.CreateRotationZ(m_spikeRotationAngle) * Matrix.CreateTranslation(m_spikeBasePos + Math.Abs(m_spikeThrustPosition) * Vector3.UnitZ * 0.03f);
		}

		private void CreateCollisionSparks()
		{
			_ = m_drillBase.Sensor.Center;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (m_drillBase.DrilledEntity != null)
			{
				flag = (m_drillBase.DrilledEntity is MyCubeGrid);
				flag2 = (m_drillBase.DrilledEntity is MyVoxelBase);
				flag3 = (m_drillBase.DrilledEntity is MyEnvironmentSector);
				if (flag)
				{
					Vector3D position = Vector3D.Transform(m_drillBase.ParticleOffset, base.WorldMatrix);
					if (m_drillBase.SparkEffect != null)
					{
						if (m_drillBase.SparkEffect.IsEmittingStopped)
						{
							m_drillBase.SparkEffect.Play();
						}
						m_drillBase.SparkEffect.WorldMatrix = MatrixD.CreateWorld(position, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
					}
					else
					{
						MyParticlesManager.TryCreateParticleEffect("Collision_Sparks_HandDrill", MatrixD.CreateWorld(position, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up), out m_drillBase.SparkEffect);
					}
				}
				if (flag2)
				{
					Vector3D position2 = Vector3D.Transform(m_drillBase.ParticleOffset, base.WorldMatrix);
					string collisionEffect = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_drillMat, (m_drillBase.DrilledEntity as MyVoxelBase).Physics.GetMaterialAt(m_drillBase.DrilledEntityPoint));
					if (string.IsNullOrEmpty(collisionEffect))
					{
						m_drillBase.CurrentDustEffectName = "Smoke_HandDrillDustStones";
					}
					else
					{
						m_drillBase.CurrentDustEffectName = collisionEffect;
					}
					if ((m_drillBase.DustParticles == null || m_drillBase.DustParticles.GetName() != m_drillBase.CurrentDustEffectName) && m_drillBase.DustParticles != null)
					{
						m_drillBase.DustParticles.Stop(instant: false);
						m_drillBase.DustParticles = null;
					}
					if (m_drillBase.DustParticles != null)
					{
						if (m_drillBase.DustParticles.IsEmittingStopped)
						{
							m_drillBase.DustParticles.Play();
						}
						m_drillBase.DustParticles.WorldMatrix = MatrixD.CreateWorld(position2, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
					}
					else
					{
						MyParticlesManager.TryCreateParticleEffect(m_drillBase.CurrentDustEffectName, MatrixD.CreateWorld(position2, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up), out m_drillBase.DustParticles);
					}
				}
				if (flag3)
				{
					Vector3D position3 = Vector3D.Transform(m_drillBase.ParticleOffset, base.WorldMatrix);
					string currentDustEffectName = m_drillBase.CurrentDustEffectName;
					m_drillBase.CurrentDustEffectName = "Tree_Drill";
					if (currentDustEffectName != m_drillBase.CurrentDustEffectName && m_drillBase.DustParticles != null)
					{
						m_drillBase.DustParticles.Stop(instant: false);
						m_drillBase.DustParticles = null;
					}
					if (m_drillBase.DustParticles != null)
					{
						if (m_drillBase.DustParticles.IsEmittingStopped)
						{
							m_drillBase.DustParticles.Play();
						}
						m_drillBase.DustParticles.WorldMatrix = MatrixD.CreateWorld(position3, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
					}
					else
					{
						MyParticlesManager.TryCreateParticleEffect(m_drillBase.CurrentDustEffectName, MatrixD.CreateWorld(position3, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up), out m_drillBase.DustParticles);
					}
				}
			}
			if (m_drillBase.SparkEffect != null && !flag)
			{
				m_drillBase.SparkEffect.StopEmitting();
			}
			if (m_drillBase.DustParticles != null && !flag2 && !flag3)
			{
				m_drillBase.DustParticles.StopEmitting();
			}
		}

		public void WorldPositionChanged(object source)
		{
			if (m_owner != null)
			{
				MatrixD identity = MatrixD.Identity;
				identity.Right = m_owner.WorldMatrix.Right;
				identity.Forward = m_owner.WeaponPosition.LogicalOrientationWorld;
				identity.Up = Vector3D.Normalize(identity.Right.Cross(identity.Forward));
				identity.Translation = m_owner.WeaponPosition.LogicalPositionWorld;
				m_drillBase.UpdatePosition(identity);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			m_drillBase.Close();
		}

		private Vector3 ComputeDrillSensorCenter()
		{
			return base.PositionComp.WorldMatrix.Forward * 1.2999999523162842 + base.PositionComp.WorldMatrix.Translation;
		}

		public void OnControlAcquired(MyCharacter owner)
		{
			m_owner = owner;
			if (owner != null)
			{
				m_shootIgnoreEntities = new MyEntity[2]
				{
					this,
					owner
				};
			}
			m_drillBase.OutputInventory = null;
			m_drillBase.IgnoredEntities.Add(m_owner);
			m_firstDraw = true;
		}

		public void OnControlReleased()
		{
			if (m_drillBase != null)
			{
				m_drillBase.IgnoredEntities.Remove(m_owner);
				m_drillBase.StopDrill();
				m_tryingToDrill = false;
				SinkComp.Update();
				m_drillBase.OutputInventory = null;
			}
			if (m_owner != null && m_owner.ControllerInfo != null)
			{
				m_oreDetectorBase.Clear();
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
				MyHud.Crosshair.Recenter();
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

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			m_drillBase.Force2DSound = (m_owner != null && m_owner.IsInFirstPersonView && m_owner == MySession.Static.LocalCharacter);
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			m_oreDetectorBase.Update(base.PositionComp.GetPosition(), base.EntityId);
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			UpdateSoundEmitter();
		}

		public void UpdateSoundEmitter()
		{
			Vector3 velocityVector = Vector3.Zero;
			if (m_owner != null)
			{
				m_owner.GetLinearVelocity(ref velocityVector);
			}
			m_drillBase.UpdateSoundEmitter(velocityVector);
		}

		private bool OnCheckControl()
		{
			if (MySession.Static.ControlledEntity != null)
			{
				return (MyEntity)MySession.Static.ControlledEntity == Owner;
			}
			return false;
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return base.PositionComp.WorldMatrix.Forward;
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (Owner == MySession.Static.LocalCharacter)
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
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_DrillingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (IsShooting && Owner != null)
			{
				Owner.EndShoot(action);
			}
		}

		public int GetAmmunitionAmount()
		{
			return 0;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
			objectBuilder.SubtypeName = m_handItemDefId.SubtypeName;
			return objectBuilder;
		}

		public bool SupressShootAnimation()
		{
			return false;
		}

		public bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			if (m_isActionDoubleClicked.ContainsKey(action))
			{
				return !m_isActionDoubleClicked[action];
			}
			return true;
		}

		public bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return true;
		}

		public void DoubleClicked(MyShootActionEnum action)
		{
			m_isActionDoubleClicked[action] = true;
		}
	}
}
