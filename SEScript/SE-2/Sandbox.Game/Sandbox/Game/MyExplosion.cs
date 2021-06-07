using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
	internal class MyExplosion
	{
		private static readonly MyProjectileAmmoDefinition SHRAPNEL_DATA;

		private BoundingSphereD m_explosionSphere;

		private MyLight m_light;

		public int ElapsedMiliseconds;

		private Vector3 m_velocity;

		private MyParticleEffect m_explosionEffect;

		private HashSet<MySlimBlock> m_explodedBlocksInner = new HashSet<MySlimBlock>();

		private HashSet<MySlimBlock> m_explodedBlocksExact = new HashSet<MySlimBlock>();

		private HashSet<MySlimBlock> m_explodedBlocksOuter = new HashSet<MySlimBlock>();

		private MyGridExplosion m_gridExplosion = new MyGridExplosion();

		private MyExplosionInfo m_explosionInfo;

		private bool m_explosionTriggered;

		private MyGridExplosion m_damageInfo;

		private Vector2[] m_explosionForceSlices = new Vector2[3]
		{
			new Vector2(0.8f, 1f),
			new Vector2(1f, 0.5f),
			new Vector2(1.2f, 0.2f)
		};

		private HashSet<MyEntity> m_pushedEntities = new HashSet<MyEntity>();

		public static bool DEBUG_EXPLOSIONS;

		private static readonly HashSet<MyVoxelBase> m_rootVoxelsToCutTmp;

		private static readonly List<MyVoxelBase> m_overlappingVoxelsTmp;

		private static MySoundPair m_explPlayer;

		private static MySoundPair m_smMissileShip;

		private static MySoundPair m_smMissileExpl;

		private static MySoundPair m_lrgWarheadExpl;

		private static MySoundPair m_missileExpl;

		public static MySoundPair SmallWarheadExpl;

		public static MySoundPair SmallPoofSound;

		public static MySoundPair LargePoofSound;

		static MyExplosion()
		{
			DEBUG_EXPLOSIONS = false;
			m_rootVoxelsToCutTmp = new HashSet<MyVoxelBase>();
			m_overlappingVoxelsTmp = new List<MyVoxelBase>();
			m_explPlayer = new MySoundPair("WepExplOnPlay", useLog: false);
			m_smMissileShip = new MySoundPair("WepSmallMissileExplShip", useLog: false);
			m_smMissileExpl = new MySoundPair("WepSmallMissileExpl", useLog: false);
			m_lrgWarheadExpl = new MySoundPair("WepLrgWarheadExpl", useLog: false);
			m_missileExpl = new MySoundPair("WepMissileExplosion", useLog: false);
			SmallWarheadExpl = new MySoundPair("WepSmallWarheadExpl", useLog: false);
			SmallPoofSound = new MySoundPair("PoofExplosionCat1", useLog: false);
			LargePoofSound = new MySoundPair("PoofExplosionCat3", useLog: false);
			SHRAPNEL_DATA = new MyProjectileAmmoDefinition();
			SHRAPNEL_DATA.DesiredSpeed = 100f;
			SHRAPNEL_DATA.SpeedVar = 0f;
			SHRAPNEL_DATA.MaxTrajectory = 1000f;
			SHRAPNEL_DATA.ProjectileHitImpulse = 10f;
			SHRAPNEL_DATA.ProjectileMassDamage = 10f;
			SHRAPNEL_DATA.ProjectileHealthDamage = 10f;
			SHRAPNEL_DATA.ProjectileTrailColor = MyProjectilesConstants.GetProjectileTrailColorByType(MyAmmoType.HighSpeed);
			SHRAPNEL_DATA.AmmoType = MyAmmoType.HighSpeed;
			SHRAPNEL_DATA.ProjectileTrailScale = 0.1f;
			SHRAPNEL_DATA.ProjectileOnHitEffectName = "Hit_BasicAmmoSmall";
		}

		public void Start(MyExplosionInfo explosionInfo)
		{
			m_explosionInfo = explosionInfo;
			ElapsedMiliseconds = 0;
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_explosionInfo.CreateDebris = false;
				m_explosionInfo.PlaySound = false;
				m_explosionInfo.CreateParticleDebris = false;
				m_explosionInfo.CreateParticleEffect = false;
				m_explosionInfo.CreateDecals = false;
			}
		}

		private void StartInternal()
		{
			m_velocity = m_explosionInfo.Velocity;
			m_explosionSphere = m_explosionInfo.ExplosionSphere;
			if (m_explosionInfo.PlaySound)
			{
				PlaySound();
			}
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start(m_explosionSphere.Center, MyExplosionsConstants.EXPLOSION_LIGHT_COLOR, Math.Min((float)m_explosionSphere.Radius * 8f, 120f), "MyExplosion");
				m_light.Intensity = 2f;
				m_light.Range = Math.Min((float)m_explosionSphere.Radius * 3f, 120f);
			}
			if (m_explosionInfo.CreateParticleEffect)
			{
				CreateParticleEffectInternal();
			}
			if (m_explosionInfo.CreateDebris && m_explosionInfo.HitEntity != null)
			{
				BoundingBoxD bb = BoundingBoxD.CreateFromSphere(new BoundingSphereD(m_explosionSphere.Center, m_explosionSphere.Radius * 0.699999988079071));
				MyDebris.Static.CreateExplosionDebris(ref m_explosionSphere, m_explosionInfo.HitEntity, ref bb, 0.5f);
			}
			if (m_explosionInfo.CreateParticleDebris)
			{
				GenerateExplosionParticles("Explosion_Debris", m_explosionSphere, 1f);
			}
			m_explosionTriggered = false;
		}

		public void Clear()
		{
			m_gridExplosion.DamagedBlocks.Clear();
			m_gridExplosion.AffectedCubeGrids.Clear();
			m_gridExplosion.AffectedCubeBlocks.Clear();
		}

		private void PerformCameraShake(float intensityWeight)
		{
			if (MySector.MainCamera != null)
			{
				float num = MySector.MainCamera.CameraShake.MaxShake * intensityWeight;
				Vector3D value = MySector.MainCamera.Position - m_explosionSphere.Center;
				double num2 = 1.0 / value.LengthSquared();
				float num3 = (float)(m_explosionSphere.Radius * m_explosionSphere.Radius * num2);
				if (num3 > 1E-05f)
				{
					MySector.MainCamera.CameraShake.AddShake(num * num3);
					MySector.MainCamera.CameraSpring.AddCurrentCameraControllerVelocity(num * value * num2);
				}
			}
		}

		private void CreateParticleEffectInternal()
		{
			string newParticlesName;
			switch (m_explosionInfo.ExplosionType)
			{
			case MyExplosionTypeEnum.MISSILE_EXPLOSION:
				newParticlesName = "Explosion_Missile";
				break;
			case MyExplosionTypeEnum.BOMB_EXPLOSION:
				newParticlesName = "Dummy";
				break;
			case MyExplosionTypeEnum.AMMO_EXPLOSION:
				newParticlesName = "Dummy";
				break;
			case MyExplosionTypeEnum.GRID_DEFORMATION:
				newParticlesName = "Dummy";
				break;
			case MyExplosionTypeEnum.GRID_DESTRUCTION:
				newParticlesName = "Grid_Destruction";
				break;
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_02:
				newParticlesName = "Explosion_Warhead_02";
				break;
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_15:
				newParticlesName = "Explosion_Warhead_15";
				break;
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_30:
				newParticlesName = "Explosion_Warhead_30";
				break;
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_50:
				newParticlesName = "Explosion_Warhead_50";
				break;
			case MyExplosionTypeEnum.CUSTOM:
				newParticlesName = m_explosionInfo.CustomEffect;
				break;
			default:
				throw new NotImplementedException();
			}
			GenerateExplosionParticles(newParticlesName, m_explosionSphere, m_explosionInfo.ParticleScale);
		}

		private void PlaySound()
		{
			MySoundPair cueByExplosionType = GetCueByExplosionType(m_explosionInfo.ExplosionType);
			if (cueByExplosionType != null && cueByExplosionType != MySoundPair.Empty)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.SetPosition(m_explosionSphere.Center);
					myEntity3DSoundEmitter.Entity = m_explosionInfo.HitEntity;
					myEntity3DSoundEmitter.PlaySound(cueByExplosionType);
				}
			}
			if (m_explosionInfo.HitEntity == MySession.Static.ControlledEntity)
			{
				MyAudio.Static.PlaySound(m_explPlayer.SoundId);
			}
		}

		private void RemoveDestroyedObjects()
		{
			if (m_explosionInfo.Damage > 0f)
			{
				ApplyExplosionOnVoxel(ref m_explosionInfo);
				BoundingSphereD boundingSphere = m_explosionSphere;
				boundingSphere.Radius *= 2.0;
				ApplyVolumetricExplosion(entities: MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere), m_explosionInfo: ref m_explosionInfo);
			}
			if (false && m_explosionInfo.CreateShrapnels)
			{
				for (int i = 0; i < 10; i++)
				{
					MyProjectiles.AddShrapnel(SHRAPNEL_DATA, (m_explosionInfo.HitEntity is MyWarhead) ? new MyEntity[1]
					{
						m_explosionInfo.HitEntity
					} : null, m_explosionSphere.Center, Vector3.Zero, MyUtils.GetRandomVector3Normalized(), groupStart: false, 1f, 1f, null);
				}
			}
		}

		private bool ApplyVolumetricExplosion(ref MyExplosionInfo m_explosionInfo, List<MyEntity> entities)
		{
			bool result = false;
			BoundingSphereD sphere = m_explosionSphere;
			sphere.Radius *= 1.25;
			MyGridExplosion myGridExplosion = ApplyVolumetricExplosionOnGrid(ref m_explosionInfo, ref sphere, entities);
			if ((m_explosionInfo.ExplosionFlags & MyExplosionFlags.APPLY_DEFORMATION) == MyExplosionFlags.APPLY_DEFORMATION)
			{
				m_damageInfo = myGridExplosion;
				m_damageInfo.ComputeDamagedBlocks();
				result = m_damageInfo.GridWasHit;
			}
			if (m_explosionInfo.HitEntity is MyWarhead)
			{
				MySlimBlock slimBlock = (m_explosionInfo.HitEntity as MyWarhead).SlimBlock;
				if (!slimBlock.CubeGrid.BlocksDestructionEnabled)
				{
					slimBlock.CubeGrid.RemoveDestroyedBlock(slimBlock, 0L);
					foreach (MySlimBlock neighbour in slimBlock.Neighbours)
					{
						neighbour.CubeGrid.Physics.AddDirtyBlock(neighbour);
					}
					slimBlock.CubeGrid.Physics.AddDirtyBlock(slimBlock);
				}
			}
			ApplyVolumetricExplosionOnEntities(ref m_explosionInfo, entities, myGridExplosion);
			entities.Clear();
			return result;
		}

		private void ApplyExplosionOnEntities(ref MyExplosionInfo m_explosionInfo, List<MyEntity> entities)
		{
			foreach (MyEntity entity in entities)
			{
				if (entity is IMyDestroyableObject)
				{
					float num = (!(entity is MyCharacter)) ? m_explosionInfo.Damage : m_explosionInfo.PlayerDamage;
					if (num != 0f)
					{
						(entity as IMyDestroyableObject).DoDamage(num, MyDamageType.Explosion, sync: true, null, (m_explosionInfo.OwnerEntity != null) ? m_explosionInfo.OwnerEntity.EntityId : 0);
					}
				}
			}
		}

		private void ApplyVolumetricExplosionOnEntities(ref MyExplosionInfo m_explosionInfo, List<MyEntity> entities, MyGridExplosion explosionDamageInfo)
		{
			float num = (float)explosionDamageInfo.Sphere.Radius * 2f;
			float num2 = 1f / num;
			float num3 = 1f / (float)explosionDamageInfo.Sphere.Radius;
			HkSphereShape? hkSphereShape = null;
			foreach (MyEntity entity in entities)
			{
				float num4 = (float)entity.PositionComp.WorldAABB.Distance(m_explosionSphere.Center);
				float num5 = num4 * num2;
				if (!(num5 > 1f))
				{
					float num6 = 1f - num5 * num5;
					MyAmmoBase myAmmoBase = entity as MyAmmoBase;
					if (myAmmoBase != null)
					{
						myAmmoBase.MarkForDestroy();
					}
					else if (entity.Physics != null && entity.Physics.Enabled && !entity.Physics.IsStatic && m_explosionInfo.ApplyForceAndDamage)
					{
						m_explosionInfo.StrengthImpulse = 100f * (float)m_explosionSphere.Radius;
						m_explosionInfo.StrengthAngularImpulse = 50000f;
						m_explosionInfo.HitEntity = ((m_explosionInfo.HitEntity != null) ? m_explosionInfo.HitEntity.GetBaseEntity() : null);
						Vector3 value;
						if (m_explosionInfo.Direction.HasValue && m_explosionInfo.HitEntity != null && m_explosionInfo.HitEntity.GetTopMostParent() == entity)
						{
							value = m_explosionInfo.Direction.Value;
						}
						else
						{
							Vector3 vector = Vector3.Zero;
							MyCubeGrid myCubeGrid = entity as MyCubeGrid;
							if (myCubeGrid != null)
							{
								if (!hkSphereShape.HasValue)
								{
									hkSphereShape = new HkSphereShape(num);
								}
								HkRigidBody rigidBody = myCubeGrid.Physics.RigidBody;
								Matrix rigidBodyMatrix = rigidBody.GetRigidBodyMatrix();
								Vector3 translation = rigidBodyMatrix.Translation;
								Quaternion rotation = Quaternion.CreateFromRotationMatrix(rigidBodyMatrix);
								Vector3 translation2 = myCubeGrid.Physics.WorldToCluster(m_explosionSphere.Center);
								ClearToken<HkShapeCollision> penetrationsShapeShape = MyPhysics.GetPenetrationsShapeShape(hkSphereShape.Value, ref translation2, ref Quaternion.Identity, rigidBody.GetShape(), ref translation, ref rotation);
								try
								{
									float gridSize = myCubeGrid.GridSize;
									MyGridShape shape = myCubeGrid.Physics.Shape;
									int num7 = Math.Min(penetrationsShapeShape.List.Count, 100);
									BoundingSphere sphere = new BoundingSphere(Vector3D.Transform(m_explosionSphere.Center, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) / gridSize, (float)m_explosionSphere.Radius / gridSize);
									BoundingBoxI boundingBoxI = BoundingBoxI.CreateFromSphere(sphere);
									boundingBoxI.Inflate(1);
									int num8 = 0;
									Vector3 zero = Vector3.Zero;
									for (int i = 0; i < num7; i++)
									{
										HkShapeCollision hkShapeCollision = penetrationsShapeShape.List[i];
										if (hkShapeCollision.ShapeKeyCount != 0 && hkShapeCollision.ShapeKeyCount <= 1)
										{
											shape.GetShapeBounds(hkShapeCollision.GetShapeKey(0), out Vector3I min, out Vector3I max);
											if (min != max)
											{
												MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(min);
												if (cubeBlock != null)
												{
													if (cubeBlock.FatBlock == null)
													{
														Vector3I.Clamp(ref min, ref boundingBoxI.Min, ref boundingBoxI.Max, out min);
														Vector3I.Clamp(ref max, ref boundingBoxI.Min, ref boundingBoxI.Max, out max);
														Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
														while (vector3I_RangeIterator.IsValid())
														{
															Vector3I current2 = vector3I_RangeIterator.Current;
															if (sphere.Contains(current2) == ContainmentType.Contains)
															{
																num8++;
																zero += (Vector3)current2;
															}
															vector3I_RangeIterator.MoveNext();
														}
													}
													else
													{
														num8++;
														zero += new Vector3(max + min) / 2f;
													}
												}
											}
											else
											{
												num8++;
												zero += (Vector3)min;
											}
										}
									}
									if (zero != Vector3.Zero)
									{
										zero /= (float)num8;
										vector = myCubeGrid.GridIntegerToWorld(zero) - m_explosionSphere.Center;
									}
								}
								finally
								{
									((IDisposable)penetrationsShapeShape).Dispose();
								}
							}
							if (vector == Vector3.Zero)
							{
								vector = entity.PositionComp.WorldAABB.Center - m_explosionSphere.Center;
							}
							vector.Normalize();
							value = vector;
						}
						bool flag = !(entity is MyCubeGrid) || MyExplosions.ShouldUseMassScaleForEntity(entity);
						float num9 = num6 / (flag ? 50f : 1f) * m_explosionInfo.StrengthImpulse;
						float mass = entity.Physics.Mass;
						if (flag)
						{
							float num10 = MathHelper.Lerp(0.1f, 1f, 1f - MyMath.FastTanH(mass / 1000000f));
							num9 *= mass * num10;
						}
						else
						{
							num9 = Math.Min(num9, mass);
						}
						entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, value * num9, m_explosionSphere.Center, null);
					}
					if (entity is IMyDestroyableObject || myAmmoBase != null || m_explosionInfo.ApplyForceAndDamage)
					{
						MyCharacter myCharacter = entity as MyCharacter;
						if (myCharacter != null)
						{
							MyCockpit myCockpit = myCharacter.IsUsing as MyCockpit;
							if (myCockpit != null)
							{
								if (explosionDamageInfo.DamagedBlocks.ContainsKey(myCockpit.SlimBlock))
								{
									float damageRemaining = explosionDamageInfo.DamageRemaining[myCockpit.SlimBlock].DamageRemaining;
									myCharacter.DoDamage(damageRemaining, MyDamageType.Explosion, updateSync: true, (m_explosionInfo.OwnerEntity != null) ? m_explosionInfo.OwnerEntity.EntityId : 0);
								}
								continue;
							}
						}
						if (!(entity is MyCubeGrid))
						{
							IMyDestroyableObject myDestroyableObject = entity as IMyDestroyableObject;
							if (myDestroyableObject != null)
							{
								float num11 = num4 * num3;
								if (!(num11 > 1f))
								{
									float num12 = 1f - num11 * num11;
									float damage = explosionDamageInfo.Damage * num12;
									myDestroyableObject.DoDamage(damage, MyDamageType.Explosion, sync: true, null, (m_explosionInfo.OwnerEntity != null) ? m_explosionInfo.OwnerEntity.EntityId : 0);
								}
							}
						}
					}
				}
			}
			if (hkSphereShape.HasValue)
			{
				hkSphereShape.Value.Base.RemoveReference();
			}
		}

		private void ApplyExplosionOnVoxel(ref MyExplosionInfo explosionInfo)
		{
			if (explosionInfo.AffectVoxels && MySession.Static.EnableVoxelDestruction && MySession.Static.HighSimulationQuality && explosionInfo.Damage > 0f)
			{
				MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref m_explosionSphere, m_overlappingVoxelsTmp);
				for (int num = m_overlappingVoxelsTmp.Count - 1; num >= 0; num--)
				{
					m_rootVoxelsToCutTmp.Add(m_overlappingVoxelsTmp[num].RootVoxel);
				}
				m_overlappingVoxelsTmp.Clear();
				foreach (MyVoxelBase item in m_rootVoxelsToCutTmp)
				{
					bool createDebris = true;
					CutOutVoxelMap((float)m_explosionSphere.Radius * explosionInfo.VoxelCutoutScale, explosionInfo.VoxelExplosionCenter, item, createDebris);
					item.RequestVoxelCutoutSphere(explosionInfo.VoxelExplosionCenter, (float)m_explosionSphere.Radius * explosionInfo.VoxelCutoutScale, createDebris, damage: false);
				}
				m_rootVoxelsToCutTmp.Clear();
			}
		}

		public static void CutOutVoxelMap(float radius, Vector3D center, MyVoxelBase voxelMap, bool createDebris, bool damage = false)
		{
			MyShapeSphere sphereShape = new MyShapeSphere
			{
				Center = center,
				Radius = radius
			};
			MyVoxelBase.OnCutOutResults results = null;
			if (createDebris)
			{
				results = delegate(float x, MyVoxelMaterialDefinition y, Dictionary<MyVoxelMaterialDefinition, int> z)
				{
					OnCutOutVoxelMap(x, y, sphereShape, voxelMap);
				};
			}
			voxelMap.CutOutShapeWithPropertiesAsync(results, sphereShape, updateSync: false, onlyCheck: false, damage);
		}

		private static void OnCutOutVoxelMap(float voxelsCountInPercent, MyVoxelMaterialDefinition voxelMaterial, MyShapeSphere sphereShape, MyVoxelBase voxelMap)
		{
			if (!(voxelsCountInPercent > 0f) || voxelMaterial == null)
			{
				return;
			}
			BoundingSphereD explosionSphere = new BoundingSphereD(sphereShape.Center, sphereShape.Radius);
			if (MyRenderConstants.RenderQualityProfile.ExplosionDebrisCountMultiplier > 0f)
			{
				if (voxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
				{
					voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxelMaterial.DamagedMaterial.ToString());
				}
				MyDebris.Static.CreateExplosionDebris(ref explosionSphere, voxelsCountInPercent, voxelMaterial, voxelMap);
			}
			if (MyParticlesManager.TryCreateParticleEffect("MaterialExplosion_Destructible", MatrixD.CreateTranslation(explosionSphere.Center), out MyParticleEffect effect))
			{
				effect.UserRadiusMultiplier = (float)explosionSphere.Radius;
				effect.UserScale = 0.2f;
			}
		}

		private MyGridExplosion ApplyVolumetricExplosionOnGrid(ref MyExplosionInfo explosionInfo, ref BoundingSphereD sphere, List<MyEntity> entities)
		{
			m_gridExplosion.Init(explosionInfo.ExplosionSphere, explosionInfo.Damage);
			MyCubeGrid myCubeGrid = null;
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = null;
			if (!MySession.Static.Settings.EnableTurretsFriendlyFire && explosionInfo.OriginEntity != 0L)
			{
				MyEntity entityById = MyEntities.GetEntityById(explosionInfo.OriginEntity);
				if (entityById != null)
				{
					myCubeGrid = (entityById.GetTopMostParent() as MyCubeGrid);
					if (myCubeGrid != null)
					{
						group = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid);
					}
				}
			}
			foreach (MyEntity entity in entities)
			{
				if (entity != explosionInfo.ExcludedEntity)
				{
					MyCubeGrid myCubeGrid2 = entity as MyCubeGrid;
					if (myCubeGrid2 != null && myCubeGrid2.CreatePhysics && myCubeGrid2 != myCubeGrid && (group == null || MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid2) != group))
					{
						m_gridExplosion.AffectedCubeGrids.Add(myCubeGrid2);
						float detectionBlockHalfSize = myCubeGrid2.GridSize / 2f / 1.25f;
						MatrixD invWorldGrid = myCubeGrid2.PositionComp.WorldMatrixInvScaled;
						BoundingSphereD sphere2 = new BoundingSphereD(sphere.Center, (float)Math.Max(0.10000000149011612, sphere.Radius - (double)myCubeGrid2.GridSize));
						BoundingSphereD sphere3 = new BoundingSphereD(sphere.Center, sphere.Radius);
						BoundingSphereD sphere4 = new BoundingSphereD(sphere.Center, sphere.Radius + (double)(myCubeGrid2.GridSize * 0.5f * (float)Math.Sqrt(3.0)));
						myCubeGrid2.GetBlocksInsideSpheres(ref sphere2, ref sphere3, ref sphere4, m_explodedBlocksInner, m_explodedBlocksExact, m_explodedBlocksOuter, respectDeformationRatio: false, detectionBlockHalfSize, ref invWorldGrid);
						m_explodedBlocksInner.UnionWith(m_explodedBlocksExact);
						m_gridExplosion.AffectedCubeBlocks.UnionWith(m_explodedBlocksInner);
						foreach (MySlimBlock item in m_explodedBlocksOuter)
						{
							myCubeGrid2.Physics.AddDirtyBlock(item);
						}
						m_explodedBlocksInner.Clear();
						m_explodedBlocksExact.Clear();
						m_explodedBlocksOuter.Clear();
					}
				}
			}
			return m_gridExplosion;
		}

		public void ApplyVolumetricDamageToGrid()
		{
			if (m_damageInfo != null)
			{
				ApplyVolumetricDamageToGrid(m_damageInfo, (m_explosionInfo.OwnerEntity != null) ? m_explosionInfo.OwnerEntity.EntityId : 0);
			}
			m_damageInfo = null;
		}

		private void ApplyVolumetricDamageToGrid(MyGridExplosion damageInfo, long attackerId)
		{
			Dictionary<MySlimBlock, float> damagedBlocks = damageInfo.DamagedBlocks;
			HashSet<MySlimBlock> affectedCubeBlocks = damageInfo.AffectedCubeBlocks;
			_ = damageInfo.AffectedCubeGrids;
			if (MyDebugDrawSettings.DEBUG_DRAW_VOLUMETRIC_EXPLOSION_COLORING)
			{
				foreach (MySlimBlock item in affectedCubeBlocks)
				{
					item.CubeGrid.ChangeColorAndSkin(item, new Vector3(0.66f, 1f, 1f));
				}
				foreach (KeyValuePair<MySlimBlock, float> item2 in damagedBlocks)
				{
					float num = 1f - item2.Value / damageInfo.Damage;
					item2.Key.CubeGrid.ChangeColorAndSkin(item2.Key, new Vector3(num / 3f, 1f, 0.5f));
				}
			}
			else
			{
				bool hasAnyBeforeHandler = MyDamageSystem.Static.HasAnyBeforeHandler;
				foreach (KeyValuePair<MySlimBlock, float> item3 in damagedBlocks)
				{
					MySlimBlock key = item3.Key;
					if (!key.CubeGrid.MarkedForClose && (key.FatBlock == null || !key.FatBlock.MarkedForClose) && !key.IsDestroyed && key.CubeGrid.BlocksDestructionEnabled)
					{
						float num2 = item3.Value;
						if (hasAnyBeforeHandler && key.UseDamageSystem)
						{
							MyDamageInformation info = new MyDamageInformation(isDeformation: false, num2, MyDamageType.Explosion, attackerId);
							MyDamageSystem.Static.RaiseBeforeDamageApplied(key, ref info);
							if (info.Amount <= 0f)
							{
								continue;
							}
							num2 = info.Amount;
						}
						if (affectedCubeBlocks.Contains(item3.Key) && !m_explosionInfo.KeepAffectedBlocks)
						{
							key.CubeGrid.RemoveDestroyedBlock(key, 0L);
						}
						else
						{
							if ((key.FatBlock == null && key.Integrity / key.DeformationRatio < num2) || key.FatBlock == m_explosionInfo.HitEntity)
							{
								key.CubeGrid.RemoveDestroyedBlock(key, 0L);
							}
							else
							{
								if (key.FatBlock != null)
								{
									num2 *= 7f;
								}
								((IMyDestroyableObject)key).DoDamage(num2, MyDamageType.Explosion, sync: true, (MyHitInfo?)null, attackerId);
								if (!key.IsDestroyed)
								{
									key.CubeGrid.ApplyDestructionDeformation(key, 1f, null, 0L);
								}
							}
							foreach (MySlimBlock neighbour in key.Neighbours)
							{
								neighbour.CubeGrid.Physics.AddDirtyBlock(neighbour);
							}
							key.CubeGrid.Physics.AddDirtyBlock(key);
						}
					}
				}
			}
		}

		private MySoundPair GetCueByExplosionType(MyExplosionTypeEnum explosionType)
		{
			MySoundPair result = null;
			switch (explosionType)
			{
			case MyExplosionTypeEnum.MISSILE_EXPLOSION:
			{
				bool flag = false;
				if (m_explosionInfo.HitEntity is MyCubeGrid)
				{
					foreach (MySlimBlock block in (m_explosionInfo.HitEntity as MyCubeGrid).GetBlocks())
					{
						if (block.FatBlock is MyCockpit && (block.FatBlock as MyCockpit).Pilot == MySession.Static.ControlledEntity)
						{
							result = m_smMissileShip;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					result = m_smMissileExpl;
				}
				break;
			}
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_02:
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_15:
				result = SmallWarheadExpl;
				break;
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_30:
			case MyExplosionTypeEnum.WARHEAD_EXPLOSION_50:
				result = m_lrgWarheadExpl;
				break;
			case MyExplosionTypeEnum.BOMB_EXPLOSION:
				result = m_lrgWarheadExpl;
				break;
			case MyExplosionTypeEnum.CUSTOM:
				result = m_explosionInfo.CustomSound;
				break;
			default:
				result = m_missileExpl;
				break;
			}
			return result;
		}

		private Vector4 GetSmutDecalRandomColor()
		{
			float randomFloat = MyUtils.GetRandomFloat(0.2f, 0.3f);
			return new Vector4(randomFloat, randomFloat, randomFloat, 1f);
		}

		public bool Update()
		{
			if (ElapsedMiliseconds == 0)
			{
				StartInternal();
			}
			ElapsedMiliseconds += 16;
			if ((float)ElapsedMiliseconds < MyExplosionsConstants.CAMERA_SHAKE_TIME_MS)
			{
				PerformCameraShake(1f - (float)ElapsedMiliseconds / MyExplosionsConstants.CAMERA_SHAKE_TIME_MS);
			}
			if (ElapsedMiliseconds > m_explosionInfo.ObjectsRemoveDelayInMiliseconds)
			{
				if (Sync.IsServer)
				{
					RemoveDestroyedObjects();
				}
				m_explosionInfo.ObjectsRemoveDelayInMiliseconds = int.MaxValue;
				m_explosionTriggered = true;
			}
			if (m_light != null)
			{
				float num = 1f - (float)ElapsedMiliseconds / (float)m_explosionInfo.LifespanMiliseconds;
				m_light.Intensity = 2f * num;
			}
			if (m_explosionEffect != null)
			{
				m_explosionSphere.Center += m_velocity * 0.0166666675f;
				m_explosionEffect.WorldMatrix = CalculateEffectMatrix(m_explosionSphere);
			}
			else if (ElapsedMiliseconds >= m_explosionInfo.LifespanMiliseconds && m_explosionTriggered)
			{
				if (DEBUG_EXPLOSIONS)
				{
					return true;
				}
				Close();
				return false;
			}
			if (m_explosionEffect != null)
			{
				return !m_explosionEffect.IsStopped;
			}
			return true;
		}

		public void DebugDraw()
		{
			if (DEBUG_EXPLOSIONS)
			{
				_ = m_light;
			}
		}

		public void Close()
		{
			if (m_light != null)
			{
				MyLights.RemoveLight(m_light);
				m_light = null;
			}
		}

		private MatrixD CalculateEffectMatrix(BoundingSphereD explosionSphere)
		{
			Vector3D vector3D = MySector.MainCamera.Position - explosionSphere.Center;
			vector3D = ((!MyUtils.IsZero(vector3D)) ? MyUtils.Normalize(vector3D) : ((Vector3D)MySector.MainCamera.ForwardVector));
			return MatrixD.CreateTranslation(m_explosionSphere.Center + vector3D * 0.89999997615814209);
		}

		private void GenerateExplosionParticles(string newParticlesName, BoundingSphereD explosionSphere, float particleScale)
		{
			if (MyParticlesManager.TryCreateParticleEffect(newParticlesName, CalculateEffectMatrix(explosionSphere), out m_explosionEffect))
			{
				m_explosionEffect.OnDelete += delegate
				{
					m_explosionInfo.LifespanMiliseconds = 0;
					m_explosionEffect = null;
				};
				m_explosionEffect.UserScale = particleScale;
			}
		}
	}
}
