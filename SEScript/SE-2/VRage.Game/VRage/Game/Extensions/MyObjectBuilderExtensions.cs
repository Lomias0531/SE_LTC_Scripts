using VRage.ObjectBuilders;

namespace VRage.Game.Extensions
{
	public static class MyObjectBuilderExtensions
	{
		public static bool HasPlanets(this MyObjectBuilder_ScenarioDefinition scenario)
		{
			if (scenario.WorldGeneratorOperations != null)
			{
				MyObjectBuilder_WorldGeneratorOperation[] worldGeneratorOperations = scenario.WorldGeneratorOperations;
				foreach (MyObjectBuilder_WorldGeneratorOperation myObjectBuilder_WorldGeneratorOperation in worldGeneratorOperations)
				{
					if (myObjectBuilder_WorldGeneratorOperation is MyObjectBuilder_WorldGeneratorOperation_CreatePlanet)
					{
						return true;
					}
					if (myObjectBuilder_WorldGeneratorOperation is MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool HasPlanets(this MyObjectBuilder_Sector sector)
		{
			if (sector.SectorObjects != null)
			{
				foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
				{
					if (sectorObject is MyObjectBuilder_Planet)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
