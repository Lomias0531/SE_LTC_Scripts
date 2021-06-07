using Sandbox.Definitions;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.ParticleEffects
{
	internal class MyCubeBlockParticleEffect
	{
		private string m_particleName = string.Empty;

		private bool m_canBeDeleted;

		private MyParticleEffect m_effect;

		private bool m_loop;

		private bool m_playedOnce;

		private bool m_playing;

		private float m_delay;

		private float m_timer;

		private float m_spawnTimeMin;

		private float m_spawnTimeMax;

		private float m_duration;

		private MyModelDummy m_originPoint;

		private MyEntity m_entity;

		public string ParticleName => m_particleName;

		public bool CanBeDeleted => m_canBeDeleted;

		public bool EffectIsRunning => m_effect != null;

		public MyCubeBlockParticleEffect(CubeBlockEffect effectData, MyEntity entity)
		{
			m_particleName = effectData.Name;
			if (string.IsNullOrEmpty(m_particleName))
			{
				m_canBeDeleted = true;
				return;
			}
			m_loop = effectData.Loop;
			m_delay = effectData.Delay;
			m_spawnTimeMin = Math.Max(0f, effectData.SpawnTimeMin);
			m_spawnTimeMax = Math.Max(m_spawnTimeMin, effectData.SpawnTimeMax);
			m_timer = m_delay;
			m_entity = entity;
			m_originPoint = GetEffectOrigin(effectData.Origin);
			m_duration = effectData.Duration;
			if (m_spawnTimeMax > 0f)
			{
				m_timer += MyUtils.GetRandomFloat(m_spawnTimeMin, m_spawnTimeMax);
			}
		}

		private MyModelDummy GetEffectOrigin(string origin)
		{
			if (m_entity.Model.Dummies.ContainsKey(origin))
			{
				return m_entity.Model.Dummies[origin];
			}
			return null;
		}

		public void Stop()
		{
			if (m_effect != null)
			{
				m_effect.Stop();
				m_effect = null;
			}
		}

		public void Update()
		{
			if (string.IsNullOrEmpty(m_particleName))
			{
				return;
			}
			if (m_effect == null || m_effect.IsStopped)
			{
				if (m_playedOnce && !m_loop)
				{
					m_canBeDeleted = true;
					return;
				}
				if (m_timer > 0f)
				{
					m_timer -= 0.0166666675f;
					return;
				}
				m_playedOnce = true;
				m_canBeDeleted = !MyParticlesManager.TryCreateParticleEffect(m_particleName, m_entity.WorldMatrix, out m_effect);
				if (m_spawnTimeMax > 0f)
				{
					m_timer = MyUtils.GetRandomFloat(m_spawnTimeMin, m_spawnTimeMax);
				}
				else
				{
					m_timer = 0f;
				}
			}
			else if (m_effect != null)
			{
				float elapsedTime = m_effect.GetElapsedTime();
				if (m_duration > 0f && elapsedTime >= m_duration)
				{
					m_effect.Stop();
				}
				else if (m_originPoint != null)
				{
					m_effect.WorldMatrix = MatrixD.Multiply(MatrixD.Normalize(m_originPoint.Matrix), m_entity.WorldMatrix);
				}
				else
				{
					m_effect.WorldMatrix = m_entity.WorldMatrix;
				}
			}
		}
	}
}
