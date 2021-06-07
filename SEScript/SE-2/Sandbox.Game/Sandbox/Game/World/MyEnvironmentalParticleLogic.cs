using System.Collections.Generic;
using VRage.Game.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[MyEnvironmentalParticleLogicType(typeof(MyObjectBuilder_EnvironmentalParticleLogic), true)]
	public class MyEnvironmentalParticleLogic
	{
		public class MyEnvironmentalParticle
		{
			private Vector3 m_position;

			private MyStringId m_material;

			private Vector4 m_color;

			private MyStringId m_materialPlanet;

			private Vector4 m_colorPlanet;

			private int m_birthTime;

			private int m_lifeTime;

			private bool m_active;

			public object UserData;

			public Vector3 Position
			{
				get
				{
					return m_position;
				}
				set
				{
					m_position = value;
				}
			}

			public MyStringId Material => m_material;

			public Vector4 Color => m_color;

			public MyStringId MaterialPlanet => m_materialPlanet;

			public Vector4 ColorPlanet => m_colorPlanet;

			public int BirthTime => m_birthTime;

			public int LifeTime => m_lifeTime;

			public bool Active => m_active;

			public MyEnvironmentalParticle(string material, string materialPlanet, Vector4 color, Vector4 colorPlanet, int lifeTime)
			{
				m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				if (material == null)
				{
					m_material = MyTransparentMaterials.ErrorMaterial.Id;
				}
				else
				{
					m_material = MyStringId.GetOrCompute(material);
				}
				if (materialPlanet == null)
				{
					m_materialPlanet = MyTransparentMaterials.ErrorMaterial.Id;
				}
				else
				{
					m_materialPlanet = MyStringId.GetOrCompute(materialPlanet);
				}
				m_color = color;
				m_colorPlanet = colorPlanet;
				m_position = default(Vector3);
				m_lifeTime = lifeTime;
				Deactivate();
			}

			public void Activate(Vector3 position)
			{
				m_birthTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_position = position;
				m_active = true;
			}

			public void Deactivate()
			{
				m_active = false;
			}
		}

		protected float m_particleDensity;

		protected float m_particleSpawnDistance;

		protected float m_particleDespawnDistance;

		private int m_maxParticles = 128;

		protected List<MyEnvironmentalParticle> m_nonActiveParticles;

		protected List<MyEnvironmentalParticle> m_activeParticles;

		protected List<int> m_particlesToRemove = new List<int>();

		protected float ParticleDensity => m_particleDensity;

		protected float ParticleSpawnDistance => m_particleSpawnDistance;

		protected float ParticleDespawnDistance => m_particleDespawnDistance;

		public virtual void Init(MyObjectBuilder_EnvironmentalParticleLogic builder)
		{
			m_particleDensity = builder.Density;
			m_particleSpawnDistance = builder.MaxSpawnDistance;
			m_particleDespawnDistance = builder.DespawnDistance;
			m_maxParticles = builder.MaxParticles;
			m_nonActiveParticles = new List<MyEnvironmentalParticle>(m_maxParticles);
			m_activeParticles = new List<MyEnvironmentalParticle>(m_maxParticles);
			string materialPlanet = builder.Material;
			Vector4 colorPlanet = builder.ParticleColor;
			MyObjectBuilder_EnvironmentalParticleLogicSpace myObjectBuilder_EnvironmentalParticleLogicSpace = builder as MyObjectBuilder_EnvironmentalParticleLogicSpace;
			if (myObjectBuilder_EnvironmentalParticleLogicSpace != null)
			{
				materialPlanet = myObjectBuilder_EnvironmentalParticleLogicSpace.MaterialPlanet;
				colorPlanet = myObjectBuilder_EnvironmentalParticleLogicSpace.ParticleColorPlanet;
			}
			for (int i = 0; i < m_maxParticles; i++)
			{
				m_nonActiveParticles.Add(new MyEnvironmentalParticle(builder.Material, materialPlanet, builder.ParticleColor, colorPlanet, builder.MaxLifeTime));
			}
		}

		public virtual void UpdateBeforeSimulation()
		{
		}

		public virtual void Simulate()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
			for (int i = 0; i < m_activeParticles.Count; i++)
			{
				MyEnvironmentalParticle myEnvironmentalParticle = m_activeParticles[i];
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - myEnvironmentalParticle.BirthTime >= myEnvironmentalParticle.LifeTime || (myEnvironmentalParticle.Position - MySector.MainCamera.Position).Length() > (double)m_particleDespawnDistance || !myEnvironmentalParticle.Active)
				{
					m_particlesToRemove.Add(i);
				}
			}
			for (int num = m_particlesToRemove.Count - 1; num >= 0; num--)
			{
				int index = m_particlesToRemove[num];
				m_nonActiveParticles.Add(m_activeParticles[index]);
				m_activeParticles[index].Deactivate();
				m_activeParticles.RemoveAt(index);
			}
			m_particlesToRemove.Clear();
		}

		public virtual void Draw()
		{
		}

		protected MyEnvironmentalParticle Spawn(Vector3 position)
		{
			int count = m_nonActiveParticles.Count;
			if (count <= 0)
			{
				return null;
			}
			MyEnvironmentalParticle myEnvironmentalParticle = m_nonActiveParticles[count - 1];
			m_activeParticles.Add(myEnvironmentalParticle);
			m_nonActiveParticles.RemoveAtFast(count - 1);
			myEnvironmentalParticle.Activate(position);
			return myEnvironmentalParticle;
		}

		protected bool Despawn(MyEnvironmentalParticle particle)
		{
			if (particle == null)
			{
				return false;
			}
			foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
			{
				if (particle == activeParticle)
				{
					m_activeParticles.Remove(particle);
					particle.Deactivate();
					m_nonActiveParticles.Add(particle);
					return true;
				}
			}
			return false;
		}

		protected void DeactivateAll()
		{
			foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
			{
				m_nonActiveParticles.Add(activeParticle);
				activeParticle.Deactivate();
			}
			m_activeParticles.Clear();
		}
	}
}
