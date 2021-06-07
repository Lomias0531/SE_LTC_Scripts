using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Game.Entity;

namespace Sandbox.Game.ParticleEffects
{
	public class MyCubeBlockEffect
	{
		public readonly int EffectId;

		private CubeBlockEffectBase m_effectDefinition;

		public bool CanBeDeleted;

		private List<MyCubeBlockParticleEffect> m_particleEffects;

		private MyEntity m_entity;

		public MyCubeBlockEffect(int EffectId, CubeBlockEffectBase effectDefinition, MyEntity block)
		{
			this.EffectId = EffectId;
			m_entity = block;
			m_effectDefinition = effectDefinition;
			m_particleEffects = new List<MyCubeBlockParticleEffect>();
			if (m_effectDefinition.ParticleEffects != null)
			{
				for (int i = 0; i < m_effectDefinition.ParticleEffects.Length; i++)
				{
					m_particleEffects.Add(new MyCubeBlockParticleEffect(m_effectDefinition.ParticleEffects[i], m_entity));
				}
			}
		}

		public void Stop()
		{
			for (int i = 0; i < m_particleEffects.Count; i++)
			{
				m_particleEffects[i].Stop();
			}
			m_particleEffects.Clear();
		}

		public void Update()
		{
			for (int i = 0; i < m_particleEffects.Count; i++)
			{
				if (m_particleEffects[i].CanBeDeleted)
				{
					m_particleEffects[i].Stop();
					m_particleEffects.RemoveAt(i);
					i--;
				}
				else
				{
					m_particleEffects[i].Update();
				}
			}
		}
	}
}
