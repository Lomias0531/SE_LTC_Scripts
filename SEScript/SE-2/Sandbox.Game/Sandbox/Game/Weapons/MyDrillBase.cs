using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
	public sealed class MyDrillBase
	{
		public struct Sounds
		{
			public MySoundPair IdleLoop;

			public MySoundPair MetalLoop;

			public MySoundPair RockLoop;
		}

		public MyInventory OutputInventory;

		public float VoxelHarvestRatio = 0.009f;

		private MyEntity m_drillEntity;

		private MyFixedPoint m_inventoryCollectionRatio;

		private MyDrillSensorBase m_sensor;

		public MyStringHash m_drillMaterial = MyStringHash.GetOrCompute("HandDrill");

		public MySoundPair m_idleSoundLoop = new MySoundPair("ToolPlayDrillIdle");

		private MyStringHash m_metalMaterial = MyStringHash.GetOrCompute("Metal");

		private MyStringHash m_rockMaterial = MyStringHash.GetOrCompute("Rock");

		private int m_lastContactTime;

		private int m_lastItemId;

		private string m_currentDustEffectName = "";

		public MyParticleEffect DustParticles;

		private MySlimBlock m_target;

		private string m_dustEffectName;

		private string m_dustEffectStonesName;

		private string m_sparksEffectName;

		private bool m_particleEffectsEnabled = true;

		private float m_animationMaxSpeedRatio;

		private float m_animationLastUpdateTime;

		private readonly float m_animationSlowdownTimeInSeconds;

		private float m_floatingObjectSpawnOffset;

		private float m_floatingObjectSpawnRadius;

		private bool m_previousDust;

		private bool m_previousSparks;

		private MyEntity m_drilledEntity;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_initialHeatup = true;

		private MyDrillCutOut m_cutOut;

		private readonly float m_drillCameraMeanShakeIntensity = 0.85f;

		public static float DRILL_MAX_SHAKE = 2f;

		private bool force2DSound;

		private Action<float, string, string> m_onOreCollected;

		private string m_drilledVoxelMaterial;

		private bool m_drilledVoxelMaterialValid;

		public MyParticleEffect SparkEffect;

		private readonly List<MyPhysics.HitInfo> m_castList = new List<MyPhysics.HitInfo>();

		public HashSet<MyEntity> IgnoredEntities => m_sensor.IgnoredEntities;

		public string CurrentDustEffectName
		{
			get
			{
				return m_currentDustEffectName;
			}
			set
			{
				m_currentDustEffectName = value;
			}
		}

		public MySoundPair CurrentLoopCueEnum
		{
			get;
			set;
		}

		public Vector3D ParticleOffset
		{
			get;
			set;
		}

		public bool IsDrilling
		{
			get;
			private set;
		}

		public MyEntity DrilledEntity
		{
			get
			{
				return m_drilledEntity;
			}
			private set
			{
				if (m_drilledEntity != null)
				{
					m_drilledEntity.OnClose -= OnDrilledEntityClose;
				}
				m_drilledEntity = value;
				if (m_drilledEntity != null)
				{
					m_drilledEntity.OnClose += OnDrilledEntityClose;
				}
			}
		}

		public bool CollectingOre
		{
			get;
			protected set;
		}

		public Vector3D DrilledEntityPoint
		{
			get;
			private set;
		}

		public float AnimationMaxSpeedRatio => m_animationMaxSpeedRatio;

		public MyDrillSensorBase Sensor => m_sensor;

		public MyDrillCutOut CutOut => m_cutOut;

		public bool Force2DSound
		{
			get
			{
				return force2DSound;
			}
			set
			{
				bool flag = m_soundEmitter != null && m_soundEmitter.IsPlaying;
				MySoundPair soundPair = m_soundEmitter.SoundPair;
				bool num = value != force2DSound;
				force2DSound = value;
				if (num && flag && soundPair != null && m_soundEmitter != null)
				{
					m_soundEmitter.PlaySound(soundPair, stopPrevious: true, skipIntro: true, Force2DSound);
				}
			}
		}

		public MyDrillBase(MyEntity drillEntity, string dustEffectName, string dustEffectStonesName, string sparksEffectName, MyDrillSensorBase sensor, MyDrillCutOut cutOut, float animationSlowdownTimeInSeconds, float floatingObjectSpawnOffset, float floatingObjectSpawnRadius, float inventoryCollectionRatio = 0f, Action<float, string, string> onOreCollected = null)
		{
			m_drillEntity = drillEntity;
			m_sensor = sensor;
			m_cutOut = cutOut;
			m_dustEffectName = dustEffectName;
			m_dustEffectStonesName = dustEffectStonesName;
			m_sparksEffectName = sparksEffectName;
			m_animationSlowdownTimeInSeconds = animationSlowdownTimeInSeconds;
			m_floatingObjectSpawnOffset = floatingObjectSpawnOffset;
			m_floatingObjectSpawnRadius = floatingObjectSpawnRadius;
			m_inventoryCollectionRatio = (MyFixedPoint)inventoryCollectionRatio;
			m_soundEmitter = new MyEntity3DSoundEmitter(m_drillEntity, useStaticList: true);
			m_onOreCollected = onOreCollected;
		}

		private bool DrillVoxel(MyDrillSensorBase.DetectionInfo entry, bool collectOre, bool performCutout, bool assignDamagedMaterial, ref MyStringHash targetMaterial)
		{
			MyVoxelBase myVoxelBase = entry.Entity as MyVoxelBase;
			Vector3D worldPosition = entry.DetectionPoint;
			bool result = false;
			if (!Sync.IsDedicated)
			{
				MyVoxelMaterialDefinition myVoxelMaterialDefinition = null;
				Vector3D vector3D = m_cutOut.Sphere.Center - m_drillEntity.WorldMatrix.Forward;
				Vector3D to = vector3D + m_drillEntity.WorldMatrix.Forward * (m_cutOut.Sphere.Radius + 1.0);
				MyPhysics.CastRay(vector3D, to, m_castList, 28);
				bool flag = false;
				foreach (MyPhysics.HitInfo cast in m_castList)
				{
					if (cast.HkHitInfo.GetHitEntity() is MyVoxelBase)
					{
						worldPosition = cast.Position;
						myVoxelMaterialDefinition = myVoxelBase.GetMaterialAt(ref worldPosition);
						if (myVoxelMaterialDefinition == null)
						{
							myVoxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().FirstOrDefault();
						}
						flag = true;
						break;
					}
				}
				if (!flag && m_drilledVoxelMaterialValid)
				{
					myVoxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_drilledVoxelMaterial);
				}
				if (myVoxelMaterialDefinition != null)
				{
					CollectingOre = collectOre;
					DrilledEntity = myVoxelBase;
					DrilledEntityPoint = worldPosition;
					targetMaterial = myVoxelMaterialDefinition.MaterialTypeNameHash;
					SpawnVoxelParticles(myVoxelMaterialDefinition);
					result = true;
				}
			}
			if (Sync.IsServer && performCutout)
			{
				TryDrillVoxels(myVoxelBase, worldPosition, collectOre, assignDamagedMaterial);
			}
			return result;
		}

		private bool DrillGrid(MyDrillSensorBase.DetectionInfo entry, bool performCutout, ref MyStringHash targetMaterial)
		{
			bool flag = false;
			MyCubeGrid myCubeGrid = entry.Entity as MyCubeGrid;
			if (myCubeGrid.Physics != null && myCubeGrid.Physics.Enabled)
			{
				flag = TryDrillBlocks(myCubeGrid, entry.DetectionPoint, !Sync.IsServer || !performCutout, out targetMaterial);
			}
			if (flag)
			{
				DrilledEntity = myCubeGrid;
				DrilledEntityPoint = entry.DetectionPoint;
				Vector3D vector3D = Vector3D.Transform(ParticleOffset, m_drillEntity.WorldMatrix);
				MatrixD matrixD = m_drillEntity.WorldMatrix;
				MyFunctionalBlock myFunctionalBlock = m_drillEntity as MyFunctionalBlock;
				if (myFunctionalBlock != null)
				{
					matrixD.Translation = vector3D;
					matrixD = MatrixD.Multiply(matrixD, myFunctionalBlock.CubeGrid.PositionComp.WorldMatrixInvScaled);
				}
				CreateParticles(vector3D, createDust: false, createSparks: true, createStones: false, matrixD, m_drillEntity.Render.ParentIDs[0], targetMaterial);
			}
			return flag;
		}

		private bool DrillFloatingObject(MyDrillSensorBase.DetectionInfo entry)
		{
			MyFloatingObject myFloatingObject = entry.Entity as MyFloatingObject;
			BoundingSphereD sphere = m_cutOut.Sphere;
			sphere.Radius *= 1.3300000429153442;
			if (myFloatingObject.GetIntersectionWithSphere(ref sphere))
			{
				DrilledEntity = myFloatingObject;
				DrilledEntityPoint = entry.DetectionPoint;
				if (Sync.IsServer)
				{
					if (myFloatingObject.Item.Content.TypeId == typeof(MyObjectBuilder_Ore))
					{
						MyEntity myEntity = (m_drillEntity != null && m_drillEntity.HasInventory) ? m_drillEntity : null;
						if (myEntity == null)
						{
							MyHandDrill myHandDrill = m_drillEntity as MyHandDrill;
							if (myHandDrill != null)
							{
								myEntity = myHandDrill.Owner;
							}
						}
						myEntity?.GetInventory().TakeFloatingObject(myFloatingObject);
					}
					else
					{
						myFloatingObject.DoDamage(70f, MyDamageType.Drill, sync: true, (m_drillEntity != null) ? m_drillEntity.EntityId : 0);
					}
				}
				m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				return true;
			}
			return false;
		}

		private bool DrillCharacter(MyDrillSensorBase.DetectionInfo entry, out MyStringHash targetMaterial)
		{
			BoundingSphereD sphere = m_cutOut.Sphere;
			sphere.Radius *= 0.800000011920929;
			MyCharacter myCharacter = entry.Entity as MyCharacter;
			targetMaterial = MyStringHash.GetOrCompute(myCharacter.Definition.PhysicalMaterial);
			if (myCharacter.GetIntersectionWithSphere(ref sphere))
			{
				DrilledEntity = myCharacter;
				DrilledEntityPoint = entry.DetectionPoint;
				if (m_drillEntity is MyHandDrill && (m_drillEntity as MyHandDrill).Owner == MySession.Static.LocalCharacter && myCharacter != MySession.Static.LocalCharacter && !myCharacter.IsDead)
				{
					MySession.Static.TotalDamageDealt += 20u;
				}
				if (Sync.IsServer)
				{
					myCharacter.DoDamage(20f, MyDamageType.Drill, updateSync: true, (m_drillEntity != null) ? m_drillEntity.EntityId : 0);
				}
				m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				return true;
			}
			BoundingSphereD boundingSphereD = new BoundingSphereD(myCharacter.PositionComp.WorldMatrix.Translation + myCharacter.WorldMatrix.Up * 1.25, 0.60000002384185791);
			if (boundingSphereD.Intersects(sphere))
			{
				DrilledEntity = myCharacter;
				DrilledEntityPoint = entry.DetectionPoint;
				if (m_drillEntity is MyHandDrill && (m_drillEntity as MyHandDrill).Owner == MySession.Static.LocalCharacter && myCharacter != MySession.Static.LocalCharacter && !myCharacter.IsDead)
				{
					MySession.Static.TotalDamageDealt += 20u;
				}
				if (Sync.IsServer)
				{
					myCharacter.DoDamage(20f, MyDamageType.Drill, updateSync: true, (m_drillEntity != null) ? m_drillEntity.EntityId : 0);
				}
				m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				return true;
			}
			return false;
		}

		private bool DrillEnvironmentSector(MyDrillSensorBase.DetectionInfo entry, float speedMultiplier, out MyStringHash targetMaterial)
		{
			targetMaterial = MyStringHash.GetOrCompute("Wood");
			DrilledEntity = entry.Entity;
			DrilledEntityPoint = entry.DetectionPoint;
			if (Sync.IsServer)
			{
				if (m_lastItemId != entry.ItemId)
				{
					m_lastItemId = entry.ItemId;
					m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
				if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastContactTime) > 1500f * speedMultiplier)
				{
					MyBreakableEnvironmentProxy module = (entry.Entity as MyEnvironmentSector).GetModule<MyBreakableEnvironmentProxy>();
					Vector3D hitnormal = m_drillEntity.WorldMatrix.Forward + m_drillEntity.WorldMatrix.Right;
					hitnormal.Normalize();
					float num = 100f;
					float num2 = 10f * 10f * num;
					module.BreakAt(entry.ItemId, entry.DetectionPoint, hitnormal, num2);
					m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					m_lastItemId = 0;
				}
			}
			return true;
		}

		public bool Drill(bool collectOre = true, bool performCutout = true, bool assignDamagedMaterial = false, float speedMultiplier = 1f)
		{
			bool flag = false;
			bool newDust = false;
			bool newSparks = false;
			MySoundPair mySoundPair = null;
			if (m_drillEntity.Parent != null && m_drillEntity.Parent.Physics != null && !m_drillEntity.Parent.Physics.Enabled)
			{
				return false;
			}
			DrilledEntity = null;
			CollectingOre = false;
			Dictionary<long, MyDrillSensorBase.DetectionInfo> cachedEntitiesInRange = m_sensor.CachedEntitiesInRange;
			MyStringHash targetMaterial = MyStringHash.NullOrEmpty;
			MyStringHash myStringHash = MyStringHash.NullOrEmpty;
			float num = float.MaxValue;
			bool flag2 = false;
			foreach (KeyValuePair<long, MyDrillSensorBase.DetectionInfo> item in cachedEntitiesInRange)
			{
				flag = false;
				MyEntity entity = item.Value.Entity;
				if (!entity.MarkedForClose)
				{
					if (entity is MyCubeGrid)
					{
						if (DrillGrid(item.Value, performCutout, ref targetMaterial))
						{
							flag = (flag2 = (newSparks = true));
						}
					}
					else if (entity is MyVoxelBase)
					{
						if (DrillVoxel(item.Value, collectOre, performCutout, assignDamagedMaterial, ref targetMaterial))
						{
							flag = (newDust = true);
						}
					}
					else if (entity is MyFloatingObject)
					{
						flag = DrillFloatingObject(item.Value);
					}
					else if (entity is MyCharacter)
					{
						flag = DrillCharacter(item.Value, out targetMaterial);
					}
					else if (entity is MyEnvironmentSector)
					{
						flag = DrillEnvironmentSector(item.Value, speedMultiplier, out targetMaterial);
					}
					if (flag)
					{
						float num2 = Vector3.DistanceSquared(item.Value.DetectionPoint, Sensor.Center);
						if (targetMaterial != MyStringHash.NullOrEmpty && num2 < num)
						{
							myStringHash = targetMaterial;
							num = num2;
						}
					}
				}
			}
			if (myStringHash != MyStringHash.NullOrEmpty)
			{
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, m_drillMaterial, myStringHash);
				if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
				{
					myStringHash = ((!flag2) ? m_rockMaterial : m_metalMaterial);
				}
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, m_drillMaterial, myStringHash);
			}
			if (mySoundPair != null && mySoundPair != MySoundPair.Empty)
			{
				StartLoopSound(mySoundPair, Force2DSound);
			}
			else
			{
				StartIdleSound(m_idleSoundLoop, Force2DSound);
			}
			StartDrillingAnimation(startSound: false);
			CheckParticles(newDust, newSparks);
			return flag;
		}

		public void StartDrillingAnimation(bool startSound)
		{
			if (startSound)
			{
				StartIdleSound(m_idleSoundLoop, Force2DSound);
			}
			if (!IsDrilling)
			{
				IsDrilling = true;
				m_animationLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
		}

		private void SpawnVoxelParticles(MyVoxelMaterialDefinition material)
		{
			Vector3D vector3D = Vector3D.Transform(ParticleOffset, m_drillEntity.WorldMatrix);
			MatrixD matrixD = m_drillEntity.WorldMatrix;
			MyFunctionalBlock myFunctionalBlock = m_drillEntity as MyFunctionalBlock;
			if (myFunctionalBlock != null)
			{
				matrixD.Translation = vector3D;
				matrixD = MatrixD.Multiply(matrixD, myFunctionalBlock.CubeGrid.PositionComp.WorldMatrixInvScaled);
			}
			CreateParticles(vector3D, createDust: true, createSparks: false, createStones: true, matrixD, m_drillEntity.Render.ParentIDs[0], material.MaterialTypeNameHash);
		}

		private void CheckParticles(bool newDust, bool newSparks)
		{
			if (m_previousDust != newDust)
			{
				if (m_previousDust)
				{
					StopDustParticles();
				}
				m_previousDust = newDust;
			}
			if (m_previousSparks != newSparks)
			{
				if (m_previousSparks)
				{
					StopSparkParticles();
				}
				m_previousSparks = newSparks;
			}
		}

		public void Close()
		{
			IsDrilling = false;
			StopDustParticles();
			StopSparkParticles();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public void StopDrill()
		{
			m_drilledVoxelMaterial = "";
			m_drilledVoxelMaterialValid = false;
			IsDrilling = false;
			m_initialHeatup = true;
			StopDustParticles();
			StopSparkParticles();
			StopLoopSound();
		}

		public void UpdateAfterSimulation()
		{
			if (!IsDrilling && m_animationMaxSpeedRatio > float.Epsilon)
			{
				float num = ((float)MySandboxGame.TotalGamePlayTimeInMilliseconds - m_animationLastUpdateTime) / 1000f;
				m_animationMaxSpeedRatio -= num / m_animationSlowdownTimeInSeconds;
				if (m_animationMaxSpeedRatio < float.Epsilon)
				{
					m_animationMaxSpeedRatio = 0f;
				}
			}
			if (IsDrilling)
			{
				float num2 = ((float)MySandboxGame.TotalGamePlayTimeInMilliseconds - m_animationLastUpdateTime) / 1000f;
				m_animationMaxSpeedRatio += 2f * num2 / m_animationSlowdownTimeInSeconds;
				if (m_animationMaxSpeedRatio > 1f)
				{
					m_animationMaxSpeedRatio = 1f;
				}
				if (m_sensor.CachedEntitiesInRange.Count == 0)
				{
					DrilledEntity = null;
					CheckParticles(newDust: false, newSparks: false);
				}
			}
			m_animationLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public void UpdatePosition(MatrixD worldMatrix)
		{
			m_sensor.OnWorldPositionChanged(ref worldMatrix);
			m_cutOut.UpdatePosition(ref worldMatrix);
		}

		private void StartIdleSound(MySoundPair cuePair, bool force2D)
		{
			if (m_soundEmitter != null)
			{
				if (!m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.PlaySound(cuePair, stopPrevious: false, skipIntro: false, force2D);
				}
				else if (!m_soundEmitter.SoundPair.Equals(cuePair))
				{
					m_soundEmitter.StopSound(forced: false);
					m_soundEmitter.PlaySound(cuePair, stopPrevious: false, skipIntro: true, force2D);
				}
			}
		}

		private void StartLoopSound(MySoundPair cueEnum, bool force2D)
		{
			if (m_soundEmitter == null)
			{
				return;
			}
			if (!m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.PlaySound(cueEnum, stopPrevious: false, skipIntro: false, force2D);
			}
			else if (!m_soundEmitter.SoundPair.Equals(cueEnum))
			{
				if (m_soundEmitter.SoundPair.Equals(m_idleSoundLoop))
				{
					m_soundEmitter.StopSound(forced: true);
					m_soundEmitter.PlaySound(cueEnum, stopPrevious: false, skipIntro: false, force2D);
				}
				else
				{
					m_soundEmitter.StopSound(forced: false);
					m_soundEmitter.PlaySound(cueEnum, stopPrevious: false, skipIntro: true, force2D);
				}
			}
		}

		public void StopLoopSound()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		private void CreateParticles(Vector3D position, bool createDust, bool createSparks, bool createStones, MatrixD parent, uint parentId, MyStringHash materialName)
		{
			if (!m_particleEffectsEnabled || Sync.IsDedicated)
			{
				return;
			}
			if (createDust)
			{
				string collisionEffect = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, MyStringHash.GetOrCompute("ShipDrill"), materialName);
				if (!m_previousDust || DustParticles == null || !m_currentDustEffectName.Equals(collisionEffect))
				{
					if (string.IsNullOrEmpty(collisionEffect))
					{
						CurrentDustEffectName = (createStones ? m_dustEffectStonesName : m_dustEffectName);
					}
					else
					{
						CurrentDustEffectName = collisionEffect;
					}
					if ((DustParticles == null || DustParticles.GetName() != m_currentDustEffectName) && DustParticles != null)
					{
						DustParticles.Stop(instant: false);
						DustParticles = null;
					}
					if (DustParticles == null)
					{
						MyParticlesManager.TryCreateParticleEffect(m_currentDustEffectName, ref parent, ref position, parentId, out DustParticles);
					}
				}
			}
			if (createSparks && (!m_previousSparks || SparkEffect == null))
			{
				if (SparkEffect != null)
				{
					SparkEffect.Stop(instant: false);
				}
				MyParticlesManager.TryCreateParticleEffect(m_sparksEffectName, ref parent, ref position, parentId, out SparkEffect);
			}
		}

		private void StopDustParticles()
		{
			if (DustParticles != null)
			{
				DustParticles.Stop(instant: false);
				DustParticles = null;
			}
		}

		public void StopSparkParticles()
		{
			if (SparkEffect != null)
			{
				SparkEffect.Stop(instant: false);
				SparkEffect = null;
			}
		}

		private bool TryDrillBlocks(MyCubeGrid grid, Vector3D worldPoint, bool onlyCheck, out MyStringHash blockMaterial)
		{
			MatrixD worldMatrixNormalizedInv = grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3D vector3D = Vector3D.Transform(m_sensor.Center, worldMatrixNormalizedInv);
			Vector3D vector3D2 = Vector3D.Transform(m_sensor.FrontPoint, worldMatrixNormalizedInv);
			Vector3D v = Vector3D.Transform(worldPoint, worldMatrixNormalizedInv);
			Vector3I pos = Vector3I.Round(vector3D / grid.GridSize);
			MySlimBlock cubeBlock = grid.GetCubeBlock(pos);
			if (cubeBlock == null)
			{
				Vector3I pos2 = Vector3I.Round(vector3D2 / grid.GridSize);
				cubeBlock = grid.GetCubeBlock(pos2);
			}
			if (cubeBlock != null)
			{
				if (cubeBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId == MyStringHash.NullOrEmpty)
				{
					blockMaterial = m_metalMaterial;
				}
				else
				{
					blockMaterial = cubeBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
				}
			}
			else
			{
				blockMaterial = MyStringHash.NullOrEmpty;
			}
			bool flag = false;
			if (!onlyCheck && cubeBlock != null && cubeBlock != null && cubeBlock.CubeGrid.BlocksDestructionEnabled)
			{
				MySlimBlock mySlimBlock = cubeBlock;
				float dRILL_DAMAGE = MyFakes.DRILL_DAMAGE;
				((IMyDestroyableObject)mySlimBlock).DoDamage(dRILL_DAMAGE, MyDamageType.Drill, Sync.IsServer, (MyHitInfo?)null, (m_drillEntity != null) ? m_drillEntity.EntityId : 0);
				Vector3 localNormal = Vector3.Normalize(vector3D2 - vector3D);
				if (cubeBlock.BlockDefinition.BlockTopology == MyBlockTopology.Cube)
				{
					float deformationOffset = MyFakes.DEFORMATION_DRILL_OFFSET_RATIO * dRILL_DAMAGE;
					float value = 0.011904f * dRILL_DAMAGE;
					float value2 = 0.008928f * dRILL_DAMAGE;
					value = MathHelper.Clamp(value, grid.GridSize * 0.75f, grid.GridSize * 1.3f);
					value2 = MathHelper.Clamp(value2, grid.GridSize * 0.9f, grid.GridSize * 1.3f);
					flag = grid.Physics.ApplyDeformation(deformationOffset, value, value2, v, localNormal, MyDamageType.Drill, 0f, 0f, (m_drillEntity != null) ? m_drillEntity.EntityId : 0);
				}
			}
			m_target = (flag ? null : cubeBlock);
			bool result = false;
			if (cubeBlock != null)
			{
				if (flag)
				{
					BoundingSphereD explosionSphere = m_cutOut.Sphere;
					BoundingBoxD bb = BoundingBoxD.CreateFromSphere(explosionSphere);
					MyDebris.Static.CreateExplosionDebris(ref explosionSphere, cubeBlock.CubeGrid, ref bb, 0.3f);
				}
				result = true;
			}
			return result;
		}

		private void TryDrillVoxels(MyVoxelBase voxels, Vector3D hitPosition, bool collectOre, bool applyDamagedMaterial)
		{
			if (voxels.GetOrePriority() != -1)
			{
				MyShapeSphere myShapeSphere = new MyShapeSphere
				{
					Center = m_cutOut.Sphere.Center,
					Radius = (float)m_cutOut.Sphere.Radius
				};
				if (!collectOre)
				{
					myShapeSphere.Radius *= 3f;
				}
				MyVoxelBase.OnCutOutResults results = delegate(float x, MyVoxelMaterialDefinition y, Dictionary<MyVoxelMaterialDefinition, int> z)
				{
					OnDrillResults(z, hitPosition, collectOre);
				};
				voxels.CutOutShapeWithPropertiesAsync(results, myShapeSphere, Sync.IsServer, onlyCheck: false, applyDamagedMaterial);
			}
		}

		private void OnDrillResults(Dictionary<MyVoxelMaterialDefinition, int> materials, Vector3D hitPosition, bool collectOre)
		{
			int num = 0;
			m_drilledVoxelMaterial = "";
			m_drilledVoxelMaterialValid = true;
			foreach (KeyValuePair<MyVoxelMaterialDefinition, int> material in materials)
			{
				int num2 = material.Value;
				if (collectOre && !TryHarvestOreMaterial(material.Key, hitPosition, num2, onlyCheck: false))
				{
					num2 = 0;
				}
				if (num2 > num)
				{
					num = num2;
					m_drilledVoxelMaterial = ((material.Key.DamagedMaterial != MyStringHash.NullOrEmpty) ? material.Key.DamagedMaterial.ToString() : material.Key.Id.SubtypeName);
				}
			}
		}

		public void PerformCameraShake(float multiplier = 1f)
		{
			if (MySector.MainCamera != null)
			{
				float num = (float)((0.0 - Math.Log(MyRandom.Instance.NextDouble())) * (double)m_drillCameraMeanShakeIntensity);
				num = MathHelper.Clamp(num * DRILL_MAX_SHAKE, 0f, DRILL_MAX_SHAKE);
				MySector.MainCamera.CameraShake.AddShake(num * multiplier);
			}
		}

		private bool TryHarvestOreMaterial(MyVoxelMaterialDefinition material, Vector3 hitPosition, int removedAmount, bool onlyCheck)
		{
			if (string.IsNullOrEmpty(material.MinedOre))
			{
				return false;
			}
			if (!onlyCheck)
			{
				MyObjectBuilder_Ore myObjectBuilder_Ore = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(material.MinedOre);
				myObjectBuilder_Ore.MaterialTypeName = material.Id.SubtypeId;
				float num = (float)removedAmount / 255f * 1f * VoxelHarvestRatio;
				num *= material.MinedOreRatio;
				if (!MySession.Static.AmountMined.ContainsKey(material.MinedOre))
				{
					MySession.Static.AmountMined[material.MinedOre] = 0;
				}
				MySession.Static.AmountMined[material.MinedOre] += (MyFixedPoint)num;
				MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_Ore);
				MyFixedPoint myFixedPoint = (MyFixedPoint)(num / physicalItemDefinition.Volume);
				MyFixedPoint myFixedPoint2 = (MyFixedPoint)(0.15f / physicalItemDefinition.Volume);
				if (OutputInventory != null)
				{
					MyFixedPoint b = myFixedPoint * (1 - m_inventoryCollectionRatio);
					b = MyFixedPoint.Min(myFixedPoint2 * 10 - (MyFixedPoint)0.001, b);
					MyFixedPoint amount = myFixedPoint * m_inventoryCollectionRatio - b;
					OutputInventory.AddItems(amount, myObjectBuilder_Ore);
					SpawnOrePieces(b, myFixedPoint2, hitPosition, myObjectBuilder_Ore, material);
					if (m_onOreCollected != null)
					{
						m_onOreCollected((float)b, myObjectBuilder_Ore.TypeId.ToString(), myObjectBuilder_Ore.SubtypeId.ToString());
					}
				}
				else
				{
					SpawnOrePieces(myFixedPoint, myFixedPoint2, hitPosition, myObjectBuilder_Ore, material);
				}
			}
			return true;
		}

		private void SpawnOrePieces(MyFixedPoint amountItems, MyFixedPoint maxAmountPerDrop, Vector3 hitPosition, MyObjectBuilder_PhysicalObject oreObjBuilder, MyVoxelMaterialDefinition voxelMaterial)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			Vector3 forward = Vector3.Normalize(m_sensor.FrontPoint - m_sensor.Center);
			Vector3 center = hitPosition - forward * m_floatingObjectSpawnRadius;
			BoundingSphere b = new BoundingSphere(center, m_floatingObjectSpawnRadius);
			while (amountItems > 0)
			{
				float randomFloat = MyRandom.Instance.GetRandomFloat((float)maxAmountPerDrop / 10f, (float)maxAmountPerDrop);
				MyFixedPoint myFixedPoint = (MyFixedPoint)MathHelper.Min((float)amountItems, randomFloat);
				amountItems -= myFixedPoint;
				MyPhysicalInventoryItem item = new MyPhysicalInventoryItem(myFixedPoint, oreObjBuilder);
				if (MyFakes.ENABLE_DRILL_ROCKS)
				{
					MyFloatingObjects.Spawn(item, b, null, voxelMaterial, delegate(MyEntity entity)
					{
						entity.Physics.LinearVelocity = MyUtils.GetRandomVector3HemisphereNormalized(forward) * MyUtils.GetRandomFloat(1.5f, 4f);
						entity.Physics.AngularVelocity = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(4f, 8f);
					});
				}
			}
		}

		public void DebugDraw()
		{
			m_sensor.DebugDraw();
			MyRenderProxy.DebugDrawSphere((Vector3)m_cutOut.Sphere.Center, (float)m_cutOut.Sphere.Radius, Color.Red, 0.6f);
		}

		private Vector3 ComputeDebrisDirection()
		{
			Vector3D v = m_sensor.Center - m_sensor.FrontPoint;
			v.Normalize();
			return v;
		}

		public void UpdateSoundEmitter(Vector3 velocity)
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.SetVelocity(velocity);
				m_soundEmitter.Update();
			}
		}

		private void OnDrilledEntityClose(MyEntity entity)
		{
			DrilledEntity = null;
		}
	}
}
