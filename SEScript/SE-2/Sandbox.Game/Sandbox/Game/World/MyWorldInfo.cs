using System;

namespace Sandbox.Game.World
{
	public class MyWorldInfo
	{
		public string SessionName;

		public string Description;

		public DateTime LastSaveTime;

		public ulong? WorkshopId;

		public string Briefing;

		public bool ScenarioEditMode;

		public bool IsCorrupted;

		public bool IsExperimental;

		public bool HasPlanets;
	}
}
