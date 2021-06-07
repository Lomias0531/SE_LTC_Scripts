using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation)]
	internal class MyEnvironmentalParticles : MySessionComponentBase
	{
		private List<MyEnvironmentalParticleLogic> m_particleHandlers = new List<MyEnvironmentalParticleLogic>();

		public override void LoadData()
		{
			base.LoadData();
			if (MySector.EnvironmentDefinition != null)
			{
				foreach (MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings environmentalParticle in MySector.EnvironmentDefinition.EnvironmentalParticles)
				{
					MyObjectBuilder_EnvironmentalParticleLogic myObjectBuilder_EnvironmentalParticleLogic = MyObjectBuilderSerializer.CreateNewObject(environmentalParticle.Id) as MyObjectBuilder_EnvironmentalParticleLogic;
					if (myObjectBuilder_EnvironmentalParticleLogic != null)
					{
						myObjectBuilder_EnvironmentalParticleLogic.Density = environmentalParticle.Density;
						myObjectBuilder_EnvironmentalParticleLogic.DespawnDistance = environmentalParticle.DespawnDistance;
						myObjectBuilder_EnvironmentalParticleLogic.ParticleColor = environmentalParticle.Color;
						myObjectBuilder_EnvironmentalParticleLogic.ParticleColorPlanet = environmentalParticle.ColorPlanet;
						myObjectBuilder_EnvironmentalParticleLogic.MaxSpawnDistance = environmentalParticle.MaxSpawnDistance;
						myObjectBuilder_EnvironmentalParticleLogic.Material = environmentalParticle.Material;
						myObjectBuilder_EnvironmentalParticleLogic.MaterialPlanet = environmentalParticle.MaterialPlanet;
						myObjectBuilder_EnvironmentalParticleLogic.MaxLifeTime = environmentalParticle.MaxLifeTime;
						myObjectBuilder_EnvironmentalParticleLogic.MaxParticles = environmentalParticle.MaxParticles;
						MyEnvironmentalParticleLogic myEnvironmentalParticleLogic = MyEnvironmentalParticleLogicFactory.CreateEnvironmentalParticleLogic(myObjectBuilder_EnvironmentalParticleLogic);
						myEnvironmentalParticleLogic.Init(myObjectBuilder_EnvironmentalParticleLogic);
						m_particleHandlers.Add(myEnvironmentalParticleLogic);
					}
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!MyParticlesManager.Paused)
			{
				foreach (MyEnvironmentalParticleLogic particleHandler in m_particleHandlers)
				{
					particleHandler.UpdateBeforeSimulation();
				}
			}
		}

		public override void Simulate()
		{
			base.Simulate();
			if (!MyParticlesManager.Paused)
			{
				foreach (MyEnvironmentalParticleLogic particleHandler in m_particleHandlers)
				{
					particleHandler.Simulate();
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!MyParticlesManager.Paused)
			{
				foreach (MyEnvironmentalParticleLogic particleHandler in m_particleHandlers)
				{
					particleHandler.UpdateAfterSimulation();
				}
			}
		}

		public override void Draw()
		{
			base.Draw();
			foreach (MyEnvironmentalParticleLogic particleHandler in m_particleHandlers)
			{
				particleHandler.Draw();
			}
		}
	}
}
