using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[StaticEventOwner]
	[MyEntityType(typeof(MyObjectBuilder_HandToolBase), true)]
	public class MyHandToolBase : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IStoppableAttackingTool
	{
		public class MyBlockingBody : MyPhysicsBody
		{
			private class Sandbox_Game_Entities_MyHandToolBase_003C_003EMyBlockingBody_003C_003EActor
			{
			}

			public MyHandToolBase HandTool
			{
				get;
				private set;
			}

			public MyBlockingBody(MyHandToolBase tool, MyEntity owner)
				: base(owner, RigidBodyFlag.RBF_KINEMATIC)
			{
				HandTool = tool;
			}

			public override void OnMotion(HkRigidBody rbo, float step, bool fromParent)
			{
			}

			public override void OnWorldPositionChanged(object source)
			{
			}

			public void SetWorldMatrix(MatrixD worldMatrix)
			{
				Vector3D objectOffset = MyPhysics.GetObjectOffset(base.ClusterObjectID);
				Matrix worldMatrix2 = Matrix.CreateWorld(worldMatrix.Translation - objectOffset, worldMatrix.Forward, worldMatrix.Up);
				if (RigidBody != null)
				{
					RigidBody.SetWorldMatrix(worldMatrix2);
				}
			}
		}

		protected sealed class StopShootingRequest_003C_003ESystem_Int64_0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in float attackDelay, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StopShootingRequest(entityId, attackDelay);
			}
		}

		private class Sandbox_Game_Entities_MyHandToolBase_003C_003EActor : IActivator, IActivator<MyHandToolBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyHandToolBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHandToolBase CreateInstance()
			{
				return new MyHandToolBase();
			}

			MyHandToolBase IActivator<MyHandToolBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyStringId m_startCue = MyStringId.GetOrCompute("Start");

		private static MyStringId m_hitCue = MyStringId.GetOrCompute("Hit");

		private const float AFTER_SHOOT_HIT_DELAY = 0.4f;

		private MyDefinitionId m_handItemDefinitionId;

		private MyToolActionDefinition? m_primaryToolAction;

		private MyToolHitCondition m_primaryHitCondition;

		private MyToolActionDefinition? m_secondaryToolAction;

		private MyToolHitCondition m_secondaryHitCondition;

		private MyToolActionDefinition? m_shotToolAction;

		private MyToolHitCondition m_shotHitCondition;

		protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();

		private bool m_wasShooting;

		private bool m_swingSoundPlayed;

		private bool m_isHit;

		protected Dictionary<string, IMyHandToolComponent> m_toolComponents = new Dictionary<string, IMyHandToolComponent>();

		private MyCharacter m_owner;

		protected int m_lastShot;

		private int m_lastHit;

		private int m_hitDelay;

		private MyPhysicalItemDefinition m_physItemDef;

		protected MyToolItemDefinition m_toolItemDef;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private Dictionary<string, MySoundPair> m_toolSounds = new Dictionary<string, MySoundPair>();

		private static MyStringId BlockId = MyStringId.Get("Block");

		private MyHudNotification m_notEnoughStatNotification;

		public MyObjectBuilder_PhysicalGunObject PhysicalObject
		{
			get;
			private set;
		}

		public new MyPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public bool IsShooting
		{
			get
			{
				if (!m_shotToolAction.HasValue)
				{
					return false;
				}
				if (m_lastShot <= MySandboxGame.TotalGamePlayTimeInMilliseconds)
				{
					if (!((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastShot) < m_shotToolAction.Value.HitDuration * 1000f))
					{
						return m_shotToolAction.Value.HitDuration == 0f;
					}
					return true;
				}
				return false;
			}
		}

		public int ShootDirectionUpdateTime => 0;

		public bool EnabledInWorldRules => true;

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

		public new MyDefinitionId DefinitionId => m_handItemDefinitionId;

		public MyToolBase GunBase
		{
			get;
			private set;
		}

		public virtual bool ForceAnimationInsteadOfIK => true;

		public bool IsBlocking
		{
			get
			{
				if (m_shotToolAction.HasValue)
				{
					return m_shotToolAction.Value.Name == MyStringId.GetOrCompute("Block");
				}
				return false;
			}
		}

		public MyPhysicalItemDefinition PhysicalItemDefinition => m_physItemDef;

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

		public bool IsSkinnable => false;

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

		public MyHandToolBase()
		{
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
			GunBase = new MyToolBase();
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			m_handItemDefinitionId = objectBuilder.GetId();
			m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(m_handItemDefinitionId);
			base.Init(objectBuilder);
			Init(null, PhysicalItemDefinition.Model, null, null);
			base.Save = false;
			PhysicalObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(m_handItemDefinitionId.SubtypeName);
			PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)objectBuilder.Clone();
			PhysicalObject.GunEntity.EntityId = base.EntityId;
			m_toolItemDef = (PhysicalItemDefinition as MyToolItemDefinition);
			m_notEnoughStatNotification = new MyHudNotification(MyCommonTexts.NotificationStatNotEnough, 1000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			InitToolComponents();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME);
			MyObjectBuilder_HandToolBase myObjectBuilder_HandToolBase = objectBuilder as MyObjectBuilder_HandToolBase;
			if (myObjectBuilder_HandToolBase.DeviceBase != null)
			{
				GunBase.Init(myObjectBuilder_HandToolBase.DeviceBase);
			}
		}

		protected virtual void InitToolComponents()
		{
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_HandToolBase obj = base.GetObjectBuilder(copy) as MyObjectBuilder_HandToolBase;
			obj.SubtypeName = m_handItemDefinitionId.SubtypeName;
			obj.DeviceBase = GunBase.GetObjectBuilder();
			return obj;
		}

		private void InitBlockingPhysics(MyEntity owner)
		{
			CloseBlockingPhysics();
			Physics = new MyBlockingBody(this, owner);
			HkShape shape = new HkBoxShape(0.5f * new Vector3(0.5f, 0.7f, 0.25f));
			Physics.CreateFromCollisionObject(shape, new Vector3(0f, 0.9f, -0.5f), base.WorldMatrix, null, 19);
			Physics.MaterialType = m_physItemDef.PhysicalMaterial;
			shape.RemoveReference();
			Physics.Enabled = false;
			m_owner.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
		}

		private void CloseBlockingPhysics()
		{
			if (Physics != null)
			{
				Physics.Close();
				Physics = null;
			}
		}

		public virtual bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastHit < m_hitDelay)
			{
				status = MyGunStatusEnum.Failed;
				return false;
			}
			status = MyGunStatusEnum.OK;
			if (IsShooting)
			{
				status = MyGunStatusEnum.Cooldown;
			}
			if (m_owner == null)
			{
				status = MyGunStatusEnum.Failed;
			}
			return status == MyGunStatusEnum.OK;
		}

		public bool ShouldEndShootOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public bool CanDoubleClickToStick(MyShootActionEnum action)
		{
			return false;
		}

		public virtual void Shoot(MyShootActionEnum shootAction, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			m_shotToolAction = null;
			m_wasShooting = false;
			m_swingSoundPlayed = false;
			m_isHit = false;
			if (!string.IsNullOrEmpty(gunAction))
			{
				switch (shootAction)
				{
				case MyShootActionEnum.PrimaryAction:
					GetPreferredToolAction(m_toolItemDef.PrimaryActions, gunAction, out m_primaryToolAction, out m_primaryHitCondition);
					break;
				case MyShootActionEnum.SecondaryAction:
					GetPreferredToolAction(m_toolItemDef.SecondaryActions, gunAction, out m_secondaryToolAction, out m_secondaryHitCondition);
					break;
				}
			}
			switch (shootAction)
			{
			case MyShootActionEnum.PrimaryAction:
				m_shotToolAction = m_primaryToolAction;
				m_shotHitCondition = m_primaryHitCondition;
				break;
			case MyShootActionEnum.SecondaryAction:
				m_shotToolAction = m_secondaryToolAction;
				m_shotHitCondition = m_secondaryHitCondition;
				break;
			}
			if (!string.IsNullOrEmpty(m_shotHitCondition.StatsAction) && m_owner.StatComp != null && !m_owner.StatComp.CanDoAction(m_shotHitCondition.StatsAction, out MyTuple<ushort, MyStringHash> message))
			{
				if (MySession.Static != null && MySession.Static.LocalCharacter == m_owner && message.Item1 == 4 && message.Item2.String.CompareTo("Stamina") == 0)
				{
					m_notEnoughStatNotification.SetTextFormatArguments(message.Item2);
					MyHud.Notifications.Add(m_notEnoughStatNotification);
				}
			}
			else
			{
				if (!m_shotToolAction.HasValue)
				{
					return;
				}
				if (m_toolComponents.TryGetValue(m_shotHitCondition.Component, out IMyHandToolComponent value))
				{
					value.Shoot();
				}
				MyFrameOption frameOption = MyFrameOption.StayOnLastFrame;
				if (m_shotToolAction.Value.HitDuration == 0f)
				{
					frameOption = MyFrameOption.JustFirstFrame;
				}
				m_owner.StopUpperCharacterAnimation(0.1f);
				m_owner.PlayCharacterAnimation(m_shotHitCondition.Animation, MyBlendOption.Immediate, frameOption, 0.2f, m_shotHitCondition.AnimationTimeScale, sync: false, null, excludeLegsWhenMoving: true);
				m_owner.TriggerCharacterAnimationEvent(m_shotHitCondition.Animation.ToLower(), sync: false);
				if (m_owner.StatComp != null)
				{
					if (!string.IsNullOrEmpty(m_shotHitCondition.StatsAction))
					{
						m_owner.StatComp.DoAction(m_shotHitCondition.StatsAction);
					}
					if (!string.IsNullOrEmpty(m_shotHitCondition.StatsModifier))
					{
						m_owner.StatComp.ApplyModifier(m_shotHitCondition.StatsModifier);
					}
				}
				Physics.Enabled = (m_shotToolAction.Value.Name == BlockId);
				m_lastShot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
		}

		private void PlaySound(string soundName)
		{
			if (!MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalMaterialDefinition), m_physItemDef.PhysicalMaterial), out MyPhysicalMaterialDefinition definition))
			{
				return;
			}
			if (definition.GeneralSounds.TryGetValue(MyStringId.GetOrCompute(soundName), out MySoundPair value) && !value.SoundId.IsNull)
			{
				m_soundEmitter.PlaySound(value);
				return;
			}
			if (!m_toolSounds.TryGetValue(soundName, out MySoundPair value2))
			{
				value2 = new MySoundPair(soundName);
				m_toolSounds.Add(soundName, value2);
			}
			m_soundEmitter.PlaySound(value2);
		}

		public virtual void OnControlAcquired(MyCharacter owner)
		{
			m_owner = owner;
			InitBlockingPhysics(m_owner);
			foreach (IMyHandToolComponent value in m_toolComponents.Values)
			{
				value.OnControlAcquired(owner);
			}
			this.RaiseEntityEvent(MyStringHash.GetOrCompute("ControlAcquired"), new MyEntityContainerEventExtensions.ControlAcquiredParams(owner));
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
		}

		public virtual void OnControlReleased()
		{
			this.RaiseEntityEvent(MyStringHash.GetOrCompute("ControlReleased"), new MyEntityContainerEventExtensions.ControlReleasedParams(m_owner));
			if (m_owner != null)
			{
				m_owner.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
			}
			m_owner = null;
			CloseBlockingPhysics();
			foreach (IMyHandToolComponent value in m_toolComponents.Values)
			{
				value.OnControlReleased();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			bool isShooting = IsShooting;
			if (!m_isHit && IsShooting && (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastShot) > m_shotToolAction.Value.HitStart * 1000f)
			{
				if (m_toolComponents.TryGetValue(m_shotHitCondition.Component, out IMyHandToolComponent value))
				{
					MyCharacterDetectorComponent myCharacterDetectorComponent = m_owner.Components.Get<MyCharacterDetectorComponent>();
					if (myCharacterDetectorComponent != null)
					{
						if (m_shotToolAction.Value.CustomShapeRadius > 0f && myCharacterDetectorComponent is MyCharacterShapecastDetectorComponent)
						{
							MyCharacterShapecastDetectorComponent obj = myCharacterDetectorComponent as MyCharacterShapecastDetectorComponent;
							obj.ShapeRadius = m_shotToolAction.Value.CustomShapeRadius;
							obj.DoDetectionModel();
							obj.ShapeRadius = 0.1f;
						}
						if (myCharacterDetectorComponent.DetectedEntity != null)
						{
							MyHitInfo hitInfo = default(MyHitInfo);
							hitInfo.Position = myCharacterDetectorComponent.HitPosition;
							hitInfo.Normal = myCharacterDetectorComponent.HitNormal;
							hitInfo.ShapeKey = myCharacterDetectorComponent.ShapeKey;
							bool isBlock = false;
							float hitEfficiency = 1f;
							bool num = CanHit(value, myCharacterDetectorComponent, ref isBlock, out hitEfficiency);
							bool flag = false;
							if (num)
							{
								if (!string.IsNullOrEmpty(m_shotToolAction.Value.StatsEfficiency) && Owner.StatComp != null)
								{
									hitEfficiency *= Owner.StatComp.GetEfficiencyModifier(m_shotToolAction.Value.StatsEfficiency);
								}
								float efficiency = m_shotToolAction.Value.Efficiency * hitEfficiency;
								MyHandToolBase myHandToolBase = myCharacterDetectorComponent.DetectedEntity as MyHandToolBase;
								flag = ((!isBlock || myHandToolBase == null) ? value.Hit((MyEntity)myCharacterDetectorComponent.DetectedEntity, hitInfo, myCharacterDetectorComponent.ShapeKey, efficiency) : value.Hit(myHandToolBase.Owner, hitInfo, myCharacterDetectorComponent.ShapeKey, efficiency));
								if (flag && Sync.IsServer && Owner.StatComp != null)
								{
									if (!string.IsNullOrEmpty(m_shotHitCondition.StatsActionIfHit))
									{
										Owner.StatComp.DoAction(m_shotHitCondition.StatsActionIfHit);
									}
									if (!string.IsNullOrEmpty(m_shotHitCondition.StatsModifierIfHit))
									{
										Owner.StatComp.ApplyModifier(m_shotHitCondition.StatsModifierIfHit);
									}
								}
							}
							if (num || isBlock)
							{
								if (!string.IsNullOrEmpty(m_shotToolAction.Value.HitSound))
								{
									PlaySound(m_shotToolAction.Value.HitSound);
								}
								else
								{
									MyStringId type = MyMaterialPropertiesHelper.CollisionType.Hit;
									bool flag2 = false;
									if (MyAudioComponent.PlayContactSound(base.EntityId, m_hitCue, myCharacterDetectorComponent.HitPosition, m_toolItemDef.PhysicalMaterial, myCharacterDetectorComponent.HitMaterial))
									{
										flag2 = true;
									}
									else if (MyAudioComponent.PlayContactSound(base.EntityId, m_startCue, myCharacterDetectorComponent.HitPosition, m_toolItemDef.PhysicalMaterial, myCharacterDetectorComponent.HitMaterial))
									{
										flag2 = true;
										type = MyMaterialPropertiesHelper.CollisionType.Start;
									}
									if (flag2)
									{
										MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, myCharacterDetectorComponent.HitPosition, myCharacterDetectorComponent.HitNormal, m_toolItemDef.PhysicalMaterial, myCharacterDetectorComponent.HitMaterial, null);
									}
								}
								this.RaiseEntityEvent(MyStringHash.GetOrCompute("Hit"), new MyEntityContainerEventExtensions.HitParams(MyStringHash.GetOrCompute(m_shotHitCondition.Component), myCharacterDetectorComponent.HitMaterial));
								m_soundEmitter.StopSound(forced: true);
							}
						}
					}
				}
				m_isHit = true;
			}
			if (!m_swingSoundPlayed && IsShooting && !m_isHit && (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastShot) > m_shotToolAction.Value.SwingSoundStart * 1000f)
			{
				if (!string.IsNullOrEmpty(m_shotToolAction.Value.SwingSound))
				{
					PlaySound(m_shotToolAction.Value.SwingSound);
				}
				m_swingSoundPlayed = true;
			}
			if (!isShooting && m_wasShooting)
			{
				m_owner.TriggerCharacterAnimationEvent("stop_tool_action", sync: false);
				m_owner.StopUpperCharacterAnimation(0.4f);
				m_shotToolAction = null;
			}
			m_wasShooting = isShooting;
			if (m_owner != null)
			{
				MatrixD worldMatrix = MatrixD.CreateWorld(((MyEntity)m_owner.CurrentWeapon).PositionComp.GetPosition(), m_owner.WorldMatrix.Forward, m_owner.WorldMatrix.Up);
				((MyBlockingBody)Physics).SetWorldMatrix(worldMatrix);
			}
			foreach (IMyHandToolComponent value2 in m_toolComponents.Values)
			{
				value2.Update();
			}
		}

		protected bool CanHit(IMyHandToolComponent toolComponent, MyCharacterDetectorComponent detectorComponent, ref bool isBlock, out float hitEfficiency)
		{
			bool flag = true;
			hitEfficiency = 1f;
			MyTuple<ushort, MyStringHash> message;
			if (detectorComponent.HitBody != null && detectorComponent.HitBody.UserObject is MyBlockingBody)
			{
				MyBlockingBody myBlockingBody = detectorComponent.HitBody.UserObject as MyBlockingBody;
				if (myBlockingBody.HandTool.IsBlocking && myBlockingBody.HandTool.m_owner.StatComp != null && myBlockingBody.HandTool.m_owner.StatComp.CanDoAction(myBlockingBody.HandTool.m_shotHitCondition.StatsActionIfHit, out message))
				{
					myBlockingBody.HandTool.m_owner.StatComp.DoAction(myBlockingBody.HandTool.m_shotHitCondition.StatsActionIfHit);
					if (!string.IsNullOrEmpty(myBlockingBody.HandTool.m_shotHitCondition.StatsModifierIfHit))
					{
						myBlockingBody.HandTool.m_owner.StatComp.ApplyModifier(myBlockingBody.HandTool.m_shotHitCondition.StatsModifierIfHit);
					}
					isBlock = true;
					if (!string.IsNullOrEmpty(myBlockingBody.HandTool.m_shotToolAction.Value.StatsEfficiency))
					{
						hitEfficiency = 1f - myBlockingBody.HandTool.m_owner.StatComp.GetEfficiencyModifier(myBlockingBody.HandTool.m_shotToolAction.Value.StatsEfficiency);
					}
					flag = (hitEfficiency > 0f);
					MyEntityContainerEventExtensions.RaiseEntityEventOn(myBlockingBody.HandTool, MyStringHash.GetOrCompute("Hit"), new MyEntityContainerEventExtensions.HitParams(MyStringHash.GetOrCompute("Block"), PhysicalItemDefinition.Id.SubtypeId));
				}
			}
			if (!flag)
			{
				hitEfficiency = 0f;
				return flag;
			}
			if (!string.IsNullOrEmpty(m_shotHitCondition.StatsActionIfHit))
			{
				flag = (m_owner.StatComp != null && m_owner.StatComp.CanDoAction(m_shotHitCondition.StatsActionIfHit, out message));
				if (!flag)
				{
					hitEfficiency = 0f;
					return flag;
				}
			}
			flag = (Vector3.Distance(detectorComponent.HitPosition, detectorComponent.StartPosition) <= m_toolItemDef.HitDistance);
			if (!flag)
			{
				hitEfficiency = 0f;
				return flag;
			}
			_ = m_owner.Entity;
			long playerIdentityId = m_owner.GetPlayerIdentityId();
			MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerIdentityId) as MyFaction;
			if (myFaction != null && !myFaction.EnableFriendlyFire)
			{
				MyCharacter myCharacter = detectorComponent.DetectedEntity as MyCharacter;
				if (myCharacter != null)
				{
					flag = !myFaction.IsMember(myCharacter.GetPlayerIdentityId());
					hitEfficiency = (flag ? hitEfficiency : 0f);
				}
			}
			return flag;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			GetMostEffectiveToolAction(m_toolItemDef.PrimaryActions, out m_primaryToolAction, out m_primaryHitCondition);
			GetMostEffectiveToolAction(m_toolItemDef.SecondaryActions, out m_secondaryToolAction, out m_secondaryHitCondition);
			if (MySession.Static.ControlledEntity == m_owner)
			{
				MyCharacterDetectorComponent myCharacterDetectorComponent = m_owner.Components.Get<MyCharacterDetectorComponent>();
				bool flag = false;
				float num = float.MaxValue;
				if (myCharacterDetectorComponent != null)
				{
					flag = (myCharacterDetectorComponent.DetectedEntity != null);
					num = Vector3.Distance(myCharacterDetectorComponent.HitPosition, base.PositionComp.GetPosition());
				}
				if (num > m_toolItemDef.HitDistance)
				{
					flag = false;
				}
				if (m_primaryToolAction.HasValue && (m_primaryHitCondition.EntityType != null || flag))
				{
					MyHud.Crosshair.ChangeDefaultSprite(m_primaryToolAction.Value.Crosshair);
				}
				else if (m_secondaryToolAction.HasValue && (m_secondaryHitCondition.EntityType != null || flag))
				{
					MyHud.Crosshair.ChangeDefaultSprite(m_secondaryToolAction.Value.Crosshair);
				}
				else
				{
					MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.crosshair);
				}
			}
		}

		private void GetMostEffectiveToolAction(List<MyToolActionDefinition> toolActions, out MyToolActionDefinition? bestAction, out MyToolHitCondition bestCondition)
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = m_owner.Components.Get<MyCharacterDetectorComponent>();
			IMyEntity myEntity = null;
			uint shapeKey = 0u;
			if (myCharacterDetectorComponent != null)
			{
				myEntity = myCharacterDetectorComponent.DetectedEntity;
				shapeKey = myCharacterDetectorComponent.ShapeKey;
				if (Vector3.Distance(myCharacterDetectorComponent.HitPosition, myCharacterDetectorComponent.StartPosition) > m_toolItemDef.HitDistance)
				{
					myEntity = null;
				}
			}
			bestAction = null;
			bestCondition = default(MyToolHitCondition);
			foreach (MyToolActionDefinition toolAction in toolActions)
			{
				if (toolAction.HitConditions != null)
				{
					MyToolHitCondition[] hitConditions = toolAction.HitConditions;
					for (int i = 0; i < hitConditions.Length; i++)
					{
						MyToolHitCondition myToolHitCondition = hitConditions[i];
						if (myToolHitCondition.EntityType == null)
						{
							bestAction = toolAction;
							bestCondition = myToolHitCondition;
							return;
						}
						if (myEntity != null)
						{
							string stateForTarget = GetStateForTarget((MyEntity)myEntity, shapeKey, myToolHitCondition.Component);
							if (myToolHitCondition.EntityType.Contains(stateForTarget))
							{
								bestAction = toolAction;
								bestCondition = myToolHitCondition;
								return;
							}
						}
					}
				}
			}
		}

		private void GetPreferredToolAction(List<MyToolActionDefinition> toolActions, string name, out MyToolActionDefinition? bestAction, out MyToolHitCondition bestCondition)
		{
			bestAction = null;
			bestCondition = default(MyToolHitCondition);
			MyStringId orCompute = MyStringId.GetOrCompute(name);
			foreach (MyToolActionDefinition toolAction in toolActions)
			{
				if (toolAction.HitConditions.Length != 0 && toolAction.Name == orCompute)
				{
					bestAction = toolAction;
					bestCondition = toolAction.HitConditions[0];
					break;
				}
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
		{
			DrawHud(camera, playerId);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			if (m_primaryToolAction.HasValue && m_toolComponents.ContainsKey(m_primaryHitCondition.Component))
			{
				m_toolComponents[m_primaryHitCondition.Component].DrawHud();
			}
		}

		private string GetStateForTarget(MyEntity targetEntity, uint shapeKey, string actionType)
		{
			if (targetEntity == null)
			{
				return null;
			}
			string text = null;
			if (m_toolComponents.TryGetValue(actionType, out IMyHandToolComponent value))
			{
				text = value.GetStateForTarget(targetEntity, shapeKey);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			foreach (KeyValuePair<string, IMyHandToolComponent> toolComponent in m_toolComponents)
			{
				text = toolComponent.Value.GetStateForTarget(targetEntity, shapeKey);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return null;
		}

		public Vector3 DirectionToTarget(Vector3D target)
		{
			return target;
		}

		public virtual void BeginShoot(MyShootActionEnum action)
		{
		}

		public virtual void EndShoot(MyShootActionEnum action)
		{
			if (m_shotToolAction.HasValue && m_shotToolAction.Value.HitDuration == 0f)
			{
				m_shotToolAction = null;
			}
			m_isActionDoubleClicked[action] = false;
		}

		public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
		}

		public int GetAmmunitionAmount()
		{
			return 0;
		}

		public void StopShooting(MyEntity attacker)
		{
			if (!IsShooting)
			{
				return;
			}
			float num = 0f;
			MyCharacter myCharacter = attacker as MyCharacter;
			if (myCharacter != null)
			{
				MyHandToolBase myHandToolBase = myCharacter.CurrentWeapon as MyHandToolBase;
				if (myHandToolBase != null && myHandToolBase.m_shotToolAction.HasValue)
				{
					num = myHandToolBase.m_shotToolAction.Value.HitDuration - ((float)MySandboxGame.TotalGamePlayTimeInMilliseconds - (float)myHandToolBase.m_lastShot / 1000f);
				}
			}
			float num2 = (num > 0f) ? num : 0.4f;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => StopShootingRequest, base.EntityId, num2);
			StopShooting(num2);
		}

		internal void StopShooting(float hitDelaySec)
		{
			if (IsShooting)
			{
				m_lastHit = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_hitDelay = (int)(hitDelaySec * 1000f);
				m_owner.PlayCharacterAnimation(m_shotHitCondition.Animation, MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0.2f, m_shotHitCondition.AnimationTimeScale, sync: false, null, excludeLegsWhenMoving: true);
				m_shotToolAction = null;
				m_wasShooting = false;
			}
		}

		[Event(null, 867)]
		[Reliable]
		[Broadcast]
		private static void StopShootingRequest(long entityId, float attackDelay)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(entityId, out entity);
			(entity as MyHandToolBase)?.StopShooting(attackDelay);
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

		public void DoubleClicked(MyShootActionEnum action)
		{
			m_isActionDoubleClicked[action] = true;
		}
	}
}
