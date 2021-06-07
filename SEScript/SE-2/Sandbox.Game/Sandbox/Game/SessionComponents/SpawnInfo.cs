using Sandbox.Game.Entities;

namespace Sandbox.Game.SessionComponents
{
	public struct SpawnInfo
	{
		public long IdentityId;

		public MyPlanet Planet;

		public float CollisionRadius;

		public float PlanetDeployAltitude;

		public float MinimalAirDensity;
	}
}
