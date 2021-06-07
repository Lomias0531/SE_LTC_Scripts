using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyAudioComponent : MySessionComponentBase
	{
		private static readonly int MIN_SOUND_DELAY_IN_FRAMES = 30;

		private static readonly MyStringId m_startCue = MyStringId.GetOrCompute("Start");

		private static readonly int POOL_CAPACITY = 50;

		public static readonly double MaxDistanceOfBlockEmitterFromPlayer = 2500.0;

		public static ConcurrentDictionary<long, int> ContactSoundsPool = new ConcurrentDictionary<long, int>();

		public static MyConcurrentHashSet<long> ContactSoundsThisFrame = new MyConcurrentHashSet<long>();

		private static MyConcurrentQueue<MyEntity3DSoundEmitter> m_singleUseEmitterPool = new MyConcurrentQueue<MyEntity3DSoundEmitter>(POOL_CAPACITY);

		private static List<MyEntity3DSoundEmitter> m_borrowedEmitters = new List<MyEntity3DSoundEmitter>();

		private static List<MyEntity3DSoundEmitter> m_emittersToRemove = new List<MyEntity3DSoundEmitter>();

		private static Dictionary<string, MyEntity3DSoundEmitter> m_emitterLibrary = new Dictionary<string, MyEntity3DSoundEmitter>();

		private static List<string> m_emitterLibraryToRemove = new List<string>();

		private static int m_currentEmitters;

		private static FastResourceLock m_emittersLock = new FastResourceLock();

		private static MyCueId m_nullCueId = new MyCueId(MyStringHash.NullOrEmpty);

		private static FastResourceLock m_contactSoundLock = new FastResourceLock();

		private int m_updateCounter;

		private List<MyEntity> m_detectedGrids;

		private static MyStringId m_destructionSound = MyStringId.GetOrCompute("Destruction");

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			ContactSoundsThisFrame.Clear();
			m_updateCounter++;
			if (m_updateCounter % 100 == 0 && MySession.Static.LocalCharacter != null)
			{
				foreach (string item in m_emitterLibraryToRemove)
				{
					if (m_emitterLibrary.ContainsKey(item))
					{
						m_emitterLibrary[item].StopSound(forced: true);
						m_emitterLibrary.Remove(item);
					}
				}
				m_emitterLibraryToRemove.Clear();
				foreach (MyEntity3DSoundEmitter value in m_emitterLibrary.Values)
				{
					value.Update();
				}
			}
		}

		public static MyEntity3DSoundEmitter TryGetSoundEmitter()
		{
			MyEntity3DSoundEmitter instance = null;
			using (m_emittersLock.AcquireExclusiveUsing())
			{
				if (m_currentEmitters >= POOL_CAPACITY)
				{
					CheckEmitters();
				}
				if (m_emittersToRemove.Count > 0)
				{
					CleanUpEmitters();
				}
				if (!m_singleUseEmitterPool.TryDequeue(out instance) && m_currentEmitters < POOL_CAPACITY)
				{
					instance = new MyEntity3DSoundEmitter(null);
					instance.StoppedPlaying += emitter_StoppedPlaying;
					instance.CanPlayLoopSounds = false;
					m_currentEmitters++;
				}
				if (instance == null)
				{
					return instance;
				}
				m_borrowedEmitters.Add(instance);
				return instance;
			}
		}

		private static void emitter_StoppedPlaying(MyEntity3DSoundEmitter emitter)
		{
			if (emitter != null)
			{
				m_emittersToRemove.Add(emitter);
			}
		}

		private static void CheckEmitters()
		{
			for (int i = 0; i < m_borrowedEmitters.Count; i++)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_borrowedEmitters[i];
				if (myEntity3DSoundEmitter != null && !myEntity3DSoundEmitter.IsPlaying)
				{
					m_emittersToRemove.Add(myEntity3DSoundEmitter);
				}
			}
		}

		private static void CleanUpEmitters()
		{
			for (int i = 0; i < m_emittersToRemove.Count; i++)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_emittersToRemove[i];
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.Entity = null;
					myEntity3DSoundEmitter.SoundId = m_nullCueId;
					m_singleUseEmitterPool.Enqueue(myEntity3DSoundEmitter);
					m_borrowedEmitters.Remove(myEntity3DSoundEmitter);
				}
			}
			m_emittersToRemove.Clear();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			foreach (MyEntity3DSoundEmitter value in m_emitterLibrary.Values)
			{
				value.StopSound(forced: true);
			}
			CleanUpEmitters();
			m_emitterLibrary.Clear();
			m_borrowedEmitters.Clear();
			m_singleUseEmitterPool.Clear();
			m_emitterLibraryToRemove.Clear();
			m_currentEmitters = 0;
		}

		public static MyEntity3DSoundEmitter CreateNewLibraryEmitter(string id, MyEntity entity = null)
		{
			if (!m_emitterLibrary.ContainsKey(id))
			{
				m_emitterLibrary.Add(id, new MyEntity3DSoundEmitter(entity, entity != null && entity is MyCubeBlock));
				return m_emitterLibrary[id];
			}
			return null;
		}

		public static MyEntity3DSoundEmitter GetLibraryEmitter(string id)
		{
			if (m_emitterLibrary.ContainsKey(id))
			{
				return m_emitterLibrary[id];
			}
			return null;
		}

		public static void RemoveLibraryEmitter(string id)
		{
			if (m_emitterLibrary.ContainsKey(id))
			{
				m_emitterLibrary[id].StopSound(forced: true);
				m_emitterLibraryToRemove.Add(id);
			}
		}

		public static bool ShouldPlayContactSound(long entityId, HkContactPointEvent.Type eventType)
		{
			int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
			int value;
			bool flag = ContactSoundsPool.TryGetValue(entityId, out value);
			if (eventType != HkContactPointEvent.Type.Manifold || ContactSoundsThisFrame.Contains(entityId) || (flag && value + MIN_SOUND_DELAY_IN_FRAMES > gameplayFrameCounter))
			{
				return false;
			}
			if (flag)
			{
				ContactSoundsPool.TryRemove(entityId, out value);
			}
			return true;
		}

		public static void PlayContactSound(ContactPointWrapper wrap, IMyEntity sourceEntity)
		{
			IMyEntity topMostParent = sourceEntity.GetTopMostParent();
			MyPhysicsBody bodyA = wrap.bodyA;
			MyPhysicsBody bodyB = wrap.bodyB;
			if (topMostParent.MarkedForClose || topMostParent.Closed || bodyA == null || bodyB == null)
			{
				return;
			}
			ContactSoundsThisFrame.Add(sourceEntity.EntityId);
			IMyEntity myEntity = (sourceEntity == wrap.entityA) ? wrap.entityB : wrap.entityA;
			if (Sync.IsServer && MyMultiplayer.Static != null)
			{
				MyEntity myEntity2 = sourceEntity as MyEntity;
				if (myEntity2 != null)
				{
					Vector3 localPosition = wrap.WorldPosition - myEntity2.PositionComp.WorldMatrix.Translation;
					myEntity2.UpdateSoundContactPoint(myEntity.EntityId, localPosition, wrap.normal, wrap.normal * wrap.separatingVelocity, Math.Abs(wrap.separatingVelocity));
				}
			}
			else
			{
				PlayContactSoundInternal(sourceEntity, myEntity, wrap.WorldPosition, wrap.normal, Math.Abs(wrap.separatingVelocity));
			}
		}

		internal static void PlayContactSoundInternal(IMyEntity entityA, IMyEntity entityB, Vector3D position, Vector3 normal, float separatingSpeed)
		{
			MyPhysicsBody myPhysicsBody = entityA.Physics as MyPhysicsBody;
			MyPhysicsBody myPhysicsBody2 = entityB.Physics as MyPhysicsBody;
			if (myPhysicsBody != null && myPhysicsBody2 != null)
			{
				MyStringHash materialAt = myPhysicsBody.GetMaterialAt(position + normal * 0.1f);
				MyStringHash materialAt2 = myPhysicsBody2.GetMaterialAt(position - normal * 0.1f);
				bool flag = myPhysicsBody2.Entity is MyVoxelBase || myPhysicsBody2.Entity.Physics == null;
				float mass = GetMass(myPhysicsBody);
				float mass2 = GetMass(myPhysicsBody2);
				bool flag2 = !myPhysicsBody.Entity.Physics.IsStatic || mass > 0f;
				bool flag3 = !myPhysicsBody2.Entity.Physics.IsStatic || mass2 > 0f;
				flag = (!flag && myPhysicsBody.Entity.Physics != null && flag2 && (!flag3 || mass < mass2));
				float volume = (!(Math.Abs(separatingSpeed) < 10f)) ? 1f : (0.5f + Math.Abs(separatingSpeed) / 20f);
				Func<bool> canHear = delegate
				{
					if (MySession.Static.ControlledEntity != null)
					{
						MyEntity topMostParent = MySession.Static.ControlledEntity.Entity.GetTopMostParent();
						if (topMostParent != entityA)
						{
							return topMostParent == entityB;
						}
						return true;
					}
					return false;
				};
				using (m_contactSoundLock.AcquireExclusiveUsing())
				{
					if (flag)
					{
						PlayContactSound(entityA.EntityId, m_startCue, position, materialAt, materialAt2, volume, canHear, (MyEntity)entityB, separatingSpeed);
					}
					else
					{
						PlayContactSound(entityA.EntityId, m_startCue, position, materialAt2, materialAt, volume, canHear, (MyEntity)entityA, separatingSpeed);
					}
				}
			}
		}

		private static float GetMass(MyPhysicsComponentBase body)
		{
			if (body == null)
			{
				return 0f;
			}
			MyGridPhysics myGridPhysics = body as MyGridPhysics;
			if (myGridPhysics != null && myGridPhysics.Shape != null)
			{
				if (!myGridPhysics.Shape.MassProperties.HasValue)
				{
					return 0f;
				}
				return myGridPhysics.Shape.MassProperties.Value.Mass;
			}
			return body.Mass;
		}

		public static bool PlayContactSound(long entityId, MyStringId strID, Vector3D position, MyStringHash materialA, MyStringHash materialB, float volume = 1f, Func<bool> canHear = null, MyEntity surfaceEntity = null, float separatingVelocity = 0f)
		{
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(entityId, out entity) || MyMaterialPropertiesHelper.Static == null || MySession.Static == null)
			{
				return false;
			}
			float mass = GetMass(entity.Physics);
			MySoundPair mySoundPair = (entity.Physics != null && (!entity.Physics.IsStatic || mass != 0f)) ? MyMaterialPropertiesHelper.Static.GetCollisionCueWithMass(strID, materialA, materialB, ref volume, mass, separatingVelocity) : MyMaterialPropertiesHelper.Static.GetCollisionCue(strID, materialA, materialB);
			if (mySoundPair != null)
			{
				_ = mySoundPair.SoundId;
				if (MyAudio.Static != null)
				{
					if (separatingVelocity > 0f && separatingVelocity < 0.5f)
					{
						return false;
					}
					if (!mySoundPair.SoundId.IsNull && MyAudio.Static.SourceIsCloseEnoughToPlaySound(position - MySector.MainCamera.Position, mySoundPair.SoundId, 0f))
					{
						MyEntity3DSoundEmitter emitter3 = TryGetSoundEmitter();
						if (emitter3 == null)
						{
							return false;
						}
						ContactSoundsPool.TryAdd(entityId, MySession.Static.GameplayFrameCounter);
						Action<MyEntity3DSoundEmitter> poolRemove = null;
						poolRemove = delegate
						{
							ContactSoundsPool.TryRemove(entityId, out int _);
							emitter3.StoppedPlaying -= poolRemove;
						};
						emitter3.StoppedPlaying += poolRemove;
						if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
						{
							Action<MyEntity3DSoundEmitter> remove2 = null;
							remove2 = delegate
							{
								emitter3.EmitterMethods[0].Remove(canHear);
								emitter3.StoppedPlaying -= remove2;
							};
							emitter3.EmitterMethods[0].Add(canHear);
							emitter3.StoppedPlaying += remove2;
						}
						bool flag = MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp.InVoid;
						if (surfaceEntity != null && !flag)
						{
							emitter3.Entity = surfaceEntity;
						}
						else
						{
							emitter3.Entity = entity;
						}
						emitter3.SetPosition(position);
						emitter3.PlaySound(mySoundPair);
						if (emitter3.Sound != null)
						{
							emitter3.Sound.SetVolume(emitter3.Sound.Volume * volume);
						}
						if (flag && surfaceEntity != null)
						{
							MyEntity3DSoundEmitter emitter2 = TryGetSoundEmitter();
							if (emitter2 == null)
							{
								return false;
							}
							Action<MyEntity3DSoundEmitter> remove = null;
							remove = delegate
							{
								emitter2.EmitterMethods[0].Remove(canHear);
								emitter2.StoppedPlaying -= remove;
							};
							emitter2.EmitterMethods[0].Add(canHear);
							emitter2.StoppedPlaying += remove;
							emitter2.Entity = surfaceEntity;
							emitter2.SetPosition(position);
							emitter2.PlaySound(mySoundPair);
							if (emitter2.Sound != null)
							{
								emitter2.Sound.SetVolume(emitter2.Sound.Volume * volume);
							}
						}
						return true;
					}
					return false;
				}
			}
			return false;
		}

		public static void PlayDestructionSound(MyFracturedPiece fp)
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(fp.OriginalBlocks[0]);
			if (cubeBlockDefinition != null && cubeBlockDefinition.PhysicalMaterial.GeneralSounds.TryGetValue(m_destructionSound, out MySoundPair value) && !value.SoundId.IsNull)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					Vector3D position = fp.PositionComp.GetPosition();
					myEntity3DSoundEmitter.SetPosition(position);
					myEntity3DSoundEmitter.PlaySound(value);
				}
			}
		}

		public static void PlayDestructionSound(MySlimBlock b)
		{
			MyPhysicalMaterialDefinition myPhysicalMaterialDefinition = null;
			if (b.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = b.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock.GetBlocksCount() > 0)
				{
					myPhysicalMaterialDefinition = myCompoundCubeBlock.GetBlocks()[0].BlockDefinition.PhysicalMaterial;
				}
			}
			else if (b.FatBlock is MyFracturedBlock)
			{
				if (MyDefinitionManager.Static.TryGetDefinition((b.FatBlock as MyFracturedBlock).OriginalBlocks[0], out MyCubeBlockDefinition definition))
				{
					myPhysicalMaterialDefinition = definition.PhysicalMaterial;
				}
			}
			else
			{
				myPhysicalMaterialDefinition = b.BlockDefinition.PhysicalMaterial;
			}
			if (myPhysicalMaterialDefinition != null && myPhysicalMaterialDefinition.GeneralSounds.TryGetValue(m_destructionSound, out MySoundPair value) && !value.SoundId.IsNull)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					b.ComputeWorldCenter(out Vector3D worldCenter);
					myEntity3DSoundEmitter.SetPosition(worldCenter);
					myEntity3DSoundEmitter.PlaySound(value);
				}
			}
		}
	}
}
