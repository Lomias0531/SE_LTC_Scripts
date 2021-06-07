using VRage.Game.Gui;
using VRage.Utils;

namespace Sandbox.Definitions
{
	public struct MyToolActionDefinition
	{
		public MyStringId Name;

		public float StartTime;

		public float EndTime;

		public float Efficiency;

		public string StatsEfficiency;

		public string SwingSound;

		public float SwingSoundStart;

		public float HitStart;

		public float HitDuration;

		public string HitSound;

		public float CustomShapeRadius;

		public MyHudTexturesEnum Crosshair;

		public MyToolHitCondition[] HitConditions;

		public override string ToString()
		{
			return Name.ToString();
		}
	}
}
