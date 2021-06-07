using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System.Collections.Generic;
using VRage.Game;
using VRage.Noise;

namespace Sandbox.Game.World.Generator
{
	internal class MyPlanetDetailModulator : IMyModule
	{
		private struct MyModulatorData
		{
			public float Height;

			public MyModuleFast Modulator;
		}

		private MyPlanetGeneratorDefinition m_planetDefinition;

		private MyPlanetMaterialProvider m_oreDeposit;

		private float m_radius;

		private Dictionary<byte, MyModulatorData> m_modulators = new Dictionary<byte, MyModulatorData>();

		public MyPlanetDetailModulator(MyPlanetGeneratorDefinition planetDefinition, MyPlanetMaterialProvider oreDeposit, int seed, float radius)
		{
			m_planetDefinition = planetDefinition;
			m_oreDeposit = oreDeposit;
			m_radius = radius;
			MyPlanetDistortionDefinition[] distortionTable = m_planetDefinition.DistortionTable;
			foreach (MyPlanetDistortionDefinition myPlanetDistortionDefinition in distortionTable)
			{
				MyModuleFast myModuleFast = null;
				float frequency = myPlanetDistortionDefinition.Frequency;
				frequency *= radius / 6f;
				switch (myPlanetDistortionDefinition.Type)
				{
				case "Billow":
				{
					int seed2 = seed;
					double frequency2 = frequency;
					myModuleFast = new MyBillowFast(MyNoiseQuality.High, myPlanetDistortionDefinition.LayerCount, seed2, frequency2);
					break;
				}
				case "RidgedMultifractal":
				{
					int seed2 = seed;
					double frequency2 = frequency;
					myModuleFast = new MyRidgedMultifractalFast(MyNoiseQuality.High, myPlanetDistortionDefinition.LayerCount, seed2, frequency2);
					break;
				}
				case "Perlin":
				{
					int seed2 = seed;
					double frequency2 = frequency;
					myModuleFast = new MyPerlinFast(MyNoiseQuality.High, myPlanetDistortionDefinition.LayerCount, seed2, frequency2);
					break;
				}
				case "Simplex":
					myModuleFast = new MySimplexFast
					{
						Seed = seed,
						Frequency = frequency
					};
					break;
				}
				if (myModuleFast != null)
				{
					m_modulators.Add(myPlanetDistortionDefinition.Value, new MyModulatorData
					{
						Height = myPlanetDistortionDefinition.Height,
						Modulator = myModuleFast
					});
				}
			}
		}

		public double GetValue(double x)
		{
			return 0.0;
		}

		public double GetValue(double x, double y)
		{
			return 0.0;
		}

		public double GetValue(double x, double y, double z)
		{
			return 0.0;
		}
	}
}
