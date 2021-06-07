using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI.Weapons;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Lights;

namespace Sandbox.Game.Weapons
{
	public abstract class MyEngineerToolBase : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IMyEngineerToolBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		protected enum EffectType
		{
			Light,
			Effect,
			EffectSecondary
		}

		public static float GLARE_SIZE = 0.068f;

		public static readonly float DEFAULT_REACH_DISTANCE = 2f;

		protected string m_effectId = "WelderContactPoint";

		protected float EffectScale = 1f;

		protected bool HasPrimaryEffect = true;

		protected bool HasSecondaryEffect;

		protected string SecondaryEffectName = "Dummy";

		protected Vector4 SecondaryLightColor = new Vector4(0.4f, 0.5f, 1f, 1f);

		protected float SecondaryLightFalloff = 2f;

		protected float SecondaryLightRadius = 7f;

		protected float SecondaryLightIntensityLower = 0.4f;

		protected float SecondaryLightIntensityUpper = 0.5f;

		protected float SecondaryLightGlareSize = GLARE_SIZE;

		protected MyShootActionEnum? EffectAction;

		private MyShootActionEnum? m_previousEffect;

		protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();

		protected MyEntity3DSoundEmitter m_soundEmitter;

		protected MyCharacter Owner;

		protected MyToolBase m_gunBase;

		private int m_lastTimeShoot;

		protected int m_lastTimeSelected;

		protected bool m_activated;

		protected bool m_shooting;

		private MyParticleEffect m_toolEffect;

		private MyParticleEffect m_toolSecondaryEffect;

		private MyLight m_toolEffectLight;

		private int m_lastMarkTime = -1;

		private int m_markedComponent = -1;

		protected bool m_tryingToShoot;

		private bool m_wasPowered;

		protected MyCasterComponent m_raycastComponent;

		private MyResourceSinkComponent m_sinkComp;

		private int m_shootFrameCounter;

		private NumberFormatInfo m_oneDecimal = new NumberFormatInfo
		{
			NumberDecimalDigits = 1,
			PercentDecimalDigits = 1
		};

		protected MyHandItemDefinition m_handItemDef;

		protected MyPhysicalItemDefinition m_physItemDef;

		protected float m_speedMultiplier = 1f;

		protected float m_distanceMultiplier = 1f;

		private MyFlareDefinition m_flare;

		protected object LastTargetObject;

		protected int LastTargetStamp;

		public bool IsDeconstructor => false;

		public int ToolCooldownMs
		{
			get;
			private set;
		}

		public int EffectStopMs => ToolCooldownMs * 2;

		public string EffectId => m_effectId;

		public long OwnerId
		{
			get
			{
				if (Owner != null)
				{
					return Owner.EntityId;
				}
				return 0L;
			}
		}

		public long OwnerIdentityId
		{
			get
			{
				if (Owner != null)
				{
					return Owner.GetPlayerIdentityId();
				}
				return 0L;
			}
		}

		public MyToolBase GunBase => m_gunBase;

		public Vector3I TargetCube
		{
			get
			{
				if (m_raycastComponent == null || m_raycastComponent.HitBlock == null)
				{
					return Vector3I.Zero;
				}
				return m_raycastComponent.HitBlock.Position;
			}
		}

		public bool HasHitBlock => m_raycastComponent.HitBlock != null;

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

		public bool IsShooting => m_activated;

		public bool ForceAnimationInsteadOfIK => false;

		public bool IsBlocking => false;

		public bool IsPreheated => m_shootFrameCounter >= 2;

		protected bool WasJustSelected => m_lastTimeSelected == MySandboxGame.TotalGamePlayTimeInMilliseconds;

		public Vector3 SensorDisplacement
		{
			get;
			set;
		}

		protected MyInventory CharacterInventory
		{
			get;
			private set;
		}

		public MyObjectBuilder_PhysicalGunObject PhysicalObject
		{
			get;
			protected set;
		}

		public float BackkickForcePerSecond => 0f;

		public float ShakeAmount
		{
			get;
			protected set;
		}

		protected bool HasCubeHighlight
		{
			get;
			set;
		}

