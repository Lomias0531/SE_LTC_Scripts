using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game
{
	public static class MyParticleEffectsSoundManager
	{
		private class EffectSoundEmitter
		{
			public readonly uint ParticleSoundId;

			public bool Updated;

			public MyEntity3DSoundEmitter Emitter;

			public MySoundPair SoundPair;

			public float OriginalVolume;

			public EffectSoundEmitter(uint id, Vector3 position, MySoundPair sound)
			{
				ParticleSoundId = id;
				Updated = true;
				MyEntity entity = null;
				if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound)
				{
					List<MyEntity> list = new List<MyEntity>();
					Vector3D vector3D = (MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.PositionComp.GetPosition() : MySector.MainCamera.Position;
					BoundingSphereD sphere = new BoundingSphereD(vector3D, 2.0);
					MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, list);
					float num = float.MaxValue;
					for (int i = 0; i < list.Count; i++)
					{
						MyCubeBlock myCubeBlock = list[i] as MyCubeBlock;
						if (myCubeBlock != null && Vector3.DistanceSquared(vector3D, myCubeBlock.PositionComp.GetPosition()) < num)
						{
							entity = myCubeBlock;
						}
					}
					list.Clear();
				}
				Emitter = new MyEntity3DSoundEmitter(entity);
				Emitter.SetPosition(position);
				if (sound == null)
				{
					sound = MySoundPair.Empty;
				}
				Emitter.PlaySound(sound);
				if (Emitter.Sound != null)
				{
					OriginalVolume = Emitter.Sound.Volume;
				}
				else
				{
					OriginalVolume = 1f;
				}
				Emitter.Update();
				SoundPair = sound;
			}
		}

		private static List<EffectSoundEmitter> m_soundEmitters = new List<EffectSoundEmitter>();

		private static short UpdateCount = 0;

		public static void UpdateEffects()
		{
			UpdateCount++;
			for (int i = 0; i < m_soundEmitters.Count; i++)
			{
				m_soundEmitters[i].Updated = false;
			}
			using (MyParticlesManager.SoundsPool.ActiveLock.Acquire())
			{
				foreach (MyParticleSound item in MyParticlesManager.SoundsPool.ActiveWithoutLock)
				{
					bool flag = true;
					for (int i = 0; i < m_soundEmitters.Count; i++)
					{
						if (m_soundEmitters[i].ParticleSoundId == item.ParticleSoundId)
						{
							m_soundEmitters[i].Updated = true;
							m_soundEmitters[i].Emitter.CustomVolume = m_soundEmitters[i].OriginalVolume * item.CurrentVolume;
							m_soundEmitters[i].Emitter.CustomMaxDistance = item.CurrentRange;
							flag = false;
							if (!m_soundEmitters[i].Emitter.Loop && item.NewLoop)
							{
								item.NewLoop = false;
								m_soundEmitters[i].Emitter.PlaySound(m_soundEmitters[i].SoundPair);
							}
							break;
						}
					}
					if (flag && (bool)item.Enabled && item.Position != Vector3.Zero)
					{
						MySoundPair mySoundPair = new MySoundPair(item.SoundName);
						if (mySoundPair != MySoundPair.Empty)
						{
							m_soundEmitters.Add(new EffectSoundEmitter(item.ParticleSoundId, item.Position, mySoundPair));
						}
					}
				}
			}
			for (int i = 0; i < m_soundEmitters.Count; i++)
			{
				if (!m_soundEmitters[i].Updated)
				{
					m_soundEmitters[i].Emitter.StopSound(forced: true);
					m_soundEmitters.RemoveAt(i);
					i--;
				}
				else if (UpdateCount == 100)
				{
					m_soundEmitters[i].Emitter.Update();
				}
			}
			if (UpdateCount >= 100)
			{
				UpdateCount = 0;
			}
		}
	}
}
