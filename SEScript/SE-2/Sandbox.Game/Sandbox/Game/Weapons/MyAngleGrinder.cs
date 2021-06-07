#define VRAGE
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.ModAPI.Weapons;
using System;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_AngleGrinder), true)]
	[StaticEventOwner]
	public class MyAngleGrinder : MyEngineerToolBase, IMyAngleGrinder, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
	{
		protected sealed class OnInventoryFulfilled_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnInventoryFulfilled();
			}
		}

		private class Sandbox_Game_Weapons_MyAngleGrinder_003C_003EActor : IActivator, IActivator<MyAngleGrinder>
		{
			private sealed override object CreateInstance()
			{
				return new MyAngleGrinder();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAngleGrinder CreateInstance()
			{
				return new MyAngleGrinder();
			}

			MyAngleGrinder IActivator<MyAngleGrinder>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MySoundPair m_idleSound = new MySoundPair("ToolPlayGrindIdle");

		private MySoundPair m_actualSound = new MySoundPair("ToolPlayGrindMetal");

		private MyStringHash m_source = MyStringHash.GetOrCompute("Grinder");

		private MyStringHash m_metal = MyStringHash.GetOrCompute("Metal");

		private static readonly float GRINDER_AMOUNT_PER_SECOND = 2f;

		private static readonly float GRINDER_MAX_SPEED_RPM = 500f;

		private static readonly float GRINDER_ACCELERATION_RPMPS = 700f;

		private static readonly float GRINDER_DECELERATION_RPMPS = 500f;

		public static float GRINDER_MAX_SHAKE = 1.5f;

		private MyHudNotification m_safezoneNotification;

		private static int m_lastTimePlayedSound;

		private int m_lastUpdateTime;

		private float m_rotationSpeed;

		private int m_lastContactTime;

		private int m_lastItemId;

		private static MyDefinitionId m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "AngleGrinderItem");

		private double m_grinderCameraMeanShakeIntensity = 1.0;

		public override bool IsSkinnable => true;

		private float GrinderAmount => MySession.Static.GrinderSpeedMultiplier * m_speedMultiplier * GRINDER_AMOUNT_PER_SECOND * (float)base.ToolCooldownMs / 1000f;

		public MyAngleGrinder()
			: base(250)
		{
			SecondaryLightIntensityLower = 0.4f;
			SecondaryLightIntensityUpper = 0.4f;
			EffectScale = 0.6f;
			base.HasCubeHighlight = true;
			base.HighlightColor = Color.Red * 0.3f;
			base.HighlightMaterial = MyStringId.GetOrCompute("GizmoDrawLineRed");
			m_rotationSpeed = 0f;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "AngleGrinderItem");
			if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
			{
				m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
			}
			base.PhysicalObject = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(m_physicalItemId);
			Init(objectBuilder, m_physicalItemId);
			m_effectId = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_handItemDef.ToolMaterial, m_metal);
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(m_physicalItemId);
			Init(null, physicalItemDefinition.Model, null, null);
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			base.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)objectBuilder.Clone();
			base.PhysicalObject.GunEntity.EntityId = base.EntityId;
			foreach (ToolSound toolSound in m_handItemDef.ToolSounds)
			{
				if (toolSound.type != null && toolSound.subtype != null && toolSound.sound != null && toolSound.type.Equals("Main") && toolSound.subtype.Equals("Idle"))
				{
					m_idleSound = new MySoundPair(toolSound.sound);
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime;
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (!m_activated)
			{
				m_effectId = null;
			}
			if (m_activated && m_rotationSpeed < GRINDER_MAX_SPEED_RPM)
			{
				m_rotationSpeed += (float)num * 0.001f * GRINDER_ACCELERATION_RPMPS;
				if (m_rotationSpeed > GRINDER_MAX_SPEED_RPM)
				{
					m_rotationSpeed = GRINDER_MAX_SPEED_RPM;
				}
			}
			else if (!m_activated && m_rotationSpeed > 0f)
			{
				m_rotationSpeed -= (float)num * 0.001f * GRINDER_DECELERATION_RPMPS;
				if (m_rotationSpeed < 0f)
				{
					m_rotationSpeed = 0f;
				}
			}
			if (m_effectId != null && Owner != null && Owner.ControllerInfo.IsLocallyControlled())
			{
				PerformCameraShake();
			}
			MyEntitySubpart myEntitySubpart = base.Subparts["grinder"];
			myEntitySubpart.PositionComp.LocalMatrix = Matrix.CreateRotationY((float)(-num) * m_rotationSpeed * 0.000104719758f) * myEntitySubpart.PositionComp.LocalMatrix;
		}

		public override void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				if (m_safezoneNotification == null)
				{
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_GrindingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			MyAnalyticsHelper.ReportActivityStartIf(!m_activated, Owner, "Grinding", "Character", "HandTools", "AngleGrinder");
			base.Shoot(action, direction, overrideWeaponPos, gunAction);
			if (action == MyShootActionEnum.PrimaryAction && base.IsPreheated && m_activated)
			{
				Grind();
			}
		}

		protected override void AddHudInfo()
		{
		}

		protected override void RemoveHudInfo()
		{
		}

		public override void BeginShoot(MyShootActionEnum action)
		{
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			MyAnalyticsHelper.ReportActivityEnd(Owner, "Grinding");
			base.EndShoot(action);
		}

		protected override MatrixD GetEffectMatrix(float muzzleOffset, EffectType effectType)
		{
			if (effectType == EffectType.Light)
			{
				return MatrixD.CreateWorld(m_gunBase.GetMuzzleWorldPosition(), base.WorldMatrix.Forward, base.WorldMatrix.Up);
			}
			if (m_raycastComponent.HitCubeGrid != null && m_raycastComponent.HitBlock != null)
			{
				_ = Owner;
			}
			return MatrixD.CreateWorld(m_gunBase.GetMuzzleWorldPosition(), base.WorldMatrix.Forward, base.WorldMatrix.Up);
		}

		private void Grind()
		{
			MySlimBlock targetBlock = GetTargetBlock();
			MyStringHash materialType = m_metal;
			m_effectId = null;
			ulong user = 0uL;
			if (Owner != null && Owner.ControllerInfo != null && Owner.ControllerInfo.Controller != null && Owner.ControllerInfo.Controller.Player != null)
			{
				user = Owner.ControllerInfo.Controller.Player.Id.SteamId;
			}
			if (targetBlock != null && !MySessionComponentSafeZones.IsActionAllowed(targetBlock.WorldAABB, MySafeZoneAction.Grinding, 0L, user))
			{
				return;
			}
			if (targetBlock != null && ((!MySession.Static.IsScenario && !MySession.Static.Settings.ScenarioEditMode) || targetBlock.CubeGrid.BlocksDestructionEnabled))
			{
				MyCubeBlockDefinition.PreloadConstructionModels(targetBlock.BlockDefinition);
				if (Sync.IsServer)
				{
					float num = 1f;
					if (targetBlock.FatBlock != null && Owner != null && Owner.ControllerInfo.Controller != null && Owner.ControllerInfo.Controller.Player != null)
					{
						MyRelationsBetweenPlayerAndBlock userRelationToOwner = targetBlock.FatBlock.GetUserRelationToOwner(Owner.ControllerInfo.Controller.Player.Identity.IdentityId);
						if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies || userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Neutral)
						{
							num = MySession.Static.HackSpeedMultiplier;
						}
					}
					float grinderAmount = GrinderAmount;
					MyDamageInformation info = new MyDamageInformation(isDeformation: false, grinderAmount * num, MyDamageType.Grind, base.EntityId);
					if (targetBlock.UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseBeforeDamageApplied(targetBlock, ref info);
					}
					if (targetBlock.CubeGrid.Editable)
					{
						targetBlock.DecreaseMountLevel(info.Amount, base.CharacterInventory, useDefaultDeconstructEfficiency: false, Owner.ControllerInfo.ControllingIdentityId);
						if (targetBlock.MoveItemsFromConstructionStockpile(base.CharacterInventory) && Owner.ControllerInfo != null && Owner.ControllerInfo.Controller != null && Owner.ControllerInfo.Controller.Player != null)
						{
							ulong steamId = Owner.ControllerInfo.Controller.Player.Id.SteamId;
							SendInventoryFullNotification(steamId);
						}
						long num2 = (targetBlock.CubeGrid.BigOwners.Count > 0) ? targetBlock.CubeGrid.BigOwners[0] : 0;
						if (Owner.ControllerInfo.ControllingIdentityId != num2)
						{
							MySession.Static.Factions.DamageFactionPlayerReputation(Owner.ControllerInfo.ControllingIdentityId, num2, MyReputationDamageType.GrindingWelding);
						}
					}
					if (MySession.Static != null && Owner == MySession.Static.LocalCharacter && MyMusicController.Static != null)
					{
						MyMusicController.Static.Building(250);
					}
					if (targetBlock.UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseAfterDamageApplied(targetBlock, info);
					}
					if (targetBlock.IsFullyDismounted)
					{
						if (targetBlock.UseDamageSystem)
						{
							MyDamageSystem.Static.RaiseDestroyed(targetBlock, info);
						}
						targetBlock.SpawnConstructionStockpile();
						ulong user2 = 0uL;
						MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(Owner);
						if (controllingPlayer != null)
						{
							user2 = controllingPlayer.Id.SteamId;
						}
						targetBlock.CubeGrid.RazeBlock(targetBlock.Min, user2);
					}
				}
				if (targetBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeName.Length > 0)
				{
					materialType = targetBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
				}
			}
			IMyDestroyableObject targetDestroyable = GetTargetDestroyable();
			if (targetDestroyable != null)
			{
				if ((targetDestroyable is MyEntity && !MySessionComponentSafeZones.IsActionAllowed((MyEntity)targetDestroyable, MySafeZoneAction.Grinding, 0L, 0uL)) || (targetDestroyable is MyCharacter && targetDestroyable as MyCharacter == Owner))
				{
					return;
				}
				if (Sync.IsServer)
				{
					if (targetDestroyable is MyCharacter && MySession.Static.ControlledEntity == Owner && !(targetDestroyable as MyCharacter).IsDead)
					{
						MySession.Static.TotalDamageDealt += 20u;
					}
					targetDestroyable.DoDamage(20f, MyDamageType.Grind, sync: true, null, (Owner != null) ? Owner.EntityId : 0);
				}
				if (targetDestroyable is MyCharacter)
				{
					materialType = MyStringHash.GetOrCompute((targetDestroyable as MyCharacter).Definition.PhysicalMaterial);
				}
			}
			MyEnvironmentSector hitEnvironmentSector = m_raycastComponent.HitEnvironmentSector;
			if (hitEnvironmentSector != null)
			{
				if (Sync.IsServer)
				{
					int environmentItem = m_raycastComponent.EnvironmentItem;
					if (environmentItem != m_lastItemId)
					{
						m_lastItemId = environmentItem;
						m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					}
					if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastContactTime) > 1500f / m_speedMultiplier)
					{
						MyBreakableEnvironmentProxy module = hitEnvironmentSector.GetModule<MyBreakableEnvironmentProxy>();
						Vector3D hitnormal = Owner.WorldMatrix.Right + Owner.WorldMatrix.Forward;
						hitnormal.Normalize();
						float mass = Owner.Physics.Mass;
						float num3 = 10f * 10f * mass;
						module.BreakAt(environmentItem, m_raycastComponent.HitPosition, hitnormal, num3);
						m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
						m_lastItemId = 0;
					}
				}
				materialType = MyStringHash.GetOrCompute("Wood");
			}
			if (targetBlock != null || targetDestroyable != null || hitEnvironmentSector != null)
			{
				m_actualSound = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, m_handItemDef.ToolMaterial, materialType);
				m_effectId = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_handItemDef.ToolMaterial, materialType);
			}
		}

		private void SendInventoryFullNotification(ulong clientId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnInventoryFulfilled, new EndpointId(clientId));
		}

		[Event(null, 407)]
		[Reliable]
		[Client]
		private static void OnInventoryFulfilled()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimePlayedSound > 2500)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudVocInventoryFull);
				m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
		}

		protected override void StartLoopSound(bool effect)
		{
			bool flag = Owner != null && Owner.IsInFirstPersonView && Owner == MySession.Static.LocalCharacter;
			MySoundPair soundId = effect ? m_actualSound : m_idleSound;
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				if (flag != m_soundEmitter.Force2D)
				{
					m_soundEmitter.PlaySound(soundId, stopPrevious: true, skipIntro: true, flag);
				}
				else
				{
					m_soundEmitter.PlaySingleSound(soundId, stopPrevious: true);
				}
			}
			else
			{
				m_soundEmitter.PlaySound(soundId, stopPrevious: true, skipIntro: false, flag);
			}
		}

		protected override void StopLoopSound()
		{
			m_soundEmitter.StopSound(forced: false);
		}

		protected override void StopSound()
		{
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public void PerformCameraShake()
		{
			if (MySector.MainCamera != null)
			{
				float num = (float)((0.0 - Math.Log(MyRandom.Instance.NextDouble())) * m_grinderCameraMeanShakeIntensity);
				num = MathHelper.Clamp(num * GRINDER_MAX_SHAKE, 0f, GRINDER_MAX_SHAKE);
				MySector.MainCamera.CameraShake.AddShake(num);
			}
		}

		public override bool SupressShootAnimation()
		{
			return false;
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (!MySessionComponentSafeZones.IsActionAllowed(Owner, MySafeZoneAction.Grinding, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			base.SinkComp.Update();
			if (!base.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				status = MyGunStatusEnum.OutOfPower;
				return false;
			}
			return base.CanShoot(action, shooter, out status);
		}

		public new bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			if (m_isActionDoubleClicked.ContainsKey(action))
			{
				return !m_isActionDoubleClicked[action];
			}
			return true;
		}

		public new bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return true;
		}
	}
}