		public Color HighlightColor
		{
			get;
			set;
		}

		public MyStringId HighlightMaterial
		{
			get;
			set;
		}

		public bool EnabledInWorldRules => true;

		public abstract bool IsSkinnable
		{
			get;
		}

		public new MyDefinitionId DefinitionId => m_handItemDef.Id;

		int IMyGunObject<MyToolBase>.ShootDirectionUpdateTime => 200;

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

		public bool CanBeDrawn()
		{
			if (Owner != null && Owner == MySession.Static.ControlledEntity && m_raycastComponent.HitCubeGrid != null && m_raycastComponent.HitCubeGrid != null && HasCubeHighlight)
			{
				return !MyFakes.HIDE_ENGINEER_TOOL_HIGHLIGHT;
			}
			return false;
		}

		public MyEngineerToolBase(int cooldownMs)
		{
			ToolCooldownMs = cooldownMs;
			m_activated = false;
			m_wasPowered = false;
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			base.Render.NeedsDraw = true;
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = WorldPositionChanged;
			base.Render = new MyRenderComponentEngineerTool();
			AddDebugRenderComponent(new MyDebugRenderComponentEngineerTool(this));
		}

		public void Init(MyObjectBuilder_EntityBase builder, MyDefinitionId id)
		{
			Init(builder, MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(id));
		}

