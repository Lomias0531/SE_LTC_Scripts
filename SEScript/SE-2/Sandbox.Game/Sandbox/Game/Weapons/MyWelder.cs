#define VRAGE
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
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
	[MyEntityType(typeof(MyObjectBuilder_Welder), true)]
	public class MyWelder : MyEngineerToolBase, IMyWelder, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
	{
		public struct ProjectionRaycastData
		{
			public BuildCheckResult raycastResult;

			public MySlimBlock hitCube;

			public MyProjectorBase cubeProjector;

			public ProjectionRaycastData(BuildCheckResult result, MySlimBlock cubeBlock, MyProjectorBase projector)
			{
				raycastResult = result;
				hitCube = cubeBlock;
				cubeProjector = projector;
			}
		}

		private class Sandbox_Game_Weapons_MyWelder_003C_003EActor : IActivator, IActivator<MyWelder>
		{
			private sealed override object CreateInstance()
			{
				return new MyWelder();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWelder CreateInstance()
			{
				return new MyWelder();
			}

			MyWelder IActivator<MyWelder>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MySoundPair m_weldSoundIdle = new MySoundPair("ToolPlayWeldIdle");

		private MySoundPair m_weldSoundWeld = new MySoundPair("ToolPlayWeldMetal");

		private MySoundPair m_weldSoundFlame = new MySoundPair("ArcShipSmNuclearLrg");

		public static readonly float WELDER_AMOUNT_PER_SECOND = 1f;

		public static readonly float WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED = 0.6f;

		public static MatrixD WELDER_ANGLE = MatrixD.CreateRotationX(0.49000000953674316);

		private static int SUPRESS_TIME_LIMIT = 180;

		private static MyHudNotificationBase m_missingComponentNotification = new MyHudNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, 2500, "Red");

		private MyHudNotification m_safezoneNotification;

		private static MyDefinitionId m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "WelderItem");

		private MySlimBlock m_failedBlock;

		private bool m_playedFailSound;

		private MySlimBlock m_failedBlockSound;

		private float m_lastWeldingDistance = float.MaxValue;

		private bool m_lastWeldingDistanceCheck;

		private int m_timedShootSupression;

		private Vector3I m_targetProjectionCube;

		private MyCubeGrid m_targetProjectionGrid;

		private MyParticleEffect m_flameEffect;

		private string m_flameEffectName = "WelderFlame";

		private bool m_showContactSpark = true;

		private bool ShowContactSpark
		{
			get
			{
				return m_showContactSpark;
			}
			set
			{
				if (m_showContactSpark != value)
				{
					m_showContactSpark = value;
					ShowContactSparkChanged();
				}
			}
		}

		public override bool IsSkinnable => true;

		private float WeldAmount => MySession.Static.WelderSpeedMultiplier * m_speedMultiplier * WELDER_AMOUNT_PER_SECOND * (float)base.ToolCooldownMs / 1000f;

		public MyWelder()
			: base(250)
		{
			base.HasCubeHighlight = true;
			base.HighlightColor = Color.Green * 0.75f;
			base.HighlightMaterial = MyStringId.GetOrCompute("GizmoDrawLine");
			SecondaryLightIntensityLower = 0.4f;
			SecondaryLightIntensityUpper = 0.4f;
			SecondaryEffectName = "WelderContactPoint";
			HasSecondaryEffect = false;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "WelderItem");
			if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
			{
				m_physicalItemId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
			}
			base.PhysicalObject = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(m_physicalItemId);
			Init(objectBuilder, m_physicalItemId);
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(m_physicalItemId);
			Init(null, physicalItemDefinition.Model, null, null);
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			base.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase)objectBuilder.Clone();
			base.PhysicalObject.GunEntity.EntityId = base.EntityId;
			MyWelderDefinition myWelderDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(m_physicalItemId) as MyWelderDefinition;
			if (myWelderDefinition != null)
			{
				m_flameEffectName = myWelderDefinition.FlameEffect;
			}
			foreach (ToolSound toolSound in m_handItemDef.ToolSounds)
			{
				if (toolSound.type != null && toolSound.subtype != null && toolSound.sound != null && toolSound.type.Equals("Main"))
				{
					if (toolSound.subtype.Equals("Idle"))
					{
						m_weldSoundIdle = new MySoundPair(toolSound.sound);
					}
					if (toolSound.subtype.Equals("Weld"))
					{
						m_weldSoundWeld = new MySoundPair(toolSound.sound);
					}
					if (toolSound.subtype.Equals("Flame"))
					{
						m_weldSoundFlame = new MySoundPair(toolSound.sound);
					}
				}
			}
		}

		protected override bool ShouldBePowered()
		{
			if (!base.ShouldBePowered())
			{
				return false;
			}
			return true;
		}

		protected override void DrawHud()
		{
			MyHud.BlockInfo.Visible = false;
			_ = m_targetProjectionCube;
			if (m_targetProjectionGrid == null)
			{
				base.DrawHud();
				return;
			}
			MySlimBlock mySlimBlock = m_targetProjectionGrid.GetCubeBlock(m_targetProjectionCube);
			if (mySlimBlock == null)
			{
				base.DrawHud();
				return;
			}
			MyHud.BlockInfo.Visible = true;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && mySlimBlock.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock.GetBlocksCount() > 0)
				{
					mySlimBlock = myCompoundCubeBlock.GetBlocks().First();
				}
			}
			int num = mySlimBlock.GetStockpileStamp() + mySlimBlock.ComponentStack.LastChangeStamp;
			if (LastTargetObject != mySlimBlock || num != LastTargetStamp)
			{
				LastTargetStamp = num;
				MyHud.BlockInfo.MissingComponentIndex = 0;
				MyHud.BlockInfo.BlockName = mySlimBlock.BlockDefinition.DisplayNameText;
				MyHud.BlockInfo.PCUCost = mySlimBlock.BlockDefinition.PCU;
				MyHud.BlockInfo.BlockIcons = mySlimBlock.BlockDefinition.Icons;
				MyHud.BlockInfo.BlockIntegrity = 0.01f;
				MyHud.BlockInfo.CriticalIntegrity = mySlimBlock.BlockDefinition.CriticalIntegrityRatio;
				MyHud.BlockInfo.CriticalComponentIndex = mySlimBlock.BlockDefinition.CriticalGroup;
				MyHud.BlockInfo.OwnershipIntegrity = mySlimBlock.BlockDefinition.OwnershipIntegrityRatio;
				MyHud.BlockInfo.BlockBuiltBy = mySlimBlock.BuiltBy;
				MyHud.BlockInfo.GridSize = mySlimBlock.CubeGrid.GridSizeEnum;
				MyHud.BlockInfo.Components.Clear();
				for (int i = 0; i < mySlimBlock.ComponentStack.GroupCount; i++)
				{
					MyComponentStack.GroupInfo groupInfo = mySlimBlock.ComponentStack.GetGroupInfo(i);
					MyHudBlockInfo.ComponentInfo item = default(MyHudBlockInfo.ComponentInfo);
					item.DefinitionId = groupInfo.Component.Id;
					item.ComponentName = groupInfo.Component.DisplayNameText;
					item.Icons = groupInfo.Component.Icons;
					item.TotalCount = groupInfo.TotalCount;
					item.MountedCount = 0;
					item.StockpileCount = 0;
					MyHud.BlockInfo.Components.Add(item);
				}
				MyHud.BlockInfo.SetContextHelp(mySlimBlock.BlockDefinition);
				LastTargetObject = mySlimBlock;
			}
		}

		public override bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
		{
			if (!MySessionComponentSafeZones.IsActionAllowed(Owner, MySafeZoneAction.Welding, 0L, 0uL))
			{
				status = MyGunStatusEnum.SafeZoneDenied;
				return false;
			}
			if (action == MyShootActionEnum.PrimaryAction)
			{
				if (!base.CanShoot(action, shooter, out status))
				{
					return false;
				}
				if (!base.SinkComp.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 0.0001f) && MySession.Static != null && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					return false;
				}
			}
			else
			{
				status = MyGunStatusEnum.OK;
			}
			MySlimBlock targetBlock = GetTargetBlock();
			MyCharacter owner = Owner;
			if (targetBlock != null && !targetBlock.CanContinueBuild(owner.GetInventory()) && !targetBlock.IsFullIntegrity && Owner != null && Owner == MySession.Static.LocalCharacter && MySession.Static.Settings.GameMode == MyGameModeEnum.Survival && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				targetBlock.ComponentStack.GetMissingInfo(out int groupIndex, out int componentCount);
				MyComponentStack.GroupInfo groupInfo = targetBlock.ComponentStack.GetGroupInfo(groupIndex);
				MarkMissingComponent(groupIndex);
				m_missingComponentNotification.SetTextFormatArguments($"{groupInfo.Component.DisplayNameText} ({componentCount}x)", targetBlock.BlockDefinition.DisplayNameText.ToString());
				MyHud.Notifications.Add(m_missingComponentNotification);
				if ((m_playedFailSound && m_failedBlockSound != targetBlock) || !m_playedFailSound)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					m_playedFailSound = true;
					m_failedBlockSound = targetBlock;
				}
			}
			return true;
		}

		private bool CanWeld(MySlimBlock block)
		{
			ulong user = 0uL;
			if (Owner != null && Owner.ControllerInfo != null && Owner.ControllerInfo.Controller != null && Owner.ControllerInfo.Controller.Player != null)
			{
				user = Owner.ControllerInfo.Controller.Player.Id.SteamId;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(block.WorldAABB, MySafeZoneAction.Welding, 0L, user))
			{
				return false;
			}
			if (!block.IsFullIntegrity || block.HasDeformation)
			{
				return true;
			}
			return false;
		}

		private MyProjectorBase GetProjector(MySlimBlock block)
		{
			MySlimBlock mySlimBlock = block.CubeGrid.GetBlocks().FirstOrDefault((MySlimBlock b) => b.FatBlock is MyProjectorBase);
			if (mySlimBlock != null)
			{
				return mySlimBlock.FatBlock as MyProjectorBase;
			}
			return null;
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			MyAnalyticsHelper.ReportActivityStartIf(!m_activated, Owner, "Welding", "Character", "HandTools", "Welder");
			base.Shoot(action, direction, overrideWeaponPos, gunAction);
			ShowContactSpark = false;
			if (action != 0)
			{
				return;
			}
			MySlimBlock targetBlock = GetTargetBlock();
			if (!base.IsPreheated)
			{
				if (targetBlock != null)
				{
					FillStockpile();
				}
				return;
			}
			if (targetBlock == null)
			{
				m_lastWeldingDistance = float.MaxValue;
				m_lastWeldingDistanceCheck = false;
			}
			if (targetBlock != null && m_activated && CanWeld(targetBlock))
			{
				if (MySession.Static.CheckResearchAndNotify(Owner.GetPlayerIdentityId(), targetBlock.BlockDefinition.Id))
				{
					Weld();
				}
			}
			else
			{
				if (Owner != MySession.Static.LocalCharacter || Owner == null)
				{
					return;
				}
				ProjectionRaycastData projectionRaycastData = FindProjectedBlock(m_raycastComponent, m_distanceMultiplier);
				if (projectionRaycastData.raycastResult != 0 || !MySession.Static.CheckResearchAndNotify(Owner.GetPlayerIdentityId(), projectionRaycastData.hitCube.BlockDefinition.Id))
				{
					return;
				}
				bool flag = MySession.Static.CreativeMode;
				if (MySession.Static.Players.TryGetPlayerId(base.OwnerIdentityId, out MyPlayer.PlayerId result) && MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
				{
					flag |= MySession.Static.CreativeToolsEnabled(Sync.MyId);
				}
				if (MySession.Static.CheckLimitsAndNotify(targetBlock?.BuiltBy ?? Owner.ControllerInfo.Controller.Player.Identity.IdentityId, projectionRaycastData.hitCube.BlockDefinition.BlockPairName, flag ? projectionRaycastData.hitCube.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 0, projectionRaycastData.cubeProjector.CubeGrid.BlocksCount))
				{
					if (MySession.Static.CreativeMode || MyBlockBuilderBase.SpectatorIsBuilding || Owner.CanStartConstruction(projectionRaycastData.hitCube.BlockDefinition) || MySession.Static.CreativeToolsEnabled(Sync.MyId))
					{
						projectionRaycastData.cubeProjector.Build(projectionRaycastData.hitCube, Owner.ControllerInfo.Controller.Player.Identity.IdentityId, Owner.EntityId, requestInstant: true, Owner.ControllerInfo.Controller.Player.Identity.IdentityId);
					}
					else
					{
						MyBlockPlacerBase.OnMissingComponents(projectionRaycastData.hitCube.BlockDefinition);
					}
				}
			}
		}

		public static void AddMissingComponentsToBuildPlanner(MySlimBlock block)
		{
			if (block == null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return;
			}
			if (block.CubeGrid.IsPreview)
			{
				if (MySession.Static.LocalCharacter.AddToBuildPlanner(block.BlockDefinition))
				{
					MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
				return;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			block.GetMissingComponents(dictionary);
			List<MyIdentity.BuildPlanItem.Component> list = new List<MyIdentity.BuildPlanItem.Component>();
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_Component), item.Key);
				MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(id);
				list.Add(new MyIdentity.BuildPlanItem.Component
				{
					ComponentDefinition = componentDefinition,
					Count = item.Value
				});
			}
			if (list.Count > 0 && MySession.Static.LocalCharacter.AddToBuildPlanner(block.BlockDefinition, -1, list))
			{
				MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
			}
		}

		public override void EndShoot(MyShootActionEnum action)
		{
			if (m_activated)
			{
				MyAnalyticsHelper.ReportActivityEnd(Owner, "Welding");
			}
			if (m_shooting && action == MyShootActionEnum.SecondaryAction && !Sync.IsDedicated && MySession.Static != null && Owner == MySession.Static.LocalCharacter)
			{
				MySlimBlock targetBlock = GetTargetBlock();
				if (targetBlock != null)
				{
					AddMissingComponentsToBuildPlanner(targetBlock);
				}
				else if (Owner != null)
				{
					ProjectionRaycastData projectionRaycastData = FindProjectedBlock(m_raycastComponent, m_distanceMultiplier);
					if (projectionRaycastData.hitCube != null)
					{
						AddMissingComponentsToBuildPlanner(projectionRaycastData.hitCube);
					}
					else
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					}
				}
			}
			m_playedFailSound = false;
			m_failedBlockSound = null;
			base.EndShoot(action);
		}

		public override void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
		{
			base.BeginFailReaction(action, status);
			FillStockpile();
		}

		public override void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.SafeZoneDenied)
			{
				if (m_safezoneNotification == null)
				{
					m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_WeldingDisabled, 2000, "Red");
				}
				MyHud.Notifications.Add(m_safezoneNotification);
			}
		}

		protected override void AddHudInfo()
		{
		}

		protected override void RemoveHudInfo()
		{
		}

		private void FillStockpile()
		{
			MySlimBlock targetBlock = GetTargetBlock();
			if (targetBlock != null)
			{
				FillStockpile(targetBlock);
			}
		}

		private void FillStockpile(MySlimBlock block)
		{
			if (Sync.IsServer)
			{
				block.MoveItemsToConstructionStockpile(base.CharacterInventory);
			}
			else
			{
				block.RequestFillStockpile(base.CharacterInventory);
			}
		}

		private void ShowContactSparkChanged()
		{
			if (!m_showContactSpark)
			{
				StopEffect();
			}
		}

		public override bool CanStartEffect()
		{
			return m_showContactSpark;
		}

		private void Weld()
		{
			bool showContactSpark = false;
			MySlimBlock targetBlock = GetTargetBlock();
			if (targetBlock != null)
			{
				MyCubeBlockDefinition.PreloadConstructionModels(targetBlock.BlockDefinition);
				if (Sync.IsServer)
				{
					targetBlock.MoveItemsToConstructionStockpile(base.CharacterInventory);
					targetBlock.MoveUnneededItemsFromConstructionStockpile(base.CharacterInventory);
				}
				bool hasDeformation = targetBlock.HasDeformation;
				if (hasDeformation || targetBlock.MaxDeformation > 0f || !targetBlock.IsFullIntegrity)
				{
					float maxAllowedBoneMovement = WELDER_MAX_REPAIR_BONE_MOVEMENT_SPEED * (float)base.ToolCooldownMs * 0.001f;
					if (Owner != null && Owner.ControllerInfo != null)
					{
						bool? flag = targetBlock.ComponentStack.WillFunctionalityRise(WeldAmount);
						if (flag.HasValue && flag.Value && !MySession.Static.CheckLimitsAndNotify(targetBlock.BuiltBy, targetBlock.BlockDefinition.BlockPairName, targetBlock.BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST))
						{
							return;
						}
						showContactSpark = ((!Sync.IsServer) ? ((!targetBlock.IsFullIntegrity || targetBlock.HasDeformation) && m_failedBlockSound == null) : targetBlock.IncreaseMountLevel(WeldAmount, Owner.ControllerInfo.ControllingIdentityId, base.CharacterInventory, maxAllowedBoneMovement, isHelping: false, MyOwnershipShareModeEnum.Faction, handWelded: true));
						showContactSpark = (showContactSpark || hasDeformation);
						if (MySession.Static != null && Owner == MySession.Static.LocalCharacter && MyMusicController.Static != null)
						{
							MyMusicController.Static.Building(250);
						}
					}
				}
			}
			if (Sync.IsServer)
			{
				IMyDestroyableObject targetDestroyable = GetTargetDestroyable();
				if (targetDestroyable is MyCharacter && Sync.IsServer)
				{
					targetDestroyable.DoDamage(20f, MyDamageType.Weld, sync: true, null, base.EntityId);
				}
			}
			ShowContactSpark = showContactSpark;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_timedShootSupression > 0)
			{
				m_timedShootSupression--;
			}
			if (Owner != null && Owner == MySession.Static.LocalCharacter)
			{
				CheckProjection();
			}
			if (Owner == null || MySession.Static.ControlledEntity != Owner)
			{
				RemoveHudInfo();
			}
			UpdateFlameEffect();
		}

		private void UpdateFlameEffect()
		{
			if (EffectAction == MyShootActionEnum.PrimaryAction || EffectAction == MyShootActionEnum.SecondaryAction)
			{
				if (m_flameEffect == null)
				{
					MyParticlesManager.TryCreateParticleEffect("WelderFlame", GetEffectMatrix(0f, EffectType.EffectSecondary), out m_flameEffect);
				}
				if (m_flameEffect != null)
				{
					m_flameEffect.WorldMatrix = GetEffectMatrix(0f, EffectType.EffectSecondary);
				}
			}
			else if (m_flameEffect != null)
			{
				m_flameEffect.Stop();
				m_flameEffect = null;
			}
		}

		protected override MatrixD GetEffectMatrix(float muzzleOffset, EffectType effectType)
		{
			Vector3D forward = base.PositionComp.WorldMatrix.Forward;
			Vector3D muzzleWorldPosition = m_gunBase.GetMuzzleWorldPosition();
			if (effectType == EffectType.Effect)
			{
				Vector3D vector3D = Vector3D.Rotate(WELDER_ANGLE.Forward, base.PositionComp.WorldMatrix);
				Vector3D vector3D2 = muzzleWorldPosition + 0.05000000074505806 * base.PositionComp.WorldMatrix.Up;
				m_lastWeldingDistance = Vector3.Dot(m_raycastComponent.HitPosition - vector3D2, forward);
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(vector3D2 - 0.5 * vector3D, vector3D2 + 1.5 * vector3D, 15);
				Vector3D position;
				if (hitInfo.HasValue)
				{
					float num = Vector3.Dot(hitInfo.Value.Position - vector3D2, vector3D);
					position = ((!(num > 0.1f)) ? (vector3D2 + num * vector3D) : (vector3D2 + 0.10000000149011612 * vector3D));
				}
				else
				{
					position = vector3D2 + 0.10000000149011612 * vector3D;
				}
				return MatrixD.CreateWorld(position, -vector3D, base.PositionComp.WorldMatrix.Up);
			}
			return MatrixD.CreateWorld(muzzleWorldPosition, forward, base.PositionComp.WorldMatrix.Up);
		}

		protected override MySlimBlock GetTargetBlockForShoot()
		{
			MySlimBlock targetBlock = GetTargetBlock();
			if (targetBlock != null && ShowContactSpark)
			{
				return targetBlock;
			}
			return null;
		}

		private void CheckProjection()
		{
			MySlimBlock targetBlock = GetTargetBlock();
			if (targetBlock != null && CanWeld(targetBlock))
			{
				m_targetProjectionGrid = null;
				return;
			}
			if (Owner != null)
			{
				ProjectionRaycastData projectionRaycastData = FindProjectedBlock(m_raycastComponent, m_distanceMultiplier);
				if (projectionRaycastData.raycastResult != BuildCheckResult.NotFound)
				{
					if (projectionRaycastData.raycastResult == BuildCheckResult.OK)
					{
						MyCubeBuilder.DrawSemiTransparentBox(projectionRaycastData.hitCube.CubeGrid, projectionRaycastData.hitCube, Color.Green.ToVector4(), onlyWireframe: true, MyStringId.GetOrCompute("GizmoDrawLine"));
						m_targetProjectionCube = projectionRaycastData.hitCube.Position;
						m_targetProjectionGrid = projectionRaycastData.hitCube.CubeGrid;
						return;
					}
					if (projectionRaycastData.raycastResult == BuildCheckResult.IntersectedWithGrid || projectionRaycastData.raycastResult == BuildCheckResult.IntersectedWithSomethingElse)
					{
						MyCubeBuilder.DrawSemiTransparentBox(projectionRaycastData.hitCube.CubeGrid, projectionRaycastData.hitCube, Color.Red.ToVector4(), onlyWireframe: true);
					}
					else if (projectionRaycastData.raycastResult == BuildCheckResult.NotConnected)
					{
						MyCubeBuilder.DrawSemiTransparentBox(projectionRaycastData.hitCube.CubeGrid, projectionRaycastData.hitCube, Color.Yellow.ToVector4(), onlyWireframe: true);
					}
				}
			}
			m_targetProjectionGrid = null;
		}

		public static ProjectionRaycastData FindProjectedBlock(MyCasterComponent rayCaster, float distanceMultiplier = 1f)
		{
			Vector3D center = rayCaster.Caster.Center;
			Vector3D value = rayCaster.Caster.FrontPoint - rayCaster.Caster.Center;
			value.Normalize();
			float num = MyEngineerToolBase.DEFAULT_REACH_DISTANCE * distanceMultiplier;
			Vector3D vector3D = center + value * num;
			LineD line = new LineD(center, vector3D);
			ProjectionRaycastData projectionRaycastData2;
			if (MyCubeGrid.GetLineIntersection(ref line, out MyCubeGrid grid, out Vector3I _, out double _, (MyCubeGrid x) => x.Projector != null) && grid.Projector != null)
			{
				MyProjectorBase projector = grid.Projector;
				List<MyCube> list = grid.RayCastBlocksAllOrdered(center, vector3D);
				ProjectionRaycastData? projectionRaycastData = null;
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					MyCube myCube = list[num2];
					BuildCheckResult buildCheckResult = projector.CanBuild(myCube.CubeBlock, checkHavokIntersections: true);
					switch (buildCheckResult)
					{
					case BuildCheckResult.OK:
						projectionRaycastData2 = new ProjectionRaycastData
						{
							raycastResult = buildCheckResult,
							hitCube = myCube.CubeBlock,
							cubeProjector = projector
						};
						projectionRaycastData = projectionRaycastData2;
						break;
					case BuildCheckResult.AlreadyBuilt:
						projectionRaycastData = null;
						break;
					}
				}
				if (projectionRaycastData.HasValue)
				{
					return projectionRaycastData.Value;
				}
			}
			projectionRaycastData2 = default(ProjectionRaycastData);
			projectionRaycastData2.raycastResult = BuildCheckResult.NotFound;
			return projectionRaycastData2;
		}

		protected override void StartLoopSound(bool effect)
		{
			bool flag = Owner != null && Owner.IsInFirstPersonView && Owner == MySession.Static.LocalCharacter;
			MySoundPair soundId = effect ? m_weldSoundWeld : m_weldSoundFlame;
			if (m_soundEmitter.Sound != null && m_soundEmitter.Sound.IsPlaying)
			{
				if (flag != m_soundEmitter.Force2D)
				{
					m_soundEmitter.PlaySound(soundId, stopPrevious: true, skipIntro: true, flag);
				}
				else
				{
					m_soundEmitter.PlaySingleSound(soundId, stopPrevious: true, skipIntro: true);
				}
			}
			else
			{
				m_soundEmitter.PlaySound(soundId, stopPrevious: true, skipIntro: true, flag);
			}
		}

		protected override void StopLoopSound()
		{
			StopSound();
		}

		protected override void StopSound()
		{
			m_soundEmitter.StopSound(forced: true);
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_flameEffect != null)
			{
				m_flameEffect.Stop();
				m_flameEffect = null;
			}
		}

		public override bool SupressShootAnimation()
		{
			bool flag = m_lastWeldingDistance < 0.05f;
			if (m_lastWeldingDistanceCheck != flag && m_timedShootSupression < SUPRESS_TIME_LIMIT)
			{
				if (m_timedShootSupression > 0)
				{
					m_timedShootSupression += (int)(MyRandom.Instance.GetRandomFloat(0.8f, 1.6f) * (float)SUPRESS_TIME_LIMIT);
				}
				else
				{
					m_timedShootSupression += SUPRESS_TIME_LIMIT;
				}
			}
			m_lastWeldingDistanceCheck = flag;
			if (!flag)
			{
				return m_timedShootSupression > SUPRESS_TIME_LIMIT;
			}
			return true;
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
