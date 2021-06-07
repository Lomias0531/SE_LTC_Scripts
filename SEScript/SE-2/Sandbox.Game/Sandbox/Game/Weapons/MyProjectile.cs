using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	internal class MyProjectile
	{
		private enum MyProjectileStateEnum : byte
		{
			ACTIVE,
			KILLED
		}

		private const int CHECK_INTERSECTION_INTERVAL = 5;

		private MyProjectileStateEnum m_state;

		private Vector3D m_origin;

		private Vector3D m_velocity_Projectile;

		private Vector3D m_velocity_Combined;

		private Vector3D m_directionNormalized;

		private float m_speed;

		private float m_maxTrajectory;

		private Vector3D m_position;

		private MyEntity[] m_ignoredEntities;

		private MyEntity m_weapon;

		private MyCharacterHitInfo m_charHitInfo;

		private MyCubeGrid.MyCubeGridHitInfo m_cubeGridHitInfo;

		public float LengthMultiplier = 1f;

		private MyProjectileAmmoDefinition m_projectileAmmoDefinition;

		private MyStringId m_projectileTrailMaterialId;

		public MyEntity OwnerEntity;

		public MyEntity OwnerEntityAbsolute;

		private int m_checkIntersectionIndex;

		private static int checkIntersectionCounter = 0;

		private bool m_positionChecked;

		private static List<MyLineSegmentOverlapResult<MyEntity>> m_entityRaycastResult = null;

		private static List<MyPhysics.HitInfo> m_raycastResult = new List<MyPhysics.HitInfo>(16);

		private const float m_impulseMultiplier = 0.5f;

		private static MyStringHash m_hashBolt = MyStringHash.GetOrCompute("Bolt");

		private static MyStringId ID_PROJECTILE_TRAIL_LINE = MyStringId.GetOrCompute("ProjectileTrailLine");

		public static readonly MyTimedItemCache CollisionSoundsTimedCache = new MyTimedItemCache(60);

		public static readonly MyTimedItemCache CollisionParticlesTimedCache = new MyTimedItemCache(200);

		public static double CollisionSoundSpaceMapping = 0.039999999105930328;

		public static double CollisionParticlesSpaceMapping = 0.800000011920929;

		private static readonly MyTimedItemCache m_prefetchedVoxelRaysTimedCache = new MyTimedItemCache(4000);

		private const double m_prefetchedVoxelRaysSourceMapping = 0.5;

		private const double m_prefetchedVoxelRaysDirectionMapping = 50.0;

		public static bool DEBUG_DRAW_PROJECTILE_TRAJECTORY = false;

		public void Start(MyProjectileAmmoDefinition ammoDefinition, MyWeaponDefinition weaponDefinition, MyEntity[] ignoreEntities, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, MyEntity weapon)
		{
			m_projectileAmmoDefinition = ammoDefinition;
			m_state = MyProjectileStateEnum.ACTIVE;
			m_ignoredEntities = ignoreEntities;
			m_origin = origin + 0.1 * (Vector3D)directionNormalized;
			m_position = m_origin;
			m_weapon = weapon;
			if (ammoDefinition.ProjectileTrailMaterial != null)
			{
				m_projectileTrailMaterialId = MyStringId.GetOrCompute(ammoDefinition.ProjectileTrailMaterial);
			}
			if (ammoDefinition.ProjectileTrailProbability >= MyUtils.GetRandomFloat(0f, 1f))
			{
				LengthMultiplier = 40f;
			}
			else
			{
				LengthMultiplier = 0f;
			}
			m_directionNormalized = directionNormalized;
			m_speed = ammoDefinition.DesiredSpeed * ((ammoDefinition.SpeedVar > 0f) ? MyUtils.GetRandomFloat(1f - ammoDefinition.SpeedVar, 1f + ammoDefinition.SpeedVar) : 1f);
			m_velocity_Projectile = m_directionNormalized * m_speed;
			m_velocity_Combined = initialVelocity + m_velocity_Projectile;
			m_maxTrajectory = ammoDefinition.MaxTrajectory;
			bool flag = true;
			if (weaponDefinition != null)
			{
				m_maxTrajectory *= weaponDefinition.RangeMultiplier;
				flag = weaponDefinition.UseRandomizedRange;
			}
			if (flag)
			{
				m_maxTrajectory *= MyUtils.GetRandomFloat(0.8f, 1f);
			}
			m_checkIntersectionIndex = checkIntersectionCounter % 5;
			checkIntersectionCounter += 3;
			m_positionChecked = false;
			PrefetchVoxelPhysicsIfNeeded();
		}

		private void PrefetchVoxelPhysicsIfNeeded()
		{
			LineD ray = new LineD(m_origin, m_origin + m_directionNormalized * m_maxTrajectory, m_maxTrajectory);
			LineD lineD = new LineD(new Vector3D(Math.Floor(ray.From.X) * 0.5, Math.Floor(ray.From.Y) * 0.5, Math.Floor(ray.From.Z) * 0.5), new Vector3D(Math.Floor(m_directionNormalized.X * 50.0), Math.Floor(m_directionNormalized.Y * 50.0), Math.Floor(m_directionNormalized.Z * 50.0)));
			if (!m_prefetchedVoxelRaysTimedCache.IsItemPresent(lineD.GetHash(), MySandboxGame.TotalSimulationTimeInMilliseconds))
			{
				using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
				{
					MyGamePruningStructure.GetAllEntitiesInRay(ref ray, m_entityRaycastResult, MyEntityQueryType.Static);
					foreach (MyLineSegmentOverlapResult<MyEntity> item in m_entityRaycastResult)
					{
						(item.Element as MyVoxelPhysics)?.PrefetchShapeOnRay(ref ray);
					}
				}
			}
		}

		private bool IsIgnoredEntity(IMyEntity entity)
		{
			if (m_ignoredEntities != null)
			{
				MyEntity[] ignoredEntities = m_ignoredEntities;
				foreach (MyEntity myEntity in ignoredEntities)
				{
					if (entity == myEntity)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Update()
		{
			if (m_state == MyProjectileStateEnum.KILLED)
			{
				return false;
			}
			Vector3D position = m_position;
			m_position += m_velocity_Combined * 0.01666666753590107 * MyFakes.SIMULATION_SPEED;
			if (DEBUG_DRAW_PROJECTILE_TRAJECTORY)
			{
				MyRenderProxy.DebugDrawLine3D(position, m_position, Color.AliceBlue, Color.AliceBlue, depthRead: true);
			}
			Vector3 vector = m_position - m_origin;
			if (Vector3.Dot(vector, vector) >= m_maxTrajectory * m_maxTrajectory)
			{
				StopEffect();
				m_state = MyProjectileStateEnum.KILLED;
				return false;
			}
			m_checkIntersectionIndex = ++m_checkIntersectionIndex % 5;
			if (m_checkIntersectionIndex != 0 && m_positionChecked)
			{
				return true;
			}
			Vector3D to = position + 5.0 * (m_velocity_Projectile * 0.01666666753590107 * MyFakes.SIMULATION_SPEED);
			LineD line = new LineD(m_positionChecked ? position : m_origin, to);
			m_positionChecked = true;
			GetHitEntityAndPosition(line, out IMyEntity entity, out MyHitInfo hitInfoRet, out object customdata);
			if (entity == null)
			{
				return true;
			}
			if (IsIgnoredEntity(entity))
			{
				return true;
			}
			bool flag = false;
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				(myCharacter.CurrentWeapon as IStoppableAttackingTool)?.StopShooting(OwnerEntity);
				if (customdata != null)
				{
					flag = ((customdata as MyCharacterHitInfo).HitHead && m_projectileAmmoDefinition.HeadShot);
				}
			}
			m_position = hitInfoRet.Position;
			m_position += line.Direction * 0.01;
			float num = 1f;
			if (m_weapon is IMyHandheldGunObject<MyGunBase>)
			{
				MyGunBase gunBase = (m_weapon as IMyHandheldGunObject<MyGunBase>).GunBase;
				if (gunBase != null && gunBase.WeaponProperties != null && gunBase.WeaponProperties.WeaponDefinition != null)
				{
					num = gunBase.WeaponProperties.WeaponDefinition.DamageMultiplier;
				}
			}
			GetSurfaceAndMaterial(entity, ref line, ref m_position, hitInfoRet.ShapeKey, out MySurfaceImpactEnum surfaceImpact, out MyStringHash materialType);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				PlayHitSound(materialType, entity, hitInfoRet.Position, m_projectileAmmoDefinition.PhysicalMaterial);
			}
			hitInfoRet.Velocity = m_velocity_Combined;
			float num2 = (!(entity is IMyCharacter)) ? m_projectileAmmoDefinition.ProjectileMassDamage : (flag ? m_projectileAmmoDefinition.ProjectileHeadShotDamage : m_projectileAmmoDefinition.ProjectileHealthDamage);
			num2 *= num;
			ulong user = 0uL;
			MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(OwnerEntity);
			if (controllingPlayer != null)
			{
				user = controllingPlayer.Id.SteamId;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(hitInfoRet.Position, MySafeZoneAction.Damage, 0L, user))
			{
				num2 = 0f;
			}
			if (num2 > 0f)
			{
				DoDamage(num2, hitInfoRet, customdata, entity);
			}
			MyDecals.HandleAddDecal(entity, hitInfoRet, materialType, m_projectileAmmoDefinition.PhysicalMaterial, customdata as MyCharacterHitInfo, num2);
			CreateDecal(materialType);
			if (!Sandbox.Engine.Platform.Game.IsDedicated && !CollisionParticlesTimedCache.IsPlaceUsed(hitInfoRet.Position, CollisionParticlesSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds + MyRandom.Instance.Next(0, CollisionParticlesTimedCache.EventTimeoutMs / 2)))
			{
				_ = Vector3.Zero;
				MyCubeBlock obj = entity as MyCubeBlock;
				IMyEntity entity2 = entity;
				if (obj != null && entity.Parent != null)
				{
					entity2 = entity.Parent;
				}
				if (!MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Hit, hitInfoRet.Position, hitInfoRet.Normal, m_projectileAmmoDefinition.PhysicalMaterial, materialType, entity2) && surfaceImpact != MySurfaceImpactEnum.CHARACTER)
				{
					CreateBasicHitParticles(m_projectileAmmoDefinition.ProjectileOnHitEffectName, ref hitInfoRet.Position, ref hitInfoRet.Normal, ref line.Direction, entity, m_weapon, 1f, OwnerEntity);
				}
			}
			if (num2 > 0f && (m_weapon == null || entity.GetTopMostParent() != m_weapon.GetTopMostParent()))
			{
				ApplyProjectileForce(entity, hitInfoRet.Position, m_directionNormalized, isPlayerShip: false, m_projectileAmmoDefinition.ProjectileHitImpulse * 0.5f);
			}
			StopEffect();
			m_state = MyProjectileStateEnum.KILLED;
			return false;
		}

		private static void CreateBasicHitParticles(string effectName, ref Vector3D hitPoint, ref Vector3 normal, ref Vector3D direction, IMyEntity physObject, MyEntity weapon, float scale, MyEntity ownerEntity = null)
		{
			Vector3D v = Vector3D.Reflect(direction, normal);
			new MyUtilRandomVector3ByDeviatingVector(v);
			MatrixD matrixD = MatrixD.CreateFromDir(normal);
			if (MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(hitPoint, matrixD.Forward, matrixD.Up), out MyParticleEffect effect))
			{
				effect.UserScale = scale;
			}
		}

		private void GetHitEntityAndPosition(LineD line, out IMyEntity entity, out MyHitInfo hitInfoRet, out object customdata)
		{
			entity = null;
			hitInfoRet = default(MyHitInfo);
			customdata = null;
			using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
			{
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref line, m_entityRaycastResult);
				foreach (MyLineSegmentOverlapResult<MyEntity> item in m_entityRaycastResult)
				{
					MySafeZone mySafeZone = item.Element as MySafeZone;
					if (mySafeZone != null && mySafeZone.Enabled && !mySafeZone.AllowedActions.HasFlag(MySafeZoneAction.Shooting))
					{
						MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(OwnerEntity);
						bool flag = false;
						if (controllingPlayer != null && MySafeZone.CheckAdminIgnoreSafezones(controllingPlayer.Id.SteamId))
						{
							flag = true;
						}
						if (!flag && mySafeZone.GetIntersectionWithLine(ref line, out Vector3D? v) && v.HasValue)
						{
							hitInfoRet.Position = v.Value;
							hitInfoRet.Normal = -line.Direction;
							entity = mySafeZone;
							return;
						}
					}
				}
			}
			int num = 0;
			using (MyUtils.ReuseCollection(ref m_raycastResult))
			{
				MyPhysics.CastRay(line.From, line.To, m_raycastResult, 15);
				do
				{
					if (num < m_raycastResult.Count)
					{
						MyPhysics.HitInfo hitInfo = m_raycastResult[num];
						entity = (hitInfo.HkHitInfo.GetHitEntity() as MyEntity);
						hitInfoRet.Position = hitInfo.Position;
						hitInfoRet.Normal = hitInfo.HkHitInfo.Normal;
						hitInfoRet.ShapeKey = hitInfo.HkHitInfo.GetShapeKey(0);
					}
					if (IsIgnoredEntity(entity))
					{
						entity = null;
					}
					if (entity is MyCharacter && !Sandbox.Engine.Platform.Game.IsDedicated)
					{
						if ((entity as MyCharacter).GetIntersectionWithLine(ref line, ref m_charHitInfo))
						{
							hitInfoRet.Position = m_charHitInfo.Triangle.IntersectionPointInWorldSpace;
							hitInfoRet.Normal = m_charHitInfo.Triangle.NormalInWorldSpace;
							customdata = m_charHitInfo;
						}
						else
						{
							entity = null;
						}
					}
					else
					{
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							if (myCubeGrid.GetIntersectionWithLine(ref line, ref m_cubeGridHitInfo))
							{
								hitInfoRet.Position = m_cubeGridHitInfo.Triangle.IntersectionPointInWorldSpace;
								hitInfoRet.Normal = m_cubeGridHitInfo.Triangle.NormalInWorldSpace;
								if (Vector3.Dot(hitInfoRet.Normal, line.Direction) > 0f)
								{
									hitInfoRet.Normal = -hitInfoRet.Normal;
								}
							}
							MyHitInfo myHitInfo = default(MyHitInfo);
							myHitInfo.Position = hitInfoRet.Position;
							myHitInfo.Normal = hitInfoRet.Normal;
							if (m_cubeGridHitInfo != null)
							{
								MyCube myCube = m_cubeGridHitInfo.Triangle.UserObject as MyCube;
								if (myCube != null && myCube.CubeBlock.FatBlock != null && myCube.CubeBlock.FatBlock.Physics == null)
								{
									entity = myCube.CubeBlock.FatBlock;
								}
							}
						}
						MyVoxelBase myVoxelBase = entity as MyVoxelBase;
						if (myVoxelBase != null && myVoxelBase.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags.DIRECT_TRIANGLES))
						{
							hitInfoRet.Position = t.Value.IntersectionPointInWorldSpace;
							hitInfoRet.Normal = t.Value.NormalInWorldSpace;
							hitInfoRet.ShapeKey = 0u;
						}
					}
				}
				while (entity == null && ++num < m_raycastResult.Count);
			}
		}

		private void DoDamage(float damage, MyHitInfo hitInfo, object customdata, IMyEntity damagedEntity)
		{
			_ = (MyEntity)MySession.Static.ControlledEntity;
			if (OwnerEntityAbsolute != null && OwnerEntityAbsolute.Equals(MySession.Static.ControlledEntity) && (damagedEntity is IMyDestroyableObject || damagedEntity is MyCubeGrid))
			{
				MySession.Static.TotalDamageDealt += (uint)damage;
			}
			if (Sync.IsServer)
			{
				if (m_projectileAmmoDefinition.PhysicalMaterial == m_hashBolt)
				{
					IMyDestroyableObject myDestroyableObject = damagedEntity as IMyDestroyableObject;
					if (myDestroyableObject != null && damagedEntity is MyCharacter)
					{
						myDestroyableObject.DoDamage(damage, MyDamageType.Bolt, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0);
					}
					return;
				}
				MyCubeGrid myCubeGrid = damagedEntity as MyCubeGrid;
				MyCubeBlock myCubeBlock = damagedEntity as MyCubeBlock;
				MySlimBlock mySlimBlock = null;
				if (myCubeBlock != null)
				{
					myCubeGrid = myCubeBlock.CubeGrid;
					mySlimBlock = myCubeBlock.SlimBlock;
				}
				else if (myCubeGrid != null)
				{
					mySlimBlock = myCubeGrid.GetTargetedBlock(hitInfo.Position - 0.001f * hitInfo.Normal);
					if (mySlimBlock != null)
					{
						myCubeBlock = mySlimBlock.FatBlock;
					}
				}
				IMyDestroyableObject myDestroyableObject2;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.Physics == null || !myCubeGrid.Physics.Enabled || (!myCubeGrid.BlocksDestructionEnabled && !MyFakes.ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE))
					{
						return;
					}
					bool flag = false;
					if (mySlimBlock != null && (myCubeGrid.BlocksDestructionEnabled || mySlimBlock.ForceBlockDestructible))
					{
						mySlimBlock.DoDamage(damage, MyDamageType.Bullet, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0);
						if (myCubeBlock == null)
						{
							flag = true;
						}
					}
					if (myCubeGrid.BlocksDestructionEnabled && flag)
					{
						ApllyDeformationCubeGrid(hitInfo.Position, myCubeGrid);
					}
				}
				else if (damagedEntity is MyEntitySubpart)
				{
					if (damagedEntity.Parent != null && damagedEntity.Parent.Parent is MyCubeGrid)
					{
						hitInfo.Position = damagedEntity.Parent.WorldAABB.Center;
						DoDamage(damage, hitInfo, customdata, damagedEntity.Parent.Parent);
					}
				}
				else if ((myDestroyableObject2 = (damagedEntity as IMyDestroyableObject)) != null)
				{
					myDestroyableObject2.DoDamage(damage, MyDamageType.Bullet, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0);
				}
			}
			else
			{
				(damagedEntity as MyCharacter)?.DoDamage(damage, MyDamageType.Bullet, updateSync: false, (m_weapon != null) ? m_weapon.EntityId : 0);
			}
		}

		private MyEntity GetSubpartOwner(MyEntity entity)
		{
			if (entity == null)
			{
				return null;
			}
			if (!(entity is MyEntitySubpart))
			{
				return entity;
			}
			MyEntity myEntity = entity;
			while (myEntity is MyEntitySubpart && myEntity != null)
			{
				myEntity = myEntity.Parent;
			}
			if (myEntity == null)
			{
				return entity;
			}
			return myEntity;
		}

		private static void GetSurfaceAndMaterial(IMyEntity entity, ref LineD line, ref Vector3D hitPosition, uint shapeKey, out MySurfaceImpactEnum surfaceImpact, out MyStringHash materialType)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null)
			{
				materialType = MyMaterialType.ROCK;
				surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
				MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref hitPosition);
				if (materialAt != null)
				{
					materialType = materialAt.MaterialTypeNameHash;
				}
				return;
			}
			if (entity is MyCharacter)
			{
				surfaceImpact = MySurfaceImpactEnum.CHARACTER;
				materialType = MyMaterialType.CHARACTER;
				if ((entity as MyCharacter).Definition.PhysicalMaterial != null)
				{
					materialType = MyStringHash.GetOrCompute((entity as MyCharacter).Definition.PhysicalMaterial);
				}
				return;
			}
			if (entity is MyFloatingObject)
			{
				MyFloatingObject myFloatingObject = entity as MyFloatingObject;
				materialType = ((myFloatingObject.VoxelMaterial != null) ? MyMaterialType.ROCK : ((myFloatingObject.ItemDefinition != null && myFloatingObject.ItemDefinition.PhysicalMaterial != MyStringHash.NullOrEmpty) ? myFloatingObject.ItemDefinition.PhysicalMaterial : MyMaterialType.METAL));
				surfaceImpact = MySurfaceImpactEnum.METAL;
				return;
			}
			if (entity is Sandbox.Game.WorldEnvironment.MyEnvironmentSector)
			{
				surfaceImpact = MySurfaceImpactEnum.METAL;
				materialType = MyMaterialType.METAL;
				Sandbox.Game.WorldEnvironment.MyEnvironmentSector myEnvironmentSector = entity as Sandbox.Game.WorldEnvironment.MyEnvironmentSector;
				int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(shapeKey);
				if (itemFromShapeKey >= 0 && myEnvironmentSector.DataView != null && myEnvironmentSector.DataView.Items != null && myEnvironmentSector.DataView.Items.Count > itemFromShapeKey)
				{
					ItemInfo itemInfo = myEnvironmentSector.DataView.Items[itemFromShapeKey];
					MyRuntimeEnvironmentItemInfo value = null;
					if (myEnvironmentSector.EnvironmentDefinition.Items.TryGetValue(itemInfo.DefinitionIndex, out value) && value.Type.Name.Equals("Tree"))
					{
						surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
						materialType = MyMaterialType.WOOD;
					}
				}
				return;
			}
			if (entity is MyTrees)
			{
				surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
				materialType = MyMaterialType.WOOD;
				return;
			}
			if (entity is IMyHandheldGunObject<MyGunBase>)
			{
				surfaceImpact = MySurfaceImpactEnum.METAL;
				materialType = MyMaterialType.METAL;
				MyGunBase gunBase = (entity as IMyHandheldGunObject<MyGunBase>).GunBase;
				if (gunBase != null && gunBase.WeaponProperties != null && gunBase.WeaponProperties.WeaponDefinition != null)
				{
					materialType = gunBase.WeaponProperties.WeaponDefinition.PhysicalMaterial;
				}
				return;
			}
			surfaceImpact = MySurfaceImpactEnum.METAL;
			materialType = MyMaterialType.METAL;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			MySlimBlock mySlimBlock = null;
			if (myCubeBlock != null)
			{
				myCubeGrid = myCubeBlock.CubeGrid;
				mySlimBlock = myCubeBlock.SlimBlock;
			}
			else if (myCubeGrid != null)
			{
				mySlimBlock = myCubeGrid.GetTargetedBlock(hitPosition);
				if (mySlimBlock != null)
				{
					myCubeBlock = mySlimBlock.FatBlock;
				}
			}
			if (myCubeGrid == null || mySlimBlock == null)
			{
				return;
			}
			if (mySlimBlock.BlockDefinition.PhysicalMaterial != null && !mySlimBlock.BlockDefinition.PhysicalMaterial.Id.TypeId.IsNull)
			{
				materialType = MyStringHash.GetOrCompute(mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeName);
			}
			else
			{
				if (myCubeBlock == null)
				{
					return;
				}
				MyIntersectionResultLineTriangleEx? t = null;
				myCubeBlock.GetIntersectionWithLine(ref line, out t);
				if (t.HasValue)
				{
					switch (myCubeBlock.ModelCollision.GetDrawTechnique(t.Value.Triangle.TriangleIndex))
					{
					case MyMeshDrawTechnique.GLASS:
						materialType = MyStringHash.GetOrCompute("Glass");
						break;
					case MyMeshDrawTechnique.HOLO:
						materialType = MyStringHash.GetOrCompute("Glass");
						break;
					case MyMeshDrawTechnique.SHIELD:
						materialType = MyStringHash.GetOrCompute("Shield");
						break;
					case MyMeshDrawTechnique.SHIELD_LIT:
						materialType = MyStringHash.GetOrCompute("ShieldLit");
						break;
					}
				}
			}
		}

		private void StopEffect()
		{
		}

		private void CreateDecal(MyStringHash materialType)
		{
		}

		private void PlayHitSound(MyStringHash materialType, IMyEntity entity, Vector3D position, MyStringHash thisType)
		{
			bool flag = CollisionSoundsTimedCache.IsPlaceUsed(position, CollisionSoundSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds);
			if (OwnerEntity is MyWarhead || flag)
			{
				return;
			}
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
			if (myEntity3DSoundEmitter == null)
			{
				return;
			}
			_ = m_weapon;
			MySoundPair mySoundPair = null;
			mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Hit, thisType, materialType);
			if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
			{
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
			}
			if (mySoundPair.SoundId.IsNull && entity is MyVoxelBase)
			{
				materialType = MyMaterialType.ROCK;
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
			}
			if (mySoundPair != null && mySoundPair != MySoundPair.Empty)
			{
				myEntity3DSoundEmitter.Entity = (MyEntity)entity;
				myEntity3DSoundEmitter.SetPosition(m_position);
				myEntity3DSoundEmitter.SetVelocity(Vector3.Zero);
				if (MySession.Static != null && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound)
				{
					Func<bool> canHear = () => MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity == entity;
					myEntity3DSoundEmitter.StoppedPlaying += delegate(MyEntity3DSoundEmitter e)
					{
						e.EmitterMethods[0].Remove(canHear, immediate: true);
					};
					myEntity3DSoundEmitter.EmitterMethods[0].Add(canHear);
				}
				myEntity3DSoundEmitter.PlaySound(mySoundPair);
			}
		}

		private void ApllyDeformationCubeGrid(Vector3D hitPosition, MyCubeGrid grid)
		{
			MatrixD worldMatrixNormalizedInv = grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3D v = Vector3D.Transform(hitPosition, worldMatrixNormalizedInv);
			Vector3D v2 = Vector3D.TransformNormal(m_directionNormalized, worldMatrixNormalizedInv);
			float deformationOffset = MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO * m_projectileAmmoDefinition.ProjectileMassDamage;
			float value = 0.011904f * m_projectileAmmoDefinition.ProjectileMassDamage;
			float value2 = 0.008928f * m_projectileAmmoDefinition.ProjectileMassDamage;
			value = MathHelper.Clamp(value, grid.GridSize * 0.75f, grid.GridSize * 1.3f);
			value2 = MathHelper.Clamp(value2, grid.GridSize * 0.9f, grid.GridSize * 1.3f);
			grid.Physics.ApplyDeformation(deformationOffset, value, value2, v, v2, MyDamageType.Bullet, 0f, 0f, 0L);
		}

		public static void ApplyProjectileForce(IMyEntity entity, Vector3D intersectionPosition, Vector3 normalizedDirection, bool isPlayerShip, float impulse)
		{
			if (entity.Physics != null && entity.Physics.Enabled && !entity.Physics.IsStatic)
			{
				if (entity is MyCharacter)
				{
					impulse *= 100f;
				}
				entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, normalizedDirection * impulse, intersectionPosition, Vector3.Zero);
			}
		}

		public void Draw()
		{
			if (m_state == MyProjectileStateEnum.KILLED)
			{
				return;
			}
			double num = Vector3D.Distance(m_position, m_origin);
			if (!(num > 0.0) || !m_positionChecked)
			{
				return;
			}
			Vector3D value = m_position - m_directionNormalized * 120.0 * 0.01666666753590107;
			Vector3D vector3D = Vector3D.Normalize(m_position - value);
			double num2 = LengthMultiplier * m_projectileAmmoDefinition.ProjectileTrailScale;
			num2 *= (double)(MyParticlesManager.Paused ? 0.6f : MyUtils.GetRandomFloat(0.6f, 0.8f));
			if (num < num2)
			{
				num2 = num;
			}
			value = ((m_state != 0 && !(num * num >= m_velocity_Combined.LengthSquared() * 0.01666666753590107 * 5.0)) ? (m_position - ((num - num2) * (double)MyUtils.GetRandomFloat(0f, 1f) + num2) * vector3D) : (m_position - num2 * vector3D));
			if (Vector3D.DistanceSquared(value, m_origin) < 4.0)
			{
				return;
			}
			float scaleFactor = MyParticlesManager.Paused ? 1f : MyUtils.GetRandomFloat(1f, 2f);
			float num3 = (MyParticlesManager.Paused ? 0.2f : MyUtils.GetRandomFloat(0.2f, 0.3f)) * m_projectileAmmoDefinition.ProjectileTrailScale;
			num3 *= MathHelper.Lerp(0.2f, 0.8f, MySector.MainCamera.Zoom.GetZoomLevel());
			float scaleFactor2 = 1f;
			float scaleFactor3 = 10f;
			if (num2 > 0.0)
			{
				if (m_projectileAmmoDefinition.ProjectileTrailMaterial != null)
				{
					MyTransparentGeometry.AddLineBillboard(m_projectileTrailMaterialId, new Vector4(m_projectileAmmoDefinition.ProjectileTrailColor * scaleFactor3, 1f), value, vector3D, (float)num2, num3);
				}
				else
				{
					MyTransparentGeometry.AddLineBillboard(ID_PROJECTILE_TRAIL_LINE, new Vector4(m_projectileAmmoDefinition.ProjectileTrailColor * scaleFactor * scaleFactor3, 1f) * scaleFactor2, value, vector3D, (float)num2, num3);
				}
			}
		}

		public void Close()
		{
			OwnerEntity = null;
			m_ignoredEntities = null;
			m_weapon = null;
		}
	}
}