		public void Init(MyObjectBuilder_EntityBase builder, MyHandItemDefinition definition)
		{
			m_handItemDef = definition;
			if (definition != null)
			{
				m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(definition.Id);
				m_gunBase = new MyToolBase(m_handItemDef.MuzzlePosition, base.WorldMatrix);
			}
			else
			{
				m_gunBase = new MyToolBase(Vector3.Zero, base.WorldMatrix);
			}
			base.Init(builder);
			if (PhysicalObject != null)
			{
				PhysicalObject.GunEntity = builder;
			}
			if (definition is MyEngineerToolBaseDefinition)
			{
				m_speedMultiplier = (m_handItemDef as MyEngineerToolBaseDefinition).SpeedMultiplier;
				m_distanceMultiplier = (m_handItemDef as MyEngineerToolBaseDefinition).DistanceMultiplier;
				string flare = (m_handItemDef as MyEngineerToolBaseDefinition).Flare;
				if (flare != "")
				{
					MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), flare);
					m_flare = ((MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition) ?? new MyFlareDefinition());
				}
				else
				{
					m_flare = new MyFlareDefinition();
				}
			}
			MyDrillSensorRayCast caster = new MyDrillSensorRayCast(0f, DEFAULT_REACH_DISTANCE * m_distanceMultiplier, PhysicalItemDefinition);
			m_raycastComponent = new MyCasterComponent(caster);
			m_raycastComponent.SetPointOfReference(m_gunBase.GetMuzzleWorldPosition());
			base.Components.Add(m_raycastComponent);
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), 0.0001f, CalculateRequiredPower);
			SinkComp = myResourceSinkComponent;
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
		}

		protected virtual bool ShouldBePowered()
		{
			return m_tryingToShoot;
		}

		protected float CalculateRequiredPower()
		{
			if (!ShouldBePowered())
			{
				return 1E-06f;
			}
			return SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
		}

		private void UpdatePower()
		{
			bool flag = ShouldBePowered();
			if (flag != m_wasPowered)
			{
				m_wasPowered = flag;
				SinkComp.Update();
			}
		}

		protected IMyDestroyableObject GetTargetDestroyable()
		{
			return m_raycastComponent.HitDestroyableObj;
		}

		public MySlimBlock GetTargetBlock()
		{
			if (Sync.IsServer)
			{
				MyCharacter owner = Owner;
				if (owner == null || owner.AimedGrid != 0)
				{
					MyCubeGrid myCubeGrid = MyEntities.GetEntityById(Owner.AimedGrid) as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(Owner.AimedBlock);
						if (cubeBlock != null && Vector3D.Distance(cubeBlock.WorldPosition, Owner.WorldMatrix.Translation) <= (double)(DEFAULT_REACH_DISTANCE * m_distanceMultiplier * 3f))
						{
							return cubeBlock;
						}
					}
				}
			}
			if (ReachesCube() && m_raycastComponent.HitCubeGrid != null)
			{
				return m_raycastComponent.HitBlock;
			}
			return null;
		}

		protected virtual MySlimBlock GetTargetBlockForShoot()
		{
			return GetTargetBlock();
		}

		public MyCubeGrid GetTargetGrid()
		{
			return m_raycastComponent.HitCubeGrid;
		}

		protected bool ReachesCube()
		{
			return m_raycastComponent.HitBlock != null;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
		}

		public override void OnRemovedFromScene(object source)
		{
			RemoveHudInfo();
			base.OnRemovedFromScene(source);
			StopSecondaryEffect();
			StopEffect();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Owner == null)
			{
				return;
			}
			Vector3 localWeaponPosition = Owner.GetLocalWeaponPosition();
			Vector3D vector = m_gunBase.GetMuzzleLocalPosition();
			MatrixD rotationMatrix = base.WorldMatrix;
			Vector3D.Rotate(ref vector, ref rotationMatrix, out Vector3D result);
			m_raycastComponent.SetPointOfReference(Owner.PositionComp.GetPosition() + localWeaponPosition + result);
			if (IsShooting && IsPreheated)
			{
				if (GetTargetBlockForShoot() == null)
				{
					EffectAction = MyShootActionEnum.SecondaryAction;
					ShakeAmount = m_handItemDef.ShakeAmountNoTarget;
				}
				else
				{
					EffectAction = MyShootActionEnum.PrimaryAction;
					ShakeAmount = m_handItemDef.ShakeAmountTarget;
				}
			}
			SinkComp.Update();
			if (IsShooting && !SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
			UpdateEffect();
			CheckEffectType();
			if (Owner != null && Owner.ControllerInfo.IsLocallyHumanControlled())
			{
				if (MySession.Static.SurvivalMode)
				{
					MySession.Static.GetCameraControllerEnum();
					MyCharacter myCharacter = (MyCharacter)CharacterInventory.Owner;
					MyCubeBuilder.Static.MaxGridDistanceFrom = myCharacter.PositionComp.GetPosition() + myCharacter.WorldMatrix.Up * 1.7999999523162842;
				}
				else
				{
					MyCubeBuilder.Static.MaxGridDistanceFrom = null;
				}
			}
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
				if (Owner != null)
				{
					Vector3 velocityVector = Vector3.Zero;
					Owner.GetLinearVelocity(ref velocityVector);
					m_soundEmitter.SetVelocity(velocityVector);
				}
				m_soundEmitter.Update();
			}
		}

		private void WorldPositionChanged(object source)
		{
			m_gunBase.OnWorldPositionChanged(base.PositionComp.WorldMatrix);
			UpdateSensorPosition();
		}

		public void UpdateSensorPosition()
		{
			if (Owner != null)
			{
				MyCharacter owner = Owner;
				MatrixD newTransform = MatrixD.Identity;
				newTransform.Translation = owner.WeaponPosition.LogicalPositionWorld;
				newTransform.Right = owner.WorldMatrix.Right;
				newTransform.Forward = owner.WeaponPosition.LogicalOrientationWorld;
				newTransform.Up = Vector3.Cross(newTransform.Right, newTransform.Forward);
				m_raycastComponent.OnWorldPosChanged(ref newTransform);
			}
		}

		public virtual bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (action == MyShootActionEnum.PrimaryAction)
			{
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < ToolCooldownMs)
				{
					status = MyGunStatusEnum.Cooldown;
					return false;
				}
				status = MyGunStatusEnum.OK;
				return true;
			}
			status = MyGunStatusEnum.Failed;
			return false;
		}

		public virtual void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			m_shooting = true;
			if (action == MyShootActionEnum.PrimaryAction)
			{
				m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_shootFrameCounter++;
				m_tryingToShoot = true;
				SinkComp.Update();
				if (!SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
				{
					EffectAction = null;
				}
				else
				{
					m_activated = true;
				}
			}
		}

		public virtual void BeginShoot(MyShootActionEnum action)
		{
		}

		public bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return false;
		}

		public virtual void EndShoot(MyShootActionEnum action)
		{
			EffectAction = null;
			StopLoopSound();
			ShakeAmount = 0f;
			m_tryingToShoot = false;
			m_shooting = false;
			SinkComp.Update();
			m_activated = false;
			m_shootFrameCounter = 0;
			m_isActionDoubleClicked[action] = false;
		}

		public virtual void OnFailShoot(MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.Failed)
			{
				EffectAction = MyShootActionEnum.SecondaryAction;
			}
		}

		protected virtual void StartLoopSound(bool effect)
		{
		}

		protected virtual void StopLoopSound()
		{
		}

		protected virtual void StopSound()
		{
		}

		protected virtual MatrixD GetEffectMatrix(float muzzleOffset, EffectType effectType)
		{
			if (m_raycastComponent.HitCubeGrid == null || m_raycastComponent.HitBlock == null)
			{
				return MatrixD.CreateWorld(m_gunBase.GetMuzzleWorldPosition(), base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
			}
			float num = Vector3.Dot(m_raycastComponent.HitPosition - m_gunBase.GetMuzzleWorldPosition(), base.PositionComp.WorldMatrix.Forward);
			Vector3D vector3D = m_gunBase.GetMuzzleWorldPosition() + base.PositionComp.WorldMatrix.Forward * (num * muzzleOffset);
			return MatrixD.CreateWorld((num > 0f && muzzleOffset == 0f) ? m_gunBase.GetMuzzleWorldPosition() : vector3D, base.PositionComp.WorldMatrix.Forward, base.PositionComp.WorldMatrix.Up);
		}

		private void CheckEffectType()
		{
			if (m_previousEffect.HasValue && m_toolEffect == null)
			{
				m_previousEffect = null;
			}
			if (EffectAction == m_previousEffect)
			{
				return;
			}
			if (m_previousEffect.HasValue)
			{
				StopEffect();
			}
			m_previousEffect = null;
			if (EffectAction.HasValue && MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) < 150.0)
			{
				if (EffectAction == MyShootActionEnum.PrimaryAction && HasPrimaryEffect)
				{
					StartEffect();
					m_previousEffect = MyShootActionEnum.PrimaryAction;
				}
				else if (EffectAction == MyShootActionEnum.SecondaryAction && HasSecondaryEffect)
				{
					StartSecondaryEffect();
					m_previousEffect = MyShootActionEnum.SecondaryAction;
				}
			}
		}

		public virtual bool CanStartEffect()
		{
			return true;
		}

		protected void StartEffect()
		{
			StopEffect();
			if (!string.IsNullOrEmpty(m_effectId) && CanStartEffect())
			{
				MyParticlesManager.TryCreateParticleEffect(m_effectId, GetEffectMatrix(0.1f, EffectType.Effect), out m_toolEffect);
				if (m_toolEffect != null)
				{
					m_toolEffect.UserScale = EffectScale;
				}
				m_toolEffectLight = CreatePrimaryLight();
			}
			UpdateEffect();
		}

		protected virtual MyLight CreatePrimaryLight()
		{
			MyLight myLight = MyLights.AddLight();
			if (myLight != null)
			{
				myLight.Start(Vector3.Zero, m_handItemDef.LightColor, m_handItemDef.LightRadius, DisplayNameText + " Tool Primary");
				CreateGlare(myLight);
			}
			return myLight;
		}

		private void CreateGlare(MyLight light)
		{
			light.GlareOn = light.LightOn;
			light.GlareQuerySize = 0.2f;
			light.GlareType = MyGlareTypeEnum.Normal;
			if (m_flare != null)
			{
				light.SubGlares = m_flare.SubGlares;
				light.GlareSize = m_flare.Size;
				light.GlareIntensity = m_flare.Intensity;
			}
		}

		private void StartSecondaryEffect()
		{
			StopEffect();
			StopSecondaryEffect();
			MyParticlesManager.TryCreateParticleEffect(SecondaryEffectName, GetEffectMatrix(0.1f, EffectType.EffectSecondary), out m_toolSecondaryEffect);
			m_toolEffectLight = CreateSecondaryLight();
			UpdateEffect();
		}

		protected virtual MyLight CreateSecondaryLight()
		{
			MyLight myLight = MyLights.AddLight();
			if (myLight != null)
			{
				myLight.Start(Vector3.Zero, SecondaryLightColor, SecondaryLightRadius, DisplayNameText + " Tool Secondary");
				CreateGlare(myLight);
			}
			return myLight;
		}

		private void UpdateEffect()
		{
			if (EffectAction == MyShootActionEnum.PrimaryAction && m_raycastComponent.HitCubeGrid == null)
			{
				EffectAction = MyShootActionEnum.SecondaryAction;
			}
			if (EffectAction == MyShootActionEnum.SecondaryAction && (m_raycastComponent.HitCharacter != null || m_raycastComponent.HitEnvironmentSector != null))
			{
				EffectAction = MyShootActionEnum.PrimaryAction;
			}
			if (!EffectAction.HasValue)
			{
				if (m_soundEmitter.IsPlaying)
				{
					StopLoopSound();
				}
			}
			else
			{
				switch (EffectAction.Value)
				{
				case MyShootActionEnum.PrimaryAction:
					StartLoopSound(effect: true);
					break;
				case MyShootActionEnum.SecondaryAction:
					StartLoopSound(effect: false);
					break;
				}
			}
			if (m_toolEffect != null)
			{
				m_toolEffect.WorldMatrix = GetEffectMatrix(0.1f, EffectType.Effect);
			}
			if (m_toolSecondaryEffect != null)
			{
				m_toolSecondaryEffect.WorldMatrix = GetEffectMatrix(0.1f, EffectType.EffectSecondary);
			}
			if (m_toolEffectLight == null)
			{
				return;
			}
			m_toolEffectLight.Position = GetEffectMatrix(0f, EffectType.Light).Translation;
			if (EffectAction == MyShootActionEnum.PrimaryAction)
			{
				m_toolEffectLight.Intensity = MyUtils.GetRandomFloat(m_handItemDef.LightIntensityLower, m_handItemDef.LightIntensityUpper);
				if (m_flare != null)
				{
					m_toolEffectLight.GlareIntensity = m_toolEffectLight.Intensity * m_handItemDef.LightGlareIntensity * m_flare.Intensity;
					m_toolEffectLight.GlareSize = m_toolEffectLight.Intensity * m_handItemDef.LightGlareSize * m_flare.Size;
				}
			}
			else
			{
				m_toolEffectLight.Intensity = MyUtils.GetRandomFloat(SecondaryLightIntensityLower, SecondaryLightIntensityUpper);
				if (m_flare != null)
				{
					m_toolEffectLight.GlareIntensity = m_toolEffectLight.Intensity * m_handItemDef.LightGlareIntensity * m_flare.Intensity;
					m_toolEffectLight.GlareSize = m_toolEffectLight.Intensity * SecondaryLightGlareSize * m_flare.Size;
				}
			}
			if (m_flare != null)
			{
				m_toolEffectLight.SubGlares = m_flare.SubGlares;
			}
			m_toolEffectLight.UpdateLight();
		}

		protected void StopEffect()
		{
			if (m_toolEffect != null)
			{
				m_toolEffect.Stop();
				m_toolEffect = null;
			}
			if (m_toolEffectLight != null)
			{
				MyLights.RemoveLight(m_toolEffectLight);
				m_toolEffectLight = null;
			}
		}

		protected void StopSecondaryEffect()
		{
			if (m_toolSecondaryEffect != null)
			{
				m_toolSecondaryEffect.Stop();
				m_toolSecondaryEffect = null;
			}
		}

		protected override void Closing()
		{
			StopEffect();
			StopSecondaryEffect();
			StopLoopSound();
			base.Closing();
		}

		protected abstract void AddHudInfo();

		protected abstract void RemoveHudInfo();

		public virtual void OnControlAcquired(MyCharacter owner)
		{
			Owner = owner;
			CharacterInventory = Owner.GetInventory();
			if (owner.ControllerInfo.IsLocallyHumanControlled())
			{
				AddHudInfo();
			}
			m_lastTimeSelected = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			LastTargetObject = null;
			LastTargetStamp = 0;
		}

		public virtual void OnControlReleased()
		{
			RemoveHudInfo();
			Owner = null;
			CharacterInventory = null;
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			MyHud.Crosshair.Recenter();
			DrawHud();
			UpdateHudComponentMark();
		}

		protected virtual void DrawHud()
		{
			MyHud.BlockInfo.Visible = true;
			MySlimBlock mySlimBlock = m_raycastComponent.HitBlock;
			if (mySlimBlock == null)
			{
				if (LastTargetObject != this)
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
					LastTargetObject = this;
				}
				return;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && mySlimBlock.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock.GetBlocksCount() > 0)
				{
					mySlimBlock = myCompoundCubeBlock.GetBlocks().First();
				}
			}
			MyHud.BlockInfo.BlockIntegrity = mySlimBlock.Integrity / mySlimBlock.MaxIntegrity;
			int num = mySlimBlock.GetStockpileStamp() + mySlimBlock.ComponentStack.LastChangeStamp;
			if (LastTargetObject != mySlimBlock || num != LastTargetStamp)
			{
				LastTargetStamp = num;
				MyHud.BlockInfo.MissingComponentIndex = -1;
				MySlimBlock.SetBlockComponents(MyHud.BlockInfo, mySlimBlock);
				if (LastTargetObject != mySlimBlock)
				{
					MyHud.BlockInfo.BlockName = mySlimBlock.BlockDefinition.DisplayNameText;
					MyHud.BlockInfo.PCUCost = mySlimBlock.BlockDefinition.PCU;
					MyHud.BlockInfo.BlockIcons = mySlimBlock.BlockDefinition.Icons;
					MyHud.BlockInfo.CriticalIntegrity = mySlimBlock.BlockDefinition.CriticalIntegrityRatio;
					MyHud.BlockInfo.CriticalComponentIndex = mySlimBlock.BlockDefinition.CriticalGroup;
					MyHud.BlockInfo.OwnershipIntegrity = mySlimBlock.BlockDefinition.OwnershipIntegrityRatio;
					MyHud.BlockInfo.BlockBuiltBy = mySlimBlock.BuiltBy;
					MyHud.BlockInfo.GridSize = mySlimBlock.CubeGrid.GridSizeEnum;
					MyHud.BlockInfo.SetContextHelp(mySlimBlock.BlockDefinition);
					LastTargetObject = mySlimBlock;
				}
			}
		}

		protected void UnmarkMissingComponent()
		{
			m_lastMarkTime = -1;
			m_markedComponent = -1;
		}

		protected void MarkMissingComponent(int componentIdx)
		{
			m_lastMarkTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_markedComponent = componentIdx;
		}

		private void UpdateHudComponentMark()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastMarkTime > 2500)
			{
				UnmarkMissingComponent();
			}
			else
			{
				MyHud.BlockInfo.MissingComponentIndex = m_markedComponent;
			}
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			MyCharacterWeaponPositionComponent myCharacterWeaponPositionComponent = Owner.Components.Get<MyCharacterWeaponPositionComponent>();
			Vector3D v = (myCharacterWeaponPositionComponent == null) ? Vector3D.Normalize(target - base.PositionComp.WorldMatrix.Translation) : Vector3D.Normalize(target - myCharacterWeaponPositionComponent.LogicalPositionWorld);
			return v;
		}

		public virtual void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (Owner == MySession.Static.LocalCharacter)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		public virtual void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public virtual void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (action == MyShootActionEnum.PrimaryAction && status == MyGunStatusEnum.Failed)
			{
				EffectAction = MyShootActionEnum.SecondaryAction;
			}
		}

		public int GetAmmunitionAmount()
		{
			return 0;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
			objectBuilder.SubtypeName = m_handItemDef.Id.SubtypeName;
			return objectBuilder;
		}

		public virtual bool SupressShootAnimation()
		{
			return false;
		}

		public void DoubleClicked(MyShootActionEnum action)
		{
			m_isActionDoubleClicked[action] = true;
		}
	}
}
